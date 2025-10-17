using System;
using UnityEngine;

// Token: 0x020001E1 RID: 481
[Serializable]
public struct StatEncounterResult
{
	// Token: 0x06000CC2 RID: 3266 RVA: 0x0006850D File Offset: 0x0006670D
	public bool IsInRange(float _Value)
	{
		return ExtraMath.FloatIsInRange(_Value, this.TriggerRange, RoundingMethods.Floor);
	}

	// Token: 0x040011AE RID: 4526
	public GameStat Stat;

	// Token: 0x040011AF RID: 4527
	public Vector2 TriggerRange;

	// Token: 0x040011B0 RID: 4528
	public EncounterResult Result;
}
