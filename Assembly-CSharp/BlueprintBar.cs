using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000042 RID: 66
public class BlueprintBar : MonoBehaviour
{
	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060002C6 RID: 710 RVA: 0x0001AAF4 File Offset: 0x00018CF4
	public bool IsPlaying
	{
		get
		{
			return this.AnimationTween != null;
		}
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0001AB00 File Offset: 0x00018D00
	public void Setup(InGameCardBase _FromCard)
	{
		this.BlueprintCard = _FromCard;
		if (this.BarTr == null)
		{
			this.BarTr = this.BarImage.GetComponent<RectTransform>();
		}
		if (this.AnimationTween != null)
		{
			if (this.AnimationTween.IsPlaying())
			{
				this.AnimationTween.Kill(false);
			}
			this.AnimationTween = null;
		}
		this.AnimatedValue = _FromCard.BlueprintBuildPercentage;
		this.BarImage.fillAmount = this.AnimatedValue;
		this.ProgressText.text = string.Concat(new string[]
		{
			this.BlueprintCard.CardName(false),
			" ",
			(this.AnimatedValue * 100f).ToString("0"),
			"% ",
			LocalizedString.Built
		});
		int num = 0;
		while (num < this.Markers.Count || num <= _FromCard.BlueprintSteps)
		{
			if (num >= this.Markers.Count)
			{
				this.Markers.Add(UnityEngine.Object.Instantiate<BarMilestoneMarker>(this.MilestoneMarkerPrefab, this.BarTr));
			}
			if (num > _FromCard.BlueprintSteps)
			{
				this.Markers[num].gameObject.SetActive(false);
			}
			else
			{
				this.Markers[num].gameObject.SetActive(true);
				this.Markers[num].transform.localPosition = new Vector3(this.BarTr.rect.xMin + this.BarTr.rect.width * ((float)num / (float)_FromCard.BlueprintSteps), this.Markers[num].transform.localPosition.y, 0f);
				this.Markers[num].SetCompleted(_FromCard.BlueprintData.CurrentStage >= num, false);
			}
			num++;
		}
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0001ACF4 File Offset: 0x00018EF4
	public IEnumerator Animate(int _Duration, Action _OnComplete)
	{
		if (this.AnimationTween != null && this.AnimationTween.IsPlaying())
		{
			this.AnimationTween.Kill(false);
		}
		this.AnimationTween = DOTween.To(() => this.AnimatedValue, delegate(float x)
		{
			this.AnimatedValue = x;
		}, this.BlueprintCard.BlueprintBuildPercentage, MBSingleton<GraphicsManager>.Instance.GetTimeSpentAnimDuration(_Duration, false)).SetEase(this.AnimationEase).OnComplete(delegate
		{
			this.AnimationTween = null;
		});
		while (this.AnimationTween.IsPlaying())
		{
			this.<Animate>g__UpdateBar|12_3();
			yield return null;
			if (this.AnimationTween == null)
			{
				this.<Animate>g__UpdateBar|12_3();
				if (_OnComplete != null)
				{
					_OnComplete();
				}
				yield break;
			}
		}
		this.<Animate>g__UpdateBar|12_3();
		if (_OnComplete != null)
		{
			_OnComplete();
		}
		yield break;
	}

	// Token: 0x060002CD RID: 717 RVA: 0x0001AD40 File Offset: 0x00018F40
	[CompilerGenerated]
	private void <Animate>g__UpdateBar|12_3()
	{
		for (int i = 0; i <= this.BlueprintCard.BlueprintSteps; i++)
		{
			if (this.AnimatedValue >= (float)i / (float)this.BlueprintCard.BlueprintSteps)
			{
				this.Markers[i].SetCompleted(true, true);
			}
		}
		this.ProgressText.text = string.Concat(new string[]
		{
			this.BlueprintCard.CardName(false),
			" ",
			(this.AnimatedValue * 100f).ToString("0"),
			"% ",
			LocalizedString.Built
		});
		this.BarImage.fillAmount = this.AnimatedValue;
	}

	// Token: 0x04000348 RID: 840
	[SerializeField]
	private BarMilestoneMarker MilestoneMarkerPrefab;

	// Token: 0x04000349 RID: 841
	[SerializeField]
	private Image BarImage;

	// Token: 0x0400034A RID: 842
	private RectTransform BarTr;

	// Token: 0x0400034B RID: 843
	[SerializeField]
	private Ease AnimationEase;

	// Token: 0x0400034C RID: 844
	[SerializeField]
	private TextMeshProUGUI ProgressText;

	// Token: 0x0400034D RID: 845
	private InGameCardBase BlueprintCard;

	// Token: 0x0400034E RID: 846
	private List<BarMilestoneMarker> Markers = new List<BarMilestoneMarker>();

	// Token: 0x0400034F RID: 847
	protected float AnimatedValue;

	// Token: 0x04000350 RID: 848
	protected Tween AnimationTween;
}
