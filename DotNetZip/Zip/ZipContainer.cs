using System;
using System.IO;
using System.Text;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x02000055 RID: 85
	internal class ZipContainer
	{
		// Token: 0x06000410 RID: 1040 RVA: 0x00018B12 File Offset: 0x00016D12
		public ZipContainer(object o)
		{
			this._zf = (o as ZipFile);
			this._zos = (o as ZipOutputStream);
			this._zis = (o as ZipInputStream);
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00018B3E File Offset: 0x00016D3E
		public ZipFile ZipFile
		{
			get
			{
				return this._zf;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00018B46 File Offset: 0x00016D46
		public ZipOutputStream ZipOutputStream
		{
			get
			{
				return this._zos;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00018B4E File Offset: 0x00016D4E
		public string Name
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Name;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos.Name;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00018B7D File Offset: 0x00016D7D
		public string Password
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._Password;
				}
				if (this._zis != null)
				{
					return this._zis._Password;
				}
				return this._zos._password;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00018BB2 File Offset: 0x00016DB2
		public Zip64Option Zip64
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._zip64;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos._zip64;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00018BE1 File Offset: 0x00016DE1
		public int BufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.BufferSize;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return 0;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00018C06 File Offset: 0x00016E06
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00018C31 File Offset: 0x00016E31
		public ParallelDeflateOutputStream ParallelDeflater
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflater;
				}
				if (this._zis != null)
				{
					return null;
				}
				return this._zos.ParallelDeflater;
			}
			set
			{
				if (this._zf != null)
				{
					this._zf.ParallelDeflater = value;
					return;
				}
				if (this._zos != null)
				{
					this._zos.ParallelDeflater = value;
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00018C5C File Offset: 0x00016E5C
		public long ParallelDeflateThreshold
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateThreshold;
				}
				return this._zos.ParallelDeflateThreshold;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00018C7D File Offset: 0x00016E7D
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateMaxBufferPairs;
				}
				return this._zos.ParallelDeflateMaxBufferPairs;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00018C9E File Offset: 0x00016E9E
		public int CodecBufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.CodecBufferSize;
				}
				if (this._zis != null)
				{
					return this._zis.CodecBufferSize;
				}
				return this._zos.CodecBufferSize;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x00018CD3 File Offset: 0x00016ED3
		public CompressionStrategy Strategy
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Strategy;
				}
				return this._zos.Strategy;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00018CF4 File Offset: 0x00016EF4
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.UseZip64WhenSaving;
				}
				return this._zos.EnableZip64;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00018D15 File Offset: 0x00016F15
		public Encoding AlternateEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncoding;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncoding;
				}
				return null;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00018D40 File Offset: 0x00016F40
		public Encoding DefaultEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return ZipFile.DefaultEncoding;
				}
				if (this._zos != null)
				{
					return ZipOutputStream.DefaultEncoding;
				}
				return null;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00018D5F File Offset: 0x00016F5F
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncodingUsage;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncodingUsage;
				}
				return ZipOption.Default;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00018D8A File Offset: 0x00016F8A
		public Stream ReadStream
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ReadStream;
				}
				return this._zis.ReadStream;
			}
		}

		// Token: 0x040002B5 RID: 693
		private ZipFile _zf;

		// Token: 0x040002B6 RID: 694
		private ZipOutputStream _zos;

		// Token: 0x040002B7 RID: 695
		private ZipInputStream _zis;
	}
}
