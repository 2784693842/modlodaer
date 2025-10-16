using System;
using System.IO;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x02000045 RID: 69
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal class AdpcmWaveFormat : WaveFormat
	{
		// Token: 0x06000141 RID: 321 RVA: 0x0000BA5A File Offset: 0x00009C5A
		private AdpcmWaveFormat() : this(8000, 1)
		{
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000BA68 File Offset: 0x00009C68
		public int SamplesPerBlock
		{
			get
			{
				return (int)this.samplesPerBlock;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000BA70 File Offset: 0x00009C70
		public int NumCoefficients
		{
			get
			{
				return (int)this.numCoeff;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000BA78 File Offset: 0x00009C78
		public short[] Coefficients
		{
			get
			{
				return this.coefficients;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000BA80 File Offset: 0x00009C80
		public AdpcmWaveFormat(int sampleRate, int channels) : base(sampleRate, 0, channels)
		{
			this.waveFormatTag = WaveFormatEncoding.Adpcm;
			this.extraSize = 32;
			int sampleRate2 = this.sampleRate;
			if (sampleRate2 <= 11025)
			{
				if (sampleRate2 == 8000 || sampleRate2 == 11025)
				{
					this.blockAlign = 256;
					goto IL_70;
				}
			}
			else
			{
				if (sampleRate2 == 22050)
				{
					this.blockAlign = 512;
					goto IL_70;
				}
				if (sampleRate2 != 44100)
				{
				}
			}
			this.blockAlign = 1024;
			IL_70:
			this.bitsPerSample = 4;
			this.samplesPerBlock = (short)(((int)this.blockAlign - 7 * channels) * 8 / ((int)this.bitsPerSample * channels) + 2);
			this.averageBytesPerSecond = base.SampleRate * (int)this.blockAlign / (int)this.samplesPerBlock;
			this.numCoeff = 7;
			this.coefficients = new short[]
			{
				256,
				0,
				512,
				-256,
				0,
				0,
				192,
				64,
				240,
				0,
				460,
				-208,
				392,
				-232
			};
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000BB5C File Offset: 0x00009D5C
		public override void Serialize(BinaryWriter writer)
		{
			base.Serialize(writer);
			writer.Write(this.samplesPerBlock);
			writer.Write(this.numCoeff);
			foreach (short value in this.coefficients)
			{
				writer.Write(value);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		public override string ToString()
		{
			return string.Format("Microsoft ADPCM {0} Hz {1} channels {2} bits per sample {3} samples per block", new object[]
			{
				base.SampleRate,
				this.channels,
				this.bitsPerSample,
				this.samplesPerBlock
			});
		}

		// Token: 0x04000241 RID: 577
		private short samplesPerBlock;

		// Token: 0x04000242 RID: 578
		private short numCoeff;

		// Token: 0x04000243 RID: 579
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
		private short[] coefficients;
	}
}
