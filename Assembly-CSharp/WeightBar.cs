using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B9 RID: 185
public class WeightBar : MonoBehaviour
{
	// Token: 0x0600073F RID: 1855 RVA: 0x00048514 File Offset: 0x00046714
	private void Start()
	{
		this.GetWeightStat();
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0004851C File Offset: 0x0004671C
	private void GetWeightStat()
	{
		if (MBSingleton<GameManager>.Instance)
		{
			this.WeightStat = MBSingleton<GameManager>.Instance.InGamePlayerWeight;
		}
		if (this.WeightStat && this.StatColorBar)
		{
			this.StatColorBar.color = this.WeightStat.StatModel.GetBarColor;
		}
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0004857C File Offset: 0x0004677C
	private void Update()
	{
		if (!this.WeightStat)
		{
			return;
		}
		this.CurrentStatus = this.WeightStat.AnyCurrentStatus(true);
		if (this.CurrentStatus != null)
		{
			GameObject weightWarning = this.WeightWarning1;
			if (weightWarning != null)
			{
				weightWarning.SetActive(this.CurrentStatus.AlertLevel == AlertLevels.High);
			}
			GameObject weightWarning2 = this.WeightWarning2;
			if (weightWarning2 != null)
			{
				weightWarning2.SetActive(this.CurrentStatus.AlertLevel == AlertLevels.Critical);
			}
		}
		else
		{
			GameObject weightWarning3 = this.WeightWarning1;
			if (weightWarning3 != null)
			{
				weightWarning3.SetActive(false);
			}
			GameObject weightWarning4 = this.WeightWarning2;
			if (weightWarning4 != null)
			{
				weightWarning4.SetActive(false);
			}
		}
		if (this.HighlightColorBar)
		{
			if (this.WeightStat.NormalizedAnimatedValue < this.WeightStat.NormalizedVisibleValue)
			{
				this.HighlightColorBar.color = this.WeightStat.StatModel.BarHighlightColor;
				if (!Mathf.Approximately(this.HighlightColorBar.fillAmount, this.WeightStat.NormalizedVisibleValue))
				{
					this.HighlightColorBar.fillAmount = this.WeightStat.NormalizedVisibleValue;
				}
			}
			else
			{
				this.HighlightColorBar.color = MBSingleton<GraphicsManager>.Instance.NegativeChangeStatusBarColor;
				if (!Mathf.Approximately(this.HighlightColorBar.fillAmount, this.WeightStat.NormalizedAnimatedValue))
				{
					this.HighlightColorBar.fillAmount = this.WeightStat.NormalizedAnimatedValue;
				}
			}
		}
		if (this.StatColorBar)
		{
			if (this.WeightStat.NormalizedAnimatedValue < this.WeightStat.NormalizedVisibleValue)
			{
				if (!Mathf.Approximately(this.StatColorBar.fillAmount, this.WeightStat.NormalizedAnimatedValue))
				{
					this.StatColorBar.fillAmount = this.WeightStat.NormalizedAnimatedValue;
					return;
				}
			}
			else if (!Mathf.Approximately(this.StatColorBar.fillAmount, this.WeightStat.NormalizedVisibleValue))
			{
				this.StatColorBar.fillAmount = this.WeightStat.NormalizedVisibleValue;
			}
		}
	}

	// Token: 0x040009ED RID: 2541
	private InGameStat WeightStat;

	// Token: 0x040009EE RID: 2542
	[SerializeField]
	private Image StatColorBar;

	// Token: 0x040009EF RID: 2543
	[SerializeField]
	private Image HighlightColorBar;

	// Token: 0x040009F0 RID: 2544
	[SerializeField]
	private GameObject WeightWarning1;

	// Token: 0x040009F1 RID: 2545
	[SerializeField]
	private GameObject WeightWarning2;

	// Token: 0x040009F2 RID: 2546
	private StatStatus CurrentStatus;
}
