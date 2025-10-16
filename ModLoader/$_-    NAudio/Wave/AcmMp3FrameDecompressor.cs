using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000AA RID: 170
	internal class AcmMp3FrameDecompressor : IMp3FrameDecompressor, IDisposable
	{
		// Token: 0x06000418 RID: 1048 RVA: 0x00015038 File Offset: 0x00013238
		public AcmMp3FrameDecompressor(WaveFormat sourceFormat)
		{
			this.pcmFormat = AcmStream.SuggestPcmFormat(sourceFormat);
			try
			{
				this.conversionStream = new AcmStream(sourceFormat, this.pcmFormat);
			}
			catch (Exception)
			{
				this.disposed = true;
				GC.SuppressFinalize(this);
				throw;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0001508C File Offset: 0x0001328C
		public WaveFormat OutputFormat
		{
			get
			{
				return this.pcmFormat;
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00015094 File Offset: 0x00013294
		public int DecompressFrame(Mp3Frame frame, byte[] dest, int destOffset)
		{
			if (frame == null)
			{
				throw new ArgumentNullException("frame", "You must provide a non-null Mp3Frame to decompress");
			}
			Array.Copy(frame.RawData, this.conversionStream.SourceBuffer, frame.FrameLength);
			int num2;
			int num = this.conversionStream.Convert(frame.FrameLength, out num2);
			if (num2 != frame.FrameLength)
			{
				throw new InvalidOperationException(string.Format("Couldn't convert the whole MP3 frame (converted {0}/{1})", num2, frame.FrameLength));
			}
			Array.Copy(this.conversionStream.DestBuffer, 0, dest, destOffset, num);
			return num;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00015123 File Offset: 0x00013323
		public void Reset()
		{
			this.conversionStream.Reposition();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00015130 File Offset: 0x00013330
		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (this.conversionStream != null)
				{
					this.conversionStream.Dispose();
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001515C File Offset: 0x0001335C
		~AcmMp3FrameDecompressor()
		{
			this.Dispose();
		}

		// Token: 0x040003FE RID: 1022
		private readonly AcmStream conversionStream;

		// Token: 0x040003FF RID: 1023
		private readonly WaveFormat pcmFormat;

		// Token: 0x04000400 RID: 1024
		private bool disposed;
	}
}
