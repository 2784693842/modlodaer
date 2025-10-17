using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020001CF RID: 463
public struct CollectionDropInfo
{
	// Token: 0x06000C7C RID: 3196 RVA: 0x000666B8 File Offset: 0x000648B8
	public int BonusWeight(bool _Stats, bool _Cards, bool _Durabilities)
	{
		float num = 0f;
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
		if (_Durabilities)
		{
			num += this.DurabilitiesWeightMods.TotalWeight;
		}
		return Mathf.RoundToInt(num);
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00066751 File Offset: 0x00064951
	public int FinalWeight
	{
		get
		{
			return this.BaseWeight + this.BonusWeight(true, true, true);
		}
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00066764 File Offset: 0x00064964
	public string SimpleSummary(int _Index, int _PrevRange, int _TotalWeight, bool _Selected)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_Index.ToString());
		stringBuilder.Append("-");
		stringBuilder.Append(string.IsNullOrEmpty(this.CollectionName) ? "Unnamed Collection" : this.CollectionName);
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
		if (this.BonusWeight(true, true, true) != 0)
		{
			if (this.BonusWeight(true, false, false) != 0)
			{
				if (this.BonusWeight(true, false, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(true, false, false).ToString());
				stringBuilder.Append("S");
			}
			if (this.BonusWeight(false, true, false) != 0)
			{
				if (this.BonusWeight(false, true, false) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, true, false).ToString());
				stringBuilder.Append("C");
			}
			if (this.BonusWeight(false, false, true) != 0)
			{
				if (this.BonusWeight(false, false, true) > 0)
				{
					stringBuilder.Append("+");
				}
				stringBuilder.Append(this.BonusWeight(false, false, true).ToString());
				stringBuilder.Append("D");
			}
			stringBuilder.Append("=");
			stringBuilder.Append(this.FinalWeight.ToString());
		}
		stringBuilder.Append(" (");
		stringBuilder.Append(Mathf.Max(0f, (float)this.FinalWeight / (float)_TotalWeight).ToString("0.###%"));
		stringBuilder.Append(") | Collection Uses: ");
		if (this.CollectionUses == Vector2Int.one * -1)
		{
			stringBuilder.Append("Infinite");
		}
		else
		{
			if (this.CollectionUses.x == this.CollectionUses.y)
			{
				stringBuilder.Append("<color=red>");
			}
			stringBuilder.Append(this.CollectionUses.x.ToString());
			stringBuilder.Append("/");
			stringBuilder.Append(this.CollectionUses.y.ToString());
			if (this.CollectionUses.x == this.CollectionUses.y)
			{
				stringBuilder.Append("(DEPLETED)</color>");
			}
		}
		stringBuilder.Append(" | ");
		if (this.Drops.Length == 0 && !this.RevealInventory)
		{
			stringBuilder.Append("<color=red>");
		}
		stringBuilder.Append(this.Drops.Length);
		stringBuilder.Append(" Cards To Drop");
		if (this.Drops.Length == 0 && !this.RevealInventory)
		{
			stringBuilder.Append(" (EMPTY)</color>");
		}
		if (this.RevealInventory)
		{
			stringBuilder.Append(" | Reveals Inventory");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04001155 RID: 4437
	public string CollectionName;

	// Token: 0x04001156 RID: 4438
	public bool IsSuccess;

	// Token: 0x04001157 RID: 4439
	public bool RevealInventory;

	// Token: 0x04001158 RID: 4440
	public int BaseWeight;

	// Token: 0x04001159 RID: 4441
	public List<StatDropWeightModReport> StatWeightMods;

	// Token: 0x0400115A RID: 4442
	public List<CardDropWeightModReport> CardWeightMods;

	// Token: 0x0400115B RID: 4443
	public DurabilityDropWeightModReport DurabilitiesWeightMods;

	// Token: 0x0400115C RID: 4444
	public int RangeUpTo;

	// Token: 0x0400115D RID: 4445
	public CardData[] Drops;

	// Token: 0x0400115E RID: 4446
	public Vector2Int CollectionUses;

	// Token: 0x0400115F RID: 4447
	public StatModifier[] StatMods;
}
