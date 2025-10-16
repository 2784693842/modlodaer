using System;
using KeraLua;

namespace NLua
{
	// Token: 0x020000A3 RID: 163
	internal class ClassGenerator
	{
		// Token: 0x060004AB RID: 1195 RVA: 0x00013C45 File Offset: 0x00011E45
		public ClassGenerator(ObjectTranslator objTranslator, Type typeClass)
		{
			this._translator = objTranslator;
			this._klass = typeClass;
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00013C5B File Offset: 0x00011E5B
		public object ExtractGenerated(Lua luaState, int stackPos)
		{
			return CodeGeneration.Instance.GetClassInstance(this._klass, this._translator.GetTable(luaState, stackPos));
		}

		// Token: 0x040001CE RID: 462
		private readonly ObjectTranslator _translator;

		// Token: 0x040001CF RID: 463
		private readonly Type _klass;
	}
}
