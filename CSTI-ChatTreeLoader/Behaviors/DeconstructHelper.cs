using System;
using System.Collections.Generic;

namespace ChatTreeLoader.Behaviors
{
	// Token: 0x02000018 RID: 24
	public static class DeconstructHelper
	{
		// Token: 0x06000050 RID: 80 RVA: 0x000038FE File Offset: 0x00001AFE
		public static void Deconstruct<TKey, TVal>(this KeyValuePair<TKey, TVal> pair, out TKey key, out TVal val)
		{
			key = pair.Key;
			val = pair.Value;
		}
	}
}
