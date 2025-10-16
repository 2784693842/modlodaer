using System;
using System.Text;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils
{
	// Token: 0x020000C5 RID: 197
	internal static class ByteArrayExtensions
	{
		// Token: 0x06000462 RID: 1122 RVA: 0x00015840 File Offset: 0x00013A40
		public static bool IsEntirelyNull(byte[] buffer)
		{
			for (int i = 0; i < buffer.Length; i++)
			{
				if (buffer[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00015868 File Offset: 0x00013A68
		public static string DescribeAsHex(byte[] buffer, string separator, int bytesPerLine)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (byte b in buffer)
			{
				stringBuilder.AppendFormat("{0:X2}{1}", b, separator);
				if (++num % bytesPerLine == 0)
				{
					stringBuilder.Append("\r\n");
				}
			}
			stringBuilder.Append("\r\n");
			return stringBuilder.ToString();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000158D0 File Offset: 0x00013AD0
		public static string DecodeAsString(byte[] buffer, int offset, int length, Encoding encoding)
		{
			for (int i = 0; i < length; i++)
			{
				if (buffer[offset + i] == 0)
				{
					length = i;
				}
			}
			return encoding.GetString(buffer, offset, length);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000158FC File Offset: 0x00013AFC
		public static byte[] Concat(params byte[][] byteArrays)
		{
			int num = 0;
			foreach (byte[] array in byteArrays)
			{
				num += array.Length;
			}
			if (num <= 0)
			{
				return new byte[0];
			}
			byte[] array2 = new byte[num];
			int num2 = 0;
			foreach (byte[] array3 in byteArrays)
			{
				Array.Copy(array3, 0, array2, num2, array3.Length);
				num2 += array3.Length;
			}
			return array2;
		}
	}
}
