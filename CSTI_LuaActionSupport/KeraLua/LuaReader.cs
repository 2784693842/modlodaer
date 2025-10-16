using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x02000075 RID: 117
	// (Invoke) Token: 0x06000345 RID: 837
	[SuppressUnmanagedCodeSecurity]
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate IntPtr LuaReader(IntPtr L, IntPtr ud, ref UIntPtr sz);
}
