using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02000020 RID: 32
	internal class SharedUtils
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x0000AE4E File Offset: 0x0000904E
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000AE58 File Offset: 0x00009058
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000AE99 File Offset: 0x00009099
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000AEA6 File Offset: 0x000090A6
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
