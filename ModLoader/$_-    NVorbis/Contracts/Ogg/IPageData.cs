using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg
{
	// Token: 0x0200008E RID: 142
	internal interface IPageData : IPageReader, IDisposable
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000355 RID: 853
		long PageOffset { get; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000356 RID: 854
		int StreamSerial { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000357 RID: 855
		int SequenceNumber { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000358 RID: 856
		PageFlags PageFlags { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000359 RID: 857
		long GranulePosition { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600035A RID: 858
		short PacketCount { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600035B RID: 859
		bool? IsResync { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600035C RID: 860
		bool IsContinued { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600035D RID: 861
		int PageOverhead { get; }

		// Token: 0x0600035E RID: 862
		Memory<byte>[] GetPackets();
	}
}
