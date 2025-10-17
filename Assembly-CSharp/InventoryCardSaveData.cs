using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000016 RID: 22
[Serializable]
public class InventoryCardSaveData : CardSaveData
{
	// Token: 0x060001EC RID: 492 RVA: 0x00014474 File Offset: 0x00012674
	public void SaveCard(InGameCardBase _Card, List<InventoryCardSaveData> _AdditionalInventories, bool _OverrideEnvironment)
	{
		InventoryCardSaveData.<>c__DisplayClass5_0 CS$<>8__locals1;
		CS$<>8__locals1._OverrideEnvironment = _OverrideEnvironment;
		CS$<>8__locals1._Card = _Card;
		CS$<>8__locals1._AdditionalInventories = _AdditionalInventories;
		CS$<>8__locals1.<>4__this = this;
		base.SaveCard(CS$<>8__locals1._Card);
		this.CookingCards = new CookingCardStatus[CS$<>8__locals1._Card.CookingCards.Count];
		this.CookingResults = new List<string>();
		CS$<>8__locals1._Card.SaveCookingResults(ref this.CookingResults);
		if (CS$<>8__locals1._Card.CardsInInventory.Count > 0)
		{
			for (int i = 0; i < CS$<>8__locals1._Card.CardsInInventory.Count; i++)
			{
				InGameCardBase mainCard = CS$<>8__locals1._Card.CardsInInventory[i].MainCard;
				if (mainCard)
				{
					if (CS$<>8__locals1._Card.IsLegacyInventory)
					{
						this.<SaveCard>g__SaveContainedCard|5_0(mainCard, CS$<>8__locals1._Card.CardsInInventory[i].CardAmt, ref CS$<>8__locals1);
					}
					else
					{
						for (int j = 0; j < CS$<>8__locals1._Card.CardsInInventory[i].CardAmt; j++)
						{
							this.<SaveCard>g__SaveContainedCard|5_0(CS$<>8__locals1._Card.CardsInInventory[i].AllCards[j], 1, ref CS$<>8__locals1);
						}
					}
				}
				else
				{
					this.CardsInInventory.Add(new InventorySlotSaveData(-1));
				}
				if (CS$<>8__locals1._Card.CookingCards.Count > 0)
				{
					int k = 0;
					while (k < CS$<>8__locals1._Card.CookingCards.Count)
					{
						bool usesLiquid = false;
						if (!(CS$<>8__locals1._Card.CookingCards[k].Card != CS$<>8__locals1._Card.CardsInInventory[i].MainCard))
						{
							goto IL_207;
						}
						if (CS$<>8__locals1._Card.CardsInInventory[i].MainCard && CS$<>8__locals1._Card.CardsInInventory[i].MainCard.ContainedLiquid && CS$<>8__locals1._Card.CookingCards[k].Card == CS$<>8__locals1._Card.CardsInInventory[i].MainCard.ContainedLiquid)
						{
							usesLiquid = true;
							goto IL_207;
						}
						IL_2FA:
						k++;
						continue;
						IL_207:
						this.CookingCards[k] = new CookingCardStatus(null, CS$<>8__locals1._Card.CookingCards[k].TargetDuration, CS$<>8__locals1._Card.CookingCards[k].CookingText, CS$<>8__locals1._Card.CookingCards[k].PausedCookingText, CS$<>8__locals1._Card.CookingCards[k].SelfPausedText, CS$<>8__locals1._Card.CookingCards[k].HideCookingProgress, CS$<>8__locals1._Card.CookingCards[k].FillsCookerLiquid, CS$<>8__locals1._Card.CookingCards[k].FillsIngredientLiquid);
						this.CookingCards[k].CardIndex = i;
						this.CookingCards[k].UsesLiquid = usesLiquid;
						this.CookingCards[k].CookedDuration = CS$<>8__locals1._Card.CookingCards[k].CookedDuration;
						goto IL_2FA;
					}
				}
			}
		}
		if (CS$<>8__locals1._Card.ContainedLiquid)
		{
			if (CS$<>8__locals1._OverrideEnvironment)
			{
				CS$<>8__locals1._Card.ContainedLiquid.Environment = CS$<>8__locals1._Card.Environment;
				CS$<>8__locals1._Card.ContainedLiquid.PrevEnvironment = CS$<>8__locals1._Card.PrevEnvironment;
				CS$<>8__locals1._Card.ContainedLiquid.PrevEnvTravelIndex = CS$<>8__locals1._Card.PrevEnvTravelIndex;
				CS$<>8__locals1._Card.ContainedLiquid.CreatedInSaveDataTick = CS$<>8__locals1._Card.CreatedInSaveDataTick;
			}
			this.LiquidContained = CS$<>8__locals1._Card.ContainedLiquid.Save();
			return;
		}
		if (CS$<>8__locals1._Card.IsPinned)
		{
			this.LiquidContained = new CardSaveData();
			this.PinnedLiquidCard = UniqueIDScriptable.SaveID(CS$<>8__locals1._Card.ContainedLiquidModel);
		}
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00014894 File Offset: 0x00012A94
	[CompilerGenerated]
	private void <SaveCard>g__SaveContainedCard|5_0(InGameCardBase _ContainedCard, int _Amt, ref InventoryCardSaveData.<>c__DisplayClass5_0 A_3)
	{
		if (!_ContainedCard)
		{
			return;
		}
		if (A_3._OverrideEnvironment)
		{
			_ContainedCard.Environment = A_3._Card.Environment;
			_ContainedCard.PrevEnvironment = A_3._Card.PrevEnvironment;
			_ContainedCard.PrevEnvTravelIndex = A_3._Card.PrevEnvTravelIndex;
			_ContainedCard.CreatedInSaveDataTick = A_3._Card.CreatedInSaveDataTick;
		}
		if (_ContainedCard.CardsInInventory.Count > 0 || _ContainedCard.IsLiquidContainer)
		{
			A_3._AdditionalInventories.Add(_ContainedCard.SaveInventory(A_3._AdditionalInventories, A_3._OverrideEnvironment));
			this.CardsInInventory.Add(new InventorySlotSaveData(A_3._AdditionalInventories.Count - 1));
			return;
		}
		this.CardsInInventory.Add(new InventorySlotSaveData(_ContainedCard.Save(), _Amt));
	}

	// Token: 0x040001E3 RID: 483
	public List<InventorySlotSaveData> CardsInInventory = new List<InventorySlotSaveData>();

	// Token: 0x040001E4 RID: 484
	public CookingCardStatus[] CookingCards;

	// Token: 0x040001E5 RID: 485
	public List<string> CookingResults;

	// Token: 0x040001E6 RID: 486
	public CardSaveData LiquidContained;

	// Token: 0x040001E7 RID: 487
	public string PinnedLiquidCard;
}
