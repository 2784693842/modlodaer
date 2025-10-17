using System;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class PercentAttribute : PropertyAttribute
{
	// Token: 0x06000B8B RID: 2955 RVA: 0x000618B3 File Offset: 0x0005FAB3
	public PercentAttribute()
	{
		this.Clamp = true;
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x000618C2 File Offset: 0x0005FAC2
	public PercentAttribute(bool _Clamp)
	{
		this.Clamp = _Clamp;
	}

	// Token: 0x04001076 RID: 4214
	public bool Clamp;
}
