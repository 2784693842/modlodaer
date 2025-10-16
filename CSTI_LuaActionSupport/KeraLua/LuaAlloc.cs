using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x0200006B RID: 107
	// (Invoke) Token: 0x06000335 RID: 821
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr LuaAlloc(IntPtr ud, IntPtr ptr, UIntPtr osize, UIntPtr nsize);
}
