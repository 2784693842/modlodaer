using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A6 RID: 166
public class StatDetailsPopup : MonoBehaviour
{
	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0004573F File Offset: 0x0004393F
	// (set) Token: 0x060006B3 RID: 1715 RVA: 0x00045747 File Offset: 0x00043947
	public InGameStat InspectedStat { get; private set; }

	// Token: 0x060006B4 RID: 1716 RVA: 0x00045750 File Offset: 0x00043950
	private void Awake()
	{
		this.VisibleFrameParent = this.VisibleRangeFrame.parent.GetComponent<RectTransform>();
		this.StatusButton.Setup(0, LocalizedString.CurrentStatus, null, false);
		this.InfluencesButton.Setup(1, LocalizedString.StatInfluences, null, false);
		IndexButton statusButton = this.StatusButton;
		statusButton.OnClicked = (Action<int>)Delegate.Combine(statusButton.OnClicked, new Action<int>(this.SetTab));
		IndexButton influencesButton = this.InfluencesButton;
		influencesButton.OnClicked = (Action<int>)Delegate.Combine(influencesButton.OnClicked, new Action<int>(this.SetTab));
		this.SetTab(0);
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x000457F8 File Offset: 0x000439F8
	public void SetTab(int _Index)
	{
		this.StatusButton.Selected = (_Index == 0);
		this.InfluencesButton.Selected = (_Index == 1);
		this.StatusObject.SetActive(_Index == 0);
		this.InfluencesObject.SetActive(_Index == 1);
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x00045838 File Offset: 0x00043A38
	public void Setup(InGameStat _Stat)
	{
		if (this.VisibleFrameParent == null)
		{
			this.Awake();
		}
		if (!_Stat)
		{
			return;
		}
		if (!_Stat.StatModel)
		{
			return;
		}
		this.InspectedStat = _Stat;
		this.StatModel = _Stat.StatModel;
		this.PinButton.isOn = _Stat.IsPinned;
		this.CurrentStatus = this.InspectedStat.AnyCurrentStatus(!this.InspectedStat.StatModel.ShowInList);
		this.HelpButton.SetActive(GuideManager.GetPageFor(this.InspectedStat));
		this.Title.text = this.StatModel.GameName;
		this.Description.text = this.StatModel.Description;
		this.StatIcon.sprite = this.StatModel.GetIcon;
		this.BarSlider.value = _Stat.NormalizedRealValue;
		float num = Mathf.Abs(this.StatModel.MinMaxValue.y - this.StatModel.MinMaxValue.x);
		float num2 = Mathf.Abs(this.StatModel.VisibleValue.y - this.StatModel.VisibleValue.x);
		Rect rect = new Rect(Mathf.InverseLerp(this.StatModel.MinMaxValue.x, this.StatModel.MinMaxValue.y, this.StatModel.VisibleValue.x) * this.VisibleFrameParent.rect.width, this.VisibleRangeFrame.transform.localPosition.y, num2 / num * this.VisibleFrameParent.rect.width, this.VisibleRangeFrame.rect.height);
		this.VisibleRangeFrame.localPosition = new Vector3(rect.x, rect.y, 0f);
		this.VisibleRangeFrame.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width);
		if (this.ColorElements != null && this.ColorElements.Length != 0)
		{
			for (int i = 0; i < this.ColorElements.Length; i++)
			{
				if (this.ColorElements[i])
				{
					this.ColorElements[i].color = this.StatModel.GetBarColor;
				}
			}
		}
		this.TrendIndicator.Setup(_Stat);
		if (this.CurrentStatus != null)
		{
			this.CurrentStatusName.text = this.CurrentStatus.GameName;
			this.CurrentStatusDescription.text = this.CurrentStatus.Description;
			if (this.CurrentStatus.Icon)
			{
				this.CurrentStatusIcon.sprite = this.CurrentStatus.Icon;
			}
			else
			{
				this.CurrentStatusIcon.sprite = this.StatModel.GetIcon;
			}
			this.CurrentStatusIcon.enabled = true;
			StatusAlertSettings statusAlert = GraphicsManager.GetStatusAlert(this.CurrentStatus.AlertLevel);
			if (this.AlertIcon)
			{
				if (!statusAlert.AlertIcon)
				{
					this.AlertIcon.enabled = false;
				}
				else
				{
					this.AlertIcon.enabled = true;
					this.AlertIcon.sprite = statusAlert.AlertIcon;
				}
			}
			if (this.StatusIconOutline)
			{
				this.StatusIconOutline.effectColor = statusAlert.OutlineColor;
			}
		}
		else
		{
			this.CurrentStatusName.text = LocalizedString.None;
			this.CurrentStatusIcon.enabled = false;
			this.CurrentStatusDescription.text = "";
			if (this.AlertIcon)
			{
				this.AlertIcon.enabled = false;
			}
		}
		this.SetAffectedStats();
		this.SetInfluences();
		this.SetTab(0);
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x00045C08 File Offset: 0x00043E08
	private void SetInfluences()
	{
		this.CardModelInfluences.Clear();
		int num = 0;
		while (num < this.InspectedStat.ModifierSources.Count || num < this.InfluencesList.Count)
		{
			if (num >= this.InspectedStat.ModifierSources.Count)
			{
				this.InfluencesList[num].gameObject.SetActive(false);
			}
			else
			{
				if (num >= this.InfluencesList.Count)
				{
					this.InfluencesList.Add(UnityEngine.Object.Instantiate<StatInfluenceInfo>(this.InfluencePrefab, this.InfluencesParent));
				}
				this.InfluencesList[num].Setup(this.InspectedStat.ModifierSources[num], this.InspectedStat.StatModel);
				if (this.InfluencesList[num].CardModelInfluence)
				{
					if (!this.CardModelInfluences.ContainsKey(this.InfluencesList[num].CardModelInfluence))
					{
						this.CardModelInfluences.Add(this.InfluencesList[num].CardModelInfluence, this.InfluencesList[num]);
					}
					else
					{
						this.CardModelInfluences[this.InfluencesList[num].CardModelInfluence].AddFromOtherInfluenceInfo(this.InfluencesList[num]);
						this.InfluencesList[num].gameObject.SetActive(false);
					}
				}
			}
			num++;
		}
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00045D80 File Offset: 0x00043F80
	private void SetAffectedStats()
	{
		int num = 0;
		if (this.CurrentStatus != null)
		{
			if (this.CurrentStatus.EffectsOnStats == null)
			{
				num = 0;
			}
			else
			{
				num = this.CurrentStatus.EffectsOnStats.Length;
			}
		}
		int num2 = 0;
		while (num2 < num || num2 < this.AffectedStatsList.Count)
		{
			if (num2 >= num)
			{
				this.AffectedStatsList[num2].gameObject.SetActive(false);
			}
			else
			{
				if (num2 >= this.AffectedStatsList.Count)
				{
					this.AffectedStatsList.Add(UnityEngine.Object.Instantiate<AffectedStatInfo>(this.AffectedStatPrefab, this.AffectedStatsParent));
				}
				if (!this.CurrentStatus.EffectsOnStats[num2].Stat)
				{
					this.AffectedStatsList[num2].gameObject.SetActive(false);
				}
				else if (!this.CurrentStatus.EffectsOnStats[num2].Stat.ShowInList)
				{
					this.AffectedStatsList[num2].gameObject.SetActive(false);
				}
				else if (Mathf.Approximately(this.CurrentStatus.EffectsOnStats[num2].MaxModifiedRate, 0f) && (Mathf.Approximately(this.CurrentStatus.EffectsOnStats[num2].MaxModifiedValue, 0f) || !this.ShowValueChangesOnAffectedStats))
				{
					this.AffectedStatsList[num2].gameObject.SetActive(false);
				}
				else
				{
					this.AffectedStatsList[num2].Setup(this.CurrentStatus.EffectsOnStats[num2].Stat, this.CurrentStatus.EffectsOnStats[num2].MaxModifiedRate, this.ShowValueChangesOnAffectedStats ? this.CurrentStatus.EffectsOnStats[num2].MaxModifiedValue : 0f);
					this.AffectedStatsList[num2].gameObject.SetActive(true);
				}
			}
			num2++;
		}
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x00045F73 File Offset: 0x00044173
	private void Update()
	{
		this.TrendIndicator.UpdateAnim();
		if (this.InspectedStat)
		{
			this.BarSlider.value = this.InspectedStat.NormalizedRealValue;
		}
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x00045FA3 File Offset: 0x000441A3
	public void OnPinClicked(bool _Value)
	{
		MBSingleton<GraphicsManager>.Instance.PinStat(this.InspectedStat, _Value);
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00045FB6 File Offset: 0x000441B6
	public void HelpClicked()
	{
		MBSingleton<GameManager>.Instance.OpenGuide(GuideManager.GetPageFor(this.InspectedStat));
	}

	// Token: 0x0400094E RID: 2382
	[SerializeField]
	private TextMeshProUGUI Title;

	// Token: 0x0400094F RID: 2383
	[SerializeField]
	private Toggle PinButton;

	// Token: 0x04000950 RID: 2384
	[SerializeField]
	private GameObject HelpButton;

	// Token: 0x04000951 RID: 2385
	[SerializeField]
	private Image StatIcon;

	// Token: 0x04000952 RID: 2386
	[SerializeField]
	private RectTransform VisibleRangeFrame;

	// Token: 0x04000953 RID: 2387
	[SerializeField]
	private Slider BarSlider;

	// Token: 0x04000954 RID: 2388
	[SerializeField]
	private StatTrendIndicator TrendIndicator;

	// Token: 0x04000955 RID: 2389
	[SerializeField]
	private Image[] ColorElements;

	// Token: 0x04000956 RID: 2390
	[SerializeField]
	private TextMeshProUGUI Description;

	// Token: 0x04000957 RID: 2391
	[SerializeField]
	private IndexButton StatusButton;

	// Token: 0x04000958 RID: 2392
	[SerializeField]
	private IndexButton InfluencesButton;

	// Token: 0x04000959 RID: 2393
	[Header("Statuses")]
	[SerializeField]
	private GameObject StatusObject;

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	private TextMeshProUGUI CurrentStatusName;

	// Token: 0x0400095B RID: 2395
	[SerializeField]
	private TextMeshProUGUI CurrentStatusDescription;

	// Token: 0x0400095C RID: 2396
	[SerializeField]
	private Image CurrentStatusIcon;

	// Token: 0x0400095D RID: 2397
	[SerializeField]
	private Outline StatusIconOutline;

	// Token: 0x0400095E RID: 2398
	[SerializeField]
	private Image AlertIcon;

	// Token: 0x0400095F RID: 2399
	[SerializeField]
	private AffectedStatInfo AffectedStatPrefab;

	// Token: 0x04000960 RID: 2400
	[SerializeField]
	private RectTransform AffectedStatsParent;

	// Token: 0x04000961 RID: 2401
	[SerializeField]
	private bool ShowValueChangesOnAffectedStats;

	// Token: 0x04000962 RID: 2402
	[Header("Influences")]
	[SerializeField]
	private GameObject InfluencesObject;

	// Token: 0x04000963 RID: 2403
	[SerializeField]
	private RectTransform InfluencesParent;

	// Token: 0x04000964 RID: 2404
	[SerializeField]
	private StatInfluenceInfo InfluencePrefab;

	// Token: 0x04000965 RID: 2405
	private RectTransform VisibleFrameParent;

	// Token: 0x04000966 RID: 2406
	private List<AffectedStatInfo> AffectedStatsList = new List<AffectedStatInfo>();

	// Token: 0x04000967 RID: 2407
	private List<StatInfluenceInfo> InfluencesList = new List<StatInfluenceInfo>();

	// Token: 0x04000968 RID: 2408
	private GameStat StatModel;

	// Token: 0x04000969 RID: 2409
	private StatStatus CurrentStatus;

	// Token: 0x0400096A RID: 2410
	private Dictionary<CardData, StatInfluenceInfo> CardModelInfluences = new Dictionary<CardData, StatInfluenceInfo>();
}
