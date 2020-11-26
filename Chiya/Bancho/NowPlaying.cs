using System;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Bancho
{
	public enum NowPlayingType
	{
		NULL,
		LISTENING,
		PLAYING,
		WATCHING
	}
	public static class NowPlayingUtil
	{
		public static string GetSplitValue(this NowPlayingType type)
		{
			switch (type)
			{
				default:
					throw new Exception("No Type of this now playing value.");
				case NowPlayingType.LISTENING:
					return "is listening to";
				case NowPlayingType.PLAYING:
					return "is playing";
				case NowPlayingType.WATCHING:
					return "is watching";
			}
		}
	}
}
