using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000068 RID: 104
	internal interface IMapping
	{
		// Token: 0x0600021A RID: 538
		void Init(IPacket packet, int channels, IFloor[] floors, IResidue[] residues, IMdct mdct);

		// Token: 0x0600021B RID: 539
		void DecodePacket(IPacket packet, int blockSize, int channels, float[][] buffer);
	}
}
