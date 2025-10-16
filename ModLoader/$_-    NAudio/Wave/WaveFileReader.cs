using System;
using System.Collections.Generic;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.FileFormats.Wav;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x0200003E RID: 62
	internal class WaveFileReader : WaveStream
	{
		// Token: 0x06000113 RID: 275 RVA: 0x0000B051 File Offset: 0x00009251
		public WaveFileReader(string waveFile) : this(File.OpenRead(waveFile), true)
		{
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000B060 File Offset: 0x00009260
		public WaveFileReader(Stream inputStream) : this(inputStream, false)
		{
		}

		// Token: 0x06000115 RID: 277 RVA: 0x0000B06C File Offset: 0x0000926C
		private WaveFileReader(Stream inputStream, bool ownInput)
		{
			this.lockObject = new object();
			base..ctor();
			this.waveStream = inputStream;
			WaveFileChunkReader waveFileChunkReader = new WaveFileChunkReader();
			try
			{
				waveFileChunkReader.ReadWaveHeader(inputStream);
				this.waveFormat = waveFileChunkReader.WaveFormat;
				this.dataPosition = waveFileChunkReader.DataChunkPosition;
				this.dataChunkLength = waveFileChunkReader.DataChunkLength;
				this.ExtraChunks = waveFileChunkReader.RiffChunks;
			}
			catch
			{
				if (ownInput)
				{
					inputStream.Dispose();
				}
				throw;
			}
			this.Position = 0L;
			this.ownInput = ownInput;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000B0FC File Offset: 0x000092FC
		public List<RiffChunk> ExtraChunks { get; }

		// Token: 0x06000117 RID: 279 RVA: 0x0000B104 File Offset: 0x00009304
		public byte[] GetChunkData(RiffChunk chunk)
		{
			long position = this.waveStream.Position;
			this.waveStream.Position = chunk.StreamPosition;
			byte[] array = new byte[chunk.Length];
			this.waveStream.Read(array, 0, array.Length);
			this.waveStream.Position = position;
			return array;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000B158 File Offset: 0x00009358
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.waveStream != null)
			{
				if (this.ownInput)
				{
					this.waveStream.Dispose();
				}
				this.waveStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000B186 File Offset: 0x00009386
		public override WaveFormat WaveFormat
		{
			get
			{
				return this.waveFormat;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000B18E File Offset: 0x0000938E
		public override long Length
		{
			get
			{
				return this.dataChunkLength;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000B198 File Offset: 0x00009398
		public long SampleCount
		{
			get
			{
				if (this.waveFormat.Encoding == WaveFormatEncoding.Pcm || this.waveFormat.Encoding == WaveFormatEncoding.Extensible || this.waveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
				{
					return this.dataChunkLength / (long)this.BlockAlign;
				}
				throw new InvalidOperationException("Sample count is calculated only for the standard encodings");
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000B1EC File Offset: 0x000093EC
		// (set) Token: 0x0600011D RID: 285 RVA: 0x0000B200 File Offset: 0x00009400
		public override long Position
		{
			get
			{
				return this.waveStream.Position - this.dataPosition;
			}
			set
			{
				object obj = this.lockObject;
				lock (obj)
				{
					value = Math.Min(value, this.Length);
					value -= value % (long)this.waveFormat.BlockAlign;
					this.waveStream.Position = value + this.dataPosition;
				}
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000B268 File Offset: 0x00009468
		public override int Read(byte[] array, int offset, int count)
		{
			if (count % this.waveFormat.BlockAlign != 0)
			{
				throw new ArgumentException(string.Format("Must read complete blocks: requested {0}, block align is {1}", count, this.WaveFormat.BlockAlign));
			}
			object obj = this.lockObject;
			int result;
			lock (obj)
			{
				if (this.Position + (long)count > this.dataChunkLength)
				{
					count = (int)(this.dataChunkLength - this.Position);
				}
				result = this.waveStream.Read(array, offset, count);
			}
			return result;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000B304 File Offset: 0x00009504
		public float[] ReadNextSampleFrame()
		{
			WaveFormatEncoding encoding = this.waveFormat.Encoding;
			if (encoding != WaveFormatEncoding.Pcm && encoding != WaveFormatEncoding.IeeeFloat && encoding != WaveFormatEncoding.Extensible)
			{
				throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
			}
			float[] array = new float[this.waveFormat.Channels];
			int num = this.waveFormat.Channels * (this.waveFormat.BitsPerSample / 8);
			byte[] array2 = new byte[num];
			int num2 = this.Read(array2, 0, num);
			if (num2 == 0)
			{
				return null;
			}
			if (num2 < num)
			{
				throw new InvalidDataException("Unexpected end of file");
			}
			int num3 = 0;
			for (int i = 0; i < this.waveFormat.Channels; i++)
			{
				if (this.waveFormat.BitsPerSample == 16)
				{
					array[i] = (float)BitConverter.ToInt16(array2, num3) / 32768f;
					num3 += 2;
				}
				else if (this.waveFormat.BitsPerSample == 24)
				{
					array[i] = (float)((int)((sbyte)array2[num3 + 2]) << 16 | (int)array2[num3 + 1] << 8 | (int)array2[num3]) / 8388608f;
					num3 += 3;
				}
				else if (this.waveFormat.BitsPerSample == 32 && this.waveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
				{
					array[i] = BitConverter.ToSingle(array2, num3);
					num3 += 4;
				}
				else
				{
					if (this.waveFormat.BitsPerSample != 32)
					{
						throw new InvalidOperationException("Unsupported bit depth");
					}
					array[i] = (float)BitConverter.ToInt32(array2, num3) / 2.1474836E+09f;
					num3 += 4;
				}
			}
			return array;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000B480 File Offset: 0x00009680
		[Obsolete("Use ReadNextSampleFrame instead (this version does not support stereo properly)")]
		public bool TryReadFloat(out float sampleValue)
		{
			float[] array = this.ReadNextSampleFrame();
			sampleValue = ((array != null) ? array[0] : 0f);
			return array != null;
		}

		// Token: 0x0400022B RID: 555
		private readonly WaveFormat waveFormat;

		// Token: 0x0400022C RID: 556
		private readonly bool ownInput;

		// Token: 0x0400022D RID: 557
		private readonly long dataPosition;

		// Token: 0x0400022E RID: 558
		private readonly long dataChunkLength;

		// Token: 0x0400022F RID: 559
		private readonly object lockObject;

		// Token: 0x04000230 RID: 560
		private Stream waveStream;
	}
}
