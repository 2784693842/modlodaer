using System;

namespace Ionic.Zlib
{
	// Token: 0x02000018 RID: 24
	internal class WorkItem
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x00009DE0 File Offset: 0x00007FE0
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x040000D8 RID: 216
		public byte[] buffer;

		// Token: 0x040000D9 RID: 217
		public byte[] compressed;

		// Token: 0x040000DA RID: 218
		public int crc;

		// Token: 0x040000DB RID: 219
		public int index;

		// Token: 0x040000DC RID: 220
		public int ordinal;

		// Token: 0x040000DD RID: 221
		public int inputBytesAvailable;

		// Token: 0x040000DE RID: 222
		public int compressedBytesAvailable;

		// Token: 0x040000DF RID: 223
		public ZlibCodec compressor;
	}
}
