namespace Chiya.Commands.Object
{
	public class CommandArguments
	{
		public string Username;
		public string Label;
		public string[] Arguments;
		public bool isPrivate = false;
		public CommandArguments(string username, string label, string[] arguments, bool isprivate)
		{
			Username = username;
			Label = label;
			Arguments = arguments;
			isPrivate = isprivate;
		}
	}
}
