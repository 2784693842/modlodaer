using System;
using System.Reflection;

namespace NLua.Method
{
	// Token: 0x02000081 RID: 129
	internal class MethodCache
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0001018B File Offset: 0x0000E38B
		public MethodCache()
		{
			this.args = new object[0];
			this.argTypes = new MethodArgs[0];
			this.outList = new int[0];
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060003DA RID: 986 RVA: 0x000101B7 File Offset: 0x0000E3B7
		// (set) Token: 0x060003DB RID: 987 RVA: 0x000101C0 File Offset: 0x0000E3C0
		public MethodBase cachedMethod
		{
			get
			{
				return this._cachedMethod;
			}
			set
			{
				this._cachedMethod = value;
				MethodInfo methodInfo = value as MethodInfo;
				if (methodInfo != null)
				{
					this.IsReturnVoid = (methodInfo.ReturnType == typeof(void));
				}
			}
		}

		// Token: 0x0400017F RID: 383
		private MethodBase _cachedMethod;

		// Token: 0x04000180 RID: 384
		public bool IsReturnVoid;

		// Token: 0x04000181 RID: 385
		public object[] args;

		// Token: 0x04000182 RID: 386
		public int[] outList;

		// Token: 0x04000183 RID: 387
		public MethodArgs[] argTypes;
	}
}
