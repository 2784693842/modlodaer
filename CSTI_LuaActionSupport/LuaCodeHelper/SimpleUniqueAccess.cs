using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.Helper;
using HarmonyLib;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200003F RID: 63
	[NullableContext(1)]
	[Nullable(0)]
	public class SimpleUniqueAccess : CommonSimpleAccess
	{
		// Token: 0x1700003E RID: 62
		[Nullable(2)]
		public override object this[string key]
		{
			[return: Nullable(2)]
			get
			{
				CardData cardData = this.UniqueIDScriptable as CardData;
				if (cardData == null)
				{
					return base[key];
				}
				TimeObjective timeObjective = cardData.TimeValues.FirstOrDefault((TimeObjective objective) => objective.ObjectiveName == key);
				if (timeObjective != null)
				{
					return timeObjective.Value;
				}
				StatSubObjective statSubObjective = cardData.StatValues.FirstOrDefault((StatSubObjective objective) => objective.ObjectiveName == key);
				if (statSubObjective != null)
				{
					return statSubObjective.StatCondition.Stat;
				}
				CardOnBoardSubObjective cardOnBoardSubObjective = cardData.CardsOnBoard.FirstOrDefault((CardOnBoardSubObjective objective) => objective.ObjectiveName == key);
				if (cardOnBoardSubObjective != null)
				{
					return cardOnBoardSubObjective.Card;
				}
				TagOnBoardSubObjective tagOnBoardSubObjective = cardData.TagsOnBoard.FirstOrDefault((TagOnBoardSubObjective objective) => objective.ObjectiveName == key);
				if (tagOnBoardSubObjective != null)
				{
					return tagOnBoardSubObjective.Tag;
				}
				return base[key];
			}
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005EC2 File Offset: 0x000040C2
		static SimpleUniqueAccess()
		{
			if (AccessTools.TypeByName("Encounter") != null)
			{
				SimpleUniqueAccess.GenEncounter = new Action<UniqueIDScriptable>(FuncFor1_0_5.GenEncounter);
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005EE7 File Offset: 0x000040E7
		public SimpleUniqueAccess(UniqueIDScriptable uniqueIDScriptable)
		{
			this.UniqueIDScriptable = uniqueIDScriptable;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00005EF8 File Offset: 0x000040F8
		// (set) Token: 0x06000134 RID: 308 RVA: 0x00005F24 File Offset: 0x00004124
		[Nullable(2)]
		public string CardDescription
		{
			[NullableContext(2)]
			get
			{
				CardData cardData = this.UniqueIDScriptable as CardData;
				if (cardData != null)
				{
					return cardData.CardDescription;
				}
				return null;
			}
			[NullableContext(2)]
			set
			{
				if (value == null)
				{
					return;
				}
				CardData cardData = this.UniqueIDScriptable as CardData;
				if (cardData != null)
				{
					CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge = CardActionPatcher.LoadCurrentSlot("zender.SimpleUniqueAccess") as CardActionPatcher.DataNodeTableAccessBridge;
					if (dataNodeTableAccessBridge != null)
					{
						CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge2 = dataNodeTableAccessBridge[cardData.UniqueID] as CardActionPatcher.DataNodeTableAccessBridge;
						if (dataNodeTableAccessBridge2 != null)
						{
							dataNodeTableAccessBridge2["CardDescription"] = value;
							return;
						}
						dataNodeTableAccessBridge[cardData.UniqueID] = new Dictionary<string, object>
						{
							{
								"CardDescription",
								value
							}
						};
						return;
					}
					else
					{
						CardActionPatcher.SaveCurrentSlot("zender.SimpleUniqueAccess", new Dictionary<string, object>
						{
							{
								cardData.UniqueID,
								new Dictionary<string, object>
								{
									{
										"CardDescription",
										value
									}
								}
							}
						});
					}
				}
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005FC4 File Offset: 0x000041C4
		[NullableContext(2)]
		[TestCode("local uid = \"cee786e0869369d4597877e838f2586f\"\r\nlocal ext = { Usage = 5 }\r\nSimpleAccessTool[uid]:Gen(1,ext)")]
		public void Gen(int count = 1, LuaTable ext = null)
		{
			CardData cardData = this.UniqueIDScriptable as CardData;
			if (cardData != null)
			{
				TransferedDurabilities transferedDurabilities = new TransferedDurabilities
				{
					Usage = cardData.UsageDurability.Copy(),
					Fuel = cardData.FuelCapacity.Copy(),
					Spoilage = cardData.SpoilageTime.Copy(),
					ConsumableCharges = cardData.Progress.Copy(),
					Special1 = cardData.SpecialDurability1.Copy(),
					Special2 = cardData.SpecialDurability2.Copy(),
					Special3 = cardData.SpecialDurability3.Copy(),
					Special4 = cardData.SpecialDurability4.Copy(),
					Liquid = UnityEngine.Random.Range(cardData.DefaultLiquidContained.Quantity[0], cardData.DefaultLiquidContained.Quantity[1])
				};
				SpawningLiquid spawningLiquid = new SpawningLiquid
				{
					LiquidCard = cardData.DefaultLiquidContained.LiquidCard,
					StayEmpty = !cardData.DefaultLiquidContained.LiquidCard
				};
				CardActionPatcher.DataNodeTableAccessBridge arg = null;
				if (ext != null)
				{
					ref transferedDurabilities.Usage.FloatValue.TryModBy(ext["Usage"]);
					ref transferedDurabilities.Fuel.FloatValue.TryModBy(ext["Fuel"]);
					ref transferedDurabilities.Spoilage.FloatValue.TryModBy(ext["Spoilage"]);
					ref transferedDurabilities.ConsumableCharges.FloatValue.TryModBy(ext["ConsumableCharges"]);
					ref transferedDurabilities.Liquid.TryModBy(ext["Liquid"]);
					ref transferedDurabilities.Special1.FloatValue.TryModBy(ext["Special1"]);
					ref transferedDurabilities.Special2.FloatValue.TryModBy(ext["Special2"]);
					ref transferedDurabilities.Special3.FloatValue.TryModBy(ext["Special3"]);
					ref transferedDurabilities.Special4.FloatValue.TryModBy(ext["Special4"]);
					SimpleUniqueAccess simpleUniqueAccess = ext["LiquidCard"] as SimpleUniqueAccess;
					CardData cardData2 = ((simpleUniqueAccess != null) ? simpleUniqueAccess.UniqueIDScriptable : null) as CardData;
					spawningLiquid.LiquidCard = cardData2;
					spawningLiquid.StayEmpty = !cardData2;
					ref count.TryModBy(ext["count"]);
					CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge = ext["initData"] as CardActionPatcher.DataNodeTableAccessBridge;
					if (dataNodeTableAccessBridge != null)
					{
						arg = dataNodeTableAccessBridge;
					}
				}
				if (cardData.CardType != CardTypes.Liquid)
				{
					for (int i = 0; i < count; i++)
					{
						List<IEnumerator> enumerators = CardActionPatcher.Enumerators;
						GameManager instance = MBSingleton<GameManager>.Instance;
						CardData data = cardData;
						InGameCardBase fromCard = null;
						bool inCurrentEnv = true;
						TransferedDurabilities transferedDurabilites = transferedDurabilities;
						bool useDefaultInventory = true;
						SpawningLiquid withLiquid = spawningLiquid;
						Vector2Int tick = new Vector2Int(MBSingleton<GameManager>.Instance.CurrentTickInfo.z, -1);
						bool moveView = false;
						Action<InGameCardBase, CardActionPatcher.DataNodeTableAccessBridge> action;
						if ((action = SimpleUniqueAccess.<>O.<0>__SetInitData) == null)
						{
							action = (SimpleUniqueAccess.<>O.<0>__SetInitData = new Action<InGameCardBase, CardActionPatcher.DataNodeTableAccessBridge>(SimpleUniqueAccess.SetInitData));
						}
						enumerators.Add(instance.MoniAddCard(data, fromCard, inCurrentEnv, transferedDurabilites, useDefaultInventory, withLiquid, tick, moveView, action, arg));
					}
				}
				return;
			}
			Action<UniqueIDScriptable> genEncounter = SimpleUniqueAccess.GenEncounter;
			if (genEncounter == null)
			{
				return;
			}
			genEncounter(this.UniqueIDScriptable);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000062B3 File Offset: 0x000044B3
		public static void SetInitData(InGameCardBase cardBase, [Nullable(2)] CardActionPatcher.DataNodeTableAccessBridge data)
		{
			if (data == null)
			{
				return;
			}
			CardAccessBridge cardAccessBridge = new CardAccessBridge(cardBase);
			cardAccessBridge.InitData(data);
			cardAccessBridge.SaveData();
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000137 RID: 311 RVA: 0x000062CC File Offset: 0x000044CC
		// (set) Token: 0x06000138 RID: 312 RVA: 0x00006304 File Offset: 0x00004504
		public float StatValue
		{
			get
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return -1f;
				}
				return MBSingleton<GameManager>.Instance.StatsDict[gameStat].SimpleCurrentValue;
			}
			set
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return;
				}
				InGameStat inGameStat = MBSingleton<GameManager>.Instance.StatsDict[gameStat];
				MBSingleton<GameManager>.Instance.ChangeStatValueTo(inGameStat, value);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006340 File Offset: 0x00004540
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00006370 File Offset: 0x00004570
		public float StatValueMin
		{
			get
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return -1f;
				}
				return gameStat.MinMaxValue.x;
			}
			set
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return;
				}
				gameStat.MinMaxValue.Set(value, gameStat.MinMaxValue.y);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000063A4 File Offset: 0x000045A4
		// (set) Token: 0x0600013C RID: 316 RVA: 0x000063D4 File Offset: 0x000045D4
		public float StatValueMax
		{
			get
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return -1f;
				}
				return gameStat.MinMaxValue.y;
			}
			set
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return;
				}
				gameStat.MinMaxValue.Set(gameStat.MinMaxValue.x, value);
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006408 File Offset: 0x00004608
		[TestCode("local sua = SimpleAccessTool[\"79290cafb08e48f4d871704c20e69b1c\"]\r\nsua:CacheRawValRange(0,100)\r\nsua.StatValueMin = -100\r\nsua.StatValueMax = 200")]
		public void CacheRawValRange(float x, float y)
		{
			GameStat gameStat = this.UniqueIDScriptable as GameStat;
			if (gameStat == null)
			{
				return;
			}
			gameStat.MinMaxValue = new Vector2(x, y);
			Dictionary<string, DataNode> dictionary = CardActionPatcher.CurrentGSlotSaveData();
			if (!dictionary.ContainsKey("__StatCache"))
			{
				dictionary["__StatCache"] = DataNode.EmptyTable;
			}
			if (!dictionary["__StatCache"].table.ContainsKey(gameStat.UniqueID))
			{
				dictionary["__StatCache"].table[gameStat.UniqueID] = DataNode.EmptyTable;
			}
			if (!dictionary["__StatCache"].table[gameStat.UniqueID].table.ContainsKey("MinMaxValue"))
			{
				dictionary["__StatCache"].table[gameStat.UniqueID].table["MinMaxValue"] = new DataNode(gameStat.MinMaxValue);
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006508 File Offset: 0x00004708
		public void CacheRawRateRange(float x, float y)
		{
			GameStat gameStat = this.UniqueIDScriptable as GameStat;
			if (gameStat == null)
			{
				return;
			}
			gameStat.MinMaxRate = new Vector2(x, y);
			Dictionary<string, DataNode> dictionary = CardActionPatcher.CurrentGSlotSaveData();
			if (!dictionary.ContainsKey("__StatCache"))
			{
				dictionary["__StatCache"] = DataNode.EmptyTable;
			}
			if (!dictionary["__StatCache"].table.ContainsKey(gameStat.UniqueID))
			{
				dictionary["__StatCache"].table[gameStat.UniqueID] = DataNode.EmptyTable;
			}
			if (!dictionary["__StatCache"].table[gameStat.UniqueID].table.ContainsKey("MinMaxRate"))
			{
				dictionary["__StatCache"].table[gameStat.UniqueID].table["MinMaxRate"] = new DataNode(gameStat.MinMaxRate);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006608 File Offset: 0x00004808
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00006640 File Offset: 0x00004840
		public float StatRate
		{
			get
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return -1f;
				}
				return MBSingleton<GameManager>.Instance.StatsDict[gameStat].SimpleRatePerTick;
			}
			set
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return;
				}
				InGameStat inGameStat = MBSingleton<GameManager>.Instance.StatsDict[gameStat];
				MBSingleton<GameManager>.Instance.ChangeStatRateTo(inGameStat, value);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000667C File Offset: 0x0000487C
		// (set) Token: 0x06000142 RID: 322 RVA: 0x000066AC File Offset: 0x000048AC
		public float StatRateMin
		{
			get
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return -1f;
				}
				return gameStat.MinMaxRate.x;
			}
			set
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return;
				}
				gameStat.MinMaxRate.Set(value, gameStat.MinMaxRate.y);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000066E0 File Offset: 0x000048E0
		// (set) Token: 0x06000144 RID: 324 RVA: 0x00006710 File Offset: 0x00004910
		public float StatRateMax
		{
			get
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return -1f;
				}
				return gameStat.MinMaxRate.y;
			}
			set
			{
				GameStat gameStat = this.UniqueIDScriptable as GameStat;
				if (gameStat == null)
				{
					return;
				}
				gameStat.MinMaxRate.Set(gameStat.MinMaxRate.x, value);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006744 File Offset: 0x00004944
		[NullableContext(2)]
		public EnvDataAccessBridge GetEnvData(long? index = null)
		{
			UniqueIDScriptable uniqueIDScriptable = this.UniqueIDScriptable;
			CardData cardData = uniqueIDScriptable as CardData;
			if (cardData == null || cardData.CardType != CardTypes.Environment)
			{
				return null;
			}
			EnvironmentSaveData environmentSaveData;
			if (cardData != null && !cardData.InstancedEnvironment && MBSingleton<GameManager>.Instance.EnvironmentsData.TryGetValue(cardData.UniqueID, out environmentSaveData))
			{
				return new EnvDataAccessBridge(environmentSaveData);
			}
			if (index != null && cardData != null && cardData.InstancedEnvironment)
			{
				return new EnvDataAccessBridge(MBSingleton<GameManager>.Instance.EnvironmentsData.FirstOrDefault((KeyValuePair<string, EnvironmentSaveData> pair) => pair.Key.StartsWith(cardData.UniqueID) && pair.Key.EndsWith(index.ToString())).Value);
			}
			return null;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000680F File Offset: 0x00004A0F
		public override object AccessObj
		{
			get
			{
				return this.UniqueIDScriptable;
			}
		}

		// Token: 0x0400008A RID: 138
		public readonly UniqueIDScriptable UniqueIDScriptable;

		// Token: 0x0400008B RID: 139
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private static readonly Action<UniqueIDScriptable> GenEncounter;

		// Token: 0x0400008C RID: 140
		public const string SaveKey = "zender.SimpleUniqueAccess";

		// Token: 0x02000040 RID: 64
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x0400008D RID: 141
			[Nullable(new byte[]
			{
				0,
				1,
				0
			})]
			public static Action<InGameCardBase, CardActionPatcher.DataNodeTableAccessBridge> <0>__SetInitData;
		}
	}
}
