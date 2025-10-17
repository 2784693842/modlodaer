using System;

// Token: 0x0200008D RID: 141
public class LocalizedStaticTooltip : TooltipProvider
{
	// Token: 0x060005EC RID: 1516 RVA: 0x0003E2D8 File Offset: 0x0003C4D8
	private void OnEnable()
	{
		base.SetTooltip(this.TooltipTitle, this.TooltipContent, "", 0);
	}

	// Token: 0x040007E8 RID: 2024
	public LocalizedString TooltipTitle;

	// Token: 0x040007E9 RID: 2025
	public LocalizedString TooltipContent;
}
