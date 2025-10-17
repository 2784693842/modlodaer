using System;

// Token: 0x0200011F RID: 287
[Serializable]
public struct EnemyWoundBasedWeightModifier
{
	// Token: 0x04000DAC RID: 3500
	public DurabilityWeightValue WeightPerAllWounds;

	// Token: 0x04000DAD RID: 3501
	public DurabilityWeightValue WeightPerHeadWounds;

	// Token: 0x04000DAE RID: 3502
	public DurabilityWeightValue WeightPerTorsoWounds;

	// Token: 0x04000DAF RID: 3503
	public DurabilityWeightValue WeightPerLArmWounds;

	// Token: 0x04000DB0 RID: 3504
	public DurabilityWeightValue WeightPerRArmWounds;

	// Token: 0x04000DB1 RID: 3505
	public DurabilityWeightValue WeightPerLLegWounds;

	// Token: 0x04000DB2 RID: 3506
	public DurabilityWeightValue WeightPerRLegWounds;
}
