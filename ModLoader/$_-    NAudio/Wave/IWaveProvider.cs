using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x0200003A RID: 58
	internal interface IWaveProvider
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000EA RID: 234
		WaveFormat WaveFormat { get; }

		// Token: 0x060000EB RID: 235
		int Read(byte[] buffer, int offset, int count);
	}
}
