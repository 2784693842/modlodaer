using System;

// Token: 0x02000017 RID: 23
[Serializable]
public class InventorySlotSaveData
{
	// Token: 0x060001EF RID: 495 RVA: 0x0001495E File Offset: 0x00012B5E
	public InventorySlotSaveData(CardSaveData _Card, int _Amt)
	{
		this.RegularCard = _Card;
		this.InventoryCardIndex = -1;
		this.CardAmt = _Amt;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00014982 File Offset: 0x00012B82
	public InventorySlotSaveData(int _Index)
	{
		this.RegularCard = null;
		this.InventoryCardIndex = _Index;
		this.CardAmt = 1;
	}

	// Token: 0x040001E8 RID: 488
	public CardSaveData RegularCard;

	// Token: 0x040001E9 RID: 489
	public int InventoryCardIndex = -1;

	// Token: 0x040001EA RID: 490
	public int CardAmt;
}
