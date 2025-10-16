using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Crc;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x02000054 RID: 84
	public class ZipOutputStream : Stream
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x00018300 File Offset: 0x00016500
		public ZipOutputStream(Stream stream) : this(stream, false)
		{
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001830C File Offset: 0x0001650C
		public ZipOutputStream(string fileName)
		{
			this._alternateEncoding = Encoding.GetEncoding("UTF-8");
			this._maxBufferPairs = 16;
			base..ctor();
			Stream stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			this._Init(stream, false, fileName);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001834A File Offset: 0x0001654A
		public ZipOutputStream(Stream stream, bool leaveOpen)
		{
			this._alternateEncoding = Encoding.GetEncoding("UTF-8");
			this._maxBufferPairs = 16;
			base..ctor();
			this._Init(stream, leaveOpen, null);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00018374 File Offset: 0x00016574
		private void _Init(Stream stream, bool leaveOpen, string name)
		{
			this._outputStream = (stream.CanRead ? stream : new CountingStream(stream));
			this.CompressionLevel = CompressionLevel.Default;
			this.CompressionMethod = CompressionMethod.Deflate;
			this._encryption = EncryptionAlgorithm.None;
			this._entriesWritten = new Dictionary<string, ZipEntry>(StringComparer.Ordinal);
			this._zip64 = Zip64Option.Default;
			this._leaveUnderlyingStreamOpen = leaveOpen;
			this.Strategy = CompressionStrategy.Default;
			this._name = (name ?? "(stream)");
			this.ParallelDeflateThreshold = -1L;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000183EA File Offset: 0x000165EA
		public override string ToString()
		{
			return string.Format("ZipOutputStream::{0}(leaveOpen({1})))", this._name, this._leaveUnderlyingStreamOpen);
		}

		// Token: 0x170000DF RID: 223
		// (set) Token: 0x060003DC RID: 988 RVA: 0x00018408 File Offset: 0x00016608
		public string Password
		{
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._password = value;
				if (this._password == null)
				{
					this._encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this._encryption == EncryptionAlgorithm.None)
				{
					this._encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003DD RID: 989 RVA: 0x00018455 File Offset: 0x00016655
		// (set) Token: 0x060003DE RID: 990 RVA: 0x0001845D File Offset: 0x0001665D
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._encryption;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._encryption = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00018496 File Offset: 0x00016696
		// (set) Token: 0x060003E0 RID: 992 RVA: 0x0001849E File Offset: 0x0001669E
		public int CodecBufferSize { get; set; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x000184A7 File Offset: 0x000166A7
		// (set) Token: 0x060003E2 RID: 994 RVA: 0x000184AF File Offset: 0x000166AF
		public CompressionStrategy Strategy { get; set; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x000184B8 File Offset: 0x000166B8
		// (set) Token: 0x060003E4 RID: 996 RVA: 0x000184C0 File Offset: 0x000166C0
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._timestamp = value;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x000184E3 File Offset: 0x000166E3
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x000184EB File Offset: 0x000166EB
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x000184F4 File Offset: 0x000166F4
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x000184FC File Offset: 0x000166FC
		public CompressionMethod CompressionMethod { get; set; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00018505 File Offset: 0x00016705
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x0001850D File Offset: 0x0001670D
		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._comment = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00018530 File Offset: 0x00016730
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x00018538 File Offset: 0x00016738
		public Zip64Option EnableZip64
		{
			get
			{
				return this._zip64;
			}
			set
			{
				if (this._disposed)
				{
					this._exceptionPending = true;
					throw new InvalidOperationException("The stream has been closed.");
				}
				this._zip64 = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0001855B File Offset: 0x0001675B
		public bool OutputUsedZip64
		{
			get
			{
				return this._anyEntriesUsedZip64 || this._directoryNeededZip64;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001856D File Offset: 0x0001676D
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00018578 File Offset: 0x00016778
		public bool IgnoreCase
		{
			get
			{
				return !this._DontIgnoreCase;
			}
			set
			{
				this._DontIgnoreCase = !value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00018584 File Offset: 0x00016784
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0001859E File Offset: 0x0001679E
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete. It will be removed in a future version of the library. Use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.UTF8 && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.UTF8;
					this._alternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this._alternateEncoding = ZipOutputStream.DefaultEncoding;
				this._alternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000185C8 File Offset: 0x000167C8
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x000185DB File Offset: 0x000167DB
		[Obsolete("use AlternateEncoding and AlternateEncodingUsage instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000185EB File Offset: 0x000167EB
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x000185F3 File Offset: 0x000167F3
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x000185FC File Offset: 0x000167FC
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00018604 File Offset: 0x00016804
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001860D File Offset: 0x0001680D
		public static Encoding DefaultEncoding
		{
			get
			{
				return Encoding.GetEncoding("UTF-8");
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001863E File Offset: 0x0001683E
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00018619 File Offset: 0x00016819
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("value must be greater than 64k, or 0, or -1");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00018646 File Offset: 0x00016846
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0001864E File Offset: 0x0001684E
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001866B File Offset: 0x0001686B
		private void InsureUniqueEntry(ZipEntry ze1)
		{
			if (this._entriesWritten.ContainsKey(ze1.FileName))
			{
				this._exceptionPending = true;
				throw new ArgumentException(string.Format("The entry '{0}' already exists in the zip archive.", ze1.FileName));
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001869D File Offset: 0x0001689D
		internal Stream OutputStream
		{
			get
			{
				return this._outputStream;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x000186A5 File Offset: 0x000168A5
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000186AD File Offset: 0x000168AD
		public bool ContainsEntry(string name)
		{
			return this._entriesWritten.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000186C0 File Offset: 0x000168C0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			if (buffer == null)
			{
				this._exceptionPending = true;
				throw new ArgumentNullException("buffer");
			}
			if (this._currentEntry == null)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You must call PutNextEntry() before calling Write().");
			}
			if (this._currentEntry.IsDirectory)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("You cannot Write() data for an entry that is a directory.");
			}
			if (this._needToWriteEntryHeader)
			{
				this._InitiateCurrentEntry(false);
			}
			if (count != 0)
			{
				this._entryOutputStream.Write(buffer, offset, count);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00018758 File Offset: 0x00016958
		public ZipEntry PutNextEntry(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			if (this._disposed)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("The stream has been closed.");
			}
			this._FinishCurrentEntry();
			this._currentEntry = ZipEntry.CreateForZipOutputStream(entryName);
			this._currentEntry._container = new ZipContainer(this);
			ZipEntry currentEntry = this._currentEntry;
			currentEntry._BitField |= 8;
			this._currentEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			this._currentEntry.CompressionLevel = this.CompressionLevel;
			this._currentEntry.CompressionMethod = this.CompressionMethod;
			this._currentEntry.Password = this._password;
			this._currentEntry.Encryption = this.Encryption;
			this._currentEntry.AlternateEncoding = this.AlternateEncoding;
			this._currentEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			if (entryName.EndsWith("/"))
			{
				this._currentEntry.MarkAsDirectory();
			}
			this._currentEntry.EmitTimesInWindowsFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Windows) > ZipEntryTimestamp.None);
			this._currentEntry.EmitTimesInUnixFormatWhenSaving = ((this._timestamp & ZipEntryTimestamp.Unix) > ZipEntryTimestamp.None);
			this.InsureUniqueEntry(this._currentEntry);
			this._needToWriteEntryHeader = true;
			return this._currentEntry;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x000188A8 File Offset: 0x00016AA8
		private void _InitiateCurrentEntry(bool finishing)
		{
			this._entriesWritten.Add(this._currentEntry.FileName, this._currentEntry);
			this._entryCount++;
			if (this._entryCount > 65534 && this._zip64 == Zip64Option.Default)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Too many entries. Consider setting ZipOutputStream.EnableZip64.");
			}
			this._currentEntry.WriteHeader(this._outputStream, finishing ? 99 : 0);
			this._currentEntry.StoreRelativeOffset();
			if (!this._currentEntry.IsDirectory)
			{
				this._currentEntry.WriteSecurityMetadata(this._outputStream);
				this._currentEntry.PrepOutputStream(this._outputStream, finishing ? 0L : -1L, out this._outputCounter, out this._encryptor, out this._deflater, out this._entryOutputStream);
			}
			this._needToWriteEntryHeader = false;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00018980 File Offset: 0x00016B80
		private void _FinishCurrentEntry()
		{
			if (this._currentEntry != null)
			{
				if (this._needToWriteEntryHeader)
				{
					this._InitiateCurrentEntry(true);
				}
				this._currentEntry.FinishOutputStream(this._outputStream, this._outputCounter, this._encryptor, this._deflater, this._entryOutputStream);
				this._currentEntry.PostProcessOutput(this._outputStream);
				if (this._currentEntry.OutputUsedZip64 != null)
				{
					this._anyEntriesUsedZip64 |= this._currentEntry.OutputUsedZip64.Value;
				}
				this._outputCounter = null;
				this._encryptor = (this._deflater = null);
				this._entryOutputStream = null;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00018A34 File Offset: 0x00016C34
		protected override void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing && !this._exceptionPending)
			{
				this._FinishCurrentEntry();
				this._directoryNeededZip64 = ZipOutput.WriteCentralDirectoryStructure(this._outputStream, this._entriesWritten.Values, 1U, this._zip64, this.Comment, new ZipContainer(this));
				CountingStream countingStream = this._outputStream as CountingStream;
				Stream stream;
				if (countingStream != null)
				{
					stream = countingStream.WrappedStream;
					countingStream.Dispose();
				}
				else
				{
					stream = this._outputStream;
				}
				if (!this._leaveUnderlyingStreamOpen)
				{
					stream.Dispose();
				}
				this._outputStream = null;
			}
			this._disposed = true;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00018ACD File Offset: 0x00016CCD
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00018AD0 File Offset: 0x00016CD0
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00018AD3 File Offset: 0x00016CD3
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00018AD6 File Offset: 0x00016CD6
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00018ADD File Offset: 0x00016CDD
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00018AEA File Offset: 0x00016CEA
		public override long Position
		{
			get
			{
				return this._outputStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00018AF1 File Offset: 0x00016CF1
		public override void Flush()
		{
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00018AF3 File Offset: 0x00016CF3
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("Read");
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00018AFF File Offset: 0x00016CFF
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("Seek");
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00018B0B File Offset: 0x00016D0B
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400029B RID: 667
		private EncryptionAlgorithm _encryption;

		// Token: 0x0400029C RID: 668
		private ZipEntryTimestamp _timestamp;

		// Token: 0x0400029D RID: 669
		internal string _password;

		// Token: 0x0400029E RID: 670
		private string _comment;

		// Token: 0x0400029F RID: 671
		private Stream _outputStream;

		// Token: 0x040002A0 RID: 672
		private ZipEntry _currentEntry;

		// Token: 0x040002A1 RID: 673
		internal Zip64Option _zip64;

		// Token: 0x040002A2 RID: 674
		private Dictionary<string, ZipEntry> _entriesWritten;

		// Token: 0x040002A3 RID: 675
		private int _entryCount;

		// Token: 0x040002A4 RID: 676
		private ZipOption _alternateEncodingUsage;

		// Token: 0x040002A5 RID: 677
		private Encoding _alternateEncoding;

		// Token: 0x040002A6 RID: 678
		private bool _leaveUnderlyingStreamOpen;

		// Token: 0x040002A7 RID: 679
		private bool _disposed;

		// Token: 0x040002A8 RID: 680
		private bool _exceptionPending;

		// Token: 0x040002A9 RID: 681
		private bool _anyEntriesUsedZip64;

		// Token: 0x040002AA RID: 682
		private bool _directoryNeededZip64;

		// Token: 0x040002AB RID: 683
		private CountingStream _outputCounter;

		// Token: 0x040002AC RID: 684
		private Stream _encryptor;

		// Token: 0x040002AD RID: 685
		private Stream _deflater;

		// Token: 0x040002AE RID: 686
		private CrcCalculatorStream _entryOutputStream;

		// Token: 0x040002AF RID: 687
		private bool _needToWriteEntryHeader;

		// Token: 0x040002B0 RID: 688
		private string _name;

		// Token: 0x040002B1 RID: 689
		private bool _DontIgnoreCase;

		// Token: 0x040002B2 RID: 690
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x040002B3 RID: 691
		private long _ParallelDeflateThreshold;

		// Token: 0x040002B4 RID: 692
		private int _maxBufferPairs;
	}
}
