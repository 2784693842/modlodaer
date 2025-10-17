using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001BE RID: 446
[RequireComponent(typeof(RectTransform))]
public class UIReorderableList : MonoBehaviour, ILayoutElement
{
	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00064D28 File Offset: 0x00062F28
	public float minWidth
	{
		get
		{
			return this.LayoutWidth;
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00064D28 File Offset: 0x00062F28
	public float preferredWidth
	{
		get
		{
			return this.LayoutWidth;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00064D30 File Offset: 0x00062F30
	public float flexibleWidth
	{
		get
		{
			return this.FlexibleWidth;
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00064D38 File Offset: 0x00062F38
	public float minHeight
	{
		get
		{
			return this.LayoutHeight;
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00064D38 File Offset: 0x00062F38
	public float preferredHeight
	{
		get
		{
			return this.LayoutHeight;
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00064D40 File Offset: 0x00062F40
	public float flexibleHeight
	{
		get
		{
			return this.FlexibleHeight;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00052B1A File Offset: 0x00050D1A
	public int layoutPriority
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00064D48 File Offset: 0x00062F48
	public void Update()
	{
		this.CalculateSize();
		if (this.AnimLerpSpeed > 0f)
		{
			UIReorderableList.SortingOrientation orientation = this.Orientation;
			if (orientation > UIReorderableList.SortingOrientation.RightToLeft)
			{
				if (orientation - UIReorderableList.SortingOrientation.DownToUp <= 1)
				{
					this.LayoutWidth = this.TargetLayoutWidth;
					this.LayoutHeight = Mathf.Lerp(this.LayoutHeight, this.TargetLayoutHeight, this.AnimLerpSpeed * Time.deltaTime);
				}
			}
			else
			{
				this.LayoutWidth = Mathf.Lerp(this.LayoutWidth, this.TargetLayoutWidth, this.AnimLerpSpeed * Time.deltaTime);
				this.LayoutHeight = this.TargetLayoutHeight;
			}
			if (!Mathf.Approximately(this.LayoutWidth, this.TargetLayoutWidth) || !Mathf.Approximately(this.LayoutHeight, this.TargetLayoutHeight))
			{
				LayoutRebuilder.MarkLayoutForRebuild(this.MyTr);
			}
		}
		else
		{
			this.LayoutWidth = this.TargetLayoutWidth;
			this.LayoutHeight = this.TargetLayoutHeight;
		}
		Vector3 vector = this.GetStartingPos();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			this.CurrentElement = base.transform.GetChild(i).GetComponent<UIReorderableListElement>();
			if (!this.CurrentElement || !this.CurrentElement.IgnoreList)
			{
				if (!this.CurrentElement)
				{
					base.transform.GetChild(i).localPosition = vector;
				}
				else if (!this.CurrentElement.IgnoreList)
				{
					this.CurrentElement.LocalPosTarget = vector;
				}
				vector += this.GetIncrement();
			}
		}
		if (this.LastChildCount != base.transform.childCount)
		{
			LayoutRebuilder.MarkLayoutForRebuild(this.MyTr);
			this.LastChildCount = base.transform.childCount;
		}
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x00064EF0 File Offset: 0x000630F0
	private void CalculateSize()
	{
		if (!this.MyTr)
		{
			this.MyTr = base.GetComponent<RectTransform>();
		}
		float num = 0f;
		if (!this.ZeroWhenNoChildren || base.transform.childCount > 0)
		{
			num = this.Padding * 2f;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (!this.CurrentElement || !this.CurrentElement.IgnoreList)
			{
				num += this.Spacing;
			}
		}
		UIReorderableList.SortingOrientation orientation = this.Orientation;
		if (orientation <= UIReorderableList.SortingOrientation.RightToLeft)
		{
			this.TargetLayoutHeight = this.MyTr.rect.height;
			this.FlexibleWidth = 0f;
			this.FlexibleHeight = 1f;
			this.TargetLayoutWidth = num;
			return;
		}
		if (orientation - UIReorderableList.SortingOrientation.DownToUp > 1)
		{
			return;
		}
		this.TargetLayoutHeight = num;
		this.FlexibleWidth = 1f;
		this.FlexibleHeight = 0f;
		this.TargetLayoutWidth = this.MyTr.rect.width;
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00064FF8 File Offset: 0x000631F8
	public Vector3 GetStartingPos()
	{
		if (!this.MyTr)
		{
			this.MyTr = base.GetComponent<RectTransform>();
		}
		if (this.Orientation == UIReorderableList.SortingOrientation.LeftToRight || this.Orientation == UIReorderableList.SortingOrientation.RightToLeft)
		{
			return new Vector3(this.LayoutWidth * this.StartingPos.x - this.Padding, this.LayoutHeight * this.StartingPos.y, this.ZPos);
		}
		return new Vector3(this.LayoutWidth * this.StartingPos.x, this.LayoutHeight * this.StartingPos.y - this.Padding, this.ZPos);
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0006509C File Offset: 0x0006329C
	public Vector3 GetIncrement()
	{
		switch (this.Orientation)
		{
		case UIReorderableList.SortingOrientation.LeftToRight:
			return Vector3.right * this.Spacing;
		case UIReorderableList.SortingOrientation.RightToLeft:
			return Vector3.left * this.Spacing;
		case UIReorderableList.SortingOrientation.DownToUp:
			return Vector3.up * this.Spacing;
		case UIReorderableList.SortingOrientation.UpToDown:
			return Vector3.down * this.Spacing;
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x00065114 File Offset: 0x00063314
	private void OnDrawGizmosSelected()
	{
		if (!this.MyTr)
		{
			this.MyTr = base.GetComponent<RectTransform>();
		}
		this.CalculateSize();
		Vector3 vector = this.GetStartingPos();
		Vector3 b = Vector3.zero;
		UIReorderableList.SortingOrientation orientation = this.Orientation;
		if (orientation > UIReorderableList.SortingOrientation.RightToLeft)
		{
			if (orientation - UIReorderableList.SortingOrientation.DownToUp <= 1)
			{
				b = base.transform.TransformVector(Vector3.right * this.MyTr.rect.width * 0.5f);
			}
		}
		else
		{
			b = base.transform.TransformVector(Vector3.up * this.MyTr.rect.height * 0.5f);
		}
		Gizmos.color = Color.red;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			UIReorderableListElement component = base.transform.GetChild(i).GetComponent<UIReorderableListElement>();
			if (!component || !component.IgnoreList)
			{
				Gizmos.DrawLine(base.transform.TransformPoint(vector) - b, base.transform.TransformPoint(vector) + b);
				vector += this.GetIncrement();
			}
		}
		Gizmos.color = Color.white;
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00065253 File Offset: 0x00063453
	public void CalculateLayoutInputHorizontal()
	{
		if (!Application.isPlaying)
		{
			this.Update();
		}
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00065253 File Offset: 0x00063453
	public void CalculateLayoutInputVertical()
	{
		if (!Application.isPlaying)
		{
			this.Update();
		}
	}

	// Token: 0x0400111F RID: 4383
	[SerializeField]
	private UIReorderableList.SortingOrientation Orientation;

	// Token: 0x04001120 RID: 4384
	[SerializeField]
	private float Padding;

	// Token: 0x04001121 RID: 4385
	[SerializeField]
	private float Spacing;

	// Token: 0x04001122 RID: 4386
	[SerializeField]
	private Vector2 StartingPos;

	// Token: 0x04001123 RID: 4387
	[SerializeField]
	private float ZPos;

	// Token: 0x04001124 RID: 4388
	[SerializeField]
	private float AnimLerpSpeed;

	// Token: 0x04001125 RID: 4389
	[SerializeField]
	private bool ZeroWhenNoChildren;

	// Token: 0x04001126 RID: 4390
	private UIReorderableListElement CurrentElement;

	// Token: 0x04001127 RID: 4391
	private RectTransform MyTr;

	// Token: 0x04001128 RID: 4392
	private int LastChildCount;

	// Token: 0x04001129 RID: 4393
	private float LayoutWidth;

	// Token: 0x0400112A RID: 4394
	private float LayoutHeight;

	// Token: 0x0400112B RID: 4395
	private float FlexibleWidth;

	// Token: 0x0400112C RID: 4396
	private float FlexibleHeight;

	// Token: 0x0400112D RID: 4397
	private float TargetLayoutWidth;

	// Token: 0x0400112E RID: 4398
	private float TargetLayoutHeight;

	// Token: 0x020002A6 RID: 678
	public enum SortingOrientation
	{
		// Token: 0x04001572 RID: 5490
		LeftToRight,
		// Token: 0x04001573 RID: 5491
		RightToLeft,
		// Token: 0x04001574 RID: 5492
		DownToUp,
		// Token: 0x04001575 RID: 5493
		UpToDown
	}
}
