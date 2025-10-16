using System;

namespace Ionic.Zip
{
	// Token: 0x02000049 RID: 73
	public enum ZipEntrySource
	{
		// Token: 0x0400021B RID: 539
		None,
		// Token: 0x0400021C RID: 540
		FileSystem,
		// Token: 0x0400021D RID: 541
		Stream,
		// Token: 0x0400021E RID: 542
		ZipFile,
		// Token: 0x0400021F RID: 543
		WriteDelegate,
		// Token: 0x04000220 RID: 544
		JitStream,
		// Token: 0x04000221 RID: 545
		ZipOutputStream
	}
}
