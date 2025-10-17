using System;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class ActiveIfRecording : MonoBehaviour
{
	// Token: 0x060009B6 RID: 2486 RVA: 0x0005914B File Offset: 0x0005734B
	private void LateUpdate()
	{
		if (!Application.isEditor)
		{
			GraphicsManager.SetActiveGroup(this.ControlledObjects, this.InactiveIfRecording);
			return;
		}
	}

	// Token: 0x04000F35 RID: 3893
	[SerializeField]
	private GameObject[] ControlledObjects;

	// Token: 0x04000F36 RID: 3894
	[SerializeField]
	private bool InactiveIfRecording;

	// Token: 0x04000F37 RID: 3895
	private bool Recording;
}
