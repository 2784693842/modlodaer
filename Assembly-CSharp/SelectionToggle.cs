using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000A2 RID: 162
[RequireComponent(typeof(CanvasGroup))]
public class SelectionToggle : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x17000131 RID: 305
	// (get) Token: 0x06000686 RID: 1670 RVA: 0x00044CEF File Offset: 0x00042EEF
	// (set) Token: 0x06000687 RID: 1671 RVA: 0x00044CF7 File Offset: 0x00042EF7
	public bool IsHovered { get; private set; }

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000688 RID: 1672 RVA: 0x00044D00 File Offset: 0x00042F00
	// (set) Token: 0x06000689 RID: 1673 RVA: 0x00044D20 File Offset: 0x00042F20
	public bool CanBeHovered
	{
		get
		{
			if (!this.Group)
			{
				this.GetGroup();
			}
			return this.Group.blocksRaycasts;
		}
		set
		{
			if (!this.Group)
			{
				this.GetGroup();
			}
			if (this.IsHovered)
			{
				this.OnHoverExit();
			}
			this.Group.blocksRaycasts = value;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x0600068A RID: 1674 RVA: 0x00044D4F File Offset: 0x00042F4F
	// (set) Token: 0x0600068B RID: 1675 RVA: 0x00044D57 File Offset: 0x00042F57
	public bool Active
	{
		get
		{
			return this.IsActive;
		}
		set
		{
			this.IsActive = value;
			if (value)
			{
				GraphicsManager.SetActiveGroup(this.OnInactive, false);
				GraphicsManager.SetActiveGroup(this.OnActive, true);
				return;
			}
			GraphicsManager.SetActiveGroup(this.OnActive, false);
			GraphicsManager.SetActiveGroup(this.OnInactive, true);
		}
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x00044D94 File Offset: 0x00042F94
	private void Awake()
	{
		this.GetGroup();
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00044D9C File Offset: 0x00042F9C
	private void OnEnable()
	{
		if (this.IsHovered)
		{
			this.OnHoverEnter();
			return;
		}
		this.OnHoverExit();
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00044DB3 File Offset: 0x00042FB3
	private void GetGroup()
	{
		this.Group = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00044DC1 File Offset: 0x00042FC1
	public void OnPointerEnter(PointerEventData _Pointer)
	{
		this.OnHoverEnter();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00044DC9 File Offset: 0x00042FC9
	public void OnPointerExit(PointerEventData _Pointer)
	{
		this.OnHoverExit();
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00044DD1 File Offset: 0x00042FD1
	protected virtual void OnHoverEnter()
	{
		GraphicsManager.SetActiveGroup(this.OnHovered, true);
		this.IsHovered = true;
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00044DE6 File Offset: 0x00042FE6
	protected virtual void OnHoverExit()
	{
		GraphicsManager.SetActiveGroup(this.OnHovered, false);
		this.IsHovered = false;
	}

	// Token: 0x04000928 RID: 2344
	[SerializeField]
	private GameObject[] OnHovered;

	// Token: 0x04000929 RID: 2345
	[SerializeField]
	private GameObject[] OnActive;

	// Token: 0x0400092A RID: 2346
	[SerializeField]
	private GameObject[] OnInactive;

	// Token: 0x0400092B RID: 2347
	private CanvasGroup Group;

	// Token: 0x0400092C RID: 2348
	private bool IsActive;
}
