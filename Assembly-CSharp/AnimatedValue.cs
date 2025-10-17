using System;
using UnityEngine;

// Token: 0x020000CA RID: 202
[Serializable]
public struct AnimatedValue
{
	// Token: 0x17000149 RID: 329
	// (get) Token: 0x0600075B RID: 1883 RVA: 0x00048F51 File Offset: 0x00047151
	// (set) Token: 0x0600075C RID: 1884 RVA: 0x00048F59 File Offset: 0x00047159
	public float CurrentValue { get; private set; }

	// Token: 0x0600075D RID: 1885 RVA: 0x00048F64 File Offset: 0x00047164
	public void Update(float _Time)
	{
		if (this.Animation == null)
		{
			this.CurrentValue = this.Values.y;
			return;
		}
		if (this.Animation.length == 0 || this.Duration <= 0f)
		{
			this.CurrentValue = this.Values.y;
			return;
		}
		float t = this.UsePerlinNoise ? (Mathf.PerlinNoise(_Time * this.NoiseFrequency, 0f) * this.NoiseAmplitude) : this.Animation.Evaluate(Mathf.Repeat(_Time, this.Duration) / this.Duration);
		this.CurrentValue = Mathf.Lerp(this.Values.x, this.Values.y, t);
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0004901B File Offset: 0x0004721B
	public static implicit operator float(AnimatedValue _Value)
	{
		return _Value.CurrentValue;
	}

	// Token: 0x04000A62 RID: 2658
	[SerializeField]
	private Vector2 Values;

	// Token: 0x04000A63 RID: 2659
	[SerializeField]
	private AnimationCurve Animation;

	// Token: 0x04000A64 RID: 2660
	[SerializeField]
	private float Duration;

	// Token: 0x04000A65 RID: 2661
	[SerializeField]
	private bool UsePerlinNoise;

	// Token: 0x04000A66 RID: 2662
	[SerializeField]
	private float NoiseFrequency;

	// Token: 0x04000A67 RID: 2663
	[SerializeField]
	private float NoiseAmplitude;
}
