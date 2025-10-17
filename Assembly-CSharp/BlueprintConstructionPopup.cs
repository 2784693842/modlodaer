using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000043 RID: 67
public class BlueprintConstructionPopup : InspectionPopup
{
	// Token: 0x060002CE RID: 718 RVA: 0x0001ADFC File Offset: 0x00018FFC
	protected override void SetupActions(DismantleCardAction[] _Actions, bool _CountTimeSpentAsEffect)
	{
		base.SetupActions(_Actions, _CountTimeSpentAsEffect);
		this.SetupActions();
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0001AE0C File Offset: 0x0001900C
	private int GetResultContainerIndex()
	{
		if (base.CurrentCard.BlueprintData.CurrentStage >= base.CurrentCard.CardModel.BlueprintStages.Length)
		{
			return -1;
		}
		BlueprintStage blueprintStage = base.CurrentCard.CardModel.BlueprintStages[base.CurrentCard.BlueprintData.CurrentStage];
		for (int i = 0; i < blueprintStage.RequiredElements.Length; i++)
		{
			if (blueprintStage.RequiredElements[i].LiquidBecomesResult && blueprintStage.RequiredElements[i].DontDestroy)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x0001AEA0 File Offset: 0x000190A0
	private void SetupActions()
	{
		if (!this.BuildButton)
		{
			this.BuildButton = UnityEngine.Object.Instantiate<DismantleActionButton>(this.ButtonPrefab, this.DismantleOptionsParent);
			this.BuildButton.transform.SetSiblingIndex(0);
			DismantleActionButton buildButton = this.BuildButton;
			buildButton.OnClicked = (Action<int>)Delegate.Combine(buildButton.OnClicked, new Action<int>(this.OnBuildAction));
		}
		if (!this.DeconstructButton)
		{
			this.DeconstructButton = UnityEngine.Object.Instantiate<DismantleActionButton>(this.ButtonPrefab, this.DismantleOptionsParent);
			this.DeconstructButton.transform.SetSiblingIndex(1);
			DismantleActionButton deconstructButton = this.DeconstructButton;
			deconstructButton.OnClicked = (Action<int>)Delegate.Combine(deconstructButton.OnClicked, new Action<int>(this.OnDeconstructAction));
		}
		AudioClip[] array = base.CurrentCard.CardModel.BuildSounds;
		if (array == null)
		{
			array = this.DefaultBuildSounds;
		}
		else if (array.Length == 0)
		{
			array = this.DefaultBuildSounds;
		}
		this.CurrentBuildAction = new CardAction(LocalizedString.Build, default(LocalizedString), base.CurrentCard.CardModel.BuildingDaytimeCost, null, null, array).ToDismantleAction();
		this.CurrentBuildAction.ActionTags = base.CurrentCard.CardModel.BlueprintActionTags;
		this.CurrentLiquidResultAction = null;
		if (base.CurrentCard.BlueprintData.CurrentStage == base.CurrentCard.BlueprintSteps - 1)
		{
			List<CardDrop> list = new List<CardDrop>();
			CardDrop cardDrop = default(CardDrop);
			bool flag = this.GetResultContainerIndex() != -1;
			if (base.CurrentCard.CardModel.BlueprintResult != null && base.CurrentCard.CardModel.BlueprintResult.Length != 0)
			{
				list.AddRange(base.CurrentCard.CardModel.BlueprintResult);
			}
			if (list.Count > 0 && flag)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (!(list[i].DroppedCard == null) && list[i].DroppedCard.CardType == CardTypes.Liquid)
					{
						cardDrop = list[i];
						list.RemoveAt(i);
						break;
					}
				}
			}
			if (base.CurrentCard.CardModel.CardType != CardTypes.EnvImprovement)
			{
				this.CurrentBuildAction.ReceivingCardChanges = new CardStateChange
				{
					ModType = CardModifications.Destroy
				};
			}
			if (this.CurrentBuildAction.ProducedCards == null)
			{
				this.CurrentBuildAction.ProducedCards = new CardsDropCollection[1];
			}
			else if (this.CurrentBuildAction.ProducedCards.Length != 1)
			{
				this.CurrentBuildAction.ProducedCards = new CardsDropCollection[1];
			}
			this.CurrentBuildAction.ProducedCards[0] = new CardsDropCollection
			{
				CollectionName = base.CurrentCard.CardModel.name + "_blueprint",
				CollectionWeight = 1
			};
			if (list.Count > 0)
			{
				this.CurrentBuildAction.ProducedCards[0].SetDroppedCards(list.ToArray(), base.CurrentCard.CardModel.BPRandomSingleDrop);
			}
			this.CurrentBuildAction.StatModifications = base.CurrentCard.CardModel.BlueprintStatModifications;
			this.CurrentBuildAction.ExtraDurabilityModifications = base.CurrentCard.CardModel.BlueprintCardModifications;
			this.CurrentBuildAction.ActionLog = base.CurrentCard.CardModel.BlueprintFinishedLog;
			if (cardDrop.DroppedCard)
			{
				this.CurrentLiquidResultAction = new CardAction(LocalizedString.Build, default(LocalizedString), 0, null, null, null);
				bool flag2 = false;
				int resultContainerIndex = this.GetResultContainerIndex();
				if (resultContainerIndex != -1)
				{
					flag2 = base.CurrentCard.CardModel.BlueprintStages[base.CurrentCard.BlueprintData.CurrentStage].RequiredElements[resultContainerIndex].RequiresEmptyLiquid;
				}
				if (!flag2)
				{
					this.CurrentLiquidResultAction.ReceivingCardChanges.ModType = CardModifications.Transform;
					this.CurrentLiquidResultAction.ReceivingCardChanges.TransformInto = cardDrop.DroppedCard;
				}
				else
				{
					this.CurrentLiquidResultAction.ProducedCards = new CardsDropCollection[1];
					this.CurrentLiquidResultAction.ProducedCards[0] = new CardsDropCollection();
					this.CurrentLiquidResultAction.ProducedCards[0].CollectionName = base.CurrentCard.CardModel.name + "_blueprint_liquid";
					this.CurrentLiquidResultAction.ProducedCards[0].SetLiquidDrop(new LiquidDrop(cardDrop.DroppedCard, cardDrop.Quantity, null));
				}
				if (cardDrop.Quantity != Vector2Int.zero)
				{
					this.CurrentLiquidResultAction.ReceivingCardChanges.ModifyLiquid = true;
					this.CurrentLiquidResultAction.ReceivingCardChanges.LiquidQuantityChange = cardDrop.Quantity;
				}
			}
		}
		this.CurrentBuildAction.RequiredCardsOnBoard = base.CurrentCard.CardModel.BuildingCardConditions;
		this.CurrentBuildAction.RequiredTagsOnBoard = base.CurrentCard.CardModel.BuildingTagConditions;
		this.CurrentBuildAction.RequiredStatValues = base.CurrentCard.CardModel.BuildingStatConditions;
		array = base.CurrentCard.CardModel.DeconstructSounds;
		if (array == null)
		{
			array = this.DefaultDeconstructSounds;
		}
		else if (array.Length == 0)
		{
			array = this.DefaultDeconstructSounds;
		}
		if (base.CurrentCard.BlueprintData.CurrentStage == 0)
		{
			this.CurrentDeconstructAction = new CardAction(LocalizedString.Cancel, LocalizedString.DismantleFirstStepDesc, 0, null, null, array).ToDismantleAction();
			this.CurrentDeconstructAction.UseMiniTicks = MiniTicksBehavior.KeepsZeroCost;
			if (base.CurrentCard.CardModel.CardType != CardTypes.EnvImprovement && base.CurrentCard.CardModel.CardType != CardTypes.EnvDamage)
			{
				this.CurrentDeconstructAction.ReceivingCardChanges = new CardStateChange
				{
					ModType = CardModifications.Destroy
				};
			}
		}
		else
		{
			BlueprintStage blueprintStage = base.CurrentCard.CardModel.BlueprintStages[base.CurrentCard.BlueprintData.CurrentStage - 1];
			List<CardData> list2 = new List<CardData>();
			for (int j = 0; j < blueprintStage.RequiredElements.Length; j++)
			{
				if (blueprintStage.RequiredElements[j].ValidRequirements && !blueprintStage.RequiredElements[j].DontDestroy)
				{
					for (int k = 0; k < blueprintStage.RequiredElements[j].GetQuantity; k++)
					{
						list2.Add(blueprintStage.RequiredElements[j].AnyCard);
					}
				}
			}
			this.CurrentDeconstructAction = new CardAction(LocalizedString.Cancel, LocalizedString.DismantleDesc, base.CurrentCard.CardModel.DeconstructDaytimeCost, list2, null, array).ToDismantleAction();
			this.CurrentDeconstructAction.ConfirmPopup = true;
			this.CurrentDeconstructAction.UseMiniTicks = MiniTicksBehavior.DefaultBehavior;
		}
		this.BuildButton.Setup(-2, this.CurrentBuildAction, base.CurrentCard, false, false);
		this.DeconstructButton.Setup(-1, this.CurrentDeconstructAction, base.CurrentCard, false, false);
		if (base.CurrentCard.CardModel)
		{
			if (base.CurrentCard.CardModel.CardType == CardTypes.EnvImprovement || base.CurrentCard.CardModel.CardType == CardTypes.EnvDamage)
			{
				this.DeconstructButton.gameObject.SetActive(base.CurrentCard.BlueprintData.CurrentStage > 0);
				return;
			}
			this.DeconstructButton.gameObject.SetActive(true);
		}
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0001B5D6 File Offset: 0x000197D6
	public override void OnButtonClicked(int _Index, bool _Stack)
	{
		if (_Index == -2)
		{
			this.OnBuildAction(-2);
			return;
		}
		if (_Index == -1)
		{
			this.OnDeconstructAction(-1);
			return;
		}
		base.OnButtonClicked(_Index, _Stack);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0001B5FC File Offset: 0x000197FC
	protected override void SetupInventory(InGameCardBase _Card, bool _ResetView)
	{
		if (this.ProgressBar)
		{
			this.ProgressBar.Setup(_Card);
		}
		base.SetupInventory(_Card, _ResetView);
		if (_Card.BlueprintData.CurrentStage >= _Card.CardModel.BlueprintStages.Length)
		{
			return;
		}
		BlueprintStage blueprintStage = _Card.CardModel.BlueprintStages[_Card.BlueprintData.CurrentStage];
		for (int i = 0; i < blueprintStage.RequiredElements.Length; i++)
		{
			this.InventorySlotsLine.Slots[i].ClearFilters();
			this.InventorySlotsLine.Slots[i].AddFilter(new CardFilter
			{
				AcceptedCards = blueprintStage.RequiredElements[i].AllCards.ToArray()
			});
			this.InventorySlotsLine.Slots[i].MaxPileCount = blueprintStage.RequiredElements[i].GetQuantity;
			this.InventorySlotsLine.Slots[i].SetTitleText(blueprintStage.RequiredElements[i].GetName);
			this.InventorySlotsLine.Slots[i].SetImageList(blueprintStage.RequiredElements[i].GetImages);
			this.InventorySlotsLine.Slots[i].UpdateText(string.Format("{0}/{1}", base.CurrentCard.CardsInInventory[i].CardAmt.ToString(), blueprintStage.RequiredElements[i].GetQuantity.ToString()));
			this.InventorySlotsLine.Slots[i].SetLiquidImage(blueprintStage.RequiredElements[i].GetLiquidQuantity > 0);
			this.InventorySlotsLine.Slots[i].SetHelpPage(blueprintStage.RequiredElements[i].GetHelpPage);
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0001B7E8 File Offset: 0x000199E8
	protected override void Update()
	{
		base.Update();
		if (!base.CurrentCard)
		{
			return;
		}
		this.BuildButton.Interactable = (base.CurrentCard.ReadyToBuildBlueprint && this.BuildButton.ConditionsValid);
		this.DeconstructButton.Interactable = (base.CurrentCard.ReadyToDeconstruct && this.DeconstructButton.ConditionsValid);
		if (base.CurrentCard.BlueprintData.CurrentStage >= 0 && base.CurrentCard.BlueprintData.CurrentStage < base.CurrentCard.CardModel.BlueprintStages.Length && !this.ProgressBar.IsPlaying)
		{
			int num = 0;
			while (num < this.InventorySlotsLine.Slots.Count && num < base.CurrentCard.CurrentBlueprintStage.RequiredElements.Length)
			{
				this.InventorySlotsLine.Slots[num].UpdateText(string.Format("{0}/{1}", base.CurrentCard.CardsInInventory[num].CardAmt.ToString(), base.CurrentCard.CurrentBlueprintStage.RequiredElements[num].GetQuantity.ToString()));
				this.InventorySlotsLine.Slots[num].UpdateTextColor((base.CurrentCard.CardsInInventory[num].CardAmt == base.CurrentCard.CurrentBlueprintStage.RequiredElements[num].GetQuantity) ? this.ValidSlotTextColor : this.DefaultSlotTextColor);
				bool flag = base.CurrentCard.CurrentBlueprintStage.RequiredElements[num].GetLiquidQuantity > 0 && (!this.InventorySlotsLine.Slots[num].AssignedCard || (this.InventorySlotsLine.Slots[num].AssignedCard.IsLiquid || this.InventorySlotsLine.Slots[num].AssignedCard.IsLiquidContainer));
				if (flag)
				{
					this.InventorySlotsLine.Slots[num].SetLiquidImage(true);
					this.InventorySlotsLine.Slots[num].UpdateLiquidText(string.Format("{0}/{1}", base.CurrentCard.CardsInInventory[num].LiquidAmt(this.LiquidUnitValue).ToString("0.0"), base.CurrentCard.CurrentBlueprintStage.RequiredElements[num].GetLiquidUnits(this.LiquidUnitValue).ToString("0.0")), (base.CurrentCard.CardsInInventory[num].LiquidAmt(this.LiquidUnitValue) >= base.CurrentCard.CurrentBlueprintStage.RequiredElements[num].GetLiquidUnits(this.LiquidUnitValue)) ? this.ValidSlotTextColor : this.DefaultSlotTextColor);
				}
				else
				{
					this.InventorySlotsLine.Slots[num].SetLiquidImage(false);
					this.InventorySlotsLine.Slots[num].UpdateLiquidText("", Color.clear);
				}
				num++;
			}
		}
		if (this.AutoFillButton)
		{
			this.AutoFillButton.interactable = !this.BuildButton.Interactable;
		}
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0001BB54 File Offset: 0x00019D54
	public void AutoFill()
	{
		if (!base.CurrentCard)
		{
			return;
		}
		if (!base.CurrentCard.CardModel)
		{
			return;
		}
		if (base.CurrentCard.CardModel.BlueprintStages == null)
		{
			return;
		}
		if (base.CurrentCard.CardModel.BlueprintStages.Length == 0)
		{
			return;
		}
		int num = Mathf.Clamp(base.CurrentCard.BlueprintData.CurrentStage, 0, base.CurrentCard.CardModel.BlueprintStages.Length - 1);
		BlueprintStage blueprintStage = base.CurrentCard.CardModel.BlueprintStages[num];
		List<InGameCardBase> list = new List<InGameCardBase>();
		if (!this.GM)
		{
			this.GetManager();
		}
		for (int i = 0; i < blueprintStage.RequiredElements.Length; i++)
		{
			if (!base.CurrentCard.CardsInInventory[i].IsFree)
			{
				if (base.CurrentCard.CardsInInventory[i].CardAmt != blueprintStage.RequiredElements[i].GetQuantity)
				{
					list.Clear();
					if (this.GM.CardIsOnBoard(base.CurrentCard.CardsInInventory[i].CardModel, true, false, false, false, list, Array.Empty<InGameCardBase>()))
					{
						this.FillSlot(i, ref list, blueprintStage.RequiredElements[i].GetQuantity);
					}
				}
			}
			else
			{
				List<CardData> allCards = blueprintStage.RequiredElements[i].AllCards;
				List<InGameCardBase> list2 = new List<InGameCardBase>();
				int index = 0;
				int num2 = 0;
				for (int j = 0; j < allCards.Count; j++)
				{
					list.Clear();
					if (this.GM.CardIsOnBoard(allCards[j], true, allCards[j].CardType == CardTypes.Liquid, false, false, list, Array.Empty<InGameCardBase>()))
					{
						for (int k = list.Count - 1; k >= 0; k--)
						{
							if (this.GrM.CharacterWindow.HasCardEquipped(list[k]))
							{
								list.RemoveAt(k);
							}
							else if (list[k].CardModel && list[k].CardModel.CardType != CardTypes.Item && list[k].CardModel.CardType != CardTypes.Base && list[k].CardModel.CardType != CardTypes.Liquid)
							{
								list.RemoveAt(k);
							}
							else if (list[k].IsLiquid && list[k].CurrentContainer && list[k].CurrentContainer.CardModel && list[k].CurrentContainer.CardModel.CardType != CardTypes.Item && list[k].CurrentContainer.CardModel.CardType != CardTypes.Base)
							{
								list.RemoveAt(k);
							}
							else if (blueprintStage.RequiredElements[i].GetLiquidQuantity > 0 || allCards[j].CanContainLiquid)
							{
								if (!blueprintStage.RequiredElements[i].CompatibleInGameCard(list[k]))
								{
									list.RemoveAt(k);
								}
								else if (list[k].CurrentContainer && (list[k].CurrentContainer.CurrentContainer || this.GrM.CharacterWindow.HasCardEquipped(list[k].CurrentContainer)))
								{
									list.RemoveAt(k);
								}
							}
						}
						if (list.Count > 0)
						{
							if (num2 == 0 || list.Count >= blueprintStage.RequiredElements[i].GetQuantity)
							{
								list2.Clear();
								list2.AddRange(list);
								index = j;
								num2 = list.Count;
							}
							if (list.Count >= blueprintStage.RequiredElements[i].GetQuantity)
							{
								break;
							}
						}
					}
				}
				if (num2 != 0)
				{
					if ((blueprintStage.RequiredElements[i].GetLiquidQuantity > 0 || allCards[index].CanContainLiquid) && list2[0].IsLiquid)
					{
						int count = list2.Count;
						for (int l = 0; l < count; l++)
						{
							list2.Add(list2[0].CurrentContainer);
							list2.RemoveAt(0);
						}
					}
					this.FillSlot(i, ref list2, blueprintStage.RequiredElements[i].GetQuantity);
				}
			}
		}
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0001BFD8 File Offset: 0x0001A1D8
	private void FillSlot(int _Index, ref List<InGameCardBase> _WithCards, int _UpTo)
	{
		int cardAmt = base.CurrentCard.CardsInInventory[_Index].CardAmt;
		int num = 0;
		while (num < _WithCards.Count && cardAmt + num < _UpTo)
		{
			if (_WithCards[num].CurrentSlot != this.InventorySlotsLine.Slots[_Index])
			{
				this.GrM.MoveCardToSlot(_WithCards[num], this.InventorySlotsLine.Slots[_Index].ToInfo(), true, false);
			}
			num++;
		}
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0001C060 File Offset: 0x0001A260
	private void OnBuildAction(int _Index)
	{
		BlueprintStage blueprintStage = base.CurrentCard.CardModel.BlueprintStages[base.CurrentCard.BlueprintData.CurrentStage];
		List<int> list = new List<int>();
		Dictionary<CardData, CardAction> dictionary = new Dictionary<CardData, CardAction>();
		for (int i = 0; i < blueprintStage.RequiredElements.Length; i++)
		{
			List<CardData> allCards = blueprintStage.RequiredElements[i].AllCards;
			for (int j = 0; j < allCards.Count; j++)
			{
				if (blueprintStage.RequiredElements[i].DontDestroy && allCards[j])
				{
					if (!list.Contains(i))
					{
						list.Add(i);
					}
					if (!dictionary.ContainsKey(allCards[j]))
					{
						dictionary.Add(allCards[j], new CardAction(new LocalizedString
						{
							LocalizationKey = "BPSTEP",
							DefaultText = "Blueprint step"
						}, default(LocalizedString)));
						dictionary[allCards[j]].ReceivingCardChanges = blueprintStage.RequiredElements[i].EffectOnIngredient;
					}
				}
			}
		}
		int resultContainerIndex = this.GetResultContainerIndex();
		InGameCardBase inGameCardBase = null;
		InGameCardBase inGameCardBase2 = null;
		if (resultContainerIndex != -1 && this.InventorySlotsLine.Slots[resultContainerIndex].AssignedCard)
		{
			inGameCardBase2 = this.InventorySlotsLine.Slots[resultContainerIndex].AssignedCard;
			if (this.InventorySlotsLine.Slots[resultContainerIndex].AssignedCard.ContainedLiquid)
			{
				inGameCardBase = this.InventorySlotsLine.Slots[resultContainerIndex].AssignedCard.ContainedLiquid;
				inGameCardBase.MarkedAsBlueprintIngredient = true;
			}
		}
		for (int k = 0; k < base.CurrentCard.CardsInInventory.Count; k++)
		{
			if (base.CurrentCard.CardsInInventory[k].CardModel)
			{
				if (!list.Contains(k))
				{
					for (int l = 0; l < base.CurrentCard.CardsInInventory[k].CardAmt; l++)
					{
						base.CurrentCard.CardsInInventory[k].AllCards[l].MarkedAsBlueprintIngredient = true;
					}
				}
				else
				{
					bool flag = dictionary.ContainsKey(base.CurrentCard.CardsInInventory[k].CardModel);
					bool flag2 = base.CurrentCard.CardsInInventory[k].MainCard.ContainedLiquidModel && dictionary.ContainsKey(base.CurrentCard.CardsInInventory[k].MainCard.ContainedLiquidModel);
					if (!flag && !flag2)
					{
						for (int m = 0; m < base.CurrentCard.CardsInInventory[k].CardAmt; m++)
						{
							base.CurrentCard.CardsInInventory[k].AllCards[m].MarkedAsBlueprintIngredient = true;
						}
					}
					else
					{
						for (int n = 0; n < base.CurrentCard.CardsInInventory[k].CardAmt; n++)
						{
							if (flag)
							{
								base.CurrentCard.CardsInInventory[k].AllCards[n].MarkedAsBlueprintIngredient = (dictionary[base.CurrentCard.CardsInInventory[k].CardModel].ReceivingCardChanges.ModType == CardModifications.Transform);
							}
							if (flag2)
							{
								base.CurrentCard.CardsInInventory[k].AllCards[n].ContainedLiquid.MarkedAsBlueprintIngredient = (dictionary[base.CurrentCard.CardsInInventory[k].MainCard.ContainedLiquidModel].ReceivingCardChanges.ModType == CardModifications.Transform);
							}
						}
					}
				}
			}
		}
		if (this.CurrentLiquidResultAction != null && (inGameCardBase2 || inGameCardBase2))
		{
			if (inGameCardBase)
			{
				this.GM.StartCoroutine(this.DoLiquidTransformAction(GameManager.PerformAction(this.CurrentBuildAction, base.CurrentCard, false), inGameCardBase, this.CurrentLiquidResultAction));
			}
			else if (inGameCardBase2)
			{
				this.GM.StartCoroutine(this.DoLiquidTransformAction(GameManager.PerformAction(this.CurrentBuildAction, base.CurrentCard, false), inGameCardBase2, this.CurrentLiquidResultAction));
			}
		}
		else
		{
			GameManager.PerformAction(this.CurrentBuildAction, base.CurrentCard, false);
		}
		for (int num = 0; num < base.CurrentCard.CardsInInventory.Count; num++)
		{
			if (base.CurrentCard.CardsInInventory[num].CardModel && list.Contains(num))
			{
				bool flag = dictionary.ContainsKey(base.CurrentCard.CardsInInventory[num].CardModel);
				bool flag2 = base.CurrentCard.CardsInInventory[num].MainCard.ContainedLiquidModel && dictionary.ContainsKey(base.CurrentCard.CardsInInventory[num].MainCard.ContainedLiquidModel);
				if (flag || flag2)
				{
					for (int num2 = 0; num2 < base.CurrentCard.CardsInInventory[num].CardAmt; num2++)
					{
						if (flag)
						{
							GameManager.PerformAction(dictionary[base.CurrentCard.CardsInInventory[num].CardModel], base.CurrentCard.CardsInInventory[num].AllCards[num2], true);
						}
						if (flag2)
						{
							GameManager.PerformAction(dictionary[base.CurrentCard.CardsInInventory[num].MainCard.ContainedLiquidModel], base.CurrentCard.CardsInInventory[num].AllCards[num2].ContainedLiquid, true);
						}
					}
				}
			}
		}
		MBSingleton<GameManager>.Instance.ClearCardInventory(base.CurrentCard, false, list);
		base.CurrentCard.IncreaseBlueprintStage();
		base.StartCoroutine(this.ProgressBar.Animate(this.CurrentBuildAction.TotalDaytimeCost, new Action(this.OnBarIncreaseComplete)));
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0001C6D3 File Offset: 0x0001A8D3
	private IEnumerator DoLiquidTransformAction(Coroutine _WaitFor, InGameCardBase _Target, CardAction _Action)
	{
		yield return _WaitFor;
		GameManager.PerformAction(_Action, _Target, true);
		yield break;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0001C6F0 File Offset: 0x0001A8F0
	private void OnBarIncreaseComplete()
	{
		if (base.CurrentCard && base.CurrentCard.CardModel && base.CurrentCard.CardModel.CardType == CardTypes.EnvImprovement && base.CurrentCard.BlueprintComplete)
		{
			MBSingleton<GraphicsManager>.Instance.OpenEnvImprovements(base.CurrentCard);
			return;
		}
		if (base.CurrentCard)
		{
			this.SetupInventory(base.CurrentCard, true);
		}
		this.SetupActions();
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0001C770 File Offset: 0x0001A970
	private void OnDeconstructAction(int _Index)
	{
		if (this.CurrentDeconstructAction.ConfirmPopup && this.ConfirmPopup && !this.ConfirmPopup.gameObject.activeInHierarchy)
		{
			this.ConfirmPopup.Setup(this.CurrentDeconstructAction.ActionName, HoursDisplay.HoursToCompleteString(GameManager.TickToHours(this.CurrentDeconstructAction.TotalDaytimeCost, this.CurrentDeconstructAction.MiniTicksCost)), _Index, false, this);
			this.ConfirmPopup.gameObject.SetActive(true);
			return;
		}
		if (base.CurrentCard.BlueprintData.CurrentStage > 0)
		{
			base.CurrentCard.DecreaseBlueprintStage();
			this.SetupInventory(base.CurrentCard, true);
		}
		GameManager.PerformAction(this.CurrentDeconstructAction, base.CurrentCard, false);
		this.SetupActions();
		base.StartCoroutine(this.ProgressBar.Animate(this.CurrentDeconstructAction.TotalDaytimeCost, null));
	}

	// Token: 0x04000351 RID: 849
	private DismantleActionButton BuildButton;

	// Token: 0x04000352 RID: 850
	private DismantleActionButton DeconstructButton;

	// Token: 0x04000353 RID: 851
	[SerializeField]
	private BlueprintBar ProgressBar;

	// Token: 0x04000354 RID: 852
	[SerializeField]
	private AudioClip[] DefaultBuildSounds;

	// Token: 0x04000355 RID: 853
	[SerializeField]
	private AudioClip[] DefaultDeconstructSounds;

	// Token: 0x04000356 RID: 854
	[SerializeField]
	private Color ValidSlotTextColor;

	// Token: 0x04000357 RID: 855
	[SerializeField]
	private Color DefaultSlotTextColor;

	// Token: 0x04000358 RID: 856
	[SerializeField]
	private Button AutoFillButton;

	// Token: 0x04000359 RID: 857
	[SerializeField]
	private float LiquidUnitValue = 300f;

	// Token: 0x0400035A RID: 858
	private DismantleCardAction CurrentBuildAction;

	// Token: 0x0400035B RID: 859
	private CardAction CurrentLiquidResultAction;

	// Token: 0x0400035C RID: 860
	private DismantleCardAction CurrentDeconstructAction;

	// Token: 0x0400035D RID: 861
	private const string SlotTextFormat = "{0}/{1}";

	// Token: 0x0400035E RID: 862
	private const string SlotFloatFormat = "0.0";
}
