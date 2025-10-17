using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A8 RID: 168
public class StatStatusGraphics : TooltipProvider
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060006CA RID: 1738 RVA: 0x000466DB File Offset: 0x000448DB
	// (set) Token: 0x060006CB RID: 1739 RVA: 0x000466E3 File Offset: 0x000448E3
	public RectTransform StatusAlertTextParent { get; private set; }

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060006CC RID: 1740 RVA: 0x000466EC File Offset: 0x000448EC
	// (set) Token: 0x060006CD RID: 1741 RVA: 0x000466F4 File Offset: 0x000448F4
	public bool IsInAlertState { get; private set; }

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060006CE RID: 1742 RVA: 0x000466FD File Offset: 0x000448FD
	public static Color DefaultBarColor
	{
		get
		{
			return new Color(0.8f, 0.8f, 0.8f, 1f);
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060006CF RID: 1743 RVA: 0x00046718 File Offset: 0x00044918
	private static Color DefaultHighlightColor
	{
		get
		{
			return Color.yellow;
		}
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x00046720 File Offset: 0x00044920
	private void Awake()
	{
		if (this.StatusOutline)
		{
			this.StatusOutlineScale = this.StatusOutline.effectDistance;
		}
		if (this.ValueBar)
		{
			this.BarsParent = (this.ValueBar.transform.parent as RectTransform);
			this.ValueBarRect = this.ValueBar.GetComponent<RectTransform>();
		}
		if (this.ChangeBar)
		{
			this.ChangeBarRect = this.ChangeBar.GetComponent<RectTransform>();
		}
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x000467A2 File Offset: 0x000449A2
	private void OnEnable()
	{
		if (this.ModelStatus == null)
		{
			return;
		}
		base.SetTooltip(this.ModelStatus.GameName, this.ModelStatus.Description, "", 0);
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x000467DC File Offset: 0x000449DC
	public void Setup(StatStatus _Model, RectTransform _TextParent)
	{
		this.ModelStatus = _Model;
		base.transform.localScale = Vector3.one;
		this.IsInAlertState = (_Model.AlertLevel > AlertLevels.None);
		this.AlertSettings = GraphicsManager.GetStatusAlert(_Model.AlertLevel);
		if (this.AlertIcon)
		{
			if (!this.AlertSettings.AlertIcon)
			{
				this.AlertIcon.enabled = false;
			}
			else
			{
				this.AlertIcon.enabled = true;
				this.AlertIcon.sprite = this.AlertSettings.AlertIcon;
			}
		}
		if (this.StatusOutline)
		{
			this.StatusOutline.effectDistance = this.StatusOutlineScale;
			this.StatusOutline.effectColor = this.AlertSettings.OutlineColor;
		}
		if (this.PulseTr)
		{
			this.PulseTr.transform.localScale = Vector3.one;
		}
		this.StatusIcon.sprite = _Model.Icon;
		base.SetTooltip(_Model.GameName, _Model.Description, "", 0);
		base.gameObject.SetActive(true);
		if (this.OnAppear)
		{
			base.StartCoroutine(this.OnAppear.PlayFeedback());
		}
		this.StatusAlertTextParent = _TextParent;
		if (!GameManager.DontRenameGOs)
		{
			base.name = this.ModelStatus.GameName + " (" + this.ModelStatus.PriorityScore.ToString() + ")";
		}
		if (this.ElementsToColor != null && this.ModelStatus.ParentStat != null)
		{
			if (this.ValueBar)
			{
				this.ValueBar.color = this.ModelStatus.ParentStat.StatModel.GetBarColor;
			}
			for (int i = 0; i < this.ElementsToColor.Length; i++)
			{
				this.ElementsToColor[i].color = this.ModelStatus.ParentStat.StatModel.GetBarColor;
			}
		}
		if (this.TrendIndicator)
		{
			this.TrendIndicator.Setup(this.ModelStatus.ParentStat);
		}
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00046A0C File Offset: 0x00044C0C
	private void Update()
	{
		if (this.ModelStatus == null)
		{
			return;
		}
		if (!this.ModelStatus.ParentStat)
		{
			return;
		}
		if (this.IsHiding)
		{
			return;
		}
		if (this.PinIcon)
		{
			this.PinIcon.SetActive(this.ModelStatus.ParentStat.IsPinned);
		}
		if (this.ValueBar && this.ChangeBar)
		{
			float num = this.ModelStatus.ParentStat.NormalizedVisibleValue - this.ModelStatus.ParentStat.NormalizedAnimatedValue;
			if (num > 0f)
			{
				this.ChangeBar.color = ((this.ModelStatus.ParentStat.StatModel.BarHighlightColor == Color.clear) ? StatStatusGraphics.DefaultHighlightColor : this.ModelStatus.ParentStat.StatModel.BarHighlightColor);
				this.ValueBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ModelStatus.ParentStat.NormalizedAnimatedValue * this.BarsParent.rect.width);
			}
			else
			{
				this.ChangeBar.color = MBSingleton<GraphicsManager>.Instance.NegativeChangeStatusBarColor;
				this.ValueBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ModelStatus.ParentStat.NormalizedVisibleValue * this.BarsParent.rect.width);
			}
			this.ChangeBarRect.localPosition = new Vector3(this.ValueBarRect.rect.xMax, 0f, 0f);
			this.ChangeBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Abs(num) * this.BarsParent.rect.width);
		}
		if (this.TrendIndicator)
		{
			this.TrendIndicator.UpdateAnim();
		}
		if (this.OnAppear.IsPlaying)
		{
			return;
		}
		if (this.StatusOutline)
		{
			this.StatusOutline.effectDistance = Vector2.Lerp(this.StatusOutlineScale, Vector2.zero, GraphicsManager.GetPulseFreq(this.ModelStatus.AlertLevel, true));
			this.StatusOutline.effectColor = Color.Lerp(this.AlertSettings.OutlineColor, this.AlertSettings.OutlineBlinkingColor, GraphicsManager.GetBlinkFreq(this.ModelStatus.AlertLevel));
		}
		if (this.PulseTr)
		{
			this.PulseTr.localScale = Vector3.Lerp(Vector3.one, Vector3.one * this.PulseSize, GraphicsManager.GetPulseFreq(this.ModelStatus.AlertLevel, false));
		}
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00046C9C File Offset: 0x00044E9C
	private void LateUpdate()
	{
		if (this.IsHiding)
		{
			return;
		}
		if (this.IsTemporary)
		{
			this.TemporaryStayTimer += Time.deltaTime;
			if (this.TemporaryStayTimer >= this.TemporaryStayDuration)
			{
				if (base.transform.parent != MBSingleton<GraphicsManager>.Instance.TemporaryGraphicsMovingParent)
				{
					UIReorderableListElement component = base.GetComponent<UIReorderableListElement>();
					if (component)
					{
						component.IgnoreList = true;
					}
					base.transform.SetParent(MBSingleton<GraphicsManager>.Instance.TemporaryGraphicsMovingParent);
				}
				if (!this.TemporaryMoveLocation)
				{
					this.FindTemporaryMoveLocation();
				}
				else if (!this.TemporaryMoveLocation.gameObject.activeInHierarchy)
				{
					this.FindTemporaryMoveLocation();
				}
				if (!this.TemporaryMoveLocation)
				{
					if (this.TemporaryScaleDownSpeed <= 0f)
					{
						this.Hide();
						return;
					}
					base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 0f, 1f), this.TemporaryScaleDownSpeed * Time.deltaTime);
					if (base.transform.localScale.y <= 0.01f)
					{
						this.Hide();
						return;
					}
				}
				else
				{
					base.transform.position = Vector3.Lerp(base.transform.position, this.TemporaryMoveLocation.position, this.MoveLerpSpeed * Time.deltaTime);
					if (Vector3.SqrMagnitude(base.transform.position - this.TemporaryMoveLocation.position) <= 0.001f)
					{
						if (this.TemporaryScaleDownSpeed <= 0f)
						{
							this.Hide();
							return;
						}
						base.transform.position = this.TemporaryMoveLocation.position;
						base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 0f, 1f), this.TemporaryScaleDownSpeed * Time.deltaTime);
						if (base.transform.localScale.y <= 0.01f)
						{
							this.Hide();
							return;
						}
					}
					else
					{
						base.transform.localScale = Vector3.one;
					}
				}
			}
		}
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x00046EC0 File Offset: 0x000450C0
	public void ActivateTemporaryTimer()
	{
		this.IsTemporary = true;
		this.TemporaryStayTimer = 0f;
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00046ED4 File Offset: 0x000450D4
	private void FindTemporaryMoveLocation()
	{
		if (this.ModelStatus == null)
		{
			return;
		}
		if (!this.ModelStatus.ParentStat)
		{
			return;
		}
		StatStatus statStatus = this.ModelStatus.ParentStat.AnyCurrentStatus(true);
		if (statStatus == null)
		{
			return;
		}
		this.TemporaryMoveLocation = statStatus.StatusGraphics.transform;
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x00046F24 File Offset: 0x00045124
	public void Hide()
	{
		if (!base.gameObject.activeInHierarchy || this.IsHiding)
		{
			return;
		}
		this.IsHiding = true;
		UIFeedback onAppear = this.OnAppear;
		if (onAppear != null)
		{
			onAppear.StopPlaying();
		}
		base.transform.localScale = Vector3.one;
		base.StartCoroutine(this.DisappearRoutine());
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x00046F7C File Offset: 0x0004517C
	private void ResetAlertStatus()
	{
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x00046F7C File Offset: 0x0004517C
	public void ResetSize()
	{
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x00046F8E File Offset: 0x0004518E
	private IEnumerator DisappearRoutine()
	{
		if (this.OnDisappear)
		{
			yield return base.StartCoroutine(this.OnDisappear.PlayFeedback());
		}
		UnityEngine.Object.Destroy(base.gameObject);
		if (this.StatusAlertTextParent)
		{
			UnityEngine.Object.Destroy(this.StatusAlertTextParent.gameObject);
		}
		GraphicsManager.RemoveStatusGraphics(this, this.ValueBar == null);
		yield break;
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00046F9D File Offset: 0x0004519D
	public void OnClick()
	{
		if (this.ModelStatus != null)
		{
			CloseOnClickOutside.CancelFlag = 2;
			MBSingleton<GraphicsManager>.Instance.InspectStat(this.ModelStatus.ParentStat);
		}
	}

	// Token: 0x0400097A RID: 2426
	[SerializeField]
	private Image StatusIcon;

	// Token: 0x0400097B RID: 2427
	[SerializeField]
	private Outline StatusOutline;

	// Token: 0x0400097C RID: 2428
	[SerializeField]
	private Image AlertIcon;

	// Token: 0x0400097D RID: 2429
	[SerializeField]
	private RectTransform PulseTr;

	// Token: 0x0400097E RID: 2430
	[SerializeField]
	private float PulseSize;

	// Token: 0x0400097F RID: 2431
	[SerializeField]
	private Image[] ElementsToColor;

	// Token: 0x04000980 RID: 2432
	[SerializeField]
	private Image ValueBar;

	// Token: 0x04000981 RID: 2433
	[SerializeField]
	private Image ChangeBar;

	// Token: 0x04000982 RID: 2434
	[SerializeField]
	private StatTrendIndicator TrendIndicator;

	// Token: 0x04000983 RID: 2435
	[SerializeField]
	private float MoveLerpSpeed;

	// Token: 0x04000984 RID: 2436
	[SerializeField]
	private float TemporaryScaleDownSpeed;

	// Token: 0x04000985 RID: 2437
	[SerializeField]
	private float TemporaryStayDuration;

	// Token: 0x04000986 RID: 2438
	[SerializeField]
	private GameObject PinIcon;

	// Token: 0x04000987 RID: 2439
	[SerializeField]
	private UIFeedback OnAppear;

	// Token: 0x04000988 RID: 2440
	[SerializeField]
	private UIFeedback OnDisappear;

	// Token: 0x04000989 RID: 2441
	private Vector2 StatusOutlineScale;

	// Token: 0x0400098A RID: 2442
	private StatusAlertSettings AlertSettings;

	// Token: 0x0400098B RID: 2443
	private RectTransform ValueBarRect;

	// Token: 0x0400098C RID: 2444
	private RectTransform ChangeBarRect;

	// Token: 0x0400098D RID: 2445
	private RectTransform BarsParent;

	// Token: 0x0400098E RID: 2446
	private float TemporaryStayTimer;

	// Token: 0x0400098F RID: 2447
	private bool IsTemporary;

	// Token: 0x04000990 RID: 2448
	private Transform TemporaryMoveLocation;

	// Token: 0x04000991 RID: 2449
	private bool IsHiding;

	// Token: 0x04000992 RID: 2450
	public ParticleSystemForceField[] ForceFields;

	// Token: 0x04000994 RID: 2452
	[NonSerialized]
	public StatStatus ModelStatus;
}
