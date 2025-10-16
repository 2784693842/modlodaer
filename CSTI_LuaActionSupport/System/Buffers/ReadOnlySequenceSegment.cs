using System;

namespace System.Buffers
{
	// Token: 0x020000BE RID: 190
	internal abstract class ReadOnlySequenceSegment<T>
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x000164C8 File Offset: 0x000146C8
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x000164D0 File Offset: 0x000146D0
		public System.ReadOnlyMemory<T> Memory { get; protected set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000164D9 File Offset: 0x000146D9
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x000164E1 File Offset: 0x000146E1
		public ReadOnlySequenceSegment<T> Next { get; protected set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x000164EA File Offset: 0x000146EA
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x000164F2 File Offset: 0x000146F2
		public long RunningIndex { get; protected set; }
	}
}
