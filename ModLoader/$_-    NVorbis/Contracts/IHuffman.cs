using System;
using System.Collections.Generic;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000063 RID: 99
	internal interface IHuffman
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600020B RID: 523
		int TableBits { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600020C RID: 524
		IReadOnlyList<HuffmanListNode> PrefixTree { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600020D RID: 525
		IReadOnlyList<HuffmanListNode> OverflowList { get; }

		// Token: 0x0600020E RID: 526
		void GenerateTable(IReadOnlyList<int> value, int[] lengthList, int[] codeList);
	}
}
