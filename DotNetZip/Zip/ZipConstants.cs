using System;

namespace Ionic.Zip
{
	// Token: 0x02000042 RID: 66
	internal static class ZipConstants
	{
		// Token: 0x040001AC RID: 428
		public const uint PackedToRemovableMedia = 808471376U;

		// Token: 0x040001AD RID: 429
		public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792U;

		// Token: 0x040001AE RID: 430
		public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008U;

		// Token: 0x040001AF RID: 431
		public const uint EndOfCentralDirectorySignature = 101010256U;

		// Token: 0x040001B0 RID: 432
		public const int ZipEntrySignature = 67324752;

		// Token: 0x040001B1 RID: 433
		public const int ZipEntryDataDescriptorSignature = 134695760;

		// Token: 0x040001B2 RID: 434
		public const int SplitArchiveSignature = 134695760;

		// Token: 0x040001B3 RID: 435
		public const int ZipDirEntrySignature = 33639248;

		// Token: 0x040001B4 RID: 436
		public const int AesKeySize = 192;

		// Token: 0x040001B5 RID: 437
		public const int AesBlockSize = 128;

		// Token: 0x040001B6 RID: 438
		public const ushort AesAlgId128 = 26126;

		// Token: 0x040001B7 RID: 439
		public const ushort AesAlgId192 = 26127;

		// Token: 0x040001B8 RID: 440
		public const ushort AesAlgId256 = 26128;
	}
}
