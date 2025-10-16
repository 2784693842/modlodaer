using System;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000027 RID: 39
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	public class TestCodeAttribute : Attribute
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000216A File Offset: 0x0000036A
		[NullableContext(1)]
		public TestCodeAttribute(string code)
		{
		}
	}
}
