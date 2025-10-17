using System;
using UnityEngine;

// Token: 0x0200015D RID: 349
public class ActiveIfCheats : MonoBehaviour
{
	// Token: 0x060009B2 RID: 2482 RVA: 0x000590B0 File Offset: 0x000572B0
	private void LateUpdate()
	{
		bool flag = this.DevBuildCountsAsCheats && Debug.isDebugBuild;
		if (!this.CheatsM)
		{
			this.CheatsM = MBSingleton<CheatsManager>.Instance;
		}
		if (this.CheatsM)
		{
			flag |= this.CheatsM.CheatsActive;
		}
		GraphicsManager.SetActiveGroup(this.ControlledObjects, (flag && !this.InactiveIfCheats) || (!flag && this.InactiveIfCheats));
	}

	// Token: 0x04000F2F RID: 3887
	[SerializeField]
	private GameObject[] ControlledObjects;

	// Token: 0x04000F30 RID: 3888
	[SerializeField]
	private bool InactiveIfCheats;

	// Token: 0x04000F31 RID: 3889
	[SerializeField]
	private bool DevBuildCountsAsCheats;

	// Token: 0x04000F32 RID: 3890
	private CheatsManager CheatsM;
}
