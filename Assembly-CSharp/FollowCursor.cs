using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class FollowCursor : MonoBehaviour
{
	// Token: 0x06000A35 RID: 2613 RVA: 0x0005B408 File Offset: 0x00059608
	private void LateUpdate()
	{
		Vector3 position;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.ParentRect, Input.mousePosition, Camera.main, out position))
		{
			base.transform.position = position;
		}
	}

	// Token: 0x04000FAD RID: 4013
	[SerializeField]
	private RectTransform ParentRect;
}
