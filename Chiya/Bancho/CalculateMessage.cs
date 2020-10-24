using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.PPCalc;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Bancho
{
	public class CalculateMessage
	{
		private readonly static double[] CalcAcc = new double[] { 95, 96, 97, 98, 99, 100 };
		private readonly static string format1 = "0.##";
		public CalculateMessage(Calculator calc)
		{
			Calculator = calc;
			Diffresult = calc.GetDifficulty();
		}
		public CalculateMessage(Calculator calc, Dictionary<string, double> diffresult)
		{
			Calculator = calc;
			Diffresult = diffresult;
		}

		public Calculator Calculator;
		public Dictionary<string, double> Diffresult;
		public double Acc = -1;
		public override string ToString()
		{
			double SR = -1;
			Diffresult.TryGetValue("SR", out SR);
			double BPM = -1;
			Diffresult.TryGetValue("BPM", out BPM);
			double MaxCombo = -1;
			Diffresult.TryGetValue("Max Combo", out MaxCombo);
			string suffix = "";
			if (Calculator.Ruleset is OsuRuleset)
				suffix += OsuString();
			string mods = "";
			if (Calculator.Mod.Length > 0)
			{
				try
				{
					Mod[] modlist = Calculator.Mod;
					if (modlist.Length > 0)
						mods += "+";
					foreach (Mod mod in modlist)
					{
						mods += mod.Acronym;
					}
					mods += " ";
				}
				catch
				{
					// just dont show mods..
				}
			}
			string calc = (Acc == -1) ? CalcString() : CalcString(Acc);
			return $"{Calculator.Beatmap.Metadata.Artist} - {Calculator.Beatmap.Metadata.Title} | {SR.ToString(format1)}★ | {suffix} | ⛏ {Calculator.Beatmap.Metadata.AuthorString} \n" +
					$"{mods} {calc}| Max Combo: {MaxCombo}x";
		}
		public string CalcString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < CalcAcc.Length; i++)
			{
				double acc = CalcAcc[i];
				sb.Append(CalcString(acc));
				if (i != CalcAcc.Length - 1)
					sb.Append("| ");
			}
			return sb.ToString();
		}
		public string CalcString(double acc)
		{
			Calculator.Accuracy = acc;
			var result = Calculator.Calculate();
			return $"{acc}% : {result.Item2.ToString(format1)}pp ";
		}
		public string OsuString()
		{
			double AimSR = -1;
			double SpeedSR = -1;
			double AR = -1;
			double OD = -1;
			Diffresult.TryGetValue("Speed Rating", out AimSR);
			Diffresult.TryGetValue("Aim Rating", out SpeedSR);
			Diffresult.TryGetValue("AR", out AR);
			Diffresult.TryGetValue("OD", out OD);
			string k = $"AR {AR.ToString(format1)} | OD {OD.ToString(format1)} | {AimSR.ToString(format1)}★ᵃ | {SpeedSR.ToString(format1)}★ˢ";
			return k;
		}
	}
}
