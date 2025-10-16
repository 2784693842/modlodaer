using System;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x0200006B RID: 107
	internal class Factory : IFactory
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000C7B5 File Offset: 0x0000A9B5
		public IHuffman CreateHuffman()
		{
			return new Huffman();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C7BC File Offset: 0x0000A9BC
		public IMdct CreateMdct()
		{
			return new Mdct();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C7C3 File Offset: 0x0000A9C3
		public ICodebook CreateCodebook()
		{
			return new Codebook();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000C7CC File Offset: 0x0000A9CC
		public IFloor CreateFloor(IPacket packet)
		{
			int num = (int)packet.ReadBits(16);
			if (num == 0)
			{
				return new Floor0();
			}
			if (num != 1)
			{
				throw new InvalidDataException("Invalid floor type!");
			}
			return new Floor1();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000C802 File Offset: 0x0000AA02
		public IMapping CreateMapping(IPacket packet)
		{
			if (packet.ReadBits(16) != 0UL)
			{
				throw new InvalidDataException("Invalid mapping type!");
			}
			return new Mapping();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000C81E File Offset: 0x0000AA1E
		public IMode CreateMode()
		{
			return new Mode();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000C828 File Offset: 0x0000AA28
		public IResidue CreateResidue(IPacket packet)
		{
			switch ((int)packet.ReadBits(16))
			{
			case 0:
				return new Residue0();
			case 1:
				return new Residue1();
			case 2:
				return new Residue2();
			default:
				throw new InvalidDataException("Invalid residue type!");
			}
		}
	}
}
