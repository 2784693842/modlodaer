using System;

// Token: 0x02000005 RID: 5
[Serializable]
public struct CardFilterRef
{
	// Token: 0x06000017 RID: 23 RVA: 0x00002E8C File Offset: 0x0000108C
	public CardFilterRef(CardData _Card, bool _NOT, bool _OnlyWithLiquid = false)
	{
		this.Card = _Card;
		this.NOT = _NOT;
		this.OnlyWithLiquid = _OnlyWithLiquid;
	}

	// Token: 0x04000033 RID: 51
	public CardData Card;

	// Token: 0x04000034 RID: 52
	public bool NOT;

	// Token: 0x04000035 RID: 53
	public bool OnlyWithLiquid;
}
