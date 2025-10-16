using System;
using System.Runtime.InteropServices;

namespace KeraLua
{
	// Token: 0x020000A5 RID: 165
	internal static class DelegateExtensions
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x00013C7A File Offset: 0x00011E7A
		public static LuaFunction ToLuaFunction(this IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer<LuaFunction>(ptr);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00013C91 File Offset: 0x00011E91
		public static IntPtr ToFunctionPointer(this LuaFunction d)
		{
			if (d == null)
			{
				return IntPtr.Zero;
			}
			return Marshal.GetFunctionPointerForDelegate<LuaFunction>(d);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00013CA2 File Offset: 0x00011EA2
		public static LuaHookFunction ToLuaHookFunction(this IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer<LuaHookFunction>(ptr);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00013CB9 File Offset: 0x00011EB9
		public static IntPtr ToFunctionPointer(this LuaHookFunction d)
		{
			if (d == null)
			{
				return IntPtr.Zero;
			}
			return Marshal.GetFunctionPointerForDelegate<LuaHookFunction>(d);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00013CCA File Offset: 0x00011ECA
		public static LuaKFunction ToLuaKFunction(this IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer<LuaKFunction>(ptr);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00013CE1 File Offset: 0x00011EE1
		public static IntPtr ToFunctionPointer(this LuaKFunction d)
		{
			if (d == null)
			{
				return IntPtr.Zero;
			}
			return Marshal.GetFunctionPointerForDelegate<LuaKFunction>(d);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00013CF2 File Offset: 0x00011EF2
		public static LuaReader ToLuaReader(this IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer<LuaReader>(ptr);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00013D09 File Offset: 0x00011F09
		public static IntPtr ToFunctionPointer(this LuaReader d)
		{
			if (d == null)
			{
				return IntPtr.Zero;
			}
			return Marshal.GetFunctionPointerForDelegate<LuaReader>(d);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00013D1A File Offset: 0x00011F1A
		public static LuaWriter ToLuaWriter(this IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer<LuaWriter>(ptr);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00013D31 File Offset: 0x00011F31
		public static IntPtr ToFunctionPointer(this LuaWriter d)
		{
			if (d == null)
			{
				return IntPtr.Zero;
			}
			return Marshal.GetFunctionPointerForDelegate<LuaWriter>(d);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00013D42 File Offset: 0x00011F42
		public static LuaAlloc ToLuaAlloc(this IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer<LuaAlloc>(ptr);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00013D59 File Offset: 0x00011F59
		public static IntPtr ToFunctionPointer(this LuaAlloc d)
		{
			if (d == null)
			{
				return IntPtr.Zero;
			}
			return Marshal.GetFunctionPointerForDelegate<LuaAlloc>(d);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00013D6A File Offset: 0x00011F6A
		public static LuaWarnFunction ToLuaWarning(this IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			return Marshal.GetDelegateForFunctionPointer<LuaWarnFunction>(ptr);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00013D81 File Offset: 0x00011F81
		public static IntPtr ToFunctionPointer(this LuaWarnFunction d)
		{
			if (d == null)
			{
				return IntPtr.Zero;
			}
			return Marshal.GetFunctionPointerForDelegate<LuaWarnFunction>(d);
		}
	}
}
