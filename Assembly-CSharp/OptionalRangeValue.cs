using System;
using UnityEngine;

// Token: 0x020001C5 RID: 453
[Serializable]
public class OptionalRangeValue : OptionalFloatValue
{
	// Token: 0x06000C5A RID: 3162 RVA: 0x00065A70 File Offset: 0x00063C70
	public OptionalRangeValue(bool _Active, float _Value, float _MaxValue) : base(_Active, _Value)
	{
		this.MaxValue = _MaxValue;
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000C5B RID: 3163 RVA: 0x00065A81 File Offset: 0x00063C81
	public float GetRandomValue
	{
		get
		{
			return UnityEngine.Random.Range(this.FloatValue, this.MaxValue);
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x00065A94 File Offset: 0x00063C94
	public bool IsInRange(float _Value)
	{
		if (!this.Active)
		{
			return true;
		}
		if (ExtraMath.FloatIsEqual(this.FloatValue, this.MaxValue))
		{
			return ExtraMath.FloatIsEqual(_Value, this.FloatValue);
		}
		if (this.FloatValue > this.MaxValue)
		{
			return ExtraMath.FloatIsGreaterOrEqual(_Value, this.FloatValue);
		}
		return ExtraMath.FloatIsInRange(_Value, new Vector2(this.FloatValue, this.MaxValue), RoundingMethods.None);
	}

	// Token: 0x0400113C RID: 4412
	public float MaxValue;
}
