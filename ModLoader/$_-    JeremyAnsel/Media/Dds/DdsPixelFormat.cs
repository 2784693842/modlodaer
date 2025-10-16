using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020JeremyAnsel.Media.Dds
{
	// Token: 0x02000030 RID: 48
	internal sealed class DdsPixelFormat
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x0000240A File Offset: 0x0000060A
		internal DdsPixelFormat()
		{
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000A2B2 File Offset: 0x000084B2
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000A2BA File Offset: 0x000084BA
		public DdsPixelFormatOptions Options { get; internal set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000A2C3 File Offset: 0x000084C3
		// (set) Token: 0x060000DA RID: 218 RVA: 0x0000A2CB File Offset: 0x000084CB
		public DdsFourCC FourCC { get; internal set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000A2D4 File Offset: 0x000084D4
		// (set) Token: 0x060000DC RID: 220 RVA: 0x0000A2DC File Offset: 0x000084DC
		public int RgbBitCount { get; internal set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000DD RID: 221 RVA: 0x0000A2E5 File Offset: 0x000084E5
		// (set) Token: 0x060000DE RID: 222 RVA: 0x0000A2ED File Offset: 0x000084ED
		public uint RedBitMask { get; internal set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000DF RID: 223 RVA: 0x0000A2F6 File Offset: 0x000084F6
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x0000A2FE File Offset: 0x000084FE
		public uint GreenBitMask { get; internal set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000A307 File Offset: 0x00008507
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x0000A30F File Offset: 0x0000850F
		public uint BlueBitMask { get; internal set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000A318 File Offset: 0x00008518
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000A320 File Offset: 0x00008520
		public uint AlphaBitMask { get; internal set; }
	}
}
