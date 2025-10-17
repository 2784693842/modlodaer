using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
[Serializable]
public struct DifficultyRatingLabel
{
	// Token: 0x04000EC0 RID: 3776
	[MinMax]
	public Vector2Int Range;

	// Token: 0x04000EC1 RID: 3777
	public LocalizedString Label;
}
