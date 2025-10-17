using System;

// Token: 0x020001D7 RID: 471
public struct StackActionStopConditions
{
	// Token: 0x06000CA4 RID: 3236 RVA: 0x00067618 File Offset: 0x00065818
	public bool ShouldStopStackAction(InGameCardBase _ReceivingCard, InGameCardBase _GivenCard)
	{
		string text;
		return !_ReceivingCard || !_GivenCard || (_ReceivingCard.Destroyed || _GivenCard.Destroyed) || (this.SpoilageIsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentSpoilagePercent, 1f)) || (this.SpoilageIsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentSpoilage, 0f)) || (this.UsageIsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentUsagePercent, 1f)) || (this.UsageIsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentUsageDurability, 0f)) || (this.FuelIsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentFuelPercent, 1f)) || (this.FuelIsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentFuel, 0f)) || (this.ProgressIsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentProgressPercent, 1f)) || (this.ProgressIsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentProgress, 0f)) || (this.Special1IsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentSpecial1Percent, 1f)) || (this.Special1IsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentSpecial1, 0f)) || (this.Special2IsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentSpecial2Percent, 1f)) || (this.Special2IsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentSpecial2, 0f)) || (this.Special3IsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentSpecial3Percent, 1f)) || (this.Special3IsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentSpecial3, 0f)) || (this.Special4IsFull && ExtraMath.FloatIsGreaterOrEqual(_ReceivingCard.CurrentSpecial4Percent, 1f)) || (this.Special4IsEmpty && ExtraMath.FloatIsLowerOrEqual(_ReceivingCard.CurrentSpecial4, 0f)) || (this.LocationWeightIsFull && !_ReceivingCard.CanTransferToTravelPlace(_GivenCard, out text));
	}

	// Token: 0x04001186 RID: 4486
	public bool SpoilageIsFull;

	// Token: 0x04001187 RID: 4487
	public bool SpoilageIsEmpty;

	// Token: 0x04001188 RID: 4488
	public bool UsageIsFull;

	// Token: 0x04001189 RID: 4489
	public bool UsageIsEmpty;

	// Token: 0x0400118A RID: 4490
	public bool FuelIsFull;

	// Token: 0x0400118B RID: 4491
	public bool FuelIsEmpty;

	// Token: 0x0400118C RID: 4492
	public bool ProgressIsFull;

	// Token: 0x0400118D RID: 4493
	public bool ProgressIsEmpty;

	// Token: 0x0400118E RID: 4494
	public bool Special1IsFull;

	// Token: 0x0400118F RID: 4495
	public bool Special1IsEmpty;

	// Token: 0x04001190 RID: 4496
	public bool Special2IsFull;

	// Token: 0x04001191 RID: 4497
	public bool Special2IsEmpty;

	// Token: 0x04001192 RID: 4498
	public bool Special3IsFull;

	// Token: 0x04001193 RID: 4499
	public bool Special3IsEmpty;

	// Token: 0x04001194 RID: 4500
	public bool Special4IsFull;

	// Token: 0x04001195 RID: 4501
	public bool Special4IsEmpty;

	// Token: 0x04001196 RID: 4502
	public bool LocationWeightIsFull;
}
