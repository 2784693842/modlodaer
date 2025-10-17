using System;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class ActiveIfPhone : MonoBehaviour
{
	// Token: 0x060009B4 RID: 2484 RVA: 0x00059126 File Offset: 0x00057326
	private void Update()
	{
		GraphicsManager.SetActiveGroup(this.ActiveObjects, MobilePlatformDetection.IsMobilePlatform);
		GraphicsManager.SetActiveGroup(this.InactiveObjects, !MobilePlatformDetection.IsMobilePlatform);
	}

	// Token: 0x04000F33 RID: 3891
	public GameObject[] ActiveObjects;

	// Token: 0x04000F34 RID: 3892
	public GameObject[] InactiveObjects;
}
