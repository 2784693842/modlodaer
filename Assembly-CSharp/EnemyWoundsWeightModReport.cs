using System;

// Token: 0x0200002C RID: 44
[Serializable]
public struct EnemyWoundsWeightModReport
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000240 RID: 576 RVA: 0x00016F56 File Offset: 0x00015156
	public float TotalWeight
	{
		get
		{
			return this.AllWoundsWeight + this.HeadWoundsWeight + this.TorsoWoundsWeight + this.LArmWoundsWeight + this.RArmWoundsWeight + this.LLegWoundsWeight + this.RLegWoundsWeight;
		}
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00016F88 File Offset: 0x00015188
	public EnemyWoundsWeightModReport(EnemyWoundBasedWeightModifier _Modifier, InGameEncounter _Encounter)
	{
		this.AllWoundsWeight = (float)_Modifier.WeightPerAllWounds.GetExtraWeight(_Encounter.AllWoundsAccumulated);
		this.HeadWoundsWeight = (float)_Modifier.WeightPerHeadWounds.GetExtraWeight(_Encounter.HeadWoundsAccumulated);
		this.TorsoWoundsWeight = (float)_Modifier.WeightPerTorsoWounds.GetExtraWeight(_Encounter.TorsoWoundsAccumulated);
		this.LArmWoundsWeight = (float)_Modifier.WeightPerLArmWounds.GetExtraWeight(_Encounter.LeftArmWoundsAccumulated);
		this.RArmWoundsWeight = (float)_Modifier.WeightPerRArmWounds.GetExtraWeight(_Encounter.RightArmWoundsAccumulated);
		this.LLegWoundsWeight = (float)_Modifier.WeightPerLLegWounds.GetExtraWeight(_Encounter.LeftLegWoundsAccumulated);
		this.RLegWoundsWeight = (float)_Modifier.WeightPerRLegWounds.GetExtraWeight(_Encounter.RightLegWoundsAccumulated);
	}

	// Token: 0x0400025D RID: 605
	public float AllWoundsWeight;

	// Token: 0x0400025E RID: 606
	public float HeadWoundsWeight;

	// Token: 0x0400025F RID: 607
	public float TorsoWoundsWeight;

	// Token: 0x04000260 RID: 608
	public float LArmWoundsWeight;

	// Token: 0x04000261 RID: 609
	public float RArmWoundsWeight;

	// Token: 0x04000262 RID: 610
	public float LLegWoundsWeight;

	// Token: 0x04000263 RID: 611
	public float RLegWoundsWeight;
}
