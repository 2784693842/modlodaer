using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000A8 RID: 168
	internal interface IMp3FrameDecompressor : IDisposable
	{
		// Token: 0x0600040C RID: 1036
		int DecompressFrame(Mp3Frame frame, byte[] dest, int destOffset);

		// Token: 0x0600040D RID: 1037
		void Reset();

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600040E RID: 1038
		WaveFormat OutputFormat { get; }
	}
}
