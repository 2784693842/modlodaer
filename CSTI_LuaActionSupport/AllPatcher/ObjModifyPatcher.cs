using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CSTI_LuaActionSupport.LuaCodeHelper;
using HarmonyLib;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.AllPatcher
{
	// Token: 0x02000054 RID: 84
	[NullableContext(1)]
	[Nullable(0)]
	[HarmonyPatch]
	public static class ObjModifyPatcher
	{
		// Token: 0x060001B8 RID: 440 RVA: 0x00008A94 File Offset: 0x00006C94
		[HarmonyPostfix]
		[HarmonyPatch(typeof(DismantleActionButton), "Setup")]
		public static void DismantleActionButton_PostSetup(DismantleActionButton __instance, int _Index, DismantleCardAction _Action, InGameCardBase _Card, bool _Highlighted, bool _StackVersion)
		{
			Match match = ObjModifyPatcher.LuaDesc.Match(__instance.Text);
			if (match == null || !match.Success)
			{
				return;
			}
			string value = match.Groups["luaCode"].Value;
			Lua lua = CardActionPatcher.InitRuntime(MBSingleton<GameManager>.Instance);
			lua["receive"] = new CardAccessBridge(_Card);
			CardActionPatcher.ModData["Args__instance"] = __instance;
			CardActionPatcher.ModData["Args__Index"] = _Index;
			CardActionPatcher.ModData["Args__Action"] = _Action;
			CardActionPatcher.ModData["Args__Highlighted"] = _Highlighted;
			CardActionPatcher.ModData["Args__StackVersion"] = _StackVersion;
			LuaScriptRetValues luaScriptRetValues = new LuaScriptRetValues();
			lua["Ret"] = luaScriptRetValues;
			try
			{
				lua.DoString(value, "LuaCodeActionName");
				string text;
				if (luaScriptRetValues.CheckKey<string>("ret", out text))
				{
					__instance.Text = (text ?? __instance.Text);
				}
				bool? flag;
				if (luaScriptRetValues.CheckKey<bool?>("show", out flag))
				{
					__instance.gameObject.SetActive(flag ?? __instance.gameObject.activeInHierarchy);
				}
				bool? flag2;
				if (luaScriptRetValues.CheckKey<bool?>("canUse", out flag2))
				{
					__instance.ConditionsValid = (flag2 ?? __instance.ConditionsValid);
					__instance.Interactable = (flag2 ?? __instance.Interactable);
				}
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008C38 File Offset: 0x00006E38
		[HarmonyPostfix]
		[HarmonyPatch(typeof(InGameCardBase), "CardDescription")]
		public static void ModifyCardDescription(InGameCardBase __instance, bool _IgnoreLiquid, ref string __result)
		{
			string uniqueID = __instance.CardModel.UniqueID;
			CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge = CardActionPatcher.LoadCurrentSlot("zender.SimpleUniqueAccess") as CardActionPatcher.DataNodeTableAccessBridge;
			if (dataNodeTableAccessBridge != null)
			{
				CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge2 = dataNodeTableAccessBridge[uniqueID] as CardActionPatcher.DataNodeTableAccessBridge;
				if (dataNodeTableAccessBridge2 != null)
				{
					if (!__instance.ContainedLiquid || _IgnoreLiquid)
					{
						__result = ((dataNodeTableAccessBridge2["CardDescription"] as string) ?? __result);
					}
					else if (__instance.ContainedLiquid)
					{
						Dictionary<string, DataNode> table = dataNodeTableAccessBridge.Table;
						bool? flag = (table != null) ? new bool?(table.ContainsKey(__instance.ContainedLiquid.CardModel.UniqueID)) : null;
						if (flag != null && flag.GetValueOrDefault())
						{
							CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge3 = dataNodeTableAccessBridge[__instance.ContainedLiquid.CardModel.UniqueID] as CardActionPatcher.DataNodeTableAccessBridge;
							if (dataNodeTableAccessBridge3 != null)
							{
								__result = ((dataNodeTableAccessBridge3["CardDescription"] as string) ?? __result);
							}
						}
					}
				}
			}
			Match match = ObjModifyPatcher.LuaDesc.Match(__result);
			if (match == null || !match.Success)
			{
				return;
			}
			string value = match.Groups["luaCode"].Value;
			Lua lua = CardActionPatcher.InitRuntime(MBSingleton<GameManager>.Instance);
			lua["receive"] = new CardAccessBridge(__instance);
			LuaScriptRetValues luaScriptRetValues = new LuaScriptRetValues();
			lua["Ret"] = luaScriptRetValues;
			try
			{
				lua.DoString(value, "LuaCodeDesc");
				string text;
				if (luaScriptRetValues.CheckKey<string>("ret", out text))
				{
					__result = (text ?? __result);
				}
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
			}
		}

		// Token: 0x040000CF RID: 207
		public static readonly Regex LuaDesc = new Regex("^\\#\\#\\#luaAction CardDescription\\n(?<luaCode>[\\s\\S]*?)\\n\\#\\#\\#$");

		// Token: 0x02000055 RID: 85
		[Nullable(0)]
		[HarmonyPatch(typeof(GameManager))]
		public static class Patch_GameManager_AddCard
		{
			// Token: 0x060001BB RID: 443 RVA: 0x00008DED File Offset: 0x00006FED
			[HarmonyTargetMethod]
			public static MethodBase FindTargetAddCard()
			{
				return AccessTools.GetDeclaredMethods(typeof(GameManager)).First((MethodInfo info) => info.Name == "AddCard" && info.GetParameters().Length > 16);
			}

			// Token: 0x060001BC RID: 444 RVA: 0x00008E22 File Offset: 0x00007022
			[HarmonyPostfix]
			public static void MoniRawAddCard(ref IEnumerator __result)
			{
				if (MoniEnum.OnMoniAddCard)
				{
					__result = MoniEnum.MoniFunc(__result);
				}
			}
		}
	}
}
