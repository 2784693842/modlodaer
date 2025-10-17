using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x0200002A RID: 42
[Serializable]
public struct EnemyActionSelectionInfo
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600023B RID: 571 RVA: 0x0001690C File Offset: 0x00014B0C
	public int FinalWeight
	{
		get
		{
			return this.BaseWeight + this.BonusWeight(true, true, true, true, true, true, true, true);
		}
	}

	// Token: 0x0600023C RID: 572 RVA: 0x00016930 File Offset: 0x00014B30
	public int BonusWeight(bool _PlayerHidden, bool _EnemyHidden, bool _Distance, bool _CloseRange, bool _Stats, bool _Cards, bool _Values, bool _Wounds)
	{
		float num = 0f;
		if (_PlayerHidden)
		{
			num += (float)this.PlayerHiddenWeightMod;
		}
		if (_EnemyHidden)
		{
			num += (float)this.EnemyHiddenWeightMod;
		}
		if (_Distance)
		{
			num += (float)this.DistanceWeightMod;
		}
		if (_CloseRange)
		{
			num += (float)this.CloseWeightMod;
		}
		if (this.StatWeightMods != null && _Stats)
		{
			for (int i = 0; i < this.StatWeightMods.Count; i++)
			{
				num += (float)this.StatWeightMods[i].BonusWeight;
			}
		}
		if (this.CardWeightMods != null && _Cards)
		{
			for (int j = 0; j < this.CardWeightMods.Count; j++)
			{
				num += this.CardWeightMods[j].BonusWeight;
			}
		}
		if (_Values)
		{
			num += this.ValuesWeightMods.TotalWeight;
		}
		if (_Wounds)
		{
			num += this.WoundsWeightMods.TotalWeight;
		}
		return Mathf.RoundToInt(num);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x00016A14 File Offset: 0x00014C14
	public string SimpleSummary(int _Index, int _PrevRange, int _TotalWeight, bool _Selected)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_Index.ToString());
		stringBuilder.Append("-");
		stringBuilder.Append(string.IsNullOrEmpty(this.ActionName) ? "Unnamed Action" : this.ActionName);
		if (_Selected)
		{
			stringBuilder.Append(" SELECTED!");
		}
		stringBuilder.Append("\nRange: ");
		if (this.RangeUpTo > 0)
		{
			stringBuilder.Append(_PrevRange.ToString());
			stringBuilder.Append("_");
			stringBuilder.Append(this.RangeUpTo.ToString());
		}
		else
		{
			stringBuilder.Append("<color=red>");
			stringBuilder.Append(this.RangeUpTo.ToString());
			stringBuilder.Append(" (OUT)</color>");
		}
		stringBuilder.Append(" | Weight: ");
		stringBuilder.Append(this.BaseWeight.ToString());
		if (this.BonusWeight(true, true, true, true, true, true, true, true) != 0)
		{
			if (this.BonusWeight(true, false, false, false, false, false, false, false) != 0)
			{
				if (this.BonusWeight(true, false, false, false, false, false, false, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(true, false, false, false, false, false, false, false).ToString());
				stringBuilder.Append("PlH");
			}
			if (this.BonusWeight(false, true, false, false, false, false, false, false) != 0)
			{
				if (this.BonusWeight(false, true, false, false, false, false, false, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, true, false, false, false, false, false, false).ToString());
				stringBuilder.Append("EnH");
			}
			if (this.BonusWeight(false, false, true, false, false, false, false, false) != 0)
			{
				if (this.BonusWeight(false, false, true, false, false, false, false, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, false, true, false, false, false, false, false).ToString());
				stringBuilder.Append("Dist");
			}
			if (this.BonusWeight(false, false, false, true, false, false, false, false) != 0)
			{
				if (this.BonusWeight(false, false, false, true, false, false, false, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, false, false, true, false, false, false, false).ToString());
				stringBuilder.Append("Close");
			}
			if (this.BonusWeight(false, false, false, false, true, false, false, false) != 0)
			{
				if (this.BonusWeight(false, false, false, false, true, false, false, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, false, false, false, true, false, false, false).ToString());
				stringBuilder.Append("S");
			}
			if (this.BonusWeight(false, false, false, false, false, true, false, false) != 0)
			{
				if (this.BonusWeight(false, false, false, false, false, true, false, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, false, false, false, false, true, false, false).ToString());
				stringBuilder.Append("C");
			}
			if (this.BonusWeight(false, false, false, false, false, false, true, false) != 0)
			{
				if (this.BonusWeight(false, false, false, false, false, false, true, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, false, false, false, false, false, true, false).ToString());
				stringBuilder.Append("V");
			}
			if (this.BonusWeight(false, false, false, false, false, false, false, true) != 0)
			{
				if (this.BonusWeight(false, false, false, false, false, false, false, true) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, false, false, false, false, false, false, true).ToString());
				stringBuilder.Append("W");
			}
			stringBuilder.Append("=");
			stringBuilder.Append(this.FinalWeight.ToString());
		}
		stringBuilder.Append(" (");
		stringBuilder.Append(Mathf.Max(0f, (float)this.FinalWeight / (float)_TotalWeight).ToString("0.###%"));
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	// Token: 0x04000249 RID: 585
	public string ActionName;

	// Token: 0x0400024A RID: 586
	public int BaseWeight;

	// Token: 0x0400024B RID: 587
	public int DistanceWeightMod;

	// Token: 0x0400024C RID: 588
	public int CloseWeightMod;

	// Token: 0x0400024D RID: 589
	public int EnemyHiddenWeightMod;

	// Token: 0x0400024E RID: 590
	public int PlayerHiddenWeightMod;

	// Token: 0x0400024F RID: 591
	public List<StatDropWeightModReport> StatWeightMods;

	// Token: 0x04000250 RID: 592
	public List<CardDropWeightModReport> CardWeightMods;

	// Token: 0x04000251 RID: 593
	public EnemyValuesWeightModReport ValuesWeightMods;

	// Token: 0x04000252 RID: 594
	public EnemyWoundsWeightModReport WoundsWeightMods;

	// Token: 0x04000253 RID: 595
	public int RangeUpTo;
}
