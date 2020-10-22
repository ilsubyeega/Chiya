using Chiya.Bancho;
using Chiya.Commands.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Commands
{
	public static class HelpCommand
	{
		public static new CommandResult Run(CommandArguments args)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Hello! this is Chiya, made by ilsubyeega.\n");
			sb.Append(new UrlMessage("here", "https://naver.com") + " is the commands list.\n");
			sb.Append("If you find the bug, please add the issue " + new UrlMessage("here", "https://naver.com") + ".");
			return new CommandResult()
			{
				Type = CommandResultType.MESSAGE,
				Result = sb.ToString()
			};
		}
	}
}
