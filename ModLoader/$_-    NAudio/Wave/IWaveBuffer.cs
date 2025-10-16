using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000CA RID: 202
	internal interface IWaveBuffer
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600049F RID: 1183
		byte[] ByteBuffer { get; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004A0 RID: 1184
		float[] FloatBuffer { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004A1 RID: 1185
		short[] ShortBuffer { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004A2 RID: 1186
		int[] IntBuffer { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004A3 RID: 1187
		int MaxSize { get; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004A4 RID: 1188
		int ByteBufferCount { get; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004A5 RID: 1189
		int FloatBufferCount { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004A6 RID: 1190
		int ShortBufferCount { get; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004A7 RID: 1191
		int IntBufferCount { get; }
	}
}
