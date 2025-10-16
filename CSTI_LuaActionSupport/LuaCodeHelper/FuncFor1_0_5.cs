using System;
using System.Collections;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.AllPatcher;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200003B RID: 59
	[NullableContext(1)]
	[Nullable(0)]
	public static class FuncFor1_0_5
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00005B5C File Offset: 0x00003D5C
		public static void GenEncounter(UniqueIDScriptable uniqueIDScriptable)
		{
			Encounter encounter = uniqueIDScriptable as Encounter;
			if (encounter == null)
			{
				return;
			}
			MBSingleton<GameManager>.Instance.GameGraphics.EncounterPopupWindow.StartEncounter(encounter, MBSingleton<GameManager>.Instance.CurrentSaveData.HasEncounterData);
			CardActionPatcher.Enumerators.Add(FuncFor1_0_5.WaitEncounter(encounter));
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005BA8 File Offset: 0x00003DA8
		private static IEnumerator WaitEncounter(Encounter encounter)
		{
			while (MBSingleton<GameManager>.Instance.GameGraphics.EncounterPopupWindow.OngoingEncounter)
			{
				yield return null;
			}
			CoroutineController controller;
			MBSingleton<GameManager>.Instance.StartCoroutineEx(MBSingleton<GameManager>.Instance.CheckAllStatsForActions(), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			yield break;
		}
	}
}
