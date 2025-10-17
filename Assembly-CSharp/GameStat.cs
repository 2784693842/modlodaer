using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000134 RID: 308
[CreateAssetMenu(menuName = "Survival/Game Stat")]
public class GameStat : UniqueIDScriptable
{
	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000903 RID: 2307 RVA: 0x00055BA0 File Offset: 0x00053DA0
	public Color GetBarColor
	{
		get
		{
			Color defaultBarColor = StatStatusGraphics.DefaultBarColor;
			if (this.BarColor == Color.clear)
			{
				return defaultBarColor;
			}
			if (this.BarColor.r + this.BarColor.g + this.BarColor.b == 0f)
			{
				return default(Color);
			}
			return new Color(this.BarColor.r, this.BarColor.g, this.BarColor.b, 1f);
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000904 RID: 2308 RVA: 0x00055C28 File Offset: 0x00053E28
	public StatStatus GetDefaultStatus
	{
		get
		{
			return new StatStatus
			{
				GameName = (string.IsNullOrEmpty(this.DefaultStatusName) ? this.GameName : this.DefaultStatusName),
				Description = this.DefaultStatusDescription,
				Icon = this.GetIcon,
				AlertLevel = AlertLevels.None,
				NotifyPlayer = AlertNotificationTypes.DontNotify,
				RepeatTextNotification = NotificationFrequency.Never,
				EffectsOnStats = new StatModifier[0]
			};
		}
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x00055C99 File Offset: 0x00053E99
	public string StatMissingText(bool _TooMuch)
	{
		if (!_TooMuch || string.IsNullOrEmpty(this.TooMuchText))
		{
			return this.NotEnoughText;
		}
		return this.TooMuchText;
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000906 RID: 2310 RVA: 0x00055CC7 File Offset: 0x00053EC7
	public Vector2 VisibleValue
	{
		get
		{
			if (Mathf.Approximately(Vector2.SqrMagnitude(this.VisibleValueRange), 0f))
			{
				return this.MinMaxValue;
			}
			return this.VisibleValueRange;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000907 RID: 2311 RVA: 0x00055CED File Offset: 0x00053EED
	public Vector2 VisibleTrend
	{
		get
		{
			if (Mathf.Approximately(Vector2.SqrMagnitude(this.TrendIndicatorRateRange), 0f))
			{
				return this.MinMaxRate;
			}
			return this.TrendIndicatorRateRange;
		}
	}

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000908 RID: 2312 RVA: 0x00055D14 File Offset: 0x00053F14
	public bool HasTimeOfDayModsWithRequirements
	{
		get
		{
			if (this.TimeOfDayMods == null)
			{
				return false;
			}
			if (this.TimeOfDayMods.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.TimeOfDayMods.Length; i++)
			{
				if (this.TimeOfDayMods[i].HasStatRequirements)
				{
					return true;
				}
				if (this.TimeOfDayMods[i].HasCardsOrTagRequirements)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000909 RID: 2313 RVA: 0x00055D74 File Offset: 0x00053F74
	public Sprite GetIcon
	{
		get
		{
			if (this.DefaultStatusIcon)
			{
				return this.DefaultStatusIcon;
			}
			if (this.FeedbackInfo.Icon)
			{
				return this.FeedbackInfo.Icon;
			}
			if (this.Statuses != null && this.Statuses.Length != 0)
			{
				for (int i = 0; i < this.Statuses.Length; i++)
				{
					if (this.Statuses[i].Icon)
					{
						return this.Statuses[i].Icon;
					}
				}
			}
			if (this.FeedbackInfo.NegativeIcon)
			{
				return this.FeedbackInfo.NegativeIcon;
			}
			return null;
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x0600090A RID: 2314 RVA: 0x00055E18 File Offset: 0x00054018
	public bool ShowInList
	{
		get
		{
			return this.CanBeVisible && (this.Visibility != StatVisibilityOptions.NeverVisible || this.VisibleInDetailedList);
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x0600090B RID: 2315 RVA: 0x00055E38 File Offset: 0x00054038
	public bool CanBeVisible
	{
		get
		{
			if (this.RequiredPerks == null)
			{
				return true;
			}
			if (this.RequiredPerks.Length == 0)
			{
				return true;
			}
			if (!GameManager.CurrentPlayerCharacter)
			{
				return false;
			}
			bool result = true;
			for (int i = 0; i < this.RequiredPerks.Length; i++)
			{
				if (this.RequiredPerks[i])
				{
					result = false;
					if (GameManager.CurrentPlayerCharacter.CharacterPerks.Contains(this.RequiredPerks[i]))
					{
						return true;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x04000E40 RID: 3648
	public LocalizedString GameName;

	// Token: 0x04000E41 RID: 3649
	public LocalizedString Description;

	// Token: 0x04000E42 RID: 3650
	public float BaseValue;

	// Token: 0x04000E43 RID: 3651
	[MinMax]
	public Vector2 MinMaxValue;

	// Token: 0x04000E44 RID: 3652
	public float BaseRatePerTick;

	// Token: 0x04000E45 RID: 3653
	[MinMax]
	public Vector2 MinMaxRate;

	// Token: 0x04000E46 RID: 3654
	[SpecialHeader("Time Of Day Modifiers", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public StatTimeOfDayModifier[] TimeOfDayMods;

	// Token: 0x04000E47 RID: 3655
	[SpecialHeader("Statuses", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[FormerlySerializedAs("NotificationPriority")]
	public int StatPriority;

	// Token: 0x04000E48 RID: 3656
	[Header("Visibility options")]
	public CharacterPerk[] RequiredPerks;

	// Token: 0x04000E49 RID: 3657
	public StatVisibilityOptions Visibility;

	// Token: 0x04000E4A RID: 3658
	[Tooltip("Only useful if Never Visible")]
	public bool VisibleInDetailedList;

	// Token: 0x04000E4B RID: 3659
	public bool CannotBeInspected;

	// Token: 0x04000E4C RID: 3660
	public bool InvertedDirection;

	// Token: 0x04000E4D RID: 3661
	[SerializeField]
	private Vector2 VisibleValueRange;

	// Token: 0x04000E4E RID: 3662
	[SerializeField]
	private Vector2 TrendIndicatorRateRange;

	// Token: 0x04000E4F RID: 3663
	public bool StatusesHaveNoBar;

	// Token: 0x04000E50 RID: 3664
	[SerializeField]
	private Color BarColor;

	// Token: 0x04000E51 RID: 3665
	public Color BarHighlightColor;

	// Token: 0x04000E52 RID: 3666
	[Header("Statuses Setup")]
	[SerializeField]
	private LocalizedString DefaultStatusName;

	// Token: 0x04000E53 RID: 3667
	[SerializeField]
	private LocalizedString DefaultStatusDescription;

	// Token: 0x04000E54 RID: 3668
	[SerializeField]
	private Sprite DefaultStatusIcon;

	// Token: 0x04000E55 RID: 3669
	public StatStatus[] Statuses;

	// Token: 0x04000E56 RID: 3670
	[SpecialHeader("Novelty Settings", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public bool UsesNovelty;

	// Token: 0x04000E57 RID: 3671
	public int NoveltyCooldownDuration;

	// Token: 0x04000E58 RID: 3672
	public float StalenessMultiplier;

	// Token: 0x04000E59 RID: 3673
	public int MaxStalenessStack;

	// Token: 0x04000E5A RID: 3674
	[SpecialHeader("Actions Feedbacks", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	public bool ShowActionEffects;

	// Token: 0x04000E5B RID: 3675
	public AmtFeedbackInfo FeedbackInfo;

	// Token: 0x04000E5C RID: 3676
	public UIFeedbackStepsBase OverrideFeedbackPrefab;

	// Token: 0x04000E5D RID: 3677
	[SpecialHeader("Notification", HeaderSizes.Big, HeaderStyles.Framed, 0f)]
	[SerializeField]
	private LocalizedString NotEnoughText;

	// Token: 0x04000E5E RID: 3678
	public Sprite NotEnoughIcon;

	// Token: 0x04000E5F RID: 3679
	[SerializeField]
	private LocalizedString TooMuchText;

	// Token: 0x04000E60 RID: 3680
	[Space]
	public bool StatusDebugMode;
}
