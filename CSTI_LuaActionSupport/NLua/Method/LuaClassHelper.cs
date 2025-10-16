using System;

namespace NLua.Method
{
	// Token: 0x020000A1 RID: 161
	internal class LuaClassHelper
	{
		// Token: 0x060004A6 RID: 1190 RVA: 0x00013B8C File Offset: 0x00011D8C
		public static LuaFunction GetTableFunction(LuaTable luaTable, string name)
		{
			if (luaTable == null)
			{
				return null;
			}
			LuaFunction luaFunction = luaTable.RawGet(name) as LuaFunction;
			if (luaFunction != null)
			{
				return luaFunction;
			}
			return null;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00013BB4 File Offset: 0x00011DB4
		public static object CallFunction(LuaFunction function, object[] args, Type[] returnTypes, object[] inArgs, int[] outArgs)
		{
			object[] array = function.Call(inArgs, returnTypes);
			if (array == null || returnTypes.Length == 0)
			{
				return null;
			}
			object result;
			int num;
			if (returnTypes[0] == typeof(void))
			{
				result = null;
				num = 0;
			}
			else
			{
				result = array[0];
				num = 1;
			}
			for (int i = 0; i < outArgs.Length; i++)
			{
				args[outArgs[i]] = array[num];
				num++;
			}
			return result;
		}
	}
}
