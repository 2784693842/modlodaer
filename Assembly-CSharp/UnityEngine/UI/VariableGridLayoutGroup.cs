using System;

namespace UnityEngine.UI
{
	// Token: 0x020001F1 RID: 497
	[AddComponentMenu("Layout/Variable Grid Layout Group", 152)]
	public class VariableGridLayoutGroup : LayoutGroup
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0006959A File Offset: 0x0006779A
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x000695A2 File Offset: 0x000677A2
		public VariableGridLayoutGroup.Corner startCorner
		{
			get
			{
				return this.m_StartCorner;
			}
			set
			{
				base.SetProperty<VariableGridLayoutGroup.Corner>(ref this.m_StartCorner, value);
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x000695B1 File Offset: 0x000677B1
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x000695B9 File Offset: 0x000677B9
		public VariableGridLayoutGroup.Axis startAxis
		{
			get
			{
				return this.m_StartAxis;
			}
			set
			{
				base.SetProperty<VariableGridLayoutGroup.Axis>(ref this.m_StartAxis, value);
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x000695C8 File Offset: 0x000677C8
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x000695D0 File Offset: 0x000677D0
		public TextAnchor cellAlignment
		{
			get
			{
				return this.m_CellAlignment;
			}
			set
			{
				base.SetProperty<TextAnchor>(ref this.m_CellAlignment, value);
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x000695DF File Offset: 0x000677DF
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x000695E7 File Offset: 0x000677E7
		public Vector2 spacing
		{
			get
			{
				return this.m_Spacing;
			}
			set
			{
				base.SetProperty<Vector2>(ref this.m_Spacing, value);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x000695F6 File Offset: 0x000677F6
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x000695FE File Offset: 0x000677FE
		public VariableGridLayoutGroup.Constraint constraint
		{
			get
			{
				return this.m_Constraint;
			}
			set
			{
				base.SetProperty<VariableGridLayoutGroup.Constraint>(ref this.m_Constraint, value);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x0006960D File Offset: 0x0006780D
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x00069615 File Offset: 0x00067815
		public int constraintCount
		{
			get
			{
				return this.m_ConstraintCount;
			}
			set
			{
				base.SetProperty<int>(ref this.m_ConstraintCount, Mathf.Max(1, value));
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0006962A File Offset: 0x0006782A
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x00069632 File Offset: 0x00067832
		public bool childForceExpandWidth
		{
			get
			{
				return this.m_ChildForceExpandWidth;
			}
			set
			{
				base.SetProperty<bool>(ref this.m_ChildForceExpandWidth, value);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00069641 File Offset: 0x00067841
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x00069649 File Offset: 0x00067849
		public bool childForceExpandHeight
		{
			get
			{
				return this.m_ChildForceExpandHeight;
			}
			set
			{
				base.SetProperty<bool>(ref this.m_ChildForceExpandHeight, value);
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00069658 File Offset: 0x00067858
		protected VariableGridLayoutGroup()
		{
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000D26 RID: 3366 RVA: 0x00069680 File Offset: 0x00067880
		// (set) Token: 0x06000D27 RID: 3367 RVA: 0x00069688 File Offset: 0x00067888
		public int columns { get; private set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000D28 RID: 3368 RVA: 0x00069691 File Offset: 0x00067891
		// (set) Token: 0x06000D29 RID: 3369 RVA: 0x00069699 File Offset: 0x00067899
		public int rows { get; private set; }

		// Token: 0x06000D2A RID: 3370 RVA: 0x000696A2 File Offset: 0x000678A2
		public int GetCellIndexAtGridRef(int column, int row)
		{
			if (column >= 0 && column < this.columns && row >= 0 && row < this.rows)
			{
				return this.cellIndexAtGridRef[column, row];
			}
			return -1;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x000696CD File Offset: 0x000678CD
		public int GetCellColumn(int cellIndex)
		{
			if (cellIndex >= 0 && cellIndex < base.rectChildren.Count)
			{
				return this.cellColumn[cellIndex];
			}
			return -1;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000696EB File Offset: 0x000678EB
		public int GetCellRow(int cellIndex)
		{
			if (cellIndex >= 0 && cellIndex < base.rectChildren.Count)
			{
				return this.cellRow[cellIndex];
			}
			return -1;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0006970C File Offset: 0x0006790C
		public float GetColumnPositionWithinGrid(int column)
		{
			if (column <= 0 || column >= this.columns)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < column; i++)
			{
				num += this.GetColumnWidth(i) + this.spacing.x;
			}
			return num;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00069758 File Offset: 0x00067958
		public float GetRowPositionWithinGrid(int row)
		{
			if (row <= 0 || row >= this.rows)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < row; i++)
			{
				num += this.GetRowHeight(i) + this.spacing.y;
			}
			return num;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x000697A1 File Offset: 0x000679A1
		public float GetColumnWidth(int column)
		{
			if (column < 0 || column >= this.columns)
			{
				return 0f;
			}
			return this.columnWidths[column];
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000697BE File Offset: 0x000679BE
		public float GetRowHeight(int row)
		{
			if (row < 0 || row >= this.rows)
			{
				return 0f;
			}
			return this.rowHeights[row];
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000697DC File Offset: 0x000679DC
		private void InitializeLayout()
		{
			this.columns = ((this.constraint == VariableGridLayoutGroup.Constraint.FixedColumnCount) ? Mathf.Min(this.constraintCount, base.rectChildren.Count) : Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.constraintCount));
			this.rows = ((this.constraint == VariableGridLayoutGroup.Constraint.FixedRowCount) ? Mathf.Min(this.constraintCount, base.rectChildren.Count) : Mathf.CeilToInt((float)base.rectChildren.Count / (float)this.constraintCount));
			this.cellIndexAtGridRef = new int[this.columns, this.rows];
			this.cellColumn = new int[base.rectChildren.Count];
			this.cellRow = new int[base.rectChildren.Count];
			this.cellPreferredSizes = new Vector2[base.rectChildren.Count];
			this.columnWidths = new float[this.columns];
			this.rowHeights = new float[this.rows];
			this.totalColumnWidth = 0f;
			this.totalRowHeight = 0f;
			for (int i = 0; i < this.columns; i++)
			{
				for (int j = 0; j < this.rows; j++)
				{
					this.cellIndexAtGridRef[i, j] = -1;
				}
			}
			int num = 0;
			int num2 = 0;
			int num3 = 1;
			int num4 = 1;
			if (this.startCorner == VariableGridLayoutGroup.Corner.UpperRight || this.startCorner == VariableGridLayoutGroup.Corner.LowerRight)
			{
				num = this.columns - 1;
				num3 = -1;
			}
			if (this.startCorner == VariableGridLayoutGroup.Corner.LowerLeft || this.startCorner == VariableGridLayoutGroup.Corner.LowerRight)
			{
				num2 = this.rows - 1;
				num4 = -1;
			}
			int num5 = num;
			int num6 = num2;
			for (int k = 0; k < base.rectChildren.Count; k++)
			{
				this.cellIndexAtGridRef[num5, num6] = k;
				this.cellColumn[k] = num5;
				this.cellRow[k] = num6;
				this.cellPreferredSizes[k] = new Vector2(LayoutUtility.GetPreferredWidth(base.rectChildren[k]), LayoutUtility.GetPreferredHeight(base.rectChildren[k]));
				this.columnWidths[num5] = Mathf.Max(this.columnWidths[num5], this.cellPreferredSizes[k].x);
				this.rowHeights[num6] = Mathf.Max(this.rowHeights[num6], this.cellPreferredSizes[k].y);
				if (this.startAxis == VariableGridLayoutGroup.Axis.Horizontal)
				{
					num5 += num3;
					if (num5 < 0 || num5 >= this.columns)
					{
						num5 = num;
						num6 += num4;
					}
				}
				else
				{
					num6 += num4;
					if (num6 < 0 || num6 >= this.rows)
					{
						num6 = num2;
						num5 += num3;
					}
				}
			}
			for (int l = 0; l < this.columns; l++)
			{
				this.totalColumnWidth += this.columnWidths[l];
			}
			for (int m = 0; m < this.rows; m++)
			{
				this.totalRowHeight += this.rowHeights[m];
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00069AE8 File Offset: 0x00067CE8
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			this.InitializeLayout();
			float totalMin = (float)base.padding.horizontal;
			float num = (float)base.padding.horizontal + this.totalColumnWidth + this.spacing.x * (float)(this.columns - 1);
			base.SetLayoutInputForAxis(totalMin, num, -1f, 0);
			float num2 = LayoutUtility.GetPreferredWidth(base.rectTransform) - num;
			if (num2 > 0f && this.childForceExpandWidth)
			{
				bool[] array = new bool[this.columns];
				int num3 = 0;
				for (int i = 0; i < this.columns; i++)
				{
					array[i] = false;
					for (int j = 0; j < this.rows; j++)
					{
						int num4 = this.GetCellIndexAtGridRef(i, j);
						if (num4 < base.rectChildren.Count)
						{
							VariableGridCell component = base.rectChildren[num4].GetComponent<VariableGridCell>();
							if (component == null || !component.overrideForceExpandWidth || component.forceExpandWidth)
							{
								array[i] = true;
								num3++;
								break;
							}
						}
					}
				}
				for (int k = 0; k < this.columns; k++)
				{
					if (array[k])
					{
						this.columnWidths[k] += num2 / (float)num3;
					}
				}
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00069C34 File Offset: 0x00067E34
		public override void CalculateLayoutInputVertical()
		{
			float totalMin = (float)base.padding.vertical;
			float num = (float)base.padding.vertical + this.totalRowHeight + this.spacing.y * (float)(this.rows - 1);
			base.SetLayoutInputForAxis(totalMin, num, -1f, 1);
			float num2 = LayoutUtility.GetPreferredHeight(base.rectTransform) - num;
			if (num2 > 0f && this.childForceExpandHeight)
			{
				bool[] array = new bool[this.rows];
				int num3 = 0;
				for (int i = 0; i < this.rows; i++)
				{
					array[i] = false;
					for (int j = 0; j < this.columns; j++)
					{
						int num4 = this.GetCellIndexAtGridRef(j, i);
						if (num4 < 0 || num4 >= base.rectChildren.Count)
						{
							array[i] = true;
							num3++;
							break;
						}
						VariableGridCell component = base.rectChildren[num4].GetComponent<VariableGridCell>();
						if (component == null || !component.overrideForceExpandHeight || component.forceExpandHeight)
						{
							array[i] = true;
							num3++;
							break;
						}
					}
				}
				for (int k = 0; k < this.rows; k++)
				{
					if (array[k])
					{
						this.rowHeights[k] += num2 / (float)num3;
					}
				}
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00069D89 File Offset: 0x00067F89
		public override void SetLayoutHorizontal()
		{
			this.SetCellsAlongAxis(0);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00069D92 File Offset: 0x00067F92
		public override void SetLayoutVertical()
		{
			this.SetCellsAlongAxis(1);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00069D9C File Offset: 0x00067F9C
		private void SetCellsAlongAxis(int axis)
		{
			float num = ((axis == 0) ? base.rectTransform.rect.width : base.rectTransform.rect.height) - LayoutUtility.GetPreferredSize(base.rectTransform, axis);
			float num2 = (float)((axis == 0) ? base.padding.left : base.padding.top);
			if (axis == 0)
			{
				if (base.childAlignment == TextAnchor.UpperCenter || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.LowerCenter)
				{
					num2 += num / 2f;
				}
				else if (base.childAlignment == TextAnchor.UpperRight || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.LowerRight)
				{
					num2 += num;
				}
			}
			else if (base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.MiddleRight)
			{
				num2 += num / 2f;
			}
			else if (base.childAlignment == TextAnchor.LowerLeft || base.childAlignment == TextAnchor.LowerCenter || base.childAlignment == TextAnchor.LowerRight)
			{
				num2 += num;
			}
			bool flag = (axis == 0) ? this.childForceExpandWidth : this.childForceExpandHeight;
			int num3 = 0;
			if (axis == 0)
			{
				if (this.cellAlignment == TextAnchor.UpperLeft || this.cellAlignment == TextAnchor.MiddleLeft || this.cellAlignment == TextAnchor.LowerLeft)
				{
					num3 = -1;
				}
				if (this.cellAlignment == TextAnchor.UpperRight || this.cellAlignment == TextAnchor.MiddleRight || this.cellAlignment == TextAnchor.LowerRight)
				{
					num3 = 1;
				}
			}
			else
			{
				if (this.cellAlignment == TextAnchor.UpperLeft || this.cellAlignment == TextAnchor.UpperCenter || this.cellAlignment == TextAnchor.UpperRight)
				{
					num3 = -1;
				}
				if (this.cellAlignment == TextAnchor.LowerLeft || this.cellAlignment == TextAnchor.LowerCenter || this.cellAlignment == TextAnchor.LowerRight)
				{
					num3 = 1;
				}
			}
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				int num4 = (axis == 0) ? this.GetCellColumn(i) : this.GetCellRow(i);
				float num5 = num2 + ((axis == 0) ? this.GetColumnPositionWithinGrid(num4) : this.GetRowPositionWithinGrid(num4));
				float num6 = (axis == 0) ? this.GetColumnWidth(num4) : this.GetRowHeight(num4);
				RectTransform rectTransform = base.rectChildren[i];
				float num7 = LayoutUtility.GetPreferredSize(rectTransform, axis);
				float num8 = num6 - num7;
				bool flag2 = flag;
				int num9 = num3;
				VariableGridCell component = rectTransform.GetComponent<VariableGridCell>();
				if (component != null)
				{
					if ((axis == 0) ? component.overrideForceExpandWidth : component.overrideForceExpandHeight)
					{
						flag2 = ((axis == 0) ? component.forceExpandWidth : component.forceExpandHeight);
					}
					if (component.overrideCellAlignment)
					{
						if (axis == 0)
						{
							if (component.cellAlignment == TextAnchor.UpperLeft || component.cellAlignment == TextAnchor.MiddleLeft || component.cellAlignment == TextAnchor.LowerLeft)
							{
								num9 = -1;
							}
							else if (component.cellAlignment == TextAnchor.UpperCenter || component.cellAlignment == TextAnchor.MiddleCenter || component.cellAlignment == TextAnchor.LowerCenter)
							{
								num9 = 0;
							}
							else
							{
								num9 = 1;
							}
						}
						else if (component.cellAlignment == TextAnchor.UpperLeft || component.cellAlignment == TextAnchor.UpperCenter || component.cellAlignment == TextAnchor.UpperRight)
						{
							num9 = -1;
						}
						else if (component.cellAlignment == TextAnchor.MiddleLeft || component.cellAlignment == TextAnchor.MiddleCenter || component.cellAlignment == TextAnchor.MiddleRight)
						{
							num9 = 0;
						}
						else
						{
							num9 = 1;
						}
					}
				}
				if (flag2)
				{
					num7 = num6;
				}
				else
				{
					if (num9 == 0)
					{
						num5 += num8 / 2f;
					}
					if (num9 == 1)
					{
						num5 += num8;
					}
				}
				base.SetChildAlongAxis(base.rectChildren[i], axis, num5, num7);
			}
		}

		// Token: 0x040011C6 RID: 4550
		[SerializeField]
		protected VariableGridLayoutGroup.Corner m_StartCorner;

		// Token: 0x040011C7 RID: 4551
		[SerializeField]
		protected VariableGridLayoutGroup.Axis m_StartAxis;

		// Token: 0x040011C8 RID: 4552
		[SerializeField]
		protected TextAnchor m_CellAlignment;

		// Token: 0x040011C9 RID: 4553
		[SerializeField]
		protected Vector2 m_Spacing = Vector2.zero;

		// Token: 0x040011CA RID: 4554
		[SerializeField]
		protected VariableGridLayoutGroup.Constraint m_Constraint;

		// Token: 0x040011CB RID: 4555
		[SerializeField]
		protected int m_ConstraintCount = 3;

		// Token: 0x040011CC RID: 4556
		[SerializeField]
		protected bool m_ChildForceExpandWidth = true;

		// Token: 0x040011CD RID: 4557
		[SerializeField]
		protected bool m_ChildForceExpandHeight = true;

		// Token: 0x040011D0 RID: 4560
		private int[,] cellIndexAtGridRef;

		// Token: 0x040011D1 RID: 4561
		private int[] cellColumn;

		// Token: 0x040011D2 RID: 4562
		private int[] cellRow;

		// Token: 0x040011D3 RID: 4563
		private Vector2[] cellPreferredSizes;

		// Token: 0x040011D4 RID: 4564
		private float[] columnWidths;

		// Token: 0x040011D5 RID: 4565
		private float[] rowHeights;

		// Token: 0x040011D6 RID: 4566
		private float totalColumnWidth;

		// Token: 0x040011D7 RID: 4567
		private float totalRowHeight;

		// Token: 0x020002AC RID: 684
		public enum Corner
		{
			// Token: 0x04001592 RID: 5522
			UpperLeft,
			// Token: 0x04001593 RID: 5523
			UpperRight,
			// Token: 0x04001594 RID: 5524
			LowerLeft,
			// Token: 0x04001595 RID: 5525
			LowerRight
		}

		// Token: 0x020002AD RID: 685
		public enum Axis
		{
			// Token: 0x04001597 RID: 5527
			Horizontal,
			// Token: 0x04001598 RID: 5528
			Vertical
		}

		// Token: 0x020002AE RID: 686
		public enum Constraint
		{
			// Token: 0x0400159A RID: 5530
			FixedColumnCount,
			// Token: 0x0400159B RID: 5531
			FixedRowCount
		}
	}
}
