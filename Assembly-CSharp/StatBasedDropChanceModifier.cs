using System;
using UnityEngine;

// Token: 0x020000EB RID: 235
[Serializable]
public struct StatBasedDropChanceModifier
{
	// Token: 0x0600080A RID: 2058 RVA: 0x0004F408 File Offset: 0x0004D608
	public StatDropWeightModReport ToReport(float _StatValue)
	{
		return new StatDropWeightModReport
		{
			Stat = this.Stat,
			BonusWeight = this.GetExtraWeight(_StatValue)
		};
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0004F43C File Offset: 0x0004D63C
	public int GetExtraWeight(float StatValue)
	{
		if (this.WhenOutOfRange == StatDropChanceModOutOfRange.UseMinMaxWeight || this.IsInRange(StatValue))
		{
			if (this.InterpWeightRange.x < this.InterpWeightRange.y)
			{
				return Mathf.FloorToInt(Mathf.Lerp((float)this.InterpWeightRange.x, (float)this.InterpWeightRange.y, Mathf.InverseLerp(this.StatRange.x, this.StatRange.y, StatValue)));
			}
			if (this.InterpWeightRange.x > this.InterpWeightRange.y)
			{
				return Mathf.CeilToInt(Mathf.Lerp((float)this.InterpWeightRange.x, (float)this.InterpWeightRange.y, Mathf.InverseLerp(this.StatRange.x, this.StatRange.y, StatValue)));
			}
			return this.InterpWeightRange.x;
		}
		else
		{
			if (this.WhenOutOfRange == StatDropChanceModOutOfRange.Add0Weight)
			{
				return 0;
			}
			return -1000000;
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0004F527 File Offset: 0x0004D727
	private bool IsInRange(float _Value)
	{
		return ExtraMath.FloatIsInRange(_Value, this.StatRange, RoundingMethods.None);
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0004F536 File Offset: 0x0004D736
	public bool WillHaveEffect(float _Value)
	{
		return this.IsInRange(_Value) || this.WhenOutOfRange > StatDropChanceModOutOfRange.Add0Weight;
	}

	// Token: 0x04000C0B RID: 3083
	public GameStat Stat;

	// Token: 0x04000C0C RID: 3084
	[MinMax]
	public Vector2 StatRange;

	// Token: 0x04000C0D RID: 3085
	public Vector2Int InterpWeightRange;

	// Token: 0x04000C0E RID: 3086
	public StatDropChanceModOutOfRange WhenOutOfRange;
}
