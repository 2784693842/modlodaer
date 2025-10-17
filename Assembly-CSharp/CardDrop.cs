using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
[Serializable]
public struct CardDrop
{
	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06000814 RID: 2068 RVA: 0x0004FD35 File Offset: 0x0004DF35
	public bool IsEmpty
	{
		get
		{
			return !this.DroppedCard || this.Quantity == Vector2Int.zero;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06000815 RID: 2069 RVA: 0x0004FD56 File Offset: 0x0004DF56
	public bool CanDrop
	{
		get
		{
			return !this.DroppedCard || this.DroppedCard.CanSpawnOnBoard();
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x0004FD72 File Offset: 0x0004DF72
	public CardDrop(CardData _Card, Vector2Int _Quantity)
	{
		this.DroppedCard = _Card;
		this.Quantity = _Quantity;
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x0004FD82 File Offset: 0x0004DF82
	public CardDrop(CardData _Card)
	{
		this.DroppedCard = _Card;
		this.Quantity = Vector2Int.one;
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06000818 RID: 2072 RVA: 0x0004FD96 File Offset: 0x0004DF96
	public float TotalDropWeight
	{
		get
		{
			if (!this.DroppedCard)
			{
				return 0f;
			}
			return this.DroppedCard.ObjectWeight * (float)this.Quantity.y;
		}
	}

	// Token: 0x04000C2C RID: 3116
	public CardData DroppedCard;

	// Token: 0x04000C2D RID: 3117
	[MinMax]
	public Vector2Int Quantity;
}
