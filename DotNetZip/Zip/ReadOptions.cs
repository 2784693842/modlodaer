using System;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
	// Token: 0x0200004F RID: 79
	public class ReadOptions
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00017822 File Offset: 0x00015A22
		// (set) Token: 0x06000396 RID: 918 RVA: 0x0001782A File Offset: 0x00015A2A
		public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00017833 File Offset: 0x00015A33
		// (set) Token: 0x06000398 RID: 920 RVA: 0x0001783B File Offset: 0x00015A3B
		public TextWriter StatusMessageWriter { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00017844 File Offset: 0x00015A44
		// (set) Token: 0x0600039A RID: 922 RVA: 0x0001784C File Offset: 0x00015A4C
		public Encoding Encoding { get; set; }
	}
}
