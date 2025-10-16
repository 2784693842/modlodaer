using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CSTI_LuaActionSupport.Helper;
using CSTI_LuaActionSupport.LuaCodeHelper;
using HarmonyLib;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.AllPatcher
{
	// Token: 0x02000047 RID: 71
	[NullableContext(1)]
	[Nullable(0)]
	public static class CardActionPatcher
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00006C59 File Offset: 0x00004E59
		public static LuaTable InnerFuncBase
		{
			get
			{
				return (LuaTable)CardActionPatcher.LuaRuntime["CSTI_LuaActionSupport__InnerFunc"];
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006C70 File Offset: 0x00004E70
		static CardActionPatcher()
		{
			CardActionPatcher.LuaRuntime.State.OpenLibs();
			CardActionPatcher.LuaRuntime.State.Encoding = Encoding.UTF8;
			CardActionPatcher.LuaRuntime["std__debug"] = CardActionPatcher.LuaRuntime["debug"];
			CardActionPatcher.LuaRuntime["debug"] = CardActionPatcher.DebugBridge;
			CardActionPatcher.LuaRuntime["SimpleAccessTool"] = new SimpleAccessTool();
			CardActionPatcher.LuaRuntime.Register(typeof(DataAccessTool), null);
			CardActionPatcher.LuaRuntime.Register(typeof(CardActionPatcher), null);
			CardActionPatcher.LuaRuntime.Register(typeof(LuaTimer), "LuaTimer");
			CardActionPatcher.LuaRuntime.Register(typeof(LuaInput), "LuaInput");
			CardActionPatcher.LuaRuntime.Register(null);
			CardActionPatcher.LuaRuntime.Register(null);
			CardActionPatcher.LuaRuntime["Enum"] = LuaEnum.Enum;
			CardActionPatcher.LuaRuntime["Register"] = LuaRegister.Register;
			CardActionPatcher.LuaRuntime.LoadCLRPackage();
			CardActionPatcher.LuaRuntime.NewTable("ModData");
			CardActionPatcher.ModData = CardActionPatcher.LuaRuntime.GetTable("ModData");
			CardActionPatcher.LuaRuntime["ModData"] = CardActionPatcher.ModData;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006DF4 File Offset: 0x00004FF4
		public static Dictionary<string, DataNode> CurrentGSlotSaveData()
		{
			Dictionary<string, DataNode> result;
			if (CardActionPatcher.GSlotSaveData.TryGetValue(GameLoad.Instance.CurrentGameDataIndex, out result))
			{
				return result;
			}
			CardActionPatcher.GSlotSaveData[GameLoad.Instance.CurrentGameDataIndex] = new Dictionary<string, DataNode>();
			return CardActionPatcher.GSlotSaveData[GameLoad.Instance.CurrentGameDataIndex];
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006E48 File Offset: 0x00005048
		[LuaFunc]
		[TestCode("SaveCurrentSlot(\"__test\",10)")]
		public static void SaveCurrentSlot(string key, object val)
		{
			CardActionPatcher.CurrentGSlotSaveData().CommonSave(key, val);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006E56 File Offset: 0x00005056
		[LuaFunc]
		public static void SaveGlobal(string key, object val)
		{
			CardActionPatcher.GSaveData.CommonSave(key, val);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006E64 File Offset: 0x00005064
		[LuaFunc]
		[return: Nullable(2)]
		public static object LoadCurrentSlot(string key)
		{
			DataNode dataNode;
			if (CardActionPatcher.CurrentGSlotSaveData().TryGetValue(key, out dataNode))
			{
				return dataNode.CommonLoad();
			}
			return "";
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006E90 File Offset: 0x00005090
		[LuaFunc]
		[return: Nullable(2)]
		public static object LoadGlobal(string key)
		{
			DataNode dataNode;
			if (CardActionPatcher.GSaveData.TryGetValue(key, out dataNode))
			{
				return dataNode.CommonLoad();
			}
			return "";
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00006EBC File Offset: 0x000050BC
		[NullableContext(2)]
		private static object CommonLoad(this DataNode node)
		{
			object result;
			switch (node.NodeType)
			{
			case DataNode.DataNodeType.Number:
			{
				DataNode dataNode = node;
				result = dataNode.number;
				break;
			}
			case DataNode.DataNodeType.Str:
			{
				DataNode dataNode = node;
				result = dataNode.str;
				break;
			}
			case DataNode.DataNodeType.Bool:
			{
				DataNode dataNode = node;
				result = dataNode._bool;
				break;
			}
			case DataNode.DataNodeType.Table:
			{
				DataNode dataNode = node;
				result = new CardActionPatcher.DataNodeTableAccessBridge(dataNode.table);
				break;
			}
			case DataNode.DataNodeType.Nil:
				result = null;
				break;
			case DataNode.DataNodeType.Vector2:
			{
				DataNode dataNode = node;
				result = dataNode.vector2;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00006F64 File Offset: 0x00005164
		[return: Nullable(2)]
		private static object CommonLoad([Nullable(new byte[]
		{
			2,
			1
		})] this IDictionary<string, DataNode> dataNodes, string key)
		{
			if (dataNodes == null)
			{
				return null;
			}
			if (dataNodes.SafeGet(key) == null)
			{
				return null;
			}
			DataNode? dataNode;
			DataNode valueOrDefault = dataNode.GetValueOrDefault();
			return valueOrDefault.CommonLoad();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006F98 File Offset: 0x00005198
		private static void CommonSave([Nullable(new byte[]
		{
			2,
			1
		})] this IDictionary<string, DataNode> dataNodes, string key, [Nullable(2)] object val)
		{
			if (dataNodes == null)
			{
				return;
			}
			if (val == null)
			{
				DataNode nil = DataNode.Nil;
				dataNodes[key] = nil;
				return;
			}
			if (val is double)
			{
				double number = (double)val;
				DataNode value = new DataNode(number);
				dataNodes[key] = value;
				return;
			}
			if (val is long)
			{
				long num = (long)val;
				DataNode value2 = new DataNode((double)num);
				dataNodes[key] = value2;
				return;
			}
			string text = val as string;
			if (text != null)
			{
				DataNode value3 = new DataNode(text);
				dataNodes[key] = value3;
				return;
			}
			if (val is bool)
			{
				bool b = (bool)val;
				DataNode value4 = new DataNode(b);
				dataNodes[key] = value4;
				return;
			}
			LuaTable luaTable = val as LuaTable;
			if (luaTable != null)
			{
				Dictionary<string, DataNode> dataNodes2 = new Dictionary<string, DataNode>();
				foreach (object obj in luaTable.Keys)
				{
					string text2 = obj as string;
					bool flag = text2 != null;
					object obj2;
					if (flag)
					{
						obj2 = luaTable[text2];
						bool flag2 = obj2 is double || obj2 is long || obj2 is string || obj2 is bool || obj2 == null || obj2 is LuaTable;
						flag = flag2;
					}
					if (flag)
					{
						dataNodes2.CommonSave(text2, obj2);
					}
				}
				DataNode value5 = new DataNode(dataNodes2);
				dataNodes[key] = value5;
				return;
			}
			Dictionary<string, object> dictionary = val as Dictionary<string, object>;
			if (dictionary == null)
			{
				return;
			}
			Dictionary<string, DataNode> dataNodes3 = new Dictionary<string, DataNode>();
			foreach (string key2 in dictionary.Keys)
			{
				object obj3 = dictionary[key2];
				bool flag = obj3 is double || obj3 is long || obj3 is string || obj3 is bool || obj3 == null || obj3 is LuaTable || obj3 is Dictionary<string, object>;
				if (flag)
				{
					dataNodes3.CommonSave(key2, obj3);
				}
			}
			DataNode value6 = new DataNode(dataNodes3);
			dataNodes[key] = value6;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000071DC File Offset: 0x000053DC
		[HarmonyPostfix]
		[HarmonyPatch(typeof(CardAction), "WillHaveAnEffect")]
		private static void LuaActionWillHaveAnEffect(CardAction __instance, ref bool __result)
		{
			string localizationKey = __instance.ActionName.LocalizationKey;
			bool? flag = (localizationKey != null) ? new bool?(localizationKey.StartsWith("LuaCardAction")) : null;
			if (flag != null && flag.GetValueOrDefault())
			{
				__result = true;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007228 File Offset: 0x00005428
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "ActionRoutine")]
		private static void LuaCardAction(CardAction _Action, InGameCardBase _ReceivingCard, GameManager __instance, ref IEnumerator __result)
		{
			string localizationKey = _Action.ActionName.LocalizationKey;
			bool? flag = (localizationKey != null) ? new bool?(localizationKey.StartsWith("LuaCardAction")) : null;
			if (flag != null && flag.GetValueOrDefault())
			{
				__result = __result.Prepend(CardActionPatcher.LuaCardActionHelper(_Action, _ReceivingCard, __instance));
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00007284 File Offset: 0x00005484
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "CardOnCardActionRoutine")]
		private static void LuaCardOnCardAction(CardOnCardAction _Action, InGameCardBase _ReceivingCard, InGameCardBase _GivenCard, GameManager __instance, ref IEnumerator __result)
		{
			string localizationKey = _Action.ActionName.LocalizationKey;
			bool? flag = (localizationKey != null) ? new bool?(localizationKey.StartsWith("LuaCardOnCardAction")) : null;
			if (flag != null && flag.GetValueOrDefault())
			{
				__result = __result.Prepend(CardActionPatcher.LuaCardOnCardActionHelper(_Action, _ReceivingCard, _GivenCard, __instance));
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000072E4 File Offset: 0x000054E4
		public static Lua InitRuntime(GameManager __instance)
		{
			CardActionPatcher.LuaRuntime["gameManager"] = __instance;
			CardActionPatcher.LuaRuntime["env"] = new CardAccessBridge(__instance.CurrentEnvironmentCard);
			CardActionPatcher.LuaRuntime["exp"] = new CardAccessBridge(__instance.CurrentExplorableCard);
			CardActionPatcher.LuaRuntime["weather"] = new CardAccessBridge(__instance.CurrentWeatherCard);
			return CardActionPatcher.LuaRuntime;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007354 File Offset: 0x00005554
		public static IEnumerator LuaCardActionHelper(CardAction _Action, InGameCardBase _ReceivingCard, GameManager __instance)
		{
			Lua lua = CardActionPatcher.InitRuntime(__instance);
			int waitTime = 0;
			int miniWaitTime = 0;
			int tickWaitTime = 0;
			lua["receive"] = new CardAccessBridge(_ReceivingCard);
			LuaScriptRetValues luaScriptRetValues = new LuaScriptRetValues();
			lua["Ret"] = luaScriptRetValues;
			try
			{
				lua.DoString(_Action.ActionName.ParentObjectID, _Action.ActionName.LocalizationKey);
				object o;
				if (luaScriptRetValues.CheckKey<object>("result", out o))
				{
					ref waitTime.TryModBy(o);
				}
				object o2;
				if (luaScriptRetValues.CheckKey<object>("miniTime", out o2))
				{
					ref miniWaitTime.TryModBy(o2);
				}
				object o3;
				if (luaScriptRetValues.CheckKey<object>("tickTime", out o3))
				{
					ref tickWaitTime.TryModBy(o3);
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			Queue<CoroutineController> queue = __instance.ProcessCache().ProcessTime(_ReceivingCard, waitTime, miniWaitTime, tickWaitTime);
			while (queue.Count > 0)
			{
				CoroutineController coroutineController = queue.Dequeue();
				while (coroutineController.state == CoroutineState.Running)
				{
					yield return null;
				}
				coroutineController = null;
			}
			yield break;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007371 File Offset: 0x00005571
		public static IEnumerator LuaCardOnCardActionHelper(CardAction _Action, InGameCardBase _ReceivingCard, InGameCardBase _GivenCard, GameManager __instance)
		{
			Lua lua = CardActionPatcher.InitRuntime(__instance);
			int waitTime = 0;
			int miniWaitTime = 0;
			int tickWaitTime = 0;
			lua["receive"] = new CardAccessBridge(_ReceivingCard);
			lua["given"] = new CardAccessBridge(_GivenCard);
			LuaScriptRetValues luaScriptRetValues = new LuaScriptRetValues();
			lua["Ret"] = luaScriptRetValues;
			try
			{
				lua.DoString(_Action.ActionName.ParentObjectID, _Action.ActionName.LocalizationKey);
				object o;
				if (luaScriptRetValues.CheckKey<object>("result", out o))
				{
					ref waitTime.TryModBy(o);
				}
				object o2;
				if (luaScriptRetValues.CheckKey<object>("miniTime", out o2))
				{
					ref miniWaitTime.TryModBy(o2);
				}
				object o3;
				if (luaScriptRetValues.CheckKey<object>("tickTime", out o3))
				{
					ref tickWaitTime.TryModBy(o3);
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			Queue<CoroutineController> queue = __instance.ProcessCache().ProcessTime(_ReceivingCard, waitTime, miniWaitTime, tickWaitTime);
			while (queue.Count > 0)
			{
				CoroutineController coroutineController = queue.Dequeue();
				while (coroutineController.state == CoroutineState.Running)
				{
					yield return null;
				}
				coroutineController = null;
			}
			yield break;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007398 File Offset: 0x00005598
		public static Queue<CoroutineController> ProcessTime(this Queue<CoroutineController> queue, InGameCardBase _ReceivingCard, int waitTime, int miniWaitTime, int tickWaitTime)
		{
			GameManager instance = MBSingleton<GameManager>.Instance;
			if (tickWaitTime > 0)
			{
				object obj = CardActionPatcher.LoadCurrentSlot("C#Used__tickWaitTime");
				if (obj != null)
				{
					tickWaitTime += obj.TryNum<int>().GetValueOrDefault();
				}
				miniWaitTime += tickWaitTime / 10;
				tickWaitTime %= 10;
				CardActionPatcher.SaveCurrentSlot("C#Used__tickWaitTime", tickWaitTime);
			}
			if (miniWaitTime > 0)
			{
				MBSingleton<GameManager>.Instance.CurrentMiniTicks += miniWaitTime;
				waitTime += MBSingleton<GameManager>.Instance.CurrentMiniTicks / 5;
				MBSingleton<GameManager>.Instance.CurrentMiniTicks %= 5;
				MBSingleton<GraphicsManager>.Instance.UpdateTimeInfo(false);
			}
			if (waitTime > 0)
			{
				queue.Enqueue(instance.SpendDaytimePoints(waitTime, _ReceivingCard).Start(instance));
			}
			return queue;
		}

		// Token: 0x04000094 RID: 148
		public static readonly Lua LuaRuntime = new Lua(true);

		// Token: 0x04000095 RID: 149
		public static readonly DebugBridge DebugBridge = new DebugBridge();

		// Token: 0x04000096 RID: 150
		public static readonly LuaTable ModData;

		// Token: 0x04000097 RID: 151
		public static readonly Dictionary<string, DataNode> GSaveData = new Dictionary<string, DataNode>();

		// Token: 0x04000098 RID: 152
		public static readonly Dictionary<int, Dictionary<string, DataNode>> GSlotSaveData = new Dictionary<int, Dictionary<string, DataNode>>();

		// Token: 0x04000099 RID: 153
		private const string _InnerFuncBase = "CSTI_LuaActionSupport__InnerFunc";

		// Token: 0x0400009A RID: 154
		public static readonly List<IEnumerator> Enumerators = new List<IEnumerator>();

		// Token: 0x02000048 RID: 72
		[NullableContext(2)]
		[Nullable(0)]
		public class DataNodeTableAccessBridge
		{
			// Token: 0x17000049 RID: 73
			// (get) Token: 0x0600016E RID: 366 RVA: 0x00007450 File Offset: 0x00005650
			// (set) Token: 0x0600016F RID: 367 RVA: 0x000074F8 File Offset: 0x000056F8
			public LuaTable LuaTable
			{
				get
				{
					if (this.Table == null)
					{
						return null;
					}
					LuaTable luaTable = CardActionPatcher.LuaRuntime.TempTable();
					foreach (KeyValuePair<string, DataNode> pair in this.Table)
					{
						string text;
						DataNode dataNode;
						pair.Deconstruct(out text, out dataNode);
						string field = text;
						DataNode dataNode2 = dataNode;
						if (dataNode2.NodeType != DataNode.DataNodeType.Nil)
						{
							object obj = dataNode2.CommonLoad();
							CardActionPatcher.DataNodeTableAccessBridge dataNodeTableAccessBridge = obj as CardActionPatcher.DataNodeTableAccessBridge;
							if (dataNodeTableAccessBridge != null)
							{
								luaTable[field] = dataNodeTableAccessBridge.LuaTable;
							}
							else
							{
								luaTable[field] = obj;
							}
						}
					}
					return luaTable;
				}
				set
				{
					if (value == null)
					{
						return;
					}
					foreach (object obj in value.Keys)
					{
						string text = obj as string;
						if (text != null)
						{
							this.Table.CommonSave(text, value[text]);
						}
					}
				}
			}

			// Token: 0x1700004A RID: 74
			// (get) Token: 0x06000170 RID: 368 RVA: 0x00007564 File Offset: 0x00005764
			public LuaTable LuaKeys
			{
				get
				{
					if (this.Keys == null)
					{
						return null;
					}
					LuaTable luaTable = CardActionPatcher.LuaRuntime.TempTable();
					foreach (string value in this.Keys)
					{
						luaTable[luaTable.Keys.Count + 1] = value;
					}
					return luaTable;
				}
			}

			// Token: 0x1700004B RID: 75
			public object this[string key]
			{
				[NullableContext(1)]
				[return: Nullable(2)]
				get
				{
					Dictionary<string, DataNode> table = this.Table;
					DataNode dataNode;
					if (table != null && table.TryGetValue(key, out dataNode))
					{
						return dataNode.CommonLoad();
					}
					return null;
				}
				[NullableContext(1)]
				[param: Nullable(2)]
				set
				{
					this.Table.CommonSave(key, value);
				}
			}

			// Token: 0x1700004C RID: 76
			// (get) Token: 0x06000173 RID: 371 RVA: 0x0000761C File Offset: 0x0000581C
			[Nullable(new byte[]
			{
				2,
				1
			})]
			public Dictionary<string, DataNode>.KeyCollection Keys
			{
				[return: Nullable(new byte[]
				{
					2,
					1
				})]
				get
				{
					Dictionary<string, DataNode> table = this.Table;
					if (table == null)
					{
						return null;
					}
					return table.Keys;
				}
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x06000174 RID: 372 RVA: 0x0000762F File Offset: 0x0000582F
			public int Count
			{
				get
				{
					Dictionary<string, DataNode> table = this.Table;
					if (table == null)
					{
						return 0;
					}
					return table.Count;
				}
			}

			// Token: 0x06000175 RID: 373 RVA: 0x00007642 File Offset: 0x00005842
			public DataNodeTableAccessBridge([Nullable(new byte[]
			{
				2,
				1
			})] Dictionary<string, DataNode> table)
			{
				this.Table = table;
			}

			// Token: 0x0400009B RID: 155
			[Nullable(new byte[]
			{
				2,
				1
			})]
			public readonly Dictionary<string, DataNode> Table;
		}
	}
}
