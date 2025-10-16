using System;

namespace KeraLua
{
	// Token: 0x02000076 RID: 118
	internal enum LuaStatus
	{
		// Token: 0x04000145 RID: 325
		OK,
		// Token: 0x04000146 RID: 326
		Yield,
		// Token: 0x04000147 RID: 327
		ErrRun,
		// Token: 0x04000148 RID: 328
		ErrSyntax,
		// Token: 0x04000149 RID: 329
		ErrMem,
		// Token: 0x0400014A RID: 330
		ErrErr
	}
}
