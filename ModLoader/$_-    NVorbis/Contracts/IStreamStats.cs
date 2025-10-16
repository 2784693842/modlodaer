using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000058 RID: 88
	internal interface IStreamStats
	{
		// Token: 0x06000196 RID: 406
		void ResetStats();

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000197 RID: 407
		int EffectiveBitRate { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000198 RID: 408
		int InstantBitRate { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000199 RID: 409
		long ContainerBits { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600019A RID: 410
		long OverheadBits { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600019B RID: 411
		long AudioBits { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600019C RID: 412
		long WasteBits { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600019D RID: 413
		int PacketCount { get; }
	}
}
