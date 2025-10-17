using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class FadeToBlackScreen : MonoBehaviour
{
	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600051A RID: 1306 RVA: 0x000345EE File Offset: 0x000327EE
	// (set) Token: 0x0600051B RID: 1307 RVA: 0x000345F6 File Offset: 0x000327F6
	public FadeToBlackTypes CurrentFadeType { get; private set; }

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x000345FF File Offset: 0x000327FF
	public bool FadeActive
	{
		get
		{
			return this.CurrentFadeType > FadeToBlackTypes.None;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x0600051D RID: 1309 RVA: 0x0003460A File Offset: 0x0003280A
	public Transform TimeSpentPosTr
	{
		get
		{
			if (this.CurrentFadeType != FadeToBlackTypes.Partial)
			{
				if (this.FullTimeSpentPos)
				{
					return this.FullTimeSpentPos;
				}
				return base.transform;
			}
			else
			{
				if (this.PartialTimeSpentPos)
				{
					return this.PartialTimeSpentPos;
				}
				return base.transform;
			}
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x0600051E RID: 1310 RVA: 0x0003464C File Offset: 0x0003284C
	public Vector3 TimeSpentPos
	{
		get
		{
			Transform timeSpentPosTr = this.TimeSpentPosTr;
			if (timeSpentPosTr)
			{
				return timeSpentPosTr.position;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00034674 File Offset: 0x00032874
	private void Awake()
	{
		if (this.FadeGroup.alpha > 0f && (this.PartialFade.activeSelf || this.FullFade.activeSelf))
		{
			this.CurrentFadeType = (this.FullFade.activeSelf ? FadeToBlackTypes.Partial : FadeToBlackTypes.Full);
		}
		else
		{
			this.CurrentFadeType = FadeToBlackTypes.None;
		}
		this.FadeGroup.blocksRaycasts = (this.CurrentFadeType > FadeToBlackTypes.None);
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x000346E4 File Offset: 0x000328E4
	public void SetFade(FadeToBlackTypes _FadeType, string _Text, bool _Tips)
	{
		if (this.FadeTween != null)
		{
			this.FadeTween.Kill(false);
		}
		this.CurrentFadeType = _FadeType;
		if (_Tips)
		{
			this.PartialFadeTips.text = GuideManager.GetTip(48);
			this.FullFadeTips.text = GuideManager.GetTip(48);
		}
		this.PartialFadeTips.gameObject.SetActive(_Tips && _FadeType == FadeToBlackTypes.Partial);
		this.FullFadeTips.gameObject.SetActive(_Tips && _FadeType == FadeToBlackTypes.Full);
		if (_FadeType != FadeToBlackTypes.None)
		{
			this.PartialFade.SetActive(_FadeType != FadeToBlackTypes.Full);
			this.FullFade.SetActive(_FadeType != FadeToBlackTypes.Partial);
		}
		if (_FadeType == FadeToBlackTypes.None)
		{
			this.FadeTween = this.FadeGroup.DOFade(0f, this.FadeDuration).SetEase(this.FadeEase).OnKill(delegate
			{
				this.FadeTween = null;
			});
		}
		else
		{
			this.FadeTween = this.FadeGroup.DOFade(1f, this.FadeDuration).SetEase(this.FadeEase).OnKill(delegate
			{
				this.FadeTween = null;
			});
		}
		if (this.PartialFadeText)
		{
			this.PartialFadeText.text = _Text;
			this.PartialFadeText.gameObject.SetActive(!string.IsNullOrEmpty(_Text));
		}
		if (this.FullFadeText)
		{
			this.FullFadeText.text = _Text;
			this.FullFadeText.gameObject.SetActive(!string.IsNullOrEmpty(_Text));
		}
		this.FadeGroup.blocksRaycasts = (this.CurrentFadeType > FadeToBlackTypes.None);
	}

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	private CanvasGroup FadeGroup;

	// Token: 0x04000690 RID: 1680
	public float FadeDuration;

	// Token: 0x04000691 RID: 1681
	public float FadeStay;

	// Token: 0x04000692 RID: 1682
	[SerializeField]
	private Ease FadeEase;

	// Token: 0x04000693 RID: 1683
	[SerializeField]
	private GameObject PartialFade;

	// Token: 0x04000694 RID: 1684
	[SerializeField]
	private Transform PartialTimeSpentPos;

	// Token: 0x04000695 RID: 1685
	[SerializeField]
	private TextMeshProUGUI PartialFadeText;

	// Token: 0x04000696 RID: 1686
	[SerializeField]
	private TextMeshProUGUI PartialFadeTips;

	// Token: 0x04000697 RID: 1687
	[SerializeField]
	private GameObject FullFade;

	// Token: 0x04000698 RID: 1688
	[SerializeField]
	private Transform FullTimeSpentPos;

	// Token: 0x04000699 RID: 1689
	[SerializeField]
	private TextMeshProUGUI FullFadeText;

	// Token: 0x0400069A RID: 1690
	[SerializeField]
	private TextMeshProUGUI FullFadeTips;

	// Token: 0x0400069C RID: 1692
	private Tween FadeTween;
}
