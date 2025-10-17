using System;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class ChildFillRect : MonoBehaviour
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x0005A5E8 File Offset: 0x000587E8
	private void LateUpdate()
	{
		if (!this.MyRect)
		{
			this.MyRect = base.GetComponent<RectTransform>();
		}
		if (this.ChildRect && this.ChildRect.parent != base.transform)
		{
			this.ChildRect = null;
		}
		if (!this.ChildRect && base.transform.childCount > 0)
		{
			this.ChildRect = base.transform.GetChild(0).GetComponent<RectTransform>();
		}
		if (!this.ChildRect)
		{
			return;
		}
		this.ChildRect.anchorMin = Vector2.zero;
		this.ChildRect.anchorMax = Vector2.one;
		this.ChildRect.offsetMin = Vector2.zero;
		this.ChildRect.offsetMax = Vector2.zero;
	}

	// Token: 0x04000F85 RID: 3973
	private RectTransform MyRect;

	// Token: 0x04000F86 RID: 3974
	private RectTransform ChildRect;
}
