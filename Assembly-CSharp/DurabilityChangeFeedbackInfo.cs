using System;

// Token: 0x02000060 RID: 96
public struct DurabilityChangeFeedbackInfo
{
	// Token: 0x060003EA RID: 1002 RVA: 0x00028479 File Offset: 0x00026679
	public DurabilityChangeFeedbackInfo(DurabilitiesTypes _Type, int _Value)
	{
		this.ChangeType = _Type;
		this.ChangeValue = _Value;
	}

	// Token: 0x04000504 RID: 1284
	public DurabilitiesTypes ChangeType;

	// Token: 0x04000505 RID: 1285
	public int ChangeValue;
}
