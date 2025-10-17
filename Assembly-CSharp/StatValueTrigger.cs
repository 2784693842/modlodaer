using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
[Serializable]
public struct StatValueTrigger
{
	// Token: 0x060007D1 RID: 2001 RVA: 0x0004D689 File Offset: 0x0004B889
	public bool IsInRange(float _Value)
	{
		return ExtraMath.FloatIsInRange(_Value, this.TriggerRange, RoundingMethods.Floor);
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0004D698 File Offset: 0x0004B898
	public bool IsTooMuch(float _Value, bool _Exact = false)
	{
		if (_Exact)
		{
			return this.TriggerRange.x != this.TriggerRange.y && _Value > this.TriggerRange.y;
		}
		return ExtraMath.FloatIsBeyondRange(_Value, this.TriggerRange, RoundingMethods.Floor);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0004D6D3 File Offset: 0x0004B8D3
	public bool IsIdentical(StatValueTrigger _To)
	{
		return !(_To.Stat != this.Stat) && !(_To.TriggerRange != this.TriggerRange);
	}

	// Token: 0x04000B91 RID: 2961
	public GameStat Stat;

	// Token: 0x04000B92 RID: 2962
	public Vector2 TriggerRange;

	// Token: 0x04000B93 RID: 2963
	[SerializeField]
	public bool NotifyWhenNotMet;

	// Token: 0x04000B94 RID: 2964
	[SerializeField]
	public bool HideAllWhenNotMet;
}
