using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000B2 RID: 178
	internal class AcmInterop
	{
		// Token: 0x06000439 RID: 1081
		[DllImport("msacm32.dll")]
		public static extern MmResult acmDriverAdd(out IntPtr driverHandle, IntPtr driverModule, IntPtr driverFunctionAddress, int priority, AcmDriverAddFlags flags);

		// Token: 0x0600043A RID: 1082
		[DllImport("msacm32.dll")]
		public static extern MmResult acmDriverRemove(IntPtr driverHandle, int removeFlags);

		// Token: 0x0600043B RID: 1083
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmDriverClose(IntPtr hAcmDriver, int closeFlags);

		// Token: 0x0600043C RID: 1084
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmDriverEnum(AcmInterop.AcmDriverEnumCallback fnCallback, IntPtr dwInstance, AcmDriverEnumFlags flags);

		// Token: 0x0600043D RID: 1085
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmDriverDetails(IntPtr hAcmDriver, ref AcmDriverDetails driverDetails, int reserved);

		// Token: 0x0600043E RID: 1086
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmDriverOpen(out IntPtr pAcmDriver, IntPtr hAcmDriverId, int openFlags);

		// Token: 0x0600043F RID: 1087
		[DllImport("Msacm32.dll", EntryPoint = "acmFormatChooseW")]
		public static extern MmResult acmFormatChoose(ref AcmFormatChoose formatChoose);

		// Token: 0x06000440 RID: 1088
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmFormatEnum(IntPtr hAcmDriver, ref AcmFormatDetails formatDetails, AcmInterop.AcmFormatEnumCallback callback, IntPtr instance, AcmFormatEnumFlags flags);

		// Token: 0x06000441 RID: 1089
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmFormatSuggest(IntPtr hAcmDriver, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = NAudio.Wave.WaveFormatCustomMarshaler)] [In] WaveFormat sourceFormat, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = NAudio.Wave.WaveFormatCustomMarshaler)] [In] [Out] WaveFormat destFormat, int sizeDestFormat, AcmFormatSuggestFlags suggestFlags);

		// Token: 0x06000442 RID: 1090
		[DllImport("Msacm32.dll", EntryPoint = "acmFormatSuggest")]
		public static extern MmResult acmFormatSuggest2(IntPtr hAcmDriver, IntPtr sourceFormatPointer, IntPtr destFormatPointer, int sizeDestFormat, AcmFormatSuggestFlags suggestFlags);

		// Token: 0x06000443 RID: 1091
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmFormatTagEnum(IntPtr hAcmDriver, ref AcmFormatTagDetails formatTagDetails, AcmInterop.AcmFormatTagEnumCallback callback, IntPtr instance, int reserved);

		// Token: 0x06000444 RID: 1092
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmMetrics(IntPtr hAcmObject, AcmMetrics metric, out int output);

		// Token: 0x06000445 RID: 1093
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmStreamOpen(out IntPtr hAcmStream, IntPtr hAcmDriver, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = NAudio.Wave.WaveFormatCustomMarshaler)] [In] WaveFormat sourceFormat, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = NAudio.Wave.WaveFormatCustomMarshaler)] [In] WaveFormat destFormat, [In] WaveFilter waveFilter, IntPtr callback, IntPtr instance, AcmStreamOpenFlags openFlags);

		// Token: 0x06000446 RID: 1094
		[DllImport("Msacm32.dll", EntryPoint = "acmStreamOpen")]
		public static extern MmResult acmStreamOpen2(out IntPtr hAcmStream, IntPtr hAcmDriver, IntPtr sourceFormatPointer, IntPtr destFormatPointer, [In] WaveFilter waveFilter, IntPtr callback, IntPtr instance, AcmStreamOpenFlags openFlags);

		// Token: 0x06000447 RID: 1095
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmStreamClose(IntPtr hAcmStream, int closeFlags);

		// Token: 0x06000448 RID: 1096
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmStreamConvert(IntPtr hAcmStream, [In] [Out] AcmStreamHeaderStruct streamHeader, AcmStreamConvertFlags streamConvertFlags);

		// Token: 0x06000449 RID: 1097
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmStreamPrepareHeader(IntPtr hAcmStream, [In] [Out] AcmStreamHeaderStruct streamHeader, int prepareFlags);

		// Token: 0x0600044A RID: 1098
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmStreamReset(IntPtr hAcmStream, int resetFlags);

		// Token: 0x0600044B RID: 1099
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmStreamSize(IntPtr hAcmStream, int inputBufferSize, out int outputBufferSize, AcmStreamSizeFlags flags);

		// Token: 0x0600044C RID: 1100
		[DllImport("Msacm32.dll")]
		public static extern MmResult acmStreamUnprepareHeader(IntPtr hAcmStream, [In] [Out] AcmStreamHeaderStruct streamHeader, int flags);

		// Token: 0x020000B3 RID: 179
		// (Invoke) Token: 0x0600044F RID: 1103
		public delegate bool AcmDriverEnumCallback(IntPtr hAcmDriverId, IntPtr instance, AcmDriverDetailsSupportFlags flags);

		// Token: 0x020000B4 RID: 180
		// (Invoke) Token: 0x06000453 RID: 1107
		public delegate bool AcmFormatChooseHookProc(IntPtr windowHandle, int message, IntPtr wParam, IntPtr lParam);

		// Token: 0x020000B5 RID: 181
		// (Invoke) Token: 0x06000457 RID: 1111
		public delegate bool AcmFormatEnumCallback(IntPtr hAcmDriverId, ref AcmFormatDetails formatDetails, IntPtr dwInstance, AcmDriverDetailsSupportFlags flags);

		// Token: 0x020000B6 RID: 182
		// (Invoke) Token: 0x0600045B RID: 1115
		public delegate bool AcmFormatTagEnumCallback(IntPtr hAcmDriverId, ref AcmFormatTagDetails formatTagDetails, IntPtr dwInstance, AcmDriverDetailsSupportFlags flags);
	}
}
