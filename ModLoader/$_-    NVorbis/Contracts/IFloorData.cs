using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000066 RID: 102
	internal interface IFloorData
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000213 RID: 531
		bool ExecuteChannel { get; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000214 RID: 532
		// (set) Token: 0x06000215 RID: 533
		bool ForceEnergy { get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000216 RID: 534
		// (set) Token: 0x06000217 RID: 535
		bool ForceNoEnergy { get; set; }
	}
}
