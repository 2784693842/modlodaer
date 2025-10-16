using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x0200005D RID: 93
	internal interface IPacketProvider
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001E2 RID: 482
		bool CanSeek { get; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001E3 RID: 483
		int StreamSerial { get; }

		// Token: 0x060001E4 RID: 484
		IPacket GetNextPacket();

		// Token: 0x060001E5 RID: 485
		IPacket PeekNextPacket();

		// Token: 0x060001E6 RID: 486
		long SeekTo(long granulePos, int preRoll, GetPacketGranuleCount getPacketGranuleCount);

		// Token: 0x060001E7 RID: 487
		long GetGranuleCount();
	}
}
