using System;
using System.IO;
using System.Text;
using Ionic.Crc;

namespace Ionic.Zip
{
	// Token: 0x02000053 RID: 83
	public class ZipInputStream : Stream
	{
		// Token: 0x060003BE RID: 958 RVA: 0x00017FD6 File Offset: 0x000161D6
		public ZipInputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00017FE0 File Offset: 0x000161E0
		public ZipInputStream(string fileName)
		{
			Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this._Init(stream, false, fileName);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00018006 File Offset: 0x00016206
		public ZipInputStream(Stream stream, bool leaveOpen)
		{
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00018018 File Offset: 0x00016218
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._inputStream = stream;
			if (!this._inputStream.CanRead)
			{
				throw new ZipException("The stream must be readable.");
			}
			this._container = new ZipContainer(this);
			this._provisionalAlternateEncoding = Encoding.GetEncoding("UTF-8");
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this._findRequired = true;
			this._name = (name ?? "(stream)");
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001807E File Offset: 0x0001627E
		public override string ToString()
		{
			return string.Format("ZipInputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x0001809B File Offset: 0x0001629B
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x000180A3 File Offset: 0x000162A3
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				return this._provisionalAlternateEncoding;
			}
			set
			{
				this._provisionalAlternateEncoding = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x000180AC File Offset: 0x000162AC
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x000180B4 File Offset: 0x000162B4
		public int CodecBufferSize { get; set; }

		// Token: 0x170000D8 RID: 216
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x000180BD File Offset: 0x000162BD
		public string Password
		{
			set
			{
				if (this._closed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._Password = value;
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000180E0 File Offset: 0x000162E0
		private void SetupStream()
		{
			this._crcStream = this._currentEntry.InternalOpenReader(this._Password);
			this._LeftToRead = this._crcStream.Length;
			this._needSetup = false;
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x00018111 File Offset: 0x00016311
		internal Stream ReadStream
		{
			get
			{
				return this._inputStream;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001811C File Offset: 0x0001631C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._closed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (this._needSetup)
			{
				this.SetupStream();
			}
			if (this._LeftToRead == 0L)
			{
				return 0;
			}
			int count2 = (this._LeftToRead > (long)count) ? count : ((int)this._LeftToRead);
			int num = this._crcStream.Read(buffer, offset, count2);
			this._LeftToRead -= (long)num;
			if (this._LeftToRead == 0L)
			{
				int crc = this._crcStream.Crc;
				this._currentEntry.VerifyCrcAfterExtract(crc);
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			return num;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000181C4 File Offset: 0x000163C4
		public ZipEntry GetNextEntry()
		{
			if (this._findRequired)
			{
				if (SharedUtilities.FindSignature(this._inputStream, 67324752) == -1L)
				{
					return null;
				}
				this._inputStream.Seek(-4L, SeekOrigin.Current);
			}
			else if (this._firstEntry)
			{
				this._inputStream.Seek(this._endOfEntry, SeekOrigin.Begin);
			}
			this._currentEntry = ZipEntry.ReadEntry(this._container, !this._firstEntry);
			this._endOfEntry = this._inputStream.Position;
			this._firstEntry = true;
			this._needSetup = true;
			this._findRequired = false;
			return this._currentEntry;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00018262 File Offset: 0x00016462
		protected override void Dispose(bool disposing)
		{
			if (this._closed)
			{
				return;
			}
			if (disposing)
			{
				if (this._exceptionPending)
				{
					return;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					this._inputStream.Dispose();
				}
			}
			this._closed = true;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00018293 File Offset: 0x00016493
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00018296 File Offset: 0x00016496
		public override bool CanSeek
		{
			get
			{
				return this._inputStream.CanSeek;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000182A3 File Offset: 0x000164A3
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000182A6 File Offset: 0x000164A6
		public override long Length
		{
			get
			{
				return this._inputStream.Length;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x000182B3 File Offset: 0x000164B3
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x000182C0 File Offset: 0x000164C0
		public override long Position
		{
			get
			{
				return this._inputStream.Position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000182CB File Offset: 0x000164CB
		public override void Flush()
		{
			throw new NotSupportedException("Flush");
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000182D7 File Offset: 0x000164D7
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Write");
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000182E3 File Offset: 0x000164E3
		public override long Seek(long offset, SeekOrigin origin)
		{
			this._findRequired = true;
			return this._inputStream.Seek(offset, origin);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000182F9 File Offset: 0x000164F9
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000288 RID: 648
		private Stream _inputStream;

		// Token: 0x04000289 RID: 649
		private Encoding _provisionalAlternateEncoding;

		// Token: 0x0400028A RID: 650
		private ZipEntry _currentEntry;

		// Token: 0x0400028B RID: 651
		private bool _firstEntry;

		// Token: 0x0400028C RID: 652
		private bool _needSetup;

		// Token: 0x0400028D RID: 653
		private ZipContainer _container;

		// Token: 0x0400028E RID: 654
		private CrcCalculatorStream _crcStream;

		// Token: 0x0400028F RID: 655
		private long _LeftToRead;

		// Token: 0x04000290 RID: 656
		internal string _Password;

		// Token: 0x04000291 RID: 657
		private long _endOfEntry;

		// Token: 0x04000292 RID: 658
		private string _name;

		// Token: 0x04000293 RID: 659
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x04000294 RID: 660
		private bool _closed;

		// Token: 0x04000295 RID: 661
		private bool _findRequired;

		// Token: 0x04000296 RID: 662
		private bool _exceptionPending;
	}
}
