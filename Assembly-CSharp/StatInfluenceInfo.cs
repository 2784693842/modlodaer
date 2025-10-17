using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A7 RID: 167
public class StatInfluenceInfo : MonoBehaviour
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060006BD RID: 1725 RVA: 0x00045FF6 File Offset: 0x000441F6
	// (set) Token: 0x060006BE RID: 1726 RVA: 0x00045FFE File Offset: 0x000441FE
	public GameStat InfluencedStat { get; private set; }

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060006BF RID: 1727 RVA: 0x00046007 File Offset: 0x00044207
	private int StackAmt
	{
		get
		{
			if (this.CardSources == null)
			{
				return 0;
			}
			return Mathf.Max(0, this.CardSources.Count - 1);
		}
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x00046028 File Offset: 0x00044228
	public void Setup(StatModifierSource _Source, GameStat _Stat)
	{
		if (_Source.GetSource == null || _Stat == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.InfluencedStat = _Stat;
		this.RateMod = _Source.Rate;
		this.ValueMod = _Source.Value;
		if (Mathf.Approximately(this.RateMod, 0f) && Mathf.Approximately(this.ValueMod, 0f))
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.TimeText.gameObject.SetActive(false);
		this.Icon.gameObject.SetActive(true);
		this.StatSource = null;
		if (this.CardSources == null)
		{
			this.CardSources = new List<InGameCardBase>();
		}
		else
		{
			this.CardSources.Clear();
		}
		this.PerkSource = null;
		this.TimeOfDaySource = null;
		object getSource = _Source.GetSource;
		if (getSource != null)
		{
			InGameStat inGameStat;
			if ((inGameStat = (getSource as InGameStat)) == null)
			{
				InGameCardBase inGameCardBase;
				if ((inGameCardBase = (getSource as InGameCardBase)) == null)
				{
					CharacterPerk characterPerk;
					if ((characterPerk = (getSource as CharacterPerk)) == null)
					{
						TimeOfDayStatModSource timeOfDayStatModSource;
						if ((timeOfDayStatModSource = (getSource as TimeOfDayStatModSource)) == null)
						{
							goto IL_131;
						}
						TimeOfDayStatModSource time = timeOfDayStatModSource;
						this.SetupTimeOfDaySource(time);
					}
					else
					{
						CharacterPerk perk = characterPerk;
						this.SetupPerkSource(perk);
					}
				}
				else
				{
					InGameCardBase card = inGameCardBase;
					this.SetupCardSource(card);
				}
			}
			else
			{
				InGameStat stat = inGameStat;
				this.SetupStatSource(stat);
			}
			this.Update();
			return;
		}
		IL_131:
		base.gameObject.SetActive(false);
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0004617C File Offset: 0x0004437C
	public void AddFromOtherInfluenceInfo(StatInfluenceInfo _Info)
	{
		if (this.CardSources == null)
		{
			this.CardSources = new List<InGameCardBase>();
		}
		if (_Info.CardSources == null)
		{
			return;
		}
		if (_Info.CardSources.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _Info.CardSources.Count; i++)
		{
			if (!this.CardSources.Contains(_Info.CardSources[i]))
			{
				this.CardSources.Add(_Info.CardSources[i]);
			}
		}
		this.ValueMod += _Info.ValueMod;
		this.RateMod += _Info.RateMod;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x00046220 File Offset: 0x00044420
	private void SetupStatSource(InGameStat _Stat)
	{
		if (!_Stat.StatModel)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (!_Stat.StatModel.ShowInList)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.InteractionButton.interactable = !_Stat.StatModel.CannotBeInspected;
		this.StatSource = _Stat;
		this.Icon.sprite = _Stat.StatModel.GetIcon;
		StatStatus statStatus = _Stat.AnyCurrentStatus(true);
		if (statStatus != null)
		{
			this.InfluenceName.text = statStatus.GameName;
		}
		else
		{
			this.InfluenceName.text = _Stat.StatModel.GameName;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x000462E4 File Offset: 0x000444E4
	private void SetupCardSource(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (_Card.InBackground)
		{
			this.InteractionButton.interactable = false;
		}
		else
		{
			this.InteractionButton.interactable = true;
		}
		InGameCardBase inGameCardBase = _Card.IsLiquid ? _Card.CurrentContainer : _Card;
		if (_Card.CardModel.CardType == CardTypes.Environment)
		{
			inGameCardBase = MBSingleton<GameManager>.Instance.CurrentExplorableCard;
		}
		if (!inGameCardBase)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (inGameCardBase.CardVisuals)
		{
			this.Icon.sprite = inGameCardBase.CurrentImage;
		}
		else
		{
			if (!inGameCardBase.CardModel.CardImage)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.Icon.sprite = inGameCardBase.CardModel.CardImage;
		}
		if (this.CardSources == null)
		{
			this.CardSources = new List<InGameCardBase>();
		}
		if (!this.CardSources.Contains(_Card))
		{
			this.CardSources.Add(_Card);
		}
		this.InfluenceName.text = _Card.CardName(true);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00046410 File Offset: 0x00044610
	private void SetupPerkSource(CharacterPerk _Perk)
	{
		this.InteractionButton.interactable = false;
		this.Icon.sprite = _Perk.PerkIcon;
		this.InfluenceName.text = _Perk.PerkName;
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0004645C File Offset: 0x0004465C
	private void SetupTimeOfDaySource(TimeOfDayStatModSource _Time)
	{
		this.InteractionButton.interactable = false;
		this.Icon.gameObject.SetActive(false);
		this.TimeText.gameObject.SetActive(true);
		this.TimeText.text = GameManager.CurrentHourOfDayString();
		this.InfluenceName.text = LocalizedString.TimeOfDayEffect(_Time.EffectStartingTime, _Time.EffectEndTime);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x000464D0 File Offset: 0x000446D0
	private void Update()
	{
		if (!Mathf.Approximately(this.RateMod, 0f))
		{
			this.ValueChangeIndicator.gameObject.SetActive(false);
			if (!this.RateChangeIndicator.gameObject.activeInHierarchy)
			{
				this.RateChangeIndicator.gameObject.SetActive(true);
			}
			this.RateChangeIndicator.UpdateAnim(this.RateMod, this.InfluencedStat.VisibleTrend, this.InfluencedStat.InvertedDirection);
		}
		else
		{
			this.RateChangeIndicator.gameObject.SetActive(false);
			if (!Mathf.Approximately(this.ValueMod, 0f))
			{
				if (!this.ValueChangeIndicator.gameObject.activeInHierarchy)
				{
					this.ValueChangeIndicator.gameObject.SetActive(true);
				}
				this.ValueChangeIndicator.UpdateAnim(this.ValueMod, new Vector2(-1f, 1f) * Mathf.Abs(this.InfluencedStat.VisibleValue.y - this.InfluencedStat.VisibleValue.x), this.InfluencedStat.InvertedDirection);
			}
			else
			{
				this.ValueChangeIndicator.gameObject.SetActive(false);
			}
		}
		if (this.StackingNumberObject)
		{
			this.StackingNumberObject.SetActive(this.StackAmt > 0);
			this.StackingNumberText.text = string.Format("x{0}", (this.StackAmt + 1).ToString());
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00046644 File Offset: 0x00044844
	public CardData CardModelInfluence
	{
		get
		{
			if (this.CardSources == null)
			{
				return null;
			}
			if (this.CardSources.Count == 0)
			{
				return null;
			}
			return this.CardSources[0].CardModel;
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00046670 File Offset: 0x00044870
	public void OnClick()
	{
		if (this.StatSource)
		{
			MBSingleton<GraphicsManager>.Instance.InspectStat(MBSingleton<GameManager>.Instance.StatsDict[this.StatSource.StatModel]);
			return;
		}
		if (this.CardSources != null && this.CardSources.Count > 0)
		{
			MBSingleton<GraphicsManager>.Instance.FindAndPulseCard(this.CardSources[0]);
		}
	}

	// Token: 0x0400096C RID: 2412
	[SerializeField]
	private TextMeshProUGUI InfluenceName;

	// Token: 0x0400096D RID: 2413
	[SerializeField]
	private TextMeshProUGUI TimeText;

	// Token: 0x0400096E RID: 2414
	[SerializeField]
	private Image Icon;

	// Token: 0x0400096F RID: 2415
	[SerializeField]
	private StatTrendIndicator RateChangeIndicator;

	// Token: 0x04000970 RID: 2416
	[SerializeField]
	private StatTrendIndicator ValueChangeIndicator;

	// Token: 0x04000971 RID: 2417
	[SerializeField]
	private Button InteractionButton;

	// Token: 0x04000972 RID: 2418
	[SerializeField]
	private TextMeshProUGUI StackingNumberText;

	// Token: 0x04000973 RID: 2419
	[SerializeField]
	private GameObject StackingNumberObject;

	// Token: 0x04000974 RID: 2420
	private InGameStat StatSource;

	// Token: 0x04000975 RID: 2421
	private List<InGameCardBase> CardSources;

	// Token: 0x04000976 RID: 2422
	private CharacterPerk PerkSource;

	// Token: 0x04000977 RID: 2423
	private TimeOfDayStatModSource TimeOfDaySource;

	// Token: 0x04000978 RID: 2424
	private float RateMod;

	// Token: 0x04000979 RID: 2425
	private float ValueMod;
}
