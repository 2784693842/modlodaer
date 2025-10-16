using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000B1 RID: 177
	[StructLayout(LayoutKind.Sequential)]
	internal class WaveFilter
	{
		// Token: 0x06000438 RID: 1080 RVA: 0x000157DC File Offset: 0x000139DC
		public WaveFilter()
		{
			this.StructureSize = Marshal.SizeOf(typeof(WaveFilter));
			base..ctor();
		}

		// Token: 0x04000442 RID: 1090
		public int StructureSize;

		// Token: 0x04000443 RID: 1091
		public int FilterTag;

		// Token: 0x04000444 RID: 1092
		public int Filter;

		// Token: 0x04000445 RID: 1093
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
		public int[] Reserved;
	}
}
