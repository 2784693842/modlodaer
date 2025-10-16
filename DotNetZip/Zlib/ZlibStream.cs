using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02000028 RID: 40
	public class ZlibStream : Stream
	{
		// Token: 0x0600012E RID: 302 RVA: 0x0000C29A File Offset: 0x0000A49A
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000C2A6 File Offset: 0x0000A4A6
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000C2B2 File Offset: 0x0000A4B2
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000C2BE File Offset: 0x0000A4BE
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000132 RID: 306 RVA: 0x0000C2DB File Offset: 0x0000A4DB
		// (set) Token: 0x06000133 RID: 307 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000C309 File Offset: 0x0000A509
		// (set) Token: 0x06000135 RID: 309 RVA: 0x0000C318 File Offset: 0x0000A518
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000136 RID: 310 RVA: 0x0000C384 File Offset: 0x0000A584
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000C396 File Offset: 0x0000A596
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000C3A8 File Offset: 0x0000A5A8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600013A RID: 314 RVA: 0x0000C419 File Offset: 0x0000A619
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000C41C File Offset: 0x0000A61C
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000C441 File Offset: 0x0000A641
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000C461 File Offset: 0x0000A661
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600013E RID: 318 RVA: 0x0000C468 File Offset: 0x0000A668
		// (set) Token: 0x0600013F RID: 319 RVA: 0x0000C4B4 File Offset: 0x0000A6B4
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000C4BB File Offset: 0x0000A6BB
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000C4DE File Offset: 0x0000A6DE
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000C4E5 File Offset: 0x0000A6E5
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000C4EC File Offset: 0x0000A6EC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000C510 File Offset: 0x0000A710
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000C558 File Offset: 0x0000A758
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400016A RID: 362
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400016B RID: 363
		private bool _disposed;
	}
}
