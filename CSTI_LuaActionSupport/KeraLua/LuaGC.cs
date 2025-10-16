using System;

namespace KeraLua
{
	// Token: 0x02000071 RID: 113
	internal enum LuaGC
	{
		// Token: 0x04000126 RID: 294
		Stop,
		// Token: 0x04000127 RID: 295
		Restart,
		// Token: 0x04000128 RID: 296
		Collect,
		// Token: 0x04000129 RID: 297
		Count,
		// Token: 0x0400012A RID: 298
		Countb,
		// Token: 0x0400012B RID: 299
		Step,
		// Token: 0x0400012C RID: 300
		[Obsolete("Deprecatad since Lua 5.4, Use Incremental instead")]
		SetPause,
		// Token: 0x0400012D RID: 301
		[Obsolete("Deprecatad since Lua 5.4, Use Incremental instead")]
		SetStepMultiplier,
		// Token: 0x0400012E RID: 302
		IsRunning = 9,
		// Token: 0x0400012F RID: 303
		Generational,
		// Token: 0x04000130 RID: 304
		Incremental
	}
}
