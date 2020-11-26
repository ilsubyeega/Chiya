using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chiya.Bancho;

namespace Chiya.Test.Bancho
{
	[TestClass]
	public class NpParseMode
	{
		[TestMethod]
		public void GetModeStd()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/2129143 Nashimoto Ui - AaAaAaAAaAaAAa [aAaAaaAaAaaA]] +Hidden +DoubleTime";
			var mode = NowPlayingParser.GetMode(value);
			Assert.AreEqual(0, mode);
		}
		[TestMethod]
		public void GetModeTaiko()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1921936 xi remixed by cosMo@bousouP - FREEDOM DiVE [METAL DIMENSIONS] [Hyper]] <Taiko> +Hidden +HardRock";
			var mode = NowPlayingParser.GetMode(value);
			Assert.AreEqual(1, mode);
		}
		[TestMethod]
		public void GetModeCatch()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1599005 Co shu Nie - asphyxia (TV edit) [Ascendance & Spec's Nightmare]] <CatchTheBeat> +Hidden +DoubleTime";
			var mode = NowPlayingParser.GetMode(value);
			Assert.AreEqual(2, mode);
		}
		[TestMethod]
		public void GetModeMania()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1582621 Cranky - Chandelier - King [Royal]] <osu!mania> +Hidden +DoubleTime |7K|";
			var mode = NowPlayingParser.GetMode(value);
			Assert.AreEqual(3, mode);
		}
	}
}
