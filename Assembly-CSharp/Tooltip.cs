using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AB RID: 171
public class Tooltip : MonoBehaviour
{
	// Token: 0x060006E8 RID: 1768 RVA: 0x000473EE File Offset: 0x000455EE
	public static void AddTooltip(TooltipText _Tooltip)
	{
		if (!Tooltip.Instance || _Tooltip == null)
		{
			return;
		}
		if (!Tooltip.Instance.CurrentTooltips.Contains(_Tooltip))
		{
			Tooltip.Instance.Add(_Tooltip);
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0004741D File Offset: 0x0004561D
	public static void RemoveTooltip(TooltipText _Tooltip)
	{
		if (!Tooltip.Instance || _Tooltip == null)
		{
			return;
		}
		if (Tooltip.Instance.CurrentTooltips.Contains(_Tooltip))
		{
			Tooltip.Instance.CurrentTooltips.Remove(_Tooltip);
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00047454 File Offset: 0x00045654
	private void Add(TooltipText _Tooltip)
	{
		this.Pulse();
		if (this.CurrentTooltips.Count == 0)
		{
			this.CurrentTooltips.Add(_Tooltip);
			return;
		}
		for (int i = 0; i < this.CurrentTooltips.Count; i++)
		{
			if (_Tooltip.Priority < this.CurrentTooltips[i].Priority)
			{
				this.CurrentTooltips.Insert(i, _Tooltip);
				return;
			}
			if (i == this.CurrentTooltips.Count - 1)
			{
				this.CurrentTooltips.Add(_Tooltip);
				return;
			}
		}
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x000474DC File Offset: 0x000456DC
	private void Awake()
	{
		if ((this.IsForPhones && !MobilePlatformDetection.IsMobilePlatform) || (!this.IsForPhones && MobilePlatformDetection.IsMobilePlatform))
		{
			if (Tooltip.Instance == this)
			{
				Tooltip.Instance = null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!Tooltip.Instance)
		{
			Tooltip.Instance = this;
		}
		else if (Tooltip.Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.TooltipRect = this.TooltipPopup.GetComponent<RectTransform>();
		this.ParentRect = base.GetComponentInParent<RectTransform>();
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00047570 File Offset: 0x00045770
	private void LateUpdate()
	{
		if (!this.MainCam)
		{
			this.MainCam = Camera.main;
		}
		if (!this.MainCam)
		{
			return;
		}
		this.TooltipCount = this.CurrentTooltips.Count;
		for (int i = this.CurrentTooltips.Count - 1; i >= 0; i--)
		{
			if (this.CurrentTooltips[i] == null)
			{
				this.CurrentTooltips.RemoveAt(i);
			}
		}
		if (this.CurrentTooltips.Count == 0)
		{
			this.TooltipPopup.alpha = 0f;
			return;
		}
		if (!this.CurrentTooltips[this.CurrentTooltips.Count - 1].ShowHoldBar)
		{
			if (this.PulseOnHeld && this.CurrentTooltips[this.CurrentTooltips.Count - 1].NormalizedHoldTime >= 1f && !this.TextObjects.activeSelf)
			{
				this.Pulse();
			}
			this.TextObjects.SetActive(true);
			this.HoldObjects.SetActive(false);
		}
		else
		{
			this.TextObjects.SetActive(false);
			this.HoldObjects.SetActive(true);
		}
		this.TooltipPopup.alpha = 1f;
		this.TooltipTitle.text = this.CurrentTooltips[this.CurrentTooltips.Count - 1].TooltipTitle;
		this.TooltipContent.text = this.CurrentTooltips[this.CurrentTooltips.Count - 1].TooltipContent;
		this.HoldText.text = this.CurrentTooltips[this.CurrentTooltips.Count - 1].HoldText;
		this.HoldTimerFill.fillAmount = this.CurrentTooltips[this.CurrentTooltips.Count - 1].NormalizedHoldTime;
		Vector3 a;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.ParentRect, Input.mousePosition, this.MainCam, out a))
		{
			base.transform.position = a + this.PointerOffset;
		}
		this.WorldRect = new Rect(this.ParentRect.transform.TransformPoint(this.ParentRect.rect.position), this.ParentRect.transform.TransformVector(this.ParentRect.rect.size));
		Vector2 vector = Vector2.zero;
		this.ScreenRectWithMargins = new Rect(this.ScreenRect.TransformPoint(this.ScreenRect.rect.position), this.ScreenRect.TransformVector(this.ScreenRect.rect.size));
		this.ScreenRectWithMargins.width = this.ScreenRectWithMargins.width - this.ScreenMargin * 2f;
		this.ScreenRectWithMargins.height = this.ScreenRectWithMargins.height - this.ScreenMargin * 2f;
		this.ScreenRectWithMargins.x = this.ScreenRectWithMargins.x + this.ScreenMargin;
		this.ScreenRectWithMargins.y = this.ScreenRectWithMargins.y + this.ScreenMargin;
		if (!this.ScreenRectWithMargins.Contains(this.WorldRect.min))
		{
			vector = new Vector2(Mathf.Clamp(this.WorldRect.min.x, this.ScreenRectWithMargins.min.x, this.ScreenRectWithMargins.max.x), Mathf.Clamp(this.WorldRect.min.y, this.ScreenRectWithMargins.min.y, this.ScreenRectWithMargins.max.y)) - this.WorldRect.min;
			this.WorldRect.position = this.WorldRect.position + vector;
			this.ParentRect.position += vector;
		}
		if (!this.ScreenRectWithMargins.Contains(this.WorldRect.max))
		{
			vector = new Vector2(Mathf.Clamp(this.WorldRect.max.x, this.ScreenRectWithMargins.min.x, this.ScreenRectWithMargins.max.x), Mathf.Clamp(this.WorldRect.max.y, this.ScreenRectWithMargins.min.y, this.ScreenRectWithMargins.max.y)) - this.WorldRect.max;
			this.WorldRect.position = this.WorldRect.position + vector;
			this.ParentRect.position += vector;
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00047A50 File Offset: 0x00045C50
	private void Pulse()
	{
		if (!this.PulseOnAppear)
		{
			return;
		}
		if (this.PulseTween != null)
		{
			this.PulseTween.Kill(false);
		}
		this.TooltipRect.localScale = new Vector3(1f, 0f, 1f);
		this.PulseTween = this.TooltipRect.DOScaleY(1f, this.PulseDuration).SetEase(Ease.OutBack).OnKill(delegate
		{
			this.PulseTween = null;
		});
	}

	// Token: 0x040009A9 RID: 2473
	public static Tooltip Instance;

	// Token: 0x040009AA RID: 2474
	public bool IsForPhones;

	// Token: 0x040009AB RID: 2475
	public Vector2 PointerOffset;

	// Token: 0x040009AC RID: 2476
	public RectTransform ScreenRect;

	// Token: 0x040009AD RID: 2477
	public float ScreenMargin = 2.5f;

	// Token: 0x040009AE RID: 2478
	public CanvasGroup TooltipPopup;

	// Token: 0x040009AF RID: 2479
	[Space]
	public GameObject TextObjects;

	// Token: 0x040009B0 RID: 2480
	public TextMeshProUGUI TooltipTitle;

	// Token: 0x040009B1 RID: 2481
	public TextMeshProUGUI TooltipContent;

	// Token: 0x040009B2 RID: 2482
	public TextMeshProUGUI HoldText;

	// Token: 0x040009B3 RID: 2483
	[Space]
	public GameObject HoldObjects;

	// Token: 0x040009B4 RID: 2484
	public Image HoldTimerFill;

	// Token: 0x040009B5 RID: 2485
	[Space]
	public bool PulseOnAppear;

	// Token: 0x040009B6 RID: 2486
	public bool PulseOnHeld;

	// Token: 0x040009B7 RID: 2487
	public float PulseSize;

	// Token: 0x040009B8 RID: 2488
	public float PulseDuration;

	// Token: 0x040009B9 RID: 2489
	private RectTransform TooltipRect;

	// Token: 0x040009BA RID: 2490
	private RectTransform ParentRect;

	// Token: 0x040009BB RID: 2491
	private Rect ScreenRectWithMargins;

	// Token: 0x040009BC RID: 2492
	private Camera MainCam;

	// Token: 0x040009BD RID: 2493
	public int TooltipCount;

	// Token: 0x040009BE RID: 2494
	private Tween PulseTween;

	// Token: 0x040009BF RID: 2495
	private Rect WorldRect;

	// Token: 0x040009C0 RID: 2496
	private List<TooltipText> CurrentTooltips = new List<TooltipText>();
}
