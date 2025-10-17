using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
[Serializable]
public class OptionalColorValue : OptionalValue
{
	// Token: 0x06000C5F RID: 3167 RVA: 0x00065B35 File Offset: 0x00063D35
	public OptionalColorValue(bool _Active, Color _Color) : base(_Active)
	{
		this.ColorValue = _Color;
	}

	// Token: 0x0400113E RID: 4414
	public Color ColorValue;
}
