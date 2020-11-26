using osu.PPCalc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Utils
{
	public static class LegacyUtil
	{
		public static Calculator GetCalculator(short mode, int id)
		{
			switch (mode)
			{
				case 0:
					return new OsuCalculator(id);
				case 1:
					return new TaikoCalculator(id);
				case 2:
					return new CatchCalculator(id);
				case 3:
					return new ManiaCalculator(id);
				default:
					return null;
			}
		}
	}
}
