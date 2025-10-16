using System;
using System.IO;

namespace Ionic.Zip
{
	// Token: 0x02000056 RID: 86
	internal class ZipSegmentedStream : Stream
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x00018DAB File Offset: 0x00016FAB
		private ZipSegmentedStream()
		{
			this._exceptionPending = false;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00018DBA File Offset: 0x00016FBA
		public static ZipSegmentedStream ForReading(string name, uint initialDiskNumber, uint maxDiskNumber)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream();
			zipSegmentedStream.rwMode = ZipSegmentedStream.RwMode.ReadOnly;
			zipSegmentedStream.CurrentSegment = initialDiskNumber;
			zipSegmentedStream._maxDiskNumber = maxDiskNumber;
			zipSegmentedStream._baseName = name;
			zipSegmentedStream._SetReadStream();
			return zipSegmentedStream;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00018DE4 File Offset: 0x00016FE4
		public static ZipSegmentedStream ForWriting(string name, int maxSegmentSize)
		{
			ZipSegmentedStream zipSegmentedStream = new ZipSegmentedStream
			{
				rwMode = ZipSegmentedStream.RwMode.Write,
				CurrentSegment = 0U,
				_baseName = name,
				_maxSegmentSize = maxSegmentSize,
				_baseDir = Path.GetDirectoryName(name)
			};
			if (zipSegmentedStream._baseDir == "")
			{
				zipSegmentedStream._baseDir = ".";
			}
			zipSegmentedStream._SetWriteStream(0U);
			return zipSegmentedStream;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00018E44 File Offset: 0x00017044
		public static Stream ForUpdate(string name, uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			return File.Open(string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(name), Path.GetFileNameWithoutExtension(name)), diskNumber + 1U), FileMode.Open, FileAccess.ReadWrite, FileShare.None);
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00018E81 File Offset: 0x00017081
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x00018E89 File Offset: 0x00017089
		public bool ContiguousWrite { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00018E92 File Offset: 0x00017092
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x00018E9A File Offset: 0x0001709A
		public uint CurrentSegment
		{
			get
			{
				return this._currentDiskNumber;
			}
			private set
			{
				this._currentDiskNumber = value;
				this._currentName = null;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00018EAA File Offset: 0x000170AA
		public string CurrentName
		{
			get
			{
				if (this._currentName == null)
				{
					this._currentName = this._NameForSegment(this.CurrentSegment);
				}
				return this._currentName;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00018ECC File Offset: 0x000170CC
		public string CurrentTempName
		{
			get
			{
				return this._currentTempName;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00018ED4 File Offset: 0x000170D4
		private string _NameForSegment(uint diskNumber)
		{
			if (diskNumber >= 99U)
			{
				this._exceptionPending = true;
				throw new OverflowException("The number of zip segments would exceed 99.");
			}
			return string.Format("{0}.z{1:D2}", Path.Combine(Path.GetDirectoryName(this._baseName), Path.GetFileNameWithoutExtension(this._baseName)), diskNumber + 1U);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00018F25 File Offset: 0x00017125
		public uint ComputeSegment(int length)
		{
			if (this._innerStream.Position + (long)length > (long)this._maxSegmentSize)
			{
				return this.CurrentSegment + 1U;
			}
			return this.CurrentSegment;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00018F50 File Offset: 0x00017150
		public override string ToString()
		{
			return string.Format("{0}[{1}][{2}], pos=0x{3:X})", new object[]
			{
				"ZipSegmentedStream",
				this.CurrentName,
				this.rwMode.ToString(),
				this.Position
			});
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00018FA0 File Offset: 0x000171A0
		private void _SetReadStream()
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
			}
			if (this.CurrentSegment + 1U == this._maxDiskNumber)
			{
				this._currentName = this._baseName;
			}
			this._innerStream = File.OpenRead(this.CurrentName);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00018FF0 File Offset: 0x000171F0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.ReadOnly)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Read.");
			}
			int num = this._innerStream.Read(buffer, offset, count);
			int num2 = num;
			while (num2 != count)
			{
				if (this._innerStream.Position != this._innerStream.Length)
				{
					this._exceptionPending = true;
					throw new ZipException(string.Format("Read error in file {0}", this.CurrentName));
				}
				if (this.CurrentSegment + 1U == this._maxDiskNumber)
				{
					return num;
				}
				uint currentSegment = this.CurrentSegment;
				this.CurrentSegment = currentSegment + 1U;
				this._SetReadStream();
				offset += num2;
				count -= num2;
				num2 = this._innerStream.Read(buffer, offset, count);
				num += num2;
			}
			return num;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000190AC File Offset: 0x000172AC
		private void _SetWriteStream(uint increment)
		{
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this.CurrentName))
				{
					File.Delete(this.CurrentName);
				}
				File.Move(this._currentTempName, this.CurrentName);
			}
			if (increment > 0U)
			{
				this.CurrentSegment += increment;
			}
			SharedUtilities.CreateAndOpenUniqueTempFile(this._baseDir, out this._innerStream, out this._currentTempName);
			if (this.CurrentSegment == 0U)
			{
				this._innerStream.Write(BitConverter.GetBytes(134695760), 0, 4);
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00019140 File Offset: 0x00017340
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException("Stream Error: Cannot Write.");
			}
			if (this.ContiguousWrite)
			{
				if (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					this._SetWriteStream(1U);
				}
			}
			else
			{
				while (this._innerStream.Position + (long)count > (long)this._maxSegmentSize)
				{
					int num = this._maxSegmentSize - (int)this._innerStream.Position;
					this._innerStream.Write(buffer, offset, num);
					this._SetWriteStream(1U);
					count -= num;
					offset += num;
				}
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000191E8 File Offset: 0x000173E8
		public long TruncateBackward(uint diskNumber, long offset)
		{
			if (diskNumber >= 99U)
			{
				throw new ArgumentOutOfRangeException("diskNumber");
			}
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new ZipException("bad state.");
			}
			if (diskNumber == this.CurrentSegment)
			{
				return this._innerStream.Seek(offset, SeekOrigin.Begin);
			}
			if (this._innerStream != null)
			{
				this._innerStream.Dispose();
				if (File.Exists(this._currentTempName))
				{
					File.Delete(this._currentTempName);
				}
			}
			for (uint num = this.CurrentSegment - 1U; num > diskNumber; num -= 1U)
			{
				string path = this._NameForSegment(num);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			this.CurrentSegment = diskNumber;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					this._currentTempName = SharedUtilities.InternalGetTempFileName();
					File.Move(this.CurrentName, this._currentTempName);
					break;
				}
				catch (IOException)
				{
					if (i == 2)
					{
						throw;
					}
				}
			}
			this._innerStream = new FileStream(this._currentTempName, FileMode.Open);
			return this._innerStream.Seek(offset, SeekOrigin.Begin);
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x000192F8 File Offset: 0x000174F8
		public override bool CanRead
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.ReadOnly && this._innerStream != null && this._innerStream.CanRead;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x00019318 File Offset: 0x00017518
		public override bool CanSeek
		{
			get
			{
				return this._innerStream != null && this._innerStream.CanSeek;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0001932F File Offset: 0x0001752F
		public override bool CanWrite
		{
			get
			{
				return this.rwMode == ZipSegmentedStream.RwMode.Write && this._innerStream != null && this._innerStream.CanWrite;
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001934F File Offset: 0x0001754F
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0001935C File Offset: 0x0001755C
		public override long Length
		{
			get
			{
				return this._innerStream.Length;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00019369 File Offset: 0x00017569
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00019376 File Offset: 0x00017576
		public override long Position
		{
			get
			{
				return this._innerStream.Position;
			}
			set
			{
				this._innerStream.Position = value;
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00019384 File Offset: 0x00017584
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._innerStream.Seek(offset, origin);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00019393 File Offset: 0x00017593
		public override void SetLength(long value)
		{
			if (this.rwMode != ZipSegmentedStream.RwMode.Write)
			{
				this._exceptionPending = true;
				throw new InvalidOperationException();
			}
			this._innerStream.SetLength(value);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000193B8 File Offset: 0x000175B8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._innerStream != null)
				{
					this._innerStream.Dispose();
					if (this.rwMode == ZipSegmentedStream.RwMode.Write)
					{
						bool exceptionPending = this._exceptionPending;
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x040002B8 RID: 696
		private ZipSegmentedStream.RwMode rwMode;

		// Token: 0x040002B9 RID: 697
		private bool _exceptionPending;

		// Token: 0x040002BA RID: 698
		private string _baseName;

		// Token: 0x040002BB RID: 699
		private string _baseDir;

		// Token: 0x040002BC RID: 700
		private string _currentName;

		// Token: 0x040002BD RID: 701
		private string _currentTempName;

		// Token: 0x040002BE RID: 702
		private uint _currentDiskNumber;

		// Token: 0x040002BF RID: 703
		private uint _maxDiskNumber;

		// Token: 0x040002C0 RID: 704
		private int _maxSegmentSize;

		// Token: 0x040002C1 RID: 705
		private Stream _innerStream;

		// Token: 0x02000067 RID: 103
		private enum RwMode
		{
			// Token: 0x04000329 RID: 809
			None,
			// Token: 0x0400032A RID: 810
			ReadOnly,
			// Token: 0x0400032B RID: 811
			Write
		}
	}
}
