using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
[Serializable]
public struct EnemyValuesModifiers
{
	// Token: 0x06000751 RID: 1873 RVA: 0x00048AE4 File Offset: 0x00046CE4
	public bool Applies(EnemyValueNames _Value, EnemyActionEffectCondition _HitCondition)
	{
		switch (_Value)
		{
		case EnemyValueNames.MeleeSkill:
			return this.MeleeApplies == EnemyActionEffectCondition.Always || this.MeleeApplies == _HitCondition;
		case EnemyValueNames.RangedSkill:
			return this.RangedApplies == EnemyActionEffectCondition.Always || this.RangedApplies == _HitCondition;
		case EnemyValueNames.Blood:
			return this.BloodApplies == EnemyActionEffectCondition.Always || this.BloodApplies == _HitCondition;
		case EnemyValueNames.Stamina:
			return this.StaminaApplies == EnemyActionEffectCondition.Always || this.StaminaApplies == _HitCondition;
		case EnemyValueNames.Morale:
			return this.MoraleApplies == EnemyActionEffectCondition.Always || this.MoraleApplies == _HitCondition;
		case EnemyValueNames.Value1:
			return this.Value1Applies == EnemyActionEffectCondition.Always || this.Value1Applies == _HitCondition;
		case EnemyValueNames.Value2:
			return this.Value2Applies == EnemyActionEffectCondition.Always || this.Value2Applies == _HitCondition;
		case EnemyValueNames.Value3:
			return this.Value3Applies == EnemyActionEffectCondition.Always || this.Value3Applies == _HitCondition;
		case EnemyValueNames.Value4:
			return this.Value4Applies == EnemyActionEffectCondition.Always || this.Value4Applies == _HitCondition;
		default:
			return false;
		}
	}

	// Token: 0x04000A2C RID: 2604
	public EnemyActionEffectCondition MeleeApplies;

	// Token: 0x04000A2D RID: 2605
	[MinMax]
	public Vector2 MeleeSkillModifier;

	// Token: 0x04000A2E RID: 2606
	[Space]
	public EnemyActionEffectCondition RangedApplies;

	// Token: 0x04000A2F RID: 2607
	[MinMax]
	public Vector2 RangedSkillModifier;

	// Token: 0x04000A30 RID: 2608
	[Space]
	public EnemyActionEffectCondition BloodApplies;

	// Token: 0x04000A31 RID: 2609
	[MinMax]
	public Vector2 BloodModifier;

	// Token: 0x04000A32 RID: 2610
	[Space]
	public EnemyActionEffectCondition StaminaApplies;

	// Token: 0x04000A33 RID: 2611
	[MinMax]
	public Vector2 StaminaModifier;

	// Token: 0x04000A34 RID: 2612
	[Space]
	public EnemyActionEffectCondition MoraleApplies;

	// Token: 0x04000A35 RID: 2613
	[MinMax]
	public Vector2 MoraleModifier;

	// Token: 0x04000A36 RID: 2614
	[Space]
	public EnemyActionEffectCondition Value1Applies;

	// Token: 0x04000A37 RID: 2615
	[MinMax]
	public Vector2 Value1Modifier;

	// Token: 0x04000A38 RID: 2616
	[Space]
	public EnemyActionEffectCondition Value2Applies;

	// Token: 0x04000A39 RID: 2617
	[MinMax]
	public Vector2 Value2Modifier;

	// Token: 0x04000A3A RID: 2618
	[Space]
	public EnemyActionEffectCondition Value3Applies;

	// Token: 0x04000A3B RID: 2619
	[MinMax]
	public Vector2 Value3Modifier;

	// Token: 0x04000A3C RID: 2620
	[Space]
	public EnemyActionEffectCondition Value4Applies;

	// Token: 0x04000A3D RID: 2621
	[MinMax]
	public Vector2 Value4Modifier;
}
