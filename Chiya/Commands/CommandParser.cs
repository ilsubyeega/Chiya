using Chiya.Commands.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chiya.Commands
{
	public class CommandParser
	{

		public static Dictionary<string, string> commands = new Dictionary<string, string>();

		public CommandResult Parse(string username, string message, bool isPrivate)
		{
			string msg = message.Substring(1);
			string command = msg.Split(' ')[0].ToLower();
			string[] args = msg.Substring(command.Length).Split(" ");
			return Parse(new CommandArguments(username, command, args, isPrivate));
		}
		public CommandResult Parse(CommandArguments args)
		{
			Type type = GetCommandType(args.Label);
			if (type == null)
			{
				
				return new CommandResult()
				{
					Type = CommandResultType.MESSAGE,
					Result = "Unknown Command."
				};
			}
			try
			{
				CommandResult result = type.GetMethod("Run").Invoke(null, new object[] { args }) as CommandResult;
				// Check the value
				if (result?.Result == null || result.Result.Length == 0) throw new Exception("Wrong Command Parsing.");
				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine($"[ERROR] {e.Message}\n[ERROR] {e.StackTrace}\n[ERROR] {e.Source}");
				return new CommandResult()
				{
					Type = CommandResultType.MESSAGE,
					Result = "An error occurred while performing the command. Please show this to the developer." +
					"\n" + e.Message
				};
			}

			
		}
		public Type GetCommandType(string label)
		{
			string up_label = GetUppercase(label);
			if (up_label == null || up_label.Length == 0)
				return null;
			return Type.GetType("Chiya.Commands." + up_label + "Command");
		}

		public string GetUppercase(string s) => (string.IsNullOrEmpty(s)) ? null : char.ToUpper(s[0]) + s.Substring(1);
	}
}
