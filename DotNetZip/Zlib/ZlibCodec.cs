using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02000026 RID: 38
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000117 RID: 279 RVA: 0x0000BE1C File Offset: 0x0000A01C
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000BE24 File Offset: 0x0000A024
		public ZlibCodec()
		{
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000BE3C File Offset: 0x0000A03C
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				if (this.InitializeDeflate() != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				if (this.InitializeInflate() != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000BE96 File Offset: 0x0000A096
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000BEB3 File Offset: 0x0000A0B3
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000BEC4 File Offset: 0x0000A0C4
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000BEF9 File Offset: 0x0000A0F9
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000BF1A File Offset: 0x0000A11A
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000BF41 File Offset: 0x0000A141
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000BF61 File Offset: 0x0000A161
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000BF6A File Offset: 0x0000A16A
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000BF7A File Offset: 0x0000A17A
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000BF8A File Offset: 0x0000A18A
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000BFA1 File Offset: 0x0000A1A1
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000BFB8 File Offset: 0x0000A1B8
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000C00D File Offset: 0x0000A20D
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000C02E File Offset: 0x0000A22E
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000C04B File Offset: 0x0000A24B
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000C06B File Offset: 0x0000A26B
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000C08D File Offset: 0x0000A28D
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000C0C4 File Offset: 0x0000A2C4
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000C210 File Offset: 0x0000A410
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x04000151 RID: 337
		public byte[] InputBuffer;

		// Token: 0x04000152 RID: 338
		public int NextIn;

		// Token: 0x04000153 RID: 339
		public int AvailableBytesIn;

		// Token: 0x04000154 RID: 340
		public long TotalBytesIn;

		// Token: 0x04000155 RID: 341
		public byte[] OutputBuffer;

		// Token: 0x04000156 RID: 342
		public int NextOut;

		// Token: 0x04000157 RID: 343
		public int AvailableBytesOut;

		// Token: 0x04000158 RID: 344
		public long TotalBytesOut;

		// Token: 0x04000159 RID: 345
		public string Message;

		// Token: 0x0400015A RID: 346
		internal DeflateManager dstate;

		// Token: 0x0400015B RID: 347
		internal InflateManager istate;

		// Token: 0x0400015C RID: 348
		internal uint _Adler32;

		// Token: 0x0400015D RID: 349
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x0400015E RID: 350
		public int WindowBits = 15;

		// Token: 0x0400015F RID: 351
		public CompressionStrategy Strategy;
	}
}
