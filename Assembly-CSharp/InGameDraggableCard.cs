using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000026 RID: 38
public class InGameDraggableCard : InGameCardBase
{
	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000217 RID: 535 RVA: 0x0001538E File Offset: 0x0001358E
	// (set) Token: 0x06000218 RID: 536 RVA: 0x00015396 File Offset: 0x00013596
	public bool Dragged { get; private set; }

	// Token: 0x06000219 RID: 537 RVA: 0x0001539F File Offset: 0x0001359F
	protected override bool CheckIfVisibleOnScreen()
	{
		return this.Dragged || base.CheckIfVisibleOnScreen();
	}

	// Token: 0x0600021A RID: 538 RVA: 0x000153B4 File Offset: 0x000135B4
	protected override void LateUpdate()
	{
		if (this.CardVisuals && this.Dragged)
		{
			if (GameManager.DraggedCard != this)
			{
				this.CardVisuals.RefreshPileInfo(0, 0);
			}
			else
			{
				this.CardVisuals.RefreshPileInfo(GameManager.DraggedCardsCount, GameManager.DraggedCardsCount);
			}
		}
		if (!MobilePlatformDetection.IsMobilePlatform && this.Dragged && !Input.GetMouseButton(0))
		{
			this.OnEndDrag(null);
		}
		base.LateUpdate();
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0001542B File Offset: 0x0001362B
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.Dragged)
		{
			this.OnEndDrag(null);
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x00015442 File Offset: 0x00013642
	protected override void SetDestroyedOn()
	{
		base.SetDestroyedOn();
		if (this.Dragged)
		{
			this.OnEndDrag(null);
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00015459 File Offset: 0x00013659
	private void OnApplicationPause(bool pause)
	{
		if (MobilePlatformDetection.IsMobilePlatform && pause && this.Dragged)
		{
			this.OnEndDrag(null);
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600021E RID: 542 RVA: 0x00015474 File Offset: 0x00013674
	public bool CanBeDragged
	{
		get
		{
			return (!base.CurrentSlot || base.CurrentSlot.SlotType != SlotsTypes.Exploration) && (!MBSingleton<GraphicsManager>.Instance || (!(MBSingleton<GraphicsManager>.Instance.InspectedCard == this) && !base.Destroyed));
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x000154C8 File Offset: 0x000136C8
	public void OnBeginDrag(PointerEventData _Pointer)
	{
		if (_Pointer != null && _Pointer.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (!this.CanBeDragged || CardSwapButton.ContinuousTransfering)
		{
			return;
		}
		if (_Pointer != null)
		{
			GameManager.BeginDragItem(this);
		}
		base.BlocksRaycasts = false;
		this.Dragged = true;
		Vector3 a;
		if (_Pointer != null && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.GM.DraggingPlane, _Pointer.position, _Pointer.pressEventCamera, out a))
		{
			this.GM.DraggingTr.position = a + this.GM.DraggedCardOffset;
		}
		if (_Pointer != null)
		{
			this.SetParent(this.GM.DraggingTr, _Pointer != null);
		}
		else
		{
			this.SetParent(this.GM.CardStackDraggingTr, false);
		}
		this.ForceActive = true;
		base.UpdateActiveState();
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0001558C File Offset: 0x0001378C
	public void OnDrag(PointerEventData _Pointer)
	{
		if (!this.CanBeDragged || !this.Dragged)
		{
			return;
		}
		Vector3 a;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.GM.DraggingPlane, _Pointer.position, _Pointer.pressEventCamera, out a))
		{
			this.GM.DraggingTr.position = a + this.GM.DraggedCardOffset;
		}
	}

	// Token: 0x06000221 RID: 545 RVA: 0x000155F0 File Offset: 0x000137F0
	public void OnEndDrag(PointerEventData _Pointer)
	{
		if (!this.CanBeDragged || !this.Dragged)
		{
			return;
		}
		this.Dragged = false;
		this.ForceActive = false;
		base.UpdateActiveState();
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		this.GM.StartCoroutine(this.EndCardDrag());
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0001564C File Offset: 0x0001384C
	private IEnumerator EndCardDrag()
	{
		yield return null;
		if (GameManager.DraggedCard == this)
		{
			GameManager.EndDragItem(this);
		}
		base.BlocksRaycasts = true;
		if (base.CurrentSlot)
		{
			this.SetParent(base.CurrentSlot.GetParent, false);
			base.CurrentSlot.SortCardPile();
		}
		yield break;
	}

	// Token: 0x0400020F RID: 527
	private const float DragExtraMoveSpeed = 40f;
}
