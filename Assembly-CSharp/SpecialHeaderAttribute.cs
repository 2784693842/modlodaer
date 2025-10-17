using System;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class SpecialHeaderAttribute : PropertyAttribute
{
	// Token: 0x06000A89 RID: 2697 RVA: 0x0005DF00 File Offset: 0x0005C100
	public SpecialHeaderAttribute(string _Text, HeaderSizes _Size, HeaderStyles _Style, float _ExtraSpace = 0f)
	{
		this.HeaderText = _Text;
		this.Style = _Style;
		this.TextSize = _Size;
		this.ExtraSpacing = _ExtraSpace;
	}

	// Token: 0x0400103A RID: 4154
	public string HeaderText;

	// Token: 0x0400103B RID: 4155
	public HeaderStyles Style;

	// Token: 0x0400103C RID: 4156
	public HeaderSizes TextSize;

	// Token: 0x0400103D RID: 4157
	public float ExtraSpacing;
}
