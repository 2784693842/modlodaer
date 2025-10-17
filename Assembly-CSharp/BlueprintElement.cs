using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FD RID: 253
[Serializable]
public struct BlueprintElement
{
	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000841 RID: 2113 RVA: 0x00051A69 File Offset: 0x0004FC69
	public bool DontDestroy
	{
		get
		{
			return this.DontSpend && this.EffectOnIngredient.ModType != CardModifications.Destroy;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000842 RID: 2114 RVA: 0x00051A86 File Offset: 0x0004FC86
	public int GetQuantity
	{
		get
		{
			if (!this.AnyCard)
			{
				return 1;
			}
			return Mathf.Max(1, this.RequiredQuantity);
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000843 RID: 2115 RVA: 0x00051AA4 File Offset: 0x0004FCA4
	public int GetLiquidQuantity
	{
		get
		{
			if (!this.RequiredLiquidContent.IsActive && !this.AnyLiquid)
			{
				return 0;
			}
			if (this.RequiredLiquidContent.IsEmpty && !this.AnyLiquid)
			{
				return 0;
			}
			if (this.RequiredLiquidContent.RequiredQuantity.y > this.RequiredLiquidContent.RequiredQuantity.x)
			{
				return Mathf.Max(1, Mathf.CeilToInt(Mathf.Min(this.RequiredLiquidContent.RequiredQuantity.x, this.RequiredLiquidContent.RequiredQuantity.y)));
			}
			return Mathf.Max(1, Mathf.CeilToInt(this.RequiredLiquidContent.RequiredQuantity.x));
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00051B57 File Offset: 0x0004FD57
	public float GetLiquidUnits(float _UnitValue)
	{
		if (Mathf.Approximately(_UnitValue, 0f))
		{
			return 0f;
		}
		return (float)this.GetLiquidQuantity / _UnitValue;
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000845 RID: 2117 RVA: 0x00051B75 File Offset: 0x0004FD75
	public bool RequiresEmptyLiquid
	{
		get
		{
			return this.RequiredLiquidContent.IsActive && this.RequiredLiquidContent.RequiredLiquid == null && this.RequiredLiquidContent.RequiredGroup == null;
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00051BAA File Offset: 0x0004FDAA
	public int GetCorrectMaxQuantity(InGameCardBase _ForCard)
	{
		if (!_ForCard.ContainedLiquidModel)
		{
			return this.GetQuantity;
		}
		if (this.GetLiquidQuantity <= 0)
		{
			return this.GetQuantity;
		}
		return this.GetLiquidQuantity;
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06000847 RID: 2119 RVA: 0x00051BD8 File Offset: 0x0004FDD8
	public CardData AnyCard
	{
		get
		{
			if (this.RequiredCard)
			{
				return this.RequiredCard;
			}
			if (this.RequiredTabGroup && this.RequiredTabGroup.IncludedCards != null && this.RequiredTabGroup.IncludedCards.Count > 0)
			{
				for (int i = 0; i < this.RequiredTabGroup.IncludedCards.Count; i++)
				{
					if (this.RequiredTabGroup.IncludedCards[i])
					{
						return this.RequiredTabGroup.IncludedCards[i];
					}
				}
			}
			return null;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x00051C6C File Offset: 0x0004FE6C
	public ContentPage GetHelpPage
	{
		get
		{
			if (this.RequiredLiquidContent.IsActive)
			{
				if (this.RequiredLiquidContent.RequiredLiquid)
				{
					ContentPage contentPage = GuideManager.GetPageFor(this.RequiredLiquidContent.RequiredLiquid);
					if (contentPage)
					{
						return contentPage;
					}
				}
				if (this.RequiredLiquidContent.RequiredGroup)
				{
					ContentPage contentPage = this.RequiredLiquidContent.RequiredGroup.GetHelpPage;
					if (contentPage)
					{
						return contentPage;
					}
				}
			}
			if (this.RequiredCard)
			{
				ContentPage contentPage = GuideManager.GetPageFor(this.RequiredCard);
				if (contentPage)
				{
					return contentPage;
				}
			}
			if (this.RequiredTabGroup)
			{
				return this.RequiredTabGroup.GetHelpPage;
			}
			return null;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06000849 RID: 2121 RVA: 0x00051D20 File Offset: 0x0004FF20
	public CardData AnyLiquid
	{
		get
		{
			if (this.RequiredCard && this.RequiredCard.CardType == CardTypes.Liquid)
			{
				return this.RequiredCard;
			}
			if (this.RequiredTabGroup && this.RequiredTabGroup.IncludedCards != null && this.RequiredTabGroup.IncludedCards.Count > 0)
			{
				for (int i = 0; i < this.RequiredTabGroup.IncludedCards.Count; i++)
				{
					if (this.RequiredTabGroup.IncludedCards[i] && this.RequiredTabGroup.IncludedCards[i].CardType == CardTypes.Liquid)
					{
						return this.RequiredTabGroup.IncludedCards[i];
					}
				}
			}
			return null;
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x0600084A RID: 2122 RVA: 0x00051DE0 File Offset: 0x0004FFE0
	public List<CardData> AllCards
	{
		get
		{
			List<CardData> list = new List<CardData>();
			if (this.RequiredCard)
			{
				list.Add(this.RequiredCard);
			}
			if (this.RequiredTabGroup && this.RequiredTabGroup.IncludedCards != null && this.RequiredTabGroup.IncludedCards.Count > 0)
			{
				for (int i = 0; i < this.RequiredTabGroup.IncludedCards.Count; i++)
				{
					if (this.RequiredTabGroup.IncludedCards[i] && !list.Contains(this.RequiredTabGroup.IncludedCards[i]))
					{
						list.Add(this.RequiredTabGroup.IncludedCards[i]);
					}
				}
			}
			return list;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x0600084B RID: 2123 RVA: 0x00051EA0 File Offset: 0x000500A0
	public bool ValidRequirements
	{
		get
		{
			if (this.RequiredCard)
			{
				return true;
			}
			if (this.RequiredTabGroup && this.RequiredTabGroup.IncludedCards != null && this.RequiredTabGroup.IncludedCards.Count > 0)
			{
				for (int i = 0; i < this.RequiredTabGroup.IncludedCards.Count; i++)
				{
					if (this.RequiredTabGroup.IncludedCards[i])
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00051F20 File Offset: 0x00050120
	public bool CompatibleInGameCard(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (!this.CompatibleCard(_Card.CardModel, _Card.ContainedLiquidModel))
		{
			return _Card.ContainedLiquid && this.CompatibleInGameCard(_Card.ContainedLiquid);
		}
		if (_Card.CardModel.SpoilageTime)
		{
			DurabilityRequirements durabilityRequirements = this.Spoilage;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentSpoilagePercent + 0.0001f < 1f)
			{
				return false;
			}
		}
		if (_Card.CardModel.UsageDurability)
		{
			DurabilityRequirements durabilityRequirements = this.Usage;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentUsagePercent + 0.0001f < 1f)
			{
				return false;
			}
		}
		if (_Card.CardModel.FuelCapacity)
		{
			DurabilityRequirements durabilityRequirements = this.Fuel;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentFuelPercent + 0.0001f < 1f)
			{
				return false;
			}
		}
		if (_Card.CardModel.Progress)
		{
			DurabilityRequirements durabilityRequirements = this.Progress;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentProgressPercent + 0.0001f < 1f)
			{
				return false;
			}
		}
		if (_Card.CardModel.SpecialDurability1)
		{
			DurabilityRequirements durabilityRequirements = this.Special1;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentSpecial1Percent + 0.0001f < 1f)
			{
				return false;
			}
		}
		if (_Card.CardModel.SpecialDurability2)
		{
			DurabilityRequirements durabilityRequirements = this.Special2;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentSpecial2Percent + 0.0001f < 1f)
			{
				return false;
			}
		}
		if (_Card.CardModel.SpecialDurability3)
		{
			DurabilityRequirements durabilityRequirements = this.Special3;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentSpecial3Percent + 0.0001f < 1f)
			{
				return false;
			}
		}
		if (_Card.CardModel.SpecialDurability4)
		{
			DurabilityRequirements durabilityRequirements = this.Special4;
			if (durabilityRequirements == DurabilityRequirements.OnlyFull && _Card.CurrentSpecial4Percent + 0.0001f < 1f)
			{
				return false;
			}
		}
		return this.RequiredLiquidContent.IsValid(_Card, true) || !_Card.CardModel.CanContainLiquid;
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00052116 File Offset: 0x00050316
	public bool CorrectLiquidQuantity(InGameCardBase _Card)
	{
		return this.RequiredLiquidContent.HasCorrectLiquidQuantity(_Card);
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00052124 File Offset: 0x00050324
	public bool CompatibleCard(CardData _Card, CardData _WithLiquid)
	{
		if (!_Card)
		{
			return false;
		}
		if (_Card == this.RequiredCard && this.RequiredLiquidContent.IsValid(_WithLiquid))
		{
			return true;
		}
		if (this.RequiredTabGroup && this.RequiredTabGroup.IncludedCards != null && this.RequiredTabGroup.IncludedCards.Count > 0)
		{
			int i = 0;
			while (i < this.RequiredTabGroup.IncludedCards.Count)
			{
				if (_Card == this.RequiredTabGroup.IncludedCards[i])
				{
					if (!_Card.CanContainLiquid)
					{
						return true;
					}
					if (this.RequiredLiquidContent.IsActive)
					{
						return this.RequiredLiquidContent.IsValid(_WithLiquid);
					}
					return _WithLiquid == null;
				}
				else
				{
					i++;
				}
			}
		}
		return false;
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x0600084F RID: 2127 RVA: 0x000521EC File Offset: 0x000503EC
	public string GetName
	{
		get
		{
			if (this.RequiredTabGroup && this.RequiredTabGroup.IsMixedLiquidGroup && !string.IsNullOrEmpty(this.RequiredTabGroup.TabName))
			{
				return this.RequiredTabGroup.TabName;
			}
			if (!this.RequiredLiquidContent.IsEmpty && this.RequiredLiquidContent.IsActive)
			{
				return this.GetLiquidContentName;
			}
			if (this.RequiredTabGroup && !string.IsNullOrEmpty(this.RequiredTabGroup.TabName))
			{
				return this.RequiredTabGroup.TabName;
			}
			if (this.RequiredCard)
			{
				return this.RequiredCard.CardName;
			}
			if (this.RequiredTabGroup && this.RequiredTabGroup.IncludedCards != null && this.RequiredTabGroup.IncludedCards.Count > 0)
			{
				for (int i = 0; i < this.RequiredTabGroup.IncludedCards.Count; i++)
				{
					if (this.RequiredTabGroup.IncludedCards[i])
					{
						return this.RequiredTabGroup.IncludedCards[i].CardName;
					}
				}
			}
			return "";
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000850 RID: 2128 RVA: 0x00052330 File Offset: 0x00050530
	private string GetLiquidContentName
	{
		get
		{
			if (this.RequiredLiquidContent.RequiredGroup && !string.IsNullOrEmpty(this.RequiredLiquidContent.RequiredGroup.TabName))
			{
				return this.RequiredLiquidContent.RequiredGroup.TabName;
			}
			if (this.RequiredLiquidContent.RequiredLiquid)
			{
				return this.RequiredLiquidContent.RequiredLiquid.CardName;
			}
			if (this.RequiredLiquidContent.RequiredGroup && this.RequiredLiquidContent.RequiredGroup.IncludedCards != null && this.RequiredLiquidContent.RequiredGroup.IncludedCards.Count > 0)
			{
				for (int i = 0; i < this.RequiredLiquidContent.RequiredGroup.IncludedCards.Count; i++)
				{
					if (this.RequiredLiquidContent.RequiredGroup.IncludedCards[i])
					{
						return this.RequiredLiquidContent.RequiredGroup.IncludedCards[i].CardName;
					}
				}
			}
			return "";
		}
	}

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x0005244C File Offset: 0x0005064C
	public List<Sprite> GetImages
	{
		get
		{
			List<Sprite> list = new List<Sprite>();
			if (this.RequiredCard)
			{
				if (this.RequiredLiquidContent.IsActive)
				{
					if (!this.AddImagesFromLiquidContent(this.RequiredCard, ref list))
					{
						list.Add(this.RequiredCard.CardImage);
					}
				}
				else
				{
					list.Add(this.RequiredCard.CardImage);
				}
			}
			if (this.RequiredTabGroup && this.RequiredTabGroup.IncludedCards != null && this.RequiredTabGroup.IncludedCards.Count > 0)
			{
				for (int i = 0; i < this.RequiredTabGroup.IncludedCards.Count; i++)
				{
					if (this.RequiredTabGroup.IncludedCards[i])
					{
						if (this.RequiredLiquidContent.IsActive)
						{
							if (!this.AddImagesFromLiquidContent(this.RequiredTabGroup.IncludedCards[i], ref list))
							{
								list.Add(this.RequiredTabGroup.IncludedCards[i].CardImage);
							}
						}
						else
						{
							list.Add(this.RequiredTabGroup.IncludedCards[i].CardImage);
						}
					}
				}
			}
			return list;
		}
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0005257C File Offset: 0x0005077C
	private bool AddImagesFromLiquidContent(CardData _FromCard, ref List<Sprite> _Result)
	{
		bool result = false;
		Sprite imageForLiquid = _FromCard.GetImageForLiquid(this.RequiredLiquidContent.RequiredLiquid);
		if (imageForLiquid)
		{
			_Result.Add(imageForLiquid);
			result = true;
		}
		if (this.RequiredLiquidContent.RequiredGroup)
		{
			for (int i = 0; i < this.RequiredLiquidContent.RequiredGroup.IncludedCards.Count; i++)
			{
				imageForLiquid = _FromCard.GetImageForLiquid(this.RequiredLiquidContent.RequiredGroup.IncludedCards[i]);
				if (imageForLiquid)
				{
					_Result.Add(imageForLiquid);
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x04000C98 RID: 3224
	[SerializeField]
	private CardData RequiredCard;

	// Token: 0x04000C99 RID: 3225
	[SerializeField]
	private CardTabGroup RequiredTabGroup;

	// Token: 0x04000C9A RID: 3226
	[SerializeField]
	private int RequiredQuantity;

	// Token: 0x04000C9B RID: 3227
	[SerializeField]
	private DurabilityRequirements Spoilage;

	// Token: 0x04000C9C RID: 3228
	[SerializeField]
	private DurabilityRequirements Usage;

	// Token: 0x04000C9D RID: 3229
	[SerializeField]
	private DurabilityRequirements Fuel;

	// Token: 0x04000C9E RID: 3230
	[SerializeField]
	private DurabilityRequirements Progress;

	// Token: 0x04000C9F RID: 3231
	[SerializeField]
	private DurabilityRequirements Special1;

	// Token: 0x04000CA0 RID: 3232
	[SerializeField]
	private DurabilityRequirements Special2;

	// Token: 0x04000CA1 RID: 3233
	[SerializeField]
	private DurabilityRequirements Special3;

	// Token: 0x04000CA2 RID: 3234
	[SerializeField]
	private DurabilityRequirements Special4;

	// Token: 0x04000CA3 RID: 3235
	[SerializeField]
	private bool DontSpend;

	// Token: 0x04000CA4 RID: 3236
	[SerializeField]
	private LiquidContentCondition RequiredLiquidContent;

	// Token: 0x04000CA5 RID: 3237
	public CardStateChange EffectOnIngredient;

	// Token: 0x04000CA6 RID: 3238
	public bool LiquidBecomesResult;
}
