using System;

namespace KeraLua
{
	// Token: 0x02000072 RID: 114
	internal enum LuaType
	{
		// Token: 0x04000132 RID: 306
		None = -1,
		// Token: 0x04000133 RID: 307
		Nil,
		// Token: 0x04000134 RID: 308
		Boolean,
		// Token: 0x04000135 RID: 309
		LightUserData,
		// Token: 0x04000136 RID: 310
		Number,
		// Token: 0x04000137 RID: 311
		String,
		// Token: 0x04000138 RID: 312
		Table,
		// Token: 0x04000139 RID: 313
		Function,
		// Token: 0x0400013A RID: 314
		UserData,
		// Token: 0x0400013B RID: 315
		Thread
	}
}
