using System;
using UnityEngine;

// Token: 0x020000DF RID: 223
[Serializable]
public struct StatInterruptionCondition
{
	// Token: 0x060007D4 RID: 2004 RVA: 0x0004D700 File Offset: 0x0004B900
	public bool IsInRange(float _Value)
	{
		return ExtraMath.FloatIsInRange(_Value, this.TriggerRange, RoundingMethods.None);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0004D70F File Offset: 0x0004B90F
	public bool IsIdentical(StatValueTrigger _To)
	{
		return !(_To.Stat != this.Stat) && !(_To.TriggerRange != this.TriggerRange);
	}

	// Token: 0x04000B95 RID: 2965
	public GameStat Stat;

	// Token: 0x04000B96 RID: 2966
	public Vector2 TriggerRange;

	// Token: 0x04000B97 RID: 2967
	public LocalizedString Notification;
}
