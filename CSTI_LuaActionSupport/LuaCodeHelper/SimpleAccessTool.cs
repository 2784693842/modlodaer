using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.AllPatcher;
using HarmonyLib;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000037 RID: 55
	[NullableContext(1)]
	[Nullable(0)]
	public class SimpleAccessTool
	{
		// Token: 0x17000038 RID: 56
		[Nullable(2)]
		public SimpleUniqueAccess this[string key]
		{
			[return: Nullable(2)]
			get
			{
				UniqueIDScriptable uniqueIDScriptable;
				if (UniqueIDScriptable.AllUniqueObjects.TryGetValue(key, out uniqueIDScriptable))
				{
					return new SimpleUniqueAccess(uniqueIDScriptable);
				}
				Debug.LogWarning("no unique id : " + key);
				return null;
			}
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000058FC File Offset: 0x00003AFC
		public void ClearCurrentEnv()
		{
			GameManager instance = MBSingleton<GameManager>.Instance;
			InGameCardBase currentExplorableCard = instance.CurrentExplorableCard;
			currentExplorableCard.ExplorationData.CurrentExploration = 0f;
			currentExplorableCard.ExplorationData.ExplorationResults.Do(delegate(ExplorationResultSaveData data)
			{
				data.Triggered = false;
				data.TriggeredWithoutResults = false;
			});
			SimpleAccessTool.ClearStats(currentExplorableCard);
			InGameCardBase gameManagerCurrentEnvironmentCard = instance.CurrentEnvironmentCard;
			SimpleAccessTool.ClearStats(gameManagerCurrentEnvironmentCard);
			IEnumerable<InGameCardBase> allVisibleCards = instance.AllVisibleCards;
			Func<InGameCardBase, bool> <>9__1;
			Func<InGameCardBase, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = delegate(InGameCardBase card)
				{
					SlotsTypes slotType = card.CurrentSlotInfo.SlotType;
					bool flag = slotType - SlotsTypes.Base <= 1;
					return flag && gameManagerCurrentEnvironmentCard.CardModel.DefaultEnvCards.All((CardData data) => data != card.CardModel);
				});
			}
			foreach (InGameCardBase card2 in allVisibleCards.Where(predicate))
			{
				CardActionPatcher.Enumerators.Add(instance.RemoveCard(card2, true, false, GameManager.RemoveOption.RemoveAll, false));
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000059EC File Offset: 0x00003BEC
		public static void ClearStats(InGameCardBase cardBase)
		{
			cardBase.DroppedCollections = new Dictionary<string, Vector2Int>();
			cardBase.CurrentUsageDurability = cardBase.CardModel.UsageDurability.FloatValue;
			cardBase.CurrentProgress = cardBase.CardModel.Progress.FloatValue;
			cardBase.CurrentFuel = cardBase.CardModel.FuelCapacity.FloatValue;
			cardBase.CurrentSpoilage = cardBase.CardModel.SpoilageTime.FloatValue;
			cardBase.CurrentSpecial1 = cardBase.CardModel.SpecialDurability1.FloatValue;
			cardBase.CurrentSpecial2 = cardBase.CardModel.SpecialDurability2.FloatValue;
			cardBase.CurrentSpecial3 = cardBase.CardModel.SpecialDurability3.FloatValue;
			cardBase.CurrentSpecial4 = cardBase.CardModel.SpecialDurability4.FloatValue;
			if (cardBase.CardVisuals)
			{
				cardBase.CardVisuals.RefreshDurabilities();
			}
		}
	}
}
