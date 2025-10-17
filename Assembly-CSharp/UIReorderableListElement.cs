using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class UIReorderableListElement : MonoBehaviour
{
	// Token: 0x06000C3F RID: 3135 RVA: 0x00065264 File Offset: 0x00063464
	private void Awake()
	{
		UIReorderableList componentInParent = base.GetComponentInParent<UIReorderableList>();
		if (componentInParent)
		{
			componentInParent.Update();
		}
		base.transform.localPosition = this.LocalPosTarget;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x00065298 File Offset: 0x00063498
	private void LateUpdate()
	{
		if (this.IgnoreList)
		{
			return;
		}
		if (!Mathf.Approximately(Vector3.SqrMagnitude(this.LocalPosTarget - base.transform.localPosition), 0f))
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.LocalPosTarget, this.MoveLerpSpeed * Time.deltaTime);
			return;
		}
		base.transform.localPosition = this.LocalPosTarget;
	}

	// Token: 0x0400112F RID: 4399
	public bool IgnoreList;

	// Token: 0x04001130 RID: 4400
	public float MoveLerpSpeed;

	// Token: 0x04001131 RID: 4401
	[NonSerialized]
	public Vector3 LocalPosTarget;
}
