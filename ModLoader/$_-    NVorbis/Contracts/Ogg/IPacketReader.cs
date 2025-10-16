using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg
{
	// Token: 0x02000095 RID: 149
	internal interface IPacketReader
	{
		// Token: 0x060003A6 RID: 934
		Memory<byte> GetPacketData(int pagePacketIndex);

		// Token: 0x060003A7 RID: 935
		void InvalidatePacketCache(IPacket packet);
	}
}
