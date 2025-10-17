using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D6 RID: 214
[Serializable]
public class CardOnCardAction : CardAction
{
	// Token: 0x060007BD RID: 1981 RVA: 0x0004CCAA File Offset: 0x0004AEAA
	public CardOnCardAction()
	{
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0004CCB2 File Offset: 0x0004AEB2
	public CardOnCardAction(LocalizedString _Name, LocalizedString _Desc, int _Duration)
	{
		this.ActionName = _Name;
		this.ActionDescription = _Desc;
		this.DaytimeCost = _Duration;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0004CCCF File Offset: 0x0004AECF
	public override bool WillHaveAnEffect(InGameCardBase _FromCard, bool _IgnoreProduceCards, bool _IgnoreChangeStats, bool _IgnoreCustomWindow, bool _CountTimeAsEffect, params CardModifications[] _IgnoreMods)
	{
		return base.WillHaveAnEffect(_FromCard, _IgnoreProduceCards, _IgnoreChangeStats, _IgnoreCustomWindow, _CountTimeAsEffect, _IgnoreMods) || this.GivenCardChanges.ModType != CardModifications.None || !this.CreatedLiquidInGivenCard.IsEmpty || this.GivenCardChanges.ModifyLiquid;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0004CD0C File Offset: 0x0004AF0C
	public bool CanGiveLiquid(InGameCardBase _ContainerCard)
	{
		return _ContainerCard.IsLiquidContainer && !this.CreatedLiquidInGivenCard.IsEmpty && (!_ContainerCard.ContainedLiquid || _ContainerCard.ContainedLiquid.CardModel == this.CreatedLiquidInGivenCard.LiquidCard);
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0004CD60 File Offset: 0x0004AF60
	public bool CardsAndTagsAreCorrect(InGameCardBase _Receiving, InGameCardBase _Given, List<CardData> failedCards = null, List<CardTag> failedTags = null)
	{
		if (_Given)
		{
			if (this.RequiredGivenContainer != null && this.RequiredGivenContainer.Length != 0)
			{
				bool flag = false;
				for (int i = 0; i < this.RequiredGivenContainer.Length; i++)
				{
					if (!_Given.CurrentContainer)
					{
						if (!this.RequiredGivenContainer[i])
						{
							flag = true;
							break;
						}
					}
					else if (this.RequiredGivenContainer[i] == _Given.CurrentContainer.CardModel)
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
			if (this.RequiredGivenContainerTag != null && this.RequiredGivenContainerTag.Length != 0)
			{
				if (!_Given.CurrentContainer)
				{
					return false;
				}
				if (!_Given.CurrentContainer.CardModel.HasAnyTag(this.RequiredGivenContainerTag))
				{
					return false;
				}
			}
		}
		return base.CardsAndTagsAreCorrect(_Receiving, failedCards, failedTags);
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0004CE28 File Offset: 0x0004B028
	public bool DurabilitiesAreCorrect(InGameCardBase _ReceivingCard, InGameCardBase _GivenCard, out string _FailMessage)
	{
		string text = "";
		string text2 = this.RequiredGivenDurabilities.FailMessage;
		bool flag = base.DurabilitiesAreCorrect(_ReceivingCard, out text);
		bool flag2 = this.RequiredGivenDurabilities.CheckDurabilities(_GivenCard) && this.RequiredGivenLiquidContent.IsValid(_GivenCard, false);
		if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
		{
			if (flag && !flag2)
			{
				_FailMessage = text2;
			}
			else if (!flag && flag2)
			{
				_FailMessage = text;
			}
			else
			{
				_FailMessage = text2;
			}
		}
		else if (string.IsNullOrEmpty(text))
		{
			_FailMessage = text2;
		}
		else
		{
			_FailMessage = text;
		}
		return flag && flag2;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0004CEB8 File Offset: 0x0004B0B8
	protected override void CalculateLiquidScales(InGameCardBase _ReceivingCard, InGameCardBase _GivenCard)
	{
		base.CalculateLiquidScales(_ReceivingCard, _GivenCard);
		if (_GivenCard && (_GivenCard.IsLiquid || _GivenCard.ContainedLiquid))
		{
			float num = this.GivenCardChanges.StatLiquidScalingValue(_GivenCard);
			float num2 = this.GivenCardChanges.DurabilitiesLiquidScalingValue(_GivenCard);
			if (this.StatsLiquidScale >= 0f)
			{
				if (num >= 0f)
				{
					this.StatsLiquidScale = (this.StatsLiquidScale + num) * 0.5f;
				}
			}
			else
			{
				this.StatsLiquidScale = num;
			}
			if (this.DurabilitiesLiquidScale >= 0f)
			{
				if (num2 >= 0f)
				{
					this.DurabilitiesLiquidScale = (this.DurabilitiesLiquidScale + num2) * 0.5f;
					return;
				}
			}
			else
			{
				this.DurabilitiesLiquidScale = num2;
			}
		}
	}

	// Token: 0x04000B66 RID: 2918
	[Space]
	public CardData[] RequiredGivenContainer;

	// Token: 0x04000B67 RID: 2919
	public CardTag[] RequiredGivenContainerTag;

	// Token: 0x04000B68 RID: 2920
	public DurabilityConditions RequiredGivenDurabilities;

	// Token: 0x04000B69 RID: 2921
	public LiquidContentCondition RequiredGivenLiquidContent;

	// Token: 0x04000B6A RID: 2922
	public CardStateChange GivenCardChanges;

	// Token: 0x04000B6B RID: 2923
	public LiquidDrop CreatedLiquidInGivenCard;

	// Token: 0x04000B6C RID: 2924
	public bool CarryOverGivenCard;

	// Token: 0x04000B6D RID: 2925
	[Space]
	public CardInteractionTrigger CompatibleCards;

	// Token: 0x04000B6E RID: 2926
	public bool WorksBothWays;
}
