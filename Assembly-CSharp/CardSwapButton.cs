using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000050 RID: 80
public class CardSwapButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000365 RID: 869 RVA: 0x000249F8 File Offset: 0x00022BF8
	public static bool ContinuousTransfering
	{
		get
		{
			return CardSwapButton.ContinuousTransferButton != null;
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00024A05 File Offset: 0x00022C05
	private void Awake()
	{
		this.GrM = MBSingleton<GraphicsManager>.Instance;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x00024A12 File Offset: 0x00022C12
	private void OnDisable()
	{
		this.IsHovered = false;
		this.CancelHold();
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00024A21 File Offset: 0x00022C21
	public void SetRaycastTarget(bool _Value)
	{
		if (this.ColliderObject)
		{
			this.ColliderObject.raycastTarget = _Value;
		}
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00024A3C File Offset: 0x00022C3C
	public void OnPointerEnter(PointerEventData _Pointer)
	{
		if (CardSwapButton.ContinuousTransfering)
		{
			return;
		}
		this.IsHovered = true;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00024A4D File Offset: 0x00022C4D
	public void OnPointerExit(PointerEventData _Pointer)
	{
		this.CancelHold();
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00024A55 File Offset: 0x00022C55
	private void CancelHold()
	{
		this.IsHeld = false;
		this.HeldTimer = 0f;
		this.FlowTimer = 0f;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00024A74 File Offset: 0x00022C74
	public void OnPointerDown(PointerEventData _Pointer)
	{
		if (!this.ValidSwapConditions())
		{
			return;
		}
		this.IsHeld = true;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00024A88 File Offset: 0x00022C88
	public void OnPointerUp(PointerEventData _Pointer)
	{
		if (_Pointer.dragging)
		{
			if (this.HeldTimer < this.HoldDuration || !this.IsHovered)
			{
				this.CancelHold();
			}
			return;
		}
		if (CardSwapButton.ContinuousTransferButton == this)
		{
			CloseOnClickOutside.CancelFlag = 1;
			CardSwapButton.ContinuousTransferButton = null;
		}
		if (!this.IsHeld)
		{
			return;
		}
		if (this.HeldTimer < this.HoldDuration && this.AssociatedSlot && this.IsHovered && this.AssociatedSlot.ParentSlotData && this.AssociatedSlot.ParentSlotData.AssignedCard)
		{
			this.AssociatedSlot.ParentSlotData.AssignedCard.SwapCard();
			CloseOnClickOutside.CancelFlag = 1;
		}
		this.CancelHold();
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00024B49 File Offset: 0x00022D49
	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			this.OnPointerExit(null);
		}
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00024B58 File Offset: 0x00022D58
	private void Update()
	{
		this.UpdateFeedbackObjects();
		this.UpdateCollider();
		if (!Input.GetMouseButton(0) && CardSwapButton.ContinuousTransferButton && (CardSwapButton.ContinuousTransferButton == this || !CardSwapButton.ContinuousTransferButton.gameObject.activeInHierarchy))
		{
			this.CancelHold();
			CloseOnClickOutside.CancelFlag = 1;
			CardSwapButton.ContinuousTransferButton = null;
		}
		if (!this.ValidSwapConditions())
		{
			this.CancelHold();
			return;
		}
		if (this.IsHeld)
		{
			this.HeldTimer += Time.deltaTime;
			if (this.HoldFeedback)
			{
				this.HoldFeedback.fillAmount = this.HeldTimer / this.HoldDuration;
			}
			if (this.HeldTimer < this.HoldDuration)
			{
				return;
			}
			CardSwapButton.ContinuousTransferButton = this;
			if (this.AssociatedSlot.ParentSlotData && this.AssociatedSlot.ParentSlotData.AssignedCard)
			{
				if (this.FlowTimer <= 0f)
				{
					this.AssociatedSlot.ParentSlotData.AssignedCard.SwapCard();
					this.FlowTimer += 1f / this.CardsPerSec;
				}
				this.FlowTimer -= Time.deltaTime;
			}
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00024C94 File Offset: 0x00022E94
	private bool ValidSwapConditions()
	{
		if (!this.AssociatedSlot)
		{
			return false;
		}
		if (!this.AssociatedSlot.ParentSlotData)
		{
			return false;
		}
		if (!this.AssociatedSlot.ParentSlotData.AssignedCard)
		{
			return false;
		}
		if (this.AssociatedSlot.ParentSlotData.AssignedCard == this.GrM.InspectedCard || this.AssociatedSlot.ParentSlotData.AssignedCard.IsPinned || this.AssociatedSlot.ParentSlotData.AssignedCard.CardModel.CardType == CardTypes.Base)
		{
			return false;
		}
		bool result = false;
		if (this.AssociatedSlot.SlotType == SlotsTypes.Item)
		{
			if (this.GrM.InspectedCard && this.GrM.CurrentInspectionPopup)
			{
				if (this.GrM.InspectedCard.CardModel.CardType != CardTypes.Explorable)
				{
					result = true;
				}
			}
			else if (this.GrM.CharacterWindow.gameObject.activeInHierarchy)
			{
				result = true;
			}
			else if (!this.GrM.HasPopup)
			{
				result = true;
			}
		}
		else if (this.AssociatedSlot.SlotType == SlotsTypes.Inventory || this.AssociatedSlot.SlotType == SlotsTypes.Base || this.AssociatedSlot.SlotType == SlotsTypes.Equipment)
		{
			if (this.AssociatedSlot.SlotType != SlotsTypes.Equipment)
			{
				result = true;
			}
			else if (this.AssociatedSlot.ParentSlotData.AssignedCard && !this.AssociatedSlot.ParentSlotData.AssignedCard.CardModel.IsMandatoryEquipment)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00024E2C File Offset: 0x0002302C
	private void UpdateCollider()
	{
		if (!this.ColliderObject)
		{
			return;
		}
		this.ColliderObject.enabled = this.ValidSwapConditions();
	}

	// Token: 0x06000372 RID: 882 RVA: 0x00024E50 File Offset: 0x00023050
	private void UpdateFeedbackObjects()
	{
		if (this.HoldFeedback)
		{
			this.HoldFeedback.fillAmount = (this.IsHeld ? (this.HeldTimer / this.HoldDuration) : 0f);
		}
		if (!this.FeedbackObjects)
		{
			return;
		}
		if (!this.ValidSwapConditions())
		{
			this.FeedbackObjects.SetActive(false);
			return;
		}
		this.FeedbackObjects.SetActive(this.IsHeld && this.HeldTimer >= Time.fixedDeltaTime * 2f);
	}

	// Token: 0x0400045A RID: 1114
	[SerializeField]
	private float HoldDuration;

	// Token: 0x0400045B RID: 1115
	[SerializeField]
	private float CardsPerSec;

	// Token: 0x0400045C RID: 1116
	[SerializeField]
	private CardSlot AssociatedSlot;

	// Token: 0x0400045D RID: 1117
	[SerializeField]
	private NonDrawingGraphic ColliderObject;

	// Token: 0x0400045E RID: 1118
	[SerializeField]
	private GameObject FeedbackObjects;

	// Token: 0x0400045F RID: 1119
	[SerializeField]
	private Image HoldFeedback;

	// Token: 0x04000460 RID: 1120
	private bool IsHovered;

	// Token: 0x04000461 RID: 1121
	private bool IsHeld;

	// Token: 0x04000462 RID: 1122
	private float HeldTimer;

	// Token: 0x04000463 RID: 1123
	private float FlowTimer;

	// Token: 0x04000464 RID: 1124
	private static CardSwapButton ContinuousTransferButton;

	// Token: 0x04000465 RID: 1125
	private GraphicsManager GrM;
}
