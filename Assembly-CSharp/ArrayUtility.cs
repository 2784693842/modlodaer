using System;
using System.Collections.Generic;

// Token: 0x020001DE RID: 478
public static class ArrayUtility
{
	// Token: 0x06000CBD RID: 3261 RVA: 0x000683A2 File Offset: 0x000665A2
	public static bool IsEmpty<T>(T[] _Target)
	{
		return _Target == null || _Target.Length == 0;
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x000683AE File Offset: 0x000665AE
	public static bool IsEmpty<T>(List<T> _Target)
	{
		return _Target == null || _Target.Count == 0;
	}
}
