using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils
{
	// Token: 0x02000043 RID: 67
	internal static class MarshalHelpers
	{
		// Token: 0x06000138 RID: 312 RVA: 0x0000B897 File Offset: 0x00009A97
		public static int SizeOf<T>()
		{
			return Marshal.SizeOf(typeof(T));
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000B8A8 File Offset: 0x00009AA8
		public static IntPtr OffsetOf<T>(string fieldName)
		{
			return Marshal.OffsetOf(typeof(T), fieldName);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000B8BA File Offset: 0x00009ABA
		public static T PtrToStructure<T>(IntPtr pointer)
		{
			return (T)((object)Marshal.PtrToStructure(pointer, typeof(T)));
		}
	}
}
