using System;
using System.IO;
using System.Runtime.InteropServices;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x0200003B RID: 59
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal class WaveFormat
	{
		// Token: 0x060000EC RID: 236 RVA: 0x0000AAB3 File Offset: 0x00008CB3
		public WaveFormat() : this(44100, 16, 2)
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000AAC3 File Offset: 0x00008CC3
		public WaveFormat(int sampleRate, int channels) : this(sampleRate, 16, channels)
		{
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000AAD0 File Offset: 0x00008CD0
		public int ConvertLatencyToByteSize(int milliseconds)
		{
			int num = (int)((double)this.AverageBytesPerSecond / 1000.0 * (double)milliseconds);
			if (num % this.BlockAlign != 0)
			{
				num = num + this.BlockAlign - num % this.BlockAlign;
			}
			return num;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000AB10 File Offset: 0x00008D10
		public static WaveFormat CreateCustomFormat(WaveFormatEncoding tag, int sampleRate, int channels, int averageBytesPerSecond, int blockAlign, int bitsPerSample)
		{
			return new WaveFormat
			{
				waveFormatTag = tag,
				channels = (short)channels,
				sampleRate = sampleRate,
				averageBytesPerSecond = averageBytesPerSecond,
				blockAlign = (short)blockAlign,
				bitsPerSample = (short)bitsPerSample,
				extraSize = 0
			};
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000AB4D File Offset: 0x00008D4D
		public static WaveFormat CreateALawFormat(int sampleRate, int channels)
		{
			return WaveFormat.CreateCustomFormat(WaveFormatEncoding.ALaw, sampleRate, channels, sampleRate * channels, channels, 8);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000AB5C File Offset: 0x00008D5C
		public static WaveFormat CreateMuLawFormat(int sampleRate, int channels)
		{
			return WaveFormat.CreateCustomFormat(WaveFormatEncoding.MuLaw, sampleRate, channels, sampleRate * channels, channels, 8);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000AB6C File Offset: 0x00008D6C
		public WaveFormat(int rate, int bits, int channels)
		{
			if (channels < 1)
			{
				throw new ArgumentOutOfRangeException("channels", "Channels must be 1 or greater");
			}
			this.waveFormatTag = WaveFormatEncoding.Pcm;
			this.channels = (short)channels;
			this.sampleRate = rate;
			this.bitsPerSample = (short)bits;
			this.extraSize = 0;
			this.blockAlign = (short)(channels * (bits / 8));
			this.averageBytesPerSecond = this.sampleRate * (int)this.blockAlign;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		public static WaveFormat CreateIeeeFloatWaveFormat(int sampleRate, int channels)
		{
			WaveFormat waveFormat = new WaveFormat();
			waveFormat.waveFormatTag = WaveFormatEncoding.IeeeFloat;
			waveFormat.channels = (short)channels;
			waveFormat.bitsPerSample = 32;
			waveFormat.sampleRate = sampleRate;
			waveFormat.blockAlign = (short)(4 * channels);
			waveFormat.averageBytesPerSecond = sampleRate * (int)waveFormat.blockAlign;
			waveFormat.extraSize = 0;
			return waveFormat;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public static WaveFormat MarshalFromPtr(IntPtr pointer)
		{
			WaveFormat waveFormat = MarshalHelpers.PtrToStructure<WaveFormat>(pointer);
			WaveFormatEncoding encoding = waveFormat.Encoding;
			if (encoding <= WaveFormatEncoding.Adpcm)
			{
				if (encoding == WaveFormatEncoding.Pcm)
				{
					waveFormat.extraSize = 0;
					return waveFormat;
				}
				if (encoding == WaveFormatEncoding.Adpcm)
				{
					return MarshalHelpers.PtrToStructure<AdpcmWaveFormat>(pointer);
				}
			}
			else
			{
				if (encoding == WaveFormatEncoding.Gsm610)
				{
					return MarshalHelpers.PtrToStructure<Gsm610WaveFormat>(pointer);
				}
				if (encoding == WaveFormatEncoding.Extensible)
				{
					return MarshalHelpers.PtrToStructure<WaveFormatExtensible>(pointer);
				}
			}
			if (waveFormat.ExtraSize > 0)
			{
				waveFormat = MarshalHelpers.PtrToStructure<WaveFormatExtraData>(pointer);
			}
			return waveFormat;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000AC9C File Offset: 0x00008E9C
		public static IntPtr MarshalToPtr(WaveFormat format)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(format));
			Marshal.StructureToPtr(format, intPtr, false);
			return intPtr;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000ACBE File Offset: 0x00008EBE
		public static WaveFormat FromFormatChunk(BinaryReader br, int formatChunkLength)
		{
			WaveFormatExtraData waveFormatExtraData = new WaveFormatExtraData();
			waveFormatExtraData.ReadWaveFormat(br, formatChunkLength);
			waveFormatExtraData.ReadExtraData(br);
			return waveFormatExtraData;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000ACD4 File Offset: 0x00008ED4
		private void ReadWaveFormat(BinaryReader br, int formatChunkLength)
		{
			if (formatChunkLength < 16)
			{
				throw new InvalidDataException("Invalid WaveFormat Structure");
			}
			this.waveFormatTag = (WaveFormatEncoding)br.ReadUInt16();
			this.channels = br.ReadInt16();
			this.sampleRate = br.ReadInt32();
			this.averageBytesPerSecond = br.ReadInt32();
			this.blockAlign = br.ReadInt16();
			this.bitsPerSample = br.ReadInt16();
			if (formatChunkLength > 16)
			{
				this.extraSize = br.ReadInt16();
				if ((int)this.extraSize != formatChunkLength - 18)
				{
					this.extraSize = (short)(formatChunkLength - 18);
				}
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000AD64 File Offset: 0x00008F64
		public WaveFormat(BinaryReader br)
		{
			int formatChunkLength = br.ReadInt32();
			this.ReadWaveFormat(br, formatChunkLength);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000AD88 File Offset: 0x00008F88
		public override string ToString()
		{
			WaveFormatEncoding waveFormatEncoding = this.waveFormatTag;
			if (waveFormatEncoding == WaveFormatEncoding.Pcm || waveFormatEncoding == WaveFormatEncoding.Extensible)
			{
				return string.Format("{0} bit PCM: {1}kHz {2} channels", this.bitsPerSample, this.sampleRate / 1000, this.channels);
			}
			return this.waveFormatTag.ToString();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000ADEC File Offset: 0x00008FEC
		public override bool Equals(object obj)
		{
			WaveFormat waveFormat = obj as WaveFormat;
			return waveFormat != null && (this.waveFormatTag == waveFormat.waveFormatTag && this.channels == waveFormat.channels && this.sampleRate == waveFormat.sampleRate && this.averageBytesPerSecond == waveFormat.averageBytesPerSecond && this.blockAlign == waveFormat.blockAlign) && this.bitsPerSample == waveFormat.bitsPerSample;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000AE5B File Offset: 0x0000905B
		public override int GetHashCode()
		{
			return (int)(this.waveFormatTag ^ (WaveFormatEncoding)this.channels) ^ this.sampleRate ^ this.averageBytesPerSecond ^ (int)this.blockAlign ^ (int)this.bitsPerSample;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000AE86 File Offset: 0x00009086
		public WaveFormatEncoding Encoding
		{
			get
			{
				return this.waveFormatTag;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000AE90 File Offset: 0x00009090
		public virtual void Serialize(BinaryWriter writer)
		{
			writer.Write((int)(18 + this.extraSize));
			writer.Write((short)this.Encoding);
			writer.Write((short)this.Channels);
			writer.Write(this.SampleRate);
			writer.Write(this.AverageBytesPerSecond);
			writer.Write((short)this.BlockAlign);
			writer.Write((short)this.BitsPerSample);
			writer.Write(this.extraSize);
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000AF04 File Offset: 0x00009104
		public int Channels
		{
			get
			{
				return (int)this.channels;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000FF RID: 255 RVA: 0x0000AF0C File Offset: 0x0000910C
		public int SampleRate
		{
			get
			{
				return this.sampleRate;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000AF14 File Offset: 0x00009114
		public int AverageBytesPerSecond
		{
			get
			{
				return this.averageBytesPerSecond;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000AF1C File Offset: 0x0000911C
		public virtual int BlockAlign
		{
			get
			{
				return (int)this.blockAlign;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000AF24 File Offset: 0x00009124
		public int BitsPerSample
		{
			get
			{
				return (int)this.bitsPerSample;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000AF2C File Offset: 0x0000912C
		public int ExtraSize
		{
			get
			{
				return (int)this.extraSize;
			}
		}

		// Token: 0x04000183 RID: 387
		protected WaveFormatEncoding waveFormatTag;

		// Token: 0x04000184 RID: 388
		protected short channels;

		// Token: 0x04000185 RID: 389
		protected int sampleRate;

		// Token: 0x04000186 RID: 390
		protected int averageBytesPerSecond;

		// Token: 0x04000187 RID: 391
		protected short blockAlign;

		// Token: 0x04000188 RID: 392
		protected short bitsPerSample;

		// Token: 0x04000189 RID: 393
		protected short extraSize;
	}
}
