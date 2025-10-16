using System;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000026 RID: 38
	[NullableContext(2)]
	[Nullable(0)]
	[AttributeUsage(AttributeTargets.Method)]
	public class LuaFuncAttribute : Attribute
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004F12 File Offset: 0x00003112
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004F1A File Offset: 0x0000311A
		public string FuncName { get; set; }
	}
}
