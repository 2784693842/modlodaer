using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000089 RID: 137
public class HoldButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x060005A4 RID: 1444 RVA: 0x0003B92C File Offset: 0x00039B2C
	private void OnDisable()
	{
		this.IsHovered = false;
		this.CancelHold();
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0003B93B File Offset: 0x00039B3B
	private void CancelHold()
	{
		this.IsHeld = false;
		this.HoldTimer = 0f;
		this.HoldActionFired = false;
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0003B956 File Offset: 0x00039B56
	public void OnPointerEnter(PointerEventData _Pointer)
	{
		this.IsHovered = true;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x0003B92C File Offset: 0x00039B2C
	public void OnPointerExit(PointerEventData _Pointer)
	{
		this.IsHovered = false;
		this.CancelHold();
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0003B92C File Offset: 0x00039B2C
	private void OnApplicationFocus(bool focus)
	{
		this.IsHovered = false;
		this.CancelHold();
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x0003B95F File Offset: 0x00039B5F
	public void OnPointerDown(PointerEventData _Pointer)
	{
		this.IsHeld = true;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0003B968 File Offset: 0x00039B68
	public void OnPointerUp(PointerEventData _Pointer)
	{
		if (!this.IsHovered)
		{
			this.CancelHold();
			return;
		}
		if (this.HoldTimer < this.HoldDuration)
		{
			UnityEvent onClick = this.OnClick;
			if (onClick != null)
			{
				onClick.Invoke();
			}
		}
		this.CancelHold();
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0003B9A0 File Offset: 0x00039BA0
	private void Update()
	{
		this.UpdateVisuals();
		if (this.IsHeld)
		{
			if (this.HoldTimer < this.HoldDuration)
			{
				this.HoldTimer += Time.deltaTime;
				return;
			}
			if (!this.HoldActionFired)
			{
				this.HoldActionFired = true;
				UnityEvent onHold = this.OnHold;
				if (onHold == null)
				{
					return;
				}
				onHold.Invoke();
			}
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0003B9FC File Offset: 0x00039BFC
	private void UpdateVisuals()
	{
		if (this.HoverObject)
		{
			this.HoverObject.SetActive(this.IsHovered);
		}
		if (this.HeldObject)
		{
			this.HeldObject.SetActive(this.IsHeld);
		}
		if (this.HeldFeedback)
		{
			this.HeldFeedback.fillAmount = (this.IsHeld ? (this.HoldTimer / this.HoldDuration) : 0f);
		}
	}

	// Token: 0x040007A1 RID: 1953
	[SerializeField]
	private float HoldDuration;

	// Token: 0x040007A2 RID: 1954
	[Space]
	[SerializeField]
	private GameObject HoverObject;

	// Token: 0x040007A3 RID: 1955
	[SerializeField]
	private GameObject HeldObject;

	// Token: 0x040007A4 RID: 1956
	[SerializeField]
	private Image HeldFeedback;

	// Token: 0x040007A5 RID: 1957
	[Space]
	public UnityEvent OnClick;

	// Token: 0x040007A6 RID: 1958
	public UnityEvent OnHold;

	// Token: 0x040007A7 RID: 1959
	private float HoldTimer;

	// Token: 0x040007A8 RID: 1960
	private bool IsHeld;

	// Token: 0x040007A9 RID: 1961
	private bool IsHovered;

	// Token: 0x040007AA RID: 1962
	private bool HoldActionFired;
}
