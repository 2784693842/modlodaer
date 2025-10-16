using System;

namespace NLua.Exceptions
{
	// Token: 0x02000088 RID: 136
	[Serializable]
	internal class LuaException : Exception
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x00011208 File Offset: 0x0000F408
		public LuaException(string message) : base(message)
		{
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00011211 File Offset: 0x0000F411
		public LuaException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
