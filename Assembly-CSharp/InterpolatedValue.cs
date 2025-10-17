using System;
using UnityEngine;

// Token: 0x020001E0 RID: 480
[Serializable]
public struct InterpolatedValue
{
	// Token: 0x06000CBF RID: 3263 RVA: 0x000683C0 File Offset: 0x000665C0
	public float GetInterpolatedValue(float _Input)
	{
		if (!this.Active)
		{
			return 0f;
		}
		if (this.WhenOutOfRange != InterpolatedValueOutOfRange.UseMinMaxOutputValue && !this.IsInRange(_Input))
		{
			return 0f;
		}
		if (this.OutputValueRange.x < this.OutputValueRange.y)
		{
			return (float)Mathf.FloorToInt(Mathf.Lerp(this.OutputValueRange.x, this.OutputValueRange.y, Mathf.InverseLerp(this.InputValueRange.x, this.InputValueRange.y, _Input)));
		}
		if (this.OutputValueRange.x > this.OutputValueRange.y)
		{
			return (float)Mathf.CeilToInt(Mathf.Lerp(this.OutputValueRange.x, this.OutputValueRange.y, Mathf.InverseLerp(this.InputValueRange.x, this.InputValueRange.y, _Input)));
		}
		return this.OutputValueRange.x;
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x000684AD File Offset: 0x000666AD
	private bool IsInRange(float _Value)
	{
		if (!this.Active)
		{
			return false;
		}
		if (this.InputValueRange.x == this.InputValueRange.y)
		{
			return _Value == this.InputValueRange.x;
		}
		return ExtraMath.FloatIsInRange(_Value, this.InputValueRange, RoundingMethods.None);
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x000684ED File Offset: 0x000666ED
	public bool WillHaveEffect(float _Value)
	{
		return this.Active && (this.IsInRange(_Value) || this.WhenOutOfRange > InterpolatedValueOutOfRange.Return0Value);
	}

	// Token: 0x040011AA RID: 4522
	public bool Active;

	// Token: 0x040011AB RID: 4523
	[MinMax(DisplayOption = MinMaxDisplay.NoHints)]
	public Vector2 InputValueRange;

	// Token: 0x040011AC RID: 4524
	[MinMax(DisplayOption = MinMaxDisplay.NoHints)]
	public Vector2 OutputValueRange;

	// Token: 0x040011AD RID: 4525
	public InterpolatedValueOutOfRange WhenOutOfRange;
}
