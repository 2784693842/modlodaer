using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000C0 RID: 192
	internal struct AcmFormatTagDetails
	{
		// Token: 0x0400049C RID: 1180
		public int structureSize;

		// Token: 0x0400049D RID: 1181
		public int formatTagIndex;

		// Token: 0x0400049E RID: 1182
		public int formatTag;

		// Token: 0x0400049F RID: 1183
		public int formatSize;

		// Token: 0x040004A0 RID: 1184
		public AcmDriverDetailsSupportFlags supportFlags;

		// Token: 0x040004A1 RID: 1185
		public int standardFormatsCount;

		// Token: 0x040004A2 RID: 1186
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
		public string formatDescription;

		// Token: 0x040004A3 RID: 1187
		public const int FormatTagDescriptionChars = 48;
	}
}
