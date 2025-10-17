using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200007C RID: 124
public class ExplorationBar : MonoBehaviour
{
	// Token: 0x060004F1 RID: 1265 RVA: 0x0003275C File Offset: 0x0003095C
	public void Setup(InGameCardBase _FromCard)
	{
		this.ExplorationCard = _FromCard;
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
		this.AnimatedValue = _FromCard.ExplorationData.CurrentExploration;
		this.BarImage.fillAmount = this.AnimatedValue;
		this.ProgressText.text = string.Concat(new string[]
		{
			this.ExplorationCard.CardName(false),
			" ",
			(this.AnimatedValue * 100f).ToString("0"),
			"% ",
			LocalizedString.Explored
		});
		int index = -1;
		int num = 0;
		while (num < this.Markers.Count || num < _FromCard.CardModel.ExplorationResults.Length)
		{
			if (num >= this.Markers.Count)
			{
				this.Markers.Add(UnityEngine.Object.Instantiate<BarMilestoneMarker>(this.MilestoneMarkerPrefab, this.BarTr));
			}
			if (num >= _FromCard.CardModel.ExplorationResults.Length)
			{
				this.Markers[num].gameObject.SetActive(false);
			}
			else
			{
				this.Markers[num].transform.localPosition = new Vector3(this.BarTr.rect.xMin + this.BarTr.rect.width * _FromCard.CardModel.ExplorationResults[num].TriggerValue, this.Markers[num].transform.localPosition.y, 0f);
				if (_FromCard.ExplorationData.HasData(_FromCard.CardModel.ExplorationResults[num].ActionName, out index))
				{
					if (!_FromCard.ExplorationData.ExplorationResults[index].TriggeredWithoutResults)
					{
						this.Markers[num].SetCompleted(_FromCard.ExplorationData.ExplorationResults[index].Triggered, false);
						if (_FromCard.ExplorationData.ExplorationResults[index].Triggered || this.ShowNotReachedMilestones)
						{
							this.Markers[num].gameObject.SetActive(true);
						}
						else
						{
							this.Markers[num].gameObject.SetActive(false);
						}
					}
					else
					{
						this.Markers[num].SetCompleted(false, false);
						this.Markers[num].gameObject.SetActive(false);
					}
				}
				else
				{
					this.Markers[num].SetCompleted(_FromCard.ExplorationData.CurrentExploration >= _FromCard.CardModel.ExplorationResults[num].TriggerValue, false);
				}
			}
			num++;
		}
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00032A49 File Offset: 0x00030C49
	public IEnumerator Animate(Action<ExplorationResult> _Callback)
	{
		if (this.AnimationTween != null && this.AnimationTween.IsPlaying())
		{
			this.AnimationTween.Kill(false);
		}
		List<int> triggeredActions = new List<int>();
		if (!Mathf.Approximately(this.AnimatedValue, this.ExplorationCard.ExplorationData.CurrentExploration))
		{
			this.AnimationTween = DOTween.To(() => this.AnimatedValue, delegate(float x)
			{
				this.AnimatedValue = x;
			}, this.ExplorationCard.ExplorationData.CurrentExploration, MBSingleton<GraphicsManager>.Instance.GetTimeSpentAnimDuration(1, false)).SetEase(this.AnimationEase).OnComplete(delegate
			{
				this.AnimationTween = null;
			});
			while (this.AnimationTween.IsPlaying())
			{
				for (int i = 0; i < this.ExplorationCard.CardModel.ExplorationResults.Length; i++)
				{
					if (this.ShouldUnlockExplorationResults(i) && !triggeredActions.Contains(i))
					{
						if (_Callback != null)
						{
							_Callback(this.ExplorationCard.CardModel.ExplorationResults[i]);
						}
						triggeredActions.Add(i);
						this.Markers[i].gameObject.SetActive(true);
						this.Markers[i].SetCompleted(true, true);
					}
				}
				this.ProgressText.text = string.Concat(new string[]
				{
					this.ExplorationCard.CardName(false),
					" ",
					(this.AnimatedValue * 100f).ToString("0"),
					"% ",
					LocalizedString.Explored
				});
				this.BarImage.fillAmount = this.AnimatedValue;
				yield return null;
				if (this.AnimationTween == null)
				{
					break;
				}
			}
		}
		this.AnimatedValue = this.ExplorationCard.ExplorationData.CurrentExploration;
		this.ProgressText.text = string.Concat(new string[]
		{
			this.ExplorationCard.CardName(false),
			" ",
			(this.AnimatedValue * 100f).ToString("0"),
			"% ",
			LocalizedString.Explored
		});
		this.BarImage.fillAmount = this.AnimatedValue;
		bool flag = false;
		for (int j = 0; j < this.ExplorationCard.CardModel.ExplorationResults.Length; j++)
		{
			if (this.ShouldUnlockExplorationResults(j) && !triggeredActions.Contains(j))
			{
				if (_Callback != null)
				{
					_Callback(this.ExplorationCard.CardModel.ExplorationResults[j]);
				}
				triggeredActions.Add(j);
				this.Markers[j].gameObject.SetActive(true);
				this.Markers[j].SetCompleted(true, true);
				flag = true;
			}
		}
		if (flag)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00032A60 File Offset: 0x00030C60
	private bool ShouldUnlockExplorationResults(int _Index)
	{
		int index;
		if (!this.ExplorationCard.ExplorationData.HasData(this.ExplorationCard.CardModel.ExplorationResults[_Index].ActionName, out index))
		{
			return false;
		}
		if (this.ExplorationCard.ExplorationData.ExplorationResults[index].Triggered && !this.ExplorationCard.ExplorationData.ExplorationResults[index].TriggeredWithoutResults)
		{
			bool flag = true;
			for (int i = 0; i < this.ExplorationCard.CardModel.ExplorationResults[_Index].Action.ProducedCards.Length; i++)
			{
				if (this.CheckForUniqueOnBoardUnspawnedCards)
				{
					if (this.ExplorationCard.CardModel.ExplorationResults[_Index].Action.ProducedCards[i].HasUnspawnedUniqueCards)
					{
						flag = false;
						break;
					}
				}
				else if (this.ExplorationCard.CardModel.ExplorationResults[_Index].Action.ProducedCards[i].HasUnspawnedEvents)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return false;
			}
		}
		if (this.ExplorationCard.CardModel.ExplorationResults[_Index].TriggerValue > this.AnimatedValue)
		{
			return false;
		}
		bool flag2 = !this.ExplorationCard.CardModel.ExplorationResults[_Index].Action.QuickRequirementsCheck(this.ExplorationCard, true);
		this.ExplorationCard.ExplorationData.ExplorationResults[index] = new ExplorationResultSaveData
		{
			ActionName = this.ExplorationCard.CardModel.ExplorationResults[_Index].ActionName,
			Triggered = true,
			TriggeredWithoutResults = flag2
		};
		return !flag2;
	}

	// Token: 0x0400064C RID: 1612
	[SerializeField]
	private BarMilestoneMarker MilestoneMarkerPrefab;

	// Token: 0x0400064D RID: 1613
	[SerializeField]
	private Image BarImage;

	// Token: 0x0400064E RID: 1614
	private RectTransform BarTr;

	// Token: 0x0400064F RID: 1615
	[SerializeField]
	private Ease AnimationEase;

	// Token: 0x04000650 RID: 1616
	[SerializeField]
	private bool ShowNotReachedMilestones;

	// Token: 0x04000651 RID: 1617
	[SerializeField]
	private TextMeshProUGUI ProgressText;

	// Token: 0x04000652 RID: 1618
	[SerializeField]
	private bool CheckForUniqueOnBoardUnspawnedCards;

	// Token: 0x04000653 RID: 1619
	private InGameCardBase ExplorationCard;

	// Token: 0x04000654 RID: 1620
	private List<BarMilestoneMarker> Markers = new List<BarMilestoneMarker>();

	// Token: 0x04000655 RID: 1621
	private float AnimatedValue;

	// Token: 0x04000656 RID: 1622
	private Tween AnimationTween;
}
