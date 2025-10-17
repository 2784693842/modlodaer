using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000D3 RID: 211
[Serializable]
public class CardAction
{
	// Token: 0x1700016B RID: 363
	// (get) Token: 0x0600079D RID: 1949 RVA: 0x0004B0B0 File Offset: 0x000492B0
	public int UnmodifiedDaytimeCost
	{
		get
		{
			return this.DaytimeCost;
		}
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x0004B0B8 File Offset: 0x000492B8
	public string NoveltyID(InGameCardBase _FromCard)
	{
		if (this.NoveltyShared || !_FromCard)
		{
			return this.ActionName.DefaultText;
		}
		if (!_FromCard.CardModel)
		{
			return this.ActionName.DefaultText;
		}
		return string.Format("{0}_{1}", _FromCard.CardModel.name, this.ActionName.DefaultText);
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x0004B11A File Offset: 0x0004931A
	public bool IsExploreAction
	{
		get
		{
			return this.CountAsExploration || (this is DismantleCardAction && (this as DismantleCardAction).ExplorationValue > 0f);
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0004B142 File Offset: 0x00049342
	public bool HasLog
	{
		get
		{
			return this.ActionLog != null && !string.IsNullOrEmpty(this.ActionLog.LogText);
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0004B166 File Offset: 0x00049366
	public bool DestroysReceivingCard
	{
		get
		{
			return this.ReceivingCardChanges.ModType == CardModifications.Destroy || this.ReceivingCardChanges.ModType == CardModifications.Transform;
		}
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x0004B188 File Offset: 0x00049388
	public void SetupStackStopCondition()
	{
		if (this.StackStopConditions.LocationWeightIsFull)
		{
			return;
		}
		this.StackStopConditions = default(StackActionStopConditions);
		if (this.ReceivingCardChanges.ModType != CardModifications.DurabilityChanges)
		{
			return;
		}
		this.StackStopConditions.SpoilageIsFull = (Mathf.Max(this.ReceivingCardChanges.SpoilageChange.x, this.ReceivingCardChanges.SpoilageChange.y) > 0f);
		this.StackStopConditions.SpoilageIsEmpty = (Mathf.Min(this.ReceivingCardChanges.SpoilageChange.x, this.ReceivingCardChanges.SpoilageChange.y) < 0f);
		this.StackStopConditions.UsageIsFull = (Mathf.Max(this.ReceivingCardChanges.UsageChange.x, this.ReceivingCardChanges.UsageChange.y) > 0f);
		this.StackStopConditions.UsageIsEmpty = (Mathf.Min(this.ReceivingCardChanges.UsageChange.x, this.ReceivingCardChanges.UsageChange.y) < 0f);
		this.StackStopConditions.FuelIsFull = (Mathf.Max(this.ReceivingCardChanges.FuelChange.x, this.ReceivingCardChanges.FuelChange.y) > 0f);
		this.StackStopConditions.FuelIsEmpty = (Mathf.Min(this.ReceivingCardChanges.FuelChange.x, this.ReceivingCardChanges.FuelChange.y) < 0f);
		this.StackStopConditions.ProgressIsFull = (Mathf.Max(this.ReceivingCardChanges.ChargesChange.x, this.ReceivingCardChanges.ChargesChange.y) > 0f);
		this.StackStopConditions.ProgressIsEmpty = (Mathf.Min(this.ReceivingCardChanges.ChargesChange.x, this.ReceivingCardChanges.ChargesChange.y) < 0f);
		this.StackStopConditions.Special1IsFull = (Mathf.Max(this.ReceivingCardChanges.Special1Change.x, this.ReceivingCardChanges.Special1Change.y) > 0f);
		this.StackStopConditions.Special1IsEmpty = (Mathf.Min(this.ReceivingCardChanges.Special1Change.x, this.ReceivingCardChanges.Special1Change.y) < 0f);
		this.StackStopConditions.Special2IsFull = (Mathf.Max(this.ReceivingCardChanges.Special2Change.x, this.ReceivingCardChanges.Special2Change.y) > 0f);
		this.StackStopConditions.Special2IsEmpty = (Mathf.Min(this.ReceivingCardChanges.Special2Change.x, this.ReceivingCardChanges.Special2Change.y) < 0f);
		this.StackStopConditions.Special3IsFull = (Mathf.Max(this.ReceivingCardChanges.Special3Change.x, this.ReceivingCardChanges.Special3Change.y) > 0f);
		this.StackStopConditions.Special3IsEmpty = (Mathf.Min(this.ReceivingCardChanges.Special3Change.x, this.ReceivingCardChanges.Special3Change.y) < 0f);
		this.StackStopConditions.Special4IsFull = (Mathf.Max(this.ReceivingCardChanges.Special4Change.x, this.ReceivingCardChanges.Special4Change.y) > 0f);
		this.StackStopConditions.Special4IsEmpty = (Mathf.Min(this.ReceivingCardChanges.Special4Change.x, this.ReceivingCardChanges.Special4Change.y) < 0f);
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0004B530 File Offset: 0x00049730
	public void CollectActionModifiers(InGameCardBase _ReceivingCard, InGameCardBase _GivenCard)
	{
		this.AllStatModifiers.Clear();
		if (this.StatModifications != null)
		{
			this.AllStatModifiers.AddRange(this.StatModifications);
		}
		this.GivenDurabilityChanges = new TransferedDurabilities();
		this.ReceivingDurabilityChanges = new TransferedDurabilities();
		this.TotalDaytimeCost = this.DaytimeCost;
		this.ActionBlockedMessage = "";
		this.CalculateLiquidScales(_ReceivingCard, _GivenCard);
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (!instance)
		{
			return;
		}
		List<ActionModifier> list = new List<ActionModifier>();
		if (instance.CurrentActionModifiers != null && instance.CurrentActionModifiers.Count > 0)
		{
			for (int i = 0; i < instance.CurrentActionModifiers.Count; i++)
			{
				if (instance.CurrentActionModifiers[i].AppliesToAction(this, instance.NotInBase, _ReceivingCard))
				{
					list.Add(instance.CurrentActionModifiers[i]);
				}
			}
		}
		if (_ReceivingCard && _ReceivingCard.CardModel)
		{
			CardTag[] cardTags = _ReceivingCard.CardModel.CardTags;
			if (cardTags != null && cardTags.Length != 0)
			{
				for (int j = 0; j < cardTags.Length; j++)
				{
					if (cardTags[j] && cardTags[j].ActionModifiers != null && cardTags[j].ActionModifiers.Length != 0)
					{
						for (int k = 0; k < cardTags[j].ActionModifiers.Length; k++)
						{
							if (cardTags[j].ActionModifiers[k].AppliesToAction(this, instance.NotInBase, _ReceivingCard))
							{
								list.Add(cardTags[j].ActionModifiers[k]);
							}
						}
					}
				}
			}
		}
		if (_GivenCard && _GivenCard.CardModel)
		{
			CardTag[] cardTags2 = _GivenCard.CardModel.CardTags;
			if (cardTags2 != null && cardTags2.Length != 0)
			{
				for (int l = 0; l < cardTags2.Length; l++)
				{
					if (cardTags2[l] && cardTags2[l].ActionModifiers != null && cardTags2[l].ActionModifiers.Length != 0)
					{
						for (int m = 0; m < cardTags2[l].ActionModifiers.Length; m++)
						{
							if (cardTags2[l].ActionModifiers[m].AppliesToAction(this, instance.NotInBase, _ReceivingCard))
							{
								list.Add(cardTags2[l].ActionModifiers[m]);
							}
						}
					}
				}
			}
		}
		for (int n = 0; n < list.Count; n++)
		{
			this.TotalDaytimeCost += list[n].DurationModifier;
			this.AllStatModifiers.AddRange(list[n].AddedStatModifiers);
			this.ReceivingDurabilityChanges.Add(list[n].ReceivingDurabilities);
			this.GivenDurabilityChanges.Add(list[n].GivenDurabilities);
			if (list[n].BlocksAction)
			{
				if (!string.IsNullOrEmpty(list[n].ActionBlockMessage))
				{
					this.ActionBlockedMessage = list[n].ActionBlockMessage;
				}
				else
				{
					this.ActionBlockedMessage = LocalizedString.ImpossibleAction;
				}
			}
		}
		this.TotalDaytimeCost = Mathf.Max(0, this.TotalDaytimeCost);
		if (this.StatsLiquidScale >= 0f)
		{
			for (int num = 0; num < this.AllStatModifiers.Count; num++)
			{
				List<StatModifier> allStatModifiers = this.AllStatModifiers;
				int index = num;
				allStatModifiers[index] *= this.StatsLiquidScale;
			}
		}
		if (this.DurabilitiesLiquidScale >= 0f)
		{
			this.ReceivingDurabilityChanges.Multiply(this.DurabilitiesLiquidScale, true);
			this.GivenDurabilityChanges.Multiply(this.DurabilitiesLiquidScale, true);
		}
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0004B8C8 File Offset: 0x00049AC8
	protected virtual void CalculateLiquidScales(InGameCardBase _ReceivingCard, InGameCardBase _GivenCard)
	{
		this.StatsLiquidScale = -1f;
		this.DurabilitiesLiquidScale = -1f;
		if (_ReceivingCard && (_ReceivingCard.IsLiquid || _ReceivingCard.ContainedLiquid))
		{
			this.StatsLiquidScale = this.ReceivingCardChanges.StatLiquidScalingValue(_ReceivingCard);
			this.DurabilitiesLiquidScale = this.ReceivingCardChanges.DurabilitiesLiquidScalingValue(_ReceivingCard);
		}
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0004B92C File Offset: 0x00049B2C
	public DismantleCardAction ToDismantleAction()
	{
		return new DismantleCardAction
		{
			ActionName = this.ActionName,
			ActionDescription = this.ActionDescription,
			ActionSounds = this.ActionSounds,
			ActionLog = this.ActionLog,
			SaveGame = this.SaveGame,
			DaytimeCost = this.DaytimeCost,
			ActionTags = this.ActionTags,
			RequiredStatValues = this.RequiredStatValues,
			RequiredCardsOnBoard = this.RequiredCardsOnBoard,
			RequiredTagsOnBoard = this.RequiredTagsOnBoard,
			RequiredReceivingContainer = this.RequiredReceivingContainer,
			RequiredReceivingContainerTag = this.RequiredReceivingContainerTag,
			RequiredReceivingDurabilities = this.RequiredReceivingDurabilities,
			RequiredReceivingLiquidContent = this.RequiredReceivingLiquidContent,
			NotBaseAction = this.NotBaseAction,
			Cancellable = this.Cancellable,
			InstantStatModifications = this.InstantStatModifications,
			ProducedCards = this.ProducedCards,
			StatModifications = this.StatModifications,
			ExtraDurabilityModifications = this.ExtraDurabilityModifications,
			BlueprintsFullUnlock = this.BlueprintsFullUnlock,
			ReceivingCardChanges = this.ReceivingCardChanges,
			CustomWindowPrefab = this.CustomWindowPrefab,
			FadeToBlack = this.FadeToBlack,
			FadeMessage = this.FadeMessage
		};
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0004BA6A File Offset: 0x00049C6A
	public CardAction()
	{
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0004BAA0 File Offset: 0x00049CA0
	public CardAction(LocalizedString _Name, LocalizedString _Desc)
	{
		this.ActionName = _Name;
		this.ActionDescription = _Desc;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0004BAF0 File Offset: 0x00049CF0
	public CardAction(LocalizedString _Name, LocalizedString _Desc, int _Duration, List<CardData> _ProducedCards, List<SimpleCardSaveData> _LoadedCards, AudioClip[] _Sounds)
	{
		this.ActionName = _Name;
		this.ActionDescription = _Desc;
		this.DaytimeCost = _Duration;
		this.ProducedCards = new CardsDropCollection[1];
		this.ProducedCards[0] = new CardsDropCollection();
		this.ProducedCards[0].CollectionName = _Name + " Drops";
		this.ProducedCards[0].SetDroppedCards(_ProducedCards);
		this.LoadedCards = _LoadedCards;
		this.ActionSounds = _Sounds;
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0004BB99 File Offset: 0x00049D99
	public bool IsTravellingAction
	{
		get
		{
			return this.TravelToPreviousEnv || this.TravelDestination != null;
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x060007AA RID: 1962 RVA: 0x0004BBB4 File Offset: 0x00049DB4
	public CardData TravelDestination
	{
		get
		{
			if (this.TravelToPreviousEnv && MBSingleton<GameManager>.Instance)
			{
				return MBSingleton<GameManager>.Instance.PrevEnvironment;
			}
			if (this.ReceivingCardChanges.ModType == CardModifications.Transform && this.ReceivingCardChanges.TransformInto && this.ReceivingCardChanges.TransformInto.CardType == CardTypes.Environment)
			{
				return this.ReceivingCardChanges.TransformInto;
			}
			if (this.ProducedCards != null && this.ProducedCards.Length != 0)
			{
				for (int i = 0; i < this.ProducedCards.Length; i++)
				{
					if (this.ProducedCards[i] != null)
					{
						CardData getTravelDestination = this.ProducedCards[i].GetTravelDestination;
						if (getTravelDestination)
						{
							return getTravelDestination;
						}
					}
				}
			}
			return null;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x060007AB RID: 1963 RVA: 0x0004BC68 File Offset: 0x00049E68
	public bool IsNotCancelledByDemo
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x0004BC6C File Offset: 0x00049E6C
	public bool QuickRequirementsCheck(InGameCardBase _Card, bool _CheckIfEffect = false)
	{
		bool isNotCancelledByDemo = this.IsNotCancelledByDemo;
		bool flag = !_CheckIfEffect;
		string text = "";
		if (_CheckIfEffect)
		{
			flag = this.WillHaveAnEffect(_Card, false, false, false, false, Array.Empty<CardModifications>());
		}
		bool flag2 = this.StatsAreCorrect(null, true);
		bool flag3 = this.CardsAndTagsAreCorrect(_Card, null, null);
		bool flag4 = this.DurabilitiesAreCorrect(_Card, out text);
		bool flag5 = this.EnoughDaylightPoints();
		return flag2 && flag3 && flag4 && flag5 && flag && isNotCancelledByDemo;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x0004BCD8 File Offset: 0x00049ED8
	public bool StatsAreCorrect(List<MissingStatInfo> failedRequirements = null, bool _IncludeAllFailedRequirements = true)
	{
		GameManager instance = MBSingleton<GameManager>.Instance;
		bool result = true;
		if (failedRequirements != null)
		{
			failedRequirements.Clear();
		}
		if (this.RequiredStatValues != null && this.RequiredStatValues.Length != 0)
		{
			for (int i = 0; i < this.RequiredStatValues.Length; i++)
			{
				if (this.RequiredStatValues[i].Stat)
				{
					float value = instance.StatsDict[this.RequiredStatValues[i].Stat].CurrentValue(this.NotBaseAction);
					if (!this.RequiredStatValues[i].IsInRange(value))
					{
						if (failedRequirements == null)
						{
							return false;
						}
						if (this.RequiredStatValues[i].HideAllWhenNotMet)
						{
							failedRequirements.Clear();
							return false;
						}
						if (_IncludeAllFailedRequirements || this.RequiredStatValues[i].NotifyWhenNotMet)
						{
							failedRequirements.Add(new MissingStatInfo(this.RequiredStatValues[i].Stat, this.RequiredStatValues[i].IsTooMuch(value, false)));
						}
						result = false;
					}
				}
			}
		}
		if (failedRequirements != null)
		{
			failedRequirements.Sort((MissingStatInfo a, MissingStatInfo b) => a.Stat.StatPriority.CompareTo(b.Stat.StatPriority));
		}
		return result;
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0004BE10 File Offset: 0x0004A010
	public bool CardsAndTagsAreCorrect(InGameCardBase _ForCard, List<CardData> failedCards = null, List<CardTag> failedTags = null)
	{
		if (ArrayUtility.IsEmpty<CardData>(this.RequiredReceivingContainer) && ArrayUtility.IsEmpty<CardTag>(this.RequiredReceivingContainerTag) && ArrayUtility.IsEmpty<CardOnBoardCondition>(this.RequiredCardsOnBoard) && ArrayUtility.IsEmpty<TagOnBoardCondition>(this.RequiredTagsOnBoard))
		{
			return true;
		}
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (!instance.CardsLoaded)
		{
			return false;
		}
		if (failedCards != null)
		{
			failedCards.Clear();
		}
		if (failedTags != null)
		{
			failedTags.Clear();
		}
		if (_ForCard)
		{
			if (this.RequiredReceivingContainer != null && this.RequiredReceivingContainer.Length != 0)
			{
				bool flag = false;
				for (int i = 0; i < this.RequiredReceivingContainer.Length; i++)
				{
					if (!_ForCard.CurrentContainer)
					{
						if (!this.RequiredReceivingContainer[i])
						{
							flag = true;
							break;
						}
					}
					else if (this.RequiredReceivingContainer[i] == _ForCard.CurrentContainer.CardModel)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			if (this.RequiredReceivingContainerTag != null && this.RequiredReceivingContainerTag.Length != 0)
			{
				if (!_ForCard.CurrentContainer)
				{
					return false;
				}
				if (!_ForCard.CurrentContainer.CardModel.HasAnyTag(this.RequiredReceivingContainerTag))
				{
					return false;
				}
			}
		}
		if (this.RequiredCardsOnBoard != null && this.RequiredCardsOnBoard.Length != 0)
		{
			for (int j = 0; j < this.RequiredCardsOnBoard.Length; j++)
			{
				if (!this.RequiredCardsOnBoard[j].Inverted)
				{
					if (!this.RequiredCardsOnBoard[j].OnlyInHand)
					{
						if (!this.RequiredCardsOnBoard[j].NotInHand)
						{
							if (!instance.CardIsOnBoard(this.RequiredCardsOnBoard[j].TriggerCard, !this.RequiredCardsOnBoard[j].ExcludeInventories, true, false, false, null, Array.Empty<InGameCardBase>()))
							{
								if (failedCards == null)
								{
									return false;
								}
								failedCards.Add(this.RequiredCardsOnBoard[j].TriggerCard);
							}
						}
						else if (!instance.CardIsInBase(this.RequiredCardsOnBoard[j].TriggerCard, !this.RequiredCardsOnBoard[j].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()) && !instance.CardIsInLocation(this.RequiredCardsOnBoard[j].TriggerCard, !this.RequiredCardsOnBoard[j].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()))
						{
							if (failedCards == null)
							{
								return false;
							}
							failedCards.Add(this.RequiredCardsOnBoard[j].TriggerCard);
						}
					}
					else if (!instance.CardIsInHand(this.RequiredCardsOnBoard[j].TriggerCard, false, null, Array.Empty<InGameCardBase>()))
					{
						if (failedCards == null)
						{
							return false;
						}
						failedCards.Add(this.RequiredCardsOnBoard[j].TriggerCard);
					}
				}
				else if (!this.RequiredCardsOnBoard[j].OnlyInHand)
				{
					if (!this.RequiredCardsOnBoard[j].NotInHand)
					{
						if (instance.CardIsOnBoard(this.RequiredCardsOnBoard[j].TriggerCard, !this.RequiredCardsOnBoard[j].ExcludeInventories, true, false, false, null, Array.Empty<InGameCardBase>()))
						{
							if (failedCards == null)
							{
								return false;
							}
							failedCards.Add(this.RequiredCardsOnBoard[j].TriggerCard);
						}
					}
					else if (instance.CardIsInBase(this.RequiredCardsOnBoard[j].TriggerCard, !this.RequiredCardsOnBoard[j].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()) || instance.CardIsInLocation(this.RequiredCardsOnBoard[j].TriggerCard, !this.RequiredCardsOnBoard[j].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()))
					{
						if (failedCards == null)
						{
							return false;
						}
						failedCards.Add(this.RequiredCardsOnBoard[j].TriggerCard);
					}
				}
				else if (instance.CardIsInHand(this.RequiredCardsOnBoard[j].TriggerCard, false, null, Array.Empty<InGameCardBase>()))
				{
					if (failedCards == null)
					{
						return false;
					}
					failedCards.Add(this.RequiredCardsOnBoard[j].TriggerCard);
				}
			}
		}
		if (this.RequiredTagsOnBoard != null && this.RequiredTagsOnBoard.Length != 0)
		{
			for (int k = 0; k < this.RequiredTagsOnBoard.Length; k++)
			{
				if (!this.RequiredTagsOnBoard[k].Inverted)
				{
					if (!this.RequiredTagsOnBoard[k].OnlyInHand)
					{
						if (!this.RequiredTagsOnBoard[k].NotInHand)
						{
							if (!instance.TagIsOnBoard(this.RequiredTagsOnBoard[k].TriggerTag, !this.RequiredTagsOnBoard[k].ExcludeInventories, true, false, false, null))
							{
								if (failedTags == null)
								{
									return false;
								}
								failedTags.Add(this.RequiredTagsOnBoard[k].TriggerTag);
							}
						}
						else if (!instance.TagIsInBase(this.RequiredTagsOnBoard[k].TriggerTag, !this.RequiredTagsOnBoard[k].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()) && !instance.TagIsInLocation(this.RequiredTagsOnBoard[k].TriggerTag, !this.RequiredTagsOnBoard[k].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()))
						{
							if (failedTags == null)
							{
								return false;
							}
							failedTags.Add(this.RequiredTagsOnBoard[k].TriggerTag);
						}
					}
					else if (!instance.TagIsInHand(this.RequiredTagsOnBoard[k].TriggerTag, false, null, Array.Empty<InGameCardBase>()))
					{
						if (failedTags == null)
						{
							return false;
						}
						failedTags.Add(this.RequiredTagsOnBoard[k].TriggerTag);
					}
				}
				else if (!this.RequiredTagsOnBoard[k].OnlyInHand)
				{
					if (!this.RequiredTagsOnBoard[k].NotInHand)
					{
						if (instance.TagIsOnBoard(this.RequiredTagsOnBoard[k].TriggerTag, !this.RequiredTagsOnBoard[k].ExcludeInventories, true, false, false, null))
						{
							if (failedTags == null)
							{
								return false;
							}
							failedTags.Add(this.RequiredTagsOnBoard[k].TriggerTag);
						}
					}
					else if (instance.TagIsInBase(this.RequiredTagsOnBoard[k].TriggerTag, !this.RequiredTagsOnBoard[k].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()) || instance.TagIsInLocation(this.RequiredTagsOnBoard[k].TriggerTag, !this.RequiredTagsOnBoard[k].ExcludeInventories, false, null, Array.Empty<InGameCardBase>()))
					{
						if (failedTags == null)
						{
							return false;
						}
						failedTags.Add(this.RequiredTagsOnBoard[k].TriggerTag);
					}
				}
				else if (instance.TagIsInHand(this.RequiredTagsOnBoard[k].TriggerTag, false, null, Array.Empty<InGameCardBase>()))
				{
					if (failedTags == null)
					{
						return false;
					}
					failedTags.Add(this.RequiredTagsOnBoard[k].TriggerTag);
				}
			}
		}
		return (failedCards == null || failedCards.Count <= 0) && (failedTags == null || failedTags.Count <= 0);
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x0004C518 File Offset: 0x0004A718
	public bool DurabilitiesAreCorrect(InGameCardBase _ReceivingCard, out string _FailMessage)
	{
		_FailMessage = "";
		if (!_ReceivingCard)
		{
			return true;
		}
		if (this.RequiredReceivingDurabilities.CheckDurabilities(_ReceivingCard))
		{
			return this.RequiredReceivingLiquidContent.IsValid(_ReceivingCard, false);
		}
		_FailMessage = this.RequiredReceivingDurabilities.FailMessage;
		return false;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0004C568 File Offset: 0x0004A768
	public virtual bool WillHaveAnEffect(InGameCardBase _FromCard, bool _IgnoreProduceCard, bool _IgnoreChangeStat, bool _IgnoreCustomWindow, bool _CountTimeAsEffect, params CardModifications[] _IgnoreCardMods)
	{
		bool flag = !_IgnoreProduceCard;
		bool flag2 = !_IgnoreChangeStat;
		bool flag3 = this.ReceivingCardChanges.ModType != CardModifications.None || this.ReceivingCardChanges.ModifyLiquid;
		bool flag4 = this.CustomWindowPrefab != null && !_IgnoreCustomWindow;
		bool flag5 = _CountTimeAsEffect && this.DaytimeCost > 0;
		bool flag6 = this.WillModifyMoreCards();
		bool flag7 = this.BlueprintsFullUnlock != null;
		bool victory = this.VictorySettings.Victory;
		if (flag)
		{
			flag &= this.WillProduceCards(_FromCard);
		}
		if (flag2)
		{
			flag2 &= this.WillChangeStats();
		}
		if (flag7)
		{
			flag7 &= (this.BlueprintsFullUnlock.Length != 0);
		}
		if (flag3 && _IgnoreCardMods != null)
		{
			for (int i = 0; i < _IgnoreCardMods.Length; i++)
			{
				if (_IgnoreCardMods[i] == this.ReceivingCardChanges.ModType)
				{
					flag3 = false;
					break;
				}
			}
		}
		return flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7 || victory;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0004C654 File Offset: 0x0004A854
	private bool WillProduceCards(InGameCardBase _FromCard)
	{
		if (this.TravelToPreviousEnv && MBSingleton<GameManager>.Instance.PrevEnvironment)
		{
			return true;
		}
		if (this.ProducedCards == null)
		{
			return false;
		}
		if (this.ProducedCards.Length == 0)
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < this.ProducedCards.Length; i++)
		{
			if (!_FromCard || _FromCard.CanUseCollection(this.ProducedCards[i]))
			{
				if (this.ProducedCards[i].RevealInventory)
				{
					flag2 = true;
					break;
				}
				if (this.ProducedCards[i].TotalPossibleDrops > 0)
				{
					flag = true;
					break;
				}
			}
		}
		return flag || flag2;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0004C6EC File Offset: 0x0004A8EC
	private bool WillChangeStats()
	{
		if (this.StatModifications == null)
		{
			return false;
		}
		if (this.StatModifications.Length == 0)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < this.StatModifications.Length; i++)
		{
			if (this.StatModifications[i].Stat != null && (this.StatModifications[i].ValueModifier.sqrMagnitude > 0f || this.StatModifications[i].RateModifier.sqrMagnitude > 0f))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0004C77B File Offset: 0x0004A97B
	private bool WillModifyMoreCards()
	{
		return this.ExtraDurabilityModifications != null && this.ExtraDurabilityModifications.Length != 0;
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x0004BC68 File Offset: 0x00049E68
	public bool EnoughDaylightPoints()
	{
		return true;
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x0004C793 File Offset: 0x0004A993
	public void SetDayTimeCost(int _Cost)
	{
		this.DaytimeCost = _Cost;
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0004C79C File Offset: 0x0004A99C
	public int MiniTicksCost
	{
		get
		{
			if (base.GetType() != typeof(DismantleCardAction) && base.GetType() != typeof(CardOnCardAction))
			{
				return 0;
			}
			if (this.TotalDaytimeCost > 0)
			{
				return 0;
			}
			return GameManager.GetMiniTicksAmt(this.UseMiniTicks);
		}
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0004C7F0 File Offset: 0x0004A9F0
	public string TooltipDescription(bool _EnoughTime, bool _AvailableInDemo, float _SuccessChance, bool _ActionPlaying, string _MissingDurabilities, string _BlockingStatus, MissingStatInfo[] _MissingStats, CardData[] _MissingCards, CardTag[] _MissingTags)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		if (!string.IsNullOrEmpty(this.ActionDescription))
		{
			stringBuilder.Append(this.ActionDescription);
			flag = true;
		}
		if (this.TotalDaytimeCost > 0 || this.MiniTicksCost > 0)
		{
			if (flag)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append(LocalizedString.Duration + ": ");
			stringBuilder.Append(HoursDisplay.HoursToCompleteString(GameManager.TickToHours(this.TotalDaytimeCost, this.MiniTicksCost)));
			flag = true;
		}
		if (!_EnoughTime)
		{
			if (flag)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append("Not enough time before night");
			flag = true;
		}
		if (!_AvailableInDemo)
		{
			if (flag)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append(LocalizedString.UnavailableInDemo);
			flag = true;
		}
		if (_ActionPlaying)
		{
			if (flag)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append(LocalizedString.ActionHappening);
			flag = true;
		}
		if (_SuccessChance >= 0f)
		{
			if (flag)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append(LocalizedString.SuccessChance);
			stringBuilder.Append(": ");
			if (MBSingleton<GameManager>.Instance)
			{
				if (MBSingleton<GameManager>.Instance.SuccessChances)
				{
					if (!string.IsNullOrEmpty(MBSingleton<GameManager>.Instance.SuccessChances.GetSuccessLabel(_SuccessChance)))
					{
						stringBuilder.Append(MBSingleton<GameManager>.Instance.SuccessChances.GetSuccessLabel(_SuccessChance));
					}
					else
					{
						stringBuilder.Append(string.Format("{0}%", (_SuccessChance * 100f).ToString("0")));
					}
				}
				else
				{
					stringBuilder.Append(string.Format("{0}%", (_SuccessChance * 100f).ToString("0")));
				}
			}
			else
			{
				stringBuilder.Append(string.Format("{0}%", (_SuccessChance * 100f).ToString("0")));
			}
			flag = true;
		}
		if (_MissingStats != null)
		{
			for (int i = 0; i < _MissingStats.Length; i++)
			{
				if (!string.IsNullOrEmpty(_MissingStats[i].GetNotification))
				{
					if (flag)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append(_MissingStats[i].GetNotification);
					flag = true;
				}
			}
		}
		if (!string.IsNullOrEmpty(_MissingDurabilities))
		{
			stringBuilder.Append("\n");
			stringBuilder.Append(_MissingDurabilities);
		}
		if (!string.IsNullOrEmpty(_BlockingStatus))
		{
			stringBuilder.Append("\n");
			stringBuilder.Append(_BlockingStatus);
		}
		if (_MissingCards != null)
		{
			for (int j = 0; j < _MissingCards.Length; j++)
			{
				if (flag)
				{
					stringBuilder.Append("\n");
				}
				bool inHand = false;
				for (int k = 0; k < this.RequiredCardsOnBoard.Length; k++)
				{
					if (this.RequiredCardsOnBoard[k].TriggerCard == _MissingCards[j])
					{
						inHand = this.RequiredCardsOnBoard[k].OnlyInHand;
						break;
					}
				}
				stringBuilder.Append(LocalizedString.MissingCard(_MissingCards[j].CardName, inHand));
				flag = true;
			}
		}
		if (_MissingTags != null)
		{
			for (int l = 0; l < _MissingTags.Length; l++)
			{
				if (flag)
				{
					stringBuilder.Append("\n");
				}
				bool inHand2 = false;
				for (int m = 0; m < this.RequiredTagsOnBoard.Length; m++)
				{
					if (this.RequiredTagsOnBoard[m].TriggerTag == _MissingTags[l])
					{
						inHand2 = this.RequiredTagsOnBoard[m].OnlyInHand;
						break;
					}
				}
				stringBuilder.Append(LocalizedString.MissingCard(_MissingTags[l].InGameName, inHand2));
				flag = true;
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0004CBAC File Offset: 0x0004ADAC
	public bool HasActionSounds
	{
		get
		{
			if (this.ActionSounds == null)
			{
				return false;
			}
			if (this.ActionSounds.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.ActionSounds.Length; i++)
			{
				if (this.ActionSounds[i])
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0004CBF4 File Offset: 0x0004ADF4
	public bool HasSuccessfulDrop
	{
		get
		{
			if (this.ProducedCards == null)
			{
				return false;
			}
			for (int i = 0; i < this.ProducedCards.Length; i++)
			{
				if (this.ProducedCards[i].CountsAsSuccess)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x04000B2A RID: 2858
	public LocalizedString ActionName;

	// Token: 0x04000B2B RID: 2859
	public LocalizedString ActionDescription;

	// Token: 0x04000B2C RID: 2860
	public bool NoveltyShared;

	// Token: 0x04000B2D RID: 2861
	public VictoryCondition VictorySettings;

	// Token: 0x04000B2E RID: 2862
	public EndgameLog ActionLog;

	// Token: 0x04000B2F RID: 2863
	public bool UnavailableInDemo;

	// Token: 0x04000B30 RID: 2864
	public bool SaveGame;

	// Token: 0x04000B31 RID: 2865
	public bool ConfirmPopup;

	// Token: 0x04000B32 RID: 2866
	public bool StackCompatible;

	// Token: 0x04000B33 RID: 2867
	[SerializeField]
	protected int DaytimeCost;

	// Token: 0x04000B34 RID: 2868
	public MiniTicksBehavior UseMiniTicks;

	// Token: 0x04000B35 RID: 2869
	public AudioClip[] ActionSounds;

	// Token: 0x04000B36 RID: 2870
	public bool DisablePitchVariation;

	// Token: 0x04000B37 RID: 2871
	public ActionTag[] ActionTags;

	// Token: 0x04000B38 RID: 2872
	[Space]
	[StatCondition]
	public StatValueTrigger[] RequiredStatValues;

	// Token: 0x04000B39 RID: 2873
	public CardOnBoardCondition[] RequiredCardsOnBoard;

	// Token: 0x04000B3A RID: 2874
	public TagOnBoardCondition[] RequiredTagsOnBoard;

	// Token: 0x04000B3B RID: 2875
	[FormerlySerializedAs("RequiredContainer")]
	public CardData[] RequiredReceivingContainer;

	// Token: 0x04000B3C RID: 2876
	[FormerlySerializedAs("RequiredContainerTag")]
	public CardTag[] RequiredReceivingContainerTag;

	// Token: 0x04000B3D RID: 2877
	public DurabilityConditions RequiredReceivingDurabilities;

	// Token: 0x04000B3E RID: 2878
	public LiquidContentCondition RequiredReceivingLiquidContent;

	// Token: 0x04000B3F RID: 2879
	public bool NotBaseAction;

	// Token: 0x04000B40 RID: 2880
	public bool Cancellable;

	// Token: 0x04000B41 RID: 2881
	public bool InstantStatModifications;

	// Token: 0x04000B42 RID: 2882
	public GameStat ResetWhenDone;

	// Token: 0x04000B43 RID: 2883
	[Space]
	public StatInterruptionCondition[] StatInterruptions;

	// Token: 0x04000B44 RID: 2884
	[Space]
	public CardsDropCollection[] ProducedCards;

	// Token: 0x04000B45 RID: 2885
	[Space]
	[StatModifierOptions(false, true)]
	public StatModifier[] StatModifications;

	// Token: 0x04000B46 RID: 2886
	[Space]
	public ExtraDurabilityChange[] ExtraDurabilityModifications;

	// Token: 0x04000B47 RID: 2887
	public CardData[] BlueprintsFullUnlock;

	// Token: 0x04000B48 RID: 2888
	public bool DontShowDestroyMessage;

	// Token: 0x04000B49 RID: 2889
	public LocalizedString CustomDestroyMessage;

	// Token: 0x04000B4A RID: 2890
	public LocalizedString NoCardsAffectedMessage;

	// Token: 0x04000B4B RID: 2891
	[Space]
	public CardStateChange ReceivingCardChanges;

	// Token: 0x04000B4C RID: 2892
	[Space]
	public GameObject CustomWindowPrefab;

	// Token: 0x04000B4D RID: 2893
	public FadeToBlackTypes FadeToBlack;

	// Token: 0x04000B4E RID: 2894
	public LocalizedString FadeMessage;

	// Token: 0x04000B4F RID: 2895
	public bool FadeTips;

	// Token: 0x04000B50 RID: 2896
	public bool TravelToPreviousEnv;

	// Token: 0x04000B51 RID: 2897
	[NonSerialized]
	public List<SimpleCardSaveData> LoadedCards = new List<SimpleCardSaveData>();

	// Token: 0x04000B52 RID: 2898
	[NonSerialized]
	public List<StatModifier> AllStatModifiers = new List<StatModifier>();

	// Token: 0x04000B53 RID: 2899
	[NonSerialized]
	public TransferedDurabilities GivenDurabilityChanges = new TransferedDurabilities();

	// Token: 0x04000B54 RID: 2900
	[NonSerialized]
	public TransferedDurabilities ReceivingDurabilityChanges = new TransferedDurabilities();

	// Token: 0x04000B55 RID: 2901
	[NonSerialized]
	public int TotalDaytimeCost;

	// Token: 0x04000B56 RID: 2902
	[NonSerialized]
	public float StatsLiquidScale;

	// Token: 0x04000B57 RID: 2903
	[NonSerialized]
	public float DurabilitiesLiquidScale;

	// Token: 0x04000B58 RID: 2904
	[NonSerialized]
	public string ActionBlockedMessage;

	// Token: 0x04000B59 RID: 2905
	[NonSerialized]
	public int DropsMultiplier;

	// Token: 0x04000B5A RID: 2906
	[NonSerialized]
	public bool CountAsExploration;

	// Token: 0x04000B5B RID: 2907
	public StackActionStopConditions StackStopConditions;
}
