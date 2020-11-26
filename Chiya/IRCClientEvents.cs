using Chiya.Bancho;
using Chiya.Commands;
using Chiya.Commands.Object;
using Chiya.Utils;
using Meebey.SmartIrc4net;
using osu.Game.Rulesets.Mods;
using osu.PPCalc;
using System;

namespace Chiya
{
	partial class IRCClient
	{
		void RegisterEvents()
		{
			irc.OnChannelMessage += OnChannelMessage;
			irc.OnQueryMessage += OnQueryMessage;
			irc.OnQueryAction += OnQueryAction;
			irc.OnError += OnError;
			irc.OnConnected += (object sender, EventArgs e) =>
			{
				Console.WriteLine("Connected");
			};
			irc.OnDisconnected += (object sender, EventArgs e) =>
			{
				Console.WriteLine("Disconnected");
			};
			irc.OnRawMessage += (object sender, IrcEventArgs e) =>
			{
				string raw = e.Data.RawMessage;
				if (raw.Contains("JOIN") || raw.Contains("QUIT") || raw.Contains("End of /WHO list.") || raw.Contains("PART")) return;
				Console.WriteLine("[RAW] " + raw);
			};
		}

		public void OnError(object sender, ErrorEventArgs e)
		{
			Console.WriteLine("Error: " + e.ErrorMessage);
		}
		public void OnChannelMessage(object sender, IrcEventArgs e)
		{
			Console.WriteLine($"[{e.Data.Channel}] {e.Data.Nick} : {e.Data.Message}");
			// Multiplay Parsing
			if (e.Data.Nick.ToLower() == "banchobot")
			{
				BanchoBotParser bparser = new BanchoBotParser(e.Data.Message);
				bparser.Parse();
				switch (bparser.Type)
				{
					case BanchoBotParseType.BEATMAP_CHANGED:
						if (bparser.Result as string == "0") return; // Skip if map is not submitted.
																	 // todo get beatmap object from beatmap id.
						irc.SendMessage(SendType.Message, e.Data.Channel, $"Bloodcat mirror: {new UrlMessage("here", $"https://bloodcat.com/osu/b/{bparser.Result as string}")}");
						break;
				}
			}
		}
		public void OnQueryMessage(object sender, IrcEventArgs e)
		{
			Console.WriteLine($"[PM] {e.Data.Nick} : {e.Data.Message}");
			// Check NowPlaying
			if (NowPlayingParser.IsNowPlaying(e.Data.Message))
			{
				NowPlaying np = NowPlayingParser.Parse(e.Data.Message);
				new CommandResult()
				{
					Type = CommandResultType.MESSAGE,
					Result = $"Is Mapset: {np.IsMapSet} | Id: {np.Id}\n" +
					$"{np.Beatmap} {np.Url}"
				};
			}
			if (e.Data.Message.StartsWith('!'))
			{
				CommandParser parser = new CommandParser();
				parser.Parse(e.Data.Nick, e.Data.Message, true).Send(irc, e.Data.Nick);
			}

		}
		public void OnQueryAction(object sender, IrcEventArgs e)
		{
			Console.WriteLine($"[PM-ACTION] {e.Data.Nick} : {e.Data.Message}");

			// Check NowPlaying
			if (NowPlayingParser.IsNowPlaying(e.Data.Message))
			{
				NowPlaying np = NowPlayingParser.Parse(e.Data.Message);
				if (np.IsMapSet == true)
				{
					new CommandResult()
					{
						Type = CommandResultType.MESSAGE,
						Result = "Sorry. We cannot calculate the mapset."
					}.Send(irc, e.Data.Nick);
					return;
				}
				try
				{
					BanchoUserHistory c = HistoryCache.Get(e.Data.Nick);
					if (c == null)
					{
						c = new BanchoUserHistory(np.Id);
						HistoryCache.cache[e.Data.Nick] = c;
					}
					short _mode = np.Mode == 0 && c.RecentMode == 0 ? (short)0 : np.Mode;
					if (np.NpType == NowPlayingType.LISTENING)
					{
						_mode = c.RecentMode; // There is no mode in text.
					}
					
					Calculator calc = LegacyUtil.GetCalculator(_mode, np.Id);
					string[] mods = np.Mods;
					if (!calc.IsConvertable())
					{
						// Fallback Calculator
						calc = LegacyUtil.GetCalculator((short)calc.Beatmap.BeatmapInfo.RulesetID, np.Id);
					}
					
					calc.Mod = Common.GetMods(mods, calc.Ruleset).ToArray();
					CalculateMessage msg = new CalculateMessage(calc);
					new CommandResult()
					{
						Type = CommandResultType.MESSAGE,
						Result = msg.ToString()
					}.Send(irc, e.Data.Nick);
					
					c.RecentBeatmapId = np.Id;
					c.RecentMode = np.Mode;
					c.RecentMod = new Mod[] { };
				}
				catch (Exception err)
				{
					new CommandResult()
					{
						Type = CommandResultType.MESSAGE,
						Result = $"A Error found: {err.Message}"
					}.Send(irc, e.Data.Nick);
				}


			}
			else if (e.Data.Message.StartsWith("\u0001ACTION is listening to "))
			{
				new CommandResult()
				{
					Type = CommandResultType.MESSAGE,
					Result = "I'm sorry. We can't check/download the un-submitted map."
				}.Send(irc, e.Data.Nick);
			}
		}
	}
}
