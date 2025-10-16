using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200000E RID: 14
	[NullableContext(1)]
	[Nullable(0)]
	public class DebugBridge
	{
		// Token: 0x17000003 RID: 3
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002F4C File Offset: 0x0000114C
		public object info
		{
			set
			{
				string format = "[Info] {0}";
				object[] array = new object[1];
				int num = 0;
				LuaTable luaTable = value as LuaTable;
				array[num] = ((luaTable != null) ? DebugBridge.TableToString(luaTable, null) : value);
				Debug.LogFormat(format, array);
			}
		}

		// Token: 0x17000004 RID: 4
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002F80 File Offset: 0x00001180
		public object debug
		{
			set
			{
				string format = "[Debug] {0}";
				object[] array = new object[1];
				int num = 0;
				LuaTable luaTable = value as LuaTable;
				array[num] = ((luaTable != null) ? DebugBridge.TableToString(luaTable, null) : value);
				Debug.LogFormat(format, array);
			}
		}

		// Token: 0x17000005 RID: 5
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002FB4 File Offset: 0x000011B4
		public object warn
		{
			set
			{
				string format = "[Warn] {0}";
				object[] array = new object[1];
				int num = 0;
				LuaTable luaTable = value as LuaTable;
				array[num] = ((luaTable != null) ? DebugBridge.TableToString(luaTable, null) : value);
				Debug.LogWarningFormat(format, array);
			}
		}

		// Token: 0x17000006 RID: 6
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002FE8 File Offset: 0x000011E8
		public object error
		{
			set
			{
				string format = "[Error] {0}";
				object[] array = new object[1];
				int num = 0;
				LuaTable luaTable = value as LuaTable;
				array[num] = ((luaTable != null) ? DebugBridge.TableToString(luaTable, null) : value);
				Debug.LogErrorFormat(format, array);
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000301C File Offset: 0x0000121C
		public static string TableToString(LuaTable table, [Nullable(new byte[]
		{
			2,
			1
		})] List<LuaTable> cache = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			foreach (KeyValuePair<object, object> pair in table.Items())
			{
				object obj;
				object obj2;
				pair.Deconstruct(out obj, out obj2);
				object obj3 = obj;
				object obj4 = obj2;
				if (stringBuilder.Length != 1)
				{
					stringBuilder.Append(", ");
				}
				LuaTable luaTable = obj3 as LuaTable;
				if (luaTable != null && !(((cache != null) ? new bool?(cache.Contains(luaTable)) : null) ?? false))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					LuaTable table2 = luaTable;
					List<LuaTable> cache2;
					if (cache != null)
					{
						cache2 = cache.Append(table).ToList<LuaTable>();
					}
					else
					{
						(cache2 = new List<LuaTable>()).Add(table);
					}
					stringBuilder2.Append(DebugBridge.TableToString(table2, cache2));
				}
				else
				{
					stringBuilder.Append(obj3);
				}
				stringBuilder.Append(" : ");
				LuaTable luaTable2 = obj4 as LuaTable;
				if (luaTable2 != null && !(((cache != null) ? new bool?(cache.Contains(luaTable2)) : null) ?? false))
				{
					StringBuilder stringBuilder3 = stringBuilder;
					LuaTable table3 = luaTable2;
					List<LuaTable> cache3;
					if (cache != null)
					{
						cache3 = cache.Append(table).ToList<LuaTable>();
					}
					else
					{
						(cache3 = new List<LuaTable>()).Add(table);
					}
					stringBuilder3.Append(DebugBridge.TableToString(table3, cache3));
				}
				else
				{
					stringBuilder.Append(obj4);
				}
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}
	}
}
