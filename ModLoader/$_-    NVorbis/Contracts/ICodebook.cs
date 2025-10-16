using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000062 RID: 98
	internal interface ICodebook
	{
		// Token: 0x06000205 RID: 517
		void Init(IPacket packet, IHuffman huffman);

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000206 RID: 518
		int Dimensions { get; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000207 RID: 519
		int Entries { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000208 RID: 520
		int MapType { get; }

		// Token: 0x170000B1 RID: 177
		float this[int entry, int dim]
		{
			get;
		}

		// Token: 0x0600020A RID: 522
		int DecodeScalar(IPacket packet);
	}
}
