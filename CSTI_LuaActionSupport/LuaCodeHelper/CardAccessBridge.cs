using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.Helper;
using gfoidl.Base64;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	[Nullable(0)]
	public class CardAccessBridge
	{
		// Token: 0x06000041 RID: 65 RVA: 0x000031A8 File Offset: 0x000013A8
		[NullableContext(2)]
		public CardAccessBridge(InGameCardBase cardBase)
		{
			this.CardBase = cardBase;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000031B7 File Offset: 0x000013B7
		[Nullable(2)]
		public SimpleUniqueAccess CardModel
		{
			[NullableContext(2)]
			get
			{
				if (!(this.CardBase != null))
				{
					return null;
				}
				return new SimpleUniqueAccess(this.CardBase.CardModel);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000031D9 File Offset: 0x000013D9
		public bool IsEquipped
		{
			get
			{
				return this.CardBase != null && this.CardBase.CurrentSlot.SlotType == SlotsTypes.Equipment;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000031FE File Offset: 0x000013FE
		public bool IsInHand
		{
			get
			{
				return this.CardBase != null && this.CardBase.CurrentSlot.SlotType == SlotsTypes.Hand;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003223 File Offset: 0x00001423
		public bool IsInBase
		{
			get
			{
				return this.CardBase != null && this.CardBase.CurrentSlot.SlotType == SlotsTypes.Base;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003248 File Offset: 0x00001448
		public bool IsInLocation
		{
			get
			{
				return this.CardBase != null && this.CardBase.CurrentSlot.SlotType == SlotsTypes.Location;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000047 RID: 71 RVA: 0x0000326D File Offset: 0x0000146D
		public bool IsInBackground
		{
			get
			{
				return this.CardBase != null && this.CardBase.InBackground;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000328A File Offset: 0x0000148A
		public bool CheckInventory(bool useAll, params string[] uid)
		{
			if (!useAll)
			{
				return uid.Any((string s) => this.HasInInventory(s, 0L));
			}
			return uid.All((string s) => this.HasInInventory(s, 0L));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000032B4 File Offset: 0x000014B4
		public bool CheckTagInventory(bool useAll, params string[] tags)
		{
			if (!useAll)
			{
				return tags.Any((string s) => this.HasTagInInventory(s, 0L));
			}
			return tags.All((string s) => this.HasTagInInventory(s, 0L));
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000032DE File Offset: 0x000014DE
		public bool CheckRegexTagInventory(bool useAll, params string[] regexTags)
		{
			if (!useAll)
			{
				return regexTags.Any((string s) => this.HasRegexTagInInventory(s, 0L));
			}
			return regexTags.All((string s) => this.HasRegexTagInInventory(s, 0L));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003308 File Offset: 0x00001508
		public bool HasInInventory(string uid, long needCount = 0L)
		{
			if (this.CardBase == null)
			{
				return false;
			}
			if (!this.CardBase.IsInventoryCard)
			{
				return false;
			}
			CardData fromID = UniqueIDScriptable.GetFromID<CardData>(uid);
			if (fromID == null)
			{
				return false;
			}
			if (needCount <= 0L)
			{
				return this.CardBase.CardsInInventory.Any((InventorySlot slot) => slot.CardModel.UniqueID == uid);
			}
			return (long)this.CardBase.InventoryCount(fromID) > needCount;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003384 File Offset: 0x00001584
		public bool HasTagInInventory(string tag, long needCount = 0L)
		{
			if (this.CardBase == null)
			{
				return false;
			}
			if (!this.CardBase.IsInventoryCard)
			{
				return false;
			}
			if (needCount <= 0L)
			{
				return this.CardBase.CardsInInventory.Any((InventorySlot slot) => CardAccessBridge.HasTag(slot.CardModel, tag));
			}
			return (long)(from slot in this.CardBase.CardsInInventory
			where CardAccessBridge.HasTag(slot.CardModel, tag)
			select slot).Sum((InventorySlot slot) => slot.CardAmt) >= needCount;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003428 File Offset: 0x00001628
		public bool HasRegexTagInInventory(string regexTag, long needCount = 0L)
		{
			if (this.CardBase == null)
			{
				return false;
			}
			if (!this.CardBase.IsInventoryCard)
			{
				return false;
			}
			if (needCount <= 0L)
			{
				return this.CardBase.CardsInInventory.Any((InventorySlot slot) => CardAccessBridge.HasRegexTag(slot.CardModel, regexTag));
			}
			return (long)(from slot in this.CardBase.CardsInInventory
			where CardAccessBridge.HasRegexTag(slot.CardModel, regexTag)
			select slot).Sum((InventorySlot slot) => slot.CardAmt) >= needCount;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000034CA File Offset: 0x000016CA
		[NullableContext(2)]
		public CardAccessBridge LiquidInventory()
		{
			if (this.CardBase == null)
			{
				return null;
			}
			if (this.CardBase.LiquidEmpty)
			{
				return null;
			}
			return new CardAccessBridge(this.CardBase.ContainedLiquid);
		}

		// Token: 0x1700000D RID: 13
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public List<CardAccessBridge> this[long index]
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				if (this.CardBase == null)
				{
					return null;
				}
				if (!this.CardBase.IsInventoryCard)
				{
					return null;
				}
				if (index < 0L || index >= (long)this.CardBase.CardsInInventory.Count)
				{
					return null;
				}
				return (from cardBase in this.CardBase.CardsInInventory[Mathf.RoundToInt((float)index)].AllCards
				select new CardAccessBridge(cardBase)).ToList<CardAccessBridge>();
			}
		}

		// Token: 0x1700000E RID: 14
		[Nullable(2)]
		public object this[string key]
		{
			[return: Nullable(2)]
			get
			{
				if (this.CardBase == null)
				{
					return null;
				}
				Vector2Int vector2Int;
				if (this.CardBase.DroppedCollections.TryGetValue(key, out vector2Int))
				{
					return vector2Int;
				}
				string text;
				if (!(from pair in this.CardBase.DroppedCollections.Keys.Select(delegate(string s)
				{
					Match match = CardAccessBridge.KVDataCheck.Match(s);
					if (match.Success)
					{
						return new KeyValuePair<string, string>?(new KeyValuePair<string, string>(match.Groups["key"].ToString(), match.Groups["val"].ToString()));
					}
					return null;
				})
				where pair != null
				select pair).ToDictionary((KeyValuePair<string, string>? pair) => pair.Value.Key, (KeyValuePair<string, string>? pair) => pair.Value.Value).TryGetValue(key, out text))
				{
					return null;
				}
				double num;
				if (double.TryParse(text, out num))
				{
					return num;
				}
				return text;
			}
			[param: Nullable(2)]
			set
			{
				if (this.CardBase == null)
				{
					return;
				}
				if (value is double)
				{
					double num = (double)value;
					this.CardBase.DroppedCollections[key] = new Vector2Int(Mathf.RoundToInt((float)num), 0);
					return;
				}
				LuaTable luaTable = value as LuaTable;
				object arg = (luaTable != null) ? DebugBridge.TableToString(luaTable, null) : value;
				this.CardBase.DroppedCollections[string.Format("zender.luaSupportData.{{{0}}}:{{{1}}}", key, arg)] = Vector2Int.one;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000036FC File Offset: 0x000018FC
		public string SlotType
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return "nil";
				}
				return this.CardBase.CurrentSlot.SlotType.ToString();
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000372D File Offset: 0x0000192D
		public string CardType
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return "nil";
				}
				return this.CardBase.CardModel.CardType.ToString();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000054 RID: 84 RVA: 0x0000375E File Offset: 0x0000195E
		public float Weight
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentWeight;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000377F File Offset: 0x0000197F
		public bool HasTag(string tag)
		{
			return !(this.CardBase == null) && CardAccessBridge.HasTag(this.CardBase.CardModel, tag);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000037A4 File Offset: 0x000019A4
		public static bool HasTag(CardData cardData, string tag)
		{
			return !(cardData == null) && cardData.CardTags.Any((CardTag cardTag) => cardTag.name == tag || cardTag.InGameName.DefaultText == tag);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000037E0 File Offset: 0x000019E0
		public bool HasRegexTag(string regexTag)
		{
			return !(this.CardBase == null) && CardAccessBridge.HasRegexTag(this.CardBase.CardModel, regexTag);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003804 File Offset: 0x00001A04
		public static bool HasRegexTag(CardData cardData, string regexTag)
		{
			if (cardData == null)
			{
				return false;
			}
			Regex tag = new Regex(regexTag);
			return cardData.CardTags.Any((CardTag cardTag) => tag.IsMatch(cardTag.name) || tag.IsMatch(cardTag.InGameName.DefaultText));
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003845 File Offset: 0x00001A45
		public int TravelCardIndex
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return -1;
				}
				return this.CardBase.TravelCardIndex;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003862 File Offset: 0x00001A62
		public string Id
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return "";
				}
				return this.CardBase.CardModel.UniqueID;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003888 File Offset: 0x00001A88
		[Nullable(2)]
		[TestCode("receive:InitData()\r\nlocal d = receive.Data\r\nif d[\"i\"] == nil then\r\n  d[\"i\"] = 10\r\nelse\r\n  d[\"i\"] = d[\"i\"] + 1\r\nend\r\nreceive:SaveData()")]
		public CardActionPatcher.DataNodeTableAccessBridge Data
		{
			[NullableContext(2)]
			get
			{
				if (this.CardBase == null)
				{
					return null;
				}
				DataNode? dataNode = this._dataNode;
				if (dataNode == null || dataNode.GetValueOrDefault().NodeType != DataNode.DataNodeType.Table)
				{
					foreach (string input in this.CardBase.DroppedCollections.Keys)
					{
						Match match = CardAccessBridge.DataNodeReg.Match(input);
						if (match != null)
						{
							using (MemoryStream memoryStream = new MemoryStream(Base64.Default.Decode(match.Groups["nbt"].Value.AsSpan())))
							{
								BinaryReader binaryReader = new BinaryReader(memoryStream);
								this._dataNode = new DataNode?(DataNode.Load(binaryReader));
								break;
							}
						}
					}
				}
				dataNode = this._dataNode;
				if (dataNode == null || dataNode.GetValueOrDefault().NodeType != DataNode.DataNodeType.Table)
				{
					return null;
				}
				return new CardActionPatcher.DataNodeTableAccessBridge(this._dataNode.Value.table);
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000039BC File Offset: 0x00001BBC
		[NullableContext(2)]
		public void InitData(CardActionPatcher.DataNodeTableAccessBridge initData = null)
		{
			if (this.Data == null)
			{
				this._dataNode = new DataNode?((((initData != null) ? initData.Table : null) == null) ? new DataNode(new Dictionary<string, DataNode>()) : new DataNode(initData.Table));
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000039F8 File Offset: 0x00001BF8
		public void SaveData()
		{
			if (this.CardBase == null)
			{
				return;
			}
			DataNode? dataNode = this._dataNode;
			if (dataNode != null)
			{
				DataNode valueOrDefault = dataNode.GetValueOrDefault();
				if (valueOrDefault.NodeType == DataNode.DataNodeType.Table)
				{
					MemoryStream memoryStream = new MemoryStream();
					BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
					valueOrDefault.Save(binaryWriter);
					byte[] array = memoryStream.ToArray();
					memoryStream.Close();
					KeyValuePair<string, Vector2Int> keyValuePair = this.CardBase.DroppedCollections.FirstOrDefault((KeyValuePair<string, Vector2Int> pair) => CardAccessBridge.DataNodeReg.IsMatch(pair.Key));
					if (keyValuePair.Key != null)
					{
						this.CardBase.DroppedCollections.Remove(keyValuePair.Key);
					}
					this.CardBase.DroppedCollections["LNbt|>" + Base64.Default.Encode(array) + "<|"] = Vector2Int.zero;
					return;
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00003ADC File Offset: 0x00001CDC
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00003AFD File Offset: 0x00001CFD
		public float Spoilage
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentSpoilage;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Spoilage));
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003B11 File Offset: 0x00001D11
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00003B32 File Offset: 0x00001D32
		public float Usage
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentUsageDurability;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Usage));
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00003B46 File Offset: 0x00001D46
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00003B67 File Offset: 0x00001D67
		public float Fuel
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentFuel;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Fuel));
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003B7B File Offset: 0x00001D7B
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00003B9C File Offset: 0x00001D9C
		public float Progress
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentProgress;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Progress));
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003BB0 File Offset: 0x00001DB0
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003BD1 File Offset: 0x00001DD1
		public float Special1
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentSpecial1;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Special1));
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003BE5 File Offset: 0x00001DE5
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003C06 File Offset: 0x00001E06
		public float Special2
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentSpecial2;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Special2));
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003C1A File Offset: 0x00001E1A
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003C3B File Offset: 0x00001E3B
		public float Special3
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentSpecial3;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Special3));
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003C4F File Offset: 0x00001E4F
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00003C70 File Offset: 0x00001E70
		public float Special4
		{
			get
			{
				if (!(this.CardBase != null))
				{
					return 0f;
				}
				return this.CardBase.CurrentSpecial4;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Special4));
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003C84 File Offset: 0x00001E84
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003CB2 File Offset: 0x00001EB2
		public float LiquidQuantity
		{
			get
			{
				if (!(this.CardBase != null) || !this.CardBase.IsLiquid)
				{
					return 0f;
				}
				return this.CardBase.CurrentLiquidQuantity;
			}
			set
			{
				CardActionPatcher.Enumerators.Add(this.ModifyDurability(value, DurabilitiesTypes.Liquid));
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003CC6 File Offset: 0x00001EC6
		private IEnumerator ModifyDurability(float val, DurabilitiesTypes types)
		{
			CardAccessBridge.<>c__DisplayClass74_0 CS$<>8__locals1;
			CS$<>8__locals1.val = val;
			if (this.CardBase == null)
			{
				yield break;
			}
			CardData cardModel = this.CardBase.CardModel;
			switch (types)
			{
			case DurabilitiesTypes.Spoilage:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.SpoilageTime, this.CardBase.CurrentSpoilage, this.CardBase, ref this.CardBase.CurrentSpoilage, ref this.CardBase.SpoilEmpty, ref this.CardBase.SpoilFull, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			case DurabilitiesTypes.Usage:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.UsageDurability, this.CardBase.CurrentUsageDurability, this.CardBase, ref this.CardBase.CurrentUsageDurability, ref this.CardBase.UsageEmpty, ref this.CardBase.UsageFull, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			case DurabilitiesTypes.Fuel:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.FuelCapacity, this.CardBase.CurrentFuel, this.CardBase, ref this.CardBase.CurrentFuel, ref this.CardBase.FuelEmpty, ref this.CardBase.FuelFull, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			case DurabilitiesTypes.Progress:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.Progress, this.CardBase.CurrentProgress, this.CardBase, ref this.CardBase.CurrentProgress, ref this.CardBase.ProgressEmpty, ref this.CardBase.ProgressFull, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			case DurabilitiesTypes.Liquid:
			{
				if (!this.CardBase.IsLiquid)
				{
					yield break;
				}
				bool liquidEmpty = this.CardBase.LiquidEmpty;
				this.CardBase.CurrentLiquidQuantity += CS$<>8__locals1.val;
				this.CardBase.CurrentLiquidQuantity = ((this.CardBase.CurrentMaxLiquidQuantity > 0f) ? Mathf.Clamp(this.CardBase.CurrentLiquidQuantity, 0f, this.CardBase.CurrentMaxLiquidQuantity) : Mathf.Min(this.CardBase.CurrentLiquidQuantity, 0f));
				this.CardBase.WeightHasChanged();
				if (!liquidEmpty && this.CardBase.LiquidEmpty)
				{
					yield return GameManager.PerformActionAsEnumerator(CardData.OnEvaporatedAction, this.CardBase, false);
				}
				break;
			}
			case DurabilitiesTypes.Special1:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.SpecialDurability1, this.CardBase.CurrentSpecial1, this.CardBase, ref this.CardBase.CurrentSpecial1, ref this.CardBase.Special1Empty, ref this.CardBase.Special1Full, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			case DurabilitiesTypes.Special2:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.SpecialDurability2, this.CardBase.CurrentSpecial2, this.CardBase, ref this.CardBase.CurrentSpecial2, ref this.CardBase.Special2Empty, ref this.CardBase.Special2Full, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			case DurabilitiesTypes.Special3:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.SpecialDurability3, this.CardBase.CurrentSpecial3, this.CardBase, ref this.CardBase.CurrentSpecial3, ref this.CardBase.Special3Empty, ref this.CardBase.Special3Full, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			case DurabilitiesTypes.Special4:
			{
				IEnumerator enumerator;
				if (CardAccessBridge.<ModifyDurability>g__inner_modify|74_0(cardModel.SpecialDurability4, this.CardBase.CurrentSpecial4, this.CardBase, ref this.CardBase.CurrentSpecial4, ref this.CardBase.Special4Empty, ref this.CardBase.Special4Full, out enumerator, ref CS$<>8__locals1))
				{
					yield return enumerator;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("types", types, null);
			}
			this.CardBase.CardVisuals.RefreshDurabilities();
			yield break;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public void AddCard(string id, int count = 1, [Nullable(2)] LuaTable ext = null)
		{
			CardData fromID = UniqueIDScriptable.GetFromID<CardData>(id);
			if (fromID == null)
			{
				return;
			}
			TransferedDurabilities transferedDurabilities = new TransferedDurabilities
			{
				Liquid = (float)((fromID.CardType == CardTypes.Liquid) ? count : 0),
				Usage = fromID.UsageDurability.Copy(),
				Fuel = fromID.FuelCapacity.Copy(),
				Spoilage = fromID.SpoilageTime.Copy(),
				ConsumableCharges = fromID.Progress.Copy(),
				Special1 = fromID.SpecialDurability1.Copy(),
				Special2 = fromID.SpecialDurability2.Copy(),
				Special3 = fromID.SpecialDurability3.Copy(),
				Special4 = fromID.SpecialDurability4.Copy()
			};
			SpawningLiquid spawningLiquid = new SpawningLiquid
			{
				LiquidCard = fromID.DefaultLiquidContained.LiquidCard,
				StayEmpty = !fromID.DefaultLiquidContained.LiquidCard
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
				CardData cardData = ((simpleUniqueAccess != null) ? simpleUniqueAccess.UniqueIDScriptable : null) as CardData;
				spawningLiquid.LiquidCard = cardData;
				spawningLiquid.StayEmpty = !cardData;
				ref count.TryModBy(ext["count"]);
				CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge = ext["initData"] as CardActionPatcher.DataNodeTableAccessBridge;
				if (dataNodeTableAccessBridge != null)
				{
					arg = dataNodeTableAccessBridge;
				}
			}
			int num = 0;
			do
			{
				num++;
				List<IEnumerator> enumerators = CardActionPatcher.Enumerators;
				GameManager instance = MBSingleton<GameManager>.Instance;
				CardData data = fromID;
				InGameCardBase cardBase = this.CardBase;
				bool inCurrentEnv = true;
				TransferedDurabilities transferedDurabilites = transferedDurabilities;
				bool useDefaultInventory = true;
				SpawningLiquid withLiquid = spawningLiquid;
				Vector2Int tick = new Vector2Int(MBSingleton<GameManager>.Instance.CurrentTickInfo.z, -1);
				bool moveView = false;
				Action<InGameCardBase, CardActionPatcher.DataNodeTableAccessBridge> action;
				if ((action = CardAccessBridge.<>O.<0>__SetInitData) == null)
				{
					action = (CardAccessBridge.<>O.<0>__SetInitData = new Action<InGameCardBase, CardActionPatcher.DataNodeTableAccessBridge>(SimpleUniqueAccess.SetInitData));
				}
				enumerators.Add(instance.MoniAddCard(data, cardBase, inCurrentEnv, transferedDurabilites, useDefaultInventory, withLiquid, tick, moveView, action, arg));
			}
			while (num < count && fromID.CardType != CardTypes.Liquid);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003FA7 File Offset: 0x000021A7
		public void Remove(bool doDrop)
		{
			CardActionPatcher.Enumerators.Add(MBSingleton<GameManager>.Instance.RemoveCard(this.CardBase, true, doDrop, GameManager.RemoveOption.Standard, false));
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004008 File Offset: 0x00002208
		[CompilerGenerated]
		internal static bool <ModifyDurability>g__inner_modify|74_0(DurabilityStat durabilityStat, float rawCurrentSpoilage, InGameCardBase card, ref float durabilityStat_ref, ref bool durabilityStat_ref_empty, ref bool durabilityStat_ref_full, [Nullable(2)] out IEnumerator _enumerator, ref CardAccessBridge.<>c__DisplayClass74_0 A_7)
		{
			_enumerator = null;
			if (!durabilityStat)
			{
				return false;
			}
			if (A_7.val >= durabilityStat.Max)
			{
				durabilityStat_ref_empty = false;
				durabilityStat_ref = durabilityStat.Max;
				if (rawCurrentSpoilage < durabilityStat.Max)
				{
					durabilityStat_ref_full = true;
					_enumerator = card.PerformDurabilitiesActions(true);
					return true;
				}
				return false;
			}
			else
			{
				if (A_7.val > 0f)
				{
					durabilityStat_ref = A_7.val;
					durabilityStat_ref_empty = false;
					durabilityStat_ref_full = false;
					return false;
				}
				durabilityStat_ref_full = false;
				durabilityStat_ref = 0f;
				if (rawCurrentSpoilage > 0f)
				{
					durabilityStat_ref_empty = true;
					_enumerator = card.PerformDurabilitiesActions(true);
					return true;
				}
				return false;
			}
		}

		// Token: 0x04000025 RID: 37
		[Nullable(2)]
		public readonly InGameCardBase CardBase;

		// Token: 0x04000026 RID: 38
		public static readonly Regex KVDataCheck = new Regex("zender\\.luaSupportData\\.\\{(?<key>.+?)\\}:\\{(?<val>.+?)\\}");

		// Token: 0x04000027 RID: 39
		private DataNode? _dataNode;

		// Token: 0x04000028 RID: 40
		private static readonly Regex DataNodeReg = new Regex("^LNbt\\|\\>(?<nbt>.+?)\\<\\|$");

		// Token: 0x02000010 RID: 16
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000029 RID: 41
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
