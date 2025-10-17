using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200004A RID: 74
public class CardFilterGroupButton : TooltipProvider
{
	// Token: 0x0600030F RID: 783 RVA: 0x0001F2AC File Offset: 0x0001D4AC
	private void Awake()
	{
		this.Active = false;
		GraphicsManager.SetActiveGroup(this.FilterActive, this.Active);
		GraphicsManager.SetActiveGroup(this.FilterInActive, !this.Active);
		GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.OnDragBegin));
		GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.OnDragEnd));
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0001F328 File Offset: 0x0001D528
	public void Setup(CardFilterGroup _Group)
	{
		this.AssociatedGroup = _Group;
		if (_Group)
		{
			this.GroupIcon.overrideSprite = _Group.FilterIcon;
			base.SetTooltip(_Group.GameName, "", "", 0);
			return;
		}
		this.GroupIcon.overrideSprite = null;
		base.SetTooltip(LocalizedString.ClearCardFilters, "", "", 0);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0001F39C File Offset: 0x0001D59C
	public void Click()
	{
		if (!MBSingleton<GraphicsManager>.Instance)
		{
			return;
		}
		if (!this.AssociatedGroup)
		{
			MBSingleton<GraphicsManager>.Instance.ClearFilterTags();
			return;
		}
		this.Active = !this.Active;
		MBSingleton<GraphicsManager>.Instance.ToggleFilterTag(this.AssociatedGroup);
		GraphicsManager.SetActiveGroup(this.FilterActive, this.Active);
		GraphicsManager.SetActiveGroup(this.FilterInActive, !this.Active);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0001F412 File Offset: 0x0001D612
	private void OnDragBegin(InGameDraggableCard _Card)
	{
		if (this.Group)
		{
			this.Group.blocksRaycasts = false;
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0001F42D File Offset: 0x0001D62D
	private void OnDragEnd(InGameDraggableCard _Card)
	{
		if (this.Group)
		{
			this.Group.blocksRaycasts = true;
		}
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0001F448 File Offset: 0x0001D648
	private void LateUpdate()
	{
		if (!MBSingleton<GraphicsManager>.Instance || !this.AssociatedGroup)
		{
			return;
		}
		this.Active = MBSingleton<GraphicsManager>.Instance.CurrentFilterTags.Contains(this.AssociatedGroup);
		GraphicsManager.SetActiveGroup(this.FilterActive, this.Active);
		GraphicsManager.SetActiveGroup(this.FilterInActive, !this.Active);
	}

	// Token: 0x040003AE RID: 942
	private CardFilterGroup AssociatedGroup;

	// Token: 0x040003AF RID: 943
	[SerializeField]
	private Image GroupIcon;

	// Token: 0x040003B0 RID: 944
	[SerializeField]
	private GameObject[] FilterActive;

	// Token: 0x040003B1 RID: 945
	[SerializeField]
	private GameObject[] FilterInActive;

	// Token: 0x040003B2 RID: 946
	[SerializeField]
	private CanvasGroup Group;

	// Token: 0x040003B3 RID: 947
	private bool Active;
}
