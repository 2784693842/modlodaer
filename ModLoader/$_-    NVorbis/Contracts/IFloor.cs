using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000065 RID: 101
	internal interface IFloor
	{
		// Token: 0x06000210 RID: 528
		void Init(IPacket packet, int channels, int block0Size, int block1Size, ICodebook[] codebooks);

		// Token: 0x06000211 RID: 529
		IFloorData Unpack(IPacket packet, int blockSize, int channel);

		// Token: 0x06000212 RID: 530
		void Apply(IFloorData floorData, int blockSize, float[] residue);
	}
}
