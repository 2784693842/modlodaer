using System;
using System.Collections.Generic;

namespace NLua
{
	// Token: 0x02000069 RID: 105
	internal class LuaGlobalEntry
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000B6C8 File Offset: 0x000098C8
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000B6D0 File Offset: 0x000098D0
		public Type Type { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000B6D9 File Offset: 0x000098D9
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000B6E1 File Offset: 0x000098E1
		public string Path { get; private set; }

		// Token: 0x0600026D RID: 621 RVA: 0x0000B6EA File Offset: 0x000098EA
		public LuaGlobalEntry(Type type, string path)
		{
			this.linkedGlobals = new List<string>();
			base..ctor();
			this.Type = type;
			this.Path = path;
		}

		// Token: 0x0400010E RID: 270
		public List<string> linkedGlobals;
	}
}
