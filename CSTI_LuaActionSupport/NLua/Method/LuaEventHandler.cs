using System;

namespace NLua.Method
{
	// Token: 0x0200009E RID: 158
	internal class LuaEventHandler
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x00013B06 File Offset: 0x00011D06
		public void HandleEvent(object[] args)
		{
			this.Handler.Call(args);
		}

		// Token: 0x040001C9 RID: 457
		public LuaFunction Handler;
	}
}
