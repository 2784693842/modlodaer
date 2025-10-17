using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class InGameStat : MonoBehaviour
{
	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06000250 RID: 592 RVA: 0x0001764A File Offset: 0x0001584A
	public float SimpleCurrentValue
	{
		get
		{
			if (this.GM)
			{
				return this.CurrentValue(this.GM.NotInBase);
			}
			return this.CurrentValue(false);
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x06000251 RID: 593 RVA: 0x00017672 File Offset: 0x00015872
	public float SimpleRatePerTick
	{
		get
		{
			if (this.GM)
			{
				return this.CurrentRatePerTick(this.GM.NotInBase);
			}
			return this.CurrentRatePerTick(false);
		}
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0001769C File Offset: 0x0001589C
	public float CurrentValue(bool _NotAtBase)
	{
		float num = this.CurrentBaseValue + this.GlobalModifiedValue;
		if (!_NotAtBase)
		{
			num += this.AtBaseModifiedValue;
		}
		if (this.StatModel && this.StatModel.MinMaxValue != Vector2.zero)
		{
			return Mathf.Clamp(num, this.StatModel.MinMaxValue.x, this.StatModel.MinMaxValue.y);
		}
		return num;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00017710 File Offset: 0x00015910
	public float CurrentRatePerTick(bool _NotAtBase)
	{
		float num = this.CurrentBaseRate + this.GlobalModifiedRate;
		if (!_NotAtBase)
		{
			num += this.AtBaseModifiedRate;
		}
		if (this.StatModel && this.StatModel.MinMaxRate != Vector2.zero)
		{
			return Mathf.Clamp(num, this.StatModel.MinMaxRate.x, this.StatModel.MinMaxRate.y);
		}
		return num;
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x06000254 RID: 596 RVA: 0x00017783 File Offset: 0x00015983
	// (set) Token: 0x06000255 RID: 597 RVA: 0x0001778B File Offset: 0x0001598B
	public List<InGameCardBase> CardValueListeners { get; private set; }

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x06000256 RID: 598 RVA: 0x00017794 File Offset: 0x00015994
	// (set) Token: 0x06000257 RID: 599 RVA: 0x0001779C File Offset: 0x0001599C
	public List<SelfTriggeredAction> ActionsValueListeners { get; private set; }

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000258 RID: 600 RVA: 0x000177A5 File Offset: 0x000159A5
	// (set) Token: 0x06000259 RID: 601 RVA: 0x000177AD File Offset: 0x000159AD
	public float AnimatedValue { get; private set; }

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x0600025A RID: 602 RVA: 0x000177B8 File Offset: 0x000159B8
	public float NormalizedRealValue
	{
		get
		{
			if (!this.StatModel)
			{
				return 0f;
			}
			if (!this.StatModel.InvertedDirection)
			{
				return Mathf.InverseLerp(this.StatModel.MinMaxValue.x, this.StatModel.MinMaxValue.y, this.SimpleCurrentValue);
			}
			return Mathf.InverseLerp(this.StatModel.MinMaxValue.y, this.StatModel.MinMaxValue.x, this.SimpleCurrentValue);
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x0600025B RID: 603 RVA: 0x0001783C File Offset: 0x00015A3C
	public float NormalizedVisibleValue
	{
		get
		{
			if (!this.StatModel)
			{
				return 0f;
			}
			if (!this.StatModel.InvertedDirection)
			{
				return Mathf.InverseLerp(this.StatModel.VisibleValue.x, this.StatModel.VisibleValue.y, this.SimpleCurrentValue);
			}
			return Mathf.InverseLerp(this.StatModel.VisibleValue.y, this.StatModel.VisibleValue.x, this.SimpleCurrentValue);
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x0600025C RID: 604 RVA: 0x000178C0 File Offset: 0x00015AC0
	public float NormalizedAnimatedValue
	{
		get
		{
			if (!this.StatModel)
			{
				return 0f;
			}
			if (!this.StatModel.InvertedDirection)
			{
				return Mathf.InverseLerp(this.StatModel.VisibleValue.x, this.StatModel.VisibleValue.y, this.AnimatedValue);
			}
			return Mathf.InverseLerp(this.StatModel.VisibleValue.y, this.StatModel.VisibleValue.x, this.AnimatedValue);
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600025D RID: 605 RVA: 0x00017944 File Offset: 0x00015B44
	public float NormalizedRateValue
	{
		get
		{
			if (!this.StatModel)
			{
				return 0f;
			}
			if (!this.StatModel.InvertedDirection)
			{
				return Mathf.InverseLerp(this.StatModel.VisibleTrend.x, this.StatModel.VisibleTrend.y, this.SimpleRatePerTick);
			}
			return Mathf.InverseLerp(this.StatModel.VisibleTrend.y, this.StatModel.VisibleTrend.x, this.SimpleRatePerTick);
		}
	}

	// Token: 0x0600025E RID: 606 RVA: 0x000179C8 File Offset: 0x00015BC8
	public StatStatus AnyCurrentStatus(bool _Visible)
	{
		if (this.CurrentStatuses == null)
		{
			return null;
		}
		if (this.CurrentStatuses.Count == 0)
		{
			return null;
		}
		if (!_Visible)
		{
			for (int i = 0; i < this.CurrentStatuses.Count; i++)
			{
				if (!string.IsNullOrEmpty(this.CurrentStatuses[i].GameName) || this.CurrentStatuses[i].Icon)
				{
					return this.CurrentStatuses[i];
				}
			}
		}
		for (int j = 0; j < this.CurrentStatuses.Count; j++)
		{
			if (this.CurrentStatuses[j].StatusGraphics)
			{
				return this.CurrentStatuses[j];
			}
		}
		return null;
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600025F RID: 607 RVA: 0x00017A88 File Offset: 0x00015C88
	public StatModifier ResetBaseValue
	{
		get
		{
			return new StatModifier
			{
				Stat = this.StatModel,
				ValueModifier = Vector2.one * (this.StatModel.BaseValue - this.CurrentBaseValue)
			};
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00017AD0 File Offset: 0x00015CD0
	public void Init(GameStat _Model)
	{
		this.StatModel = _Model;
		this.CurrentBaseValue = _Model.BaseValue;
		this.GlobalModifiedValue = 0f;
		this.AtBaseModifiedValue = 0f;
		this.CurrentBaseRate = _Model.BaseRatePerTick;
		this.GlobalModifiedRate = 0f;
		this.AtBaseModifiedRate = 0f;
		this.GM = MBSingleton<GameManager>.Instance;
		this.GraphManager = MBSingleton<GraphicsManager>.Instance;
		this.AnimatedValue = this.CurrentValue(this.GM.NotInBase);
		this.LastKnownValue = this.CurrentValue(this.GM.NotInBase);
		this.StalenessValues = new List<StalenessData>();
	}

	// Token: 0x06000261 RID: 609 RVA: 0x00017B78 File Offset: 0x00015D78
	public bool HasStatus(StatStatus _Status)
	{
		for (int i = 0; i < this.CurrentStatuses.Count; i++)
		{
			if (this.CurrentStatuses[i].IsSameStatus(_Status))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00017BB4 File Offset: 0x00015DB4
	public void RegisterAction(string _Action, int _ActionTick)
	{
		if (string.IsNullOrEmpty(_Action) || !this.StatModel.UsesNovelty || _ActionTick < 0)
		{
			return;
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.StalenessValues == null)
		{
			this.StalenessValues = new List<StalenessData>();
		}
		int i = 0;
		while (i < this.StalenessValues.Count)
		{
			StalenessData stalenessData = this.StalenessValues[i];
			if (stalenessData.ModifierSource == _Action)
			{
				if (_ActionTick == this.LastActionRegisteredTick && this.ActionIsNotEncounterAction(_Action))
				{
					return;
				}
				this.LastActionRegisteredTick = _ActionTick;
				stalenessData.Quantity++;
				if (this.StatModel.MaxStalenessStack > 0)
				{
					stalenessData.Quantity = Mathf.Min(stalenessData.Quantity, this.StatModel.MaxStalenessStack);
				}
				this.StalenessValues[i] = stalenessData;
				return;
			}
			else
			{
				i++;
			}
		}
		this.StalenessValues.Add(new StalenessData(_Action, this.GM.CurrentTickInfo.z));
		this.LastActionRegisteredTick = _ActionTick;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x00017CC4 File Offset: 0x00015EC4
	private bool ActionIsNotEncounterAction(string _Action)
	{
		return _Action != "Encounter_Enemy_Action" && _Action != "Encounter_Player_Action" && _Action != "Encounter_Player_Wound";
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00017CED File Offset: 0x00015EED
	public void AddModifierSource(StatModifierSource _Source)
	{
		this.ModifierSources.Add(_Source);
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00017CFC File Offset: 0x00015EFC
	public void RemoveModifierSource(StatModifierSource _Source)
	{
		for (int i = 0; i < this.ModifierSources.Count; i++)
		{
			if (this.ModifierSources[i].IsIdentical(_Source, true))
			{
				this.ModifierSources.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00017D44 File Offset: 0x00015F44
	public float GetStalenessModifier(string _ForAction)
	{
		if (string.IsNullOrEmpty(_ForAction) || this.StalenessValues == null)
		{
			return 1f;
		}
		for (int i = 0; i < this.StalenessValues.Count; i++)
		{
			if (this.StalenessValues[i].ModifierSource == _ForAction && this.StalenessValues[i].Quantity > 0)
			{
				return Mathf.Pow(this.StatModel.StalenessMultiplier, (float)this.StalenessValues[i].Quantity);
			}
		}
		return 1f;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00017DD2 File Offset: 0x00015FD2
	public void RegisterListener(InGameCardBase _Card)
	{
		if (this.CardValueListeners == null)
		{
			this.CardValueListeners = new List<InGameCardBase>();
		}
		if (!this.CardValueListeners.Contains(_Card))
		{
			this.CardValueListeners.Add(_Card);
		}
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00017E01 File Offset: 0x00016001
	public void RemoveListener(InGameCardBase _Card)
	{
		if (this.CardValueListeners == null)
		{
			this.CardValueListeners = new List<InGameCardBase>();
		}
		if (this.CardValueListeners.Contains(_Card))
		{
			this.CardValueListeners.Remove(_Card);
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00017E31 File Offset: 0x00016031
	public void RegisterListener(SelfTriggeredAction _Action)
	{
		if (this.ActionsValueListeners == null)
		{
			this.ActionsValueListeners = new List<SelfTriggeredAction>();
		}
		if (!this.ActionsValueListeners.Contains(_Action))
		{
			this.ActionsValueListeners.Add(_Action);
		}
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00017E60 File Offset: 0x00016060
	public void RemoveListener(SelfTriggeredAction _Action)
	{
		if (this.ActionsValueListeners == null)
		{
			this.ActionsValueListeners = new List<SelfTriggeredAction>();
		}
		if (this.ActionsValueListeners.Contains(_Action))
		{
			this.ActionsValueListeners.Remove(_Action);
		}
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00017E90 File Offset: 0x00016090
	public void Load(StatSaveData _Data)
	{
		UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(_Data.StatID);
		if (!fromID)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (fromID.GetType() == typeof(GameStat))
		{
			this.Init(fromID as GameStat);
			this.CurrentBaseValue = _Data.BaseValue;
			this.AnimatedValue = this.CurrentValue(this.GM.NotInBase);
			this.LastKnownValue = this.CurrentValue(this.GM.NotInBase);
			if (_Data.StaleActions != null)
			{
				if (this.StalenessValues == null)
				{
					this.StalenessValues = new List<StalenessData>();
				}
				this.StalenessValues.AddRange(_Data.StaleActions);
			}
			if (_Data.Pinned)
			{
				this.GraphManager.PinStat(this, true);
			}
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00017F6C File Offset: 0x0001616C
	public StatSaveData Save()
	{
		StatSaveData statSaveData = new StatSaveData
		{
			StatID = UniqueIDScriptable.SaveID(this.StatModel),
			BaseValue = this.CurrentBaseValue,
			BaseRate = this.CurrentBaseRate,
			Pinned = this.IsPinned
		};
		if (this.CurrentStatuses != null && this.CurrentStatuses.Count > 0)
		{
			statSaveData.StatusDurations = new List<StatusDurationData>();
			for (int i = 0; i < this.CurrentStatuses.Count; i++)
			{
				if (this.CurrentStatuses[i].HasLog && !string.IsNullOrEmpty(this.CurrentStatuses[i].GameName))
				{
					statSaveData.StatusDurations.Add(this.CurrentStatuses[i].Save);
				}
			}
		}
		if (this.StalenessValues != null)
		{
			statSaveData.StaleActions = new List<StalenessData>();
			statSaveData.StaleActions.AddRange(this.StalenessValues);
		}
		return statSaveData;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0001805C File Offset: 0x0001625C
	public void UpdateTicks()
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.StalenessValues != null && this.StatModel.NoveltyCooldownDuration > 0)
		{
			for (int i = this.StalenessValues.Count - 1; i >= 0; i--)
			{
				StalenessData stalenessData = this.StalenessValues[i];
				int num = (this.GM.CurrentTickInfo.z - this.StalenessValues[i].LastTick) / this.StatModel.NoveltyCooldownDuration;
				stalenessData.Quantity -= num;
				stalenessData.LastTick += num * this.StatModel.NoveltyCooldownDuration;
				if (stalenessData.Quantity <= -1)
				{
					this.StalenessValues.RemoveAt(i);
				}
				else
				{
					this.StalenessValues[i] = stalenessData;
				}
			}
		}
		if (this.CurrentStatuses == null)
		{
			return;
		}
		if (this.CurrentStatuses.Count == 0)
		{
			return;
		}
		for (int j = 0; j < this.CurrentStatuses.Count; j++)
		{
			if (this.CurrentStatuses[j].HasLog && this.GM.CurrentTickInfo.z - this.CurrentStatuses[j].CreatedOnTick >= this.CurrentStatuses[j].StatusLog.TicksToRegister && !this.CurrentStatuses[j].HasBeenLogged)
			{
				this.CurrentStatuses[j].HasBeenLogged = true;
				this.GM.AddEndgameLog(this.CurrentStatuses[j].StatusLog);
			}
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00018214 File Offset: 0x00016414
	public void UpdateAnimatedValue()
	{
		if (!this.GM || !this.GraphManager)
		{
			return;
		}
		if (this.LastKnownValue != this.CurrentValue(this.GM.NotInBase))
		{
			this.LastKnownValue = this.CurrentValue(this.GM.NotInBase);
			this.LastChangeTimer = this.GraphManager.StatusBarChangeAnimationDelay;
		}
		if (this.LastChangeTimer <= 0f)
		{
			this.AnimatedValue = Mathf.MoveTowards(this.AnimatedValue, this.CurrentValue(this.GM.NotInBase), (this.StatModel.VisibleValue.y - this.StatModel.VisibleValue.x) * this.GraphManager.StatusBarChangeAnimationSpeed * Time.deltaTime);
			return;
		}
		this.LastChangeTimer -= Time.deltaTime;
	}

	// Token: 0x04000285 RID: 645
	public GameStat StatModel;

	// Token: 0x04000286 RID: 646
	public List<StalenessData> StalenessValues = new List<StalenessData>();

	// Token: 0x04000287 RID: 647
	public List<StatModifierSource> ModifierSources = new List<StatModifierSource>();

	// Token: 0x04000288 RID: 648
	public bool IsPinned;

	// Token: 0x04000289 RID: 649
	private int LastActionRegisteredTick = -1;

	// Token: 0x0400028A RID: 650
	public List<StatTimeOfDayModifier> CurrentTimeOfDayMods = new List<StatTimeOfDayModifier>();

	// Token: 0x0400028B RID: 651
	public List<StatStatus> CurrentStatuses = new List<StatStatus>();

	// Token: 0x0400028C RID: 652
	public float CurrentBaseRate;

	// Token: 0x0400028D RID: 653
	public float GlobalModifiedRate;

	// Token: 0x0400028E RID: 654
	public float AtBaseModifiedRate;

	// Token: 0x0400028F RID: 655
	public float CurrentBaseValue;

	// Token: 0x04000290 RID: 656
	public float GlobalModifiedValue;

	// Token: 0x04000291 RID: 657
	public float AtBaseModifiedValue;

	// Token: 0x04000295 RID: 661
	private float LastKnownValue;

	// Token: 0x04000296 RID: 662
	private float LastChangeTimer;

	// Token: 0x04000297 RID: 663
	public StatStatus DefaultStatus;

	// Token: 0x04000298 RID: 664
	private GameManager GM;

	// Token: 0x04000299 RID: 665
	private GraphicsManager GraphManager;
}
