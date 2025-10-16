using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000BA RID: 186
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct AcmDriverDetails
	{
		// Token: 0x04000456 RID: 1110
		public int structureSize;

		// Token: 0x04000457 RID: 1111
		public uint fccType;

		// Token: 0x04000458 RID: 1112
		public uint fccComp;

		// Token: 0x04000459 RID: 1113
		public ushort manufacturerId;

		// Token: 0x0400045A RID: 1114
		public ushort productId;

		// Token: 0x0400045B RID: 1115
		public uint acmVersion;

		// Token: 0x0400045C RID: 1116
		public uint driverVersion;

		// Token: 0x0400045D RID: 1117
		public AcmDriverDetailsSupportFlags supportFlags;

		// Token: 0x0400045E RID: 1118
		public int formatTagsCount;

		// Token: 0x0400045F RID: 1119
		public int filterTagsCount;

		// Token: 0x04000460 RID: 1120
		public IntPtr hicon;

		// Token: 0x04000461 RID: 1121
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string shortName;

		// Token: 0x04000462 RID: 1122
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string longName;

		// Token: 0x04000463 RID: 1123
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string copyright;

		// Token: 0x04000464 RID: 1124
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string licensing;

		// Token: 0x04000465 RID: 1125
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string features;

		// Token: 0x04000466 RID: 1126
		private const int ShortNameChars = 32;

		// Token: 0x04000467 RID: 1127
		private const int LongNameChars = 128;

		// Token: 0x04000468 RID: 1128
		private const int CopyrightChars = 80;

		// Token: 0x04000469 RID: 1129
		private const int LicensingChars = 128;

		// Token: 0x0400046A RID: 1130
		private const int FeaturesChars = 512;
	}
}
