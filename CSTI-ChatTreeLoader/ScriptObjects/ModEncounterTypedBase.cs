using System;
using System.Collections.Generic;

namespace ChatTreeLoader.ScriptObjects
{
	// Token: 0x02000009 RID: 9
	public abstract class ModEncounterTypedBase<T> : ModEncounterBase where T : ModEncounterTypedBase<T>
	{
		// Token: 0x06000015 RID: 21
		public abstract Dictionary<string, T> GetValidEncounterTable();
	}
}
