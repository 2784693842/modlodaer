using System;
using System.IO;
using System.Runtime.InteropServices;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Dmo;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x02000044 RID: 68
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal class WaveFormatExtensible : WaveFormat
	{
		// Token: 0x0600013B RID: 315 RVA: 0x0000B8D1 File Offset: 0x00009AD1
		private WaveFormatExtensible()
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000B8DC File Offset: 0x00009ADC
		public WaveFormatExtensible(int rate, int bits, int channels) : base(rate, bits, channels)
		{
			this.waveFormatTag = WaveFormatEncoding.Extensible;
			this.extraSize = 22;
			this.wValidBitsPerSample = (short)bits;
			for (int i = 0; i < channels; i++)
			{
				this.dwChannelMask |= 1 << i;
			}
			if (bits == 32)
			{
				this.subFormat = AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT;
				return;
			}
			this.subFormat = AudioMediaSubtypes.MEDIASUBTYPE_PCM;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000B948 File Offset: 0x00009B48
		public WaveFormat ToStandardWaveFormat()
		{
			if (this.subFormat == AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT && this.bitsPerSample == 32)
			{
				return WaveFormat.CreateIeeeFloatWaveFormat(this.sampleRate, (int)this.channels);
			}
			if (this.subFormat == AudioMediaSubtypes.MEDIASUBTYPE_PCM)
			{
				return new WaveFormat(this.sampleRate, (int)this.bitsPerSample, (int)this.channels);
			}
			return this;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000B9AE File Offset: 0x00009BAE
		public Guid SubFormat
		{
			get
			{
				return this.subFormat;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000B9B8 File Offset: 0x00009BB8
		public override void Serialize(BinaryWriter writer)
		{
			base.Serialize(writer);
			writer.Write(this.wValidBitsPerSample);
			writer.Write(this.dwChannelMask);
			byte[] array = this.subFormat.ToByteArray();
			writer.Write(array, 0, array.Length);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000B9FC File Offset: 0x00009BFC
		public override string ToString()
		{
			return string.Format("{0} wBitsPerSample:{1} dwChannelMask:{2} subFormat:{3} extraSize:{4}", new object[]
			{
				base.ToString(),
				this.wValidBitsPerSample,
				this.dwChannelMask,
				this.subFormat,
				this.extraSize
			});
		}

		// Token: 0x0400023E RID: 574
		private short wValidBitsPerSample;

		// Token: 0x0400023F RID: 575
		private int dwChannelMask;

		// Token: 0x04000240 RID: 576
		private Guid subFormat;
	}
}
