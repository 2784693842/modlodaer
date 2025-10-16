using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.LuaCodeHelper;
using gfoidl.Base64;
using HarmonyLib;
using UnityEngine;

namespace CSTI_LuaActionSupport.AllPatcher
{
	// Token: 0x0200005A RID: 90
	[NullableContext(1)]
	[Nullable(0)]
	public static class SavePatcher
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00009269 File Offset: 0x00007469
		public static string SavePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "LuaModData", "LuaSave.bin");
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000927F File Offset: 0x0000747F
		public static string SaveBackupPath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "LuaModData", "LuaSave.bin.backup");
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009298 File Offset: 0x00007498
		[HarmonyPrefix]
		[HarmonyPatch(typeof(GameLoad), "DeleteGameData")]
		public static void OnDeleteGameData(GameLoad __instance, int _Index)
		{
			try
			{
				if (CardActionPatcher.GSlotSaveData.ContainsKey(_Index))
				{
					CardActionPatcher.GSlotSaveData.Remove(_Index);
				}
				SavePatcher.OnSave(__instance, -1, false);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000092E0 File Offset: 0x000074E0
		[HarmonyPrefix]
		[HarmonyPatch(typeof(GameLoad), "SaveGame")]
		public static void OnSave(GameLoad __instance, int _GameIndex, bool _Checkpoint)
		{
			try
			{
				DataNode dataNode;
				if (CardActionPatcher.CurrentGSlotSaveData().TryGetValue("__StatCache", out dataNode))
				{
					foreach (KeyValuePair<string, DataNode> pair5 in dataNode.table)
					{
						string text;
						DataNode dataNode2;
						pair5.Deconstruct(out text, out dataNode2);
						string id = text;
						DataNode dataNode3 = dataNode2;
						GameStat fromID = UniqueIDScriptable.GetFromID<GameStat>(id);
						if (fromID != null)
						{
							Vector2? vector = null;
							Vector2? vector2 = null;
							foreach (KeyValuePair<string, DataNode> pair2 in dataNode3.table)
							{
								pair2.Deconstruct(out text, out dataNode2);
								string a = text;
								if (!(a == "MinMaxValue"))
								{
									if (a == "MinMaxRate")
									{
										vector2 = new Vector2?(fromID.MinMaxRate);
									}
								}
								else
								{
									vector = new Vector2?(fromID.MinMaxValue);
								}
							}
							if (vector != null)
							{
								dataNode3.table["MinMaxValue"] = new DataNode(vector.Value);
							}
							if (vector2 != null)
							{
								dataNode3.table["MinMaxRate"] = new DataNode(vector2.Value);
							}
						}
					}
				}
				MemoryStream memoryStream = new MemoryStream();
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				MemoryStream memoryStream2 = new MemoryStream();
				BinaryWriter binaryWriter2 = new BinaryWriter(memoryStream2);
				using (SavePatcher.SaveEnv.BeginSaveEnv(binaryWriter, 0))
				{
					binaryWriter.Write(CardActionPatcher.GSaveData.Count((KeyValuePair<string, DataNode> pair) => pair.Value.NodeType != DataNode.DataNodeType.Nil));
					foreach (KeyValuePair<string, DataNode> pair3 in CardActionPatcher.GSaveData)
					{
						string text;
						DataNode dataNode2;
						pair3.Deconstruct(out text, out dataNode2);
						string value = text;
						DataNode dataNode4 = dataNode2;
						if (dataNode4.NodeType != DataNode.DataNodeType.Nil)
						{
							binaryWriter.Write(value);
							dataNode4.Save(binaryWriter);
						}
					}
				}
				if (File.Exists(SavePatcher.SavePath))
				{
					if (File.Exists(SavePatcher.SaveBackupPath))
					{
						File.Delete(SavePatcher.SaveBackupPath);
					}
					File.Move(SavePatcher.SavePath, SavePatcher.SaveBackupPath);
				}
				Directory.CreateDirectory(Path.GetDirectoryName(SavePatcher.SavePath) ?? string.Empty);
				using (FileStream fileStream = File.Create(SavePatcher.SavePath))
				{
					memoryStream.WriteTo(fileStream);
					fileStream.Flush();
					Dictionary<string, DataNode> dictionary;
					if (CardActionPatcher.GSlotSaveData.TryGetValue(_GameIndex, out dictionary))
					{
						using (SavePatcher.SaveEnv.BeginSaveEnv(binaryWriter2, 0))
						{
							binaryWriter2.Write(dictionary.Count((KeyValuePair<string, DataNode> pair) => pair.Value.NodeType != DataNode.DataNodeType.Nil));
							foreach (KeyValuePair<string, DataNode> pair4 in dictionary)
							{
								string text;
								DataNode dataNode2;
								pair4.Deconstruct(out text, out dataNode2);
								string value2 = text;
								DataNode dataNode5 = dataNode2;
								if (dataNode5.NodeType != DataNode.DataNodeType.Nil)
								{
									binaryWriter2.Write(value2);
									dataNode5.Save(binaryWriter2);
								}
							}
						}
						SavePatcher.SaveTo(__instance.Games[_GameIndex].MainData, memoryStream2);
						if (_Checkpoint)
						{
							SavePatcher.SaveTo(__instance.Games[_GameIndex].CheckpointData, memoryStream2);
						}
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00009708 File Offset: 0x00007908
		private static void SaveTo([Nullable(2)] GameSaveData saveData, MemoryStream data)
		{
			if (saveData == null)
			{
				return;
			}
			if (saveData.AllEndgameLogs == null)
			{
				saveData.AllEndgameLogs = new List<LogSaveData>();
			}
			if (saveData.AllEndgameLogs.Count > 0 && saveData.AllEndgameLogs[0].CategoryID == "zender.modLoaderUse.luaLongTimeSave")
			{
				LogSaveData value = saveData.AllEndgameLogs[0];
				value.LogText = Base64.Default.Encode(data.ToArray());
				saveData.AllEndgameLogs[0] = value;
				return;
			}
			LogSaveData item = new LogSaveData
			{
				CategoryID = "zender.modLoaderUse.luaLongTimeSave",
				LoggedOnTick = -1000,
				LogText = Base64.Default.Encode(data.ToArray())
			};
			saveData.AllEndgameLogs.Insert(0, item);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000097DB File Offset: 0x000079DB
		[HarmonyPrefix]
		[HarmonyPatch(typeof(EndgameMenu), "Setup")]
		public static void FixEndgameMenu_Setup(ref GameSaveData _SaveData, out List<LogSaveData> __state)
		{
			__state = _SaveData.AllEndgameLogs;
			_SaveData.AllEndgameLogs = (from data in __state
			where data.CategoryID != "zender.modLoaderUse.luaLongTimeSave"
			select data).ToList<LogSaveData>();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00009818 File Offset: 0x00007A18
		[HarmonyPostfix]
		[HarmonyPatch(typeof(EndgameMenu), "Setup")]
		public static void FixEndgameMenu_Setup_Post(ref GameSaveData _SaveData, List<LogSaveData> __state)
		{
			_SaveData.AllEndgameLogs = __state;
		}

		// Token: 0x040000D5 RID: 213
		public const string StatCache = "__StatCache";

		// Token: 0x040000D6 RID: 214
		public const string LuaLongTimeSaveId = "zender.modLoaderUse.luaLongTimeSave";

		// Token: 0x0200005B RID: 91
		[Nullable(0)]
		public class SaveEnv : IDisposable
		{
			// Token: 0x060001D1 RID: 465 RVA: 0x00009822 File Offset: 0x00007A22
			private SaveEnv(int envKey, BinaryWriter binaryWriter)
			{
				this.EnvKey = envKey;
				this.BinaryWriter = binaryWriter;
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x00009838 File Offset: 0x00007A38
			public static SavePatcher.SaveEnv BeginSaveEnv(BinaryWriter writer, int envKey)
			{
				writer.Write(envKey);
				return new SavePatcher.SaveEnv(envKey, writer);
			}

			// Token: 0x060001D3 RID: 467 RVA: 0x00009848 File Offset: 0x00007A48
			public void Dispose()
			{
				this.BinaryWriter.Write(this.EnvKey);
			}

			// Token: 0x040000D7 RID: 215
			private readonly int EnvKey;

			// Token: 0x040000D8 RID: 216
			private readonly BinaryWriter BinaryWriter;
		}

		// Token: 0x0200005C RID: 92
		[Nullable(0)]
		public class LoadEnv : IDisposable
		{
			// Token: 0x060001D4 RID: 468 RVA: 0x0000985B File Offset: 0x00007A5B
			private LoadEnv(int envKey, BinaryReader binaryReader)
			{
				this.EnvKey = envKey;
				this.BinaryReader = binaryReader;
			}

			// Token: 0x060001D5 RID: 469 RVA: 0x00009874 File Offset: 0x00007A74
			public static SavePatcher.LoadEnv BeginLoadEnv(BinaryReader reader, out int envKey, int? req = null)
			{
				envKey = reader.ReadInt32();
				if (req != null)
				{
					int num = envKey;
					int? num2 = req;
					if (!(num == num2.GetValueOrDefault() & num2 != null))
					{
						throw new Exception("Load Error");
					}
				}
				return new SavePatcher.LoadEnv(envKey, reader);
			}

			// Token: 0x060001D6 RID: 470 RVA: 0x000098BC File Offset: 0x00007ABC
			public void Dispose()
			{
				try
				{
					if (this.BinaryReader.ReadInt32() != this.EnvKey)
					{
						throw new Exception("Load Error");
					}
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
			}

			// Token: 0x040000D9 RID: 217
			private readonly int EnvKey;

			// Token: 0x040000DA RID: 218
			private readonly BinaryReader BinaryReader;
		}
	}
}
