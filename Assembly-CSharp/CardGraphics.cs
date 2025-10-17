using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x0200004E RID: 78
public class CardGraphics : MonoBehaviour, IDropHandler, IEventSystemHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x0600031A RID: 794 RVA: 0x0001F55F File Offset: 0x0001D75F
	// (set) Token: 0x0600031B RID: 795 RVA: 0x0001F567 File Offset: 0x0001D767
	public Graphic CurrentCollision { get; private set; }

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x0600031C RID: 796 RVA: 0x0001F570 File Offset: 0x0001D770
	// (set) Token: 0x0600031D RID: 797 RVA: 0x0001F578 File Offset: 0x0001D778
	public InGameCardBase CardLogic { get; private set; }

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x0600031E RID: 798 RVA: 0x0001F581 File Offset: 0x0001D781
	// (set) Token: 0x0600031F RID: 799 RVA: 0x0001F589 File Offset: 0x0001D789
	public Transform CardNotificationsTr { get; private set; }

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000320 RID: 800 RVA: 0x0001F592 File Offset: 0x0001D792
	// (set) Token: 0x06000321 RID: 801 RVA: 0x0001F59A File Offset: 0x0001D79A
	public bool HiddenInPile { get; private set; }

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000322 RID: 802 RVA: 0x0001F5A3 File Offset: 0x0001D7A3
	// (set) Token: 0x06000323 RID: 803 RVA: 0x0001F5AB File Offset: 0x0001D7AB
	public CardTypes CardType { get; private set; }

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000324 RID: 804 RVA: 0x0001F5B4 File Offset: 0x0001D7B4
	// (set) Token: 0x06000325 RID: 805 RVA: 0x0001F5BC File Offset: 0x0001D7BC
	public bool IsBlueprintInstance { get; private set; }

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000326 RID: 806 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
	public Vector3 CardNotificationsPos
	{
		get
		{
			if (!this.CardNotificationsTr)
			{
				this.CardNotificationsTr = new GameObject("Notifications").transform;
				this.CardNotificationsTr.position = base.transform.position + this.CardNotificationsOffset;
				this.CardNotificationsTr.SetParent(base.transform);
			}
			return this.CardNotificationsTr.position;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000327 RID: 807 RVA: 0x0001F634 File Offset: 0x0001D834
	public Vector3 BlueprintPurchasePos
	{
		get
		{
			if (this.BuyBlueprintButton)
			{
				return this.BuyBlueprintButton.transform.position;
			}
			if (this.SunsCostText)
			{
				return this.SunsCostText.transform.position;
			}
			return base.transform.position;
		}
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0001F688 File Offset: 0x0001D888
	private void Awake()
	{
		this.GrM = MBSingleton<GraphicsManager>.Instance;
		this.GM = MBSingleton<GameManager>.Instance;
		if (this.BuyBlueprintButton)
		{
			this.BuyBlueprintRaycast = this.BuyBlueprintButton.GetComponent<Graphic>();
		}
		if (this.ResearchBlueprintButton)
		{
			this.ResearchBlueprintRaycast = this.ResearchBlueprintButton.GetComponent<Graphic>();
		}
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0001F6E8 File Offset: 0x0001D8E8
	private void OnEnable()
	{
		GameManager.OnDismantleActionHovered = (Action)Delegate.Combine(GameManager.OnDismantleActionHovered, new Action(this.SetEventProbabilities));
		if (!this.CardLogic)
		{
			this.SetCollider(false);
			return;
		}
		this.SetCollider(this.CardLogic.SmallCollider);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0001F73B File Offset: 0x0001D93B
	private void OnDisable()
	{
		GameManager.OnDismantleActionHovered = (Action)Delegate.Remove(GameManager.OnDismantleActionHovered, new Action(this.SetEventProbabilities));
		this.DestroyDuplicate();
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0001F764 File Offset: 0x0001D964
	public void Setup(InGameCardBase _From)
	{
		this.CardLogic = _From;
		if (_From is InGameDraggableCard)
		{
			this.DraggableCardLogic = (_From as InGameDraggableCard);
		}
		if (this.CardLogic.CardModel)
		{
			this.CardType = this.CardLogic.CardModel.CardType;
		}
		this.IsBlueprintInstance = this.CardLogic.IsBlueprintInstance;
		this.CardImage.overrideSprite = this.CardLogic.CurrentImage;
		this.CardBG.overrideSprite = _From.CardModel.CardBackground;
		this.CardTitle.text = _From.CardName(false);
		this.CookingSprite = _From.CardModel.CookingSprite;
		this.RefreshDurabilities();
		if (this.TravelIcon && _From.CardModel)
		{
			this.TravelIcon.SetActive(_From.CardModel.IsTravellingCard);
		}
		if (this.BlueprintIcon && _From.CardModel)
		{
			this.BlueprintIcon.SetActive(_From.CardModel.CardType == CardTypes.Blueprint || (_From.CardModel.CardType == CardTypes.EnvImprovement && !_From.BlueprintComplete) || _From.CardModel.CardType == CardTypes.EnvDamage);
		}
		this.UpdateInventoryInfo();
		if (this.BlueprintHelpButton)
		{
			this.BlueprintHelpButton.SetActive(GuideManager.GetPageFor(this.CardLogic));
		}
		if (!GameManager.DontRenameGOs)
		{
			base.name = (_From ? string.Format("{0} ({1})", this.PoolIndex.ToString(), _From.name) : string.Format("NONE_Visuals", Array.Empty<object>()));
		}
		if (this.CardLogic.CurrentContainer && this.CardLogic.CurrentContainer.CardVisuals)
		{
			this.CardLogic.CurrentContainer.CardVisuals.RefreshCookingStatus();
		}
		this.UpdateTutorialHighlight();
		this.SetGraphicState(this.CardLogic.CurrentGraphicState, this.CardLogic.GraphicStateInventoryCard);
		this.UpdateMissingRequirements(this.CardLogic.CurrentMissingRequirements);
		this.UpdateCookingProgress(this.CardLogic.CurrentCookingBarInfo);
		this.SetCollider(this.CardLogic.SmallCollider);
		GraphicsManager.SetActiveGroup(this.ActionPerformedObjects, this.CardLogic.ActionPerformedObjects);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
	public void SetCollider(bool _Small)
	{
		if (_Small)
		{
			if (this.CurrentCollision && this.CurrentCollision != this.SmallCollision)
			{
				this.CurrentCollision.raycastTarget = false;
			}
			this.CurrentCollision = this.SmallCollision;
		}
		else
		{
			if (this.CurrentCollision && this.CurrentCollision != this.FullCollision)
			{
				this.CurrentCollision.raycastTarget = false;
			}
			this.CurrentCollision = this.FullCollision;
		}
		if (this.CurrentCollision)
		{
			if (this.CardLogic)
			{
				this.CurrentCollision.raycastTarget = this.CardLogic.BlocksRaycasts;
				return;
			}
			this.CurrentCollision.raycastTarget = false;
		}
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0001FA80 File Offset: 0x0001DC80
	public void UpdateInventoryInfo()
	{
		if (this.Duplicate)
		{
			this.Duplicate.UpdateInventoryInfo();
		}
		if (this.CardLogic == null)
		{
			for (int i = 0; i < this.InventoryIndicators.Count; i++)
			{
				this.InventoryIndicators[i].SetActive(false);
			}
			if (this.InventoryBarIndicatorParent)
			{
				this.InventoryBarIndicatorParent.SetActive(false);
			}
			return;
		}
		if (this.CardLogic.CardModel && this.GM && this.CardLogic.CardModel.CardType == CardTypes.Explorable)
		{
			for (int j = 0; j < this.InventoryIndicators.Count; j++)
			{
				this.InventoryIndicators[j].SetActive(false);
			}
			if (this.InventoryBarIndicatorParent)
			{
				this.InventoryBarIndicatorParent.SetActive(this.GM.MaxEnvWeight > 0f);
			}
			if (this.InventoryBarIndicator)
			{
				this.InventoryBarIndicator.fillAmount = this.GM.CurrentEnvWeight / this.GM.MaxEnvWeight;
			}
			return;
		}
		if (this.CardLogic.TravelToData != null)
		{
			if (this.InventoryBarIndicatorParent)
			{
				this.InventoryBarIndicatorParent.SetActive(this.CardLogic.TravelToData.CurrentMaxWeight > 0f);
			}
			return;
		}
		if (this.CardLogic.CardsInInventory == null)
		{
			for (int k = 0; k < this.InventoryIndicators.Count; k++)
			{
				this.InventoryIndicators[k].SetActive(false);
			}
			if (this.InventoryBarIndicatorParent)
			{
				this.InventoryBarIndicatorParent.SetActive(false);
			}
			return;
		}
		if (this.CardLogic.CardsInInventory != null)
		{
			if (this.CardLogic.IsLegacyInventory)
			{
				if (this.InventoryBarIndicatorParent)
				{
					this.InventoryBarIndicatorParent.SetActive(false);
				}
				int num = 0;
				while (num < this.CardLogic.CardsInInventory.Count || num < this.InventoryIndicators.Count)
				{
					if (num >= this.InventoryIndicators.Count)
					{
						this.InventoryIndicators.Add(new CardGraphics.CardInventoryInfo(this.InventoryIndicatorPrefab, this.InventoryIndicatorsParent));
					}
					if (num >= this.CardLogic.CardsInInventory.Count)
					{
						this.InventoryIndicators[num].SetActive(false);
					}
					else
					{
						this.InventoryIndicators[num].SetActive(true);
						if (this.CardLogic.BlueprintData != null && (this.CardLogic.CardModel.CardType == CardTypes.Blueprint || this.CardLogic.CardModel.CardType == CardTypes.EnvImprovement || this.CardLogic.CardModel.CardType == CardTypes.EnvDamage))
						{
							this.InventoryIndicators[num].Setup(this.CardLogic.CardModel.BlueprintStages[this.CardLogic.BlueprintData.CurrentStage].RequiredElements[num].GetQuantity);
						}
						else
						{
							this.InventoryIndicators[num].Setup(0);
						}
					}
					num++;
				}
				return;
			}
			for (int l = 0; l < this.InventoryIndicators.Count; l++)
			{
				this.InventoryIndicators[l].SetActive(false);
			}
			if (this.InventoryBarIndicatorParent)
			{
				this.InventoryBarIndicatorParent.SetActive(true);
			}
			if (this.InventoryBarIndicator)
			{
				this.InventoryBarIndicator.fillAmount = this.CardLogic.InventoryWeight(true) / this.CardLogic.MaxWeightCapacity;
			}
		}
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0001FE24 File Offset: 0x0001E024
	public void UpdateTutorialHighlight()
	{
		if (!this.CardLogic)
		{
			GraphicsManager.SetActiveGroup(this.TutorialHighlightedObjects, false);
			if (this.TutorialArrow)
			{
				this.TutorialArrow.SetActive(false);
				return;
			}
		}
		else
		{
			GraphicsManager.SetActiveGroup(this.TutorialHighlightedObjects, this.CardLogic.TutorialHighlight > TutorialHighlightState.NotHighlighted);
			if (this.TutorialArrow)
			{
				this.TutorialArrow.SetActive(this.CardLogic.TutorialHighlight == TutorialHighlightState.HighlightedWithArrow);
			}
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0001FEA4 File Offset: 0x0001E0A4
	public void ModifyDurability(DurabilitiesTypes _Type, float _Amt, bool _UseLiquid)
	{
		if (!base.gameObject.activeInHierarchy || this.HiddenInPile)
		{
			return;
		}
		CardData cardData = _UseLiquid ? this.CardLogic.ContainedLiquidModel : this.CardLogic.CardModel;
		switch (_Type)
		{
		case DurabilitiesTypes.Spoilage:
			if (this.SpoilageChangedPrefab)
			{
				if (!this.CurrentSpoilageChange)
				{
					this.LastSpoilageChange = _Amt;
					string text = this.AmtToText(this.LastSpoilageChange, cardData.SpoilageTime.Max, this.CardLogic.CurrentSpoilageRate, cardData.SpoilageTime.TextDisplay, true);
					this.CurrentSpoilageChange = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.SpoilageChangedPrefab, base.transform);
					if (this.RightSidePlaying)
					{
						this.CurrentSpoilageChange.Delay = 1f;
					}
					else
					{
						this.LeftSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentSpoilageChange.PlayFeedback(this.SpoilageValue.transform.position, text, this.AmtToColor(this.LastSpoilageChange), cardData.SpoilageTime.OverrideIcon, Color.white));
				}
				else
				{
					this.LastSpoilageChange += _Amt;
					string text = this.AmtToText(this.LastSpoilageChange, cardData.SpoilageTime.Max, this.CardLogic.CurrentSpoilageRate, cardData.SpoilageTime.TextDisplay, true);
					this.CurrentSpoilageChange.UpdateText(text, this.AmtToColor(this.LastSpoilageChange), true);
				}
			}
			break;
		case DurabilitiesTypes.Usage:
			if (this.UsageChangedPrefab)
			{
				if (!this.CurrentUsageChange)
				{
					this.LastUsageChange = _Amt;
					string text = this.AmtToText(this.LastUsageChange, cardData.UsageDurability.Max, this.CardLogic.CurrentUsageRate, cardData.UsageDurability.TextDisplay, true);
					this.CurrentUsageChange = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.UsageChangedPrefab, base.transform);
					if (this.RightSidePlaying)
					{
						this.CurrentUsageChange.Delay = 1f;
					}
					else
					{
						this.LeftSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentUsageChange.PlayFeedback(this.UsageValue.transform.position, text, this.AmtToColor(this.LastUsageChange), cardData.UsageDurability.OverrideIcon, Color.white));
				}
				else
				{
					this.LastUsageChange += _Amt;
					string text = this.AmtToText(this.LastUsageChange, cardData.UsageDurability.Max, this.CardLogic.CurrentUsageRate, cardData.UsageDurability.TextDisplay, true);
					this.CurrentUsageChange.UpdateText(text, this.AmtToColor(this.LastUsageChange), true);
				}
			}
			break;
		case DurabilitiesTypes.Fuel:
			if (this.FuelChangedPrefab)
			{
				if (!this.CurrentFuelChange)
				{
					this.LastFuelChange = _Amt;
					string text = this.AmtToText(this.LastFuelChange, cardData.FuelCapacity.Max, this.CardLogic.CurrentFuelRate, cardData.FuelCapacity.TextDisplay, true);
					this.CurrentFuelChange = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.FuelChangedPrefab, base.transform);
					if (this.LeftSidePlaying)
					{
						this.CurrentFuelChange.Delay = 1f;
					}
					else
					{
						this.RightSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentFuelChange.PlayFeedback(this.FuelValue.transform.position, text, this.AmtToColor(this.LastFuelChange), cardData.FuelCapacity.OverrideIcon, Color.white));
				}
				else
				{
					this.LastFuelChange += _Amt;
					string text = this.AmtToText(this.LastFuelChange, cardData.FuelCapacity.Max, this.CardLogic.CurrentFuelRate, cardData.FuelCapacity.TextDisplay, true);
					this.CurrentFuelChange.UpdateText(text, this.AmtToColor(this.LastFuelChange), true);
				}
			}
			break;
		case DurabilitiesTypes.Progress:
			if (this.ChargesChangedPrefab)
			{
				if (!this.CurrentChargesChange)
				{
					this.LastChargesChange = _Amt;
					string text = this.AmtToText(this.LastChargesChange, cardData.Progress.Max, this.CardLogic.CurrentConsumableRate, cardData.Progress.TextDisplay, true);
					this.CurrentChargesChange = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.ChargesChangedPrefab, base.transform);
					if (this.LeftSidePlaying)
					{
						this.CurrentChargesChange.Delay = 1f;
					}
					else
					{
						this.RightSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentChargesChange.PlayFeedback(this.ChargesValue.transform.position, text, this.AmtToColor(this.LastChargesChange), cardData.Progress.OverrideIcon, Color.white));
				}
				else
				{
					this.LastChargesChange += _Amt;
					string text = this.AmtToText(this.LastChargesChange, cardData.Progress.Max, this.CardLogic.CurrentConsumableRate, cardData.Progress.TextDisplay, true);
					this.CurrentChargesChange.UpdateText(text, this.AmtToColor(this.LastChargesChange), true);
				}
			}
			break;
		case DurabilitiesTypes.Liquid:
			if (this.LiquidChangedPrefab && this.CardLogic.ContainedLiquid)
			{
				if (!this.CurrentLiquidChange)
				{
					this.LastLiquidChange = _Amt;
					string text = this.AmtToText(this.LastLiquidChange, this.CardLogic.CardModel.MaxLiquidCapacity, this.CardLogic.ContainedLiquid.CurrentEvaporationRate, DurabilityTextDisplay.Percent, true);
					this.CurrentLiquidChange = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.LiquidChangedPrefab, base.transform);
					if (this.RightSidePlaying)
					{
						this.CurrentLiquidChange.Delay = 1f;
					}
					else
					{
						this.LeftSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentLiquidChange.PlayFeedback(this.LiquidValue.transform.position, text, this.AmtToColor(this.LastLiquidChange), this.CardLogic.ContainedLiquidModel.CardImage, Color.white));
				}
				else
				{
					this.LastLiquidChange += _Amt;
					string text = this.AmtToText(this.LastLiquidChange, this.CardLogic.CardModel.MaxLiquidCapacity, this.CardLogic.ContainedLiquid.CurrentEvaporationRate, DurabilityTextDisplay.Percent, true);
					this.CurrentLiquidChange.UpdateText(text, this.AmtToColor(this.LastLiquidChange), true);
				}
			}
			break;
		case DurabilitiesTypes.Special1:
			if (this.SpecialChangedPrefabLeft)
			{
				if (!this.CurrentSpecial1Change)
				{
					this.LastSpecial1Change = _Amt;
					string text = this.AmtToText(this.LastSpecial1Change, cardData.SpecialDurability1.Max, this.CardLogic.CurrentSpecial1Rate, cardData.SpecialDurability1.TextDisplay, true);
					this.CurrentSpecial1Change = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.SpecialChangedPrefabLeft, base.transform);
					if (this.RightSidePlaying)
					{
						this.CurrentSpecial1Change.Delay = 1f;
					}
					else
					{
						this.LeftSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentSpecial1Change.PlayFeedback(this.Special1Value.transform.position, text, this.AmtToColor(this.LastSpecial1Change), cardData.SpecialDurability1.OverrideIcon, Color.white));
				}
				else
				{
					this.LastSpecial1Change += _Amt;
					string text = this.AmtToText(this.LastSpecial1Change, cardData.SpecialDurability1.Max, this.CardLogic.CurrentSpecial1Rate, cardData.SpecialDurability1.TextDisplay, true);
					this.CurrentSpecial1Change.UpdateText(text, this.AmtToColor(this.LastSpecial1Change), true);
				}
			}
			break;
		case DurabilitiesTypes.Special2:
			if (this.SpecialChangedPrefabLeft)
			{
				if (!this.CurrentSpecial2Change)
				{
					this.LastSpecial2Change = _Amt;
					string text = this.AmtToText(this.LastSpecial2Change, cardData.SpecialDurability2.Max, this.CardLogic.CurrentSpecial2Rate, cardData.SpecialDurability2.TextDisplay, true);
					this.CurrentSpecial2Change = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.SpecialChangedPrefabLeft, base.transform);
					if (this.RightSidePlaying)
					{
						this.CurrentSpecial2Change.Delay = 1f;
					}
					else
					{
						this.LeftSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentSpecial2Change.PlayFeedback(this.Special2Value.transform.position, text, this.AmtToColor(this.LastSpecial2Change), cardData.SpecialDurability2.OverrideIcon, Color.white));
				}
				else
				{
					this.LastSpecial2Change += _Amt;
					string text = this.AmtToText(this.LastSpecial2Change, cardData.SpecialDurability2.Max, this.CardLogic.CurrentSpecial2Rate, cardData.SpecialDurability2.TextDisplay, true);
					this.CurrentSpecial2Change.UpdateText(text, this.AmtToColor(this.LastSpecial2Change), true);
				}
			}
			break;
		case DurabilitiesTypes.Special3:
			if (this.SpecialChangedPrefabRight)
			{
				if (!this.CurrentSpecial3Change)
				{
					this.LastSpecial3Change = _Amt;
					string text = this.AmtToText(this.LastSpecial3Change, cardData.SpecialDurability3.Max, this.CardLogic.CurrentSpecial3Rate, cardData.SpecialDurability3.TextDisplay, true);
					this.CurrentSpecial3Change = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.SpecialChangedPrefabRight, base.transform);
					if (this.LeftSidePlaying)
					{
						this.CurrentSpecial3Change.Delay = 1f;
					}
					else
					{
						this.RightSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentSpecial3Change.PlayFeedback(this.Special3Value.transform.position, text, this.AmtToColor(this.LastSpecial3Change), cardData.SpecialDurability3.OverrideIcon, Color.white));
				}
				else
				{
					this.LastSpecial3Change += _Amt;
					string text = this.AmtToText(this.LastSpecial3Change, cardData.SpecialDurability3.Max, this.CardLogic.CurrentSpecial3Rate, cardData.SpecialDurability3.TextDisplay, true);
					this.CurrentSpecial3Change.UpdateText(text, this.AmtToColor(this.LastSpecial3Change), true);
				}
			}
			break;
		case DurabilitiesTypes.Special4:
			if (this.SpecialChangedPrefabRight)
			{
				if (!this.CurrentSpecial4Change)
				{
					this.LastSpecial4Change = _Amt;
					string text = this.AmtToText(this.LastSpecial4Change, cardData.SpecialDurability4.Max, this.CardLogic.CurrentSpecial4Rate, cardData.SpecialDurability4.TextDisplay, true);
					this.CurrentSpecial4Change = UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.SpecialChangedPrefabRight, base.transform);
					if (this.LeftSidePlaying)
					{
						this.CurrentSpecial4Change.Delay = 1f;
					}
					else
					{
						this.RightSidePlaying = true;
					}
					base.StartCoroutine(this.CurrentSpecial4Change.PlayFeedback(this.Special4Value.transform.position, text, this.AmtToColor(this.LastSpecial4Change), cardData.SpecialDurability4.OverrideIcon, Color.white));
				}
				else
				{
					this.LastSpecial4Change += _Amt;
					string text = this.AmtToText(this.LastSpecial4Change, cardData.SpecialDurability4.Max, this.CardLogic.CurrentSpecial4Rate, cardData.SpecialDurability4.TextDisplay, true);
					this.CurrentSpecial4Change.UpdateText(text, this.AmtToColor(this.LastSpecial4Change), true);
				}
			}
			break;
		}
		CardGraphics duplicate = this.Duplicate;
		if (duplicate == null)
		{
			return;
		}
		duplicate.ModifyDurability(_Type, _Amt, _UseLiquid);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x000209CC File Offset: 0x0001EBCC
	private string AmtToText(float _Amt, float _Max, float _Rate, DurabilityTextDisplay _Display, bool _ShowPositive)
	{
		switch (_Display)
		{
		case DurabilityTextDisplay.Percent:
			return this.AmtToText(_Amt, _Max, _Rate, DurabilityTextDisplay.PercentNoSymbol, _ShowPositive) + "%";
		case DurabilityTextDisplay.PercentNoSymbol:
		{
			if (_Max <= 0f)
			{
				return this.AmtToText(_Amt, _Max, _Rate, DurabilityTextDisplay.ExplicitPoints, _ShowPositive);
			}
			float num = _Amt / _Max * 100f;
			string format = "0";
			if (!Mathf.Approximately(num, 0f))
			{
				if (Mathf.Abs(num) < 0.1f)
				{
					format = "0.00";
				}
				else if (Mathf.Abs(num) < 1f)
				{
					format = "0.0";
				}
			}
			if (!_ShowPositive)
			{
				return num.ToString(format);
			}
			if (_Amt < 0f)
			{
				return num.ToString(format);
			}
			return "+" + num.ToString(format);
		}
		default:
			if (!_ShowPositive)
			{
				return _Amt.ToString();
			}
			if (_Amt < 0f)
			{
				return _Amt.ToString("0");
			}
			return "+" + _Amt.ToString("0");
		case DurabilityTextDisplay.TimeDisplay:
		{
			if (_Rate <= 0f)
			{
				return this.AmtToText(_Amt, _Max, _Rate, DurabilityTextDisplay.ExplicitPoints, _ShowPositive);
			}
			int num2 = Mathf.CeilToInt(_Amt / _Rate);
			int miniTicksAmt = 0;
			if (this.GM.CurrentMiniTicks != 0)
			{
				num2--;
				miniTicksAmt = this.GM.DaySettings.MiniTicksPerTick - this.GM.CurrentMiniTicks;
			}
			if (!_ShowPositive)
			{
				return HoursDisplay.HoursToShortString(GameManager.TickToHours(num2, miniTicksAmt));
			}
			if (_Amt < 0f)
			{
				return HoursDisplay.HoursToShortString(GameManager.TickToHours(num2, miniTicksAmt));
			}
			return "+" + HoursDisplay.HoursToShortString(GameManager.TickToHours(num2, miniTicksAmt));
		}
		}
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00020B55 File Offset: 0x0001ED55
	private Color AmtToColor(float _Amt)
	{
		if (_Amt < 0f)
		{
			return this.GrM.NegativeTextColor;
		}
		return this.GrM.PositiveTextColor;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00020B78 File Offset: 0x0001ED78
	private void Update()
	{
		if (!this.CurrentSpoilageChange && !this.CurrentUsageChange && !this.CurrentSpecial1Change && !this.CurrentSpecial2Change)
		{
			this.LeftSidePlaying = false;
		}
		if (!this.CurrentFuelChange && !this.CurrentChargesChange && !this.CurrentSpecial3Change && !this.CurrentSpecial4Change)
		{
			this.RightSidePlaying = false;
		}
		if (this.BlueprintReadyIcon)
		{
			if (!this.CardLogic)
			{
				this.BlueprintReadyIcon.SetActive(false);
			}
			else
			{
				this.BlueprintReadyIcon.SetActive(this.CardLogic.ReadyToBuildBlueprint);
			}
		}
		if (this.LiquidFillingNotification)
		{
			if (this.CardLogic)
			{
				if (this.CardLogic.ContainedLiquid != null)
				{
					this.LiquidFillingNotification.SetActive(this.CardLogic.ContainedLiquid.IsFillingWithLiquid || this.LiquidFilledByCooking);
				}
				else if (this.CardLogic.CardModel)
				{
					this.LiquidFillingNotification.SetActive(this.LiquidFilledByCooking && this.CardLogic.CardModel.CanContainLiquid);
				}
				else
				{
					this.LiquidFillingNotification.SetActive(false);
				}
			}
			else
			{
				this.LiquidFillingNotification.SetActive(false);
			}
		}
		if (this.DamageAlertIcon && this.CardLogic)
		{
			if (this.CardLogic.CardModel)
			{
				if (this.CardLogic.CardModel.CardType == CardTypes.Explorable)
				{
					if (this.GM.EnvDamageCards.Count > 0)
					{
						bool active = false;
						for (int i = 0; i < this.GM.EnvDamageCards.Count; i++)
						{
							if (this.GM.EnvDamageCards[i] && !this.GM.EnvDamageCards[i].Destroyed && this.CardLogic.CardModel.HasDamage(this.GM.EnvDamageCards[i].CardModel))
							{
								active = true;
								break;
							}
						}
						this.DamageAlertIcon.SetActive(active);
					}
					else
					{
						this.DamageAlertIcon.SetActive(false);
					}
				}
				else
				{
					this.DamageAlertIcon.SetActive(false);
				}
			}
			else
			{
				this.DamageAlertIcon.SetActive(false);
			}
		}
		bool flag = false;
		if (this.CardLogic)
		{
			if (this.CardLogic.CardModel)
			{
				if (this.CardLogic.CardModel.CardType == CardTypes.EnvImprovement)
				{
					GraphicsManager.SetActiveGroup(this.ImprovementBlueprintObjects, !this.CardLogic.BlueprintComplete);
				}
				else
				{
					GraphicsManager.SetActiveGroup(this.ImprovementBlueprintObjects, false);
				}
				if (this.GM && this.GM.BlueprintPurchasing && this.CardLogic.CardModel.CardType == CardTypes.Blueprint && this.CardLogic.CurrentSlotInfo != null && this.CardLogic.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint && this.GM.BlueprintModelStates[this.CardLogic.CardModel] == BlueprintModelState.Purchasable)
				{
					if (this.SunsCostText)
					{
						this.SunsCostText.text = this.CardLogic.CardModel.BlueprintUnlockSunsCost.ToString();
					}
					flag = true;
				}
			}
			else
			{
				GraphicsManager.SetActiveGroup(this.ImprovementBlueprintObjects, false);
			}
		}
		GraphicsManager.SetActiveGroup(this.PurchasableBlueprintObjects, flag);
		if (flag)
		{
			this.BuyButtonObject.SetActive(!this.GM.PurchasingWithTime);
			if (this.BuyBlueprintButton && !this.GM.PurchasingWithTime)
			{
				this.BuyBlueprintButton.interactable = this.GrM.BlueprintModelsPopup.CanAffordBlueprint(this.CardLogic.CardModel);
				if (this.BuyBlueprintRaycast)
				{
					this.BuyBlueprintRaycast.raycastTarget = this.BuyBlueprintButton.interactable;
				}
			}
			this.ResearchButtonObject.SetActive(this.GM.PurchasingWithTime);
			if (this.GM.PurchasingWithTime)
			{
				int num = GameManager.DaysToTicks(this.CardLogic.CardModel.BlueprintUnlockSunsCost);
				bool flag2 = this.GM.BlueprintResearchTimes.ContainsKey(this.CardLogic.CardModel);
				int num2 = (!flag2) ? -1 : (num - this.GM.BlueprintResearchTimes[this.CardLogic.CardModel]);
				bool flag3 = this.GrM.BlueprintModelsPopup.CurrentResearch != this.CardLogic.CardModel;
				this.ResearchBlueprintButton.interactable = flag3;
				this.ResearchBlueprintRaycast.raycastTarget = flag3;
				GraphicsManager.SetActiveGroup(this.CurrentlyResearchedObjects, !flag3);
				if (flag3)
				{
					if (!flag2)
					{
						this.ResearchText.text = string.Format("{0}\n<size=75%>({1})</size>", LocalizedString.Research, HoursDisplay.HoursToShortString(GameManager.TickToHours(num, 0)));
					}
					else
					{
						this.ResearchText.text = LocalizedString.Resume;
					}
				}
				else
				{
					this.ResearchText.text = LocalizedString.Researching;
				}
				if (flag2)
				{
					this.UpdateCookingProgress(new CookingBarInfo((float)this.GM.BlueprintResearchTimes[this.CardLogic.CardModel] / (float)num, num2, flag3, LocalizedString.BlueprintResearchText(num2, flag3), false, false));
				}
				else
				{
					this.UpdateCookingProgress(null);
				}
			}
		}
		else if (this.CardLogic)
		{
			if (this.CardLogic.CardModel && this.CardLogic.CardModel.CardType == CardTypes.Blueprint)
			{
				this.UpdateCookingProgress(null);
			}
			GraphicsManager.SetActiveGroup(this.CurrentlyResearchedObjects, false);
		}
		if (this.CardLogic && this.CardNotificationsTr)
		{
			if (this.CardLogic.CurrentSlot)
			{
				if (this.CardLogic.CurrentSlot.SlotObject)
				{
					this.LastCardSlot = this.CardLogic.CurrentSlot.SlotObject.transform;
				}
				else
				{
					this.LastCardSlot = null;
				}
			}
			if (this.LastCardSlot)
			{
				this.CardNotificationsTr.position = this.LastCardSlot.position;
			}
			else
			{
				this.CardNotificationsTr.position = base.transform.position;
			}
		}
		if (this.BookmarkImage)
		{
			if (this.CardLogic && this.GrM)
			{
				int bookmarkIndex = this.GrM.GetBookmarkIndex(this.CardLogic.CardModel, this.CardLogic.ContainedLiquidModel);
				if (bookmarkIndex != -1)
				{
					this.BookmarkImage.gameObject.SetActive(this.CardLogic != GameManager.DraggedCard && this.CardLogic.CurrentSlotInfo.SlotType != SlotsTypes.Item && !this.CardLogic.CurrentContainer);
					this.BookmarkImage.color = this.GrM.Bookmarks[bookmarkIndex].BookmarkColor;
					if (this.BookmarkText)
					{
						this.BookmarkText.text = (bookmarkIndex + 1).ToString();
					}
				}
				else
				{
					this.BookmarkImage.gameObject.SetActive(false);
				}
			}
			else
			{
				this.BookmarkImage.gameObject.SetActive(false);
			}
		}
		if (this.CardLogic && this.CardLogic.CardModel)
		{
			if (this.CardLogic.CardModel.CardType != CardTypes.Explorable)
			{
				if (this.CardLogic.TravelToData != null)
				{
					if (this.InventoryBarIndicator)
					{
						this.InventoryBarIndicator.fillAmount = this.CardLogic.TravelToData.CurrentWeight / this.CardLogic.TravelToData.CurrentMaxWeight;
					}
				}
				else if (this.CardLogic.CardsInInventory != null)
				{
					if (this.CardLogic.IsLegacyInventory)
					{
						for (int j = 0; j < this.CardLogic.CardsInInventory.Count; j++)
						{
							this.InventoryIndicators[j].Update(this.CardLogic.CardsInInventory[j]);
						}
					}
					else if (this.InventoryBarIndicator)
					{
						this.InventoryBarIndicator.fillAmount = this.CardLogic.InventoryWeight(true) / this.CardLogic.MaxWeightCapacity;
					}
					else
					{
						for (int k = 0; k < this.InventoryIndicators.Count; k++)
						{
							this.InventoryIndicators[k].SetIndicator(this.CardLogic.InventoryWeight(true) >= GraphicsManager.WeightPerCircle * (float)(k + 1));
						}
					}
				}
			}
			else if (this.GM)
			{
				if (this.InventoryBarIndicatorParent)
				{
					this.InventoryBarIndicatorParent.SetActive(this.GM.MaxEnvWeight > 0f);
				}
				if (this.InventoryBarIndicator)
				{
					this.InventoryBarIndicator.fillAmount = this.GM.CurrentEnvWeight / this.GM.MaxEnvWeight;
				}
			}
		}
		if (!MBSingleton<CameraModeSwitch>.Instance)
		{
			return;
		}
		if (!MBSingleton<CameraModeSwitch>.Instance.IsPerspective)
		{
			this.MovementRotationTransform.rotation = Quaternion.identity;
			return;
		}
		Vector3 vector = base.transform.position - this.PreviousPos;
		this.XAngle = Mathf.Lerp(this.XAngle, 0f, this.RotationResetForce * Time.deltaTime);
		this.YAngle = Mathf.Lerp(this.YAngle, 0f, this.RotationResetForce * Time.deltaTime);
		this.XAngle += vector.y * this.MovementToRotation;
		this.YAngle -= vector.x * this.MovementToRotation;
		this.XAngle = Mathf.Clamp(this.XAngle, -this.MaxMovementRot.x, this.MaxMovementRot.x);
		this.YAngle = Mathf.Clamp(this.YAngle, -this.MaxMovementRot.y, this.MaxMovementRot.y);
		this.MovementRotationTransform.rotation = Quaternion.Euler(this.XAngle, this.YAngle, 0f);
		this.PreviousPos = base.transform.position;
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00021618 File Offset: 0x0001F818
	public void RefreshDurabilities()
	{
		if (!this.CardLogic)
		{
			this.SpoilageValue.gameObject.SetActive(false);
			this.UsageValue.gameObject.SetActive(false);
			this.FuelValue.gameObject.SetActive(false);
			this.ChargesValue.gameObject.SetActive(false);
			this.LiquidValue.gameObject.SetActive(false);
			this.LiquidSpoilageValue.gameObject.SetActive(false);
			this.LiquidUsageValue.gameObject.SetActive(false);
			this.LiquidFuelValue.gameObject.SetActive(false);
			this.LiquidProgressValue.gameObject.SetActive(false);
			this.Special1Value.gameObject.SetActive(false);
			this.Special2Value.gameObject.SetActive(false);
			this.Special3Value.gameObject.SetActive(false);
			this.Special4Value.gameObject.SetActive(false);
			this.LiquidSpecial1Value.gameObject.SetActive(false);
			this.LiquidSpecial2Value.gameObject.SetActive(false);
			this.LiquidSpecial3Value.gameObject.SetActive(false);
			this.LiquidSpecial4Value.gameObject.SetActive(false);
			return;
		}
		InGameCardBase containedLiquid = this.CardLogic.ContainedLiquid;
		this.SpoilageValue.gameObject.SetActive(this.CardLogic.CardModel.SpoilageTime.Show(containedLiquid, this.CardLogic.CurrentSpoilage) && !this.CardLogic.IsPinned);
		this.SpoilageValue.Sprite = this.CardLogic.CardModel.SpoilageTime.OverrideIcon;
		this.SpoilageValue.Text = this.AmtToText(this.CardLogic.CurrentSpoilage, this.CardLogic.CardModel.SpoilageTime.Max, this.CardLogic.CurrentSpoilageRate, this.CardLogic.CardModel.SpoilageTime.TextDisplay, false);
		this.UsageValue.gameObject.SetActive(this.CardLogic.CardModel.UsageDurability.Show(containedLiquid, this.CardLogic.CurrentUsageDurability) && !this.CardLogic.IsPinned);
		this.UsageValue.Sprite = this.CardLogic.CardModel.UsageDurability.OverrideIcon;
		this.UsageValue.Text = this.AmtToText(this.CardLogic.CurrentUsageDurability, this.CardLogic.CardModel.UsageDurability.Max, this.CardLogic.CurrentUsageRate, this.CardLogic.CardModel.UsageDurability.TextDisplay, false);
		this.FuelValue.gameObject.SetActive(this.CardLogic.CardModel.FuelCapacity.Show(containedLiquid, this.CardLogic.CurrentFuel) && !this.CardLogic.IsPinned);
		this.FuelValue.Sprite = this.CardLogic.CardModel.FuelCapacity.OverrideIcon;
		this.FuelValue.Text = this.AmtToText(this.CardLogic.CurrentFuel, this.CardLogic.CardModel.FuelCapacity.Max, this.CardLogic.CurrentFuelRate, this.CardLogic.CardModel.FuelCapacity.TextDisplay, false);
		this.ChargesValue.gameObject.SetActive(this.CardLogic.CardModel.Progress.Show(containedLiquid, this.CardLogic.CurrentProgress) && !this.CardLogic.IsPinned);
		this.ChargesValue.Sprite = this.CardLogic.CardModel.Progress.OverrideIcon;
		this.ChargesValue.Text = this.AmtToText(this.CardLogic.CurrentProgress, this.CardLogic.CardModel.Progress.Max, this.CardLogic.CurrentConsumableRate, this.CardLogic.CardModel.Progress.TextDisplay, false);
		this.Special1Value.gameObject.SetActive(this.CardLogic.CardModel.SpecialDurability1.Show(containedLiquid, this.CardLogic.CurrentSpecial1) && !this.CardLogic.IsPinned);
		this.Special1Value.Sprite = this.CardLogic.CardModel.SpecialDurability1.OverrideIcon;
		this.Special1Value.Text = this.AmtToText(this.CardLogic.CurrentSpecial1, this.CardLogic.CardModel.SpecialDurability1.Max, this.CardLogic.CurrentSpecial1Rate, this.CardLogic.CardModel.SpecialDurability1.TextDisplay, false);
		this.Special2Value.gameObject.SetActive(this.CardLogic.CardModel.SpecialDurability2.Show(containedLiquid, this.CardLogic.CurrentSpecial2) && !this.CardLogic.IsPinned);
		this.Special2Value.Sprite = this.CardLogic.CardModel.SpecialDurability2.OverrideIcon;
		this.Special2Value.Text = this.AmtToText(this.CardLogic.CurrentSpecial2, this.CardLogic.CardModel.SpecialDurability2.Max, this.CardLogic.CurrentSpecial2Rate, this.CardLogic.CardModel.SpecialDurability2.TextDisplay, false);
		this.Special3Value.gameObject.SetActive(this.CardLogic.CardModel.SpecialDurability3.Show(containedLiquid, this.CardLogic.CurrentSpecial3) && !this.CardLogic.IsPinned);
		this.Special3Value.Sprite = this.CardLogic.CardModel.SpecialDurability3.OverrideIcon;
		this.Special3Value.Text = this.AmtToText(this.CardLogic.CurrentSpecial3, this.CardLogic.CardModel.SpecialDurability3.Max, this.CardLogic.CurrentSpecial3Rate, this.CardLogic.CardModel.SpecialDurability3.TextDisplay, false);
		this.Special4Value.gameObject.SetActive(this.CardLogic.CardModel.SpecialDurability4.Show(containedLiquid, this.CardLogic.CurrentSpecial4) && !this.CardLogic.IsPinned);
		this.Special4Value.Sprite = this.CardLogic.CardModel.SpecialDurability4.OverrideIcon;
		this.Special4Value.Text = this.AmtToText(this.CardLogic.CurrentSpecial4, this.CardLogic.CardModel.SpecialDurability4.Max, this.CardLogic.CurrentSpecial4Rate, this.CardLogic.CardModel.SpecialDurability4.TextDisplay, false);
		if (containedLiquid)
		{
			this.LiquidValue.gameObject.SetActive(!this.CardLogic.IsPinned);
			this.LiquidValue.Sprite = containedLiquid.CardModel.CardImage;
			this.LiquidValue.Text = this.AmtToText(containedLiquid.CurrentLiquidQuantity, containedLiquid.CurrentMaxLiquidQuantity, containedLiquid.CurrentEvaporationRate, DurabilityTextDisplay.Percent, false);
			this.LiquidSpoilageValue.gameObject.SetActive(containedLiquid.CardModel.SpoilageTime.Show(false, containedLiquid.CurrentSpoilage) && !this.CardLogic.IsPinned);
			this.LiquidSpoilageValue.Sprite = containedLiquid.CardModel.SpoilageTime.OverrideIcon;
			this.LiquidSpoilageValue.Text = this.AmtToText(containedLiquid.CurrentSpoilage, containedLiquid.CardModel.SpoilageTime.Max, containedLiquid.CurrentSpoilageRate, containedLiquid.CardModel.SpoilageTime.TextDisplay, false);
			this.LiquidUsageValue.gameObject.SetActive(containedLiquid.CardModel.UsageDurability.Show(false, containedLiquid.CurrentUsageDurability) && !this.CardLogic.IsPinned);
			this.LiquidUsageValue.Sprite = containedLiquid.CardModel.UsageDurability.OverrideIcon;
			this.LiquidUsageValue.Text = this.AmtToText(containedLiquid.CurrentUsageDurability, containedLiquid.CardModel.UsageDurability.Max, containedLiquid.CurrentUsageRate, containedLiquid.CardModel.UsageDurability.TextDisplay, false);
			this.LiquidFuelValue.gameObject.SetActive(containedLiquid.CardModel.FuelCapacity.Show(false, containedLiquid.CurrentFuel) && !this.CardLogic.IsPinned);
			this.LiquidFuelValue.Sprite = containedLiquid.CardModel.FuelCapacity.OverrideIcon;
			this.LiquidFuelValue.Text = this.AmtToText(containedLiquid.CurrentFuel, containedLiquid.CardModel.FuelCapacity.Max, containedLiquid.CurrentFuelRate, containedLiquid.CardModel.FuelCapacity.TextDisplay, false);
			this.LiquidProgressValue.gameObject.SetActive(containedLiquid.CardModel.Progress.Show(false, containedLiquid.CurrentProgress) && !this.CardLogic.IsPinned);
			this.LiquidProgressValue.Sprite = containedLiquid.CardModel.Progress.OverrideIcon;
			this.LiquidProgressValue.Text = this.AmtToText(containedLiquid.CurrentProgress, containedLiquid.CardModel.Progress.Max, containedLiquid.CurrentConsumableRate, containedLiquid.CardModel.Progress.TextDisplay, false);
			this.LiquidSpecial1Value.gameObject.SetActive(containedLiquid.CardModel.SpecialDurability1.Show(containedLiquid, containedLiquid.CurrentSpecial1) && !this.CardLogic.IsPinned);
			this.LiquidSpecial1Value.Sprite = containedLiquid.CardModel.SpecialDurability1.OverrideIcon;
			this.LiquidSpecial1Value.Text = this.AmtToText(containedLiquid.CurrentSpecial1, containedLiquid.CardModel.SpecialDurability1.Max, containedLiquid.CurrentSpecial1Rate, containedLiquid.CardModel.SpecialDurability1.TextDisplay, false);
			this.LiquidSpecial2Value.gameObject.SetActive(containedLiquid.CardModel.SpecialDurability2.Show(containedLiquid, containedLiquid.CurrentSpecial2) && !this.CardLogic.IsPinned);
			this.LiquidSpecial2Value.Sprite = containedLiquid.CardModel.SpecialDurability2.OverrideIcon;
			this.LiquidSpecial2Value.Text = this.AmtToText(containedLiquid.CurrentSpecial2, containedLiquid.CardModel.SpecialDurability2.Max, containedLiquid.CurrentSpecial2Rate, containedLiquid.CardModel.SpecialDurability2.TextDisplay, false);
			this.LiquidSpecial3Value.gameObject.SetActive(containedLiquid.CardModel.SpecialDurability3.Show(containedLiquid, containedLiquid.CurrentSpecial3) && !this.CardLogic.IsPinned);
			this.LiquidSpecial3Value.Sprite = containedLiquid.CardModel.SpecialDurability3.OverrideIcon;
			this.LiquidSpecial3Value.Text = this.AmtToText(containedLiquid.CurrentSpecial3, containedLiquid.CardModel.SpecialDurability3.Max, containedLiquid.CurrentSpecial3Rate, containedLiquid.CardModel.SpecialDurability3.TextDisplay, false);
			this.LiquidSpecial4Value.gameObject.SetActive(containedLiquid.CardModel.SpecialDurability4.Show(containedLiquid, containedLiquid.CurrentSpecial4) && !this.CardLogic.IsPinned);
			this.LiquidSpecial4Value.Sprite = containedLiquid.CardModel.SpecialDurability4.OverrideIcon;
			this.LiquidSpecial4Value.Text = this.AmtToText(containedLiquid.CurrentSpecial4, containedLiquid.CardModel.SpecialDurability4.Max, containedLiquid.CurrentSpecial4Rate, containedLiquid.CardModel.SpecialDurability4.TextDisplay, false);
		}
		else
		{
			this.LiquidValue.gameObject.SetActive(false);
			this.LiquidSpoilageValue.gameObject.SetActive(false);
			this.LiquidUsageValue.gameObject.SetActive(false);
			this.LiquidFuelValue.gameObject.SetActive(false);
			this.LiquidProgressValue.gameObject.SetActive(false);
			this.LiquidSpecial1Value.gameObject.SetActive(false);
			this.LiquidSpecial2Value.gameObject.SetActive(false);
			this.LiquidSpecial3Value.gameObject.SetActive(false);
			this.LiquidSpecial4Value.gameObject.SetActive(false);
		}
		this.RefreshCookingStatus();
		CardGraphics duplicate = this.Duplicate;
		if (duplicate == null)
		{
			return;
		}
		duplicate.RefreshDurabilities();
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00022300 File Offset: 0x00020500
	public void SetEventProbabilities()
	{
		if (!this.EventProbabilitiesText || !this.GM)
		{
			return;
		}
		if (this.GM.CurrentDismantleActions.Count == 0)
		{
			GraphicsManager.SetActiveGroup(this.EventProbabilitiesObjects, false);
			this.EventProbabilitiesText.text = "";
			return;
		}
		float addedSuccessFromCard = MBSingleton<GameManager>.Instance.CurrentDismantleActions[0].GetAddedSuccessFromCard(this.CardLogic);
		if (Mathf.Approximately(addedSuccessFromCard, 0f))
		{
			GraphicsManager.SetActiveGroup(this.EventProbabilitiesObjects, false);
			this.EventProbabilitiesText.text = "";
			return;
		}
		if (this.GM && this.GM.SuccessChances && !string.IsNullOrEmpty(this.GM.SuccessChances.GetContributionLabel(addedSuccessFromCard)))
		{
			GraphicsManager.SetActiveGroup(this.EventProbabilitiesObjects, true);
			this.EventProbabilitiesText.text = this.GM.SuccessChances.GetContributionLabel(addedSuccessFromCard);
			return;
		}
		GraphicsManager.SetActiveGroup(this.EventProbabilitiesObjects, true);
		this.EventProbabilitiesText.text = LocalizedString.SuccessChance + " +" + (addedSuccessFromCard * 100f).ToString("0") + "%";
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00022448 File Offset: 0x00020648
	public void SetGraphicState(CardGraphics.CardGraphicsStates _State, InGameCardBase _InventoryCard)
	{
		if (this.CardLogic && this.CardLogic.IsPinned && _State != CardGraphics.CardGraphicsStates.Pinned)
		{
			return;
		}
		this.SetAllStatesFalse(_State);
		switch (_State)
		{
		case CardGraphics.CardGraphicsStates.Normal:
			GraphicsManager.SetActiveGroup(this.NormalStateObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.Highlighted:
			GraphicsManager.SetActiveGroup(this.HighlightedObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.GreyedOut:
			GraphicsManager.SetActiveGroup(this.GreyedOutObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.Inspected:
			GraphicsManager.SetActiveGroup(this.InspectedObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.MissingRequirements:
			GraphicsManager.SetActiveGroup(this.MissingRequirementsObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.Pinned:
			GraphicsManager.SetActiveGroup(this.PinnedObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.InventoryDisplay:
			GraphicsManager.SetActiveGroup(this.InventoryDisplayObjects, true);
			if (this.InventoryText)
			{
				if (!_InventoryCard)
				{
					this.InventoryText.text = "";
					return;
				}
				if (this.CardLogic.IsLegacyInventory)
				{
					this.InventoryText.text = this.CardLogic.InventoryCount(_InventoryCard.CardModel).ToString() + "/" + this.CardLogic.MaxInventoryContent(_InventoryCard, true).ToString();
					return;
				}
				this.InventoryText.text = Mathf.FloorToInt(this.CardLogic.InventoryWeight(true) / this.CardLogic.MaxWeightCapacity * 100f).ToString() + "%";
				return;
			}
			break;
		case CardGraphics.CardGraphicsStates.ExplorationNormal:
			GraphicsManager.SetActiveGroup(this.ExplorationNormalObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.ExplorationSelected:
			GraphicsManager.SetActiveGroup(this.ExplorationSelectedObjects, true);
			return;
		case CardGraphics.CardGraphicsStates.ExplorationAutoSelected:
			GraphicsManager.SetActiveGroup(this.ExplorationAutoSelectedObjects, true);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x000225E8 File Offset: 0x000207E8
	private void SetAllStatesFalse(CardGraphics.CardGraphicsStates _Exception)
	{
		if (_Exception != CardGraphics.CardGraphicsStates.Normal)
		{
			GraphicsManager.SetActiveGroup(this.NormalStateObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.Highlighted)
		{
			GraphicsManager.SetActiveGroup(this.HighlightedObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.GreyedOut)
		{
			GraphicsManager.SetActiveGroup(this.GreyedOutObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.Inspected)
		{
			GraphicsManager.SetActiveGroup(this.InspectedObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.MissingRequirements)
		{
			GraphicsManager.SetActiveGroup(this.MissingRequirementsObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.Pinned)
		{
			GraphicsManager.SetActiveGroup(this.PinnedObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.InventoryDisplay)
		{
			GraphicsManager.SetActiveGroup(this.InventoryDisplayObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.ExplorationNormal)
		{
			GraphicsManager.SetActiveGroup(this.ExplorationNormalObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.ExplorationSelected)
		{
			GraphicsManager.SetActiveGroup(this.ExplorationSelectedObjects, false);
		}
		if (_Exception != CardGraphics.CardGraphicsStates.ExplorationAutoSelected)
		{
			GraphicsManager.SetActiveGroup(this.ExplorationAutoSelectedObjects, false);
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00022698 File Offset: 0x00020898
	public void UpdateMissingRequirements(MissingReqInfo _Info)
	{
		if (_Info == null)
		{
			return;
		}
		if (_Info.Stat == null && string.IsNullOrEmpty(_Info.MissingDurabilities) && string.IsNullOrEmpty(_Info.BlockingStatus) && !_Info.ActionPlaying)
		{
			if (this.MissingRequirementsIcon)
			{
				this.MissingRequirementsIcon.overrideSprite = this.NotEnoughTimeSprite;
			}
			if (this.MissingRequirementsText)
			{
				this.MissingRequirementsText.text = LocalizedString.ImpossibleAction;
			}
			return;
		}
		if (_Info.UnavailableInDemo)
		{
			if (this.MissingRequirementsIcon)
			{
				this.MissingRequirementsIcon.overrideSprite = null;
			}
			if (this.MissingRequirementsText)
			{
				this.MissingRequirementsText.text = LocalizedString.UnavailableInDemo;
			}
			return;
		}
		if (_Info.ActionPlaying)
		{
			if (this.MissingRequirementsIcon)
			{
				this.MissingRequirementsIcon.overrideSprite = null;
			}
			if (this.MissingRequirementsText)
			{
				this.MissingRequirementsText.text = LocalizedString.ActionHappening;
			}
			return;
		}
		if (!string.IsNullOrEmpty(_Info.MissingDurabilities))
		{
			if (this.MissingRequirementsIcon)
			{
				this.MissingRequirementsIcon.overrideSprite = null;
			}
			if (this.MissingRequirementsText)
			{
				this.MissingRequirementsText.text = _Info.MissingDurabilities;
				return;
			}
		}
		else if (_Info.Stat != null)
		{
			if (this.MissingRequirementsIcon)
			{
				this.MissingRequirementsIcon.overrideSprite = _Info.Stat.Stat.NotEnoughIcon;
			}
			if (this.MissingRequirementsText)
			{
				this.MissingRequirementsText.text = _Info.Stat.GetNotification;
				return;
			}
		}
		else if (!string.IsNullOrEmpty(_Info.BlockingStatus))
		{
			if (this.MissingRequirementsIcon)
			{
				this.MissingRequirementsIcon.overrideSprite = null;
			}
			if (this.MissingRequirementsText)
			{
				this.MissingRequirementsText.text = _Info.BlockingStatus;
			}
		}
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0002287C File Offset: 0x00020A7C
	public void RefreshCookingStatus()
	{
		if (this.CookingSprite)
		{
			this.CardImage.overrideSprite = (this.CardLogic.IsCooking() ? this.CookingSprite : this.CardLogic.CurrentImage);
		}
		else
		{
			this.CardImage.overrideSprite = this.CardLogic.CurrentImage;
		}
		if (this.CookedContentNotification)
		{
			this.CookedContentNotification.SetActive(this.CardLogic.ContainsCookedContent());
		}
		if (this.CookingContentNotification)
		{
			this.CookingContentNotification.SetActive(this.CardLogic.IsCooking());
		}
		if (this.CookingPausedNotification)
		{
			this.CookingPausedNotification.SetActive(this.CardLogic.CookingIsPaused() || this.CardLogic.OneOrMoreRecipesArePaused());
		}
		for (int i = 0; i < this.CardLogic.CookingCards.Count; i++)
		{
			if (this.CardLogic.CookingCards[i].Card)
			{
				this.CardLogic.CookingCards[i].UpdateCookingProgressVisuals((float)this.CardLogic.CookingCards[i].CookedDuration / (float)this.CardLogic.CookingCards[i].TargetDuration, this.CardLogic.CookingCards[i].TargetDuration - this.CardLogic.CookingCards[i].CookedDuration, this.CardLogic.CookingIsPaused() || this.CardLogic.CookingCards[i].SelfPaused, this.CardLogic.CookingCards[i].GetCookingText(this.CardLogic.CookingIsPaused(), this.CardLogic.CookingCards[i].TargetDuration - this.CardLogic.CookingCards[i].CookedDuration));
			}
		}
		CardGraphics duplicate = this.Duplicate;
		if (duplicate == null)
		{
			return;
		}
		duplicate.RefreshCookingStatus();
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00022A8C File Offset: 0x00020C8C
	public void UpdateCookingProgress(CookingBarInfo _Info)
	{
		if (_Info == null)
		{
			if (this.CookingBarObjects)
			{
				this.CookingBarObjects.SetActive(false);
			}
			CardGraphics duplicate = this.Duplicate;
			if (duplicate == null)
			{
				return;
			}
			duplicate.UpdateCookingProgress(_Info);
			return;
		}
		else
		{
			if (this.CookingBarObjects)
			{
				this.CookingBarObjects.SetActive((_Info.Value > 0f || _Info.RemainingTicks > 0) && !_Info.HideProgress);
			}
			if (this.CookingBar)
			{
				this.CookingBar.fillAmount = _Info.Value;
			}
			if (this.CookingText)
			{
				this.CookingText.text = _Info.CustomCookingText;
			}
			this.LiquidFilledByCooking = (_Info.FillsLiquid && _Info.HideProgress);
			GraphicsManager.SetActiveGroup(this.CookingBarPausedObjects, _Info.Paused);
			CardGraphics duplicate2 = this.Duplicate;
			if (duplicate2 == null)
			{
				return;
			}
			duplicate2.UpdateCookingProgress(_Info);
			return;
		}
	}

	// Token: 0x0600033A RID: 826 RVA: 0x00022B78 File Offset: 0x00020D78
	public void RefreshPileInfo(int _Count, int _TotalCount)
	{
		if (this.CardStackObject)
		{
			this.CardStackObject.SetActive(_Count > 1);
		}
		if (this.CardStackNumber)
		{
			this.CardStackNumber.text = "x" + _Count.ToString();
		}
		CardGraphics duplicate = this.Duplicate;
		if (duplicate != null)
		{
			duplicate.RefreshPileInfo(_Count, _TotalCount);
		}
		this.HiddenInPile = (_Count < _TotalCount && _TotalCount > 1);
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00022BF0 File Offset: 0x00020DF0
	public Transform CreateDuplicate(Transform _Parent)
	{
		if (this.Duplicate)
		{
			return this.Duplicate.transform;
		}
		this.Duplicate = UnityEngine.Object.Instantiate<CardGraphics>(this, _Parent);
		this.Duplicate.transform.localPosition = Vector3.zero;
		if (this.Duplicate.GetComponent<InGameCardBase>())
		{
			UnityEngine.Object.Destroy(this.Duplicate.GetComponent<InGameCardBase>());
		}
		if (this.InventoryIndicators != null)
		{
			for (int i = 0; i < this.InventoryIndicators.Count; i++)
			{
				UnityEngine.Object.Destroy(this.Duplicate.InventoryIndicatorsParent.GetChild(i).gameObject);
			}
		}
		this.Duplicate.Setup(this.CardLogic);
		this.Duplicate.SetGraphicState(CardGraphics.CardGraphicsStates.Inspected, null);
		this.Duplicate.IsDuplicate = true;
		GraphicsManager.SetActiveGroup(this.TutorialHighlightedObjects, false);
		return this.Duplicate.transform;
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00022CD4 File Offset: 0x00020ED4
	public void OnLogicReset()
	{
		if (this.FullCollision)
		{
			this.FullCollision.enabled = false;
		}
		if (this.SmallCollision)
		{
			this.SmallCollision.enabled = false;
		}
		if (this.CardNotificationsTr)
		{
			if (this.CardLogic)
			{
				this.CardNotificationsTr.transform.SetParent(this.CardLogic.CurrentParentObject);
			}
			UnityEngine.Object.Destroy(this.CardNotificationsTr.gameObject, 5f);
		}
		this.DestroyDuplicate();
		this.LastSpoilageChange = 0f;
		this.CurrentSpoilageChange = null;
		this.LastUsageChange = 0f;
		this.CurrentUsageChange = null;
		this.LastFuelChange = 0f;
		this.CurrentFuelChange = null;
		this.LastChargesChange = 0f;
		this.CurrentChargesChange = null;
		this.LastSpecial1Change = 0f;
		this.CurrentSpecial1Change = null;
		this.LastSpecial2Change = 0f;
		this.CurrentSpecial2Change = null;
		this.LastSpecial3Change = 0f;
		this.CurrentSpecial3Change = null;
		this.LastSpecial4Change = 0f;
		this.CurrentSpecial4Change = null;
		this.XAngle = 0f;
		this.YAngle = 0f;
		this.PreviousPos = Vector3.zero;
		this.CardLogic = null;
		this.DraggableCardLogic = null;
		this.UpdateInventoryInfo();
		this.UpdateCookingProgress(null);
		this.HiddenInPile = false;
		GraphicsManager.SetActiveGroup(this.TutorialHighlightedObjects, false);
		GraphicsManager.SetActiveGroup(this.ActionPerformedObjects, false);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00022E50 File Offset: 0x00021050
	public void OnLogicDestroyed()
	{
		if (this.FullCollision)
		{
			this.FullCollision.enabled = false;
		}
		if (this.SmallCollision)
		{
			this.SmallCollision.enabled = false;
		}
		if (this.CardNotificationsTr)
		{
			this.CardNotificationsTr.transform.SetParent(this.CardLogic.CurrentParentObject);
			UnityEngine.Object.Destroy(this.CardNotificationsTr.gameObject, 5f);
		}
		this.DestroyDuplicate();
		GraphicsManager.SetActiveGroup(this.TutorialHighlightedObjects, false);
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00022EDE File Offset: 0x000210DE
	public void DestroyDuplicate()
	{
		if (!this.Duplicate)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.Duplicate.gameObject);
		this.Duplicate = null;
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00022F05 File Offset: 0x00021105
	public void OnDrop(PointerEventData _Pointer)
	{
		if (!this.CardLogic || this.IsDuplicate)
		{
			return;
		}
		this.CardLogic.OnDrop(_Pointer);
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00022F29 File Offset: 0x00021129
	public void OnPointerClick(PointerEventData _Pointer)
	{
		if (!this.CardLogic || this.IsDuplicate)
		{
			return;
		}
		this.CardLogic.OnPointerClick(_Pointer);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00022F4D File Offset: 0x0002114D
	public void OnPointerEnter(PointerEventData _Pointer)
	{
		if (!this.CardLogic || this.IsDuplicate)
		{
			return;
		}
		this.CardLogic.OnHoverEnter();
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00022F70 File Offset: 0x00021170
	public void OnPointerExit(PointerEventData _Pointer)
	{
		if (!this.CardLogic || this.IsDuplicate)
		{
			return;
		}
		this.CardLogic.OnHoverExit();
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00022F93 File Offset: 0x00021193
	public void OnBeginDrag(PointerEventData _Pointer)
	{
		if (this.DraggableCardLogic)
		{
			this.DraggableCardLogic.OnBeginDrag(_Pointer);
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00022FAE File Offset: 0x000211AE
	public void OnDrag(PointerEventData _Pointer)
	{
		if (this.DraggableCardLogic)
		{
			this.DraggableCardLogic.OnDrag(_Pointer);
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00022FC9 File Offset: 0x000211C9
	public void OnEndDrag(PointerEventData _Pointer)
	{
		if (this.DraggableCardLogic)
		{
			this.DraggableCardLogic.OnEndDrag(_Pointer);
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00022FE4 File Offset: 0x000211E4
	public void BuyBlueprint()
	{
		if (this.CardLogic)
		{
			this.CardLogic.BuyBlueprint();
		}
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00022FFE File Offset: 0x000211FE
	public void OpenBlueprintGuidePage()
	{
		if (this.CardLogic)
		{
			this.CardLogic.OpenBlueprintGuidePage();
		}
	}

	// Token: 0x040003C2 RID: 962
	public const float CardWidth = 800f;

	// Token: 0x040003C3 RID: 963
	public const float CardHeight = 1200f;

	// Token: 0x040003C4 RID: 964
	public bool DontBlockRaycasts;

	// Token: 0x040003C5 RID: 965
	public Graphic FullCollision;

	// Token: 0x040003C6 RID: 966
	public Graphic SmallCollision;

	// Token: 0x040003C8 RID: 968
	public Transform MovementRotationTransform;

	// Token: 0x040003C9 RID: 969
	public Vector2 MaxMovementRot;

	// Token: 0x040003CA RID: 970
	public float MovementToRotation;

	// Token: 0x040003CB RID: 971
	public float RotationResetForce;

	// Token: 0x040003CC RID: 972
	public Transform TimeAnimationTr;

	// Token: 0x040003CD RID: 973
	public Image CardImage;

	// Token: 0x040003CE RID: 974
	public Image CardBG;

	// Token: 0x040003CF RID: 975
	public TextMeshProUGUI CardTitle;

	// Token: 0x040003D0 RID: 976
	public GameObject CardStackObject;

	// Token: 0x040003D1 RID: 977
	public TextMeshProUGUI CardStackNumber;

	// Token: 0x040003D2 RID: 978
	public Image CookingBar;

	// Token: 0x040003D3 RID: 979
	public TextMeshProUGUI CookingText;

	// Token: 0x040003D4 RID: 980
	public GameObject CookingBarObjects;

	// Token: 0x040003D5 RID: 981
	public GameObject[] CookingBarPausedObjects;

	// Token: 0x040003D6 RID: 982
	public GameObject CookedContentNotification;

	// Token: 0x040003D7 RID: 983
	public GameObject CookingContentNotification;

	// Token: 0x040003D8 RID: 984
	public GameObject CookingPausedNotification;

	// Token: 0x040003D9 RID: 985
	public GameObject LiquidFillingNotification;

	// Token: 0x040003DA RID: 986
	public IconAndTextPair SpoilageValue;

	// Token: 0x040003DB RID: 987
	public IconAndTextPair UsageValue;

	// Token: 0x040003DC RID: 988
	public IconAndTextPair FuelValue;

	// Token: 0x040003DD RID: 989
	public IconAndTextPair ChargesValue;

	// Token: 0x040003DE RID: 990
	public IconAndTextPair LiquidValue;

	// Token: 0x040003DF RID: 991
	public IconAndTextPair LiquidSpoilageValue;

	// Token: 0x040003E0 RID: 992
	public IconAndTextPair LiquidUsageValue;

	// Token: 0x040003E1 RID: 993
	public IconAndTextPair LiquidFuelValue;

	// Token: 0x040003E2 RID: 994
	public IconAndTextPair LiquidProgressValue;

	// Token: 0x040003E3 RID: 995
	public IconAndTextPair Special1Value;

	// Token: 0x040003E4 RID: 996
	public IconAndTextPair Special2Value;

	// Token: 0x040003E5 RID: 997
	public IconAndTextPair Special3Value;

	// Token: 0x040003E6 RID: 998
	public IconAndTextPair Special4Value;

	// Token: 0x040003E7 RID: 999
	public IconAndTextPair LiquidSpecial1Value;

	// Token: 0x040003E8 RID: 1000
	public IconAndTextPair LiquidSpecial2Value;

	// Token: 0x040003E9 RID: 1001
	public IconAndTextPair LiquidSpecial3Value;

	// Token: 0x040003EA RID: 1002
	public IconAndTextPair LiquidSpecial4Value;

	// Token: 0x040003EB RID: 1003
	public GameObject TravelIcon;

	// Token: 0x040003EC RID: 1004
	public GameObject BlueprintIcon;

	// Token: 0x040003ED RID: 1005
	public GameObject DamageAlertIcon;

	// Token: 0x040003EE RID: 1006
	public GameObject BlueprintReadyIcon;

	// Token: 0x040003EF RID: 1007
	public Image BookmarkImage;

	// Token: 0x040003F0 RID: 1008
	public TextMeshProUGUI BookmarkText;

	// Token: 0x040003F1 RID: 1009
	public Toggle InventoryIndicatorPrefab;

	// Token: 0x040003F2 RID: 1010
	public RectTransform InventoryIndicatorsParent;

	// Token: 0x040003F3 RID: 1011
	public GameObject InventoryBarIndicatorParent;

	// Token: 0x040003F4 RID: 1012
	public Image InventoryBarIndicator;

	// Token: 0x040003F6 RID: 1014
	private InGameDraggableCard DraggableCardLogic;

	// Token: 0x040003F7 RID: 1015
	public GameObject[] NormalStateObjects;

	// Token: 0x040003F8 RID: 1016
	public GameObject[] ActionPerformedObjects;

	// Token: 0x040003F9 RID: 1017
	public GameObject[] HighlightedObjects;

	// Token: 0x040003FA RID: 1018
	public GameObject[] GreyedOutObjects;

	// Token: 0x040003FB RID: 1019
	public GameObject[] InspectedObjects;

	// Token: 0x040003FC RID: 1020
	[FormerlySerializedAs("NotEnoughTimeObject")]
	public GameObject[] MissingRequirementsObjects;

	// Token: 0x040003FD RID: 1021
	public GameObject[] PinnedObjects;

	// Token: 0x040003FE RID: 1022
	public GameObject[] InventoryDisplayObjects;

	// Token: 0x040003FF RID: 1023
	public GameObject[] ExplorationNormalObjects;

	// Token: 0x04000400 RID: 1024
	public GameObject[] ExplorationSelectedObjects;

	// Token: 0x04000401 RID: 1025
	public GameObject[] ExplorationAutoSelectedObjects;

	// Token: 0x04000402 RID: 1026
	public GameObject[] ImprovementBlueprintObjects;

	// Token: 0x04000403 RID: 1027
	public GameObject[] PurchasableBlueprintObjects;

	// Token: 0x04000404 RID: 1028
	public GameObject BuyButtonObject;

	// Token: 0x04000405 RID: 1029
	public GameObject ResearchButtonObject;

	// Token: 0x04000406 RID: 1030
	public TextMeshProUGUI SunsCostText;

	// Token: 0x04000407 RID: 1031
	public TextMeshProUGUI ResearchText;

	// Token: 0x04000408 RID: 1032
	public GameObject BlueprintHelpButton;

	// Token: 0x04000409 RID: 1033
	public Button BuyBlueprintButton;

	// Token: 0x0400040A RID: 1034
	private Graphic BuyBlueprintRaycast;

	// Token: 0x0400040B RID: 1035
	public Button ResearchBlueprintButton;

	// Token: 0x0400040C RID: 1036
	private Graphic ResearchBlueprintRaycast;

	// Token: 0x0400040D RID: 1037
	public GameObject[] CurrentlyResearchedObjects;

	// Token: 0x0400040E RID: 1038
	[SerializeField]
	private GameObject[] TutorialHighlightedObjects;

	// Token: 0x0400040F RID: 1039
	[SerializeField]
	private GameObject TutorialArrow;

	// Token: 0x04000410 RID: 1040
	public GameObject[] EventProbabilitiesObjects;

	// Token: 0x04000411 RID: 1041
	public TextMeshProUGUI InventoryText;

	// Token: 0x04000412 RID: 1042
	public TextMeshProUGUI EventProbabilitiesText;

	// Token: 0x04000413 RID: 1043
	[Header("Missing Requirements")]
	public Image MissingRequirementsIcon;

	// Token: 0x04000414 RID: 1044
	public TextMeshProUGUI MissingRequirementsText;

	// Token: 0x04000415 RID: 1045
	public Sprite NotEnoughTimeSprite;

	// Token: 0x04000416 RID: 1046
	[Header("Text Feedbacks")]
	public UIFeedbackTextAndIcon SpoilageChangedPrefab;

	// Token: 0x04000417 RID: 1047
	public UIFeedbackTextAndIcon UsageChangedPrefab;

	// Token: 0x04000418 RID: 1048
	public UIFeedbackTextAndIcon FuelChangedPrefab;

	// Token: 0x04000419 RID: 1049
	public UIFeedbackTextAndIcon ChargesChangedPrefab;

	// Token: 0x0400041A RID: 1050
	public UIFeedbackTextAndIcon LiquidChangedPrefab;

	// Token: 0x0400041B RID: 1051
	public UIFeedbackTextAndIcon SpecialChangedPrefabLeft;

	// Token: 0x0400041C RID: 1052
	public UIFeedbackTextAndIcon SpecialChangedPrefabRight;

	// Token: 0x0400041D RID: 1053
	public RectTransform StatFeedbacksParent;

	// Token: 0x0400041F RID: 1055
	public Vector3 CardNotificationsOffset;

	// Token: 0x04000420 RID: 1056
	private GraphicsManager GrM;

	// Token: 0x04000421 RID: 1057
	private GameManager GM;

	// Token: 0x04000422 RID: 1058
	private Sprite CookingSprite;

	// Token: 0x04000423 RID: 1059
	private CardGraphics Duplicate;

	// Token: 0x04000424 RID: 1060
	private bool IsDuplicate;

	// Token: 0x04000425 RID: 1061
	private float LastSpoilageChange;

	// Token: 0x04000426 RID: 1062
	private UIFeedbackTextAndIcon CurrentSpoilageChange;

	// Token: 0x04000427 RID: 1063
	private float LastUsageChange;

	// Token: 0x04000428 RID: 1064
	private UIFeedbackTextAndIcon CurrentUsageChange;

	// Token: 0x04000429 RID: 1065
	private float LastFuelChange;

	// Token: 0x0400042A RID: 1066
	private UIFeedbackTextAndIcon CurrentFuelChange;

	// Token: 0x0400042B RID: 1067
	private float LastChargesChange;

	// Token: 0x0400042C RID: 1068
	private UIFeedbackTextAndIcon CurrentChargesChange;

	// Token: 0x0400042D RID: 1069
	private float LastLiquidChange;

	// Token: 0x0400042E RID: 1070
	private UIFeedbackTextAndIcon CurrentLiquidChange;

	// Token: 0x0400042F RID: 1071
	private float LastSpecial1Change;

	// Token: 0x04000430 RID: 1072
	private UIFeedbackTextAndIcon CurrentSpecial1Change;

	// Token: 0x04000431 RID: 1073
	private float LastSpecial2Change;

	// Token: 0x04000432 RID: 1074
	private UIFeedbackTextAndIcon CurrentSpecial2Change;

	// Token: 0x04000433 RID: 1075
	private float LastSpecial3Change;

	// Token: 0x04000434 RID: 1076
	private UIFeedbackTextAndIcon CurrentSpecial3Change;

	// Token: 0x04000435 RID: 1077
	private float LastSpecial4Change;

	// Token: 0x04000436 RID: 1078
	private UIFeedbackTextAndIcon CurrentSpecial4Change;

	// Token: 0x04000437 RID: 1079
	private bool LeftSidePlaying;

	// Token: 0x04000438 RID: 1080
	private bool RightSidePlaying;

	// Token: 0x04000439 RID: 1081
	private float XAngle;

	// Token: 0x0400043A RID: 1082
	private float YAngle;

	// Token: 0x0400043B RID: 1083
	private Vector3 PreviousPos;

	// Token: 0x0400043C RID: 1084
	private Transform LastCardSlot;

	// Token: 0x0400043D RID: 1085
	private bool LiquidFilledByCooking;

	// Token: 0x0400043E RID: 1086
	private List<CardGraphics.CardInventoryInfo> InventoryIndicators = new List<CardGraphics.CardInventoryInfo>();

	// Token: 0x04000442 RID: 1090
	public int PoolIndex;

	// Token: 0x02000251 RID: 593
	private class CardInventoryInfo
	{
		// Token: 0x06000F3E RID: 3902 RVA: 0x0007E88B File Offset: 0x0007CA8B
		public void SetActive(bool _Active)
		{
			if (this.Indicator)
			{
				this.Indicator.gameObject.SetActive(_Active);
			}
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0007E8AB File Offset: 0x0007CAAB
		public void SetIndicator(bool _Active)
		{
			this.Indicator.isOn = _Active;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0007E8B9 File Offset: 0x0007CAB9
		public CardInventoryInfo(Toggle _Prefab, RectTransform _Parent)
		{
			this.Indicator = UnityEngine.Object.Instantiate<Toggle>(_Prefab, _Parent);
			this.Capacity = 0;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0007E8D5 File Offset: 0x0007CAD5
		public void Setup(int _Capacity)
		{
			this.Capacity = _Capacity;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0007E8E0 File Offset: 0x0007CAE0
		public void Update(InventorySlot _FromSlot)
		{
			if (_FromSlot == null)
			{
				this.Indicator.isOn = false;
				return;
			}
			if (this.Capacity == 0)
			{
				this.Indicator.isOn = (_FromSlot.CardAmt > 0);
				if (this.Indicator.isOn)
				{
					this.Indicator.transform.SetAsFirstSibling();
					return;
				}
			}
			else
			{
				this.Indicator.isOn = (_FromSlot.CardAmt >= this.Capacity);
			}
		}

		// Token: 0x0400140C RID: 5132
		public Toggle Indicator;

		// Token: 0x0400140D RID: 5133
		private int Capacity;
	}

	// Token: 0x02000252 RID: 594
	public enum CardGraphicsStates
	{
		// Token: 0x0400140F RID: 5135
		Normal,
		// Token: 0x04001410 RID: 5136
		Highlighted,
		// Token: 0x04001411 RID: 5137
		GreyedOut,
		// Token: 0x04001412 RID: 5138
		Inspected,
		// Token: 0x04001413 RID: 5139
		MissingRequirements,
		// Token: 0x04001414 RID: 5140
		Pinned,
		// Token: 0x04001415 RID: 5141
		InventoryDisplay,
		// Token: 0x04001416 RID: 5142
		ExplorationNormal,
		// Token: 0x04001417 RID: 5143
		ExplorationSelected,
		// Token: 0x04001418 RID: 5144
		ExplorationAutoSelected
	}
}
