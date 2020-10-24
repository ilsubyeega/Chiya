using Chiya.Bancho;
using Chiya.Commands.Object;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.PPCalc;
using System;
using System.Text;

namespace Chiya.Commands
{
	public class WithCommand
	{
		public static CommandResult Run(CommandArguments args)
		{
			if (args.Arguments.Length == 0)
				return new CommandResult
				{
					Type = CommandResultType.MESSAGE,
					Result = $"Usuage: !with [Mods] (Mods) (Mods)... or !with reset to reset."
				};
			Mod[] mods = null;


			BanchoUserHistory history = HistoryCache.Get(args.Username);
			if (history == null)
				HistoryCache.Init(args.Username);
			history = HistoryCache.Get(args.Username);


			if (args.Arguments[0] == "reset")
			{
				history.RecentMod = new Mod[] { };
				return new CommandResult
				{
					Type = CommandResultType.MESSAGE,
					Result = $"Reseted mods."
				};
			}
			try
			{
				mods = Common.GetMods(args.Arguments, new OsuRuleset()).ToArray();
			}
			catch (Exception ex)
			{
				return new CommandResult
				{
					Type = CommandResultType.MESSAGE,
					Result = ex.Message
				};
			}
			history.RecentMod = mods;
			StringBuilder sb = new StringBuilder();
			foreach (Mod mod in mods)
				sb.Append(mod.Acronym);
			CommandResult rs = new CommandResult
			{
				Type = CommandResultType.MESSAGE,
				Result = "Applied Mods: +" + sb.ToString() + ". Try it by !acc [Accuracy]."
			};
			return rs;
		}
	}
}
