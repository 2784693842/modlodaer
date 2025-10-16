using System;
using System.IO;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000056 RID: 86
	internal interface IStreamDecoder : IDisposable
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600016F RID: 367
		int Channels { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000170 RID: 368
		int SampleRate { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000171 RID: 369
		int UpperBitrate { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000172 RID: 370
		int NominalBitrate { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000173 RID: 371
		int LowerBitrate { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000174 RID: 372
		ITagData Tags { get; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000175 RID: 373
		TimeSpan TotalTime { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000176 RID: 374
		long TotalSamples { get; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000177 RID: 375
		// (set) Token: 0x06000178 RID: 376
		TimeSpan TimePosition { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000179 RID: 377
		// (set) Token: 0x0600017A RID: 378
		long SamplePosition { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600017B RID: 379
		// (set) Token: 0x0600017C RID: 380
		bool ClipSamples { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600017D RID: 381
		bool HasClipped { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600017E RID: 382
		bool IsEndOfStream { get; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600017F RID: 383
		IStreamStats Stats { get; }

		// Token: 0x06000180 RID: 384
		void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = SeekOrigin.Begin);

		// Token: 0x06000181 RID: 385
		void SeekTo(long samplePosition, SeekOrigin seekOrigin = SeekOrigin.Begin);

		// Token: 0x06000182 RID: 386
		int Read(Span<float> buffer, int offset, int count);
	}
}
