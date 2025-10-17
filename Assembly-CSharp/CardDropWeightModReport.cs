using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public struct CardDropWeightModReport
{
	// Token: 0x06000C81 RID: 3201 RVA: 0x00066AE0 File Offset: 0x00064CE0
	public override string ToString()
	{
		if (this.Card)
		{
			return this.Card.name + " adding " + this.BonusWeight.ToString() + " weight";
		}
		if (Mathf.Approximately(this.BonusWeight, 0f))
		{
			return "Invalid report (null card)";
		}
		return "Unknown card (possibly environment) adding " + this.BonusWeight.ToString() + " weight";
	}

	// Token: 0x04001166 RID: 4454
	public InGameCardBase Card;

	// Token: 0x04001167 RID: 4455
	public float BonusWeight;
}
