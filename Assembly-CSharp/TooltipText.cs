using System;

// Token: 0x020000AC RID: 172
public class TooltipText
{
	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00047AF4 File Offset: 0x00045CF4
	public bool ShowHoldBar
	{
		get
		{
			return this.NormalizedHoldTime > 0f && this.NormalizedHoldTime < 1f;
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00047B14 File Offset: 0x00045D14
	public override string ToString()
	{
		string text = "Title: ";
		if (!string.IsNullOrEmpty(this.TooltipTitle))
		{
			text += this.TooltipTitle;
		}
		text += " | Content: ";
		if (!string.IsNullOrEmpty(this.TooltipContent))
		{
			text += this.TooltipContent;
		}
		return text;
	}

	// Token: 0x040009C1 RID: 2497
	public string TooltipTitle;

	// Token: 0x040009C2 RID: 2498
	public string TooltipContent;

	// Token: 0x040009C3 RID: 2499
	public int Priority;

	// Token: 0x040009C4 RID: 2500
	public float NormalizedHoldTime;

	// Token: 0x040009C5 RID: 2501
	public string HoldText;
}
