using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FE RID: 254
[Serializable]
public class LiquidVisuals
{
	// Token: 0x06000853 RID: 2131 RVA: 0x00052611 File Offset: 0x00050811
	public bool UseSprite(InGameCardBase _ForLiquid)
	{
		return _ForLiquid && this.UseSprite(_ForLiquid.CardModel);
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00052629 File Offset: 0x00050829
	public bool UseSprite(CardData _ForLiquid)
	{
		return _ForLiquid && this.LiquidCards.Contains(_ForLiquid) && this.LiquidImage;
	}

	// Token: 0x04000CA7 RID: 3239
	public List<CardData> LiquidCards;

	// Token: 0x04000CA8 RID: 3240
	public Sprite LiquidImage;
}
