using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class StatTrendIndicator : MonoBehaviour
{
	// Token: 0x060006DD RID: 1757 RVA: 0x00046FC4 File Offset: 0x000451C4
	private void Awake()
	{
		this.AllTrendObjects.Add(this.HighDownTrend);
		this.AllTrendObjects.Add(this.MedDownTrend);
		this.AllTrendObjects.Add(this.LowDownTrend);
		this.AllTrendObjects.Add(this.NeutralTrend);
		this.AllTrendObjects.Add(this.LowUpTrend);
		this.AllTrendObjects.Add(this.MedUpTrend);
		this.AllTrendObjects.Add(this.HighUpTrend);
		this.AnimParent = this.AnimatedTr.parent.GetComponent<RectTransform>();
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0004705E File Offset: 0x0004525E
	public void Setup(InGameStat _Stat)
	{
		this.LinkedStat = _Stat;
		this.UpdateAnim();
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00047070 File Offset: 0x00045270
	public void UpdateAnim()
	{
		if (!this.LinkedStat)
		{
			this.UpdateAnim(0f, Vector2.zero, false);
			return;
		}
		this.UpdateAnim(this.LinkedStat.SimpleRatePerTick, this.LinkedStat.StatModel.VisibleTrend, this.LinkedStat.StatModel.InvertedDirection);
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x000470D0 File Offset: 0x000452D0
	public void UpdateAnim(float _Rate, Vector2 _MinMax, bool _Inverted)
	{
		if (Mathf.Approximately(_Rate, 0f))
		{
			this.FreqLevel = 0;
		}
		else if (_Rate < 0f)
		{
			if (_MinMax.x >= 0f)
			{
				this.FreqLevel = -1;
			}
			else
			{
				this.FreqLevel = Mathf.CeilToInt(Mathf.Clamp01(_Rate / _MinMax.x) * 3f) * -1;
			}
		}
		else if (_MinMax.y <= 0f)
		{
			this.FreqLevel = 1;
		}
		else
		{
			this.FreqLevel = Mathf.CeilToInt(Mathf.Clamp01(_Rate / _MinMax.y) * 3f);
		}
		if (_Inverted)
		{
			this.FreqLevel *= -1;
		}
		this.Animate();
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00047180 File Offset: 0x00045380
	public void Animate()
	{
		if (!this.AnimParent)
		{
			this.AnimParent = this.AnimatedTr.parent.GetComponent<RectTransform>();
		}
		float trendAnimFreq = GraphicsManager.GetTrendAnimFreq(Mathf.Abs(this.FreqLevel));
		for (int i = 0; i < this.AllTrendObjects.Count; i++)
		{
			this.AllTrendObjects[i].SetActive(this.FreqLevel == i - 3);
		}
		this.AnimatedTr.localPosition = this.AnimParent.rect.center + Vector3.up * Mathf.Lerp(0f, this.AnimYOffset, trendAnimFreq) * Mathf.Sign((float)this.FreqLevel) * GraphicsManager.GetTrendAnimScale(Mathf.Abs(this.FreqLevel));
		this.AnimatedTr.localScale = Vector3.one * Mathf.Lerp(1f, this.AnimScaleOffset, trendAnimFreq) * GraphicsManager.GetTrendAnimScale(Mathf.Abs(this.FreqLevel));
	}

	// Token: 0x04000996 RID: 2454
	private InGameStat LinkedStat;

	// Token: 0x04000997 RID: 2455
	[SerializeField]
	private Transform AnimatedTr;

	// Token: 0x04000998 RID: 2456
	[SerializeField]
	private float AnimYOffset;

	// Token: 0x04000999 RID: 2457
	[SerializeField]
	private float AnimScaleOffset;

	// Token: 0x0400099A RID: 2458
	[SerializeField]
	private GameObject HighDownTrend;

	// Token: 0x0400099B RID: 2459
	[SerializeField]
	private GameObject MedDownTrend;

	// Token: 0x0400099C RID: 2460
	[SerializeField]
	private GameObject LowDownTrend;

	// Token: 0x0400099D RID: 2461
	[SerializeField]
	private GameObject NeutralTrend;

	// Token: 0x0400099E RID: 2462
	[SerializeField]
	private GameObject LowUpTrend;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private GameObject MedUpTrend;

	// Token: 0x040009A0 RID: 2464
	[SerializeField]
	private GameObject HighUpTrend;

	// Token: 0x040009A1 RID: 2465
	private int FreqLevel;

	// Token: 0x040009A2 RID: 2466
	private List<GameObject> AllTrendObjects = new List<GameObject>();

	// Token: 0x040009A3 RID: 2467
	private RectTransform AnimParent;
}
