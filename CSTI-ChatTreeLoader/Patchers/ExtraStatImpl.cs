using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChatTreeLoader.ScriptObjects;
using ChatTreeLoader.Util;
using HarmonyLib;
using UnityEngine;

namespace ChatTreeLoader.Patchers
{
	// Token: 0x0200000D RID: 13
	public static class ExtraStatImpl
	{
		// Token: 0x06000024 RID: 36 RVA: 0x000025FC File Offset: 0x000007FC
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "ChangeEnvironment")]
		public static void OnEnvChange(GameManager __instance, ref IEnumerator __result)
		{
			__result = __result.OnStart(delegate()
			{
				foreach (GameStat gameStat in from stat in ExtraStatTable.AllTables.SelectMany((ExtraStatTable table) => table.EnvBindStats)
				where stat
				select stat)
				{
					string key = gameStat.UniqueID + "_" + __instance.CurrentEnvironment.UniqueID + "__EnvBind_Stat";
					InGameStat inGameStat = __instance.StatsDict[gameStat];
					__instance.CurrentEnvironmentCard.DroppedCollections[key] = new Vector2Int(Mathf.RoundToInt(inGameStat.CurrentBaseValue), 1000);
				}
			}).OnEnd(delegate()
			{
				Queue<CoroutineController> queue = new Queue<CoroutineController>();
				foreach (GameStat gameStat in from stat in ExtraStatTable.AllTables.SelectMany((ExtraStatTable table) => table.EnvBindStats)
				where stat
				select stat)
				{
					Vector2Int vector2Int;
					if (__instance.CurrentEnvironmentCard.DroppedCollections.TryGetValue(gameStat.UniqueID + "_" + __instance.CurrentEnvironment.UniqueID + "__EnvBind_Stat", out vector2Int))
					{
						InGameStat inGameStat = __instance.StatsDict[gameStat];
						CoroutineController item;
						__instance.StartCoroutineEx(__instance.ChangeStatValue(inGameStat, gameStat.BaseValue - inGameStat.CurrentBaseValue, StatModification.Permanent).OnEnd(__instance.ChangeStatValue(inGameStat, (float)vector2Int.x - gameStat.BaseValue, StatModification.Permanent)), out item);
						queue.Enqueue(item);
					}
				}
				__instance.WaitAll(queue);
			});
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000263C File Offset: 0x0000083C
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "ActionRoutine")]
		public static void OnCardAction(GameManager __instance, InGameCardBase _ReceivingCard, ref IEnumerator __result)
		{
			Func<CardData, bool> <>9__2;
			Func<CardData, bool> <>9__3;
			__result = __result.OnStart(delegate()
			{
				foreach (ExtraStatTable extraStatTable in ExtraStatTable.AllTables)
				{
					IEnumerable<CardData> cardBindCards = extraStatTable.CardBindCards;
					Func<CardData, bool> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((CardData data) => data != _ReceivingCard.CardModel));
					}
					if (!cardBindCards.All(predicate))
					{
						foreach (GameStat gameStat in extraStatTable.CardBindStats)
						{
							InGameStat inGameStat = __instance.StatsDict[gameStat];
							string key = gameStat.UniqueID + "_" + _ReceivingCard.CardModel.UniqueID + "__CardBind_Stat";
							Vector2Int vector2Int;
							if (_ReceivingCard.DroppedCollections.TryGetValue(key, out vector2Int))
							{
								inGameStat.CurrentBaseValue = (float)vector2Int.x;
							}
							else
							{
								inGameStat.CurrentBaseValue = (float)Mathf.RoundToInt(gameStat.BaseValue);
								_ReceivingCard.DroppedCollections[key] = new Vector2Int(Mathf.RoundToInt(gameStat.BaseValue), 0);
							}
						}
					}
				}
			}).OnEnd(delegate()
			{
				foreach (ExtraStatTable extraStatTable in ExtraStatTable.AllTables)
				{
					IEnumerable<CardData> cardBindCards = extraStatTable.CardBindCards;
					Func<CardData, bool> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = ((CardData data) => data != _ReceivingCard.CardModel));
					}
					if (!cardBindCards.All(predicate))
					{
						foreach (GameStat gameStat in extraStatTable.CardBindStats)
						{
							InGameStat inGameStat = __instance.StatsDict[gameStat];
							string key = gameStat.UniqueID + "_" + _ReceivingCard.CardModel.UniqueID + "__CardBind_Stat";
							_ReceivingCard.DroppedCollections[key] = new Vector2Int(Mathf.RoundToInt(inGameStat.CurrentBaseValue), 0);
						}
					}
				}
			});
		}
	}
}
