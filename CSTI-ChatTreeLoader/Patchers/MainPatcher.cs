using System;
using ChatTreeLoader.Behaviors;
using ChatTreeLoader.ScriptObjects;
using HarmonyLib;
using UnityEngine;

namespace ChatTreeLoader.Patchers
{
	// Token: 0x0200000E RID: 14
	public static class MainPatcher
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002684 File Offset: 0x00000884
		public static bool DoPatch(Harmony harmony)
		{
			try
			{
				harmony.PatchAll(typeof(MainPatcher));
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				return false;
			}
			return true;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000026C0 File Offset: 0x000008C0
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EncounterPopup), "DisplayPlayerActions")]
		public static bool DisplayModEncounter(EncounterPopup __instance)
		{
			return __instance.DisplayModEncounterEx<ModEncounter>() && __instance.DisplayModEncounterEx<SimpleTraderEncounter>();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000026D2 File Offset: 0x000008D2
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EncounterPopup), "DoPlayerAction")]
		public static bool DoModPlayerAction(EncounterPopup __instance, int _Action)
		{
			return __instance.DoModPlayerActionEx(_Action) && __instance.DoModPlayerActionEx(_Action);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000026E6 File Offset: 0x000008E6
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EncounterPopup), "RoundStart")]
		public static bool ModRoundStart(bool _Loaded, EncounterPopup __instance)
		{
			return __instance.ModRoundStartEx(_Loaded) && __instance.ModRoundStartEx(_Loaded);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000026FA File Offset: 0x000008FA
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EncounterPopup), "Update")]
		public static void UpdateMod(EncounterPopup __instance)
		{
			__instance.UpdateModEx<ModEncounter>();
			__instance.UpdateModEx<SimpleTraderEncounter>();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002708 File Offset: 0x00000908
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EncounterPopup), "CalculateStealthChecks")]
		public static bool CalculateModStealthChecks(EncounterPopup __instance)
		{
			return !__instance.CheckEnable<ModEncounter>() && !__instance.CheckEnable<SimpleTraderEncounter>();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000271D File Offset: 0x0000091D
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EncounterPopup), "ApplyEncounterResult")]
		public static bool ApplyEncounterResultMod(EncounterPopup __instance, out bool __result)
		{
			__result = false;
			if (!__instance.CheckEnable<ModEncounter>() && !__instance.CheckEnable<SimpleTraderEncounter>())
			{
				return true;
			}
			if (__instance.CurrentEncounter.EncounterResult == EncounterResult.Ongoing)
			{
				return false;
			}
			__result = true;
			return false;
		}
	}
}
