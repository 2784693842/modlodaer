using System;

// Token: 0x020001D4 RID: 468
public struct DurabilityDropWeightModReport
{
	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00066B52 File Offset: 0x00064D52
	public float TotalWeight
	{
		get
		{
			return this.SpoilageWeight + this.UsageWeight + this.FuelWeight + this.ProgressWeight + this.Special1Weight + this.Special2Weight + this.Special3Weight + this.Special4Weight;
		}
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00066B8C File Offset: 0x00064D8C
	public DurabilityDropWeightModReport(DurabilityBasedDropChanceModifier _Modifier, InGameCardBase _Card)
	{
		if (!_Modifier.WillHaveEffect(_Card))
		{
			this.SpoilageWeight = 0f;
			this.UsageWeight = 0f;
			this.FuelWeight = 0f;
			this.ProgressWeight = 0f;
			this.Special1Weight = 0f;
			this.Special2Weight = 0f;
			this.Special3Weight = 0f;
			this.Special4Weight = 0f;
			return;
		}
		this.SpoilageWeight = (float)_Modifier.Spoilage.GetExtraWeight(_Card.CurrentSpoilage);
		this.UsageWeight = (float)_Modifier.Usage.GetExtraWeight(_Card.CurrentUsageDurability);
		this.FuelWeight = (float)_Modifier.Fuel.GetExtraWeight(_Card.CurrentFuel);
		this.ProgressWeight = (float)_Modifier.Progress.GetExtraWeight(_Card.CurrentProgress);
		this.Special1Weight = (float)_Modifier.Special1.GetExtraWeight(_Card.CurrentSpecial1);
		this.Special2Weight = (float)_Modifier.Special2.GetExtraWeight(_Card.CurrentSpecial2);
		this.Special3Weight = (float)_Modifier.Special3.GetExtraWeight(_Card.CurrentSpecial3);
		this.Special4Weight = (float)_Modifier.Special4.GetExtraWeight(_Card.CurrentSpecial4);
	}

	// Token: 0x04001168 RID: 4456
	public float SpoilageWeight;

	// Token: 0x04001169 RID: 4457
	public float UsageWeight;

	// Token: 0x0400116A RID: 4458
	public float FuelWeight;

	// Token: 0x0400116B RID: 4459
	public float ProgressWeight;

	// Token: 0x0400116C RID: 4460
	public float Special1Weight;

	// Token: 0x0400116D RID: 4461
	public float Special2Weight;

	// Token: 0x0400116E RID: 4462
	public float Special3Weight;

	// Token: 0x0400116F RID: 4463
	public float Special4Weight;
}
