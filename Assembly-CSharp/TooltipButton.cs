using System;
using TMPro;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class TooltipButton : TooltipProvider
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x000633B7 File Offset: 0x000615B7
	public Transform OtherIconParent
	{
		get
		{
			if (this.OtherIconPos)
			{
				return this.OtherIconPos;
			}
			return base.transform;
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x000633D3 File Offset: 0x000615D3
	// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x000633EF File Offset: 0x000615EF
	public bool Interactable
	{
		get
		{
			return !this.Group || this.Group.interactable;
		}
		set
		{
			if (this.Group)
			{
				this.Group.interactable = value;
			}
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0006340A File Offset: 0x0006160A
	// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0006342A File Offset: 0x0006162A
	public string Text
	{
		get
		{
			if (!this.ButtonText)
			{
				return "";
			}
			return this.ButtonText.text;
		}
		set
		{
			if (this.ButtonText)
			{
				this.ButtonText.text = value;
			}
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00063445 File Offset: 0x00061645
	// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0006344D File Offset: 0x0006164D
	public string ButtonTooltip
	{
		get
		{
			return base.Content;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				base.CancelTooltip();
				return;
			}
			base.SetTooltip("", value, null, 0);
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0006346C File Offset: 0x0006166C
	// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x00063474 File Offset: 0x00061674
	public bool Highlighted
	{
		get
		{
			return this.HighlightedFlag;
		}
		set
		{
			this.HighlightedFlag = value;
			GraphicsManager.SetActiveGroup(this.HighlightedObjects, value);
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00063489 File Offset: 0x00061689
	// (set) Token: 0x06000BEB RID: 3051 RVA: 0x00063491 File Offset: 0x00061691
	public bool NewNotification
	{
		get
		{
			return this.NewNotificationFlag;
		}
		set
		{
			this.NewNotificationFlag = value;
			GraphicsManager.SetActiveGroup(this.NewNotificationObjects, value);
		}
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x000634A6 File Offset: 0x000616A6
	public void Setup(string _Text, string _Tooltip, bool _Highlighted)
	{
		this.Text = _Text;
		this.ButtonTooltip = _Tooltip;
		this.Highlighted = _Highlighted;
	}

	// Token: 0x040010EA RID: 4330
	[SerializeField]
	private CanvasGroup Group;

	// Token: 0x040010EB RID: 4331
	[SerializeField]
	protected TextMeshProUGUI ButtonText;

	// Token: 0x040010EC RID: 4332
	[SerializeField]
	private GameObject[] HighlightedObjects;

	// Token: 0x040010ED RID: 4333
	[SerializeField]
	private GameObject[] NewNotificationObjects;

	// Token: 0x040010EE RID: 4334
	[SerializeField]
	private Transform OtherIconPos;

	// Token: 0x040010EF RID: 4335
	private bool HighlightedFlag;

	// Token: 0x040010F0 RID: 4336
	private bool NewNotificationFlag;
}
