using System;

namespace Ionic.Zip
{
	// Token: 0x02000031 RID: 49
	public enum ZipProgressEventType
	{
		// Token: 0x0400017C RID: 380
		Adding_Started,
		// Token: 0x0400017D RID: 381
		Adding_AfterAddEntry,
		// Token: 0x0400017E RID: 382
		Adding_Completed,
		// Token: 0x0400017F RID: 383
		Reading_Started,
		// Token: 0x04000180 RID: 384
		Reading_BeforeReadEntry,
		// Token: 0x04000181 RID: 385
		Reading_AfterReadEntry,
		// Token: 0x04000182 RID: 386
		Reading_Completed,
		// Token: 0x04000183 RID: 387
		Reading_ArchiveBytesRead,
		// Token: 0x04000184 RID: 388
		Saving_Started,
		// Token: 0x04000185 RID: 389
		Saving_BeforeWriteEntry,
		// Token: 0x04000186 RID: 390
		Saving_AfterWriteEntry,
		// Token: 0x04000187 RID: 391
		Saving_Completed,
		// Token: 0x04000188 RID: 392
		Saving_AfterSaveTempArchive,
		// Token: 0x04000189 RID: 393
		Saving_BeforeRenameTempArchive,
		// Token: 0x0400018A RID: 394
		Saving_AfterRenameTempArchive,
		// Token: 0x0400018B RID: 395
		Saving_AfterCompileSelfExtractor,
		// Token: 0x0400018C RID: 396
		Saving_EntryBytesRead,
		// Token: 0x0400018D RID: 397
		Extracting_BeforeExtractEntry,
		// Token: 0x0400018E RID: 398
		Extracting_AfterExtractEntry,
		// Token: 0x0400018F RID: 399
		Extracting_ExtractEntryWouldOverwrite,
		// Token: 0x04000190 RID: 400
		Extracting_EntryBytesWritten,
		// Token: 0x04000191 RID: 401
		Extracting_BeforeExtractAll,
		// Token: 0x04000192 RID: 402
		Extracting_AfterExtractAll,
		// Token: 0x04000193 RID: 403
		Error_Saving
	}
}
