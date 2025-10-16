using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	public static class DataAccessTool
	{
		// Token: 0x0600001B RID: 27 RVA: 0x0000269C File Offset: 0x0000089C
		[LuaFunc]
		[TestCode("debug.debug=GetCard(\"8695a7aa22521aa45be582d3c1558f78\")")]
		public static CardData GetCard(string id)
		{
			return UniqueIDScriptable.GetFromID<CardData>(id);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000026A4 File Offset: 0x000008A4
		[LuaFunc]
		public static CardAccessBridge GetGameCard(string id)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			MBSingleton<GameManager>.Instance.CardIsOnBoard(UniqueIDScriptable.GetFromID<CardData>(id), true, true, false, false, list, Array.Empty<InGameCardBase>());
			return new CardAccessBridge(list.FirstOrDefault<InGameCardBase>());
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000026E0 File Offset: 0x000008E0
		[LuaFunc]
		[return: Nullable(2)]
		public static CardAccessBridge GetGameCardByTag(string tag)
		{
			Func<CardTag, bool> <>9__1;
			InGameCardBase inGameCardBase = new List<InGameCardBase>(MBSingleton<GameManager>.Instance.AllCards.Where(delegate(InGameCardBase cardBase)
			{
				IEnumerable<CardTag> cardTags = cardBase.CardModel.CardTags;
				Func<CardTag, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((CardTag cardTag) => cardTag != null && (cardTag.InGameName.DefaultText == tag || cardTag.name == tag)));
				}
				return cardTags.Any(predicate);
			})).FirstOrDefault<InGameCardBase>();
			if (inGameCardBase == null)
			{
				return null;
			}
			return new CardAccessBridge(inGameCardBase);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000272C File Offset: 0x0000092C
		public static IEnumerable<InGameCardBase> ProcessType(this IEnumerable<InGameCardBase> enumerable, string type, [Nullable(2)] CardData cardData)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			if (type != null)
			{
				switch (type.Length)
				{
				case 4:
				{
					char c = type[0];
					if (c != 'B')
					{
						if (c == 'H')
						{
							if (type == "Hand")
							{
								if (cardData)
								{
									return MBSingleton<GameManager>.Instance.CardIsInHand(cardData, true, list, Array.Empty<InGameCardBase>()) ? list : list;
								}
								return from cardBase in enumerable
								where cardBase.CurrentSlotInfo.SlotType == SlotsTypes.Hand
								select cardBase;
							}
						}
					}
					else if (type == "Base")
					{
						return from cardBase in enumerable
						where cardBase.CurrentSlotInfo.SlotType == SlotsTypes.Base
						select cardBase;
					}
					break;
				}
				case 7:
					if (type == "Weather")
					{
						return new <>z__ReadOnlyArray<InGameCardBase>(new InGameCardBase[]
						{
							MBSingleton<GameManager>.Instance.CurrentWeatherCard
						});
					}
					break;
				case 8:
					if (type == "Location")
					{
						return from cardBase in enumerable
						where cardBase.CurrentSlotInfo.SlotType == SlotsTypes.Location
						select cardBase;
					}
					break;
				case 9:
				{
					char c = type[0];
					if (c != 'E')
					{
						if (c == 'I')
						{
							if (type == "Inventory")
							{
								return from cardBase in enumerable
								where cardBase.CurrentSlotInfo.SlotType == SlotsTypes.Inventory
								select cardBase;
							}
						}
					}
					else if (type == "Equipment")
					{
						return from cardBase in enumerable
						where cardBase.CurrentSlotInfo.SlotType == SlotsTypes.Equipment
						select cardBase;
					}
					break;
				}
				case 10:
					if (type == "Explorable")
					{
						return new <>z__ReadOnlyArray<InGameCardBase>(new InGameCardBase[]
						{
							MBSingleton<GameManager>.Instance.CurrentExplorableCard
						});
					}
					break;
				case 11:
					if (type == "Environment")
					{
						return new <>z__ReadOnlyArray<InGameCardBase>(new InGameCardBase[]
						{
							MBSingleton<GameManager>.Instance.CurrentEnvironmentCard
						});
					}
					break;
				case 13:
					if (type == "NotBackGround")
					{
						return from cardBase in enumerable
						where !cardBase.InBackground
						select cardBase;
					}
					break;
				case 14:
					if (type == "OnlyBackGround")
					{
						return from cardBase in enumerable
						where cardBase.InBackground
						select cardBase;
					}
					break;
				}
			}
			return enumerable;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002A31 File Offset: 0x00000C31
		public static List<CardAccessBridge> IntoBridge(this IEnumerable<InGameCardBase> enumerable)
		{
			return (from card in enumerable
			select new CardAccessBridge(card)).ToList<CardAccessBridge>();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A60 File Offset: 0x00000C60
		[LuaFunc]
		[TestCode("local uid = \"8695a7aa22521aa45be582d3c1558f78\"\r\nlocal ext = { type = \"Base\" }\r\ndebug.debug = GetGameCards(uid,ext)[0].CardBase")]
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		public static List<CardAccessBridge> GetGameCards(string id, [Nullable(2)] LuaTable ext = null)
		{
			List<CardAccessBridge> result;
			try
			{
				List<InGameCardBase> list = new List<InGameCardBase>();
				CardData fromID = UniqueIDScriptable.GetFromID<CardData>(id);
				if (fromID == null)
				{
					result = null;
				}
				else
				{
					MBSingleton<GameManager>.Instance.CardIsOnBoard(fromID, true, true, false, false, list, Array.Empty<InGameCardBase>());
					object obj = (ext != null) ? ext["type"] : null;
					string text = obj as string;
					if (text == null)
					{
						LuaTable luaTable = obj as LuaTable;
						if (luaTable == null)
						{
							result = list.IntoBridge();
						}
						else
						{
							IEnumerable<InGameCardBase> enumerable = list;
							foreach (object field in luaTable.Keys)
							{
								string text2 = luaTable[field] as string;
								if (text2 != null)
								{
									enumerable = enumerable.ProcessType(text2, fromID);
								}
							}
							result = enumerable.IntoBridge();
						}
					}
					else
					{
						result = list.ProcessType(text, fromID).IntoBridge();
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				result = null;
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B74 File Offset: 0x00000D74
		[LuaFunc]
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		public static List<CardAccessBridge> GetGameCardsByTag(string tag, [Nullable(2)] LuaTable ext = null)
		{
			List<CardAccessBridge> result;
			try
			{
				Func<CardTag, bool> <>9__1;
				List<InGameCardBase> list = new List<InGameCardBase>(MBSingleton<GameManager>.Instance.AllCards.Where(delegate(InGameCardBase cardBase)
				{
					IEnumerable<CardTag> cardTags = cardBase.CardModel.CardTags;
					Func<CardTag, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((CardTag cardTag) => cardTag != null && (cardTag.InGameName.DefaultText == tag || cardTag.name == tag)));
					}
					return cardTags.Any(predicate);
				}));
				object obj = (ext != null) ? ext["type"] : null;
				string text = obj as string;
				if (text == null)
				{
					LuaTable luaTable = obj as LuaTable;
					if (luaTable == null)
					{
						result = list.IntoBridge();
					}
					else
					{
						IEnumerable<InGameCardBase> enumerable = list;
						foreach (object field in luaTable.Keys)
						{
							string text2 = luaTable[field] as string;
							if (text2 != null)
							{
								enumerable = enumerable.ProcessType(text2, null);
							}
						}
						result = enumerable.IntoBridge();
					}
				}
				else
				{
					result = list.ProcessType(text, null).IntoBridge();
				}
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				result = null;
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C84 File Offset: 0x00000E84
		[LuaFunc]
		public static int CountCardOnBoard(string id, bool _CountInInventories = true, bool _CountInBackground = false)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			MBSingleton<GameManager>.Instance.CardIsOnBoard(UniqueIDScriptable.GetFromID<CardData>(id), true, _CountInInventories, false, _CountInBackground, list, Array.Empty<InGameCardBase>());
			return list.Count;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002CB8 File Offset: 0x00000EB8
		[LuaFunc]
		public static int CountCardInBase(string id, bool _CountInInventories = true)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			MBSingleton<GameManager>.Instance.CardIsOnBoard(UniqueIDScriptable.GetFromID<CardData>(id), false, _CountInInventories, false, false, list, Array.Empty<InGameCardBase>());
			return list.Count((InGameCardBase cardBase) => cardBase.CurrentSlotInfo.SlotType == SlotsTypes.Base);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002D0C File Offset: 0x00000F0C
		[LuaFunc]
		public static int CountCardEquipped(string id)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			MBSingleton<GameManager>.Instance.CardIsOnBoard(UniqueIDScriptable.GetFromID<CardData>(id), false, false, false, false, list, Array.Empty<InGameCardBase>());
			return list.Count((InGameCardBase cardBase) => cardBase.CurrentSlotInfo.SlotType == SlotsTypes.Equipment);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D60 File Offset: 0x00000F60
		[LuaFunc]
		public static int CountCardInHand(string id, bool _CountInInventories = true)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			MBSingleton<GameManager>.Instance.CardIsInHand(UniqueIDScriptable.GetFromID<CardData>(id), _CountInInventories, list, Array.Empty<InGameCardBase>());
			return list.Count;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002D94 File Offset: 0x00000F94
		[LuaFunc]
		public static int CountCardInLocation(string id, bool _CountInInventories = true)
		{
			List<InGameCardBase> list = new List<InGameCardBase>();
			MBSingleton<GameManager>.Instance.CardIsInLocation(UniqueIDScriptable.GetFromID<CardData>(id), _CountInInventories, false, list, Array.Empty<InGameCardBase>());
			return list.Count;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002DC6 File Offset: 0x00000FC6
		[LuaFunc]
		public static GameStat GetStat(string id)
		{
			return UniqueIDScriptable.GetFromID<GameStat>(id);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002DCE File Offset: 0x00000FCE
		[LuaFunc]
		public static GameStatAccessBridge GetGameStat(string id)
		{
			return new GameStatAccessBridge(MBSingleton<GameManager>.Instance.StatsDict[DataAccessTool.GetStat(id)]);
		}
	}
}
