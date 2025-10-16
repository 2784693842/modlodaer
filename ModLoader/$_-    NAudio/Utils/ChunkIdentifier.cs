using System;
using System.Text;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils
{
	// Token: 0x02000041 RID: 65
	internal class ChunkIdentifier
	{
		// Token: 0x06000131 RID: 305 RVA: 0x0000B7D9 File Offset: 0x000099D9
		public static int ChunkIdentifierToInt32(string s)
		{
			if (s.Length != 4)
			{
				throw new ArgumentException("Must be a four character string");
			}
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			if (bytes.Length != 4)
			{
				throw new ArgumentException("Must encode to exactly four bytes");
			}
			return BitConverter.ToInt32(bytes, 0);
		}
	}
}
