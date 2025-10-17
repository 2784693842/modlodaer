using System;
using System.Collections.Generic;

// Token: 0x0200001F RID: 31
public class CardSaveDataComparer : IComparer<CardSaveData>
{
	// Token: 0x06000209 RID: 521 RVA: 0x00015013 File Offset: 0x00013213
	public int Compare(CardSaveData _A, CardSaveData _B)
	{
		return CardComparer.Compare(_A, _B);
	}
}
