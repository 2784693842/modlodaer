using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ChatTreeLoader.Util
{
	// Token: 0x02000006 RID: 6
	public static class SafeRemoveUtil
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002193 File Offset: 0x00000393
		public static void SafeRemove<TKey, TVal>(this Dictionary<TKey, TVal> dictionary, [CanBeNull] TKey key)
		{
			if (key == null)
			{
				return;
			}
			if (!dictionary.ContainsKey(key))
			{
				return;
			}
			dictionary.Remove(key);
		}
	}
}
