using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class BlueprintUnlockedPopup : MonoBehaviour
{
	// Token: 0x060002F9 RID: 761 RVA: 0x0001E96C File Offset: 0x0001CB6C
	public void Play(CardData _Blueprint, Transform _CardDestination)
	{
		if (!_Blueprint)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Canvas.ForceUpdateCanvases();
		if (this.IsForWounds)
		{
			this.MainTitle.text = LocalizedString.Wounded;
		}
		else if (_Blueprint.CardType != CardTypes.EnvDamage)
		{
			this.MainTitle.text = ((_Blueprint.CardType == CardTypes.EnvImprovement) ? LocalizedString.NewImprovement : LocalizedString.NewBlueprint);
		}
		else
		{
			this.MainTitle.text = LocalizedString.NewDamage;
		}
		this.BlueprintNameText.text = _Blueprint.CardName;
		base.transform.localScale = Vector3.one * 0.001f;
		this.BlueprintPreview.Setup(_Blueprint, false);
		this.PlaySequence = DOTween.Sequence();
		this.PlaySequence.Append(base.transform.DOScale(Vector3.one, 0.3f));
		this.PlaySequence.Append(this.PulseTr.DOPunchScale(Vector3.one * this.PulseScale, 0.3f, 5, this.CardMovementDuration).SetEase(this.PulseEase));
		this.PlaySequence.AppendInterval(this.StayDuration);
		this.PlaySequence.Append(this.BlueprintPreview.transform.DOMove(_CardDestination.transform.position, this.CardMovementDuration, false).SetEase(this.CardMovementEase));
		this.PlaySequence.Insert(this.StayDuration + 0.3f + this.CardMovementDuration, this.BlueprintPreview.transform.DOScale(Vector3.one * 0.001f, this.CardMovementDuration).SetEase(Ease.Linear));
		this.PlaySequence.Insert(this.StayDuration + 0.3f + this.CardMovementDuration, this.FadeGroup.DOFade(0f, this.CardMovementDuration).SetEase(this.CardMovementEase));
		this.PlaySequence.AppendCallback(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0001EB8C File Offset: 0x0001CD8C
	public void Skip()
	{
		if (this.PlaySequence.position < this.AllowSkipTime)
		{
			this.PlaySequence.Goto(this.AllowSkipTime, true);
			return;
		}
		float num = 0.6f + this.StayDuration;
		if (num > this.AllowSkipTime && num > this.PlaySequence.position)
		{
			this.PlaySequence.Goto(num, true);
			if (this.RaycastGroup)
			{
				this.RaycastGroup.blocksRaycasts = false;
			}
		}
	}

	// Token: 0x04000388 RID: 904
	[SerializeField]
	private TextMeshProUGUI MainTitle;

	// Token: 0x04000389 RID: 905
	[SerializeField]
	private TextMeshProUGUI BlueprintNameText;

	// Token: 0x0400038A RID: 906
	[SerializeField]
	private bool IsForWounds;

	// Token: 0x0400038B RID: 907
	[SerializeField]
	private CanvasGroup FadeGroup;

	// Token: 0x0400038C RID: 908
	[SerializeField]
	private float StayDuration;

	// Token: 0x0400038D RID: 909
	[SerializeField]
	private float PulseScale;

	// Token: 0x0400038E RID: 910
	[SerializeField]
	private Ease PulseEase = Ease.InOutSine;

	// Token: 0x0400038F RID: 911
	[SerializeField]
	private float CardMovementDuration;

	// Token: 0x04000390 RID: 912
	[SerializeField]
	private Ease CardMovementEase;

	// Token: 0x04000391 RID: 913
	[SerializeField]
	private Transform PulseTr;

	// Token: 0x04000392 RID: 914
	[SerializeField]
	private MenuCardPreview BlueprintPreview;

	// Token: 0x04000393 RID: 915
	[SerializeField]
	private float AllowSkipTime;

	// Token: 0x04000394 RID: 916
	[SerializeField]
	private CanvasGroup RaycastGroup;

	// Token: 0x04000395 RID: 917
	private Sequence PlaySequence;

	// Token: 0x04000396 RID: 918
	private const float PulseDuration = 0.3f;
}
