using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
[Serializable]
public struct BodyLocationModifiers
{
	// Token: 0x04000A16 RID: 2582
	[MinMax]
	public Vector2Int HeadProbModifier;

	// Token: 0x04000A17 RID: 2583
	[MinMax]
	public Vector2 HeadDefenseModifier;

	// Token: 0x04000A18 RID: 2584
	[MinMax]
	[Space]
	public Vector2Int TorsoProbModifier;

	// Token: 0x04000A19 RID: 2585
	[MinMax]
	public Vector2 TorsoDefenseModifier;

	// Token: 0x04000A1A RID: 2586
	[MinMax]
	[Space]
	public Vector2Int LArmProbModifier;

	// Token: 0x04000A1B RID: 2587
	[MinMax]
	public Vector2 LArmDefenseModifier;

	// Token: 0x04000A1C RID: 2588
	[MinMax]
	[Space]
	public Vector2Int RArmProbModifier;

	// Token: 0x04000A1D RID: 2589
	[MinMax]
	public Vector2 RArmDefenseModifier;

	// Token: 0x04000A1E RID: 2590
	[MinMax]
	[Space]
	public Vector2Int LLegProbModifier;

	// Token: 0x04000A1F RID: 2591
	[MinMax]
	public Vector2 LLegDefenseModifier;

	// Token: 0x04000A20 RID: 2592
	[MinMax]
	[Space]
	public Vector2Int RLegProbModifier;

	// Token: 0x04000A21 RID: 2593
	[MinMax]
	public Vector2 RLegDefenseModifier;
}
