using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.SampleProviders
{
	// Token: 0x020000C9 RID: 201
	internal class SampleToWaveProvider16 : IWaveProvider
	{
		// Token: 0x0600049A RID: 1178 RVA: 0x0001619C File Offset: 0x0001439C
		public SampleToWaveProvider16(ISampleProvider sourceProvider)
		{
			if (sourceProvider.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
			{
				throw new ArgumentException("Input source provider must be IEEE float", "sourceProvider");
			}
			if (sourceProvider.WaveFormat.BitsPerSample != 32)
			{
				throw new ArgumentException("Input source provider must be 32 bit", "sourceProvider");
			}
			this.waveFormat = new WaveFormat(sourceProvider.WaveFormat.SampleRate, 16, sourceProvider.WaveFormat.Channels);
			this.sourceProvider = sourceProvider;
			this.volume = 1f;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00016224 File Offset: 0x00014424
		public int Read(byte[] destBuffer, int offset, int numBytes)
		{
			int num = numBytes / 2;
			this.sourceBuffer = BufferHelpers.Ensure(this.sourceBuffer, num);
			int num2 = this.sourceProvider.Read(this.sourceBuffer, 0, num);
			WaveBuffer waveBuffer = new WaveBuffer(destBuffer);
			int num3 = offset / 2;
			for (int i = 0; i < num2; i++)
			{
				float num4 = this.sourceBuffer[i] * this.volume;
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				if (num4 < -1f)
				{
					num4 = -1f;
				}
				waveBuffer.ShortBuffer[num3++] = (short)(num4 * 32767f);
			}
			return num2 * 2;
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x000162C2 File Offset: 0x000144C2
		public WaveFormat WaveFormat
		{
			get
			{
				return this.waveFormat;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x000162CA File Offset: 0x000144CA
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x000162D4 File Offset: 0x000144D4
		public float Volume
		{
			get
			{
				return this.volume;
			}
			set
			{
				this.volume = value;
			}
		}

		// Token: 0x040004D0 RID: 1232
		private readonly ISampleProvider sourceProvider;

		// Token: 0x040004D1 RID: 1233
		private readonly WaveFormat waveFormat;

		// Token: 0x040004D2 RID: 1234
		private volatile float volume;

		// Token: 0x040004D3 RID: 1235
		private float[] sourceBuffer;
	}
}
