using System;

namespace System.Numerics.Hashing
{
	// Token: 0x020000D5 RID: 213
	internal static class HashHelpers
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x0002E018 File Offset: 0x0002C218
		public static int Combine(int h1, int h2)
		{
			uint num = (uint)(h1 << 5 | (int)((uint)h1 >> 27));
			return (int)(num + (uint)h1 ^ (uint)h2);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002E034 File Offset: 0x0002C234
		// Note: this type is marked as 'beforefieldinit'.
		static HashHelpers()
		{
			HashHelpers.RandomSeed = Guid.NewGuid().GetHashCode();
		}

		// Token: 0x04000268 RID: 616
		public static readonly int RandomSeed;
	}
}
