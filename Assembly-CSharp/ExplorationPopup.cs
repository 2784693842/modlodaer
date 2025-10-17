using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x0200007D RID: 125
public class ExplorationPopup : MBSingleton<ExplorationPopup>
{
	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060004F8 RID: 1272 RVA: 0x00032C2D File Offset: 0x00030E2D
	// (set) Token: 0x060004F9 RID: 1273 RVA: 0x00032C35 File Offset: 0x00030E35
	public InGameCardBase ExplorationCard { get; private set; }

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x060004FA RID: 1274 RVA: 0x00032C3E File Offset: 0x00030E3E
	public Vector3 CardSpawnPos
	{
		get
		{
			if (!this.CardSpawn)
			{
				return base.transform.position;
			}
			return this.CardSpawn.position;
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x060004FB RID: 1275 RVA: 0x00032C64 File Offset: 0x00030E64
	public bool CanAddCards
	{
		get
		{
			return this.ExplorationCard && !this.ExplorationCard.InventoryFull;
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x060004FC RID: 1276 RVA: 0x00032C83 File Offset: 0x00030E83
	public bool CanHide
	{
		get
		{
			return this.CurrentPhase == ExplorationPopup.ExplorationPhase.Initial;
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00032C8E File Offset: 0x00030E8E
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00032C98 File Offset: 0x00030E98
	public void Init()
	{
		if (this.Initialized)
		{
			return;
		}
		this.GM = MBSingleton<GameManager>.Instance;
		this.SlotModel = new DynamicLayoutSlot(this.SlotSettings, null);
		this.SetSlots();
		this.ImprovementSlotsLine.Init(this.ImprovementSlotsParent, this.ImprovementSlotSettings, this.ImprovementSlotsView);
		this.DamagesSlotsLine.Init(this.DamagesSlotsParent, this.EnvDamageSlotSettings, this.DamagesSlotsView);
		if (this.InteractionTabButton)
		{
			this.InteractionTabButton.Setup(0, "", LocalizedString.Interaction, false);
			this.InteractionTabButton.NewNotification = false;
			IndexButton interactionTabButton = this.InteractionTabButton;
			interactionTabButton.OnClicked = (Action<int>)Delegate.Combine(interactionTabButton.OnClicked, new Action<int>(this.SelectTab));
		}
		if (this.ExplorationTabButton)
		{
			this.ExplorationTabButton.Setup(1, "", LocalizedString.Exploration, false);
			this.ExplorationTabButton.NewNotification = false;
			IndexButton explorationTabButton = this.ExplorationTabButton;
			explorationTabButton.OnClicked = (Action<int>)Delegate.Combine(explorationTabButton.OnClicked, new Action<int>(this.SelectTab));
		}
		if (this.ImprovementsTabButton)
		{
			this.ImprovementsTabButton.Setup(2, "", LocalizedString.Improvements, false);
			this.ImprovementsTabButton.NewNotification = false;
			IndexButton improvementsTabButton = this.ImprovementsTabButton;
			improvementsTabButton.OnClicked = (Action<int>)Delegate.Combine(improvementsTabButton.OnClicked, new Action<int>(this.SelectTab));
		}
		if (this.DamagesTabButton)
		{
			this.DamagesTabButton.Setup(3, "", LocalizedString.Damages, false);
			this.DamagesTabButton.NewNotification = false;
			IndexButton damagesTabButton = this.DamagesTabButton;
			damagesTabButton.OnClicked = (Action<int>)Delegate.Combine(damagesTabButton.OnClicked, new Action<int>(this.SelectTab));
		}
		this.Initialized = true;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x00032E84 File Offset: 0x00031084
	private void LateUpdate()
	{
		if (this.CloseOnClick && this.CurrentPhase == ExplorationPopup.ExplorationPhase.Initial && this.CloseOnClick.UpdateClickedOutside())
		{
			this.Hide(true);
		}
		if (this.ImprovementsDescriptionText && this.CurrentButtonTab == 2 && this.ExplorationCard)
		{
			if (!this.HoveredCard)
			{
				if (this.ImprovementsVisible)
				{
					this.ImprovementsDescriptionText.text = LocalizedString.ImprovementDescription(MobilePlatformDetection.IsMobilePlatform);
				}
				else
				{
					this.ImprovementsDescriptionText.text = LocalizedString.NoImprovementsUnlocked;
				}
			}
			else
			{
				if (this.ExplorationCard.CardModel && !this.ExplorationCard.CardModel.HasImprovement(this.HoveredCard.CardModel))
				{
					this.HoveredCard = null;
				}
				if (this.HoveredCard)
				{
					this.ImprovementsDescriptionText.text = string.Format("<b>{0}</b>: {1}", this.HoveredCard.CardName(true), this.HoveredCard.CardDescription(true));
				}
			}
		}
		if (this.DamagesDescriptionText && this.CurrentButtonTab == 3 && this.ExplorationCard)
		{
			if (!this.HoveredCard)
			{
				if (this.DamagesVisible)
				{
					this.DamagesDescriptionText.text = LocalizedString.ImprovementDescription(MobilePlatformDetection.IsMobilePlatform);
					return;
				}
				this.DamagesDescriptionText.text = LocalizedString.NoDamagesPresent;
				return;
			}
			else
			{
				if (this.ExplorationCard.CardModel && !this.ExplorationCard.CardModel.HasDamage(this.HoveredCard.CardModel))
				{
					this.HoveredCard = null;
				}
				if (this.HoveredCard)
				{
					this.DamagesDescriptionText.text = string.Format("<b>{0}</b>: {1}", this.HoveredCard.CardName(true), this.HoveredCard.CardDescription(true));
				}
			}
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0003307C File Offset: 0x0003127C
	private void SetSlots()
	{
		if (this.ExplorationSlotsLine != null && this.ExplorationSlotsLine.Slots != null && this.ExplorationSlotsLine.Slots.Count == this.ExplorationSlotCount)
		{
			return;
		}
		this.ExplorationSlotsLine.Init(this.SlotsParent, this.SlotSettings, null);
		for (int i = 0; i < this.ExplorationSlotCount; i++)
		{
			this.ExplorationSlotsLine.AddSlot(i);
		}
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000330F3 File Offset: 0x000312F3
	public void SetInstance()
	{
		if (MBSingleton<ExplorationPopup>.PrivateInstance != this)
		{
			MBSingleton<ExplorationPopup>.PrivateInstance = this;
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00033108 File Offset: 0x00031308
	public void Setup(InGameCardBase _ExplorationCard)
	{
		this.GM = MBSingleton<GameManager>.Instance;
		if (!this.GM)
		{
			return;
		}
		this.ExplorationCard = _ExplorationCard;
		this.ConfirmPopup.gameObject.SetActive(false);
		this.SetupInteraction();
		this.SetupExploration();
		this.SetupImprovements();
		this.SetupDamages();
		int num = -1;
		int num2 = 0;
		if (_ExplorationCard.CardModel.HasNormalDismantleActions)
		{
			this.InteractionTabButton.gameObject.SetActive(true);
			num2++;
			num = 0;
		}
		else
		{
			this.InteractionTabButton.gameObject.SetActive(false);
		}
		if (_ExplorationCard.CardModel.ExplorationActionIndex >= 0)
		{
			this.ExplorationTabButton.gameObject.SetActive(true);
			num2++;
			if (num == -1)
			{
				num = 1;
			}
		}
		else
		{
			this.ExplorationTabButton.gameObject.SetActive(false);
		}
		if (_ExplorationCard.CardModel.HasImprovements)
		{
			this.ImprovementsTabButton.gameObject.SetActive(true);
			num2++;
			if (num == -1)
			{
				num = 2;
			}
		}
		else
		{
			this.ImprovementsTabButton.gameObject.SetActive(false);
		}
		if (_ExplorationCard.CardModel.HasDamages)
		{
			this.DamagesTabButton.gameObject.SetActive(true);
			num2++;
			if (num == -1)
			{
				num = 3;
			}
		}
		else
		{
			this.DamagesTabButton.gameObject.SetActive(false);
		}
		if (num2 <= 1)
		{
			this.InteractionTabButton.gameObject.SetActive(false);
			this.ExplorationTabButton.gameObject.SetActive(false);
			this.ImprovementsTabButton.gameObject.SetActive(false);
			this.DamagesTabButton.gameObject.SetActive(false);
		}
		if (num == -1)
		{
			num = 0;
		}
		if ((!_ExplorationCard.CardModel.HasNormalDismantleActions && this.CurrentButtonTab == 0) || (_ExplorationCard.CardModel.ExplorationActionIndex < 0 && this.CurrentButtonTab == 1) || (!_ExplorationCard.CardModel.HasImprovements && this.CurrentButtonTab == 2) || (!_ExplorationCard.CardModel.HasDamages && this.CurrentButtonTab == 3))
		{
			this.CurrentButtonTab = num;
		}
		this.SelectTab(this.CurrentButtonTab);
		if (this.TitleText)
		{
			this.TitleText.text = _ExplorationCard.CardName(false);
		}
		if (this.CloseButton)
		{
			this.CloseButton.SetActive(true);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00033348 File Offset: 0x00031548
	private void SetupInteraction()
	{
		if (!this.ExplorationCard)
		{
			return;
		}
		if (!this.ExplorationCard.CardModel)
		{
			return;
		}
		this.InteractionCardBackground.overrideSprite = this.ExplorationCard.CardModel.CardBackground;
		this.InteractionCardImage.overrideSprite = this.ExplorationCard.CurrentImage;
		this.InteractionTabButton.Sprite = this.ExplorationCard.CurrentImage;
		this.InteractionDesc.text = this.ExplorationCard.CardDescription(false);
		if (this.ExplorationCard.CardModel.DismantleActions != null)
		{
			while (this.InteractionButtons.Count < this.ExplorationCard.CardModel.DismantleActions.Count)
			{
				this.InteractionButtons.Add(UnityEngine.Object.Instantiate<DismantleActionButton>(this.InteractionButtonPrefab, this.InteractionButtonsParent));
				DismantleActionButton dismantleActionButton = this.InteractionButtons[this.InteractionButtons.Count - 1];
				dismantleActionButton.OnClicked = (Action<int>)Delegate.Combine(dismantleActionButton.OnClicked, new Action<int>(this.OnActionButtonClicked));
			}
		}
		List<string> list = new List<string>();
		for (int i = 0; i < this.InteractionButtons.Count; i++)
		{
			if (this.ExplorationCard.CardModel.DismantleActions == null)
			{
				this.InteractionButtons[i].gameObject.SetActive(false);
			}
			else if (i >= this.ExplorationCard.CardModel.DismantleActions.Count)
			{
				this.InteractionButtons[i].gameObject.SetActive(false);
			}
			else if (!this.ExplorationCard.CardModel.DismantleActions[i].CanAppear(this.ExplorationCard, false, false) || list.Contains(this.ExplorationCard.CardModel.DismantleActions[i].ActionName.DefaultText))
			{
				this.InteractionButtons[i].gameObject.SetActive(false);
			}
			else if (!this.ExplorationCard.CardModel.DismantleActions[i].PerformUponInspection)
			{
				if (this.InteractionButtons[i].Setup(i, this.ExplorationCard.CardModel.DismantleActions[i], this.ExplorationCard, MBSingleton<TutorialManager>.Instance.IsActionHighlighted(this.ExplorationCard.CardModel, this.ExplorationCard.CardModel.DismantleActions[i]), false))
				{
					list.Add(this.ExplorationCard.CardModel.DismantleActions[i].ActionName);
				}
			}
			else
			{
				this.InteractionButtons[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0003360C File Offset: 0x0003180C
	private void SetupExploration()
	{
		if (!this.ExplorationCard)
		{
			return;
		}
		if (!this.ExplorationCard.CardModel)
		{
			return;
		}
		if (this.ExplorationCard.CardModel.ExplorationActionIndex < 0)
		{
			return;
		}
		this.SetSlots();
		this.ExplorationAction = this.ExplorationCard.CardModel.DismantleActions[this.ExplorationCard.CardModel.ExplorationActionIndex];
		this.ExplorationAction.CollectActionModifiers(this.ExplorationCard, null);
		this.ExplorationPerTick = this.ExplorationAction.ExplorationValue / (float)this.ExplorationAction.TotalDaytimeCost;
		for (int i = 0; i < this.ExplorationSlotsLine.Slots.Count; i++)
		{
			this.ExplorationSlotsLine.Slots[i].ClearFilters();
			this.ExplorationSlotsLine.Slots[i].AddFilter(this.ExplorationCard.CardModel.InventoryFilter);
			this.ExplorationSlotsLine.Slots[i].Index = i;
		}
		for (int j = 0; j < this.ExplorationCard.CardsInInventory.Count; j++)
		{
			if (this.ExplorationCard.CardsInInventory[j] != null && !this.ExplorationCard.CardsInInventory[j].IsFree)
			{
				for (int k = 0; k < this.ExplorationCard.CardsInInventory[j].CardAmt; k++)
				{
					this.ExplorationSlotsLine.Slots[this.ExplorationCard.CardsInInventory[j].AllCards[k].CurrentSlotInfo.SlotIndex].AssignCard(this.ExplorationCard.CardsInInventory[j].AllCards[k], false);
				}
			}
		}
		if (this.MainButton)
		{
			this.MainButton.Setup(0, this.ExplorationAction, this.ExplorationCard, false, false);
		}
		if (this.ExploringBar)
		{
			this.ExploringBar.Setup(this.ExplorationCard);
		}
		this.SelectedCards.Clear();
		this.AutoSelectedCards.Clear();
		this.CurrentPhase = ExplorationPopup.ExplorationPhase.Initial;
		this.CurrentSlot = this.ExplorationSlotsLine.Slots.Count;
		for (int l = 0; l < this.ExplorationSlotsLine.Slots.Count; l++)
		{
			if (this.ExplorationSlotsLine.Slots[l].AssignedCard == null)
			{
				this.CurrentSlot = l;
				break;
			}
		}
		this.UpdateSelectionText();
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x000338A8 File Offset: 0x00031AA8
	private void SetupImprovements()
	{
		if (!this.ExplorationCard)
		{
			return;
		}
		if (!this.ExplorationCard.CardModel)
		{
			return;
		}
		if (!this.ExplorationCard.CardModel.HasImprovements)
		{
			return;
		}
		List<InGameCardBase> list = new List<InGameCardBase>();
		this.ImprovementsVisible = false;
		int num = 0;
		while (num < this.ExplorationCard.CardModel.EnvironmentImprovements.Length || num < this.ImprovementSlotsLine.Slots.Count)
		{
			if (num >= this.ImprovementSlotsLine.Slots.Count)
			{
				this.ImprovementSlotsLine.AddSlot(this.ImprovementSlotsLine.Count);
			}
			if (num >= this.ExplorationCard.CardModel.EnvironmentImprovements.Length)
			{
				this.ImprovementSlotsLine.Slots[num].IsActive = false;
			}
			else
			{
				list.Clear();
				if (this.GM.CardIsOnBoard(this.ExplorationCard.CardModel.EnvironmentImprovements[num].Card, false, true, false, false, list, Array.Empty<InGameCardBase>()))
				{
					this.ImprovementSlotsLine.Slots[num].IsActive = true;
					this.ImprovementsVisible = true;
					for (int i = 0; i < list.Count; i++)
					{
						this.ImprovementSlotsLine.Slots[num].AssignCard(list[i], false);
					}
				}
				else
				{
					this.ImprovementSlotsLine.Slots[num].IsActive = false;
				}
			}
			num++;
		}
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00033A24 File Offset: 0x00031C24
	private void SetupDamages()
	{
		if (!this.ExplorationCard)
		{
			return;
		}
		if (!this.ExplorationCard.CardModel)
		{
			return;
		}
		if (!this.ExplorationCard.CardModel.HasDamages)
		{
			return;
		}
		this.DamagesVisible = false;
		int num = 0;
		while (num < this.GM.EnvDamageCards.Count || num < this.DamagesSlotsLine.Slots.Count)
		{
			if (num >= this.DamagesSlotsLine.Slots.Count)
			{
				this.DamagesSlotsLine.AddSlot(this.DamagesSlotsLine.Slots.Count);
			}
			if (num >= this.GM.EnvDamageCards.Count)
			{
				this.DamagesSlotsLine.Slots[num].IsActive = false;
			}
			else if (!this.GM.EnvDamageCards[num])
			{
				this.DamagesSlotsLine.Slots[num].IsActive = false;
			}
			else if (this.GM.EnvDamageCards[num].Destroyed)
			{
				this.DamagesSlotsLine.Slots[num].IsActive = false;
			}
			else if (!this.ExplorationCard.CardModel.HasDamage(this.GM.EnvDamageCards[num].CardModel))
			{
				this.DamagesSlotsLine.Slots[num].IsActive = false;
			}
			else
			{
				this.DamagesSlotsLine.Slots[num].IsActive = true;
				this.DamagesVisible = true;
				this.DamagesSlotsLine.Slots[num].AssignCard(this.GM.EnvDamageCards[num], false);
			}
			num++;
		}
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00033BF0 File Offset: 0x00031DF0
	public void Hide(bool _Clear)
	{
		this.CurrentPhase = ExplorationPopup.ExplorationPhase.Initial;
		MBSingleton<GraphicsManager>.Instance.ClearInspectedCard();
		this.HoveredCard = null;
		if (this.GM && this.CurrentButtonTab == 2 && this.ExplorationCard)
		{
			this.GM.CheckImprovements(this.ExplorationCard.CardModel);
		}
		if (_Clear)
		{
			this.Clear();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00033C64 File Offset: 0x00031E64
	public void Clear()
	{
		if (this.ExplorationSlotsLine != null)
		{
			for (int i = 0; i < this.ExplorationSlotsLine.Slots.Count; i++)
			{
				this.ExplorationSlotsLine.Slots[i].ClearSlot(false);
			}
		}
		this.ClearCurrentExplorationCard();
		if (this.ImprovementSlotsLine != null)
		{
			for (int j = 0; j < this.ImprovementSlotsLine.Slots.Count; j++)
			{
				this.ImprovementSlotsLine.Slots[j].ClearSlot(true);
			}
		}
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00033CF8 File Offset: 0x00031EF8
	private void ClearCurrentExplorationCard()
	{
		if (this.ExplorationCard)
		{
			for (int i = 0; i < this.ExplorationCard.CardsInInventory.Count; i++)
			{
				if (this.ExplorationCard.CardsInInventory[i] != null && !this.ExplorationCard.CardsInInventory[i].IsFree)
				{
					for (int j = 0; j < this.ExplorationCard.CardsInInventory[i].CardAmt; j++)
					{
						this.ExplorationCard.CardsInInventory[i].AllCards[j].CurrentSlotInfo = new SlotInfo(SlotsTypes.Exploration, i);
						this.ExplorationCard.CardsInInventory[i].AllCards[j].UpdateVisibility();
					}
				}
			}
			this.ExplorationCard = null;
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00033DD8 File Offset: 0x00031FD8
	public void SelectTab(int _Index)
	{
		if (this.GM && this.CurrentButtonTab == 1 && this.ExplorationCard)
		{
			this.GM.CheckImprovements(this.ExplorationCard.CardModel);
		}
		this.CurrentButtonTab = _Index;
		this.InteractionTabButton.Selected = (this.CurrentButtonTab == 0);
		this.ExplorationTabButton.Selected = (this.CurrentButtonTab == 1);
		this.ImprovementsTabButton.Selected = (this.CurrentButtonTab == 2);
		this.DamagesTabButton.Selected = (this.CurrentButtonTab == 3);
		this.InteractionGroup.SetActive(this.CurrentButtonTab == 0);
		this.ExplorationGroup.SetActive(this.CurrentButtonTab == 1);
		this.ImprovementsGroup.SetActive(this.CurrentButtonTab == 2);
		this.DamagesGroup.SetActive(this.CurrentButtonTab == 3);
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00033EC8 File Offset: 0x000320C8
	public void OnActionButtonClicked(int _Index)
	{
		if (this.ExplorationCard && this.ExplorationCard.DismantleActions.Length > _Index)
		{
			if (this.ExplorationCard.DismantleActions[_Index].ConfirmPopup && this.ConfirmPopup && !this.ConfirmPopup.gameObject.activeInHierarchy)
			{
				this.ConfirmPopup.Setup(this.ExplorationCard.DismantleActions[_Index].ActionName, HoursDisplay.HoursToCompleteString(GameManager.TickToHours(this.ExplorationCard.DismantleActions[_Index].TotalDaytimeCost, this.ExplorationCard.DismantleActions[_Index].MiniTicksCost)), _Index, this.ExplorationCard, null);
				this.ConfirmPopup.gameObject.SetActive(true);
				return;
			}
			GameManager.PerformAction(this.ExplorationCard.DismantleActions[_Index], this.ExplorationCard, false);
			if (this.ExplorationCard.DismantleActions[_Index].ReceivingCardChanges.ModType == CardModifications.Destroy || this.ExplorationCard.DismantleActions[_Index].ReceivingCardChanges.ModType == CardModifications.Transform || !this.ExplorationCard.DismantleActions[_Index].DontCloseInspectionWindow)
			{
				if (this.ExplorationCard.CardModel.CardType == CardTypes.Blueprint)
				{
					MBSingleton<GraphicsManager>.Instance.BlueprintModelsPopup.Hide();
				}
				this.Hide(true);
			}
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0003402C File Offset: 0x0003222C
	public void ClickMainButton()
	{
		switch (this.CurrentPhase)
		{
		case ExplorationPopup.ExplorationPhase.Initial:
			this.StartExploration();
			return;
		case ExplorationPopup.ExplorationPhase.Exploring:
			return;
		case ExplorationPopup.ExplorationPhase.Selecting:
			this.ConfirmSelection();
			return;
		default:
			return;
		}
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00034064 File Offset: 0x00032264
	private void StartExploration()
	{
		this.MainButton.Interactable = false;
		this.MainButton.Text = LocalizedString.Exploring;
		this.MainButton.ButtonTooltip = "";
		this.InteractionTabButton.Interactable = false;
		this.ExplorationTabButton.Interactable = false;
		this.ImprovementsTabButton.Interactable = false;
		this.DamagesTabButton.Interactable = false;
		this.CurrentPhase = ExplorationPopup.ExplorationPhase.Exploring;
		if (this.CloseButton)
		{
			this.CloseButton.SetActive(false);
		}
		GameManager.PerformAction(this.ExplorationAction, this.ExplorationCard, false);
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00034108 File Offset: 0x00032308
	private void ConfirmSelection()
	{
		if (!this.SelectionIsReady)
		{
			return;
		}
		this.InteractionTabButton.Interactable = true;
		this.ExplorationTabButton.Interactable = true;
		this.ImprovementsTabButton.Interactable = true;
		this.DamagesTabButton.Interactable = true;
		this.Hide(false);
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00034158 File Offset: 0x00032358
	public void StartSelection()
	{
		this.CurrentPhase = ExplorationPopup.ExplorationPhase.Selecting;
		this.MainButton.Text = LocalizedString.Confirm;
		this.MainButton.Interactable = this.SelectionIsReady;
		this.UpdateSelectionText();
		if (this.ExplorationPickupCount > 0)
		{
			this.MainButton.ButtonTooltip = this.SelectionPhaseTooltip;
			for (int i = 0; i < this.AutoSelectedCards.Count; i++)
			{
				if (this.AutoSelectedCards[i].AssignedCard.CardVisuals)
				{
					this.AutoSelectedCards[i].AssignedCard.SetGraphicState(CardGraphics.CardGraphicsStates.ExplorationAutoSelected, null);
				}
			}
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00034204 File Offset: 0x00032404
	public void ClickCard(InGameCardBase _Card)
	{
		DynamicLayoutSlot currentSlot = _Card.CurrentSlot;
		if (this.AutoSelectedCards.Contains(currentSlot) || this.CurrentPhase != ExplorationPopup.ExplorationPhase.Selecting)
		{
			return;
		}
		if (this.SelectedCards.Contains(currentSlot))
		{
			this.SelectedCards.Remove(currentSlot);
			_Card.SetGraphicState(CardGraphics.CardGraphicsStates.ExplorationNormal, null);
		}
		else if (this.SelectedCards.Count < this.ExplorationPickupCount)
		{
			this.SelectedCards.Add(currentSlot);
			_Card.SetGraphicState(CardGraphics.CardGraphicsStates.ExplorationSelected, null);
		}
		this.UpdateSelectionText();
		if (this.MainButton)
		{
			this.MainButton.Interactable = this.SelectionIsReady;
		}
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x000342A0 File Offset: 0x000324A0
	private void UpdateSelectionText()
	{
		if (this.SelectionText)
		{
			this.SelectionText.enabled = (this.CurrentMaxSelectionCount > 0 && this.CurrentPhase == ExplorationPopup.ExplorationPhase.Selecting);
			this.SelectionText.text = string.Concat(new string[]
			{
				this.SelectedCards.Count.ToString(),
				"/",
				this.CurrentMaxSelectionCount.ToString(),
				" ",
				LocalizedString.SelectedCards
			});
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000512 RID: 1298 RVA: 0x00034334 File Offset: 0x00032534
	private bool SelectionIsReady
	{
		get
		{
			if (this.SelectedCards.Count == this.ExplorationPickupCount)
			{
				return true;
			}
			for (int i = 0; i < this.ExplorationSlotsLine.Slots.Count; i++)
			{
				if (!this.AutoSelectedCards.Contains(this.ExplorationSlotsLine.Slots[i]) && !this.SelectedCards.Contains(this.ExplorationSlotsLine.Slots[i]) && this.ExplorationSlotsLine.Slots[i].AssignedCard)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000513 RID: 1299 RVA: 0x000343D0 File Offset: 0x000325D0
	private int CurrentMaxSelectionCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.ExplorationSlotsLine.Slots.Count; i++)
			{
				if (!this.AutoSelectedCards.Contains(this.ExplorationSlotsLine.Slots[i]) && this.ExplorationSlotsLine.Slots[i].AssignedCard)
				{
					num++;
					if (num == this.ExplorationPickupCount)
					{
						return num;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00034445 File Offset: 0x00032645
	public IEnumerator AddTickToExploration()
	{
		if (this.ExplorationPerTick <= 0f)
		{
			yield break;
		}
		this.ExplorationCard.ExplorationData.CurrentExploration = Mathf.Clamp01(this.ExplorationCard.ExplorationData.CurrentExploration + this.ExplorationPerTick);
		this.GM.StartCoroutineEx(this.ExploringBar.Animate(new Action<ExplorationResult>(this.AddActionToPerform)), out this.BarRoutine);
		List<CoroutineController> waitFor = new List<CoroutineController>();
		CoroutineController item = null;
		while (this.BarRoutine.state == CoroutineState.Running)
		{
			while (this.ActionsQueue.Count > 0)
			{
				this.GM.StartCoroutineEx(GameManager.PerformActionAsEnumerator(this.ActionsQueue[0], this.ExplorationCard, false), out item);
				waitFor.Add(item);
				this.ActionsQueue.RemoveAt(0);
			}
			yield return null;
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00034454 File Offset: 0x00032654
	private void AddActionToPerform(ExplorationResult _FromResult)
	{
		_FromResult.Action.CountAsExploration = true;
		_FromResult.Action.UseMiniTicks = MiniTicksBehavior.KeepsZeroCost;
		this.ActionsQueue.Add(_FromResult.Action);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00034480 File Offset: 0x00032680
	public void NextSlot()
	{
		for (int i = this.CurrentSlot + 1; i < this.ExplorationSlotsLine.Slots.Count; i++)
		{
			if (this.ExplorationSlotsLine.Slots[i].AssignedCard == null)
			{
				this.CurrentSlot = i;
				return;
			}
		}
		this.CurrentSlot = this.ExplorationSlotsLine.Slots.Count;
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x000344EB File Offset: 0x000326EB
	public IEnumerator AddCard(InGameCardBase _Card, bool _AutoSelect)
	{
		if (this.CurrentSlot >= this.ExplorationSlotsLine.Slots.Count)
		{
			yield break;
		}
		if (this.ExplorationSlotsLine.Slots[this.CurrentSlot].AssignedCard != null && (this.ExplorationSlotsLine.Slots[this.CurrentSlot].AssignedCard.CardModel != _Card.CardModel || !this.ExplorationSlotsLine.Slots[this.CurrentSlot].PileCompatible(_Card.CardModel)))
		{
			this.CurrentSlot++;
			if (this.CurrentSlot >= this.ExplorationSlotsLine.Slots.Count)
			{
				yield break;
			}
		}
		_Card.transform.SetParent(MBSingleton<GraphicsManager>.Instance.CardsMovingParent);
		this.ExplorationSlotsLine.Slots[this.CurrentSlot].AssignCard(_Card, false);
		if ((_AutoSelect || this.ExplorationPickupCount <= 0) && !this.AutoSelectedCards.Contains(this.ExplorationSlotsLine.Slots[this.CurrentSlot]))
		{
			this.AutoSelectedCards.Add(this.ExplorationSlotsLine.Slots[this.CurrentSlot]);
		}
		yield break;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00034508 File Offset: 0x00032708
	public List<InGameCardBase> GetSelection()
	{
		List<InGameCardBase> list = new List<InGameCardBase>();
		for (int i = 0; i < this.ExplorationSlotsLine.Slots.Count; i++)
		{
			if ((this.SelectedCards.Contains(this.ExplorationSlotsLine.Slots[i]) || this.AutoSelectedCards.Contains(this.ExplorationSlotsLine.Slots[i])) && this.ExplorationSlotsLine.Slots[i].CardPileCount(true) > 0)
			{
				list.AddRange(this.ExplorationSlotsLine.Slots[i].GetCardPile(false, false, false));
			}
		}
		return list;
	}

	// Token: 0x04000657 RID: 1623
	public int ExplorationSlotCount;

	// Token: 0x04000658 RID: 1624
	public int ExplorationPickupCount;

	// Token: 0x0400065A RID: 1626
	[SerializeField]
	private CloseOnClickOutside CloseOnClick;

	// Token: 0x0400065B RID: 1627
	[SerializeField]
	private TextMeshProUGUI TitleText;

	// Token: 0x0400065C RID: 1628
	[SerializeField]
	private GameObject CloseButton;

	// Token: 0x0400065D RID: 1629
	[Header("Interaction")]
	[SerializeField]
	private GameObject InteractionGroup;

	// Token: 0x0400065E RID: 1630
	[SerializeField]
	private IndexButton InteractionTabButton;

	// Token: 0x0400065F RID: 1631
	[SerializeField]
	private Image InteractionCardBackground;

	// Token: 0x04000660 RID: 1632
	[SerializeField]
	private Image InteractionCardImage;

	// Token: 0x04000661 RID: 1633
	[SerializeField]
	private TextMeshProUGUI InteractionDesc;

	// Token: 0x04000662 RID: 1634
	[SerializeField]
	private DismantleActionButton InteractionButtonPrefab;

	// Token: 0x04000663 RID: 1635
	[SerializeField]
	private RectTransform InteractionButtonsParent;

	// Token: 0x04000664 RID: 1636
	[SerializeField]
	private ActionConfirmPopup ConfirmPopup;

	// Token: 0x04000665 RID: 1637
	[Header("Exploration")]
	public CardLine ExplorationSlotsLine;

	// Token: 0x04000666 RID: 1638
	[SerializeField]
	private GameObject ExplorationGroup;

	// Token: 0x04000667 RID: 1639
	[SerializeField]
	private IndexButton ExplorationTabButton;

	// Token: 0x04000668 RID: 1640
	public SlotSettings SlotSettings;

	// Token: 0x04000669 RID: 1641
	public DynamicLayoutSlot SlotModel;

	// Token: 0x0400066A RID: 1642
	[SerializeField]
	private RectTransform SlotsParent;

	// Token: 0x0400066B RID: 1643
	[SerializeField]
	private ExplorationBar ExploringBar;

	// Token: 0x0400066C RID: 1644
	[SerializeField]
	private DismantleActionButton MainButton;

	// Token: 0x0400066D RID: 1645
	[SerializeField]
	private TextMeshProUGUI SelectionText;

	// Token: 0x0400066E RID: 1646
	[SerializeField]
	private Transform CardSpawn;

	// Token: 0x0400066F RID: 1647
	[SerializeField]
	private LocalizedString SelectionPhaseTooltip;

	// Token: 0x04000670 RID: 1648
	[Header("Improvements")]
	public CardLine ImprovementSlotsLine;

	// Token: 0x04000671 RID: 1649
	[SerializeField]
	private GameObject ImprovementsGroup;

	// Token: 0x04000672 RID: 1650
	[SerializeField]
	private IndexButton ImprovementsTabButton;

	// Token: 0x04000673 RID: 1651
	public SlotSettings ImprovementSlotSettings;

	// Token: 0x04000674 RID: 1652
	[SerializeField]
	private RectTransform ImprovementSlotsParent;

	// Token: 0x04000675 RID: 1653
	[SerializeField]
	private ScrollRect ImprovementSlotsView;

	// Token: 0x04000676 RID: 1654
	[FormerlySerializedAs("DescriptionText")]
	[SerializeField]
	private TextMeshProUGUI ImprovementsDescriptionText;

	// Token: 0x04000677 RID: 1655
	[Header("Env Damage")]
	public CardLine DamagesSlotsLine;

	// Token: 0x04000678 RID: 1656
	[SerializeField]
	private GameObject DamagesGroup;

	// Token: 0x04000679 RID: 1657
	[SerializeField]
	private IndexButton DamagesTabButton;

	// Token: 0x0400067A RID: 1658
	public SlotSettings EnvDamageSlotSettings;

	// Token: 0x0400067B RID: 1659
	[SerializeField]
	private RectTransform DamagesSlotsParent;

	// Token: 0x0400067C RID: 1660
	[SerializeField]
	private ScrollRect DamagesSlotsView;

	// Token: 0x0400067D RID: 1661
	[SerializeField]
	private TextMeshProUGUI DamagesDescriptionText;

	// Token: 0x0400067E RID: 1662
	[NonSerialized]
	public InGameCardBase HoveredCard;

	// Token: 0x0400067F RID: 1663
	private List<DynamicLayoutSlot> AutoSelectedCards = new List<DynamicLayoutSlot>();

	// Token: 0x04000680 RID: 1664
	private List<DynamicLayoutSlot> SelectedCards = new List<DynamicLayoutSlot>();

	// Token: 0x04000681 RID: 1665
	private DismantleCardAction ExplorationAction;

	// Token: 0x04000682 RID: 1666
	private float ExplorationPerTick;

	// Token: 0x04000683 RID: 1667
	private GameManager GM;

	// Token: 0x04000684 RID: 1668
	private CoroutineController BarRoutine;

	// Token: 0x04000685 RID: 1669
	private List<CardAction> ActionsQueue = new List<CardAction>();

	// Token: 0x04000686 RID: 1670
	private Rect WorldRect;

	// Token: 0x04000687 RID: 1671
	private int CurrentSlot;

	// Token: 0x04000688 RID: 1672
	private List<int> OccupiedSlots = new List<int>();

	// Token: 0x04000689 RID: 1673
	private List<DismantleActionButton> InteractionButtons = new List<DismantleActionButton>();

	// Token: 0x0400068A RID: 1674
	private bool ImprovementsVisible;

	// Token: 0x0400068B RID: 1675
	private bool DamagesVisible;

	// Token: 0x0400068C RID: 1676
	private int CurrentButtonTab;

	// Token: 0x0400068D RID: 1677
	private bool Initialized;

	// Token: 0x0400068E RID: 1678
	private ExplorationPopup.ExplorationPhase CurrentPhase;

	// Token: 0x0200025B RID: 603
	private enum ExplorationPhase
	{
		// Token: 0x04001434 RID: 5172
		Initial,
		// Token: 0x04001435 RID: 5173
		Exploring,
		// Token: 0x04001436 RID: 5174
		Selecting
	}
}
