using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace gfoidl.Base64.Internal
{
	// Token: 0x020000B5 RID: 181
	internal sealed class Base64UrlEncoder : Base64EncoderImpl
	{
		// Token: 0x060005BE RID: 1470 RVA: 0x00015780 File Offset: 0x00013980
		public override int GetMaxDecodedLength(int encodedLength)
		{
			if (encodedLength >= 2147483647)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.encodedLength);
			}
			int num;
			int result;
			if (Base64UrlEncoder.TryGetDataLength(encodedLength, out num, out result, true))
			{
				return result;
			}
			return Base64UrlEncoder.GetMaxDecodedLengthForMalformedInput(encodedLength);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000157B0 File Offset: 0x000139B0
		public override int GetDecodedLength(System.ReadOnlySpan<byte> encoded)
		{
			return this.GetDecodedLength(encoded.Length);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000157BF File Offset: 0x000139BF
		public override int GetDecodedLength(System.ReadOnlySpan<char> encoded)
		{
			return this.GetDecodedLength(encoded.Length);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000157D0 File Offset: 0x000139D0
		internal int GetDecodedLength(int encodedLength)
		{
			if (encodedLength >= 2147483647)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.encodedLength);
			}
			int result;
			if (encodedLength != 22)
			{
				int num;
				if (!Base64UrlEncoder.TryGetNumBase64PaddingCharsToAddForDecode(encodedLength, out num))
				{
					ThrowHelper.ThrowMalformedInputException(encodedLength);
				}
				int num2 = encodedLength + num;
				if (num2 < 0)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.encodedLength);
					throw null;
				}
				result = (num2 >> 2) * 3 - num;
			}
			else
			{
				result = 16;
			}
			return result;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00015820 File Offset: 0x00013A20
		[return: Nullable(1)]
		public override byte[] Decode(System.ReadOnlySpan<char> encoded)
		{
			if (encoded.IsEmpty)
			{
				return Array.Empty<byte>();
			}
			byte[] array = new byte[this.GetDecodedLength(encoded)];
			int num;
			int num2;
			OperationStatus operationStatus = this.DecodeImpl<char>(encoded, array, out num, out num2, true);
			if (operationStatus == OperationStatus.InvalidData)
			{
				ThrowHelper.ThrowForOperationNotDone(operationStatus);
			}
			return array;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00015867 File Offset: 0x00013A67
		protected override OperationStatus DecodeCore(System.ReadOnlySpan<byte> encoded, System.Span<byte> data, out int consumed, out int written, int decodedLength = -1, bool isFinalBlock = true)
		{
			return this.DecodeImpl<byte>(encoded, data, out consumed, out written, isFinalBlock);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00015876 File Offset: 0x00013A76
		protected override OperationStatus DecodeCore(System.ReadOnlySpan<char> encoded, System.Span<byte> data, out int consumed, out int written, int decodedLength = -1, bool isFinalBlock = true)
		{
			return this.DecodeImpl<char>(encoded, data, out consumed, out written, isFinalBlock);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00015888 File Offset: 0x00013A88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private OperationStatus DecodeImpl<[IsUnmanaged] T>(System.ReadOnlySpan<T> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true) where T : struct, ValueType
		{
			if (encoded.IsEmpty)
			{
				consumed = 0;
				written = 0;
				return OperationStatus.Done;
			}
			ref T reference = ref MemoryMarshal.GetReference<T>(encoded);
			int length = encoded.Length;
			return this.DecodeImpl<T>(ref reference, length, data, out consumed, out written, isFinalBlock);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000158C4 File Offset: 0x00013AC4
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static int DecodeThree<[Nullable(2)] T>(ref T encoded, ref sbyte decodingMap)
		{
			uint num;
			uint num2;
			uint num3;
			if (typeof(T) == typeof(byte))
			{
				ref byte source = ref Unsafe.As<T, byte>(ref encoded);
				num = (uint)(*Unsafe.Add<byte>(ref source, 0));
				num2 = (uint)(*Unsafe.Add<byte>(ref source, 1));
				num3 = (uint)(*Unsafe.Add<byte>(ref source, 2));
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
				if ((num | num2 | num3) > 256U)
				{
					num2 = (num = (num3 = 0U));
				}
			}
			int num4 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num))));
			int num5 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num2))));
			int num6 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num3))));
			return num4 << 18 | num5 << 12 | num6 << 6;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000159A0 File Offset: 0x00013BA0
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static int DecodeTwo<[Nullable(2)] T>(ref T encoded, ref sbyte decodingMap)
		{
			uint num;
			uint num2;
			if (typeof(T) == typeof(byte))
			{
				ref byte source = ref Unsafe.As<T, byte>(ref encoded);
				num = (uint)(*Unsafe.Add<byte>(ref source, 0));
				num2 = (uint)(*Unsafe.Add<byte>(ref source, 1));
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
				if ((num | num2) > 256U)
				{
					num2 = (num = 0U);
				}
			}
			int num3 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num))));
			int num4 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, (IntPtr)((long)((ulong)num2))));
			return num3 << 18 | num4 << 12;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00015A4E File Offset: 0x00013C4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void WriteTwoLowOrderBytes(ref byte destination, uint destIndex, int value)
		{
			*Unsafe.Add<byte>(ref destination, (IntPtr)((long)((ulong)destIndex))) = (byte)(value >> 16);
			*Unsafe.Add<byte>(ref destination, (IntPtr)((long)((ulong)(destIndex + 1U)))) = (byte)(value >> 8);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00015A77 File Offset: 0x00013C77
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void WriteOneLowOrderByte(ref byte destination, uint destIndex, int value)
		{
			*Unsafe.Add<byte>(ref destination, (IntPtr)((long)((ulong)destIndex))) = (byte)(value >> 16);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00015A8C File Offset: 0x00013C8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryGetDataLength(int urlEncodedLen, out int base64Len, out int dataLength, bool isFinalBlock = true)
		{
			if (!isFinalBlock)
			{
				base64Len = urlEncodedLen;
				dataLength = (urlEncodedLen >> 2) * 3;
				return true;
			}
			if (urlEncodedLen != 22)
			{
				int num;
				if (Base64UrlEncoder.TryGetNumBase64PaddingCharsToAddForDecode(urlEncodedLen, out num))
				{
					int num2 = urlEncodedLen + num;
					if (num2 >= 0)
					{
						dataLength = (num2 >> 2) * 3 - num;
						base64Len = num2;
						return true;
					}
				}
				base64Len = 0;
				dataLength = 0;
				return false;
			}
			base64Len = 24;
			dataLength = 16;
			return true;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00015AE0 File Offset: 0x00013CE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool TryGetNumBase64PaddingCharsToAddForDecode(int urlEncodedLen, out int numPaddingChars)
		{
			int num = 4 - urlEncodedLen & 3;
			numPaddingChars = num;
			return num != 3;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00015AFD File Offset: 0x00013CFD
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static int GetMaxDecodedLengthForMalformedInput(int encodedLength)
		{
			return (encodedLength + 2 >> 2) * 3;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00015B08 File Offset: 0x00013D08
		internal unsafe static System.ReadOnlySpan<sbyte> DecodingMap
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new System.ReadOnlySpan<sbyte>((void*)(&gfoidl.Base64.dll!<PrivateImplementationDetails>.DDE4412C9CA3118D8C57C0231EAFDFF9212FAB34B3B3C5219051A67F6051BE69), 257).Slice(1);
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00015B30 File Offset: 0x00013D30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetEncodedLength(int sourceLength)
		{
			if (sourceLength == 16)
			{
				return 22;
			}
			int base64EncodedLength = Base64EncoderImpl.GetBase64EncodedLength(sourceLength);
			int numBase64PaddingCharsAddedByEncode = Base64UrlEncoder.GetNumBase64PaddingCharsAddedByEncode(sourceLength);
			return base64EncodedLength - numBase64PaddingCharsAddedByEncode;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00015B54 File Offset: 0x00013D54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetBufferSizeRequiredToBase64Encode(int sourceLength, out int numPaddingChars)
		{
			if (sourceLength == 16)
			{
				numPaddingChars = 2;
				return 24;
			}
			numPaddingChars = Base64UrlEncoder.GetNumBase64PaddingCharsAddedByEncode(sourceLength);
			return Base64EncoderImpl.GetBase64EncodedLength(sourceLength);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00015B6F File Offset: 0x00013D6F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[return: Nullable(1)]
		public override string Encode(System.ReadOnlySpan<byte> data)
		{
			if (data.IsEmpty)
			{
				return string.Empty;
			}
			return this.<Encode>g__EncodeWithNewString|19_0(data);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00015B87 File Offset: 0x00013D87
		protected override OperationStatus EncodeCore(System.ReadOnlySpan<byte> data, System.Span<byte> encoded, out int consumed, out int written, int encodedLength = -1, bool isFinalBlock = true)
		{
			return this.EncodeImpl<byte>(data, encoded, out consumed, out written, encodedLength, isFinalBlock);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00015B98 File Offset: 0x00013D98
		protected override OperationStatus EncodeCore(System.ReadOnlySpan<byte> data, System.Span<char> encoded, out int consumed, out int written, int encodedLength = -1, bool isFinalBlock = true)
		{
			return this.EncodeImpl<char>(data, encoded, out consumed, out written, encodedLength, isFinalBlock);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00015BAC File Offset: 0x00013DAC
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

		// Token: 0x060005D4 RID: 1492 RVA: 0x00015C08 File Offset: 0x00013E08
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
				ref byte source = ref Unsafe.As<T, byte>(ref encoded);
				*Unsafe.Add<byte>(ref source, 0) = (byte)num2;
				*Unsafe.Add<byte>(ref source, 1) = (byte)num3;
				*Unsafe.Add<byte>(ref source, 2) = (byte)num4;
				return;
			}
			if (typeof(T) == typeof(char))
			{
				ref char source2 = ref Unsafe.As<T, char>(ref encoded);
				*Unsafe.Add<char>(ref source2, 0) = (char)num2;
				*Unsafe.Add<char>(ref source2, 1) = (char)num3;
				*Unsafe.Add<char>(ref source2, 2) = (char)num4;
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00015CE4 File Offset: 0x00013EE4
		[NullableContext(1)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void EncodeOneByte<[Nullable(2)] T>(ref byte oneByte, ref T encoded, ref byte encodingMap)
		{
			uint num = (uint)((uint)oneByte << 8);
			uint num2 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num >> 10)))));
			uint num3 = (uint)(*Unsafe.Add<byte>(ref encodingMap, (IntPtr)((long)((ulong)(num >> 4 & 63U)))));
			if (typeof(T) == typeof(byte))
			{
				ref byte source = ref Unsafe.As<T, byte>(ref encoded);
				*Unsafe.Add<byte>(ref source, 0) = (byte)num2;
				*Unsafe.Add<byte>(ref source, 1) = (byte)num3;
				return;
			}
			if (typeof(T) == typeof(char))
			{
				ref char source2 = ref Unsafe.As<T, char>(ref encoded);
				*Unsafe.Add<char>(ref source2, 0) = (char)num2;
				*Unsafe.Add<char>(ref source2, 1) = (char)num3;
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00015D8C File Offset: 0x00013F8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int GetNumBase64PaddingCharsAddedByEncode(int dataLength)
		{
			int num = Base64UrlEncoder.FastMod3(dataLength);
			if (num != 0)
			{
				return 3 - num;
			}
			return 0;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00015DA8 File Offset: 0x00013FA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int FastMod3(int value)
		{
			if (Environment.Is64BitProcess)
			{
				return (int)((uint)(((6148914691236517206UL * (ulong)value >> 32) + 1UL) * 3UL >> 32));
			}
			return value % 3;
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00015DD0 File Offset: 0x00013FD0
		internal unsafe static System.ReadOnlySpan<byte> EncodingMap
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new System.ReadOnlySpan<byte>((void*)(&gfoidl.Base64.dll!<PrivateImplementationDetails>.1CB077363135942AF8861C72BDAA3473E30DD11D04387CFB3B44E47199E9FCA3), 65).Slice(1);
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00015DF4 File Offset: 0x00013FF4
		private OperationStatus DecodeImpl<[Nullable(2)] T>([Nullable(1)] ref T src, int inputLength, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true)
		{
			uint num = 0U;
			uint num2 = 0U;
			int num3;
			int num4;
			if (Base64UrlEncoder.TryGetDataLength(inputLength, out num3, out num4, isFinalBlock))
			{
				int num5 = num3 & -4;
				ref byte reference = ref MemoryMarshal.GetReference<byte>(data);
				ref sbyte reference2 = ref MemoryMarshal.GetReference<sbyte>(Base64UrlEncoder.DecodingMap);
				int num6 = isFinalBlock ? 4 : 0;
				int length = data.Length;
				int num7;
				if (length >= num4)
				{
					num7 = num5 - num6;
				}
				else
				{
					num7 = length / 3 * 4;
				}
				if ((ulong)num < (ulong)((long)num7))
				{
					do
					{
						int num8 = Base64EncoderImpl.DecodeFour<T>(Unsafe.Add<T>(ref src, (IntPtr)((long)((ulong)num))), ref reference2);
						if (num8 < 0)
						{
							goto IL_191;
						}
						Base64EncoderImpl.WriteThreeLowOrderBytes(ref reference, num2, num8);
						num2 += 3U;
						num += 4U;
					}
					while (num < (uint)num7);
				}
				if (num7 == num5 - num6)
				{
					if ((ulong)num == (ulong)((long)num5))
					{
						if (isFinalBlock)
						{
							goto IL_191;
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
						int num9 = num3 - inputLength;
						ref T encoded = ref Unsafe.Add<T>(ref src, num5 - 4);
						if (num9 == 0)
						{
							int num10 = Base64EncoderImpl.DecodeFour<T>(ref encoded, ref reference2);
							if (num10 < 0)
							{
								goto IL_191;
							}
							if ((ulong)num2 > (ulong)((long)(length - 3)))
							{
								goto IL_170;
							}
							Base64EncoderImpl.WriteThreeLowOrderBytes(ref reference, num2, num10);
							num += 4U;
							num2 += 3U;
						}
						else if (num9 == 1)
						{
							int num11 = Base64UrlEncoder.DecodeThree<T>(ref encoded, ref reference2);
							if (num11 < 0)
							{
								goto IL_191;
							}
							if ((ulong)num2 > (ulong)((long)(length - 2)))
							{
								goto IL_170;
							}
							Base64UrlEncoder.WriteTwoLowOrderBytes(ref reference, num2, num11);
							num += 3U;
							num2 += 2U;
						}
						else
						{
							int num12 = Base64UrlEncoder.DecodeTwo<T>(ref encoded, ref reference2);
							if (num12 < 0)
							{
								goto IL_191;
							}
							if ((ulong)num2 > (ulong)((long)(length - 1)))
							{
								goto IL_170;
							}
							Base64UrlEncoder.WriteOneLowOrderByte(ref reference, num2, num12);
							num += 2U;
							num2 += 1U;
						}
						if (num5 != num3)
						{
							goto IL_191;
						}
					}
					consumed = (int)num;
					written = (int)num2;
					return OperationStatus.Done;
				}
				IL_170:
				if (num5 == inputLength || !isFinalBlock)
				{
					consumed = (int)num;
					written = (int)num2;
					return OperationStatus.DestinationTooSmall;
				}
			}
			IL_191:
			consumed = (int)num;
			written = (int)num2;
			return OperationStatus.InvalidData;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00015F9C File Offset: 0x0001419C
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
			ref byte reference = ref MemoryMarshal.GetReference<byte>(Base64UrlEncoder.EncodingMap);
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
					Base64UrlEncoder.EncodeOneByte<T>(Unsafe.Add<byte>(ref srcBytes, (IntPtr)((long)((ulong)num))), Unsafe.Add<T>(ref dest, (IntPtr)((long)((ulong)num2))), ref reference);
					num2 += 2U;
					num += 1U;
				}
				else if ((ulong)num == (ulong)((long)(srcLength - 2)))
				{
					Base64UrlEncoder.EncodeTwoBytes<T>(Unsafe.Add<byte>(ref srcBytes, (IntPtr)((long)((ulong)num))), Unsafe.Add<T>(ref dest, (IntPtr)((long)((ulong)num2))), ref reference);
					num2 += 3U;
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

		// Token: 0x060005DC RID: 1500 RVA: 0x000160A0 File Offset: 0x000142A0
		[CompilerGenerated]
		[return: Nullable(1)]
		private unsafe string <Encode>g__EncodeWithNewString|19_0(System.ReadOnlySpan<byte> data)
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
	}
}
