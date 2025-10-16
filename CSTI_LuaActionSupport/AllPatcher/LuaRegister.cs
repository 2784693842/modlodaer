using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.Helper;
using CSTI_LuaActionSupport.LuaCodeHelper;
using HarmonyLib;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.AllPatcher
{
	// Token: 0x0200004F RID: 79
	[NullableContext(1)]
	[Nullable(0)]
	public class LuaRegister
	{
		// Token: 0x06000198 RID: 408 RVA: 0x00007D83 File Offset: 0x00005F83
		private LuaRegister()
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007D98 File Offset: 0x00005F98
		public bool TryGet(string klass, string method, out Dictionary<string, List<LuaFunction>> regs)
		{
			Dictionary<string, Dictionary<string, List<LuaFunction>>> dictionary;
			if (this.AllReg.TryGetValue(klass, out dictionary) && dictionary.TryGetValue(method, out regs))
			{
				return true;
			}
			regs = null;
			return false;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007DC8 File Offset: 0x00005FC8
		public bool TryGet(string klass, string method, string uid, out List<LuaFunction> regs)
		{
			Dictionary<string, Dictionary<string, List<LuaFunction>>> dictionary;
			Dictionary<string, List<LuaFunction>> dictionary2;
			if (this.AllReg.TryGetValue(klass, out dictionary) && dictionary.TryGetValue(method, out dictionary2) && dictionary2.TryGetValue(uid, out regs))
			{
				return true;
			}
			regs = null;
			return false;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007E04 File Offset: 0x00006004
		public void Reg(string klass, string method, string uid, LuaFunction function)
		{
			if (!this.AllReg.ContainsKey(klass))
			{
				this.AllReg[klass] = new Dictionary<string, Dictionary<string, List<LuaFunction>>>();
			}
			if (!this.AllReg[klass].ContainsKey(method))
			{
				this.AllReg[klass][method] = new Dictionary<string, List<LuaFunction>>();
			}
			if (!this.AllReg[klass][method].ContainsKey(uid))
			{
				this.AllReg[klass][method][uid] = new List<LuaFunction>();
			}
			this.AllReg[klass][method][uid].Add(function);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007EB4 File Offset: 0x000060B4
		[HarmonyPostfix]
		[HarmonyPatch(typeof(CardOnCardAction), "CardsAndTagsAreCorrect")]
		public static void Lua_CardOnCardAction_CardsAndTagsAreCorrect(CardOnCardAction __instance, InGameCardBase _Receiving, InGameCardBase _Given, ref bool __result)
		{
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("CardOnCardAction", "CardsAndTagsAreCorrect", __instance.ActionName.LocalizationKey, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						__instance,
						new CardAccessBridge(_Receiving),
						new CardAccessBridge(_Given),
						__result
					});
					if (array.Length != 0)
					{
						object obj = array[0];
						if (obj is bool)
						{
							bool flag = (bool)obj;
							__result = flag;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007F80 File Offset: 0x00006180
		[HarmonyPostfix]
		[HarmonyPatch(typeof(CardAction), "CardsAndTagsAreCorrect")]
		public static void Lua_CardAction_CardsAndTagsAreCorrect(CardAction __instance, InGameCardBase _ForCard, ref bool __result)
		{
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("CardAction", "CardsAndTagsAreCorrect", __instance.ActionName.LocalizationKey, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						__instance,
						new CardAccessBridge(_ForCard),
						__result
					});
					if (array.Length != 0)
					{
						object obj = array[0];
						if (obj is bool)
						{
							bool flag = (bool)obj;
							__result = flag;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008044 File Offset: 0x00006244
		[HarmonyPostfix]
		[HarmonyPatch(typeof(InGameStat), "CurrentValue")]
		public static void LuaGameStatValue(InGameStat __instance, bool _NotAtBase, ref float __result)
		{
			if (__instance == null || __instance.StatModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("InGameStat", "CurrentValue", __instance.StatModel.UniqueID, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						new GameStatAccessBridge(__instance),
						new SimpleUniqueAccess(__instance.StatModel),
						__result,
						_NotAtBase
					});
					if (array.Length != 0)
					{
						object obj = array[0];
						if (obj != null)
						{
							float? num = obj.TryNum<float>();
							if (num != null)
							{
								float valueOrDefault = num.GetValueOrDefault();
								__result = valueOrDefault;
							}
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008140 File Offset: 0x00006340
		[HarmonyPostfix]
		[HarmonyPatch(typeof(InGameCardBase), "CanReceiveInInventory")]
		public static void LuaCanReceiveInInventory(InGameCardBase __instance, CardData _Card, CardData _WithLiquid, ref bool __result)
		{
			if (__instance == null || __instance.CardModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("InGameCardBase", "CanReceiveInInventory", __instance.CardModel.UniqueID, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						new CardAccessBridge(__instance),
						new SimpleUniqueAccess(_Card),
						new SimpleUniqueAccess(_WithLiquid)
					});
					if (array.Length != 0)
					{
						object obj = array[0];
						if (obj is bool)
						{
							bool flag = (bool)obj;
							__result = flag;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008220 File Offset: 0x00006420
		[HarmonyPostfix]
		[HarmonyPatch(typeof(InGameCardBase), "CanReceiveInInventoryInstance")]
		public static void LuaCanReceiveInInventoryInstance(InGameCardBase __instance, InGameCardBase _Card, ref bool __result)
		{
			if (__instance == null || __instance.CardModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("InGameCardBase", "CanReceiveInInventoryInstance", __instance.CardModel.UniqueID, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						new CardAccessBridge(__instance),
						new CardAccessBridge(_Card)
					});
					if (array.Length != 0)
					{
						object obj = array[0];
						if (obj is bool)
						{
							bool flag = (bool)obj;
							__result = flag;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000082F8 File Offset: 0x000064F8
		[HarmonyPostfix]
		[HarmonyPatch(typeof(InGameCardBase), "InventoryWeight")]
		public static void LuaInventoryWeight(InGameCardBase __instance, ref float __result)
		{
			if (__instance == null || __instance.CardModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("InGameCardBase", "InventoryWeight", __instance.CardModel.UniqueID, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						new CardAccessBridge(__instance),
						__result
					});
					if (array.Length != 0)
					{
						ref __result.TryModBy(array[0]);
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000083C0 File Offset: 0x000065C0
		[HarmonyPostfix]
		[HarmonyPatch(typeof(InGameCardBase), "CardName")]
		public static void LuaCardName(InGameCardBase __instance, bool _IgnoreLiquid, ref string __result)
		{
			if (__instance == null || __instance.CardModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("InGameCardBase", "CardName", __instance.CardModel.UniqueID, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						new CardAccessBridge(__instance),
						_IgnoreLiquid
					});
					if (array.Length != 0)
					{
						string text = array[0] as string;
						if (text != null)
						{
							__result = text;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008490 File Offset: 0x00006690
		[HarmonyPostfix]
		[HarmonyPatch(typeof(InGameCardBase), "CardDescription")]
		public static void LuaCardDescription(InGameCardBase __instance, bool _IgnoreLiquid, ref string __result)
		{
			if (__instance == null || __instance.CardModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("InGameCardBase", "CardDescription", __instance.CardModel.UniqueID, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						new CardAccessBridge(__instance),
						_IgnoreLiquid
					});
					if (array.Length != 0)
					{
						string text = array[0] as string;
						if (text != null)
						{
							__result = text;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008560 File Offset: 0x00006760
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "ChangeStatValue")]
		public static void LuaChangeStat(GameManager __instance, InGameStat _Stat, float _Value, StatModification _Modification, ref IEnumerator __result)
		{
			LuaRegister.<>c__DisplayClass14_0 CS$<>8__locals1 = new LuaRegister.<>c__DisplayClass14_0();
			CS$<>8__locals1._Stat = _Stat;
			CS$<>8__locals1._Value = _Value;
			CS$<>8__locals1._Modification = _Modification;
			if (CS$<>8__locals1._Stat == null || CS$<>8__locals1._Stat.StatModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("GameManager", "ChangeStatValue", CS$<>8__locals1._Stat.StatModel.UniqueID, out list))
			{
				return;
			}
			__result = __result.Concat(CS$<>8__locals1.<LuaChangeStat>g__Inner|0(__instance));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000085E8 File Offset: 0x000067E8
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "ChangeStatRate")]
		public static void LuaChangeStatRate(GameManager __instance, InGameStat _Stat, float _Rate, StatModification _Modification, ref IEnumerator __result)
		{
			LuaRegister.<>c__DisplayClass15_0 CS$<>8__locals1 = new LuaRegister.<>c__DisplayClass15_0();
			CS$<>8__locals1._Stat = _Stat;
			CS$<>8__locals1._Rate = _Rate;
			CS$<>8__locals1._Modification = _Modification;
			if (CS$<>8__locals1._Stat == null || CS$<>8__locals1._Stat.StatModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("GameManager", "ChangeStatRate", CS$<>8__locals1._Stat.StatModel.UniqueID, out list))
			{
				return;
			}
			__result = __result.Concat(CS$<>8__locals1.<LuaChangeStatRate>g__Inner|0(__instance));
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00008670 File Offset: 0x00006870
		[HarmonyPostfix]
		[HarmonyPatch(typeof(DismantleActionButton), "Setup")]
		public static void LuaDismantleActionButton_Setup(DismantleActionButton __instance, DismantleCardAction _Action, InGameCardBase _Card)
		{
			if (_Card == null || _Card.CardModel == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (!LuaRegister.Register.TryGet("DismantleActionButton", "Setup", _Card.CardModel.UniqueID, out list))
			{
				return;
			}
			foreach (LuaFunction luaFunction in list)
			{
				try
				{
					object[] array = luaFunction.Call(new object[]
					{
						__instance,
						_Action,
						new CardAccessBridge(_Card),
						_Action.ActionName.LocalizationKey
					});
					if (array.Length != 0)
					{
						object obj = array[0];
						if (obj is bool)
						{
							bool active = (bool)obj;
							__instance.gameObject.SetActive(active);
						}
					}
					if (array.Length > 1)
					{
						object obj = array[1];
						if (obj is bool)
						{
							bool flag = (bool)obj;
							__instance.ConditionsValid = flag;
							__instance.Interactable = flag;
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x040000BB RID: 187
		public static readonly LuaRegister Register = new LuaRegister();

		// Token: 0x040000BC RID: 188
		private readonly Dictionary<string, Dictionary<string, Dictionary<string, List<LuaFunction>>>> AllReg = new Dictionary<string, Dictionary<string, Dictionary<string, List<LuaFunction>>>>();
	}
}
