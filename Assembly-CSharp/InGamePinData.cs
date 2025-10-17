using System;

// Token: 0x02000020 RID: 32
public class InGamePinData
{
	// Token: 0x0600020B RID: 523 RVA: 0x0001501C File Offset: 0x0001321C
	public InGamePinData(InGameCardBase _Card, CardData _WithLiquid)
	{
		this.CorrespondingCard = _Card;
		this.PinnedCard = _Card.CardModel;
		this.PinnedLiquid = _WithLiquid;
		this.IsPinned = true;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00015048 File Offset: 0x00013248
	public InGamePinData(PinSaveData _SaveData)
	{
		UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(_SaveData.CardID);
		if (fromID && fromID is CardData)
		{
			this.PinnedCard = (fromID as CardData);
		}
		fromID = UniqueIDScriptable.GetFromID(_SaveData.LiquidCardID);
		if (fromID && fromID is CardData)
		{
			this.PinnedLiquid = (fromID as CardData);
		}
		this.IsPinned = _SaveData.IsPinned;
	}

	// Token: 0x04000201 RID: 513
	public CardData PinnedCard;

	// Token: 0x04000202 RID: 514
	public CardData PinnedLiquid;

	// Token: 0x04000203 RID: 515
	public InGameCardBase CorrespondingCard;

	// Token: 0x04000204 RID: 516
	public bool IsPinned;
}
