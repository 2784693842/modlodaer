using System;
using UnityEngine;

// Token: 0x020000E1 RID: 225
[Serializable]
public struct CardStateChange
{
	// Token: 0x060007D6 RID: 2006 RVA: 0x0004D73C File Offset: 0x0004B93C
	public bool IsIdentical(CardStateChange _To)
	{
		return _To.ModType == this.ModType && !(_To.TransformInto != this.TransformInto) && _To.TransferSpoilage == this.TransferSpoilage && _To.TransferUsage == this.TransferUsage && _To.TransferFuel == this.TransferFuel && _To.TransferCharges == this.TransferCharges && _To.TransferInventory == this.TransferInventory && _To.TransferLiquid == this.TransferLiquid && !(_To.SpoilageChange != this.SpoilageChange) && !(_To.UsageChange != this.UsageChange) && !(_To.FuelChange != this.FuelChange) && !(_To.ChargesChange != this.ChargesChange) && !(_To.LiquidQuantityChange != this.LiquidQuantityChange) && _To.LiquidEffectScaling == this.LiquidEffectScaling && _To.ModifyDurability == this.ModifyDurability && _To.ModifyLiquid == this.ModifyLiquid && _To.TransferSpecial1 == this.TransferSpecial1 && _To.TransferSpecial2 == this.TransferSpecial2 && _To.TransferSpecial3 == this.TransferSpecial3 && _To.TransferSpecial4 == this.TransferSpecial4 && !(_To.Special1Change != this.Special1Change) && !(_To.Special2Change != this.Special2Change) && !(_To.Special3Change != this.Special3Change) && !(_To.Special4Change != this.Special4Change);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x0004D8FC File Offset: 0x0004BAFC
	public void ApplyDurabilityChanges(TransferedDurabilities _Durabilities)
	{
		this.SpoilageChange = Vector2.one * _Durabilities.Spoilage;
		this.UsageChange = Vector2.one * _Durabilities.Usage;
		this.FuelChange = Vector2.one * _Durabilities.Fuel;
		this.ChargesChange = Vector2.one * _Durabilities.ConsumableCharges;
		this.LiquidQuantityChange = Vector2.one * _Durabilities.Liquid;
		this.Special1Change = Vector2.one * _Durabilities.Special1;
		this.Special2Change = Vector2.one * _Durabilities.Special2;
		this.Special3Change = Vector2.one * _Durabilities.Special3;
		this.Special4Change = Vector2.one * _Durabilities.Special4;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x0004D9F8 File Offset: 0x0004BBF8
	public float StatLiquidScalingValue(InGameCardBase _ForCard)
	{
		if (!this.ModifyLiquid)
		{
			return -1f;
		}
		if (this.LiquidQuantityChange == Vector2.zero)
		{
			return -1f;
		}
		if (Mathf.Min(this.LiquidQuantityChange.x, this.LiquidQuantityChange.y) > 0f)
		{
			return -1f;
		}
		if (!_ForCard)
		{
			return -1f;
		}
		InGameCardBase inGameCardBase = _ForCard.IsLiquid ? _ForCard : _ForCard.ContainedLiquid;
		if (!inGameCardBase)
		{
			return -1f;
		}
		if (inGameCardBase.CurrentLiquidQuantity >= -Mathf.Min(this.LiquidQuantityChange.x, this.LiquidQuantityChange.y))
		{
			return 1f;
		}
		if (this.LiquidEffectScaling == LiquidScaling.AllEffects || this.LiquidEffectScaling == LiquidScaling.StatEffects)
		{
			return inGameCardBase.CurrentLiquidQuantity / -Mathf.Min(this.LiquidQuantityChange.x, this.LiquidQuantityChange.y);
		}
		return -1f;
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0004DAE8 File Offset: 0x0004BCE8
	public float DurabilitiesLiquidScalingValue(InGameCardBase _ForCard)
	{
		if (!this.ModifyLiquid)
		{
			return -1f;
		}
		if (this.LiquidQuantityChange == Vector2.zero)
		{
			return -1f;
		}
		if (Mathf.Min(this.LiquidQuantityChange.x, this.LiquidQuantityChange.y) > 0f)
		{
			return -1f;
		}
		if (!_ForCard)
		{
			return -1f;
		}
		InGameCardBase inGameCardBase = _ForCard.IsLiquid ? _ForCard : _ForCard.ContainedLiquid;
		if (!inGameCardBase)
		{
			return -1f;
		}
		if (inGameCardBase.CurrentLiquidQuantity >= -Mathf.Min(this.LiquidQuantityChange.x, this.LiquidQuantityChange.y))
		{
			return 1f;
		}
		if (this.LiquidEffectScaling == LiquidScaling.AllEffects || this.LiquidEffectScaling == LiquidScaling.DurabilityEffects)
		{
			return inGameCardBase.CurrentLiquidQuantity / -Mathf.Min(this.LiquidQuantityChange.x, this.LiquidQuantityChange.y);
		}
		return -1f;
	}

	// Token: 0x04000B9D RID: 2973
	public CardModifications ModType;

	// Token: 0x04000B9E RID: 2974
	public CardData TransformInto;

	// Token: 0x04000B9F RID: 2975
	public bool TransferSpoilage;

	// Token: 0x04000BA0 RID: 2976
	public bool TransferUsage;

	// Token: 0x04000BA1 RID: 2977
	public bool TransferFuel;

	// Token: 0x04000BA2 RID: 2978
	public bool TransferCharges;

	// Token: 0x04000BA3 RID: 2979
	public bool TransferSpecial1;

	// Token: 0x04000BA4 RID: 2980
	public bool TransferSpecial2;

	// Token: 0x04000BA5 RID: 2981
	public bool TransferSpecial3;

	// Token: 0x04000BA6 RID: 2982
	public bool TransferSpecial4;

	// Token: 0x04000BA7 RID: 2983
	public bool TransferInventory;

	// Token: 0x04000BA8 RID: 2984
	[Tooltip("Only for containers, liquids themselves transfer their quantity automatically when transforming to another liquid")]
	public bool TransferLiquid;

	// Token: 0x04000BA9 RID: 2985
	public bool ModifyDurability;

	// Token: 0x04000BAA RID: 2986
	public bool ModifyLiquid;

	// Token: 0x04000BAB RID: 2987
	public bool DropOnDestroyList;

	// Token: 0x04000BAC RID: 2988
	[SerializeField]
	[Tooltip("Makes the effects of actions be proportional to how much liquid was actually spent by the action\nOnly works if the action TAKES from the liquid\nDOES NOT APPLY TO EXTRA DURABILITY CHANGES")]
	private LiquidScaling LiquidEffectScaling;

	// Token: 0x04000BAD RID: 2989
	[MinMax]
	public Vector2 SpoilageChange;

	// Token: 0x04000BAE RID: 2990
	[MinMax]
	public Vector2 UsageChange;

	// Token: 0x04000BAF RID: 2991
	[MinMax]
	public Vector2 FuelChange;

	// Token: 0x04000BB0 RID: 2992
	[MinMax]
	public Vector2 ChargesChange;

	// Token: 0x04000BB1 RID: 2993
	[MinMax]
	public Vector2 LiquidQuantityChange;

	// Token: 0x04000BB2 RID: 2994
	[MinMax]
	public Vector2 Special1Change;

	// Token: 0x04000BB3 RID: 2995
	[MinMax]
	public Vector2 Special2Change;

	// Token: 0x04000BB4 RID: 2996
	[MinMax]
	public Vector2 Special3Change;

	// Token: 0x04000BB5 RID: 2997
	[MinMax]
	public Vector2 Special4Change;
}
