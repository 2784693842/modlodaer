using System;

namespace ModLoader
{
	// Token: 0x02000010 RID: 16
	public static class StringHelper
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002C50 File Offset: 0x00000E50
		public static string EscapeStr(this string s)
		{
			return s.Replace("'", string.Format("?u{0:x}", 39)).Replace("\"", string.Format("?u{0:x}", 34)).Replace("[", string.Format("?u{0:x}", 91)).Replace("]", string.Format("?u{0:x}", 93)).Replace("\\", string.Format("?u{0:x}", 92)).Replace("\n", string.Format("?u{0:x}", 10)).Replace("\t", string.Format("?u{0:x}", 9));
		}
	}
}
