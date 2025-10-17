using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class ClampedAttribute : PropertyAttribute
{
	// Token: 0x06000A0C RID: 2572 RVA: 0x0005A6BA File Offset: 0x000588BA
	public ClampedAttribute(float _Min, float _Max, bool _ShowRange = true)
	{
		this.Min = _Min;
		this.Max = _Max;
		this.ShowRange = _ShowRange;
	}

	// Token: 0x04000F87 RID: 3975
	public float Min;

	// Token: 0x04000F88 RID: 3976
	public float Max;

	// Token: 0x04000F89 RID: 3977
	public bool ShowRange;
}
