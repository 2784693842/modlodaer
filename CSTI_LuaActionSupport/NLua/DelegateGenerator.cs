using System;
using KeraLua;

namespace NLua
{
	// Token: 0x020000A2 RID: 162
	internal class DelegateGenerator
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x00013C10 File Offset: 0x00011E10
		public DelegateGenerator(ObjectTranslator objectTranslator, Type type)
		{
			this._translator = objectTranslator;
			this._delegateType = type;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00013C26 File Offset: 0x00011E26
		public object ExtractGenerated(Lua luaState, int stackPos)
		{
			return CodeGeneration.Instance.GetDelegate(this._delegateType, this._translator.GetFunction(luaState, stackPos));
		}

		// Token: 0x040001CC RID: 460
		private readonly ObjectTranslator _translator;

		// Token: 0x040001CD RID: 461
		private readonly Type _delegateType;
	}
}
