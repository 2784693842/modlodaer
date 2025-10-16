using System;
using KeraLua;

namespace NLua.Event
{
	// Token: 0x02000063 RID: 99
	internal class DebugHookEventArgs : EventArgs
	{
		// Token: 0x0600024E RID: 590 RVA: 0x0000B10E File Offset: 0x0000930E
		public DebugHookEventArgs(LuaDebug luaDebug)
		{
			this.LuaDebug = luaDebug;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000B11D File Offset: 0x0000931D
		public LuaDebug LuaDebug { get; }
	}
}
