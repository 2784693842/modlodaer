using System;

// Token: 0x0200002B RID: 43
[Serializable]
public struct EnemyValuesWeightModReport
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600023E RID: 574 RVA: 0x00016E27 File Offset: 0x00015027
	public float TotalWeight
	{
		get
		{
			return this.MeleeSkillWeight + this.RangedSkillWeight + this.BloodWeight + this.StaminaWeight + this.MoraleWeight + this.Value1Weight + this.Value2Weight + this.Value3Weight + this.Value4Weight;
		}
	}

	// Token: 0x0600023F RID: 575 RVA: 0x00016E68 File Offset: 0x00015068
	public EnemyValuesWeightModReport(EnemyValueBasedWeightModifier _Modifier, InGameEncounter _Encounter)
	{
		this.MeleeSkillWeight = (float)_Modifier.WeightPerMeleeSkill.GetExtraWeight(_Encounter.CurrentEnemyMeleeSkill);
		this.RangedSkillWeight = (float)_Modifier.WeightPerRangedSkill.GetExtraWeight(_Encounter.CurrentEnemyRangedSkill);
		this.BloodWeight = (float)_Modifier.WeightPerBlood.GetExtraWeight(_Encounter.CurrentEnemyBlood);
		this.StaminaWeight = (float)_Modifier.WeightPerStamina.GetExtraWeight(_Encounter.CurrentEnemyStamina);
		this.MoraleWeight = (float)_Modifier.WeightPerMorale.GetExtraWeight(_Encounter.CurrentEnemyMorale);
		this.Value1Weight = (float)_Modifier.WeightPerValue1.GetExtraWeight(_Encounter.CurrentEnemyValue1);
		this.Value2Weight = (float)_Modifier.WeightPerValue2.GetExtraWeight(_Encounter.CurrentEnemyValue2);
		this.Value3Weight = (float)_Modifier.WeightPerValue3.GetExtraWeight(_Encounter.CurrentEnemyValue3);
		this.Value4Weight = (float)_Modifier.WeightPerValue4.GetExtraWeight(_Encounter.CurrentEnemyValue4);
	}

	// Token: 0x04000254 RID: 596
	public float MeleeSkillWeight;

	// Token: 0x04000255 RID: 597
	public float RangedSkillWeight;

	// Token: 0x04000256 RID: 598
	public float BloodWeight;

	// Token: 0x04000257 RID: 599
	public float StaminaWeight;

	// Token: 0x04000258 RID: 600
	public float MoraleWeight;

	// Token: 0x04000259 RID: 601
	public float Value1Weight;

	// Token: 0x0400025A RID: 602
	public float Value2Weight;

	// Token: 0x0400025B RID: 603
	public float Value3Weight;

	// Token: 0x0400025C RID: 604
	public float Value4Weight;
}
