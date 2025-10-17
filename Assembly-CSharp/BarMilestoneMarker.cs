using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000041 RID: 65
public class BarMilestoneMarker : MonoBehaviour
{
	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060002C0 RID: 704 RVA: 0x0001A9E8 File Offset: 0x00018BE8
	// (set) Token: 0x060002C1 RID: 705 RVA: 0x0001A9F0 File Offset: 0x00018BF0
	public bool Completed { get; private set; }

	// Token: 0x060002C2 RID: 706 RVA: 0x0001A9F9 File Offset: 0x00018BF9
	private void OnEnable()
	{
		this.SetCompleted(this.Completed, false);
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0001AA08 File Offset: 0x00018C08
	public void SetCompleted(bool _Completed, bool _Animate)
	{
		if (this.Icon)
		{
			this.Icon.overrideSprite = (_Completed ? this.CompletedSprite : null);
			this.Icon.color = (_Completed ? this.CompletedColor : this.DefaultColor);
		}
		if (this.Completed == _Completed)
		{
			return;
		}
		this.Completed = _Completed;
		if (!_Animate)
		{
			return;
		}
		if (this.AnimTween != null)
		{
			if (this.AnimTween.IsPlaying())
			{
				this.AnimTween.Kill(true);
			}
			this.AnimTween = null;
		}
		base.transform.localScale = Vector3.one;
		this.AnimTween = base.transform.DOPunchScale(Vector3.one * this.PulseScale, this.PulseDuration, 5, 0.5f).SetEase(this.PulseEase).OnComplete(delegate
		{
			this.AnimTween = null;
		});
	}

	// Token: 0x0400033F RID: 831
	[SerializeField]
	private Image Icon;

	// Token: 0x04000340 RID: 832
	[SerializeField]
	private Sprite CompletedSprite;

	// Token: 0x04000341 RID: 833
	[SerializeField]
	private Color DefaultColor;

	// Token: 0x04000342 RID: 834
	[SerializeField]
	private Color CompletedColor;

	// Token: 0x04000343 RID: 835
	[SpecialHeader("Animation", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	[SerializeField]
	private float PulseScale;

	// Token: 0x04000344 RID: 836
	[SerializeField]
	private float PulseDuration;

	// Token: 0x04000345 RID: 837
	[SerializeField]
	private Ease PulseEase;

	// Token: 0x04000347 RID: 839
	private Tween AnimTween;
}
