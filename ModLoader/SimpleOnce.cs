using System;
using System.Threading;

namespace ModLoader
{
	// Token: 0x0200000F RID: 15
	public class SimpleOnce
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002C12 File Offset: 0x00000E12
		public bool DoOnce()
		{
			return Interlocked.CompareExchange(ref this.OnceStat, 1L, 0L) == 0L;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002C27 File Offset: 0x00000E27
		public bool SetDone()
		{
			return Interlocked.CompareExchange(ref this.OnceStat, 2L, 1L) == 1L;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002C3C File Offset: 0x00000E3C
		public bool Done()
		{
			return Interlocked.Read(ref this.OnceStat) == 2L;
		}

		// Token: 0x0400002E RID: 46
		private long OnceStat;
	}
}
