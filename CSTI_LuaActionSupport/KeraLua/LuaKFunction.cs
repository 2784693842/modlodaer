using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x0200006E RID: 110
	// (Invoke) Token: 0x0600033D RID: 829
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int LuaKFunction(IntPtr L, int status, IntPtr ctx);
}
