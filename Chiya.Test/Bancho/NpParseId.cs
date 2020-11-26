using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chiya.Bancho;

namespace Chiya.Test.Bancho
{
	[TestClass]
	public class NpParseId
	{
		[TestMethod]
		public void GetIdtd()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/2129143 Nashimoto Ui - AaAaAaAAaAaAAa [aAaAaaAaAaaA]] +Hidden +DoubleTime";
			NowPlaying np = new NowPlaying();
			NowPlayingParser.GetSongId(np, value);
			Assert.AreEqual(2129143, np.Id);
		}
		[TestMethod]
		public void GetIdTaiko()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1921936 xi remixed by cosMo@bousouP - FREEDOM DiVE [METAL DIMENSIONS] [Hyper]] <Taiko> +Hidden +HardRock";
			NowPlaying np = new NowPlaying();
			NowPlayingParser.GetSongId(np, value);
			Assert.AreEqual(1921936, np.Id);
		}
		[TestMethod]
		public void GetIdCatch()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1599005 Co shu Nie - asphyxia (TV edit) [Ascendance & Spec's Nightmare]] <CatchTheBeat> +Hidden +DoubleTime";
			NowPlaying np = new NowPlaying();
			NowPlayingParser.GetSongId(np, value);
			Assert.AreEqual(1599005, np.Id);
		}
		[TestMethod]
		public void GetIdMania()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1582621 Cranky - Chandelier - King [Royal]] <osu!mania> +Hidden +DoubleTime |7K|";
			NowPlaying np = new NowPlaying();
			NowPlayingParser.GetSongId(np, value);
			Assert.AreEqual(1582621, np.Id);
		}
	}
}
