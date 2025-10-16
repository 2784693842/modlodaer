using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NLua;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200001C RID: 28
	[NullableContext(1)]
	[Nullable(0)]
	public static class DeconstructHelper
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00004A5B File Offset: 0x00002C5B
		public static void Deconstruct<[Nullable(2)] TK, [Nullable(2)] TV>([Nullable(new byte[]
		{
			0,
			1,
			1
		})] this KeyValuePair<TK, TV> pair, out TK key, out TV value)
		{
			key = pair.Key;
			value = pair.Value;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004A77 File Offset: 0x00002C77
		public static PackDictionaryEnumerable Items(this LuaTable table)
		{
			return new PackDictionaryEnumerable(table);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004A7F File Offset: 0x00002C7F
		public static PackCollectionEnum Enum(this ICollection collection)
		{
			return new PackCollectionEnum(collection);
		}
	}
}
