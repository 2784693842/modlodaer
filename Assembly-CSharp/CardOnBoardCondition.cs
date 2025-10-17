using System;
using UnityEngine;

// Token: 0x020000DA RID: 218
[Serializable]
public struct CardOnBoardCondition
{
	// Token: 0x060007C8 RID: 1992 RVA: 0x0004D0F8 File Offset: 0x0004B2F8
	public bool IsIdentical(CardOnBoardCondition _To)
	{
		return _To.TriggerCard == this.TriggerCard && _To.NotInHand == this.NotInHand && _To.ExcludeInventories == this.ExcludeInventories && _To.Inverted == this.Inverted && _To.OnlyInHand == this.OnlyInHand;
	}

	// Token: 0x04000B78 RID: 2936
	public CardData TriggerCard;

	// Token: 0x04000B79 RID: 2937
	[Tooltip("If Only In Hand => Cards in inventories DON'T count unless the card has 'In Hand When Equipped' checked\nOtherwise => Depends on 'Inv!'")]
	public bool OnlyInHand;

	// Token: 0x04000B7A RID: 2938
	public bool NotInHand;

	// Token: 0x04000B7B RID: 2939
	public bool ExcludeInventories;

	// Token: 0x04000B7C RID: 2940
	public bool Inverted;
}
