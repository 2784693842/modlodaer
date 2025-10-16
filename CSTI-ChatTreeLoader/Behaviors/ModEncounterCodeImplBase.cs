using System;
using System.Collections.Generic;
using ChatTreeLoader.ScriptObjects;

namespace ChatTreeLoader.Behaviors
{
	// Token: 0x02000016 RID: 22
	public abstract class ModEncounterCodeImplBase
	{
		// Token: 0x06000049 RID: 73
		public abstract void DisplayChatModEncounter(EncounterPopup __instance);

		// Token: 0x0600004A RID: 74
		public abstract void DoModPlayerAction(EncounterPopup __instance, int _Action);

		// Token: 0x0600004B RID: 75
		public abstract void ModRoundStart(EncounterPopup __instance, bool _Loaded);

		// Token: 0x0600004C RID: 76
		public abstract void UpdateModEx(EncounterPopup __instance);

		// Token: 0x04000036 RID: 54
		public static readonly Dictionary<Type, ModEncounterCodeImplBase> AllImpls = new Dictionary<Type, ModEncounterCodeImplBase>
		{
			{
				typeof(ModEncounter),
				new ChatEncounterExt()
			},
			{
				typeof(SimpleTraderEncounter),
				new TraderEncounterExt()
			}
		};

		// Token: 0x04000037 RID: 55
		public bool IsRunning;
	}
}
