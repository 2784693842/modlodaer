using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x0200002A RID: 42
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x0600015B RID: 347 RVA: 0x0000CAD6 File Offset: 0x0000ACD6
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000CAE6 File Offset: 0x0000ACE6
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000CAF6 File Offset: 0x0000ACF6
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000CB12 File Offset: 0x0000AD12
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000CB2E File Offset: 0x0000AD2E
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000CB4B File Offset: 0x0000AD4B
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			this._innerStream = stream;
			this._Crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000CB82 File Offset: 0x0000AD82
		public long TotalBytesSlurped
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000CB8F File Offset: 0x0000AD8F
		public int Crc
		{
			get
			{
				return this._Crc32.Crc32Result;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000CBA4 File Offset: 0x0000ADA4
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000CBB0 File Offset: 0x0000ADB0
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._Crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._Crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000CC1E File Offset: 0x0000AE1E
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000CC40 File Offset: 0x0000AE40
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000CC4D File Offset: 0x0000AE4D
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000CC50 File Offset: 0x0000AE50
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000CC5D File Offset: 0x0000AE5D
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000CC6A File Offset: 0x0000AE6A
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000CC8B File Offset: 0x0000AE8B
		// (set) Token: 0x0600016D RID: 365 RVA: 0x0000CC98 File Offset: 0x0000AE98
		public override long Position
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000CC9F File Offset: 0x0000AE9F
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000CCA6 File Offset: 0x0000AEA6
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000CCAD File Offset: 0x0000AEAD
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000CCB5 File Offset: 0x0000AEB5
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x04000172 RID: 370
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x04000173 RID: 371
		internal Stream _innerStream;

		// Token: 0x04000174 RID: 372
		private CRC32 _Crc32;

		// Token: 0x04000175 RID: 373
		private long _lengthLimit = -99L;

		// Token: 0x04000176 RID: 374
		private bool _leaveOpen;
	}
}
