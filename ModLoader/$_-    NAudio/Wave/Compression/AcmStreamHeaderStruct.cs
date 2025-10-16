using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000AD RID: 173
	[StructLayout(LayoutKind.Sequential, Size = 128)]
	internal class AcmStreamHeaderStruct
	{
		// Token: 0x0400040D RID: 1037
		public int cbStruct;

		// Token: 0x0400040E RID: 1038
		public AcmStreamHeaderStatusFlags fdwStatus;

		// Token: 0x0400040F RID: 1039
		public IntPtr userData;

		// Token: 0x04000410 RID: 1040
		public IntPtr sourceBufferPointer;

		// Token: 0x04000411 RID: 1041
		public int sourceBufferLength;

		// Token: 0x04000412 RID: 1042
		public int sourceBufferLengthUsed;

		// Token: 0x04000413 RID: 1043
		public IntPtr sourceUserData;

		// Token: 0x04000414 RID: 1044
		public IntPtr destBufferPointer;

		// Token: 0x04000415 RID: 1045
		public int destBufferLength;

		// Token: 0x04000416 RID: 1046
		public int destBufferLengthUsed;

		// Token: 0x04000417 RID: 1047
		public IntPtr destUserData;
	}
}
