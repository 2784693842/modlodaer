using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio
{
	// Token: 0x020000C4 RID: 196
	internal class MmException : Exception
	{
		// Token: 0x0600045E RID: 1118 RVA: 0x000157F9 File Offset: 0x000139F9
		public MmException(MmResult result, string function) : base(MmException.ErrorMessage(result, function))
		{
			this.result = result;
			this.function = function;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00015816 File Offset: 0x00013A16
		private static string ErrorMessage(MmResult result, string function)
		{
			return string.Format("{0} calling {1}", result, function);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00015829 File Offset: 0x00013A29
		public static void Try(MmResult result, string function)
		{
			if (result != MmResult.NoError)
			{
				throw new MmException(result, function);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00015836 File Offset: 0x00013A36
		public MmResult Result
		{
			get
			{
				return this.result;
			}
		}

		// Token: 0x040004C4 RID: 1220
		private MmResult result;

		// Token: 0x040004C5 RID: 1221
		private string function;
	}
}
