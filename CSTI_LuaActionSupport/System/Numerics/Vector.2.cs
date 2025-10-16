using System;
using System.Runtime.CompilerServices;

namespace System.Numerics
{
	// Token: 0x020000D4 RID: 212
	[Intrinsic]
	internal static class Vector
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x0002D46C File Offset: 0x0002B66C
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static void Widen(Vector<byte> source, out Vector<ushort> low, out Vector<ushort> high)
		{
			int count = Vector<byte>.Count;
			ushort* ptr = stackalloc ushort[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ushort)source[i];
			}
			ushort* ptr2 = stackalloc ushort[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (ushort)source[j + count / 2];
			}
			low = new Vector<ushort>((void*)ptr);
			high = new Vector<ushort>((void*)ptr2);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002D4F0 File Offset: 0x0002B6F0
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static void Widen(Vector<ushort> source, out Vector<uint> low, out Vector<uint> high)
		{
			int count = Vector<ushort>.Count;
			uint* ptr = stackalloc uint[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (uint)source[i];
			}
			uint* ptr2 = stackalloc uint[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (uint)source[j + count / 2];
			}
			low = new Vector<uint>((void*)ptr);
			high = new Vector<uint>((void*)ptr2);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002D574 File Offset: 0x0002B774
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static void Widen(Vector<uint> source, out Vector<ulong> low, out Vector<ulong> high)
		{
			int count = Vector<uint>.Count;
			ulong* ptr = stackalloc ulong[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ulong)source[i];
			}
			ulong* ptr2 = stackalloc ulong[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (ulong)source[j + count / 2];
			}
			low = new Vector<ulong>((void*)ptr);
			high = new Vector<ulong>((void*)ptr2);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002D5F8 File Offset: 0x0002B7F8
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static void Widen(Vector<sbyte> source, out Vector<short> low, out Vector<short> high)
		{
			int count = Vector<sbyte>.Count;
			short* ptr = stackalloc short[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (short)source[i];
			}
			short* ptr2 = stackalloc short[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (short)source[j + count / 2];
			}
			low = new Vector<short>((void*)ptr);
			high = new Vector<short>((void*)ptr2);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002D67C File Offset: 0x0002B87C
		[Intrinsic]
		public unsafe static void Widen(Vector<short> source, out Vector<int> low, out Vector<int> high)
		{
			int count = Vector<short>.Count;
			int* ptr = stackalloc int[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (int)source[i];
			}
			int* ptr2 = stackalloc int[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (int)source[j + count / 2];
			}
			low = new Vector<int>((void*)ptr);
			high = new Vector<int>((void*)ptr2);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002D700 File Offset: 0x0002B900
		[Intrinsic]
		public unsafe static void Widen(Vector<int> source, out Vector<long> low, out Vector<long> high)
		{
			int count = Vector<int>.Count;
			long* ptr = stackalloc long[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (long)source[i];
			}
			long* ptr2 = stackalloc long[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (long)source[j + count / 2];
			}
			low = new Vector<long>((void*)ptr);
			high = new Vector<long>((void*)ptr2);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002D784 File Offset: 0x0002B984
		[Intrinsic]
		public unsafe static void Widen(Vector<float> source, out Vector<double> low, out Vector<double> high)
		{
			int count = Vector<float>.Count;
			double* ptr = stackalloc double[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (double)source[i];
			}
			double* ptr2 = stackalloc double[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (double)source[j + count / 2];
			}
			low = new Vector<double>((void*)ptr);
			high = new Vector<double>((void*)ptr2);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002D808 File Offset: 0x0002BA08
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<byte> Narrow(Vector<ushort> low, Vector<ushort> high)
		{
			int count = Vector<byte>.Count;
			byte* ptr = stackalloc byte[(UIntPtr)count];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (byte)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (byte)high[j];
			}
			return new Vector<byte>((void*)ptr);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002D860 File Offset: 0x0002BA60
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<ushort> Narrow(Vector<uint> low, Vector<uint> high)
		{
			int count = Vector<ushort>.Count;
			ushort* ptr = stackalloc ushort[checked(unchecked((UIntPtr)count) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ushort)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (ushort)high[j];
			}
			return new Vector<ushort>((void*)ptr);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002D8C0 File Offset: 0x0002BAC0
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<uint> Narrow(Vector<ulong> low, Vector<ulong> high)
		{
			int count = Vector<uint>.Count;
			uint* ptr = stackalloc uint[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (uint)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (uint)high[j];
			}
			return new Vector<uint>((void*)ptr);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002D920 File Offset: 0x0002BB20
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<sbyte> Narrow(Vector<short> low, Vector<short> high)
		{
			int count = Vector<sbyte>.Count;
			sbyte* ptr = stackalloc sbyte[(UIntPtr)count];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (sbyte)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (sbyte)high[j];
			}
			return new Vector<sbyte>((void*)ptr);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002D978 File Offset: 0x0002BB78
		[Intrinsic]
		public unsafe static Vector<short> Narrow(Vector<int> low, Vector<int> high)
		{
			int count = Vector<short>.Count;
			short* ptr = stackalloc short[checked(unchecked((UIntPtr)count) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (short)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (short)high[j];
			}
			return new Vector<short>((void*)ptr);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002D9D8 File Offset: 0x0002BBD8
		[Intrinsic]
		public unsafe static Vector<int> Narrow(Vector<long> low, Vector<long> high)
		{
			int count = Vector<int>.Count;
			int* ptr = stackalloc int[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (int)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (int)high[j];
			}
			return new Vector<int>((void*)ptr);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0002DA38 File Offset: 0x0002BC38
		[Intrinsic]
		public unsafe static Vector<float> Narrow(Vector<double> low, Vector<double> high)
		{
			int count = Vector<float>.Count;
			float* ptr = stackalloc float[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (float)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (float)high[j];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0002DA98 File Offset: 0x0002BC98
		[Intrinsic]
		public unsafe static Vector<float> ConvertToSingle(Vector<int> value)
		{
			int count = Vector<float>.Count;
			float* ptr = stackalloc float[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (float)value[i];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002DAD4 File Offset: 0x0002BCD4
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<float> ConvertToSingle(Vector<uint> value)
		{
			int count = Vector<float>.Count;
			float* ptr = stackalloc float[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = value[i];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0002DB14 File Offset: 0x0002BD14
		[Intrinsic]
		public unsafe static Vector<double> ConvertToDouble(Vector<long> value)
		{
			int count = Vector<double>.Count;
			double* ptr = stackalloc double[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (double)value[i];
			}
			return new Vector<double>((void*)ptr);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002DB50 File Offset: 0x0002BD50
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<double> ConvertToDouble(Vector<ulong> value)
		{
			int count = Vector<double>.Count;
			double* ptr = stackalloc double[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = value[i];
			}
			return new Vector<double>((void*)ptr);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002DB90 File Offset: 0x0002BD90
		[Intrinsic]
		public unsafe static Vector<int> ConvertToInt32(Vector<float> value)
		{
			int count = Vector<int>.Count;
			int* ptr = stackalloc int[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (int)value[i];
			}
			return new Vector<int>((void*)ptr);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0002DBCC File Offset: 0x0002BDCC
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<uint> ConvertToUInt32(Vector<float> value)
		{
			int count = Vector<uint>.Count;
			uint* ptr = stackalloc uint[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (uint)value[i];
			}
			return new Vector<uint>((void*)ptr);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0002DC08 File Offset: 0x0002BE08
		[Intrinsic]
		public unsafe static Vector<long> ConvertToInt64(Vector<double> value)
		{
			int count = Vector<long>.Count;
			long* ptr = stackalloc long[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (long)value[i];
			}
			return new Vector<long>((void*)ptr);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002DC44 File Offset: 0x0002BE44
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<ulong> ConvertToUInt64(Vector<double> value)
		{
			int count = Vector<ulong>.Count;
			ulong* ptr = stackalloc ulong[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (ulong)value[i];
			}
			return new Vector<ulong>((void*)ptr);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002DC80 File Offset: 0x0002BE80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<float> ConditionalSelect(Vector<int> condition, Vector<float> left, Vector<float> right)
		{
			return Vector<float>.ConditionalSelect((Vector<float>)condition, left, right);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0002DC8F File Offset: 0x0002BE8F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<double> ConditionalSelect(Vector<long> condition, Vector<double> left, Vector<double> right)
		{
			return Vector<double>.ConditionalSelect((Vector<double>)condition, left, right);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002DC9E File Offset: 0x0002BE9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> ConditionalSelect<T>(Vector<T> condition, Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.ConditionalSelect(condition, left, right);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002DCA8 File Offset: 0x0002BEA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Equals<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Equals(left, right);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002DCB1 File Offset: 0x0002BEB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> Equals(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.Equals(left, right);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002DCBF File Offset: 0x0002BEBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> Equals(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.Equals(left, right);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002DCC8 File Offset: 0x0002BEC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> Equals(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.Equals(left, right);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002DCD6 File Offset: 0x0002BED6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> Equals(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.Equals(left, right);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0002DCDF File Offset: 0x0002BEDF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left == right;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0002DCE8 File Offset: 0x0002BEE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !Vector<T>.Equals(left, right).Equals(Vector<T>.Zero);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0002DD0C File Offset: 0x0002BF0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> LessThan<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.LessThan(left, right);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0002DD15 File Offset: 0x0002BF15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThan(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.LessThan(left, right);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002DD23 File Offset: 0x0002BF23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThan(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.LessThan(left, right);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0002DD2C File Offset: 0x0002BF2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThan(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.LessThan(left, right);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0002DD3A File Offset: 0x0002BF3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThan(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.LessThan(left, right);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0002DD44 File Offset: 0x0002BF44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.LessThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0002DD6C File Offset: 0x0002BF6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.LessThan(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0002DD95 File Offset: 0x0002BF95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> LessThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.LessThanOrEqual(left, right);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0002DD9E File Offset: 0x0002BF9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThanOrEqual(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.LessThanOrEqual(left, right);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0002DDAC File Offset: 0x0002BFAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThanOrEqual(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.LessThanOrEqual(left, right);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0002DDB5 File Offset: 0x0002BFB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThanOrEqual(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.LessThanOrEqual(left, right);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0002DDBE File Offset: 0x0002BFBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThanOrEqual(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.LessThanOrEqual(left, right);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0002DDCC File Offset: 0x0002BFCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0002DDF4 File Offset: 0x0002BFF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0002DE1D File Offset: 0x0002C01D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> GreaterThan<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.GreaterThan(left, right);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002DE26 File Offset: 0x0002C026
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThan(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.GreaterThan(left, right);
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002DE34 File Offset: 0x0002C034
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThan(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.GreaterThan(left, right);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0002DE3D File Offset: 0x0002C03D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThan(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.GreaterThan(left, right);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002DE4B File Offset: 0x0002C04B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThan(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.GreaterThan(left, right);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002DE54 File Offset: 0x0002C054
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002DE7C File Offset: 0x0002C07C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0002DEA5 File Offset: 0x0002C0A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> GreaterThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002DEAE File Offset: 0x0002C0AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThanOrEqual(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0002DEBC File Offset: 0x0002C0BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThanOrEqual(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002DEC5 File Offset: 0x0002C0C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThanOrEqual(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002DECE File Offset: 0x0002C0CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThanOrEqual(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0002DEDC File Offset: 0x0002C0DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0002DF04 File Offset: 0x0002C104
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00017134 File Offset: 0x00015334
		public static bool IsHardwareAccelerated
		{
			[Intrinsic]
			get
			{
				return false;
			}
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002DF2D File Offset: 0x0002C12D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Abs<T>(Vector<T> value) where T : struct
		{
			return Vector<T>.Abs(value);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0002DF35 File Offset: 0x0002C135
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Min<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Min(left, right);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002DF3E File Offset: 0x0002C13E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Max<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Max(left, right);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0002DF47 File Offset: 0x0002C147
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Dot<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.DotProduct(left, right);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0002DF50 File Offset: 0x0002C150
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> SquareRoot<T>(Vector<T> value) where T : struct
		{
			return Vector<T>.SquareRoot(value);
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0002DF58 File Offset: 0x0002C158
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Add<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left + right;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002DF61 File Offset: 0x0002C161
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Subtract<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left - right;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002DF6A File Offset: 0x0002C16A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left * right;
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002DF73 File Offset: 0x0002C173
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(Vector<T> left, T right) where T : struct
		{
			return left * right;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002DF7C File Offset: 0x0002C17C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(T left, Vector<T> right) where T : struct
		{
			return left * right;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0002DF85 File Offset: 0x0002C185
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Divide<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left / right;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002DF8E File Offset: 0x0002C18E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Negate<T>(Vector<T> value) where T : struct
		{
			return -value;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002DF96 File Offset: 0x0002C196
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> BitwiseAnd<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left & right;
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002DF9F File Offset: 0x0002C19F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> BitwiseOr<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left | right;
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002DFA8 File Offset: 0x0002C1A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> OnesComplement<T>(Vector<T> value) where T : struct
		{
			return ~value;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0002DFB0 File Offset: 0x0002C1B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Xor<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left ^ right;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002DFB9 File Offset: 0x0002C1B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> AndNot<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left & ~right;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0002DFC7 File Offset: 0x0002C1C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<byte> AsVectorByte<T>(Vector<T> value) where T : struct
		{
			return (Vector<byte>)value;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002DFCF File Offset: 0x0002C1CF
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<sbyte> AsVectorSByte<T>(Vector<T> value) where T : struct
		{
			return (Vector<sbyte>)value;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002DFD7 File Offset: 0x0002C1D7
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<ushort> AsVectorUInt16<T>(Vector<T> value) where T : struct
		{
			return (Vector<ushort>)value;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002DFDF File Offset: 0x0002C1DF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<short> AsVectorInt16<T>(Vector<T> value) where T : struct
		{
			return (Vector<short>)value;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002DFE7 File Offset: 0x0002C1E7
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<uint> AsVectorUInt32<T>(Vector<T> value) where T : struct
		{
			return (Vector<uint>)value;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0002DFEF File Offset: 0x0002C1EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> AsVectorInt32<T>(Vector<T> value) where T : struct
		{
			return (Vector<int>)value;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0002DFF7 File Offset: 0x0002C1F7
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<ulong> AsVectorUInt64<T>(Vector<T> value) where T : struct
		{
			return (Vector<ulong>)value;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0002DFFF File Offset: 0x0002C1FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> AsVectorInt64<T>(Vector<T> value) where T : struct
		{
			return (Vector<long>)value;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0002E007 File Offset: 0x0002C207
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<float> AsVectorSingle<T>(Vector<T> value) where T : struct
		{
			return (Vector<float>)value;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0002E00F File Offset: 0x0002C20F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<double> AsVectorDouble<T>(Vector<T> value) where T : struct
		{
			return (Vector<double>)value;
		}
	}
}
