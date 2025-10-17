using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class InGameEncounter : MonoBehaviour
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000224 RID: 548 RVA: 0x00015663 File Offset: 0x00013863
	// (set) Token: 0x06000225 RID: 549 RVA: 0x0001566B File Offset: 0x0001386B
	public bool Distant { get; private set; }

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000226 RID: 550 RVA: 0x00015674 File Offset: 0x00013874
	// (set) Token: 0x06000227 RID: 551 RVA: 0x0001567C File Offset: 0x0001387C
	public BodyLocationTracking CurrentEnemyBodyProbabilities { get; private set; }

	// Token: 0x06000228 RID: 552 RVA: 0x00015688 File Offset: 0x00013888
	private void Awake()
	{
		this.GM = MBSingleton<GameManager>.Instance;
		this.Popup = MBSingleton<EncounterPopup>.Instance;
		this.CurrentEnemyBodyProbabilities = default(BodyLocationTracking);
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000229 RID: 553 RVA: 0x000156BC File Offset: 0x000138BC
	public string EnemyName
	{
		get
		{
			if (!this.EncounterModel)
			{
				return "";
			}
			if (!string.IsNullOrEmpty(this.EncounterModel.EnemyName))
			{
				return this.EncounterModel.EnemyName;
			}
			return LocalizedString.DefaultEnemyName;
		}
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00015710 File Offset: 0x00013910
	public float GetEnemyValue(EnemyValueNames _Value)
	{
		switch (_Value)
		{
		case EnemyValueNames.MeleeSkill:
			return this.CurrentEnemyMeleeSkill;
		case EnemyValueNames.RangedSkill:
			return this.CurrentEnemyRangedSkill;
		case EnemyValueNames.Blood:
			return this.CurrentEnemyBlood;
		case EnemyValueNames.Stamina:
			return this.CurrentEnemyStamina;
		case EnemyValueNames.Morale:
			return this.CurrentEnemyMorale;
		case EnemyValueNames.Value1:
			return this.CurrentEnemyValue1;
		case EnemyValueNames.Value2:
			return this.CurrentEnemyValue2;
		case EnemyValueNames.Value3:
			return this.CurrentEnemyValue2;
		case EnemyValueNames.Value4:
			return this.CurrentEnemyValue4;
		default:
			return 0f;
		}
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00015790 File Offset: 0x00013990
	public void Init(Encounter _Model, EncounterPopup _Popup)
	{
		this.Popup = _Popup;
		this.EncounterModel = _Model;
		this.CurrentEnemyStealth = UnityEngine.Random.Range(_Model.EnemyStealth.x, _Model.EnemyStealth.y);
		this.CurrentEnemyAlertness = UnityEngine.Random.Range(_Model.EnemyAwareness.x, _Model.EnemyAwareness.y);
		this.CurrentEnemySize = UnityEngine.Random.Range(_Model.EnemySize.x, _Model.EnemySize.y);
		this.CurrentEnemyMeleeSkill = UnityEngine.Random.Range(_Model.MeleeSkill.StartingValue.x, _Model.MeleeSkill.StartingValue.y);
		this.CurrentEnemyRangedSkill = UnityEngine.Random.Range(_Model.RangedSkill.StartingValue.x, _Model.RangedSkill.StartingValue.y);
		this.CurrentEnemyBlood = UnityEngine.Random.Range(_Model.Blood.StartingValue.x, _Model.Blood.StartingValue.y);
		this.CurrentEnemyStamina = UnityEngine.Random.Range(_Model.Stamina.StartingValue.x, _Model.Stamina.StartingValue.y);
		this.CurrentEnemyMorale = UnityEngine.Random.Range(_Model.Morale.StartingValue.x, _Model.Morale.StartingValue.y);
		this.CurrentEnemyValue1 = UnityEngine.Random.Range(_Model.Value1.StartingValue.x, _Model.Value1.StartingValue.y);
		this.CurrentEnemyValue2 = UnityEngine.Random.Range(_Model.Value2.StartingValue.x, _Model.Value2.StartingValue.y);
		this.CurrentEnemyValue3 = UnityEngine.Random.Range(_Model.Value3.StartingValue.x, _Model.Value3.StartingValue.y);
		this.CurrentEnemyValue4 = UnityEngine.Random.Range(_Model.Value4.StartingValue.x, _Model.Value4.StartingValue.y);
		if (_Model.MeleeSkill.MaxValue > 0f)
		{
			this.CurrentEnemyMeleeSkill = Mathf.Min(this.CurrentEnemyMeleeSkill, _Model.MeleeSkill.MaxValue);
		}
		if (_Model.RangedSkill.MaxValue > 0f)
		{
			this.CurrentEnemyRangedSkill = Mathf.Min(this.CurrentEnemyRangedSkill, _Model.RangedSkill.MaxValue);
		}
		if (_Model.Blood.MaxValue > 0f)
		{
			this.CurrentEnemyBlood = Mathf.Min(this.CurrentEnemyBlood, _Model.Blood.MaxValue);
		}
		if (_Model.Stamina.MaxValue > 0f)
		{
			this.CurrentEnemyStamina = Mathf.Min(this.CurrentEnemyStamina, _Model.Stamina.MaxValue);
		}
		if (_Model.Morale.MaxValue > 0f)
		{
			this.CurrentEnemyMorale = Mathf.Min(this.CurrentEnemyMorale, _Model.Morale.MaxValue);
		}
		if (_Model.Value1.MaxValue > 0f)
		{
			this.CurrentEnemyValue1 = Mathf.Min(this.CurrentEnemyValue1, _Model.Value1.MaxValue);
		}
		if (_Model.Value2.MaxValue > 0f)
		{
			this.CurrentEnemyValue2 = Mathf.Min(this.CurrentEnemyValue2, _Model.Value2.MaxValue);
		}
		if (_Model.Value3.MaxValue > 0f)
		{
			this.CurrentEnemyValue3 = Mathf.Min(this.CurrentEnemyValue3, _Model.Value3.MaxValue);
		}
		if (_Model.Value4.MaxValue > 0f)
		{
			this.CurrentEnemyValue4 = Mathf.Min(this.CurrentEnemyValue4, _Model.Value4.MaxValue);
		}
		this.CurrentEnemyBodyProbabilities = default(BodyLocationTracking);
		this.AllWoundsAccumulated = 0f;
		this.HeadWoundsAccumulated = 0f;
		this.TorsoWoundsAccumulated = 0f;
		this.LeftArmWoundsAccumulated = 0f;
		this.RightArmWoundsAccumulated = 0f;
		this.LeftLegWoundsAccumulated = 0f;
		this.RightLegWoundsAccumulated = 0f;
		this.MeleeSkillEncounterResult = EncounterResult.Ongoing;
		this.RangedSkillEncounterResult = EncounterResult.Ongoing;
		this.BloodEncounterResult = EncounterResult.Ongoing;
		this.StaminaEncounterResult = EncounterResult.Ongoing;
		this.MoraleEncounterResult = EncounterResult.Ongoing;
		this.Value1EncounterResult = EncounterResult.Ongoing;
		this.Value2EncounterResult = EncounterResult.Ongoing;
		this.Value3EncounterResult = EncounterResult.Ongoing;
		this.Value4EncounterResult = EncounterResult.Ongoing;
		this.EncounterResult = EncounterResult.Ongoing;
		this.Distant = true;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00015BD8 File Offset: 0x00013DD8
	public EncounterSaveData Save()
	{
		EncounterSaveData encounterSaveData = new EncounterSaveData();
		if (this.CurrentEnemyAction == null)
		{
			encounterSaveData.SelectedEnemyAction = "";
		}
		else
		{
			encounterSaveData.SelectedEnemyAction = this.CurrentEnemyAction.ActionLog.MainLogKey;
		}
		encounterSaveData.EncounterID = UniqueIDScriptable.SaveID(this.EncounterModel);
		encounterSaveData.CurrentRound = this.CurrentRound;
		encounterSaveData.Distant = this.Distant;
		encounterSaveData.PlayerHidden = this.PlayerHidden;
		encounterSaveData.EnemyHidden = this.EnemyHidden;
		encounterSaveData.CurrentEnemyMeleeSkill = this.CurrentEnemyMeleeSkill;
		encounterSaveData.CurrentEnemyRangedSkill = this.CurrentEnemyRangedSkill;
		encounterSaveData.CurrentEnemyBlood = this.CurrentEnemyBlood;
		encounterSaveData.CurrentEnemyStamina = this.CurrentEnemyStamina;
		encounterSaveData.CurrentEnemyMorale = this.CurrentEnemyMorale;
		encounterSaveData.CurrentEnemyValue1 = this.CurrentEnemyValue1;
		encounterSaveData.CurrentEnemyValue2 = this.CurrentEnemyValue2;
		encounterSaveData.CurrentEnemyValue3 = this.CurrentEnemyValue3;
		encounterSaveData.CurrentEnemyValue4 = this.CurrentEnemyValue4;
		encounterSaveData.AllWoundsAccumulated = this.AllWoundsAccumulated;
		encounterSaveData.HeadWoundsAccumulated = this.HeadWoundsAccumulated;
		encounterSaveData.TorsoWoundsAccumulated = this.TorsoWoundsAccumulated;
		encounterSaveData.LeftArmWoundsAccumulated = this.LeftArmWoundsAccumulated;
		encounterSaveData.RightArmWoundsAccumulated = this.RightArmWoundsAccumulated;
		encounterSaveData.LeftLegWoundsAccumulated = this.LeftLegWoundsAccumulated;
		encounterSaveData.RightLegWoundsAccumulated = this.RightLegWoundsAccumulated;
		encounterSaveData.CurrentEnemyBodyProbabilities = this.CurrentEnemyBodyProbabilities;
		encounterSaveData.Result = this.EncounterResult;
		return encounterSaveData;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x00015D30 File Offset: 0x00013F30
	public void Load(EncounterSaveData _From)
	{
		this.CurrentRound = _From.CurrentRound;
		this.Distant = _From.Distant;
		this.PlayerHidden = _From.PlayerHidden;
		this.EnemyHidden = _From.EnemyHidden;
		this.CurrentEnemyMeleeSkill = _From.CurrentEnemyMeleeSkill;
		this.CurrentEnemyRangedSkill = _From.CurrentEnemyRangedSkill;
		this.CurrentEnemyBlood = _From.CurrentEnemyBlood;
		this.CurrentEnemyStamina = _From.CurrentEnemyStamina;
		this.CurrentEnemyMorale = _From.CurrentEnemyMorale;
		this.CurrentEnemyValue1 = _From.CurrentEnemyValue1;
		this.CurrentEnemyValue2 = _From.CurrentEnemyValue2;
		this.CurrentEnemyValue3 = _From.CurrentEnemyValue3;
		this.CurrentEnemyValue4 = _From.CurrentEnemyValue4;
		this.AllWoundsAccumulated = _From.AllWoundsAccumulated;
		this.HeadWoundsAccumulated = _From.HeadWoundsAccumulated;
		this.TorsoWoundsAccumulated = _From.TorsoWoundsAccumulated;
		this.LeftArmWoundsAccumulated = _From.LeftArmWoundsAccumulated;
		this.RightArmWoundsAccumulated = _From.RightArmWoundsAccumulated;
		this.LeftLegWoundsAccumulated = _From.LeftLegWoundsAccumulated;
		this.RightLegWoundsAccumulated = _From.RightLegWoundsAccumulated;
		this.CurrentEnemyBodyProbabilities = _From.CurrentEnemyBodyProbabilities;
		this.EncounterResult = _From.Result;
		this.CurrentEnemyAction = null;
		if (!string.IsNullOrEmpty(_From.SelectedEnemyAction) && this.EncounterModel)
		{
			if (_From.SelectedEnemyAction == "IGNOREKEY")
			{
				this.CurrentEnemyAction = this.PlayerHiddenFreeAction();
				return;
			}
			if (_From.SelectedEnemyAction == this.EncounterModel.EnemyGettingCloseLog.MainLogKey || _From.SelectedEnemyAction == "ENCOUNTER_ENEMY_GETTING_CLOSE")
			{
				this.CurrentEnemyAction = this.EnemyGetCloseAction();
				return;
			}
			for (int i = 0; i < this.EncounterModel.EnemyActions.Length; i++)
			{
				if (_From.SelectedEnemyAction == this.EncounterModel.EnemyActions[i].ActionLog.MainLogKey)
				{
					this.CurrentEnemyAction = this.EncounterModel.EnemyActions[i];
					return;
				}
			}
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x00015F17 File Offset: 0x00014117
	public void SetDistant(bool _Value)
	{
		this.Distant = _Value;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x00015F20 File Offset: 0x00014120
	public EnemyAction SelectAction()
	{
		List<EnemyAction> list = new List<EnemyAction>();
		if (this.PlayerHidden)
		{
			list.Add(this.PlayerHiddenFreeAction());
		}
		else
		{
			list.AddRange(this.EncounterModel.EnemyActions);
			if (!this.EnemyHidden)
			{
				if (this.Distant)
				{
					for (int i = list.Count - 1; i >= 0; i--)
					{
						if (list[i].RequiredDistance == EncounterDistanceCondition.NeedsCloseRange)
						{
							list.RemoveAt(i);
						}
					}
					list.Insert(0, this.EnemyGetCloseAction());
				}
				else
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (list[j].RequiredDistance == EncounterDistanceCondition.NeedsDistance)
						{
							list.RemoveAt(j);
						}
					}
				}
			}
		}
		EnemyActionSelectionReport enemyActionSelectionReport = this.GetEnemyActionSelectionReport(list);
		if (enemyActionSelectionReport.Actions == null)
		{
			Debug.Log("no actions");
			Action<EnemyActionSelectionReport> onEnemyActionSelected = InGameEncounter.OnEnemyActionSelected;
			if (onEnemyActionSelected != null)
			{
				onEnemyActionSelected(enemyActionSelectionReport);
			}
			return null;
		}
		if (enemyActionSelectionReport.Actions.Length == 0)
		{
			Debug.Log("no actions length");
			Action<EnemyActionSelectionReport> onEnemyActionSelected2 = InGameEncounter.OnEnemyActionSelected;
			if (onEnemyActionSelected2 != null)
			{
				onEnemyActionSelected2(enemyActionSelectionReport);
			}
			return null;
		}
		if (enemyActionSelectionReport.TotalWeight == 0)
		{
			enemyActionSelectionReport.SelectedAction = 0;
			enemyActionSelectionReport.RandomValue = 0f;
			Action<EnemyActionSelectionReport> onEnemyActionSelected3 = InGameEncounter.OnEnemyActionSelected;
			if (onEnemyActionSelected3 != null)
			{
				onEnemyActionSelected3(enemyActionSelectionReport);
			}
			return list[enemyActionSelectionReport.SelectedAction];
		}
		float num = UnityEngine.Random.Range(0f, (float)enemyActionSelectionReport.TotalWeight - 0.001f);
		enemyActionSelectionReport.RandomValue = num;
		for (int k = 0; k < enemyActionSelectionReport.Actions.Length; k++)
		{
			if (num < (float)enemyActionSelectionReport.Actions[k].RangeUpTo)
			{
				enemyActionSelectionReport.SelectedAction = k;
				Action<EnemyActionSelectionReport> onEnemyActionSelected4 = InGameEncounter.OnEnemyActionSelected;
				if (onEnemyActionSelected4 != null)
				{
					onEnemyActionSelected4(enemyActionSelectionReport);
				}
				return list[k];
			}
		}
		enemyActionSelectionReport.SelectedAction = -1;
		Action<EnemyActionSelectionReport> onEnemyActionSelected5 = InGameEncounter.OnEnemyActionSelected;
		if (onEnemyActionSelected5 != null)
		{
			onEnemyActionSelected5(enemyActionSelectionReport);
		}
		Debug.Log("failed the rest");
		return null;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x000160F4 File Offset: 0x000142F4
	public bool AnyDistanceChange(EncounterDistanceChange _Change)
	{
		if (this.CurrentEnemyAction != null)
		{
			if (this.CurrentEnemyAction.PreClashDistanceChange == _Change)
			{
				return true;
			}
			if (this.CurrentEnemyAction.PreClashDistanceChange != EncounterDistanceChange.DontChangeDistance)
			{
				return false;
			}
		}
		if (this.CurrentPlayerAction == null)
		{
			return false;
		}
		if (this.CurrentPlayerAction.DistanceChange == _Change)
		{
			return true;
		}
		EncounterDistanceChange distanceChange = this.CurrentPlayerAction.DistanceChange;
		return false;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0001614F File Offset: 0x0001434F
	public bool AnyResultFromWounds(EncounterResult _Result)
	{
		return (this.CurrentRoundEnemyWound != null && this.CurrentRoundEnemyWound.EncounterResult == _Result) || (this.CurrentRoundPlayerWound != null && this.CurrentRoundPlayerWound.EncounterResult == _Result);
	}

	// Token: 0x06000232 RID: 562 RVA: 0x00016184 File Offset: 0x00014384
	public bool AnyResultFromEnemyState(EncounterResult _Result)
	{
		return this.CurrentPlayerAction.EncounterResult == _Result || this.CurrentEnemyAction.EncounterResult == _Result || this.MeleeSkillEncounterResult == _Result || this.RangedSkillEncounterResult == _Result || this.BloodEncounterResult == _Result || this.StaminaEncounterResult == _Result || this.MoraleEncounterResult == _Result || this.Value1EncounterResult == _Result || this.Value2EncounterResult == _Result || this.Value3EncounterResult == _Result || this.Value4EncounterResult == _Result;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x00016200 File Offset: 0x00014400
	public void ModifyEnemyValues(EnemyValuesModifiers _Modifiers, EnemyActionEffectCondition _HitCondition)
	{
		if (_Modifiers.Applies(EnemyValueNames.MeleeSkill, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.MeleeSkill, UnityEngine.Random.Range(_Modifiers.MeleeSkillModifier.x, _Modifiers.MeleeSkillModifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.RangedSkill, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.RangedSkill, UnityEngine.Random.Range(_Modifiers.RangedSkillModifier.x, _Modifiers.RangedSkillModifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.Blood, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.Blood, UnityEngine.Random.Range(_Modifiers.BloodModifier.x, _Modifiers.BloodModifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.Stamina, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.Stamina, UnityEngine.Random.Range(_Modifiers.StaminaModifier.x, _Modifiers.StaminaModifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.Morale, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.Morale, UnityEngine.Random.Range(_Modifiers.MoraleModifier.x, _Modifiers.MoraleModifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.Value1, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.Value1, UnityEngine.Random.Range(_Modifiers.Value1Modifier.x, _Modifiers.Value1Modifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.Value2, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.Value2, UnityEngine.Random.Range(_Modifiers.Value2Modifier.x, _Modifiers.Value2Modifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.Value3, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.Value3, UnityEngine.Random.Range(_Modifiers.Value3Modifier.x, _Modifiers.Value3Modifier.y));
		}
		if (_Modifiers.Applies(EnemyValueNames.Value4, _HitCondition))
		{
			this.ModifyEnemyValue(EnemyValueNames.Value4, UnityEngine.Random.Range(_Modifiers.Value4Modifier.x, _Modifiers.Value4Modifier.y));
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x000163A4 File Offset: 0x000145A4
	private void ModifyEnemyValue(EnemyValueNames _ValueToModify, float _ByAmt)
	{
		if (Mathf.Approximately(_ByAmt, 0f))
		{
			return;
		}
		ref float ptr = ref this.CurrentEnemyMeleeSkill;
		ref EncounterResult ptr2 = ref this.MeleeSkillEncounterResult;
		float num = this.CurrentEnemyMeleeSkill;
		EnemyValue enemyValue = this.EncounterModel.MeleeSkill;
		switch (_ValueToModify)
		{
		case EnemyValueNames.RangedSkill:
			ptr = ref this.CurrentEnemyRangedSkill;
			num = this.CurrentEnemyRangedSkill;
			enemyValue = this.EncounterModel.RangedSkill;
			ptr2 = ref this.RangedSkillEncounterResult;
			break;
		case EnemyValueNames.Blood:
			ptr = ref this.CurrentEnemyBlood;
			num = this.CurrentEnemyBlood;
			enemyValue = this.EncounterModel.Blood;
			ptr2 = ref this.BloodEncounterResult;
			break;
		case EnemyValueNames.Stamina:
			ptr = ref this.CurrentEnemyStamina;
			num = this.CurrentEnemyStamina;
			enemyValue = this.EncounterModel.Stamina;
			ptr2 = ref this.StaminaEncounterResult;
			break;
		case EnemyValueNames.Morale:
			ptr = ref this.CurrentEnemyMorale;
			num = this.CurrentEnemyMorale;
			enemyValue = this.EncounterModel.Morale;
			ptr2 = ref this.MoraleEncounterResult;
			break;
		case EnemyValueNames.Value1:
			ptr = ref this.CurrentEnemyValue1;
			num = this.CurrentEnemyValue1;
			enemyValue = this.EncounterModel.Value1;
			ptr2 = ref this.Value1EncounterResult;
			break;
		case EnemyValueNames.Value2:
			ptr = ref this.CurrentEnemyValue2;
			num = this.CurrentEnemyValue2;
			enemyValue = this.EncounterModel.Value2;
			ptr2 = ref this.Value2EncounterResult;
			break;
		case EnemyValueNames.Value3:
			ptr = ref this.CurrentEnemyValue3;
			num = this.CurrentEnemyValue3;
			enemyValue = this.EncounterModel.Value3;
			ptr2 = ref this.Value3EncounterResult;
			break;
		case EnemyValueNames.Value4:
			ptr = ref this.CurrentEnemyValue4;
			num = this.CurrentEnemyValue4;
			enemyValue = this.EncounterModel.Value4;
			ptr2 = ref this.Value4EncounterResult;
			break;
		}
		ptr += _ByAmt;
		if (enemyValue.MaxValue > 0f)
		{
			ptr = Mathf.Min(enemyValue.MaxValue, ptr);
		}
		ptr = Mathf.Max(ptr, 0f);
		if (num > 0f && ExtraMath.FloatIsLowerOrEqual(ptr, 0f))
		{
			ptr2 = enemyValue.OnZeroEncounterResult;
		}
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00016580 File Offset: 0x00014780
	private EnemyActionSelectionReport GetEnemyActionSelectionReport(List<EnemyAction> _PossibleActions)
	{
		EnemyActionSelectionReport result = default(EnemyActionSelectionReport);
		result.FillActionSelectionInfo(this, _PossibleActions);
		return result;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x000165A0 File Offset: 0x000147A0
	private EnemyAction EnemyGetCloseAction()
	{
		return new EnemyAction
		{
			ActionLog = ((!string.IsNullOrEmpty(this.EncounterModel.EnemyGettingCloseLog)) ? this.EncounterModel.EnemyGettingCloseLog : new EncounterLogMessage(LocalizedString.DefaultEnemyGettingCloseLog(this.EnemyName))),
			RequiredDistance = EncounterDistanceCondition.NeedsDistance,
			DoesNotAttack = true,
			BaseClashValue = -10000f,
			SuccessLog = this.Popup.EnemyGetCloseActionResultLog,
			FailureLog = this.Popup.EnemyGetCloseActionResultLog
		};
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0001662C File Offset: 0x0001482C
	private EnemyAction PlayerHiddenFreeAction()
	{
		return new EnemyAction
		{
			ActionLog = new EncounterLogMessage("Enemy unaware"),
			ActionRange = ActionRange.Ranged,
			DoesNotAttack = true
		};
	}

	// Token: 0x06000238 RID: 568 RVA: 0x00016654 File Offset: 0x00014854
	public EncounterPlayerAction EnemyHiddenFreeAction(float _PlayerAlertness)
	{
		return new EncounterPlayerAction(null, this.EnemyName)
		{
			ActionName = "Player ambushed",
			ActionRange = ActionRange.Ranged,
			Clash = Vector2.one * _PlayerAlertness,
			DamageTypes = new DamageType[0],
			DoesNotAttack = true,
			DontShowSuccessChance = true
		};
	}

	// Token: 0x04000216 RID: 534
	public Encounter EncounterModel;

	// Token: 0x04000217 RID: 535
	public int CurrentRound;

	// Token: 0x04000218 RID: 536
	public EnemyAction CurrentEnemyAction;

	// Token: 0x04000219 RID: 537
	public EncounterPlayerAction CurrentPlayerAction;

	// Token: 0x0400021A RID: 538
	public float CurrentPlayerCover;

	// Token: 0x0400021B RID: 539
	public float CurrentEnemyCover;

	// Token: 0x0400021C RID: 540
	public float CurrentEnemyStealth;

	// Token: 0x0400021D RID: 541
	public float CurrentEnemyAlertness;

	// Token: 0x0400021E RID: 542
	public float CurrentEnemySize;

	// Token: 0x0400021F RID: 543
	public float CurrentEnemyMeleeSkill;

	// Token: 0x04000220 RID: 544
	public float CurrentEnemyRangedSkill;

	// Token: 0x04000221 RID: 545
	public float CurrentEnemyBlood;

	// Token: 0x04000222 RID: 546
	public float CurrentEnemyStamina;

	// Token: 0x04000223 RID: 547
	public float CurrentEnemyMorale;

	// Token: 0x04000224 RID: 548
	public float CurrentEnemyValue1;

	// Token: 0x04000225 RID: 549
	public float CurrentEnemyValue2;

	// Token: 0x04000226 RID: 550
	public float CurrentEnemyValue3;

	// Token: 0x04000227 RID: 551
	public float CurrentEnemyValue4;

	// Token: 0x04000228 RID: 552
	public EncounterResult MeleeSkillEncounterResult;

	// Token: 0x04000229 RID: 553
	public EncounterResult RangedSkillEncounterResult;

	// Token: 0x0400022A RID: 554
	public EncounterResult BloodEncounterResult;

	// Token: 0x0400022B RID: 555
	public EncounterResult StaminaEncounterResult;

	// Token: 0x0400022C RID: 556
	public EncounterResult MoraleEncounterResult;

	// Token: 0x0400022D RID: 557
	public EncounterResult Value1EncounterResult;

	// Token: 0x0400022E RID: 558
	public EncounterResult Value2EncounterResult;

	// Token: 0x0400022F RID: 559
	public EncounterResult Value3EncounterResult;

	// Token: 0x04000230 RID: 560
	public EncounterResult Value4EncounterResult;

	// Token: 0x04000231 RID: 561
	public ClashResults CurrentRoundClashResult;

	// Token: 0x04000232 RID: 562
	public BodyLocations CurrentRoundEnemyWoundLocation;

	// Token: 0x04000233 RID: 563
	public WoundSeverity CurrentRoundEnemyWoundSeverity;

	// Token: 0x04000234 RID: 564
	public EnemyWound CurrentRoundEnemyWound;

	// Token: 0x04000235 RID: 565
	public PlayerWound CurrentRoundPlayerWound;

	// Token: 0x04000236 RID: 566
	public EncounterResult EncounterResult;

	// Token: 0x04000237 RID: 567
	public float AllWoundsAccumulated;

	// Token: 0x04000238 RID: 568
	public float HeadWoundsAccumulated;

	// Token: 0x04000239 RID: 569
	public float TorsoWoundsAccumulated;

	// Token: 0x0400023A RID: 570
	public float LeftArmWoundsAccumulated;

	// Token: 0x0400023B RID: 571
	public float RightArmWoundsAccumulated;

	// Token: 0x0400023C RID: 572
	public float LeftLegWoundsAccumulated;

	// Token: 0x0400023D RID: 573
	public float RightLegWoundsAccumulated;

	// Token: 0x0400023F RID: 575
	public bool PlayerHidden;

	// Token: 0x04000240 RID: 576
	public bool EnemyHidden;

	// Token: 0x04000242 RID: 578
	private GameManager GM;

	// Token: 0x04000243 RID: 579
	private EncounterPopup Popup;

	// Token: 0x04000244 RID: 580
	public static Action<EnemyActionSelectionReport> OnEnemyActionSelected;
}
