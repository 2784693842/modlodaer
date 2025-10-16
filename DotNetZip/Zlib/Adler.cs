using System;

namespace Ionic.Zlib
{
	// Token: 0x02000023 RID: 35
	public sealed class Adler
	{
		// Token: 0x060000FB RID: 251 RVA: 0x0000AFEC File Offset: 0x000091EC
		public static uint Adler32(uint adler, byte[] buf, int index, int len)
		{
			if (buf == null)
			{
				return 1U;
			}
			uint num = adler & 65535U;
			uint num2 = adler >> 16 & 65535U;
			while (len > 0)
			{
				int i = (len < Adler.NMAX) ? len : Adler.NMAX;
				len -= i;
				while (i >= 16)
				{
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					num += (uint)buf[index++];
					num2 += num;
					i -= 16;
				}
				if (i != 0)
				{
					do
					{
						num += (uint)buf[index++];
						num2 += num;
					}
					while (--i != 0);
				}
				num %= Adler.BASE;
				num2 %= Adler.BASE;
			}
			return num2 << 16 | num;
		}

		// Token: 0x04000139 RID: 313
		private static readonly uint BASE = 65521U;

		// Token: 0x0400013A RID: 314
		private static readonly int NMAX = 5552;
	}
}
