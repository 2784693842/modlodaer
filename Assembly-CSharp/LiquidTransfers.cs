using System;

// Token: 0x02000147 RID: 327
[Serializable]
public class LiquidTransfers
{
	// Token: 0x0600095A RID: 2394 RVA: 0x00057B30 File Offset: 0x00055D30
	public bool AppliesTo(CardData _Given, CardData _Receiving)
	{
		return _Given && _Receiving && (_Given == this.GivenLiquidCard || _Given.HasTag(this.ReceivingLiquidTag)) && (_Receiving == this.ReceivingLiquidCard || _Receiving.HasTag(this.ReceivingLiquidTag));
	}

	// Token: 0x04000ED0 RID: 3792
	public CardData ReceivingLiquidCard;

	// Token: 0x04000ED1 RID: 3793
	public CardTag ReceivingLiquidTag;

	// Token: 0x04000ED2 RID: 3794
	public CardData GivenLiquidCard;

	// Token: 0x04000ED3 RID: 3795
	public CardTag GivenLiquidTag;

	// Token: 0x04000ED4 RID: 3796
	public LiquidTransferInteraction Interaction;
}
