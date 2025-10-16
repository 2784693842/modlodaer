using System;

namespace KeraLua
{
	// Token: 0x02000074 RID: 116
	[Flags]
	internal enum LuaHookMask
	{
		// Token: 0x0400013F RID: 319
		Disabled = 0,
		// Token: 0x04000140 RID: 320
		Call = 1,
		// Token: 0x04000141 RID: 321
		Return = 2,
		// Token: 0x04000142 RID: 322
		Line = 4,
		// Token: 0x04000143 RID: 323
		Count = 8
	}
}
