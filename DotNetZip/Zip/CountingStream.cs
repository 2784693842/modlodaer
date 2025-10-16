using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000041 RID: 65
	public class CountingStream : Stream
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x0000D95C File Offset: 0x0000BB5C
		public CountingStream(Stream stream)
		{
			this._s = stream;
			try
			{
				this._initialOffset = this._s.Position;
			}
			catch
			{
				this._initialOffset = 0L;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
		public Stream WrappedStream
		{
			get
			{
				return this._s;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000D9AC File Offset: 0x0000BBAC
		public long BytesWritten
		{
			get
			{
				return this._bytesWritten;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		public long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D9BC File Offset: 0x0000BBBC
		public void Adjust(long delta)
		{
			this._bytesWritten -= delta;
			if (this._bytesWritten < 0L)
			{
				throw new InvalidOperationException();
			}
			if (this._s is CountingStream)
			{
				((CountingStream)this._s).Adjust(delta);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000D9FC File Offset: 0x0000BBFC
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._s.Read(buffer, offset, count);
			this._bytesRead += (long)num;
			return num;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000DA28 File Offset: 0x0000BC28
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			this._s.Write(buffer, offset, count);
			this._bytesWritten += (long)count;
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000DA4B File Offset: 0x0000BC4B
		public override bool CanRead
		{
			get
			{
				return this._s.CanRead;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000DA58 File Offset: 0x0000BC58
		public override bool CanSeek
		{
			get
			{
				return this._s.CanSeek;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000DA65 File Offset: 0x0000BC65
		public override bool CanWrite
		{
			get
			{
				return this._s.CanWrite;
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000DA72 File Offset: 0x0000BC72
		public override void Flush()
		{
			this._s.Flush();
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000DA7F File Offset: 0x0000BC7F
		public override long Length
		{
			get
			{
				return this._s.Length;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		public long ComputedPosition
		{
			get
			{
				return this._initialOffset + this._bytesWritten;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000DA9B File Offset: 0x0000BC9B
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
		public override long Position
		{
			get
			{
				return this._s.Position;
			}
			set
			{
				this._s.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._s.Seek(offset, origin);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000DAC7 File Offset: 0x0000BCC7
		public override void SetLength(long value)
		{
			this._s.SetLength(value);
		}

		// Token: 0x040001A8 RID: 424
		private Stream _s;

		// Token: 0x040001A9 RID: 425
		private long _bytesWritten;

		// Token: 0x040001AA RID: 426
		private long _bytesRead;

		// Token: 0x040001AB RID: 427
		private long _initialOffset;
	}
}
