using System;
using UnityEngine;

// Token: 0x020000C0 RID: 192
[Serializable]
public class EnemyWound
{
	// Token: 0x04000A0D RID: 2573
	public EncounterLogMessage CombatLog;

	// Token: 0x04000A0E RID: 2574
	public DamageType[] RequiredDamageTypes;

	// Token: 0x04000A0F RID: 2575
	[StatModifierOptions(true, false)]
	public StatModifier[] StatChanges;

	// Token: 0x04000A10 RID: 2576
	public CardData[] DroppedCards;

	// Token: 0x04000A11 RID: 2577
	public BodyLocationModifiers BodyLocationModifiers;

	// Token: 0x04000A12 RID: 2578
	public EnemyValuesModifiers EnemyValuesModifiers;

	// Token: 0x04000A13 RID: 2579
	public OptionalRangeValue AccumulatedWoundValue;

	// Token: 0x04000A14 RID: 2580
	[Tooltip("Default (unchecked) = -1")]
	public OptionalFloatValue WeaponDurabilityDamage;

	// Token: 0x04000A15 RID: 2581
	public EncounterResult EncounterResult;
}
