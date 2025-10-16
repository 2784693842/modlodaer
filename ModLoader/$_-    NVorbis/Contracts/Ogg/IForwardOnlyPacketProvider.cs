using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg
{
	// Token: 0x02000089 RID: 137
	internal interface IForwardOnlyPacketProvider : IPacketProvider
	{
		// Token: 0x06000328 RID: 808
		bool AddPage(byte[] buf, bool isResync);

		// Token: 0x06000329 RID: 809
		void SetEndOfStream();
	}
}
