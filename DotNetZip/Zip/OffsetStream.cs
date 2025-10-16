using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x0200003F RID: 63
	internal class OffsetStream : Stream, IDisposable
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0000D0E6 File Offset: 0x0000B2E6
		public OffsetStream(Stream s)
		{
			this._originalPosition = s.Position;
			this._innerStream = s;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000D101 File Offset: 0x0000B301
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._innerStream.Read(buffer, offset, count);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000D111 File Offset: 0x0000B311
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000D118 File Offset: 0x0000B318
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000D125 File Offset: 0x0000B325
		public override bool CanSeek
		{
			get
			{
				return this._innerStream.CanSeek;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000D132 File Offset: 0x0000B332
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000D135 File Offset: 0x0000B335
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000D142 File Offset: 0x0000B342
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000D14F File Offset: 0x0000B34F
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000D163 File Offset: 0x0000B363
		public override long Position
		{
			get
			{
				return this._innerStream.Position - this._originalPosition;
			}
			set
			{
				this._innerStream.Position = this._originalPosition + value;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000D178 File Offset: 0x0000B378
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(this._originalPosition + offset, origin) - this._originalPosition;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000D195 File Offset: 0x0000B395
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000D19C File Offset: 0x0000B39C
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000D1A4 File Offset: 0x0000B3A4
		public override void Close()
		{
			base.Close();
		}

		// Token: 0x040001A4 RID: 420
		private long _originalPosition;

		// Token: 0x040001A5 RID: 421
		private Stream _innerStream;
	}
}
