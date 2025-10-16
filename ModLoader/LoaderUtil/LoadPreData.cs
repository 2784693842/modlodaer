using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using LitJson;
using ModLoader.FFI;
using UnityEngine;

namespace ModLoader.LoaderUtil
{
	// Token: 0x02000024 RID: 36
	public static class LoadPreData
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00008F78 File Offset: 0x00007178
		public static void LoadFromPreLoadData()
		{
			try
			{
				foreach (Task<ValueTuple<List<ValueTuple<byte[], string, Type>>, string>> task in ModLoader.uniqueObjWaitList)
				{
					task.Wait();
					ValueTuple<List<ValueTuple<byte[], string, Type>>, string> result = task.Result;
					List<ValueTuple<byte[], string, Type>> item = result.Item1;
					string item2 = result.Item2;
					foreach (ValueTuple<byte[], string, Type> valueTuple in item)
					{
						byte[] item3 = valueTuple.Item1;
						string item4 = valueTuple.Item2;
						Type item5 = valueTuple.Item3;
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item4);
						string cardPath = item4;
						try
						{
							string text = Encoding.UTF8.GetString(item3);
							if (Path.GetExtension(item4).EndsWith("jsonnet", true, null))
							{
								text = JsonnetRuntime.JsonnetEval(Path.GetFileNameWithoutExtension(item4), text);
							}
							JsonData jsonData = JsonMapper.ToObject(text);
							if (!jsonData.ContainsKey("UniqueID") || !jsonData["UniqueID"].IsString || Utility.IsNullOrWhiteSpace(jsonData["UniqueID"].ToString()))
							{
								Debug.LogErrorFormat("{0} EditorLoadZip {1} {2} try to load a UniqueIDScriptable without GUID", new object[]
								{
									item5.Name,
									item2,
									fileNameWithoutExtension
								});
							}
							else
							{
								UniqueIDScriptable uniqueIDScriptable = ScriptableObject.CreateInstance(item5) as UniqueIDScriptable;
								JsonUtility.FromJsonOverwrite(text, uniqueIDScriptable);
								uniqueIDScriptable.name = item2 + "_" + fileNameWithoutExtension;
								string uniqueID = uniqueIDScriptable.UniqueID;
								ModLoader.AllGUIDDict.Add(uniqueID, uniqueIDScriptable);
								GameLoad.Instance.DataBase.AllData.Add(uniqueIDScriptable);
								if (!ModLoader.WaitForWarpperEditorGuidDict.ContainsKey(uniqueID))
								{
									ModLoader.WaitForWarpperEditorGuidDict.Add(uniqueID, new ModLoader.ScriptableObjectPack(uniqueIDScriptable, "", cardPath, item2, text));
								}
								else
								{
									Debug.LogWarningFormat("{0} WaitForWarpperEditorGuidDict Same Key was Add {1}", new object[]
									{
										item2,
										uniqueID
									});
								}
								if (!ModLoader.AllScriptableObjectDict.ContainsKey(uniqueID))
								{
									ModLoader.AllScriptableObjectDict.Add(uniqueID, uniqueIDScriptable);
								}
								Dictionary<string, UniqueIDScriptable> dictionary;
								if (ModLoader.AllGUIDTypeDict.TryGetValue(item5, out dictionary) && !dictionary.ContainsKey(uniqueID))
								{
									dictionary.Add(uniqueID, uniqueIDScriptable);
								}
							}
						}
						catch (Exception ex)
						{
							Debug.LogWarningFormat("{0} EditorLoad {1} {2} Error {3}", new object[]
							{
								item5.Name,
								item2,
								fileNameWithoutExtension,
								ex.Message
							});
						}
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00009240 File Offset: 0x00007440
		public static void LoadData(string mods_dir)
		{
			try
			{
				foreach (string text in Directory.GetDirectories(mods_dir))
				{
					if (File.Exists(ModLoader.CombinePaths(new string[]
					{
						text,
						"ModInfo.json"
					})))
					{
						ModInfo modInfo = new ModInfo();
						string text2 = Path.GetFileName(text);
						try
						{
							using (StreamReader streamReader = new StreamReader(ModLoader.CombinePaths(new string[]
							{
								text,
								"ModInfo.json"
							})))
							{
								JsonUtility.FromJsonOverwrite(streamReader.ReadToEnd(), modInfo);
							}
							if (!Utility.IsNullOrWhiteSpace(modInfo.Name))
							{
								text2 = modInfo.Name;
							}
							ModLoader.ModPacks[text2] = new ModPack(modInfo, text2, ModLoader.Instance.Config.Bind<bool>("是否加载某个模组", (text2 + "_" + modInfo.Name).EscapeStr(), true, "是否加载" + text2));
							if (!ModLoader.ModPacks[text2].EnableEntry.Value)
							{
								goto IL_212;
							}
							Debug.Log("ModLoader PreLoad Mod " + text2 + " " + modInfo.Version);
							Version version = Version.Parse(modInfo.ModLoaderVerison);
							if (ModLoader.PluginVersion.CompareTo(version) < 0)
							{
								Debug.LogWarningFormat("ModLoader Version {0} is lower than {1} Request Version {2}", new object[]
								{
									ModLoader.PluginVersion,
									text2,
									version
								});
							}
						}
						catch (Exception ex)
						{
							Debug.LogWarningFormat("{0} Check Version Error {1}", new object[]
							{
								text2,
								ex
							});
						}
						try
						{
							string path = ModLoader.CombinePaths(new string[]
							{
								text,
								ResourceLoadHelper.ResourcePat,
								ResourceLoadHelper.PicturePat
							});
							if (Directory.Exists(path))
							{
								string[] files = Directory.GetFiles(path);
								PostSpriteLoad.SpriteLoadQueue.Enqueue(ResourceLoadHelper.LoadPictures(text2, files));
							}
						}
						catch (Exception ex2)
						{
							Debug.LogWarningFormat("{0} Load Pictures Error {1}", new object[]
							{
								text2,
								ex2
							});
						}
						try
						{
							ModLoader.uniqueObjWaitList.Add(ResourceLoadHelper.LoadUniqueObjs(text2, text, ModLoader.GameSourceAssembly, modInfo));
						}
						catch (Exception ex3)
						{
							Debug.LogWarningFormat("{0} Load UniqueIDScriptable Error {1}", new object[]
							{
								text2,
								ex3
							});
						}
					}
					IL_212:;
				}
			}
			catch (Exception arg)
			{
				ModLoader.Instance.CommonLogger.LogError(string.Format("loading error :{0}", arg));
			}
			finally
			{
				PostSpriteLoad.NoMoreSpriteLoadQueue = true;
			}
		}
	}
}
