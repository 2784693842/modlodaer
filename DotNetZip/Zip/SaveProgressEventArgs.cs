using System;

namespace Ionic.Zip
{
	// Token: 0x02000035 RID: 53
	public class SaveProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x0000CE58 File Offset: 0x0000B058
		internal SaveProgressEventArgs(string archiveName, bool before, int entriesTotal, int entriesSaved, ZipEntry entry) : base(archiveName, before ? ZipProgressEventType.Saving_BeforeWriteEntry : ZipProgressEventType.Saving_AfterWriteEntry)
		{
			base.EntriesTotal = entriesTotal;
			base.CurrentEntry = entry;
			this._entriesSaved = entriesSaved;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000CE81 File Offset: 0x0000B081
		internal SaveProgressEventArgs()
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000CE89 File Offset: 0x0000B089
		internal SaveProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000CE93 File Offset: 0x0000B093
		internal static SaveProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_EntryBytesRead)
			{
				ArchiveName = archiveName,
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000CEB9 File Offset: 0x0000B0B9
		internal static SaveProgressEventArgs Started(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Started);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000CEC2 File Offset: 0x0000B0C2
		internal static SaveProgressEventArgs Completed(string archiveName)
		{
			return new SaveProgressEventArgs(archiveName, ZipProgressEventType.Saving_Completed);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000CECC File Offset: 0x0000B0CC
		public int EntriesSaved
		{
			get
			{
				return this._entriesSaved;
			}
		}

		// Token: 0x0400019B RID: 411
		private int _entriesSaved;
	}
}
