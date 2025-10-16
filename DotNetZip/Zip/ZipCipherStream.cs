using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000045 RID: 69
	internal class ZipCipherStream : Stream
	{
		// Token: 0x0600020E RID: 526 RVA: 0x0000DD56 File Offset: 0x0000BF56
		public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
		{
			this._cipher = cipher;
			this._s = s;
			this._mode = mode;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000DD74 File Offset: 0x0000BF74
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException("This stream does not encrypt via Read()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte[] array = new byte[count];
			int num = this._s.Read(array, 0, count);
			byte[] array2 = this._cipher.DecryptMessage(array, num);
			for (int i = 0; i < num; i++)
			{
				buffer[offset + i] = array2[i];
			}
			return num;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException("This stream does not Decrypt via Write()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count == 0)
			{
				return;
			}
			byte[] array;
			if (offset != 0)
			{
				array = new byte[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = buffer[offset + i];
				}
			}
			else
			{
				array = buffer;
			}
			byte[] array2 = this._cipher.EncryptMessage(array, count);
			this._s.Write(array2, 0, array2.Length);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000DE51 File Offset: 0x0000C051
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoMode.Decrypt;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000DE5C File Offset: 0x0000C05C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000DE5F File Offset: 0x0000C05F
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoMode.Encrypt;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000DE6A File Offset: 0x0000C06A
		public override void Flush()
		{
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000DE6C File Offset: 0x0000C06C
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000DE73 File Offset: 0x0000C073
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000DE7A File Offset: 0x0000C07A
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000DE81 File Offset: 0x0000C081
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000DE88 File Offset: 0x0000C088
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040001BE RID: 446
		private ZipCrypto _cipher;

		// Token: 0x040001BF RID: 447
		private Stream _s;

		// Token: 0x040001C0 RID: 448
		private CryptoMode _mode;
	}
}
