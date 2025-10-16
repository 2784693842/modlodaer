using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x020000C7 RID: 199
	internal interface ISampleProvider
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000487 RID: 1159
		WaveFormat WaveFormat { get; }

		// Token: 0x06000488 RID: 1160
		int Read(float[] buffer, int offset, int count);
	}
}
