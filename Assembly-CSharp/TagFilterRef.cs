using System;

// Token: 0x02000006 RID: 6
[Serializable]
public struct TagFilterRef
{
	// Token: 0x06000018 RID: 24 RVA: 0x00002EA3 File Offset: 0x000010A3
	public TagFilterRef(CardTag _Tag, bool _NOT, bool _OnlyWithLiquid = false)
	{
		this.Tag = _Tag;
		this.NOT = _NOT;
		this.OnlyWithLiquid = _OnlyWithLiquid;
	}

	// Token: 0x04000036 RID: 54
	public CardTag Tag;

	// Token: 0x04000037 RID: 55
	public bool NOT;

	// Token: 0x04000038 RID: 56
	public bool OnlyWithLiquid;
}
