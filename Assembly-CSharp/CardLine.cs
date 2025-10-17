using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200004F RID: 79
[DefaultExecutionOrder(10)]
public class CardLine : DynamicViewLayoutGroup, IDropHandler, IEventSystemHandler
{
	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000349 RID: 841 RVA: 0x0002302B File Offset: 0x0002122B
	// (set) Token: 0x0600034A RID: 842 RVA: 0x00023033 File Offset: 0x00021233
	public List<DynamicLayoutSlot> Slots { get; private set; }

	// Token: 0x0600034B RID: 843 RVA: 0x0002303C File Offset: 0x0002123C
	public void Init(RectTransform _Parent, SlotSettings _Model, ScrollRect _View)
	{
		this.AllElements = new List<DynamicElementRef>();
		this.Manager = MBSingleton<GraphicsManager>.Instance;
		this.Slots = new List<DynamicLayoutSlot>();
		this.SlotModel = _Model;
		this.Pointer = MBSingleton<GameManager>.Instance.DraggingTr;
		if (this.SeparatorPrefab)
		{
			this.Separator = UnityEngine.Object.Instantiate<RectTransform>(this.SeparatorPrefab, _Parent);
			this.Separator.gameObject.SetActive(false);
			this.SeparatorSize = this.Separator.GetComponent<RectTransform>().rect.width;
		}
		if (_View)
		{
			this.ScrollView = _View;
			this.ScrollViewRect = _View.viewport;
		}
		this.CurrentAttemptedAction = new CardLine.LineAction
		{
			ActionType = LineActionTypes.None,
			ActionTime = 0f,
			ActionIndex = -1
		};
		if (this.SlotModel.Visuals)
		{
			this.SlotSize = this.SlotModel.Visuals.GetComponent<RectTransform>().rect.width;
		}
		if (!this.ExplicitLineLayoutGroup && this.ScrollView)
		{
			this.LineLayoutGroup = this.ScrollView.content.GetComponent<HorizontalLayoutGroup>();
		}
		else
		{
			this.LineLayoutGroup = this.ExplicitLineLayoutGroup;
		}
		base.Awake();
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00023190 File Offset: 0x00021390
	public DynamicLayoutSlot AddSlot(int _AtIndex)
	{
		int num = Mathf.Clamp(_AtIndex, 0, this.Slots.Count);
		this.Slots.Insert(num, new DynamicLayoutSlot(this.SlotModel, base.AddElement(num), this));
		this.RefreshSlotIndices(num);
		this.NewSlotsAdded = true;
		return this.Slots[num];
	}

	// Token: 0x0600034D RID: 845 RVA: 0x000231E9 File Offset: 0x000213E9
	public void RemoveSlot(int _AtIndex)
	{
		if (_AtIndex < 0 || _AtIndex >= this.Slots.Count)
		{
			return;
		}
		this.Slots.RemoveAt(_AtIndex);
		base.RemoveElement(_AtIndex);
		this.RefreshSlotIndices(_AtIndex);
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00023218 File Offset: 0x00021418
	public void MoveSlot(int _From, int _To)
	{
		if (this.Slots == null)
		{
			return;
		}
		if (this.Slots.Count == 0)
		{
			return;
		}
		if (_From < 0 || _From >= this.Slots.Count)
		{
			return;
		}
		Debug.LogWarning("Moving slot " + _From.ToString() + " to " + _To.ToString());
		DynamicLayoutSlot item = this.Slots[_From];
		this.Slots.RemoveAt(_From);
		this.Slots.Insert(Mathf.Clamp(_To, 0, this.Slots.Count), item);
		base.MoveElement(_From, _To);
		this.RefreshSlotIndices(Mathf.Min(_From, _To));
	}

	// Token: 0x0600034F RID: 847 RVA: 0x000232C0 File Offset: 0x000214C0
	public void SortSlots(Comparison<DynamicLayoutSlot> _Comparison)
	{
		this.AllElements.Clear();
		this.Slots.Sort(_Comparison);
		for (int i = 0; i < this.Slots.Count; i++)
		{
			this.AllElements.Add(this.Slots[i].DynamicSlotObject);
		}
		this.RecalculateSize = true;
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0002331D File Offset: 0x0002151D
	protected override void OnElementVisible(int _Index)
	{
		base.OnElementVisible(_Index);
		this.Slots[_Index].OnSlotBecameVisible();
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00023337 File Offset: 0x00021537
	protected override void OnElementNotVisible(int _Index)
	{
		base.OnElementNotVisible(_Index);
		this.Slots[_Index].OnSlotBecameNotVisible();
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00023354 File Offset: 0x00021554
	public void RefreshSlotIndices(int _FromIndex)
	{
		if (_FromIndex >= this.Slots.Count || _FromIndex < 0)
		{
			return;
		}
		for (int i = _FromIndex; i < this.Slots.Count; i++)
		{
			this.Slots[i].Index = i;
		}
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0002339C File Offset: 0x0002159C
	public void MoveViewTo(DynamicLayoutSlot _Slot, bool _ForceCenter, bool _Pulse)
	{
		if (!this.ScrollView)
		{
			return;
		}
		if (!this.Slots.Contains(_Slot))
		{
			return;
		}
		Canvas.ForceUpdateCanvases();
		this.Update();
		if (this.NewSlotsAdded)
		{
			base.CalculateSizeAndProperties();
		}
		if (this.WorldRect.Contains(_Slot.WorldPosition) && !_ForceCenter)
		{
			this.ScrollView.DOKill(false);
			return;
		}
		float num = this.MaskRect.rect.width / 2f;
		float b = this.RectTr.rect.width - num;
		float pos = Mathf.InverseLerp(num, b, _Slot.LocalPosition.x);
		if (!_Pulse)
		{
			this.MoveToPos(pos, 0.3f, Ease.InOutSine, null);
			return;
		}
		this.MoveToPos(pos, 0.3f, Ease.InOutSine, delegate()
		{
			InGameCardBase assignedCard = _Slot.AssignedCard;
			if (assignedCard == null)
			{
				return;
			}
			assignedCard.Pulse(0f);
		});
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00023490 File Offset: 0x00021690
	public void MoveViewTo(Transform _Child, bool _ForceCenter, TweenCallback _OnComplete)
	{
		Canvas.ForceUpdateCanvases();
		this.Update();
		if (this.WorldRect.Contains(_Child.position) && !_ForceCenter)
		{
			this.ScrollView.DOKill(false);
			return;
		}
		float num = this.ScrollViewRect.rect.width / 2f;
		float b = this.ScrollView.content.rect.width - num;
		float pos = Mathf.InverseLerp(num, b, this.ScrollView.content.InverseTransformPoint(_Child.position).x);
		this.MoveToPos(pos, 0.3f, Ease.InOutSine, _OnComplete);
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00023532 File Offset: 0x00021732
	public void MoveToNextPos()
	{
		this.MoveToSection(true);
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0002353B File Offset: 0x0002173B
	public void MoveToPrevPos()
	{
		this.MoveToSection(false);
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00023544 File Offset: 0x00021744
	private void MoveToSection(bool _Forward)
	{
		if (!this.ScrollView)
		{
			return;
		}
		int num = 0;
		float num2 = this.ScrollViewRect.rect.width / 2f;
		float b = this.ScrollView.content.rect.width - num2;
		if ((this.ButtonMoveTarget > 0f && !_Forward) || (this.ButtonMoveTarget < 0f && _Forward))
		{
			this.ButtonMoveTarget = 0f;
		}
		if (Mathf.Approximately(this.ButtonMoveTarget, 0f))
		{
			float num3 = Mathf.Lerp(num2, b, this.ScrollView.horizontalNormalizedPosition);
			this.ButtonMoveTarget = (_Forward ? (num3 + this.ScrollViewRect.rect.width) : (num3 - this.ScrollViewRect.rect.width));
		}
		else
		{
			this.ButtonMoveTarget += (_Forward ? this.ScrollViewRect.rect.width : (-this.ScrollViewRect.rect.width));
		}
		float pos;
		if (_Forward)
		{
			for (float num4 = (float)this.LineLayoutGroup.padding.left; num4 < this.ButtonMoveTarget; num4 += this.SlotSize + this.LineLayoutGroup.spacing)
			{
				num++;
			}
			num--;
			pos = Mathf.InverseLerp(num2, b, (float)this.LineLayoutGroup.padding.left + (float)num * (this.SlotSize + this.LineLayoutGroup.spacing));
		}
		else
		{
			for (float num5 = this.ScrollView.content.rect.width - (float)this.LineLayoutGroup.padding.right; num5 > this.ButtonMoveTarget; num5 -= this.SlotSize + this.LineLayoutGroup.spacing)
			{
				num++;
			}
			num--;
			pos = Mathf.InverseLerp(num2, b, this.ScrollView.content.rect.width - (float)this.LineLayoutGroup.padding.right - (float)num * (this.SlotSize + this.LineLayoutGroup.spacing));
		}
		this.MoveToPos(pos, 0.15f, Ease.InOutSine, delegate()
		{
			this.ButtonMoveTarget = 0f;
		});
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00023795 File Offset: 0x00021995
	public void MoveToPos(float _Pos)
	{
		this.MoveToPos(_Pos, 0.3f, Ease.InOutSine, null);
	}

	// Token: 0x06000359 RID: 857 RVA: 0x000237A8 File Offset: 0x000219A8
	public void MoveToPos(float _Pos, float _Time, Ease _Ease, TweenCallback _OnComplete)
	{
		if (!this.ScrollView)
		{
			return;
		}
		this.ScrollView.DOKill(false);
		if (_OnComplete == null)
		{
			this.ScrollView.DOHorizontalNormalizedPos(Mathf.Clamp01(_Pos), _Time, false).SetEase(_Ease);
			return;
		}
		this.ScrollView.DOHorizontalNormalizedPos(Mathf.Clamp01(_Pos), _Time, false).SetEase(_Ease).OnComplete(_OnComplete);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00023810 File Offset: 0x00021A10
	private void Update()
	{
		if (!this.RectTr)
		{
			base.SetRectTr();
		}
		if (this.MaskRect)
		{
			this.WorldRect = this.WorldMaskRect;
			return;
		}
		this.WorldRect = new Rect(base.transform.TransformPoint(this.RectTr.rect.position), base.transform.TransformVector(this.RectTr.rect.size));
	}

	// Token: 0x0600035B RID: 859 RVA: 0x000238A8 File Offset: 0x00021AA8
	private void UpdateActionType(LineActionTypes _NewType, int _NewIndex = -1)
	{
		if (_NewType != this.CurrentAttemptedAction.ActionType || _NewIndex != this.CurrentAttemptedAction.ActionIndex)
		{
			this.CurrentAttemptedAction.ActionType = _NewType;
			this.CurrentAttemptedAction.ActionIndex = _NewIndex;
			this.CurrentAttemptedAction.ActionTime = 0f;
		}
	}

	// Token: 0x0600035C RID: 860 RVA: 0x000238FC File Offset: 0x00021AFC
	protected override void LateUpdate()
	{
		base.LateUpdate();
		this.NewSlotsAdded = false;
		if (this.Slots != null && this.Slots.Count > 0)
		{
			for (int i = 0; i < this.Slots.Count; i++)
			{
				this.Slots[i].LateUpdate();
			}
		}
		if (this.RightButton && this.ScrollView)
		{
			this.RightButton.SetActive(!GameManager.DraggedCard && this.ScrollView.horizontalNormalizedPosition < 0.999f && this.ScrollView.content.rect.width > this.ScrollView.viewport.rect.width);
		}
		if (this.LeftButton && this.ScrollView)
		{
			this.LeftButton.SetActive(!GameManager.DraggedCard && this.ScrollView.horizontalNormalizedPosition > 0.001f && this.ScrollView.content.rect.width > this.ScrollView.viewport.rect.width);
		}
		if (!this.Separator || this.CantMoveSlots || (this.Manager.HasPopup && !this.IsPopup))
		{
			if (this.LeftArrow)
			{
				this.LeftArrow.SetActive(false);
			}
			if (this.RightArrow)
			{
				this.RightArrow.SetActive(false);
			}
			return;
		}
		this.CurrentAttemptedAction.ActionTime = this.CurrentAttemptedAction.ActionTime + Time.deltaTime;
		this.Separator.gameObject.SetActive(false);
		if (this.SeparatorSpace == null)
		{
			this.SeparatorSpace = new DynamicViewExtraSpace(0, 0f);
			base.AddExtraSpace(this.SeparatorSpace);
		}
		else
		{
			this.RecalculateSize |= (this.SeparatorSpace.Space > 0f);
			this.SeparatorSpace.Space = 0f;
		}
		if (this.LeftArrow && this.ScrollView)
		{
			this.LeftArrow.SetActive(this.Pointer.position.y < this.WorldRect.yMax && this.Pointer.position.y > this.WorldRect.yMin && this.ScrollView.content.rect.size.x > this.ScrollViewRect.rect.size.x && GameManager.DraggedCard && this.ScrollView.horizontalNormalizedPosition > 0f);
		}
		if (this.RightArrow && this.ScrollView)
		{
			this.RightArrow.SetActive(this.Pointer.position.y < this.WorldRect.yMax && this.Pointer.position.y > this.WorldRect.yMin && this.ScrollView.content.rect.size.x > this.ScrollViewRect.rect.size.x && GameManager.DraggedCard && this.ScrollView.horizontalNormalizedPosition < 1f);
		}
		if (!GameManager.DraggedCard)
		{
			this.UpdateActionType(LineActionTypes.None, -1);
			return;
		}
		if (this.ScrollView && this.Pointer.position.y < this.WorldRect.yMax && this.Pointer.position.y > this.WorldRect.yMin)
		{
			float num = this.WorldRect.xMax - this.Pointer.position.x;
			float num2 = this.Pointer.position.x - this.WorldRect.xMin;
			float x = base.transform.TransformVector(Vector3.right * this.Manager.DragScrollMargins).x;
			if (this.ScrollView.content.rect.size.x > this.ScrollViewRect.rect.size.x)
			{
				if (num < x && this.ScrollView.horizontalNormalizedPosition < 1f && num > -x * 0.5f)
				{
					this.UpdateActionType(LineActionTypes.Scroll, -1);
					if (this.CurrentAttemptedAction.ActionTime >= this.Manager.GetActionTime(LineActionTypes.Scroll))
					{
						this.ScrollView.velocity = Vector2.left * Mathf.Lerp(this.Manager.MinMaxDragScrollingSpeed.x, this.Manager.MinMaxDragScrollingSpeed.y, 1f - Mathf.Max(0f, num / x));
					}
				}
				else if (num2 < x && this.ScrollView.horizontalNormalizedPosition > 0f && num2 > -x * 0.5f)
				{
					this.UpdateActionType(LineActionTypes.Scroll, 1);
					if (this.CurrentAttemptedAction.ActionTime >= this.Manager.GetActionTime(LineActionTypes.Scroll))
					{
						this.ScrollView.velocity = Vector2.right * Mathf.Lerp(this.Manager.MinMaxDragScrollingSpeed.x, this.Manager.MinMaxDragScrollingSpeed.y, 1f - Mathf.Max(0f, num2 / x));
					}
				}
				else
				{
					this.ScrollView.velocity = Vector2.zero;
					if (this.CurrentAttemptedAction.ActionType == LineActionTypes.Scroll)
					{
						this.UpdateActionType(LineActionTypes.None, -1);
					}
				}
			}
		}
		if (!this.WorldRect.Contains(this.Pointer.position))
		{
			if (this.CurrentAttemptedAction.ActionType == LineActionTypes.Insert)
			{
				this.UpdateActionType(LineActionTypes.None, -1);
			}
			return;
		}
		if (!this.CanMoveCard(GameManager.DraggedCard))
		{
			if (this.CurrentAttemptedAction.ActionType == LineActionTypes.Insert)
			{
				this.UpdateActionType(LineActionTypes.None, -1);
			}
			return;
		}
		if (this.SlotModel.SlotType == SlotsTypes.Inventory && this.Manager.InspectedCard && (this.Manager.InspectedCard.CardModel.CardType == CardTypes.Blueprint || this.Manager.InspectedCard.CardModel.CardType == CardTypes.EnvImprovement || this.Manager.InspectedCard.CardModel.CardType == CardTypes.EnvDamage))
		{
			if (this.CurrentAttemptedAction.ActionType == LineActionTypes.Insert)
			{
				this.UpdateActionType(LineActionTypes.None, -1);
			}
			return;
		}
		int pointerIndex = this.GetPointerIndex(GameManager.DraggedCard);
		if (pointerIndex == -1)
		{
			if (this.CurrentAttemptedAction.ActionType == LineActionTypes.Insert)
			{
				this.UpdateActionType(LineActionTypes.None, -1);
			}
			return;
		}
		if (this.CurrentAttemptedAction.ActionType == LineActionTypes.None)
		{
			this.UpdateActionType(LineActionTypes.Insert, pointerIndex);
		}
		if (this.CurrentAttemptedAction.ActionType == LineActionTypes.Insert && this.CurrentAttemptedAction.ActionTime >= this.Manager.GetActionTime(this.CurrentAttemptedAction.ActionType))
		{
			if (pointerIndex >= this.Slots.Count)
			{
				this.Separator.gameObject.SetActive(true);
			}
			else
			{
				this.Separator.gameObject.SetActive(!this.Slots[pointerIndex].CurrentRect.Contains(new Vector2(this.Pointer.position.x, this.Slots[pointerIndex].WorldPosition.y)));
			}
			this.Separator.SetSiblingIndex(pointerIndex);
			if (pointerIndex < this.Slots.Count && !this.Slots[pointerIndex].CurrentRect.Contains(new Vector2(this.Pointer.position.x, this.Slots[pointerIndex].WorldPosition.y)))
			{
				if (this.SeparatorSpace == null)
				{
					this.SeparatorSpace = new DynamicViewExtraSpace(pointerIndex, this.SeparatorSize);
					base.AddExtraSpace(this.SeparatorSpace);
				}
				else
				{
					this.RecalculateSize = true;
					this.SeparatorSpace.AtIndex = pointerIndex;
					this.SeparatorSpace.Space = this.SeparatorSize;
				}
			}
			else if (this.SeparatorSpace == null)
			{
				this.SeparatorSpace = new DynamicViewExtraSpace(0, 0f);
				base.AddExtraSpace(this.SeparatorSpace);
			}
			else
			{
				this.RecalculateSize |= (this.SeparatorSpace.Space > 0f);
				this.SeparatorSpace.Space = 0f;
			}
			this.Separator.localPosition = base.GetExtraSpacePosition(this.SeparatorSpace);
		}
	}

	// Token: 0x0600035D RID: 861 RVA: 0x000241D4 File Offset: 0x000223D4
	private bool CanMoveCard(InGameCardBase _Card)
	{
		return this.Manager.CanFindSlot(_Card, new SlotInfo(this.SlotModel.SlotType, 0), null);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000241F4 File Offset: 0x000223F4
	public void OnDrop(PointerEventData _Data)
	{
		if (!GameManager.DraggedCard)
		{
			return;
		}
		int pointerIndex = this.GetPointerIndex(GameManager.DraggedCard);
		if (this.CurrentAttemptedAction.ActionType != LineActionTypes.Insert || (this.CurrentAttemptedAction.ActionTime < this.Manager.GetActionTime(LineActionTypes.Insert) && !this.StraightToInsert(pointerIndex)))
		{
			return;
		}
		if (pointerIndex != -1)
		{
			bool flag = !this.Slots[Mathf.Clamp(pointerIndex, 0, this.Slots.Count - 1)].CurrentRect.Contains(new Vector2(this.Pointer.position.x, this.Slots[Mathf.Clamp(pointerIndex, 0, this.Slots.Count - 1)].WorldPosition.y)) || pointerIndex == 0;
			MBSingleton<GraphicsManager>.Instance.MoveCardToSlot(GameManager.DraggedCard, new SlotInfo(this.SlotModel.SlotType, flag ? (pointerIndex - 1) : pointerIndex), true, flag);
		}
	}

	// Token: 0x0600035F RID: 863 RVA: 0x000242F0 File Offset: 0x000224F0
	private bool StraightToInsert(int _Index)
	{
		return _Index <= 0 || _Index >= this.Slots.Count || !this.Slots[_Index].AssignedCard || !this.Slots[_Index].AssignedCard.HasAction || this.CurrentAttemptedAction.ActionTime >= this.Manager.GetActionTime(LineActionTypes.Insert);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00024364 File Offset: 0x00022564
	private int GetPointerIndex(InGameCardBase _Card)
	{
		if (this.Slots == null)
		{
			return -1;
		}
		int num = this.Slots.IndexOf(_Card.CurrentSlot);
		float num2 = this.WorldRect.xMin;
		DebugDrawings.DrawPoint(this.Pointer.position, Color.black, 1f);
		for (int i = 0; i < this.Slots.Count; i++)
		{
			DebugDrawings.DrawRect(this.Slots[i].CurrentRect, Color.Lerp(Color.red, Color.blue, (float)i / (float)(this.Slots.Count - 1)));
			if (this.Slots[i].IsActive && this.Slots[i].IsVisible && (!(this.Slots[i].AssignedCard != null) || !(_Card.CurrentContainer == this.Slots[i].AssignedCard)) && i != num)
			{
				if (this.Pointer.position.x > this.Slots[i].CurrentRect.xMin && this.Pointer.position.x < this.Slots[i].CurrentRect.xMax)
				{
					return i;
				}
				if (i + 1 != num || this.Pointer.position.x <= this.Slots[0].CurrentRect.xMin)
				{
					float num3;
					if (this.Pointer.position.x < this.Slots[0].CurrentRect.xMin && i == 0)
					{
						num2 = this.WorldRect.xMin;
						num3 = this.Slots[0].CurrentRect.xMin;
					}
					else
					{
						num2 = this.Slots[i].CurrentRect.xMax;
						num3 = this.WorldRect.xMax;
						if (i < this.Slots.Count - 1)
						{
							for (int j = i + 1; j < this.Slots.Count; j++)
							{
								if (this.Slots[j].IsActive)
								{
									num3 = this.Slots[j].CurrentRect.xMin;
									break;
								}
							}
						}
					}
					if (this.Pointer.position.x > num2 && this.Pointer.position.x < num3)
					{
						Debug.DrawRay(new Vector3(num2, this.RectTr.position.y, 0f), Vector3.up * 1.5f, Color.blue);
						Debug.DrawRay(new Vector3(num3, this.RectTr.position.y, 0f), Vector3.up, Color.red);
						if (i == 0 && this.Pointer.position.x < this.Slots[0].CurrentRect.xMin)
						{
							return 0;
						}
						return i + 1;
					}
				}
			}
		}
		return -1;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x000246A8 File Offset: 0x000228A8
	public int GetCenterSlotIndex()
	{
		if (this.Slots == null)
		{
			return -1;
		}
		float num = this.WorldRect.xMin;
		for (int i = 0; i < this.Slots.Count; i++)
		{
			if (this.Slots[i].IsActive)
			{
				if (this.WorldRect.center.x > this.Slots[i].CurrentRect.xMin && this.WorldRect.center.x < this.Slots[i].CurrentRect.xMax)
				{
					return Mathf.Max(0, i - 1);
				}
				float num2;
				if (this.WorldRect.center.x < this.Slots[0].CurrentRect.xMin && i == 0)
				{
					num = this.WorldRect.xMin;
					num2 = this.Slots[0].CurrentRect.xMin;
				}
				else
				{
					num = this.Slots[i].CurrentRect.xMax;
					num2 = this.WorldRect.xMax;
					if (i < this.Slots.Count - 1)
					{
						for (int j = i + 1; j < this.Slots.Count; j++)
						{
							if (this.Slots[j].IsActive)
							{
								num2 = this.Slots[j].CurrentRect.xMin;
								break;
							}
						}
					}
				}
				if (this.WorldRect.center.x > num && this.WorldRect.center.x < num2)
				{
					if (i == 0 && this.WorldRect.center.x < this.Slots[0].CurrentRect.xMin)
					{
						return 0;
					}
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00024898 File Offset: 0x00022A98
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (!this.Manager)
		{
			return;
		}
		Gizmos.color = Color.magenta;
		Rect rect = new Rect(this.WorldRect.position, base.transform.TransformVector(new Vector2(this.Manager.DragScrollMargins, this.RectTr.rect.size.y)));
		Gizmos.DrawWireCube(rect.center, rect.size + Vector3.forward * 0.2f);
		Gizmos.color = Color.cyan;
		rect = new Rect(this.WorldRect.max - rect.size, base.transform.TransformVector(new Vector2(this.Manager.DragScrollMargins, this.RectTr.rect.size.y)));
		Gizmos.DrawWireCube(rect.center, rect.size + Vector3.forward * 0.2f);
		Gizmos.color = Color.white;
	}

	// Token: 0x04000443 RID: 1091
	[Header("Line Setup")]
	public RectTransform SeparatorPrefab;

	// Token: 0x04000444 RID: 1092
	public GameObject RightArrow;

	// Token: 0x04000445 RID: 1093
	public GameObject LeftArrow;

	// Token: 0x04000446 RID: 1094
	public GameObject RightButton;

	// Token: 0x04000447 RID: 1095
	public GameObject LeftButton;

	// Token: 0x04000448 RID: 1096
	public bool CantMoveSlots;

	// Token: 0x04000449 RID: 1097
	public bool IsPopup;

	// Token: 0x0400044A RID: 1098
	private SlotSettings SlotModel;

	// Token: 0x0400044C RID: 1100
	private RectTransform Separator;

	// Token: 0x0400044D RID: 1101
	private DynamicViewExtraSpace SeparatorSpace;

	// Token: 0x0400044E RID: 1102
	private Rect WorldRect;

	// Token: 0x0400044F RID: 1103
	private Transform Pointer;

	// Token: 0x04000450 RID: 1104
	private bool NewSlotsAdded;

	// Token: 0x04000451 RID: 1105
	private ScrollRect ScrollView;

	// Token: 0x04000452 RID: 1106
	private RectTransform ScrollViewRect;

	// Token: 0x04000453 RID: 1107
	private HorizontalLayoutGroup LineLayoutGroup;

	// Token: 0x04000454 RID: 1108
	[SerializeField]
	private HorizontalLayoutGroup ExplicitLineLayoutGroup;

	// Token: 0x04000455 RID: 1109
	private GraphicsManager Manager;

	// Token: 0x04000456 RID: 1110
	private CardLine.LineAction CurrentAttemptedAction;

	// Token: 0x04000457 RID: 1111
	private float ButtonMoveTarget;

	// Token: 0x04000458 RID: 1112
	private float SlotSize;

	// Token: 0x04000459 RID: 1113
	private float SeparatorSize;

	// Token: 0x02000253 RID: 595
	private struct LineAction
	{
		// Token: 0x04001419 RID: 5145
		public LineActionTypes ActionType;

		// Token: 0x0400141A RID: 5146
		public float ActionTime;

		// Token: 0x0400141B RID: 5147
		public int ActionIndex;
	}
}
