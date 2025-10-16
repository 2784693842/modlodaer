using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using NLua;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	public static class LuaRegistrable
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00004D2C File Offset: 0x00002F2C
		public static void Register<[Nullable(0)] T>(this Lua lua, [Nullable(2)] string basename = null) where T : struct, Enum
		{
			Type typeFromHandle = typeof(T);
			if (basename == null)
			{
				basename = typeFromHandle.Name;
			}
			if (lua.GetObjectFromPath(basename) != null)
			{
				return;
			}
			lua.NewTable(basename);
			LuaTable table = lua.GetTable(basename);
			foreach (object value in Enum.GetValues(typeFromHandle))
			{
				table[Enum.GetName(typeFromHandle, value)] = value;
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004DBC File Offset: 0x00002FBC
		public static void Register(this Lua LuaRuntime, Type typeInfo, [Nullable(2)] string basePath = null)
		{
			List<MethodInfo> declaredMethods = AccessTools.GetDeclaredMethods(typeInfo);
			if (basePath != null && LuaRuntime.GetTable(basePath) == null)
			{
				LuaRuntime.NewTable(basePath);
			}
			foreach (MethodInfo methodInfo in declaredMethods)
			{
				if (methodInfo.IsStatic)
				{
					CustomAttributeData customAttributeData = methodInfo.CustomAttributes.FirstOrDefault((CustomAttributeData data) => data.AttributeType == typeof(LuaFuncAttribute));
					if (customAttributeData != null)
					{
						string str = (basePath == null) ? "" : (basePath + ".");
						IList<CustomAttributeNamedArgument> namedArguments = customAttributeData.NamedArguments;
						object obj;
						if (namedArguments == null)
						{
							obj = null;
						}
						else
						{
							obj = namedArguments.FirstOrDefault((CustomAttributeNamedArgument namedArgument) => namedArgument.MemberName == "FuncName").TypedValue.Value;
						}
						object obj2 = obj;
						object obj3 = (obj2 != null) ? obj2 : methodInfo.Name;
						LuaRuntime.RegisterFunction(str + ((obj3 != null) ? obj3.ToString() : null), null, methodInfo);
					}
				}
			}
		}
	}
}
