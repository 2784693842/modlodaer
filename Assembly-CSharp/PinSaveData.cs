using System;

// Token: 0x02000021 RID: 33
[Serializable]
public class PinSaveData
{
	// Token: 0x0600020D RID: 525 RVA: 0x000150B7 File Offset: 0x000132B7
	public virtual void SavePin(InGamePinData _Pin)
	{
		this.CardID = UniqueIDScriptable.SaveID(_Pin.PinnedCard);
		this.LiquidCardID = UniqueIDScriptable.SaveID(_Pin.PinnedLiquid);
		this.IsPinned = _Pin.IsPinned;
	}

	// Token: 0x04000205 RID: 517
	public string CardID;

	// Token: 0x04000206 RID: 518
	public string LiquidCardID;

	// Token: 0x04000207 RID: 519
	public bool IsPinned;
}
