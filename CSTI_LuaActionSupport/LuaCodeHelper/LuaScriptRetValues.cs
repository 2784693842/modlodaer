using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200002C RID: 44
	[NullableContext(1)]
	[Nullable(0)]
	public class LuaScriptRetValues
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x00005244 File Offset: 0x00003444
		[NullableContext(2)]
		public bool CheckKey<TVal>([Nullable(1)] string key, out TVal value)
		{
			object obj;
			if (this.Values.TryGetValue(key, out obj) && obj is TVal)
			{
				TVal tval = (TVal)((object)obj);
				value = tval;
				return true;
			}
			value = default(TVal);
			return false;
		}

		// Token: 0x1700002F RID: 47
		[Nullable(2)]
		public object this[string key]
		{
			[return: Nullable(2)]
			get
			{
				object result;
				this.Values.TryGetValue(key, out result);
				return result;
			}
			[param: Nullable(2)]
			set
			{
				this.Values[key] = value;
			}
		}

		// Token: 0x04000057 RID: 87
		[Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private readonly Dictionary<string, object> Values = new Dictionary<string, object>();
	}
}
