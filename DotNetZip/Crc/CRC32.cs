using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ionic.Crc
{
	// Token: 0x02000029 RID: 41
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000C")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class CRC32
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000C628 File Offset: 0x0000A828
		public long TotalBytesRead
		{
			get
			{
				return this._TotalBytesRead;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000149 RID: 329 RVA: 0x0000C630 File Offset: 0x0000A830
		public int Crc32Result
		{
			get
			{
				return (int)(~(int)this._register);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000C639 File Offset: 0x0000A839
		public int GetCrc32(Stream input)
		{
			return this.GetCrc32AndCopy(input, null);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000C644 File Offset: 0x0000A844
		public int GetCrc32AndCopy(Stream input, Stream output)
		{
			if (input == null)
			{
				throw new Exception("The input stream must not be null.");
			}
			byte[] array = new byte[8192];
			int count = 8192;
			this._TotalBytesRead = 0L;
			int i = input.Read(array, 0, count);
			if (output != null)
			{
				output.Write(array, 0, i);
			}
			this._TotalBytesRead += (long)i;
			while (i > 0)
			{
				this.SlurpBlock(array, 0, i);
				i = input.Read(array, 0, count);
				if (output != null)
				{
					output.Write(array, 0, i);
				}
				this._TotalBytesRead += (long)i;
			}
			return (int)(~(int)this._register);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000C6D8 File Offset: 0x0000A8D8
		public int ComputeCrc32(int W, byte B)
		{
			return this._InternalComputeCrc32((uint)W, B);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000C6E2 File Offset: 0x0000A8E2
		internal int _InternalComputeCrc32(uint W, byte B)
		{
			return (int)(this.crc32Table[(int)((W ^ (uint)B) & 255U)] ^ W >> 8);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000C6F8 File Offset: 0x0000A8F8
		public void SlurpBlock(byte[] block, int offset, int count)
		{
			if (block == null)
			{
				throw new Exception("The data buffer must not be null.");
			}
			for (int i = 0; i < count; i++)
			{
				int num = offset + i;
				byte b = block[num];
				if (this.reverseBits)
				{
					uint num2 = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)num2]);
				}
				else
				{
					uint num3 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)num3]);
				}
			}
			this._TotalBytesRead += (long)count;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000C78C File Offset: 0x0000A98C
		public void UpdateCRC(byte b)
		{
			if (this.reverseBits)
			{
				uint num = this._register >> 24 ^ (uint)b;
				this._register = (this._register << 8 ^ this.crc32Table[(int)num]);
				return;
			}
			uint num2 = (this._register & 255U) ^ (uint)b;
			this._register = (this._register >> 8 ^ this.crc32Table[(int)num2]);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000C7EC File Offset: 0x0000A9EC
		public void UpdateCRC(byte b, int n)
		{
			while (n-- > 0)
			{
				if (this.reverseBits)
				{
					uint num = this._register >> 24 ^ (uint)b;
					this._register = (this._register << 8 ^ this.crc32Table[(int)((num >= 0U) ? num : (num + 256U))]);
				}
				else
				{
					uint num2 = (this._register & 255U) ^ (uint)b;
					this._register = (this._register >> 8 ^ this.crc32Table[(int)((num2 >= 0U) ? num2 : (num2 + 256U))]);
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000C874 File Offset: 0x0000AA74
		private static uint ReverseBits(uint data)
		{
			uint num = (data & 1431655765U) << 1 | (data >> 1 & 1431655765U);
			num = ((num & 858993459U) << 2 | (num >> 2 & 858993459U));
			num = ((num & 252645135U) << 4 | (num >> 4 & 252645135U));
			return num << 24 | (num & 65280U) << 8 | (num >> 8 & 65280U) | num >> 24;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		private static byte ReverseBits(byte data)
		{
			int num = (int)data * 131586;
			uint num2 = 17055760U;
			uint num3 = (uint)(num & (int)num2);
			uint num4 = (uint)(num << 2 & (int)((int)num2 << 1));
			return (byte)(16781313U * (num3 + num4) >> 24);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000C914 File Offset: 0x0000AB14
		private void GenerateLookupTable()
		{
			this.crc32Table = new uint[256];
			byte b = 0;
			do
			{
				uint num = (uint)b;
				for (byte b2 = 8; b2 > 0; b2 -= 1)
				{
					if ((num & 1U) == 1U)
					{
						num = (num >> 1 ^ this.dwPolynomial);
					}
					else
					{
						num >>= 1;
					}
				}
				if (this.reverseBits)
				{
					this.crc32Table[(int)CRC32.ReverseBits(b)] = CRC32.ReverseBits(num);
				}
				else
				{
					this.crc32Table[(int)b] = num;
				}
				b += 1;
			}
			while (b != 0);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000C988 File Offset: 0x0000AB88
		private uint gf2_matrix_times(uint[] matrix, uint vec)
		{
			uint num = 0U;
			int num2 = 0;
			while (vec != 0U)
			{
				if ((vec & 1U) == 1U)
				{
					num ^= matrix[num2];
				}
				vec >>= 1;
				num2++;
			}
			return num;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000C9B4 File Offset: 0x0000ABB4
		private void gf2_matrix_square(uint[] square, uint[] mat)
		{
			for (int i = 0; i < 32; i++)
			{
				square[i] = this.gf2_matrix_times(mat, mat[i]);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000C9DC File Offset: 0x0000ABDC
		public void Combine(int crc, int length)
		{
			uint[] array = new uint[32];
			uint[] array2 = new uint[32];
			if (length == 0)
			{
				return;
			}
			uint num = ~this._register;
			array2[0] = this.dwPolynomial;
			uint num2 = 1U;
			for (int i = 1; i < 32; i++)
			{
				array2[i] = num2;
				num2 <<= 1;
			}
			this.gf2_matrix_square(array, array2);
			this.gf2_matrix_square(array2, array);
			uint num3 = (uint)length;
			do
			{
				this.gf2_matrix_square(array, array2);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array, num);
				}
				num3 >>= 1;
				if (num3 == 0U)
				{
					break;
				}
				this.gf2_matrix_square(array2, array);
				if ((num3 & 1U) == 1U)
				{
					num = this.gf2_matrix_times(array2, num);
				}
				num3 >>= 1;
			}
			while (num3 != 0U);
			num ^= (uint)crc;
			this._register = ~num;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000CA93 File Offset: 0x0000AC93
		public CRC32() : this(false)
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		public CRC32(bool reverseBits) : this(-306674912, reverseBits)
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000CAAA File Offset: 0x0000ACAA
		public CRC32(int polynomial, bool reverseBits)
		{
			this.reverseBits = reverseBits;
			this.dwPolynomial = (uint)polynomial;
			this.GenerateLookupTable();
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000CACD File Offset: 0x0000ACCD
		public void Reset()
		{
			this._register = uint.MaxValue;
		}

		// Token: 0x0400016C RID: 364
		private uint dwPolynomial;

		// Token: 0x0400016D RID: 365
		private long _TotalBytesRead;

		// Token: 0x0400016E RID: 366
		private bool reverseBits;

		// Token: 0x0400016F RID: 367
		private uint[] crc32Table;

		// Token: 0x04000170 RID: 368
		private const int BUFFER_SIZE = 8192;

		// Token: 0x04000171 RID: 369
		private uint _register = uint.MaxValue;
	}
}
