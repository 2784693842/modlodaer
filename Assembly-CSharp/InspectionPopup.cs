using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008B RID: 139
public class InspectionPopup : MonoBehaviour
{
	// Token: 0x17000128 RID: 296
	// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0003BBA3 File Offset: 0x00039DA3
	// (set) Token: 0x060005BA RID: 1466 RVA: 0x0003BBAB File Offset: 0x00039DAB
	public InGameCardBase CurrentCard { get; private set; }

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060005BB RID: 1467 RVA: 0x0003BBB4 File Offset: 0x00039DB4
	// (set) Token: 0x060005BC RID: 1468 RVA: 0x0003BBBC File Offset: 0x00039DBC
	public SpecialActionSet CurrentSet { get; private set; }

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060005BD RID: 1469 RVA: 0x0003BBC5 File Offset: 0x00039DC5
	// (set) Token: 0x060005BE RID: 1470 RVA: 0x0003BBCD File Offset: 0x00039DCD
	public bool Initialized { get; protected set; }

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x0003BBD6 File Offset: 0x00039DD6
	// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0003BBDE File Offset: 0x00039DDE
	public int SlotCount { get; private set; }

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0003BBE7 File Offset: 0x00039DE7
	// (set) Token: 0x060005C2 RID: 1474 RVA: 0x0003BBEF File Offset: 0x00039DEF
	public bool CannotCloseWindow { get; protected set; }

	// Token: 0x060005C3 RID: 1475 RVA: 0x0003BBF8 File Offset: 0x00039DF8
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x0003BC00 File Offset: 0x00039E00
	public void Init()
	{
		if (this.Initialized)
		{
			return;
		}
		this.InspectionSlot = new DynamicLayoutSlot(this.InspectionSlotSettings, this.InspectionSlotVisuals);
		this.InventorySlotModel = new DynamicLayoutSlot(this.InventorySlotSettings, null);
		if (this.InventorySlotsLine)
		{
			this.InventorySlotsLine.Init(this.InventorySlotsParent, this.InventorySlotSettings, this.InventoryScrollView);
		}
		this.GetManager();
		if (this.RenameInput)
		{
			this.RenameInputSounds = this.RenameInput.GetComponent<ButtonSounds>();
		}
		if (this.PopupTitle)
		{
			this.TitleRect = this.PopupTitle.rectTransform;
		}
		this.Initialized = true;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x0003BCB2 File Offset: 0x00039EB2
	protected virtual void GetManager()
	{
		if (!this.GrM)
		{
			this.GrM = MBSingleton<GraphicsManager>.Instance;
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x0003BCE4 File Offset: 0x00039EE4
	protected virtual void Update()
	{
		if (this.CurrentCard && this.CurrentCard.Destroyed && !this.CurrentSet)
		{
			this.Hide(false);
		}
		if (this.CloseButton)
		{
			this.CloseButton.SetActive(!this.CannotCloseWindow);
		}
		if (this.EmptyInventoryButton && this.CurrentCard)
		{
			this.EmptyInventoryButton.interactable = this.CurrentCard.HasInventoryContent;
		}
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x0003BD70 File Offset: 0x00039F70
	protected virtual void LateUpdate()
	{
		if (this.CloseOnClick && this.CloseOnClick.UpdateClickedOutside() && !this.CannotCloseWindow)
		{
			this.Hide(true);
		}
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x0003BD9B File Offset: 0x00039F9B
	public int GetIndex(DynamicLayoutSlot _Slot)
	{
		if (!this.InventorySlotsLine.Slots.Contains(_Slot))
		{
			return -1;
		}
		return this.InventorySlotsLine.Slots.IndexOf(_Slot);
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x0003BDC3 File Offset: 0x00039FC3
	public void MoveSlot(int _From, int _To)
	{
		this.InventorySlotsLine.MoveSlot(_From, _To);
		this.CurrentCard.UpdateInventory(this.InventorySlotsLine.Slots);
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0003BDE8 File Offset: 0x00039FE8
	public DynamicLayoutSlot GetSlot(int _Index)
	{
		return this.InventorySlotsLine.Slots[_Index];
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0003BDFC File Offset: 0x00039FFC
	public void SelectBookmark(int _Index)
	{
		if (!this.CurrentCard)
		{
			return;
		}
		this.GetManager();
		BookmarkGraphics bookmark = this.GrM.GetBookmark(this.CurrentCard.CardModel, this.CurrentCard.ContainedLiquidModel);
		if (_Index == -1)
		{
			if (bookmark)
			{
				bookmark.SetCard(null, null, true);
			}
		}
		else
		{
			if (bookmark)
			{
				bookmark.SetCard(null, null, false);
			}
			this.GrM.Bookmarks[_Index].SetCard(this.CurrentCard.CardModel, this.CurrentCard.ContainedLiquidModel, true);
			if (this.CurrentCard.CanBePinned && !this.GrM.GetGroupForCard(this.CurrentCard.CardModel))
			{
				if (this.CurrentCard.CurrentSlot)
				{
					this.CurrentCard.CurrentSlot.PinCurrentCard(false);
				}
				if (this.PinButton)
				{
					this.PinButton.isOn = this.CurrentCard.CurrentSlot.PinnedCard;
				}
			}
			this.SetBookmarkButtons[_Index].UpdateVisuals();
		}
		this.UpdateBookmarkIcon();
		this.SelectBookmarkPopup.SetActive(false);
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x0003BF34 File Offset: 0x0003A134
	private void UpdateBookmarkIcon()
	{
		if (!this.CurrentCard)
		{
			if (this.CurrentBookmarkImage)
			{
				this.CurrentBookmarkImage.color = Color.white;
			}
			if (this.CurrentBookmarkNumber)
			{
				this.CurrentBookmarkNumber.text = "X";
				this.CurrentBookmarkNumber.color = this.NullBookmarkTextColor;
			}
			return;
		}
		BookmarkGraphics bookmark = this.GrM.GetBookmark(this.CurrentCard.CardModel, this.CurrentCard.ContainedLiquidModel);
		if (!bookmark)
		{
			if (this.CurrentBookmarkImage)
			{
				this.CurrentBookmarkImage.color = Color.white;
			}
			if (this.CurrentBookmarkNumber)
			{
				this.CurrentBookmarkNumber.text = "X";
				this.CurrentBookmarkNumber.color = this.NullBookmarkTextColor;
			}
		}
		else
		{
			if (this.CurrentBookmarkImage)
			{
				this.CurrentBookmarkImage.color = bookmark.BookmarkColor;
			}
			if (this.CurrentBookmarkNumber)
			{
				this.CurrentBookmarkNumber.text = (bookmark.GetIndex + 1).ToString();
				this.CurrentBookmarkNumber.color = this.BookmarkTextColor;
			}
		}
		if (this.BookmarksObjects.Length != 0)
		{
			GraphicsManager.SetActiveGroup(this.BookmarksObjects, this.CurrentCard.CardModel.CanBookmark && this.CurrentCard.CurrentSlotInfo.SlotType != SlotsTypes.Blueprint);
			this.SelectBookmarkPopup.SetActive(false);
			if (this.SetBookmarkButtons.Count == 0)
			{
				this.SetBookmarkButtons.Add(UnityEngine.Object.Instantiate<SetBookmarkButton>(this.SelectBookmarkPrefab, this.SelectBookmarksParent));
				this.SetBookmarkButtons[0].Setup(-1, "X", null, false);
				this.SetBookmarkButtons[0].TextColor = this.NullBookmarkTextColor;
				SetBookmarkButton setBookmarkButton = this.SetBookmarkButtons[0];
				setBookmarkButton.OnClicked = (Action<int>)Delegate.Combine(setBookmarkButton.OnClicked, new Action<int>(this.SelectBookmark));
			}
			int num = 1;
			while (num < this.SetBookmarkButtons.Count || num - 1 < this.GrM.Bookmarks.Length)
			{
				if (num >= this.SetBookmarkButtons.Count)
				{
					this.SetBookmarkButtons.Add(UnityEngine.Object.Instantiate<SetBookmarkButton>(this.SelectBookmarkPrefab, this.SelectBookmarksParent));
					this.SetBookmarkButtons[num].Setup(num - 1, num.ToString(), null, false);
					SetBookmarkButton setBookmarkButton2 = this.SetBookmarkButtons[num];
					setBookmarkButton2.OnClicked = (Action<int>)Delegate.Combine(setBookmarkButton2.OnClicked, new Action<int>(this.SelectBookmark));
				}
				this.SetBookmarkButtons[num].UpdateVisuals();
				this.SetBookmarkButtons[num].gameObject.SetActive(num - 1 < this.GrM.Bookmarks.Length);
				num++;
			}
			for (int i = 0; i < this.SetBookmarkButtons.Count; i++)
			{
				if (this.SetBookmarkButtons[i].LinkedBookmark == bookmark)
				{
					this.SetBookmarkButtons[i].transform.SetAsFirstSibling();
				}
				else
				{
					this.SetBookmarkButtons[i].transform.SetAsLastSibling();
				}
			}
		}
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x0003C278 File Offset: 0x0003A478
	public void ResetRenameInput()
	{
		if (this.RenamePopup)
		{
			this.RenamePopup.SetActive(false);
		}
		if (this.RenameInput)
		{
			if (this.RenameInputSounds)
			{
				this.RenameInputSounds.MuteSpecialSounds = true;
			}
			this.RenameInput.text = (this.CurrentCard ? this.CurrentCard.CardName(true) : "");
			if (this.RenameInputSounds)
			{
				this.RenameInputSounds.MuteSpecialSounds = false;
			}
		}
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x0003C308 File Offset: 0x0003A508
	public void SetupSingleAction(InGameCardBase _Card, CardAction _Action, string _Message)
	{
		if (_Card == null || _Card == this.CurrentCard)
		{
			return;
		}
		if (this.PinButton)
		{
			this.PinButton.gameObject.SetActive(false);
		}
		if (this.TrashButton)
		{
			this.TrashButton.gameObject.SetActive(false);
		}
		if (this.HelpButton)
		{
			this.HelpButton.SetActive(false);
		}
		if (this.BookmarksObjects != null && this.BookmarksObjects.Length != 0)
		{
			GraphicsManager.SetActiveGroup(this.BookmarksObjects, false);
		}
		if (this.SelectBookmarkPopup)
		{
			this.SelectBookmarkPopup.SetActive(false);
		}
		this.CommonSetup(_Card.CardName(false), _Message);
		if (this.LiquidFillingText)
		{
			this.LiquidFillingText.gameObject.SetActive(false);
		}
		if (this.RenameButton)
		{
			this.RenameButton.SetActive(false);
		}
		this.CannotCloseWindow = true;
		this.InspectionSlot.IsActive = false;
		this.CurrentSet = null;
		this.CurrentCard = _Card;
		this.InspectionSlot.IsActive = true;
		if (_Card.CurrentSlot == null)
		{
			this.InspectionSlot.AssignCard(_Card, false);
		}
		else if (_Card.CurrentSlot != this.InspectionSlot)
		{
			_Card.SetInspectionParent(this.InspectionSlot.GetParent, true, true);
		}
		this.SetupSingleActionButton(_Action);
		this.CenterTitleText();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0003C47C File Offset: 0x0003A67C
	private void SetupSingleActionButton(CardAction _Action)
	{
		for (int i = 0; i < this.OptionsButtons.Count; i++)
		{
			this.OptionsButtons[i].gameObject.SetActive(false);
		}
		if (!this.SingleActionButton)
		{
			this.SingleActionButton = UnityEngine.Object.Instantiate<DismantleActionButton>(this.ButtonPrefab, this.DismantleOptionsParent);
			DismantleActionButton singleActionButton = this.SingleActionButton;
			singleActionButton.OnClicked = (Action<int>)Delegate.Combine(singleActionButton.OnClicked, new Action<int>(delegate(int a)
			{
				base.gameObject.SetActive(false);
			}));
		}
		else
		{
			this.SingleActionButton.gameObject.SetActive(true);
		}
		this.SingleActionButton.Setup(LocalizedString.Confirm, null, false);
		this.CenterTitleText();
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0003C534 File Offset: 0x0003A734
	public void Setup(InGameCardBase _Card)
	{
		if (_Card == null || _Card == this.CurrentCard)
		{
			return;
		}
		this.GetManager();
		if (this.PinButton)
		{
			this.PinButton.gameObject.SetActive(_Card.CanBePinned);
			if (_Card.CurrentSlot)
			{
				this.PinButton.isOn = _Card.CurrentSlot.PinnedCard;
			}
		}
		if (this.HelpButton)
		{
			this.HelpButton.SetActive(GuideManager.GetPageFor(_Card) != null);
		}
		this.CannotCloseWindow = (_Card.CardModel.CardType == CardTypes.Event);
		this.CommonSetup(_Card.CardName(false), _Card.CardDescription(false));
		if (this.LiquidFillingText)
		{
			if (_Card.ContainedLiquid)
			{
				this.LiquidFillingText.gameObject.SetActive(_Card.ContainedLiquid.IsFillingWithLiquid);
				this.LiquidFillingText.text = LocalizedString.LiquidAutoFilling(_Card.ContainedLiquid.CardName(false));
			}
			else if (_Card.CardModel)
			{
				if (_Card.CardModel.CardType == CardTypes.Liquid)
				{
					this.LiquidFillingText.gameObject.SetActive(_Card.IsFillingWithLiquid);
					this.LiquidFillingText.text = LocalizedString.LiquidAutoFilling(_Card.CardName(false));
				}
				else
				{
					this.LiquidFillingText.gameObject.SetActive(false);
				}
			}
			else
			{
				this.LiquidFillingText.gameObject.SetActive(false);
			}
		}
		this.CurrentSet = null;
		this.CurrentCard = _Card;
		if (this.RenameButton)
		{
			this.RenameButton.SetActive(_Card.CardModel.CanBeRenamed);
		}
		this.ResetRenameInput();
		if (this.SingleActionButton)
		{
			this.SingleActionButton.gameObject.SetActive(false);
		}
		this.InspectionSlot.IsActive = true;
		if (_Card.CurrentSlot == null)
		{
			this.InspectionSlot.AssignCard(_Card, false);
		}
		else if (_Card.CurrentSlot != this.InspectionSlot)
		{
			_Card.SetInspectionParent(this.InspectionSlot.GetParent, true, false);
		}
		if (_Card.IsPinned)
		{
			this.SetupActions(null, false);
		}
		else
		{
			List<DismantleCardAction> list = new List<DismantleCardAction>();
			if (_Card.ContainedLiquid && _Card.ContainedLiquid.DismantleActions != null && _Card.ContainedLiquid.DismantleActions.Length != 0)
			{
				list.AddRange(_Card.ContainedLiquid.DismantleActions);
				if (_Card.CardModel.CannotEmpty && list[list.Count - 1].ActionName == LocalizedString.Empty)
				{
					list.RemoveAt(list.Count - 1);
				}
			}
			if (_Card.DismantleActions != null && _Card.DismantleActions.Length != 0)
			{
				list.AddRange(_Card.DismantleActions);
			}
			this.SetupActions(list.ToArray(), false);
		}
		this.SetupInventory(_Card, true);
		if (this.TrashButton)
		{
			this.TrashButton.gameObject.SetActive(this.ShowTrashButton);
		}
		if (this.TrashConfirmText)
		{
			this.TrashConfirmText.text = LocalizedString.TrashConfirmText(_Card.CardModel);
		}
		if (this.BookmarksObjects != null)
		{
			this.UpdateBookmarkIcon();
		}
		if (this.ThrowAllButton)
		{
			bool interactable;
			if (this.CurrentCard.CurrentSlot.PinnedCard)
			{
				interactable = (this.CurrentCard.CurrentSlot.CardPileCount(true) > 2);
			}
			else
			{
				interactable = (this.CurrentCard.CurrentSlot.CardPileCount(true) > 1);
			}
			this.ThrowAllButton.interactable = interactable;
		}
		this.CenterTitleText();
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0003C8D4 File Offset: 0x0003AAD4
	public void Setup(SpecialActionSet _ActionSet)
	{
		if (_ActionSet == null || _ActionSet == this.CurrentSet)
		{
			return;
		}
		if (this.LiquidFillingText)
		{
			this.LiquidFillingText.gameObject.SetActive(false);
		}
		if (this.PinButton)
		{
			this.PinButton.gameObject.SetActive(false);
		}
		if (this.TrashButton)
		{
			this.TrashButton.gameObject.SetActive(false);
		}
		if (this.RenameButton)
		{
			this.RenameButton.SetActive(false);
		}
		if (this.HelpButton)
		{
			this.HelpButton.SetActive(false);
		}
		if (this.BookmarksObjects != null && this.BookmarksObjects.Length != 0)
		{
			GraphicsManager.SetActiveGroup(this.BookmarksObjects, false);
		}
		if (this.SelectBookmarkPopup)
		{
			this.SelectBookmarkPopup.SetActive(false);
		}
		this.CommonSetup(_ActionSet.SetName, _ActionSet.Description);
		this.CannotCloseWindow = false;
		if (this.SingleActionButton)
		{
			this.SingleActionButton.gameObject.SetActive(false);
		}
		this.InspectionSlot.IsActive = false;
		this.CurrentCard = null;
		this.CurrentSet = _ActionSet;
		this.SetupActions(_ActionSet.Actions, true);
		this.CenterTitleText();
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0003CA2C File Offset: 0x0003AC2C
	protected void CommonSetup(string _Title, string _Text)
	{
		this.EmptyingInventory = false;
		if (this.ConfirmPopup)
		{
			this.ConfirmPopup.gameObject.SetActive(false);
		}
		if (this.TrashConfirm)
		{
			this.TrashConfirm.SetActive(false);
		}
		if (this.RenamePopup)
		{
			this.RenamePopup.SetActive(false);
		}
		this.ClearCurrentCard(true);
		if (this.DescriptionText)
		{
			this.DescriptionText.text = _Text;
		}
		if (this.PopupTitle)
		{
			this.PopupTitle.text = _Title;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0003CAD8 File Offset: 0x0003ACD8
	protected void CenterTitleText()
	{
		if (!this.TitleRect)
		{
			return;
		}
		float num = 0f;
		float num2 = this.TitleRect.transform.parent.GetComponent<RectTransform>().rect.width;
		if (this.TitleBarLeftButtons != null && this.TitleBarLeftButtons.Length != 0)
		{
			for (int i = 0; i < this.TitleBarLeftButtons.Length; i++)
			{
				if (this.TitleBarLeftButtons[i] && this.TitleBarLeftButtons[i].gameObject.activeInHierarchy && this.TitleBarLeftButtons[i].localPosition.x + this.TitleBarLeftButtons[i].rect.xMax > num)
				{
					num = this.TitleBarLeftButtons[i].localPosition.x + this.TitleBarLeftButtons[i].rect.xMax;
				}
			}
		}
		if (this.TitleBarRightButtons != null && this.TitleBarRightButtons.Length != 0)
		{
			for (int j = 0; j < this.TitleBarRightButtons.Length; j++)
			{
				if (this.TitleBarRightButtons[j] && this.TitleBarRightButtons[j].gameObject.activeInHierarchy && this.TitleBarRightButtons[j].localPosition.x + this.TitleBarRightButtons[j].rect.xMin < num2)
				{
					num2 = this.TitleBarRightButtons[j].localPosition.x + this.TitleBarRightButtons[j].rect.xMin;
				}
			}
		}
		num += 10f;
		num2 -= num;
		num2 -= 10f;
		this.TitleRect.localPosition = new Vector3(num, 0f, 0f);
		this.TitleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num2);
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0003CCB0 File Offset: 0x0003AEB0
	protected virtual void SetupActions(DismantleCardAction[] _Actions, bool _CountTimeSpentAsEffect)
	{
		if (_Actions != null)
		{
			while (this.OptionsButtons.Count < _Actions.Length)
			{
				this.OptionsButtons.Add(UnityEngine.Object.Instantiate<DismantleActionButton>(this.ButtonPrefab, this.DismantleOptionsParent));
				DismantleActionButton dismantleActionButton = this.OptionsButtons[this.OptionsButtons.Count - 1];
				dismantleActionButton.OnClicked = (Action<int>)Delegate.Combine(dismantleActionButton.OnClicked, new Action<int>(this.OnMainButtonClicked));
				DismantleActionButton dismantleActionButton2 = this.OptionsButtons[this.OptionsButtons.Count - 1];
				dismantleActionButton2.OnStackButtonClicked = (Action<int>)Delegate.Combine(dismantleActionButton2.OnStackButtonClicked, new Action<int>(this.OnStackButtonClicked));
			}
		}
		InGameCardBase inGameCardBase = this.CurrentCard;
		int num = -1;
		List<string> list = new List<string>();
		if (this.CurrentCard && this.CurrentCard.ContainedLiquid)
		{
			num = (this.CurrentCard.CardModel.CannotEmpty ? (this.CurrentCard.ContainedLiquid.DismantleActions.Length - 1) : this.CurrentCard.ContainedLiquid.DismantleActions.Length);
		}
		bool flag = false;
		for (int i = 0; i < this.OptionsButtons.Count; i++)
		{
			if (_Actions == null)
			{
				this.OptionsButtons[i].gameObject.SetActive(false);
			}
			else if (i >= _Actions.Length)
			{
				this.OptionsButtons[i].gameObject.SetActive(false);
			}
			else
			{
				if (num != -1)
				{
					inGameCardBase = ((i < num) ? this.CurrentCard.ContainedLiquid : this.CurrentCard);
				}
				bool flag2 = !_Actions[i].CanAppear(inGameCardBase, _CountTimeSpentAsEffect, this.InventorySlotSettings) || list.Contains(_Actions[i].ActionName.DefaultText);
				if (this.CurrentCard)
				{
					if ((this.CurrentCard.CardModel.CardType == CardTypes.Blueprint || this.CurrentCard.CardModel.CardType == CardTypes.EnvDamage) && !_Actions[i].PerformUponInspection)
					{
						flag2 = false;
					}
					else if (this.CurrentCard.CardModel.CardType == CardTypes.EnvImprovement && !_Actions[i].PerformUponInspection && !flag2)
					{
						flag2 = !this.CurrentCard.BlueprintComplete;
					}
				}
				if (flag2)
				{
					this.OptionsButtons[i].gameObject.SetActive(false);
				}
				else
				{
					if (!_Actions[i].PerformUponInspection)
					{
						bool flag3 = _Actions[i].StackCompatible && this.CurrentCard;
						if (flag3)
						{
							if (this.CurrentCard.CurrentSlot.PinnedCard)
							{
								flag3 = (this.CurrentCard.CurrentSlot.CardPileCount(true) > 2);
							}
							else
							{
								flag3 = (this.CurrentCard.CurrentSlot.CardPileCount(true) > 1);
							}
						}
						if (this.OptionsButtons[i].Setup(i, _Actions[i], inGameCardBase, MBSingleton<TutorialManager>.Instance.IsActionHighlighted(inGameCardBase ? inGameCardBase.CardModel : this.CurrentSet, _Actions[i]), flag3))
						{
							list.Add(_Actions[i].ActionName);
						}
						if (this.OptionsButtons[i].Interactable)
						{
							flag = true;
						}
					}
					else
					{
						this.OptionsButtons[i].gameObject.SetActive(false);
					}
					if (_Actions[i].PerformUponInspection)
					{
						List<MissingStatInfo> list2 = new List<MissingStatInfo>();
						List<CardData> list3 = new List<CardData>();
						List<CardTag> list4 = new List<CardTag>();
						string missingDurabilities = "";
						string blockingStatus = "";
						bool isNotCancelledByDemo = _Actions[i].IsNotCancelledByDemo;
						bool flag4 = _Actions[i].WillHaveAnEffect(inGameCardBase, false, false, false, false, Array.Empty<CardModifications>());
						bool flag5 = _Actions[i].StatsAreCorrect(list2, false);
						bool flag6 = _Actions[i].CardsAndTagsAreCorrect(inGameCardBase, list3, list4);
						bool flag7 = _Actions[i].DurabilitiesAreCorrect(inGameCardBase, out missingDurabilities);
						this.GM.CheckActionBlockers(_Actions[i], out blockingStatus);
						bool flag8 = _Actions[i].EnoughDaylightPoints();
						if (flag4 && flag5 && flag6 && flag7 && flag8 && isNotCancelledByDemo)
						{
							GameManager.PerformAction(_Actions[i], inGameCardBase, false);
						}
						else if (!flag5 || !isNotCancelledByDemo)
						{
							MBSingleton<GraphicsManager>.Instance.ShowImpossibleToInspect(inGameCardBase, _Actions[i].TooltipDescription(flag8, isNotCancelledByDemo, -1f, GameManager.PerformingAction, missingDurabilities, blockingStatus, list2.ToArray(), list3.ToArray(), list4.ToArray()));
						}
						if (!_Actions[i].DontCloseInspectionWindow)
						{
							this.Hide(!_Actions[i].HasActionSounds);
							return;
						}
					}
				}
			}
		}
		if (!flag && this.CurrentCard && _Actions != null && this.CurrentCard.CardModel.CardType == CardTypes.Event && _Actions.Length != 0)
		{
			this.OptionsButtons[0].ForceActionAvailable();
		}
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0003D174 File Offset: 0x0003B374
	public void RefreshInventory()
	{
		if (this.CurrentCard && !this.EmptyingInventory)
		{
			this.SetupInventory(this.CurrentCard, false);
		}
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0003D198 File Offset: 0x0003B398
	protected virtual void SetupInventory(InGameCardBase _Card, bool _ResetView)
	{
		if (!this.InventorySlotSettings)
		{
			return;
		}
		if (_ResetView)
		{
			this.InventorySlotsLine.MoveToPos(0f);
		}
		int num = _Card.IsLegacyInventory ? _Card.CardsInInventory.Count : (_Card.CardsInInventory.Count + 1);
		while (this.InventorySlotsLine.Slots.Count < num)
		{
			this.InventorySlotsLine.AddSlot(this.InventorySlotsLine.Slots.Count);
		}
		for (int i = 0; i < this.InventorySlotsLine.Slots.Count; i++)
		{
			this.InventorySlotsLine.Slots[i].ClearFilters();
			this.InventorySlotsLine.Slots[i].AddFilter(_Card.CardModel.CompleteInventoryFilter);
			this.InventorySlotsLine.Slots[i].UpdateText(_Card.CardModel.GetInventorySlotsText);
			this.InventorySlotsLine.Slots[i].Index = i;
			this.InventorySlotsLine.Slots[i].IsActive = (i < num);
			this.InventorySlotsLine.Slots[i].UpdateCookingProgress(0f, 0, false, "", false);
			this.InventorySlotsLine.Slots[i].MaxPileCount = 0;
			this.InventorySlotsLine.Slots[i].CanHostPile = (!_Card.IsLegacyInventory || _Card.CardModel.CardType == CardTypes.Blueprint || _Card.CardModel.CardType == CardTypes.Explorable || _Card.CardModel.CardType == CardTypes.EnvImprovement || _Card.CardModel.CardType == CardTypes.EnvDamage);
		}
		_Card.SortInventory();
		if (_Card.IsCooking() || _Card.CookingIsPaused())
		{
			for (int j = 0; j < _Card.CookingCards.Count; j++)
			{
				if (_Card.CookingCards[j].Card == null && _Card.CookingCards[j].CardIndex >= 0 && _Card.CookingCards[j].CardIndex < this.InventorySlotsLine.Slots.Count)
				{
					this.InventorySlotsLine.Slots[_Card.CookingCards[j].CardIndex].UpdateCookingProgress((float)_Card.CookingCards[j].CookedDuration / (float)_Card.CookingCards[j].TargetDuration, _Card.CookingCards[j].TargetDuration - _Card.CookingCards[j].CookedDuration, _Card.CookingIsPaused(), _Card.CookingCards[j].GetCookingText(_Card.CookingIsPaused(), _Card.CookingCards[j].TargetDuration - _Card.CookingCards[j].CookedDuration), _Card.CookingCards[j].HideCookingProgress);
				}
			}
		}
		for (int k = 0; k < _Card.CardsInInventory.Count; k++)
		{
			if (_Card.CardsInInventory[k] != null && !_Card.CardsInInventory[k].IsFree)
			{
				for (int l = 0; l < _Card.CardsInInventory[k].CardAmt; l++)
				{
					this.InventorySlotsLine.Slots[_Card.CardsInInventory[k].AllCards[l].CurrentSlotInfo.SlotIndex].AssignCard(_Card.CardsInInventory[k].AllCards[l], true);
				}
			}
		}
		this.SlotCount = num;
		this.RefreshVisibleSlots();
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0003D563 File Offset: 0x0003B763
	public void SortInventory()
	{
		if (this.CurrentCard && this.CurrentCard.HasInventoryContent)
		{
			this.SetupInventory(this.CurrentCard, false);
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0003D58C File Offset: 0x0003B78C
	public void UpdateCookingProgress()
	{
		for (int i = 0; i < this.InventorySlotsLine.Slots.Count; i++)
		{
			this.InventorySlotsLine.Slots[i].UpdateCookingProgress(0f, 0, false, "", false);
		}
		if (!this.CurrentCard)
		{
			return;
		}
		if (this.CurrentCard.IsCooking() || this.CurrentCard.CookingIsPaused())
		{
			for (int j = 0; j < this.CurrentCard.CookingCards.Count; j++)
			{
				if (this.CurrentCard.CookingCards[j].Card == null && this.CurrentCard.CookingCards[j].CardIndex >= 0 && this.CurrentCard.CookingCards[j].CardIndex < this.InventorySlotsLine.Slots.Count)
				{
					this.InventorySlotsLine.Slots[this.CurrentCard.CookingCards[j].CardIndex].UpdateCookingProgress((float)this.CurrentCard.CookingCards[j].CookedDuration / (float)this.CurrentCard.CookingCards[j].TargetDuration, this.CurrentCard.CookingCards[j].TargetDuration - this.CurrentCard.CookingCards[j].CookedDuration, this.CurrentCard.CookingIsPaused(), this.CurrentCard.CookingCards[j].GetCookingText(this.CurrentCard.CookingIsPaused(), this.CurrentCard.CookingCards[j].TargetDuration - this.CurrentCard.CookingCards[j].CookedDuration), this.CurrentCard.CookingCards[j].HideCookingProgress);
				}
			}
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0003D784 File Offset: 0x0003B984
	private void RefreshVisibleSlots()
	{
		this.VisibleSlots.Clear();
		for (int i = 0; i < this.SlotCount; i++)
		{
			this.VisibleSlots.Add(this.InventorySlotsLine.Slots[i]);
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0003D7CC File Offset: 0x0003B9CC
	public void Hide(bool _PlaySound)
	{
		MBSingleton<GraphicsManager>.Instance.ClearInspectedCard();
		if (this.InventorySlotsLine)
		{
			for (int i = 0; i < this.InventorySlotsLine.Slots.Count; i++)
			{
				this.InventorySlotsLine.Slots[i].ClearSlot(false);
			}
		}
		this.ClearCurrentCard(_PlaySound);
		this.CurrentSet = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0003D83C File Offset: 0x0003BA3C
	private void ClearCurrentCard(bool _PlaySound)
	{
		if (this.CurrentCard)
		{
			if (this.CurrentCard.CardModel && (this.CurrentCard.CardModel.CardType == CardTypes.Event || this.CurrentCard.Destroyed))
			{
				this.CurrentCard.SetParent(MBSingleton<GraphicsManager>.Instance.CardsMovingParent, false);
			}
			if (!this.CurrentCard.Destroyed && this.CurrentCard.CurrentSlot != this.InspectionSlot)
			{
				this.CurrentCard.SetInspectionParent(null, _PlaySound, false);
			}
			if (this.CurrentCard.CardsInInventory != null)
			{
				for (int i = 0; i < this.CurrentCard.CardsInInventory.Count; i++)
				{
					if (this.CurrentCard.CardsInInventory[i] != null && !this.CurrentCard.CardsInInventory[i].IsFree)
					{
						for (int j = 0; j < this.CurrentCard.CardsInInventory[i].CardAmt; j++)
						{
							if (this.InventorySlotSettings)
							{
								this.CurrentCard.CardsInInventory[i].AllCards[j].CurrentSlotInfo = new SlotInfo(SlotsTypes.Inventory, this.InventorySlotsLine.Slots.IndexOf(this.CurrentCard.CardsInInventory[i].AllCards[j].CurrentSlot));
								this.CurrentCard.CardsInInventory[i].AllCards[j].SetSlot(null, true);
							}
							this.CurrentCard.CardsInInventory[i].AllCards[j].UpdateVisibility();
						}
					}
				}
			}
			this.CurrentCard = null;
		}
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0003DA0C File Offset: 0x0003BC0C
	public void RenameCurrentCard()
	{
		if (this.RenamePopup)
		{
			this.RenamePopup.SetActive(false);
		}
		if (!this.CurrentCard)
		{
			return;
		}
		this.CurrentCard.SetCustomName(this.RenameInput.text);
		if (string.IsNullOrEmpty(this.RenameInput.text))
		{
			if (this.RenameInputSounds)
			{
				this.RenameInputSounds.MuteSpecialSounds = true;
			}
			this.RenameInput.text = this.CurrentCard.CardName(true);
			if (this.RenameInputSounds)
			{
				this.RenameInputSounds.MuteSpecialSounds = false;
			}
		}
		this.PopupTitle.text = this.CurrentCard.CardName(false);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0003DAC8 File Offset: 0x0003BCC8
	public void ClickHelp()
	{
		ContentPage pageFor = GuideManager.GetPageFor(this.CurrentCard);
		if (pageFor)
		{
			this.GM.OpenGuide(pageFor);
		}
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x0003DAF8 File Offset: 0x0003BCF8
	public void TrashCurrentCard(bool _Stack)
	{
		if (!this.CurrentCard)
		{
			return;
		}
		if (!_Stack)
		{
			GameManager.PerformAction(this.CurrentCard.CardModel.DefaultDiscardAction, this.CurrentCard, false);
		}
		else
		{
			GameManager.PerformStackAction(this.CurrentCard.CardModel.DefaultDiscardAction, this.CurrentCard.CurrentSlot, false);
		}
		this.Hide(true);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0003DB60 File Offset: 0x0003BD60
	public void EmptyInventory()
	{
		if (!this.CurrentCard)
		{
			return;
		}
		this.EmptyingInventory = true;
		if (this.CurrentCard.CardsInInventory.Count != 0)
		{
			for (int i = this.CurrentCard.CardsInInventory.Count - 1; i >= 0; i--)
			{
				if (this.CurrentCard.CardsInInventory[i] != null && !this.CurrentCard.CardsInInventory[i].IsFree)
				{
					for (int j = this.CurrentCard.CardsInInventory[i].CardAmt - 1; j >= 0; j--)
					{
						if (this.CurrentCard.CardsInInventory[i].CardModel)
						{
							this.GrM.MoveCardToSlot(this.CurrentCard.CardsInInventory[i].AllCards[j], (this.CurrentCard.CurrentSlotInfo.SlotType == SlotsTypes.Base) ? this.CurrentCard.CurrentSlotInfo : new SlotInfo(SlotsTypes.Base, -2), false, false);
						}
					}
				}
			}
		}
		this.EmptyingInventory = false;
		this.Hide(true);
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0003DC8A File Offset: 0x0003BE8A
	private void OnMainButtonClicked(int _Index)
	{
		this.OnButtonClicked(_Index, false);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0003DC94 File Offset: 0x0003BE94
	private void OnStackButtonClicked(int _Index)
	{
		this.OnButtonClicked(_Index, true);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0003DCA0 File Offset: 0x0003BEA0
	public virtual void OnButtonClicked(int _Index, bool _Stack)
	{
		InspectionPopup.<>c__DisplayClass99_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1._Index = _Index;
		CS$<>8__locals1._Stack = _Stack;
		if (this.CurrentCard)
		{
			int num = CS$<>8__locals1._Index;
			bool flag = false;
			if (this.CurrentCard.ContainedLiquid)
			{
				if (num < this.CurrentCard.ContainedLiquid.DismantleActions.Length - 1 || !this.CurrentCard.CardModel.CannotEmpty)
				{
					if (!this.<OnButtonClicked>g__DoCardAction|99_0(this.CurrentCard.ContainedLiquid, num, ref CS$<>8__locals1))
					{
						num -= this.CurrentCard.ContainedLiquid.DismantleActions.Length;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					num -= this.CurrentCard.ContainedLiquid.DismantleActions.Length - 1;
				}
			}
			if (!flag)
			{
				this.<OnButtonClicked>g__DoCardAction|99_0(this.CurrentCard, num, ref CS$<>8__locals1);
				return;
			}
		}
		else if (this.CurrentSet && this.CurrentSet.Actions.Length > CS$<>8__locals1._Index)
		{
			if (this.CurrentSet.Actions[CS$<>8__locals1._Index].ConfirmPopup && this.ConfirmPopup && !this.ConfirmPopup.gameObject.activeInHierarchy)
			{
				this.ConfirmPopup.Setup(this.CurrentSet.Actions[CS$<>8__locals1._Index].ActionName, HoursDisplay.HoursToCompleteString(GameManager.TickToHours(this.CurrentSet.Actions[CS$<>8__locals1._Index].TotalDaytimeCost, this.CurrentSet.Actions[CS$<>8__locals1._Index].MiniTicksCost)), CS$<>8__locals1._Index, CS$<>8__locals1._Stack, this);
				this.ConfirmPopup.gameObject.SetActive(true);
				return;
			}
			GameManager.PerformAction(this.CurrentSet.Actions[CS$<>8__locals1._Index], this.CurrentCard, false);
			if (!this.CurrentSet.Actions[CS$<>8__locals1._Index].DontCloseInspectionWindow)
			{
				this.Hide(false);
			}
		}
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0003DE9C File Offset: 0x0003C09C
	public void OnPinButtonClicked(bool _NewValue)
	{
		if (!this.CurrentCard)
		{
			return;
		}
		if (!this.CurrentCard.CurrentSlot)
		{
			return;
		}
		if (_NewValue == this.CurrentCard.CurrentSlot.PinnedCard)
		{
			return;
		}
		if (!this.CurrentCard.CurrentSlot.PinnedCard)
		{
			this.CurrentCard.CurrentSlot.PinCurrentCard(true);
			return;
		}
		this.CurrentCard.CurrentSlot.FreePin();
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0003DF1C File Offset: 0x0003C11C
	public bool ShowTrashButton
	{
		get
		{
			return this.CurrentCard && !this.HideTrashButton && ((CheatsManager.CanDeleteAllCards && MBSingleton<CheatsManager>.Instance.CheatsActive) || ((this.CurrentCard.CardModel.CardType == CardTypes.Base || this.CurrentCard.CardModel.CardType == CardTypes.Item) && !this.CurrentCard.CardModel.CannotBeTrashed && !this.CurrentCard.IsPinned));
		}
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0003DFC8 File Offset: 0x0003C1C8
	[CompilerGenerated]
	private bool <OnButtonClicked>g__DoCardAction|99_0(InGameCardBase _Card, int _ActionIndex, ref InspectionPopup.<>c__DisplayClass99_0 A_3)
	{
		if (_Card.DismantleActions.Length <= _ActionIndex)
		{
			return false;
		}
		if (_Card.DismantleActions[_ActionIndex].ConfirmPopup && this.ConfirmPopup && !this.ConfirmPopup.gameObject.activeInHierarchy)
		{
			this.ConfirmPopup.Setup(_Card.DismantleActions[_ActionIndex].ActionName, HoursDisplay.HoursToCompleteString(GameManager.TickToHours(_Card.DismantleActions[_ActionIndex].TotalDaytimeCost, _Card.DismantleActions[_ActionIndex].MiniTicksCost)), A_3._Index, A_3._Stack, this);
			this.ConfirmPopup.gameObject.SetActive(true);
			return true;
		}
		if (!A_3._Stack)
		{
			GameManager.PerformAction(_Card.DismantleActions[_ActionIndex], _Card, false);
		}
		else
		{
			GameManager.PerformStackAction(_Card.DismantleActions[_ActionIndex], this.CurrentCard.CurrentSlot, _Card == this.CurrentCard.ContainedLiquid);
		}
		if (_Card.DismantleActions[_ActionIndex].ReceivingCardChanges.ModType == CardModifications.Destroy || _Card.DismantleActions[_ActionIndex].ReceivingCardChanges.ModType == CardModifications.Transform || !_Card.DismantleActions[_ActionIndex].DontCloseInspectionWindow)
		{
			if (_Card.CardModel.CardType == CardTypes.Blueprint)
			{
				MBSingleton<GraphicsManager>.Instance.BlueprintModelsPopup.Hide();
			}
			this.Hide(!_Card.DismantleActions[_ActionIndex].HasActionSounds);
		}
		return true;
	}

	// Token: 0x040007AD RID: 1965
	public DismantleActionButton ButtonPrefab;

	// Token: 0x040007AE RID: 1966
	public TextMeshProUGUI PopupTitle;

	// Token: 0x040007AF RID: 1967
	public TextMeshProUGUI DescriptionText;

	// Token: 0x040007B0 RID: 1968
	public TextMeshProUGUI LiquidFillingText;

	// Token: 0x040007B1 RID: 1969
	public RectTransform DismantleOptionsParent;

	// Token: 0x040007B2 RID: 1970
	public DynamicLayoutSlot InspectionSlot;

	// Token: 0x040007B3 RID: 1971
	public CardSlot InspectionSlotVisuals;

	// Token: 0x040007B4 RID: 1972
	public SlotSettings InspectionSlotSettings;

	// Token: 0x040007B5 RID: 1973
	public DynamicLayoutSlot InventorySlotModel;

	// Token: 0x040007B6 RID: 1974
	public SlotSettings InventorySlotSettings;

	// Token: 0x040007B7 RID: 1975
	public RectTransform InventorySlotsParent;

	// Token: 0x040007B8 RID: 1976
	public RectTransform InventoryCardsParent;

	// Token: 0x040007B9 RID: 1977
	public CardLine InventorySlotsLine;

	// Token: 0x040007BA RID: 1978
	public ScrollRect InventoryScrollView;

	// Token: 0x040007BB RID: 1979
	public GameObject CloseButton;

	// Token: 0x040007BC RID: 1980
	[SerializeField]
	private CloseOnClickOutside CloseOnClick;

	// Token: 0x040007BD RID: 1981
	public Toggle PinButton;

	// Token: 0x040007BE RID: 1982
	public Button EmptyInventoryButton;

	// Token: 0x040007BF RID: 1983
	public GameObject TrashButton;

	// Token: 0x040007C0 RID: 1984
	public GameObject TrashConfirm;

	// Token: 0x040007C1 RID: 1985
	public Button ThrowAllButton;

	// Token: 0x040007C2 RID: 1986
	public TextMeshProUGUI TrashConfirmText;

	// Token: 0x040007C3 RID: 1987
	public GameObject RenameButton;

	// Token: 0x040007C4 RID: 1988
	public GameObject RenamePopup;

	// Token: 0x040007C5 RID: 1989
	public GameObject HelpButton;

	// Token: 0x040007C6 RID: 1990
	public TMP_InputField RenameInput;

	// Token: 0x040007C7 RID: 1991
	public bool HideTrashButton;

	// Token: 0x040007C8 RID: 1992
	public GameObject[] BookmarksObjects;

	// Token: 0x040007C9 RID: 1993
	public Image CurrentBookmarkImage;

	// Token: 0x040007CA RID: 1994
	public TextMeshProUGUI CurrentBookmarkNumber;

	// Token: 0x040007CB RID: 1995
	public GameObject SelectBookmarkPopup;

	// Token: 0x040007CC RID: 1996
	public RectTransform SelectBookmarksParent;

	// Token: 0x040007CD RID: 1997
	public SetBookmarkButton SelectBookmarkPrefab;

	// Token: 0x040007CE RID: 1998
	public Color BookmarkTextColor;

	// Token: 0x040007CF RID: 1999
	public Color NullBookmarkTextColor;

	// Token: 0x040007D0 RID: 2000
	private List<SetBookmarkButton> SetBookmarkButtons = new List<SetBookmarkButton>();

	// Token: 0x040007D1 RID: 2001
	public ActionConfirmPopup ConfirmPopup;

	// Token: 0x040007D2 RID: 2002
	[SerializeField]
	private RectTransform[] TitleBarLeftButtons;

	// Token: 0x040007D3 RID: 2003
	[SerializeField]
	private RectTransform[] TitleBarRightButtons;

	// Token: 0x040007D4 RID: 2004
	private RectTransform TitleRect;

	// Token: 0x040007D5 RID: 2005
	private DismantleActionButton SingleActionButton;

	// Token: 0x040007D6 RID: 2006
	private List<DismantleActionButton> OptionsButtons = new List<DismantleActionButton>();

	// Token: 0x040007D9 RID: 2009
	protected List<DynamicLayoutSlot> VisibleSlots = new List<DynamicLayoutSlot>();

	// Token: 0x040007DA RID: 2010
	private Rect WorldRect;

	// Token: 0x040007DB RID: 2011
	protected GraphicsManager GrM;

	// Token: 0x040007DC RID: 2012
	protected GameManager GM;

	// Token: 0x040007DD RID: 2013
	protected bool EmptyingInventory;

	// Token: 0x040007DF RID: 2015
	private ButtonSounds RenameInputSounds;
}
