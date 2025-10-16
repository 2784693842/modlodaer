using System;
using System.Collections.Generic;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x0200005B RID: 91
	internal interface IContainerReader : IDisposable
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001D6 RID: 470
		// (set) Token: 0x060001D7 RID: 471
		NewStreamHandler NewStreamCallback { get; set; }

		// Token: 0x060001D8 RID: 472
		IReadOnlyList<IPacketProvider> GetStreams();

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001D9 RID: 473
		bool CanSeek { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001DA RID: 474
		long ContainerBits { get; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001DB RID: 475
		long WasteBits { get; }

		// Token: 0x060001DC RID: 476
		bool TryInit();

		// Token: 0x060001DD RID: 477
		bool FindNextStream();
	}
}
