using System;

// Token: 0x020001CA RID: 458
public struct SpawningLiquid
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00065B8D File Offset: 0x00063D8D
	public bool UseDefault
	{
		get
		{
			return !this.StayEmpty && !this.LiquidCard;
		}
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x00065BA7 File Offset: 0x00063DA7
	public CardData GetLiquid(CardData _Default)
	{
		if (this.StayEmpty)
		{
			return null;
		}
		if (this.LiquidCard)
		{
			return this.LiquidCard;
		}
		return _Default;
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00065BC8 File Offset: 0x00063DC8
	public static SpawningLiquid DefaultLiquid
	{
		get
		{
			return default(SpawningLiquid);
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00065BE0 File Offset: 0x00063DE0
	public static SpawningLiquid Empty
	{
		get
		{
			return new SpawningLiquid
			{
				StayEmpty = true
			};
		}
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00065BFE File Offset: 0x00063DFE
	public SpawningLiquid(CardData _WithLiquid)
	{
		this.StayEmpty = false;
		this.LiquidCard = _WithLiquid;
	}

	// Token: 0x04001144 RID: 4420
	public bool StayEmpty;

	// Token: 0x04001145 RID: 4421
	public CardData LiquidCard;
}
