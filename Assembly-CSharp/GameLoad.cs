using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000188 RID: 392
public class GameLoad : MonoBehaviour
{
	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0005BE69 File Offset: 0x0005A069
	// (set) Token: 0x06000A5A RID: 2650 RVA: 0x0005BE71 File Offset: 0x0005A071
	public int CurrentGameDataIndex { get; private set; }

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0005BE7A File Offset: 0x0005A07A
	// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0005BE82 File Offset: 0x0005A082
	public GameOptions CurrentGameOptions { get; private set; }

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0005BE8B File Offset: 0x0005A08B
	public static uint SteamID
	{
		get
		{
			return 1694420U;
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06000A5E RID: 2654 RVA: 0x0005BE92 File Offset: 0x0005A092
	// (set) Token: 0x06000A5F RID: 2655 RVA: 0x0005BE9F File Offset: 0x0005A09F
	public int LastCheckedNotesVersion
	{
		get
		{
			return this.SaveData.LastCheckedInfoNotes;
		}
		set
		{
			this.SaveData.LastCheckedInfoNotes = value;
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0005BEAD File Offset: 0x0005A0AD
	public static GameOptions GetGameOptions
	{
		get
		{
			if (GameLoad.Instance)
			{
				return GameLoad.Instance.CurrentGameOptions;
			}
			return null;
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0005BEC7 File Offset: 0x0005A0C7
	private static string GameFilesDirectoryPath
	{
		get
		{
			return string.Format("{0}/Games", Application.persistentDataPath);
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0005BED8 File Offset: 0x0005A0D8
	private static string WritingFileBackupPath(string _Path)
	{
		return string.Format("{0}.backup", _Path);
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0005BEE8 File Offset: 0x0005A0E8
	private static string CreationDateID
	{
		get
		{
			return string.Format("{0}-{1}-{2}-{3}.{4}.{5}", new object[]
			{
				DateTime.Now.Year,
				DateTime.Now.Month,
				DateTime.Now.Day,
				DateTime.Now.Hour,
				DateTime.Now.Minute,
				DateTime.Now.Second
			});
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0005BF84 File Offset: 0x0005A184
	private static string GenerateGameFileName(int _Index)
	{
		return string.Format("Slot_{0}.json", (_Index < 4) ? (_Index + 1).ToString() : string.Format("{0}_{1}", GameLoad.CreationDateID, (_Index + 1).ToString()));
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0005BFC6 File Offset: 0x0005A1C6
	private static string GameFilePath(string _FileName)
	{
		return string.Format("{0}/{1}", GameLoad.GameFilesDirectoryPath, _FileName);
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0005BFD8 File Offset: 0x0005A1D8
	private static string MainSaveFilePath
	{
		get
		{
			return string.Format("{0}/SaveData.json", Application.persistentDataPath);
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0005BFE9 File Offset: 0x0005A1E9
	private static string OptionsFilePath
	{
		get
		{
			return string.Format("{0}/Options.json", Application.persistentDataPath);
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000A68 RID: 2664 RVA: 0x0005BFFA File Offset: 0x0005A1FA
	private static string BackupsPath
	{
		get
		{
			return string.Format("{0}/Backups", Application.persistentDataPath);
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0005C00B File Offset: 0x0005A20B
	private static string UnlocksFixBackup(int _Version)
	{
		return string.Format("{0}/UnlocksFix_{1}", GameLoad.BackupsPath, _Version.ToString());
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0005C023 File Offset: 0x0005A223
	private static string OldGamesFileBackupPath
	{
		get
		{
			return string.Format("{0}/SaveData.json", GameLoad.BackupsPath);
		}
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0005C034 File Offset: 0x0005A234
	public static bool WriteAllTextWithBackup(string path, string contents)
	{
		string text = Application.persistentDataPath + "/SavingCodeTempFile";
		string text2 = GameLoad.WritingFileBackupPath(path);
		if (File.Exists(text2))
		{
			File.Delete(text2);
		}
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(contents);
		if (File.Exists(path))
		{
			using (FileStream fileStream = File.Create(text, 4096, FileOptions.WriteThrough))
			{
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
			}
			bool flag = false;
			try
			{
				File.Replace(text, path, text2);
				Debug.LogWarning("Success!");
				return true;
			}
			catch (IOException e)
			{
				if ((Marshal.GetHRForException(e) & 65535) != 32)
				{
					throw;
				}
				flag = true;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			while (flag)
			{
				if (Time.realtimeSinceStartup - realtimeSinceStartup >= 3f)
				{
					break;
				}
				try
				{
					File.Replace(text, path, text2);
					Debug.LogWarning("Success!");
					return true;
				}
				catch (IOException e2)
				{
					if ((Marshal.GetHRForException(e2) & 65535) != 32)
					{
						throw;
					}
				}
			}
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			return false;
		}
		using (FileStream fileStream2 = File.Create(path, 4096, FileOptions.WriteThrough))
		{
			fileStream2.Write(bytes, 0, bytes.Length);
		}
		return true;
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0005C1B0 File Offset: 0x0005A3B0
	private void Awake()
	{
		if (GameLoad.Instance)
		{
			AsyncLoading.LoadScene(this.MenuSceneIndex);
			return;
		}
		GameLoad.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Application.targetFrameRate = Mathf.Max(Screen.currentResolution.refreshRate, 60);
		if (!this.DataBase)
		{
			Debug.LogError("No data base loaded!");
		}
		UniqueIDScriptable.Duplicates.Clear();
		UniqueIDScriptable.ClearDict();
		for (int i = 0; i < this.DataBase.AllData.Count; i++)
		{
			if (this.DataBase.AllData[i])
			{
				this.DataBase.AllData[i].Init();
			}
		}
		if (UniqueIDScriptable.Duplicates.Count > 0)
		{
			Debug.LogError("Some objects have the same IDs!");
			for (int j = 0; j < UniqueIDScriptable.Duplicates.Count; j++)
			{
				Debug.LogError(UniqueIDScriptable.Duplicates[j].name + " has the ID of registered item " + UniqueIDScriptable.GetFromID(UniqueIDScriptable.Duplicates[j].UniqueID).name, UniqueIDScriptable.Duplicates[j]);
			}
		}
		else
		{
			Debug.Log("Data dictionnary successfully loaded.");
		}
		if (!Directory.Exists(GameLoad.GameFilesDirectoryPath))
		{
			Directory.CreateDirectory(GameLoad.GameFilesDirectoryPath);
		}
		this.LoadOptions();
		this.LoadMainGameData();
		this.LoadGameFilesData();
		this.ImportOldSaves();
		for (int k = 0; k < 4; k++)
		{
			if (this.Games.Count <= k)
			{
				this.Games.Add(new GameSaveFile(GameLoad.GenerateGameFileName(k), k));
			}
			else if (this.Games[k] == null)
			{
				this.Games.Add(new GameSaveFile(GameLoad.GenerateGameFileName(k), k));
			}
		}
		if (ForceGameLoad.ShouldLoadGameScene)
		{
			this.StartNewGame(-1, false, false);
			return;
		}
		AsyncLoading.LoadScene(this.MenuSceneIndex);
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0005C390 File Offset: 0x0005A590
	private void Update()
	{
		if (MBSingleton<GameManager>.Instance)
		{
			if (Input.GetKeyDown(KeyCode.F5) && Application.isEditor)
			{
				this.AutoSaveGame(false);
			}
			if (Input.GetKeyDown(KeyCode.F6) && Application.isEditor)
			{
				this.AutoLoadGame();
			}
		}
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0005C3DD File Offset: 0x0005A5DD
	private void OnApplicationQuit()
	{
		if (MBSingleton<GameManager>.Instance && !Application.isEditor)
		{
			this.AutoSaveGame(false);
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0005C3FC File Offset: 0x0005A5FC
	public void LoadCheckpoint()
	{
		if (this.CurrentGameDataIndex < 0 || this.CurrentGameDataIndex >= this.Games.Count)
		{
			this.CurrentGameDataIndex = 0;
		}
		if (this.Games[this.CurrentGameDataIndex] != null && this.Games[this.CurrentGameDataIndex].CheckpointData != null)
		{
			this.Games[this.CurrentGameDataIndex].MainData = this.Games[this.CurrentGameDataIndex].CheckpointData.Copy();
		}
		this.LoadGame(this.CurrentGameDataIndex);
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0005C494 File Offset: 0x0005A694
	public void AutoLoadGame()
	{
		if (this.CurrentGameDataIndex < 0 || this.CurrentGameDataIndex >= this.Games.Count)
		{
			this.CurrentGameDataIndex = 0;
		}
		this.LoadGame(this.CurrentGameDataIndex);
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0005C4C8 File Offset: 0x0005A6C8
	public void StartNewGame(int _Index, bool _EasyPackage, bool _SafeMode)
	{
		this.CurrentGameDataIndex = _Index;
		if (_Index >= 0)
		{
			while (this.Games.Count <= _Index)
			{
				this.Games.Add(new GameSaveFile(GameLoad.GenerateGameFileName(_Index), _Index));
			}
			this.Games[_Index].MainData = new GameSaveData();
			this.Games[_Index].MainData.EasyPackage = _EasyPackage;
			this.Games[_Index].MainData.SafeMode = _SafeMode;
		}
		if (ForceGameLoad.ShouldLoadGameScene)
		{
			SceneManager.LoadScene(this.GameSceneIndex);
			return;
		}
		AsyncLoading.LoadScene(this.GameSceneIndex);
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0005C56C File Offset: 0x0005A76C
	public void LoadGame(int _Index)
	{
		this.CurrentGameDataIndex = _Index;
		this.Games[this.CurrentGameDataIndex].MainData.CreateDicts();
		this.Games[this.CurrentGameDataIndex].MainData.SortCards();
		AsyncLoading.LoadScene(this.GameSceneIndex);
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0005C5C1 File Offset: 0x0005A7C1
	private bool ValidSaveCheck
	{
		get
		{
			if (SteamManager.StartedWithAppIdFile)
			{
				if (!Application.isEditor && !Debug.isDebugBuild)
				{
					Debug.LogError("This game was not started from Steam");
					return false;
				}
				Debug.LogWarning("steam_appid.txt detected but ignored because this is a test build");
			}
			return true;
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0005C5EF File Offset: 0x0005A7EF
	public bool AutoSaveGame(bool _Checkpoint)
	{
		return this.SaveGame(this.CurrentGameDataIndex, _Checkpoint);
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0005C600 File Offset: 0x0005A800
	public bool SaveGame(int _GameIndex, bool _Checkpoint)
	{
		if (!this.ValidSaveCheck)
		{
			return false;
		}
		if (_GameIndex < 0)
		{
			_GameIndex = 4;
		}
		this.GM = MBSingleton<GameManager>.Instance;
		if (this.SaveData == null)
		{
			this.SaveData = new GlobalSaveData();
			this.SaveData.IsValid = true;
		}
		else
		{
			this.SaveData.IsValid = true;
		}
		this.SaveData.SavedFromFullGame = true;
		int num = Mathf.Clamp(_GameIndex, 0, this.Games.Count);
		GameSaveData gameSaveData;
		if (this.Games.Count <= num)
		{
			this.Games.Add(new GameSaveFile(GameLoad.GenerateGameFileName(num), num));
		}
		else
		{
			gameSaveData = this.Games[num].CheckpointData.Copy();
			this.Games[num] = new GameSaveFile(this.Games[num].FileName, num);
			this.Games[num].CheckpointData = gameSaveData;
		}
		gameSaveData = this.Games[num].MainData;
		gameSaveData.AllEndgameLogs = MBSingleton<GameManager>.Instance.GetSavedLogs;
		gameSaveData.OpenedJournal = MBSingleton<GameManager>.Instance.OpenedJournal;
		gameSaveData.CurrentGamemode = (GameManager.CurrentGamemode ? UniqueIDScriptable.SaveID(GameManager.CurrentGamemode) : "Null");
		gameSaveData.SafeMode = MBSingleton<GameManager>.Instance.IsSafeMode;
		if (!GameManager.CurrentPlayerCharacter)
		{
			gameSaveData.CurrentCharacter = "Null";
		}
		else
		{
			gameSaveData.CurrentCharacter = (GameManager.CurrentPlayerCharacter.CustomCharacter ? "Custom" : UniqueIDScriptable.SaveID(GameManager.CurrentPlayerCharacter));
			if (GameManager.CurrentPlayerCharacter.CustomCharacter)
			{
				gameSaveData.CharacterData = new PlayerCharacterSaveData();
				gameSaveData.CharacterData.CharacterName = GameManager.CurrentPlayerCharacter.CharacterName;
				gameSaveData.CharacterData.CharacterBio = GameManager.CurrentPlayerCharacter.CharacterDescription;
				gameSaveData.CharacterData.PortraitID = GameManager.CurrentPlayerCharacter.PortraitID;
				gameSaveData.CharacterData.CharacterPerks = new List<string>();
				if (GameManager.CurrentPlayerCharacter.CharacterPerks != null)
				{
					for (int i = 0; i < GameManager.CurrentPlayerCharacter.CharacterPerks.Count; i++)
					{
						gameSaveData.CharacterData.CharacterPerks.Add(UniqueIDScriptable.SaveID(GameManager.CurrentPlayerCharacter.CharacterPerks[i]));
					}
				}
			}
			else
			{
				gameSaveData.CharacterData = null;
			}
		}
		gameSaveData.ModifierPackages = new List<string>();
		if (GameManager.CurrentModifierPackages != null)
		{
			for (int j = 0; j < GameManager.CurrentModifierPackages.Count; j++)
			{
				gameSaveData.ModifierPackages.Add(UniqueIDScriptable.SaveID(GameManager.CurrentModifierPackages[j]));
			}
		}
		gameSaveData.CurrentDay = this.GM.CurrentDay;
		gameSaveData.CurrentDayTimePoints = this.GM.DayTimePoints;
		gameSaveData.CurrentMiniTicks = this.GM.CurrentMiniTicks;
		gameSaveData.DaytimeToHour = GameManager.TotalTicksToHourOfTheDayString(GameManager.HoursToTick((float)this.GM.DaySettings.DayStartingHour) + this.GM.CurrentTickInfo.z, this.GM.CurrentMiniTicks);
		if (this.GM.CurrentHandCard)
		{
			gameSaveData.CurrentHandCard = this.GM.CurrentHandCard.Save();
		}
		gameSaveData.CurrentEnvironmentCard = this.GM.CurrentEnvironmentCard.Save();
		gameSaveData.PrevEnvironmentID = UniqueIDScriptable.SaveID(this.GM.PrevEnvironment);
		gameSaveData.CurrentTravelIndex = this.GM.CurrentTravelIndex;
		if (this.GM.CurrentEventCard)
		{
			gameSaveData.CurrentEventCard = this.GM.CurrentEventCard.Save();
		}
		gameSaveData.CurrentWeatherCard = this.GM.CurrentWeatherCard.Save();
		gameSaveData.AllActions = new List<SelfTriggeredActionSaveData>();
		gameSaveData.EnvironmentsData.Clear();
		foreach (KeyValuePair<string, EnvironmentSaveData> keyValuePair in this.GM.EnvironmentsData)
		{
			gameSaveData.EnvironmentsData.Add(keyValuePair.Value);
		}
		for (int k = 0; k < this.GM.AllCards.Count; k++)
		{
			if (!(this.GM.AllCards[k].CurrentContainer != null) && !(this.GM.AllCards[k] == this.GM.CurrentEnvironmentCard) && !(this.GM.AllCards[k] == this.GM.CurrentHandCard) && !(this.GM.AllCards[k] == this.GM.CurrentWeatherCard) && !(this.GM.AllCards[k] == this.GM.CurrentEventCard))
			{
				if (this.GM.AllCards[k].CardModel.CardType == CardTypes.Explorable)
				{
					if (this.GM.AllCards[k].InventoryCount(null) > 0)
					{
						for (int l = 0; l < this.GM.AllCards[k].CardsInInventory.Count; l++)
						{
							if (!this.GM.AllCards[k].CardsInInventory[l].IsFree)
							{
								for (int m = 0; m < this.GM.AllCards[k].CardsInInventory[l].CardAmt; m++)
								{
									gameSaveData.CurrentCardsData.Add(this.GM.AllCards[k].CardsInInventory[l].AllCards[m].Save());
									if (MBSingleton<GraphicsManager>.Instance)
									{
										gameSaveData.CurrentCardsData[gameSaveData.CurrentCardsData.Count - 1].SlotInformation.SlotType = MBSingleton<GraphicsManager>.Instance.CardToSlotType(this.GM.AllCards[k].CardsInInventory[l].AllCards[m].CardModel.CardType, false);
									}
									else
									{
										gameSaveData.CurrentCardsData[gameSaveData.CurrentCardsData.Count - 1].SlotInformation.SlotType = SlotsTypes.Base;
									}
									if (this.GM.AllCards[k].CardsInInventory[l].AllCards[m].CardModel.IsTravellingCard)
									{
										gameSaveData.CurrentCardsData[gameSaveData.CurrentCardsData.Count - 1].SlotInformation.SlotIndex = this.GM.GetTravellingCardIndex(null);
									}
									else
									{
										gameSaveData.CurrentCardsData[gameSaveData.CurrentCardsData.Count - 1].SlotInformation.SlotIndex = this.GM.AllCards.Count;
									}
								}
							}
						}
					}
					gameSaveData.CurrentCardsData.Add(this.GM.AllCards[k].Save());
				}
				else if (this.GM.AllCards[k].IsInventoryCard || this.GM.AllCards[k].IsLiquidContainer)
				{
					gameSaveData.CurrentInventoryCards.Add(this.GM.AllCards[k].SaveInventory(gameSaveData.CurrentNestedInventoryCards, false));
				}
				else
				{
					gameSaveData.CurrentCardsData.Add(this.GM.AllCards[k].Save());
				}
			}
		}
		gameSaveData.EncounteredEvents = new List<string>();
		for (int n = 0; n < this.GM.EncounteredEvents.Count; n++)
		{
			gameSaveData.EncounteredEvents.Add(UniqueIDScriptable.SaveID(this.GM.EncounteredEvents[n]));
		}
		gameSaveData.EventCardQueue = new List<string>();
		for (int num2 = 0; num2 < this.GM.EventCardQueue.Count; num2++)
		{
			gameSaveData.EventCardQueue.Add(UniqueIDScriptable.SaveID(this.GM.EventCardQueue[num2]));
		}
		gameSaveData.CheckedBlueprints = new List<string>();
		for (int num3 = 0; num3 < this.GM.CheckedBlueprints.Count; num3++)
		{
			gameSaveData.CheckedBlueprints.Add(UniqueIDScriptable.SaveID(this.GM.CheckedBlueprints[num3]));
		}
		gameSaveData.PurchasableBlueprintCards = new List<string>();
		for (int num4 = 0; num4 < this.GM.PurchasableBlueprintCards.Count; num4++)
		{
			gameSaveData.PurchasableBlueprintCards.Add(UniqueIDScriptable.SaveID(this.GM.PurchasableBlueprintCards[num4]));
		}
		gameSaveData.ResearchedBlueprintCards = new List<BlueprintResearchData>();
		foreach (KeyValuePair<CardData, int> keyValuePair2 in this.GM.BlueprintResearchTimes)
		{
			gameSaveData.ResearchedBlueprintCards.Add(new BlueprintResearchData(UniqueIDScriptable.SaveID(keyValuePair2.Key), keyValuePair2.Value));
		}
		gameSaveData.CurrentResearchedBlueprint = UniqueIDScriptable.SaveID(MBSingleton<GraphicsManager>.Instance.BlueprintModelsPopup.CurrentResearch);
		gameSaveData.AllStats = new List<StatSaveData>();
		for (int num5 = 0; num5 < this.GM.AllStats.Count; num5++)
		{
			gameSaveData.AllStats.Add(this.GM.AllStats[num5].Save());
		}
		for (int num6 = 0; num6 < this.GM.AllCounters.Count; num6++)
		{
			gameSaveData.AllCounters.Add(this.GM.AllCounters[num6]);
		}
		for (int num7 = 0; num7 < this.GM.AllSelfActions.Count; num7++)
		{
			gameSaveData.AllActions.Add(this.GM.AllSelfActions[num7].Save());
		}
		for (int num8 = 0; num8 < this.GM.AllObjectives.Count; num8++)
		{
			gameSaveData.AllObjectives.Add(this.GM.AllObjectives[num8].Save());
		}
		if (MBSingleton<EncounterPopup>.Instance)
		{
			gameSaveData.CurrentEncounter = MBSingleton<EncounterPopup>.Instance.SaveCurrentEncounter();
		}
		this.CurrentGameDataIndex = _GameIndex;
		if (_Checkpoint)
		{
			Debug.Log("saving checkpoint");
			this.Games[num].CheckpointData = gameSaveData.Copy();
		}
		return this.SaveGameDataToFile(num) && this.SaveMainDataToFile();
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0005D0F4 File Offset: 0x0005B2F4
	public bool SaveGameDataToFile(int _Index)
	{
		if (!this.ValidSaveCheck || _Index < 0 || _Index >= this.Games.Count)
		{
			return false;
		}
		string contents = JsonUtility.ToJson(this.Games[_Index], Debug.isDebugBuild);
		if (GameLoad.WriteAllTextWithBackup(GameLoad.GameFilePath(this.Games[_Index].FileName), contents))
		{
			return true;
		}
		Action onSaveFail = this.OnSaveFail;
		if (onSaveFail != null)
		{
			onSaveFail();
		}
		return false;
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0005D168 File Offset: 0x0005B368
	public bool SaveMainDataToFile()
	{
		if (!this.ValidSaveCheck)
		{
			return false;
		}
		string contents = JsonUtility.ToJson(this.SaveData, Debug.isDebugBuild);
		if (GameLoad.WriteAllTextWithBackup(GameLoad.MainSaveFilePath, contents))
		{
			return true;
		}
		Action onSaveFail = this.OnSaveFail;
		if (onSaveFail != null)
		{
			onSaveFail();
		}
		return false;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0005D1B4 File Offset: 0x0005B3B4
	public void DeleteAll()
	{
		if (File.Exists(GameLoad.MainSaveFilePath))
		{
			File.Delete(GameLoad.MainSaveFilePath);
		}
		if (File.Exists(GameLoad.WritingFileBackupPath(GameLoad.MainSaveFilePath)))
		{
			File.Delete(GameLoad.WritingFileBackupPath(GameLoad.MainSaveFilePath));
		}
		if (Directory.Exists(GameLoad.GameFilesDirectoryPath))
		{
			Directory.Delete(GameLoad.GameFilesDirectoryPath, true);
		}
		if (File.Exists(GameLoad.OptionsFilePath))
		{
			File.Delete(GameLoad.OptionsFilePath);
		}
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		SceneManager.LoadScene(0);
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0005D238 File Offset: 0x0005B438
	public void DeleteGameData(int _Index)
	{
		if (_Index < 0 || _Index >= this.Games.Count)
		{
			return;
		}
		string path = GameLoad.GameFilePath(this.Games[_Index].FileName);
		if (_Index < 4)
		{
			this.Games[_Index] = new GameSaveFile(GameLoad.GenerateGameFileName(_Index), _Index);
		}
		else
		{
			this.Games.RemoveAt(_Index);
		}
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		if (File.Exists(GameLoad.WritingFileBackupPath(path)))
		{
			File.Delete(GameLoad.WritingFileBackupPath(path));
		}
		for (int i = 0; i < this.Games.Count; i++)
		{
			this.Games[i].SlotIndex = i;
		}
		if (_Index == this.CurrentGameDataIndex)
		{
			this.CurrentGameDataIndex = this.Games.Count;
		}
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0005D304 File Offset: 0x0005B504
	public void LoadGameFilesData()
	{
		if (this.Games == null)
		{
			this.Games = new List<GameSaveFile>();
		}
		FileInfo[] files = new DirectoryInfo(GameLoad.GameFilesDirectoryPath).GetFiles();
		if (files == null)
		{
			return;
		}
		if (files.Length == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		int i = 0;
		while (i < files.Length)
		{
			if (list.Count <= 0)
			{
				goto IL_78;
			}
			bool flag = false;
			for (int j = list.Count - 1; j >= 0; j--)
			{
				if (files[i].FullName.Contains(list[j]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				goto IL_78;
			}
			IL_94:
			i++;
			continue;
			IL_78:
			this.LoadGameFile(files[i].FullName);
			list.Add(files[i].FullName);
			goto IL_94;
		}
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0005D3B4 File Offset: 0x0005B5B4
	private void LoadGameFile(string _Path)
	{
		if (!_Path.EndsWith(".json") && !_Path.EndsWith(".backup"))
		{
			return;
		}
		string text = "";
		string text2 = "";
		if (File.Exists(_Path))
		{
			text = File.ReadAllText(_Path);
		}
		if (File.Exists(GameLoad.WritingFileBackupPath(_Path)))
		{
			text2 = File.ReadAllText(GameLoad.WritingFileBackupPath(_Path));
		}
		if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
		{
			Debug.Log("No save file nor backup found");
			return;
		}
		GameSaveFile gameSaveFile = null;
		GameSaveFile gameSaveFile2 = null;
		GameSaveFile gameSaveFile3 = null;
		if (!string.IsNullOrEmpty(text))
		{
			if (this.SaveEdits)
			{
				string text3 = this.SaveEdits.DoSaveEdit(text);
				if (!string.IsNullOrEmpty(text3))
				{
					try
					{
						gameSaveFile = JsonUtility.FromJson<GameSaveFile>(text3);
					}
					catch (ArgumentException ex)
					{
						Debug.LogWarning(_Path + " - Edited save file was unreadable: " + ex.Message);
					}
				}
			}
			if (gameSaveFile == null)
			{
				Debug.Log(_Path + " - loading true main data, edit is not valid :(");
				try
				{
					gameSaveFile = JsonUtility.FromJson<GameSaveFile>(text);
				}
				catch (ArgumentException ex2)
				{
					Debug.LogWarning(_Path + " - Main save file was unreadable: " + ex2.Message);
				}
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			if (this.SaveEdits)
			{
				string text3 = this.SaveEdits.DoSaveEdit(text2);
				if (!string.IsNullOrEmpty(text3))
				{
					try
					{
						gameSaveFile2 = JsonUtility.FromJson<GameSaveFile>(text3);
					}
					catch (ArgumentException ex3)
					{
						Debug.LogWarning("Edited save file was unreadable: " + ex3.Message);
					}
				}
			}
			if (gameSaveFile2 == null)
			{
				try
				{
					gameSaveFile2 = JsonUtility.FromJson<GameSaveFile>(text2);
				}
				catch (ArgumentException ex4)
				{
					Debug.LogWarning("Backup save file was unreadable: " + ex4.Message);
				}
			}
		}
		if (gameSaveFile != null)
		{
			gameSaveFile3 = gameSaveFile;
		}
		else if (gameSaveFile2 != null)
		{
			gameSaveFile3 = gameSaveFile2;
		}
		if (gameSaveFile3 == null)
		{
			Debug.Log("No valid save nor backup files found");
			return;
		}
		while (gameSaveFile3.SlotIndex >= this.Games.Count)
		{
			this.Games.Add(new GameSaveFile(GameLoad.GenerateGameFileName(this.Games.Count), this.Games.Count));
		}
		this.Games[gameSaveFile3.SlotIndex] = gameSaveFile3;
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0005D5D4 File Offset: 0x0005B7D4
	private void ImportOldSaves()
	{
		if (this.SaveData == null)
		{
			return;
		}
		if (!this.SaveData.IsValid)
		{
			return;
		}
		if (this.SaveData.Games == null)
		{
			return;
		}
		if (this.SaveData.Games.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.SaveData.Games.Count; i++)
		{
			if (this.Games.Count <= i || this.Games[i] == null || this.Games[i].MainData == null || !this.Games[i].MainData.HasCardsData)
			{
				if (i >= this.Games.Count)
				{
					this.Games.Add(new GameSaveFile(GameLoad.GenerateGameFileName(i), i));
				}
				else
				{
					this.Games[i] = new GameSaveFile(GameLoad.GenerateGameFileName(i), i);
				}
				this.Games[i].MainData = this.SaveData.Games[i];
				if (this.SaveData.Checkpoints != null && this.SaveData.Checkpoints.Count > i)
				{
					this.Games[i].CheckpointData = this.SaveData.Checkpoints[i];
				}
				this.SaveGameDataToFile(i);
			}
		}
		this.ClearOldSaves();
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0005D738 File Offset: 0x0005B938
	private void ClearOldSaves()
	{
		if (this.SaveData == null)
		{
			return;
		}
		string contents = JsonUtility.ToJson(this.SaveData);
		if (!Directory.Exists(GameLoad.BackupsPath))
		{
			Directory.CreateDirectory(GameLoad.BackupsPath);
		}
		if (!File.Exists(GameLoad.OldGamesFileBackupPath))
		{
			File.Create(GameLoad.OldGamesFileBackupPath).Dispose();
		}
		File.WriteAllText(GameLoad.OldGamesFileBackupPath, contents);
		this.SaveData.Games.Clear();
		this.SaveData.Checkpoints.Clear();
		this.SaveMainDataToFile();
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0005D7C0 File Offset: 0x0005B9C0
	public void LoadMainGameData()
	{
		string text = "";
		string text2 = "";
		this.SaveData = new GlobalSaveData();
		this.SaveData.IsValid = true;
		if (File.Exists(GameLoad.MainSaveFilePath))
		{
			text = File.ReadAllText(GameLoad.MainSaveFilePath);
		}
		if (File.Exists(GameLoad.WritingFileBackupPath(GameLoad.MainSaveFilePath)))
		{
			text2 = File.ReadAllText(GameLoad.WritingFileBackupPath(GameLoad.MainSaveFilePath));
		}
		if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2))
		{
			Debug.Log("No save file nor backup found");
			return;
		}
		GlobalSaveData globalSaveData = new GlobalSaveData();
		GlobalSaveData globalSaveData2 = new GlobalSaveData();
		if (!string.IsNullOrEmpty(text))
		{
			if (this.SaveEdits)
			{
				string text3 = this.SaveEdits.DoSaveEdit(text);
				if (!string.IsNullOrEmpty(text3))
				{
					try
					{
						globalSaveData = JsonUtility.FromJson<GlobalSaveData>(text3);
					}
					catch (ArgumentException ex)
					{
						Debug.LogWarning("Edited save file was unreadable: " + ex.Message);
					}
				}
			}
			if (!globalSaveData.IsValid)
			{
				Debug.Log("loading true main data, edit is not valid :(");
				try
				{
					globalSaveData = JsonUtility.FromJson<GlobalSaveData>(text);
				}
				catch (ArgumentException ex2)
				{
					Debug.LogWarning("Main save file was unreadable: " + ex2.Message);
				}
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			try
			{
				globalSaveData2 = JsonUtility.FromJson<GlobalSaveData>(text2);
			}
			catch (ArgumentException ex3)
			{
				Debug.LogWarning("Backup save file was unreadable: " + ex3.Message);
			}
		}
		if (!globalSaveData.IsValid && !globalSaveData2.IsValid)
		{
			Debug.Log("No valid save nor backup files found");
			return;
		}
		if (globalSaveData.IsValid)
		{
			this.SaveData = globalSaveData;
			Debug.Log("Properly loaded main save data");
		}
		else
		{
			this.SaveData = globalSaveData2;
			Debug.Log("Had to load backup because main data was invalid");
		}
		this.DoUnlocksFixes();
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0005D97C File Offset: 0x0005BB7C
	public void CheckSteamAchievements()
	{
		if (this.SaveData.GlobalObjectives != null && this.SaveData.GlobalObjectives.Count > 0)
		{
			for (int i = 0; i < this.SaveData.GlobalObjectives.Count; i++)
			{
				Objective fromID = UniqueIDScriptable.GetFromID<Objective>(this.SaveData.GlobalObjectives[i]);
				if (fromID)
				{
					fromID.ForceComplete(true);
				}
			}
		}
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0005D9EC File Offset: 0x0005BBEC
	private void DoUnlocksFixes()
	{
		if (!this.SaveEdits)
		{
			return;
		}
		if (2 != this.SaveData.PerkUnlockFixVersion)
		{
			this.SaveUnlocksBackup(2, this.SaveData.UnlockedPerks);
		}
		if (this.SaveEdits.DoPerkUnlockFix(ref this.SaveData.PerkUnlockFixVersion, ref this.SaveData.UnlockedPerks))
		{
			this.SaveMainDataToFile();
		}
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0005DA54 File Offset: 0x0005BC54
	public void LoadOptions()
	{
		if (Screen.height < 50 || Screen.width < 50)
		{
			Screen.SetResolution(854, 480, false);
		}
		this.CurrentGameOptions = ScriptableObject.CreateInstance<GameOptions>();
		string text = "";
		if (File.Exists(GameLoad.OptionsFilePath))
		{
			text = File.ReadAllText(GameLoad.OptionsFilePath);
		}
		if (string.IsNullOrEmpty(text))
		{
			Debug.Log("No options file found");
			this.LoadDefaultOptions();
			return;
		}
		try
		{
			JsonUtility.FromJsonOverwrite(text, this.CurrentGameOptions);
		}
		catch (ArgumentException ex)
		{
			Debug.LogWarning("Options file was unreadable: " + ex.Message);
			this.LoadDefaultOptions();
		}
		FontsManager.SelectFontSet(this.CurrentGameOptions.CurrentFontSet);
		if (!this.CurrentGameOptions.UsingCustomLanguage)
		{
			this.CurrentGameOptions.SetLanguageToDefault();
		}
		if (!LocalizationManager.ValidLanguageIndex(this.CurrentGameOptions.CurrentLanguage))
		{
			this.CurrentGameOptions.CurrentLanguage = 0;
		}
		LocalizationManager.SetLanguage(this.CurrentGameOptions.CurrentLanguage, true);
		QualitySettings.vSyncCount = (this.CurrentGameOptions.VSync ? 1 : 0);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0005DB70 File Offset: 0x0005BD70
	public string GetSaveDataToString()
	{
		this.AutoSaveGame(false);
		return JsonUtility.ToJson(this.SaveData, false);
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0005DB88 File Offset: 0x0005BD88
	public string GetLogFileToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (File.Exists(Application.persistentDataPath + "/Player.log"))
		{
			stringBuilder.Append("Player.log-");
			stringBuilder.Append(File.ReadAllText(Application.persistentDataPath + "/Player.log"));
			stringBuilder.Append("-");
		}
		if (File.Exists(Application.persistentDataPath + "/Player-prev.log"))
		{
			stringBuilder.Append("Player-prev.log-");
			stringBuilder.Append(File.ReadAllText(Application.persistentDataPath + "/Player-prev.log"));
			stringBuilder.Append("-");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0005DC34 File Offset: 0x0005BE34
	public void LoadDefaultOptions()
	{
		if (!this.CurrentGameOptions)
		{
			this.CurrentGameOptions = ScriptableObject.CreateInstance<GameOptions>();
		}
		this.CurrentGameOptions.NormalizedMusicVolume = this.DefaultOptions.NormalizedMusicVolume;
		this.CurrentGameOptions.NormalizedAmbienceVolume = this.DefaultOptions.NormalizedAmbienceVolume;
		this.CurrentGameOptions.NormalizedSFXVolume = this.DefaultOptions.NormalizedSFXVolume;
		this.CurrentGameOptions.NormalizedMouseWheelSensitivity = this.DefaultOptions.NormalizedMouseWheelSensitivity;
		this.CurrentGameOptions.DisableSpecialEffects = this.DefaultOptions.DisableSpecialEffects;
		this.CurrentGameOptions.CurrentFontSet = this.DefaultOptions.CurrentFontSet;
		this.CurrentGameOptions.CurrentLanguage = this.DefaultOptions.CurrentLanguage;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0005DCF4 File Offset: 0x0005BEF4
	[ContextMenu("Test Options Save")]
	public void SaveOptions()
	{
		if (!this.CurrentGameOptions)
		{
			this.LoadDefaultOptions();
		}
		if (!this.CurrentGameOptions)
		{
			return;
		}
		string contents = JsonUtility.ToJson(this.CurrentGameOptions, true);
		if (!File.Exists(GameLoad.OptionsFilePath))
		{
			File.Create(GameLoad.OptionsFilePath).Dispose();
		}
		File.WriteAllText(GameLoad.OptionsFilePath, contents);
		Action onOptionsSaved = this.OnOptionsSaved;
		if (onOptionsSaved == null)
		{
			return;
		}
		onOptionsSaved();
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0005DD68 File Offset: 0x0005BF68
	public void SaveUnlocksBackup(int _Version, List<string> _Unlocks)
	{
		if (_Unlocks == null)
		{
			return;
		}
		if (_Unlocks.Count == 0)
		{
			return;
		}
		Debug.LogWarning("Saving Unlocked Perks Backup version " + _Version.ToString());
		UnlocksBugFixBackup unlocksBugFixBackup = new UnlocksBugFixBackup();
		unlocksBugFixBackup.BackupPerks = new List<string>();
		unlocksBugFixBackup.BackupPerks.AddRange(_Unlocks);
		string contents = JsonUtility.ToJson(unlocksBugFixBackup);
		if (!Directory.Exists(GameLoad.BackupsPath))
		{
			Directory.CreateDirectory(GameLoad.BackupsPath);
		}
		if (!File.Exists(GameLoad.UnlocksFixBackup(_Version)))
		{
			File.Create(GameLoad.UnlocksFixBackup(_Version)).Dispose();
		}
		File.WriteAllText(GameLoad.UnlocksFixBackup(_Version), contents);
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0005DDFC File Offset: 0x0005BFFC
	public void RestoreUnlocksBackup(int _Version)
	{
		Debug.LogWarning("Restoring Unlocked Perks from backup version " + _Version.ToString());
		string text = "";
		if (File.Exists(GameLoad.UnlocksFixBackup(_Version)))
		{
			text = File.ReadAllText(GameLoad.UnlocksFixBackup(_Version));
		}
		if (string.IsNullOrEmpty(text))
		{
			Debug.Log("No backup file found for version " + _Version.ToString());
			return;
		}
		try
		{
			UnlocksBugFixBackup unlocksBugFixBackup = JsonUtility.FromJson<UnlocksBugFixBackup>(text);
			if (unlocksBugFixBackup.BackupPerks != null && unlocksBugFixBackup.BackupPerks.Count != 0)
			{
				for (int i = 0; i < unlocksBugFixBackup.BackupPerks.Count; i++)
				{
					if (!this.SaveData.UnlockedPerks.Contains(unlocksBugFixBackup.BackupPerks[i]))
					{
						this.SaveData.UnlockedPerks.Add(unlocksBugFixBackup.BackupPerks[i]);
					}
				}
			}
		}
		catch (ArgumentException ex)
		{
			Debug.LogWarning("Backup file for version " + _Version.ToString() + " was unreadable: " + ex.Message);
		}
	}

	// Token: 0x0400101C RID: 4124
	public GameDataBase DataBase;

	// Token: 0x0400101D RID: 4125
	public SaveFileEditing SaveEdits;

	// Token: 0x0400101E RID: 4126
	public GlobalSaveData SaveData;

	// Token: 0x0400101F RID: 4127
	public GameOptions DefaultOptions;

	// Token: 0x04001020 RID: 4128
	public int MenuSceneIndex;

	// Token: 0x04001021 RID: 4129
	public int GameSceneIndex;

	// Token: 0x04001024 RID: 4132
	public static GameLoad Instance;

	// Token: 0x04001025 RID: 4133
	private GameManager GM;

	// Token: 0x04001026 RID: 4134
	public const int OfficialSaveSlots = 4;

	// Token: 0x04001027 RID: 4135
	public Action OnOptionsSaved;

	// Token: 0x04001028 RID: 4136
	public Action OnSaveFail;

	// Token: 0x04001029 RID: 4137
	private const int SharingViolation = 32;

	// Token: 0x0400102A RID: 4138
	private const int MaxTimeSaving = 3;

	// Token: 0x0400102B RID: 4139
	public const int CurrentNotesVersion = 17;

	// Token: 0x0400102C RID: 4140
	public const bool DemoVersion = false;

	// Token: 0x0400102D RID: 4141
	public const int DemoDaysLimit = 8;

	// Token: 0x0400102E RID: 4142
	private const uint GameSteamID = 1694420U;

	// Token: 0x0400102F RID: 4143
	private const uint DemoSteamID = 2147680U;

	// Token: 0x04001030 RID: 4144
	public const int RestoreUnlocksBackupVersion = -1;

	// Token: 0x04001031 RID: 4145
	[NonSerialized]
	public List<GameSaveFile> Games;
}
