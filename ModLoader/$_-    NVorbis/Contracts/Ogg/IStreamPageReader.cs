using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg
{
	// Token: 0x02000092 RID: 146
	internal interface IStreamPageReader
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000383 RID: 899
		IPacketProvider PacketProvider { get; }

		// Token: 0x06000384 RID: 900
		void AddPage();

		// Token: 0x06000385 RID: 901
		Memory<byte>[] GetPagePackets(int pageIndex);

		// Token: 0x06000386 RID: 902
		int FindPage(long granulePos);

		// Token: 0x06000387 RID: 903
		bool GetPage(int pageIndex, out long granulePos, out bool isResync, out bool isContinuation, out bool isContinued, out int packetCount, out int pageOverhead);

		// Token: 0x06000388 RID: 904
		void SetEndOfStream();

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000389 RID: 905
		int PageCount { get; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600038A RID: 906
		bool HasAllPages { get; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600038B RID: 907
		long? MaxGranulePosition { get; }

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600038C RID: 908
		int FirstDataPageIndex { get; }
	}
}
