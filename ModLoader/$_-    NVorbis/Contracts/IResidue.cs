using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000067 RID: 103
	internal interface IResidue
	{
		// Token: 0x06000218 RID: 536
		void Init(IPacket packet, int channels, ICodebook[] codebooks);

		// Token: 0x06000219 RID: 537
		void Decode(IPacket packet, bool[] doNotDecodeChannel, int blockSize, float[][] buffer);
	}
}
