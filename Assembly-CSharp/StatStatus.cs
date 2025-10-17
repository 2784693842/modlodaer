using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200013C RID: 316
[Serializable]
public class StatStatus
{
	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x0005657A File Offset: 0x0005477A
	// (set) Token: 0x06000920 RID: 2336 RVA: 0x00056582 File Offset: 0x00054782
	public StatStatusGraphics StatusGraphics { get; private set; }

	// Token: 0x06000921 RID: 2337 RVA: 0x0005658C File Offset: 0x0005478C
	public bool IsInRange(float _Value)
	{
		int num = ExtraMath.RoundOrFloor(_Value);
		if (this.ValueRange.x == this.ValueRange.y)
		{
			return num == this.ValueRange.x;
		}
		return num >= this.ValueRange.x && num <= this.ValueRange.y;
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x000565E8 File Offset: 0x000547E8
	public bool IsSameStatus(StatStatus _Other)
	{
		return _Other.GameName == this.GameName && _Other.Icon == this.Icon && _Other.ValueRange == this.ValueRange;
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x00056638 File Offset: 0x00054838
	public bool HasLog
	{
		get
		{
			return this.StatusLog != null && !string.IsNullOrEmpty(this.StatusLog.LogText);
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06000924 RID: 2340 RVA: 0x0005665C File Offset: 0x0005485C
	public StatusDurationData Save
	{
		get
		{
			return new StatusDurationData
			{
				StatusName = this.GameName,
				CreatedOnTick = this.CreatedOnTick,
				HasBeenLogged = this.HasBeenLogged
			};
		}
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x000566A0 File Offset: 0x000548A0
	public void Load(StatSaveData _FromSave)
	{
		if (_FromSave == null)
		{
			return;
		}
		if (_FromSave.StatusDurations == null)
		{
			return;
		}
		if (_FromSave.StatusDurations.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _FromSave.StatusDurations.Count; i++)
		{
			if (_FromSave.StatusDurations[i].StatusName == this.GameName)
			{
				this.CreatedOnTick = _FromSave.StatusDurations[i].CreatedOnTick;
				this.HasBeenLogged = _FromSave.StatusDurations[i].HasBeenLogged;
				return;
			}
		}
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00056730 File Offset: 0x00054930
	public StatStatus Instantiate(bool _Ascending, InGameStat _Parent)
	{
		StatStatus statStatus = new StatStatus
		{
			GameName = this.GameName,
			Icon = this.Icon,
			ValueRange = this.ValueRange,
			EffectsOnStats = new StatModifier[this.EffectsOnStats.Length],
			GameOver = this.GameOver,
			NotifyPlayer = this.NotifyPlayer,
			AlertLevel = this.AlertLevel,
			AlertSounds = this.AlertSounds,
			Description = this.Description,
			ParentStat = _Parent,
			EffectsOnActions = this.EffectsOnActions,
			StatusLog = this.StatusLog,
			ConfidenceModifier = this.ConfidenceModifier
		};
		if (this.ActionsMaxTicks > 0)
		{
			statStatus.ActionBlocker = new StatusActionBlocker
			{
				BlockedMessage = this.PreventingActionMessage,
				BlockThreshold = this.ActionsMaxTicks
			};
		}
		else
		{
			statStatus.ActionBlocker = null;
		}
		statStatus.ResetAlertDelays();
		for (int i = 0; i < this.EffectsOnStats.Length; i++)
		{
			statStatus.EffectsOnStats[i] = new StatModifier
			{
				Stat = this.EffectsOnStats[i].Stat,
				ValueModifier = (Mathf.Approximately(this.EffectsOnStats[i].ValueModifier.x, this.EffectsOnStats[i].ValueModifier.y) ? (Vector2.one * this.EffectsOnStats[i].ValueModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.EffectsOnStats[i].ValueModifier.x, this.EffectsOnStats[i].ValueModifier.y))),
				RateModifier = (Mathf.Approximately(this.EffectsOnStats[i].RateModifier.x, this.EffectsOnStats[i].RateModifier.y) ? (Vector2.one * this.EffectsOnStats[i].RateModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.EffectsOnStats[i].RateModifier.x, this.EffectsOnStats[i].RateModifier.y)))
			};
		}
		if (this.Icon && _Parent.StatModel.ShowInList)
		{
			statStatus.StatusGraphics = GraphicsManager.CreateStatusGraphics(statStatus, _Ascending);
		}
		else
		{
			statStatus.StatusGraphics = null;
		}
		if (MBSingleton<GameManager>.Instance)
		{
			statStatus.CreatedOnTick = MBSingleton<GameManager>.Instance.CurrentTickInfo.z;
		}
		else
		{
			statStatus.CreatedOnTick = -1;
		}
		return statStatus;
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x000569F4 File Offset: 0x00054BF4
	public int PriorityScore
	{
		get
		{
			int num = 0;
			if (this.ParentStat && this.ParentStat.StatModel)
			{
				num += this.ParentStat.StatModel.StatPriority;
			}
			if (this.ParentStat.IsPinned && this.AlertLevel == AlertLevels.None)
			{
				num += 5000;
			}
			return (int)(num + this.AlertLevel * (AlertLevels)10000);
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00056A64 File Offset: 0x00054C64
	public void UpdateAlertEffects(float _DeltaTime)
	{
		if (this.SoundDelay != Vector2.zero)
		{
			this.SoundTimer += _DeltaTime;
			if (this.SoundTimer >= this.NextSoundPlay)
			{
				this.<UpdateAlertEffects>g__PlaySound|41_0();
			}
		}
		if (this.TextDelay != Vector2.zero)
		{
			this.TextTimer += _DeltaTime;
			if (this.TextTimer >= this.NextTextDisplay)
			{
				MBSingleton<GraphicsManager>.Instance.PlayStatusAlertText(this);
				this.NextTextDisplay = UnityEngine.Random.Range(this.TextDelay.x, this.TextDelay.y);
				this.TextTimer = 0f;
				this.<UpdateAlertEffects>g__PlaySound|41_0();
			}
		}
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00056B10 File Offset: 0x00054D10
	private void ResetAlertDelays()
	{
		this.AlertSettings = GraphicsManager.GetStatusAlert(this.AlertLevel);
		this.SoundDelay = GraphicsManager.GetNotificationDelay(this.RepeatAlertSounds);
		this.TextDelay = GraphicsManager.GetNotificationDelay(this.RepeatTextNotification);
		this.NextSoundPlay = UnityEngine.Random.Range(this.SoundDelay.x, this.SoundDelay.y);
		this.NextTextDisplay = UnityEngine.Random.Range(this.TextDelay.x, this.TextDelay.y);
		this.SoundTimer = 0f;
		this.TextTimer = 0f;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00056BA8 File Offset: 0x00054DA8
	[CompilerGenerated]
	private void <UpdateAlertEffects>g__PlaySound|41_0()
	{
		MBSingleton<SoundManager>.Instance.PerformStatusAlertSound(this.AlertSounds);
		this.NextSoundPlay = UnityEngine.Random.Range(this.SoundDelay.x, this.SoundDelay.y);
		this.SoundTimer = 0f;
	}

	// Token: 0x04000E87 RID: 3719
	public LocalizedString GameName;

	// Token: 0x04000E88 RID: 3720
	public LocalizedString Description;

	// Token: 0x04000E89 RID: 3721
	public StatusEndgameLog StatusLog;

	// Token: 0x04000E8A RID: 3722
	public Sprite Icon;

	// Token: 0x04000E8B RID: 3723
	[MinMax]
	public Vector2Int ValueRange;

	// Token: 0x04000E8C RID: 3724
	[StatModifierOptions(true, false)]
	public StatModifier[] EffectsOnStats;

	// Token: 0x04000E8D RID: 3725
	public ActionModifier[] EffectsOnActions;

	// Token: 0x04000E8E RID: 3726
	[Space]
	[FormerlySerializedAs("AlertPlayer")]
	public AlertNotificationTypes NotifyPlayer;

	// Token: 0x04000E8F RID: 3727
	public NotificationFrequency RepeatTextNotification;

	// Token: 0x04000E90 RID: 3728
	public AlertLevels AlertLevel;

	// Token: 0x04000E91 RID: 3729
	public AudioClip[] AlertSounds;

	// Token: 0x04000E92 RID: 3730
	public NotificationFrequency RepeatAlertSounds;

	// Token: 0x04000E93 RID: 3731
	[Space]
	public bool GameOver;

	// Token: 0x04000E94 RID: 3732
	public int ActionsMaxTicks;

	// Token: 0x04000E95 RID: 3733
	public LocalizedString PreventingActionMessage;

	// Token: 0x04000E96 RID: 3734
	[NonSerialized]
	public InGameStat ParentStat;

	// Token: 0x04000E97 RID: 3735
	private float NextTextDisplay;

	// Token: 0x04000E98 RID: 3736
	private float NextSoundPlay;

	// Token: 0x04000E99 RID: 3737
	private float TextTimer;

	// Token: 0x04000E9A RID: 3738
	private float SoundTimer;

	// Token: 0x04000E9B RID: 3739
	private StatusAlertSettings AlertSettings;

	// Token: 0x04000E9C RID: 3740
	private Vector2 SoundDelay;

	// Token: 0x04000E9D RID: 3741
	private Vector2 TextDelay;

	// Token: 0x04000E9E RID: 3742
	[NonSerialized]
	public int CreatedOnTick;

	// Token: 0x04000E9F RID: 3743
	[NonSerialized]
	public bool HasBeenLogged;

	// Token: 0x04000EA0 RID: 3744
	public StatusActionBlocker ActionBlocker;

	// Token: 0x04000EA1 RID: 3745
	public float ConfidenceModifier;
}
