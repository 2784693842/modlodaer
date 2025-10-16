using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.AllPatcher;
using NLua;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	public static class CLR2Lua
	{
		// Token: 0x0600009C RID: 156 RVA: 0x0000472C File Offset: 0x0000292C
		public static LuaTable ToLuaTable<[Nullable(2)] TK, [Nullable(2)] TV>(this Dictionary<TK, TV> dictionary)
		{
			LuaTable luaTable = CardActionPatcher.LuaRuntime.TempTable();
			foreach (KeyValuePair<TK, TV> pair in dictionary)
			{
				TK tk;
				TV tv;
				pair.Deconstruct(out tk, out tv);
				TK tk2 = tk;
				TV tv2 = tv;
				luaTable[tk2] = tv2;
			}
			return luaTable;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000047A0 File Offset: 0x000029A0
		public static LuaTable ToLuaTable<[Nullable(2)] TItem>(this List<TItem> list)
		{
			LuaTable luaTable = CardActionPatcher.LuaRuntime.TempTable();
			for (int i = 0; i < list.Count; i++)
			{
				luaTable[i + 1] = list[i];
			}
			return luaTable;
		}
	}
}
