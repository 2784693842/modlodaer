using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000BE RID: 190
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct AcmFormatDetails
	{
		// Token: 0x0400048E RID: 1166
		public int structSize;

		// Token: 0x0400048F RID: 1167
		public int formatIndex;

		// Token: 0x04000490 RID: 1168
		public int formatTag;

		// Token: 0x04000491 RID: 1169
		public AcmDriverDetailsSupportFlags supportFlags;

		// Token: 0x04000492 RID: 1170
		public IntPtr waveFormatPointer;

		// Token: 0x04000493 RID: 1171
		public int waveFormatByteSize;

		// Token: 0x04000494 RID: 1172
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string formatDescription;

		// Token: 0x04000495 RID: 1173
		public const int FormatDescriptionChars = 128;
	}
}
