using System;
using System.Runtime.InteropServices;

namespace KeraLua
{
	// Token: 0x02000078 RID: 120
	internal struct LuaRegister
	{
		// Token: 0x0400014B RID: 331
		public string name;

		// Token: 0x0400014C RID: 332
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public LuaFunction function;
	}
}
