using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000C2 RID: 194
	[Flags]
	internal enum AcmStreamOpenFlags
	{
		// Token: 0x040004B7 RID: 1207
		Query = 1,
		// Token: 0x040004B8 RID: 1208
		Async = 2,
		// Token: 0x040004B9 RID: 1209
		NonRealTime = 4,
		// Token: 0x040004BA RID: 1210
		CallbackTypeMask = 458752,
		// Token: 0x040004BB RID: 1211
		CallbackNull = 0,
		// Token: 0x040004BC RID: 1212
		CallbackWindow = 65536,
		// Token: 0x040004BD RID: 1213
		CallbackTask = 131072,
		// Token: 0x040004BE RID: 1214
		CallbackFunction = 196608,
		// Token: 0x040004BF RID: 1215
		CallbackThread = 131072,
		// Token: 0x040004C0 RID: 1216
		CallbackEvent = 327680
	}
}
