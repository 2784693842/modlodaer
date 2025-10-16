using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using gfoidl.Base64.Internal;

namespace gfoidl.Base64
{
	// Token: 0x020000B1 RID: 177
	internal abstract class Base64 : IBase64
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000149AF File Offset: 0x00012BAF
		[Nullable(1)]
		public static Base64Encoder Default
		{
			[NullableContext(1)]
			get
			{
				return Base64.s_default;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x000149B6 File Offset: 0x00012BB6
		[Nullable(1)]
		public static Base64UrlEncoder Url
		{
			[NullableContext(1)]
			get
			{
				return Base64.s_url;
			}
		}

		// Token: 0x06000589 RID: 1417
		public abstract int GetEncodedLength(int sourceLength);

		// Token: 0x0600058A RID: 1418
		public abstract int GetMaxDecodedLength(int encodedLength);

		// Token: 0x0600058B RID: 1419
		public abstract int GetDecodedLength(System.ReadOnlySpan<byte> encoded);

		// Token: 0x0600058C RID: 1420
		public abstract int GetDecodedLength(System.ReadOnlySpan<char> encoded);

		// Token: 0x0600058D RID: 1421
		public abstract OperationStatus Encode(System.ReadOnlySpan<byte> data, System.Span<byte> encoded, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x0600058E RID: 1422
		public abstract OperationStatus Encode(System.ReadOnlySpan<byte> data, System.Span<char> encoded, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x0600058F RID: 1423
		public abstract OperationStatus Decode(System.ReadOnlySpan<byte> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x06000590 RID: 1424
		public abstract OperationStatus Decode(System.ReadOnlySpan<char> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x06000591 RID: 1425
		[return: Nullable(1)]
		public abstract string Encode(System.ReadOnlySpan<byte> data);

		// Token: 0x06000592 RID: 1426
		[return: Nullable(1)]
		public abstract byte[] Decode(System.ReadOnlySpan<char> encoded);

		// Token: 0x06000593 RID: 1427 RVA: 0x000149BD File Offset: 0x00012BBD
		public static EncodingType DetectEncoding(System.ReadOnlySpan<byte> encoded, bool fast = false)
		{
			return Base64.DetectEncoding<byte>(encoded, fast);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x000149C6 File Offset: 0x00012BC6
		public static EncodingType DetectEncoding(System.ReadOnlySpan<char> encoded, bool fast = false)
		{
			return Base64.DetectEncoding<char>(encoded, fast);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000149D0 File Offset: 0x00012BD0
		internal static EncodingType DetectEncoding<T>([Nullable(new byte[]
		{
			0,
			1
		})] System.ReadOnlySpan<T> encoded, bool fast = false) where T : IEquatable<T>
		{
			if (encoded.Length < 4)
			{
				return EncodingType.Unknown;
			}
			T value;
			T value2;
			T value3;
			T value4;
			if (typeof(T) == typeof(byte))
			{
				value = (T)((object)43);
				value2 = (T)((object)47);
				value3 = (T)((object)45);
				value4 = (T)((object)95);
			}
			else
			{
				if (!(typeof(T) == typeof(char)))
				{
					throw new NotSupportedException();
				}
				value = (T)((object)'+');
				value2 = (T)((object)'/');
				value3 = (T)((object)'-');
				value4 = (T)((object)'_');
			}
			int num = encoded.LastIndexOfAny(value3, value4);
			if (fast)
			{
				if (num < 0)
				{
					return EncodingType.Base64;
				}
				return EncodingType.Base64Url;
			}
			else
			{
				int num2 = encoded.LastIndexOfAny(value, value2);
				if (num < 0)
				{
					return EncodingType.Base64;
				}
				if (num2 < 0)
				{
					return EncodingType.Base64Url;
				}
				return EncodingType.Unknown;
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00014AC0 File Offset: 0x00012CC0
		// Note: this type is marked as 'beforefieldinit'.
		static Base64()
		{
			Base64.s_default = new Base64Encoder();
			Base64.s_url = new Base64UrlEncoder();
		}

		// Token: 0x040001E2 RID: 482
		public const int MaximumEncodeLength = 1610612733;

		// Token: 0x040001E3 RID: 483
		[Nullable(1)]
		private static readonly Base64Encoder s_default;

		// Token: 0x040001E4 RID: 484
		[Nullable(1)]
		private static readonly Base64UrlEncoder s_url;
	}
}
