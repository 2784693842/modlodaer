using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000AA RID: 170
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class Pinnable<T>
	{
		// Token: 0x040001D6 RID: 470
		public T Data;
	}
}
