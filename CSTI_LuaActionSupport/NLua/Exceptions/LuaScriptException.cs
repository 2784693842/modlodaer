using System;

namespace NLua.Exceptions
{
	// Token: 0x02000089 RID: 137
	[Serializable]
	internal class LuaScriptException : LuaException
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001121B File Offset: 0x0000F41B
		public bool IsNetException { get; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00011223 File Offset: 0x0000F423
		public override string Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001122B File Offset: 0x0000F42B
		public LuaScriptException(string message, string source) : base(message)
		{
			this._source = source;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001123B File Offset: 0x0000F43B
		public LuaScriptException(Exception innerException, string source) : base("A .NET exception occurred in user-code", innerException)
		{
			this._source = source;
			this.IsNetException = 1;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00011257 File Offset: 0x0000F457
		public override string ToString()
		{
			return base.GetType().FullName + ": " + this._source + this.Message;
		}

		// Token: 0x0400018E RID: 398
		private readonly string _source;
	}
}
