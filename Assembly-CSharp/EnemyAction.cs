using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200011D RID: 285
[Serializable]
public class EnemyAction
{
	// Token: 0x060008CA RID: 2250 RVA: 0x0005488C File Offset: 0x00052A8C
	public void GetStatWeightMods(Encounter _EncounterObject, List<StatDropWeightModReport> _Reports, bool _ClearList = true)
	{
		if (_Reports == null)
		{
			return;
		}
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (_ClearList)
		{
			_Reports.Clear();
		}
		if (this.StatsWeightModifiers != null && this.StatsWeightModifiers.Length != 0)
		{
			for (int i = 0; i < this.StatsWeightModifiers.Length; i++)
			{
				if (!this.StatsWeightModifiers[i].Stat)
				{
					if (!_EncounterObject)
					{
						Debug.LogError("Empty drop modifier in Enemy Action " + this.ActionLog);
					}
					else
					{
						Debug.LogError("Empty drop modifier in Enemy Action " + this.ActionLog + " in " + _EncounterObject.name, _EncounterObject);
					}
				}
				else if (instance.StatsDict.ContainsKey(this.StatsWeightModifiers[i].Stat) && this.StatsWeightModifiers[i].WillHaveEffect(instance.StatsDict[this.StatsWeightModifiers[i].Stat].CurrentValue(instance.NotInBase)))
				{
					_Reports.Add(this.StatsWeightModifiers[i].ToReport(instance.StatsDict[this.StatsWeightModifiers[i].Stat].CurrentValue(instance.NotInBase)));
				}
			}
		}
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x000549DC File Offset: 0x00052BDC
	public void GetCardWeightMods(List<CardDropWeightModReport> _Reports, bool _ClearList = true)
	{
		if (_Reports == null)
		{
			return;
		}
		if (_ClearList)
		{
			_Reports.Clear();
		}
		if (this.CardsOnBoardWeightModifiers != null && this.CardsOnBoardWeightModifiers.Length != 0)
		{
			for (int i = 0; i < this.CardsOnBoardWeightModifiers.Length; i++)
			{
				this.CardsOnBoardWeightModifiers[i].GetExtraWeight(_Reports);
			}
		}
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00054A30 File Offset: 0x00052C30
	public void AddClashValueFromEnemyValues(InGameEncounter _CurrentEncounter, ref ClashResultsReport _Report, bool _WithRandomness)
	{
		_Report.EnemyClashAddedFromMeleeSkill = this.EnemySkillClashModifier.EnemyMeleeSkillMod.GenerateValue(_CurrentEncounter.CurrentEnemyMeleeSkill, _WithRandomness);
		_Report.EnemyClashAddedFromRangedSkill = this.EnemySkillClashModifier.EnemyRangedSkillMod.GenerateValue(_CurrentEncounter.CurrentEnemyRangedSkill, _WithRandomness);
		_Report.EnemyClashAddedFromBlood = this.EnemySkillClashModifier.EnemyBloodMod.GenerateValue(_CurrentEncounter.CurrentEnemyBlood, _WithRandomness);
		_Report.EnemyClashAddedFromStamina = this.EnemySkillClashModifier.EnemyStaminaMod.GenerateValue(_CurrentEncounter.CurrentEnemyStamina, _WithRandomness);
		_Report.EnemyClashAddedFromMorale = this.EnemySkillClashModifier.EnemyMoraleMod.GenerateValue(_CurrentEncounter.CurrentEnemyMorale, _WithRandomness);
		_Report.EnemyClashAddedFromValue1 = this.EnemySkillClashModifier.EnemyValue1Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue1, _WithRandomness);
		_Report.EnemyClashAddedFromValue2 = this.EnemySkillClashModifier.EnemyValue2Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue2, _WithRandomness);
		_Report.EnemyClashAddedFromValue3 = this.EnemySkillClashModifier.EnemyValue3Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue3, _WithRandomness);
		_Report.EnemyClashAddedFromValue4 = this.EnemySkillClashModifier.EnemyValue4Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue4, _WithRandomness);
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00054B44 File Offset: 0x00052D44
	public float AddedDamageFromEnemyValues(InGameEncounter _CurrentEncounter, bool _WithRandomness)
	{
		return 0f + this.EnemySkillDamageModifier.EnemyMeleeSkillMod.GenerateValue(_CurrentEncounter.CurrentEnemyMeleeSkill, _WithRandomness) + this.EnemySkillDamageModifier.EnemyRangedSkillMod.GenerateValue(_CurrentEncounter.CurrentEnemyRangedSkill, _WithRandomness) + this.EnemySkillDamageModifier.EnemyBloodMod.GenerateValue(_CurrentEncounter.CurrentEnemyBlood, _WithRandomness) + this.EnemySkillDamageModifier.EnemyStaminaMod.GenerateValue(_CurrentEncounter.CurrentEnemyStamina, _WithRandomness) + this.EnemySkillDamageModifier.EnemyMoraleMod.GenerateValue(_CurrentEncounter.CurrentEnemyMorale, _WithRandomness) + this.EnemySkillDamageModifier.EnemyValue1Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue1, _WithRandomness) + this.EnemySkillDamageModifier.EnemyValue2Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue2, _WithRandomness) + this.EnemySkillDamageModifier.EnemyValue3Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue3, _WithRandomness) + this.EnemySkillDamageModifier.EnemyValue4Mod.GenerateValue(_CurrentEncounter.CurrentEnemyValue4, _WithRandomness);
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00054C30 File Offset: 0x00052E30
	public float AddedClashValueFromWounds(InGameEncounter _CurrentEncounter, bool _WithRandomness)
	{
		return 0f + this.EnemyWoundClashModifier.AllWoundModifier.GenerateValue(_CurrentEncounter.AllWoundsAccumulated, _WithRandomness) + this.EnemyWoundClashModifier.TorsoWoundModifier.GenerateValue(_CurrentEncounter.TorsoWoundsAccumulated, _WithRandomness) + this.EnemyWoundClashModifier.HeadWoundModifier.GenerateValue(_CurrentEncounter.HeadWoundsAccumulated, _WithRandomness) + this.EnemyWoundClashModifier.LArmWoundModifier.GenerateValue(_CurrentEncounter.LeftArmWoundsAccumulated, _WithRandomness) + this.EnemyWoundClashModifier.RArmWoundModifier.GenerateValue(_CurrentEncounter.RightArmWoundsAccumulated, _WithRandomness) + this.EnemyWoundClashModifier.LLegWoundModifier.GenerateValue(_CurrentEncounter.LeftLegWoundsAccumulated, _WithRandomness) + this.EnemyWoundClashModifier.RLegWoundModifier.GenerateValue(_CurrentEncounter.RightLegWoundsAccumulated, _WithRandomness);
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00054CEC File Offset: 0x00052EEC
	public float AddedDamageValueFromWounds(InGameEncounter _CurrentEncounter, bool _WithRandomness)
	{
		return 0f + this.EnemyWoundDamageModifier.AllWoundModifier.GenerateValue(_CurrentEncounter.AllWoundsAccumulated, _WithRandomness) + this.EnemyWoundDamageModifier.TorsoWoundModifier.GenerateValue(_CurrentEncounter.TorsoWoundsAccumulated, _WithRandomness) + this.EnemyWoundDamageModifier.HeadWoundModifier.GenerateValue(_CurrentEncounter.HeadWoundsAccumulated, _WithRandomness) + this.EnemyWoundDamageModifier.LArmWoundModifier.GenerateValue(_CurrentEncounter.LeftArmWoundsAccumulated, _WithRandomness) + this.EnemyWoundDamageModifier.RArmWoundModifier.GenerateValue(_CurrentEncounter.RightArmWoundsAccumulated, _WithRandomness) + this.EnemyWoundDamageModifier.LLegWoundModifier.GenerateValue(_CurrentEncounter.LeftLegWoundsAccumulated, _WithRandomness) + this.EnemyWoundDamageModifier.RLegWoundModifier.GenerateValue(_CurrentEncounter.RightLegWoundsAccumulated, _WithRandomness);
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00054DA8 File Offset: 0x00052FA8
	public void AddClashFromStats(ref ClashResultsReport _Report, bool _WithRandomness)
	{
		if (this.EnemyStatsClashModifiers == null)
		{
			_Report.EnemyClashStatsAddedValues = null;
			return;
		}
		_Report.EnemyClashStatsAddedValues = new List<StatAndFloatValue>();
		for (int i = 0; i < this.EnemyStatsClashModifiers.Length; i++)
		{
			_Report.EnemyClashStatsAddedValues.Add(new StatAndFloatValue(this.EnemyStatsClashModifiers[i].Stat, this.EnemyStatsClashModifiers[i].GenerateValue(_WithRandomness)));
		}
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x00054E18 File Offset: 0x00053018
	public float AddedDamageFromStats(bool _WithRandomness)
	{
		if (this.EnemyStatsDamageModifier == null)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < this.EnemyStatsDamageModifier.Length; i++)
		{
			num += this.EnemyStatsDamageModifier[i].GenerateValue(_WithRandomness);
		}
		return num;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00054E64 File Offset: 0x00053064
	public float AddedDamageVsEscape(bool _WithRandomness)
	{
		if (Mathf.Approximately(this.DmgVsEscapeModifier.x, this.DmgVsEscapeModifier.y))
		{
			return this.DmgVsEscapeModifier.x;
		}
		if (_WithRandomness)
		{
			return UnityEngine.Random.Range(this.DmgVsEscapeModifier.x, this.DmgVsEscapeModifier.y);
		}
		return Mathf.Lerp(this.DmgVsEscapeModifier.x, this.DmgVsEscapeModifier.y, 0.5f);
	}

	// Token: 0x04000D7C RID: 3452
	public EncounterLogMessage ActionLog;

	// Token: 0x04000D7D RID: 3453
	public EncounterLogMessage SuccessLog;

	// Token: 0x04000D7E RID: 3454
	public EncounterLogMessage FailureLog;

	// Token: 0x04000D7F RID: 3455
	[SpecialHeader("Selection Conditions", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public EncounterDistanceCondition RequiredDistance;

	// Token: 0x04000D80 RID: 3456
	[SpecialHeader("Selection Weights", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public int BaseWeight;

	// Token: 0x04000D81 RID: 3457
	public int DistanceWeightModifier;

	// Token: 0x04000D82 RID: 3458
	public int CloseRangeWeightModifier;

	// Token: 0x04000D83 RID: 3459
	public int EnemyHiddenWeightModifier;

	// Token: 0x04000D84 RID: 3460
	public int PlayerHiddenWeightModifier;

	// Token: 0x04000D85 RID: 3461
	public CardBasedDropChanceModifier[] CardsOnBoardWeightModifiers;

	// Token: 0x04000D86 RID: 3462
	public StatBasedDropChanceModifier[] StatsWeightModifiers;

	// Token: 0x04000D87 RID: 3463
	public EnemyValueBasedWeightModifier ValuesWeightModifiers;

	// Token: 0x04000D88 RID: 3464
	public EnemyWoundBasedWeightModifier WoundsWeightModifiers;

	// Token: 0x04000D89 RID: 3465
	[SpecialHeader("Attack properties", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public ActionRange ActionRange;

	// Token: 0x04000D8A RID: 3466
	public float Reach;

	// Token: 0x04000D8B RID: 3467
	[FormerlySerializedAs("ActionDistanceChange")]
	public EncounterDistanceChange PreClashDistanceChange;

	// Token: 0x04000D8C RID: 3468
	public bool CannotFailClash;

	// Token: 0x04000D8D RID: 3469
	public bool DoesNotAttack;

	// Token: 0x04000D8E RID: 3470
	public bool IsEscapeAction;

	// Token: 0x04000D8F RID: 3471
	[Space]
	public float BaseClashValue;

	// Token: 0x04000D90 RID: 3472
	public Vector2 ClashRangedInaccuracy;

	// Token: 0x04000D91 RID: 3473
	public Vector2 ClashStealthBonus;

	// Token: 0x04000D92 RID: 3474
	public Vector2 IneffectiveRangeMalus;

	// Token: 0x04000D93 RID: 3475
	public Vector2 ClashVsEscapeModifier;

	// Token: 0x04000D94 RID: 3476
	public EnemySkillModifier EnemySkillClashModifier;

	// Token: 0x04000D95 RID: 3477
	public EnemyWoundModifier EnemyWoundClashModifier;

	// Token: 0x04000D96 RID: 3478
	public PlayerEncounterVariable[] EnemyStatsClashModifiers;

	// Token: 0x04000D97 RID: 3479
	[Space]
	public Vector2 Damage;

	// Token: 0x04000D98 RID: 3480
	public DamageType[] DamageTypes;

	// Token: 0x04000D99 RID: 3481
	public EnemySkillModifier EnemySkillDamageModifier;

	// Token: 0x04000D9A RID: 3482
	public EnemyWoundModifier EnemyWoundDamageModifier;

	// Token: 0x04000D9B RID: 3483
	public PlayerEncounterVariable[] EnemyStatsDamageModifier;

	// Token: 0x04000D9C RID: 3484
	public Vector2 DmgVsEscapeModifier;

	// Token: 0x04000D9D RID: 3485
	public SimpleHitProbabilityModifier AddedPlayerLocationHitProbabilities;

	// Token: 0x04000D9E RID: 3486
	public OptionalFloatValue ArmorDurabilityDamage;

	// Token: 0x04000D9F RID: 3487
	[SpecialHeader("Attack effects", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public EnemyValuesModifiers EnemyValueChanges;

	// Token: 0x04000DA0 RID: 3488
	public PlayerWounds PlayerWounds;

	// Token: 0x04000DA1 RID: 3489
	public EncounterResult EncounterResult;

	// Token: 0x04000DA2 RID: 3490
	public EnemyValueToStatModifier[] TransferValuesToStats;
}
