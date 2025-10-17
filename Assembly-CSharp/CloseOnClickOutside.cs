using System;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class CloseOnClickOutside : MonoBehaviour
{
	// Token: 0x06000A0D RID: 2573 RVA: 0x0005A6D7 File Offset: 0x000588D7
	private void OnEnable()
	{
		this.MouseDownOutside = false;
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0005A6E0 File Offset: 0x000588E0
	private void LateUpdate()
	{
		if (this.CloseFromThisScript && this.UpdateClickedOutside())
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0005A700 File Offset: 0x00058900
	public bool UpdateClickedOutside()
	{
		if (!this.InnerWindowRect)
		{
			this.MouseDownOutside = false;
			return false;
		}
		if (CloseOnClickOutside.CancelFlag > 0 && CloseOnClickOutside.CancelFlag == this.CancelFlagIndex)
		{
			this.MouseDownOutside = false;
			return false;
		}
		this.WorldRect = new Rect(this.InnerWindowRect.transform.TransformPoint(this.InnerWindowRect.rect.position), this.InnerWindowRect.transform.TransformVector(this.InnerWindowRect.rect.size));
		if (this.DebugWorldRect)
		{
			Debug.DrawLine(this.WorldRect.min, new Vector2(this.WorldRect.max.x, this.WorldRect.min.y));
			Debug.DrawLine(this.WorldRect.min, new Vector2(this.WorldRect.min.x, this.WorldRect.max.y));
			Debug.DrawLine(this.WorldRect.max, new Vector2(this.WorldRect.max.x, this.WorldRect.min.y));
			Debug.DrawLine(this.WorldRect.max, new Vector2(this.WorldRect.min.x, this.WorldRect.max.y));
		}
		if (GameManager.DraggedCard)
		{
			this.MouseDownOutside = false;
			return false;
		}
		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButtonDown(0) && !this.WorldRect.Contains(point))
		{
			this.MouseDownOutside = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (!this.WorldRect.Contains(point) && this.MouseDownOutside)
			{
				this.MouseDownOutside = false;
				return true;
			}
			this.MouseDownOutside = false;
		}
		return false;
	}

	// Token: 0x04000F8A RID: 3978
	[SerializeField]
	private RectTransform InnerWindowRect;

	// Token: 0x04000F8B RID: 3979
	[SerializeField]
	private bool DebugWorldRect;

	// Token: 0x04000F8C RID: 3980
	[SerializeField]
	private bool CloseFromThisScript;

	// Token: 0x04000F8D RID: 3981
	[SerializeField]
	private int CancelFlagIndex;

	// Token: 0x04000F8E RID: 3982
	private Rect WorldRect;

	// Token: 0x04000F8F RID: 3983
	private bool MouseDownOutside;

	// Token: 0x04000F90 RID: 3984
	public static int CancelFlag;

	// Token: 0x04000F91 RID: 3985
	public const int CardSwapFlag = 1;

	// Token: 0x04000F92 RID: 3986
	public const int StatInspectionFlag = 2;
}
