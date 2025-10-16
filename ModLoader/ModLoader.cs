using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ChatTreeLoader.Patchers;
using HarmonyLib;
using Ionic.Zip;
using LitJson;
using ModLoader.LoaderUtil;
using ModLoader.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModLoader
{
	// Token: 0x0200001B RID: 27
	[BepInPlugin("Dop.plugin.CSTI.ModLoader", "ModLoader", "2.3.4.13")]
	public class ModLoader : BaseUnityPlugin
	{
		// Token: 0x06000050 RID: 80 RVA: 0x000043FC File Offset: 0x000025FC
		static ModLoader()
		{
			try
			{
				NormalPatcher.DoPatch(ModLoader.HarmonyInstance);
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogWarning(message);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00004618 File Offset: 0x00002818
		public ManualLogSource CommonLogger
		{
			get
			{
				return base.Logger;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004620 File Offset: 0x00002820
		private void Start()
		{
			base.StartCoroutine(ModLoader.FontLoader());
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000462E File Offset: 0x0000282E
		private static IEnumerator FontLoader()
		{
			AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromStreamAsync(EmbeddedResources.CSTIFonts);
			yield return assetBundleCreateRequest;
			ModLoader.FontAssetBundle = assetBundleCreateRequest.assetBundle;
			AssetBundleRequest assetBundleRequest = ModLoader.FontAssetBundle.LoadAssetAsync<TMP_FontAsset>("SourceHanSerifCN-SemiBold SDF");
			yield return assetBundleRequest;
			TMP_FontAsset tmp_FontAsset = assetBundleRequest.asset as TMP_FontAsset;
			tmp_FontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
			using (HashSet<TMP_FontAsset>.Enumerator enumerator = new HashSet<TMP_FontAsset>(from settings in Resources.FindObjectsOfTypeAll<FontSet>().SelectMany((FontSet set) => set.Settings)
			select settings.FontObject).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TMP_FontAsset tmp_FontAsset2 = enumerator.Current;
					if (tmp_FontAsset2.fallbackFontAssetTable == null)
					{
						tmp_FontAsset2.fallbackFontAssetTable = new List<TMP_FontAsset>
						{
							tmp_FontAsset
						};
					}
					else
					{
						tmp_FontAsset2.fallbackFontAssetTable.Add(tmp_FontAsset);
					}
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004638 File Offset: 0x00002838
		private void Awake()
		{
			MainUI.CreatePanel();
			ModLoader.MainUIBackPanelRT.sizeDelta = new Vector2(1920f, 1080f) * 0.55f;
			ModLoader.MainUIBackPanelRT.position = new Vector2(758.4f, 653.4f);
			ModLoader.MainUIBackPanelRT.gameObject.SetActive(false);
			ModLoader.Instance = this;
			CoroutineController controller;
			this.StartCoroutineEx(PostSpriteLoad.CompressOnLate(), out controller);
			PostSpriteLoad.Controller = controller;
			base.Config.Bind<bool>("是否将加载的纹理设置为只读", "SetTexture2ReadOnly", false, "将加载的纹理设置为只读可以减少内存使用但是之后不能再读取纹理");
			ModLoader.TexCompatibilityMode = base.Config.Bind<bool>("兼容性设置", "TexCompatibilityMode", false, "开启后纹理占用内存会增加，请仅在缺图时开启");
			if (AccessTools.TypeByName("EncounterPopup") != null)
			{
				MainPatcher.DoPatch(ModLoader.HarmonyInstance);
				ModLoader.HasEncounterType = true;
			}
			foreach (Type type in AccessTools.AllTypes())
			{
				if (type.IsSubclassOf(typeof(ScriptableObject)))
				{
					bool flag = false;
					using (IEnumerator<CustomAttributeData> enumerator2 = type.CustomAttributes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.AttributeType == typeof(SerializableAttribute))
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						ModLoader.AllScriptableObjectWithoutGuidTypeDict[type] = new Dictionary<string, ScriptableObject>();
					}
				}
			}
			ModLoader.PluginVersion = Version.Parse(base.Info.Metadata.Version.ToString());
			try
			{
				HarmonyMethod prefix = new HarmonyMethod(typeof(ModLoader), "UniqueIDScriptableClearDictPrefix", null);
				ModLoader.HarmonyInstance.Patch(AccessTools.Method(typeof(UniqueIDScriptable), "ClearDict", null, null), prefix, null, null, null, null);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarningFormat("{0} {1}", new object[]
				{
					"UniqueIDScriptableClearDictPrefix",
					ex
				});
			}
			try
			{
				HarmonyMethod postfix = new HarmonyMethod(typeof(ModLoader), "LocalizationManagerLoadLanguagePostfix", null);
				ModLoader.HarmonyInstance.Patch(AccessTools.Method(typeof(LocalizationManager), "LoadLanguage", null, null), null, postfix, null, null, null);
			}
			catch (Exception ex2)
			{
				UnityEngine.Debug.LogWarningFormat("{0} {1}", new object[]
				{
					"LocalizationManagerLoadLanguagePostfix",
					ex2
				});
			}
			try
			{
				HarmonyMethod prefix2 = new HarmonyMethod(typeof(ModLoader), "GuideManagerStartPrefix", null);
				ModLoader.HarmonyInstance.Patch(AccessTools.Method(typeof(GuideManager), "Start", null, null), prefix2, null, null, null, null);
			}
			catch (Exception ex3)
			{
				UnityEngine.Debug.LogWarningFormat("{0} {1}", new object[]
				{
					"GuideManagerStartPrefix",
					ex3
				});
			}
			try
			{
				HarmonyMethod postfix2 = new HarmonyMethod(typeof(ModLoader), "GraphicsManagerInitPostfix", null);
				ModLoader.HarmonyInstance.Patch(AccessTools.Method(typeof(GraphicsManager), "Init", null, null), null, postfix2, null, null, null);
			}
			catch (Exception ex4)
			{
				UnityEngine.Debug.LogWarningFormat("{0} {1}", new object[]
				{
					"GraphicsManagerInitPostfix",
					ex4
				});
			}
			try
			{
				HarmonyMethod prefix3 = new HarmonyMethod(typeof(ModLoader), "FixFXMaskAwake", null);
				ModLoader.HarmonyInstance.Patch(AccessTools.Method(typeof(FXMask), "Awake", null, null), prefix3, null, null, null, null);
			}
			catch (Exception ex5)
			{
				base.Logger.LogWarning(ex5);
			}
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.GetName().Name == "Assembly-CSharp")
				{
					ModLoader.GameSourceAssembly = assembly;
					break;
				}
			}
			base.Logger.LogInfo("Plugin ModLoader is loaded! ");
			LoadPreData.LoadData(Path.Combine(Paths.BepInExRootPath, "plugins"));
			base.Logger.LogInfo("ModLoader Resource Preload being");
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004A74 File Offset: 0x00002C74
		public static string CombinePaths(params string[] paths)
		{
			if (paths == null)
			{
				throw new ArgumentNullException("paths");
			}
			return paths.Aggregate(new Func<string, string, string>(Path.Combine));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004A98 File Offset: 0x00002C98
		public static bool IsSubDirectory(string dir, string parent_dir)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(parent_dir);
			DirectoryInfo directoryInfo2 = new DirectoryInfo(dir);
			bool result = false;
			while (directoryInfo2.Parent != null)
			{
				if (directoryInfo2.Parent.FullName == directoryInfo.FullName)
				{
					result = true;
					break;
				}
				directoryInfo2 = directoryInfo2.Parent;
			}
			return result;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public static void LogErrorWithModInfo(string error_info)
		{
			UnityEngine.Debug.LogWarning(string.Format("{0}.{1} Error: {2}", ModLoader.ProcessingScriptableObjectPack.ModName, ModLoader.ProcessingScriptableObjectPack.obj.name, error_info));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004B10 File Offset: 0x00002D10
		private static void LoadGameResource()
		{
			try
			{
				foreach (Type type2 in from type in ModLoader.GameSourceAssembly.GetTypes()
				where type.IsSubclassOf(typeof(ScriptableObject))
				select type)
				{
					ModLoader.ScriptableObjectKeyType.Add(type2.Name, type2);
				}
			}
			catch
			{
			}
			foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof(ScriptableObject)))
			{
				if (!(@object.GetType().Assembly != ModLoader.GameSourceAssembly))
				{
					try
					{
						if (@object is UniqueIDScriptable)
						{
							if (!ModLoader.AllScriptableObjectDict.ContainsKey((@object as UniqueIDScriptable).UniqueID))
							{
								ModLoader.AllScriptableObjectDict.Add((@object as UniqueIDScriptable).UniqueID, @object as ScriptableObject);
							}
							else
							{
								UnityEngine.Debug.LogWarning("AllScriptableObjectDict Same Key was Add " + (@object as UniqueIDScriptable).name);
							}
						}
						else if (!ModLoader.AllScriptableObjectDict.ContainsKey(@object.name))
						{
							ModLoader.AllScriptableObjectDict.Add(@object.name, @object as ScriptableObject);
						}
						else
						{
							UnityEngine.Debug.LogWarning("AllScriptableObjectDict Same Key was Add " + (@object as UniqueIDScriptable).name);
						}
						if (!(@object is UniqueIDScriptable))
						{
							Dictionary<string, ScriptableObject> dictionary2;
							if (!ModLoader.AllScriptableObjectWithoutGuidTypeDict.ContainsKey(@object.GetType()))
							{
								ModLoader.AllScriptableObjectWithoutGuidTypeDict.Add(@object.GetType(), new Dictionary<string, ScriptableObject>());
								Dictionary<string, ScriptableObject> dictionary;
								if (ModLoader.AllScriptableObjectWithoutGuidTypeDict.TryGetValue(@object.GetType(), out dictionary))
								{
									dictionary.Add(@object.name, @object as ScriptableObject);
								}
							}
							else if (ModLoader.AllScriptableObjectWithoutGuidTypeDict.TryGetValue(@object.GetType(), out dictionary2))
							{
								dictionary2.Add(@object.name, @object as ScriptableObject);
							}
						}
						if (@object is UniqueIDScriptable)
						{
							Dictionary<string, UniqueIDScriptable> dictionary4;
							if (!ModLoader.AllGUIDTypeDict.ContainsKey(@object.GetType()))
							{
								ModLoader.AllGUIDTypeDict.Add(@object.GetType(), new Dictionary<string, UniqueIDScriptable>());
								Dictionary<string, UniqueIDScriptable> dictionary3;
								if (ModLoader.AllGUIDTypeDict.TryGetValue(@object.GetType(), out dictionary3))
								{
									dictionary3.Add(@object.name, @object as UniqueIDScriptable);
								}
							}
							else if (ModLoader.AllGUIDTypeDict.TryGetValue(@object.GetType(), out dictionary4))
							{
								dictionary4.Add(@object.name, @object as UniqueIDScriptable);
							}
							if (!ModLoader.AllGUIDDict.ContainsKey((@object as UniqueIDScriptable).UniqueID))
							{
								ModLoader.AllGUIDDict.Add((@object as UniqueIDScriptable).UniqueID, @object as UniqueIDScriptable);
							}
							else
							{
								UnityEngine.Debug.LogWarning("AllGUIDDict Same Key was Add " + (@object as UniqueIDScriptable).UniqueID);
							}
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogWarning("LoadGameResource Error " + ex.Message);
					}
				}
			}
			foreach (UnityEngine.Object object2 in Resources.FindObjectsOfTypeAll(typeof(Sprite)))
			{
				if (!ModLoader.SpriteDict.ContainsKey(object2.name))
				{
					ModLoader.SpriteDict.Add(object2.name, object2 as Sprite);
				}
				else
				{
					UnityEngine.Debug.Log("SpriteDict Same Key was Add " + object2.name);
				}
			}
			foreach (UnityEngine.Object object3 in Resources.FindObjectsOfTypeAll(typeof(AudioClip)))
			{
				if (!ModLoader.AudioClipDict.ContainsKey(object3.name))
				{
					ModLoader.AudioClipDict.Add(object3.name, object3 as AudioClip);
				}
				else
				{
					UnityEngine.Debug.Log("AudioClipDict Same Key was Add " + object3.name);
				}
			}
			foreach (UnityEngine.Object object4 in Resources.FindObjectsOfTypeAll(typeof(WeatherSpecialEffect)))
			{
				if (!ModLoader.WeatherSpecialEffectDict.ContainsKey(object4.name))
				{
					ModLoader.WeatherSpecialEffectDict.Add(object4.name, object4 as WeatherSpecialEffect);
				}
				else
				{
					UnityEngine.Debug.Log("WeatherSpecialEffectDict Same Key was Add " + object4.name);
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004F70 File Offset: 0x00003170
		private static void LoadModsFromZip()
		{
			try
			{
				foreach (string text in Directory.GetFiles(Path.Combine(Paths.BepInExRootPath, "plugins")))
				{
					if (text.EndsWith(".zip"))
					{
						ModInfo modInfo = new ModInfo();
						string text2 = Path.GetFileNameWithoutExtension(text);
						string str = Path.GetFileNameWithoutExtension(text);
						ICollection<ZipEntry> entries;
						try
						{
							ZipFile zipFile = ZipFile.Read(text);
							entries = zipFile.Entries;
							str = entries.ElementAt(0).FileName.Substring(0, entries.ElementAt(0).FileName.Length - 1);
							ZipEntry zipEntry = zipFile[str + "/ModInfo.json"];
							if (zipEntry == null)
							{
								goto IL_D8F;
							}
							MemoryStream memoryStream = new MemoryStream();
							zipEntry.Extract(memoryStream);
							memoryStream.Seek(0L, SeekOrigin.Begin);
							using (StreamReader streamReader = new StreamReader(memoryStream))
							{
								JsonUtility.FromJsonOverwrite(streamReader.ReadToEnd(), modInfo);
							}
							if (Utility.IsNullOrWhiteSpace(modInfo.ModEditorVersion))
							{
								goto IL_D8F;
							}
							if (!Utility.IsNullOrWhiteSpace(modInfo.Name))
							{
								text2 = modInfo.Name;
							}
							ModLoader.ModPacks[text2] = new ModPack(modInfo, text2, ModLoader.Instance.Config.Bind<bool>("是否加载某个模组", (text2 + "_" + modInfo.Name).EscapeStr(), true, "是否加载" + text2));
							if (!ModLoader.ModPacks[text2].EnableEntry.Value)
							{
								goto IL_D8F;
							}
							UnityEngine.Debug.Log("ModLoader Load EditorZipMod " + text2 + " " + modInfo.Version);
							Version version = Version.Parse(modInfo.ModLoaderVerison);
							if (ModLoader.PluginVersion.CompareTo(version) < 0)
							{
								UnityEngine.Debug.LogWarningFormat("ModLoader Version {0} is lower than {1} Request Version {2}", new object[]
								{
									ModLoader.PluginVersion,
									text2,
									version
								});
							}
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogWarning("LoadModsFromZip " + ex.Message);
							goto IL_D8F;
						}
						try
						{
							foreach (ZipEntry zipEntry2 in entries)
							{
								if (zipEntry2.FileName.StartsWith(str + "/Resource") && zipEntry2.FileName.EndsWith(".ab"))
								{
									MemoryStream memoryStream2 = new MemoryStream();
									zipEntry2.Extract(memoryStream2);
									memoryStream2.Seek(0L, SeekOrigin.Begin);
									foreach (UnityEngine.Object @object in AssetBundle.LoadFromStream(memoryStream2).LoadAllAssets())
									{
										Sprite sprite = @object as Sprite;
										if (sprite != null)
										{
											if (!ModLoader.SpriteDict.ContainsKey(sprite.name))
											{
												ModLoader.SpriteDict.Add(sprite.name, sprite);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} SpriteDict Same Key was Add {1}", new object[]
												{
													text2,
													sprite.name
												});
											}
										}
										AudioClip audioClip = @object as AudioClip;
										if (audioClip != null)
										{
											if (!ModLoader.AudioClipDict.ContainsKey(audioClip.name))
											{
												ModLoader.AudioClipDict.Add(audioClip.name, audioClip);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
												{
													text2,
													audioClip.name
												});
											}
										}
									}
								}
							}
						}
						catch (Exception ex2)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load Resource Error {1}", new object[]
							{
								text2,
								ex2.Message
							});
						}
						try
						{
							foreach (ZipEntry zipEntry3 in entries)
							{
								if (zipEntry3.FileName.StartsWith(str + "/Resource/Picture") && (zipEntry3.FileName.EndsWith(".jpg") || zipEntry3.FileName.EndsWith(".jpeg") || zipEntry3.FileName.EndsWith(".png")))
								{
									string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(zipEntry3.FileName);
									Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, 0, false);
									MemoryStream memoryStream3 = new MemoryStream();
									zipEntry3.Extract(memoryStream3);
									texture2D.LoadImage(memoryStream3.ToArray());
									if (!ModLoader.TexCompatibilityMode.Value)
									{
										texture2D.ToCompress();
									}
									Sprite sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
									sprite2.name = fileNameWithoutExtension;
									if (!ModLoader.SpriteDict.ContainsKey(fileNameWithoutExtension))
									{
										ModLoader.SpriteDict.Add(fileNameWithoutExtension, sprite2);
									}
									else
									{
										UnityEngine.Debug.LogWarningFormat("{0} SpriteDict Same Key was Add {1}", new object[]
										{
											text2,
											fileNameWithoutExtension
										});
									}
								}
							}
						}
						catch (Exception ex3)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load Resource Custom Pictures Error {1}", new object[]
							{
								text2,
								ex3.Message
							});
						}
						try
						{
							foreach (ZipEntry zipEntry4 in entries)
							{
								if (zipEntry4.FileName.StartsWith(str + "/Resource/Audio"))
								{
									if (zipEntry4.FileName.EndsWith(".wav", true, null))
									{
										string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(zipEntry4.FileName);
										AudioClip audioClipFromWav = ResourceDataLoader.GetAudioClipFromWav(zipEntry4.OpenReader(), fileNameWithoutExtension2);
										if (audioClipFromWav)
										{
											if (!ModLoader.AudioClipDict.ContainsKey(audioClipFromWav.name))
											{
												ModLoader.AudioClipDict.Add(audioClipFromWav.name, audioClipFromWav);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
												{
													text2,
													audioClipFromWav.name
												});
											}
										}
									}
									else if (zipEntry4.FileName.EndsWith(".mp3", true, null))
									{
										string fileNameWithoutExtension3 = Path.GetFileNameWithoutExtension(zipEntry4.FileName);
										AudioClip audioClipFromMp = ResourceDataLoader.GetAudioClipFromMp3(zipEntry4.OpenReader(), fileNameWithoutExtension3);
										if (audioClipFromMp)
										{
											if (!ModLoader.AudioClipDict.ContainsKey(audioClipFromMp.name))
											{
												ModLoader.AudioClipDict.Add(audioClipFromMp.name, audioClipFromMp);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
												{
													text2,
													audioClipFromMp.name
												});
											}
										}
									}
									else if (zipEntry4.FileName.EndsWith(".ogg", true, null))
									{
										string fileNameWithoutExtension4 = Path.GetFileNameWithoutExtension(zipEntry4.FileName);
										AudioClip audioClipFromOgg = ResourceDataLoader.GetAudioClipFromOgg(zipEntry4.OpenReader(), fileNameWithoutExtension4);
										if (audioClipFromOgg)
										{
											if (!ModLoader.AudioClipDict.ContainsKey(audioClipFromOgg.name))
											{
												ModLoader.AudioClipDict.Add(audioClipFromOgg.name, audioClipFromOgg);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
												{
													text2,
													audioClipFromOgg.name
												});
											}
										}
									}
								}
							}
						}
						catch (Exception ex4)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load Resource Custom Audio Error {1}", new object[]
							{
								text2,
								ex4.Message
							});
						}
						try
						{
							foreach (Type type3 in from type in AccessTools.AllTypes()
							where type.IsSubclassOf(typeof(ScriptableObject))
							select type)
							{
								if (!ModLoader.AllScriptableObjectWithoutGuidTypeDict.ContainsKey(type3))
								{
									ModLoader.AllScriptableObjectWithoutGuidTypeDict[type3] = new Dictionary<string, ScriptableObject>();
								}
								if (!type3.IsSubclassOf(typeof(UniqueIDScriptable)) && !(type3 == typeof(UniqueIDScriptable)))
								{
									foreach (ZipEntry zipEntry5 in entries)
									{
										if (zipEntry5.FileName.StartsWith(str + "/ScriptableObject/" + type3.Name) && zipEntry5.FileName.EndsWith(".json"))
										{
											string fileNameWithoutExtension5 = Path.GetFileNameWithoutExtension(zipEntry5.FileName);
											Dictionary<string, ScriptableObject> dictionary;
											if (ModLoader.AllScriptableObjectWithoutGuidTypeDict.TryGetValue(type3, out dictionary) && !dictionary.ContainsKey(fileNameWithoutExtension5))
											{
												ScriptableObject scriptableObject = ScriptableObject.CreateInstance(type3);
												MemoryStream memoryStream4 = new MemoryStream();
												zipEntry5.Extract(memoryStream4);
												memoryStream4.Seek(0L, SeekOrigin.Begin);
												string text3;
												using (StreamReader streamReader2 = new StreamReader(memoryStream4))
												{
													text3 = streamReader2.ReadToEnd();
												}
												scriptableObject.name = fileNameWithoutExtension5;
												JsonUtility.FromJsonOverwrite(text3, scriptableObject);
												dictionary.Add(fileNameWithoutExtension5, scriptableObject);
												ModLoader.WaitForWarpperEditorNoGuidList.Add(new ModLoader.ScriptableObjectPack(scriptableObject, "", "", text2, text3));
												if (!ModLoader.AllScriptableObjectDict.ContainsKey(fileNameWithoutExtension5))
												{
													ModLoader.AllScriptableObjectDict.Add(fileNameWithoutExtension5, scriptableObject);
												}
											}
										}
									}
								}
							}
						}
						catch (Exception ex5)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load ScriptableObject Error {1}", new object[]
							{
								text2,
								ex5.Message
							});
						}
						try
						{
							foreach (ZipEntry zipEntry6 in entries)
							{
								if (zipEntry6.FileName.StartsWith(str + "/Localization") && zipEntry6.FileName.EndsWith(".csv"))
								{
									MemoryStream memoryStream5 = new MemoryStream();
									zipEntry6.Extract(memoryStream5);
									memoryStream5.Seek(0L, SeekOrigin.Begin);
									using (StreamReader streamReader3 = new StreamReader(memoryStream5))
									{
										ModLoader.WaitForLoadCSVList.Add(new Tuple<string, string>(Path.GetFileName(zipEntry6.FileName), streamReader3.ReadToEnd()));
									}
								}
							}
						}
						catch (Exception ex6)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load Localization Error {1}", new object[]
							{
								text2,
								ex6.Message
							});
						}
						try
						{
							foreach (Type type2 in from type in ModLoader.GameSourceAssembly.GetTypes()
							where type.IsSubclassOf(typeof(UniqueIDScriptable))
							select type)
							{
								foreach (ZipEntry zipEntry7 in entries)
								{
									if (zipEntry7.FileName.StartsWith(str + "/" + type2.Name) && zipEntry7.FileName.EndsWith(".json"))
									{
										string fileNameWithoutExtension6 = Path.GetFileNameWithoutExtension(zipEntry7.FileName);
										try
										{
											MemoryStream memoryStream6 = new MemoryStream();
											zipEntry7.Extract(memoryStream6);
											memoryStream6.Seek(0L, SeekOrigin.Begin);
											string text4;
											JsonData jsonData;
											using (StreamReader streamReader4 = new StreamReader(memoryStream6))
											{
												text4 = streamReader4.ReadToEnd();
												jsonData = JsonMapper.ToObject(text4);
											}
											if (!jsonData.ContainsKey("UniqueID") || !jsonData["UniqueID"].IsString || Utility.IsNullOrWhiteSpace(jsonData["UniqueID"].ToString()))
											{
												UnityEngine.Debug.LogErrorFormat("{0} EditorLoadZip {1} {2} try to load a UniqueIDScriptable without GUID", new object[]
												{
													type2.Name,
													text2,
													fileNameWithoutExtension6
												});
											}
											else
											{
												UniqueIDScriptable uniqueIDScriptable = ScriptableObject.CreateInstance(type2) as UniqueIDScriptable;
												JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(uniqueIDScriptable), uniqueIDScriptable);
												JsonUtility.FromJsonOverwrite(text4, uniqueIDScriptable);
												uniqueIDScriptable.name = text2 + "_" + fileNameWithoutExtension6;
												string uniqueID = uniqueIDScriptable.UniqueID;
												ModLoader.AllGUIDDict.Add(uniqueID, uniqueIDScriptable);
												GameLoad.Instance.DataBase.AllData.Add(uniqueIDScriptable);
												if (!ModLoader.WaitForWarpperEditorGuidDict.ContainsKey(uniqueID))
												{
													ModLoader.WaitForWarpperEditorGuidDict.Add(uniqueID, new ModLoader.ScriptableObjectPack(uniqueIDScriptable, "", "", text2, text4));
												}
												else
												{
													UnityEngine.Debug.LogWarningFormat("{0} WaitForWarpperEditorGuidDict Same Key was Add {1}", new object[]
													{
														text2,
														uniqueID
													});
												}
												if (!ModLoader.AllScriptableObjectDict.ContainsKey(uniqueID))
												{
													ModLoader.AllScriptableObjectDict.Add(uniqueID, uniqueIDScriptable);
												}
												Dictionary<string, UniqueIDScriptable> dictionary2;
												if (ModLoader.AllGUIDTypeDict.TryGetValue(type2, out dictionary2) && !dictionary2.ContainsKey(uniqueID))
												{
													dictionary2.Add(uniqueID, uniqueIDScriptable);
												}
											}
										}
										catch (Exception ex7)
										{
											UnityEngine.Debug.LogWarningFormat("{0} EditorLoadZip {1} {2} Error {3}", new object[]
											{
												type2.Name,
												text2,
												fileNameWithoutExtension6,
												ex7.Message
											});
										}
									}
								}
							}
						}
						catch (Exception ex8)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load UniqueIDScriptable Error {1}", new object[]
							{
								text2,
								ex8.Message
							});
						}
						try
						{
							foreach (ZipEntry zipEntry8 in entries)
							{
								if (zipEntry8.FileName.StartsWith(str + "/GameSourceModify") && zipEntry8.FileName.EndsWith(".json"))
								{
									string fileNameWithoutExtension7 = Path.GetFileNameWithoutExtension(zipEntry8.FileName);
									MemoryStream memoryStream7 = new MemoryStream();
									zipEntry8.Extract(memoryStream7);
									memoryStream7.Seek(0L, SeekOrigin.Begin);
									string cardData;
									using (StreamReader streamReader5 = new StreamReader(memoryStream7))
									{
										cardData = streamReader5.ReadToEnd();
									}
									UniqueIDScriptable obj;
									ModLoader.WaitForWarpperEditorGameSourceGUIDList.Add(ModLoader.AllGUIDDict.TryGetValue(fileNameWithoutExtension7, out obj) ? new ModLoader.ScriptableObjectPack(obj, "", "", text2, cardData) : new ModLoader.ScriptableObjectPack(null, fileNameWithoutExtension7, "", text2, cardData));
								}
							}
						}
						catch (Exception ex9)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load GameSourceModify Error {1}", new object[]
							{
								text2,
								ex9.Message
							});
						}
					}
					IL_D8F:;
				}
			}
			catch (Exception ex10)
			{
				UnityEngine.Debug.LogWarning(ex10.Message);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005F6C File Offset: 0x0000416C
		private static void LoadMods(string mods_dir)
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
							if (!ModLoader.ModPacks.ContainsKey(text2))
							{
								ModLoader.ModPacks[text2] = new ModPack(modInfo, text2, ModLoader.Instance.Config.Bind<bool>("是否加载某个模组", (text2 + "_" + modInfo.Name).EscapeStr(), true, "是否加载" + text2));
							}
							if (!ModLoader.ModPacks[text2].EnableEntry.Value)
							{
								goto IL_91C;
							}
							UnityEngine.Debug.Log("ModLoader Load Mod " + text2 + " " + modInfo.Version);
							Version version = Version.Parse(modInfo.ModLoaderVerison);
							if (ModLoader.PluginVersion.CompareTo(version) < 0)
							{
								UnityEngine.Debug.LogWarningFormat("ModLoader Version {0} is lower than {1} Request Version {2}", new object[]
								{
									ModLoader.PluginVersion,
									text2,
									version
								});
							}
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Check Version Error {1}", new object[]
							{
								text2,
								ex.Message
							});
						}
						try
						{
							if (Directory.Exists(ModLoader.CombinePaths(new string[]
							{
								text,
								"Resource"
							})))
							{
								foreach (string text3 in Directory.GetFiles(ModLoader.CombinePaths(new string[]
								{
									text,
									"Resource"
								})))
								{
									if (text3.EndsWith(".ab"))
									{
										foreach (UnityEngine.Object @object in AssetBundle.LoadFromFile(text3).LoadAllAssets())
										{
											Sprite sprite = @object as Sprite;
											if (sprite != null)
											{
												if (!ModLoader.SpriteDict.ContainsKey(sprite.name))
												{
													ModLoader.SpriteDict.Add(sprite.name, sprite);
												}
												else
												{
													UnityEngine.Debug.LogWarningFormat("{0} SpriteDict Same Key was Add {1}", new object[]
													{
														text2,
														sprite.name
													});
												}
											}
											AudioClip audioClip = @object as AudioClip;
											if (audioClip != null)
											{
												if (!ModLoader.AudioClipDict.ContainsKey(audioClip.name))
												{
													ModLoader.AudioClipDict.Add(audioClip.name, audioClip);
												}
												else
												{
													UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
													{
														text2,
														audioClip.name
													});
												}
											}
										}
									}
								}
							}
						}
						catch (Exception ex2)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load Resource Error {1}", new object[]
							{
								text2,
								ex2.Message
							});
						}
						try
						{
							if (Directory.Exists(ModLoader.CombinePaths(new string[]
							{
								text,
								"Resource",
								"Audio"
							})))
							{
								foreach (string text4 in Directory.GetFiles(ModLoader.CombinePaths(new string[]
								{
									text,
									"Resource",
									"Audio"
								})))
								{
									if (text4.EndsWith(".wav", true, null))
									{
										FileStream raw_data = File.Open(text4, FileMode.Open);
										string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text4);
										AudioClip audioClipFromWav = ResourceDataLoader.GetAudioClipFromWav(raw_data, fileNameWithoutExtension);
										if (audioClipFromWav)
										{
											if (!ModLoader.AudioClipDict.ContainsKey(audioClipFromWav.name))
											{
												ModLoader.AudioClipDict.Add(audioClipFromWav.name, audioClipFromWav);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
												{
													text2,
													audioClipFromWav.name
												});
											}
										}
									}
									else if (text4.EndsWith(".mp3", true, null))
									{
										FileStream raw_data2 = File.Open(text4, FileMode.Open);
										string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(text4);
										AudioClip audioClipFromMp = ResourceDataLoader.GetAudioClipFromMp3(raw_data2, fileNameWithoutExtension2);
										if (audioClipFromMp)
										{
											if (!ModLoader.AudioClipDict.ContainsKey(audioClipFromMp.name))
											{
												ModLoader.AudioClipDict.Add(audioClipFromMp.name, audioClipFromMp);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
												{
													text2,
													audioClipFromMp.name
												});
											}
										}
									}
									else if (text4.EndsWith(".ogg", true, null))
									{
										FileStream raw_data3 = File.Open(text4, FileMode.Open);
										string fileNameWithoutExtension3 = Path.GetFileNameWithoutExtension(text4);
										AudioClip audioClipFromOgg = ResourceDataLoader.GetAudioClipFromOgg(raw_data3, fileNameWithoutExtension3);
										if (audioClipFromOgg)
										{
											if (!ModLoader.AudioClipDict.ContainsKey(audioClipFromOgg.name))
											{
												ModLoader.AudioClipDict.Add(audioClipFromOgg.name, audioClipFromOgg);
											}
											else
											{
												UnityEngine.Debug.LogWarningFormat("{0} AudioClipDict Same Key was Add {1}", new object[]
												{
													text2,
													audioClipFromOgg.name
												});
											}
										}
									}
								}
							}
						}
						catch (Exception ex3)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load Resource Custom Audio Error {1}", new object[]
							{
								text2,
								ex3.Message
							});
						}
						try
						{
							if (Directory.Exists(ModLoader.CombinePaths(new string[]
							{
								text,
								"ScriptableObject"
							})))
							{
								foreach (Type type2 in from type in AccessTools.AllTypes()
								where type.IsSubclassOf(typeof(ScriptableObject))
								select type)
								{
									if (!ModLoader.AllScriptableObjectWithoutGuidTypeDict.ContainsKey(type2))
									{
										ModLoader.AllScriptableObjectWithoutGuidTypeDict[type2] = new Dictionary<string, ScriptableObject>();
									}
									if (!type2.IsSubclassOf(typeof(UniqueIDScriptable)) && !(type2 == typeof(UniqueIDScriptable)) && Directory.Exists(ModLoader.CombinePaths(new string[]
									{
										text,
										"ScriptableObject",
										type2.Name
									})))
									{
										foreach (string path in Directory.EnumerateFiles(ModLoader.CombinePaths(new string[]
										{
											text,
											"ScriptableObject",
											type2.Name
										}), "*.json", SearchOption.AllDirectories))
										{
											string fileNameWithoutExtension4 = Path.GetFileNameWithoutExtension(path);
											Dictionary<string, ScriptableObject> dictionary;
											if (ModLoader.AllScriptableObjectWithoutGuidTypeDict.TryGetValue(type2, out dictionary) && !dictionary.ContainsKey(fileNameWithoutExtension4))
											{
												ScriptableObject scriptableObject = ScriptableObject.CreateInstance(type2);
												string text5;
												using (StreamReader streamReader2 = new StreamReader(path))
												{
													text5 = streamReader2.ReadToEnd();
												}
												scriptableObject.name = fileNameWithoutExtension4;
												JsonUtility.FromJsonOverwrite(text5, scriptableObject);
												dictionary.Add(fileNameWithoutExtension4, scriptableObject);
												ModLoader.WaitForWarpperEditorNoGuidList.Add(new ModLoader.ScriptableObjectPack(scriptableObject, "", "", text2, text5));
												if (!ModLoader.AllScriptableObjectDict.ContainsKey(fileNameWithoutExtension4))
												{
													ModLoader.AllScriptableObjectDict.Add(fileNameWithoutExtension4, scriptableObject);
												}
											}
										}
									}
								}
							}
						}
						catch (Exception ex4)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load ScriptableObject Error {1}", new object[]
							{
								text2,
								ex4.Message
							});
						}
						try
						{
							if (Directory.Exists(ModLoader.CombinePaths(new string[]
							{
								text,
								"Localization"
							})))
							{
								foreach (string text6 in Directory.GetFiles(ModLoader.CombinePaths(new string[]
								{
									text,
									"Localization"
								})))
								{
									if (text6.EndsWith(".csv"))
									{
										using (StreamReader streamReader3 = new StreamReader(text6))
										{
											ModLoader.WaitForLoadCSVList.Add(new Tuple<string, string>(Path.GetFileName(text6), streamReader3.ReadToEnd()));
										}
									}
								}
							}
						}
						catch (Exception ex5)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load Localization Error {1}", new object[]
							{
								text2,
								ex5.Message
							});
						}
						try
						{
							if (Utility.IsNullOrWhiteSpace(modInfo.ModEditorVersion))
							{
								UnityEngine.Debug.LogWarningFormat("{0} Only Support Editor Mod", new object[]
								{
									text2
								});
							}
							else if (Directory.Exists(ModLoader.CombinePaths(new string[]
							{
								text,
								"GameSourceModify"
							})))
							{
								foreach (string text7 in Directory.EnumerateFiles(ModLoader.CombinePaths(new string[]
								{
									text,
									"GameSourceModify"
								}), "*.json", SearchOption.AllDirectories))
								{
									string path2;
									string fileNameWithoutExtension5 = Path.GetFileNameWithoutExtension(path2 = text7);
									string cardData;
									using (StreamReader streamReader4 = new StreamReader(path2))
									{
										cardData = streamReader4.ReadToEnd();
									}
									UniqueIDScriptable obj;
									if (ModLoader.AllGUIDDict.TryGetValue(fileNameWithoutExtension5, out obj))
									{
										ModLoader.WaitForWarpperEditorGameSourceGUIDList.Add(new ModLoader.ScriptableObjectPack(obj, "", "", text2, cardData));
									}
									else
									{
										ModLoader.WaitForWarpperEditorGameSourceGUIDList.Add(new ModLoader.ScriptableObjectPack(null, fileNameWithoutExtension5, "", text2, cardData));
									}
								}
							}
						}
						catch (Exception ex6)
						{
							UnityEngine.Debug.LogWarningFormat("{0} Load GameSourceModify Error {1}", new object[]
							{
								text2,
								ex6.Message
							});
						}
					}
					IL_91C:;
				}
			}
			catch (Exception ex7)
			{
				UnityEngine.Debug.LogWarning(ex7.Message);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00006A04 File Offset: 0x00004C04
		private static void LoadEditorScriptableObject()
		{
			foreach (ModLoader.ScriptableObjectPack scriptableObjectPack in ModLoader.WaitForWarpperEditorNoGuidList)
			{
				try
				{
					ModLoader.ProcessingScriptableObjectPack = scriptableObjectPack;
					JsonData json = JsonMapper.ToObject(scriptableObjectPack.CardData);
					WarpperFunction.JsonCommonWarpper(scriptableObjectPack.obj, json);
					if (scriptableObjectPack.obj is CardTabGroup && scriptableObjectPack.obj.name.StartsWith("Tab_"))
					{
						ModLoader.WaitForAddCardTabGroup.Add(new ModLoader.ScriptableObjectPack(scriptableObjectPack.obj, "", "", "", scriptableObjectPack.CardData));
					}
					if (scriptableObjectPack.obj is ContentPage)
					{
						if (scriptableObjectPack.obj.name.EndsWith("Default"))
						{
							ModLoader.WaitForAddDefaultContentPage.Add(new ModLoader.ScriptableObjectPack(scriptableObjectPack.obj, "", "", "", scriptableObjectPack.CardData));
						}
						else if (scriptableObjectPack.obj.name.EndsWith("Main"))
						{
							ModLoader.WaitForAddMainContentPage.Add(new ModLoader.ScriptableObjectPack(scriptableObjectPack.obj, "", "", "", scriptableObjectPack.CardData));
						}
					}
					GuideEntry guideEntry = scriptableObjectPack.obj as GuideEntry;
					if (guideEntry != null)
					{
						ModLoader.WaitForAddGuideEntry.Add(guideEntry);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("LoadEditorScriptableObject " + ex.Message);
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00006BB0 File Offset: 0x00004DB0
		private unsafe static void LoadLocalization()
		{
			Regex regex = new Regex("\\\\n");
			if (MBSingleton<LocalizationManager>.Instance.Languages[LocalizationManager.CurrentLanguage].LanguageName == "简体中文")
			{
				foreach (Tuple<string, string> tuple in ModLoader.WaitForLoadCSVList)
				{
					try
					{
						if (tuple.Item1.Contains("SimpCn"))
						{
							Dictionary<string, string> dictionary = *ModLoader.CurrentTextsFieldRef();
							foreach (KeyValuePair<string, List<string>> keyValuePair in CSVParser.LoadFromString(tuple.Item2, Delimiter.Comma))
							{
								if (!dictionary.ContainsKey(keyValuePair.Key) && keyValuePair.Value.Count >= 2)
								{
									string text = regex.Replace(keyValuePair.Value[1], "\n");
									if (!Utility.IsNullOrWhiteSpace(text.Trim()))
									{
										dictionary.Add(keyValuePair.Key, text);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogWarning("LoadLocalization " + ex.Message);
					}
				}
			}
			if (MBSingleton<LocalizationManager>.Instance.Languages[LocalizationManager.CurrentLanguage].LanguageName == "English")
			{
				foreach (Tuple<string, string> tuple2 in ModLoader.WaitForLoadCSVList)
				{
					try
					{
						if (tuple2.Item1.Contains("SimpEn"))
						{
							Dictionary<string, string> dictionary2 = *ModLoader.CurrentTextsFieldRef();
							foreach (KeyValuePair<string, List<string>> keyValuePair2 in CSVParser.LoadFromString(tuple2.Item2, Delimiter.Comma))
							{
								if (!dictionary2.ContainsKey(keyValuePair2.Key) && keyValuePair2.Value.Count >= 2)
								{
									string text2 = regex.Replace(keyValuePair2.Value[0], "\n");
									if (!Utility.IsNullOrWhiteSpace(text2.Trim()))
									{
										dictionary2.Add(keyValuePair2.Key, text2);
									}
								}
							}
						}
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogWarning("LoadLocalization " + ex2.Message);
					}
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00006E64 File Offset: 0x00005064
		private static void AddBlueprintCardData(GraphicsManager instance)
		{
			foreach (Tuple<string, string, CardData> tuple in ModLoader.WaitForAddBlueprintCard)
			{
				try
				{
					foreach (CardTabGroup cardTabGroup in instance.BlueprintModelsPopup.BlueprintTabs)
					{
						if (cardTabGroup.name == tuple.Item1)
						{
							cardTabGroup.ShopSortingList.Add(tuple.Item3);
							using (List<CardTabGroup>.Enumerator enumerator2 = cardTabGroup.SubGroups.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									CardTabGroup cardTabGroup2 = enumerator2.Current;
									if (cardTabGroup2.name == tuple.Item2)
									{
										cardTabGroup2.IncludedCards.Add(tuple.Item3);
										break;
									}
								}
								break;
							}
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("AddBlueprintCardData " + ex.Message);
				}
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00006F90 File Offset: 0x00005190
		private static void AddVisibleGameStat(GraphicsManager instance)
		{
			foreach (Tuple<string, GameStat> tuple in ModLoader.WaitForAddVisibleGameStat)
			{
				try
				{
					foreach (StatListTab statListTab in Traverse.Create(instance.AllStatsList).Field<StatListTab[]>("Tabs").Value)
					{
						if (statListTab.name == tuple.Item1)
						{
							statListTab.ContainedStats.Add(tuple.Item2);
							break;
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("AddVisibleGameStat " + ex.Message);
				}
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000705C File Offset: 0x0000525C
		private static void AddPerkGroup()
		{
			foreach (Tuple<string, CharacterPerk> tuple in ModLoader.WaitForAddPerkGroup)
			{
				try
				{
					Dictionary<string, UniqueIDScriptable> dictionary;
					UniqueIDScriptable uniqueIDScriptable;
					if (ModLoader.AllGUIDTypeDict.TryGetValue(typeof(PerkGroup), out dictionary) && dictionary.TryGetValue(tuple.Item1, out uniqueIDScriptable))
					{
						PerkGroup perkGroup = uniqueIDScriptable as PerkGroup;
						if (perkGroup)
						{
							Array.Resize<CharacterPerk>(ref perkGroup.PerksList, perkGroup.PerksList.Length + 1);
							perkGroup.PerksList[perkGroup.PerksList.Length - 1] = tuple.Item2;
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("AddPerkGroup " + ex.Message);
				}
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00007140 File Offset: 0x00005340
		private static void AddCardTabGroup(GraphicsManager instance)
		{
			foreach (ModLoader.ScriptableObjectPack scriptableObjectPack in ModLoader.WaitForAddCardTabGroup)
			{
				try
				{
					CardTabGroup cardTabGroup = scriptableObjectPack.obj as CardTabGroup;
					if (cardTabGroup != null)
					{
						cardTabGroup.FillSortingList();
						if (cardTabGroup.name.StartsWith("Tab_"))
						{
							if (cardTabGroup.SubGroups.Count != 0)
							{
								Array.Resize<CardTabGroup>(ref instance.BlueprintModelsPopup.BlueprintTabs, instance.BlueprintModelsPopup.BlueprintTabs.Length + 1);
								instance.BlueprintModelsPopup.BlueprintTabs[instance.BlueprintModelsPopup.BlueprintTabs.Length - 1] = cardTabGroup;
							}
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("AddCardTabGroup " + ex.Message);
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000722C File Offset: 0x0000542C
		private static void AddCardTabGroupOnce(GraphicsManager instance)
		{
			foreach (ModLoader.ScriptableObjectPack scriptableObjectPack in ModLoader.WaitForAddCardTabGroup)
			{
				try
				{
					CardTabGroup cardTabGroup = scriptableObjectPack.obj as CardTabGroup;
					if (cardTabGroup != null)
					{
						cardTabGroup.FillSortingList();
						if (cardTabGroup.name.StartsWith("Tab_"))
						{
							if (cardTabGroup.SubGroups.Count == 0)
							{
								JsonData jsonData = JsonMapper.ToObject(scriptableObjectPack.CardData);
								if (jsonData.ContainsKey("BlueprintCardDataCardTabGroup") && jsonData["BlueprintCardDataCardTabGroup"].IsString && !Utility.IsNullOrWhiteSpace(jsonData["BlueprintCardDataCardTabGroup"].ToString()))
								{
									foreach (CardTabGroup cardTabGroup2 in instance.BlueprintModelsPopup.BlueprintTabs)
									{
										if (cardTabGroup2.name == jsonData["BlueprintCardDataCardTabGroup"].ToString())
										{
											cardTabGroup2.SubGroups.Add(cardTabGroup);
											cardTabGroup2.FillSortingList();
											break;
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("AddCustomCardTabGroup " + ex.Message);
				}
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000073A0 File Offset: 0x000055A0
		private static void AddCardFilterGroupOnce()
		{
			Dictionary<string, CardFilterGroup> dictionary = new Dictionary<string, CardFilterGroup>();
			foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof(CardFilterGroup)))
			{
				if (!(@object.GetType().Assembly != ModLoader.GameSourceAssembly))
				{
					dictionary.Add(@object.name, @object as CardFilterGroup);
				}
			}
			foreach (Tuple<string, CardData> tuple in ModLoader.WaitForAddCardFilterGroupCard)
			{
				CardFilterGroup cardFilterGroup;
				if (dictionary.TryGetValue(tuple.Item1, out cardFilterGroup))
				{
					cardFilterGroup.IncludedCards.Add(tuple.Item2);
				}
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00007464 File Offset: 0x00005664
		private static IEnumerator FXMaskPostAwake(FXMask instance)
		{
			while (!MBSingleton<AmbienceImageEffect>.Instance)
			{
				yield return null;
			}
			Traverse traverse = Traverse.Create(instance);
			traverse.Field<RectTransform>("MyRectTr").Value = instance.GetComponent<RectTransform>();
			traverse.Field<AmbienceImageEffect>("AmbienceEffects").Value = MBSingleton<AmbienceImageEffect>.Instance;
			Traverse<SpriteMask> traverse2 = traverse.Field<SpriteMask>("MaskObject");
			traverse2.Value = UnityEngine.Object.Instantiate<SpriteMask>(MBSingleton<AmbienceImageEffect>.Instance.MaskPrefab, MBSingleton<AmbienceImageEffect>.Instance.WeatherEffectsParent);
			if (GameManager.DontRenameGOs)
			{
				yield break;
			}
			traverse2.Value.name = instance.name + "_Mask";
			yield break;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00007473 File Offset: 0x00005673
		private static bool FixFXMaskAwake(FXMask __instance)
		{
			if (!MBSingleton<AmbienceImageEffect>.Instance)
			{
				__instance.StartCoroutine(ModLoader.FXMaskPostAwake(__instance));
				return false;
			}
			return true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00007491 File Offset: 0x00005691
		private static IEnumerator WaiterForContentDisplayer()
		{
			bool done = false;
			for (;;)
			{
				ContentDisplayer[] array = Resources.FindObjectsOfTypeAll<ContentDisplayer>();
				int i = 0;
				while (i < array.Length)
				{
					ContentDisplayer contentDisplayer = array[i];
					if (!(contentDisplayer.gameObject.name != "JournalTourist"))
					{
						ContentDisplayer contentDisplayer2 = null;
						GameObject gameObject = null;
						try
						{
							gameObject = UnityEngine.Object.Instantiate<GameObject>(contentDisplayer.gameObject);
							contentDisplayer2 = gameObject.GetComponent<ContentDisplayer>();
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogWarning("FXMask Warning " + ex.Message);
						}
						if (!(contentDisplayer2 == null))
						{
							gameObject.name = "JournalDefaultSample";
							gameObject.hideFlags = HideFlags.HideAndDontSave;
							ModLoader.CustomGameObjectListDict.Add(gameObject.name, gameObject);
							ModLoader.CustomContentDisplayerDict.Add(gameObject.name, contentDisplayer2);
							done = true;
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
				if (done)
				{
					break;
				}
				yield return new WaitForSeconds(0.5f);
			}
			foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof(ContentDisplayer)))
			{
				try
				{
					if (!ModLoader.CustomContentDisplayerDict.ContainsKey(@object.name))
					{
						ModLoader.CustomContentDisplayerDict.Add(@object.name, @object as ContentDisplayer);
					}
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogWarning("CustomContentDisplayerDict Warning " + ex2.Message);
				}
			}
			while (!ModLoader._onceWarp.Done())
			{
				yield return null;
			}
			foreach (ModLoader.ScriptableObjectPack scriptableObjectPack in ModLoader.WaitForAddDefaultContentPage)
			{
				try
				{
					if (!ModLoader.CustomGameObjectListDict.ContainsKey(scriptableObjectPack.obj.name))
					{
						GameObject original;
						if (ModLoader.CustomGameObjectListDict.TryGetValue("JournalDefaultSample", out original))
						{
							GameObject gameObject2 = null;
							ContentDisplayer contentDisplayer3 = null;
							try
							{
								gameObject2 = UnityEngine.Object.Instantiate<GameObject>(original);
								contentDisplayer3 = (gameObject2.GetComponent(typeof(ContentDisplayer)) as ContentDisplayer);
							}
							catch (Exception ex3)
							{
								UnityEngine.Debug.LogWarning("FXMask Warning " + ex3.Message);
							}
							if (!(contentDisplayer3 == null))
							{
								ContentPage contentPage = scriptableObjectPack.obj as ContentPage;
								if (!(contentPage == null))
								{
									Traverse traverse = Traverse.Create(contentDisplayer3);
									List<ContentPage> value = traverse.Field<List<ContentPage>>("ExplicitPageContent").Value;
									value.Clear();
									value.Add(contentPage);
									traverse.Field<ContentPage>("DefaultPage").Value = contentPage;
									string[] array3 = scriptableObjectPack.obj.name.Split(new char[]
									{
										'_'
									});
									if (array3.Length > 2)
									{
										gameObject2.name = array3[0] + "_" + array3[1];
										gameObject2.hideFlags = HideFlags.HideAndDontSave;
										ModLoader.CustomGameObjectListDict.Add(gameObject2.name, gameObject2);
										ModLoader.CustomContentDisplayerDict.Add(gameObject2.name, contentDisplayer3);
									}
								}
							}
						}
					}
				}
				catch (Exception ex4)
				{
					UnityEngine.Debug.LogWarning("WaiterForContentDisplayer WaitForAddDefaultContentPage " + ex4.Message);
				}
			}
			foreach (ModLoader.ScriptableObjectPack scriptableObjectPack2 in ModLoader.WaitForAddMainContentPage)
			{
				try
				{
					string[] array4 = scriptableObjectPack2.obj.name.Split(new char[]
					{
						'_'
					});
					ContentDisplayer obj;
					if (array4.Length > 2 && ModLoader.CustomContentDisplayerDict.TryGetValue(array4[0] + "_" + array4[1], out obj))
					{
						BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
						(typeof(ContentDisplayer).GetField("ExplicitPageContent", bindingAttr).GetValue(obj) as IList).Add(scriptableObjectPack2.obj);
					}
				}
				catch (Exception ex5)
				{
					UnityEngine.Debug.LogWarning("WaiterForContentDisplayer WaitForAddMainContentPage " + ex5.Message);
				}
			}
			using (List<ModLoader.ScriptableObjectPack>.Enumerator enumerator = ModLoader.WaitForAddJournalPlayerCharacter.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ModLoader.ScriptableObjectPack scriptableObjectPack3 = enumerator.Current;
					try
					{
						PlayerCharacter playerCharacter = scriptableObjectPack3.obj as PlayerCharacter;
						if (playerCharacter != null)
						{
							JsonData jsonData = JsonMapper.ToObject(scriptableObjectPack3.CardData);
							ContentDisplayer journal;
							if (jsonData.ContainsKey("PlayerCharacterJournalName") && jsonData["PlayerCharacterJournalName"].IsString && !Utility.IsNullOrWhiteSpace(jsonData["PlayerCharacterJournalName"].ToString()) && ModLoader.CustomContentDisplayerDict.TryGetValue(jsonData["PlayerCharacterJournalName"].ToString(), out journal))
							{
								playerCharacter.Journal = journal;
							}
						}
					}
					catch (Exception ex6)
					{
						UnityEngine.Debug.LogWarning("WaiterForContentDisplayer PlayerCharacterJournalName " + ex6.Message);
					}
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000749C File Offset: 0x0000569C
		private static void CustomGameObjectFixed()
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in ModLoader.CustomGameObjectListDict)
			{
				try
				{
					Transform transform = keyValuePair.Value.transform.Find("Shadow/GuideFrame/GuideContentPage/Content/Horizontal");
					if (transform != null)
					{
						for (int i = 0; i < transform.childCount; i++)
						{
							UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
						}
					}
					transform = keyValuePair.Value.transform.Find("Shadow/GuideFrame");
					FXMask fxmask = transform.gameObject.GetComponent(typeof(FXMask)) as FXMask;
					if (fxmask)
					{
						fxmask.enabled = true;
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("CustomGameObjectFixed " + ex.Message);
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000075A0 File Offset: 0x000057A0
		private static void AddPlayerCharacter(GuideManager instance)
		{
			try
			{
				instance.StartCoroutine(ModLoader.WaiterForContentDisplayer());
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("AddPlayerCharacter" + ex.Message);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000075E4 File Offset: 0x000057E4
		private static void LoadGuideEntry(GuideManager instance)
		{
			try
			{
				foreach (GuideEntry item in ModLoader.WaitForAddGuideEntry)
				{
					instance.AllEntries.Add(item);
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning("LoadGuideEntry" + ex.Message);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00007664 File Offset: 0x00005864
		public static void UniqueIDScriptableClearDictPrefix()
		{
			if (!ModLoader._once.DoOnce())
			{
				return;
			}
			try
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				ModLoader.LoadGameResource();
				ModLoader.LoadMods(Path.Combine(Paths.BepInExRootPath, "plugins"));
				ModLoader.LoadModsFromZip();
				PostSpriteLoad.BeginCompress = true;
				LoadPreData.LoadFromPreLoadData();
				ModLoader.LoadEditorScriptableObject();
				Stopwatch stopwatch2 = new Stopwatch();
				stopwatch2.Start();
				DoWarpperLoader.WarpperAllEditorMods();
				DoWarpperLoader.WarpperAllEditorGameSrouces();
				DoWarpperLoader.MatchAndWarpperAllEditorGameSrouce();
				stopwatch2.Stop();
				UnityEngine.Debug.LogWarning(string.Format("warp time taken:{0}", stopwatch2.Elapsed));
				ModLoader.AddPerkGroup();
				stopwatch.Stop();
				UnityEngine.Debug.Log("ModLoader Time taken: " + stopwatch.Elapsed.ToString());
				ModLoader._once.SetDone();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning(ex.Message);
			}
			finally
			{
				PostSpriteLoad.CanEnd = true;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00007760 File Offset: 0x00005960
		public static void LocalizationManagerLoadLanguagePostfix()
		{
			try
			{
				ModLoader.LoadLocalization();
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning(ex.Message);
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00007790 File Offset: 0x00005990
		public static void GuideManagerStartPrefix(GuideManager __instance)
		{
			try
			{
				ModLoader.LoadGuideEntry(__instance);
				ModLoader.AddPlayerCharacter(__instance);
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning(ex.Message);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000077C8 File Offset: 0x000059C8
		public static void GraphicsManagerInitPostfix(GraphicsManager __instance)
		{
			try
			{
				ModLoader.AddCardTabGroup(__instance);
				ModLoader.AddBlueprintCardData(__instance);
				ModLoader.AddVisibleGameStat(__instance);
				if (!ModLoader.init_flag)
				{
					ModLoader.AddCardTabGroupOnce(__instance);
					ModLoader.CustomGameObjectFixed();
					ModLoader.AddCardFilterGroupOnce();
					ModLoader.init_flag = true;
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogWarning(ex.Message);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00007824 File Offset: 0x00005A24
		private void Update()
		{
			if (ModLoader.ReqQuit)
			{
				if ((double)ModLoader.WaitTime > 0.5)
				{
					ModLoader.WaitTime -= Time.deltaTime;
				}
				else if (ModLoader.WaitTime > 0f)
				{
					if (!ModLoader.HadBootNew)
					{
						ModLoader.HadBootNew = true;
						Process.Start("explorer.exe", Paths.ExecutablePath);
					}
					ModLoader.WaitTime -= Time.deltaTime;
				}
				else
				{
					Application.Quit();
				}
			}
			if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightCommand)) && Input.GetKeyUp(KeyCode.Tab))
			{
				ModLoader.ModManagerUIOn = !ModLoader.ModManagerUIOn;
				ModLoader.MainUIBackPanelRT.gameObject.SetActive(ModLoader.ModManagerUIOn);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000078F8 File Offset: 0x00005AF8
		private void OnGUI()
		{
			if (ModLoader.bigLabel == null)
			{
				ModLoader.bigLabel = new GUIStyle(GUI.skin.label)
				{
					fontSize = 32
				};
			}
			if (ModLoader.ShowLoadSuccess > 0f)
			{
				ModLoader.ShowLoadSuccess -= Time.deltaTime;
				GUILayout.Window(8994810, ModLoader.LoadSuccessUIWindowRect, new GUI.WindowFunction(ModLoader.PostLoadSuccessWindow), "PostLoadSuccess", Array.Empty<GUILayoutOption>());
			}
			if (!ModLoader.ModManagerUIOn)
			{
				return;
			}
			GUILayout.Window(ModLoader.CurrentMainUIId, ModLoader.ModManagerUIWindowRect, new GUI.WindowFunction(ModLoader.ModLoaderMainUIWindow), "ModManagerUI", Array.Empty<GUILayoutOption>());
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00007998 File Offset: 0x00005B98
		private static void PostLoadSuccessWindow(int id)
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label("加载正式完成", ModLoader.bigLabel, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Load All Success", ModLoader.bigLabel, Array.Empty<GUILayoutOption>());
			GUILayout.EndVertical();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000079D4 File Offset: 0x00005BD4
		private static void ModLoaderMainUIWindow(int id)
		{
			ModLoader.CurrentMainUIIdSelectScroll = GUILayout.BeginScrollView(ModLoader.CurrentMainUIIdSelectScroll, new GUILayoutOption[]
			{
				GUILayout.MaxHeight(ModLoader.ModManagerUIWindowRect.height * 0.08f)
			});
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button((id == 0) ? "管理器(Manager)♪(´▽｀)" : "管理器(Manager)", Array.Empty<GUILayoutOption>()))
			{
				id = 0;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
			ModLoader.ModLoaderMainUIWindowById(id);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00007A46 File Offset: 0x00005C46
		private static void ModLoaderMainUIWindowById(int id)
		{
			if (id == 0)
			{
				ModLoader.ModManagerUIWindow();
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00007A50 File Offset: 0x00005C50
		private static void ModManagerUIWindow()
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			ModLoader.TexCompatibilityMode.Value = GUILayout.Toggle(ModLoader.TexCompatibilityMode.Value, "是否启用纹理兼容模式（开启后纹理占用内存会增加，请仅在缺图时开启）", Array.Empty<GUILayoutOption>());
			ModLoader.ModManagerUIScrollViewPos = GUILayout.BeginScrollView(ModLoader.ModManagerUIScrollViewPos, Array.Empty<GUILayoutOption>());
			foreach (KeyValuePair<string, ModPack> keyValuePair in ModLoader.ModPacks)
			{
				string text;
				ModPack modPack;
				keyValuePair.Deconstruct(out text, out modPack);
				ModLoader.ModManagerIns(modPack);
			}
			GUILayout.EndScrollView();
			if (GUILayout.Button("应用(三秒后关闭)/Apply(Three seconds to close)", Array.Empty<GUILayoutOption>()))
			{
				ModLoader.Instance.Config.Save();
				ModLoader.ReqQuit = true;
				ModLoader.WaitTime = 3f;
			}
			GUILayout.EndVertical();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00007B24 File Offset: 0x00005D24
		public static void ModManagerIns(ModPack modPack)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			modPack.EnableEntry.Value = GUILayout.Toggle(modPack.EnableEntry.Value, modPack.ModInfo.Name, Array.Empty<GUILayoutOption>());
			GUILayout.Space(20f);
			GUILayout.Label("文件名/FileName:" + modPack.FileName, Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
		}

		// Token: 0x0400004C RID: 76
		public static readonly Dictionary<string, JsonData> UniqueIdObjectExtraData = new Dictionary<string, JsonData>();

		// Token: 0x0400004D RID: 77
		public static readonly Dictionary<int, JsonData> ScriptableObjectExtraData = new Dictionary<int, JsonData>();

		// Token: 0x0400004E RID: 78
		public static readonly Dictionary<object, JsonData> ClassObjectExtraData = new Dictionary<object, JsonData>();

		// Token: 0x0400004F RID: 79
		public static readonly AccessTools.FieldRef<Dictionary<string, string>> CurrentTextsFieldRef = AccessTools.StaticFieldRefAccess<Dictionary<string, string>>(AccessTools.Field(typeof(LocalizationManager), "CurrentTexts"));

		// Token: 0x04000050 RID: 80
		public static ConfigEntry<bool> TexCompatibilityMode;

		// Token: 0x04000051 RID: 81
		public static readonly Dictionary<string, ModPack> ModPacks = new Dictionary<string, ModPack>();

		// Token: 0x04000052 RID: 82
		public static Version PluginVersion;

		// Token: 0x04000053 RID: 83
		public static Assembly GameSourceAssembly;

		// Token: 0x04000054 RID: 84
		public static readonly Harmony HarmonyInstance = new Harmony("Dop.plugin.CSTI.ModLoader");

		// Token: 0x04000055 RID: 85
		public static ModLoader Instance;

		// Token: 0x04000056 RID: 86
		public static readonly Dictionary<string, Sprite> SpriteDict = new Dictionary<string, Sprite>();

		// Token: 0x04000057 RID: 87
		public static readonly Dictionary<string, AudioClip> AudioClipDict = new Dictionary<string, AudioClip>();

		// Token: 0x04000058 RID: 88
		public static readonly Dictionary<string, WeatherSpecialEffect> WeatherSpecialEffectDict = new Dictionary<string, WeatherSpecialEffect>();

		// Token: 0x04000059 RID: 89
		public static readonly Dictionary<string, UniqueIDScriptable> AllGUIDDict = new Dictionary<string, UniqueIDScriptable>();

		// Token: 0x0400005A RID: 90
		public static readonly Dictionary<Type, Dictionary<string, UniqueIDScriptable>> AllGUIDTypeDict = new Dictionary<Type, Dictionary<string, UniqueIDScriptable>>();

		// Token: 0x0400005B RID: 91
		public static readonly Dictionary<string, Dictionary<string, CardData>> AllCardTagGuidCardDataDict = new Dictionary<string, Dictionary<string, CardData>>();

		// Token: 0x0400005C RID: 92
		public static readonly Dictionary<string, ScriptableObject> AllScriptableObjectDict = new Dictionary<string, ScriptableObject>();

		// Token: 0x0400005D RID: 93
		public static readonly Dictionary<Type, Dictionary<string, ScriptableObject>> AllScriptableObjectWithoutGuidTypeDict = new Dictionary<Type, Dictionary<string, ScriptableObject>>();

		// Token: 0x0400005E RID: 94
		public static readonly Dictionary<string, Type> ScriptableObjectKeyType = new Dictionary<string, Type>();

		// Token: 0x0400005F RID: 95
		public static readonly Dictionary<string, ContentDisplayer> CustomContentDisplayerDict = new Dictionary<string, ContentDisplayer>();

		// Token: 0x04000060 RID: 96
		public static readonly Dictionary<string, GameObject> CustomGameObjectListDict = new Dictionary<string, GameObject>();

		// Token: 0x04000061 RID: 97
		public static ModLoader.ScriptableObjectPack ProcessingScriptableObjectPack;

		// Token: 0x04000062 RID: 98
		public static readonly Dictionary<string, ModLoader.ScriptableObjectPack> WaitForWarpperEditorGuidDict = new Dictionary<string, ModLoader.ScriptableObjectPack>();

		// Token: 0x04000063 RID: 99
		private static readonly List<ModLoader.ScriptableObjectPack> WaitForWarpperEditorNoGuidList = new List<ModLoader.ScriptableObjectPack>();

		// Token: 0x04000064 RID: 100
		public static readonly List<ModLoader.ScriptableObjectPack> WaitForWarpperEditorGameSourceGUIDList = new List<ModLoader.ScriptableObjectPack>();

		// Token: 0x04000065 RID: 101
		public static readonly List<ModLoader.ScriptableObjectPack> WaitForMatchAndWarpperEditorGameSourceList = new List<ModLoader.ScriptableObjectPack>();

		// Token: 0x04000066 RID: 102
		private static readonly List<Tuple<string, string>> WaitForLoadCSVList = new List<Tuple<string, string>>();

		// Token: 0x04000067 RID: 103
		public static readonly List<Tuple<string, string, CardData>> WaitForAddBlueprintCard = new List<Tuple<string, string, CardData>>();

		// Token: 0x04000068 RID: 104
		public static readonly List<Tuple<string, CardData>> WaitForAddCardFilterGroupCard = new List<Tuple<string, CardData>>();

		// Token: 0x04000069 RID: 105
		public static readonly List<Tuple<string, GameStat>> WaitForAddVisibleGameStat = new List<Tuple<string, GameStat>>();

		// Token: 0x0400006A RID: 106
		private static readonly List<GuideEntry> WaitForAddGuideEntry = new List<GuideEntry>();

		// Token: 0x0400006B RID: 107
		public static readonly List<Tuple<string, CharacterPerk>> WaitForAddPerkGroup = new List<Tuple<string, CharacterPerk>>();

		// Token: 0x0400006C RID: 108
		private static readonly List<ModLoader.ScriptableObjectPack> WaitForAddCardTabGroup = new List<ModLoader.ScriptableObjectPack>();

		// Token: 0x0400006D RID: 109
		public static readonly List<ModLoader.ScriptableObjectPack> WaitForAddJournalPlayerCharacter = new List<ModLoader.ScriptableObjectPack>();

		// Token: 0x0400006E RID: 110
		private static readonly List<ModLoader.ScriptableObjectPack> WaitForAddDefaultContentPage = new List<ModLoader.ScriptableObjectPack>();

		// Token: 0x0400006F RID: 111
		private static readonly List<ModLoader.ScriptableObjectPack> WaitForAddMainContentPage = new List<ModLoader.ScriptableObjectPack>();

		// Token: 0x04000070 RID: 112
		public static bool HasEncounterType;

		// Token: 0x04000071 RID: 113
		public static Image MainUIBackPanel;

		// Token: 0x04000072 RID: 114
		public static RectTransform MainUIBackPanelRT;

		// Token: 0x04000073 RID: 115
		public static AssetBundle FontAssetBundle;

		// Token: 0x04000074 RID: 116
		[TupleElementNames(new string[]
		{
			"sprites",
			"modName",
			"dat",
			"name"
		})]
		private static readonly List<Task<ValueTuple<List<ValueTuple<byte[], string>>, string>>> spritesWaitList = new List<Task<ValueTuple<List<ValueTuple<byte[], string>>, string>>>();

		// Token: 0x04000075 RID: 117
		[TupleElementNames(new string[]
		{
			"uniqueObjs",
			"modName",
			"dat",
			"pat",
			"type"
		})]
		public static readonly List<Task<ValueTuple<List<ValueTuple<byte[], string, Type>>, string>>> uniqueObjWaitList = new List<Task<ValueTuple<List<ValueTuple<byte[], string, Type>>, string>>>();

		// Token: 0x04000076 RID: 118
		public static readonly SimpleOnce _onceWarp = new SimpleOnce();

		// Token: 0x04000077 RID: 119
		private static readonly SimpleOnce _once = new SimpleOnce();

		// Token: 0x04000078 RID: 120
		private static bool init_flag;

		// Token: 0x04000079 RID: 121
		public static bool ModManagerUIOn;

		// Token: 0x0400007A RID: 122
		public static Vector2 ModManagerUIScrollViewPos;

		// Token: 0x0400007B RID: 123
		public static Rect ModManagerUIWindowRect = new Rect((float)Screen.width * 0.12f, (float)Screen.height * 0.12f, (float)Screen.width * 0.55f, (float)Screen.height * 0.55f);

		// Token: 0x0400007C RID: 124
		public static Rect LoadSuccessUIWindowRect = new Rect((float)Screen.width * 0.45f, (float)Screen.height * 0.45f, (float)Screen.width * 0.1f, (float)Screen.height * 0.1f);

		// Token: 0x0400007D RID: 125
		public static bool ReqQuit;

		// Token: 0x0400007E RID: 126
		public static float WaitTime;

		// Token: 0x0400007F RID: 127
		public static bool HadBootNew;

		// Token: 0x04000080 RID: 128
		public static float ShowLoadSuccess;

		// Token: 0x04000081 RID: 129
		public static GUIStyle bigLabel;

		// Token: 0x04000082 RID: 130
		public static int CurrentMainUIId;

		// Token: 0x04000083 RID: 131
		public static Vector2 CurrentMainUIIdSelectScroll;

		// Token: 0x0200001C RID: 28
		public struct ScriptableObjectPack
		{
			// Token: 0x06000075 RID: 117 RVA: 0x00007B97 File Offset: 0x00005D97
			public ScriptableObjectPack(ScriptableObject obj, string CardDirOrGuid, string CardPath, string ModName, string CardData = "")
			{
				this.obj = obj;
				this.CardDirOrGuid = CardDirOrGuid;
				this.CardPath = CardPath;
				this.ModName = ModName;
				this.CardData = CardData;
			}

			// Token: 0x04000084 RID: 132
			public ScriptableObject obj;

			// Token: 0x04000085 RID: 133
			public readonly string CardDirOrGuid;

			// Token: 0x04000086 RID: 134
			public string CardPath;

			// Token: 0x04000087 RID: 135
			public readonly string ModName;

			// Token: 0x04000088 RID: 136
			public readonly string CardData;
		}

		// Token: 0x0200001D RID: 29
		public class MyScriptableObject : ScriptableObject
		{
			// Token: 0x04000089 RID: 137
			public int aaa;
		}
	}
}
