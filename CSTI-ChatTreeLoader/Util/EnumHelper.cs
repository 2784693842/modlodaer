using System;
using System.Collections;

namespace ChatTreeLoader.Util
{
	// Token: 0x02000005 RID: 5
	public static class EnumHelper
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000213B File Offset: 0x0000033B
		public static IEnumerator OnEnd(this IEnumerator iEnumerator, Action action)
		{
			while (iEnumerator.MoveNext())
			{
				object obj = iEnumerator.Current;
				yield return obj;
			}
			action();
			yield break;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002151 File Offset: 0x00000351
		public static IEnumerator OnStart(this IEnumerator e1, Action action)
		{
			action();
			while (e1.MoveNext())
			{
				object obj = e1.Current;
				yield return obj;
			}
			yield break;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002167 File Offset: 0x00000367
		public static IEnumerator OnEnd(this IEnumerator iEnumerator, IEnumerator enumerator)
		{
			while (iEnumerator.MoveNext())
			{
				object obj = iEnumerator.Current;
				yield return obj;
			}
			while (enumerator.MoveNext())
			{
				object obj2 = enumerator.Current;
				yield return obj2;
			}
			yield break;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000217D File Offset: 0x0000037D
		public static IEnumerator OnStart(this IEnumerator e1, IEnumerator e2)
		{
			while (e2.MoveNext())
			{
				object obj = e2.Current;
				yield return obj;
			}
			while (e1.MoveNext())
			{
				object obj2 = e1.Current;
				yield return obj2;
			}
			yield break;
		}
	}
}
