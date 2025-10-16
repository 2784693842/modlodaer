using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using BepInEx;
using CSTI_LuaActionSupport.LuaCodeHelper;
using gfoidl.Base64;
using HarmonyLib;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.AllPatcher
{
	// Token: 0x02000057 RID: 87
	[NullableContext(1)]
	[Nullable(0)]
	public static class OnGameLoad
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x00008E67 File Offset: 0x00007067
		private static byte[] Decode(string data)
		{
			return Base64.Default.Decode(data.AsSpan());
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00008E7C File Offset: 0x0000707C
		[HarmonyPrefix]
		[HarmonyPatch(typeof(GameSaveData), "CreateDicts")]
		public static void LoadLuaLongTimeData(GameSaveData __instance)
		{
			if (__instance.AllEndgameLogs.Count > 0 && __instance.AllEndgameLogs[0].CategoryID == "zender.modLoaderUse.luaLongTimeSave")
			{
				CardActionPatcher.GSlotSaveData[GameLoad.Instance.CurrentGameDataIndex] = new Dictionary<string, DataNode>();
				string logText = __instance.AllEndgameLogs[0].LogText;
				BinaryReader binaryReader = new BinaryReader(new MemoryStream(Base64.Default.Decode(logText.AsSpan())));
				try
				{
					int num;
					using (SavePatcher.LoadEnv.BeginLoadEnv(binaryReader, out num, new int?(0)))
					{
						int num2 = binaryReader.ReadInt32();
						for (int i = 0; i < num2; i++)
						{
							string key = binaryReader.ReadString();
							CardActionPatcher.GSlotSaveData[GameLoad.Instance.CurrentGameDataIndex][key] = DataNode.Load(binaryReader);
						}
					}
				}
				catch (Exception message)
				{
					Debug.LogWarning(message);
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008F84 File Offset: 0x00007184
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GameManager), "Awake")]
		public static void DoOnGameLoad(GameManager __instance)
		{
			Lua initRuntime = CardActionPatcher.InitRuntime(__instance);
			LuaSupportRuntime.LuaFilesOnGameLoad.Do(delegate(string pat)
			{
				initRuntime.DoString(File.ReadAllText(pat), "chunk");
			});
			DataNode dataNode;
			if (CardActionPatcher.CurrentGSlotSaveData().TryGetValue("__StatCache", out dataNode))
			{
				foreach (KeyValuePair<string, DataNode> pair in dataNode.table)
				{
					string text;
					DataNode dataNode2;
					pair.Deconstruct(out text, out dataNode2);
					string id = text;
					DataNode dataNode3 = dataNode2;
					GameStat fromID = UniqueIDScriptable.GetFromID<GameStat>(id);
					if (fromID != null)
					{
						foreach (KeyValuePair<string, DataNode> pair2 in dataNode3.table)
						{
							pair2.Deconstruct(out text, out dataNode2);
							string a = text;
							DataNode dataNode4 = dataNode2;
							if (!(a == "MinMaxValue"))
							{
								if (a == "MinMaxRate")
								{
									fromID.MinMaxRate = dataNode4.vector2;
								}
							}
							else
							{
								fromID.MinMaxValue = dataNode4.vector2;
							}
						}
					}
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000090B8 File Offset: 0x000072B8
		[HarmonyPrefix]
		[HarmonyPatch(typeof(GameLoad), "SaveGame")]
		public static void DoOnGameSave()
		{
			LuaSupportRuntime.LuaFilesOnGameSave.Do(delegate(string pat)
			{
				CardActionPatcher.LuaRuntime.DoString(File.ReadAllText(pat), "chunk");
			});
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000090E4 File Offset: 0x000072E4
		[HarmonyPostfix]
		[HarmonyPatch(typeof(UniqueIDScriptable), "ClearDict")]
		public static void DoOnAfterModLoader()
		{
			foreach (string path in Directory.EnumerateDirectories(Paths.PluginPath))
			{
				if (File.Exists(Path.Combine(path, LuaSupportRuntime.ModInfo)))
				{
					if (Directory.Exists(Path.Combine(path, LuaSupportRuntime.LuaInit)))
					{
						foreach (string path2 in Directory.EnumerateFiles(Path.Combine(path, LuaSupportRuntime.LuaInit), "*.lua"))
						{
							try
							{
								CardActionPatcher.LuaRuntime.DoString(File.ReadAllText(path2), "chunk");
							}
							catch (Exception message)
							{
								Debug.LogWarning(message);
							}
						}
					}
					if (Directory.Exists(Path.Combine(path, LuaSupportRuntime.LuaOnGameLoad)))
					{
						LuaSupportRuntime.LuaFilesOnGameLoad.AddRange(Directory.EnumerateFiles(Path.Combine(path, LuaSupportRuntime.LuaOnGameLoad), "*.lua"));
					}
					if (Directory.Exists(Path.Combine(path, LuaSupportRuntime.LuaOnGameSave)))
					{
						LuaSupportRuntime.LuaFilesOnGameSave.AddRange(Directory.EnumerateFiles(Path.Combine(path, LuaSupportRuntime.LuaOnGameSave), "*.lua"));
					}
				}
			}
		}
	}
}
