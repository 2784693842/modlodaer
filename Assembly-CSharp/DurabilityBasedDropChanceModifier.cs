using System;

// Token: 0x020000ED RID: 237
[Serializable]
public struct DurabilityBasedDropChanceModifier
{
	// Token: 0x0600080F RID: 2063 RVA: 0x0004FAA8 File Offset: 0x0004DCA8
	public int GetExtraWeight(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return 0;
		}
		return this.Spoilage.GetExtraWeight(_Card.CurrentSpoilage) + this.Usage.GetExtraWeight(_Card.CurrentUsageDurability) + this.Fuel.GetExtraWeight(_Card.CurrentFuel) + this.Progress.GetExtraWeight(_Card.CurrentProgress) + this.Special1.GetExtraWeight(_Card.CurrentSpecial1) + this.Special2.GetExtraWeight(_Card.CurrentSpecial2) + this.Special3.GetExtraWeight(_Card.CurrentSpecial3) + this.Special4.GetExtraWeight(_Card.CurrentSpecial4);
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x0004FB50 File Offset: 0x0004DD50
	public bool WillHaveEffect(InGameCardBase _Card)
	{
		return _Card && (this.Spoilage.WillHaveEffect(_Card.CurrentSpoilage) || this.Usage.WillHaveEffect(_Card.CurrentUsageDurability) || this.Fuel.WillHaveEffect(_Card.CurrentFuel) || this.Progress.WillHaveEffect(_Card.CurrentProgress) || this.Special1.WillHaveEffect(_Card.CurrentSpecial1) || this.Special2.WillHaveEffect(_Card.CurrentSpecial2) || this.Special3.WillHaveEffect(_Card.CurrentSpecial3) || this.Special4.WillHaveEffect(_Card.CurrentSpecial4));
	}

	// Token: 0x04000C20 RID: 3104
	public DurabilityWeightValue Spoilage;

	// Token: 0x04000C21 RID: 3105
	public DurabilityWeightValue Usage;

	// Token: 0x04000C22 RID: 3106
	public DurabilityWeightValue Fuel;

	// Token: 0x04000C23 RID: 3107
	public DurabilityWeightValue Progress;

	// Token: 0x04000C24 RID: 3108
	public DurabilityWeightValue Special1;

	// Token: 0x04000C25 RID: 3109
	public DurabilityWeightValue Special2;

	// Token: 0x04000C26 RID: 3110
	public DurabilityWeightValue Special3;

	// Token: 0x04000C27 RID: 3111
	public DurabilityWeightValue Special4;
}
