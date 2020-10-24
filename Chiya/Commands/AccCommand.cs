using Chiya.Bancho;
using Chiya.Commands.Object;
using osu.PPCalc;

namespace Chiya.Commands
{
	public class AccCommand
	{
		public static CommandResult Run(CommandArguments args)
		{
			if (args.Arguments.Length == 0)
				return new CommandResult
				{
					Type = CommandResultType.MESSAGE,
					Result = $"Usuage: !acc [Accuracy] (Miss) (Max Combo)"
				};
			double acc = -1;
			int miss = 0;
			int max_combo = -1;
			try
			{
				acc = double.Parse(args.Arguments[0]);
				miss = args.Arguments.Length > 1 ? int.Parse(args.Arguments[1]) : 0;
				max_combo = args.Arguments.Length > 2 ? int.Parse(args.Arguments[2]) : -1;
			}
			catch
			{
				return new CommandResult
				{
					Type = CommandResultType.MESSAGE,
					Result = $"Wrong Argument. (Not a number.)"
				};
			}
			BanchoUserHistory history = HistoryCache.Get(args.Username);
			if (history.RecentBeatmapId == 0)
			{
				return new CommandResult
				{
					Type = CommandResultType.MESSAGE,
					Result = $"Oops! we dont have history! plase /np and do it again!"
				};
			}
			OsuCalculator calc = new OsuCalculator(history.RecentBeatmapId);
			if (history.RecentMod.Length > 0)
				calc.Mod = history.RecentMod;
			calc.Misses = miss;
			CalculateMessage msg = new CalculateMessage(calc);
			msg.Acc = acc;
			if (max_combo != -1)
				calc.Combo = max_combo;
			CommandResult rs = new CommandResult()
			{
				Type = CommandResultType.MESSAGE,
				Result = msg.ToString()
			};


			return rs;
		}
	}

}
