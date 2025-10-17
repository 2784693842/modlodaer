using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
[Serializable]
public class AddedDurabilityModifier
{
	// Token: 0x1700017A RID: 378
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x0004DEE0 File Offset: 0x0004C0E0
	public bool IsEmpty
	{
		get
		{
			return this.SpoilageChange == Vector2.zero && this.UsageChange == Vector2.zero && this.FuelChange == Vector2.zero && this.ChargesChange == Vector2.zero && this.Special1Change == Vector2.zero && this.Special2Change == Vector2.zero && this.Special3Change == Vector2.zero && this.Special4Change == Vector2.zero;
		}
	}

	// Token: 0x04000BD8 RID: 3032
	[MinMax]
	public Vector2 SpoilageChange;

	// Token: 0x04000BD9 RID: 3033
	[MinMax]
	public Vector2 UsageChange;

	// Token: 0x04000BDA RID: 3034
	[MinMax]
	public Vector2 FuelChange;

	// Token: 0x04000BDB RID: 3035
	[MinMax]
	public Vector2 ChargesChange;

	// Token: 0x04000BDC RID: 3036
	[MinMax]
	public Vector2 Special1Change;

	// Token: 0x04000BDD RID: 3037
	[MinMax]
	public Vector2 Special2Change;

	// Token: 0x04000BDE RID: 3038
	[MinMax]
	public Vector2 Special3Change;

	// Token: 0x04000BDF RID: 3039
	[MinMax]
	public Vector2 Special4Change;
}
