using System;
using System.Collections.Generic;
using System.IO;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000054 RID: 84
	internal interface IVorbisReader : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600014F RID: 335
		// (remove) Token: 0x06000150 RID: 336
		event EventHandler<NewStreamEventArgs> NewStream;

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000151 RID: 337
		long ContainerOverheadBits { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000152 RID: 338
		long ContainerWasteBits { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000153 RID: 339
		IReadOnlyList<IStreamDecoder> Streams { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000154 RID: 340
		int StreamIndex { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000155 RID: 341
		int Channels { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000156 RID: 342
		int SampleRate { get; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000157 RID: 343
		int UpperBitrate { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000158 RID: 344
		int NominalBitrate { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000159 RID: 345
		int LowerBitrate { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600015A RID: 346
		TimeSpan TotalTime { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600015B RID: 347
		long TotalSamples { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600015C RID: 348
		// (set) Token: 0x0600015D RID: 349
		bool ClipSamples { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600015E RID: 350
		// (set) Token: 0x0600015F RID: 351
		TimeSpan TimePosition { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000160 RID: 352
		// (set) Token: 0x06000161 RID: 353
		long SamplePosition { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000162 RID: 354
		bool HasClipped { get; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000163 RID: 355
		bool IsEndOfStream { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000164 RID: 356
		IStreamStats StreamStats { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000165 RID: 357
		ITagData Tags { get; }

		// Token: 0x06000166 RID: 358
		bool FindNextStream();

		// Token: 0x06000167 RID: 359
		bool SwitchStreams(int index);

		// Token: 0x06000168 RID: 360
		int ReadSamples(float[] buffer, int offset, int count);

		// Token: 0x06000169 RID: 361
		void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = SeekOrigin.Begin);

		// Token: 0x0600016A RID: 362
		void SeekTo(long samplePosition, SeekOrigin seekOrigin = SeekOrigin.Begin);
	}
}
