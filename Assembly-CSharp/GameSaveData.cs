using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000183 RID: 387
[Serializable]
public class GameSaveData
{
	// Token: 0x06000A46 RID: 2630 RVA: 0x0005B664 File Offset: 0x00059864
	public GameSaveData Copy()
	{
		string json = JsonUtility.ToJson(this);
		GameSaveData result = null;
		try
		{
			result = JsonUtility.FromJson<GameSaveData>(json);
		}
		catch
		{
			return null;
		}
		return result;
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0005B69C File Offset: 0x0005989C
	public GameSaveData()
	{
		this.SaveDataVersion = 1;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x0005B77C File Offset: 0x0005997C
	public void CreateDicts()
	{
		for (int i = 0; i < this.AllStats.Count; i++)
		{
			if (!this.StatsDict.ContainsKey(UniqueIDScriptable.LoadID(this.AllStats[i].StatID)))
			{
				this.StatsDict.Add(UniqueIDScriptable.LoadID(this.AllStats[i].StatID), this.AllStats[i]);
			}
		}
		for (int j = 0; j < this.AllActions.Count; j++)
		{
			if (!this.ActionsDict.ContainsKey(UniqueIDScriptable.LoadID(this.AllActions[j].ActionID)))
			{
				this.ActionsDict.Add(UniqueIDScriptable.LoadID(this.AllActions[j].ActionID), this.AllActions[j]);
			}
		}
		for (int k = 0; k < this.AllObjectives.Count; k++)
		{
			if (!this.ObjectivesDict.ContainsKey(UniqueIDScriptable.LoadID(this.AllObjectives[k].UniqueID)))
			{
				this.ObjectivesDict.Add(UniqueIDScriptable.LoadID(this.AllObjectives[k].UniqueID), this.AllObjectives[k]);
			}
		}
		for (int l = 0; l < this.AllCounters.Count; l++)
		{
			if (!this.CountersDict.ContainsKey(UniqueIDScriptable.LoadID(this.AllCounters[l].ModelID)))
			{
				this.CountersDict.Add(UniqueIDScriptable.LoadID(this.AllCounters[l].ModelID), this.AllCounters[l]);
			}
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0005B921 File Offset: 0x00059B21
	public bool IsValidData
	{
		get
		{
			return 1 == this.SaveDataVersion;
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0005B92C File Offset: 0x00059B2C
	public bool HasCardsData
	{
		get
		{
			return this.CurrentEnvironmentCard != null && this.CurrentWeatherCard != null && !string.IsNullOrEmpty(this.CurrentEnvironmentCard.CardID) && !string.IsNullOrEmpty(this.CurrentWeatherCard.CardID) && !(UniqueIDScriptable.GetFromID(this.CurrentEnvironmentCard.CardID) == null) && !(UniqueIDScriptable.GetFromID(this.CurrentWeatherCard.CardID) == null);
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0005B9A4 File Offset: 0x00059BA4
	public bool HasStatsData
	{
		get
		{
			return this.AllStats != null && this.AllStats.Count != 0;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0005B9C0 File Offset: 0x00059BC0
	public bool HasEncounterData
	{
		get
		{
			return this.CurrentEncounter != null && !string.IsNullOrEmpty(this.CurrentEncounter.EncounterID);
		}
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0005B9E4 File Offset: 0x00059BE4
	public void SortCards()
	{
		if (this.EnvironmentsData == null)
		{
			return;
		}
		if (this.EnvironmentsData.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.EnvironmentsData.Count; i++)
		{
			this.EnvironmentsData[i].SortCards();
		}
	}

	// Token: 0x04000FCA RID: 4042
	private const int CurrentSaveDataVersion = 1;

	// Token: 0x04000FCB RID: 4043
	public int SaveDataVersion;

	// Token: 0x04000FCC RID: 4044
	public bool OpenedJournal;

	// Token: 0x04000FCD RID: 4045
	public bool OpenedGuide;

	// Token: 0x04000FCE RID: 4046
	public bool SafeMode;

	// Token: 0x04000FCF RID: 4047
	public int CurrentDayTimePoints;

	// Token: 0x04000FD0 RID: 4048
	public int CurrentMiniTicks;

	// Token: 0x04000FD1 RID: 4049
	public string DaytimeToHour;

	// Token: 0x04000FD2 RID: 4050
	public int CurrentDay;

	// Token: 0x04000FD3 RID: 4051
	public CardSaveData CurrentHandCard;

	// Token: 0x04000FD4 RID: 4052
	public CardSaveData CurrentEnvironmentCard;

	// Token: 0x04000FD5 RID: 4053
	public string PrevEnvironmentID;

	// Token: 0x04000FD6 RID: 4054
	public int CurrentTravelIndex;

	// Token: 0x04000FD7 RID: 4055
	public CardSaveData CurrentWeatherCard;

	// Token: 0x04000FD8 RID: 4056
	public CardSaveData CurrentEventCard;

	// Token: 0x04000FD9 RID: 4057
	public List<EnvironmentSaveData> EnvironmentsData = new List<EnvironmentSaveData>();

	// Token: 0x04000FDA RID: 4058
	public List<CardSaveData> CurrentCardsData = new List<CardSaveData>();

	// Token: 0x04000FDB RID: 4059
	public List<InventoryCardSaveData> CurrentInventoryCards = new List<InventoryCardSaveData>();

	// Token: 0x04000FDC RID: 4060
	public List<InventoryCardSaveData> CurrentNestedInventoryCards = new List<InventoryCardSaveData>();

	// Token: 0x04000FDD RID: 4061
	public List<StatSaveData> AllStats = new List<StatSaveData>();

	// Token: 0x04000FDE RID: 4062
	public List<InGameTickCounter> AllCounters = new List<InGameTickCounter>();

	// Token: 0x04000FDF RID: 4063
	public List<string> EncounteredEvents = new List<string>();

	// Token: 0x04000FE0 RID: 4064
	public List<string> EventCardQueue = new List<string>();

	// Token: 0x04000FE1 RID: 4065
	public List<string> CheckedBlueprints = new List<string>();

	// Token: 0x04000FE2 RID: 4066
	public List<string> PurchasableBlueprintCards = new List<string>();

	// Token: 0x04000FE3 RID: 4067
	public List<BlueprintResearchData> ResearchedBlueprintCards = new List<BlueprintResearchData>();

	// Token: 0x04000FE4 RID: 4068
	public string CurrentResearchedBlueprint;

	// Token: 0x04000FE5 RID: 4069
	public List<SelfTriggeredActionSaveData> AllActions = new List<SelfTriggeredActionSaveData>();

	// Token: 0x04000FE6 RID: 4070
	public List<ObjectiveSaveData> AllObjectives = new List<ObjectiveSaveData>();

	// Token: 0x04000FE7 RID: 4071
	public List<LogSaveData> AllEndgameLogs = new List<LogSaveData>();

	// Token: 0x04000FE8 RID: 4072
	public EncounterSaveData CurrentEncounter;

	// Token: 0x04000FE9 RID: 4073
	public string CurrentGamemode;

	// Token: 0x04000FEA RID: 4074
	public string CurrentCharacter;

	// Token: 0x04000FEB RID: 4075
	public PlayerCharacterSaveData CharacterData;

	// Token: 0x04000FEC RID: 4076
	public bool EasyPackage;

	// Token: 0x04000FED RID: 4077
	public List<string> ModifierPackages;

	// Token: 0x04000FEE RID: 4078
	public Dictionary<string, StatSaveData> StatsDict = new Dictionary<string, StatSaveData>();

	// Token: 0x04000FEF RID: 4079
	public Dictionary<string, SelfTriggeredActionSaveData> ActionsDict = new Dictionary<string, SelfTriggeredActionSaveData>();

	// Token: 0x04000FF0 RID: 4080
	public Dictionary<string, ObjectiveSaveData> ObjectivesDict = new Dictionary<string, ObjectiveSaveData>();

	// Token: 0x04000FF1 RID: 4081
	public Dictionary<string, InGameTickCounter> CountersDict = new Dictionary<string, InGameTickCounter>();
}
