using System;
using System.IO;
using System.Runtime.InteropServices;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Wave
{
	// Token: 0x02000046 RID: 70
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal class Gsm610WaveFormat : WaveFormat
	{
		// Token: 0x06000148 RID: 328 RVA: 0x0000BC00 File Offset: 0x00009E00
		public Gsm610WaveFormat()
		{
			this.waveFormatTag = WaveFormatEncoding.Gsm610;
			this.channels = 1;
			this.averageBytesPerSecond = 1625;
			this.bitsPerSample = 0;
			this.blockAlign = 65;
			this.sampleRate = 8000;
			this.extraSize = 2;
			this.samplesPerBlock = 320;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000BC59 File Offset: 0x00009E59
		public short SamplesPerBlock
		{
			get
			{
				return this.samplesPerBlock;
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000BC61 File Offset: 0x00009E61
		public override void Serialize(BinaryWriter writer)
		{
			base.Serialize(writer);
			writer.Write(this.samplesPerBlock);
		}

		// Token: 0x04000244 RID: 580
		private readonly short samplesPerBlock;
	}
}
