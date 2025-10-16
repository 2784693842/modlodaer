using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000AB RID: 171
	internal class AcmStream : IDisposable
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x00015188 File Offset: 0x00013388
		public AcmStream(WaveFormat sourceFormat, WaveFormat destFormat)
		{
			try
			{
				this.streamHandle = IntPtr.Zero;
				this.sourceFormat = sourceFormat;
				int num = Math.Max(65536, sourceFormat.AverageBytesPerSecond);
				num -= num % sourceFormat.BlockAlign;
				IntPtr intPtr = WaveFormat.MarshalToPtr(sourceFormat);
				IntPtr intPtr2 = WaveFormat.MarshalToPtr(destFormat);
				try
				{
					MmException.Try(AcmInterop.acmStreamOpen2(out this.streamHandle, IntPtr.Zero, intPtr, intPtr2, null, IntPtr.Zero, IntPtr.Zero, AcmStreamOpenFlags.NonRealTime), "acmStreamOpen");
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
					Marshal.FreeHGlobal(intPtr2);
				}
				int destBufferLength = this.SourceToDest(num);
				this.streamHeader = new AcmStreamHeader(this.streamHandle, num, destBufferLength);
				this.driverHandle = IntPtr.Zero;
			}
			catch
			{
				this.Dispose();
				throw;
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001525C File Offset: 0x0001345C
		public AcmStream(IntPtr driverId, WaveFormat sourceFormat, WaveFilter waveFilter)
		{
			int num = Math.Max(16384, sourceFormat.AverageBytesPerSecond);
			this.sourceFormat = sourceFormat;
			num -= num % sourceFormat.BlockAlign;
			MmException.Try(AcmInterop.acmDriverOpen(out this.driverHandle, driverId, 0), "acmDriverOpen");
			IntPtr intPtr = WaveFormat.MarshalToPtr(sourceFormat);
			try
			{
				MmException.Try(AcmInterop.acmStreamOpen2(out this.streamHandle, this.driverHandle, intPtr, intPtr, waveFilter, IntPtr.Zero, IntPtr.Zero, AcmStreamOpenFlags.NonRealTime), "acmStreamOpen");
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			this.streamHeader = new AcmStreamHeader(this.streamHandle, num, this.SourceToDest(num));
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001530C File Offset: 0x0001350C
		public int SourceToDest(int source)
		{
			if (source == 0)
			{
				return 0;
			}
			int result;
			MmException.Try(AcmInterop.acmStreamSize(this.streamHandle, source, out result, AcmStreamSizeFlags.Source), "acmStreamSize");
			return result;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00015338 File Offset: 0x00013538
		public int DestToSource(int dest)
		{
			if (dest == 0)
			{
				return 0;
			}
			int result;
			MmException.Try(AcmInterop.acmStreamSize(this.streamHandle, dest, out result, AcmStreamSizeFlags.Destination), "acmStreamSize");
			return result;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00015364 File Offset: 0x00013564
		public static WaveFormat SuggestPcmFormat(WaveFormat compressedFormat)
		{
			WaveFormat waveFormat = new WaveFormat(compressedFormat.SampleRate, 16, compressedFormat.Channels);
			IntPtr intPtr = WaveFormat.MarshalToPtr(waveFormat);
			IntPtr intPtr2 = WaveFormat.MarshalToPtr(compressedFormat);
			try
			{
				MmResult result = AcmInterop.acmFormatSuggest2(IntPtr.Zero, intPtr2, intPtr, Marshal.SizeOf(waveFormat), AcmFormatSuggestFlags.FormatTag);
				waveFormat = WaveFormat.MarshalFromPtr(intPtr);
				MmException.Try(result, "acmFormatSuggest");
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			return waveFormat;
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x000153DC File Offset: 0x000135DC
		public byte[] SourceBuffer
		{
			get
			{
				return this.streamHeader.SourceBuffer;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000153E9 File Offset: 0x000135E9
		public byte[] DestBuffer
		{
			get
			{
				return this.streamHeader.DestBuffer;
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000153F6 File Offset: 0x000135F6
		public void Reposition()
		{
			this.streamHeader.Reposition();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00015403 File Offset: 0x00013603
		public int Convert(int bytesToConvert, out int sourceBytesConverted)
		{
			if (bytesToConvert % this.sourceFormat.BlockAlign != 0)
			{
				bytesToConvert -= bytesToConvert % this.sourceFormat.BlockAlign;
			}
			return this.streamHeader.Convert(bytesToConvert, out sourceBytesConverted);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00015434 File Offset: 0x00013634
		[Obsolete("Call the version returning sourceBytesConverted instead")]
		public int Convert(int bytesToConvert)
		{
			int num;
			int result = this.Convert(bytesToConvert, out num);
			if (num != bytesToConvert)
			{
				throw new MmException(MmResult.NotSupported, "AcmStreamHeader.Convert didn't convert everything");
			}
			return result;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001545A File Offset: 0x0001365A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001546C File Offset: 0x0001366C
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.streamHeader != null)
			{
				this.streamHeader.Dispose();
				this.streamHeader = null;
			}
			if (this.streamHandle != IntPtr.Zero)
			{
				MmResult mmResult = AcmInterop.acmStreamClose(this.streamHandle, 0);
				this.streamHandle = IntPtr.Zero;
				if (mmResult != MmResult.NoError)
				{
					throw new MmException(mmResult, "acmStreamClose");
				}
			}
			if (this.driverHandle != IntPtr.Zero)
			{
				AcmInterop.acmDriverClose(this.driverHandle, 0);
				this.driverHandle = IntPtr.Zero;
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000154FC File Offset: 0x000136FC
		~AcmStream()
		{
			this.Dispose(false);
		}

		// Token: 0x04000401 RID: 1025
		private IntPtr streamHandle;

		// Token: 0x04000402 RID: 1026
		private IntPtr driverHandle;

		// Token: 0x04000403 RID: 1027
		private AcmStreamHeader streamHeader;

		// Token: 0x04000404 RID: 1028
		private readonly WaveFormat sourceFormat;
	}
}
