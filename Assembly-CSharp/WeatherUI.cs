using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x020000B7 RID: 183
[RequireComponent(typeof(CanvasGroup))]
public class WeatherUI : WeatherSpecialEffect
{
	// Token: 0x0600073A RID: 1850 RVA: 0x00048420 File Offset: 0x00046620
	private void Awake()
	{
		this.Group = base.GetComponent<CanvasGroup>();
		if (this.FadeIn)
		{
			this.Group.alpha = 0f;
			this.Group.DOFade(1f, this.FadeTime).SetEase(Ease.OutSine);
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0004846E File Offset: 0x0004666E
	public override void Remove()
	{
		this.Group.DOKill(true);
		this.Group.DOFade(0f, this.FadeTime).SetEase(Ease.OutSine).OnComplete(new TweenCallback(base.Remove));
	}

	// Token: 0x040009E7 RID: 2535
	[SerializeField]
	private float FadeTime;

	// Token: 0x040009E8 RID: 2536
	[SerializeField]
	private bool FadeIn;

	// Token: 0x040009E9 RID: 2537
	private CanvasGroup Group;
}
