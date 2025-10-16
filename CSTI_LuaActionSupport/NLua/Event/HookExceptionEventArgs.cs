using System;

namespace NLua.Event
{
	// Token: 0x02000062 RID: 98
	internal class HookExceptionEventArgs : EventArgs
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000B0F7 File Offset: 0x000092F7
		public Exception Exception { get; }

		// Token: 0x0600024D RID: 589 RVA: 0x0000B0FF File Offset: 0x000092FF
		public HookExceptionEventArgs(Exception ex)
		{
			this.Exception = ex;
		}
	}
}
