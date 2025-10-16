using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg
{
	// Token: 0x02000083 RID: 131
	internal interface IPageReader : IDisposable
	{
		// Token: 0x060002F6 RID: 758
		void Lock();

		// Token: 0x060002F7 RID: 759
		bool Release();

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060002F8 RID: 760
		long ContainerBits { get; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060002F9 RID: 761
		long WasteBits { get; }

		// Token: 0x060002FA RID: 762
		bool ReadNextPage();

		// Token: 0x060002FB RID: 763
		bool ReadPageAt(long offset);
	}
}
