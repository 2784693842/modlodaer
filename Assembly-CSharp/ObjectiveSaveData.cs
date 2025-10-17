using System;

// Token: 0x0200010C RID: 268
[Serializable]
public class ObjectiveSaveData
{
	// Token: 0x04000D02 RID: 3330
	public string UniqueID;

	// Token: 0x04000D03 RID: 3331
	public bool NotifiedWhenHidden;

	// Token: 0x04000D04 RID: 3332
	public bool HasBeenCheckedNew;

	// Token: 0x04000D05 RID: 3333
	public bool HasBeenCheckedComplete;

	// Token: 0x04000D06 RID: 3334
	public bool Complete;

	// Token: 0x04000D07 RID: 3335
	public SubObjectiveSaveData[] SubObjectives;
}
