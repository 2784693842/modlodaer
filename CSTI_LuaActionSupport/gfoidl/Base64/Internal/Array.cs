using System;
using System.Runtime.CompilerServices;

namespace gfoidl.Base64.Internal
{
	// Token: 0x020000DF RID: 223
	internal static class Array
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x00030403 File Offset: 0x0002E603
		[NullableContext(1)]
		public static T[] Empty<[Nullable(2)] T>()
		{
			return Array.EmptyArray<T>.s_value;
		}

		// Token: 0x020000E0 RID: 224
		private static class EmptyArray<[Nullable(2)] T>
		{
			// Token: 0x04000275 RID: 629
			[Nullable(1)]
			public static readonly T[] s_value = new T[0];
		}
	}
}
