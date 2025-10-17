using System;
using System.Collections.Generic;

// Token: 0x02000187 RID: 391
[Serializable]
public class EncounterSaveData
{
	// Token: 0x04001003 RID: 4099
	public string EncounterID;

	// Token: 0x04001004 RID: 4100
	public int CurrentRound;

	// Token: 0x04001005 RID: 4101
	public List<LocalizedString> PreviousLogs;

	// Token: 0x04001006 RID: 4102
	public string SelectedEnemyAction;

	// Token: 0x04001007 RID: 4103
	public float CurrentEnemyMeleeSkill;

	// Token: 0x04001008 RID: 4104
	public float CurrentEnemyRangedSkill;

	// Token: 0x04001009 RID: 4105
	public float CurrentEnemyBlood;

	// Token: 0x0400100A RID: 4106
	public float CurrentEnemyStamina;

	// Token: 0x0400100B RID: 4107
	public float CurrentEnemyMorale;

	// Token: 0x0400100C RID: 4108
	public float CurrentEnemyValue1;

	// Token: 0x0400100D RID: 4109
	public float CurrentEnemyValue2;

	// Token: 0x0400100E RID: 4110
	public float CurrentEnemyValue3;

	// Token: 0x0400100F RID: 4111
	public float CurrentEnemyValue4;

	// Token: 0x04001010 RID: 4112
	public BodyLocationTracking CurrentEnemyBodyProbabilities;

	// Token: 0x04001011 RID: 4113
	public float AllWoundsAccumulated;

	// Token: 0x04001012 RID: 4114
	public float HeadWoundsAccumulated;

	// Token: 0x04001013 RID: 4115
	public float TorsoWoundsAccumulated;

	// Token: 0x04001014 RID: 4116
	public float LeftArmWoundsAccumulated;

	// Token: 0x04001015 RID: 4117
	public float RightArmWoundsAccumulated;

	// Token: 0x04001016 RID: 4118
	public float LeftLegWoundsAccumulated;

	// Token: 0x04001017 RID: 4119
	public float RightLegWoundsAccumulated;

	// Token: 0x04001018 RID: 4120
	public bool EnemyHidden;

	// Token: 0x04001019 RID: 4121
	public bool PlayerHidden;

	// Token: 0x0400101A RID: 4122
	public bool Distant;

	// Token: 0x0400101B RID: 4123
	public EncounterResult Result;
}
