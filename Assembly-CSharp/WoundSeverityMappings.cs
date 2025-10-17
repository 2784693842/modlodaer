using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
[Serializable]
public struct WoundSeverityMappings
{
	// Token: 0x040005A8 RID: 1448
	[SerializeField]
	[MinMax]
	public Vector2 AttackDefenseRatio;

	// Token: 0x040005A9 RID: 1449
	[SerializeField]
	public WoundSeverity WoundSeverity;
}
