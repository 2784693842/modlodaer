using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02000011 RID: 17
	public class DeflateStream : Stream
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00005EC5 File Offset: 0x000040C5
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005ED1 File Offset: 0x000040D1
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005EDD File Offset: 0x000040DD
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005EE9 File Offset: 0x000040E9
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00005F0D File Offset: 0x0000410D
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00005F1A File Offset: 0x0000411A
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00005F3B File Offset: 0x0000413B
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00005F48 File Offset: 0x00004148
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00005FB4 File Offset: 0x000041B4
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00005FC1 File Offset: 0x000041C1
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00005FE2 File Offset: 0x000041E2
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00005FF4 File Offset: 0x000041F4
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00006008 File Offset: 0x00004208
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00006054 File Offset: 0x00004254
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00006079 File Offset: 0x00004279
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000607C File Offset: 0x0000427C
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000060A1 File Offset: 0x000042A1
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000060C1 File Offset: 0x000042C1
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000060C8 File Offset: 0x000042C8
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00006114 File Offset: 0x00004314
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
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000611B File Offset: 0x0000431B
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000613E File Offset: 0x0000433E
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00006145 File Offset: 0x00004345
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000614C File Offset: 0x0000434C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00006170 File Offset: 0x00004370
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000061B8 File Offset: 0x000043B8
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006200 File Offset: 0x00004400
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006244 File Offset: 0x00004444
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04000077 RID: 119
		internal ZlibBaseStream _baseStream;

		// Token: 0x04000078 RID: 120
		internal Stream _innerStream;

		// Token: 0x04000079 RID: 121
		private bool _disposed;
	}
}
