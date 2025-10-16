using System;
using System.Collections.Generic;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.FileFormats.Wav
{
	// Token: 0x02000040 RID: 64
	internal class WaveFileChunkReader
	{
		// Token: 0x06000128 RID: 296 RVA: 0x0000B505 File Offset: 0x00009705
		public WaveFileChunkReader()
		{
			this.storeAllChunks = true;
			this.strictMode = false;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000B51C File Offset: 0x0000971C
		public void ReadWaveHeader(Stream stream)
		{
			this.dataChunkPosition = -1L;
			this.waveFormat = null;
			this.riffChunks = new List<RiffChunk>();
			this.dataChunkLength = 0L;
			BinaryReader binaryReader = new BinaryReader(stream);
			this.ReadRiffHeader(binaryReader);
			this.riffSize = (long)((ulong)binaryReader.ReadUInt32());
			if (binaryReader.ReadInt32() != ChunkIdentifier.ChunkIdentifierToInt32("WAVE"))
			{
				throw new FormatException("Not a WAVE file - no WAVE header");
			}
			if (this.isRf64)
			{
				this.ReadDs64Chunk(binaryReader);
			}
			int num = ChunkIdentifier.ChunkIdentifierToInt32("data");
			int num2 = ChunkIdentifier.ChunkIdentifierToInt32("fmt ");
			long num3 = Math.Min(this.riffSize + 8L, stream.Length);
			while (stream.Position <= num3 - 8L)
			{
				int num4 = binaryReader.ReadInt32();
				uint num5 = binaryReader.ReadUInt32();
				if (num4 == num)
				{
					this.dataChunkPosition = stream.Position;
					if (!this.isRf64)
					{
						this.dataChunkLength = (long)((ulong)num5);
					}
					stream.Position += (long)((ulong)num5);
				}
				else if (num4 == num2)
				{
					if (num5 > 2147483647U)
					{
						throw new InvalidDataException(string.Format("Format chunk length must be between 0 and {0}.", int.MaxValue));
					}
					this.waveFormat = WaveFormat.FromFormatChunk(binaryReader, (int)num5);
				}
				else if ((ulong)num5 > (ulong)(stream.Length - stream.Position))
				{
					if (this.strictMode)
					{
						break;
					}
					break;
				}
				else
				{
					if (this.storeAllChunks)
					{
						if (num5 > 2147483647U)
						{
							throw new InvalidDataException(string.Format("RiffChunk chunk length must be between 0 and {0}.", int.MaxValue));
						}
						this.riffChunks.Add(WaveFileChunkReader.GetRiffChunk(stream, num4, (int)num5));
					}
					stream.Position += (long)((ulong)num5);
				}
				if (num5 % 2U != 0U && binaryReader.PeekChar() == 0)
				{
					long position = stream.Position;
					stream.Position = position + 1L;
				}
			}
			if (this.waveFormat == null)
			{
				throw new FormatException("Invalid WAV file - No fmt chunk found");
			}
			if (this.dataChunkPosition == -1L)
			{
				throw new FormatException("Invalid WAV file - No data chunk found");
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000B70C File Offset: 0x0000990C
		private void ReadDs64Chunk(BinaryReader reader)
		{
			int num = ChunkIdentifier.ChunkIdentifierToInt32("ds64");
			if (reader.ReadInt32() != num)
			{
				throw new FormatException("Invalid RF64 WAV file - No ds64 chunk found");
			}
			int num2 = reader.ReadInt32();
			this.riffSize = reader.ReadInt64();
			this.dataChunkLength = reader.ReadInt64();
			reader.ReadInt64();
			reader.ReadBytes(num2 - 24);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000B769 File Offset: 0x00009969
		private static RiffChunk GetRiffChunk(Stream stream, int chunkIdentifier, int chunkLength)
		{
			return new RiffChunk(chunkIdentifier, chunkLength, stream.Position);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000B778 File Offset: 0x00009978
		private void ReadRiffHeader(BinaryReader br)
		{
			int num = br.ReadInt32();
			if (num == ChunkIdentifier.ChunkIdentifierToInt32("RF64"))
			{
				this.isRf64 = true;
				return;
			}
			if (num != ChunkIdentifier.ChunkIdentifierToInt32("RIFF"))
			{
				throw new FormatException("Not a WAVE file - no RIFF header");
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000B7B9 File Offset: 0x000099B9
		public WaveFormat WaveFormat
		{
			get
			{
				return this.waveFormat;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000B7C1 File Offset: 0x000099C1
		public long DataChunkPosition
		{
			get
			{
				return this.dataChunkPosition;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000B7C9 File Offset: 0x000099C9
		public long DataChunkLength
		{
			get
			{
				return this.dataChunkLength;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000130 RID: 304 RVA: 0x0000B7D1 File Offset: 0x000099D1
		public List<RiffChunk> RiffChunks
		{
			get
			{
				return this.riffChunks;
			}
		}

		// Token: 0x04000235 RID: 565
		private WaveFormat waveFormat;

		// Token: 0x04000236 RID: 566
		private long dataChunkPosition;

		// Token: 0x04000237 RID: 567
		private long dataChunkLength;

		// Token: 0x04000238 RID: 568
		private List<RiffChunk> riffChunks;

		// Token: 0x04000239 RID: 569
		private readonly bool strictMode;

		// Token: 0x0400023A RID: 570
		private bool isRf64;

		// Token: 0x0400023B RID: 571
		private readonly bool storeAllChunks;

		// Token: 0x0400023C RID: 572
		private long riffSize;
	}
}
