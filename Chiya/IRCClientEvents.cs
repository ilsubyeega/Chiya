using Chiya.Bancho;
using Chiya.Commands;
using Chiya.Commands.Object;
using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Text;

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
				if (raw.Contains("JOIN") || raw.Contains("QUIT") || raw.Contains("End of /WHO list.") || raw.Contains("PART") ) return;
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
				new CommandResult()
				{
					Type = CommandResultType.MESSAGE,
					Result = $"Is Mapset: {np.IsMapSet} | Id: {np.Id}\n" +
					$"{np.Beatmap} {np.Url}"
				}.Send(irc, e.Data.Nick);
			} else if (e.Data.Message.StartsWith("\u0001ACTION is listening to "))
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
