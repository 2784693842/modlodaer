using System;
using System.Collections.Generic;

// Token: 0x020001DB RID: 475
public class CardPileComparer : IComparer<InGameCardBase>
{
	// Token: 0x06000CB7 RID: 3255 RVA: 0x000680D8 File Offset: 0x000662D8
	public CardPileComparer(bool _Invert = false)
	{
		this.Inverted = _Invert;
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x000680E8 File Offset: 0x000662E8
	public int Compare(InGameCardBase _A, InGameCardBase _B)
	{
		if (_A.CardModel != _B.CardModel)
		{
			return 0;
		}
		if (this.Inverted)
		{
			return this.CardScore(_A).CompareTo(this.CardScore(_B));
		}
		return this.CardScore(_B).CompareTo(this.CardScore(_A));
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x00068140 File Offset: 0x00066340
	private float CardScore(InGameCardBase _Card)
	{
		float num = 0f;
		num += _Card.CurrentUsageDurability;
		num += _Card.CurrentFuel;
		num += _Card.CurrentSpoilage;
		num += _Card.CurrentProgress;
		num += _Card.CurrentSpecial1;
		num += _Card.CurrentSpecial2;
		num += _Card.CurrentSpecial3;
		num += _Card.CurrentSpecial4;
		if (_Card.ContainedLiquid)
		{
			num += _Card.ContainedLiquid.CurrentLiquidQuantity;
		}
		num += (float)_Card.CardCreationIndex * 0.001f;
		if (_Card.IsPinned)
		{
			num = float.MaxValue;
		}
		return num;
	}

	// Token: 0x040011A2 RID: 4514
	private bool Inverted;
}
