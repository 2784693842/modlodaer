using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02000012 RID: 18
	public class GZipStream : Stream
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00006288 File Offset: 0x00004488
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00006290 File Offset: 0x00004490
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000062AC File Offset: 0x000044AC
		// (set) Token: 0x06000087 RID: 135 RVA: 0x000062B4 File Offset: 0x000044B4
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00006353 File Offset: 0x00004553
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000635B File Offset: 0x0000455B
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006367 File Offset: 0x00004567
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006373 File Offset: 0x00004573
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000637F File Offset: 0x0000457F
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000639C File Offset: 0x0000459C
		// (set) Token: 0x0600008E RID: 142 RVA: 0x000063A9 File Offset: 0x000045A9
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
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000063CA File Offset: 0x000045CA
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000063D8 File Offset: 0x000045D8
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
					throw new ObjectDisposedException("GZipStream");
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00006444 File Offset: 0x00004644
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00006456 File Offset: 0x00004656
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00006468 File Offset: 0x00004668
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000064C8 File Offset: 0x000046C8
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000064ED File Offset: 0x000046ED
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000096 RID: 150 RVA: 0x000064F0 File Offset: 0x000046F0
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006515 File Offset: 0x00004715
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00006535 File Offset: 0x00004735
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000653C File Offset: 0x0000473C
		// (set) Token: 0x0600009A RID: 154 RVA: 0x0000659D File Offset: 0x0000479D
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000065A4 File Offset: 0x000047A4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00006603 File Offset: 0x00004803
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000660A File Offset: 0x0000480A
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00006614 File Offset: 0x00004814
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00006674 File Offset: 0x00004874
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			byte[] array3 = new byte[10 + num + num2];
			int num3 = 0;
			array3[num3++] = 31;
			array3[num3++] = 139;
			array3[num3++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num3++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			Array.Copy(BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, array3, num3, 4);
			num3 += 4;
			array3[num3++] = 0;
			array3[num3++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num3, num2 - 1);
				num3 += num2 - 1;
				array3[num3++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num3, num - 1);
				num3 += num - 1;
				array3[num3++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006810 File Offset: 0x00004A10
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00006858 File Offset: 0x00004A58
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000068A0 File Offset: 0x00004AA0
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000068E4 File Offset: 0x00004AE4
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400007A RID: 122
		public DateTime? LastModified;

		// Token: 0x0400007B RID: 123
		private int _headerByteCount;

		// Token: 0x0400007C RID: 124
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400007D RID: 125
		private bool _disposed;

		// Token: 0x0400007E RID: 126
		private bool _firstReadDone;

		// Token: 0x0400007F RID: 127
		private string _FileName;

		// Token: 0x04000080 RID: 128
		private string _Comment;

		// Token: 0x04000081 RID: 129
		private int _Crc32;

		// Token: 0x04000082 RID: 130
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04000083 RID: 131
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
