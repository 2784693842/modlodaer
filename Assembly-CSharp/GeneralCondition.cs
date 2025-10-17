using System;
using System.Collections.Generic;

// Token: 0x020000F5 RID: 245
[Serializable]
public struct GeneralCondition
{
	// Token: 0x0600082C RID: 2092 RVA: 0x00050678 File Offset: 0x0004E878
	public bool ConditionsValid(bool _NotInBase, InGameCardBase _FromCard)
	{
		if (_FromCard)
		{
			if (_FromCard.InBackground && this.NotInBackground)
			{
				return false;
			}
			if (!this.RequiredDurabilityRanges.ValidConditions(_FromCard))
			{
				return false;
			}
			if (this.RequiredCardsInInventory != null && this.RequiredCardsInInventory.Length != 0)
			{
				bool flag = false;
				for (int i = 0; i < this.RequiredCardsInInventory.Length; i++)
				{
					if (this.RequiredCardsInInventory[i] && _FromCard.HasCardInInventory(this.RequiredCardsInInventory[i]))
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
			if (this.RequiredTagsInInventory != null && this.RequiredTagsInInventory.Length != 0)
			{
				bool flag2 = false;
				for (int j = 0; j < this.RequiredTagsInInventory.Length; j++)
				{
					if (this.RequiredTagsInInventory[j] && _FromCard.HasTagInInventory(this.RequiredTagsInInventory[j]))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
		}
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (this.RequiredEnvironment != null)
		{
			if (_FromCard)
			{
				if (_FromCard.Environment != this.RequiredEnvironment)
				{
					return false;
				}
			}
			else if (instance.CurrentEnvironment != this.RequiredEnvironment)
			{
				return false;
			}
		}
		if (this.RequiredEnvironmentTags != null && this.RequiredEnvironmentTags.Length != 0)
		{
			for (int k = 0; k < this.RequiredEnvironmentTags.Length; k++)
			{
				if (this.RequiredEnvironmentTags[k])
				{
					if (_FromCard)
					{
						if (_FromCard.Environment && !_FromCard.Environment.HasTag(this.RequiredEnvironmentTags[k]))
						{
							return false;
						}
					}
					else if (instance.CurrentEnvironment && !instance.CurrentEnvironment.HasTag(this.RequiredEnvironmentTags[k]))
					{
						return false;
					}
				}
			}
		}
		if (_FromCard)
		{
			if (this.RequiredContainer != null && this.RequiredContainer.Length != 0)
			{
				if (!_FromCard.CurrentContainer)
				{
					return false;
				}
				bool flag3 = false;
				for (int l = 0; l < this.RequiredContainer.Length; l++)
				{
					if (_FromCard.CurrentContainer.CardModel == this.RequiredContainer[l])
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					return false;
				}
			}
			if (this.RequiredContainerTags != null && this.RequiredContainerTags.Length != 0)
			{
				if (!_FromCard.CurrentContainer)
				{
					return false;
				}
				if (!_FromCard.CurrentContainer.CardModel)
				{
					return false;
				}
				for (int m = 0; m < this.RequiredContainerTags.Length; m++)
				{
					if (this.RequiredContainerTags[m] && !_FromCard.CurrentContainer.CardModel.HasTag(this.RequiredContainerTags[m]))
					{
						return false;
					}
				}
			}
		}
		if (this.RequiredCardsOnBoard != null && this.RequiredCardsOnBoard.Length != 0)
		{
			for (int n = 0; n < this.RequiredCardsOnBoard.Length; n++)
			{
				if (!instance.CardIsOnBoard(this.RequiredCardsOnBoard[n], true, true, false, false, null, Array.Empty<InGameCardBase>()))
				{
					return false;
				}
			}
		}
		if (this.RequiredCardsNOTOnBoard != null && this.RequiredCardsNOTOnBoard.Length != 0)
		{
			for (int num = 0; num < this.RequiredCardsNOTOnBoard.Length; num++)
			{
				if (instance.CardIsOnBoard(this.RequiredCardsNOTOnBoard[num], true, true, false, false, null, Array.Empty<InGameCardBase>()))
				{
					return false;
				}
			}
		}
		if (this.RequiredTagsOnBoard != null && this.RequiredTagsOnBoard.Length != 0)
		{
			for (int num2 = 0; num2 < this.RequiredTagsOnBoard.Length; num2++)
			{
				if (!instance.TagIsOnBoard(this.RequiredTagsOnBoard[num2], true, true, false, false, null))
				{
					return false;
				}
			}
		}
		if (this.RequiredTagsNOTOnBoard != null && this.RequiredTagsNOTOnBoard.Length != 0)
		{
			for (int num3 = 0; num3 < this.RequiredTagsNOTOnBoard.Length; num3++)
			{
				if (instance.TagIsOnBoard(this.RequiredTagsNOTOnBoard[num3], true, true, false, false, null))
				{
					return false;
				}
			}
		}
		if (this.RequiredLiquidContainers != null && this.RequiredLiquidContainers.Length != 0)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			for (int num4 = 0; num4 < this.RequiredLiquidContainers.Length; num4++)
			{
				list.Clear();
				if (this.RequiredLiquidContainers[num4].Card)
				{
					instance.CardIsOnBoard(this.RequiredLiquidContainers[num4].Card, true, true, false, false, list, Array.Empty<InGameCardBase>());
				}
				else if (this.RequiredLiquidContainers[num4].Tag)
				{
					instance.TagIsOnBoard(this.RequiredLiquidContainers[num4].Tag, true, true, false, false, list);
				}
				if (list.Count == 0)
				{
					return false;
				}
				for (int num5 = list.Count - 1; num5 >= 0; num5--)
				{
					if (!this.RequiredLiquidContainers[num4].Liquid && !this.RequiredLiquidContainers[num4].LiquidTag && list[num5].ContainedLiquidModel != null)
					{
						list.RemoveAt(num5);
					}
					else if (this.RequiredLiquidContainers[num4].Liquid && list[num5].ContainedLiquidModel != this.RequiredLiquidContainers[num4].Liquid)
					{
						list.RemoveAt(num5);
					}
					else if (this.RequiredLiquidContainers[num4].LiquidTag)
					{
						if (!list[num5].ContainedLiquidModel)
						{
							list.RemoveAt(num5);
						}
						else if (!list[num5].ContainedLiquidModel.HasTag(this.RequiredLiquidContainers[num4].LiquidTag))
						{
							list.RemoveAt(num5);
						}
					}
				}
				if (list.Count == 0)
				{
					return false;
				}
			}
		}
		if (this.RequiredStatValues != null && this.RequiredStatValues.Length != 0)
		{
			for (int num6 = 0; num6 < this.RequiredStatValues.Length; num6++)
			{
				if (this.RequiredStatValues[num6].Stat && !this.RequiredStatValues[num6].IsInRange(instance.StatsDict[this.RequiredStatValues[num6].Stat].CurrentValue(_NotInBase)))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00050C88 File Offset: 0x0004EE88
	public int RequiredCardsCount(InGameCardBase _ForCard)
	{
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (!instance)
		{
			return 1;
		}
		List<InGameCardBase> list = new List<InGameCardBase>();
		if (this.RequiredCardsOnBoard != null && this.RequiredCardsOnBoard.Length != 0)
		{
			for (int i = 0; i < this.RequiredCardsOnBoard.Length; i++)
			{
				instance.CardIsOnBoard(this.RequiredCardsOnBoard[i], true, true, false, false, list, Array.Empty<InGameCardBase>());
			}
		}
		if (this.RequiredTagsOnBoard != null && this.RequiredTagsOnBoard.Length != 0)
		{
			for (int j = 0; j < this.RequiredTagsOnBoard.Length; j++)
			{
				instance.TagIsOnBoard(this.RequiredTagsOnBoard[j], true, true, false, false, list);
			}
		}
		if (this.RequiredCardsInInventory != null && _ForCard && this.RequiredCardsInInventory.Length != 0 && _ForCard.HasInventoryContent)
		{
			for (int k = 0; k < _ForCard.CardsInInventory.Count; k++)
			{
				if (_ForCard.CardsInInventory[k] != null)
				{
					for (int l = 0; l < this.RequiredCardsInInventory.Length; l++)
					{
						if (!(this.RequiredCardsInInventory[l] == null) && !(_ForCard.CardsInInventory[k].CardModel == null) && _ForCard.CardsInInventory[k].CardModel == this.RequiredCardsInInventory[l])
						{
							for (int m = 0; m < _ForCard.CardsInInventory[k].CardAmt; m++)
							{
								if (_ForCard.CardsInInventory[k].AllCards[m])
								{
									list.Add(_ForCard.CardsInInventory[k].AllCards[m]);
								}
							}
						}
					}
				}
			}
		}
		if (this.RequiredTagsInInventory != null && _ForCard && this.RequiredTagsInInventory.Length != 0 && _ForCard.HasInventoryContent)
		{
			for (int n = 0; n < _ForCard.CardsInInventory.Count; n++)
			{
				if (_ForCard.CardsInInventory[n] != null)
				{
					for (int num = 0; num < this.RequiredTagsInInventory.Length; num++)
					{
						if (!(this.RequiredTagsInInventory[num] == null) && !(_ForCard.CardsInInventory[n].CardModel == null) && _ForCard.CardsInInventory[n].CardModel.HasTag(this.RequiredTagsInInventory[num]))
						{
							for (int num2 = 0; num2 < _ForCard.CardsInInventory[n].CardAmt; num2++)
							{
								if (_ForCard.CardsInInventory[n].AllCards[num2])
								{
									list.Add(_ForCard.CardsInInventory[n].AllCards[num2]);
								}
							}
						}
					}
				}
			}
		}
		if (this.RequiredLiquidContainers != null && this.RequiredLiquidContainers.Length != 0)
		{
			List<InGameCardBase> list2 = new List<InGameCardBase>();
			for (int num3 = 0; num3 < this.RequiredLiquidContainers.Length; num3++)
			{
				list2.Clear();
				if (this.RequiredLiquidContainers[num3].Card)
				{
					instance.CardIsOnBoard(this.RequiredLiquidContainers[num3].Card, true, true, false, false, list2, Array.Empty<InGameCardBase>());
				}
				else if (this.RequiredLiquidContainers[num3].Tag)
				{
					instance.TagIsOnBoard(this.RequiredLiquidContainers[num3].Tag, true, true, false, false, list2);
				}
				if (list2.Count != 0)
				{
					for (int num4 = list2.Count - 1; num4 >= 0; num4--)
					{
						if (!this.RequiredLiquidContainers[num3].Liquid && !this.RequiredLiquidContainers[num3].LiquidTag && list2[num4].ContainedLiquidModel != null)
						{
							list2.RemoveAt(num4);
						}
						else if (this.RequiredLiquidContainers[num3].Liquid && list2[num4].ContainedLiquidModel != this.RequiredLiquidContainers[num3].Liquid)
						{
							list2.RemoveAt(num4);
						}
						else if (this.RequiredLiquidContainers[num3].LiquidTag)
						{
							if (!list2[num4].ContainedLiquidModel)
							{
								list2.RemoveAt(num4);
							}
							else if (!list2[num4].ContainedLiquidModel.HasTag(this.RequiredLiquidContainers[num3].LiquidTag))
							{
								list2.RemoveAt(num4);
							}
						}
					}
					if (list2.Count > 0)
					{
						for (int num5 = 0; num5 < list2.Count; num5++)
						{
							if (!list.Contains(list2[num5]))
							{
								list.Add(list2[num5]);
							}
						}
					}
				}
			}
		}
		return list.Count;
	}

	// Token: 0x04000C57 RID: 3159
	public bool NotInBackground;

	// Token: 0x04000C58 RID: 3160
	public CardData RequiredEnvironment;

	// Token: 0x04000C59 RID: 3161
	public CardTag[] RequiredEnvironmentTags;

	// Token: 0x04000C5A RID: 3162
	public CardData[] RequiredContainer;

	// Token: 0x04000C5B RID: 3163
	public CardTag[] RequiredContainerTags;

	// Token: 0x04000C5C RID: 3164
	public CardData[] RequiredCardsOnBoard;

	// Token: 0x04000C5D RID: 3165
	public CardTag[] RequiredTagsOnBoard;

	// Token: 0x04000C5E RID: 3166
	public CardData[] RequiredCardsNOTOnBoard;

	// Token: 0x04000C5F RID: 3167
	public CardTag[] RequiredTagsNOTOnBoard;

	// Token: 0x04000C60 RID: 3168
	public CardGeneralCondition[] RequiredLiquidContainers;

	// Token: 0x04000C61 RID: 3169
	public CardData[] RequiredCardsInInventory;

	// Token: 0x04000C62 RID: 3170
	public CardTag[] RequiredTagsInInventory;

	// Token: 0x04000C63 RID: 3171
	public DurabilitiesConditions RequiredDurabilityRanges;

	// Token: 0x04000C64 RID: 3172
	public StatValueTrigger[] RequiredStatValues;
}
