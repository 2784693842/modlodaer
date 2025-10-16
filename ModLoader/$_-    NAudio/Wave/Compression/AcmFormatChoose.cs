using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000BB RID: 187
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
	internal struct AcmFormatChoose
	{
		// Token: 0x0400046B RID: 1131
		public int structureSize;

		// Token: 0x0400046C RID: 1132
		public AcmFormatChooseStyleFlags styleFlags;

		// Token: 0x0400046D RID: 1133
		public IntPtr ownerWindowHandle;

		// Token: 0x0400046E RID: 1134
		public IntPtr selectedWaveFormatPointer;

		// Token: 0x0400046F RID: 1135
		public int selectedWaveFormatByteSize;

		// Token: 0x04000470 RID: 1136
		[MarshalAs(UnmanagedType.LPTStr)]
		public string title;

		// Token: 0x04000471 RID: 1137
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
		public string formatTagDescription;

		// Token: 0x04000472 RID: 1138
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string formatDescription;

		// Token: 0x04000473 RID: 1139
		[MarshalAs(UnmanagedType.LPTStr)]
		public string name;

		// Token: 0x04000474 RID: 1140
		public int nameByteSize;

		// Token: 0x04000475 RID: 1141
		public AcmFormatEnumFlags formatEnumFlags;

		// Token: 0x04000476 RID: 1142
		public IntPtr waveFormatEnumPointer;

		// Token: 0x04000477 RID: 1143
		public IntPtr instanceHandle;

		// Token: 0x04000478 RID: 1144
		[MarshalAs(UnmanagedType.LPTStr)]
		public string templateName;

		// Token: 0x04000479 RID: 1145
		public IntPtr customData;

		// Token: 0x0400047A RID: 1146
		public AcmInterop.AcmFormatChooseHookProc windowCallbackFunction;
	}
}
