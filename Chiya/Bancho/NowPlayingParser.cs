using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

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
			GetSongId(np, text);
			var mods = GetMods(text);
			var mode = GetMode(text);
			np.Mods = mods;
			np.Mode = mode;
			np.NpType = GetNpType(text);
			return np;
		}
		public static NowPlayingType GetNpType(string text)
		{
			if (text.StartsWith("\u0001ACTION is listening to "))
			{
				return NowPlayingType.LISTENING;
			} else if (text.StartsWith("\u0001ACTION is playing "))
			{
				return NowPlayingType.PLAYING;
			} else if (text.StartsWith("\u0001ACTION is watching "))
			{
				return NowPlayingType.WATCHING;
			}
			return NowPlayingType.NULL;
		}
		public static void GetSongId(NowPlaying np, string text)
		{
			var tmp = text.Split("\u0001ACTION ")[1].Split(GetNpType(text).GetSplitValue() + " [")[1].Split(" ")[0];
			if (tmp.StartsWith("https://osu.ppy.sh/b/"))
			{
				np.IsMapSet = false;
				np.Id = int.Parse(tmp.Split("https://osu.ppy.sh/b/")[1]);
			} else if (tmp.StartsWith("https://osu.ppy.sh/s/")){
				np.IsMapSet = true;
				np.Id = int.Parse(tmp.Split("https://osu.ppy.sh/s/")[1]);
			} else
			{
				throw new Exception("Cannot parse url for id: " + tmp);
			}
		}
		public static string[] GetMods(string text)
		{
			var tmp = text.Split("\u0001ACTION ")[1].Split(GetNpType(text).GetSplitValue() + " [")[1].Split(" ").Skip(1).Join(" ").Split("] ");
			if (!tmp.Join(" ").Contains(" +")) return new string[] { };
			var tmp_2 = tmp[tmp.Length-1].Split("\u0001")[0].Split("+");
			List<String> value = new List<string>();
			foreach (string _t in tmp_2)
			{
				string _tmp = null;
				if (_t.Length <= 1) // dont add null.
					continue;
				if (_t.EndsWith(" ")) // "Hidden "
					_tmp = _t.Substring(0, _t.IndexOf(" "));
				if (_t.StartsWith("<")) // <osu!mania>
					continue;
				else if (_t.Contains(" |")) // DoubleTime |7K|
				{
					var tmp2 = _t.Split(" ");
					
					if (tmp2.Length >= 1)
					{
						value.Add(tmp2[0]);
						_tmp = tmp2[1].Replace("|", "").Replace("|", ""); // Add external value, if mania.
					} else
					{
						_tmp = tmp2[0];
					}
				} else
				{
					if (_tmp == null || _tmp.Length <= 1)
						_tmp = _t;
				}
				
				value.Add(_tmp);
			}
			return value.ToArray();
		}
		public static short GetMode(string text)
		{
			if (text.Contains(" <Taiko> ")) return 1;
			if (text.Contains(" <CatchTheBeat> ")) return 2;
			if (text.Contains(" <osu!mania> ")) return 3;
			return 0;
		}
	}
	public class NowPlaying
	{
		public short Type = 0; // 0: Std, 1: Taiko, 2: Catch, 3: Mania
		public string Url;
		public string Beatmap;
		public bool IsMapSet; // if it is Mapset 1, if it is Map 0.
		public int Id;
		public string[] Mods = null;
		public NowPlayingType NpType = NowPlayingType.NULL;
		public short Mode = 0;
	}
}
