using System;

namespace Ionic.Zip
{
	// Token: 0x02000034 RID: 52
	public class AddProgressEventArgs : ZipProgressEventArgs
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x0000CE1D File Offset: 0x0000B01D
		internal AddProgressEventArgs()
		{
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000CE25 File Offset: 0x0000B025
		private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor) : base(archiveName, flavor)
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000CE2F File Offset: 0x0000B02F
		internal static AddProgressEventArgs AfterEntry(string archiveName, ZipEntry entry, int entriesTotal)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry)
			{
				EntriesTotal = entriesTotal,
				CurrentEntry = entry
			};
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000CE46 File Offset: 0x0000B046
		internal static AddProgressEventArgs Started(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000CE4F File Offset: 0x0000B04F
		internal static AddProgressEventArgs Completed(string archiveName)
		{
			return new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
		}
	}
}
