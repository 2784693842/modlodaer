using System;

namespace System.Buffers
{
	// Token: 0x020000B0 RID: 176
	internal enum OperationStatus
	{
		// Token: 0x040001DE RID: 478
		Done,
		// Token: 0x040001DF RID: 479
		DestinationTooSmall,
		// Token: 0x040001E0 RID: 480
		NeedMoreData,
		// Token: 0x040001E1 RID: 481
		InvalidData
	}
}
