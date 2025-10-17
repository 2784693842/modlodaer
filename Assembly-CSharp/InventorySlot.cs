using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
[Serializable]
public class InventorySlot
{
	// Token: 0x060001D4 RID: 468 RVA: 0x000138F2 File Offset: 0x00011AF2
	public InventorySlot()
	{
		this.AllCards = new List<InGameCardBase>();
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060001D5 RID: 469 RVA: 0x00013905 File Offset: 0x00011B05
	// (set) Token: 0x060001D6 RID: 470 RVA: 0x0001390D File Offset: 0x00011B0D
	public List<InGameCardBase> AllCards { get; private set; }

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060001D7 RID: 471 RVA: 0x00013918 File Offset: 0x00011B18
	public float CurrentWeight
	{
		get
		{
			if (this.AllCards == null)
			{
				this.AllCards = new List<InGameCardBase>();
			}
			if (this.AllCards.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.AllCards.Count; i++)
			{
				if (this.AllCards[i])
				{
					num += this.AllCards[i].CurrentWeight;
				}
			}
			return num;
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0001398F File Offset: 0x00011B8F
	public void AddCard(InGameCardBase _Card)
	{
		if (!this.AllCards.Contains(_Card))
		{
			this.AllCards.Add(_Card);
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x000139AB File Offset: 0x00011BAB
	public void RemoveCard(InGameCardBase _Card)
	{
		if (this.AllCards.Contains(_Card))
		{
			this.AllCards.Remove(_Card);
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x000139C8 File Offset: 0x00011BC8
	public void AddCards(List<InGameCardBase> _Cards)
	{
		if (_Cards == null)
		{
			return;
		}
		for (int i = 0; i < _Cards.Count; i++)
		{
			this.AddCard(_Cards[i]);
		}
	}

	// Token: 0x060001DB RID: 475 RVA: 0x000139F8 File Offset: 0x00011BF8
	public void Clear()
	{
		for (int i = this.AllCards.Count - 1; i >= 0; i--)
		{
			this.AllCards.RemoveAt(i);
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00013A29 File Offset: 0x00011C29
	public bool HasCard(InGameCardBase _Card)
	{
		return this.AllCards.Contains(_Card);
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060001DD RID: 477 RVA: 0x00013A38 File Offset: 0x00011C38
	public InGameCardBase MainCard
	{
		get
		{
			if (this.IsFree)
			{
				return null;
			}
			for (int i = 0; i < this.AllCards.Count; i++)
			{
				if (this.AllCards[i] && !this.AllCards[i].Destroyed)
				{
					return this.AllCards[i];
				}
			}
			return null;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x060001DE RID: 478 RVA: 0x00013A9C File Offset: 0x00011C9C
	public CardData CardModel
	{
		get
		{
			InGameCardBase mainCard = this.MainCard;
			if (mainCard)
			{
				return mainCard.CardModel;
			}
			return null;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x060001DF RID: 479 RVA: 0x00013AC0 File Offset: 0x00011CC0
	public CardData LiquidModel
	{
		get
		{
			InGameCardBase mainCard = this.MainCard;
			if (mainCard)
			{
				return mainCard.ContainedLiquidModel;
			}
			return null;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x00013AE4 File Offset: 0x00011CE4
	public int CardAmt
	{
		get
		{
			if (this.AllCards == null)
			{
				return 0;
			}
			return this.AllCards.Count;
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00013AFC File Offset: 0x00011CFC
	public float LiquidAmt(float _UnitValue)
	{
		if (this.AllCards == null)
		{
			return 0f;
		}
		if (this.AllCards.Count == 0)
		{
			return 0f;
		}
		if (Mathf.Approximately(_UnitValue, 0f))
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < this.AllCards.Count; i++)
		{
			if (this.AllCards[i] && !this.AllCards[i].Destroyed && this.AllCards[i].ContainedLiquid && !this.AllCards[i].ContainedLiquid.Destroyed)
			{
				num += this.AllCards[i].ContainedLiquid.CurrentLiquidQuantity;
			}
		}
		return num / _UnitValue;
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x00013BD0 File Offset: 0x00011DD0
	public bool IsFree
	{
		get
		{
			if (this.AllCards == null)
			{
				return true;
			}
			if (this.AllCards.Count == 0)
			{
				return true;
			}
			for (int i = 0; i < this.AllCards.Count; i++)
			{
				if (!this.AllCards[i].Destroyed)
				{
					return false;
				}
			}
			return true;
		}
	}
}
