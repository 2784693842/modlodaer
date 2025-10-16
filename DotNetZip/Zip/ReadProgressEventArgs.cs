using System;

namespace Ionic.Zip
{
	// Token: 0x02000033 RID: 51
	public class ReadProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x0600019A RID: 410 RVA: 0x0000CDB4 File Offset: 0x0000AFB4
		internal ReadProgressEventArgs()
		{
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000CDBC File Offset: 0x0000AFBC
		private ReadProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000CDC6 File Offset: 0x0000AFC6
		internal static ReadProgressEventArgs Before(string archiveName, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_BeforeReadEntry)
			{
				EntriesTotal = entriesTotal
			};
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000CDD6 File Offset: 0x0000AFD6
		internal static ReadProgressEventArgs After(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_AfterReadEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000CDED File Offset: 0x0000AFED
		internal static ReadProgressEventArgs Started(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Started);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000CDF6 File Offset: 0x0000AFF6
		internal static ReadProgressEventArgs ByteUpdate(string archiveName, ZipEntry entry, long bytesXferred, long totalBytes)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_ArchiveBytesRead)
			{
				CurrentEntry = entry,
				BytesTransferred = bytesXferred,
				TotalBytesToTransfer = totalBytes
			};
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000CE14 File Offset: 0x0000B014
		internal static ReadProgressEventArgs Completed(string archiveName)
		{
			return new ReadProgressEventArgs(archiveName, ZipProgressEventType.Reading_Completed);
		}
	}
}
