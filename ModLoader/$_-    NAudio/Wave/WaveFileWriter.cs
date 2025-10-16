using System;
using System.IO;
using System.Text;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.SampleProviders;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000C6 RID: 198
	internal class WaveFileWriter : Stream
	{
		// Token: 0x06000466 RID: 1126 RVA: 0x00015970 File Offset: 0x00013B70
		public static void CreateWaveFile16(string filename, ISampleProvider sourceProvider)
		{
			WaveFileWriter.CreateWaveFile(filename, new SampleToWaveProvider16(sourceProvider));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00015980 File Offset: 0x00013B80
		public static void CreateWaveFile(string filename, IWaveProvider sourceProvider)
		{
			using (WaveFileWriter waveFileWriter = new WaveFileWriter(filename, sourceProvider.WaveFormat))
			{
				byte[] array = new byte[sourceProvider.WaveFormat.AverageBytesPerSecond * 4];
				for (;;)
				{
					int num = sourceProvider.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					waveFileWriter.Write(array, 0, num);
				}
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000159E4 File Offset: 0x00013BE4
		public static void WriteWavFileToStream(Stream outStream, IWaveProvider sourceProvider)
		{
			using (WaveFileWriter waveFileWriter = new WaveFileWriter(new IgnoreDisposeStream(outStream), sourceProvider.WaveFormat))
			{
				byte[] array = new byte[sourceProvider.WaveFormat.AverageBytesPerSecond * 4];
				for (;;)
				{
					int num = sourceProvider.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					waveFileWriter.Write(array, 0, num);
				}
				outStream.Flush();
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00015A54 File Offset: 0x00013C54
		public WaveFileWriter(Stream outStream, WaveFormat format)
		{
			this.value24 = new byte[3];
			base..ctor();
			this.outStream = outStream;
			this.format = format;
			this.writer = new BinaryWriter(outStream, Encoding.UTF8);
			this.writer.Write(Encoding.UTF8.GetBytes("RIFF"));
			this.writer.Write(0);
			this.writer.Write(Encoding.UTF8.GetBytes("WAVE"));
			this.writer.Write(Encoding.UTF8.GetBytes("fmt "));
			format.Serialize(this.writer);
			this.CreateFactChunk();
			this.WriteDataChunkHeader();
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00015B04 File Offset: 0x00013D04
		public WaveFileWriter(string filename, WaveFormat format) : this(new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read), format)
		{
			this.filename = filename;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00015B1D File Offset: 0x00013D1D
		private void WriteDataChunkHeader()
		{
			this.writer.Write(Encoding.UTF8.GetBytes("data"));
			this.dataSizePos = this.outStream.Position;
			this.writer.Write(0);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00015B58 File Offset: 0x00013D58
		private void CreateFactChunk()
		{
			if (this.HasFactChunk())
			{
				this.writer.Write(Encoding.UTF8.GetBytes("fact"));
				this.writer.Write(4);
				this.factSampleCountPos = this.outStream.Position;
				this.writer.Write(0);
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00015BB0 File Offset: 0x00013DB0
		private bool HasFactChunk()
		{
			return this.format.Encoding != WaveFormatEncoding.Pcm && this.format.BitsPerSample != 0;
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x00015BD0 File Offset: 0x00013DD0
		public string Filename
		{
			get
			{
				return this.filename;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00015BD8 File Offset: 0x00013DD8
		public override long Length
		{
			get
			{
				return this.dataChunkSize;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00015BE0 File Offset: 0x00013DE0
		public TimeSpan TotalTime
		{
			get
			{
				return TimeSpan.FromSeconds((double)this.Length / (double)this.WaveFormat.AverageBytesPerSecond);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00015BFB File Offset: 0x00013DFB
		public WaveFormat WaveFormat
		{
			get
			{
				return this.format;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0000AF37 File Offset: 0x00009137
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0000AF34 File Offset: 0x00009134
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000AF37 File Offset: 0x00009137
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00015C03 File Offset: 0x00013E03
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new InvalidOperationException("Cannot read from a WaveFileWriter");
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00015C0F File Offset: 0x00013E0F
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new InvalidOperationException("Cannot seek within a WaveFileWriter");
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00015C1B File Offset: 0x00013E1B
		public override void SetLength(long value)
		{
			throw new InvalidOperationException("Cannot set length of a WaveFileWriter");
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00015BD8 File Offset: 0x00013DD8
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x00015C27 File Offset: 0x00013E27
		public override long Position
		{
			get
			{
				return this.dataChunkSize;
			}
			set
			{
				throw new InvalidOperationException("Repositioning a WaveFileWriter is not supported");
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00015C33 File Offset: 0x00013E33
		[Obsolete("Use Write instead")]
		public void WriteData(byte[] data, int offset, int count)
		{
			this.Write(data, offset, count);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00015C40 File Offset: 0x00013E40
		public override void Write(byte[] data, int offset, int count)
		{
			if (this.outStream.Length + (long)count > (long)((ulong)-1))
			{
				throw new ArgumentException("WAV file too large", "count");
			}
			this.outStream.Write(data, offset, count);
			this.dataChunkSize += (long)count;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00015C8C File Offset: 0x00013E8C
		public void WriteSample(float sample)
		{
			if (this.WaveFormat.BitsPerSample == 16)
			{
				this.writer.Write((short)(32767f * sample));
				this.dataChunkSize += 2L;
				return;
			}
			if (this.WaveFormat.BitsPerSample == 24)
			{
				byte[] bytes = BitConverter.GetBytes((int)(2.1474836E+09f * sample));
				this.value24[0] = bytes[1];
				this.value24[1] = bytes[2];
				this.value24[2] = bytes[3];
				this.writer.Write(this.value24);
				this.dataChunkSize += 3L;
				return;
			}
			if (this.WaveFormat.BitsPerSample == 32 && this.WaveFormat.Encoding == WaveFormatEncoding.Extensible)
			{
				this.writer.Write(65535 * (int)sample);
				this.dataChunkSize += 4L;
				return;
			}
			if (this.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
			{
				this.writer.Write(sample);
				this.dataChunkSize += 4L;
				return;
			}
			throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00015DA4 File Offset: 0x00013FA4
		public void WriteSamples(float[] samples, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.WriteSample(samples[offset + i]);
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00015DC8 File Offset: 0x00013FC8
		[Obsolete("Use WriteSamples instead")]
		public void WriteData(short[] samples, int offset, int count)
		{
			this.WriteSamples(samples, offset, count);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00015DD4 File Offset: 0x00013FD4
		public void WriteSamples(short[] samples, int offset, int count)
		{
			if (this.WaveFormat.BitsPerSample == 16)
			{
				for (int i = 0; i < count; i++)
				{
					this.writer.Write(samples[i + offset]);
				}
				this.dataChunkSize += (long)(count * 2);
				return;
			}
			if (this.WaveFormat.BitsPerSample == 24)
			{
				for (int j = 0; j < count; j++)
				{
					byte[] bytes = BitConverter.GetBytes(65535 * (int)samples[j + offset]);
					this.value24[0] = bytes[1];
					this.value24[1] = bytes[2];
					this.value24[2] = bytes[3];
					this.writer.Write(this.value24);
				}
				this.dataChunkSize += (long)(count * 3);
				return;
			}
			if (this.WaveFormat.BitsPerSample == 32 && this.WaveFormat.Encoding == WaveFormatEncoding.Extensible)
			{
				for (int k = 0; k < count; k++)
				{
					this.writer.Write(65535 * (int)samples[k + offset]);
				}
				this.dataChunkSize += (long)(count * 4);
				return;
			}
			if (this.WaveFormat.BitsPerSample == 32 && this.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
			{
				for (int l = 0; l < count; l++)
				{
					this.writer.Write((float)samples[l + offset] / 32768f);
				}
				this.dataChunkSize += (long)(count * 4);
				return;
			}
			throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00015F44 File Offset: 0x00014144
		public override void Flush()
		{
			long position = this.writer.BaseStream.Position;
			this.UpdateHeader(this.writer);
			this.writer.BaseStream.Position = position;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00015F80 File Offset: 0x00014180
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.outStream != null)
			{
				try
				{
					this.UpdateHeader(this.writer);
				}
				finally
				{
					this.outStream.Dispose();
					this.outStream = null;
				}
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00015FCC File Offset: 0x000141CC
		protected virtual void UpdateHeader(BinaryWriter writer)
		{
			writer.Flush();
			this.UpdateRiffChunk(writer);
			this.UpdateFactChunk(writer);
			this.UpdateDataChunk(writer);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00015FE9 File Offset: 0x000141E9
		private void UpdateDataChunk(BinaryWriter writer)
		{
			writer.Seek((int)this.dataSizePos, SeekOrigin.Begin);
			writer.Write((uint)this.dataChunkSize);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00016007 File Offset: 0x00014207
		private void UpdateRiffChunk(BinaryWriter writer)
		{
			writer.Seek(4, SeekOrigin.Begin);
			writer.Write((uint)(this.outStream.Length - 8L));
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00016028 File Offset: 0x00014228
		private void UpdateFactChunk(BinaryWriter writer)
		{
			if (this.HasFactChunk())
			{
				int num = this.format.BitsPerSample * this.format.Channels;
				if (num != 0)
				{
					writer.Seek((int)this.factSampleCountPos, SeekOrigin.Begin);
					writer.Write((int)(this.dataChunkSize * 8L / (long)num));
				}
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0001607C File Offset: 0x0001427C
		~WaveFileWriter()
		{
			this.Dispose(false);
		}

		// Token: 0x040004C6 RID: 1222
		private Stream outStream;

		// Token: 0x040004C7 RID: 1223
		private readonly BinaryWriter writer;

		// Token: 0x040004C8 RID: 1224
		private long dataSizePos;

		// Token: 0x040004C9 RID: 1225
		private long factSampleCountPos;

		// Token: 0x040004CA RID: 1226
		private long dataChunkSize;

		// Token: 0x040004CB RID: 1227
		private readonly WaveFormat format;

		// Token: 0x040004CC RID: 1228
		private readonly string filename;

		// Token: 0x040004CD RID: 1229
		private readonly byte[] value24;
	}
}
