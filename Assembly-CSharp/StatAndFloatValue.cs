using System;

// Token: 0x020001D1 RID: 465
public struct StatAndFloatValue
{
	// Token: 0x06000C7F RID: 3199 RVA: 0x00066ABD File Offset: 0x00064CBD
	public StatAndFloatValue(GameStat _Stat, float _Value)
	{
		this.Stat = _Stat;
		this.Value = _Value;
	}

	// Token: 0x04001162 RID: 4450
	public GameStat Stat;

	// Token: 0x04001163 RID: 4451
	public float Value;
}
