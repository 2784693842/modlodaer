using System;

// Token: 0x0200014A RID: 330
[Serializable]
public struct LocalCounterEffect
{
	// Token: 0x06000960 RID: 2400 RVA: 0x00057C08 File Offset: 0x00055E08
	public bool IsActive(InGameCardBase _ForCard)
	{
		GameManager instance = MBSingleton<GameManager>.Instance;
		return instance && _ForCard && this.Counter && !_ForCard.InBackground && instance.LocalCountersEnv != null && instance.LocalCountersEnv.CountersDict[this.Counter].Value < instance.CountersDict[this.Counter].Value;
	}

	// Token: 0x04000ED9 RID: 3801
	public LocalTickCounter Counter;

	// Token: 0x04000EDA RID: 3802
	public OptionalFloatValue SpoilageRateModifier;

	// Token: 0x04000EDB RID: 3803
	public OptionalFloatValue UsageRateModifier;

	// Token: 0x04000EDC RID: 3804
	public OptionalFloatValue FuelRateModifier;

	// Token: 0x04000EDD RID: 3805
	public OptionalFloatValue ConsumableChargesModifier;

	// Token: 0x04000EDE RID: 3806
	public OptionalFloatValue Special1RateModifier;

	// Token: 0x04000EDF RID: 3807
	public OptionalFloatValue Special2RateModifier;

	// Token: 0x04000EE0 RID: 3808
	public OptionalFloatValue Special3RateModifier;

	// Token: 0x04000EE1 RID: 3809
	public OptionalFloatValue Special4RateModifier;
}
