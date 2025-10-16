using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Ionic.Crc;
using Ionic.Zlib;

namespace Ionic.Zip
{
	// Token: 0x02000046 RID: 70
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00004")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ZipEntry
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000DE8F File Offset: 0x0000C08F
		internal bool AttributesIndicateDirectory
		{
			get
			{
				return this._InternalFileAttrs == 0 && (this._ExternalFileAttrs & 16) == 16;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000DEA8 File Offset: 0x0000C0A8
		internal void ResetDirEntry()
		{
			this.__FileDataPosition = -1L;
			this._LengthOfHeader = 0;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000DEBC File Offset: 0x0000C0BC
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipEntry: {0}\n", this.FileName)).Append(string.Format("   Version Made By: {0}\n", this._VersionMadeBy)).Append(string.Format(" Needed to extract: {0}\n", this.VersionNeeded));
				if (this._IsDirectory)
				{
					stringBuilder.Append("        Entry type: directory\n");
				}
				else
				{
					stringBuilder.Append(string.Format("         File type: {0}\n", this._IsText ? "text" : "binary")).Append(string.Format("       Compression: {0}\n", this.CompressionMethod)).Append(string.Format("        Compressed: 0x{0:X}\n", this.CompressedSize)).Append(string.Format("      Uncompressed: 0x{0:X}\n", this.UncompressedSize)).Append(string.Format("             CRC32: 0x{0:X8}\n", this._Crc32));
				}
				stringBuilder.Append(string.Format("       Disk Number: {0}\n", this._diskNumber));
				if (this._RelativeOffsetOfLocalHeader > (long)((ulong)-1))
				{
					stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X16}\n", this._RelativeOffsetOfLocalHeader));
				}
				else
				{
					stringBuilder.Append(string.Format("   Relative Offset: 0x{0:X8}\n", this._RelativeOffsetOfLocalHeader));
				}
				stringBuilder.Append(string.Format("         Bit Field: 0x{0:X4}\n", this._BitField)).Append(string.Format("        Encrypted?: {0}\n", this._sourceIsEncrypted)).Append(string.Format("          Timeblob: 0x{0:X8}\n", this._TimeBlob)).Append(string.Format("              Time: {0}\n", SharedUtilities.PackedToDateTime(this._TimeBlob)));
				stringBuilder.Append(string.Format("         Is Zip64?: {0}\n", this._InputUsesZip64));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("           Comment: {0}\n", this._Comment));
				}
				stringBuilder.Append("\n");
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		internal static ZipEntry ReadDirEntry(ZipFile zf, Dictionary<string, object> previouslySeen)
		{
			Stream readStream = zf.ReadStream;
			Encoding encoding = (zf.AlternateEncodingUsage == ZipOption.Always) ? zf.AlternateEncoding : ZipFile.DefaultEncoding;
			int num = SharedUtilities.ReadSignature(readStream);
			if (ZipEntry.IsNotValidZipDirEntrySig(num))
			{
				readStream.Seek(-4L, SeekOrigin.Current);
				if ((long)num != 101010256L && (long)num != 101075792L && num != 67324752)
				{
					throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position 0x{1:X8}", num, readStream.Position));
				}
				return null;
			}
			else
			{
				int num2 = 46;
				byte[] array = new byte[42];
				int num3 = readStream.Read(array, 0, array.Length);
				if (num3 != array.Length)
				{
					return null;
				}
				int num4 = 0;
				ZipEntry zipEntry = new ZipEntry();
				zipEntry.AlternateEncoding = encoding;
				zipEntry._Source = ZipEntrySource.ZipFile;
				zipEntry._container = new ZipContainer(zf);
				zipEntry._VersionMadeBy = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._VersionNeeded = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._BitField = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._CompressionMethod = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._TimeBlob = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				zipEntry._LastModified = SharedUtilities.PackedToDateTime(zipEntry._TimeBlob);
				zipEntry._timestamp |= ZipEntryTimestamp.DOS;
				zipEntry._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				zipEntry._CompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				zipEntry._UncompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				zipEntry._CompressionMethod_FromZipFile = zipEntry._CompressionMethod;
				zipEntry._filenameLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._extraFieldLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._commentLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._diskNumber = (uint)array[num4++] + (uint)array[num4++] * 256U;
				zipEntry._InternalFileAttrs = (short)((int)array[num4++] + (int)array[num4++] * 256);
				zipEntry._ExternalFileAttrs = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				zipEntry._RelativeOffsetOfLocalHeader = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				zipEntry.IsText = ((zipEntry._InternalFileAttrs & 1) == 1);
				array = new byte[(int)zipEntry._filenameLength];
				num3 = readStream.Read(array, 0, array.Length);
				num2 += num3;
				if ((zipEntry._BitField & 2048) == 2048)
				{
					zipEntry._FileNameInArchive = SharedUtilities.Utf8StringFromBuffer(array);
				}
				else
				{
					zipEntry._FileNameInArchive = SharedUtilities.StringFromBuffer(array, encoding);
				}
				while (previouslySeen.ContainsKey(zipEntry._FileNameInArchive))
				{
					zipEntry._FileNameInArchive = ZipEntry.CopyHelper.AppendCopyToFileName(zipEntry._FileNameInArchive);
					zipEntry._metadataChanged = true;
				}
				if (zipEntry.AttributesIndicateDirectory)
				{
					zipEntry.MarkAsDirectory();
				}
				else if (zipEntry._FileNameInArchive.EndsWith("/"))
				{
					zipEntry.MarkAsDirectory();
				}
				zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				if ((zipEntry._BitField & 1) == 1)
				{
					zipEntry._Encryption_FromZipFile = (zipEntry._Encryption = EncryptionAlgorithm.PkzipWeak);
					zipEntry._sourceIsEncrypted = true;
				}
				if (zipEntry._extraFieldLength > 0)
				{
					zipEntry._InputUsesZip64 = (zipEntry._CompressedSize == (long)((ulong)-1) || zipEntry._UncompressedSize == (long)((ulong)-1) || zipEntry._RelativeOffsetOfLocalHeader == (long)((ulong)-1));
					num2 += zipEntry.ProcessExtraField(readStream, zipEntry._extraFieldLength);
					zipEntry._CompressedFileDataSize = zipEntry._CompressedSize;
				}
				if (zipEntry._Encryption == EncryptionAlgorithm.PkzipWeak)
				{
					zipEntry._CompressedFileDataSize -= 12L;
				}
				if ((zipEntry._BitField & 8) == 8)
				{
					if (zipEntry._InputUsesZip64)
					{
						zipEntry._LengthOfTrailer += 24;
					}
					else
					{
						zipEntry._LengthOfTrailer += 16;
					}
				}
				zipEntry.AlternateEncoding = (((zipEntry._BitField & 2048) == 2048) ? Encoding.UTF8 : encoding);
				zipEntry.AlternateEncodingUsage = ZipOption.Always;
				if (zipEntry._commentLength > 0)
				{
					array = new byte[(int)zipEntry._commentLength];
					num3 = readStream.Read(array, 0, array.Length);
					num2 += num3;
					if ((zipEntry._BitField & 2048) == 2048)
					{
						zipEntry._Comment = SharedUtilities.Utf8StringFromBuffer(array);
					}
					else
					{
						zipEntry._Comment = SharedUtilities.StringFromBuffer(array, encoding);
					}
				}
				return zipEntry;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000E74E File Offset: 0x0000C94E
		internal static bool IsNotValidZipDirEntrySig(int signature)
		{
			return signature != 33639248;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000E75C File Offset: 0x0000C95C
		public ZipEntry()
		{
			this._CompressionMethod = 8;
			this._CompressionLevel = CompressionLevel.Default;
			this._Encryption = EncryptionAlgorithm.None;
			this._Source = ZipEntrySource.None;
			this.AlternateEncoding = Encoding.GetEncoding("utf-8");
			this.AlternateEncodingUsage = ZipOption.Default;
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000E7C3 File Offset: 0x0000C9C3
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
		public DateTime LastModified
		{
			get
			{
				return this._LastModified.ToLocalTime();
			}
			set
			{
				this._LastModified = ((value.Kind == DateTimeKind.Unspecified) ? DateTime.SpecifyKind(value, DateTimeKind.Local) : value.ToLocalTime());
				this._Mtime = SharedUtilities.AdjustTime_Reverse(this._LastModified).ToUniversalTime();
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000E81C File Offset: 0x0000CA1C
		private int BufferSize
		{
			get
			{
				return this._container.BufferSize;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000E829 File Offset: 0x0000CA29
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000E831 File Offset: 0x0000CA31
		public DateTime ModifiedTime
		{
			get
			{
				return this._Mtime;
			}
			set
			{
				this.SetEntryTimes(this._Ctime, this._Atime, value);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000E846 File Offset: 0x0000CA46
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000E84E File Offset: 0x0000CA4E
		public DateTime AccessedTime
		{
			get
			{
				return this._Atime;
			}
			set
			{
				this.SetEntryTimes(this._Ctime, value, this._Mtime);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000E863 File Offset: 0x0000CA63
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000E86B File Offset: 0x0000CA6B
		public DateTime CreationTime
		{
			get
			{
				return this._Ctime;
			}
			set
			{
				this.SetEntryTimes(value, this._Atime, this._Mtime);
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000E880 File Offset: 0x0000CA80
		public void SetEntryTimes(DateTime created, DateTime accessed, DateTime modified)
		{
			this._ntfsTimesAreSet = true;
			if (created == ZipEntry._zeroHour && created.Kind == ZipEntry._zeroHour.Kind)
			{
				created = ZipEntry._win32Epoch;
			}
			if (accessed == ZipEntry._zeroHour && accessed.Kind == ZipEntry._zeroHour.Kind)
			{
				accessed = ZipEntry._win32Epoch;
			}
			if (modified == ZipEntry._zeroHour && modified.Kind == ZipEntry._zeroHour.Kind)
			{
				modified = ZipEntry._win32Epoch;
			}
			this._Ctime = created.ToUniversalTime();
			this._Atime = accessed.ToUniversalTime();
			this._Mtime = modified.ToUniversalTime();
			this._LastModified = this._Mtime;
			if (!this._emitUnixTimes && !this._emitNtfsTimes)
			{
				this._emitNtfsTimes = true;
			}
			this._metadataChanged = true;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000E95A File Offset: 0x0000CB5A
		// (set) Token: 0x0600022B RID: 555 RVA: 0x0000E962 File Offset: 0x0000CB62
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000E972 File Offset: 0x0000CB72
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000E97A File Offset: 0x0000CB7A
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000E98A File Offset: 0x0000CB8A
		public ZipEntryTimestamp Timestamp
		{
			get
			{
				return this._timestamp;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000E992 File Offset: 0x0000CB92
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000E99A File Offset: 0x0000CB9A
		public FileAttributes Attributes
		{
			get
			{
				return (FileAttributes)this._ExternalFileAttrs;
			}
			set
			{
				this._ExternalFileAttrs = (int)value;
				this._VersionMadeBy = 45;
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000E9B2 File Offset: 0x0000CBB2
		internal string LocalFileName
		{
			get
			{
				return this._LocalFileName;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000E9BA File Offset: 0x0000CBBA
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000E9C4 File Offset: 0x0000CBC4
		public string FileName
		{
			get
			{
				return this._FileNameInArchive;
			}
			set
			{
				if (this._container.ZipFile == null)
				{
					throw new ZipException("Cannot rename; this is not supported in ZipOutputStream/ZipInputStream.");
				}
				if (string.IsNullOrEmpty(value))
				{
					throw new ZipException("The FileName must be non empty and non-null.");
				}
				string text = ZipEntry.NameInArchive(value, null);
				if (this._FileNameInArchive == text)
				{
					return;
				}
				this._container.ZipFile.RemoveEntry(this);
				this._container.ZipFile.InternalAddEntry(text, this);
				this._FileNameInArchive = text;
				this._container.ZipFile.NotifyEntryChanged();
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000EA54 File Offset: 0x0000CC54
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000EA5C File Offset: 0x0000CC5C
		public Stream InputStream
		{
			get
			{
				return this._sourceStream;
			}
			set
			{
				if (this._Source != ZipEntrySource.Stream)
				{
					throw new ZipException("You must not set the input stream for this entry.");
				}
				this._sourceWasJitProvided = true;
				this._sourceStream = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000EA80 File Offset: 0x0000CC80
		public bool InputStreamWasJitProvided
		{
			get
			{
				return this._sourceWasJitProvided;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000EA88 File Offset: 0x0000CC88
		public ZipEntrySource Source
		{
			get
			{
				return this._Source;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000EA90 File Offset: 0x0000CC90
		public short VersionNeeded
		{
			get
			{
				return this._VersionNeeded;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000EA98 File Offset: 0x0000CC98
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000EAA0 File Offset: 0x0000CCA0
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._metadataChanged = true;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
		public bool? RequiresZip64
		{
			get
			{
				return this._entryRequiresZip64;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000EAB8 File Offset: 0x0000CCB8
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		public short BitField
		{
			get
			{
				return this._BitField;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000EAC8 File Offset: 0x0000CCC8
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
		public CompressionMethod CompressionMethod
		{
			get
			{
				return (CompressionMethod)this._CompressionMethod;
			}
			set
			{
				if (value == (CompressionMethod)this._CompressionMethod)
				{
					return;
				}
				if (value != CompressionMethod.None && value != CompressionMethod.Deflate)
				{
					throw new InvalidOperationException("Unsupported compression method.");
				}
				this._CompressionMethod = (short)value;
				if (this._CompressionMethod == 0)
				{
					this._CompressionLevel = CompressionLevel.None;
				}
				else if (this.CompressionLevel == CompressionLevel.None)
				{
					this._CompressionLevel = CompressionLevel.Default;
				}
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
				this._restreamRequiredOnSave = true;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000EB45 File Offset: 0x0000CD45
		// (set) Token: 0x06000241 RID: 577 RVA: 0x0000EB50 File Offset: 0x0000CD50
		public CompressionLevel CompressionLevel
		{
			get
			{
				return this._CompressionLevel;
			}
			set
			{
				if (this._CompressionMethod != 8 && this._CompressionMethod != 0)
				{
					return;
				}
				if (value == CompressionLevel.Default && this._CompressionMethod == 8)
				{
					return;
				}
				this._CompressionLevel = value;
				if (value == CompressionLevel.None && this._CompressionMethod == 0)
				{
					return;
				}
				if (this._CompressionLevel == CompressionLevel.None)
				{
					this._CompressionMethod = 0;
				}
				else
				{
					this._CompressionMethod = 8;
				}
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
				this._restreamRequiredOnSave = true;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000EBCC File Offset: 0x0000CDCC
		public long CompressedSize
		{
			get
			{
				return this._CompressedSize;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000EBD4 File Offset: 0x0000CDD4
		public long UncompressedSize
		{
			get
			{
				return this._UncompressedSize;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000EBDC File Offset: 0x0000CDDC
		public double CompressionRatio
		{
			get
			{
				if (this.UncompressedSize == 0L)
				{
					return 0.0;
				}
				return 100.0 * (1.0 - 1.0 * (double)this.CompressedSize / (1.0 * (double)this.UncompressedSize));
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000EC32 File Offset: 0x0000CE32
		public int Crc
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000EC3A File Offset: 0x0000CE3A
		public bool IsDirectory
		{
			get
			{
				return this._IsDirectory;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000EC42 File Offset: 0x0000CE42
		public bool UsesEncryption
		{
			get
			{
				return this._Encryption_FromZipFile > EncryptionAlgorithm.None;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000EC4D File Offset: 0x0000CE4D
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000EC58 File Offset: 0x0000CE58
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == this._Encryption)
				{
					return;
				}
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
				this._restreamRequiredOnSave = true;
				if (this._container.ZipFile != null)
				{
					this._container.ZipFile.NotifyEntryChanged();
				}
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000ECE9 File Offset: 0x0000CEE9
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000ECA9 File Offset: 0x0000CEA9
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password == null)
				{
					this._Encryption = EncryptionAlgorithm.None;
					return;
				}
				if (this._Source == ZipEntrySource.ZipFile && !this._sourceIsEncrypted)
				{
					this._restreamRequiredOnSave = true;
				}
				if (this.Encryption == EncryptionAlgorithm.None)
				{
					this._Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000ECF1 File Offset: 0x0000CEF1
		internal bool IsChanged
		{
			get
			{
				return this._restreamRequiredOnSave | this._metadataChanged;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000ED00 File Offset: 0x0000CF00
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000ED08 File Offset: 0x0000CF08
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000ED11 File Offset: 0x0000CF11
		// (set) Token: 0x06000250 RID: 592 RVA: 0x0000ED19 File Offset: 0x0000CF19
		public ZipErrorAction ZipErrorAction { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000ED22 File Offset: 0x0000CF22
		public bool IncludedInMostRecentSave
		{
			get
			{
				return !this._skippedDuringSave;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000ED2D File Offset: 0x0000CF2D
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000ED35 File Offset: 0x0000CF35
		public SetCompressionCallback SetCompression { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000ED3E File Offset: 0x0000CF3E
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000ED5D File Offset: 0x0000CF5D
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this.AlternateEncoding == Encoding.GetEncoding("UTF-8") && this.AlternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this.AlternateEncoding = Encoding.GetEncoding("UTF-8");
					this.AlternateEncodingUsage = ZipOption.AsNecessary;
					return;
				}
				this.AlternateEncoding = ZipFile.DefaultEncoding;
				this.AlternateEncodingUsage = ZipOption.Default;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000ED8C File Offset: 0x0000CF8C
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000ED94 File Offset: 0x0000CF94
		[Obsolete("This property is obsolete since v1.9.1.6. Use AlternateEncoding and AlternateEncodingUsage instead.", true)]
		public Encoding ProvisionalAlternateEncoding { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000ED9D File Offset: 0x0000CF9D
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000EDA5 File Offset: 0x0000CFA5
		public Encoding AlternateEncoding { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000EDAE File Offset: 0x0000CFAE
		// (set) Token: 0x0600025B RID: 603 RVA: 0x0000EDB6 File Offset: 0x0000CFB6
		public ZipOption AlternateEncodingUsage { get; set; }

		// Token: 0x0600025C RID: 604 RVA: 0x0000EDC0 File Offset: 0x0000CFC0
		internal static string NameInArchive(string filename, string directoryPathInArchive)
		{
			string pathName;
			if (directoryPathInArchive == null)
			{
				pathName = filename;
			}
			else if (string.IsNullOrEmpty(directoryPathInArchive))
			{
				pathName = Path.GetFileName(filename);
			}
			else
			{
				pathName = Path.Combine(directoryPathInArchive, Path.GetFileName(filename));
			}
			return SharedUtilities.NormalizePathForUseInZipFile(pathName);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000EDFC File Offset: 0x0000CFFC
		internal static ZipEntry CreateFromNothing(string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.None, null, null);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000EE07 File Offset: 0x0000D007
		internal static ZipEntry CreateFromFile(string filename, string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.FileSystem, filename, null);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000EE12 File Offset: 0x0000D012
		internal static ZipEntry CreateForStream(string entryName, Stream s)
		{
			return ZipEntry.Create(entryName, ZipEntrySource.Stream, s, null);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000EE1D File Offset: 0x0000D01D
		internal static ZipEntry CreateForWriter(string entryName, WriteDelegate d)
		{
			return ZipEntry.Create(entryName, ZipEntrySource.WriteDelegate, d, null);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000EE28 File Offset: 0x0000D028
		internal static ZipEntry CreateForJitStreamProvider(string nameInArchive, OpenDelegate opener, CloseDelegate closer)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.JitStream, opener, closer);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000EE33 File Offset: 0x0000D033
		internal static ZipEntry CreateForZipOutputStream(string nameInArchive)
		{
			return ZipEntry.Create(nameInArchive, ZipEntrySource.ZipOutputStream, null, null);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000EE40 File Offset: 0x0000D040
		private static ZipEntry Create(string nameInArchive, ZipEntrySource source, object arg1, object arg2)
		{
			if (string.IsNullOrEmpty(nameInArchive))
			{
				throw new ZipException("The entry name must be non-null and non-empty.");
			}
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._VersionMadeBy = 45;
			zipEntry._Source = source;
			zipEntry._Mtime = (zipEntry._Atime = (zipEntry._Ctime = DateTime.UtcNow));
			if (source == ZipEntrySource.Stream)
			{
				zipEntry._sourceStream = (arg1 as Stream);
			}
			else if (source == ZipEntrySource.WriteDelegate)
			{
				zipEntry._WriteDelegate = (arg1 as WriteDelegate);
			}
			else if (source == ZipEntrySource.JitStream)
			{
				zipEntry._OpenDelegate = (arg1 as OpenDelegate);
				zipEntry._CloseDelegate = (arg2 as CloseDelegate);
			}
			else if (source != ZipEntrySource.ZipOutputStream)
			{
				if (source == ZipEntrySource.None)
				{
					zipEntry._Source = ZipEntrySource.FileSystem;
				}
				else
				{
					string text = arg1 as string;
					if (string.IsNullOrEmpty(text))
					{
						throw new ZipException("The filename must be non-null and non-empty.");
					}
					try
					{
						zipEntry._Mtime = File.GetLastWriteTime(text).ToUniversalTime();
						zipEntry._Ctime = File.GetCreationTime(text).ToUniversalTime();
						zipEntry._Atime = File.GetLastAccessTime(text).ToUniversalTime();
						if (File.Exists(text) || Directory.Exists(text))
						{
							zipEntry._ExternalFileAttrs = (int)File.GetAttributes(text);
						}
						zipEntry._ntfsTimesAreSet = true;
						zipEntry._LocalFileName = Path.GetFullPath(text);
					}
					catch (PathTooLongException innerException)
					{
						throw new ZipException(string.Format("The path is too long, filename={0}", text), innerException);
					}
				}
			}
			zipEntry._LastModified = zipEntry._Mtime;
			zipEntry._FileNameInArchive = SharedUtilities.NormalizePathForUseInZipFile(nameInArchive);
			return zipEntry;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		internal void MarkAsDirectory()
		{
			this._IsDirectory = true;
			if (!this._FileNameInArchive.EndsWith("/"))
			{
				this._FileNameInArchive += "/";
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000EFE9 File Offset: 0x0000D1E9
		// (set) Token: 0x06000266 RID: 614 RVA: 0x0000EFF1 File Offset: 0x0000D1F1
		public bool IsText
		{
			get
			{
				return this._IsText;
			}
			set
			{
				this._IsText = value;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000EFFA File Offset: 0x0000D1FA
		public override string ToString()
		{
			return string.Format("ZipEntry::{0}", this.FileName);
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000F00C File Offset: 0x0000D20C
		internal Stream ArchiveStream
		{
			get
			{
				if (this._archiveStream == null)
				{
					if (this._container.ZipFile != null)
					{
						ZipFile zipFile = this._container.ZipFile;
						zipFile.Reset(false);
						this._archiveStream = zipFile.StreamForDiskNumber(this._diskNumber);
					}
					else
					{
						this._archiveStream = this._container.ZipOutputStream.OutputStream;
					}
				}
				return this._archiveStream;
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000F074 File Offset: 0x0000D274
		private void SetFdpLoh()
		{
			long position = this.ArchiveStream.Position;
			try
			{
				this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			}
			catch (IOException innerException)
			{
				throw new BadStateException(string.Format("Exception seeking  entry({0}) offset(0x{1:X8}) len(0x{2:X8})", this.FileName, this._RelativeOffsetOfLocalHeader, this.ArchiveStream.Length), innerException);
			}
			byte[] array = new byte[30];
			this.ArchiveStream.Read(array, 0, array.Length);
			short num = (short)((int)array[26] + (int)array[27] * 256);
			short num2 = (short)((int)array[28] + (int)array[29] * 256);
			this.ArchiveStream.Seek((long)(num + num2), SeekOrigin.Current);
			this._LengthOfHeader = (int)(30 + num2 + num) + ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
			this.__FileDataPosition = this._RelativeOffsetOfLocalHeader + (long)this._LengthOfHeader;
			this.ArchiveStream.Seek(position, SeekOrigin.Begin);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000F170 File Offset: 0x0000D370
		internal static int GetLengthOfCryptoHeaderBytes(EncryptionAlgorithm a)
		{
			if (a == EncryptionAlgorithm.None)
			{
				return 0;
			}
			if (a == EncryptionAlgorithm.PkzipWeak)
			{
				return 12;
			}
			throw new ZipException("internal error");
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000F188 File Offset: 0x0000D388
		internal long FileDataPosition
		{
			get
			{
				if (this.__FileDataPosition == -1L)
				{
					this.SetFdpLoh();
				}
				return this.__FileDataPosition;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000F1A0 File Offset: 0x0000D3A0
		private int LengthOfHeader
		{
			get
			{
				if (this._LengthOfHeader == 0)
				{
					this.SetFdpLoh();
				}
				return this._LengthOfHeader;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F1B6 File Offset: 0x0000D3B6
		public void Extract()
		{
			this.InternalExtract(".", null, null);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000F1C5 File Offset: 0x0000D3C5
		public void Extract(ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(".", null, null);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000F1DB File Offset: 0x0000D3DB
		public void Extract(Stream stream)
		{
			this.InternalExtract(null, stream, null);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000F1E6 File Offset: 0x0000D3E6
		public void Extract(string baseDirectory)
		{
			this.InternalExtract(baseDirectory, null, null);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000F1F1 File Offset: 0x0000D3F1
		public void Extract(string baseDirectory, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(baseDirectory, null, null);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000F203 File Offset: 0x0000D403
		public void ExtractWithPassword(string password)
		{
			this.InternalExtract(".", null, password);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000F212 File Offset: 0x0000D412
		public void ExtractWithPassword(string baseDirectory, string password)
		{
			this.InternalExtract(baseDirectory, null, password);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000F21D File Offset: 0x0000D41D
		public void ExtractWithPassword(ExtractExistingFileAction extractExistingFile, string password)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(".", null, password);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000F233 File Offset: 0x0000D433
		public void ExtractWithPassword(string baseDirectory, ExtractExistingFileAction extractExistingFile, string password)
		{
			this.ExtractExistingFile = extractExistingFile;
			this.InternalExtract(baseDirectory, null, password);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000F245 File Offset: 0x0000D445
		public void ExtractWithPassword(Stream stream, string password)
		{
			this.InternalExtract(null, stream, password);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000F250 File Offset: 0x0000D450
		public CrcCalculatorStream OpenReader()
		{
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return this.InternalOpenReader(this._Password ?? this._container.Password);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000F285 File Offset: 0x0000D485
		public CrcCalculatorStream OpenReader(string password)
		{
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use OpenReader() only with ZipFile.");
			}
			return this.InternalOpenReader(password);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000F2A8 File Offset: 0x0000D4A8
		internal CrcCalculatorStream InternalOpenReader(string password)
		{
			this.ValidateCompression();
			this.ValidateEncryption();
			this.SetupCryptoForExtract(password);
			if (this._Source != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling OpenReader");
			}
			long length = (this._CompressionMethod_FromZipFile == 0) ? this._CompressedFileDataSize : this.UncompressedSize;
			Stream archiveStream = this.ArchiveStream;
			this.ArchiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
			this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
			return new CrcCalculatorStream(this.GetExtractDecompressor(this._inputDecryptorStream), length);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000F32C File Offset: 0x0000D52C
		private void OnExtractProgress(long bytesWritten, long totalBytesToWrite)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnExtractBlock(this, bytesWritten, totalBytesToWrite);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000F354 File Offset: 0x0000D554
		private void OnBeforeExtract(string path)
		{
			if (this._container.ZipFile != null && !this._container.ZipFile._inExtractAll)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnSingleEntryExtract(this, path, true);
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000F38E File Offset: 0x0000D58E
		private void OnAfterExtract(string path)
		{
			if (this._container.ZipFile != null && !this._container.ZipFile._inExtractAll)
			{
				this._container.ZipFile.OnSingleEntryExtract(this, path, false);
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000F3C3 File Offset: 0x0000D5C3
		private void OnExtractExisting(string path)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnExtractExisting(this, path);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000F3EA File Offset: 0x0000D5EA
		private static void ReallyDelete(string fileName)
		{
			if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				File.SetAttributes(fileName, FileAttributes.Normal);
			}
			File.Delete(fileName);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000F408 File Offset: 0x0000D608
		private void WriteStatus(string format, params object[] args)
		{
			if (this._container.ZipFile != null && this._container.ZipFile.Verbose)
			{
				this._container.ZipFile.StatusMessageTextWriter.WriteLine(format, args);
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000F440 File Offset: 0x0000D640
		private void InternalExtract(string baseDir, Stream outstream, string password)
		{
			if (this._container == null)
			{
				throw new BadStateException("This entry is an orphan");
			}
			if (this._container.ZipFile == null)
			{
				throw new InvalidOperationException("Use Extract() only with ZipFile.");
			}
			this._container.ZipFile.Reset(false);
			if (this._Source != ZipEntrySource.ZipFile)
			{
				throw new BadStateException("You must call ZipFile.Save before calling any Extract method");
			}
			this.OnBeforeExtract(baseDir);
			this._ioOperationCanceled = false;
			string text = null;
			Stream stream = null;
			bool flag = false;
			bool flag2 = false;
			try
			{
				this.ValidateCompression();
				this.ValidateEncryption();
				if (this.ValidateOutput(baseDir, outstream, out text))
				{
					this.WriteStatus("extract dir {0}...", new object[]
					{
						text
					});
					this.OnAfterExtract(baseDir);
				}
				else
				{
					if (text != null && File.Exists(text))
					{
						flag = true;
						int num = this.CheckExtractExistingFile(baseDir, text);
						if (num == 2)
						{
							goto IL_27F;
						}
						if (num == 1)
						{
							return;
						}
					}
					string text2 = password ?? (this._Password ?? this._container.Password);
					if (this._Encryption_FromZipFile != EncryptionAlgorithm.None)
					{
						if (text2 == null)
						{
							throw new BadPasswordException();
						}
						this.SetupCryptoForExtract(text2);
					}
					if (text != null)
					{
						this.WriteStatus("extract file {0}...", new object[]
						{
							text
						});
						text += ".tmp";
						string directoryName = Path.GetDirectoryName(text);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						else if (this._container.ZipFile != null)
						{
							flag2 = this._container.ZipFile._inExtractAll;
						}
						stream = new FileStream(text, FileMode.CreateNew);
					}
					else
					{
						this.WriteStatus("extract entry {0} to stream...", new object[]
						{
							this.FileName
						});
						stream = outstream;
					}
					if (!this._ioOperationCanceled)
					{
						int actualCrc = this.ExtractOne(stream);
						if (!this._ioOperationCanceled)
						{
							this.VerifyCrcAfterExtract(actualCrc);
							if (text != null)
							{
								stream.Close();
								stream = null;
								string text3 = text;
								string text4 = null;
								text = text3.Substring(0, text3.Length - 4);
								if (flag)
								{
									text4 = text + ".PendingOverwrite";
									File.Move(text, text4);
								}
								File.Move(text3, text);
								this._SetTimes(text, true);
								if (text4 != null && File.Exists(text4))
								{
									ZipEntry.ReallyDelete(text4);
								}
								if (flag2 && this.FileName.IndexOf('/') != -1)
								{
									string directoryName2 = Path.GetDirectoryName(this.FileName);
									if (this._container.ZipFile[directoryName2] == null)
									{
										this._SetTimes(Path.GetDirectoryName(text), false);
									}
								}
								if (((int)this._VersionMadeBy & 65280) == 2560 || ((int)this._VersionMadeBy & 65280) == 0)
								{
									File.SetAttributes(text, (FileAttributes)this._ExternalFileAttrs);
								}
							}
							this.OnAfterExtract(baseDir);
						}
					}
					IL_27F:;
				}
			}
			catch (Exception)
			{
				this._ioOperationCanceled = true;
				throw;
			}
			finally
			{
				if (this._ioOperationCanceled && text != null)
				{
					if (stream != null)
					{
						stream.Close();
					}
					if (File.Exists(text) && !flag)
					{
						File.Delete(text);
					}
				}
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000F734 File Offset: 0x0000D934
		internal void VerifyCrcAfterExtract(int actualCrc32)
		{
			if (actualCrc32 != this._Crc32)
			{
				throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " + string.Format("Expected 0x{0:X8}, Actual 0x{1:X8}", this._Crc32, actualCrc32));
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000F76C File Offset: 0x0000D96C
		private int CheckExtractExistingFile(string baseDir, string targetFileName)
		{
			int num = 0;
			for (;;)
			{
				switch (this.ExtractExistingFile)
				{
				case ExtractExistingFileAction.OverwriteSilently:
					goto IL_21;
				case ExtractExistingFileAction.DoNotOverwrite:
					goto IL_38;
				case ExtractExistingFileAction.InvokeExtractProgressEvent:
					if (num > 0)
					{
						goto Block_2;
					}
					this.OnExtractExisting(baseDir);
					if (this._ioOperationCanceled)
					{
						return 2;
					}
					num++;
					continue;
				}
				break;
			}
			goto IL_81;
			IL_21:
			this.WriteStatus("the file {0} exists; will overwrite it...", new object[]
			{
				targetFileName
			});
			return 0;
			IL_38:
			this.WriteStatus("the file {0} exists; not extracting entry...", new object[]
			{
				this.FileName
			});
			this.OnAfterExtract(baseDir);
			return 1;
			Block_2:
			throw new ZipException(string.Format("The file {0} already exists.", targetFileName));
			IL_81:
			throw new ZipException(string.Format("The file {0} already exists.", targetFileName));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000F813 File Offset: 0x0000DA13
		private void _CheckRead(int nbytes)
		{
			if (nbytes == 0)
			{
				throw new BadReadException(string.Format("bad read of entry {0} from compressed archive.", this.FileName));
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000F830 File Offset: 0x0000DA30
		private int ExtractOne(Stream output)
		{
			int result = 0;
			Stream archiveStream = this.ArchiveStream;
			try
			{
				archiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
				byte[] array = new byte[this.BufferSize];
				long num = (this._CompressionMethod_FromZipFile != 0) ? this.UncompressedSize : this._CompressedFileDataSize;
				this._inputDecryptorStream = this.GetExtractDecryptor(archiveStream);
				Stream extractDecompressor = this.GetExtractDecompressor(this._inputDecryptorStream);
				long num2 = 0L;
				using (CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(extractDecompressor))
				{
					while (num > 0L)
					{
						int count = (num > (long)array.Length) ? array.Length : ((int)num);
						int num3 = crcCalculatorStream.Read(array, 0, count);
						this._CheckRead(num3);
						output.Write(array, 0, num3);
						num -= (long)num3;
						num2 += (long)num3;
						this.OnExtractProgress(num2, this.UncompressedSize);
						if (this._ioOperationCanceled)
						{
							break;
						}
					}
					result = crcCalculatorStream.Crc;
				}
			}
			finally
			{
				ZipSegmentedStream zipSegmentedStream = archiveStream as ZipSegmentedStream;
				if (zipSegmentedStream != null)
				{
					zipSegmentedStream.Dispose();
					this._archiveStream = null;
				}
			}
			return result;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F944 File Offset: 0x0000DB44
		internal Stream GetExtractDecompressor(Stream input2)
		{
			short compressionMethod_FromZipFile = this._CompressionMethod_FromZipFile;
			if (compressionMethod_FromZipFile == 0)
			{
				return input2;
			}
			if (compressionMethod_FromZipFile != 8)
			{
				return null;
			}
			return new DeflateStream(input2, CompressionMode.Decompress, true);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F970 File Offset: 0x0000DB70
		internal Stream GetExtractDecryptor(Stream input)
		{
			Stream result;
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.PkzipWeak)
			{
				result = new ZipCipherStream(input, this._zipCrypto_forExtract, CryptoMode.Decrypt);
			}
			else
			{
				result = input;
			}
			return result;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F99C File Offset: 0x0000DB9C
		internal void _SetTimes(string fileOrDirectory, bool isFile)
		{
			try
			{
				if (this._ntfsTimesAreSet)
				{
					if (isFile)
					{
						if (File.Exists(fileOrDirectory))
						{
							File.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
							File.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
							File.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
						}
					}
					else if (Directory.Exists(fileOrDirectory))
					{
						Directory.SetCreationTimeUtc(fileOrDirectory, this._Ctime);
						Directory.SetLastAccessTimeUtc(fileOrDirectory, this._Atime);
						Directory.SetLastWriteTimeUtc(fileOrDirectory, this._Mtime);
					}
				}
				else
				{
					DateTime lastWriteTime = SharedUtilities.AdjustTime_Reverse(this.LastModified);
					if (isFile)
					{
						File.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
					else
					{
						Directory.SetLastWriteTime(fileOrDirectory, lastWriteTime);
					}
				}
			}
			catch (IOException ex)
			{
				this.WriteStatus("failed to set time on {0}: {1}", new object[]
				{
					fileOrDirectory,
					ex.Message
				});
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000FA64 File Offset: 0x0000DC64
		private string UnsupportedAlgorithm
		{
			get
			{
				string empty = string.Empty;
				uint unsupportedAlgorithmId = this._UnsupportedAlgorithmId;
				if (unsupportedAlgorithmId <= 26128U)
				{
					if (unsupportedAlgorithmId <= 26115U)
					{
						if (unsupportedAlgorithmId == 0U)
						{
							return "--";
						}
						switch (unsupportedAlgorithmId)
						{
						case 26113U:
							return "DES";
						case 26114U:
							return "RC2";
						case 26115U:
							return "3DES-168";
						}
					}
					else
					{
						if (unsupportedAlgorithmId == 26121U)
						{
							return "3DES-112";
						}
						switch (unsupportedAlgorithmId)
						{
						case 26126U:
							return "PKWare AES128";
						case 26127U:
							return "PKWare AES192";
						case 26128U:
							return "PKWare AES256";
						}
					}
				}
				else if (unsupportedAlgorithmId <= 26400U)
				{
					if (unsupportedAlgorithmId == 26370U)
					{
						return "RC2";
					}
					if (unsupportedAlgorithmId == 26400U)
					{
						return "Blowfish";
					}
				}
				else
				{
					if (unsupportedAlgorithmId == 26401U)
					{
						return "Twofish";
					}
					if (unsupportedAlgorithmId == 26625U)
					{
						return "RC4";
					}
					if (unsupportedAlgorithmId != 65535U)
					{
					}
				}
				return string.Format("Unknown (0x{0:X4})", this._UnsupportedAlgorithmId);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000FB80 File Offset: 0x0000DD80
		private string UnsupportedCompressionMethod
		{
			get
			{
				string empty = string.Empty;
				int compressionMethod = (int)this._CompressionMethod;
				if (compressionMethod <= 1)
				{
					if (compressionMethod == 0)
					{
						return "Store";
					}
					if (compressionMethod == 1)
					{
						return "Shrink";
					}
				}
				else
				{
					switch (compressionMethod)
					{
					case 8:
						return "DEFLATE";
					case 9:
						return "Deflate64";
					case 10:
					case 11:
					case 13:
						break;
					case 12:
						return "BZIP2";
					case 14:
						return "LZMA";
					default:
						if (compressionMethod == 19)
						{
							return "LZ77";
						}
						if (compressionMethod == 98)
						{
							return "PPMd";
						}
						break;
					}
				}
				return string.Format("Unknown (0x{0:X4})", this._CompressionMethod);
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000FC30 File Offset: 0x0000DE30
		internal void ValidateEncryption()
		{
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak || this.Encryption == EncryptionAlgorithm.None)
			{
				return;
			}
			if (this._UnsupportedAlgorithmId != 0U)
			{
				throw new ZipException(string.Format("Cannot extract: Entry {0} is encrypted with an algorithm not supported by DotNetZip: {1}", this.FileName, this.UnsupportedAlgorithm));
			}
			throw new ZipException(string.Format("Cannot extract: Entry {0} uses an unsupported encryption algorithm ({1:X2})", this.FileName, (int)this.Encryption));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000FC93 File Offset: 0x0000DE93
		private void ValidateCompression()
		{
			if (this._CompressionMethod_FromZipFile != 0 && this._CompressionMethod_FromZipFile != 8)
			{
				throw new ZipException(string.Format("Entry {0} uses an unsupported compression method (0x{1:X2}, {2})", this.FileName, this._CompressionMethod_FromZipFile, this.UnsupportedCompressionMethod));
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000FCD0 File Offset: 0x0000DED0
		private void SetupCryptoForExtract(string password)
		{
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.None)
			{
				return;
			}
			if (this._Encryption_FromZipFile == EncryptionAlgorithm.PkzipWeak)
			{
				if (password == null)
				{
					throw new ZipException("Missing password.");
				}
				this.ArchiveStream.Seek(this.FileDataPosition - 12L, SeekOrigin.Begin);
				this._zipCrypto_forExtract = ZipCrypto.ForRead(password, this);
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000FD24 File Offset: 0x0000DF24
		private bool ValidateOutput(string basedir, Stream outstream, out string outFileName)
		{
			if (basedir != null)
			{
				string text = this.FileName.Replace("\\", "/");
				if (text.IndexOf(':') == 1)
				{
					text = text.Substring(2);
				}
				if (text.StartsWith("/"))
				{
					text = text.Substring(1);
				}
				if (this._container.ZipFile.FlattenFoldersOnExtract)
				{
					outFileName = Path.Combine(basedir, (text.IndexOf('/') != -1) ? Path.GetFileName(text) : text);
				}
				else
				{
					outFileName = Path.Combine(basedir, text);
				}
				outFileName = outFileName.Replace("/", "\\");
				if (this.IsDirectory || this.FileName.EndsWith("/"))
				{
					if (!Directory.Exists(outFileName))
					{
						Directory.CreateDirectory(outFileName);
						this._SetTimes(outFileName, false);
					}
					else if (this.ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently)
					{
						this._SetTimes(outFileName, false);
					}
					return true;
				}
				return false;
			}
			else
			{
				if (outstream != null)
				{
					outFileName = null;
					return this.IsDirectory || this.FileName.EndsWith("/");
				}
				throw new ArgumentNullException("outstream");
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000FE3C File Offset: 0x0000E03C
		private void ReadExtraField()
		{
			this._readExtraDepth++;
			long position = this.ArchiveStream.Position;
			this.ArchiveStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			byte[] array = new byte[30];
			this.ArchiveStream.Read(array, 0, array.Length);
			int num = 26;
			short num2 = (short)((int)array[num++] + (int)array[num++] * 256);
			short extraFieldLength = (short)((int)array[num++] + (int)array[num++] * 256);
			this.ArchiveStream.Seek((long)num2, SeekOrigin.Current);
			this.ProcessExtraField(this.ArchiveStream, extraFieldLength);
			this.ArchiveStream.Seek(position, SeekOrigin.Begin);
			this._readExtraDepth--;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000FEFC File Offset: 0x0000E0FC
		private static bool ReadHeader(ZipEntry ze, Encoding defaultEncoding)
		{
			int num = 0;
			ze._RelativeOffsetOfLocalHeader = ze.ArchiveStream.Position;
			int num2 = SharedUtilities.ReadEntrySignature(ze.ArchiveStream);
			num += 4;
			if (ZipEntry.IsNotValidSig(num2))
			{
				ze.ArchiveStream.Seek(-4L, SeekOrigin.Current);
				if (ZipEntry.IsNotValidZipDirEntrySig(num2) && (long)num2 != 101010256L)
				{
					throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) at position  0x{1:X8}", num2, ze.ArchiveStream.Position));
				}
				return false;
			}
			else
			{
				byte[] array = new byte[26];
				int num3 = ze.ArchiveStream.Read(array, 0, array.Length);
				if (num3 != array.Length)
				{
					return false;
				}
				num += num3;
				int num4 = 0;
				ze._VersionNeeded = (short)((int)array[num4++] + (int)array[num4++] * 256);
				ze._BitField = (short)((int)array[num4++] + (int)array[num4++] * 256);
				ze._CompressionMethod_FromZipFile = (ze._CompressionMethod = (short)((int)array[num4++] + (int)array[num4++] * 256));
				ze._TimeBlob = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				ze._LastModified = SharedUtilities.PackedToDateTime(ze._TimeBlob);
				ze._timestamp |= ZipEntryTimestamp.DOS;
				if ((ze._BitField & 1) == 1)
				{
					ze._Encryption_FromZipFile = (ze._Encryption = EncryptionAlgorithm.PkzipWeak);
					ze._sourceIsEncrypted = true;
				}
				ze._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
				ze._CompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				ze._UncompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
				if ((uint)ze._CompressedSize == 4294967295U || (uint)ze._UncompressedSize == 4294967295U)
				{
					ze._InputUsesZip64 = true;
				}
				int num5 = (int)((short)((int)array[num4++] + (int)array[num4++] * 256));
				short extraFieldLength = (short)((int)array[num4++] + (int)array[num4++] * 256);
				array = new byte[num5];
				num3 = ze.ArchiveStream.Read(array, 0, array.Length);
				num += num3;
				if ((ze._BitField & 2048) == 2048)
				{
					ze.AlternateEncoding = Encoding.UTF8;
					ze.AlternateEncodingUsage = ZipOption.Always;
				}
				ze._FileNameInArchive = ze.AlternateEncoding.GetString(array, 0, array.Length);
				if (ze._FileNameInArchive.EndsWith("/"))
				{
					ze.MarkAsDirectory();
				}
				num += ze.ProcessExtraField(ze.ArchiveStream, extraFieldLength);
				ze._LengthOfTrailer = 0;
				if (!ze._FileNameInArchive.EndsWith("/") && (ze._BitField & 8) == 8)
				{
					long position = ze.ArchiveStream.Position;
					bool flag = true;
					long num6 = 0L;
					int num7 = 0;
					while (flag)
					{
						num7++;
						if (ze._container.ZipFile != null)
						{
							ze._container.ZipFile.OnReadBytes(ze);
						}
						long num8 = SharedUtilities.FindSignature(ze.ArchiveStream, 134695760);
						if (num8 == -1L)
						{
							return false;
						}
						num6 += num8;
						if (ze._InputUsesZip64)
						{
							array = new byte[20];
							num3 = ze.ArchiveStream.Read(array, 0, array.Length);
							if (num3 != 20)
							{
								return false;
							}
							num4 = 0;
							ze._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
							ze._CompressedSize = BitConverter.ToInt64(array, num4);
							num4 += 8;
							ze._UncompressedSize = BitConverter.ToInt64(array, num4);
							num4 += 8;
							ze._LengthOfTrailer += 24;
						}
						else
						{
							array = new byte[12];
							num3 = ze.ArchiveStream.Read(array, 0, array.Length);
							if (num3 != 12)
							{
								return false;
							}
							num4 = 0;
							ze._Crc32 = (int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256;
							ze._CompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
							ze._UncompressedSize = (long)((ulong)((int)array[num4++] + (int)array[num4++] * 256 + (int)array[num4++] * 256 * 256 + (int)array[num4++] * 256 * 256 * 256));
							ze._LengthOfTrailer += 16;
						}
						flag = (num6 != ze._CompressedSize);
						if (flag)
						{
							ze.ArchiveStream.Seek(-12L, SeekOrigin.Current);
							num6 += 4L;
						}
					}
					ze.ArchiveStream.Seek(position, SeekOrigin.Begin);
				}
				ze._CompressedFileDataSize = ze._CompressedSize;
				if ((ze._BitField & 1) == 1)
				{
					ze._WeakEncryptionHeader = new byte[12];
					num += ZipEntry.ReadWeakEncryptionHeader(ze._archiveStream, ze._WeakEncryptionHeader);
					ze._CompressedFileDataSize -= 12L;
				}
				ze._LengthOfHeader = num;
				ze._TotalEntrySize = (long)ze._LengthOfHeader + ze._CompressedFileDataSize + (long)ze._LengthOfTrailer;
				return true;
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00010583 File Offset: 0x0000E783
		internal static int ReadWeakEncryptionHeader(Stream s, byte[] buffer)
		{
			int num = s.Read(buffer, 0, 12);
			if (num != 12)
			{
				throw new ZipException(string.Format("Unexpected end of data at position 0x{0:X8}", s.Position));
			}
			return num;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000105AF File Offset: 0x0000E7AF
		private static bool IsNotValidSig(int signature)
		{
			return signature != 67324752;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000105BC File Offset: 0x0000E7BC
		internal static ZipEntry ReadEntry(ZipContainer zc, bool first)
		{
			ZipFile zipFile = zc.ZipFile;
			Stream readStream = zc.ReadStream;
			Encoding alternateEncoding = zc.AlternateEncoding;
			ZipEntry zipEntry = new ZipEntry();
			zipEntry._Source = ZipEntrySource.ZipFile;
			zipEntry._container = zc;
			zipEntry._archiveStream = readStream;
			if (zipFile != null)
			{
				zipFile.OnReadEntry(true, null);
			}
			if (first)
			{
				ZipEntry.HandlePK00Prefix(readStream);
			}
			if (!ZipEntry.ReadHeader(zipEntry, alternateEncoding))
			{
				return null;
			}
			zipEntry.__FileDataPosition = zipEntry.ArchiveStream.Position;
			readStream.Seek(zipEntry._CompressedFileDataSize + (long)zipEntry._LengthOfTrailer, SeekOrigin.Current);
			ZipEntry.HandleUnexpectedDataDescriptor(zipEntry);
			if (zipFile != null)
			{
				zipFile.OnReadBytes(zipEntry);
				zipFile.OnReadEntry(false, zipEntry);
			}
			return zipEntry;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00010658 File Offset: 0x0000E858
		internal static void HandlePK00Prefix(Stream s)
		{
			if (SharedUtilities.ReadInt(s) != 808471376)
			{
				s.Seek(-4L, SeekOrigin.Current);
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00010674 File Offset: 0x0000E874
		private static void HandleUnexpectedDataDescriptor(ZipEntry entry)
		{
			Stream archiveStream = entry.ArchiveStream;
			if ((ulong)SharedUtilities.ReadInt(archiveStream) == (ulong)((long)entry._Crc32))
			{
				if ((long)SharedUtilities.ReadInt(archiveStream) != entry._CompressedSize)
				{
					archiveStream.Seek(-8L, SeekOrigin.Current);
					return;
				}
				if ((long)SharedUtilities.ReadInt(archiveStream) != entry._UncompressedSize)
				{
					archiveStream.Seek(-12L, SeekOrigin.Current);
					return;
				}
			}
			else
			{
				archiveStream.Seek(-4L, SeekOrigin.Current);
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000106DC File Offset: 0x0000E8DC
		internal static int FindExtraFieldSegment(byte[] extra, int offx, ushort targetHeaderId)
		{
			int num = offx;
			while (num + 3 < extra.Length)
			{
				if ((ushort)((int)extra[num++] + (int)extra[num++] * 256) == targetHeaderId)
				{
					return num - 2;
				}
				short num2 = (short)((int)extra[num++] + (int)extra[num++] * 256);
				num += (int)num2;
			}
			return -1;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00010730 File Offset: 0x0000E930
		internal int ProcessExtraField(Stream s, short extraFieldLength)
		{
			int num = 0;
			if (extraFieldLength > 0)
			{
				byte[] array = this._Extra = new byte[(int)extraFieldLength];
				num = s.Read(array, 0, array.Length);
				long posn = s.Position - (long)num;
				int num2 = 0;
				while (num2 + 3 < array.Length)
				{
					int num3 = num2;
					ushort num4 = (ushort)((int)array[num2++] + (int)array[num2++] * 256);
					short num5 = (short)((int)array[num2++] + (int)array[num2++] * 256);
					if (num4 <= 23)
					{
						if (num4 != 1)
						{
							if (num4 != 10)
							{
								if (num4 == 23)
								{
									num2 = this.ProcessExtraFieldPkwareStrongEncryption(array, num2);
								}
							}
							else
							{
								num2 = this.ProcessExtraFieldWindowsTimes(array, num2, num5, posn);
							}
						}
						else
						{
							num2 = this.ProcessExtraFieldZip64(array, num2, num5, posn);
						}
					}
					else if (num4 <= 22613)
					{
						if (num4 != 21589)
						{
							if (num4 == 22613)
							{
								num2 = this.ProcessExtraFieldInfoZipTimes(array, num2, num5, posn);
							}
						}
						else
						{
							num2 = this.ProcessExtraFieldUnixTimes(array, num2, num5, posn);
						}
					}
					else if (num4 != 30805 && num4 != 30837)
					{
					}
					num2 = num3 + (int)num5 + 4;
				}
			}
			return num;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00010848 File Offset: 0x0000EA48
		private int ProcessExtraFieldPkwareStrongEncryption(byte[] Buffer, int j)
		{
			j += 2;
			this._UnsupportedAlgorithmId = (uint)((ushort)((int)Buffer[j++] + (int)Buffer[j++] * 256));
			this._Encryption_FromZipFile = (this._Encryption = EncryptionAlgorithm.Unsupported);
			return j;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0001088C File Offset: 0x0000EA8C
		private int ProcessExtraFieldZip64(byte[] buffer, int j, short dataSize, long posn)
		{
			this._InputUsesZip64 = true;
			if (dataSize > 28)
			{
				throw new BadReadException(string.Format("  Inconsistent size (0x{0:X4}) for ZIP64 extra field at position 0x{1:X16}", dataSize, posn));
			}
			int remainingData = (int)dataSize;
			ZipEntry.Func<long> func = delegate()
			{
				if (remainingData < 8)
				{
					throw new BadReadException(string.Format("  Missing data for ZIP64 extra field, position 0x{0:X16}", posn));
				}
				long result = BitConverter.ToInt64(buffer, j);
				j += 8;
				remainingData -= 8;
				return result;
			};
			if (this._UncompressedSize == (long)((ulong)-1))
			{
				this._UncompressedSize = func();
			}
			if (this._CompressedSize == (long)((ulong)-1))
			{
				this._CompressedSize = func();
			}
			if (this._RelativeOffsetOfLocalHeader == (long)((ulong)-1))
			{
				this._RelativeOffsetOfLocalHeader = func();
			}
			return j;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00010940 File Offset: 0x0000EB40
		private int ProcessExtraFieldInfoZipTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 12 && dataSize != 8)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for InfoZip v1 extra field at position 0x{1:X16}", dataSize, posn));
			}
			int num = BitConverter.ToInt32(buffer, j);
			this._Mtime = ZipEntry._unixEpoch.AddSeconds((double)num);
			j += 4;
			num = BitConverter.ToInt32(buffer, j);
			this._Atime = ZipEntry._unixEpoch.AddSeconds((double)num);
			j += 4;
			this._Ctime = DateTime.UtcNow;
			this._ntfsTimesAreSet = true;
			this._timestamp |= ZipEntryTimestamp.InfoZip1;
			return j;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000109D4 File Offset: 0x0000EBD4
		private int ProcessExtraFieldUnixTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 13 && dataSize != 9 && dataSize != 5)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for Extended Timestamp extra field at position 0x{1:X16}", dataSize, posn));
			}
			int remainingData = (int)dataSize;
			ZipEntry.Func<DateTime> func = delegate()
			{
				int num2 = BitConverter.ToInt32(buffer, j);
				j += 4;
				remainingData -= 4;
				return ZipEntry._unixEpoch.AddSeconds((double)num2);
			};
			if (dataSize == 13 || this._readExtraDepth > 0)
			{
				byte[] buffer2 = buffer;
				int num = j;
				j = num + 1;
				byte b = buffer2[num];
				num = remainingData;
				remainingData = num - 1;
				if ((b & 1) != 0 && remainingData >= 4)
				{
					this._Mtime = func();
				}
				this._Atime = (((b & 2) != 0 && remainingData >= 4) ? func() : DateTime.UtcNow);
				this._Ctime = (((b & 4) != 0 && remainingData >= 4) ? func() : DateTime.UtcNow);
				this._timestamp |= ZipEntryTimestamp.Unix;
				this._ntfsTimesAreSet = true;
				this._emitUnixTimes = true;
			}
			else
			{
				this.ReadExtraField();
			}
			return j;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00010AF4 File Offset: 0x0000ECF4
		private int ProcessExtraFieldWindowsTimes(byte[] buffer, int j, short dataSize, long posn)
		{
			if (dataSize != 32)
			{
				throw new BadReadException(string.Format("  Unexpected size (0x{0:X4}) for NTFS times extra field at position 0x{1:X16}", dataSize, posn));
			}
			j += 4;
			int num = (int)((short)((int)buffer[j] + (int)buffer[j + 1] * 256));
			short num2 = (short)((int)buffer[j + 2] + (int)buffer[j + 3] * 256);
			j += 4;
			if (num == 1 && num2 == 24)
			{
				long fileTime = BitConverter.ToInt64(buffer, j);
				this._Mtime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				this._Atime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				fileTime = BitConverter.ToInt64(buffer, j);
				this._Ctime = DateTime.FromFileTimeUtc(fileTime);
				j += 8;
				this._ntfsTimesAreSet = true;
				this._timestamp |= ZipEntryTimestamp.Windows;
				this._emitNtfsTimes = true;
			}
			return j;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00010BC0 File Offset: 0x0000EDC0
		internal void WriteCentralDirectoryEntry(Stream s)
		{
			byte[] array = new byte[4096];
			int num = 0;
			array[num++] = 80;
			array[num++] = 75;
			array[num++] = 1;
			array[num++] = 2;
			array[num++] = (byte)(this._VersionMadeBy & 255);
			array[num++] = (byte)(((int)this._VersionMadeBy & 65280) >> 8);
			short num2 = (this.VersionNeeded != 0) ? this.VersionNeeded : 20;
			if (this._OutputUsesZip64 == null)
			{
				this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always);
			}
			short num3 = this._OutputUsesZip64.Value ? 45 : num2;
			array[num++] = (byte)(num3 & 255);
			array[num++] = (byte)(((int)num3 & 65280) >> 8);
			array[num++] = (byte)(this._BitField & 255);
			array[num++] = (byte)(((int)this._BitField & 65280) >> 8);
			array[num++] = (byte)(this._CompressionMethod & 255);
			array[num++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			array[num++] = (byte)(this._TimeBlob & 255);
			array[num++] = (byte)((this._TimeBlob & 65280) >> 8);
			array[num++] = (byte)((this._TimeBlob & 16711680) >> 16);
			array[num++] = (byte)(((long)this._TimeBlob & (long)((ulong)-16777216)) >> 24);
			array[num++] = (byte)(this._Crc32 & 255);
			array[num++] = (byte)((this._Crc32 & 65280) >> 8);
			array[num++] = (byte)((this._Crc32 & 16711680) >> 16);
			array[num++] = (byte)(((long)this._Crc32 & (long)((ulong)-16777216)) >> 24);
			if (this._OutputUsesZip64.Value)
			{
				for (int i = 0; i < 8; i++)
				{
					array[num++] = byte.MaxValue;
				}
			}
			else
			{
				array[num++] = (byte)(this._CompressedSize & 255L);
				array[num++] = (byte)((this._CompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
				array[num++] = (byte)(this._UncompressedSize & 255L);
				array[num++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
			}
			byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
			short num4 = (short)encodedFileNameBytes.Length;
			array[num++] = (byte)(num4 & 255);
			array[num++] = (byte)(((int)num4 & 65280) >> 8);
			this._presumeZip64 = this._OutputUsesZip64.Value;
			this._Extra = this.ConstructExtraField(true);
			short num5 = (short)((this._Extra == null) ? 0 : this._Extra.Length);
			array[num++] = (byte)(num5 & 255);
			array[num++] = (byte)(((int)num5 & 65280) >> 8);
			int num6 = (this._CommentBytes == null) ? 0 : this._CommentBytes.Length;
			if (num6 + num > array.Length)
			{
				num6 = array.Length - num;
			}
			array[num++] = (byte)(num6 & 255);
			array[num++] = (byte)((num6 & 65280) >> 8);
			if (this._container.ZipFile != null && this._container.ZipFile.MaxOutputSegmentSize != 0)
			{
				array[num++] = (byte)(this._diskNumber & 255U);
				array[num++] = (byte)((this._diskNumber & 65280U) >> 8);
			}
			else
			{
				array[num++] = 0;
				array[num++] = 0;
			}
			array[num++] = (this._IsText ? 1 : 0);
			array[num++] = 0;
			array[num++] = (byte)(this._ExternalFileAttrs & 255);
			array[num++] = (byte)((this._ExternalFileAttrs & 65280) >> 8);
			array[num++] = (byte)((this._ExternalFileAttrs & 16711680) >> 16);
			array[num++] = (byte)(((long)this._ExternalFileAttrs & (long)((ulong)-16777216)) >> 24);
			if (this._RelativeOffsetOfLocalHeader > (long)((ulong)-1))
			{
				array[num++] = byte.MaxValue;
				array[num++] = byte.MaxValue;
				array[num++] = byte.MaxValue;
				array[num++] = byte.MaxValue;
			}
			else
			{
				array[num++] = (byte)(this._RelativeOffsetOfLocalHeader & 255L);
				array[num++] = (byte)((this._RelativeOffsetOfLocalHeader & 65280L) >> 8);
				array[num++] = (byte)((this._RelativeOffsetOfLocalHeader & 16711680L) >> 16);
				array[num++] = (byte)((this._RelativeOffsetOfLocalHeader & (long)((ulong)-16777216)) >> 24);
			}
			Buffer.BlockCopy(encodedFileNameBytes, 0, array, num, (int)num4);
			num += (int)num4;
			if (this._Extra != null)
			{
				Array extra = this._Extra;
				int srcOffset = 0;
				Buffer.BlockCopy(extra, srcOffset, array, num, (int)num5);
				num += (int)num5;
			}
			if (num6 != 0)
			{
				Buffer.BlockCopy(this._CommentBytes, 0, array, num, num6);
				num += num6;
			}
			s.Write(array, 0, num);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00011114 File Offset: 0x0000F314
		private byte[] ConstructExtraField(bool forCentralDirectory)
		{
			List<byte[]> list = new List<byte[]>();
			if (this._container.Zip64 == Zip64Option.Always || (this._container.Zip64 == Zip64Option.AsNecessary && (!forCentralDirectory || this._entryRequiresZip64.Value)))
			{
				int num = 4 + (forCentralDirectory ? 28 : 16);
				byte[] array = new byte[num];
				int num2 = 0;
				if (this._presumeZip64 || forCentralDirectory)
				{
					array[num2++] = 1;
					array[num2++] = 0;
				}
				else
				{
					array[num2++] = 153;
					array[num2++] = 153;
				}
				array[num2++] = (byte)(num - 4);
				array[num2++] = 0;
				Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, array, num2, 8);
				num2 += 8;
				Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, array, num2, 8);
				num2 += 8;
				if (forCentralDirectory)
				{
					Array.Copy(BitConverter.GetBytes(this._RelativeOffsetOfLocalHeader), 0, array, num2, 8);
					num2 += 8;
					Array.Copy(BitConverter.GetBytes(0), 0, array, num2, 4);
				}
				list.Add(array);
			}
			if (this._ntfsTimesAreSet && this._emitNtfsTimes)
			{
				byte[] array = new byte[36];
				int num3 = 0;
				array[num3++] = 10;
				array[num3++] = 0;
				array[num3++] = 32;
				array[num3++] = 0;
				num3 += 4;
				array[num3++] = 1;
				array[num3++] = 0;
				array[num3++] = 24;
				array[num3++] = 0;
				Array.Copy(BitConverter.GetBytes(this._Mtime.ToFileTime()), 0, array, num3, 8);
				num3 += 8;
				Array.Copy(BitConverter.GetBytes(this._Atime.ToFileTime()), 0, array, num3, 8);
				num3 += 8;
				Array.Copy(BitConverter.GetBytes(this._Ctime.ToFileTime()), 0, array, num3, 8);
				num3 += 8;
				list.Add(array);
			}
			if (this._ntfsTimesAreSet && this._emitUnixTimes)
			{
				int num4 = 9;
				if (!forCentralDirectory)
				{
					num4 += 8;
				}
				byte[] array = new byte[num4];
				int num5 = 0;
				array[num5++] = 85;
				array[num5++] = 84;
				array[num5++] = (byte)(num4 - 4);
				array[num5++] = 0;
				array[num5++] = 7;
				Array.Copy(BitConverter.GetBytes((int)(this._Mtime - ZipEntry._unixEpoch).TotalSeconds), 0, array, num5, 4);
				num5 += 4;
				if (!forCentralDirectory)
				{
					Array.Copy(BitConverter.GetBytes((int)(this._Atime - ZipEntry._unixEpoch).TotalSeconds), 0, array, num5, 4);
					num5 += 4;
					Array.Copy(BitConverter.GetBytes((int)(this._Ctime - ZipEntry._unixEpoch).TotalSeconds), 0, array, num5, 4);
					num5 += 4;
				}
				list.Add(array);
			}
			byte[] array2 = null;
			if (list.Count > 0)
			{
				int num6 = 0;
				int num7 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num6 += list[i].Length;
				}
				array2 = new byte[num6];
				for (int i = 0; i < list.Count; i++)
				{
					Array.Copy(list[i], 0, array2, num7, list[i].Length);
					num7 += list[i].Length;
				}
			}
			return array2;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00011488 File Offset: 0x0000F688
		private string NormalizeFileName()
		{
			string text = this.FileName.Replace("\\", "/");
			string result;
			if (this._TrimVolumeFromFullyQualifiedPaths && this.FileName.Length >= 3 && this.FileName[1] == ':' && text[2] == '/')
			{
				result = text.Substring(3);
			}
			else if (this.FileName.Length >= 4 && text[0] == '/' && text[1] == '/')
			{
				int num = text.IndexOf('/', 2);
				if (num == -1)
				{
					throw new ArgumentException("The path for that entry appears to be badly formatted");
				}
				result = text.Substring(num + 1);
			}
			else if (this.FileName.Length >= 3 && text[0] == '.' && text[1] == '/')
			{
				result = text.Substring(2);
			}
			else
			{
				result = text;
			}
			return result;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00011564 File Offset: 0x0000F764
		private byte[] GetEncodedFileNameBytes()
		{
			string text = this.NormalizeFileName();
			ZipOption alternateEncodingUsage = this.AlternateEncodingUsage;
			if (alternateEncodingUsage == ZipOption.Default)
			{
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = ZipEntry.utf8.GetBytes(this._Comment);
				}
				this._actualEncoding = ZipEntry.utf8;
				return ZipEntry.utf8.GetBytes(text);
			}
			if (alternateEncodingUsage == ZipOption.Always)
			{
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				}
				this._actualEncoding = this.AlternateEncoding;
				return this.AlternateEncoding.GetBytes(text);
			}
			byte[] bytes = ZipEntry.utf8.GetBytes(text);
			string @string = ZipEntry.utf8.GetString(bytes, 0, bytes.Length);
			this._CommentBytes = null;
			if (@string != text)
			{
				bytes = this.AlternateEncoding.GetBytes(text);
				if (this._Comment != null && this._Comment.Length != 0)
				{
					this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				}
				this._actualEncoding = this.AlternateEncoding;
				return bytes;
			}
			this._actualEncoding = ZipEntry.utf8;
			if (this._Comment == null || this._Comment.Length == 0)
			{
				return bytes;
			}
			byte[] bytes2 = ZipEntry.utf8.GetBytes(this._Comment);
			if (ZipEntry.utf8.GetString(bytes2, 0, bytes2.Length) != this.Comment)
			{
				bytes = this.AlternateEncoding.GetBytes(text);
				this._CommentBytes = this.AlternateEncoding.GetBytes(this._Comment);
				this._actualEncoding = this.AlternateEncoding;
				return bytes;
			}
			this._CommentBytes = bytes2;
			return bytes;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0001170C File Offset: 0x0000F90C
		private bool WantReadAgain()
		{
			return this._UncompressedSize >= 16L && this._CompressionMethod != 0 && this.CompressionLevel != CompressionLevel.None && this._CompressedSize >= this._UncompressedSize && (this._Source != ZipEntrySource.Stream || this._sourceStream.CanSeek) && (this._zipCrypto_forWrite == null || this.CompressedSize - 12L > this.UncompressedSize);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00011780 File Offset: 0x0000F980
		private void MaybeUnsetCompressionMethodForWriting(int cycle)
		{
			if (cycle > 1)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this.IsDirectory)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this._Source == ZipEntrySource.ZipFile)
			{
				return;
			}
			if (this._Source == ZipEntrySource.Stream)
			{
				if (this._sourceStream != null && this._sourceStream.CanSeek && this._sourceStream.Length == 0L)
				{
					this._CompressionMethod = 0;
					return;
				}
			}
			else if (this._Source == ZipEntrySource.FileSystem && SharedUtilities.GetFileLength(this.LocalFileName) == 0L)
			{
				this._CompressionMethod = 0;
				return;
			}
			if (this.SetCompression != null)
			{
				this.CompressionLevel = this.SetCompression(this.LocalFileName, this._FileNameInArchive);
			}
			if (this.CompressionLevel == CompressionLevel.None && this.CompressionMethod == CompressionMethod.Deflate)
			{
				this._CompressionMethod = 0;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00011844 File Offset: 0x0000FA44
		internal void WriteHeader(Stream s, int cycle)
		{
			CountingStream countingStream = s as CountingStream;
			this._future_ROLH = ((countingStream != null) ? countingStream.ComputedPosition : s.Position);
			int num = 0;
			byte[] array = new byte[30];
			array[num++] = 80;
			array[num++] = 75;
			array[num++] = 3;
			array[num++] = 4;
			this._presumeZip64 = (this._container.Zip64 == Zip64Option.Always || (this._container.Zip64 == Zip64Option.AsNecessary && !s.CanSeek));
			short num2 = this._presumeZip64 ? 45 : 20;
			array[num++] = (byte)(num2 & 255);
			array[num++] = (byte)(((int)num2 & 65280) >> 8);
			byte[] encodedFileNameBytes = this.GetEncodedFileNameBytes();
			short num3 = (short)encodedFileNameBytes.Length;
			if (this._Encryption == EncryptionAlgorithm.None)
			{
				this._BitField &= -2;
			}
			else
			{
				this._BitField |= 1;
			}
			if (this._actualEncoding.CodePage == Encoding.UTF8.CodePage)
			{
				this._BitField |= 2048;
			}
			if (this.IsDirectory || cycle == 99)
			{
				this._BitField &= -9;
				this._BitField &= -2;
				this.Encryption = EncryptionAlgorithm.None;
				this.Password = null;
			}
			else if (!s.CanSeek)
			{
				this._BitField |= 8;
			}
			array[num++] = (byte)(this._BitField & 255);
			array[num++] = (byte)(((int)this._BitField & 65280) >> 8);
			if (this.__FileDataPosition == -1L)
			{
				this._CompressedSize = 0L;
				this._crcCalculated = false;
			}
			this.MaybeUnsetCompressionMethodForWriting(cycle);
			array[num++] = (byte)(this._CompressionMethod & 255);
			array[num++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			if (cycle == 99)
			{
				this.SetZip64Flags();
			}
			this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
			array[num++] = (byte)(this._TimeBlob & 255);
			array[num++] = (byte)((this._TimeBlob & 65280) >> 8);
			array[num++] = (byte)((this._TimeBlob & 16711680) >> 16);
			array[num++] = (byte)(((long)this._TimeBlob & (long)((ulong)-16777216)) >> 24);
			array[num++] = (byte)(this._Crc32 & 255);
			array[num++] = (byte)((this._Crc32 & 65280) >> 8);
			array[num++] = (byte)((this._Crc32 & 16711680) >> 16);
			array[num++] = (byte)(((long)this._Crc32 & (long)((ulong)-16777216)) >> 24);
			if (this._presumeZip64)
			{
				for (int i = 0; i < 8; i++)
				{
					array[num++] = byte.MaxValue;
				}
			}
			else
			{
				array[num++] = (byte)(this._CompressedSize & 255L);
				array[num++] = (byte)((this._CompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
				array[num++] = (byte)(this._UncompressedSize & 255L);
				array[num++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				array[num++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				array[num++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
			}
			array[num++] = (byte)(num3 & 255);
			array[num++] = (byte)(((int)num3 & 65280) >> 8);
			this._Extra = this.ConstructExtraField(false);
			short num4 = (short)((this._Extra == null) ? 0 : this._Extra.Length);
			array[num++] = (byte)(num4 & 255);
			array[num++] = (byte)(((int)num4 & 65280) >> 8);
			byte[] array2 = new byte[num + (int)num3 + (int)num4];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			Buffer.BlockCopy(encodedFileNameBytes, 0, array2, num, encodedFileNameBytes.Length);
			num += encodedFileNameBytes.Length;
			if (this._Extra != null)
			{
				Buffer.BlockCopy(this._Extra, 0, array2, num, this._Extra.Length);
				num += this._Extra.Length;
			}
			this._LengthOfHeader = num;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = true;
				uint num5 = zipSegmentedStream.ComputeSegment(num);
				if (num5 != zipSegmentedStream.CurrentSegment)
				{
					this._future_ROLH = 0L;
				}
				else
				{
					this._future_ROLH = zipSegmentedStream.Position;
				}
				this._diskNumber = num5;
			}
			if (this._container.Zip64 == Zip64Option.Default && (uint)this._RelativeOffsetOfLocalHeader >= 4294967295U)
			{
				throw new ZipException("Offset within the zip archive exceeds 0xFFFFFFFF. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			s.Write(array2, 0, num);
			if (zipSegmentedStream != null)
			{
				zipSegmentedStream.ContiguousWrite = false;
			}
			this._EntryHeader = array2;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00011D28 File Offset: 0x0000FF28
		private int FigureCrc32()
		{
			if (!this._crcCalculated)
			{
				Stream stream = null;
				if (this._Source == ZipEntrySource.WriteDelegate)
				{
					CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(Stream.Null);
					this._WriteDelegate(this.FileName, crcCalculatorStream);
					this._Crc32 = crcCalculatorStream.Crc;
				}
				else if (this._Source != ZipEntrySource.ZipFile)
				{
					if (this._Source == ZipEntrySource.Stream)
					{
						this.PrepSourceStream();
						stream = this._sourceStream;
					}
					else if (this._Source == ZipEntrySource.JitStream)
					{
						if (this._sourceStream == null)
						{
							this._sourceStream = this._OpenDelegate(this.FileName);
						}
						this.PrepSourceStream();
						stream = this._sourceStream;
					}
					else if (this._Source != ZipEntrySource.ZipOutputStream)
					{
						stream = File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					}
					CRC32 crc = new CRC32();
					this._Crc32 = crc.GetCrc32(stream);
					if (this._sourceStream == null)
					{
						stream.Dispose();
					}
				}
				this._crcCalculated = true;
			}
			return this._Crc32;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00011E1C File Offset: 0x0001001C
		private void PrepSourceStream()
		{
			if (this._sourceStream == null)
			{
				throw new ZipException(string.Format("The input stream is null for entry '{0}'.", this.FileName));
			}
			if (this._sourceStreamOriginalPosition != null)
			{
				this._sourceStream.Position = this._sourceStreamOriginalPosition.Value;
				return;
			}
			if (this._sourceStream.CanSeek)
			{
				this._sourceStreamOriginalPosition = new long?(this._sourceStream.Position);
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak && this._Source != ZipEntrySource.ZipFile && (this._BitField & 8) != 8)
			{
				throw new ZipException("It is not possible to use PKZIP encryption on a non-seekable input stream");
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00011EB8 File Offset: 0x000100B8
		internal void CopyMetaData(ZipEntry source)
		{
			this.__FileDataPosition = source.__FileDataPosition;
			this.CompressionMethod = source.CompressionMethod;
			this._CompressionMethod_FromZipFile = source._CompressionMethod_FromZipFile;
			this._CompressedFileDataSize = source._CompressedFileDataSize;
			this._UncompressedSize = source._UncompressedSize;
			this._BitField = source._BitField;
			this._Source = source._Source;
			this._LastModified = source._LastModified;
			this._Mtime = source._Mtime;
			this._Atime = source._Atime;
			this._Ctime = source._Ctime;
			this._ntfsTimesAreSet = source._ntfsTimesAreSet;
			this._emitUnixTimes = source._emitUnixTimes;
			this._emitNtfsTimes = source._emitNtfsTimes;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00011F6D File Offset: 0x0001016D
		private void OnWriteBlock(long bytesXferred, long totalBytesToXfer)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnSaveBlock(this, bytesXferred, totalBytesToXfer);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00011F98 File Offset: 0x00010198
		private void _WriteEntryData(Stream s)
		{
			Stream stream = null;
			long _FileDataPosition = -1L;
			try
			{
				_FileDataPosition = s.Position;
			}
			catch (Exception)
			{
			}
			try
			{
				long num = this.SetInputAndFigureFileLength(ref stream);
				CountingStream countingStream = new CountingStream(s);
				Stream stream2;
				Stream stream3;
				if (num != 0L)
				{
					stream2 = this.MaybeApplyEncryption(countingStream);
					stream3 = this.MaybeApplyCompression(stream2, num);
				}
				else
				{
					stream3 = (stream2 = countingStream);
				}
				CrcCalculatorStream crcCalculatorStream = new CrcCalculatorStream(stream3, true);
				if (this._Source == ZipEntrySource.WriteDelegate)
				{
					this._WriteDelegate(this.FileName, crcCalculatorStream);
				}
				else
				{
					byte[] array = new byte[this.BufferSize];
					int count;
					while ((count = SharedUtilities.ReadWithRetry(stream, array, 0, array.Length, this.FileName)) != 0)
					{
						crcCalculatorStream.Write(array, 0, count);
						this.OnWriteBlock(crcCalculatorStream.TotalBytesSlurped, num);
						if (this._ioOperationCanceled)
						{
							break;
						}
					}
				}
				this.FinishOutputStream(s, countingStream, stream2, stream3, crcCalculatorStream);
			}
			finally
			{
				if (this._Source == ZipEntrySource.JitStream)
				{
					if (this._CloseDelegate != null)
					{
						this._CloseDelegate(this.FileName, stream);
					}
				}
				else if (stream is FileStream)
				{
					stream.Dispose();
				}
			}
			if (this._ioOperationCanceled)
			{
				return;
			}
			this.__FileDataPosition = _FileDataPosition;
			this.PostProcessOutput(s);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000120D0 File Offset: 0x000102D0
		private long SetInputAndFigureFileLength(ref Stream input)
		{
			long result = -1L;
			if (this._Source == ZipEntrySource.Stream)
			{
				this.PrepSourceStream();
				input = this._sourceStream;
				try
				{
					return this._sourceStream.Length;
				}
				catch (NotSupportedException)
				{
					return result;
				}
			}
			if (this._Source == ZipEntrySource.ZipFile)
			{
				string password = (this._Encryption_FromZipFile == EncryptionAlgorithm.None) ? null : (this._Password ?? this._container.Password);
				this._sourceStream = this.InternalOpenReader(password);
				this.PrepSourceStream();
				input = this._sourceStream;
				result = this._sourceStream.Length;
			}
			else
			{
				if (this._Source == ZipEntrySource.JitStream)
				{
					if (this._sourceStream == null)
					{
						this._sourceStream = this._OpenDelegate(this.FileName);
					}
					this.PrepSourceStream();
					input = this._sourceStream;
					try
					{
						return this._sourceStream.Length;
					}
					catch (NotSupportedException)
					{
						return result;
					}
				}
				if (this._Source == ZipEntrySource.FileSystem)
				{
					FileShare fileShare = FileShare.ReadWrite;
					fileShare |= FileShare.Delete;
					input = File.Open(this.LocalFileName, FileMode.Open, FileAccess.Read, fileShare);
					result = input.Length;
				}
			}
			return result;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000121EC File Offset: 0x000103EC
		internal void FinishOutputStream(Stream s, CountingStream entryCounter, Stream encryptor, Stream compressor, CrcCalculatorStream output)
		{
			if (output == null)
			{
				return;
			}
			output.Close();
			if (compressor is DeflateStream)
			{
				compressor.Close();
			}
			else if (compressor is ParallelDeflateOutputStream)
			{
				compressor.Close();
			}
			encryptor.Flush();
			encryptor.Close();
			this._LengthOfTrailer = 0;
			this._UncompressedSize = output.TotalBytesSlurped;
			this._CompressedFileDataSize = entryCounter.BytesWritten;
			this._CompressedSize = this._CompressedFileDataSize;
			this._Crc32 = output.Crc;
			this.StoreRelativeOffset();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00012274 File Offset: 0x00010474
		internal void PostProcessOutput(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			if (this._UncompressedSize == 0L && this._CompressedSize == 0L)
			{
				if (this._Source == ZipEntrySource.ZipOutputStream)
				{
					return;
				}
				if (this._Password != null)
				{
					int num = 0;
					if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
					{
						num = 12;
					}
					if (this._Source == ZipEntrySource.ZipOutputStream && !s.CanSeek)
					{
						throw new ZipException("Zero bytes written, encryption in use, and non-seekable output.");
					}
					if (this.Encryption != EncryptionAlgorithm.None)
					{
						s.Seek((long)(-1 * num), SeekOrigin.Current);
						s.SetLength(s.Position);
						if (countingStream != null)
						{
							countingStream.Adjust((long)num);
						}
						this._LengthOfHeader -= num;
						this.__FileDataPosition -= (long)num;
					}
					this._Password = null;
					this._BitField &= -2;
					int num2 = 6;
					this._EntryHeader[num2++] = (byte)(this._BitField & 255);
					this._EntryHeader[num2++] = (byte)(((int)this._BitField & 65280) >> 8);
				}
				this.CompressionMethod = CompressionMethod.None;
				this.Encryption = EncryptionAlgorithm.None;
			}
			else if (this._zipCrypto_forWrite != null && this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				this._CompressedSize += 12L;
			}
			int num3 = 8;
			this._EntryHeader[num3++] = (byte)(this._CompressionMethod & 255);
			this._EntryHeader[num3++] = (byte)(((int)this._CompressionMethod & 65280) >> 8);
			num3 = 14;
			this._EntryHeader[num3++] = (byte)(this._Crc32 & 255);
			this._EntryHeader[num3++] = (byte)((this._Crc32 & 65280) >> 8);
			this._EntryHeader[num3++] = (byte)((this._Crc32 & 16711680) >> 16);
			this._EntryHeader[num3++] = (byte)(((long)this._Crc32 & (long)((ulong)-16777216)) >> 24);
			this.SetZip64Flags();
			short num4 = (short)((int)this._EntryHeader[26] + (int)this._EntryHeader[27] * 256);
			short num5 = (short)((int)this._EntryHeader[28] + (int)this._EntryHeader[29] * 256);
			if (this._OutputUsesZip64.Value)
			{
				this._EntryHeader[4] = 45;
				this._EntryHeader[5] = 0;
				for (int i = 0; i < 8; i++)
				{
					this._EntryHeader[num3++] = byte.MaxValue;
				}
				num3 = (int)(30 + num4);
				this._EntryHeader[num3++] = 1;
				this._EntryHeader[num3++] = 0;
				num3 += 2;
				Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, this._EntryHeader, num3, 8);
				num3 += 8;
				Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, this._EntryHeader, num3, 8);
			}
			else
			{
				this._EntryHeader[4] = 20;
				this._EntryHeader[5] = 0;
				num3 = 18;
				this._EntryHeader[num3++] = (byte)(this._CompressedSize & 255L);
				this._EntryHeader[num3++] = (byte)((this._CompressedSize & 65280L) >> 8);
				this._EntryHeader[num3++] = (byte)((this._CompressedSize & 16711680L) >> 16);
				this._EntryHeader[num3++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
				this._EntryHeader[num3++] = (byte)(this._UncompressedSize & 255L);
				this._EntryHeader[num3++] = (byte)((this._UncompressedSize & 65280L) >> 8);
				this._EntryHeader[num3++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
				this._EntryHeader[num3++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
				if (num5 != 0)
				{
					num3 = (int)(30 + num4);
					if ((short)((int)this._EntryHeader[num3 + 2] + (int)this._EntryHeader[num3 + 3] * 256) == 16)
					{
						this._EntryHeader[num3++] = 153;
						this._EntryHeader[num3++] = 153;
					}
				}
			}
			if ((this._BitField & 8) != 8 || (this._Source == ZipEntrySource.ZipOutputStream && s.CanSeek))
			{
				ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
				if (zipSegmentedStream != null && this._diskNumber != zipSegmentedStream.CurrentSegment)
				{
					using (Stream stream = ZipSegmentedStream.ForUpdate(this._container.ZipFile.Name, this._diskNumber))
					{
						stream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
						stream.Write(this._EntryHeader, 0, this._EntryHeader.Length);
						goto IL_4C2;
					}
				}
				s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
				s.Write(this._EntryHeader, 0, this._EntryHeader.Length);
				if (countingStream != null)
				{
					countingStream.Adjust((long)this._EntryHeader.Length);
				}
				s.Seek(this._CompressedSize, SeekOrigin.Current);
			}
			IL_4C2:
			if ((this._BitField & 8) == 8 && !this.IsDirectory)
			{
				byte[] array = new byte[16 + (this._OutputUsesZip64.Value ? 8 : 0)];
				num3 = 0;
				Array.Copy(BitConverter.GetBytes(134695760), 0, array, num3, 4);
				num3 += 4;
				Array.Copy(BitConverter.GetBytes(this._Crc32), 0, array, num3, 4);
				num3 += 4;
				if (this._OutputUsesZip64.Value)
				{
					Array.Copy(BitConverter.GetBytes(this._CompressedSize), 0, array, num3, 8);
					num3 += 8;
					Array.Copy(BitConverter.GetBytes(this._UncompressedSize), 0, array, num3, 8);
					num3 += 8;
				}
				else
				{
					array[num3++] = (byte)(this._CompressedSize & 255L);
					array[num3++] = (byte)((this._CompressedSize & 65280L) >> 8);
					array[num3++] = (byte)((this._CompressedSize & 16711680L) >> 16);
					array[num3++] = (byte)((this._CompressedSize & (long)((ulong)-16777216)) >> 24);
					array[num3++] = (byte)(this._UncompressedSize & 255L);
					array[num3++] = (byte)((this._UncompressedSize & 65280L) >> 8);
					array[num3++] = (byte)((this._UncompressedSize & 16711680L) >> 16);
					array[num3++] = (byte)((this._UncompressedSize & (long)((ulong)-16777216)) >> 24);
				}
				s.Write(array, 0, array.Length);
				this._LengthOfTrailer += array.Length;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000128DC File Offset: 0x00010ADC
		private void SetZip64Flags()
		{
			this._entryRequiresZip64 = new bool?(this._CompressedSize >= (long)((ulong)-1) || this._UncompressedSize >= (long)((ulong)-1) || this._RelativeOffsetOfLocalHeader >= (long)((ulong)-1));
			if (this._container.Zip64 == Zip64Option.Default && this._entryRequiresZip64.Value)
			{
				throw new ZipException("Compressed or Uncompressed size, or offset exceeds the maximum value. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
			}
			this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00012964 File Offset: 0x00010B64
		internal void PrepOutputStream(Stream s, long streamLength, out CountingStream outputCounter, out Stream encryptor, out Stream compressor, out CrcCalculatorStream output)
		{
			outputCounter = new CountingStream(s);
			if (streamLength != 0L)
			{
				encryptor = this.MaybeApplyEncryption(outputCounter);
				compressor = this.MaybeApplyCompression(encryptor, streamLength);
			}
			else
			{
				Stream stream;
				compressor = (stream = outputCounter);
				encryptor = stream;
			}
			output = new CrcCalculatorStream(compressor, true);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000129B0 File Offset: 0x00010BB0
		private Stream MaybeApplyCompression(Stream s, long streamLength)
		{
			if (this._CompressionMethod != 8 || this.CompressionLevel == CompressionLevel.None)
			{
				return s;
			}
			if (this._container.ParallelDeflateThreshold == 0L || (streamLength > this._container.ParallelDeflateThreshold && this._container.ParallelDeflateThreshold > 0L))
			{
				if (this._container.ParallelDeflater == null)
				{
					this._container.ParallelDeflater = new ParallelDeflateOutputStream(s, this.CompressionLevel, this._container.Strategy, true);
					if (this._container.CodecBufferSize > 0)
					{
						this._container.ParallelDeflater.BufferSize = this._container.CodecBufferSize;
					}
					if (this._container.ParallelDeflateMaxBufferPairs > 0)
					{
						this._container.ParallelDeflater.MaxBufferPairs = this._container.ParallelDeflateMaxBufferPairs;
					}
				}
				ParallelDeflateOutputStream parallelDeflater = this._container.ParallelDeflater;
				parallelDeflater.Reset(s);
				return parallelDeflater;
			}
			DeflateStream deflateStream = new DeflateStream(s, CompressionMode.Compress, this.CompressionLevel, true);
			if (this._container.CodecBufferSize > 0)
			{
				deflateStream.BufferSize = this._container.CodecBufferSize;
			}
			deflateStream.Strategy = this._container.Strategy;
			return deflateStream;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00012ADB File Offset: 0x00010CDB
		private Stream MaybeApplyEncryption(Stream s)
		{
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				return new ZipCipherStream(s, this._zipCrypto_forWrite, CryptoMode.Encrypt);
			}
			return s;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00012AF5 File Offset: 0x00010CF5
		private void OnZipErrorWhileSaving(Exception e)
		{
			if (this._container.ZipFile != null)
			{
				this._ioOperationCanceled = this._container.ZipFile.OnZipErrorSaving(this, e);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00012B1C File Offset: 0x00010D1C
		internal void Write(Stream s)
		{
			CountingStream countingStream = s as CountingStream;
			ZipSegmentedStream zipSegmentedStream = s as ZipSegmentedStream;
			bool flag = false;
			do
			{
				try
				{
					if (this._Source == ZipEntrySource.ZipFile && !this._restreamRequiredOnSave)
					{
						this.CopyThroughOneEntry(s);
						break;
					}
					if (this.IsDirectory)
					{
						this.WriteHeader(s, 1);
						this.StoreRelativeOffset();
						this._entryRequiresZip64 = new bool?(this._RelativeOffsetOfLocalHeader >= (long)((ulong)-1));
						this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
						if (zipSegmentedStream != null)
						{
							this._diskNumber = zipSegmentedStream.CurrentSegment;
						}
						break;
					}
					int num = 0;
					bool flag2;
					do
					{
						num++;
						this.WriteHeader(s, num);
						this.WriteSecurityMetadata(s);
						this._WriteEntryData(s);
						this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
						flag2 = (num <= 1 && s.CanSeek && this.WantReadAgain());
						if (flag2)
						{
							if (zipSegmentedStream != null)
							{
								zipSegmentedStream.TruncateBackward(this._diskNumber, this._RelativeOffsetOfLocalHeader);
							}
							else
							{
								s.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
							}
							s.SetLength(s.Position);
							if (countingStream != null)
							{
								countingStream.Adjust(this._TotalEntrySize);
							}
						}
					}
					while (flag2);
					this._skippedDuringSave = false;
					flag = true;
				}
				catch (Exception ex)
				{
					ZipErrorAction zipErrorAction = this.ZipErrorAction;
					int num2 = 0;
					while (this.ZipErrorAction != ZipErrorAction.Throw)
					{
						if (this.ZipErrorAction == ZipErrorAction.Skip || this.ZipErrorAction == ZipErrorAction.Retry)
						{
							long num3 = (countingStream != null) ? countingStream.ComputedPosition : s.Position;
							long num4 = num3 - this._future_ROLH;
							if (num4 > 0L)
							{
								s.Seek(num4, SeekOrigin.Current);
								long position = s.Position;
								s.SetLength(s.Position);
								if (countingStream != null)
								{
									countingStream.Adjust(num3 - position);
								}
							}
							if (this.ZipErrorAction == ZipErrorAction.Skip)
							{
								this.WriteStatus("Skipping file {0} (exception: {1})", new object[]
								{
									this.LocalFileName,
									ex.ToString()
								});
								this._skippedDuringSave = true;
								flag = true;
							}
							else
							{
								this.ZipErrorAction = zipErrorAction;
							}
						}
						else
						{
							if (num2 > 0)
							{
								throw;
							}
							if (this.ZipErrorAction == ZipErrorAction.InvokeErrorEvent)
							{
								this.OnZipErrorWhileSaving(ex);
								if (this._ioOperationCanceled)
								{
									flag = true;
									goto IL_236;
								}
							}
							num2++;
							continue;
						}
						IL_236:
						goto IL_238;
					}
					throw;
				}
				IL_238:;
			}
			while (!flag);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00012D84 File Offset: 0x00010F84
		internal void StoreRelativeOffset()
		{
			this._RelativeOffsetOfLocalHeader = this._future_ROLH;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00012D92 File Offset: 0x00010F92
		internal void NotifySaveComplete()
		{
			this._Encryption_FromZipFile = this._Encryption;
			this._CompressionMethod_FromZipFile = this._CompressionMethod;
			this._restreamRequiredOnSave = false;
			this._metadataChanged = false;
			this._Source = ZipEntrySource.ZipFile;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00012DC4 File Offset: 0x00010FC4
		internal void WriteSecurityMetadata(Stream outstream)
		{
			if (this.Encryption == EncryptionAlgorithm.None)
			{
				return;
			}
			string password = this._Password;
			if (this._Source == ZipEntrySource.ZipFile && password == null)
			{
				password = this._container.Password;
			}
			if (password == null)
			{
				this._zipCrypto_forWrite = null;
				return;
			}
			if (this.Encryption == EncryptionAlgorithm.PkzipWeak)
			{
				this._zipCrypto_forWrite = ZipCrypto.ForWrite(password);
				Random random = new Random();
				byte[] array = new byte[12];
				random.NextBytes(array);
				if ((this._BitField & 8) == 8)
				{
					this._TimeBlob = SharedUtilities.DateTimeToPacked(this.LastModified);
					array[11] = (byte)(this._TimeBlob >> 8 & 255);
				}
				else
				{
					this.FigureCrc32();
					array[11] = (byte)(this._Crc32 >> 24 & 255);
				}
				byte[] array2 = this._zipCrypto_forWrite.EncryptMessage(array, array.Length);
				outstream.Write(array2, 0, array2.Length);
				this._LengthOfHeader += array2.Length;
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00012EA8 File Offset: 0x000110A8
		private void CopyThroughOneEntry(Stream outStream)
		{
			if (this.LengthOfHeader == 0)
			{
				throw new BadStateException("Bad header length.");
			}
			if (this._metadataChanged || this.ArchiveStream is ZipSegmentedStream || outStream is ZipSegmentedStream || (this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default) || (!this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always))
			{
				this.CopyThroughWithRecompute(outStream);
			}
			else
			{
				this.CopyThroughWithNoChange(outStream);
			}
			this._entryRequiresZip64 = new bool?(this._CompressedSize >= (long)((ulong)-1) || this._UncompressedSize >= (long)((ulong)-1) || this._RelativeOffsetOfLocalHeader >= (long)((ulong)-1));
			this._OutputUsesZip64 = new bool?(this._container.Zip64 == Zip64Option.Always || this._entryRequiresZip64.Value);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00012F80 File Offset: 0x00011180
		private void CopyThroughWithRecompute(Stream outstream)
		{
			byte[] array = new byte[this.BufferSize];
			CountingStream countingStream = new CountingStream(this.ArchiveStream);
			long relativeOffsetOfLocalHeader = this._RelativeOffsetOfLocalHeader;
			int lengthOfHeader = this.LengthOfHeader;
			this.WriteHeader(outstream, 0);
			this.StoreRelativeOffset();
			if (!this.FileName.EndsWith("/"))
			{
				long num = relativeOffsetOfLocalHeader + (long)lengthOfHeader;
				int num2 = ZipEntry.GetLengthOfCryptoHeaderBytes(this._Encryption_FromZipFile);
				num -= (long)num2;
				this._LengthOfHeader += num2;
				countingStream.Seek(num, SeekOrigin.Begin);
				long num3 = this._CompressedSize;
				while (num3 > 0L)
				{
					num2 = ((num3 > (long)array.Length) ? array.Length : ((int)num3));
					int num4 = countingStream.Read(array, 0, num2);
					outstream.Write(array, 0, num4);
					num3 -= (long)num4;
					this.OnWriteBlock(countingStream.BytesRead, this._CompressedSize);
					if (this._ioOperationCanceled)
					{
						break;
					}
				}
				if ((this._BitField & 8) == 8)
				{
					int num5 = 16;
					if (this._InputUsesZip64)
					{
						num5 += 8;
					}
					byte[] buffer = new byte[num5];
					countingStream.Read(buffer, 0, num5);
					if (this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Default)
					{
						outstream.Write(buffer, 0, 8);
						if (this._CompressedSize > (long)((ulong)-1))
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 8, 4);
						if (this._UncompressedSize > (long)((ulong)-1))
						{
							throw new InvalidOperationException("ZIP64 is required");
						}
						outstream.Write(buffer, 16, 4);
						this._LengthOfTrailer -= 8;
					}
					else if (!this._InputUsesZip64 && this._container.UseZip64WhenSaving == Zip64Option.Always)
					{
						byte[] buffer2 = new byte[4];
						outstream.Write(buffer, 0, 8);
						outstream.Write(buffer, 8, 4);
						outstream.Write(buffer2, 0, 4);
						outstream.Write(buffer, 12, 4);
						outstream.Write(buffer2, 0, 4);
						this._LengthOfTrailer += 8;
					}
					else
					{
						outstream.Write(buffer, 0, num5);
					}
				}
			}
			this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00013190 File Offset: 0x00011390
		private void CopyThroughWithNoChange(Stream outstream)
		{
			byte[] array = new byte[this.BufferSize];
			CountingStream countingStream = new CountingStream(this.ArchiveStream);
			countingStream.Seek(this._RelativeOffsetOfLocalHeader, SeekOrigin.Begin);
			if (this._TotalEntrySize == 0L)
			{
				this._TotalEntrySize = (long)this._LengthOfHeader + this._CompressedFileDataSize + (long)this._LengthOfTrailer;
			}
			CountingStream countingStream2 = outstream as CountingStream;
			this._RelativeOffsetOfLocalHeader = ((countingStream2 != null) ? countingStream2.ComputedPosition : outstream.Position);
			long num = this._TotalEntrySize;
			while (num > 0L)
			{
				int count = (num > (long)array.Length) ? array.Length : ((int)num);
				int num2 = countingStream.Read(array, 0, count);
				outstream.Write(array, 0, num2);
				num -= (long)num2;
				this.OnWriteBlock(countingStream.BytesRead, this._TotalEntrySize);
				if (this._ioOperationCanceled)
				{
					break;
				}
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0001325C File Offset: 0x0001145C
		[Conditional("Trace")]
		private void TraceWriteLine(string format, params object[] varParams)
		{
			object outputLock = this._outputLock;
			lock (outputLock)
			{
				int hashCode = Thread.CurrentThread.GetHashCode();
				Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
				Console.Write("{0:000} ZipEntry.Write ", hashCode);
				Console.WriteLine(format, varParams);
				Console.ResetColor();
			}
		}

		// Token: 0x040001C1 RID: 449
		private short _VersionMadeBy;

		// Token: 0x040001C2 RID: 450
		private short _InternalFileAttrs;

		// Token: 0x040001C3 RID: 451
		private int _ExternalFileAttrs;

		// Token: 0x040001C4 RID: 452
		private short _filenameLength;

		// Token: 0x040001C5 RID: 453
		private short _extraFieldLength;

		// Token: 0x040001C6 RID: 454
		private short _commentLength;

		// Token: 0x040001CD RID: 461
		private ZipCrypto _zipCrypto_forExtract;

		// Token: 0x040001CE RID: 462
		private ZipCrypto _zipCrypto_forWrite;

		// Token: 0x040001CF RID: 463
		internal DateTime _LastModified;

		// Token: 0x040001D0 RID: 464
		private DateTime _Mtime;

		// Token: 0x040001D1 RID: 465
		private DateTime _Atime;

		// Token: 0x040001D2 RID: 466
		private DateTime _Ctime;

		// Token: 0x040001D3 RID: 467
		private bool _ntfsTimesAreSet;

		// Token: 0x040001D4 RID: 468
		private bool _emitNtfsTimes = true;

		// Token: 0x040001D5 RID: 469
		private bool _emitUnixTimes;

		// Token: 0x040001D6 RID: 470
		private bool _TrimVolumeFromFullyQualifiedPaths = true;

		// Token: 0x040001D7 RID: 471
		internal string _LocalFileName;

		// Token: 0x040001D8 RID: 472
		private string _FileNameInArchive;

		// Token: 0x040001D9 RID: 473
		internal short _VersionNeeded;

		// Token: 0x040001DA RID: 474
		internal short _BitField;

		// Token: 0x040001DB RID: 475
		internal short _CompressionMethod;

		// Token: 0x040001DC RID: 476
		private short _CompressionMethod_FromZipFile;

		// Token: 0x040001DD RID: 477
		private CompressionLevel _CompressionLevel;

		// Token: 0x040001DE RID: 478
		internal string _Comment;

		// Token: 0x040001DF RID: 479
		private bool _IsDirectory;

		// Token: 0x040001E0 RID: 480
		private byte[] _CommentBytes;

		// Token: 0x040001E1 RID: 481
		internal long _CompressedSize;

		// Token: 0x040001E2 RID: 482
		internal long _CompressedFileDataSize;

		// Token: 0x040001E3 RID: 483
		internal long _UncompressedSize;

		// Token: 0x040001E4 RID: 484
		internal int _TimeBlob;

		// Token: 0x040001E5 RID: 485
		private bool _crcCalculated;

		// Token: 0x040001E6 RID: 486
		internal int _Crc32;

		// Token: 0x040001E7 RID: 487
		internal byte[] _Extra;

		// Token: 0x040001E8 RID: 488
		private bool _metadataChanged;

		// Token: 0x040001E9 RID: 489
		private bool _restreamRequiredOnSave;

		// Token: 0x040001EA RID: 490
		private bool _sourceIsEncrypted;

		// Token: 0x040001EB RID: 491
		private bool _skippedDuringSave;

		// Token: 0x040001EC RID: 492
		private uint _diskNumber;

		// Token: 0x040001ED RID: 493
		private static Encoding utf8 = Encoding.GetEncoding("UTF-8");

		// Token: 0x040001EE RID: 494
		private Encoding _actualEncoding;

		// Token: 0x040001EF RID: 495
		internal ZipContainer _container;

		// Token: 0x040001F0 RID: 496
		private long __FileDataPosition = -1L;

		// Token: 0x040001F1 RID: 497
		private byte[] _EntryHeader;

		// Token: 0x040001F2 RID: 498
		internal long _RelativeOffsetOfLocalHeader;

		// Token: 0x040001F3 RID: 499
		private long _future_ROLH;

		// Token: 0x040001F4 RID: 500
		private long _TotalEntrySize;

		// Token: 0x040001F5 RID: 501
		private int _LengthOfHeader;

		// Token: 0x040001F6 RID: 502
		private int _LengthOfTrailer;

		// Token: 0x040001F7 RID: 503
		internal bool _InputUsesZip64;

		// Token: 0x040001F8 RID: 504
		private uint _UnsupportedAlgorithmId;

		// Token: 0x040001F9 RID: 505
		internal string _Password;

		// Token: 0x040001FA RID: 506
		internal ZipEntrySource _Source;

		// Token: 0x040001FB RID: 507
		internal EncryptionAlgorithm _Encryption;

		// Token: 0x040001FC RID: 508
		internal EncryptionAlgorithm _Encryption_FromZipFile;

		// Token: 0x040001FD RID: 509
		internal byte[] _WeakEncryptionHeader;

		// Token: 0x040001FE RID: 510
		internal Stream _archiveStream;

		// Token: 0x040001FF RID: 511
		private Stream _sourceStream;

		// Token: 0x04000200 RID: 512
		private long? _sourceStreamOriginalPosition;

		// Token: 0x04000201 RID: 513
		private bool _sourceWasJitProvided;

		// Token: 0x04000202 RID: 514
		private bool _ioOperationCanceled;

		// Token: 0x04000203 RID: 515
		private bool _presumeZip64;

		// Token: 0x04000204 RID: 516
		private bool? _entryRequiresZip64;

		// Token: 0x04000205 RID: 517
		private bool? _OutputUsesZip64;

		// Token: 0x04000206 RID: 518
		private bool _IsText;

		// Token: 0x04000207 RID: 519
		private ZipEntryTimestamp _timestamp;

		// Token: 0x04000208 RID: 520
		private static DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04000209 RID: 521
		private static DateTime _win32Epoch = DateTime.FromFileTimeUtc(0L);

		// Token: 0x0400020A RID: 522
		private static DateTime _zeroHour = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400020B RID: 523
		private WriteDelegate _WriteDelegate;

		// Token: 0x0400020C RID: 524
		private OpenDelegate _OpenDelegate;

		// Token: 0x0400020D RID: 525
		private CloseDelegate _CloseDelegate;

		// Token: 0x0400020E RID: 526
		private Stream _inputDecryptorStream;

		// Token: 0x0400020F RID: 527
		private int _readExtraDepth;

		// Token: 0x04000210 RID: 528
		private object _outputLock = new object();

		// Token: 0x02000060 RID: 96
		private class CopyHelper
		{
			// Token: 0x06000447 RID: 1095 RVA: 0x0001956C File Offset: 0x0001776C
			internal static string AppendCopyToFileName(string f)
			{
				ZipEntry.CopyHelper.callCount++;
				if (ZipEntry.CopyHelper.callCount > 25)
				{
					throw new OverflowException("overflow while creating filename");
				}
				int num = 1;
				int num2 = f.LastIndexOf(".");
				if (num2 == -1)
				{
					Match match = ZipEntry.CopyHelper.re.Match(f);
					if (match.Success)
					{
						num = int.Parse(match.Groups[1].Value) + 1;
						string str = string.Format(" (copy {0})", num);
						f = f.Substring(0, match.Index) + str;
					}
					else
					{
						string str2 = string.Format(" (copy {0})", num);
						f += str2;
					}
				}
				else
				{
					Match match2 = ZipEntry.CopyHelper.re.Match(f.Substring(0, num2));
					if (match2.Success)
					{
						num = int.Parse(match2.Groups[1].Value) + 1;
						string str3 = string.Format(" (copy {0})", num);
						f = f.Substring(0, match2.Index) + str3 + f.Substring(num2);
					}
					else
					{
						string str4 = string.Format(" (copy {0})", num);
						f = f.Substring(0, num2) + str4 + f.Substring(num2);
					}
				}
				return f;
			}

			// Token: 0x04000316 RID: 790
			private static Regex re = new Regex(" \\(copy (\\d+)\\)$");

			// Token: 0x04000317 RID: 791
			private static int callCount = 0;
		}

		// Token: 0x02000061 RID: 97
		// (Invoke) Token: 0x0600044B RID: 1099
		private delegate T Func<T>();
	}
}
