using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x0200008D RID: 141
	internal class Crc : ICrc
	{
		// Token: 0x06000350 RID: 848 RVA: 0x0001275C File Offset: 0x0001095C
		static Crc()
		{
			Crc.s_crcTable = new uint[256];
			for (uint num = 0U; num < 256U; num += 1U)
			{
				uint num2 = num << 24;
				for (int i = 0; i < 8; i++)
				{
					num2 = (num2 << 1 ^ ((num2 >= 2147483648U) ? 79764919U : 0U));
				}
				Crc.s_crcTable[(int)num] = num2;
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000127B6 File Offset: 0x000109B6
		public Crc()
		{
			this.Reset();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000127C4 File Offset: 0x000109C4
		public void Reset()
		{
			this._crc = 0U;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000127CD File Offset: 0x000109CD
		public void Update(int nextVal)
		{
			this._crc = (this._crc << 8 ^ Crc.s_crcTable[(int)(checked((IntPtr)(unchecked((long)nextVal ^ (long)((ulong)(this._crc >> 24))))))]);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000127F2 File Offset: 0x000109F2
		public bool Test(uint checkCrc)
		{
			return this._crc == checkCrc;
		}

		// Token: 0x0400036B RID: 875
		private const uint CRC32_POLY = 79764919U;

		// Token: 0x0400036C RID: 876
		private static readonly uint[] s_crcTable;

		// Token: 0x0400036D RID: 877
		private uint _crc;
	}
}
