using System;

// Token: 0x02000030 RID: 48
[Serializable]
public struct StalenessData
{
	// Token: 0x06000245 RID: 581 RVA: 0x0001729F File Offset: 0x0001549F
	public StalenessData(string _Source, int _Tick)
	{
		this.ModifierSource = _Source;
		this.Quantity = 0;
		this.LastTick = _Tick;
	}

	// Token: 0x04000279 RID: 633
	public string ModifierSource;

	// Token: 0x0400027A RID: 634
	public int Quantity;

	// Token: 0x0400027B RID: 635
	public int LastTick;
}
