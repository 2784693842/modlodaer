using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace gfoidl.Base64.Internal
{
	// Token: 0x020000B2 RID: 178
	internal abstract class Base64EncoderImpl : Base64
	{
		// Token: 0x06000598 RID: 1432 RVA: 0x00014AD6 File Offset: 0x00012CD6
		public override OperationStatus Encode(System.ReadOnlySpan<byte> data, System.Span<byte> encoded, out int consumed, out int written, bool isFinalBlock = true)
		{
			return this.EncodeCore(data, encoded, out consumed, out written, -1, isFinalBlock);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00014AE6 File Offset: 0x00012CE6
		public override OperationStatus Encode(System.ReadOnlySpan<byte> data, System.Span<char> encoded, out int consumed, out int written, bool isFinalBlock = true)
		{
			return this.EncodeCore(data, encoded, out consumed, out written, -1, isFinalBlock);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00014AF6 File Offset: 0x00012CF6
		public override OperationStatus Decode(System.ReadOnlySpan<byte> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true)
		{
			return this.DecodeCore(encoded, data, out consumed, out written, -1, isFinalBlock);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00014B06 File Offset: 0x00012D06
		public override OperationStatus Decode(System.ReadOnlySpan<char> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true)
		{
			return this.DecodeCore(encoded, data, out consumed, out written, -1, isFinalBlock);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00014B18 File Offset: 0x00012D18
		internal OperationStatus EncodeCore<T>(System.ReadOnlySpan<byte> data, System.Span<T> encoded, out int consumed, out int written, bool isFinalBlock = true) where T : struct
		{
			if (typeof(T) == typeof(byte))
			{
				return this.EncodeCore(data, MemoryMarshal.AsBytes<T>(encoded), out consumed, out written, -1, isFinalBlock);
			}
			if (typeof(T) == typeof(char))
			{
				return this.EncodeCore(data, MemoryMarshal.Cast<T, char>(encoded), out consumed, out written, -1, isFinalBlock);
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00014B88 File Offset: 0x00012D88
		internal OperationStatus DecodeCore<T>(System.ReadOnlySpan<T> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true) where T : struct
		{
			if (typeof(T) == typeof(byte))
			{
				return this.DecodeCore(MemoryMarshal.AsBytes<T>(encoded), data, out consumed, out written, -1, isFinalBlock);
			}
			if (typeof(T) == typeof(char))
			{
				return this.DecodeCore(MemoryMarshal.Cast<T, char>(encoded), data, out consumed, out written, -1, isFinalBlock);
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600059E RID: 1438
		protected abstract OperationStatus EncodeCore(System.ReadOnlySpan<byte> data, System.Span<byte> encoded, out int consumed, out int written, int encodedLength = -1, bool isFinalBlock = true);

		// Token: 0x0600059F RID: 1439
		protected abstract OperationStatus EncodeCore(System.ReadOnlySpan<byte> data, System.Span<char> encoded, out int consumed, out int written, int encodedLength = -1, bool isFinalBlock = true);

		// Token: 0x060005A0 RID: 1440
		protected abstract OperationStatus DecodeCore(System.ReadOnlySpan<byte> encoded, System.Span<byte> data, out int consumed, out int written, int decodedLength = -1, bool isFinalBlock = true);

		// Token: 0x060005A1 RID: 1441
		protected abstract OperationStatus DecodeCore(System.ReadOnlySpan<char> encoded, System.Span<byte> data, out int consumed, out int written, int decodedLength = -1, bool isFinalBlock = true);

		// Token: 0x060005A2 RID: 1442 RVA: 0x00014BF8 File Offset: 0x00012DF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static int GetBase64EncodedLength(int sourceLength)
		{
			if (sourceLength > 1610612733)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
			}
			return (sourceLength + 2) / 3 * 4;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00014C10 File Offset: 0x00012E10
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected unsafe static void EncodeThreeBytes<[Nullable(2)] T>(ref byte threeBytes, ref T encoded, ref byte encodingMap)
		{
			uint num = (uint)threeBytes;
			uint num2 = (uint)(*Unsafe.Add<byte>(ref threeBytes, 1));
			uint num3 = (uint)(*Unsafe.Add<byte>(ref threeBytes, 2));
			uint num4 = num << 16 | num2 << 8 | num3;
			uint num5 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num4 >> 18)))));
			uint num6 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num4 >> 12 & 63U)))));
			uint num7 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num4 >> 6 & 63U)))));
			uint num8 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num4 & 63U)))));
			if (typeof(T) == typeof(byte))
			{
				num4 = (num5 | num6 << 8 | num7 << 16 | num8 << 24);
				Unsafe.WriteUnaligned<uint>(Unsafe.As<T, byte>(ref encoded), num4);
				return;
			}
			if (typeof(T) == typeof(char))
			{
				ref char source = ref Unsafe.As<T, char>(ref encoded);
				*Unsafe.Add<char>(ref source, 0) = (char)num5;
				*Unsafe.Add<char>(ref source, 1) = (char)num6;
				*Unsafe.Add<char>(ref source, 2) = (char)num7;
				*Unsafe.Add<char>(ref source, 3) = (char)num8;
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00014D18 File Offset: 0x00012F18
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected unsafe static int DecodeFour<[Nullable(2)] T>(ref T encoded, ref sbyte decodingMap)
		{
			uint num;
			uint num2;
			uint num3;
			uint num4;
			if (typeof(T) == typeof(byte))
			{
				ref byte source = ref Unsafe.As<T, byte>(ref encoded);
				num = (uint)(*Unsafe.Add<byte>(ref source, 0));
				num2 = (uint)(*Unsafe.Add<byte>(ref source, 1));
				num3 = (uint)(*Unsafe.Add<byte>(ref source, 2));
				num4 = (uint)(*Unsafe.Add<byte>(ref source, 3));
			}
			else
			{
				if (!(typeof(T) == typeof(char)))
				{
					throw new NotSupportedException();
				}
				ref char source2 = ref Unsafe.As<T, char>(ref encoded);
				num = (uint)(*Unsafe.Add<char>(ref source2, 0));
				num2 = (uint)(*Unsafe.Add<char>(ref source2, 1));
				num3 = (uint)(*Unsafe.Add<char>(ref source2, 2));
				num4 = (uint)(*Unsafe.Add<char>(ref source2, 3));
				if ((num | num2 | (num3 | num4)) > 256U)
				{
					num2 = (num = (num3 = (num4 = 0U)));
				}
			}
			int num5 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num))));
			int num6 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num2))));
			int num7 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num3))));
			int num8 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num4))));
			int num9 = num5 << 18;
			num6 <<= 12;
			num7 <<= 6;
			int num10 = num9 | num8;
			num6 |= num7;
			return num10 | num6;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00014E28 File Offset: 0x00013028
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected unsafe static void WriteThreeLowOrderBytes(ref byte destination, uint destIndex, int value)
		{
			*Unsafe.Add<byte>(ref destination, (IntPtr)((long)((ulong)destIndex))) = (byte)(value >> 16);
			*Unsafe.Add<byte>(ref destination, (IntPtr)((long)((ulong)(destIndex + 1U)))) = (byte)(value >> 8);
			*Unsafe.Add<byte>(ref destination, (IntPtr)((long)((ulong)(destIndex + 2U)))) = (byte)value;
		}

		// Token: 0x040001E5 RID: 485
		protected const int MaxStackallocBytes = 256;
	}
}
