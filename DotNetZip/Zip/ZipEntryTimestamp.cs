using System;

namespace Ionic.Zip
{
	// Token: 0x02000047 RID: 71
	[Flags]
	public enum ZipEntryTimestamp
	{
		// Token: 0x04000212 RID: 530
		None = 0,
		// Token: 0x04000213 RID: 531
		DOS = 1,
		// Token: 0x04000214 RID: 532
		Windows = 2,
		// Token: 0x04000215 RID: 533
		Unix = 4,
		// Token: 0x04000216 RID: 534
		InfoZip1 = 8
	}
}
