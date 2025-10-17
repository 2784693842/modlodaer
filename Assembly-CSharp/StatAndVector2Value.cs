using System;
using UnityEngine;

// Token: 0x020001D2 RID: 466
public struct StatAndVector2Value
{
	// Token: 0x06000C80 RID: 3200 RVA: 0x00066ACD File Offset: 0x00064CCD
	public StatAndVector2Value(GameStat _Stat, Vector2 _Value)
	{
		this.Stat = _Stat;
		this.Value = _Value;
	}

	// Token: 0x04001164 RID: 4452
	public GameStat Stat;

	// Token: 0x04001165 RID: 4453
	public Vector2 Value;
}
