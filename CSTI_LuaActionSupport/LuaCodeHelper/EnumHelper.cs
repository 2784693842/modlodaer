using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	[Nullable(0)]
	public static class EnumHelper
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public static IEnumerator Prepend(this IEnumerator enumerator, IEnumerator other)
		{
			while (other.MoveNext())
			{
				object obj = other.Current;
				yield return obj;
			}
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				yield return obj2;
			}
			yield break;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004BC6 File Offset: 0x00002DC6
		public static IEnumerator Concat(this IEnumerator enumerator, IEnumerator other)
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				yield return obj;
			}
			while (other.MoveNext())
			{
				object obj2 = other.Current;
				yield return obj2;
			}
			yield break;
		}
	}
}
