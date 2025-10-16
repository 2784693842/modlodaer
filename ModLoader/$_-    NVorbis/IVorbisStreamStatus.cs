using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000060 RID: 96
	[Obsolete("Moved to NVorbis.Contracts.IStreamStats", true)]
	internal interface IVorbisStreamStatus : IStreamStats
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001F8 RID: 504
		[Obsolete("No longer supported.", true)]
		TimeSpan PageLatency { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001F9 RID: 505
		[Obsolete("No longer supported.", true)]
		TimeSpan PacketLatency { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001FA RID: 506
		[Obsolete("No longer supported.", true)]
		TimeSpan SecondLatency { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001FB RID: 507
		[Obsolete("No longer supported.", true)]
		int PagesRead { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001FC RID: 508
		[Obsolete("No longer supported.", true)]
		int TotalPages { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001FD RID: 509
		[Obsolete("Use IStreamDecoder.HasClipped instead.  VorbisReader.HasClipped will return the same value for the stream it is handling.", true)]
		bool Clipped { get; }
	}
}
