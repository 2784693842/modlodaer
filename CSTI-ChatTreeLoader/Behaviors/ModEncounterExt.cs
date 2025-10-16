using System;
using System.Collections.Generic;
using ChatTreeLoader.ScriptObjects;
using UnityEngine;

namespace ChatTreeLoader.Behaviors
{
	// Token: 0x02000015 RID: 21
	public static class ModEncounterExt
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003648 File Offset: 0x00001848
		public static void RegAllEncounter<T>() where T : ModEncounterTypedBase<T>
		{
			if (!ModEncounterBase.AllModEncounters.ContainsKey(typeof(T)))
			{
				return;
			}
			foreach (ModEncounterBase modEncounterBase in ModEncounterBase.AllModEncounters[typeof(T)])
			{
				modEncounterBase.Init();
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000036C0 File Offset: 0x000018C0
		public static bool CheckId<T>(string id) where T : ModEncounterTypedBase<T>
		{
			if (!ModEncounterBase.AllModEncounters.ContainsKey(typeof(T)))
			{
				return false;
			}
			List<ModEncounterBase> list = ModEncounterBase.AllModEncounters[typeof(T)];
			return list.Count != 0 && ((ModEncounterTypedBase<T>)list[0]).GetValidEncounterTable().ContainsKey(id);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000371C File Offset: 0x0000191C
		public static bool DisplayModEncounterEx<T>(this EncounterPopup __instance) where T : ModEncounterTypedBase<T>
		{
			if (!__instance.CheckEnable<T>())
			{
				return true;
			}
			ModEncounterCodeImplBase modEncounterCodeImplBase;
			if (ModEncounterCodeImplBase.AllImpls.TryGetValue(typeof(T), out modEncounterCodeImplBase))
			{
				modEncounterCodeImplBase.DisplayChatModEncounter(__instance);
			}
			return false;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003754 File Offset: 0x00001954
		public static bool DoModPlayerActionEx<T>(this EncounterPopup __instance, int _Action) where T : ModEncounterTypedBase<T>
		{
			if (!__instance.CheckEnable<T>())
			{
				return true;
			}
			ModEncounterCodeImplBase modEncounterCodeImplBase;
			if (ModEncounterCodeImplBase.AllImpls.TryGetValue(typeof(T), out modEncounterCodeImplBase))
			{
				modEncounterCodeImplBase.DoModPlayerAction(__instance, _Action);
			}
			return false;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000378C File Offset: 0x0000198C
		public static bool ModRoundStartEx<T>(this EncounterPopup __instance, bool _Loaded) where T : ModEncounterTypedBase<T>
		{
			if (!__instance.CheckEnable<T>())
			{
				return true;
			}
			ModEncounterCodeImplBase modEncounterCodeImplBase;
			if (ModEncounterCodeImplBase.AllImpls.TryGetValue(typeof(T), out modEncounterCodeImplBase))
			{
				modEncounterCodeImplBase.ModRoundStart(__instance, _Loaded);
			}
			return false;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000037C4 File Offset: 0x000019C4
		public static bool CheckEnable<T>(this EncounterPopup __instance) where T : ModEncounterTypedBase<T>
		{
			ModEncounterExt.RegAllEncounter<T>();
			return ModEncounterExt.CheckId<T>(__instance.CurrentEncounter.EncounterModel.UniqueID);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000037E0 File Offset: 0x000019E0
		public static void CheckButtonCountAndEnable(this EncounterPopup __instance, int count, Action<EncounterPopup, EncounterOptionButton, int> initAct)
		{
			while (__instance.ActionButtons.Count < count)
			{
				EncounterOptionButton encounterOptionButton = UnityEngine.Object.Instantiate<EncounterOptionButton>(__instance.ActionButtonPrefab, __instance.ActionButtonsParent);
				__instance.ActionButtons.Add(encounterOptionButton);
				encounterOptionButton.OnClicked = (Action<int>)Delegate.Combine(encounterOptionButton.OnClicked, new Action<int>(__instance.DoPlayerAction));
			}
			for (int i = 0; i < __instance.ActionButtons.Count; i++)
			{
				if (i >= count)
				{
					__instance.ActionButtons[i].gameObject.SetActive(false);
				}
				else
				{
					initAct(__instance, __instance.ActionButtons[i], i);
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003884 File Offset: 0x00001A84
		public static void UpdateModEx<T>(this EncounterPopup __instance)
		{
			ModEncounterCodeImplBase modEncounterCodeImplBase;
			if (ModEncounterCodeImplBase.AllImpls.TryGetValue(typeof(T), out modEncounterCodeImplBase) && modEncounterCodeImplBase.IsRunning)
			{
				modEncounterCodeImplBase.UpdateModEx(__instance);
			}
		}
	}
}
