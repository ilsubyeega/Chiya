using osu.Game.Rulesets.Mods;

namespace Chiya.Bancho
{
	public class BanchoUserHistory
	{
		public BanchoUserHistory() { }
		public BanchoUserHistory(int recentbeatmapid)
		{
			RecentBeatmapId = recentbeatmapid;
		}
		public int RecentBeatmapId = 0;
		public int Acc = 100;
		public Mod[] RecentMod = new Mod[] { };
	}
}
