using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
[Serializable]
public struct TagOnBoardCondition
{
	// Token: 0x060007C9 RID: 1993 RVA: 0x0004D154 File Offset: 0x0004B354
	public bool IsIdentical(TagOnBoardCondition _To)
	{
		return _To.TriggerTag == this.TriggerTag && _To.NotInHand == this.NotInHand && _To.ExcludeInventories == this.ExcludeInventories && _To.Inverted == this.Inverted && _To.OnlyInHand == this.OnlyInHand;
	}

	// Token: 0x04000B7D RID: 2941
	public CardTag TriggerTag;

	// Token: 0x04000B7E RID: 2942
	[Tooltip("If Only In Hand => Cards in inventories DON'T count unless the card has 'In Hand When Equipped' checked\nOtherwise => Depends on 'Inv!'")]
	public bool OnlyInHand;

	// Token: 0x04000B7F RID: 2943
	public bool NotInHand;

	// Token: 0x04000B80 RID: 2944
	public bool ExcludeInventories;

	// Token: 0x04000B81 RID: 2945
	public bool Inverted;
}
