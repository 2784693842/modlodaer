using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NLua;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200002B RID: 43
	public static class TableHelper
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x000051DE File Offset: 0x000033DE
		[NullableContext(1)]
		public static LuaTable TempTable(this Lua lua)
		{
			lua.NewTable("__temp");
			return lua.GetTable("__temp");
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000051F8 File Offset: 0x000033F8
		[NullableContext(1)]
		[return: Nullable(2)]
		public static TVal SafeGet<[Nullable(2)] TKey, TVal>(this IDictionary<TKey, TVal> dictionary, TKey key, [Nullable(2)] object __ = null) where TVal : class
		{
			TVal result;
			if (!dictionary.TryGetValue(key, out result))
			{
				return default(TVal);
			}
			return result;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000521C File Offset: 0x0000341C
		public static TVal? SafeGet<[Nullable(2)] TKey, TVal>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] this IDictionary<TKey, TVal> dictionary, [Nullable(1)] TKey key) where TVal : struct
		{
			TVal value;
			if (!dictionary.TryGetValue(key, out value))
			{
				return null;
			}
			return new TVal?(value);
		}
	}
}
