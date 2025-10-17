using System;
using System.Collections.Generic;

// Token: 0x0200001E RID: 30
public class InGameCardComparer : IComparer<InGameCardBase>
{
	// Token: 0x06000207 RID: 519 RVA: 0x0001500A File Offset: 0x0001320A
	public int Compare(InGameCardBase _A, InGameCardBase _B)
	{
		return CardComparer.Compare(_A, _B);
	}
}
