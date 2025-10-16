using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000A7 RID: 167
	internal class Mp3Index
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x00014BE3 File Offset: 0x00012DE3
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x00014BEB File Offset: 0x00012DEB
		public long FilePosition { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00014BF4 File Offset: 0x00012DF4
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x00014BFC File Offset: 0x00012DFC
		public long SamplePosition { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00014C05 File Offset: 0x00012E05
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00014C0D File Offset: 0x00012E0D
		public int SampleCount { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00014C16 File Offset: 0x00012E16
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00014C1E File Offset: 0x00012E1E
		public int ByteCount { get; set; }
	}
}
