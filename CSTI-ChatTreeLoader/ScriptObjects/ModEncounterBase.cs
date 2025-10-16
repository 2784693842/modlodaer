using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChatTreeLoader.ScriptObjects
{
	// Token: 0x02000008 RID: 8
	public abstract class ModEncounterBase : ScriptableObject
	{
		// Token: 0x06000011 RID: 17
		public abstract void Init();

		// Token: 0x06000012 RID: 18
		public abstract void OnEnable();

		// Token: 0x04000019 RID: 25
		public static readonly Dictionary<Type, List<ModEncounterBase>> AllModEncounters = new Dictionary<Type, List<ModEncounterBase>>();

		// Token: 0x0400001A RID: 26
		protected bool HadInit;
	}
}
