using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200005D RID: 93
public class DismantleActionButton : IndexButton
{
	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060003D5 RID: 981 RVA: 0x00027A40 File Offset: 0x00025C40
	// (set) Token: 0x060003D6 RID: 982 RVA: 0x00027A48 File Offset: 0x00025C48
	public bool ConditionsValid { get; private set; }

	// Token: 0x060003D7 RID: 983 RVA: 0x00027A51 File Offset: 0x00025C51
	private void Awake()
	{
		if (this.StackActionButton)
		{
			this.StackActionButton.onClick.AddListener(new UnityAction(this.ClickStackAction));
		}
		this.GM = MBSingleton<GameManager>.Instance;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00027A88 File Offset: 0x00025C88
	private void SetClickSoundsMute(bool _Value)
	{
		if (this.SoundScripts == null)
		{
			return;
		}
		if (this.SoundScripts.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.SoundScripts.Length; i++)
		{
			if (this.SoundScripts[i])
			{
				this.SoundScripts[i].MuteClickSounds = _Value;
			}
		}
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00027AD8 File Offset: 0x00025CD8
	public bool Setup(int _Index, DismantleCardAction _Action, InGameCardBase _Card, bool _Highlighted, bool _StackVersion)
	{
		_Action.CollectActionModifiers(_Card, null);
		if (_Action.HasActionSounds)
		{
			this.SetClickSoundsMute(true);
		}
		else if (_Card)
		{
			if (_Card.CardModel)
			{
				if (_Card.CardModel.CardType != CardTypes.Liquid)
				{
					this.SetClickSoundsMute(_Card.CardModel.HasOnCreatedSound);
				}
				else if (_Card.CurrentContainer)
				{
					if (_Card.CurrentContainer.CardModel)
					{
						this.SetClickSoundsMute(_Card.CurrentContainer.CardModel.HasOnCreatedSound);
					}
				}
				else
				{
					this.SetClickSoundsMute(false);
				}
			}
			else
			{
				this.SetClickSoundsMute(false);
			}
		}
		else
		{
			this.SetClickSoundsMute(false);
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.MissingStats == null)
		{
			this.MissingStats = new List<MissingStatInfo>();
		}
		else
		{
			this.MissingStats.Clear();
		}
		if (this.MissingCards == null)
		{
			this.MissingCards = new List<CardData>();
		}
		else
		{
			this.MissingCards.Clear();
		}
		if (this.MissingTags == null)
		{
			this.MissingTags = new List<CardTag>();
		}
		else
		{
			this.MissingTags.Clear();
		}
		this.MissingDurabilities = "";
		this.BlockingStatus = "";
		string text = _Action.ActionName;
		this.EnoughTime = _Action.EnoughDaylightPoints();
		this.DemoOK = _Action.IsNotCancelledByDemo;
		bool flag = _Action.StatsAreCorrect(this.MissingStats, false);
		bool flag2 = _Action.DurabilitiesAreCorrect(_Card, out this.MissingDurabilities);
		bool hideActionIfNotMet = _Action.RequiredReceivingDurabilities.HideActionIfNotMet;
		bool flag3 = this.GM.CheckActionBlockers(_Action, out this.BlockingStatus);
		bool flag4 = _Action.CardsAndTagsAreCorrect(_Card, this.MissingCards, this.MissingTags);
		if (_Action.HasSuccessfulDrop)
		{
			this.DropReport = this.GM.GetCollectionDropsReport(_Action, _Card, false);
		}
		else
		{
			this.DropReport = default(CollectionDropReport);
		}
		base.gameObject.SetActive((flag || this.MissingStats.Count > 0) && (!hideActionIfNotMet || flag2));
		this.ConditionsValid = (this.DemoOK && flag && this.EnoughTime && flag2 && flag4 && flag3);
		base.Interactable = this.ConditionsValid;
		if (this.StackActionButton)
		{
			this.StackActionObject.SetActive(_StackVersion);
			this.StackActionButton.interactable = base.Interactable;
			if (_Card && _StackVersion)
			{
				if (_Card.CurrentSlot)
				{
					this.StackActionText.text = LocalizedString.DoStackAction(_Card.CurrentSlot.CardPileCount(false));
				}
				else if (_Card.CurrentContainer && _Card.IsLiquid && _Card.CurrentContainer.CurrentSlot)
				{
					this.StackActionText.text = LocalizedString.DoStackAction(_Card.CurrentContainer.CurrentSlot.CardPileCount(false));
				}
			}
		}
		if (_Action.HasSuccessfulDrop)
		{
			float successPercent = this.DropReport.GetSuccessPercent(true, true, true);
			if (this.GM)
			{
				if (this.GM.SuccessChances)
				{
					if (!string.IsNullOrEmpty(this.GM.SuccessChances.GetSuccessLabel(successPercent)))
					{
						text += string.Format("\n<size=75%>({0})</size>", this.GM.SuccessChances.GetSuccessLabel(successPercent));
					}
					else
					{
						text += string.Format("\n({0}%)", (successPercent * 100f).ToString("0"));
					}
				}
				else
				{
					text += string.Format("\n({0}%)", (successPercent * 100f).ToString("0"));
				}
			}
			else
			{
				text += string.Format("\n({0}%)", (successPercent * 100f).ToString("0"));
			}
		}
		if (!base.gameObject.activeSelf)
		{
			return false;
		}
		string tooltip = _Action.TooltipDescription(this.EnoughTime, this.DemoOK, this.DropReport.GetSuccessPercent(true, true, true), false, this.MissingDurabilities, this.BlockingStatus, this.MissingStats.ToArray(), this.MissingCards.ToArray(), this.MissingTags.ToArray());
		base.Setup(_Index, text, tooltip, _Highlighted);
		if (this.MissingIcons)
		{
			if (flag && this.EnoughTime && flag2)
			{
				if (flag3)
				{
					this.MissingIcons.gameObject.SetActive(false);
					return true;
				}
			}
			while (this.MissingIconImages.Count < this.MissingStats.Count + 1)
			{
				this.MissingIconImages.Add(UnityEngine.Object.Instantiate<Image>(this.MissingIconPrefab, this.MissingIcons));
			}
			bool flag5 = false;
			int num = 0;
			while (num < this.MissingIconImages.Count || num < this.MissingStats.Count)
			{
				if (num >= this.MissingStats.Count)
				{
					if (num == this.MissingStats.Count)
					{
						this.MissingIconImages[num].overrideSprite = null;
						this.MissingIconImages[num].gameObject.SetActive(!this.EnoughTime || !flag2);
					}
					else
					{
						this.MissingIconImages[num].gameObject.SetActive(false);
					}
				}
				else
				{
					this.MissingIconImages[num].overrideSprite = this.MissingStats[num].Stat.NotEnoughIcon;
					this.MissingIconImages[num].gameObject.SetActive(this.MissingStats[num].Stat.NotEnoughIcon != null);
					flag5 |= (this.MissingStats[num].Stat.NotEnoughIcon != null);
				}
				num++;
			}
			this.MissingIcons.gameObject.SetActive(flag5);
		}
		return true;
	}

	// Token: 0x060003DA RID: 986 RVA: 0x000280C0 File Offset: 0x000262C0
	public void ForceActionAvailable()
	{
		this.EnoughTime = true;
		this.MissingDurabilities = "";
		this.BlockingStatus = "";
		if (this.MissingStats != null)
		{
			this.MissingStats.Clear();
		}
		if (this.MissingCards != null)
		{
			this.MissingCards.Clear();
		}
		if (this.MissingTags != null)
		{
			this.MissingTags.Clear();
		}
		if (this.MissingIcons)
		{
			this.MissingIcons.gameObject.SetActive(false);
		}
		base.Interactable = true;
		if (this.StackActionButton)
		{
			this.StackActionButton.interactable = true;
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00028164 File Offset: 0x00026364
	public override void OnHoverEnter()
	{
		if (this.DropReport.FromCard != null && this.DropReport.FromAction != null)
		{
			this.DropReport = this.GM.GetCollectionDropsReport(this.DropReport.FromAction, this.DropReport.FromCard, false);
			string text = this.DropReport.FromAction.TooltipDescription(this.EnoughTime, this.DemoOK, this.DropReport.GetSuccessPercent(true, true, true), GameManager.PerformingAction && !base.Interactable, this.MissingDurabilities, this.BlockingStatus, this.MissingStats.ToArray(), this.MissingCards.ToArray(), this.MissingTags.ToArray());
			if (!string.IsNullOrEmpty(text))
			{
				base.SetTooltip("", text, "", -1);
			}
			else
			{
				base.CancelTooltip();
			}
			this.GM.AddHoveredDismantleAction(this.DropReport);
		}
		base.OnHoverEnter();
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00028263 File Offset: 0x00026463
	public override void OnHoverExit()
	{
		this.GM.RemoveHoveredDismantleAction(this.DropReport);
		base.OnHoverExit();
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0002827C File Offset: 0x0002647C
	protected override void OnDisable()
	{
		base.OnDisable();
		this.GM.RemoveHoveredDismantleAction(this.DropReport);
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00028295 File Offset: 0x00026495
	private void ClickStackAction()
	{
		Action<int> onStackButtonClicked = this.OnStackButtonClicked;
		if (onStackButtonClicked == null)
		{
			return;
		}
		onStackButtonClicked(base.Index);
	}

	// Token: 0x040004E9 RID: 1257
	public RectTransform MissingIcons;

	// Token: 0x040004EA RID: 1258
	public Image MissingIconPrefab;

	// Token: 0x040004EB RID: 1259
	private List<Image> MissingIconImages = new List<Image>();

	// Token: 0x040004EC RID: 1260
	[SerializeField]
	private GameObject StackActionObject;

	// Token: 0x040004ED RID: 1261
	[SerializeField]
	private Button StackActionButton;

	// Token: 0x040004EE RID: 1262
	[SerializeField]
	private TextMeshProUGUI StackActionText;

	// Token: 0x040004EF RID: 1263
	[SerializeField]
	private ButtonSounds[] SoundScripts;

	// Token: 0x040004F0 RID: 1264
	private bool EnoughTime;

	// Token: 0x040004F1 RID: 1265
	private bool DemoOK;

	// Token: 0x040004F2 RID: 1266
	private string MissingDurabilities;

	// Token: 0x040004F3 RID: 1267
	private string BlockingStatus;

	// Token: 0x040004F4 RID: 1268
	private List<MissingStatInfo> MissingStats = new List<MissingStatInfo>();

	// Token: 0x040004F5 RID: 1269
	private List<CardData> MissingCards = new List<CardData>();

	// Token: 0x040004F6 RID: 1270
	private List<CardTag> MissingTags = new List<CardTag>();

	// Token: 0x040004F7 RID: 1271
	private CollectionDropReport DropReport;

	// Token: 0x040004F8 RID: 1272
	private GameManager GM;

	// Token: 0x040004F9 RID: 1273
	public Action<int> OnStackButtonClicked;
}
