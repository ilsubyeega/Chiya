using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Commands.Object
{
	public class CommandResult
	{
		public string Result;
		public CommandResultType Type = CommandResultType.NONE;

		public void Send(IrcClient irc, string username)
		{
			string[] rs = Result.Split("\n");
			foreach (string r in rs)
				irc.SendMessage(SendType.Message, username, r);
		}
	}
	public enum CommandResultType
	{
		NONE,
		MESSAGE
	}
}
