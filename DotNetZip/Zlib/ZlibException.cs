using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x0200001F RID: 31
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x0000AE3D File Offset: 0x0000903D
		public ZlibException()
		{
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000AE45 File Offset: 0x00009045
		public ZlibException(string s) : base(s)
		{
		}
	}
}
