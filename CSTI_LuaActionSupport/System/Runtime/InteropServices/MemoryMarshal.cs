using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	// Token: 0x020000D8 RID: 216
	internal static class MemoryMarshal
	{
		// Token: 0x060007BC RID: 1980 RVA: 0x0002FB00 File Offset: 0x0002DD00
		public static bool TryGetArray<T>(System.ReadOnlyMemory<T> memory, out ArraySegment<T> segment)
		{
			int num;
			int num2;
			object objectStartLength = memory.GetObjectStartLength(out num, out num2);
			T[] array;
			if (num < 0)
			{
				ArraySegment<T> arraySegment;
				if (((MemoryManager<T>)objectStartLength).TryGetArray(out arraySegment))
				{
					segment = new ArraySegment<T>(arraySegment.Array, arraySegment.Offset + (num & int.MaxValue), num2);
					return true;
				}
			}
			else if ((array = (objectStartLength as T[])) != null)
			{
				segment = new ArraySegment<T>(array, num, num2 & int.MaxValue);
				return true;
			}
			if ((num2 & 2147483647) == 0)
			{
				segment = new ArraySegment<T>(SpanHelpers.PerTypeValues<T>.EmptyArray);
				return true;
			}
			segment = default(ArraySegment<T>);
			return false;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0002FB98 File Offset: 0x0002DD98
		public static bool TryGetMemoryManager<T, TManager>(System.ReadOnlyMemory<T> memory, out TManager manager) where TManager : MemoryManager<T>
		{
			int num;
			int num2;
			manager = (memory.GetObjectStartLength(out num, out num2) as TManager);
			return manager != null;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002FBD0 File Offset: 0x0002DDD0
		public static bool TryGetMemoryManager<T, TManager>(System.ReadOnlyMemory<T> memory, out TManager manager, out int start, out int length) where TManager : MemoryManager<T>
		{
			manager = (memory.GetObjectStartLength(out start, out length) as TManager);
			start &= int.MaxValue;
			if (manager == null)
			{
				start = 0;
				length = 0;
				return false;
			}
			return true;
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002FC18 File Offset: 0x0002DE18
		public unsafe static IEnumerable<T> ToEnumerable<T>(System.ReadOnlyMemory<T> memory)
		{
			int num;
			for (int i = 0; i < memory.Length; i = num + 1)
			{
				yield return *memory.Span[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0002FC28 File Offset: 0x0002DE28
		public static bool TryGetString(System.ReadOnlyMemory<char> memory, out string text, out int start, out int length)
		{
			int num;
			int num2;
			string text2;
			if ((text2 = (memory.GetObjectStartLength(out num, out num2) as string)) != null)
			{
				text = text2;
				start = num;
				length = num2;
				return true;
			}
			text = null;
			start = 0;
			length = 0;
			return false;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0002FC5E File Offset: 0x0002DE5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Read<T>(System.ReadOnlySpan<byte> source) where T : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
			}
			if (Unsafe.SizeOf<T>() > source.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
			}
			return Unsafe.ReadUnaligned<T>(MemoryMarshal.GetReference<byte>(source));
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0002FC98 File Offset: 0x0002DE98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryRead<T>(System.ReadOnlySpan<byte> source, out T value) where T : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
			}
			if ((long)Unsafe.SizeOf<T>() > (long)((ulong)source.Length))
			{
				value = default(T);
				return false;
			}
			value = Unsafe.ReadUnaligned<T>(MemoryMarshal.GetReference<byte>(source));
			return true;
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002FCE6 File Offset: 0x0002DEE6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Write<T>(System.Span<byte> destination, ref T value) where T : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
			}
			if (Unsafe.SizeOf<T>() > destination.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
			}
			Unsafe.WriteUnaligned<T>(MemoryMarshal.GetReference<byte>(destination), value);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002FD23 File Offset: 0x0002DF23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryWrite<T>(System.Span<byte> destination, ref T value) where T : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
			}
			if ((long)Unsafe.SizeOf<T>() > (long)((ulong)destination.Length))
			{
				return false;
			}
			Unsafe.WriteUnaligned<T>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0002FD60 File Offset: 0x0002DF60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Memory<T> CreateFromPinnedArray<T>(T[] array, int start, int length)
		{
			if (array == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException();
				}
				return default(System.Memory<T>);
			}
			if (default(T) == null && array.GetType() != typeof(T[]))
			{
				ThrowHelper.ThrowArrayTypeMismatchException();
			}
			if (start > array.Length || length > array.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException();
			}
			return new System.Memory<T>(array, start, length | int.MinValue);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002FDD4 File Offset: 0x0002DFD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Span<byte> AsBytes<T>(System.Span<T> span) where T : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
			}
			int length = checked(span.Length * Unsafe.SizeOf<T>());
			return new System.Span<byte>(Unsafe.As<Pinnable<byte>>(span.Pinnable), span.ByteOffset, length);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002FE20 File Offset: 0x0002E020
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.ReadOnlySpan<byte> AsBytes<T>(System.ReadOnlySpan<T> span) where T : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<T>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(T));
			}
			int length = checked(span.Length * Unsafe.SizeOf<T>());
			return new System.ReadOnlySpan<byte>(Unsafe.As<Pinnable<byte>>(span.Pinnable), span.ByteOffset, length);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002FE6A File Offset: 0x0002E06A
		public unsafe static System.Memory<T> AsMemory<T>(System.ReadOnlyMemory<T> memory)
		{
			return *Unsafe.As<System.ReadOnlyMemory<T>, System.Memory<T>>(ref memory);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0002FE78 File Offset: 0x0002E078
		public static ref T GetReference<T>(System.Span<T> span)
		{
			if (span.Pinnable == null)
			{
				return Unsafe.AsRef<T>(span.ByteOffset.ToPointer());
			}
			return Unsafe.AddByteOffset<T>(ref span.Pinnable.Data, span.ByteOffset);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002FEBC File Offset: 0x0002E0BC
		public static ref T GetReference<T>(System.ReadOnlySpan<T> span)
		{
			if (span.Pinnable == null)
			{
				return Unsafe.AsRef<T>(span.ByteOffset.ToPointer());
			}
			return Unsafe.AddByteOffset<T>(ref span.Pinnable.Data, span.ByteOffset);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002FF00 File Offset: 0x0002E100
		public static System.Span<TTo> Cast<TFrom, TTo>(System.Span<TFrom> span) where TFrom : struct where TTo : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<TFrom>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TFrom));
			}
			if (SpanHelpers.IsReferenceOrContainsReferences<TTo>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TTo));
			}
			int length = checked((int)(unchecked((long)span.Length) * unchecked((long)Unsafe.SizeOf<TFrom>()) / unchecked((long)Unsafe.SizeOf<TTo>())));
			return new System.Span<TTo>(Unsafe.As<Pinnable<TTo>>(span.Pinnable), span.ByteOffset, length);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002FF6C File Offset: 0x0002E16C
		public static System.ReadOnlySpan<TTo> Cast<TFrom, TTo>(System.ReadOnlySpan<TFrom> span) where TFrom : struct where TTo : struct
		{
			if (SpanHelpers.IsReferenceOrContainsReferences<TFrom>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TFrom));
			}
			if (SpanHelpers.IsReferenceOrContainsReferences<TTo>())
			{
				ThrowHelper.ThrowArgumentException_InvalidTypeWithPointersNotSupported(typeof(TTo));
			}
			int length = checked((int)(unchecked((long)span.Length) * unchecked((long)Unsafe.SizeOf<TFrom>()) / unchecked((long)Unsafe.SizeOf<TTo>())));
			return new System.ReadOnlySpan<TTo>(Unsafe.As<Pinnable<TTo>>(span.Pinnable), span.ByteOffset, length);
		}
	}
}
