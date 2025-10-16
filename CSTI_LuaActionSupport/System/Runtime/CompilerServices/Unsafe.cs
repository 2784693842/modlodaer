using System;
using System.Runtime.Versioning;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020000DA RID: 218
	internal static class Unsafe
	{
		// Token: 0x060007D5 RID: 2005 RVA: 0x000300D8 File Offset: 0x0002E2D8
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T Read<T>(void* source)
		{
			return *(T*)source;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000300EC File Offset: 0x0002E2EC
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static T ReadUnaligned<T>(void* source)
		{
			return *(T*)source;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00030104 File Offset: 0x0002E304
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T ReadUnaligned<T>(ref byte source)
		{
			return source;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0003011C File Offset: 0x0002E31C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Write<T>(void* destination, T value)
		{
			*(T*)destination = value;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00030130 File Offset: 0x0002E330
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteUnaligned<T>(void* destination, T value)
		{
			*(T*)destination = value;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00030148 File Offset: 0x0002E348
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUnaligned<T>(ref byte destination, T value)
		{
			destination = value;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00030160 File Offset: 0x0002E360
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Copy<T>(void* destination, ref T source)
		{
			*(T*)destination = source;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0003017C File Offset: 0x0002E37C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void Copy<T>(ref T destination, void* source)
		{
			destination = *(T*)source;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00030198 File Offset: 0x0002E398
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* AsPointer<T>(ref T value)
		{
			return (void*)(&value);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000301A8 File Offset: 0x0002E3A8
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SkipInit<T>(out T value)
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x000301B8 File Offset: 0x0002E3B8
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SizeOf<T>()
		{
			return sizeof(T);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000301CC File Offset: 0x0002E3CC
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyBlock(void* destination, void* source, uint byteCount)
		{
			cpblk(destination, source, byteCount);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000301E0 File Offset: 0x0002E3E0
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyBlock(ref byte destination, ref byte source, uint byteCount)
		{
			cpblk(ref destination, ref source, byteCount);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000301F4 File Offset: 0x0002E3F4
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
		{
			cpblk(destination, source, byteCount);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0003020C File Offset: 0x0002E40C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyBlockUnaligned(ref byte destination, ref byte source, uint byteCount)
		{
			cpblk(ref destination, ref source, byteCount);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00030224 File Offset: 0x0002E424
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void InitBlock(void* startAddress, byte value, uint byteCount)
		{
			initblk(startAddress, value, byteCount);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00030238 File Offset: 0x0002E438
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InitBlock(ref byte startAddress, byte value, uint byteCount)
		{
			initblk(ref startAddress, value, byteCount);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0003024C File Offset: 0x0002E44C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
		{
			initblk(startAddress, value, byteCount);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00030264 File Offset: 0x0002E464
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
		{
			initblk(ref startAddress, value, byteCount);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0003027C File Offset: 0x0002E47C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T As<T>(object o) where T : class
		{
			return o;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0003028C File Offset: 0x0002E48C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static ref T AsRef<T>(void* source)
		{
			return ref *(T*)source;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0003029C File Offset: 0x0002E49C
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T AsRef<T>(in T source)
		{
			return ref source;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000302AC File Offset: 0x0002E4AC
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref TTo As<TFrom, TTo>(ref TFrom source)
		{
			return ref source;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000302BC File Offset: 0x0002E4BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Unbox<T>(object box) where T : ValueType
		{
			return ref (T)box;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000302D0 File Offset: 0x0002E4D0
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Add<T>(ref T source, int elementOffset)
		{
			return ref source + (IntPtr)elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000302E8 File Offset: 0x0002E4E8
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* Add<T>(void* source, int elementOffset)
		{
			return (void*)((byte*)source + (IntPtr)elementOffset * (IntPtr)sizeof(T));
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00030300 File Offset: 0x0002E500
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Add<T>(ref T source, IntPtr elementOffset)
		{
			return ref source + elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00030318 File Offset: 0x0002E518
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
		{
			return ref source + byteOffset;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00030328 File Offset: 0x0002E528
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Subtract<T>(ref T source, int elementOffset)
		{
			return ref source - (IntPtr)elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00030340 File Offset: 0x0002E540
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void* Subtract<T>(void* source, int elementOffset)
		{
			return (void*)((byte*)source - (IntPtr)elementOffset * (IntPtr)sizeof(T));
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00030358 File Offset: 0x0002E558
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Subtract<T>(ref T source, IntPtr elementOffset)
		{
			return ref source - elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00030370 File Offset: 0x0002E570
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T SubtractByteOffset<T>(ref T source, IntPtr byteOffset)
		{
			return ref source - byteOffset;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00030380 File Offset: 0x0002E580
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IntPtr ByteOffset<T>(ref T origin, ref T target)
		{
			return ref target - ref origin;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00030390 File Offset: 0x0002E590
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool AreSame<T>(ref T left, ref T right)
		{
			return ref left == ref right;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000303A4 File Offset: 0x0002E5A4
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAddressGreaterThan<T>(ref T left, ref T right)
		{
			return ref left != ref right;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000303B8 File Offset: 0x0002E5B8
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAddressLessThan<T>(ref T left, ref T right)
		{
			return ref left < ref right;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000303CC File Offset: 0x0002E5CC
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNullRef<T>(ref T source)
		{
			return ref source == (UIntPtr)0;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000303E0 File Offset: 0x0002E5E0
		[NonVersionable]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T NullRef<T>()
		{
			return (UIntPtr)0;
		}
	}
}
