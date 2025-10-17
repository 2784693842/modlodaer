using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
[Serializable]
public struct Chance
{
	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00065B64 File Offset: 0x00063D64
	public bool Succeeded
	{
		get
		{
			return !this.UseChance || UnityEngine.Random.value < this.ChanceValue || this.ChanceValue >= 1f;
		}
	}

	// Token: 0x04001142 RID: 4418
	public bool UseChance;

	// Token: 0x04001143 RID: 4419
	[Clamped(0f, 1f, false)]
	public float ChanceValue;
}
