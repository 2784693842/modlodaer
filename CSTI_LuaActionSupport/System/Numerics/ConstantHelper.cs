using System;
using System.Runtime.CompilerServices;

namespace System.Numerics
{
	// Token: 0x020000D3 RID: 211
	internal class ConstantHelper
	{
		// Token: 0x060006FB RID: 1787 RVA: 0x0002D368 File Offset: 0x0002B568
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static byte GetByteWithAllBitsSet()
		{
			byte result = 0;
			*(&result) = byte.MaxValue;
			return result;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002D384 File Offset: 0x0002B584
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static sbyte GetSByteWithAllBitsSet()
		{
			sbyte result = 0;
			*(&result) = -1;
			return result;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002D39C File Offset: 0x0002B59C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ushort GetUInt16WithAllBitsSet()
		{
			ushort result = 0;
			*(&result) = ushort.MaxValue;
			return result;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002D3B8 File Offset: 0x0002B5B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static short GetInt16WithAllBitsSet()
		{
			short result = 0;
			*(&result) = -1;
			return result;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002D3D0 File Offset: 0x0002B5D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static uint GetUInt32WithAllBitsSet()
		{
			uint result = 0U;
			*(&result) = uint.MaxValue;
			return result;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002D3E8 File Offset: 0x0002B5E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int GetInt32WithAllBitsSet()
		{
			int result = 0;
			*(&result) = -1;
			return result;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002D400 File Offset: 0x0002B600
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ulong GetUInt64WithAllBitsSet()
		{
			ulong result = 0UL;
			*(&result) = ulong.MaxValue;
			return result;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002D418 File Offset: 0x0002B618
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static long GetInt64WithAllBitsSet()
		{
			long result = 0L;
			*(&result) = -1L;
			return result;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002D430 File Offset: 0x0002B630
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float GetSingleWithAllBitsSet()
		{
			float result = 0f;
			*(int*)(&result) = -1;
			return result;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002D44C File Offset: 0x0002B64C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static double GetDoubleWithAllBitsSet()
		{
			double result = 0.0;
			*(long*)(&result) = -1L;
			return result;
		}
	}
}
