using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000AD RID: 173
public class TooltipProvider : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060006F3 RID: 1779 RVA: 0x00047B68 File Offset: 0x00045D68
	public string Title
	{
		get
		{
			if (this.MyTooltip == null)
			{
				return "";
			}
			return this.MyTooltip.TooltipTitle;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00047B83 File Offset: 0x00045D83
	public string Content
	{
		get
		{
			if (this.MyTooltip == null)
			{
				return "";
			}
			return this.MyTooltip.TooltipContent;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00047B9E File Offset: 0x00045D9E
	// (set) Token: 0x060006F6 RID: 1782 RVA: 0x00047BB9 File Offset: 0x00045DB9
	public float NormalizedHoldTime
	{
		get
		{
			if (this.MyTooltip == null)
			{
				return 0f;
			}
			return this.MyTooltip.NormalizedHoldTime;
		}
		set
		{
			if (this.MyTooltip == null)
			{
				return;
			}
			this.MyTooltip.NormalizedHoldTime = value;
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00047BD0 File Offset: 0x00045DD0
	public void OnPointerEnter(PointerEventData _Pointer)
	{
		this.OnHoverEnter();
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00047BD8 File Offset: 0x00045DD8
	public void OnPointerExit(PointerEventData _Pointer)
	{
		this.OnHoverExit();
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00047BE0 File Offset: 0x00045DE0
	public virtual void OnHoverEnter()
	{
		Tooltip.AddTooltip(this.MyTooltip);
		this.IsHovered = true;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00047BF4 File Offset: 0x00045DF4
	public virtual void OnHoverExit()
	{
		Tooltip.RemoveTooltip(this.MyTooltip);
		this.IsHovered = false;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00047C08 File Offset: 0x00045E08
	protected virtual void OnDisable()
	{
		if (!this.DontClearTooltipOnDisable)
		{
			this.CancelTooltip();
		}
		else
		{
			Tooltip.RemoveTooltip(this.MyTooltip);
		}
		this.IsHovered = false;
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00047C2C File Offset: 0x00045E2C
	protected virtual void OnDestroy()
	{
		this.CancelTooltip();
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00047C34 File Offset: 0x00045E34
	protected void CancelTooltip()
	{
		Tooltip.RemoveTooltip(this.MyTooltip);
		this.MyTooltip = null;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00047C48 File Offset: 0x00045E48
	public void SetTooltip(string _Title, string _Content, string _HoldText, int Priority = 0)
	{
		if (this.MyTooltip == null)
		{
			this.MyTooltip = new TooltipText();
		}
		this.MyTooltip.TooltipTitle = _Title;
		this.MyTooltip.TooltipContent = _Content;
		this.MyTooltip.HoldText = _HoldText;
	}

	// Token: 0x040009C6 RID: 2502
	private TooltipText MyTooltip;

	// Token: 0x040009C7 RID: 2503
	protected bool IsHovered;

	// Token: 0x040009C8 RID: 2504
	public bool DontClearTooltipOnDisable;
}
