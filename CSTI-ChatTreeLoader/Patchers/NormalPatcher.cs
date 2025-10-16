using System;
using HarmonyLib;

namespace ChatTreeLoader.Patchers
{
	// Token: 0x0200000F RID: 15
	public static class NormalPatcher
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002747 File Offset: 0x00000947
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000274E File Offset: 0x0000094E
		public static bool ShouldWaitExtra { get; set; }

		// Token: 0x0600002F RID: 47 RVA: 0x00002756 File Offset: 0x00000956
		public static void DoPatch(Harmony harmony)
		{
			NormalPatcher.HarmonyInstance = harmony;
			if (NormalPatcher.HarmonyInstance == null)
			{
				return;
			}
			NormalPatcher.HarmonyInstance.PatchAll(typeof(ExtraStatImpl));
			NormalPatcher.HarmonyInstance.PatchAll(typeof(NormalPatcher));
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000278E File Offset: 0x0000098E
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "PerformingAction", MethodType.Getter)]
		public static void WaitExtraAct(ref bool __result)
		{
			__result |= NormalPatcher.ShouldWaitExtra;
		}

		// Token: 0x0400002E RID: 46
		public static Harmony HarmonyInstance;
	}
}
