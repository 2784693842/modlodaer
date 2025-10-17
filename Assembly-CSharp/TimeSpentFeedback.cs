using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class TimeSpentFeedback : ContinuousImageFeedback
{
	// Token: 0x060006E3 RID: 1763 RVA: 0x000472AC File Offset: 0x000454AC
	protected override void PlayStart()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		if (this.PulseTween != null)
		{
			this.PulseTween.Kill(true);
			this.PulseAnimTr.localScale = Vector3.one;
		}
		this.PulseTween = this.PulseAnimTr.DOPunchScale(Vector3.one * this.PulseScale, this.PulseDuration, 5, 0.5f).SetEase(this.PulseEase).OnKill(delegate
		{
			this.PulseTween = null;
		});
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00047340 File Offset: 0x00045540
	public override void PlayStop()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.PulseTween != null)
		{
			this.PulseTween.Kill(true);
			this.PulseAnimTr.localScale = Vector3.one;
		}
		this.PulseTween = this.PulseAnimTr.DOPunchScale(Vector3.one * this.PulseScale, this.PulseDuration, 5, 0.5f).SetEase(this.PulseEase).OnKill(delegate
		{
			this.PulseTween = null;
			base.gameObject.SetActive(false);
		});
	}

	// Token: 0x040009A4 RID: 2468
	[Header("Start and stop")]
	[SerializeField]
	private Transform PulseAnimTr;

	// Token: 0x040009A5 RID: 2469
	[SerializeField]
	private float PulseDuration;

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	private float PulseScale;

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	private Ease PulseEase;

	// Token: 0x040009A8 RID: 2472
	private Tween PulseTween;
}
