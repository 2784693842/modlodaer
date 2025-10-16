using System;

namespace Ionic.Zip
{
	// Token: 0x02000036 RID: 54
	public class ExtractProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x060001AD RID: 429 RVA: 0x0000CED4 File Offset: 0x0000B0D4
		internal ExtractProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesExtracted, ZipEntry entry, string extractLocation) : base(archiveName, before ? ZipProgressEventType.Extracting_BeforeExtractEntry : ZipProgressEventType.Extracting_AfterExtractEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesExtracted = entriesExtracted;
			this._target = extractLocation;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000CF05 File Offset: 0x0000B105
		internal ExtractProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000CF0F File Offset: 0x0000B10F
		internal ExtractProgressEventArgs()
		{
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000CF17 File Offset: 0x0000B117
		internal static ExtractProgressEventArgs BeforeExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_BeforeExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000CF3B File Offset: 0x0000B13B
		internal static ExtractProgressEventArgs ExtractExisting(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000CF5F File Offset: 0x0000B15F
		internal static ExtractProgressEventArgs AfterExtractEntry(string archiveName, ZipEntry entry, string extractLocation)
		{
			return new ExtractProgressEventArgs
			{
				ArchiveName = archiveName,
				EventType = ZipProgressEventType.Extracting_AfterExtractEntry,
				CurrentEntry = entry,
				_target = extractLocation
			};
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000CF83 File Offset: 0x0000B183
		internal static ExtractProgressEventArgs ExtractAllStarted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_BeforeExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000CF94 File Offset: 0x0000B194
		internal static ExtractProgressEventArgs ExtractAllCompleted(string archiveName, string extractLocation)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_AfterExtractAll)
			{
				_target = extractLocation
			};
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000CFA5 File Offset: 0x0000B1A5
		internal static ExtractProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesWritten, long totalBytes)
		{
			return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_EntryBytesWritten)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesWritten,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000CFCB File Offset: 0x0000B1CB
		public int EntriesExtracted
		{
			get
			{
				return this._entriesExtracted;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000CFD3 File Offset: 0x0000B1D3
		public string ExtractLocation
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x0400019C RID: 412
		private int _entriesExtracted;

		// Token: 0x0400019D RID: 413
		private string _target;
	}
}
