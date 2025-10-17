using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
[Serializable]
public struct LiquidContentCondition
{
	// Token: 0x17000177 RID: 375
	// (get) Token: 0x060007CC RID: 1996 RVA: 0x0004D3EC File Offset: 0x0004B5EC
	public bool IsEmpty
	{
		get
		{
			return !this.RequiredLiquid && !this.RequiredGroup;
		}
	}

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060007CD RID: 1997 RVA: 0x0004D40B File Offset: 0x0004B60B
	public bool AnyQuantity
	{
		get
		{
			return !this.IsEmpty && Mathf.Approximately(this.RequiredQuantity.sqrMagnitude, 0f);
		}
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0004D42C File Offset: 0x0004B62C
	public bool IsValid(InGameCardBase _ForCard, bool _InactiveMeansEmpty)
	{
		if (!_ForCard)
		{
			return !this.IsActive;
		}
		if (!this.IsActive && !_InactiveMeansEmpty)
		{
			return true;
		}
		if (this.IsEmpty || !this.IsActive)
		{
			return !_ForCard.IsLiquidContainer || !_ForCard.ContainedLiquid;
		}
		if (_ForCard.IsLiquid)
		{
			if (this.RequiredLiquid)
			{
				return this.RequiredLiquid == _ForCard.CardModel;
			}
			if (this.RequiredGroup)
			{
				return this.RequiredGroup.IncludedCards.Contains(_ForCard.CardModel);
			}
		}
		if (!_ForCard.IsLiquidContainer)
		{
			return false;
		}
		if (!_ForCard.ContainedLiquid)
		{
			return false;
		}
		if (this.RequiredLiquid)
		{
			if (_ForCard.ContainedLiquidModel != this.RequiredLiquid)
			{
				return false;
			}
		}
		else if (this.RequiredGroup && !this.RequiredGroup.IncludedCards.Contains(_ForCard.ContainedLiquidModel))
		{
			return false;
		}
		return this.HasCorrectLiquidQuantity(_ForCard);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0004D538 File Offset: 0x0004B738
	public bool HasCorrectLiquidQuantity(InGameCardBase _ForCard)
	{
		if (_ForCard.IsLiquid)
		{
			if (_ForCard.CardModel != this.RequiredLiquid)
			{
				return false;
			}
			if (this.RequiredQuantity.x <= this.RequiredQuantity.y)
			{
				if (!ExtraMath.FloatIsInRange(_ForCard.ContainedLiquid.CurrentLiquidQuantity, this.RequiredQuantity, RoundingMethods.None) && !this.AnyQuantity)
				{
					return false;
				}
			}
			else if (_ForCard.ContainedLiquid.CurrentLiquidQuantity < this.RequiredQuantity.x)
			{
				return false;
			}
			return true;
		}
		else
		{
			if (!_ForCard.IsLiquidContainer)
			{
				return false;
			}
			if (!_ForCard.ContainedLiquid)
			{
				return false;
			}
			if (this.RequiredQuantity.x <= this.RequiredQuantity.y)
			{
				if (!ExtraMath.FloatIsInRange(_ForCard.ContainedLiquid.CurrentLiquidQuantity, this.RequiredQuantity, RoundingMethods.None) && !this.AnyQuantity)
				{
					return false;
				}
			}
			else if (_ForCard.ContainedLiquid.CurrentLiquidQuantity < this.RequiredQuantity.x)
			{
				return false;
			}
			return true;
		}
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0004D628 File Offset: 0x0004B828
	public bool IsValid(CardData _ForLiquid)
	{
		if (this.IsEmpty || !this.IsActive)
		{
			return !_ForLiquid;
		}
		if (this.RequiredLiquid)
		{
			return _ForLiquid == this.RequiredLiquid;
		}
		return !this.RequiredGroup || this.RequiredGroup.IncludedCards.Contains(_ForLiquid);
	}

	// Token: 0x04000B8D RID: 2957
	public bool IsActive;

	// Token: 0x04000B8E RID: 2958
	public CardData RequiredLiquid;

	// Token: 0x04000B8F RID: 2959
	public CardTabGroup RequiredGroup;

	// Token: 0x04000B90 RID: 2960
	[MinMax(DisplayOption = MinMaxDisplay.MaxCanBeInfinite)]
	public Vector2 RequiredQuantity;
}
