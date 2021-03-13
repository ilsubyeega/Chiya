using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Mania;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Osu;
using osu.Game.Rulesets.Taiko;
using osu.PPCalc;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Bancho
{
	public class CalculateMessage
	{
		private readonly static double[] CalcAcc = new double[] { 0.9, 0.95, 0.96, 0.98, 0.99, 1 };
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
			else if (Calculator.Ruleset is TaikoRuleset)
				suffix += TaikoString();
			if (Calculator.Ruleset is CatchRuleset)
				suffix += CatchString();
			else if (Calculator.Ruleset is ManiaRuleset)
				suffix += ManiaString();
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
			return $"({Calculator.GetType()}) {SR.ToString(format1)} ★ | {Calculator.Beatmap.Metadata.Artist} - {Calculator.Beatmap.Metadata.Title} | {suffix} | ⛏ {Calculator.Beatmap.Metadata.AuthorString} \n" +
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
			if (Calculator.Ruleset is ManiaRuleset)
				Calculator.Score = (int)(10000 * acc * 100);
			var result = Calculator.Calculate();
			return $"{(acc*100).ToString("N2")}% : {result.Item2.ToString(format1)}pp ";
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
			return $"AR {AR.ToString(format1)} | OD {OD.ToString(format1)} | {AimSR.ToString(format1)}★ᵃ | {SpeedSR.ToString(format1)}★ˢ";
		}
		public string TaikoString()
		{
			double OD = -1;
			double RhythmRating = -1;
			double StaminaRating = -1;
			double HitWindow = -1;
			Diffresult.TryGetValue("OD", out OD);
			Diffresult.TryGetValue("Rhythm Rating", out RhythmRating);
			Diffresult.TryGetValue("Stamina Rating", out StaminaRating);
			Diffresult.TryGetValue("Hit Window", out HitWindow);
			return $"OD {OD.ToString(format1)} | {RhythmRating.ToString(format1)} ★ʳ | {StaminaRating.ToString(format1)} ★ˢ | {HitWindow.ToString(format1)} HW";
		}
		public string CatchString()
		{
			double AR = -1;
			double OD = -1;
			Diffresult.TryGetValue("AR", out AR);
			Diffresult.TryGetValue("OD", out OD);
			return $"AR {AR.ToString(format1)} | OD {OD.ToString(format1)}";
		}
		public string ManiaString()
		{
			double OD = -1;
			double HitWindow = -1;
			Diffresult.TryGetValue("OD", out OD);
			Diffresult.TryGetValue("Hit Window", out HitWindow);
			return $"OD {OD.ToString(format1)} | {HitWindow.ToString(format1)} HW";
		}
	}
}
