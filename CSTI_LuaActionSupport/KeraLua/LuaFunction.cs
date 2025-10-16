using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x0200006D RID: 109
	// (Invoke) Token: 0x06000339 RID: 825
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int LuaFunction(IntPtr luaState);
}
