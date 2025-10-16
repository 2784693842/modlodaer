using System;

namespace LitJson
{
	// Token: 0x02000006 RID: 6
	public class JsonException : ApplicationException
	{
		// Token: 0x0600007D RID: 125 RVA: 0x00003089 File Offset: 0x00001289
		public JsonException()
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003091 File Offset: 0x00001291
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000030A9 File Offset: 0x000012A9
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000030C2 File Offset: 0x000012C2
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000030DB File Offset: 0x000012DB
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000030F5 File Offset: 0x000012F5
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000030FE File Offset: 0x000012FE
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
