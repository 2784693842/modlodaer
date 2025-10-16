using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000071 RID: 113
	internal static class Utils
	{
		// Token: 0x06000286 RID: 646 RVA: 0x0000DD78 File Offset: 0x0000BF78
		internal static int ilog(int x)
		{
			int num = 0;
			while (x > 0)
			{
				num++;
				x >>= 1;
			}
			return num;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000DD97 File Offset: 0x0000BF97
		internal static uint BitReverse(uint n)
		{
			return Utils.BitReverse(n, 32);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		internal static uint BitReverse(uint n, int bits)
		{
			n = ((n & 2863311530U) >> 1 | (n & 1431655765U) << 1);
			n = ((n & 3435973836U) >> 2 | (n & 858993459U) << 2);
			n = ((n & 4042322160U) >> 4 | (n & 252645135U) << 4);
			n = ((n & 4278255360U) >> 8 | (n & 16711935U) << 8);
			return (n >> 16 | n << 16) >> 32 - bits;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000DE16 File Offset: 0x0000C016
		internal static float ClipValue(float value, ref bool clipped)
		{
			if (value > 0.99999994f)
			{
				clipped = true;
				return 0.99999994f;
			}
			if (value < -0.99999994f)
			{
				clipped = true;
				return -0.99999994f;
			}
			return value;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000DE3C File Offset: 0x0000C03C
		internal static float ConvertFromVorbisFloat32(uint bits)
		{
			int num = (int)bits >> 31;
			double y = (double)(((bits & 2145386496U) >> 21) - 788U);
			return (float)(((ulong)(bits & 2097151U) ^ (ulong)((long)num)) + (ulong)((long)(num & 1))) * (float)Math.Pow(2.0, y);
		}
	}
}
