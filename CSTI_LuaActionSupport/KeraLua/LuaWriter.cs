using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x02000070 RID: 112
	// (Invoke) Token: 0x06000341 RID: 833
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate int LuaWriter(IntPtr L, IntPtr p, UIntPtr size, IntPtr ud);
}
