using System;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave.Compression
{
	// Token: 0x020000AC RID: 172
	internal class AcmStreamHeader : IDisposable
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x0001552C File Offset: 0x0001372C
		public AcmStreamHeader(IntPtr streamHandle, int sourceBufferLength, int destBufferLength)
		{
			this.streamHeader = new AcmStreamHeaderStruct();
			this.SourceBuffer = new byte[sourceBufferLength];
			this.hSourceBuffer = GCHandle.Alloc(this.SourceBuffer, GCHandleType.Pinned);
			this.DestBuffer = new byte[destBufferLength];
			this.hDestBuffer = GCHandle.Alloc(this.DestBuffer, GCHandleType.Pinned);
			this.streamHandle = streamHandle;
			this.firstTime = true;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00015594 File Offset: 0x00013794
		private void Prepare()
		{
			this.streamHeader.cbStruct = Marshal.SizeOf(this.streamHeader);
			this.streamHeader.sourceBufferLength = this.SourceBuffer.Length;
			this.streamHeader.sourceBufferPointer = this.hSourceBuffer.AddrOfPinnedObject();
			this.streamHeader.destBufferLength = this.DestBuffer.Length;
			this.streamHeader.destBufferPointer = this.hDestBuffer.AddrOfPinnedObject();
			MmException.Try(AcmInterop.acmStreamPrepareHeader(this.streamHandle, this.streamHeader, 0), "acmStreamPrepareHeader");
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00015628 File Offset: 0x00013828
		private void Unprepare()
		{
			this.streamHeader.sourceBufferLength = this.SourceBuffer.Length;
			this.streamHeader.sourceBufferPointer = this.hSourceBuffer.AddrOfPinnedObject();
			this.streamHeader.destBufferLength = this.DestBuffer.Length;
			this.streamHeader.destBufferPointer = this.hDestBuffer.AddrOfPinnedObject();
			MmResult mmResult = AcmInterop.acmStreamUnprepareHeader(this.streamHandle, this.streamHeader, 0);
			if (mmResult != MmResult.NoError)
			{
				throw new MmException(mmResult, "acmStreamUnprepareHeader");
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x000156A9 File Offset: 0x000138A9
		public void Reposition()
		{
			this.firstTime = true;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000156B4 File Offset: 0x000138B4
		public int Convert(int bytesToConvert, out int sourceBytesConverted)
		{
			this.Prepare();
			try
			{
				this.streamHeader.sourceBufferLength = bytesToConvert;
				this.streamHeader.sourceBufferLengthUsed = bytesToConvert;
				AcmStreamConvertFlags streamConvertFlags = this.firstTime ? (AcmStreamConvertFlags.BlockAlign | AcmStreamConvertFlags.Start) : AcmStreamConvertFlags.BlockAlign;
				MmException.Try(AcmInterop.acmStreamConvert(this.streamHandle, this.streamHeader, streamConvertFlags), "acmStreamConvert");
				this.firstTime = false;
				sourceBytesConverted = this.streamHeader.sourceBufferLengthUsed;
			}
			finally
			{
				this.Unprepare();
			}
			return this.streamHeader.destBufferLengthUsed;
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00015744 File Offset: 0x00013944
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0001574C File Offset: 0x0001394C
		public byte[] SourceBuffer { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00015755 File Offset: 0x00013955
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0001575D File Offset: 0x0001395D
		public byte[] DestBuffer { get; private set; }

		// Token: 0x06000434 RID: 1076 RVA: 0x00015766 File Offset: 0x00013966
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose(true);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00015775 File Offset: 0x00013975
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.SourceBuffer = null;
				this.DestBuffer = null;
				this.hSourceBuffer.Free();
				this.hDestBuffer.Free();
			}
			this.disposed = true;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000157AC File Offset: 0x000139AC
		~AcmStreamHeader()
		{
			this.Dispose(false);
		}

		// Token: 0x04000405 RID: 1029
		private AcmStreamHeaderStruct streamHeader;

		// Token: 0x04000406 RID: 1030
		private GCHandle hSourceBuffer;

		// Token: 0x04000407 RID: 1031
		private GCHandle hDestBuffer;

		// Token: 0x04000408 RID: 1032
		private IntPtr streamHandle;

		// Token: 0x04000409 RID: 1033
		private bool firstTime;

		// Token: 0x0400040C RID: 1036
		private bool disposed;
	}
}
