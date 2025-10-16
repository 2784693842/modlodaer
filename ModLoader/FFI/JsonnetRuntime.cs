using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ModLoader.FFI
{
	// Token: 0x0200002D RID: 45
	public static class JsonnetRuntime
	{
		// Token: 0x060000AE RID: 174
		[DllImport("Jsonnet4CSTIModLoader", EntryPoint = "JsonnetRuntime_add_pat")]
		public static extern void JsonnetRuntimeAddPat([MarshalAs(UnmanagedType.LPUTF8Str)] string pat);

		// Token: 0x060000AF RID: 175
		[DllImport("Jsonnet4CSTIModLoader", EntryPoint = "JsonnetRuntime_reg_global")]
		public static extern void JsonnetRuntimeRegGlobal([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string val);

		// Token: 0x060000B0 RID: 176
		[DllImport("Jsonnet4CSTIModLoader", EntryPoint = "JsonnetRuntime_read")]
		private static extern Byte64 JsonnetRuntimeRead(long key);

		// Token: 0x060000B1 RID: 177
		[DllImport("Jsonnet4CSTIModLoader", EntryPoint = "JsonnetRuntime_eval")]
		private static extern long JsonnetRuntimeEval([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string val);

		// Token: 0x060000B2 RID: 178 RVA: 0x00009EAC File Offset: 0x000080AC
		public unsafe static string JsonnetEval(string name, string val)
		{
			long num = JsonnetRuntime.JsonnetRuntimeEval(name, val);
			if (num < 0L)
			{
				return "";
			}
			List<byte> list = new List<byte>(64);
			for (;;)
			{
				Byte64 @byte = JsonnetRuntime.JsonnetRuntimeRead(num);
				for (int i = 0; i < 64; i++)
				{
					if (*(ref @byte.Data.FixedElementField + i) == 0)
					{
						goto IL_57;
					}
					list.Add(*(ref @byte.Data.FixedElementField + i));
				}
			}
			IL_57:
			return Encoding.UTF8.GetString(list.ToArray());
		}
	}
}
