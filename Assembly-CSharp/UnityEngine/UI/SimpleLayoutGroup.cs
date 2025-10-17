using System;

namespace UnityEngine.UI
{
	// Token: 0x020001F0 RID: 496
	[ExecuteAlways]
	public class SimpleLayoutGroup : LayoutGroup
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00069245 File Offset: 0x00067445
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x0006924D File Offset: 0x0006744D
		public float spacing
		{
			get
			{
				return this.m_Spacing;
			}
			set
			{
				base.SetProperty<float>(ref this.m_Spacing, value);
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0006925C File Offset: 0x0006745C
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x00069264 File Offset: 0x00067464
		public SimpleLayoutGroup.Orientation orientation
		{
			get
			{
				return this.m_Orientation;
			}
			set
			{
				base.SetProperty<SimpleLayoutGroup.Orientation>(ref this.m_Orientation, value);
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00069273 File Offset: 0x00067473
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x0006927B File Offset: 0x0006747B
		public bool adjustSize
		{
			get
			{
				return this.m_AdjustSize;
			}
			set
			{
				base.SetProperty<bool>(ref this.m_AdjustSize, value);
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0006928A File Offset: 0x0006748A
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x00069292 File Offset: 0x00067492
		public float emptySize
		{
			get
			{
				return this.m_EmptySize;
			}
			set
			{
				base.SetProperty<float>(ref this.m_EmptySize, value);
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000692A4 File Offset: 0x000674A4
		protected void CalcAlongAxis(int axis, bool isVertical)
		{
			float num = (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical);
			if ((isVertical ^ axis == 1) || !this.m_AdjustSize)
			{
				this.m_LayoutSize[axis] = base.rectTransform.sizeDelta[axis];
				base.SetLayoutInputForAxis(this.m_LayoutSize[axis], this.m_LayoutSize[axis], 0f, axis);
				return;
			}
			float num2 = num + (float)base.rectChildren.Count * this.m_Spacing;
			this.m_LayoutSize[axis] = Mathf.Max(this.m_EmptySize, num2);
			base.SetLayoutInputForAxis(num2, num2, 0f, axis);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00069360 File Offset: 0x00067560
		protected void SetChildrenAlongAxis(int axis, bool isVertical)
		{
			float num = base.rectTransform.rect.size[axis];
			float alignmentOnAxis = base.GetAlignmentOnAxis(axis);
			if (isVertical ^ axis == 1)
			{
				float value = num - (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical);
				for (int i = 0; i < base.rectChildren.Count; i++)
				{
					RectTransform rectTransform = base.rectChildren[i];
					float num2 = Mathf.Clamp(value, rectTransform.sizeDelta[axis], num);
					float startOffset = base.GetStartOffset(axis, num2);
					float num3 = (num2 - rectTransform.sizeDelta[axis]) * alignmentOnAxis;
					base.SetChildAlongAxisWithScale(rectTransform, axis, startOffset + num3, 1f);
				}
				return;
			}
			float num4 = base.GetStartOffset(axis, base.GetTotalPreferredSize(axis) - (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical));
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				RectTransform rectTransform2 = base.rectChildren[j];
				float num5 = (this.spacing - rectTransform2.sizeDelta[axis]) * alignmentOnAxis;
				base.SetChildAlongAxisWithScale(rectTransform2, axis, num4 + num5, 1f);
				num4 += this.spacing;
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000694C8 File Offset: 0x000676C8
		private void HandleSelfFittingAlongAxis(int axis)
		{
			if (axis != (int)this.m_Orientation || !this.m_AdjustSize)
			{
				this.m_Tracker.Add(this, base.rectTransform, DrivenTransformProperties.None);
				return;
			}
			this.m_Tracker.Add(this, base.rectTransform, (axis == 0) ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY);
			base.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, this.m_LayoutSize[axis]);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00069536 File Offset: 0x00067736
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			this.CalcAlongAxis(0, this.m_Orientation == SimpleLayoutGroup.Orientation.Vertical);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0006954E File Offset: 0x0006774E
		public override void CalculateLayoutInputVertical()
		{
			this.CalcAlongAxis(1, this.m_Orientation == SimpleLayoutGroup.Orientation.Vertical);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00069560 File Offset: 0x00067760
		public override void SetLayoutHorizontal()
		{
			this.HandleSelfFittingAlongAxis(0);
			this.SetChildrenAlongAxis(0, this.m_Orientation == SimpleLayoutGroup.Orientation.Vertical);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00069579 File Offset: 0x00067779
		public override void SetLayoutVertical()
		{
			this.HandleSelfFittingAlongAxis(1);
			this.SetChildrenAlongAxis(1, this.m_Orientation == SimpleLayoutGroup.Orientation.Vertical);
		}

		// Token: 0x040011C1 RID: 4545
		[SerializeField]
		protected float m_Spacing;

		// Token: 0x040011C2 RID: 4546
		[SerializeField]
		protected SimpleLayoutGroup.Orientation m_Orientation;

		// Token: 0x040011C3 RID: 4547
		[SerializeField]
		protected bool m_AdjustSize;

		// Token: 0x040011C4 RID: 4548
		[SerializeField]
		protected float m_EmptySize;

		// Token: 0x040011C5 RID: 4549
		private Vector2 m_LayoutSize;

		// Token: 0x020002AB RID: 683
		public enum Orientation
		{
			// Token: 0x0400158F RID: 5519
			Horizontal,
			// Token: 0x04001590 RID: 5520
			Vertical
		}
	}
}
