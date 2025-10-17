using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class ObjectiveCompletePopup : MonoBehaviour
{
	// Token: 0x0600064D RID: 1613 RVA: 0x00042054 File Offset: 0x00040254
	public void Play(Objective _Complete, Transform _Origin)
	{
		Canvas.ForceUpdateCanvases();
		this.ObjectiveText.SetText(_Complete.ObjectiveDescription);
		this.ObjectiveText.AssociatedObjective = _Complete;
		base.transform.position = _Origin.position;
		base.transform.localScale = Vector3.one * 0.001f;
		this.PlaySequence = DOTween.Sequence();
		this.PlaySequence.Append(base.transform.DOLocalMove(Vector3.zero, this.MovementDuration, false).SetEase(this.MovementEase));
		this.PlaySequence.Insert(0f, base.transform.DOScale(Vector3.one, this.MovementDuration).SetEase(Ease.Linear));
		this.PlaySequence.AppendInterval(this.StayDuration);
		this.PlaySequence.Insert(this.MovementDuration, this.PulseTr.DOPunchScale(Vector3.one * this.PulseScale, 0.3f, 5, 0.5f).SetEase(this.PulseEase));
		this.PlaySequence.InsertCallback(0.15f, new TweenCallback(this.ObjectiveText.Refresh));
		this.PlaySequence.Append(base.transform.DOMove(_Origin.position, this.MovementDuration, false).SetEase(this.MovementEase));
		this.PlaySequence.Insert(this.MovementDuration + this.StayDuration, base.transform.DOScale(Vector3.one * 0.001f, this.MovementDuration).SetEase(Ease.Linear));
		this.PlaySequence.AppendCallback(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	// Token: 0x040008A2 RID: 2210
	[SerializeField]
	private ObjectiveToggle ObjectiveText;

	// Token: 0x040008A3 RID: 2211
	[Space]
	[SerializeField]
	private float MovementDuration;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private Ease MovementEase;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	private float StayDuration;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private float PulseScale;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	private Ease PulseEase = Ease.InOutSine;

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private Transform PulseTr;

	// Token: 0x040008A9 RID: 2217
	private Sequence PlaySequence;

	// Token: 0x040008AA RID: 2218
	private const float PulseDuration = 0.3f;
}
