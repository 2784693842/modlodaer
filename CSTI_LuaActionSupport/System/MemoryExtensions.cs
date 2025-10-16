using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000D6 RID: 214
	internal static class MemoryExtensions
	{
		// Token: 0x06000760 RID: 1888 RVA: 0x0002E059 File Offset: 0x0002C259
		public static System.ReadOnlySpan<char> Trim(this System.ReadOnlySpan<char> span)
		{
			return span.TrimStart().TrimEnd();
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002E068 File Offset: 0x0002C268
		public unsafe static System.ReadOnlySpan<char> TrimStart(this System.ReadOnlySpan<char> span)
		{
			int num = 0;
			while (num < span.Length && char.IsWhiteSpace((char)(*span[num])))
			{
				num++;
			}
			return span.Slice(num);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002E0A0 File Offset: 0x0002C2A0
		public unsafe static System.ReadOnlySpan<char> TrimEnd(this System.ReadOnlySpan<char> span)
		{
			int num = span.Length - 1;
			while (num >= 0 && char.IsWhiteSpace((char)(*span[num])))
			{
				num--;
			}
			return span.Slice(0, num + 1);
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002E0DC File Offset: 0x0002C2DC
		public static System.ReadOnlySpan<char> Trim(this System.ReadOnlySpan<char> span, char trimChar)
		{
			return span.TrimStart(trimChar).TrimEnd(trimChar);
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0002E0EC File Offset: 0x0002C2EC
		public unsafe static System.ReadOnlySpan<char> TrimStart(this System.ReadOnlySpan<char> span, char trimChar)
		{
			int num = 0;
			while (num < span.Length && *span[num] == (ushort)trimChar)
			{
				num++;
			}
			return span.Slice(num);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0002E120 File Offset: 0x0002C320
		public unsafe static System.ReadOnlySpan<char> TrimEnd(this System.ReadOnlySpan<char> span, char trimChar)
		{
			int num = span.Length - 1;
			while (num >= 0 && *span[num] == (ushort)trimChar)
			{
				num--;
			}
			return span.Slice(0, num + 1);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0002E158 File Offset: 0x0002C358
		public static System.ReadOnlySpan<char> Trim(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> trimChars)
		{
			return span.TrimStart(trimChars).TrimEnd(trimChars);
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002E168 File Offset: 0x0002C368
		public unsafe static System.ReadOnlySpan<char> TrimStart(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> trimChars)
		{
			if (trimChars.IsEmpty)
			{
				return span.TrimStart();
			}
			int i = 0;
			IL_40:
			while (i < span.Length)
			{
				for (int j = 0; j < trimChars.Length; j++)
				{
					if (*span[i] == *trimChars[j])
					{
						i++;
						goto IL_40;
					}
				}
				break;
			}
			return span.Slice(i);
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002E1C8 File Offset: 0x0002C3C8
		public unsafe static System.ReadOnlySpan<char> TrimEnd(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> trimChars)
		{
			if (trimChars.IsEmpty)
			{
				return span.TrimEnd();
			}
			int i = span.Length - 1;
			IL_48:
			while (i >= 0)
			{
				for (int j = 0; j < trimChars.Length; j++)
				{
					if (*span[i] == *trimChars[j])
					{
						i--;
						goto IL_48;
					}
				}
				break;
			}
			return span.Slice(0, i + 1);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002E22C File Offset: 0x0002C42C
		public unsafe static bool IsWhiteSpace(this System.ReadOnlySpan<char> span)
		{
			for (int i = 0; i < span.Length; i++)
			{
				if (!char.IsWhiteSpace((char)(*span[i])))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002E260 File Offset: 0x0002C460
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int IndexOf<T>(this System.Span<T> span, T value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value), span.Length);
			}
			if (typeof(T) == typeof(char))
			{
				return SpanHelpers.IndexOf(Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, char>(ref value), span.Length);
			}
			return SpanHelpers.IndexOf<T>(MemoryMarshal.GetReference<T>(span), value, span.Length);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002E2F8 File Offset: 0x0002C4F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOf<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), value.Length);
			}
			return SpanHelpers.IndexOf<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(value), value.Length);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0002E36C File Offset: 0x0002C56C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int LastIndexOf<T>(this System.Span<T> span, T value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value), span.Length);
			}
			if (typeof(T) == typeof(char))
			{
				return SpanHelpers.LastIndexOf(Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, char>(ref value), span.Length);
			}
			return SpanHelpers.LastIndexOf<T>(MemoryMarshal.GetReference<T>(span), value, span.Length);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002E404 File Offset: 0x0002C604
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int LastIndexOf<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), value.Length);
			}
			return SpanHelpers.LastIndexOf<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(value), value.Length);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002E478 File Offset: 0x0002C678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool SequenceEqual<T>(this System.Span<T> span, System.ReadOnlySpan<T> other) where T : IEquatable<T>
		{
			int length = span.Length;
			NUInt right;
			if (default(T) != null && MemoryExtensions.IsTypeComparableAsBytes<T>(out right))
			{
				return length == other.Length && SpanHelpers.SequenceEqual(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(other)), (NUInt)length * right);
			}
			return length == other.Length && SpanHelpers.SequenceEqual<T>(MemoryMarshal.GetReference<T>(span), MemoryMarshal.GetReference<T>(other), length);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002E4F8 File Offset: 0x0002C6F8
		public static int SequenceCompareTo<T>(this System.Span<T> span, System.ReadOnlySpan<T> other) where T : IComparable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.SequenceCompareTo(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(other)), other.Length);
			}
			if (typeof(T) == typeof(char))
			{
				return SpanHelpers.SequenceCompareTo(Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(other)), other.Length);
			}
			return SpanHelpers.SequenceCompareTo<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(other), other.Length);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002E5B0 File Offset: 0x0002C7B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int IndexOf<T>(this System.ReadOnlySpan<T> span, T value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value), span.Length);
			}
			if (typeof(T) == typeof(char))
			{
				return SpanHelpers.IndexOf(Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, char>(ref value), span.Length);
			}
			return SpanHelpers.IndexOf<T>(MemoryMarshal.GetReference<T>(span), value, span.Length);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002E648 File Offset: 0x0002C848
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOf<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), value.Length);
			}
			return SpanHelpers.IndexOf<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(value), value.Length);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002E6BC File Offset: 0x0002C8BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int LastIndexOf<T>(this System.ReadOnlySpan<T> span, T value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value), span.Length);
			}
			if (typeof(T) == typeof(char))
			{
				return SpanHelpers.LastIndexOf(Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, char>(ref value), span.Length);
			}
			return SpanHelpers.LastIndexOf<T>(MemoryMarshal.GetReference<T>(span), value, span.Length);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002E754 File Offset: 0x0002C954
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int LastIndexOf<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOf(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), value.Length);
			}
			return SpanHelpers.LastIndexOf<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(value), value.Length);
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002E7C8 File Offset: 0x0002C9C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int IndexOfAny<T>(this System.Span<T> span, T value0, T value1) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), span.Length);
			}
			return SpanHelpers.IndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, span.Length);
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002E82C File Offset: 0x0002CA2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int IndexOfAny<T>(this System.Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), *Unsafe.As<T, byte>(ref value2), span.Length);
			}
			return SpanHelpers.IndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, value2, span.Length);
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0002E89C File Offset: 0x0002CA9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOfAny<T>(this System.Span<T> span, System.ReadOnlySpan<T> values) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(values)), values.Length);
			}
			return SpanHelpers.IndexOfAny<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(values), values.Length);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0002E910 File Offset: 0x0002CB10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int IndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), span.Length);
			}
			return SpanHelpers.IndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, span.Length);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002E974 File Offset: 0x0002CB74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int IndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), *Unsafe.As<T, byte>(ref value2), span.Length);
			}
			return SpanHelpers.IndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, value2, span.Length);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002E9E4 File Offset: 0x0002CBE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOfAny<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> values) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.IndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(values)), values.Length);
			}
			return SpanHelpers.IndexOfAny<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(values), values.Length);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002EA58 File Offset: 0x0002CC58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int LastIndexOfAny<T>(this System.Span<T> span, T value0, T value1) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), span.Length);
			}
			return SpanHelpers.LastIndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, span.Length);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002EABC File Offset: 0x0002CCBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int LastIndexOfAny<T>(this System.Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), *Unsafe.As<T, byte>(ref value2), span.Length);
			}
			return SpanHelpers.LastIndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, value2, span.Length);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002EB2C File Offset: 0x0002CD2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int LastIndexOfAny<T>(this System.Span<T> span, System.ReadOnlySpan<T> values) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(values)), values.Length);
			}
			return SpanHelpers.LastIndexOfAny<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(values), values.Length);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0002EBA0 File Offset: 0x0002CDA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int LastIndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), span.Length);
			}
			return SpanHelpers.LastIndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, span.Length);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002EC04 File Offset: 0x0002CE04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int LastIndexOfAny<T>(this System.ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), *Unsafe.As<T, byte>(ref value0), *Unsafe.As<T, byte>(ref value1), *Unsafe.As<T, byte>(ref value2), span.Length);
			}
			return SpanHelpers.LastIndexOfAny<T>(MemoryMarshal.GetReference<T>(span), value0, value1, value2, span.Length);
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002EC74 File Offset: 0x0002CE74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int LastIndexOfAny<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> values) where T : IEquatable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.LastIndexOfAny(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(values)), values.Length);
			}
			return SpanHelpers.LastIndexOfAny<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(values), values.Length);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002ECE8 File Offset: 0x0002CEE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool SequenceEqual<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other) where T : IEquatable<T>
		{
			int length = span.Length;
			NUInt right;
			if (default(T) != null && MemoryExtensions.IsTypeComparableAsBytes<T>(out right))
			{
				return length == other.Length && SpanHelpers.SequenceEqual(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(other)), (NUInt)length * right);
			}
			return length == other.Length && SpanHelpers.SequenceEqual<T>(MemoryMarshal.GetReference<T>(span), MemoryMarshal.GetReference<T>(other), length);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002ED68 File Offset: 0x0002CF68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int SequenceCompareTo<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other) where T : IComparable<T>
		{
			if (typeof(T) == typeof(byte))
			{
				return SpanHelpers.SequenceCompareTo(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(other)), other.Length);
			}
			if (typeof(T) == typeof(char))
			{
				return SpanHelpers.SequenceCompareTo(Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(span)), span.Length, Unsafe.As<T, char>(MemoryMarshal.GetReference<T>(other)), other.Length);
			}
			return SpanHelpers.SequenceCompareTo<T>(MemoryMarshal.GetReference<T>(span), span.Length, MemoryMarshal.GetReference<T>(other), other.Length);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002EE20 File Offset: 0x0002D020
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool StartsWith<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			int length = value.Length;
			NUInt right;
			if (default(T) != null && MemoryExtensions.IsTypeComparableAsBytes<T>(out right))
			{
				return length <= span.Length && SpanHelpers.SequenceEqual(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), (NUInt)length * right);
			}
			return length <= span.Length && SpanHelpers.SequenceEqual<T>(MemoryMarshal.GetReference<T>(span), MemoryMarshal.GetReference<T>(value), length);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002EEA0 File Offset: 0x0002D0A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool StartsWith<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			int length = value.Length;
			NUInt right;
			if (default(T) != null && MemoryExtensions.IsTypeComparableAsBytes<T>(out right))
			{
				return length <= span.Length && SpanHelpers.SequenceEqual(Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(span)), Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), (NUInt)length * right);
			}
			return length <= span.Length && SpanHelpers.SequenceEqual<T>(MemoryMarshal.GetReference<T>(span), MemoryMarshal.GetReference<T>(value), length);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002EF20 File Offset: 0x0002D120
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EndsWith<T>(this System.Span<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			int length = span.Length;
			int length2 = value.Length;
			NUInt right;
			if (default(T) != null && MemoryExtensions.IsTypeComparableAsBytes<T>(out right))
			{
				return length2 <= length && SpanHelpers.SequenceEqual(Unsafe.As<T, byte>(Unsafe.Add<T>(MemoryMarshal.GetReference<T>(span), length - length2)), Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), (NUInt)length2 * right);
			}
			return length2 <= length && SpanHelpers.SequenceEqual<T>(Unsafe.Add<T>(MemoryMarshal.GetReference<T>(span), length - length2), MemoryMarshal.GetReference<T>(value), length2);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002EFAC File Offset: 0x0002D1AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EndsWith<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> value) where T : IEquatable<T>
		{
			int length = span.Length;
			int length2 = value.Length;
			NUInt right;
			if (default(T) != null && MemoryExtensions.IsTypeComparableAsBytes<T>(out right))
			{
				return length2 <= length && SpanHelpers.SequenceEqual(Unsafe.As<T, byte>(Unsafe.Add<T>(MemoryMarshal.GetReference<T>(span), length - length2)), Unsafe.As<T, byte>(MemoryMarshal.GetReference<T>(value)), (NUInt)length2 * right);
			}
			return length2 <= length && SpanHelpers.SequenceEqual<T>(Unsafe.Add<T>(MemoryMarshal.GetReference<T>(span), length - length2), MemoryMarshal.GetReference<T>(value), length2);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002F038 File Offset: 0x0002D238
		public unsafe static void Reverse<T>(this System.Span<T> span)
		{
			ref T reference = ref MemoryMarshal.GetReference<T>(span);
			int i = 0;
			int num = span.Length - 1;
			while (i < num)
			{
				T t = *Unsafe.Add<T>(ref reference, i);
				*Unsafe.Add<T>(ref reference, i) = *Unsafe.Add<T>(ref reference, num);
				*Unsafe.Add<T>(ref reference, num) = t;
				i++;
				num--;
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0002F098 File Offset: 0x0002D298
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Span<T> AsSpan<T>(this T[] array)
		{
			return new System.Span<T>(array);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002F0A0 File Offset: 0x0002D2A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Span<T> AsSpan<T>(this T[] array, int start, int length)
		{
			return new System.Span<T>(array, start, length);
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002F0AA File Offset: 0x0002D2AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Span<T> AsSpan<T>(this ArraySegment<T> segment)
		{
			return new System.Span<T>(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002F0C6 File Offset: 0x0002D2C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Span<T> AsSpan<T>(this ArraySegment<T> segment, int start)
		{
			if ((ulong)start > (ulong)((long)segment.Count))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.Span<T>(segment.Array, segment.Offset + start, segment.Count - start);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002F0F8 File Offset: 0x0002D2F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.Span<T> AsSpan<T>(this ArraySegment<T> segment, int start, int length)
		{
			if ((ulong)start > (ulong)((long)segment.Count))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			if ((ulong)length > (ulong)((long)(segment.Count - start)))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
			}
			return new System.Span<T>(segment.Array, segment.Offset + start, length);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002F136 File Offset: 0x0002D336
		public static System.Memory<T> AsMemory<T>(this T[] array)
		{
			return new System.Memory<T>(array);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0002F13E File Offset: 0x0002D33E
		public static System.Memory<T> AsMemory<T>(this T[] array, int start)
		{
			return new System.Memory<T>(array, start);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002F147 File Offset: 0x0002D347
		public static System.Memory<T> AsMemory<T>(this T[] array, int start, int length)
		{
			return new System.Memory<T>(array, start, length);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0002F151 File Offset: 0x0002D351
		public static System.Memory<T> AsMemory<T>(this ArraySegment<T> segment)
		{
			return new System.Memory<T>(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002F16D File Offset: 0x0002D36D
		public static System.Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start)
		{
			if ((ulong)start > (ulong)((long)segment.Count))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.Memory<T>(segment.Array, segment.Offset + start, segment.Count - start);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002F19F File Offset: 0x0002D39F
		public static System.Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start, int length)
		{
			if ((ulong)start > (ulong)((long)segment.Count))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			if ((ulong)length > (ulong)((long)(segment.Count - start)))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
			}
			return new System.Memory<T>(segment.Array, segment.Offset + start, length);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002F1E0 File Offset: 0x0002D3E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyTo<T>(this T[] source, System.Span<T> destination)
		{
			new System.ReadOnlySpan<T>(source).CopyTo(destination);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0002F1FC File Offset: 0x0002D3FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyTo<T>(this T[] source, System.Memory<T> destination)
		{
			source.CopyTo(destination.Span);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002F20B File Offset: 0x0002D40B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Overlaps<T>(this System.Span<T> span, System.ReadOnlySpan<T> other)
		{
			return span.Overlaps(other);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002F219 File Offset: 0x0002D419
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Overlaps<T>(this System.Span<T> span, System.ReadOnlySpan<T> other, out int elementOffset)
		{
			return span.Overlaps(other, out elementOffset);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002F228 File Offset: 0x0002D428
		public static bool Overlaps<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other)
		{
			if (span.IsEmpty || other.IsEmpty)
			{
				return false;
			}
			IntPtr value = Unsafe.ByteOffset<T>(MemoryMarshal.GetReference<T>(span), MemoryMarshal.GetReference<T>(other));
			if (Unsafe.SizeOf<IntPtr>() == 4)
			{
				return (int)value < span.Length * Unsafe.SizeOf<T>() || (int)value > -(other.Length * Unsafe.SizeOf<T>());
			}
			return (long)value < (long)span.Length * (long)Unsafe.SizeOf<T>() || (long)value > -((long)other.Length * (long)Unsafe.SizeOf<T>());
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002F2C4 File Offset: 0x0002D4C4
		public static bool Overlaps<T>(this System.ReadOnlySpan<T> span, System.ReadOnlySpan<T> other, out int elementOffset)
		{
			if (span.IsEmpty || other.IsEmpty)
			{
				elementOffset = 0;
				return false;
			}
			IntPtr value = Unsafe.ByteOffset<T>(MemoryMarshal.GetReference<T>(span), MemoryMarshal.GetReference<T>(other));
			if (Unsafe.SizeOf<IntPtr>() == 4)
			{
				if ((int)value < span.Length * Unsafe.SizeOf<T>() || (int)value > -(other.Length * Unsafe.SizeOf<T>()))
				{
					if ((int)value % Unsafe.SizeOf<T>() != 0)
					{
						ThrowHelper.ThrowArgumentException_OverlapAlignmentMismatch();
					}
					elementOffset = (int)value / Unsafe.SizeOf<T>();
					return true;
				}
				elementOffset = 0;
				return false;
			}
			else
			{
				if ((long)value < (long)span.Length * (long)Unsafe.SizeOf<T>() || (long)value > -((long)other.Length * (long)Unsafe.SizeOf<T>()))
				{
					if ((long)value % (long)Unsafe.SizeOf<T>() != 0L)
					{
						ThrowHelper.ThrowArgumentException_OverlapAlignmentMismatch();
					}
					elementOffset = (int)((long)value / (long)Unsafe.SizeOf<T>());
					return true;
				}
				elementOffset = 0;
				return false;
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002F3AE File Offset: 0x0002D5AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BinarySearch<T>(this System.Span<T> span, IComparable<T> comparable)
		{
			return span.BinarySearch(comparable);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002F3B7 File Offset: 0x0002D5B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BinarySearch<T, TComparable>(this System.Span<T> span, TComparable comparable) where TComparable : IComparable<T>
		{
			return span.BinarySearch(comparable);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002F3C5 File Offset: 0x0002D5C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BinarySearch<T, TComparer>(this System.Span<T> span, T value, TComparer comparer) where TComparer : IComparer<T>
		{
			return span.BinarySearch(value, comparer);
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0002F3D4 File Offset: 0x0002D5D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BinarySearch<T>(this System.ReadOnlySpan<T> span, IComparable<T> comparable)
		{
			return span.BinarySearch(comparable);
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0002F3DD File Offset: 0x0002D5DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BinarySearch<T, TComparable>(this System.ReadOnlySpan<T> span, TComparable comparable) where TComparable : IComparable<T>
		{
			return span.BinarySearch(comparable);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002F3E8 File Offset: 0x0002D5E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BinarySearch<T, TComparer>(this System.ReadOnlySpan<T> span, T value, TComparer comparer) where TComparer : IComparer<T>
		{
			if (comparer == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparer);
			}
			SpanHelpers.ComparerComparable<T, TComparer> comparable = new SpanHelpers.ComparerComparable<T, TComparer>(value, comparer);
			return span.BinarySearch(comparable);
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002F414 File Offset: 0x0002D614
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsTypeComparableAsBytes<T>(out NUInt size)
		{
			if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
			{
				size = (NUInt)1;
				return true;
			}
			if (typeof(T) == typeof(char) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort))
			{
				size = (NUInt)2;
				return true;
			}
			if (typeof(T) == typeof(int) || typeof(T) == typeof(uint))
			{
				size = (NUInt)4;
				return true;
			}
			if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
			{
				size = (NUInt)8;
				return true;
			}
			size = default(NUInt);
			return false;
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002F554 File Offset: 0x0002D754
		public static System.Span<T> AsSpan<T>(this T[] array, int start)
		{
			return System.Span<T>.Create(array, start);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002F55D File Offset: 0x0002D75D
		public static bool Contains(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, StringComparison comparisonType)
		{
			return span.IndexOf(value, comparisonType) >= 0;
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002F570 File Offset: 0x0002D770
		public static bool Equals(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> other, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				return span.SequenceEqual(other);
			}
			if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				return span.Length == other.Length && MemoryExtensions.EqualsOrdinalIgnoreCase(span, other);
			}
			return span.ToString().Equals(other.ToString(), comparisonType);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002F5C7 File Offset: 0x0002D7C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool EqualsOrdinalIgnoreCase(System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> other)
		{
			return other.Length == 0 || MemoryExtensions.CompareToOrdinalIgnoreCase(span, other) == 0;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002F5DE File Offset: 0x0002D7DE
		public static int CompareTo(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> other, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				return span.SequenceCompareTo(other);
			}
			if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				return MemoryExtensions.CompareToOrdinalIgnoreCase(span, other);
			}
			return string.Compare(span.ToString(), other.ToString(), comparisonType);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002F618 File Offset: 0x0002D818
		private unsafe static int CompareToOrdinalIgnoreCase(System.ReadOnlySpan<char> strA, System.ReadOnlySpan<char> strB)
		{
			int num = Math.Min(strA.Length, strB.Length);
			int num2 = num;
			fixed (char* reference = MemoryMarshal.GetReference<char>(strA))
			{
				char* ptr = reference;
				fixed (char* reference2 = MemoryMarshal.GetReference<char>(strB))
				{
					char* ptr2 = reference2;
					char* ptr3 = ptr;
					char* ptr4 = ptr2;
					while (num != 0 && *ptr3 <= '\u007f' && *ptr4 <= '\u007f')
					{
						int num3 = (int)(*ptr3);
						int num4 = (int)(*ptr4);
						if (num3 == num4)
						{
							ptr3++;
							ptr4++;
							num--;
						}
						else
						{
							if (num3 - 97 <= 25)
							{
								num3 -= 32;
							}
							if (num4 - 97 <= 25)
							{
								num4 -= 32;
							}
							if (num3 != num4)
							{
								return num3 - num4;
							}
							ptr3++;
							ptr4++;
							num--;
						}
					}
					if (num == 0)
					{
						return strA.Length - strB.Length;
					}
					num2 -= num;
					return string.Compare(strA.Slice(num2).ToString(), strB.Slice(num2).ToString(), StringComparison.OrdinalIgnoreCase);
				}
			}
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002F715 File Offset: 0x0002D915
		public static int IndexOf(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				return span.IndexOf(value);
			}
			return span.ToString().IndexOf(value.ToString(), comparisonType);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002F744 File Offset: 0x0002D944
		public static int ToLower(this System.ReadOnlySpan<char> source, System.Span<char> destination, CultureInfo culture)
		{
			if (culture == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.culture);
			}
			if (destination.Length < source.Length)
			{
				return -1;
			}
			string text = source.ToString();
			string text2 = text.ToLower(culture);
			text2.AsSpan().CopyTo(destination);
			return source.Length;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0002F799 File Offset: 0x0002D999
		public static int ToLowerInvariant(this System.ReadOnlySpan<char> source, System.Span<char> destination)
		{
			return source.ToLower(destination, CultureInfo.InvariantCulture);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002F7A8 File Offset: 0x0002D9A8
		public static int ToUpper(this System.ReadOnlySpan<char> source, System.Span<char> destination, CultureInfo culture)
		{
			if (culture == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.culture);
			}
			if (destination.Length < source.Length)
			{
				return -1;
			}
			string text = source.ToString();
			string text2 = text.ToUpper(culture);
			text2.AsSpan().CopyTo(destination);
			return source.Length;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0002F7FD File Offset: 0x0002D9FD
		public static int ToUpperInvariant(this System.ReadOnlySpan<char> source, System.Span<char> destination)
		{
			return source.ToUpper(destination, CultureInfo.InvariantCulture);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0002F80C File Offset: 0x0002DA0C
		public static bool EndsWith(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				return span.EndsWith(value);
			}
			if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				return value.Length <= span.Length && MemoryExtensions.EqualsOrdinalIgnoreCase(span.Slice(span.Length - value.Length), value);
			}
			string text = span.ToString();
			string value2 = value.ToString();
			return text.EndsWith(value2, comparisonType);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0002F87C File Offset: 0x0002DA7C
		public static bool StartsWith(this System.ReadOnlySpan<char> span, System.ReadOnlySpan<char> value, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				return span.StartsWith(value);
			}
			if (comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				return value.Length <= span.Length && MemoryExtensions.EqualsOrdinalIgnoreCase(span.Slice(0, value.Length), value);
			}
			string text = span.ToString();
			string value2 = value.ToString();
			return text.StartsWith(value2, comparisonType);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0002F8E8 File Offset: 0x0002DAE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.ReadOnlySpan<char> AsSpan(this string text)
		{
			if (text == null)
			{
				return default(System.ReadOnlySpan<char>);
			}
			return new System.ReadOnlySpan<char>(Unsafe.As<Pinnable<char>>(text), MemoryExtensions.StringAdjustment, text.Length);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0002F918 File Offset: 0x0002DB18
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.ReadOnlySpan<char> AsSpan(this string text, int start)
		{
			if (text == null)
			{
				if (start != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				return default(System.ReadOnlySpan<char>);
			}
			if (start > text.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.ReadOnlySpan<char>(Unsafe.As<Pinnable<char>>(text), MemoryExtensions.StringAdjustment + start * 2, text.Length - start);
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0002F96C File Offset: 0x0002DB6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static System.ReadOnlySpan<char> AsSpan(this string text, int start, int length)
		{
			if (text == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				return default(System.ReadOnlySpan<char>);
			}
			if (start > text.Length || length > text.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.ReadOnlySpan<char>(Unsafe.As<Pinnable<char>>(text), MemoryExtensions.StringAdjustment + start * 2, length);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0002F9C8 File Offset: 0x0002DBC8
		public static System.ReadOnlyMemory<char> AsMemory(this string text)
		{
			if (text == null)
			{
				return default(System.ReadOnlyMemory<char>);
			}
			return new System.ReadOnlyMemory<char>(text, 0, text.Length);
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0002F9F0 File Offset: 0x0002DBF0
		public static System.ReadOnlyMemory<char> AsMemory(this string text, int start)
		{
			if (text == null)
			{
				if (start != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				return default(System.ReadOnlyMemory<char>);
			}
			if (start > text.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.ReadOnlyMemory<char>(text, start, text.Length - start);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002FA34 File Offset: 0x0002DC34
		public static System.ReadOnlyMemory<char> AsMemory(this string text, int start, int length)
		{
			if (text == null)
			{
				if (start != 0 || length != 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				return default(System.ReadOnlyMemory<char>);
			}
			if (start > text.Length || length > text.Length - start)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return new System.ReadOnlyMemory<char>(text, start, length);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0002FA7C File Offset: 0x0002DC7C
		private unsafe static IntPtr MeasureStringAdjustment()
		{
			string text = "a";
			fixed (string text2 = text)
			{
				char* ptr = text2;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				return Unsafe.ByteOffset<char>(ref Unsafe.As<Pinnable<char>>(text).Data, Unsafe.AsRef<char>((void*)ptr));
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0002FAB5 File Offset: 0x0002DCB5
		// Note: this type is marked as 'beforefieldinit'.
		static MemoryExtensions()
		{
			MemoryExtensions.StringAdjustment = MemoryExtensions.MeasureStringAdjustment();
		}

		// Token: 0x04000269 RID: 617
		internal static readonly IntPtr StringAdjustment;
	}
}
