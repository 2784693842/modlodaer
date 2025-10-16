using System;

namespace NLua.Method
{
	// Token: 0x0200009F RID: 159
	internal class LuaDelegate
	{
		// Token: 0x060004A3 RID: 1187 RVA: 0x00013B15 File Offset: 0x00011D15
		public LuaDelegate()
		{
			this.Function = null;
			this.ReturnTypes = null;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00013B2C File Offset: 0x00011D2C
		public object CallFunction(object[] args, object[] inArgs, int[] outArgs)
		{
			object[] array = this.Function.Call(inArgs, this.ReturnTypes);
			object result;
			int num;
			if (this.ReturnTypes[0] == typeof(void))
			{
				result = null;
				num = 0;
			}
			else
			{
				result = array[0];
				num = 1;
			}
			for (int i = 0; i < outArgs.Length; i++)
			{
				args[outArgs[i]] = array[num];
				num++;
			}
			return result;
		}

		// Token: 0x040001CA RID: 458
		public LuaFunction Function;

		// Token: 0x040001CB RID: 459
		public Type[] ReturnTypes;
	}
}
