using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x02000066 RID: 102
	// (Invoke) Token: 0x06000258 RID: 600
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void LuaHookFunction(IntPtr luaState, IntPtr ar);
}
