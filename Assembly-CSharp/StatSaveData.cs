using System;
using System.Collections.Generic;

// Token: 0x0200002E RID: 46
[Serializable]
public class StatSaveData
{
	// Token: 0x04000270 RID: 624
	public string StatID;

	// Token: 0x04000271 RID: 625
	public float BaseValue;

	// Token: 0x04000272 RID: 626
	public float BaseRate;

	// Token: 0x04000273 RID: 627
	public bool Pinned;

	// Token: 0x04000274 RID: 628
	public List<StatusDurationData> StatusDurations;

	// Token: 0x04000275 RID: 629
	public List<StalenessData> StaleActions;
}
