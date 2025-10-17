using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
[Serializable]
public struct DurabilitiesConditions
{
	// Token: 0x06000829 RID: 2089 RVA: 0x00050308 File Offset: 0x0004E508
	public void Clear()
	{
		this.SpoilageRange = Vector2.zero;
		this.UsageRange = Vector2.zero;
		this.FuelRange = Vector2.zero;
		this.ProgressRange = Vector2.zero;
		this.Special1Range = Vector2.zero;
		this.Special2Range = Vector2.zero;
		this.Special3Range = Vector2.zero;
		this.Special4Range = Vector2.zero;
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x0600082A RID: 2090 RVA: 0x00050370 File Offset: 0x0004E570
	public bool IsEmpty
	{
		get
		{
			return this.SpoilageRange == Vector2.zero && this.UsageRange == Vector2.zero && this.FuelRange == Vector2.zero && this.ProgressRange == Vector2.zero && this.Special1Range == Vector2.zero && this.Special2Range == Vector2.zero && this.Special3Range == Vector2.zero && this.Special4Range == Vector2.zero && this.LiquidQuantityRange == Vector2.zero;
		}
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00050424 File Offset: 0x0004E624
	public bool ValidConditions(InGameCardBase _ForCard)
	{
		if (this.IsEmpty)
		{
			return true;
		}
		if (this.SpoilageRange != Vector2.zero && _ForCard.CardModel.SpoilageTime && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentSpoilage, this.SpoilageRange, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.UsageRange != Vector2.zero && _ForCard.CardModel.UsageDurability && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentUsageDurability, this.UsageRange, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.FuelRange != Vector2.zero && _ForCard.CardModel.FuelCapacity && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentFuel, this.FuelRange, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.ProgressRange != Vector2.zero && _ForCard.CardModel.Progress && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentProgress, this.ProgressRange, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.Special1Range != Vector2.zero && _ForCard.CardModel.SpecialDurability1 && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentSpecial1, this.Special1Range, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.Special2Range != Vector2.zero && _ForCard.CardModel.SpecialDurability2 && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentSpecial2, this.Special2Range, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.Special3Range != Vector2.zero && _ForCard.CardModel.SpecialDurability3 && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentSpecial3, this.Special3Range, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.Special4Range != Vector2.zero && _ForCard.CardModel.SpecialDurability4 && !ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentSpecial4, this.Special4Range, RoundingMethods.Floor))
		{
			return false;
		}
		if (this.LiquidQuantityRange != Vector2.zero && (_ForCard.IsLiquid || _ForCard.IsLiquidContainer))
		{
			if (_ForCard.IsLiquid)
			{
				if (!ExtraMath.FloatIsInRangeOrGreater(_ForCard.CurrentLiquidQuantity, this.LiquidQuantityRange, RoundingMethods.Floor))
				{
					return false;
				}
			}
			else
			{
				if (!_ForCard.ContainedLiquid)
				{
					return false;
				}
				if (!ExtraMath.FloatIsInRangeOrGreater(_ForCard.ContainedLiquid.CurrentLiquidQuantity, this.LiquidQuantityRange, RoundingMethods.Floor))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04000C4A RID: 3146
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 SpoilageRange;

	// Token: 0x04000C4B RID: 3147
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 UsageRange;

	// Token: 0x04000C4C RID: 3148
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 FuelRange;

	// Token: 0x04000C4D RID: 3149
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 ProgressRange;

	// Token: 0x04000C4E RID: 3150
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 Special1Range;

	// Token: 0x04000C4F RID: 3151
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 Special2Range;

	// Token: 0x04000C50 RID: 3152
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 Special3Range;

	// Token: 0x04000C51 RID: 3153
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 Special4Range;

	// Token: 0x04000C52 RID: 3154
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 LiquidQuantityRange;
}
