using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200001F RID: 31
	[NullableContext(1)]
	[Nullable(0)]
	public class PackCollectionEnum : IEnumerable<object>, IEnumerable
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00004B03 File Offset: 0x00002D03
		public PackCollectionEnum(ICollection collection)
		{
			this.Collection = collection;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004B12 File Offset: 0x00002D12
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			foreach (object obj in this)
			{
				yield return obj;
			}
			yield break;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004B21 File Offset: 0x00002D21
		public IEnumerator GetEnumerator()
		{
			return this.Collection.GetEnumerator();
		}

		// Token: 0x04000041 RID: 65
		private readonly ICollection Collection;
	}
}
