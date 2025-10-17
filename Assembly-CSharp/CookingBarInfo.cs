using System;

// Token: 0x0200004D RID: 77
public class CookingBarInfo
{
	// Token: 0x06000319 RID: 793 RVA: 0x0001F52A File Offset: 0x0001D72A
	public CookingBarInfo(float _Value, int _RemainingTicks, bool _Paused, string _CustomCookingText, bool _HideProgress, bool _FillsLiquid)
	{
		this.Value = _Value;
		this.RemainingTicks = _RemainingTicks;
		this.Paused = _Paused;
		this.CustomCookingText = _CustomCookingText;
		this.HideProgress = _HideProgress;
		this.FillsLiquid = _FillsLiquid;
	}

	// Token: 0x040003BC RID: 956
	public float Value;

	// Token: 0x040003BD RID: 957
	public int RemainingTicks;

	// Token: 0x040003BE RID: 958
	public bool Paused;

	// Token: 0x040003BF RID: 959
	public string CustomCookingText;

	// Token: 0x040003C0 RID: 960
	public bool HideProgress;

	// Token: 0x040003C1 RID: 961
	public bool FillsLiquid;
}
