using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
[Serializable]
public struct EnemyValue
{
	// Token: 0x04000DCD RID: 3533
	public string Name;

	// Token: 0x04000DCE RID: 3534
	public Vector2 StartingValue;

	// Token: 0x04000DCF RID: 3535
	public float MaxValue;

	// Token: 0x04000DD0 RID: 3536
	public EncounterResult OnZeroEncounterResult;
}
