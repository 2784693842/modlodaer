using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
[Serializable]
public struct LiquidDrop
{
	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06000819 RID: 2073 RVA: 0x0004FDC3 File Offset: 0x0004DFC3
	public bool IsEmpty
	{
		get
		{
			return !this.LiquidCard || this.Quantity == Vector2.zero;
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x0004FDE4 File Offset: 0x0004DFE4
	public LiquidDrop(CardData _Card, Vector2 _Quantity, TransferedDurabilities _Durabilities)
	{
		this.LiquidCard = _Card;
		this.Quantity = _Quantity;
		this.LiquidDurabilities = _Durabilities;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0004FDFB File Offset: 0x0004DFFB
	public LiquidDrop(CardData _Card)
	{
		this.LiquidCard = _Card;
		this.Quantity = Vector2.one;
		this.LiquidDurabilities = null;
	}

	// Token: 0x04000C2E RID: 3118
	public CardData LiquidCard;

	// Token: 0x04000C2F RID: 3119
	[MinMax]
	public Vector2 Quantity;

	// Token: 0x04000C30 RID: 3120
	[NonSerialized]
	public TransferedDurabilities LiquidDurabilities;
}
