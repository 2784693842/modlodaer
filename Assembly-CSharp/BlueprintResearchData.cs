using System;

// Token: 0x02000185 RID: 389
[Serializable]
public struct BlueprintResearchData
{
	// Token: 0x06000A56 RID: 2646 RVA: 0x0005BE59 File Offset: 0x0005A059
	public BlueprintResearchData(string _ID, int _Counter)
	{
		this.BlueprintID = _ID;
		this.TickCounter = _Counter;
	}

	// Token: 0x04001000 RID: 4096
	public string BlueprintID;

	// Token: 0x04001001 RID: 4097
	public int TickCounter;
}
