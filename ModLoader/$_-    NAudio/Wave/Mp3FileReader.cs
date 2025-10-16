using System;
using System.Collections.Generic;
using System.IO;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x0200009D RID: 157
	internal class Mp3FileReader : WaveStream
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00013C6A File Offset: 0x00011E6A
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00013C72 File Offset: 0x00011E72
		public Mp3WaveFormat Mp3WaveFormat { get; private set; }

		// Token: 0x060003C0 RID: 960 RVA: 0x00013C7B File Offset: 0x00011E7B
		public Mp3FileReader(string mp3FileName) : this(File.OpenRead(mp3FileName), new Mp3FileReader.FrameDecompressorBuilder(Mp3FileReader.CreateAcmFrameDecompressor), true)
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00013C96 File Offset: 0x00011E96
		public Mp3FileReader(string mp3FileName, Mp3FileReader.FrameDecompressorBuilder frameDecompressorBuilder) : this(File.OpenRead(mp3FileName), frameDecompressorBuilder, true)
		{
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00013CA6 File Offset: 0x00011EA6
		public Mp3FileReader(Stream inputStream) : this(inputStream, new Mp3FileReader.FrameDecompressorBuilder(Mp3FileReader.CreateAcmFrameDecompressor), false)
		{
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00013CBC File Offset: 0x00011EBC
		public Mp3FileReader(Stream inputStream, Mp3FileReader.FrameDecompressorBuilder frameDecompressorBuilder) : this(inputStream, frameDecompressorBuilder, false)
		{
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00013CC8 File Offset: 0x00011EC8
		private Mp3FileReader(Stream inputStream, Mp3FileReader.FrameDecompressorBuilder frameDecompressorBuilder, bool ownInputStream)
		{
			this.repositionLock = new object();
			base..ctor();
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (frameDecompressorBuilder == null)
			{
				throw new ArgumentNullException("frameDecompressorBuilder");
			}
			this.ownInputStream = ownInputStream;
			try
			{
				this.mp3Stream = inputStream;
				this.Id3v2Tag = Id3v2Tag.ReadTag(this.mp3Stream);
				this.dataStartPosition = this.mp3Stream.Position;
				Mp3Frame mp3Frame = Mp3Frame.LoadFromStream(this.mp3Stream);
				if (mp3Frame == null)
				{
					throw new InvalidDataException("Invalid MP3 file - no MP3 Frames Detected");
				}
				double num = (double)mp3Frame.BitRate;
				this.xingHeader = XingHeader.LoadXingHeader(mp3Frame);
				if (this.xingHeader != null)
				{
					this.dataStartPosition = this.mp3Stream.Position;
				}
				Mp3Frame mp3Frame2 = Mp3Frame.LoadFromStream(this.mp3Stream);
				if (mp3Frame2 != null && (mp3Frame2.SampleRate != mp3Frame.SampleRate || mp3Frame2.ChannelMode != mp3Frame.ChannelMode))
				{
					this.dataStartPosition = mp3Frame2.FileOffset;
					mp3Frame = mp3Frame2;
				}
				this.mp3DataLength = this.mp3Stream.Length - this.dataStartPosition;
				this.mp3Stream.Position = this.mp3Stream.Length - 128L;
				byte[] array = new byte[128];
				this.mp3Stream.Read(array, 0, 128);
				if (array[0] == 84 && array[1] == 65 && array[2] == 71)
				{
					this.Id3v1Tag = array;
					this.mp3DataLength -= 128L;
				}
				this.mp3Stream.Position = this.dataStartPosition;
				this.Mp3WaveFormat = new Mp3WaveFormat(mp3Frame.SampleRate, (mp3Frame.ChannelMode == ChannelMode.Mono) ? 1 : 2, mp3Frame.FrameLength, (int)num);
				this.CreateTableOfContents();
				this.tocIndex = 0;
				num = (double)this.mp3DataLength * 8.0 / this.TotalSeconds();
				this.mp3Stream.Position = this.dataStartPosition;
				this.Mp3WaveFormat = new Mp3WaveFormat(mp3Frame.SampleRate, (mp3Frame.ChannelMode == ChannelMode.Mono) ? 1 : 2, mp3Frame.FrameLength, (int)num);
				this.decompressor = frameDecompressorBuilder(this.Mp3WaveFormat);
				this.waveFormat = this.decompressor.OutputFormat;
				this.bytesPerSample = this.decompressor.OutputFormat.BitsPerSample / 8 * this.decompressor.OutputFormat.Channels;
				this.bytesPerDecodedFrame = 1152 * this.bytesPerSample;
				this.decompressBuffer = new byte[this.bytesPerDecodedFrame * 2];
			}
			catch (Exception)
			{
				if (ownInputStream)
				{
					inputStream.Dispose();
				}
				throw;
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00013F64 File Offset: 0x00012164
		public static IMp3FrameDecompressor CreateAcmFrameDecompressor(WaveFormat mp3Format)
		{
			return new AcmMp3FrameDecompressor(mp3Format);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00013F6C File Offset: 0x0001216C
		private void CreateTableOfContents()
		{
			try
			{
				this.tableOfContents = new List<Mp3Index>((int)(this.mp3DataLength / 400L));
				Mp3Frame mp3Frame;
				do
				{
					Mp3Index mp3Index = new Mp3Index();
					mp3Index.FilePosition = this.mp3Stream.Position;
					mp3Index.SamplePosition = this.totalSamples;
					mp3Frame = this.ReadNextFrame(false);
					if (mp3Frame != null)
					{
						this.ValidateFrameFormat(mp3Frame);
						this.totalSamples += (long)mp3Frame.SampleCount;
						mp3Index.SampleCount = mp3Frame.SampleCount;
						mp3Index.ByteCount = (int)(this.mp3Stream.Position - mp3Index.FilePosition);
						this.tableOfContents.Add(mp3Index);
					}
				}
				while (mp3Frame != null);
			}
			catch (EndOfStreamException)
			{
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00014024 File Offset: 0x00012224
		private void ValidateFrameFormat(Mp3Frame frame)
		{
			if (frame.SampleRate != this.Mp3WaveFormat.SampleRate)
			{
				throw new InvalidOperationException(string.Format("Got a frame at sample rate {0}, in an MP3 with sample rate {1}. Mp3FileReader does not support sample rate changes.", frame.SampleRate, this.Mp3WaveFormat.SampleRate));
			}
			if (((frame.ChannelMode == ChannelMode.Mono) ? 1 : 2) != this.Mp3WaveFormat.Channels)
			{
				throw new InvalidOperationException(string.Format("Got a frame with channel mode {0}, in an MP3 with {1} channels. Mp3FileReader does not support changes to channel count.", frame.ChannelMode, this.Mp3WaveFormat.Channels));
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000140B4 File Offset: 0x000122B4
		private double TotalSeconds()
		{
			return (double)this.totalSamples / (double)this.Mp3WaveFormat.SampleRate;
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x000140CA File Offset: 0x000122CA
		public Id3v2Tag Id3v2Tag { get; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003CA RID: 970 RVA: 0x000140D2 File Offset: 0x000122D2
		public byte[] Id3v1Tag { get; }

		// Token: 0x060003CB RID: 971 RVA: 0x000140DC File Offset: 0x000122DC
		public Mp3Frame ReadNextFrame()
		{
			Mp3Frame mp3Frame = this.ReadNextFrame(true);
			if (mp3Frame != null)
			{
				this.position += (long)(mp3Frame.SampleCount * this.bytesPerSample);
			}
			return mp3Frame;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00014110 File Offset: 0x00012310
		private Mp3Frame ReadNextFrame(bool readData)
		{
			Mp3Frame mp3Frame = null;
			try
			{
				mp3Frame = Mp3Frame.LoadFromStream(this.mp3Stream, readData);
				if (mp3Frame != null)
				{
					this.tocIndex++;
				}
			}
			catch (EndOfStreamException)
			{
			}
			return mp3Frame;
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00014154 File Offset: 0x00012354
		public override long Length
		{
			get
			{
				return this.totalSamples * (long)this.bytesPerSample;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00014164 File Offset: 0x00012364
		public override WaveFormat WaveFormat
		{
			get
			{
				return this.waveFormat;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0001416C File Offset: 0x0001236C
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x00014174 File Offset: 0x00012374
		public override long Position
		{
			get
			{
				return this.position;
			}
			set
			{
				object obj = this.repositionLock;
				lock (obj)
				{
					value = Math.Max(Math.Min(value, this.Length), 0L);
					long num = value / (long)this.bytesPerSample;
					Mp3Index mp3Index = null;
					for (int i = 0; i < this.tableOfContents.Count; i++)
					{
						if (this.tableOfContents[i].SamplePosition + (long)this.tableOfContents[i].SampleCount > num)
						{
							mp3Index = this.tableOfContents[i];
							this.tocIndex = i;
							break;
						}
					}
					this.decompressBufferOffset = 0;
					this.decompressLeftovers = 0;
					this.repositionedFlag = true;
					if (mp3Index != null)
					{
						this.mp3Stream.Position = mp3Index.FilePosition;
						long num2 = num - mp3Index.SamplePosition;
						if (num2 > 0L)
						{
							this.decompressBufferOffset = (int)num2 * this.bytesPerSample;
						}
					}
					else
					{
						this.mp3Stream.Position = this.mp3DataLength + this.dataStartPosition;
					}
					this.position = value;
				}
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00014288 File Offset: 0x00012488
		public override int Read(byte[] sampleBuffer, int offset, int numBytes)
		{
			int i = 0;
			object obj = this.repositionLock;
			lock (obj)
			{
				if (this.decompressLeftovers != 0)
				{
					int num = Math.Min(this.decompressLeftovers, numBytes);
					Array.Copy(this.decompressBuffer, this.decompressBufferOffset, sampleBuffer, offset, num);
					this.decompressLeftovers -= num;
					if (this.decompressLeftovers == 0)
					{
						this.decompressBufferOffset = 0;
					}
					else
					{
						this.decompressBufferOffset += num;
					}
					i += num;
					offset += num;
				}
				int num2 = this.tocIndex;
				if (this.repositionedFlag)
				{
					this.decompressor.Reset();
					this.tocIndex = Math.Max(0, this.tocIndex - 3);
					this.mp3Stream.Position = this.tableOfContents[this.tocIndex].FilePosition;
					this.repositionedFlag = false;
				}
				while (i < numBytes)
				{
					Mp3Frame mp3Frame = this.ReadNextFrame(true);
					if (mp3Frame == null)
					{
						break;
					}
					int num3 = this.decompressor.DecompressFrame(mp3Frame, this.decompressBuffer, 0);
					if (this.tocIndex > num2 && num3 != 0)
					{
						if (this.tocIndex == num2 + 1 && num3 == this.bytesPerDecodedFrame * 2)
						{
							Array.Copy(this.decompressBuffer, this.bytesPerDecodedFrame, this.decompressBuffer, 0, this.bytesPerDecodedFrame);
							num3 = this.bytesPerDecodedFrame;
						}
						int num4 = Math.Min(num3 - this.decompressBufferOffset, numBytes - i);
						Array.Copy(this.decompressBuffer, this.decompressBufferOffset, sampleBuffer, offset, num4);
						if (num4 + this.decompressBufferOffset < num3)
						{
							this.decompressBufferOffset = num4 + this.decompressBufferOffset;
							this.decompressLeftovers = num3 - this.decompressBufferOffset;
						}
						else
						{
							this.decompressBufferOffset = 0;
							this.decompressLeftovers = 0;
						}
						offset += num4;
						i += num4;
					}
				}
			}
			this.position += (long)i;
			return i;
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00014484 File Offset: 0x00012684
		public XingHeader XingHeader
		{
			get
			{
				return this.xingHeader;
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001448C File Offset: 0x0001268C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.mp3Stream != null)
				{
					if (this.ownInputStream)
					{
						this.mp3Stream.Dispose();
					}
					this.mp3Stream = null;
				}
				if (this.decompressor != null)
				{
					this.decompressor.Dispose();
					this.decompressor = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040003AA RID: 938
		private readonly WaveFormat waveFormat;

		// Token: 0x040003AB RID: 939
		private Stream mp3Stream;

		// Token: 0x040003AC RID: 940
		private readonly long mp3DataLength;

		// Token: 0x040003AD RID: 941
		private readonly long dataStartPosition;

		// Token: 0x040003AF RID: 943
		private readonly XingHeader xingHeader;

		// Token: 0x040003B0 RID: 944
		private readonly bool ownInputStream;

		// Token: 0x040003B1 RID: 945
		private List<Mp3Index> tableOfContents;

		// Token: 0x040003B2 RID: 946
		private int tocIndex;

		// Token: 0x040003B3 RID: 947
		private long totalSamples;

		// Token: 0x040003B4 RID: 948
		private readonly int bytesPerSample;

		// Token: 0x040003B5 RID: 949
		private readonly int bytesPerDecodedFrame;

		// Token: 0x040003B6 RID: 950
		private IMp3FrameDecompressor decompressor;

		// Token: 0x040003B7 RID: 951
		private readonly byte[] decompressBuffer;

		// Token: 0x040003B8 RID: 952
		private int decompressBufferOffset;

		// Token: 0x040003B9 RID: 953
		private int decompressLeftovers;

		// Token: 0x040003BA RID: 954
		private bool repositionedFlag;

		// Token: 0x040003BB RID: 955
		private long position;

		// Token: 0x040003BC RID: 956
		private readonly object repositionLock;

		// Token: 0x0200009E RID: 158
		// (Invoke) Token: 0x060003D5 RID: 981
		public delegate IMp3FrameDecompressor FrameDecompressorBuilder(WaveFormat mp3Format);
	}
}
