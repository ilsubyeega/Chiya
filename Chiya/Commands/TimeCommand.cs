using Chiya.Commands.Object;
using System;

namespace Chiya.Commands
{
	public static class TimeCommand
	{
		public static CommandResult Run(CommandArguments args)
		{
			DateTime now = DateTime.Now;
			DateTime utcnow = DateTime.UtcNow;
			TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
			CommandResult rs = new CommandResult()
			{
				Type = CommandResultType.MESSAGE,
				Result = $"The Current time on this bot is: {now.ToString()} (+{offset.ToString()})\n" +
				$"And the UTC Time is {utcnow.ToString()}."
			};


			return rs;
		}
	}
}
