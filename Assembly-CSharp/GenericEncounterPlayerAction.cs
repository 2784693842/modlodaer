using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000127 RID: 295
[Serializable]
public class GenericEncounterPlayerAction
{
	// Token: 0x04000DE1 RID: 3553
	public LocalizedString ActionName;

	// Token: 0x04000DE2 RID: 3554
	public EncounterLogMessage ActionSuccessLog;

	// Token: 0x04000DE3 RID: 3555
	public EncounterLogMessage ActionFailureLog;

	// Token: 0x04000DE4 RID: 3556
	public EncounterDistanceCondition RequiredDistance;

	// Token: 0x04000DE5 RID: 3557
	[FormerlySerializedAs("DistanceChange")]
	public EncounterDistanceChange PreClashDistanceChange;

	// Token: 0x04000DE6 RID: 3558
	public ActionRange ActionRange;

	// Token: 0x04000DE7 RID: 3559
	public float Reach;

	// Token: 0x04000DE8 RID: 3560
	public bool DoesNotAttack;

	// Token: 0x04000DE9 RID: 3561
	public bool CannotFailClash;

	// Token: 0x04000DEA RID: 3562
	public bool DontShowSuccessChance;

	// Token: 0x04000DEB RID: 3563
	public bool IsEscapeAction;

	// Token: 0x04000DEC RID: 3564
	public DamageType[] DamageTypes;

	// Token: 0x04000DED RID: 3565
	public Vector2 InitialDamage;

	// Token: 0x04000DEE RID: 3566
	public Vector2 InitialClashValue;

	// Token: 0x04000DEF RID: 3567
	public Vector2 ClashRangedInaccuracy;

	// Token: 0x04000DF0 RID: 3568
	public Vector2 ClashStealthBonus;

	// Token: 0x04000DF1 RID: 3569
	public Vector2 ClashIneffectiveRangeMalus;

	// Token: 0x04000DF2 RID: 3570
	public Vector2 ClashVsEscapeModifier;

	// Token: 0x04000DF3 RID: 3571
	public Vector2 DmgVsEscapeModifier;

	// Token: 0x04000DF4 RID: 3572
	public PlayerEncounterVariable[] ClashStatInfluences;

	// Token: 0x04000DF5 RID: 3573
	public PlayerEncounterVariable[] DamageStatInfluences;

	// Token: 0x04000DF6 RID: 3574
	public StatModifier[] ActionStatChanges;

	// Token: 0x04000DF7 RID: 3575
	public EncounterResult EncounterResult;
}
