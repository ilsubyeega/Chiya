using Chiya.Bancho;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Test.Bancho
{
	[TestClass]
	public class NpParseMods
	{
		[TestMethod]
		public void GetModsStdNull()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/2245774 Turbo - PADORU / PADORU [Gift]]";
			var mods = NowPlayingParser.GetMods(value);
			string[] excepted = { };
			CollectionAssert.AreEqual(excepted, mods);
		}
		[TestMethod]
		public void GetModsStd()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/2129143 Nashimoto Ui - AaAaAaAAaAaAAa [aAaAaaAaAaaA]] +Hidden +DoubleTime";
			var mods = NowPlayingParser.GetMods(value);
			string[] excepted = { "Hidden", "DoubleTime" };
			CollectionAssert.AreEqual(excepted, mods);
		}
		[TestMethod]
		public void GetModsTaiko()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1921936 xi remixed by cosMo@bousouP - FREEDOM DiVE [METAL DIMENSIONS] [Hyper]] <Taiko> +Hidden +HardRock";
			var mods = NowPlayingParser.GetMods(value);
			string[] excepted = { "Hidden", "HardRock" };
			CollectionAssert.AreEqual(excepted, mods);
		}
		[TestMethod]
		public void GetModsCatch()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1599005 Co shu Nie - asphyxia (TV edit) [Ascendance & Spec's Nightmare]] <CatchTheBeat> +Hidden +DoubleTime";
			var mods = NowPlayingParser.GetMods(value);
			string[] excepted = { "Hidden", "DoubleTime" };
			CollectionAssert.AreEqual(excepted, mods);
		}
		[TestMethod]
		public void GetModsMania()
		{
			string value = "ACTION is playing [https://osu.ppy.sh/b/1582621 Cranky - Chandelier - King [Royal]] <osu!mania> +Hidden +DoubleTime |7K|";
			var mods = NowPlayingParser.GetMods(value);
			string[] excepted = { "Hidden", "DoubleTime", "7K" };
			CollectionAssert.AreEqual(excepted, mods);
		}
	}
}
