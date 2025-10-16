using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x02000077 RID: 119
	// (Invoke) Token: 0x06000349 RID: 841
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void LuaWarnFunction(IntPtr ud, IntPtr msg, int tocont);
}
