using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200011C RID: 284
[CreateAssetMenu(menuName = "Survival/Combat/Encounter")]
public class Encounter : UniqueIDScriptable
{
	// Token: 0x04000D59 RID: 3417
	public LocalizedString EncounterTitle;

	// Token: 0x04000D5A RID: 3418
	public EncounterLogMessage EncounterStartingLog;

	// Token: 0x04000D5B RID: 3419
	public Sprite EncounterImage;

	// Token: 0x04000D5C RID: 3420
	public LocalizedString EnemyName;

	// Token: 0x04000D5D RID: 3421
	public bool UsesPlural;

	// Token: 0x04000D5E RID: 3422
	[SpecialHeader("Values", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[MinMax]
	public Vector2 EnemyCover;

	// Token: 0x04000D5F RID: 3423
	[MinMax]
	public Vector2 PlayerCover;

	// Token: 0x04000D60 RID: 3424
	[MinMax]
	public Vector2 EnemyStealth;

	// Token: 0x04000D61 RID: 3425
	[MinMax]
	public Vector2 EnemyAwareness;

	// Token: 0x04000D62 RID: 3426
	[MinMax]
	public Vector2 EnemySize;

	// Token: 0x04000D63 RID: 3427
	public BodyTemplate EnemyBodyTemplate;

	// Token: 0x04000D64 RID: 3428
	public ArmorValues EnemyArmor;

	// Token: 0x04000D65 RID: 3429
	[Space]
	public EnemyValue MeleeSkill;

	// Token: 0x04000D66 RID: 3430
	public EnemyValue RangedSkill;

	// Token: 0x04000D67 RID: 3431
	public EnemyValue Blood;

	// Token: 0x04000D68 RID: 3432
	public EnemyValue Stamina;

	// Token: 0x04000D69 RID: 3433
	public EnemyValue Morale;

	// Token: 0x04000D6A RID: 3434
	public EnemyValue Value1;

	// Token: 0x04000D6B RID: 3435
	public EnemyValue Value2;

	// Token: 0x04000D6C RID: 3436
	public EnemyValue Value3;

	// Token: 0x04000D6D RID: 3437
	public EnemyValue Value4;

	// Token: 0x04000D6E RID: 3438
	[SpecialHeader("Enemy Actions", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public EnemyAction[] EnemyActions;

	// Token: 0x04000D6F RID: 3439
	public EncounterLogMessage EnemyGettingCloseLog;

	// Token: 0x04000D70 RID: 3440
	[SpecialHeader("Player Setup", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public GenericEncounterPlayerAction[] PlayerActions;

	// Token: 0x04000D71 RID: 3441
	public LocalizedString OverrideEscapeActionName;

	// Token: 0x04000D72 RID: 3442
	public EncounterLogMessage OverrideEscapeActionLog;

	// Token: 0x04000D73 RID: 3443
	public PlayerWounds DefaultPlayerWounds;

	// Token: 0x04000D74 RID: 3444
	[SpecialHeader("Results", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[FormerlySerializedAs("PlayerWinEffects")]
	public EncounterResultEffect EnemyDefeatedEffects;

	// Token: 0x04000D75 RID: 3445
	[FormerlySerializedAs("EnemyWinEffects")]
	public EncounterResultEffect EnemyEscapedEffects;

	// Token: 0x04000D76 RID: 3446
	[FormerlySerializedAs("TieEffects")]
	public EncounterResultEffect PlayerEscapedEffects;

	// Token: 0x04000D77 RID: 3447
	public EncounterResultEffect PlayerDemoralizedEffects;

	// Token: 0x04000D78 RID: 3448
	public EncounterResultEffect Special1Effects;

	// Token: 0x04000D79 RID: 3449
	public EncounterResultEffect Special2Effects;

	// Token: 0x04000D7A RID: 3450
	public EncounterResultEffect Special3Effects;

	// Token: 0x04000D7B RID: 3451
	public EncounterResultEffect Special4Effects;
}
