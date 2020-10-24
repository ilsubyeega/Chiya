using System;

namespace Chiya.Bancho
{
	public class BanchoBotParser
	{
		public BanchoBotParser(String message)
		{
			RawMessage = message;
		}
		public string RawMessage;
		public BanchoBotParseType Type;
		public object Result;
		public void Parse()
		{
			// Beatmap changed to: Nanahira - Chikatto Chika Chika [Sotarks' 1+2 IQ] (https://osu.ppy.sh/b/1969946)
			if (RawMessage.StartsWith("Beatmap changed to") &&
				RawMessage.EndsWith(")"))
			{
				string[] r1 = RawMessage.Split("(https://osu.ppy.sh/b/");
				string r2 = r1[r1.Length - 1];
				string r3 = r2.Split(")")[0];
				Type = BanchoBotParseType.BEATMAP_CHANGED;
				Result = r3;
				Console.WriteLine("TRIGGER");
			}
		}
	}
}
