using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000061 RID: 97
	internal interface IFactory
	{
		// Token: 0x060001FE RID: 510
		ICodebook CreateCodebook();

		// Token: 0x060001FF RID: 511
		IFloor CreateFloor(IPacket packet);

		// Token: 0x06000200 RID: 512
		IResidue CreateResidue(IPacket packet);

		// Token: 0x06000201 RID: 513
		IMapping CreateMapping(IPacket packet);

		// Token: 0x06000202 RID: 514
		IMode CreateMode();

		// Token: 0x06000203 RID: 515
		IMdct CreateMdct();

		// Token: 0x06000204 RID: 516
		IHuffman CreateHuffman();
	}
}
