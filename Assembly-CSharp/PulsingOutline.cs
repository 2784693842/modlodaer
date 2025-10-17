using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A0 RID: 160
public class PulsingOutline : MonoBehaviour
{
	// Token: 0x0600067B RID: 1659 RVA: 0x0004493C File Offset: 0x00042B3C
	private void Awake()
	{
		if (!this.TargetOutline)
		{
			this.TargetOutline = base.GetComponent<Outline>();
		}
		if (this.TargetOutline)
		{
			this.DefaultDistance = this.TargetOutline.effectDistance;
			return;
		}
		this.DefaultDistance = Vector2.one * -1000f;
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00044996 File Offset: 0x00042B96
	private void OnEnable()
	{
		if (this.SyncOutlineEnable)
		{
			this.SetOutline(true);
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x000449A8 File Offset: 0x00042BA8
	private void OnDisable()
	{
		if (this.DefaultDistance != Vector2.one * -1000f)
		{
			this.TargetOutline.effectDistance = this.DefaultDistance;
		}
		if (this.AffectColor && this.DefaultColor != Color.clear)
		{
			this.TargetOutline.effectColor = this.DefaultColor;
		}
		if (this.SyncOutlineEnable)
		{
			this.SetOutline(false);
		}
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00044A1C File Offset: 0x00042C1C
	public void SetPulsing(bool _Value)
	{
		this.FreqTime = 0f;
		this.Pulsing = _Value;
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00044A30 File Offset: 0x00042C30
	public void SetOutline(bool _Value)
	{
		this.FreqTime = 0f;
		if (this.TargetOutline)
		{
			this.TargetOutline.enabled = _Value;
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00044A58 File Offset: 0x00042C58
	private void Update()
	{
		if (!this.TargetOutline)
		{
			return;
		}
		if (this.DefaultDistance == Vector2.one * -1000f)
		{
			this.DefaultDistance = this.TargetOutline.effectDistance;
		}
		if (this.DefaultColor == Color.clear)
		{
			this.DefaultColor = this.TargetOutline.effectColor;
		}
		if (!this.TargetOutline.enabled)
		{
			return;
		}
		if (!this.Pulsing)
		{
			this.TargetOutline.effectDistance = this.DefaultDistance;
			return;
		}
		this.FreqTime += Time.deltaTime;
		if (this.FreqTime >= 1000f)
		{
			this.FreqTime -= 1000f;
		}
		this.TargetOutline.effectDistance = Vector2.Lerp(this.DefaultDistance * this.MinMaxPulseDistance.x, this.DefaultDistance * this.MinMaxPulseDistance.y, this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.PulseFreq * this.FreqTime - 1.5707964f) * 0.5f + 0.5f));
		if (this.AffectColor)
		{
			this.TargetOutline.effectColor = this.OutlineColor;
		}
	}

	// Token: 0x04000916 RID: 2326
	[SerializeField]
	private Outline TargetOutline;

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	private bool SyncOutlineEnable;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	private bool Pulsing;

	// Token: 0x04000919 RID: 2329
	[SerializeField]
	private float PulseFreq;

	// Token: 0x0400091A RID: 2330
	[SerializeField]
	private AnimationCurve PulseCurve;

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private Vector2 MinMaxPulseDistance;

	// Token: 0x0400091C RID: 2332
	[Space]
	[SerializeField]
	private bool AffectColor;

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	private Color OutlineColor;

	// Token: 0x0400091E RID: 2334
	private float FreqTime;

	// Token: 0x0400091F RID: 2335
	private Vector2 DefaultDistance;

	// Token: 0x04000920 RID: 2336
	private Color DefaultColor;
}
