using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using LitJson;
using UnityEngine;

namespace ModLoader.LoaderUtil
{
	// Token: 0x02000023 RID: 35
	public static class DoWarpperLoader
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00008594 File Offset: 0x00006794
		public static void MatchAndWarpperAllEditorGameSrouce()
		{
			foreach (UniqueIDScriptable uniqueIDScriptable in ModLoader.AllGUIDDict.Values)
			{
				try
				{
					CardData cardData = uniqueIDScriptable as CardData;
					if (cardData != null)
					{
						foreach (CardTag cardTag in cardData.CardTags)
						{
							if (!ModLoader.AllCardTagGuidCardDataDict.ContainsKey(cardTag.name))
							{
								ModLoader.AllCardTagGuidCardDataDict.Add(cardTag.name, new Dictionary<string, CardData>());
							}
							Dictionary<string, CardData> dictionary;
							if (ModLoader.AllCardTagGuidCardDataDict.TryGetValue(cardTag.name, out dictionary))
							{
								dictionary.Add(cardData.UniqueID, cardData);
							}
						}
					}
				}
				catch
				{
				}
			}
			foreach (ModLoader.ScriptableObjectPack scriptableObjectPack in ModLoader.WaitForMatchAndWarpperEditorGameSourceList)
			{
				try
				{
					if (!Utility.IsNullOrWhiteSpace(scriptableObjectPack.CardData))
					{
						JsonData jsonData = JsonMapper.ToObject(scriptableObjectPack.CardData);
						if (jsonData.ContainsKey("MatchTagWarpData") && jsonData["MatchTagWarpData"].IsArray && jsonData["MatchTagWarpData"].Count > 0)
						{
							Dictionary<string, CardData> dictionary2;
							if (ModLoader.AllCardTagGuidCardDataDict.TryGetValue(jsonData["MatchTagWarpData"][0].ToString(), out dictionary2))
							{
								List<string> list = dictionary2.Keys.ToList<string>();
								for (int j = 1; j < jsonData["MatchTagWarpData"].Count; j++)
								{
									Dictionary<string, CardData> dictionary3;
									if (ModLoader.AllCardTagGuidCardDataDict.TryGetValue(jsonData["MatchTagWarpData"][j].ToString(), out dictionary3))
									{
										list = list.Intersect(dictionary3.Keys).ToList<string>();
									}
								}
								foreach (string key in list)
								{
									UniqueIDScriptable uniqueIDScriptable2;
									if (ModLoader.AllGUIDDict.TryGetValue(key, out uniqueIDScriptable2))
									{
										CardData cardData2 = uniqueIDScriptable2 as CardData;
										if (cardData2 != null && (!jsonData.ContainsKey("MatchTypeWarpData") || !jsonData["MatchTypeWarpData"].IsString || !(cardData2.CardType.ToString() != jsonData["MatchTypeWarpData"].ToString())))
										{
											WarpperFunction.JsonCommonWarpper(uniqueIDScriptable2, jsonData);
											Traverse traverse = Traverse.Create(cardData2).Method("FillDropsList", Array.Empty<object>());
											if (traverse != null)
											{
												traverse.GetValue();
											}
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("MatchAndWarpperAllEditorGameSrouce Warpper " + ex.Message);
				}
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000088E0 File Offset: 0x00006AE0
		public static void WarpperAllEditorGameSrouces()
		{
			for (int i = 0; i < ModLoader.WaitForWarpperEditorGameSourceGUIDList.Count; i++)
			{
				ModLoader.ScriptableObjectPack scriptableObjectPack = ModLoader.WaitForWarpperEditorGameSourceGUIDList[i];
				try
				{
					if (scriptableObjectPack.obj == null)
					{
						UniqueIDScriptable obj;
						if (!ModLoader.AllGUIDDict.TryGetValue(scriptableObjectPack.CardDirOrGuid, out obj))
						{
							goto IL_14A;
						}
						scriptableObjectPack.obj = obj;
					}
					ModLoader.ProcessingScriptableObjectPack = scriptableObjectPack;
					if (!Utility.IsNullOrWhiteSpace(scriptableObjectPack.CardData))
					{
						JsonData jsonData = JsonMapper.ToObject(scriptableObjectPack.CardData);
						if (jsonData.ContainsKey("MatchTagWarpData") && jsonData["MatchTagWarpData"].IsArray && jsonData["MatchTagWarpData"].Count > 0)
						{
							ModLoader.WaitForMatchAndWarpperEditorGameSourceList.Add(scriptableObjectPack);
							goto IL_14A;
						}
						if (jsonData.ContainsKey("ModLoaderSpecialOverwrite") && jsonData["ModLoaderSpecialOverwrite"].IsBoolean && (bool)jsonData["ModLoaderSpecialOverwrite"])
						{
							JsonUtility.FromJsonOverwrite(scriptableObjectPack.CardData, scriptableObjectPack.obj);
						}
						WarpperFunction.JsonCommonWarpper(scriptableObjectPack.obj, jsonData);
					}
					CardData cardData = scriptableObjectPack.obj as CardData;
					if (cardData != null)
					{
						Traverse traverse = Traverse.Create(cardData).Method("FillDropsList", Array.Empty<object>());
						if (traverse != null)
						{
							traverse.GetValue();
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("WarpperAllEditorGameSrouces " + ex.Message);
				}
				IL_14A:;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00008A68 File Offset: 0x00006C68
		public static void WarpperAllEditorMods()
		{
			if (!ModLoader._onceWarp.DoOnce())
			{
				return;
			}
			foreach (KeyValuePair<string, ModLoader.ScriptableObjectPack> keyValuePair in ModLoader.WaitForWarpperEditorGuidDict)
			{
				try
				{
					ModLoader.ProcessingScriptableObjectPack = keyValuePair.Value;
					JsonData jsonData = JsonMapper.ToObject(keyValuePair.Value.CardData);
					WarpperFunction.JsonCommonWarpper(keyValuePair.Value.obj, jsonData);
					CardData cardData = keyValuePair.Value.obj as CardData;
					if (cardData != null)
					{
						if (cardData.CardType == CardTypes.Blueprint && jsonData.ContainsKey("BlueprintCardDataCardTabGroup") && jsonData["BlueprintCardDataCardTabGroup"].IsString && !Utility.IsNullOrWhiteSpace(jsonData["BlueprintCardDataCardTabGroup"].ToString()) && jsonData.ContainsKey("BlueprintCardDataCardTabSubGroup") && jsonData["BlueprintCardDataCardTabSubGroup"].IsString && !Utility.IsNullOrWhiteSpace(jsonData["BlueprintCardDataCardTabSubGroup"].ToString()))
						{
							ModLoader.WaitForAddBlueprintCard.Add(new Tuple<string, string, CardData>(jsonData["BlueprintCardDataCardTabGroup"].ToString(), jsonData["BlueprintCardDataCardTabSubGroup"].ToString(), cardData));
						}
						Dictionary<string, ScriptableObject> dictionary;
						if (jsonData.ContainsKey("ItemCardDataCardTabGpGroup") && jsonData["ItemCardDataCardTabGpGroup"].IsArray && ModLoader.AllScriptableObjectWithoutGuidTypeDict.TryGetValue(typeof(CardTabGroup), out dictionary))
						{
							for (int i = 0; i < jsonData["ItemCardDataCardTabGpGroup"].Count; i++)
							{
								ScriptableObject scriptableObject;
								if (jsonData["ItemCardDataCardTabGpGroup"][i].IsString && dictionary.TryGetValue(jsonData["ItemCardDataCardTabGpGroup"][i].ToString(), out scriptableObject))
								{
									(scriptableObject as CardTabGroup).IncludedCards.Add(cardData);
								}
							}
						}
						if (jsonData.ContainsKey("CardDataCardFilterGroup") && jsonData["CardDataCardFilterGroup"].IsArray)
						{
							for (int j = 0; j < jsonData["CardDataCardFilterGroup"].Count; j++)
							{
								if (jsonData["CardDataCardFilterGroup"][j].IsString && !Utility.IsNullOrWhiteSpace(jsonData["CardDataCardFilterGroup"][j].ToString()))
								{
									ModLoader.WaitForAddCardFilterGroupCard.Add(new Tuple<string, CardData>(jsonData["CardDataCardFilterGroup"][j].ToString(), cardData));
								}
							}
						}
						Traverse traverse = Traverse.Create(cardData).Method("FillDropsList", Array.Empty<object>());
						if (traverse != null)
						{
							traverse.GetValue();
						}
					}
					else if (keyValuePair.Value.obj is CharacterPerk)
					{
						if (jsonData.ContainsKey("CharacterPerkPerkGroup") && jsonData["CharacterPerkPerkGroup"].IsString && !Utility.IsNullOrWhiteSpace(jsonData["CharacterPerkPerkGroup"].ToString()))
						{
							ModLoader.WaitForAddPerkGroup.Add(new Tuple<string, CharacterPerk>(jsonData["CharacterPerkPerkGroup"].ToString(), keyValuePair.Value.obj as CharacterPerk));
						}
					}
					else if (keyValuePair.Value.obj is GameStat)
					{
						if (jsonData.ContainsKey("VisibleGameStatStatListTab") && jsonData["VisibleGameStatStatListTab"].IsString && !Utility.IsNullOrWhiteSpace(jsonData["VisibleGameStatStatListTab"].ToString()))
						{
							ModLoader.WaitForAddVisibleGameStat.Add(new Tuple<string, GameStat>(jsonData["VisibleGameStatStatListTab"].ToString(), keyValuePair.Value.obj as GameStat));
						}
					}
					else if (keyValuePair.Value.obj is PlayerCharacter)
					{
						Dictionary<string, UniqueIDScriptable> dictionary2;
						if (ModLoader.AllGUIDTypeDict.TryGetValue(typeof(Gamemode), out dictionary2))
						{
							foreach (KeyValuePair<string, UniqueIDScriptable> keyValuePair2 in dictionary2)
							{
								Gamemode gamemode = keyValuePair2.Value as Gamemode;
								Array.Resize<PlayerCharacter>(ref gamemode.PlayableCharacters, gamemode.PlayableCharacters.Length + 1);
								gamemode.PlayableCharacters[gamemode.PlayableCharacters.Length - 1] = (keyValuePair.Value.obj as PlayerCharacter);
							}
						}
						ModLoader.WaitForAddJournalPlayerCharacter.Add(new ModLoader.ScriptableObjectPack(keyValuePair.Value.obj, "", "", "", keyValuePair.Value.CardData));
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("WarpperAllEditorMods " + ex.Message);
				}
			}
			ModLoader._onceWarp.SetDone();
		}
	}
}
