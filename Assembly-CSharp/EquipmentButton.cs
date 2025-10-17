using System;
using UnityEngine.EventSystems;

// Token: 0x0200007B RID: 123
public class EquipmentButton : TooltipProvider, IDropHandler, IEventSystemHandler
{
	// Token: 0x060004E9 RID: 1257 RVA: 0x00032610 File Offset: 0x00030810
	private void Start()
	{
		this.GrM = MBSingleton<GraphicsManager>.Instance;
		GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.OnBeginCardDrag));
		GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.OnEndDragCard));
		this.DefaultTooltip();
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00032670 File Offset: 0x00030870
	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Remove(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.OnBeginCardDrag));
		GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Remove(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.OnEndDragCard));
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x000326C3 File Offset: 0x000308C3
	private void OnBeginCardDrag(InGameDraggableCard _Card)
	{
		if (string.IsNullOrEmpty(this.GrM.CharacterWindow.ReasonForNotEquipping(_Card.CardModel, _Card)))
		{
			base.SetTooltip(LocalizedString.Equip(_Card), null, null, 0);
			return;
		}
		this.DefaultTooltip();
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x000326F9 File Offset: 0x000308F9
	private void OnEndDragCard(InGameDraggableCard _Card)
	{
		this.DefaultTooltip();
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00032701 File Offset: 0x00030901
	private void DefaultTooltip()
	{
		base.SetTooltip(LocalizedString.Equipment, null, null, 0);
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00032716 File Offset: 0x00030916
	private void Update()
	{
		if (!this.GrM)
		{
			this.GrM = MBSingleton<GraphicsManager>.Instance;
		}
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00032730 File Offset: 0x00030930
	public void OnDrop(PointerEventData _Pointer)
	{
		if (!GameManager.DraggedCard)
		{
			return;
		}
		this.GrM.MoveCardToSlot(GameManager.DraggedCard, new SlotInfo(SlotsTypes.Equipment, 1000), true, false);
	}

	// Token: 0x0400064B RID: 1611
	private GraphicsManager GrM;
}
