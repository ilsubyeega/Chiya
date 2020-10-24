using System;

namespace Chiya
{
	class Program
	{
		static void Main(string[] args)
		{
			string s = Environment.GetEnvironmentVariable("CHIYA_AUTH");
			if (s == null || s.Split("|").Length != 2)
			{
				Console.WriteLine("[ERR] CHIYA_AUTH Environment is not specified. (or invalid)");
				return;
			}
			var client = new IRCClient();
			string[] auth = s.Split("|");
			client.name = auth[0];
			client.password = auth[1];
			client.Connect();
			while (true)
			{
				String a = Console.ReadLine();
				client.irc.RfcJoin(a);
				Console.WriteLine($"Connecting to {a} ({a.Length})");
			}

		}
	}
}
