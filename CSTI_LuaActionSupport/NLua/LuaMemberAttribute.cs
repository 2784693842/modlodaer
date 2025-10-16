using System;
using System.Linq;
using System.Reflection;

namespace NLua
{
	// Token: 0x02000094 RID: 148
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	internal sealed class LuaMemberAttribute : Attribute
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0001222A File Offset: 0x0001042A
		// (set) Token: 0x06000475 RID: 1141 RVA: 0x00012232 File Offset: 0x00010432
		public string Name { get; set; }

		// Token: 0x06000476 RID: 1142 RVA: 0x0001223C File Offset: 0x0001043C
		public static MethodInfo[] GetMethodsForType(Type type, string methodName, BindingFlags bindingFlags, Type[] signature)
		{
			return type.GetMethods(bindingFlags).Where(delegate(MethodInfo m)
			{
				if (m.GetCustomAttribute<LuaHideAttribute>() != null)
				{
					return false;
				}
				if (m.GetCustomAttribute<LuaMemberAttribute>() != null)
				{
					if (m.GetCustomAttribute<LuaMemberAttribute>().Name == methodName)
					{
						return (from p in m.GetParameters()
						select p.ParameterType).SequenceEqual(signature);
					}
					return false;
				}
				else
				{
					if (m.Name == methodName)
					{
						return (from p in m.GetParameters()
						select p.ParameterType).SequenceEqual(signature);
					}
					return false;
				}
			}).ToArray<MethodInfo>();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001227C File Offset: 0x0001047C
		public static MethodInfo[] GetMethodsForType(Type type, string methodName, BindingFlags bindingFlags)
		{
			return type.GetMethods(bindingFlags).Where(delegate(MethodInfo m)
			{
				if (m.GetCustomAttribute<LuaHideAttribute>() != null)
				{
					return false;
				}
				if (m.GetCustomAttribute<LuaMemberAttribute>() != null)
				{
					return m.GetCustomAttribute<LuaMemberAttribute>().Name == methodName;
				}
				return m.Name == methodName;
			}).ToArray<MethodInfo>();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000122B4 File Offset: 0x000104B4
		public static MemberInfo[] GetMembersForType(Type type, string memberName, BindingFlags bindingFlags)
		{
			return type.GetMembers(bindingFlags).Where(delegate(MemberInfo m)
			{
				if (m.GetCustomAttribute<LuaHideAttribute>() != null)
				{
					return false;
				}
				if (m.GetCustomAttribute<LuaMemberAttribute>() != null)
				{
					return m.GetCustomAttribute<LuaMemberAttribute>().Name == memberName;
				}
				return m.Name == memberName;
			}).ToArray<MemberInfo>();
		}
	}
}
