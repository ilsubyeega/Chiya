using Microsoft.EntityFrameworkCore.Internal;
using System;

namespace Chiya.Bancho
{
	public static class NowPlayingParser
	{
		public static bool IsNowPlaying(String text)
		{
			string[] pattern = new string[] { "\u0001", "is", "ing", "[https://osu.ppy.sh/", "]" };
			foreach (string p in pattern)
				if (!text.Contains(p)) return false;
			return true;
		}

		public static NowPlaying Parse(string text)
		{
			NowPlaying np = new NowPlaying();
			if (text.StartsWith("\u0001ACTION is listening to"))
			{
				np.Url = text.Split("\u0001ACTION is listening to [")[1].Split(" ")[0];
			} else if (text.StartsWith("\u0001ACTION is playing"))
			{
				np.Url = text.Split("\u0001ACTION is playing [")[1].Split(" ")[0];
			}
			
			np.Beatmap = text.Split(np.Url)[1].Substring(1).Split("]\u0001")[0];
			if (np.Url.Contains("https://osu.ppy.sh/b/"))
			{
				np.Id = int.Parse(np.Url.Split("https://osu.ppy.sh/b/")[1]);
				np.IsMapSet = false;
			}
			else if (np.Url.Contains("https://osu.ppy.sh/s/"))
			{
				np.Id = int.Parse(np.Url.Split("https://osu.ppy.sh/s/")[1]);
				np.IsMapSet = true;
			}
			else
			{
				np.Id = 0; // null
			}
			return np;
		}
	}
	public class NowPlaying
	{
		public string Url;
		public string Beatmap;
		public bool IsMapSet; // if it is Mapset 1, if it is Map 0.
		public int Id;
	}
}
