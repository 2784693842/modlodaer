using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x0200006A RID: 106
	internal interface IMode
	{
		// Token: 0x0600021D RID: 541
		void Init(IPacket packet, int channels, int block0Size, int block1Size, IMapping[] mappings);

		// Token: 0x0600021E RID: 542
		bool Decode(IPacket packet, float[][] buffer, out int packetStartindex, out int packetValidLength, out int packetTotalLength);

		// Token: 0x0600021F RID: 543
		int GetPacketSampleCount(IPacket packet);
	}
}
