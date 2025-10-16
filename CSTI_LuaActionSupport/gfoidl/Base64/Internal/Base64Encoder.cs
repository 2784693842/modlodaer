using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace gfoidl.Base64.Internal
{
	// Token: 0x020000B3 RID: 179
	internal sealed class Base64Encoder : Base64EncoderImpl
	{
		// Token: 0x060005A7 RID: 1447 RVA: 0x00014E6B File Offset: 0x0001306B
		public override int GetMaxDecodedLength(int encodedLength)
		{
			if (encodedLength < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.encodedLength, ExceptionRessource.EncodedLengthOutOfRange);
			}
			return this.GetDecodedLength(encodedLength);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00014E7F File Offset: 0x0001307F
		public override int GetDecodedLength(System.ReadOnlySpan<byte> encoded)
		{
			return this.GetDecodedLengthImpl<byte>(encoded);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00014E88 File Offset: 0x00013088
		public override int GetDecodedLength(System.ReadOnlySpan<char> encoded)
		{
			return this.GetDecodedLengthImpl<char>(encoded);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00014E94 File Offset: 0x00013094
		[NullableContext(2)]
		internal unsafe int GetDecodedLengthImpl<T>([Nullable(new byte[]
		{
			0,
			1
		})] System.ReadOnlySpan<T> encoded)
		{
			if (encoded.IsEmpty)
			{
				return 0;
			}
			if (encoded.Length < 4)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.encodedLength, ExceptionRessource.EncodedLengthOutOfRange);
			}
			int decodedLength = this.GetDecodedLength(encoded.Length);
			ref T source = ref Unsafe.Add<T>(MemoryMarshal.GetReference<T>(encoded), (IntPtr)((long)((ulong)(encoded.Length - 1))));
			int num = 0;
			if (typeof(T) == typeof(byte))
			{
				ref byte ptr = ref Unsafe.As<T, byte>(ref source);
				if (ptr == 61)
				{
					num++;
				}
				if (*Unsafe.Subtract<byte>(ref ptr, 1) == 61)
				{
					num++;
				}
			}
			else
			{
				if (!(typeof(T) == typeof(char)))
				{
					throw new NotSupportedException();
				}
				ref char ptr2 = ref Unsafe.As<T, char>(ref source);
				if (ptr2 == '=')
				{
					num++;
				}
				if (*Unsafe.Subtract<char>(ref ptr2, 1) == '=')
				{
					num++;
				}
			}
			return decodedLength - num;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00014F67 File Offset: 0x00013167
		internal int GetDecodedLength(int encodedLength)
		{
			return (encodedLength >> 2) * 3;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00014F70 File Offset: 0x00013170
		[return: Nullable(1)]
		public override byte[] Decode(System.ReadOnlySpan<char> encoded)
		{
			if (encoded.IsEmpty)
			{
				return Array.Empty<byte>();
			}
			int decodedLength = this.GetDecodedLength(encoded);
			byte[] array = new byte[decodedLength];
			int num;
			int num2;
			OperationStatus operationStatus = this.DecodeImpl<char>(encoded, array, out num, out num2, decodedLength, true);
			if (operationStatus == OperationStatus.InvalidData)
			{
				ThrowHelper.ThrowForOperationNotDone(operationStatus);
			}
			return array;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00014FBA File Offset: 0x000131BA
		protected override OperationStatus DecodeCore(System.ReadOnlySpan<byte> encoded, System.Span<byte> data, out int consumed, out int written, int decodedLength = -1, bool isFinalBlock = true)
		{
			return this.DecodeImpl<byte>(encoded, data, out consumed, out written, decodedLength, isFinalBlock);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00014FCB File Offset: 0x000131CB
		protected override OperationStatus DecodeCore(System.ReadOnlySpan<char> encoded, System.Span<byte> data, out int consumed, out int written, int decodedLength = -1, bool isFinalBlock = true)
		{
			return this.DecodeImpl<char>(encoded, data, out consumed, out written, decodedLength, isFinalBlock);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00014FDC File Offset: 0x000131DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private OperationStatus DecodeImpl<[IsUnmanaged] T>(System.ReadOnlySpan<T> encoded, System.Span<byte> data, out int consumed, out int written, int decodedLength = -1, bool isFinalBlock = true) where T : struct, ValueType
		{
			if (encoded.IsEmpty)
			{
				consumed = 0;
				written = 0;
				return OperationStatus.Done;
			}
			ref T reference = ref MemoryMarshal.GetReference<T>(encoded);
			int length = encoded.Length;
			if (decodedLength == -1)
			{
				decodedLength = this.GetDecodedLength(length);
			}
			return this.DecodeImpl<T>(ref reference, length, data, decodedLength, out consumed, out written, isFinalBlock);
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00015028 File Offset: 0x00013228
		internal unsafe static System.ReadOnlySpan<sbyte> DecodingMap
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new System.ReadOnlySpan<sbyte>((void*)(&gfoidl.Base64.dll!<PrivateImplementationDetails>.A23A8D9D85695FE23F6B87ED7ECE3F75C98699EE38B854C517E2B41B04F2C4EB), 257).Slice(1);
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001504D File Offset: 0x0001324D
		public override int GetEncodedLength(int sourceLength)
		{
			return Base64EncoderImpl.GetBase64EncodedLength(sourceLength);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00015055 File Offset: 0x00013255
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(1)]
		public override string Encode(System.ReadOnlySpan<byte> data)
		{
			if (data.IsEmpty)
			{
				return string.Empty;
			}
			return this.<Encode>g__EncodeWithNewString|13_0(data);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001506D File Offset: 0x0001326D
		protected override OperationStatus EncodeCore(System.ReadOnlySpan<byte> data, System.Span<byte> encoded, out int consumed, out int written, int encodedLength = -1, bool isFinalBlock = true)
		{
			return this.EncodeImpl<byte>(data, encoded, out consumed, out written, encodedLength, isFinalBlock);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001507E File Offset: 0x0001327E
		protected override OperationStatus EncodeCore(System.ReadOnlySpan<byte> data, System.Span<char> encoded, out int consumed, out int written, int encodedLength = -1, bool isFinalBlock = true)
		{
			return this.EncodeImpl<char>(data, encoded, out consumed, out written, encodedLength, isFinalBlock);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00015090 File Offset: 0x00013290
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private OperationStatus EncodeImpl<[IsUnmanaged] T>(System.ReadOnlySpan<byte> data, System.Span<T> encoded, out int consumed, out int written, int encodedLength = -1, bool isFinalBlock = true) where T : struct, ValueType
		{
			if (data.IsEmpty)
			{
				consumed = 0;
				written = 0;
				return OperationStatus.Done;
			}
			int length = data.Length;
			ref byte reference = ref MemoryMarshal.GetReference<byte>(data);
			ref T reference2 = ref MemoryMarshal.GetReference<T>(encoded);
			if (encodedLength == -1)
			{
				encodedLength = this.GetEncodedLength(length);
			}
			return this.EncodeImpl<T>(ref reference, length, ref reference2, encoded.Length, encodedLength, out consumed, out written, isFinalBlock);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x000150EC File Offset: 0x000132EC
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void EncodeTwoBytes<[Nullable(2)] T>(ref byte twoBytes, ref T encoded, ref byte encodingMap)
		{
			uint num = (uint)((int)twoBytes << 16 | (int)(*Unsafe.Add<byte>(ref twoBytes, 1)) << 8);
			uint num2 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num >> 18)))));
			uint num3 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num >> 12 & 63U)))));
			uint num4 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num >> 6 & 63U)))));
			if (typeof(T) == typeof(byte))
			{
				num = (num2 | num3 << 8 | num4 << 16 | 1023410176U);
				Unsafe.WriteUnaligned<uint>(Unsafe.As<T, byte>(ref encoded), num);
				return;
			}
			if (typeof(T) == typeof(char))
			{
				ref char source = ref Unsafe.As<T, char>(ref encoded);
				*Unsafe.Add<char>(ref source, 0) = (char)num2;
				*Unsafe.Add<char>(ref source, 1) = (char)num3;
				*Unsafe.Add<char>(ref source, 2) = (char)num4;
				*Unsafe.Add<char>(ref source, 3) = '=';
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x000151CC File Offset: 0x000133CC
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void EncodeOneByte<[Nullable(2)] T>(ref byte oneByte, ref T encoded, ref byte encodingMap)
		{
			uint num = (uint)((uint)oneByte << 8);
			uint num2 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num >> 10)))));
			uint num3 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num >> 4 & 63U)))));
			if (typeof(T) == typeof(byte))
			{
				num = (num2 | num3 << 8 | 3997696U | 1023410176U);
				Unsafe.WriteUnaligned<uint>(Unsafe.As<T, byte>(ref encoded), num);
				return;
			}
			if (typeof(T) == typeof(char))
			{
				ref char source = ref Unsafe.As<T, char>(ref encoded);
				*Unsafe.Add<char>(ref source, 0) = (char)num2;
				*Unsafe.Add<char>(ref source, 1) = (char)num3;
				*Unsafe.Add<char>(ref source, 2) = '=';
				*Unsafe.Add<char>(ref source, 3) = '=';
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x0001528C File Offset: 0x0001348C
		internal unsafe static System.ReadOnlySpan<byte> EncodingMap
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new System.ReadOnlySpan<byte>((void*)(&gfoidl.Base64.dll!<PrivateImplementationDetails>.47D95AF99014133C298EB132A0EC3AE77C002A3954DFABCFE3DB6DFE4B3F41C1), 65).Slice(1);
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000152B0 File Offset: 0x000134B0
		private unsafe OperationStatus DecodeImpl<[Nullable(2)] T>([Nullable(1)] ref T src, int inputLength, System.Span<byte> data, int decodedLength, out int consumed, out int written, bool isFinalBlock = true)
		{
			uint num = 0U;
			uint num2 = 0U;
			int num3 = inputLength & -4;
			ref byte reference = ref MemoryMarshal.GetReference<byte>(data);
			ref sbyte reference2 = ref MemoryMarshal.GetReference<sbyte>(Base64Encoder.DecodingMap);
			int num4 = isFinalBlock ? 4 : 0;
			int length = data.Length;
			int num5;
			if (length >= decodedLength)
			{
				num5 = num3 - num4;
			}
			else
			{
				num5 = length / 3 * 4;
			}
			if ((ulong)num < (ulong)((long)num5))
			{
				do
				{
					int num6 = Base64EncoderImpl.DecodeFour<T>(Unsafe.Add<T>(ref src, (IntPtr)((long)((ulong)num))), ref reference2);
					if (num6 < 0)
					{
						goto IL_2FB;
					}
					Base64EncoderImpl.WriteThreeLowOrderBytes(ref reference, num2, num6);
					num2 += 3U;
					num += 4U;
				}
				while (num < (uint)num5);
			}
			if (num5 == num3 - num4)
			{
				if ((ulong)num == (ulong)((long)num3))
				{
					if (isFinalBlock)
					{
						goto IL_2FB;
					}
					if ((ulong)num != (ulong)((long)inputLength))
					{
						consumed = (int)num;
						written = (int)num2;
						return OperationStatus.NeedMoreData;
					}
				}
				else
				{
					uint num7;
					uint num8;
					uint num9;
					uint num10;
					if (typeof(T) == typeof(byte))
					{
						ref byte source = ref Unsafe.As<T, byte>(ref src);
						num7 = (uint)(*Unsafe.Add<byte>(ref source, (IntPtr)((long)((ulong)(num3 - 4)))));
						num8 = (uint)(*Unsafe.Add<byte>(ref source, (IntPtr)((long)((ulong)(num3 - 3)))));
						num9 = (uint)(*Unsafe.Add<byte>(ref source, (IntPtr)((long)((ulong)(num3 - 2)))));
						num10 = (uint)(*Unsafe.Add<byte>(ref source, (IntPtr)((long)((ulong)(num3 - 1)))));
					}
					else
					{
						if (!(typeof(T) == typeof(char)))
						{
							throw new NotSupportedException();
						}
						ref char source2 = ref Unsafe.As<T, char>(ref src);
						num7 = (uint)(*Unsafe.Add<char>(ref source2, (IntPtr)((long)((ulong)(num3 - 4)))));
						num8 = (uint)(*Unsafe.Add<char>(ref source2, (IntPtr)((long)((ulong)(num3 - 3)))));
						num9 = (uint)(*Unsafe.Add<char>(ref source2, (IntPtr)((long)((ulong)(num3 - 2)))));
						num10 = (uint)(*Unsafe.Add<char>(ref source2, (IntPtr)((long)((ulong)(num3 - 1)))));
						if ((num7 | num8 | (num9 | num10)) > 256U)
						{
							num8 = (num7 = (num9 = (num10 = 0U)));
						}
					}
					int num11 = (int)(*Unsafe.Add<sbyte>(ref reference2, (IntPtr)((long)((ulong)num7))));
					int num12 = (int)(*Unsafe.Add<sbyte>(ref reference2, (IntPtr)((long)((ulong)num8))));
					num11 <<= 18;
					num12 <<= 12;
					num11 |= num12;
					if (num10 != 61U)
					{
						int num13 = (int)(*Unsafe.Add<sbyte>(ref reference2, (IntPtr)((long)((ulong)num9))));
						int num14 = (int)(*Unsafe.Add<sbyte>(ref reference2, (IntPtr)((long)((ulong)num10))));
						num13 <<= 6;
						num11 |= num14;
						num11 |= num13;
						if (num11 < 0)
						{
							goto IL_2FB;
						}
						if ((ulong)num2 > (ulong)((long)(length - 3)))
						{
							goto IL_2DB;
						}
						Base64EncoderImpl.WriteThreeLowOrderBytes(ref reference, num2, num11);
						num2 += 3U;
					}
					else if (num9 != 61U)
					{
						int num15 = (int)(*Unsafe.Add<sbyte>(ref reference2, (IntPtr)((long)((ulong)num9))));
						num15 <<= 6;
						num11 |= num15;
						if (num11 < 0)
						{
							goto IL_2FB;
						}
						if ((ulong)num2 > (ulong)((long)(length - 2)))
						{
							goto IL_2DB;
						}
						*Unsafe.Add<byte>(ref reference, (IntPtr)((long)((ulong)num2))) = (byte)(num11 >> 16);
						*Unsafe.Add<byte>(ref reference, (IntPtr)((long)((ulong)(num2 + 1U)))) = (byte)(num11 >> 8);
						num2 += 2U;
					}
					else
					{
						if (num11 < 0)
						{
							goto IL_2FB;
						}
						if ((ulong)num2 > (ulong)((long)(length - 1)))
						{
							goto IL_2DB;
						}
						*Unsafe.Add<byte>(ref reference, (IntPtr)((long)((ulong)num2))) = (byte)(num11 >> 16);
						num2 += 1U;
					}
					num += 4U;
					if (num3 != inputLength)
					{
						goto IL_2FB;
					}
				}
				consumed = (int)num;
				written = (int)num2;
				return OperationStatus.Done;
			}
			IL_2DB:
			if (num3 == inputLength || !isFinalBlock)
			{
				consumed = (int)num;
				written = (int)num2;
				return OperationStatus.DestinationTooSmall;
			}
			IL_2FB:
			consumed = (int)num;
			written = (int)num2;
			return OperationStatus.InvalidData;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x000155C4 File Offset: 0x000137C4
		[NullableContext(1)]
		private OperationStatus EncodeImpl<[Nullable(2)] T>(ref byte srcBytes, int srcLength, ref T dest, int destLength, int encodedLength, out int consumed, out int written, bool isFinalBlock = true)
		{
			uint num = 0U;
			uint num2 = 0U;
			int num3 = -2;
			if (srcLength <= 1610612733 && destLength >= encodedLength)
			{
				num3 += srcLength;
			}
			else
			{
				num3 += (destLength >> 2) * 3;
			}
			ref byte reference = ref MemoryMarshal.GetReference<byte>(Base64Encoder.EncodingMap);
			if ((ulong)num < (ulong)((long)num3))
			{
				do
				{
					Base64EncoderImpl.EncodeThreeBytes<T>(Unsafe.Add<byte>(ref srcBytes, (IntPtr)((long)((ulong)num))), Unsafe.Add<T>(ref dest, (IntPtr)((long)((ulong)num2))), ref reference);
					num2 += 4U;
					num += 3U;
				}
				while (num < (uint)num3);
			}
			if (num3 == srcLength - 2)
			{
				if (!isFinalBlock)
				{
					if ((ulong)num != (ulong)((long)srcLength))
					{
						consumed = (int)num;
						written = (int)num2;
						return OperationStatus.NeedMoreData;
					}
				}
				else if ((ulong)num == (ulong)((long)(srcLength - 1)))
				{
					Base64Encoder.EncodeOneByte<T>(Unsafe.Add<byte>(ref srcBytes, (IntPtr)((long)((ulong)num))), Unsafe.Add<T>(ref dest, (IntPtr)((long)((ulong)num2))), ref reference);
					num2 += 4U;
					num += 1U;
				}
				else if ((ulong)num == (ulong)((long)(srcLength - 2)))
				{
					Base64Encoder.EncodeTwoBytes<T>(Unsafe.Add<byte>(ref srcBytes, (IntPtr)((long)((ulong)num))), Unsafe.Add<T>(ref dest, (IntPtr)((long)((ulong)num2))), ref reference);
					num2 += 4U;
					num += 2U;
				}
				consumed = (int)num;
				written = (int)num2;
				return OperationStatus.Done;
			}
			consumed = (int)num;
			written = (int)num2;
			return OperationStatus.DestinationTooSmall;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x000156D0 File Offset: 0x000138D0
		[CompilerGenerated]
		[return: Nullable(1)]
		private unsafe string <Encode>g__EncodeWithNewString|13_0(System.ReadOnlySpan<byte> data)
		{
			int encodedLength = this.GetEncodedLength(data.Length);
			char[] array = null;
			System.Span<char> span;
			if (encodedLength <= 128)
			{
				span = new System.Span<char>(stackalloc byte[(UIntPtr)256], 128);
			}
			else
			{
				span = ArrayPool<char>.Shared.Rent(encodedLength);
			}
			System.Span<char> span2 = span;
			string result;
			try
			{
				int num;
				int length;
				this.EncodeImpl<char>(data, span2, out num, out length, encodedLength, true);
				try
				{
					fixed (char* ptr = MemoryMarshal.GetReference<char>(span2))
					{
						result = new string(ptr, 0, length);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				if (array != null)
				{
					span2.Clear();
					ArrayPool<char>.Shared.Return(array, false);
				}
			}
			return result;
		}

		// Token: 0x040001E6 RID: 486
		internal const byte EncodingPad = 61;
	}
}
