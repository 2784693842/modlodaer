using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
[Serializable]
public struct DurabilityWeightValue
{
	// Token: 0x06000811 RID: 2065 RVA: 0x0004FC04 File Offset: 0x0004DE04
	public int GetExtraWeight(float _Durability)
	{
		if (!this.Active)
		{
			return 0;
		}
		bool flag = this.IsInRange(_Durability);
		if (this.WhenOutOfRange == StatDropChanceModOutOfRange.UseMinMaxWeight || flag)
		{
			if (this.InterpWeightRange.x < this.InterpWeightRange.y)
			{
				return Mathf.FloorToInt(Mathf.Lerp((float)this.InterpWeightRange.x, (float)this.InterpWeightRange.y, Mathf.InverseLerp(this.DurabilityRange.x, this.DurabilityRange.y, _Durability)));
			}
			if (this.InterpWeightRange.x > this.InterpWeightRange.y)
			{
				return Mathf.CeilToInt(Mathf.Lerp((float)this.InterpWeightRange.x, (float)this.InterpWeightRange.y, Mathf.InverseLerp(this.DurabilityRange.x, this.DurabilityRange.y, _Durability)));
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

	// Token: 0x06000812 RID: 2066 RVA: 0x0004FCFC File Offset: 0x0004DEFC
	private bool IsInRange(float _Value)
	{
		return this.Active && ExtraMath.FloatIsInRange(_Value, this.DurabilityRange, RoundingMethods.None);
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x0004FD15 File Offset: 0x0004DF15
	public bool WillHaveEffect(float _Value)
	{
		return this.Active && (this.IsInRange(_Value) || this.WhenOutOfRange > StatDropChanceModOutOfRange.Add0Weight);
	}

	// Token: 0x04000C28 RID: 3112
	public bool Active;

	// Token: 0x04000C29 RID: 3113
	[MinMax]
	public Vector2 DurabilityRange;

	// Token: 0x04000C2A RID: 3114
	public Vector2Int InterpWeightRange;

	// Token: 0x04000C2B RID: 3115
	public StatDropChanceModOutOfRange WhenOutOfRange;
}
