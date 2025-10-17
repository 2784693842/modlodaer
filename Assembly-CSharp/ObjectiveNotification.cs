using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009A RID: 154
public class ObjectiveNotification : MonoBehaviour
{
	// Token: 0x06000650 RID: 1616 RVA: 0x00042228 File Offset: 0x00040428
	private void Awake()
	{
		this.PreferredSizeController = base.GetComponent<LayoutElement>();
		if (this.ObjectiveText)
		{
			this.ObjectiveRectTr = this.ObjectiveText.GetComponent<RectTransform>();
		}
		if (this.MovementParent == null)
		{
			this.MovementParent = (this.ObjectiveRectTr ? this.ObjectiveRectTr : base.GetComponent<RectTransform>());
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00042290 File Offset: 0x00040490
	private void LateUpdate()
	{
		bool flag = this.PercentCompletionObjects != null;
		if (flag)
		{
			flag = this.PercentCompletionObjects.activeSelf;
		}
		if (this.ObjectiveRectTr)
		{
			this.PreferredSizeController.preferredHeight = (flag ? (this.ObjectiveRectTr.rect.height + 80f) : this.ObjectiveRectTr.rect.height) + this.ExtraHeight;
		}
		if (flag)
		{
			if (this.PercentBar)
			{
				this.PercentBar.fillAmount = this.CurrentPercentage;
			}
			if (this.PercentText)
			{
				this.PercentText.text = (this.CurrentPercentage * 100f).ToString("0") + "%";
			}
		}
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00042368 File Offset: 0x00040568
	public void Play(Objective _Complete, float _Progress, Action<Objective> _CompleteCallback)
	{
		if (!this.ObjectiveText || !_Complete)
		{
			return;
		}
		if (!_Complete.Complete && _Complete.TotalCompletionValue <= 0f)
		{
			return;
		}
		Canvas.ForceUpdateCanvases();
		this.ObjectiveText.SetText(_Complete.ObjectiveDescription);
		this.ObjectiveText.AssociatedObjective = _Complete;
		this.GivenObjective = _Complete;
		this.ComeInAnim = DOTween.Sequence();
		this.ComeInAnim.Append(this.MovementParent.DOLocalMove(this.MovementParent.localPosition + Vector2.Scale(this.AnimDirection, this.MovementParent.rect.size), this.MovementDuration, false).SetEase(this.MovementEase));
		if (this.PercentCompletionObjects)
		{
			this.PercentCompletionObjects.SetActive(_Complete.NotificationSettings.Frequency == ObjectiveNotificationFrequencies.OnPercentThreshold);
		}
		this.ComeInAnim.AppendInterval(this.StayDuration);
		this.ComeInAnim.Insert(this.MovementDuration + this.StayDuration * this.PulsePosition, this.MovementParent.DOPunchScale(Vector3.one * this.PulseScale, 0.3f, 5, 0.5f).SetEase(this.PulseEase));
		this.ComeInAnim.InsertCallback(this.MovementDuration + this.StayDuration * this.PulsePosition + 0.15f, new TweenCallback(this.ObjectiveText.Refresh));
		if (_Complete.Complete && _CompleteCallback != null)
		{
			this.ComeInAnim.InsertCallback(this.MovementDuration + this.StayDuration * this.PulsePosition, delegate
			{
				_CompleteCallback(_Complete);
			});
		}
		if (this.PercentCompletionObjects && this.PercentCompletionObjects.activeInHierarchy)
		{
			float atPosition = Mathf.Max(0f, this.MovementDuration + this.StayDuration * this.PulsePosition - this.PercentUpdateDuration);
			float currentPercentage = _Complete.CompletionPercent - _Progress;
			float completionPercent = _Complete.CompletionPercent;
			this.CurrentPercentage = currentPercentage;
			this.ComeInAnim.Insert(atPosition, DOTween.To(() => this.CurrentPercentage, delegate(float x)
			{
				this.CurrentPercentage = x;
			}, completionPercent, Mathf.Min(this.PercentUpdateDuration, this.MovementDuration + this.StayDuration * this.PulsePosition)).SetEase(Ease.OutQuad));
		}
		this.ComeInAnim.AppendCallback(new TweenCallback(this.SetupGoAway));
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00042648 File Offset: 0x00040848
	private void SetupGoAway()
	{
		this.GoAwayAnim = DOTween.Sequence();
		this.GoAwayAnim.Append(this.FadeGroup.DOFade(0f, this.FadeDuration).SetEase(this.FadeEase));
		this.GoAwayAnim.AppendCallback(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x000426A8 File Offset: 0x000408A8
	public void Close(bool _Instant)
	{
		if (this.ComeInAnim != null && this.ComeInAnim.active && this.ComeInAnim.IsPlaying())
		{
			this.ComeInAnim.Complete(true);
		}
		if (_Instant && this.GoAwayAnim != null && this.GoAwayAnim.active && this.GoAwayAnim.IsPlaying())
		{
			this.GoAwayAnim.Complete(true);
		}
	}

	// Token: 0x040008AB RID: 2219
	[SerializeField]
	private ObjectiveToggle ObjectiveText;

	// Token: 0x040008AC RID: 2220
	[SerializeField]
	private RectTransform MovementParent;

	// Token: 0x040008AD RID: 2221
	[SerializeField]
	private Vector2 AnimDirection;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private float MovementDuration;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private Ease MovementEase = Ease.InOutSine;

	// Token: 0x040008B0 RID: 2224
	[SerializeField]
	private float StayDuration;

	// Token: 0x040008B1 RID: 2225
	[Range(0f, 1f)]
	[SerializeField]
	private float PulsePosition;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private float PulseScale;

	// Token: 0x040008B3 RID: 2227
	[SerializeField]
	private Ease PulseEase = Ease.InOutSine;

	// Token: 0x040008B4 RID: 2228
	[SerializeField]
	private CanvasGroup FadeGroup;

	// Token: 0x040008B5 RID: 2229
	[SerializeField]
	private float FadeDuration;

	// Token: 0x040008B6 RID: 2230
	[SerializeField]
	private Ease FadeEase = Ease.InOutSine;

	// Token: 0x040008B7 RID: 2231
	[SerializeField]
	private float ExtraHeight;

	// Token: 0x040008B8 RID: 2232
	[SerializeField]
	private GameObject PercentCompletionObjects;

	// Token: 0x040008B9 RID: 2233
	[SerializeField]
	private Image PercentBar;

	// Token: 0x040008BA RID: 2234
	[SerializeField]
	private TextMeshProUGUI PercentText;

	// Token: 0x040008BB RID: 2235
	[SerializeField]
	private float PercentUpdateDuration;

	// Token: 0x040008BC RID: 2236
	private Sequence ComeInAnim;

	// Token: 0x040008BD RID: 2237
	private Sequence GoAwayAnim;

	// Token: 0x040008BE RID: 2238
	private RectTransform ObjectiveRectTr;

	// Token: 0x040008BF RID: 2239
	private LayoutElement PreferredSizeController;

	// Token: 0x040008C0 RID: 2240
	private Objective GivenObjective;

	// Token: 0x040008C1 RID: 2241
	private float CurrentPercentage;

	// Token: 0x040008C2 RID: 2242
	private const float PulseDuration = 0.3f;
}
