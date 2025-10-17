using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class PulsingScale : MonoBehaviour
{
	// Token: 0x06000682 RID: 1666 RVA: 0x00044BA6 File Offset: 0x00042DA6
	private void OnDisable()
	{
		if (this.TargetScale)
		{
			this.TargetScale.localScale = Vector3.one;
		}
		this.FreqTime = 0f;
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00044BD0 File Offset: 0x00042DD0
	public void SetPulsing(bool _Value)
	{
		this.FreqTime = 0f;
		this.Pulsing = _Value;
		if (!_Value && !this.KeepScaleOnStop)
		{
			this.TargetScale.localScale = Vector3.one;
		}
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00044C00 File Offset: 0x00042E00
	private void Update()
	{
		if (!this.TargetScale)
		{
			return;
		}
		if (!this.TargetScale.gameObject.activeInHierarchy)
		{
			return;
		}
		if (!this.Pulsing)
		{
			if (!this.KeepScaleOnStop)
			{
				this.TargetScale.localScale = Vector3.one;
			}
			return;
		}
		this.FreqTime += Time.deltaTime;
		if (this.FreqTime >= 1000f)
		{
			this.FreqTime -= 1000f;
		}
		this.TargetScale.localScale = Vector3.Lerp(Vector3.one * this.MinMaxPulseScale.x, Vector3.one * this.MinMaxPulseScale.y, this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.PulseFreq * this.FreqTime - 1.5707964f) * 0.5f + 0.5f));
	}

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	private Transform TargetScale;

	// Token: 0x04000922 RID: 2338
	[SerializeField]
	private bool Pulsing;

	// Token: 0x04000923 RID: 2339
	[SerializeField]
	private float PulseFreq;

	// Token: 0x04000924 RID: 2340
	[SerializeField]
	private AnimationCurve PulseCurve;

	// Token: 0x04000925 RID: 2341
	[SerializeField]
	private Vector2 MinMaxPulseScale;

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private bool KeepScaleOnStop;

	// Token: 0x04000927 RID: 2343
	private float FreqTime;
}
