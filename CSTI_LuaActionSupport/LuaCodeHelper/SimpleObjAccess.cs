using System;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200003E RID: 62
	[NullableContext(1)]
	[Nullable(0)]
	public class SimpleObjAccess : CommonSimpleAccess
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00005DCE File Offset: 0x00003FCE
		public override object AccessObj
		{
			get
			{
				return this._AccessObj;
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005DD6 File Offset: 0x00003FD6
		public SimpleObjAccess(object accessObj)
		{
			this._AccessObj = accessObj;
		}

		// Token: 0x04000089 RID: 137
		public readonly object _AccessObj;
	}
}
