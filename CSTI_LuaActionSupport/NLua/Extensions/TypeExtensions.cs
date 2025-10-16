using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NLua.Extensions
{
	// Token: 0x0200008E RID: 142
	internal static class TypeExtensions
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x00011D34 File Offset: 0x0000FF34
		public static bool HasMethod(this Type t, string name)
		{
			return t.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).Any((MethodInfo m) => m.Name == name);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00011D67 File Offset: 0x0000FF67
		public static bool HasAdditionOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_Addition");
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00011D7E File Offset: 0x0000FF7E
		public static bool HasSubtractionOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_Subtraction");
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00011D95 File Offset: 0x0000FF95
		public static bool HasMultiplyOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_Multiply");
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00011DAC File Offset: 0x0000FFAC
		public static bool HasDivisionOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_Division");
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00011DC3 File Offset: 0x0000FFC3
		public static bool HasModulusOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_Modulus");
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00011DDA File Offset: 0x0000FFDA
		public static bool HasUnaryNegationOperator(this Type t)
		{
			return t.IsPrimitive || t.GetMethod("op_UnaryNegation", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public) != null;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00011DF9 File Offset: 0x0000FFF9
		public static bool HasEqualityOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_Equality");
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011E10 File Offset: 0x00010010
		public static bool HasLessThanOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_LessThan");
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011E27 File Offset: 0x00010027
		public static bool HasLessThanOrEqualOperator(this Type t)
		{
			return t.IsPrimitive || t.HasMethod("op_LessThanOrEqual");
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00011E40 File Offset: 0x00010040
		public static MethodInfo[] GetMethods(this Type t, string name, BindingFlags flags)
		{
			return (from m in t.GetMethods(flags)
			where m.Name == name
			select m).ToArray<MethodInfo>();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00011E78 File Offset: 0x00010078
		public static MethodInfo[] GetExtensionMethods(this Type type, string name, IEnumerable<Assembly> assemblies = null)
		{
			List<Type> list = new List<Type>();
			list.AddRange(from t in type.Assembly.GetTypes()
			where t.IsPublic
			select t);
			if (assemblies != null)
			{
				foreach (Assembly assembly in assemblies)
				{
					if (!(assembly == type.Assembly))
					{
						list.AddRange(from t in assembly.GetTypes()
						where t.IsPublic && t.IsClass && t.IsSealed && t.IsAbstract && !t.IsNested
						select t);
					}
				}
			}
			return (from extensionType in list
			from method in extensionType.GetMethods(name, BindingFlags.Static | BindingFlags.Public)
			select new NLua.dll!<>f__AnonymousType0<Type, MethodInfo>(extensionType, method) into t
			where t.method.IsDefined(typeof(ExtensionAttribute), false)
			where t.method.GetParameters()[0].ParameterType == type || t.method.GetParameters()[0].ParameterType.IsAssignableFrom(type) || type.GetInterfaces().Contains(t.method.GetParameters()[0].ParameterType)
			select t.method).ToArray<MethodInfo>();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00011FE4 File Offset: 0x000101E4
		public static MethodInfo GetExtensionMethod(this Type t, string name, IEnumerable<Assembly> assemblies = null)
		{
			MethodInfo[] array = t.GetExtensionMethods(name, assemblies).ToArray<MethodInfo>();
			if (array.Length == 0)
			{
				return null;
			}
			return array[0];
		}
	}
}
