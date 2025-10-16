using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x0200009F RID: 159
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal class Mp3WaveFormat : WaveFormat
	{
		// Token: 0x060003D8 RID: 984 RVA: 0x000144E0 File Offset: 0x000126E0
		public Mp3WaveFormat(int sampleRate, int channels, int blockSize, int bitRate)
		{
			this.waveFormatTag = WaveFormatEncoding.MpegLayer3;
			this.channels = (short)channels;
			this.averageBytesPerSecond = bitRate / 8;
			this.bitsPerSample = 0;
			this.blockAlign = 1;
			this.sampleRate = sampleRate;
			this.extraSize = 12;
			this.id = Mp3WaveFormatId.Mpeg;
			this.flags = Mp3WaveFormatFlags.PaddingIso;
			this.blockSize = (ushort)blockSize;
			this.framesPerBlock = 1;
			this.codecDelay = 0;
		}

		// Token: 0x040003BF RID: 959
		public Mp3WaveFormatId id;

		// Token: 0x040003C0 RID: 960
		public Mp3WaveFormatFlags flags;

		// Token: 0x040003C1 RID: 961
		public ushort blockSize;

		// Token: 0x040003C2 RID: 962
		public ushort framesPerBlock;

		// Token: 0x040003C3 RID: 963
		public ushort codecDelay;

		// Token: 0x040003C4 RID: 964
		private const short Mp3WaveFormatExtraBytes = 12;
	}
}
