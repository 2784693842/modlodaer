using System;
using UnityEngine;

// Token: 0x0200006A RID: 106
[Serializable]
public struct EncounterVariable
{
	// Token: 0x06000488 RID: 1160 RVA: 0x0002F398 File Offset: 0x0002D598
	public float GenerateValue(float _From, bool _WithRandomness)
	{
		if (Mathf.Approximately(this.RandomAddedValue.x, this.RandomAddedValue.y))
		{
			return this.InterpolatedAddedValue.GetInterpolatedValue(_From) + this.RandomAddedValue.x;
		}
		if (_WithRandomness)
		{
			return this.InterpolatedAddedValue.GetInterpolatedValue(_From) + UnityEngine.Random.Range(this.RandomAddedValue.x, this.RandomAddedValue.y);
		}
		return this.InterpolatedAddedValue.GetInterpolatedValue(_From) + Mathf.Lerp(this.RandomAddedValue.x, this.RandomAddedValue.y, 0.5f);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0002F434 File Offset: 0x0002D634
	public Vector2 GenerateRandomRange(float _From)
	{
		if (Mathf.Approximately(this.RandomAddedValue.x, this.RandomAddedValue.y))
		{
			return Vector2.one * (this.InterpolatedAddedValue.GetInterpolatedValue(_From) + this.RandomAddedValue.x);
		}
		return Vector2.one * this.InterpolatedAddedValue.GetInterpolatedValue(_From) + this.RandomAddedValue;
	}

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	private InterpolatedValue InterpolatedAddedValue;

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	[MinMax]
	private Vector2 RandomAddedValue;
}
