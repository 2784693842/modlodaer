using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class GameManager : MBSingleton<GameManager>
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600002B RID: 43 RVA: 0x000044D4 File Offset: 0x000026D4
	public static InGameDraggableCard DraggedCard
	{
		get
		{
			if (GameManager.DraggedCardStack == null)
			{
				return null;
			}
			if (GameManager.DraggedCardStack.Count == 0)
			{
				return null;
			}
			return GameManager.DraggedCardStack[0];
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600002C RID: 44 RVA: 0x000044F8 File Offset: 0x000026F8
	public static float DragStackDuration
	{
		get
		{
			if (!MBSingleton<GameManager>.Instance)
			{
				return 0f;
			}
			if (GameManager.DraggedCardStack == null)
			{
				GameManager.DraggedCardStack = new List<InGameDraggableCard>();
			}
			if (GameManager.DraggedCardStack.Count < 3)
			{
				return MBSingleton<GameManager>.Instance.HoldActionDuration;
			}
			if (GameManager.DraggedCardStack.Count < 6)
			{
				return MBSingleton<GameManager>.Instance.HoldActionDuration / 2f;
			}
			if (GameManager.DraggedCardStack.Count < 9)
			{
				return MBSingleton<GameManager>.Instance.HoldActionDuration / 4f;
			}
			return MBSingleton<GameManager>.Instance.HoldActionDuration / 8f;
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00004590 File Offset: 0x00002790
	public static bool CardIsInDraggedStack(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (GameManager.DraggedCardStack == null)
		{
			GameManager.DraggedCardStack = new List<InGameDraggableCard>();
		}
		for (int i = 0; i < GameManager.DraggedCardStack.Count; i++)
		{
			if (GameManager.DraggedCardStack[i].gameObject == _Card.gameObject)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000045F0 File Offset: 0x000027F0
	public static List<InGameCardBase> DraggedStackResult()
	{
		List<InGameCardBase> list = new List<InGameCardBase>();
		if (GameManager.DraggedCardStack != null)
		{
			list.AddRange(GameManager.DraggedCardStack);
		}
		return list;
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600002F RID: 47 RVA: 0x00004616 File Offset: 0x00002816
	public static int DraggedCardsCount
	{
		get
		{
			if (GameManager.DraggedCardStack == null)
			{
				return 0;
			}
			return GameManager.DraggedCardStack.Count;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0000462C File Offset: 0x0000282C
	public static void AddCardToDraggedStack()
	{
		if (!GameManager.DraggedCard)
		{
			return;
		}
		if (!GameManager.DraggedCard.CurrentSlot)
		{
			return;
		}
		if (GameManager.DraggedCard.CurrentSlot.CardPileCount(false) <= 1)
		{
			return;
		}
		if (!GameManager.DraggedCard.CurrentSlot)
		{
			return;
		}
		if (GameManager.DraggedCardStack == null)
		{
			GameManager.DraggedCardStack = new List<InGameDraggableCard>();
		}
		InGameDraggableCard nextDraggableCard = GameManager.DraggedCard.CurrentSlot.NextDraggableCard;
		if (nextDraggableCard)
		{
			nextDraggableCard.OnBeginDrag(null);
			GameManager.DraggedCardStack.Add(nextDraggableCard);
			GameManager.DraggedCard.CurrentSlot.SortCardPileTransformsOnly();
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x000046C8 File Offset: 0x000028C8
	public static void ClearDraggedStack()
	{
		if (GameManager.DraggedCardStack == null)
		{
			return;
		}
		if (GameManager.DraggedCardStack.Count <= 1)
		{
			return;
		}
		for (int i = GameManager.DraggedCardStack.Count - 1; i >= 1; i--)
		{
			GameManager.DraggedCardStack[i].OnEndDrag(null);
			GameManager.DraggedCardStack.RemoveAt(i);
		}
		if (GameManager.DraggedCard && GameManager.DraggedCard.CurrentSlot)
		{
			GameManager.DraggedCard.CurrentSlot.SortCardPileTransformsOnly();
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x0000474A File Offset: 0x0000294A
	public static void BeginDragItem(InGameDraggableCard _Card)
	{
		if (GameManager.DraggedCardStack == null)
		{
			GameManager.DraggedCardStack = new List<InGameDraggableCard>();
		}
		GameManager.DraggedCardStack.Add(_Card);
		Action<InGameDraggableCard> onBeginDragItem = GameManager.OnBeginDragItem;
		if (onBeginDragItem == null)
		{
			return;
		}
		onBeginDragItem(_Card);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00004778 File Offset: 0x00002978
	public static void EndDragItem(InGameDraggableCard _Card)
	{
		if (GameManager.DraggedCardStack == null)
		{
			GameManager.DraggedCardStack = new List<InGameDraggableCard>();
		}
		GameManager.DraggedCardStack.Clear();
		Action<InGameDraggableCard> onEndDragItem = GameManager.OnEndDragItem;
		if (onEndDragItem != null)
		{
			onEndDragItem(_Card);
		}
		MBSingleton<GameManager>.Instance.GameGraphics.RefreshSlots(false);
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000034 RID: 52 RVA: 0x000047B6 File Offset: 0x000029B6
	public static GameStates CurrentState
	{
		get
		{
			return MBSingleton<GameManager>.Instance.CurrentGameState;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000035 RID: 53 RVA: 0x000047C2 File Offset: 0x000029C2
	// (set) Token: 0x06000036 RID: 54 RVA: 0x000047CA File Offset: 0x000029CA
	public int DayTimePoints { get; private set; }

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000037 RID: 55 RVA: 0x000047D3 File Offset: 0x000029D3
	// (set) Token: 0x06000038 RID: 56 RVA: 0x000047DB File Offset: 0x000029DB
	public int CurrentDay { get; private set; }

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000039 RID: 57 RVA: 0x000047E4 File Offset: 0x000029E4
	// (set) Token: 0x0600003A RID: 58 RVA: 0x000047EC File Offset: 0x000029EC
	public int CurrentMiniTicks { get; private set; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600003B RID: 59 RVA: 0x000047F5 File Offset: 0x000029F5
	// (set) Token: 0x0600003C RID: 60 RVA: 0x000047FD File Offset: 0x000029FD
	public int CurrentActionCounter { get; private set; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600003D RID: 61 RVA: 0x00004808 File Offset: 0x00002A08
	public int AutoSaveTicks
	{
		get
		{
			if (Application.isEditor)
			{
				if (this.EditorAutoSavesPerDay <= 0)
				{
					return -1;
				}
				return Mathf.Max(1, this.DaySettings.DailyPoints / this.EditorAutoSavesPerDay);
			}
			else
			{
				if (this.AutoSavesPerDay <= 0)
				{
					return -1;
				}
				return Mathf.Max(1, this.DaySettings.DailyPoints / this.AutoSavesPerDay);
			}
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600003E RID: 62 RVA: 0x00004863 File Offset: 0x00002A63
	// (set) Token: 0x0600003F RID: 63 RVA: 0x0000486B File Offset: 0x00002A6B
	public Vector3Int CurrentTickInfo { get; private set; }

	// Token: 0x06000040 RID: 64 RVA: 0x00004874 File Offset: 0x00002A74
	private void SaveTickInfo()
	{
		this.CurrentTickInfo = new Vector3Int(this.CurrentDay, this.DaySettings.DailyPoints - this.DayTimePoints, (this.CurrentDay - 1) * this.DaySettings.DailyPoints + (this.DaySettings.DailyPoints - this.DayTimePoints));
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000048CC File Offset: 0x00002ACC
	public void SetTimeTo(int _Day, int _Tick)
	{
		if (this.DayTimePoints != this.DaySettings.DailyPoints - _Tick)
		{
			this.CurrentActionCounter = 0;
		}
		this.CurrentDay = _Day;
		this.DayTimePoints = this.DaySettings.DailyPoints - _Tick;
		this.SaveTickInfo();
		this.GameGraphics.UpdateTimeInfo(true);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00004924 File Offset: 0x00002B24
	public static string TickInfoToString(Vector3Int _Tick)
	{
		if (MBSingleton<GameManager>.Instance == null)
		{
			return "";
		}
		if (_Tick.y >= MBSingleton<GameManager>.Instance.DaySettings.DailyPoints)
		{
			return string.Concat(new string[]
			{
				"Day ",
				_Tick.x.ToString(),
				", Tick ",
				_Tick.y.ToString(),
				" (NIGHT)"
			});
		}
		return "Day " + _Tick.x.ToString() + ", Tick " + _Tick.y.ToString();
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000049D4 File Offset: 0x00002BD4
	public static int GetMiniTicksAmt(MiniTicksBehavior _Behavior)
	{
		if (!MBSingleton<GameManager>.Instance)
		{
			return 0;
		}
		if (!MBSingleton<GameManager>.Instance.DaySettings.UseMiniTicks)
		{
			return 0;
		}
		switch (_Behavior)
		{
		case MiniTicksBehavior.DefaultBehavior:
			if (MBSingleton<GameManager>.Instance.DaySettings.DefaultMiniTicksBehavior != MiniTicksBehavior.DefaultBehavior)
			{
				return GameManager.GetMiniTicksAmt(MBSingleton<GameManager>.Instance.DaySettings.DefaultMiniTicksBehavior);
			}
			return GameManager.GetMiniTicksAmt(MiniTicksBehavior.KeepsZeroCost);
		case MiniTicksBehavior.CostsAMiniTick:
			return 1;
		}
		return 0;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00004A46 File Offset: 0x00002C46
	public static float TickToHours(int _TickAmt, int _MiniTicksAmt)
	{
		if (!MBSingleton<GameManager>.Instance)
		{
			return (float)_TickAmt;
		}
		return MBSingleton<GameManager>.Instance.DaySettings.PointToHours * (float)_TickAmt + MBSingleton<GameManager>.Instance.DaySettings.MiniTicksToHours * (float)_MiniTicksAmt;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00004A7C File Offset: 0x00002C7C
	public static int HoursToTick(float _Hours)
	{
		if (!MBSingleton<GameManager>.Instance)
		{
			return Mathf.FloorToInt(_Hours);
		}
		return Mathf.FloorToInt(_Hours / MBSingleton<GameManager>.Instance.DaySettings.PointToHours);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00004AA7 File Offset: 0x00002CA7
	public static int DaysToTicks(float _Days)
	{
		return GameManager.HoursToTick(_Days * 24f);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00004AB5 File Offset: 0x00002CB5
	public static float HourOfTheDayValue(int _Ticks, int _MiniTicks)
	{
		return (float)MBSingleton<GameManager>.Instance.DaySettings.DayStartingHour + GameManager.TickToHours(MBSingleton<GameManager>.Instance.DaySettings.DailyPoints, 0) - GameManager.TickToHours(_Ticks, _MiniTicks);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00004AE8 File Offset: 0x00002CE8
	public static string CurrentHourOfDayString()
	{
		if (MBSingleton<GameManager>.Instance)
		{
			return GameManager.TotalTicksToHourOfTheDayString(GameManager.HoursToTick((float)MBSingleton<GameManager>.Instance.DaySettings.DayStartingHour) + MBSingleton<GameManager>.Instance.DayTimePoints, MBSingleton<GameManager>.Instance.CurrentMiniTicks);
		}
		return GameManager.TotalTicksToHourOfTheDayString(0, 0);
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00004B38 File Offset: 0x00002D38
	public static string TotalTicksToHourOfTheDayString(int _Ticks, int _MiniTicks)
	{
		return HoursDisplay.HoursToTimeOfDay(GameManager.TickToHours(_Ticks, _MiniTicks));
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600004A RID: 74 RVA: 0x00004B46 File Offset: 0x00002D46
	// (set) Token: 0x0600004B RID: 75 RVA: 0x00004B4E File Offset: 0x00002D4E
	public RectTransform DraggingPlane { get; private set; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600004C RID: 76 RVA: 0x00004B57 File Offset: 0x00002D57
	// (set) Token: 0x0600004D RID: 77 RVA: 0x00004B5F File Offset: 0x00002D5F
	public Transform CardStackDraggingTr { get; private set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600004E RID: 78 RVA: 0x00004B68 File Offset: 0x00002D68
	// (set) Token: 0x0600004F RID: 79 RVA: 0x00004B70 File Offset: 0x00002D70
	public CardData PrevEnvironment { get; private set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000050 RID: 80 RVA: 0x00004B79 File Offset: 0x00002D79
	// (set) Token: 0x06000051 RID: 81 RVA: 0x00004B81 File Offset: 0x00002D81
	public int CurrentTravelIndex { get; private set; }

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000052 RID: 82 RVA: 0x00004B8A File Offset: 0x00002D8A
	// (set) Token: 0x06000053 RID: 83 RVA: 0x00004B92 File Offset: 0x00002D92
	public int NextTravelIndex { get; private set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000054 RID: 84 RVA: 0x00004B9B File Offset: 0x00002D9B
	// (set) Token: 0x06000055 RID: 85 RVA: 0x00004BA3 File Offset: 0x00002DA3
	public bool LeavingEnvironment { get; private set; }

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000056 RID: 86 RVA: 0x00004BAC File Offset: 0x00002DAC
	// (set) Token: 0x06000057 RID: 87 RVA: 0x00004BB4 File Offset: 0x00002DB4
	public bool EnvironmentTransition { get; private set; }

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000058 RID: 88 RVA: 0x00004BBD File Offset: 0x00002DBD
	// (set) Token: 0x06000059 RID: 89 RVA: 0x00004BC5 File Offset: 0x00002DC5
	public bool IsInitializing { get; private set; }

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600005A RID: 90 RVA: 0x00004BCE File Offset: 0x00002DCE
	// (set) Token: 0x0600005B RID: 91 RVA: 0x00004BD6 File Offset: 0x00002DD6
	public bool CardsLoaded { get; private set; }

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x0600005C RID: 92 RVA: 0x00004BDF File Offset: 0x00002DDF
	// (set) Token: 0x0600005D RID: 93 RVA: 0x00004BE7 File Offset: 0x00002DE7
	public bool IsCatchingUp { get; private set; }

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600005E RID: 94 RVA: 0x00004BF0 File Offset: 0x00002DF0
	public EnvironmentSaveData LocalCountersEnv
	{
		get
		{
			if (this.CatchingUpEnvData != null)
			{
				return this.CatchingUpEnvData;
			}
			return this.CurrentEnvData(false);
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00004C08 File Offset: 0x00002E08
	public EnvironmentSaveData CurrentEnvData(bool _CreateIfNull)
	{
		return this.GetEnvSaveData(this.CurrentEnvironment, this.PrevEnvironment, this.CurrentTravelIndex, _CreateIfNull);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00004C24 File Offset: 0x00002E24
	public EnvironmentSaveData GetEnvSaveData(CardData _Env, CardData _PrevEnv, int _Index, bool _CreateIfNull)
	{
		if (this.EnvironmentsData == null || _Env == null)
		{
			return null;
		}
		string text = _Env.EnvironmentDictionaryKey(_PrevEnv, _Index);
		if (!this.EnvironmentsData.ContainsKey(text))
		{
			if (!_CreateIfNull)
			{
				return null;
			}
			this.EnvironmentsData.Add(text, new EnvironmentSaveData(_Env, this.CurrentTickInfo.z, UniqueIDScriptable.AddNamesToComplexID(text)));
			this.EnvironmentsData[text].CurrentMaxWeight = _Env.GetWeightCapacity(0f);
			this.EnvironmentsData[text].FillCounters(this.AllCounters);
			if (_Env.DefaultEnvCards != null)
			{
				for (int i = 0; i < _Env.DefaultEnvCards.Length; i++)
				{
					if (!(_Env.DefaultEnvCards[i] == null))
					{
						this.CreateCardAsSaveData(_Env.DefaultEnvCards[i], _Env, text, null, null, true);
					}
				}
			}
		}
		return this.EnvironmentsData[text];
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000061 RID: 97 RVA: 0x00004D07 File Offset: 0x00002F07
	// (set) Token: 0x06000062 RID: 98 RVA: 0x00004D0F File Offset: 0x00002F0F
	public bool NotInBase { get; private set; }

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000063 RID: 99 RVA: 0x00004D18 File Offset: 0x00002F18
	// (set) Token: 0x06000064 RID: 100 RVA: 0x00004D20 File Offset: 0x00002F20
	public CardAction CurrentOutOfBaseAction { get; private set; }

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000065 RID: 101 RVA: 0x00004D29 File Offset: 0x00002F29
	// (set) Token: 0x06000066 RID: 102 RVA: 0x00004D31 File Offset: 0x00002F31
	public CardAction RootAction { get; private set; }

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000067 RID: 103 RVA: 0x00004D3A File Offset: 0x00002F3A
	// (set) Token: 0x06000068 RID: 104 RVA: 0x00004D42 File Offset: 0x00002F42
	public CardAction EventAction { get; private set; }

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000069 RID: 105 RVA: 0x00004D4B File Offset: 0x00002F4B
	public static bool PerformingAction
	{
		get
		{
			return MBSingleton<GameManager>.Instance && MBSingleton<GameManager>.Instance.RootAction != null;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600006A RID: 106 RVA: 0x00004D68 File Offset: 0x00002F68
	public static bool DontRenameGOs
	{
		get
		{
			return !MBSingleton<GameManager>.Instance || MBSingleton<GameManager>.Instance.DontRenameGameObjects;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600006B RID: 107 RVA: 0x00004D82 File Offset: 0x00002F82
	public bool HasContentOpen
	{
		get
		{
			return this.OpenContent != null;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x0600006C RID: 108 RVA: 0x00004D90 File Offset: 0x00002F90
	// (set) Token: 0x0600006D RID: 109 RVA: 0x00004D98 File Offset: 0x00002F98
	public bool CheckForPassiveEffects { get; private set; }

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x0600006E RID: 110 RVA: 0x00004DA1 File Offset: 0x00002FA1
	// (set) Token: 0x0600006F RID: 111 RVA: 0x00004DA9 File Offset: 0x00002FA9
	public bool WillCheckForPassiveEffects { get; private set; }

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000070 RID: 112 RVA: 0x00004DB2 File Offset: 0x00002FB2
	public GameSaveData CurrentSaveData
	{
		get
		{
			return this.CurrentGameData;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000071 RID: 113 RVA: 0x00004DBA File Offset: 0x00002FBA
	public bool IsSafeMode
	{
		get
		{
			return this.CurrentSaveData.SafeMode;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000072 RID: 114 RVA: 0x00004DC7 File Offset: 0x00002FC7
	public List<LogSaveData> GetSavedLogs
	{
		get
		{
			if (this.CurrentGameData == null)
			{
				return new List<LogSaveData>();
			}
			if (this.CurrentGameData.AllEndgameLogs == null)
			{
				return new List<LogSaveData>();
			}
			return this.CurrentGameData.AllEndgameLogs;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000073 RID: 115 RVA: 0x00004DF5 File Offset: 0x00002FF5
	// (set) Token: 0x06000074 RID: 116 RVA: 0x00004E00 File Offset: 0x00003000
	public bool PoolCards
	{
		get
		{
			return !this.DontPoolCards;
		}
		set
		{
			this.DontPoolCards = !value;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000075 RID: 117 RVA: 0x00004E0C File Offset: 0x0000300C
	public ContentDisplayer GetJournal
	{
		get
		{
			if (GameManager.CurrentPlayerCharacter == null)
			{
				return this.DefaultJournal;
			}
			if (GameManager.CurrentPlayerCharacter.Journal)
			{
				return GameManager.CurrentPlayerCharacter.Journal;
			}
			return this.DefaultJournal;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000076 RID: 118 RVA: 0x00004E44 File Offset: 0x00003044
	public ContentDisplayer GetGuide
	{
		get
		{
			if (GameManager.CurrentPlayerCharacter == null)
			{
				return this.DefaultGuide;
			}
			if (GameManager.CurrentPlayerCharacter.Guide)
			{
				return GameManager.CurrentPlayerCharacter.Guide;
			}
			return this.DefaultGuide;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000077 RID: 119 RVA: 0x00004E7C File Offset: 0x0000307C
	public bool JournalIsOpen
	{
		get
		{
			return this.Journal && this.Journal.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000078 RID: 120 RVA: 0x00004E9D File Offset: 0x0000309D
	public bool GuideIsOpen
	{
		get
		{
			return this.Guide && this.Guide.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000079 RID: 121 RVA: 0x00004EBE File Offset: 0x000030BE
	public bool OpenedJournal
	{
		get
		{
			return this.CurrentGameData != null && this.CurrentGameData.OpenedJournal;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x0600007A RID: 122 RVA: 0x00004ED5 File Offset: 0x000030D5
	public CardData CurrentEnvironment
	{
		get
		{
			if (!this.CurrentEnvironmentCard)
			{
				return this.NextEnvironment;
			}
			return this.CurrentEnvironmentCard.CardModel;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600007B RID: 123 RVA: 0x00004EF6 File Offset: 0x000030F6
	public float MaxEnvWeight
	{
		get
		{
			if (this.CurrentEnvironmentCard)
			{
				return this.CurrentEnvironmentCard.MaxWeightCapacity;
			}
			return 0f;
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00004F16 File Offset: 0x00003116
	public void AddActionModifier(ActionModifier _Mod, string _Source)
	{
		_Mod.SetSource(_Source);
		this.CurrentActionModifiers.Add(_Mod);
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600007D RID: 125 RVA: 0x00004F2B File Offset: 0x0000312B
	public bool AnyActionBlockers
	{
		get
		{
			return this.CurrentActionBlockers.Count > 0;
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00004F3C File Offset: 0x0000313C
	public bool CheckActionBlockers(CardAction _ForAction, out string _Message)
	{
		if (!string.IsNullOrEmpty(_ForAction.ActionBlockedMessage))
		{
			_Message = _ForAction.ActionBlockedMessage;
			return false;
		}
		if (this.CurrentActionBlockers.Count == 0)
		{
			_Message = "";
			return true;
		}
		bool result = true;
		int num = _ForAction.TotalDaytimeCost;
		if (num <= 0)
		{
			num = _ForAction.UnmodifiedDaytimeCost;
		}
		for (int i = 0; i < this.CurrentActionBlockers.Count; i++)
		{
			if (this.CurrentActionBlockers[i].BlockThreshold <= num)
			{
				result = false;
				if (!string.IsNullOrEmpty(this.CurrentActionBlockers[i].BlockedMessage))
				{
					_Message = this.CurrentActionBlockers[i].BlockedMessage;
					return false;
				}
			}
		}
		_Message = "";
		return result;
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600007F RID: 127 RVA: 0x00004FEB File Offset: 0x000031EB
	public static int CharacterDifficultyScore
	{
		get
		{
			if (!GameManager.CurrentPlayerCharacter)
			{
				return 0;
			}
			return GameManager.CurrentPlayerCharacter.CharacterScore;
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005005 File Offset: 0x00003205
	public void CloseJournal()
	{
		if (this.Journal)
		{
			this.GameSounds.PerformSingleSound(this.Journal.ClosingSound, true, false);
			this.Journal.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00005040 File Offset: 0x00003240
	public void OpenJournal()
	{
		this.GameGraphics.CloseAllPopups();
		if (this.Journal)
		{
			return;
		}
		this.Journal = UnityEngine.Object.Instantiate<ContentDisplayer>(this.GetJournal, this.GameGraphics.SpecialInspectionPopupParent);
		if (this.Journal)
		{
			this.GameSounds.PerformSingleSound(this.Journal.OpeningSound, true, false);
			this.Journal.gameObject.SetActive(true);
			this.OpenWindow = this.Journal.gameObject;
			this.OpenContent = this.Journal;
			this.CurrentGameData.OpenedJournal = true;
			MBSingleton<TutorialManager>.Instance.UpdateTutorials();
			base.StartCoroutine(this.JournalOpened());
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000050F8 File Offset: 0x000032F8
	public void OpenEndgameJournal(bool _GameOver)
	{
		MBSingleton<EndgameMenu>.Instance.Setup("I cheated to check the journal", 0, 0, this.CurrentGameData, _GameOver, true, !_GameOver, false, true);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00005124 File Offset: 0x00003324
	private IEnumerator JournalOpened()
	{
		while (this.Journal.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(this.Journal.gameObject);
		this.Journal = null;
		this.OpenWindow = null;
		this.OpenContent = null;
		yield break;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00005134 File Offset: 0x00003334
	public void CloseGuide()
	{
		if (this.Guide && this.Guide.gameObject.activeInHierarchy)
		{
			this.GameSounds.PerformSingleSound(this.Guide.ClosingSound, true, false);
			this.Guide.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0000518C File Offset: 0x0000338C
	public void OpenGuide()
	{
		this.GameGraphics.CloseAllPopups();
		if (this.Guide)
		{
			if (this.Guide.gameObject.activeInHierarchy)
			{
				return;
			}
		}
		else
		{
			this.Guide = UnityEngine.Object.Instantiate<ContentDisplayer>(this.GetGuide, this.GameGraphics.SpecialInspectionPopupParent);
		}
		if (this.Guide)
		{
			this.GameSounds.PerformSingleSound(this.Guide.OpeningSound, true, false);
			this.Guide.gameObject.SetActive(true);
			this.OpenWindow = this.Guide.gameObject;
			this.OpenContent = this.Guide;
			this.CurrentGameData.OpenedGuide = true;
			MBSingleton<TutorialManager>.Instance.UpdateTutorials();
			base.StartCoroutine(this.GuideOpened());
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00005256 File Offset: 0x00003456
	public void OpenGuide(ContentPage _OnPage)
	{
		if (!_OnPage)
		{
			return;
		}
		this.OpenGuide();
		if (this.Guide)
		{
			this.Guide.OpenPage(_OnPage);
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00005280 File Offset: 0x00003480
	private IEnumerator GuideOpened()
	{
		while (this.Guide.gameObject.activeInHierarchy)
		{
			yield return null;
		}
		this.OpenWindow = null;
		this.OpenContent = null;
		yield break;
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000088 RID: 136 RVA: 0x0000528F File Offset: 0x0000348F
	public bool NewBlueprints
	{
		get
		{
			return this.BlueprintModelCards.Count != this.CheckedBlueprints.Count;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000089 RID: 137 RVA: 0x000052AC File Offset: 0x000034AC
	public bool NewImprovements
	{
		get
		{
			if (this.ImprovementCards == null)
			{
				return false;
			}
			if (this.ImprovementCards.Count == 0)
			{
				return false;
			}
			EnvironmentSaveData environmentSaveData = this.CurrentEnvData(true);
			for (int i = 0; i < this.ImprovementCards.Count; i++)
			{
				if (!environmentSaveData.ImprovementWasChecked(this.ImprovementCards[i].CardModel))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0000530C File Offset: 0x0000350C
	public void CheckBlueprints(CardTabGroup _TabFilter, bool _OnlyAvailable)
	{
		if (!_TabFilter)
		{
			for (int i = 0; i < this.BlueprintModelCards.Count; i++)
			{
				if ((!_OnlyAvailable || this.BlueprintModelStates[this.BlueprintModelCards[i]] == BlueprintModelState.Available) && !this.CheckedBlueprints.Contains(this.BlueprintModelCards[i]))
				{
					this.CheckedBlueprints.Add(this.BlueprintModelCards[i]);
				}
			}
			return;
		}
		for (int j = 0; j < this.BlueprintModelCards.Count; j++)
		{
			if ((!_OnlyAvailable || this.BlueprintModelStates[this.BlueprintModelCards[j]] == BlueprintModelState.Available) && !this.CheckedBlueprints.Contains(this.BlueprintModelCards[j]) && _TabFilter.IncludedCards.Contains(this.BlueprintModelCards[j]))
			{
				this.CheckedBlueprints.Add(this.BlueprintModelCards[j]);
			}
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00005400 File Offset: 0x00003600
	public void CheckImprovements(CardData _ExplorationCard)
	{
		if (!_ExplorationCard)
		{
			return;
		}
		if (_ExplorationCard.CardType != CardTypes.Explorable)
		{
			return;
		}
		if (!_ExplorationCard.HasImprovements)
		{
			return;
		}
		EnvironmentSaveData environmentSaveData = this.CurrentEnvData(true);
		for (int i = 0; i < this.ImprovementCards.Count; i++)
		{
			if (this.ImprovementCards[i] && _ExplorationCard.HasImprovement(this.ImprovementCards[i].CardModel))
			{
				environmentSaveData.CheckImprovement(this.ImprovementCards[i].CardModel);
			}
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x0000548A File Offset: 0x0000368A
	public void OpenLogFileLocation()
	{
		OpenLogLocation.OpenLog();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00005494 File Offset: 0x00003694
	private void Update()
	{
		if (!GameManager.DraggedCard)
		{
			return;
		}
		if (!GameManager.HoveredCard)
		{
			this.DragStackTimer = 0f;
			if (GameManager.DraggedCardStack.Count > 1)
			{
				this.CancelDragStackTimer += Time.deltaTime;
				if (this.CancelDragStackTimer >= this.HoldActionDuration / 2f)
				{
					GameManager.ClearDraggedStack();
				}
			}
			return;
		}
		this.CancelDragStackTimer = 0f;
		if (GameManager.HoveredCard.DragStackCompatible && this.CanUseDragStacks)
		{
			this.DragStackTimer += Time.deltaTime;
			float dragStackDuration = GameManager.DragStackDuration;
			if (this.DragStackTimer >= dragStackDuration)
			{
				this.DragStackTimer -= dragStackDuration;
				GameManager.AddCardToDraggedStack();
			}
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00005551 File Offset: 0x00003751
	private void LateUpdate()
	{
		if (this.ObjectivesUpdateRequested)
		{
			this.UpdateObjectivesCompletion();
			this.ObjectivesUpdateRequested = false;
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00005568 File Offset: 0x00003768
	private void Awake()
	{
		if (!GameLoad.Instance)
		{
			return;
		}
		this.IsInitializing = true;
		this.DontCheckObjectivesYet = true;
		if (this.SuccessChances)
		{
			this.SuccessChances.Init();
		}
		if (this.DraggingTr)
		{
			this.CardStackDraggingTr = new GameObject("DragStack").transform;
			this.CardStackDraggingTr.SetParent(this.DraggingTr);
			this.CardStackDraggingTr.localPosition = Vector3.zero;
			this.CardStackDraggingTr.localScale = Vector3.one;
		}
		if (GameManager.DraggedCardStack == null)
		{
			GameManager.DraggedCardStack = new List<InGameDraggableCard>();
		}
		else
		{
			GameManager.DraggedCardStack.Clear();
		}
		if (GameLoad.Instance.Games.Count >= GameLoad.Instance.CurrentGameDataIndex && GameLoad.Instance.CurrentGameDataIndex >= 0)
		{
			this.CurrentGameData = GameLoad.Instance.Games[GameLoad.Instance.CurrentGameDataIndex].MainData;
		}
		else
		{
			this.CurrentGameData = new GameSaveData();
		}
		if (!this.CurrentGameData.IsValidData)
		{
			this.CurrentGameData = new GameSaveData();
		}
		this.CurrentGameOptions = GameLoad.Instance.CurrentGameOptions;
		if (!string.IsNullOrEmpty(this.CurrentGameData.CurrentGamemode))
		{
			if (this.CurrentGameData.CurrentGamemode == "Null")
			{
				GameManager.CurrentGamemode = null;
			}
			else
			{
				UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(UniqueIDScriptable.LoadID(this.CurrentGameData.CurrentGamemode));
				if (fromID && fromID is Gamemode)
				{
					GameManager.CurrentGamemode = (fromID as Gamemode);
				}
			}
		}
		bool flag = false;
		if (this.CurrentGameData.CharacterData != null && this.CurrentGameData.CharacterData.IsValid)
		{
			GameManager.CurrentPlayerCharacter = ScriptableObject.CreateInstance<PlayerCharacter>();
			GameManager.CurrentPlayerCharacter.LoadCustomCharacter(this.CurrentGameData.CharacterData, this.Portraits.GetCharacterPortrait(this.CurrentGameData.CharacterData.PortraitID));
			flag = true;
		}
		if (!string.IsNullOrEmpty(this.CurrentGameData.CurrentCharacter) && !flag)
		{
			if (this.CurrentGameData.CurrentCharacter == "Null")
			{
				GameManager.CurrentPlayerCharacter = null;
			}
			else
			{
				UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(UniqueIDScriptable.LoadID(this.CurrentGameData.CurrentCharacter));
				if (fromID && fromID is PlayerCharacter)
				{
					GameManager.CurrentPlayerCharacter = (fromID as PlayerCharacter);
				}
			}
		}
		if (this.CurrentGameData.ModifierPackages != null && this.CurrentGameData.ModifierPackages.Count > 0)
		{
			if (GameManager.CurrentModifierPackages == null)
			{
				GameManager.CurrentModifierPackages = new List<GameModifierPackage>();
			}
			else
			{
				GameManager.CurrentModifierPackages.Clear();
			}
			for (int i = 0; i < this.CurrentGameData.ModifierPackages.Count; i++)
			{
				UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(UniqueIDScriptable.LoadID(this.CurrentGameData.ModifierPackages[i]));
				if (fromID && fromID is GameModifierPackage)
				{
					GameManager.CurrentModifierPackages.Add(fromID as GameModifierPackage);
				}
			}
		}
		if (GameManager.CurrentGamemode)
		{
			Debug.Log("Current Gamemode: " + GameManager.CurrentGamemode.name);
		}
		else
		{
			Debug.Log("Current Gamemode: NULL");
		}
		if (GameManager.CurrentPlayerCharacter)
		{
			Debug.Log("Current Character: " + GameManager.CurrentPlayerCharacter.name);
		}
		else
		{
			Debug.Log("Current Character: NULL");
		}
		if (this.CurrentGameData.EncounteredEvents != null && this.CurrentGameData.EncounteredEvents.Count > 0)
		{
			for (int j = 0; j < this.CurrentGameData.EncounteredEvents.Count; j++)
			{
				UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(UniqueIDScriptable.LoadID(this.CurrentGameData.EncounteredEvents[j]));
				if (fromID && fromID is CardData && !this.EncounteredEvents.Contains(fromID as CardData))
				{
					this.EncounteredEvents.Add(fromID as CardData);
				}
			}
		}
		if (this.CurrentGameData.EventCardQueue != null && this.CurrentGameData.EventCardQueue.Count > 0)
		{
			for (int k = 0; k < this.CurrentGameData.EventCardQueue.Count; k++)
			{
				UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(UniqueIDScriptable.LoadID(this.CurrentGameData.EventCardQueue[k]));
				if (fromID && fromID is CardData)
				{
					this.EventCardQueue.Add(fromID as CardData);
				}
			}
		}
		if (this.CurrentGameData.CheckedBlueprints != null && this.CurrentGameData.CheckedBlueprints.Count > 0)
		{
			for (int l = 0; l < this.CurrentGameData.CheckedBlueprints.Count; l++)
			{
				UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(UniqueIDScriptable.LoadID(this.CurrentGameData.CheckedBlueprints[l]));
				if (fromID && fromID is CardData)
				{
					this.CheckedBlueprints.Add(fromID as CardData);
				}
			}
		}
		if (this.DraggingTr)
		{
			this.DraggingPlane = (this.DraggingTr.parent as RectTransform);
		}
		this.GameSounds = MBSingleton<SoundManager>.Instance;
		this.GameSounds.Init();
		this.GameGraphics = MBSingleton<GraphicsManager>.Instance;
		this.GameGraphics.Init();
		this.CurrentDay = Mathf.Max(this.CurrentGameData.CurrentDay, 1);
		this.DayTimePoints = ((this.CurrentGameData.CurrentDayTimePoints == 0) ? this.DaySettings.DailyPoints : this.CurrentGameData.CurrentDayTimePoints);
		if (this.DaySettings.UseMiniTicks)
		{
			this.CurrentMiniTicks = Mathf.Clamp(this.CurrentGameData.CurrentMiniTicks, 0, this.DaySettings.MiniTicksPerTick - 1);
		}
		else
		{
			this.CurrentMiniTicks = 0;
		}
		if (GameManager.CurrentPlayerCharacter && this.CurrentGameData.CurrentDayTimePoints == 0 && this.CurrentDay == 1)
		{
			int m = GameManager.CurrentPlayerCharacter.AddedHours;
			if (m > 0)
			{
				while (m >= 24)
				{
					int currentDay = this.CurrentDay;
					this.CurrentDay = currentDay + 1;
					m -= 24;
				}
				this.DayTimePoints += -GameManager.HoursToTick((float)m);
			}
		}
		this.SaveTickInfo();
		this.GameGraphics.UpdateTimeInfo(true);
		this.GameStatsParent = new GameObject("Stats").transform;
		this.GameStatsParent.SetParent(base.transform.parent);
		CardData fromID2;
		if (this.CurrentGameData.PurchasableBlueprintCards != null && this.CurrentGameData.PurchasableBlueprintCards.Count > 0)
		{
			for (int n = 0; n < this.CurrentGameData.PurchasableBlueprintCards.Count; n++)
			{
				fromID2 = UniqueIDScriptable.GetFromID<CardData>(this.CurrentGameData.PurchasableBlueprintCards[n]);
				if (fromID2 && !this.PurchasableBlueprintCards.Contains(fromID2))
				{
					this.PurchasableBlueprintCards.Add(fromID2);
				}
			}
		}
		if (this.CurrentSaveData.ResearchedBlueprintCards != null && this.CurrentSaveData.ResearchedBlueprintCards.Count > 0)
		{
			for (int num = 0; num < this.CurrentSaveData.ResearchedBlueprintCards.Count; num++)
			{
				fromID2 = UniqueIDScriptable.GetFromID<CardData>(this.CurrentSaveData.ResearchedBlueprintCards[num].BlueprintID);
				if (fromID2 && !this.BlueprintResearchTimes.ContainsKey(fromID2))
				{
					this.BlueprintResearchTimes.Add(fromID2, this.CurrentSaveData.ResearchedBlueprintCards[num].TickCounter);
				}
			}
		}
		fromID2 = UniqueIDScriptable.GetFromID<CardData>(this.CurrentSaveData.CurrentResearchedBlueprint);
		if (fromID2)
		{
			this.GameGraphics.BlueprintModelsPopup.LoadResearchedBlueprint(fromID2);
		}
		this.InitializeStatsAndActions();
		if (!this.CurrentGameData.HasCardsData)
		{
			this.InitializeDefaultCards();
		}
		else
		{
			this.LoadCards();
			this.InitializePlayerCharacter(true);
			if (GameManager.CurrentGamemode)
			{
				this.GetStartingCardsFromArray(GameManager.CurrentGamemode.BaseAndItemCards, true);
			}
		}
		this.CheckForPassiveEffects = true;
		base.StartCoroutine(this.UpdatePassiveEffects());
		this.GameGraphics.LoadBookmarks(this.CurrentEnvData(false));
		this.GameGraphics.CollectVisibleStats();
		base.StartCoroutine(this.FinishInitializing());
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00005D73 File Offset: 0x00003F73
	private void OnApplicationQuit()
	{
		this.QuitGame();
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00005D7C File Offset: 0x00003F7C
	public void QuitGame()
	{
		if (this.AllObjectives != null && this.AllObjectives.Count > 0)
		{
			for (int i = 0; i < this.AllObjectives.Count; i++)
			{
				this.AllObjectives[i].OnGameQuit();
			}
		}
		if (this.AllPerks != null && this.AllPerks.Count > 0)
		{
			for (int j = 0; j < this.AllPerks.Count; j++)
			{
				this.AllPerks[j].OnGameQuit();
			}
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00005E03 File Offset: 0x00004003
	private IEnumerator FinishInitializing()
	{
		if (!this.CurrentGameData.HasCardsData && !this.CurrentGameData.HasStatsData)
		{
			this.AutoSolveEvents = true;
		}
		this.DontCheckObjectivesYet = false;
		CoroutineController controller;
		this.StartCoroutineEx(this.SpendDaytimePoints(0, true, false, false, null, FadeToBlackTypes.None, "", false, false, null, null, null), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		this.AutoSolveEvents = false;
		if (this.CurrentGameData.HasEncounterData)
		{
			Encounter fromID = UniqueIDScriptable.GetFromID<Encounter>(this.CurrentGameData.CurrentEncounter.EncounterID);
			if (fromID)
			{
				CardAction cardAction = new CardAction();
				cardAction.ProducedCards = new CardsDropCollection[1];
				cardAction.ProducedCards[0] = new CardsDropCollection();
				cardAction.ProducedCards[0].DroppedEncounter = fromID;
				this.StartCoroutineEx(this.ActionRoutine(cardAction, null, true, false), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
			}
		}
		yield return null;
		if (!this.CurrentGameData.HasCardsData && GameLoad.Instance.CurrentGameDataIndex >= 0)
		{
			GameLoad.Instance.AutoSaveGame(this.IsSafeMode);
			this.CurrentGameData = GameLoad.Instance.Games[GameLoad.Instance.CurrentGameDataIndex].MainData;
		}
		this.IsInitializing = false;
		yield break;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00005E14 File Offset: 0x00004014
	private void InitializeStatsAndActions()
	{
		GameDataBase dataBase = GameLoad.Instance.DataBase;
		for (int i = 0; i < dataBase.AllData.Count; i++)
		{
			if (dataBase.AllData[i].GetType() == typeof(SelfTriggeredAction))
			{
				this.AllSelfActions.Add(dataBase.AllData[i] as SelfTriggeredAction);
			}
			else if (dataBase.AllData[i].GetType() == typeof(Objective))
			{
				this.AllObjectives.Add(dataBase.AllData[i] as Objective);
			}
			else if (dataBase.AllData[i].GetType() == typeof(ExclusiveCardOnBoardGroup))
			{
				this.ExclusiveGroups.Add(dataBase.AllData[i] as ExclusiveCardOnBoardGroup);
			}
			else if (dataBase.AllData[i].GetType() == typeof(CharacterPerk))
			{
				this.AllPerks.Add(dataBase.AllData[i] as CharacterPerk);
			}
			else if (dataBase.AllData[i].GetType() == typeof(BookmarkGroup))
			{
				this.GameGraphics.AllBookmarkGroups.Add(dataBase.AllData[i] as BookmarkGroup);
			}
			else if (dataBase.AllData[i].GetType() == typeof(CardData))
			{
				CardData cardData = dataBase.AllData[i] as CardData;
				CardUnlockConditions getUnlockConditions = cardData.GetUnlockConditions;
				if (cardData.CardType == CardTypes.Blueprint)
				{
					if (!this.AllBlueprintModels.Contains(cardData))
					{
						this.AllBlueprintModels.Add(cardData);
					}
					if (!this.BlueprintModelStates.ContainsKey(cardData))
					{
						this.BlueprintModelStates.Add(cardData, (!string.IsNullOrEmpty(cardData.UnlockConditionsDesc) || !this.BlueprintPurchasing) ? BlueprintModelState.Locked : BlueprintModelState.Hidden);
					}
					if (cardData.BlueprintResult != null)
					{
						for (int j = 0; j < cardData.BlueprintResult.Length; j++)
						{
							if (!cardData.BlueprintResult[j].IsEmpty && !this.AllBlueprintResults.ContainsKey(cardData.BlueprintResult[j].DroppedCard))
							{
								this.AllBlueprintResults.Add(cardData.BlueprintResult[j].DroppedCard, cardData);
							}
						}
					}
				}
				if (getUnlockConditions != null)
				{
					this.UnlockableCards.Add(getUnlockConditions);
				}
			}
			else if (dataBase.AllData[i].GetType() == typeof(LocalTickCounter))
			{
				LocalTickCounter localTickCounter = dataBase.AllData[i] as LocalTickCounter;
				if (this.CurrentGameData != null)
				{
					if (this.CurrentGameData.CountersDict.ContainsKey(localTickCounter.UniqueID))
					{
						this.AllCounters.Add(new InGameTickCounter(this.CurrentGameData.CountersDict[localTickCounter.UniqueID]));
					}
					else
					{
						this.AllCounters.Add(new InGameTickCounter(localTickCounter));
					}
				}
				this.CountersDict.Add(localTickCounter, this.AllCounters[this.AllCounters.Count - 1]);
			}
			else if (!(dataBase.AllData[i].GetType() != typeof(GameStat)))
			{
				GameStat gameStat = dataBase.AllData[i] as GameStat;
				InGameStat inGameStat = new GameObject(gameStat.GameName).AddComponent<InGameStat>();
				inGameStat.transform.SetParent(this.GameStatsParent);
				if (gameStat == this.PlayerWeightStat)
				{
					this.InGamePlayerWeight = inGameStat;
				}
				if (this.CurrentGameData != null && gameStat != this.PlayerWeightStat)
				{
					if (this.CurrentGameData.StatsDict.ContainsKey(gameStat.UniqueID))
					{
						inGameStat.Load(this.CurrentGameData.StatsDict[gameStat.UniqueID]);
					}
					else
					{
						inGameStat.Init(gameStat);
					}
				}
				else
				{
					inGameStat.Init(gameStat);
				}
				this.AllStats.Add(inGameStat);
				this.StatsDict.Add(gameStat, inGameStat);
			}
		}
		for (int k = 0; k < this.CurrentGameData.EnvironmentsData.Count; k++)
		{
			if (this.CurrentGameData.EnvironmentsData[k] != null)
			{
				this.CurrentGameData.EnvironmentsData[k].FillCounterDictionnary(this.AllCounters);
			}
		}
		Action onStatsListReady = GameManager.OnStatsListReady;
		if (onStatsListReady != null)
		{
			onStatsListReady();
		}
		for (int l = 0; l < this.AllStats.Count; l++)
		{
			GameStat gameStat = this.AllStats[l].StatModel;
			InGameStat inGameStat = this.AllStats[l];
			if (gameStat.TimeOfDayMods != null && gameStat.TimeOfDayMods.Length != 0)
			{
				if (!gameStat.HasTimeOfDayModsWithRequirements)
				{
					this.StatsWithTimeOfDayMods.Add(inGameStat);
				}
				else
				{
					this.StatsWithTimeOfDayModsAndRequirements.Add(inGameStat);
				}
				base.StartCoroutine(this.UpdateTimeOfDayMods(inGameStat));
			}
		}
		bool flag = this.CurrentGameData == null;
		List<GameStat> list = new List<GameStat>();
		if (!flag)
		{
			flag |= !this.CurrentGameData.HasStatsData;
		}
		if (flag)
		{
			flag &= (GameManager.CurrentGamemode || GameManager.CurrentPlayerCharacter);
		}
		if (flag)
		{
			if (GameManager.CurrentGamemode)
			{
				for (int m = 0; m < GameManager.CurrentGamemode.InitialStatModifiers.Length; m++)
				{
					if (GameManager.CurrentGamemode.InitialStatModifiers[m].Stat)
					{
						this.ApplyStatModifier(GameManager.CurrentGamemode.InitialStatModifiers[m], StatModification.Permanent, StatModifierReport.SourceFromGamemode(GameManager.CurrentGamemode), null);
						if (!list.Contains(GameManager.CurrentGamemode.InitialStatModifiers[m].Stat))
						{
							list.Add(GameManager.CurrentGamemode.InitialStatModifiers[m].Stat);
						}
					}
				}
			}
			if (GameManager.CurrentPlayerCharacter && GameManager.CurrentPlayerCharacter.InitialStatModifiers != null)
			{
				for (int n = 0; n < GameManager.CurrentPlayerCharacter.InitialStatModifiers.Length; n++)
				{
					if (GameManager.CurrentPlayerCharacter.InitialStatModifiers[n].Stat)
					{
						this.ApplyStatModifier(GameManager.CurrentPlayerCharacter.InitialStatModifiers[n], StatModification.Permanent, StatModifierReport.SourceFromPlayerCharacter(GameManager.CurrentPlayerCharacter), null);
						if (!list.Contains(GameManager.CurrentPlayerCharacter.InitialStatModifiers[n].Stat))
						{
							list.Add(GameManager.CurrentPlayerCharacter.InitialStatModifiers[n].Stat);
						}
					}
				}
			}
			if (GameManager.CurrentModifierPackages != null)
			{
				for (int num = 0; num < GameManager.CurrentModifierPackages.Count; num++)
				{
					if (GameManager.CurrentModifierPackages[num] && GameManager.CurrentModifierPackages[num].StartingStatModifiers != null)
					{
						for (int num2 = 0; num2 < GameManager.CurrentModifierPackages[num].StartingStatModifiers.Length; num2++)
						{
							if (GameManager.CurrentModifierPackages[num].StartingStatModifiers[num2].Stat)
							{
								this.ApplyStatModifier(GameManager.CurrentModifierPackages[num].StartingStatModifiers[num2], StatModification.Permanent, StatModifierReport.SourceFromPlayerCharacter(GameManager.CurrentPlayerCharacter), null);
								if (!list.Contains(GameManager.CurrentModifierPackages[num].StartingStatModifiers[num2].Stat))
								{
									list.Add(GameManager.CurrentModifierPackages[num].StartingStatModifiers[num2].Stat);
								}
							}
						}
					}
				}
			}
		}
		if (GameManager.CurrentPlayerCharacter.CharacterPerks != null)
		{
			for (int num3 = 0; num3 < GameManager.CurrentPlayerCharacter.CharacterPerks.Count; num3++)
			{
				if (GameManager.CurrentPlayerCharacter.CharacterPerks[num3].ActionModifiers != null && GameManager.CurrentPlayerCharacter.CharacterPerks[num3].ActionModifiers.Length != 0)
				{
					for (int num4 = 0; num4 < GameManager.CurrentPlayerCharacter.CharacterPerks[num3].ActionModifiers.Length; num4++)
					{
						this.AddActionModifier(GameManager.CurrentPlayerCharacter.CharacterPerks[num3].ActionModifiers[num4], ActionModifier.SourceFromPerk(GameManager.CurrentPlayerCharacter.CharacterPerks[num3], num4));
					}
				}
				if (GameManager.CurrentPlayerCharacter.CharacterPerks[num3].PassiveStatModifiers != null && GameManager.CurrentPlayerCharacter.CharacterPerks[num3].PassiveStatModifiers.Length != 0)
				{
					for (int num5 = 0; num5 < GameManager.CurrentPlayerCharacter.CharacterPerks[num3].PassiveStatModifiers.Length; num5++)
					{
						this.ApplyStatModifier(GameManager.CurrentPlayerCharacter.CharacterPerks[num3].PassiveStatModifiers[num5], StatModification.GlobalModifier, StatModifierReport.SourceFromPerk(GameManager.CurrentPlayerCharacter.CharacterPerks[num3]), GameManager.CurrentPlayerCharacter.CharacterPerks[num3]);
					}
				}
				if (flag)
				{
					for (int num6 = 0; num6 < GameManager.CurrentPlayerCharacter.CharacterPerks[num3].StartingStatModifiers.Length; num6++)
					{
						if (GameManager.CurrentPlayerCharacter.CharacterPerks[num3].StartingStatModifiers[num6].Stat)
						{
							this.ApplyStatModifier(GameManager.CurrentPlayerCharacter.CharacterPerks[num3].StartingStatModifiers[num6], StatModification.Permanent, StatModifierReport.SourceFromPlayerCharacter(GameManager.CurrentPlayerCharacter), null);
							if (!list.Contains(GameManager.CurrentPlayerCharacter.CharacterPerks[num3].StartingStatModifiers[num6].Stat))
							{
								list.Add(GameManager.CurrentPlayerCharacter.CharacterPerks[num3].StartingStatModifiers[num6].Stat);
							}
						}
					}
				}
			}
		}
		for (int num7 = 0; num7 < this.AllSelfActions.Count; num7++)
		{
			if (this.CurrentGameData != null)
			{
				if (this.CurrentGameData.ActionsDict.ContainsKey(this.AllSelfActions[num7].UniqueID))
				{
					this.AllSelfActions[num7].Init(this.CurrentGameData.ActionsDict[this.AllSelfActions[num7].UniqueID].StatTriggeredActions);
				}
				else
				{
					this.AllSelfActions[num7].Init(null);
				}
			}
			else
			{
				this.AllSelfActions[num7].Init(null);
			}
			for (int num8 = 0; num8 < this.AllSelfActions[num7].Actions.Length; num8++)
			{
				for (int num9 = 0; num9 < this.AllSelfActions[num7].Actions[num8].StatChangeTrigger.Length; num9++)
				{
					if (!this.AllSelfActions[num7].Actions[num8].StatChangeTrigger[num9].Stat)
					{
						Debug.LogError("Empty stat trigger condition on " + this.AllSelfActions[num7].name, this.AllSelfActions[num7]);
					}
					else if (this.StatsDict.ContainsKey(this.AllSelfActions[num7].Actions[num8].StatChangeTrigger[num9].Stat))
					{
						this.StatsDict[this.AllSelfActions[num7].Actions[num8].StatChangeTrigger[num9].Stat].RegisterListener(this.AllSelfActions[num7]);
						this.AllSelfActions[num7].StatTriggeredActions[num8].Conditions[num9] = this.AllSelfActions[num7].Actions[num8].StatChangeTrigger[num9].IsInRange(this.StatsDict[this.AllSelfActions[num7].Actions[num8].StatChangeTrigger[num9].Stat].CurrentValue(this.NotInBase));
					}
				}
			}
		}
		for (int num10 = 0; num10 < this.AllObjectives.Count; num10++)
		{
			this.AllObjectives[num10].ClearPages();
		}
		this.GetGuide.FillAllPagesList();
		this.GetJournal.FillAllPagesList();
		for (int num11 = 0; num11 < this.AllObjectives.Count; num11++)
		{
			if (this.CurrentGameData != null)
			{
				if (this.CurrentGameData.ObjectivesDict.ContainsKey(this.AllObjectives[num11].UniqueID))
				{
					this.AllObjectives[num11].Init(this.CurrentGameData.ObjectivesDict[this.AllObjectives[num11].UniqueID]);
				}
				else
				{
					this.AllObjectives[num11].Init(null);
				}
			}
			else
			{
				this.AllObjectives[num11].Init(null);
			}
		}
		for (int num12 = 0; num12 < this.AllPerks.Count; num12++)
		{
			if (UniqueIDScriptable.ListContains(GameLoad.Instance.SaveData.UnlockedPerks, this.AllPerks[num12]) && !this.AllPerks[num12].ShouldBeLockedOnRestart)
			{
				this.AllPerks[num12].ForceComplete(false);
			}
		}
		StatSaveData savedStatusInfo = null;
		for (int num13 = 0; num13 < this.AllStats.Count; num13++)
		{
			if (!list.Contains(this.AllStats[num13].StatModel) && this.AllStats[num13].StatModel.Statuses != null)
			{
				if (this.CurrentGameData.HasStatsData)
				{
					if (this.CurrentGameData.StatsDict.ContainsKey(this.AllStats[num13].StatModel.UniqueID))
					{
						savedStatusInfo = this.CurrentGameData.StatsDict[this.AllStats[num13].StatModel.UniqueID];
					}
				}
				else
				{
					savedStatusInfo = null;
				}
				base.StartCoroutine(this.UpdateStatStatuses(this.AllStats[num13], this.AllStats[num13].StatModel.MinMaxValue.x - 1f, savedStatusInfo));
			}
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00006D38 File Offset: 0x00004F38
	private void InitializeDefaultCards()
	{
		base.StartCoroutine(this.AddCard(this.HandCard, null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
		CardData cardData = this.StartingEnvironment;
		if (GameManager.CurrentPlayerCharacter)
		{
			if (GameManager.CurrentPlayerCharacter.Environment)
			{
				cardData = GameManager.CurrentPlayerCharacter.Environment;
			}
			if (GameManager.CurrentPlayerCharacter.CharacterPerks != null)
			{
				for (int i = 0; i < GameManager.CurrentPlayerCharacter.CharacterPerks.Count; i++)
				{
					if (GameManager.CurrentPlayerCharacter.CharacterPerks[i].OverrideEnvironment)
					{
						cardData = GameManager.CurrentPlayerCharacter.CharacterPerks[i].OverrideEnvironment;
						break;
					}
				}
			}
		}
		base.StartCoroutine(this.AddCard(cardData, null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
		if (cardData.DefaultEnvCards != null)
		{
			for (int j = 0; j < cardData.DefaultEnvCards.Length; j++)
			{
				base.StartCoroutine(this.AddCard(cardData.DefaultEnvCards[j], null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
			}
		}
		if (!GameManager.CurrentPlayerCharacter)
		{
			for (int k = 0; k < this.StartingLocations.Count; k++)
			{
				base.StartCoroutine(this.AddCard(this.StartingLocations[k], null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
			}
		}
		else if (!GameManager.CurrentPlayerCharacter.OverrideLocations)
		{
			for (int l = 0; l < GameManager.CurrentPlayerCharacter.Locations.Length; l++)
			{
				base.StartCoroutine(this.AddCard(GameManager.CurrentPlayerCharacter.Locations[l], null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
			}
		}
		if (!GameManager.CurrentGamemode)
		{
			for (int m = 0; m < this.StartingItems.Count; m++)
			{
				base.StartCoroutine(this.AddCard(this.StartingItems[m], null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
			}
			for (int n = 0; n < this.StartingBaseStructures.Count; n++)
			{
				base.StartCoroutine(this.AddCard(this.StartingBaseStructures[n], null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
			}
		}
		else
		{
			this.GetStartingCardsFromArray(GameManager.CurrentGamemode.BaseAndItemCards, false);
		}
		this.InitializePlayerCharacter(false);
		this.InitializeModifierPackages();
		CardData data = this.StartingWeather;
		if (GameManager.CurrentPlayerCharacter)
		{
			if (GameManager.CurrentPlayerCharacter.Environment)
			{
				data = GameManager.CurrentPlayerCharacter.Weather;
			}
			if (GameManager.CurrentPlayerCharacter.CharacterPerks != null)
			{
				for (int num = 0; num < GameManager.CurrentPlayerCharacter.CharacterPerks.Count; num++)
				{
					if (GameManager.CurrentPlayerCharacter.CharacterPerks[num].OverrideWeather)
					{
						data = GameManager.CurrentPlayerCharacter.CharacterPerks[num].OverrideWeather;
						break;
					}
				}
			}
		}
		base.StartCoroutine(this.AddCard(data, null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
		this.CardsLoaded = true;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00007058 File Offset: 0x00005258
	private void InitializePlayerCharacter(bool _OnlyBlueprintsMode)
	{
		if (GameManager.CurrentPlayerCharacter)
		{
			this.GetStartingCardsFromArray(GameManager.CurrentPlayerCharacter.BaseAndItemCards, _OnlyBlueprintsMode);
			SlotInfo slot = new SlotInfo(SlotsTypes.Equipment, -2);
			if (!_OnlyBlueprintsMode && !GameManager.CurrentPlayerCharacter.OverridesClothes)
			{
				for (int i = 0; i < GameManager.CurrentPlayerCharacter.StartingClothes.Length; i++)
				{
					base.StartCoroutine(this.AddCard(GameManager.CurrentPlayerCharacter.StartingClothes[i], slot, null, null, true, null, null, null, null, null, Vector3.zero, true, SpawningLiquid.DefaultLiquid, false, false, Vector2Int.zero, null, 0, -1, false, ""));
				}
			}
			if (GameManager.CurrentPlayerCharacter.CharacterPerks != null)
			{
				for (int j = 0; j < GameManager.CurrentPlayerCharacter.CharacterPerks.Count; j++)
				{
					if (GameManager.CurrentPlayerCharacter.CharacterPerks[j])
					{
						if (!_OnlyBlueprintsMode && GameManager.CurrentPlayerCharacter.CharacterPerks[j].EquippedCards != null)
						{
							for (int k = 0; k < GameManager.CurrentPlayerCharacter.CharacterPerks[j].EquippedCards.Length; k++)
							{
								base.StartCoroutine(this.AddCard(GameManager.CurrentPlayerCharacter.CharacterPerks[j].EquippedCards[k], slot, null, null, true, null, null, null, null, null, Vector3.zero, true, SpawningLiquid.DefaultLiquid, false, false, Vector2Int.zero, null, 0, -1, false, ""));
							}
						}
						this.GetStartingCardsFromArray(GameManager.CurrentPlayerCharacter.CharacterPerks[j].AddedCards, _OnlyBlueprintsMode);
					}
				}
			}
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x000071E0 File Offset: 0x000053E0
	private void InitializeModifierPackages()
	{
		if (GameManager.CurrentModifierPackages != null)
		{
			for (int i = 0; i < GameManager.CurrentModifierPackages.Count; i++)
			{
				this.GetStartingCardsFromArray(GameManager.CurrentModifierPackages[i].AddedCards, false);
			}
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00007220 File Offset: 0x00005420
	private void GetStartingCardsFromArray(CardData[] _Array, bool _OnlyBlueprintsMode)
	{
		if (_Array == null)
		{
			return;
		}
		for (int i = 0; i < _Array.Length; i++)
		{
			if (_Array[i])
			{
				if (_Array[i].CardType == CardTypes.Blueprint || _Array[i].CardType == CardTypes.EnvImprovement)
				{
					if (_Array[i].CardType == CardTypes.Blueprint && !this.StartingBlueprints.Contains(_Array[i]))
					{
						this.StartingBlueprints.Add(_Array[i]);
					}
					CardUnlockConditions item = new CardUnlockConditions(_Array[i], true);
					this.UnlockableCards.Add(item);
				}
				else if (!_OnlyBlueprintsMode)
				{
					base.StartCoroutine(this.AddCard(_Array[i], null, true, null, true, SpawningLiquid.DefaultLiquid, Vector2Int.zero, true));
				}
			}
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x000072CC File Offset: 0x000054CC
	private void LoadCards()
	{
		if (this.HandCard)
		{
			this.LoadCard(this.CurrentGameData.CurrentHandCard, null, false, null);
		}
		this.LoadCard(this.CurrentGameData.CurrentEnvironmentCard, null, false, null);
		UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(this.CurrentGameData.PrevEnvironmentID);
		if (fromID != null && fromID is CardData)
		{
			this.PrevEnvironment = (fromID as CardData);
		}
		this.CurrentTravelIndex = this.CurrentGameData.CurrentTravelIndex;
		this.TravelCardCopies.Clear();
		for (int i = 0; i < this.CurrentGameData.EnvironmentsData.Count; i++)
		{
			fromID = UniqueIDScriptable.GetFromID(this.CurrentGameData.EnvironmentsData[i].EnvironmentID);
			if (fromID && fromID is CardData)
			{
				CardData cardData = fromID as CardData;
				if (string.IsNullOrEmpty(this.CurrentGameData.EnvironmentsData[i].DictionaryKey))
				{
					this.CurrentGameData.EnvironmentsData[i].DictionaryKey = cardData.EnvironmentDictionaryKey(null, 0);
				}
				if (this.EnvironmentsData.ContainsKey(UniqueIDScriptable.RemoveNamesFromComplexID(this.CurrentGameData.EnvironmentsData[i].DictionaryKey)))
				{
					if (cardData != this.CurrentEnvironment)
					{
						Debug.LogError(cardData.name + " is already registered as an environment, something is wrong...");
					}
					else
					{
						this.EnvironmentsData[UniqueIDScriptable.RemoveNamesFromComplexID(this.CurrentGameData.EnvironmentsData[i].DictionaryKey)] = this.CurrentGameData.EnvironmentsData[i];
					}
				}
				else
				{
					this.EnvironmentsData.Add(UniqueIDScriptable.RemoveNamesFromComplexID(this.CurrentGameData.EnvironmentsData[i].DictionaryKey), this.CurrentGameData.EnvironmentsData[i]);
				}
			}
		}
		base.StartCoroutine(this.LoadCardSet(this.CurrentGameData.CurrentCardsData, this.CurrentGameData.CurrentInventoryCards, this.CurrentGameData.CurrentNestedInventoryCards, null, false));
		this.LoadCard(this.CurrentGameData.CurrentWeatherCard, null, false, null);
		this.LoadCard(this.CurrentGameData.CurrentEventCard, null, false, null);
		this.CardsLoaded = true;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00007511 File Offset: 0x00005711
	private IEnumerator LoadCardSet(List<CardSaveData> _RegularCards, List<InventoryCardSaveData> _InventoryCards, List<InventoryCardSaveData> _NestedInventoryCards, List<InGameRefCardSaveData> _RefCards, bool _NextEnv)
	{
		List<CardSaveData> list = new List<CardSaveData>();
		if (_InventoryCards != null)
		{
			list.AddRange(_InventoryCards);
		}
		if (_RegularCards != null)
		{
			list.AddRange(_RegularCards);
		}
		if (_RefCards != null)
		{
			list.AddRange(_RefCards);
		}
		list.Sort(new CardSaveDataComparer());
		int num = 0;
		int num2 = 0;
		GameManager.<>c__DisplayClass326_0 CS$<>8__locals1;
		CS$<>8__locals1.envToCheck = (_NextEnv ? this.NextEnvironment : this.CurrentEnvironment);
		CS$<>8__locals1.prevEnvToCheck = (_NextEnv ? this.CurrentEnvironment : this.PrevEnvironment);
		CS$<>8__locals1.indexToCheck = (_NextEnv ? this.NextTravelIndex : this.CurrentTravelIndex);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].SlotInformation.SlotType == SlotsTypes.Location)
			{
				if (GameManager.<LoadCardSet>g__cardWillBeInEnv|326_0(list[i], ref CS$<>8__locals1))
				{
					list[i].SlotInformation.SlotIndex = num;
					num++;
				}
			}
			else if (list[i].SlotInformation.SlotType == SlotsTypes.Base && GameManager.<LoadCardSet>g__cardWillBeInEnv|326_0(list[i], ref CS$<>8__locals1))
			{
				list[i].SlotInformation.SlotIndex = num2;
				num2++;
			}
			if (!(list[i] is InventoryCardSaveData))
			{
				if (!(list[i] is InGameRefCardSaveData))
				{
					this.LoadCard(list[i], null, _NextEnv, null);
				}
				else
				{
					InGameRefCardSaveData inGameRefCardSaveData = list[i] as InGameRefCardSaveData;
					inGameRefCardSaveData.ReferencedCard.CurrentSlotInfo = list[i].SlotInformation;
					this.AllCards.Add(inGameRefCardSaveData.ReferencedCard);
					if ((inGameRefCardSaveData.ReferencedCard.CardModel.HasPassiveEffects || inGameRefCardSaveData.ReferencedCard.HasExternalPassiveEffects) && !this.CardsWithPassiveEffects.Contains(inGameRefCardSaveData.ReferencedCard))
					{
						this.CardsWithPassiveEffects.Add(inGameRefCardSaveData.ReferencedCard);
					}
					if (inGameRefCardSaveData.ReferencedCard.CardModel.HasRemoteEffects)
					{
						this.AddRemotePassiveEffects(inGameRefCardSaveData.ReferencedCard);
					}
				}
			}
			else
			{
				this.LoadInventoryCard(list[i] as InventoryCardSaveData, null, _NestedInventoryCards, _NextEnv);
			}
		}
		CoroutineController controller;
		this.StartCoroutineEx(this.UpdatePassiveEffects(), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00007545 File Offset: 0x00005745
	public void ConfirmCardOnCardAction()
	{
		this.ConfirmedAction = true;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0000754E File Offset: 0x0000574E
	public static Coroutine PerformCardOnCardActionStack(CardOnCardAction _Action, List<InGameCardBase> _GivenCards, InGameCardBase _ReceivingCard)
	{
		return MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.WaitForCardOnCardActionConfirmation(_Action, _GivenCards, _ReceivingCard));
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00007568 File Offset: 0x00005768
	public static Coroutine PerformCardOnCardAction(CardOnCardAction _Action, InGameCardBase _GivenCard, InGameCardBase _ReceivingCard)
	{
		return GameManager.PerformCardOnCardActionStack(_Action, new List<InGameCardBase>
		{
			_GivenCard
		}, _ReceivingCard);
	}

	// Token: 0x0600009D RID: 157 RVA: 0x0000758A File Offset: 0x0000578A
	private IEnumerator WaitForCardOnCardActionConfirmation(CardOnCardAction _Action, List<InGameCardBase> _GivenCards, InGameCardBase _ReceivingCard)
	{
		if (_Action.ConfirmPopup)
		{
			this.ConfirmedAction = false;
			this.GameGraphics.ConfirmActionName.text = string.Format("{0} - {1}", _ReceivingCard.CardName(true), _Action.ActionName);
			this.GameGraphics.ConfirmActionPopup.SetActive(true);
			while (MBSingleton<GameManager>.Instance.GameGraphics.ConfirmActionPopup.activeInHierarchy)
			{
				yield return null;
			}
			if (!MBSingleton<GameManager>.Instance.ConfirmedAction)
			{
				yield break;
			}
		}
		if (_GivenCards.Count > 1)
		{
			_Action.SetupStackStopCondition();
		}
		this.CurrentGameState = GameStates.PLAYINGCARD;
		this.GameSounds.PerformActionSound(_Action.ActionSounds, _Action.DisablePitchVariation);
		CoroutineController controller = null;
		int num;
		for (int i = 0; i < _GivenCards.Count; i = num + 1)
		{
			if (_GivenCards[i] && !_GivenCards[i].Destroyed)
			{
				if (_Action.StackStopConditions.ShouldStopStackAction(_ReceivingCard, _GivenCards[i]))
				{
					yield break;
				}
				this.StartCoroutineEx(MBSingleton<GameManager>.Instance.CardOnCardActionRoutine(_Action, _GivenCards[i], _ReceivingCard, i != 0), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
				if (!_ReceivingCard)
				{
					yield break;
				}
				if (_ReceivingCard.Destroyed)
				{
					yield break;
				}
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000075B0 File Offset: 0x000057B0
	public static Coroutine PerformStackAction(CardAction _Action, DynamicLayoutSlot _Slot, bool _Liquids)
	{
		List<InGameCardBase> cardPile = _Slot.GetCardPile(_Liquids, false, true);
		if (cardPile == null)
		{
			return null;
		}
		return MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.PerformStackActionRoutine(_Action, cardPile));
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000075E2 File Offset: 0x000057E2
	private IEnumerator PerformStackActionRoutine(CardAction _Action, List<InGameCardBase> _Cards)
	{
		CoroutineController controller = null;
		int num;
		for (int i = 0; i < _Cards.Count; i = num + 1)
		{
			this.StartCoroutineEx(GameManager.PerformActionAsEnumerator(_Action, _Cards[i], i != _Cards.Count - 1), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000075FF File Offset: 0x000057FF
	public static Coroutine PerformAction(CardAction _Action, InGameCardBase _ReceivingCard, bool _FastMode)
	{
		MBSingleton<GameManager>.Instance.CurrentGameState = GameStates.PLAYINGCARD;
		MBSingleton<GameManager>.Instance.GameSounds.PerformActionSound(_Action.ActionSounds, _Action.DisablePitchVariation);
		return MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.ActionRoutine(_Action, _ReceivingCard, _FastMode, false));
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0000763F File Offset: 0x0000583F
	public static IEnumerator PerformActionAsEnumerator(CardAction _Action, InGameCardBase _ReceivingCard, bool _FastMode)
	{
		MBSingleton<GameManager>.Instance.CurrentGameState = GameStates.PLAYINGCARD;
		MBSingleton<GameManager>.Instance.GameSounds.PerformActionSound(_Action.ActionSounds, _Action.DisablePitchVariation);
		CoroutineController controller = null;
		MBSingleton<GameManager>.Instance.StartCoroutineEx(MBSingleton<GameManager>.Instance.ActionRoutine(_Action, _ReceivingCard, _FastMode, false), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0000765C File Offset: 0x0000585C
	private IEnumerator CardOnCardActionRoutine(CardOnCardAction _Action, InGameCardBase _GivenCard, InGameCardBase _ReceivingCard, bool _FastMode)
	{
		if (!_FastMode)
		{
			yield return null;
		}
		if (_Action.CarryOverGivenCard)
		{
			_GivenCard.CanCarryToNewEnv = true;
		}
		_Action.CollectActionModifiers(_ReceivingCard, _GivenCard);
		CoroutineController controller;
		this.StartCoroutineEx(this.ApplyExtraDurabilitiesChanges(_Action.ExtraDurabilityModifications, 1f, _GivenCard, _ReceivingCard, _Action.ActionName, _Action.NoCardsAffectedMessage, _Action.CustomDestroyMessage, _Action.DontShowDestroyMessage, false), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		if (_GivenCard)
		{
			_GivenCard.IgnoreTickDurabilityChanges = (_Action.GivenCardChanges.ModType == CardModifications.Destroy || _Action.GivenCardChanges.ModType == CardModifications.Transform);
		}
		if (_Action.GivenCardChanges.ModType != CardModifications.Destroy)
		{
			if (!_Action.GivenDurabilityChanges.IsEmpty)
			{
				this.StartCoroutineEx(this.ChangeCardDurabilities(_GivenCard, _Action.GivenDurabilityChanges.Spoilage, _Action.GivenDurabilityChanges.Usage, _Action.GivenDurabilityChanges.Fuel, _Action.GivenDurabilityChanges.ConsumableCharges, _Action.GivenDurabilityChanges.Liquid, _Action.GivenDurabilityChanges.Special1, _Action.GivenDurabilityChanges.Special2, _Action.GivenDurabilityChanges.Special3, _Action.GivenDurabilityChanges.Special4, true, true), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
			}
			if (_GivenCard && !_Action.CreatedLiquidInGivenCard.IsEmpty && _GivenCard.CardModel && _GivenCard.CardModel.CanContainLiquid)
			{
				TransferedDurabilities transferedDurabilities = new TransferedDurabilities();
				transferedDurabilities.Spoilage.FloatValue = _Action.CreatedLiquidInGivenCard.LiquidCard.SpoilageTime;
				transferedDurabilities.Usage.FloatValue = _Action.CreatedLiquidInGivenCard.LiquidCard.UsageDurability;
				transferedDurabilities.Fuel.FloatValue = _Action.CreatedLiquidInGivenCard.LiquidCard.FuelCapacity;
				transferedDurabilities.ConsumableCharges.FloatValue = _Action.CreatedLiquidInGivenCard.LiquidCard.Progress;
				transferedDurabilities.Liquid = UnityEngine.Random.Range(_Action.CreatedLiquidInGivenCard.Quantity.x, _Action.CreatedLiquidInGivenCard.Quantity.y);
				transferedDurabilities.Special1 = _Action.CreatedLiquidInGivenCard.LiquidCard.SpecialDurability1;
				transferedDurabilities.Special2 = _Action.CreatedLiquidInGivenCard.LiquidCard.SpecialDurability2;
				transferedDurabilities.Special3 = _Action.CreatedLiquidInGivenCard.LiquidCard.SpecialDurability3;
				transferedDurabilities.Special4 = _Action.CreatedLiquidInGivenCard.LiquidCard.SpecialDurability4;
				this.StartCoroutineEx(this.AddCard(_Action.CreatedLiquidInGivenCard.LiquidCard, _GivenCard, false, transferedDurabilities, false, SpawningLiquid.Empty, new Vector2Int(this.CurrentTickInfo.z, 0), true), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
			}
		}
		this.StartCoroutineEx(this.ApplyCardStateChange(_Action.GivenCardChanges, _GivenCard, _Action.DurabilitiesLiquidScale), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		this.StartCoroutineEx(this.ActionRoutine(_Action, _ReceivingCard, _FastMode, true), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00007688 File Offset: 0x00005888
	private IEnumerator ActionRoutine(CardAction _Action, InGameCardBase _ReceivingCard, bool _FastMode, bool _ModifiersAlreadyCollected = false)
	{
		int startingActionTick = this.CurrentTickInfo.z;
		bool isQueuedCardAction = false;
		if (!_ModifiersAlreadyCollected)
		{
			_Action.CollectActionModifiers(_ReceivingCard, null);
		}
		while (this.QueuedCardActions.Count != 0)
		{
			if (!(this.QueuedCardActions[0].OnCard != _ReceivingCard) && this.QueuedCardActions[0].Action == _Action)
			{
				isQueuedCardAction = true;
				this.GameGraphics.ActionHighlightPopup.SetupSingleAction(_ReceivingCard, _Action, this.QueuedCardActions[0].Message);
				while (this.GameGraphics.ActionHighlightPopup.gameObject.activeInHierarchy)
				{
					yield return null;
				}
				break;
			}
			yield return null;
		}
		CardData fromCard = null;
		if (_ReceivingCard)
		{
			_ReceivingCard.IgnoreTickDurabilityChanges = (_Action.ReceivingCardChanges.ModType == CardModifications.Destroy || _Action.ReceivingCardChanges.ModType == CardModifications.Transform);
			fromCard = _ReceivingCard.CardModel;
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		this.ActionCancelled = false;
		this.CancelledAtTick = 0;
		Transform feedbackSource = null;
		if (_ReceivingCard)
		{
			if (_ReceivingCard.CardVisuals)
			{
				feedbackSource = _ReceivingCard.CardVisuals.StatFeedbacksParent;
			}
			else
			{
				feedbackSource = this.GameGraphics.FadeToBlack.TimeSpentPosTr;
			}
		}
		else if (this.GameGraphics.FadeToBlack.FadeActive)
		{
			feedbackSource = this.GameGraphics.FadeToBlack.TimeSpentPosTr;
		}
		int startedAtDay = this.CurrentTickInfo.x;
		ActionReport actionReport = default(ActionReport);
		actionReport.StartAction(_Action, fromCard, this.CurrentTickInfo.z, this.CurrentMiniTicks, this.CurrentActionCounter, Time.frameCount);
		Action<ActionReport> onActionStarted = GameManager.OnActionStarted;
		if (onActionStarted != null)
		{
			onActionStarted(actionReport);
		}
		if (_ReceivingCard && _ReceivingCard.CardModel && _ReceivingCard.CardModel.CardType == CardTypes.Event)
		{
			this.EventAction = _Action;
		}
		bool isNotFirstRootAction = false;
		CoroutineController controller;
		int num;
		if (this.RootAction == null && !isQueuedCardAction)
		{
			if (_Action.NotBaseAction && !this.NotInBase)
			{
				this.NotInBase = true;
				this.CurrentOutOfBaseAction = _Action;
				for (int k = 0; k < this.AllStats.Count; k++)
				{
					this.StartCoroutineEx(this.UpdateStatStatuses(this.AllStats[k], this.AllStats[k].CurrentValue(false), null), out controller);
					waitFor.Add(controller);
				}
				for (int i = 0; i < waitFor.Count; i = num + 1)
				{
					if (waitFor[i].state != CoroutineState.Finished)
					{
						i = -1;
						yield return null;
					}
					num = i;
				}
			}
			this.SaveAfterAction = false;
			this.RootAction = _Action;
		}
		else if (this.RootAction == _Action)
		{
			isNotFirstRootAction = true;
		}
		StatModifier[] array = null;
		if (!_Action.InstantStatModifications && _Action.AllStatModifiers != null && _Action.AllStatModifiers.Count > 0)
		{
			array = new StatModifier[_Action.AllStatModifiers.Count];
			for (int l = 0; l < _Action.AllStatModifiers.Count; l++)
			{
				array[l] = new StatModifier(_Action.AllStatModifiers[l], _Action.TotalDaytimeCost, true, StatModifierReport.SourceFromAction(_Action, _ReceivingCard), _Action.NoveltyID(_ReceivingCard), startingActionTick);
			}
		}
		OverTimeDurabilityChanges durabilities = null;
		if (!_Action.InstantStatModifications && _Action.ReceivingCardChanges.ModType == CardModifications.DurabilityChanges)
		{
			durabilities = new OverTimeDurabilityChanges(_Action.ReceivingCardChanges, _Action.TotalDaytimeCost, _Action.DurabilitiesLiquidScale);
		}
		if (!_Action.ReceivingDurabilityChanges.IsEmpty && !_Action.InstantStatModifications)
		{
			if (durabilities == null)
			{
				durabilities = new OverTimeDurabilityChanges(_Action.ReceivingDurabilityChanges, _Action.TotalDaytimeCost);
			}
			else
			{
				durabilities.Add(_Action.ReceivingDurabilityChanges);
			}
		}
		bool checkForStats = this.RootAction == _Action && !isNotFirstRootAction;
		int num2 = _Action.TotalDaytimeCost;
		if (num2 == 0 && _Action.MiniTicksCost > 0)
		{
			num = this.CurrentMiniTicks;
			this.CurrentMiniTicks = num + 1;
			this.CurrentActionCounter = 0;
			if (!_ReceivingCard)
			{
				this.GameGraphics.TimeSpentWheel.transform.position = this.GameGraphics.FadeToBlack.TimeSpentPos;
			}
			else
			{
				this.GameGraphics.TimeSpentWheel.transform.position = _ReceivingCard.transform.position;
			}
			if (this.CurrentMiniTicks == this.DaySettings.MiniTicksPerTick)
			{
				this.CurrentMiniTicks = 0;
				num2 = 1;
			}
			else
			{
				this.GameGraphics.UpdateTimeInfo(false);
			}
		}
		checkForStats &= (!_FastMode || num2 != 0);
		this.StartCoroutineEx(this.SpendDaytimePoints(num2, checkForStats, num2 > 0, _Action.TotalDaytimeCost == 0, _ReceivingCard, _Action.FadeToBlack, _Action.FadeMessage, _Action.Cancellable, _Action.FadeTips, array, durabilities, _Action.StatInterruptions), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		if (!this.ActionCancelled)
		{
			this.CancelledAtTick = Mathf.Max(1, _Action.TotalDaytimeCost);
		}
		if (_ReceivingCard && !_FastMode && _ReceivingCard.CardModel && (_ReceivingCard.CardModel.CardType != CardTypes.Event || (_ReceivingCard.CardModel.CardType == CardTypes.Event && _Action.WillHaveAnEffect(_ReceivingCard, false, false, false, false, new CardModifications[]
		{
			CardModifications.Destroy
		}))))
		{
			if (!_ReceivingCard.IsLiquid)
			{
				_ReceivingCard.Pulse((_Action.ReceivingCardChanges.ModType == CardModifications.Destroy) ? 0.5f : 0.25f);
			}
			else
			{
				_ReceivingCard.CurrentContainer.Pulse((_Action.ReceivingCardChanges.ModType == CardModifications.Destroy) ? 0.5f : 0.25f);
			}
		}
		if (_Action.InstantStatModifications && _Action.AllStatModifiers != null)
		{
			this.WillCheckForPassiveEffects = true;
			for (int m = 0; m < _Action.AllStatModifiers.Count; m++)
			{
				if (_Action.AllStatModifiers[m].ApplyEachTick)
				{
					_Action.AllStatModifiers[m] = StatModifier.InstantMod(_Action.AllStatModifiers[m], true, _Action.TotalDaytimeCost);
				}
				else
				{
					_Action.AllStatModifiers[m] = StatModifier.InstantMod(_Action.AllStatModifiers[m], true, 1);
				}
			}
			this.StartCoroutineEx(this.ApplyStatModifiers(_Action.AllStatModifiers.ToArray(), _Action.NoveltyID(_ReceivingCard), startingActionTick, feedbackSource, (float)this.CancelledAtTick / (float)Mathf.Max(1, _Action.TotalDaytimeCost), StatModifierReport.SourceFromAction(_Action, _ReceivingCard)), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
		}
		if (_Action.CustomWindowPrefab && !this.ActionCancelled)
		{
			this.OpenWindow = UnityEngine.Object.Instantiate<GameObject>(_Action.CustomWindowPrefab, this.GameGraphics.SpecialInspectionPopupParent);
			this.OpenContent = this.OpenWindow.GetComponent<ContentDisplayer>();
			while (this.OpenWindow)
			{
				if (!this.OpenWindow.activeInHierarchy)
				{
					UnityEngine.Object.Destroy(this.OpenWindow.gameObject);
					this.OpenWindow = null;
					this.OpenContent = null;
					break;
				}
				yield return null;
			}
		}
		this.StartCoroutineEx(this.ApplyExtraDurabilitiesChanges(_Action.ExtraDurabilityModifications, (float)this.CancelledAtTick / (float)Mathf.Max(1, _Action.TotalDaytimeCost), null, _ReceivingCard, _Action.ActionName, _Action.NoCardsAffectedMessage, _Action.CustomDestroyMessage, _Action.DontShowDestroyMessage, _FastMode), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		CardsDropCollection collectionToDrop = null;
		bool flag = _ReceivingCard != null;
		if (flag)
		{
			flag = (_ReceivingCard.CardModel != null);
		}
		if (flag)
		{
			flag = (_ReceivingCard.CardModel.CardType == CardTypes.Event);
		}
		if (!this.ActionCancelled || flag)
		{
			if (durabilities == null || _Action.ReceivingCardChanges.ModType != CardModifications.DurabilityChanges)
			{
				if (_Action.InstantStatModifications && !_Action.ReceivingDurabilityChanges.IsEmpty && _Action.ReceivingCardChanges.ModType != CardModifications.Destroy)
				{
					this.StartCoroutineEx(this.ChangeCardDurabilities(_ReceivingCard, _Action.ReceivingDurabilityChanges.Spoilage, _Action.ReceivingDurabilityChanges.Usage, _Action.ReceivingDurabilityChanges.Fuel, _Action.ReceivingDurabilityChanges.ConsumableCharges, _Action.ReceivingDurabilityChanges.Liquid, _Action.ReceivingDurabilityChanges.Special1, _Action.ReceivingDurabilityChanges.Special2, _Action.ReceivingDurabilityChanges.Special3, _Action.ReceivingDurabilityChanges.Special4, true, true), out controller);
					while (controller.state != CoroutineState.Finished)
					{
						yield return null;
					}
				}
				this.StartCoroutineEx(this.ApplyCardStateChange(_Action.ReceivingCardChanges, _ReceivingCard, _Action.DurabilitiesLiquidScale), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
			}
			bool transformsIntoEnv = _Action.ReceivingCardChanges.ModType == CardModifications.Transform && _Action.ReceivingCardChanges.TransformInto;
			if (transformsIntoEnv)
			{
				transformsIntoEnv = (_Action.ReceivingCardChanges.TransformInto.CardType == CardTypes.Environment);
			}
			collectionToDrop = this.SelectCardCollection(_Action, _ReceivingCard);
			bool noSpecialCase = true;
			if (_ReceivingCard && _ReceivingCard.CardModel && _ReceivingCard.CardModel.CardType == CardTypes.Explorable && !(_Action is CardOnCardAction) && _Action.IsExploreAction)
			{
				noSpecialCase = false;
				if (_Action is DismantleCardAction)
				{
					DismantleCardAction dismantleCardAction = _Action as DismantleCardAction;
					int i = (dismantleCardAction.MinMaxExplorationDrops.sqrMagnitude > 0) ? UnityEngine.Random.Range(dismantleCardAction.MinMaxExplorationDrops.x, dismantleCardAction.MinMaxExplorationDrops.y + 1) : MBSingleton<ExplorationPopup>.Instance.ExplorationSlotCount;
					List<ConditionalStatModifier> allStatModifiers = new List<ConditionalStatModifier>();
					int j = 0;
					while (j < i && MBSingleton<ExplorationPopup>.Instance.CanAddCards)
					{
						if (j > 0)
						{
							collectionToDrop = this.SelectCardCollection(_Action, _ReceivingCard);
						}
						yield return base.StartCoroutine(this.ProduceCards(collectionToDrop, _ReceivingCard, transformsIntoEnv, true, _Action.TravelToPreviousEnv));
						if (collectionToDrop.StatModifications != null && collectionToDrop.StatModifications.Length != 0)
						{
							allStatModifiers.AddRange(collectionToDrop.StatModifications);
						}
						MBSingleton<ExplorationPopup>.Instance.NextSlot();
						num = j;
						j = num + 1;
					}
					MBSingleton<ExplorationPopup>.Instance.StartSelection();
					while (MBSingleton<ExplorationPopup>.Instance.gameObject.activeSelf)
					{
						yield return null;
					}
					List<InGameCardBase> selection = MBSingleton<ExplorationPopup>.Instance.GetSelection();
					CardDrop[] array2 = new CardDrop[selection.Count];
					collectionToDrop = new CardsDropCollection();
					this.ExplorationDroppedEvents.Clear();
					for (int n = 0; n < selection.Count; n++)
					{
						array2[n] = new CardDrop
						{
							DroppedCard = selection[n].CardModel,
							Quantity = Vector2Int.one
						};
						base.StartCoroutine(this.RemoveCard(selection[n], true, false, GameManager.RemoveOption.Standard, false));
					}
					collectionToDrop.SetDroppedCards(array2, false);
					collectionToDrop.SetStatModifiers(allStatModifiers.ToArray());
					noSpecialCase = true;
					MBSingleton<ExplorationPopup>.Instance.Clear();
					allStatModifiers = null;
				}
				else if (MBSingleton<ExplorationPopup>.Instance.CanAddCards)
				{
					yield return base.StartCoroutine(this.ProduceCards(collectionToDrop, _ReceivingCard, transformsIntoEnv, true, _Action.TravelToPreviousEnv));
				}
			}
			if (noSpecialCase)
			{
				waitFor.Clear();
				this.StartCoroutineEx(this.ProduceCards(collectionToDrop, _ReceivingCard, transformsIntoEnv, false, _Action.TravelToPreviousEnv), out controller);
				waitFor.Add(controller);
				if (collectionToDrop != null)
				{
					if (collectionToDrop.CurrentStatModifiers != null)
					{
						this.StartCoroutineEx(this.ApplyStatModifiers(collectionToDrop.CurrentStatModifiers.ToArray(), _Action.NoveltyID(_ReceivingCard), startingActionTick, feedbackSource, 1f, StatModifierReport.SourceFromAction(_Action, _ReceivingCard)), out controller);
					}
					if (collectionToDrop.DurabilityModifications != null && !collectionToDrop.DurabilityModifications.IsEmpty)
					{
						TransferedDurabilities transferedDurabilities = new TransferedDurabilities();
						transferedDurabilities.Add(collectionToDrop.DurabilityModifications);
						this.StartCoroutineEx(this.ChangeCardDurabilities(_ReceivingCard, transferedDurabilities.Spoilage, transferedDurabilities.Usage, transferedDurabilities.Fuel, transferedDurabilities.ConsumableCharges, transferedDurabilities.Liquid, transferedDurabilities.Special1, transferedDurabilities.Special2, transferedDurabilities.Special3, transferedDurabilities.Special4, true, true), out controller);
						waitFor.Add(controller);
					}
				}
				for (int i = 0; i < waitFor.Count; i = num + 1)
				{
					if (waitFor[i].state != CoroutineState.Finished)
					{
						i = -1;
						yield return null;
					}
					num = i;
				}
			}
			if (collectionToDrop != null && !string.IsNullOrEmpty(collectionToDrop.CurrentMessage))
			{
				this.GameGraphics.PlayCardNotification(_ReceivingCard, collectionToDrop.CurrentMessage);
			}
			if (_Action.LoadedCards != null && _Action.LoadedCards.Count > 0)
			{
				waitFor.Clear();
				for (int num3 = 0; num3 < _Action.LoadedCards.Count; num3++)
				{
					this.StartCoroutineEx(this.AddCard(_Action.LoadedCards[num3], _ReceivingCard ? _ReceivingCard.ValidPosition : Vector3.zero), out controller);
					waitFor.Add(controller);
				}
				for (int i = 0; i < waitFor.Count; i = num + 1)
				{
					if (waitFor[i].state != CoroutineState.Finished)
					{
						i = -1;
						yield return null;
					}
					num = i;
				}
			}
		}
		if (_Action.BlueprintsFullUnlock != null && _Action.BlueprintsFullUnlock.Length != 0)
		{
			waitFor.Clear();
			for (int num4 = 0; num4 < _Action.BlueprintsFullUnlock.Length; num4++)
			{
				this.StartCoroutineEx(this.MakeBlueprintAvailableRoutine(_Action.BlueprintsFullUnlock[num4]), out controller);
			}
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
		}
		if (collectionToDrop != null && collectionToDrop.DroppedEncounter)
		{
			this.GameGraphics.EncounterPopupWindow.StartEncounter(collectionToDrop.DroppedEncounter, this.CurrentSaveData.HasEncounterData);
			while (this.GameGraphics.EncounterPopupWindow.OngoingEncounter)
			{
				yield return null;
			}
			this.StartCoroutineEx(this.CheckAllStatsForActions(), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
		}
		this.StartCoroutineEx(this.SpendDaytimePoints(0, checkForStats, false, false, null, FadeToBlackTypes.None, "", false, false, null, null, null), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		if (_Action.ResetWhenDone && this.StatsDict.ContainsKey(_Action.ResetWhenDone))
		{
			this.StartCoroutineEx(this.ChangeStat(this.StatsDict[_Action.ResetWhenDone].ResetBaseValue, StatModification.Permanent, StatModifierReport.SourceFromAction(_Action, _ReceivingCard), null, 0, null, null, true), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
		}
		if (this.RootAction == _Action && !isNotFirstRootAction)
		{
			CardData playingEvent = null;
			bool noSpecialCase = false;
			while (this.EventCardQueue.Count > 0)
			{
				playingEvent = this.EventCardQueue[0];
				if (this.EventCardQueue[0].CanSpawnOnBoard())
				{
					yield return base.StartCoroutine(this.AddCard(playingEvent, null, true, null, false, SpawningLiquid.Empty, new Vector2Int(this.CurrentTickInfo.z, 0), true));
					noSpecialCase = (this.CurrentEventCard != null);
					if (noSpecialCase)
					{
						noSpecialCase = (this.CurrentEventCard.CardModel == playingEvent || this.EventAction != null);
					}
					else
					{
						noSpecialCase = (this.EventAction != null);
					}
					while (noSpecialCase)
					{
						noSpecialCase = (this.CurrentEventCard != null);
						if (noSpecialCase)
						{
							noSpecialCase = (this.CurrentEventCard.CardModel == playingEvent || this.EventAction != null);
						}
						else
						{
							noSpecialCase = (this.EventAction != null);
						}
						yield return null;
					}
				}
				this.EventCardQueue.Remove(playingEvent);
				yield return null;
			}
			this.StartCoroutineEx(this.CheckAllStatsForActions(), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			playingEvent = null;
		}
		if (this.NotInBase && this.CurrentOutOfBaseAction == _Action)
		{
			this.NotInBase = false;
			this.CurrentOutOfBaseAction = null;
			this.WillCheckForPassiveEffects = true;
			waitFor.Clear();
			for (int num5 = 0; num5 < this.AllStats.Count; num5++)
			{
				this.StartCoroutineEx(this.UpdateStatStatuses(this.AllStats[num5], this.AllStats[num5].CurrentValue(true), null), out controller);
				waitFor.Add(controller);
			}
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
		}
		this.StartCoroutineEx(this.UpdatePassiveEffects(), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		actionReport.EndAction(this.ActionCancelled, this.CurrentTickInfo.z, this.CurrentMiniTicks, this.CurrentActionCounter, Time.frameCount);
		Action<ActionReport> onActionPerformed = GameManager.OnActionPerformed;
		if (onActionPerformed != null)
		{
			onActionPerformed(actionReport);
		}
		num = this.CurrentActionCounter;
		this.CurrentActionCounter = num + 1;
		this.UpdateObjectivesCompletion();
		this.SaveAfterAction |= _Action.SaveGame;
		if (_Action.HasLog)
		{
			this.AddEndgameLog(_Action.ActionLog);
		}
		if (!VictoryMenu.IsVictory && _Action.VictorySettings.Victory)
		{
			int num6 = (this.VictorySunsPerDay > 0f) ? Mathf.FloorToInt(this.VictorySunsPerDay * (float)this.CurrentTickInfo.x) : -1;
			int num7 = (this.VictoryMoonsPerDay > 0f) ? Mathf.FloorToInt(this.VictoryMoonsPerDay * (float)this.CurrentTickInfo.x) : -1;
			if (num6 > 0)
			{
				GameLoad.Instance.SaveData.Suns += num6;
			}
			if (num7 > 0)
			{
				GameLoad.Instance.SaveData.Moons += num7;
			}
			MBSingleton<EndgameMenu>.Instance.Setup(_Action.VictorySettings.VictoryMessage, num6, num7, this.CurrentGameData, false, false, true, _Action.VictorySettings.SpecialEnding, true);
		}
		while (EndgameMenu.IsVictory)
		{
			yield return null;
		}
		if (this.RootAction == _Action && !isNotFirstRootAction)
		{
			if (this.IgnoredBaseRowCards.Count > 0)
			{
				for (int num8 = this.IgnoredBaseRowCards.Count - 1; num8 >= 0; num8--)
				{
					this.IgnoredBaseRowCards[num8].IgnoreBaseRow = false;
					this.IgnoredBaseRowCards.RemoveAt(num8);
				}
			}
			if (startedAtDay != this.CurrentTickInfo.x)
			{
				GameLoad.Instance.SaveData.Suns++;
				if (this.CurrentTickInfo.x % this.DaysPerMoon == 0)
				{
					GameLoad.Instance.SaveData.Moons++;
				}
			}
			if ((startedAtDay != this.CurrentTickInfo.x || this.SaveAfterAction) && this.AutoSaveTicks > 0)
			{
				GameLoad.Instance.AutoSaveGame(this.IsSafeMode);
			}
			else if (this.AutoSaveTicks > 1)
			{
				if (startingActionTick < this.CurrentTickInfo.z)
				{
					int num9 = startingActionTick - this.CurrentTickInfo.x * this.DaySettings.DailyPoints;
					int num10 = this.CurrentTickInfo.z - this.CurrentTickInfo.x * this.DaySettings.DailyPoints;
					for (int num11 = num9 + 1; num11 <= num10; num11++)
					{
						if (num11 != 0 && num11 % this.AutoSaveTicks == 0)
						{
							GameLoad.Instance.AutoSaveGame(false);
							break;
						}
					}
				}
			}
			else if (this.AutoSaveTicks == 1)
			{
				GameLoad.Instance.AutoSaveGame(false);
			}
			if (this.FinishedBlueprintResearch)
			{
				this.GameGraphics.BlueprintResearched.Show(this.FinishedBlueprintResearch);
				while (this.GameGraphics.BlueprintResearched.gameObject.activeInHierarchy)
				{
					yield return null;
				}
				this.FinishedBlueprintResearch = null;
			}
			this.RootAction = null;
		}
		if (this.EventAction == _Action)
		{
			this.EventAction = null;
		}
		if (this.QueuedCardActions.Count != 0 && this.QueuedCardActions[0].OnCard == _ReceivingCard && this.QueuedCardActions[0].Action == _Action)
		{
			this.QueuedCardActions.RemoveAt(0);
		}
		this.CurrentGameState = GameStates.SELECT;
		if (_ReceivingCard && !this.ActionCancelled && collectionToDrop != null && _ReceivingCard.CardModel && !_ReceivingCard.Destroyed && collectionToDrop.RevealInventory && _ReceivingCard.CardModel.InventoryIsHidden)
		{
			this.GameGraphics.InspectCard(_ReceivingCard, true);
		}
		if (this.CardToInspect != null && this.RootAction == null)
		{
			this.GameGraphics.InspectCard(this.CardToInspect, false);
			this.CardToInspect = null;
		}
		yield break;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000076B4 File Offset: 0x000058B4
	public void AddEndgameLog(EndgameLog _Log)
	{
		if (this.AutoSolveEvents || this.DontCheckObjectivesYet)
		{
			return;
		}
		Debug.Log("Adding log: " + _Log.LogText);
		this.CurrentGameData.AllEndgameLogs.Add(new LogSaveData(_Log, this.CurrentTickInfo.z));
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00007710 File Offset: 0x00005910
	public void UpdateObjectivesCompletion(bool _Instant)
	{
		if (!_Instant)
		{
			this.ObjectivesUpdateRequested = true;
			return;
		}
		this.UpdateObjectivesCompletion();
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00007724 File Offset: 0x00005924
	private void UpdateObjectivesCompletion()
	{
		if (this.DontCheckObjectivesYet)
		{
			return;
		}
		if (Application.isEditor)
		{
			this.GetGuide.FillAllPagesList();
			this.GetJournal.FillAllPagesList();
		}
		for (int i = 0; i < this.AllPerks.Count; i++)
		{
			this.AllPerks[i].CheckForCompletion(false);
		}
		if (this.HiddenObjectives == null)
		{
			this.HiddenObjectives = new List<Objective>();
		}
		else
		{
			this.HiddenObjectives.Clear();
		}
		for (int j = 0; j < this.AllObjectives.Count; j++)
		{
			if (this.AllObjectives[j].NotifiedWhenHidden)
			{
				this.HiddenObjectives.Add(this.AllObjectives[j]);
			}
			this.AllObjectives[j].CheckForCompletion(false);
		}
		for (int k = 1; k < 2; k++)
		{
			for (int l = 0; l < this.AllObjectives.Count; l++)
			{
				this.AllObjectives[l].CheckForCompletion(false);
			}
			for (int m = 0; m < this.AllPerks.Count; m++)
			{
				this.AllPerks[m].CheckForCompletion(false);
			}
		}
		for (int n = 0; n < this.HiddenObjectives.Count; n++)
		{
			if (this.HiddenObjectives[n].Complete)
			{
				this.HiddenObjectives[n].CheckForCompletion(false);
			}
		}
		if (!this.IsInitializing || this.AutoSolveEvents)
		{
			bool flag = false;
			for (int num = 0; num < this.UnlockableCards.Count; num++)
			{
				if (this.UnlockableCards[num].UnlockedCard != null)
				{
					flag = (this.UnlockableCards[num].UnlockedCard.CardType == CardTypes.EnvImprovement);
				}
				if ((!this.UnlockableCards[num].IsUnlocked() || flag) && (!flag || (this.CurrentExplorableCard && this.CurrentExplorableCard.CardModel && this.CurrentExplorableCard.CardModel.HasImprovement(this.UnlockableCards[num].UnlockedCard) && !this.CardIsOnBoard(this.UnlockableCards[num].UnlockedCard, false, true, false, false, null, Array.Empty<InGameCardBase>()) && !(this.CurrentEnvironmentCard == null) && !(this.NextEnvironment != this.CurrentEnvironmentCard.CardModel))) && (this.UnlockableCards[num].CheckForCompletion() || (flag && this.UnlockableCards[num].IsUnlocked())) && !this.CardsAboutToBeUnlocked.Contains(this.UnlockableCards[num].UnlockedCard))
				{
					this.CardsAboutToBeUnlocked.Add(this.UnlockableCards[num].UnlockedCard);
					this.ObjectiveActionsQueue.Add(this.UnlockableCards[num].UnlockAction);
				}
			}
		}
		MBSingleton<TutorialManager>.Instance.UpdateTutorials();
		if (!this.PerformingObjectiveActions)
		{
			base.StartCoroutine(this.PerformObjectiveActions());
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00007A65 File Offset: 0x00005C65
	public void CancelAction()
	{
		this.ActionCancelled = true;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00007A6E File Offset: 0x00005C6E
	private IEnumerator PerformObjectiveActions()
	{
		this.PerformingObjectiveActions = true;
		List<CoroutineController> waitFor = new List<CoroutineController>();
		for (int i = 0; i < this.ObjectiveActionsQueue.Count; i++)
		{
			CoroutineController item;
			this.StartCoroutineEx(GameManager.PerformActionAsEnumerator(this.ObjectiveActionsQueue[i], null, this.AutoSolveEvents), out item);
			waitFor.Add(item);
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		this.ObjectiveActionsQueue.Clear();
		this.CardsAboutToBeUnlocked.Clear();
		this.PerformingObjectiveActions = false;
		yield break;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00007A80 File Offset: 0x00005C80
	public void PassDay()
	{
		if (!this.TimeOptions)
		{
			if (this.CurrentGameState == GameStates.SELECT && this.DayTimePoints > 0)
			{
				Debug.Log("passing day");
				base.StartCoroutine(this.SpendDaytimePoints(Mathf.Min(1, this.DayTimePoints), true, true, false, null, FadeToBlackTypes.None, "", false, false, null, null, null));
			}
			return;
		}
		if (this.GameGraphics.InspectedCard != null && this.GameGraphics.InspectedCard.CardModel.CardType == CardTypes.Event)
		{
			return;
		}
		if (GameManager.PerformingAction || this.GameGraphics.CannotCloseCurrentPopup)
		{
			return;
		}
		this.GameGraphics.CloseAllPopups();
		this.GameGraphics.CurrentInspectionPopup = this.GameGraphics.CardInspectionPopup;
		this.GameGraphics.CardInspectionPopup.Setup(this.TimeOptions);
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00007B58 File Offset: 0x00005D58
	private IEnumerator SpendDaytimePoints(int _Amt, bool _CheckForStatTriggers, bool _CheckForTimeOfDayMods, bool _HideWheel, InGameCardBase _FromCard, FadeToBlackTypes _FadeToBlack, string _FadeText, bool _Cancellable, bool _FadeTips, StatModifier[] _ExtraStatMods, OverTimeDurabilityChanges _DurabilitiesMods, StatInterruptionCondition[] _StatInterrupts)
	{
		CoroutineController timeSpentAnim = null;
		List<CoroutineController> waitFor = new List<CoroutineController>();
		Transform statsFeedbackSource = null;
		if (_FromCard)
		{
			if (_FromCard.CardVisuals)
			{
				statsFeedbackSource = _FromCard.CardVisuals.StatFeedbacksParent;
			}
			else if (_FromCard.IsLiquid && _FromCard.CurrentContainer)
			{
				if (_FromCard.CurrentContainer.CardVisuals)
				{
					statsFeedbackSource = _FromCard.CurrentContainer.CardVisuals.StatFeedbacksParent;
				}
				else
				{
					statsFeedbackSource = this.GameGraphics.FadeToBlack.TimeSpentPosTr;
				}
			}
			else
			{
				statsFeedbackSource = this.GameGraphics.FadeToBlack.TimeSpentPosTr;
			}
		}
		else if (_FadeToBlack != FadeToBlackTypes.None)
		{
			statsFeedbackSource = this.GameGraphics.FadeToBlack.TimeSpentPosTr;
		}
		if (_FromCard)
		{
			if (_Amt > 0 && !_HideWheel)
			{
				if (!_FromCard.IsLiquid)
				{
					_FromCard.TimeAnimate(this.GameGraphics.GetTimeSpentAnimDuration(_Amt, _FadeToBlack > FadeToBlackTypes.None) + Time.fixedDeltaTime * 2f);
				}
				else
				{
					_FromCard.CurrentContainer.TimeAnimate(this.GameGraphics.GetTimeSpentAnimDuration(_Amt, _FadeToBlack > FadeToBlackTypes.None) + Time.fixedDeltaTime * 2f);
				}
			}
			_FromCard.IsPerformingAction = true;
		}
		if (_ExtraStatMods != null && _ExtraStatMods.Length != 0)
		{
			this.GameGraphics.ScrollToTopOfStats();
		}
		int num;
		for (int t = 0; t < Mathf.Max(_Amt, 1); t = num + 1)
		{
			while (this.QueuedCardActions.Count > 0 && t > 0)
			{
				yield return null;
			}
			CoroutineController controller;
			if (_DurabilitiesMods != null)
			{
				this.StartCoroutineEx(this.ChangeCardDurabilities(_FromCard, _DurabilitiesMods.SpoilagePerTick, _DurabilitiesMods.UsagePerTick, _DurabilitiesMods.FuelPerTick, _DurabilitiesMods.ProgressPerTick, _DurabilitiesMods.LiquidPerTick, _DurabilitiesMods.Special1PerTick, _DurabilitiesMods.Special2PerTick, _DurabilitiesMods.Special3PerTick, _DurabilitiesMods.Special4PerTick, true, true), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
			}
			waitFor.Clear();
			if (_ExtraStatMods != null && t == 0)
			{
				for (int j = 0; j < _ExtraStatMods.Length; j++)
				{
					if (_ExtraStatMods[j].InstantModifier)
					{
						this.StartCoroutineEx(this.ChangeStat(_ExtraStatMods[j], StatModification.Permanent, _ExtraStatMods[j].ReportSource, _ExtraStatMods[j].ActionSource, _ExtraStatMods[j].ActionTick, null, statsFeedbackSource, false), out controller);
						waitFor.Add(controller);
					}
				}
			}
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
			this.WillCheckForPassiveEffects = true;
			if (_Amt > 0)
			{
				if (!_FromCard)
				{
					this.CurrentGameState = GameStates.PLAYINGCARD;
				}
				this.DayTimePoints--;
				this.CurrentActionCounter = 0;
				this.SaveTickInfo();
				if (!_HideWheel)
				{
					this.StartCoroutineEx(this.GameGraphics.UpdateSpendingTime(t + 1, _Amt, _FromCard, _FadeToBlack, _FadeText, _Cancellable, _FadeTips), out timeSpentAnim);
				}
				CoroutineController explorationAnim;
				if (_FromCard)
				{
					if (_FromCard.CardModel)
					{
						if (_FromCard.CardModel.CardType == CardTypes.Explorable && MBSingleton<ExplorationPopup>.Instance.gameObject.activeInHierarchy)
						{
							this.StartCoroutineEx(MBSingleton<ExplorationPopup>.Instance.AddTickToExploration(), out explorationAnim);
						}
						else
						{
							explorationAnim = null;
						}
					}
					else
					{
						explorationAnim = null;
					}
				}
				else
				{
					explorationAnim = null;
				}
				this.StartCoroutineEx(this.ApplyRates(1, 0), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
				this.StartCoroutineEx(this.ProgressCurrentResearch(), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
				if (!_HideWheel)
				{
					while (timeSpentAnim.state != CoroutineState.Finished)
					{
						yield return null;
					}
				}
				if (explorationAnim != null)
				{
					while (explorationAnim.state != CoroutineState.Finished)
					{
						yield return null;
					}
				}
				this.GameGraphics.UpdateTimeInfo(false);
				if (this.DayTimePoints <= 0)
				{
					if (_CheckForStatTriggers)
					{
						yield return base.StartCoroutine(this.CheckAllStatsForActions());
					}
					this.GameGraphics.UpdateTimeInfo(false);
					this.NightRoutine(!_FromCard, _CheckForStatTriggers);
				}
				if (8 <= this.CurrentDay)
				{
				}
				IL_677:
				if (_StatInterrupts != null && _StatInterrupts.Length != 0)
				{
					int k = 0;
					while (k < _StatInterrupts.Length)
					{
						if (this.StatsDict.ContainsKey(_StatInterrupts[k].Stat) && _StatInterrupts[k].IsInRange(this.StatsDict[_StatInterrupts[k].Stat].CurrentValue(this.NotInBase)))
						{
							this.ActionCancelled = true;
							if (string.IsNullOrEmpty(_StatInterrupts[k].Notification))
							{
								break;
							}
							if (_FromCard)
							{
								this.GameGraphics.PlayCardNotification(_FromCard, _StatInterrupts[k].Notification);
								break;
							}
							this.GameGraphics.PlayMessage(this.GameGraphics.TimeSpentWheel.transform.position, _StatInterrupts[k].Notification, this.GameGraphics.TimeSpentWheel.transform);
							break;
						}
						else
						{
							k++;
						}
					}
				}
				if (this.ActionCancelled)
				{
					this.CancelledAtTick = t + 1;
					this.GameGraphics.CancelWheel();
					if (!_FromCard)
					{
						break;
					}
					if (!_FromCard.IsLiquid)
					{
						_FromCard.CancelTimeAnimation();
						break;
					}
					_FromCard.CurrentContainer.CancelTimeAnimation();
					break;
				}
			}
			this.StartCoroutineEx(this.CheckStatsForTimeOfDayMods(_CheckForTimeOfDayMods), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			base.StartCoroutine(this.GameGraphics.UpdateStatusAlerts());
			if (_CheckForStatTriggers)
			{
				yield return base.StartCoroutine(this.CheckAllStatsForActions());
			}
			this.UpdateObjectivesCompletion();
			waitFor.Clear();
			if (_ExtraStatMods != null)
			{
				for (int l = 0; l < _ExtraStatMods.Length; l++)
				{
					if (!_ExtraStatMods[l].InstantModifier)
					{
						this.StartCoroutineEx(this.ChangeStat(_ExtraStatMods[l], StatModification.Permanent, _ExtraStatMods[l].ReportSource, _ExtraStatMods[l].ActionSource, _ExtraStatMods[l].ActionTick, null, statsFeedbackSource, false), out controller);
						waitFor.Add(controller);
					}
				}
			}
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
			if (_Amt > 0)
			{
				this.StartCoroutineEx(this.UpdatePassiveEffects(), out controller);
			}
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			num = t;
		}
		if (!_FromCard)
		{
			this.CurrentGameState = GameStates.SELECT;
		}
		else
		{
			_FromCard.IsPerformingAction = false;
		}
		yield break;
		IL_658:
		yield return null;
		if (EndgameMenu.IsVictory)
		{
			goto IL_658;
		}
		goto IL_677;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00007BD0 File Offset: 0x00005DD0
	private void NightRoutine(bool _FromButton, bool _CheckForStatTriggers)
	{
		base.StartCoroutine(this.GameGraphics.DayOverPopup(this.CurrentDay + 1));
		int currentDay = this.CurrentDay;
		this.CurrentDay = currentDay + 1;
		this.DayTimePoints = this.DaySettings.DailyPoints;
		this.SaveTickInfo();
		this.GameGraphics.UpdateTimeInfo(false);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00007C2A File Offset: 0x00005E2A
	private IEnumerator ProgressCurrentResearch()
	{
		if (!this.GameGraphics.BlueprintModelsPopup.CurrentResearch)
		{
			yield break;
		}
		if (!this.BlueprintResearchTimes.ContainsKey(this.GameGraphics.BlueprintModelsPopup.CurrentResearch))
		{
			this.BlueprintResearchTimes.Add(this.GameGraphics.BlueprintModelsPopup.CurrentResearch, 0);
		}
		Dictionary<CardData, int> blueprintResearchTimes = this.BlueprintResearchTimes;
		CardData currentResearch = this.GameGraphics.BlueprintModelsPopup.CurrentResearch;
		int num = blueprintResearchTimes[currentResearch];
		blueprintResearchTimes[currentResearch] = num + 1;
		if (this.BlueprintResearchTimes[this.GameGraphics.BlueprintModelsPopup.CurrentResearch] >= GameManager.DaysToTicks(this.GameGraphics.BlueprintModelsPopup.CurrentResearch.BlueprintUnlockSunsCost))
		{
			this.FinishedBlueprintResearch = this.GameGraphics.BlueprintModelsPopup.CurrentResearch;
			this.GameGraphics.BlueprintModelsPopup.FinishBlueprintResearch();
		}
		yield break;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00007C39 File Offset: 0x00005E39
	private IEnumerator ApplyRates(int _TimePoints, int _CatchupTick)
	{
		InGameCardBase[] cardsWithCountersArray = this.CardsWithCounters.ToArray();
		InGameCardBase[] array = this.CardsWithCooking.ToArray();
		InGameCardBase[] allCardsArray = this.AllCards.ToArray();
		List<CoroutineController> waitFor = new List<CoroutineController>();
		bool catchupMode = _CatchupTick > 0;
		array = this.CardsWithCooking.ToArray();
		allCardsArray = this.AllCards.ToArray();
		waitFor.Clear();
		for (int j = array.Length - 1; j >= 0; j--)
		{
			if (!catchupMode || !array[j].IndependentFromEnv)
			{
				CoroutineController item;
				this.StartCoroutineEx(this.UpdateCardCooking(array[j], _TimePoints), out item);
				waitFor.Add(item);
			}
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		waitFor.Clear();
		for (int k = 0; k < this.AllCounters.Count; k++)
		{
			this.AllCounters[k].Updated = false;
		}
		for (int l = 0; l < cardsWithCountersArray.Length; l++)
		{
			if (cardsWithCountersArray[l] && cardsWithCountersArray[l].CardModel && !cardsWithCountersArray[l].Destroyed && (!catchupMode || !cardsWithCountersArray[l].IndependentFromEnv) && !cardsWithCountersArray[l].MarkedAsBlueprintIngredient && (!cardsWithCountersArray[l].CardModel || cardsWithCountersArray[l].CardModel.CardType != CardTypes.EnvImprovement || cardsWithCountersArray[l].BlueprintComplete) && (!catchupMode || _CatchupTick > cardsWithCountersArray[l].CreatedInSaveDataTick) && cardsWithCountersArray[l].CardModel.ActiveCounters != null && cardsWithCountersArray[l].CardModel.ActiveCounters.Length != 0)
			{
				for (int m = 0; m < cardsWithCountersArray[l].CardModel.ActiveCounters.Length; m++)
				{
					if (cardsWithCountersArray[l].CardModel.ActiveCounters[m] != null && !this.CountersDict[cardsWithCountersArray[l].CardModel.ActiveCounters[m]].Updated)
					{
						if (!catchupMode)
						{
							this.CountersDict[cardsWithCountersArray[l].CardModel.ActiveCounters[m]].Value += _TimePoints;
						}
						this.CountersDict[cardsWithCountersArray[l].CardModel.ActiveCounters[m]].Updated = true;
					}
				}
			}
		}
		for (int n = allCardsArray.Length - 1; n >= 0; n--)
		{
			if (allCardsArray[n] && allCardsArray[n].CardModel && !allCardsArray[n].Destroyed && (!catchupMode || !allCardsArray[n].IndependentFromEnv) && !allCardsArray[n].MarkedAsBlueprintIngredient && (!allCardsArray[n].CardModel || allCardsArray[n].CardModel.CardType != CardTypes.EnvImprovement || allCardsArray[n].BlueprintComplete) && (!catchupMode || _CatchupTick > allCardsArray[n].CreatedInSaveDataTick))
			{
				allCardsArray[n].UpdatePassiveEffectStacks();
				allCardsArray[n].UpdateProducedLiquids();
				if (allCardsArray[n].CurrentSlotInfo != null)
				{
					if (allCardsArray[n].CurrentSlotInfo.SlotType == SlotsTypes.Base)
					{
						if (allCardsArray[n].IgnoreBaseRow)
						{
							if (this.IgnoredBaseRowCards.Contains(allCardsArray[n]))
							{
								allCardsArray[n].IgnoreBaseRow = false;
								this.IgnoredBaseRowCards.Remove(allCardsArray[n]);
							}
							else
							{
								this.IgnoredBaseRowCards.Add(allCardsArray[n]);
							}
						}
					}
					else
					{
						allCardsArray[n].IgnoreBaseRow = true;
					}
				}
				else
				{
					allCardsArray[n].IgnoreBaseRow = true;
				}
				if (!allCardsArray[n].IgnoreTickDurabilityChanges)
				{
					CoroutineController item;
					this.StartCoroutineEx(this.ChangeCardDurabilities(allCardsArray[n], allCardsArray[n].CurrentSpoilageRate * (float)_TimePoints, allCardsArray[n].CurrentUsageRate * (float)_TimePoints, allCardsArray[n].CurrentFuelRate * (float)_TimePoints, allCardsArray[n].CurrentConsumableRate * (float)_TimePoints, allCardsArray[n].CurrentEvaporationRate * (float)_TimePoints, allCardsArray[n].CurrentSpecial1Rate * (float)_TimePoints, allCardsArray[n].CurrentSpecial2Rate * (float)_TimePoints, allCardsArray[n].CurrentSpecial3Rate * (float)_TimePoints, allCardsArray[n].CurrentSpecial4Rate * (float)_TimePoints, !catchupMode, false), out item);
					waitFor.Add(item);
				}
			}
		}
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		waitFor.Clear();
		EnvironmentSaveData environmentSaveData = (this.CatchingUpEnvData != null) ? this.CatchingUpEnvData : this.CurrentEnvData(true);
		for (int num2 = 0; num2 < this.AllCounters.Count; num2++)
		{
			if (environmentSaveData.CountersDict[this.AllCounters[num2].Model].Value < this.AllCounters[num2].Value)
			{
				if (!this.AllCounters[num2].Updated)
				{
					environmentSaveData.CountersDict[this.AllCounters[num2].Model].Value = Mathf.Min(environmentSaveData.CountersDict[this.AllCounters[num2].Model].Value + _TimePoints, this.AllCounters[num2].Value);
				}
				else
				{
					environmentSaveData.CountersDict[this.AllCounters[num2].Model].Value = this.AllCounters[num2].Value;
				}
			}
		}
		if (catchupMode)
		{
			yield break;
		}
		for (int num3 = 0; num3 < this.AllStats.Count; num3++)
		{
			this.AllStats[num3].UpdateTicks();
			CoroutineController item;
			this.StartCoroutineEx(this.ChangeStatValue(this.AllStats[num3], this.AllStats[num3].CurrentRatePerTick(this.NotInBase) * (float)_TimePoints, StatModification.Permanent), out item);
			waitFor.Add(item);
		}
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00007C56 File Offset: 0x00005E56
	private IEnumerator ApplyStatModifiers(StatModifier[] _Mods, string _Action, int _ActionTick, Transform _FeedbackSource, float _CompletePercentage, string _Source)
	{
		if (_Mods == null)
		{
			yield break;
		}
		if (_Mods.Length == 0)
		{
			yield break;
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		this.GameGraphics.ScrollToTopOfStats();
		for (int j = 0; j < _Mods.Length; j++)
		{
			CoroutineController item;
			this.StartCoroutineEx(this.ChangeStat(_Mods[j] * _CompletePercentage, StatModification.Permanent, _Source, _Action, _ActionTick, _FeedbackSource, null, true), out item);
			waitFor.Add(item);
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00007C92 File Offset: 0x00005E92
	public void CalculateCarryWeight()
	{
		if (!this.InGamePlayerWeight)
		{
			return;
		}
		if (this.WillCalculateCarryWeight)
		{
			return;
		}
		base.StartCoroutine(this.CalculateCarryWeightRoutine());
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00007CB8 File Offset: 0x00005EB8
	private IEnumerator CalculateCarryWeightRoutine()
	{
		this.WillCalculateCarryWeight = true;
		yield return new WaitForEndOfFrame();
		List<InGameCardBase> handCards = this.GameGraphics.GetHandCards(false);
		handCards.AddRange(this.GameGraphics.GetEquippedCards(false));
		float num = this.InGamePlayerWeight.CurrentValue(this.NotInBase);
		float num2 = 0f;
		for (int i = 0; i < handCards.Count; i++)
		{
			num2 += handCards[i].CurrentWeight;
		}
		if (!Mathf.Approximately(num, num2))
		{
			this.ApplyStatModifier(new StatModifier
			{
				Stat = this.PlayerWeightStat,
				ValueModifier = Vector2.one * (num2 - num)
			}, StatModification.Permanent, "Carried Weight", null);
		}
		this.WillCalculateCarryWeight = false;
		yield break;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00007CC7 File Offset: 0x00005EC7
	public void CalculateEnvironmentWeight(bool _Instant)
	{
		if (this.MaxEnvWeight == 0f)
		{
			this.CurrentEnvWeight = 0f;
			return;
		}
		if (this.WillCalculateEnvironmentWeight)
		{
			return;
		}
		base.StartCoroutine(this.CalculateEnvWeightRoutine(_Instant));
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00007CF9 File Offset: 0x00005EF9
	private IEnumerator CalculateEnvWeightRoutine(bool _Instant)
	{
		this.WillCalculateEnvironmentWeight = true;
		if (!_Instant)
		{
			yield return new WaitForEndOfFrame();
		}
		List<InGameCardBase> locationCards = this.GameGraphics.GetLocationCards(false, false);
		locationCards.AddRange(this.GameGraphics.GetBaseCards(false, false));
		this.CurrentEnvWeight = 0f;
		for (int i = 0; i < locationCards.Count; i++)
		{
			this.CurrentEnvWeight += locationCards[i].CurrentWeight;
		}
		if (this.CurrentExplorableCard && this.CurrentExplorableCard.CardVisuals)
		{
			this.CurrentExplorableCard.CardVisuals.UpdateInventoryInfo();
		}
		this.WillCalculateEnvironmentWeight = false;
		yield break;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00007D10 File Offset: 0x00005F10
	public CoroutineController ApplyStatModifier(StatModifier _Stat, StatModification _Modification, string _From, object _Source)
	{
		CoroutineController result;
		this.StartCoroutineEx(this.ChangeStat(_Stat, _Modification, _From, null, -1, _Source, null, true), out result);
		return result;
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00007D36 File Offset: 0x00005F36
	private IEnumerator CheckStatsForTimeOfDayMods(bool _DoAll)
	{
		if (this.StatsWithTimeOfDayMods.Count == 0 && this.StatsWithTimeOfDayModsAndRequirements.Count == 0)
		{
			yield break;
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		for (int j = 0; j < this.StatsWithTimeOfDayModsAndRequirements.Count; j++)
		{
			CoroutineController item;
			this.StartCoroutineEx(this.UpdateTimeOfDayMods(this.StatsWithTimeOfDayModsAndRequirements[j]), out item);
			waitFor.Add(item);
		}
		if (_DoAll)
		{
			for (int k = 0; k < this.StatsWithTimeOfDayMods.Count; k++)
			{
				CoroutineController item;
				this.StartCoroutineEx(this.UpdateTimeOfDayMods(this.StatsWithTimeOfDayMods[k]), out item);
				waitFor.Add(item);
			}
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00007D4C File Offset: 0x00005F4C
	private IEnumerator UpdateTimeOfDayMods(InGameStat _Stat)
	{
		List<Vector2Int> alreadyActive = new List<Vector2Int>();
		float hourOfTheDay = Mathf.Repeat(GameManager.HourOfTheDayValue(this.DayTimePoints, 0), 24f);
		List<CoroutineController> waitFor = new List<CoroutineController>();
		if (_Stat.CurrentTimeOfDayMods.Count > 0)
		{
			int num = _Stat.CurrentTimeOfDayMods.Count - 1;
			while (num >= 0 && _Stat.CurrentTimeOfDayMods.Count > 0)
			{
				if (!_Stat.CurrentTimeOfDayMods[num].EffectIsActive(hourOfTheDay))
				{
					StatModifier inverse = _Stat.CurrentTimeOfDayMods[num].ToStatMod(_Stat.StatModel).Inverse;
					string statusName = string.Concat(new object[]
					{
						"Time of day is between ",
						_Stat.CurrentTimeOfDayMods[num].StartingHour,
						" and ",
						_Stat.CurrentTimeOfDayMods[num].EndingHour
					});
					TimeOfDayStatModSource toSource = _Stat.CurrentTimeOfDayMods[num].ToSource;
					_Stat.CurrentTimeOfDayMods.RemoveAt(num);
					CoroutineController item;
					this.StartCoroutineEx(this.ChangeStat(inverse, StatModification.GlobalModifier, StatModifierReport.SourceFromStatStatus(statusName, _Stat, false), null, -1, toSource, null, true), out item);
					waitFor.Add(item);
				}
				else
				{
					alreadyActive.Add(_Stat.CurrentTimeOfDayMods[num].TimeRange);
				}
				num--;
			}
		}
		int num2;
		for (int i = 0; i < waitFor.Count; i = num2 + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num2 = i;
		}
		waitFor.Clear();
		for (int j = 0; j < _Stat.StatModel.TimeOfDayMods.Length; j++)
		{
			if (_Stat.StatModel.TimeOfDayMods[j].EffectIsActive(hourOfTheDay) && !alreadyActive.Contains(_Stat.StatModel.TimeOfDayMods[j].TimeRange))
			{
				TimeOfDayStatModSource toSource = _Stat.StatModel.TimeOfDayMods[j].ToSource;
				_Stat.CurrentTimeOfDayMods.Add(_Stat.StatModel.TimeOfDayMods[j].Instantiate());
				alreadyActive.Add(_Stat.StatModel.TimeOfDayMods[j].TimeRange);
				CoroutineController item;
				this.StartCoroutineEx(this.ChangeStat(_Stat.CurrentTimeOfDayMods[_Stat.CurrentTimeOfDayMods.Count - 1].ToStatMod(_Stat.StatModel), StatModification.GlobalModifier, StatModifierReport.SourceFromStatStatus(string.Concat(new object[]
				{
					"Time of day is between ",
					_Stat.CurrentTimeOfDayMods[_Stat.CurrentTimeOfDayMods.Count - 1].StartingHour,
					" and ",
					_Stat.CurrentTimeOfDayMods[_Stat.CurrentTimeOfDayMods.Count - 1].EndingHour
				}), _Stat, true), null, -1, toSource, null, true), out item);
				waitFor.Add(item);
			}
		}
		for (int i = 0; i < waitFor.Count; i = num2 + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num2 = i;
		}
		yield break;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00007D62 File Offset: 0x00005F62
	private IEnumerator CheckAllStatsForActions()
	{
		List<CoroutineController> waitFor = new List<CoroutineController>();
		List<InGameCardBase> list = new List<InGameCardBase>();
		List<SelfTriggeredAction> list2 = new List<SelfTriggeredAction>();
		for (int j = 0; j < this.AllStats.Count; j++)
		{
			if (this.AllStats[j].CardValueListeners != null && this.AllStats[j].CardValueListeners.Count > 0)
			{
				for (int k = this.AllStats[j].CardValueListeners.Count - 1; k >= 0; k--)
				{
					if (this.AllStats[j].CardValueListeners[k] && (!this.AllStats[j].CardValueListeners[k].CardModel || this.AllStats[j].CardValueListeners[k].CardModel.CardType != CardTypes.EnvImprovement || this.AllStats[j].CardValueListeners[k].BlueprintComplete))
					{
						InGameCardBase inGameCardBase = this.AllStats[j].CardValueListeners[k];
						if (!list.Contains(inGameCardBase))
						{
							list.Add(inGameCardBase);
						}
						this.CheckForStatTriggeredActions(this.AllStats[j], inGameCardBase.CardModel.OnStatsChangeActions, inGameCardBase.StatTriggeredActions, inGameCardBase);
					}
				}
			}
			if (this.AllStats[j].ActionsValueListeners != null && this.AllStats[j].ActionsValueListeners.Count > 0)
			{
				for (int l = this.AllStats[j].ActionsValueListeners.Count - 1; l >= 0; l--)
				{
					if (this.AllStats[j].ActionsValueListeners[l])
					{
						SelfTriggeredAction selfTriggeredAction = this.AllStats[j].ActionsValueListeners[l];
						if (!list2.Contains(selfTriggeredAction))
						{
							list2.Add(selfTriggeredAction);
						}
						this.CheckForStatTriggeredActions(this.AllStats[j], selfTriggeredAction.Actions, selfTriggeredAction.StatTriggeredActions, null);
					}
				}
			}
		}
		int num = 0;
		while (num < list.Count || num < list2.Count)
		{
			if (num < list.Count)
			{
				for (int m = 0; m < list[num].StatTriggeredActions.Length; m++)
				{
					if (list[num].StatTriggeredActions[m].ReadyToPlay)
					{
						list[num].StatTriggeredActions[m].PlayAction();
						CoroutineController item;
						this.StartCoroutineEx(this.ActionRoutine(list[num].CardModel.OnStatsChangeActions[m], list[num], false, false), out item);
						waitFor.Add(item);
						if (list[num].Destroyed)
						{
							break;
						}
					}
				}
			}
			if (num < list2.Count)
			{
				for (int n = 0; n < list2[num].StatTriggeredActions.Length; n++)
				{
					if (list2[num].StatTriggeredActions[n].ReadyToPlay)
					{
						list2[num].StatTriggeredActions[n].PlayAction();
						CoroutineController item;
						this.StartCoroutineEx(this.ActionRoutine(list2[num].Actions[n], null, false, false), out item);
						waitFor.Add(item);
					}
				}
			}
			num++;
		}
		int num2;
		for (int i = 0; i < waitFor.Count; i = num2 + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num2 = i;
		}
		yield break;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00007D74 File Offset: 0x00005F74
	public void CheckForStatTriggeredActions(InGameStat _Stat, FromStatChangeAction[] _ModelList, StatTriggeredActionStatus[] _StatusList, InGameCardBase _Card)
	{
		if (_ModelList == null)
		{
			return;
		}
		if (_ModelList.Length == 0)
		{
			return;
		}
		if (_Card && _Card.Destroyed)
		{
			return;
		}
		for (int i = 0; i < _ModelList.Length; i++)
		{
			for (int j = 0; j < _ModelList[i].StatChangeTrigger.Length; j++)
			{
				if (!(_ModelList[i].StatChangeTrigger[j].Stat != _Stat.StatModel) && !(_ModelList[i].StatChangeTrigger[j].Stat == null))
				{
					_StatusList[i].Conditions[j] = _ModelList[i].StatChangeTrigger[j].IsInRange(_Stat.CurrentValue(this.NotInBase));
				}
			}
			string text;
			_StatusList[i].UpdateReadyToPlay(_ModelList[i].IsNotCancelledByDemo && _ModelList[i].CardsAndTagsAreCorrect(_Card, null, null) && _ModelList[i].StatsAreCorrect(null, true) && _ModelList[i].WillHaveAnEffect(_Card, false, false, false, false, Array.Empty<CardModifications>()) && _ModelList[i].DurabilitiesAreCorrect(_Card, out text), _ModelList[i].RepeatOptions, _ModelList[i].OncePerTick);
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00007E94 File Offset: 0x00006094
	private IEnumerator ChangeStat(StatModifier _Stat, StatModification _Modification, string _From, string _Action, int _ActionTick, object _Source, Transform _FeedbackPos = null, bool _WaitForAnimation = true)
	{
		if (!_Stat.Stat)
		{
			yield break;
		}
		if (!this.StatsDict.ContainsKey(_Stat.Stat))
		{
			yield break;
		}
		float rate = Mathf.Approximately(_Stat.RateModifier.x, _Stat.RateModifier.y) ? _Stat.RateModifier.x : UnityEngine.Random.Range(_Stat.RateModifier.x, _Stat.RateModifier.y);
		float num = Mathf.Approximately(_Stat.ValueModifier.x, _Stat.ValueModifier.y) ? _Stat.ValueModifier.x : UnityEngine.Random.Range(_Stat.ValueModifier.x, _Stat.ValueModifier.y);
		if (!string.IsNullOrEmpty(_Action) && _Stat.Stat.UsesNovelty)
		{
			this.StatsDict[_Stat.Stat].RegisterAction(_Action, _ActionTick);
			num *= this.StatsDict[_Stat.Stat].GetStalenessModifier(_Action);
		}
		StatModifierReport obj = new StatModifierReport
		{
			TickInfo = this.CurrentTickInfo,
			ModifierSource = _From,
			Stat = this.StatsDict[_Stat.Stat],
			Value = num,
			Rate = rate,
			IsInverse = _Stat.IsInverse,
			ModificationType = _Modification
		};
		if (_Stat.Stat.StatusDebugMode)
		{
			Debug.Log("Sending report: " + _From);
		}
		Action<StatModifierReport> onStatModified = GameManager.OnStatModified;
		if (onStatModified != null)
		{
			onStatModified(obj);
		}
		CoroutineController rateChange;
		this.StartCoroutineEx(this.ChangeStatRate(this.StatsDict[_Stat.Stat], rate, _Modification), out rateChange);
		CoroutineController valueChange;
		this.StartCoroutineEx(this.ChangeStatValue(this.StatsDict[_Stat.Stat], num, _Modification), out valueChange);
		if (_Source != null)
		{
			if (_Stat.IsInverse)
			{
				this.StatsDict[_Stat.Stat].RemoveModifierSource(new StatModifierSource(num, rate, _Source));
			}
			else
			{
				this.StatsDict[_Stat.Stat].AddModifierSource(new StatModifierSource(num, rate, _Source));
			}
		}
		if (_FeedbackPos)
		{
			if (_WaitForAnimation)
			{
				yield return base.StartCoroutine(this.GameGraphics.ActionStatModFeedback(this.StatsDict[_Stat.Stat], num, _FeedbackPos));
			}
			else
			{
				base.StartCoroutine(this.GameGraphics.ActionStatModFeedback(this.StatsDict[_Stat.Stat], num, _FeedbackPos));
			}
		}
		while (rateChange.state != CoroutineState.Finished && valueChange.state != CoroutineState.Finished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00007EEB File Offset: 0x000060EB
	private IEnumerator ChangeStatValue(InGameStat _Stat, float _Value, StatModification _Modification)
	{
		if (_Value == 0f || !_Stat)
		{
			yield break;
		}
		if (_Stat.StatModel.StatusDebugMode)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Changing ",
				_Stat,
				" Value by ",
				_Value.ToString(),
				" (frame ",
				Time.frameCount,
				")"
			}));
		}
		float prevValue = _Stat.CurrentValue(this.NotInBase);
		float num = _Stat.CurrentBaseValue + _Value;
		float num2 = (_Stat.StatModel.MinMaxValue != Vector2.zero) ? Mathf.Clamp(num, _Stat.StatModel.MinMaxValue.x, _Stat.StatModel.MinMaxValue.y) : num;
		switch (_Modification)
		{
		case StatModification.Permanent:
			_Stat.CurrentBaseValue += _Value - (num - num2);
			break;
		case StatModification.GlobalModifier:
			_Stat.GlobalModifiedValue += _Value;
			break;
		case StatModification.AtBaseModifier:
			_Stat.AtBaseModifiedValue += _Value;
			break;
		}
		this.CheckForPassiveEffects = true;
		CoroutineController controller;
		this.StartCoroutineEx(this.UpdateStatStatuses(_Stat, prevValue, null), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		if (_Stat.StatModel.StatusDebugMode)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Finished with ",
				_Stat,
				" (frame ",
				Time.frameCount,
				")"
			}));
		}
		yield break;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00007F0F File Offset: 0x0000610F
	private IEnumerator ChangeStatRate(InGameStat _Stat, float _Rate, StatModification _Modification)
	{
		if (_Rate == 0f || !_Stat)
		{
			yield break;
		}
		float num = _Stat.CurrentBaseRate + _Rate;
		float num2 = (_Stat.StatModel.MinMaxRate != Vector2.zero) ? Mathf.Clamp(num, _Stat.StatModel.MinMaxRate.x, _Stat.StatModel.MinMaxRate.y) : num;
		if (_Stat.StatModel.StatusDebugMode)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"Changing ",
				_Stat,
				" Rate by ",
				_Rate.ToString()
			}));
		}
		switch (_Modification)
		{
		case StatModification.Permanent:
			_Stat.CurrentBaseRate += _Rate - (num - num2);
			break;
		case StatModification.GlobalModifier:
			_Stat.GlobalModifiedRate += _Rate;
			break;
		case StatModification.AtBaseModifier:
			_Stat.AtBaseModifiedRate += _Rate;
			break;
		}
		yield break;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00007F2C File Offset: 0x0000612C
	private IEnumerator UpdateStatStatuses(InGameStat _Stat, float _PrevValue, StatSaveData _SavedStatusInfo)
	{
		List<CoroutineController> waitFor = new List<CoroutineController>();
		CoroutineController item = null;
		if (_Stat.StatModel.StatusDebugMode)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				_Stat.name,
				" | Prev value: ",
				_PrevValue,
				", current value: ",
				_Stat.CurrentValue(this.NotInBase)
			}));
		}
		if (_Stat.StatModel.Statuses != null)
		{
			for (int j = 0; j < _Stat.StatModel.Statuses.Length; j++)
			{
				if (_Stat.StatModel.Statuses[j].IsInRange(_Stat.CurrentValue(this.NotInBase)) && !_Stat.StatModel.Statuses[j].IsInRange(_PrevValue) && !_Stat.HasStatus(_Stat.StatModel.Statuses[j]))
				{
					_Stat.CurrentStatuses.Add(_Stat.StatModel.Statuses[j].Instantiate(_Stat.CurrentValue(this.NotInBase) > _PrevValue, _Stat));
					_Stat.CurrentStatuses[_Stat.CurrentStatuses.Count - 1].Load(_SavedStatusInfo);
					this.StartCoroutineEx(this.OnStatStatusChange(_Stat.CurrentStatuses[_Stat.CurrentStatuses.Count - 1], true, _Stat), out item);
					waitFor.Add(item);
					if (_Stat.StatModel.StatusDebugMode)
					{
						Debug.LogWarning("Adding status " + _Stat.StatModel.Statuses[j].GameName + " to " + _Stat.name);
					}
				}
			}
		}
		if (_Stat.CurrentStatuses.Count != 0)
		{
			for (int k = _Stat.CurrentStatuses.Count - 1; k >= 0; k--)
			{
				if (!_Stat.CurrentStatuses[k].IsInRange(_Stat.CurrentValue(this.NotInBase)))
				{
					if (_Stat.StatModel.StatusDebugMode)
					{
						Debug.LogWarning("Removing status " + _Stat.CurrentStatuses[k].GameName + " from " + _Stat.name);
					}
					this.StartCoroutineEx(this.OnStatStatusChange(_Stat.CurrentStatuses[k], false, _Stat), out item);
					waitFor.Add(item);
					if (k >= _Stat.CurrentStatuses.Count)
					{
						break;
					}
					_Stat.CurrentStatuses.RemoveAt(k);
				}
			}
		}
		if (_Stat.StatModel.Visibility == StatVisibilityOptions.AlwaysVisible || _Stat.IsPinned)
		{
			bool flag = _Stat.AnyCurrentStatus(true) != null;
			if (!flag && _Stat.DefaultStatus == null)
			{
				_Stat.DefaultStatus = _Stat.StatModel.GetDefaultStatus.Instantiate(true, _Stat);
			}
			else if (flag && _Stat.DefaultStatus != null)
			{
				if (_Stat.DefaultStatus.StatusGraphics)
				{
					_Stat.DefaultStatus.StatusGraphics.Hide();
				}
				_Stat.DefaultStatus = null;
			}
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00007F50 File Offset: 0x00006150
	private IEnumerator OnStatStatusChange(StatStatus _Status, bool _Added, InGameStat _FromStat)
	{
		List<CoroutineController> waitFor = new List<CoroutineController>();
		if (!Mathf.Approximately(_Status.ConfidenceModifier, 0f) && this.SuccessChances)
		{
			if (_Added)
			{
				this.SuccessChances.AddConfidenceMod(_FromStat.StatModel, _Status.ConfidenceModifier);
			}
			else
			{
				this.SuccessChances.RemoveConfidenceMod(_FromStat.StatModel, _Status.ConfidenceModifier);
			}
		}
		if (_Status.EffectsOnStats != null)
		{
			for (int j = 0; j < _Status.EffectsOnStats.Length; j++)
			{
				CoroutineController item;
				this.StartCoroutineEx(this.ChangeStat(_Added ? _Status.EffectsOnStats[j] : _Status.EffectsOnStats[j].Inverse, StatModification.GlobalModifier, StatModifierReport.SourceFromStatStatus(_Status.GameName, _FromStat, _Added), null, -1, _FromStat, null, true), out item);
				waitFor.Add(item);
			}
		}
		if (_Status.ActionBlocker != null)
		{
			if (_Added)
			{
				this.CurrentActionBlockers.Add(_Status.ActionBlocker);
			}
			else if (this.CurrentActionBlockers.Contains(_Status.ActionBlocker))
			{
				this.CurrentActionBlockers.Remove(_Status.ActionBlocker);
			}
		}
		if (_Status.EffectsOnActions != null && _Status.EffectsOnActions.Length != 0)
		{
			if (_Added)
			{
				for (int k = 0; k < _Status.EffectsOnActions.Length; k++)
				{
					if (!this.CurrentActionModifiers.Contains(_Status.EffectsOnActions[k]))
					{
						this.AddActionModifier(_Status.EffectsOnActions[k], ActionModifier.SourceFromStatus(_Status, k));
					}
				}
			}
			else
			{
				for (int l = 0; l < _Status.EffectsOnActions.Length; l++)
				{
					if (this.CurrentActionModifiers.Contains(_Status.EffectsOnActions[l]))
					{
						this.CurrentActionModifiers.Remove(_Status.EffectsOnActions[l]);
					}
				}
			}
		}
		if (!_Added && _Status.StatusGraphics)
		{
			_Status.StatusGraphics.Hide();
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		if (!GameOverMenu.IsGameOver && _Status.GameOver && _Added)
		{
			int num2 = (this.GameOverSunsPerDay > 0f) ? Mathf.FloorToInt(this.GameOverSunsPerDay * (float)this.CurrentTickInfo.x) : -1;
			int num3 = (this.GameOverMoonsPerDay > 0f) ? Mathf.FloorToInt(this.GameOverMoonsPerDay * (float)this.CurrentTickInfo.x) : -1;
			if (num2 > 0)
			{
				GameLoad.Instance.SaveData.Suns += num2;
			}
			if (num3 > 0)
			{
				GameLoad.Instance.SaveData.Moons += num3;
			}
			MBSingleton<EndgameMenu>.Instance.Setup(_Status.Description, num2, num3, this.CurrentGameData, true, false, false, false, true);
		}
		while (EndgameMenu.IsGameOver)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00007F74 File Offset: 0x00006174
	public IEnumerator UpdatePassiveEffects()
	{
		List<CoroutineController> waitFor = new List<CoroutineController>();
		while (this.CheckForPassiveEffects)
		{
			this.CheckForPassiveEffects = false;
			for (int i = 0; i < this.CardsWithPassiveEffects.Count; i++)
			{
				CoroutineController item;
				this.StartCoroutineEx(this.CardsWithPassiveEffects[i].UpdatePassiveEffects(), out item);
				waitFor.Add(item);
			}
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		this.WillCheckForPassiveEffects = false;
		yield break;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00007F83 File Offset: 0x00006183
	private IEnumerator StandardCardCollectionDrop(CardsDropCollection _Collection, InGameCardBase _FromCard, bool _TransformsIntoEnv, CardAction _FromAction, int _StartingTick, Transform _FeedbackSource)
	{
		List<CoroutineController> waitFor = new List<CoroutineController>();
		waitFor.Clear();
		CoroutineController item;
		this.StartCoroutineEx(this.ProduceCards(_Collection, _FromCard, _TransformsIntoEnv, false, _FromAction.TravelToPreviousEnv), out item);
		waitFor.Add(item);
		if (_Collection != null)
		{
			if (_Collection.CurrentStatModifiers != null)
			{
				this.StartCoroutineEx(this.ApplyStatModifiers(_Collection.CurrentStatModifiers.ToArray(), _FromAction.NoveltyID(_FromCard), _StartingTick, _FeedbackSource, 1f, StatModifierReport.SourceFromAction(_FromAction, _FromCard)), out item);
			}
			if (_Collection.DurabilityModifications != null && !_Collection.DurabilityModifications.IsEmpty)
			{
				TransferedDurabilities transferedDurabilities = new TransferedDurabilities();
				transferedDurabilities.Add(_Collection.DurabilityModifications);
				this.StartCoroutineEx(this.ChangeCardDurabilities(_FromCard, transferedDurabilities.Spoilage, transferedDurabilities.Usage, transferedDurabilities.Fuel, transferedDurabilities.ConsumableCharges, transferedDurabilities.Liquid, transferedDurabilities.Special1, transferedDurabilities.Special2, transferedDurabilities.Special3, transferedDurabilities.Special4, true, true), out item);
				waitFor.Add(item);
			}
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00007FBF File Offset: 0x000061BF
	private IEnumerator ProduceCards(CardsDropCollection _Collection, InGameCardBase _FromCard, bool _TransformsIntoEnv, bool _ToExplorationSlots, bool _TravelToPrevEnv)
	{
		if (_Collection == null)
		{
			if (this.PrevEnvironment && _TravelToPrevEnv)
			{
				this.NextTravelIndex = 0;
				yield return base.StartCoroutine(this.AddCard(this.PrevEnvironment, _FromCard, true, null, true, SpawningLiquid.DefaultLiquid, new Vector2Int(this.CurrentTickInfo.z, 0), true));
			}
			yield break;
		}
		if (_FromCard)
		{
			_FromCard.UseCollection(_Collection);
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		if (_Collection.CurrentSaveDataDrop.Count > 0)
		{
			for (int j = 0; j < _Collection.CurrentSaveDataDrop.Count; j++)
			{
				this.CreateCardAsSaveData(_Collection.CurrentSaveDataDrop[j], _Collection.CurrentDrop[0], _Collection.SaveDataKey, null, null, false);
			}
		}
		int moveViewIndex = -1;
		for (int k = 0; k < _Collection.CurrentDrop.Count; k++)
		{
			if (_Collection.CurrentDrop[k] && _Collection.CurrentDrop[k].CanSpawnOnBoard())
			{
				if (_Collection.CurrentDrop[k].CardType == CardTypes.Environment)
				{
					moveViewIndex = -1;
				}
				else if (!_Collection.CurrentDrop[k].CanPile)
				{
					moveViewIndex = k;
				}
			}
		}
		int envIndex = _Collection.CurrentDrop.Count;
		if (_TransformsIntoEnv)
		{
			envIndex = -1;
		}
		int num;
		for (int i = 0; i < _Collection.CurrentDrop.Count; i = num + 1)
		{
			if (!(_Collection.CurrentDrop[i] == null))
			{
				if (i > 0)
				{
					yield return new WaitForSeconds(0.1f);
				}
				if (_FromCard && _FromCard.UpdatedInBackground && !_Collection.CurrentDrop[i].IndependentFromEnv)
				{
					this.CreateCardAsSaveData(_FromCard, _Collection.CurrentDrop[i], null);
				}
				else if (_Collection.CurrentDrop[i].CanSpawnOnBoard())
				{
					bool flag = moveViewIndex == i || (moveViewIndex == -1 && _FromCard != null);
					if (this.GameGraphics && this.GameGraphics.EncounterPopupWindow)
					{
						flag |= this.GameGraphics.EncounterPopupWindow.OngoingEncounter;
					}
					if (!_ToExplorationSlots)
					{
						if (_Collection.CurrentDrop[i].CardType == CardTypes.Environment)
						{
							envIndex = i;
							yield return base.StartCoroutine(this.AddCard(_Collection.CurrentDrop[i], _FromCard, i >= envIndex, null, true, SpawningLiquid.DefaultLiquid, new Vector2Int(this.CurrentTickInfo.z, 0), flag));
						}
						else if (_Collection.CurrentDrop[i].CardType == CardTypes.Event)
						{
							if (_FromCard)
							{
								if (_FromCard.CardModel.CardType == CardTypes.Event)
								{
									if (this.EventCardQueue.Contains(_FromCard.CardModel))
									{
										this.EventCardQueue.Insert(this.EventCardQueue.IndexOf(_FromCard.CardModel) + 1, _Collection.CurrentDrop[i]);
									}
									else
									{
										this.EventCardQueue.Insert(0, _Collection.CurrentDrop[i]);
									}
								}
								else
								{
									this.EventCardQueue.Add(_Collection.CurrentDrop[i]);
								}
							}
							else
							{
								this.EventCardQueue.Add(_Collection.CurrentDrop[i]);
							}
						}
						else
						{
							CoroutineController item;
							this.StartCoroutineEx(this.AddCard(_Collection.CurrentDrop[i], _FromCard, i >= envIndex, null, true, SpawningLiquid.DefaultLiquid, new Vector2Int(this.CurrentTickInfo.z, 0), flag), out item);
							waitFor.Add(item);
						}
					}
					else
					{
						CoroutineController item;
						this.StartCoroutineEx(this.AddCard(_Collection.CurrentDrop[i], new SlotInfo(SlotsTypes.Exploration, -1), this.CurrentEnvironment, _FromCard, true, null, null, null, null, null, MBSingleton<ExplorationPopup>.Instance.CardSpawnPos, true, SpawningLiquid.DefaultLiquid, false, false, new Vector2Int(this.CurrentTickInfo.z, 0), null, 0, -1, false, ""), out item);
						waitFor.Add(item);
					}
				}
			}
			num = i;
		}
		if (_FromCard && !_Collection.CurrentLiquidDrop.IsEmpty && _FromCard.CardModel && _FromCard.CardModel.CanContainLiquid)
		{
			TransferedDurabilities transferedDurabilities = (_Collection.CurrentLiquidDrop.LiquidDurabilities == null) ? new TransferedDurabilities() : _Collection.CurrentLiquidDrop.LiquidDurabilities;
			transferedDurabilities.Liquid = _Collection.CurrentLiquidDrop.Quantity.x;
			if (_Collection.CurrentLiquidDrop.LiquidDurabilities == null)
			{
				transferedDurabilities.Spoilage.FloatValue = _Collection.CurrentLiquidDrop.LiquidCard.SpoilageTime;
				transferedDurabilities.Usage.FloatValue = _Collection.CurrentLiquidDrop.LiquidCard.UsageDurability;
				transferedDurabilities.Fuel.FloatValue = _Collection.CurrentLiquidDrop.LiquidCard.FuelCapacity;
				transferedDurabilities.ConsumableCharges.FloatValue = _Collection.CurrentLiquidDrop.LiquidCard.Progress;
				transferedDurabilities.Special1 = _Collection.CurrentLiquidDrop.LiquidCard.SpecialDurability1;
				transferedDurabilities.Special2 = _Collection.CurrentLiquidDrop.LiquidCard.SpecialDurability2;
				transferedDurabilities.Special3 = _Collection.CurrentLiquidDrop.LiquidCard.SpecialDurability3;
				transferedDurabilities.Special4 = _Collection.CurrentLiquidDrop.LiquidCard.SpecialDurability4;
			}
			CoroutineController item;
			this.StartCoroutineEx(this.AddCard(_Collection.CurrentLiquidDrop.LiquidCard, _FromCard, false, transferedDurabilities, false, SpawningLiquid.Empty, new Vector2Int(this.CurrentTickInfo.z, 0), moveViewIndex == -1), out item);
			waitFor.Add(item);
		}
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		if (_TravelToPrevEnv && this.PrevEnvironment)
		{
			yield return base.StartCoroutine(this.AddCard(this.PrevEnvironment, _FromCard, true, null, true, SpawningLiquid.DefaultLiquid, new Vector2Int(this.CurrentTickInfo.z, 0), true));
		}
		yield break;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00007FF4 File Offset: 0x000061F4
	public void CreateCardAsSaveData(InGameCardBase _FromCard, CardData _Card, TransferedDurabilities _Durabilities)
	{
		string envKey = _FromCard.Environment.EnvironmentDictionaryKey(_FromCard.PrevEnvironment, _FromCard.PrevEnvTravelIndex);
		this.CreateCardAsSaveData(_Card, _FromCard.Environment, envKey, _FromCard.CurrentSlotInfo, _Durabilities, false);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00008030 File Offset: 0x00006230
	public void CreateCardAsSaveData(CardData _Card, CardData _Env, string _EnvKey, SlotInfo _SlotInfo, TransferedDurabilities _Durabilities, bool _DefaultCard)
	{
		if (!this.EnvironmentsData.ContainsKey(_EnvKey))
		{
			this.EnvironmentsData.Add(_EnvKey, new EnvironmentSaveData(_Env, this.CurrentTickInfo.z, UniqueIDScriptable.AddNamesToComplexID(_EnvKey)));
			this.EnvironmentsData[_EnvKey].CurrentMaxWeight = _Env.GetWeightCapacity(0f);
			this.EnvironmentsData[_EnvKey].FillCounters(this.AllCounters);
		}
		if (_Card.UniqueOnBoard && this.EnvironmentsData[_EnvKey].ContainsCard(_Card))
		{
			return;
		}
		CardSaveData cardSaveData = new CardSaveData();
		if (_SlotInfo != null)
		{
			cardSaveData.SaveCardModel(_Card, _Env, _SlotInfo);
		}
		else if (MBSingleton<GraphicsManager>.Instance)
		{
			cardSaveData.SaveCardModel(_Card, _Env, new SlotInfo(MBSingleton<GraphicsManager>.Instance.CardToSlotType(_Card.CardType, false), this.EnvironmentsData[_EnvKey].AllRegularCards.Count));
		}
		else
		{
			cardSaveData.SaveCardModel(_Card, _Env, new SlotInfo(SlotsTypes.Base, this.EnvironmentsData[_EnvKey].AllRegularCards.Count));
		}
		if (_DefaultCard)
		{
			cardSaveData.CreatedInSaveDataTick = -1;
		}
		if (_Durabilities != null)
		{
			if (_Durabilities.Spoilage)
			{
				cardSaveData.Spoilage = _Durabilities.Spoilage;
			}
			if (_Durabilities.Usage)
			{
				cardSaveData.Usage = _Durabilities.Usage;
			}
			if (_Durabilities.Fuel)
			{
				cardSaveData.Fuel = _Durabilities.Fuel;
			}
			if (_Durabilities.ConsumableCharges)
			{
				cardSaveData.ConsumableCharges = _Durabilities.ConsumableCharges;
			}
			cardSaveData.LiquidQuantity = _Durabilities.Liquid;
			if (_Durabilities.Special1)
			{
				cardSaveData.Special1 = _Durabilities.Special1;
			}
			if (_Durabilities.Special2)
			{
				cardSaveData.Special2 = _Durabilities.Special2;
			}
			if (_Durabilities.Special3)
			{
				cardSaveData.Special3 = _Durabilities.Special3;
			}
			if (_Durabilities.Special4)
			{
				cardSaveData.Special4 = _Durabilities.Special4;
			}
		}
		if (!_Card.IsTravellingCard)
		{
			this.EnvironmentsData[_EnvKey].AllRegularCards.Add(cardSaveData);
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.EnvironmentsData[_EnvKey].AllRegularCards.Count; i++)
		{
			if (!this.EnvironmentsData[_EnvKey].AllRegularCards[i].IsTravelCard)
			{
				cardSaveData.SlotInformation.SlotIndex = i;
				this.EnvironmentsData[_EnvKey].AllRegularCards.Insert(i, cardSaveData);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.EnvironmentsData[_EnvKey].AllRegularCards.Add(cardSaveData);
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00008300 File Offset: 0x00006500
	private CardsDropCollection SelectCardCollection(CardAction _Action, InGameCardBase _FromCard)
	{
		if (_Action.ProducedCards == null)
		{
			return null;
		}
		if (_Action.ProducedCards.Length == 0)
		{
			return null;
		}
		CollectionDropReport collectionDropsReport = this.GetCollectionDropsReport(_Action, _FromCard, true);
		if (collectionDropsReport.TotalValue == 0)
		{
			collectionDropsReport.SelectedDrop = UnityEngine.Random.Range(0, collectionDropsReport.DropsInfo.Length);
			collectionDropsReport.RandomValue = (float)collectionDropsReport.SelectedDrop;
			Action<CollectionDropReport> onCollectionDropsSelected = GameManager.OnCollectionDropsSelected;
			if (onCollectionDropsSelected != null)
			{
				onCollectionDropsSelected(collectionDropsReport);
			}
			return _Action.ProducedCards[collectionDropsReport.SelectedDrop];
		}
		float num = UnityEngine.Random.Range(0f, (float)collectionDropsReport.TotalValue - 0.001f);
		collectionDropsReport.RandomValue = num;
		for (int i = 0; i < collectionDropsReport.DropsInfo.Length; i++)
		{
			if (num < (float)collectionDropsReport.DropsInfo[i].RangeUpTo)
			{
				collectionDropsReport.SelectedDrop = i;
				Action<CollectionDropReport> onCollectionDropsSelected2 = GameManager.OnCollectionDropsSelected;
				if (onCollectionDropsSelected2 != null)
				{
					onCollectionDropsSelected2(collectionDropsReport);
				}
				return _Action.ProducedCards[i];
			}
		}
		collectionDropsReport.SelectedDrop = -1;
		Action<CollectionDropReport> onCollectionDropsSelected3 = GameManager.OnCollectionDropsSelected;
		if (onCollectionDropsSelected3 != null)
		{
			onCollectionDropsSelected3(collectionDropsReport);
		}
		return null;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x000083FC File Offset: 0x000065FC
	public CollectionDropReport GetCollectionDropsReport(CardAction _Action, InGameCardBase _FromCard, bool _CheckEnvironment)
	{
		int num = 0;
		int baseValue = 0;
		CardData fromData = _FromCard ? _FromCard.CardModel : null;
		CollectionDropReport collectionDropReport = default(CollectionDropReport);
		collectionDropReport.TickInfo = this.CurrentTickInfo;
		collectionDropReport.FromCard = _FromCard;
		collectionDropReport.FromData = fromData;
		collectionDropReport.FromAction = _Action;
		collectionDropReport.DropsInfo = new CollectionDropInfo[_Action.ProducedCards.Length];
		if (_FromCard && _FromCard.CardModel && _FromCard.CardModel.IsTravellingCard)
		{
			this.NextTravelIndex = _FromCard.TravelCardIndex;
		}
		for (int i = 0; i < _Action.ProducedCards.Length; i++)
		{
			collectionDropReport.DropsInfo[i].CollectionName = _Action.ProducedCards[i].CollectionName;
			collectionDropReport.DropsInfo[i].IsSuccess = _Action.ProducedCards[i].CountsAsSuccess;
			collectionDropReport.DropsInfo[i].RevealInventory = _Action.ProducedCards[i].RevealInventory;
			if (_FromCard)
			{
				if (_FromCard.DroppedCollections.ContainsKey(_Action.ProducedCards[i].CollectionName))
				{
					collectionDropReport.DropsInfo[i].CollectionUses = _FromCard.DroppedCollections[_Action.ProducedCards[i].CollectionName];
				}
				else
				{
					collectionDropReport.DropsInfo[i].CollectionUses = Vector2Int.one * -1;
				}
			}
			else
			{
				collectionDropReport.DropsInfo[i].CollectionUses = Vector2Int.one * -1;
			}
			if (_FromCard && !_FromCard.CanUseCollection(_Action.ProducedCards[i]))
			{
				collectionDropReport.DropsInfo[i].RangeUpTo = -1;
				collectionDropReport.DropsInfo[i].Drops = new CardData[0];
				collectionDropReport.DropsInfo[i].BaseWeight = 0;
				collectionDropReport.DropsInfo[i].StatMods = new StatModifier[0];
			}
			else
			{
				_Action.ProducedCards[i].FillDropList(_CheckEnvironment, _Action.DropsMultiplier);
				collectionDropReport.DropsInfo[i].Drops = _Action.ProducedCards[i].CurrentDrop.ToArray();
				_Action.ProducedCards[i].FillStatModsList();
				collectionDropReport.DropsInfo[i].StatMods = _Action.ProducedCards[i].CurrentStatModifiers.ToArray();
				if (_Action.ProducedCards[i].CurrentDrop.Count == 0 && !_Action.ProducedCards[i].RevealInventory)
				{
					collectionDropReport.DropsInfo[i].BaseWeight = 0;
					collectionDropReport.DropsInfo[i].RangeUpTo = -1;
				}
				else
				{
					collectionDropReport.DropsInfo[i].BaseWeight = _Action.ProducedCards[i].CollectionWeight;
					collectionDropReport.DropsInfo[i].StatWeightMods = new List<StatDropWeightModReport>();
					collectionDropReport.DropsInfo[i].CardWeightMods = new List<CardDropWeightModReport>();
					_Action.ProducedCards[i].GetStatWeightMods(_FromCard ? _FromCard.CardModel : null, collectionDropReport.DropsInfo[i].StatWeightMods, true);
					_Action.ProducedCards[i].GetCardWeightMods(collectionDropReport.DropsInfo[i].CardWeightMods, true);
					collectionDropReport.DropsInfo[i].DurabilitiesWeightMods = _Action.ProducedCards[i].GetDurabilitiesWeightMods(_FromCard);
					if (collectionDropReport.DropsInfo[i].FinalWeight <= 0)
					{
						collectionDropReport.DropsInfo[i].RangeUpTo = -1;
					}
					else
					{
						baseValue = collectionDropReport.DropsInfo[i].BaseWeight;
						num += collectionDropReport.DropsInfo[i].FinalWeight;
						collectionDropReport.DropsInfo[i].RangeUpTo = num;
					}
				}
			}
		}
		collectionDropReport.TotalValue = num;
		collectionDropReport.BaseValue = baseValue;
		return collectionDropReport;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00008800 File Offset: 0x00006A00
	public void AddHoveredDismantleAction(CollectionDropReport _Report)
	{
		if (_Report.IsIdentical(default(CollectionDropReport)))
		{
			return;
		}
		for (int i = 0; i < this.CurrentDismantleActions.Count; i++)
		{
			if (this.CurrentDismantleActions[i].IsIdentical(_Report))
			{
				return;
			}
		}
		this.CurrentDismantleActions.Insert(0, _Report);
		Action onDismantleActionHovered = GameManager.OnDismantleActionHovered;
		if (onDismantleActionHovered == null)
		{
			return;
		}
		onDismantleActionHovered();
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0000886C File Offset: 0x00006A6C
	public void RemoveHoveredDismantleAction(CollectionDropReport _Report)
	{
		if (_Report.IsIdentical(default(CollectionDropReport)))
		{
			return;
		}
		int i = 0;
		while (i < this.CurrentDismantleActions.Count)
		{
			if (this.CurrentDismantleActions[i].CollectionReportName == _Report.CollectionReportName)
			{
				this.CurrentDismantleActions.RemoveAt(i);
				Action onDismantleActionHovered = GameManager.OnDismantleActionHovered;
				if (onDismantleActionHovered == null)
				{
					return;
				}
				onDismantleActionHovered();
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000088E0 File Offset: 0x00006AE0
	private bool LoadCard(CardSaveData _FromData, InGameCardBase _Container, bool _NextEnv, CardData _WithLiquid = null)
	{
		if (_FromData == null)
		{
			return false;
		}
		UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(_FromData.CardID);
		if (!fromID)
		{
			return false;
		}
		CardData cardData = fromID as CardData;
		if (!cardData)
		{
			return false;
		}
		if (_FromData.IsPinned)
		{
			InGamePinData pinData = this.GetPinData(cardData, _WithLiquid);
			if (pinData != null && (!pinData.IsPinned || pinData.CorrespondingCard))
			{
				return false;
			}
		}
		fromID = UniqueIDScriptable.GetFromID(_FromData.EnvironmentID);
		CardData cardData2 = fromID ? (fromID as CardData) : null;
		CardData y = _NextEnv ? this.NextEnvironment : this.CurrentEnvironment;
		fromID = UniqueIDScriptable.GetFromID(_FromData.PrevEnvironmentID);
		CardData prevEnv = fromID ? (fromID as CardData) : null;
		bool flag = cardData.IndependentFromEnv;
		if (!flag && _WithLiquid)
		{
			flag = _WithLiquid.IndependentFromEnv;
		}
		if (_FromData.IsPinned)
		{
			flag = false;
		}
		if (cardData2 == y || flag)
		{
			if (this.EventCardQueue != null && cardData.CardType == CardTypes.Event && this.EventCardQueue.Count > 0 && this.EventCardQueue[0] == cardData)
			{
				this.EventCardQueue.RemoveAt(0);
			}
			SpawningLiquid withLiquid = default(SpawningLiquid);
			if (!_FromData.NotYetCreated)
			{
				if (_WithLiquid == null)
				{
					withLiquid.StayEmpty = true;
				}
				else
				{
					withLiquid.LiquidCard = _WithLiquid;
				}
			}
			base.StartCoroutine(this.AddCard(cardData, _FromData.SlotInformation, cardData2, _Container, false, _FromData.GetDurabilities(), _FromData.CollectionUses, _FromData.StatTriggeredActions, _FromData.ExplorationData, _FromData.BlueprintData, Vector3.zero, _FromData.NotYetCreated, withLiquid, false, _FromData.IsPinned, new Vector2Int(_FromData.CreatedOnTick, _FromData.CreatedInSaveDataTick), prevEnv, _FromData.PrevEnvTravelIndex, _FromData.TravelCardIndex, _FromData.IgnoreBaseRow, _FromData.CustomName));
			return true;
		}
		return false;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00008AB8 File Offset: 0x00006CB8
	private void LoadInventoryCard(InventoryCardSaveData _FromData, InGameCardBase _FromContainer, List<InventoryCardSaveData> _NestedInventoryCards, bool _NextEnv)
	{
		if (_FromData == null)
		{
			return;
		}
		CardData cardData = null;
		if (_FromData.IsPinned)
		{
			cardData = UniqueIDScriptable.GetFromID<CardData>(_FromData.PinnedLiquidCard);
		}
		else if (_FromData.LiquidContained != null)
		{
			cardData = UniqueIDScriptable.GetFromID<CardData>(_FromData.LiquidContained.CardID);
		}
		if (!this.LoadCard(_FromData, _FromContainer, _NextEnv, cardData) || _FromData.CardsInInventory == null)
		{
			return;
		}
		CardData fromID = UniqueIDScriptable.GetFromID<CardData>(_FromData.CardID);
		InGameCardBase inGameCardBase = this.FindLatestCreatedCard(UniqueIDScriptable.GetFromID<CardData>(_FromData.CardID));
		if (!inGameCardBase.CardModel || inGameCardBase.CardModel != fromID)
		{
			return;
		}
		if (inGameCardBase.CardsInInventory == null)
		{
			inGameCardBase.CardsInInventory = new List<InventorySlot>();
		}
		for (int i = 0; i < _FromData.CardsInInventory.Count; i++)
		{
			if (_FromData.CardsInInventory[i] != null)
			{
				if (_FromData.CardsInInventory[i].InventoryCardIndex == -1)
				{
					if (i < inGameCardBase.CardsInInventory.Count || !inGameCardBase.IsLegacyInventory)
					{
						for (int j = 0; j < Mathf.Max(1, _FromData.CardsInInventory[i].CardAmt); j++)
						{
							this.LoadCard(_FromData.CardsInInventory[i].RegularCard, inGameCardBase, _NextEnv, null);
						}
					}
					else
					{
						if (inGameCardBase.CardModel.CardType == CardTypes.Explorable)
						{
							break;
						}
						_FromData.CardsInInventory[i].RegularCard.SlotInformation.SlotType = SlotsTypes.Base;
						for (int k = 0; k < Mathf.Max(1, _FromData.CardsInInventory[i].CardAmt); k++)
						{
							this.LoadCard(_FromData.CardsInInventory[i].RegularCard, null, _NextEnv, null);
						}
					}
				}
				else if (i < inGameCardBase.CardsInInventory.Count || !inGameCardBase.IsLegacyInventory)
				{
					this.LoadInventoryCard(_NestedInventoryCards[_FromData.CardsInInventory[i].InventoryCardIndex], inGameCardBase, _NestedInventoryCards, _NextEnv);
				}
				else
				{
					if (inGameCardBase.CardModel.CardType == CardTypes.Explorable)
					{
						break;
					}
					_NestedInventoryCards[_FromData.CardsInInventory[i].InventoryCardIndex].SlotInformation.SlotType = SlotsTypes.Base;
					this.LoadInventoryCard(_NestedInventoryCards[_FromData.CardsInInventory[i].InventoryCardIndex], null, _NestedInventoryCards, _NextEnv);
				}
			}
		}
		if (cardData && !_FromData.IsPinned)
		{
			this.LoadCard(_FromData.LiquidContained, inGameCardBase, _NextEnv, null);
		}
		if (_FromData.CookingResults != null && _FromData.CookingResults.Count > 0)
		{
			for (int l = 0; l < _FromData.CookingResults.Count; l++)
			{
				CardData fromID2 = UniqueIDScriptable.GetFromID<CardData>(_FromData.CookingResults[l]);
				inGameCardBase.AddCookingResult(fromID2);
			}
		}
		if (_FromData.CookingCards.Length == 0 || !inGameCardBase.CardModel.CanCook)
		{
			return;
		}
		InGameCardBase inGameCardBase2 = null;
		for (int m = 0; m < _FromData.CookingCards.Length; m++)
		{
			if (!_FromData.CookingCards[m].UsesLiquid)
			{
				inGameCardBase2 = inGameCardBase.CardsInInventory[_FromData.CookingCards[m].CardIndex].MainCard;
			}
			else if (inGameCardBase.CardsInInventory[_FromData.CookingCards[m].CardIndex].MainCard)
			{
				inGameCardBase2 = inGameCardBase.CardsInInventory[_FromData.CookingCards[m].CardIndex].MainCard.ContainedLiquid;
			}
			CookingRecipe recipeForCard;
			if (inGameCardBase2)
			{
				recipeForCard = inGameCardBase.CardModel.GetRecipeForCard(inGameCardBase2.CardModel, inGameCardBase2, inGameCardBase);
			}
			else
			{
				recipeForCard = inGameCardBase.CardModel.GetRecipeForCard(null, null, inGameCardBase);
			}
			if (recipeForCard != null)
			{
				CookingCardStatus cookingStatusForCard = inGameCardBase.GetCookingStatusForCard(inGameCardBase2);
				if (cookingStatusForCard != null)
				{
					if (_FromData.CookingCards[m].TargetDuration == cookingStatusForCard.TargetDuration)
					{
						cookingStatusForCard.CookedDuration = _FromData.CookingCards[m].CookedDuration;
					}
					else if (_FromData.CookingCards[m].TargetDuration >= recipeForCard.MinDuration && _FromData.CookingCards[m].TargetDuration <= recipeForCard.MaxDuration)
					{
						cookingStatusForCard.CookedDuration = _FromData.CookingCards[m].CookedDuration;
						cookingStatusForCard.TargetDuration = _FromData.CookingCards[m].TargetDuration;
					}
					else
					{
						float num = (float)_FromData.CookingCards[m].CookedDuration / (float)_FromData.CookingCards[m].TargetDuration;
						cookingStatusForCard.CookedDuration = Mathf.FloorToInt(num * (float)cookingStatusForCard.TargetDuration);
					}
					if (cookingStatusForCard != null && inGameCardBase2 != null)
					{
						cookingStatusForCard.UpdateCookingProgressVisuals((float)cookingStatusForCard.CookedDuration / (float)cookingStatusForCard.TargetDuration, cookingStatusForCard.TargetDuration - cookingStatusForCard.CookedDuration, inGameCardBase2.CookingIsPaused(), cookingStatusForCard.GetCookingText(inGameCardBase2.CookingIsPaused(), cookingStatusForCard.TargetDuration - cookingStatusForCard.CookedDuration));
					}
				}
			}
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00008F90 File Offset: 0x00007190
	public static void MakeBlueprintAvailable(CardData _Blueprint)
	{
		if (!_Blueprint)
		{
			return;
		}
		if (_Blueprint.CardType != CardTypes.Blueprint)
		{
			return;
		}
		if (!MBSingleton<GameManager>.Instance)
		{
			return;
		}
		MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.MakeBlueprintAvailableRoutine(_Blueprint));
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00008FC8 File Offset: 0x000071C8
	private IEnumerator MakeBlueprintAvailableRoutine(CardData _Blueprint)
	{
		if (!_Blueprint)
		{
			yield break;
		}
		if (_Blueprint.CardType != CardTypes.Blueprint)
		{
			yield break;
		}
		if (!this.BlueprintModelStates.ContainsKey(_Blueprint))
		{
			yield break;
		}
		if (this.BlueprintModelStates[_Blueprint] == BlueprintModelState.Available)
		{
			yield break;
		}
		if (this.BlueprintModelStates[_Blueprint] == BlueprintModelState.Locked || !this.BlueprintModelCards.Contains(_Blueprint))
		{
			yield return base.StartCoroutine(this.AddCard(_Blueprint, null, true, null, true, SpawningLiquid.Empty, new Vector2Int(MBSingleton<GameManager>.Instance.CurrentTickInfo.z, 0), true));
		}
		if (_Blueprint == this.GameGraphics.BlueprintModelsPopup.CurrentResearch)
		{
			this.GameGraphics.BlueprintModelsPopup.FinishBlueprintResearch();
		}
		if (this.PurchasableBlueprintCards.Contains(_Blueprint))
		{
			this.PurchasableBlueprintCards.Remove(_Blueprint);
		}
		this.BlueprintModelStates[_Blueprint] = BlueprintModelState.Available;
		yield break;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00008FE0 File Offset: 0x000071E0
	public static void GiveCard(CardData _Data, bool _Complete)
	{
		if (!MBSingleton<GameManager>.Instance)
		{
			return;
		}
		if (_Data.CardType == CardTypes.EnvImprovement && MBSingleton<GameManager>.Instance.ImprovementCards != null && MBSingleton<GameManager>.Instance.ImprovementCards.Count > 0)
		{
			for (int i = 0; i < MBSingleton<GameManager>.Instance.ImprovementCards.Count; i++)
			{
				if (MBSingleton<GameManager>.Instance.ImprovementCards[i] && !MBSingleton<GameManager>.Instance.ImprovementCards[i].Destroyed && !MBSingleton<GameManager>.Instance.ImprovementCards[i].InBackground && MBSingleton<GameManager>.Instance.ImprovementCards[i].CardModel == _Data)
				{
					if (!MBSingleton<GameManager>.Instance.ImprovementCards[i].BlueprintComplete && _Complete)
					{
						MBSingleton<GameManager>.Instance.ImprovementCards[i].SetBlueprintStage(MBSingleton<GameManager>.Instance.ImprovementCards[i].BlueprintSteps, false);
					}
					return;
				}
			}
		}
		InGameCardBase fromCard = null;
		TransferedDurabilities transferedDurabilities = null;
		if (_Data.CardType == CardTypes.Liquid && MBSingleton<GameManager>.Instance.LiquidCheatContainers != null && MBSingleton<GameManager>.Instance.LiquidCheatContainers.Length != 0)
		{
			CardData containerModelForLiquid = GameManager.GetContainerModelForLiquid(_Data);
			MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.AddCard(containerModelForLiquid, null, true, null, true, new SpawningLiquid(_Data), new Vector2Int(MBSingleton<GameManager>.Instance.CurrentTickInfo.z, 0), true));
			fromCard = MBSingleton<GameManager>.Instance.FindLatestCreatedCard(containerModelForLiquid);
			transferedDurabilities = new TransferedDurabilities();
			transferedDurabilities.Spoilage.FloatValue = _Data.SpoilageTime;
			transferedDurabilities.Usage.FloatValue = _Data.UsageDurability;
			transferedDurabilities.Fuel.FloatValue = _Data.FuelCapacity;
			transferedDurabilities.ConsumableCharges.FloatValue = _Data.Progress;
			transferedDurabilities.Liquid = containerModelForLiquid.MaxLiquidCapacity;
			transferedDurabilities.Special1.FloatValue = _Data.SpecialDurability1;
			transferedDurabilities.Special2.FloatValue = _Data.SpecialDurability2;
			transferedDurabilities.Special3.FloatValue = _Data.SpecialDurability3;
			transferedDurabilities.Special4.FloatValue = _Data.SpecialDurability4;
		}
		MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.AddCard(_Data, fromCard, true, transferedDurabilities, true, SpawningLiquid.DefaultLiquid, new Vector2Int(MBSingleton<GameManager>.Instance.CurrentTickInfo.z, 0), true));
		if (_Complete && _Data.CardType == CardTypes.EnvImprovement)
		{
			InGameCardBase inGameCardBase = MBSingleton<GameManager>.Instance.FindLatestCreatedCard(_Data);
			if (inGameCardBase.BlueprintData.CurrentStage < inGameCardBase.BlueprintSteps)
			{
				inGameCardBase.SetBlueprintStage(inGameCardBase.BlueprintSteps, false);
			}
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000092B0 File Offset: 0x000074B0
	public static CardData GetContainerModelForLiquid(CardData _Liquid)
	{
		if (!MBSingleton<GameManager>.Instance)
		{
			return null;
		}
		for (int i = 0; i < MBSingleton<GameManager>.Instance.LiquidCheatContainers.Length; i++)
		{
			if (MBSingleton<GameManager>.Instance.LiquidCheatContainers[i].CanContainThisLiquid(_Liquid))
			{
				return MBSingleton<GameManager>.Instance.LiquidCheatContainers[i];
			}
		}
		return null;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00009304 File Offset: 0x00007504
	public static void PinCard(CardData _Data, CardData _WithLiquid, DynamicLayoutSlot _Slot)
	{
		MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.AddCard(_Data, _Slot.ToInfo(), MBSingleton<GameManager>.Instance.CurrentEnvironment, null, true, null, null, null, null, null, _Slot.WorldPosition, false, new SpawningLiquid(_WithLiquid), false, true, new Vector2Int(MBSingleton<GameManager>.Instance.CurrentTickInfo.z, 0), null, 0, -1, false, ""));
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00009370 File Offset: 0x00007570
	public static void UnpinCard(InGameCardBase _PinnedCard)
	{
		InGamePinData pinData = MBSingleton<GameManager>.Instance.GetPinData(_PinnedCard.CardModel, _PinnedCard.ContainedLiquidModel);
		if (pinData == null)
		{
			return;
		}
		pinData.IsPinned = false;
		MBSingleton<GameManager>.Instance.StartCoroutine(MBSingleton<GameManager>.Instance.RemoveCard(_PinnedCard, true, false, GameManager.RemoveOption.Standard, false));
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000093BC File Offset: 0x000075BC
	private IEnumerator AddCard(CardData _Data, InGameCardBase _FromCard, bool _InCurrentEnv, TransferedDurabilities _TransferedDurabilites, bool _UseDefaultInventory, SpawningLiquid _WithLiquid, Vector2Int _Tick, bool _MoveView = true)
	{
		if (_FromCard)
		{
			if (_Data.CardType == CardTypes.Liquid)
			{
				if (_FromCard.IsLiquidContainer)
				{
					yield return base.StartCoroutine(this.AddCard(_Data, _FromCard.CurrentSlotInfo, _FromCard.Environment, _FromCard, _InCurrentEnv, _TransferedDurabilites, null, null, null, null, _FromCard.ValidPosition, _UseDefaultInventory, _WithLiquid, _MoveView, false, _Tick, _FromCard.PrevEnvironment, _FromCard.PrevEnvTravelIndex, -1, false, ""));
				}
				else if (_FromCard.IsLiquid)
				{
					yield return base.StartCoroutine(this.AddCard(_Data, _FromCard.CurrentSlotInfo, _FromCard.Environment, _FromCard.CurrentContainer, _InCurrentEnv, _TransferedDurabilites, null, null, null, null, _FromCard.ValidPosition, _UseDefaultInventory, _WithLiquid, _MoveView, false, _Tick, _FromCard.PrevEnvironment, _FromCard.PrevEnvTravelIndex, -1, false, ""));
				}
			}
			else if (_Data.CardType == CardTypes.Blueprint)
			{
				if (_FromCard.CurrentSlotInfo.SlotType != SlotsTypes.Blueprint)
				{
					yield return base.StartCoroutine(this.AddCard(_Data, new SlotInfo(SlotsTypes.Blueprint, -2), _FromCard.Environment, _FromCard.CurrentContainer, _InCurrentEnv, _TransferedDurabilites, null, null, null, null, _FromCard.ValidPosition, _UseDefaultInventory, _WithLiquid, _MoveView, false, _Tick, _FromCard.PrevEnvironment, _FromCard.PrevEnvTravelIndex, -1, false, ""));
				}
				else
				{
					yield return base.StartCoroutine(this.AddCard(_Data, new SlotInfo(this.GameGraphics.BlueprintInstanceGoToLocations ? SlotsTypes.Location : SlotsTypes.Base, -2), _FromCard.Environment, _FromCard.CurrentContainer, _InCurrentEnv, _TransferedDurabilites, null, null, null, null, _FromCard.ValidPosition, _UseDefaultInventory, _WithLiquid, _MoveView, false, _Tick, _FromCard.PrevEnvironment, _FromCard.PrevEnvTravelIndex, -1, false, ""));
				}
			}
			else
			{
				bool flag = !_FromCard.CardModel;
				if (!flag)
				{
					flag = (_FromCard.CardModel.CardType != CardTypes.Blueprint);
				}
				if (flag || _FromCard.Destroyed)
				{
					yield return base.StartCoroutine(this.AddCard(_Data, _FromCard.CurrentSlotInfo, _FromCard.Environment, _FromCard.CurrentContainer, _InCurrentEnv, _TransferedDurabilites, null, null, null, null, _FromCard.ValidPosition, _UseDefaultInventory, _WithLiquid, _MoveView, false, _Tick, _FromCard.PrevEnvironment, _FromCard.PrevEnvTravelIndex, -1, false, ""));
				}
				else
				{
					yield return base.StartCoroutine(this.AddCard(_Data, new SlotInfo(SlotsTypes.Inventory, -2), _FromCard.Environment, _FromCard, _InCurrentEnv, _TransferedDurabilites, null, null, null, null, _FromCard.ValidPosition, _UseDefaultInventory, _WithLiquid, _MoveView, false, _Tick, _FromCard.PrevEnvironment, _FromCard.PrevEnvTravelIndex, -1, false, ""));
				}
			}
		}
		else
		{
			yield return base.StartCoroutine(this.AddCard(_Data, null, this.CurrentEnvironment, null, _InCurrentEnv, _TransferedDurabilites, null, null, null, null, this.IsInitializing ? Vector3.zero : this.GameGraphics.FadeToBlack.TimeSpentPos, _UseDefaultInventory, _WithLiquid, _MoveView, false, _Tick, null, 0, -1, false, ""));
		}
		yield break;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00009413 File Offset: 0x00007613
	private IEnumerator AddCard(SimpleCardSaveData _FromSave, Vector3 _FromPos)
	{
		if (_FromSave == null)
		{
			yield break;
		}
		UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(_FromSave.CardID);
		if (!fromID)
		{
			yield break;
		}
		CardData cardData = fromID as CardData;
		if (!cardData)
		{
			yield break;
		}
		fromID = UniqueIDScriptable.GetFromID(_FromSave.EnvironmentID);
		CardData cardData2 = fromID ? (fromID as CardData) : null;
		fromID = UniqueIDScriptable.GetFromID(_FromSave.PrevEnvironmentID);
		CardData prevEnv = fromID ? (fromID as CardData) : null;
		if (cardData2 == this.CurrentEnvironment || cardData.IndependentFromEnv)
		{
			CoroutineController controller;
			this.StartCoroutineEx(this.AddCard(cardData, _FromSave.SlotInformation, cardData2, null, false, _FromSave.GetDurabilities(), null, null, null, null, _FromPos, true, SpawningLiquid.DefaultLiquid, false, false, new Vector2Int(_FromSave.CreatedOnTick, _FromSave.CreatedInSaveDataTick), prevEnv, _FromSave.PrevEnvTravelIndex, -1, false, ""), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			controller = null;
		}
		yield break;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00009430 File Offset: 0x00007630
	private IEnumerator AddCard(CardData _Data, SlotInfo _Slot, CardData _FromEnvironment, InGameCardBase _Container, bool _InCurrentEnv, TransferedDurabilities _TransferedDurabilites, List<CollectionDropsSaveData> _Drops, List<StatTriggeredActionStatus> _StatTriggeredActions, ExplorationSaveData _Exploration, BlueprintSaveData _Blueprint, Vector3 _FromPosition, bool _UseDefaultInventory, SpawningLiquid _WithLiquid, bool _MoveView, bool _Pinned, Vector2Int _Tick, CardData _PrevEnv, int _PrevEnvTravelIndex, int _TravelCardIndex = -1, bool _IgnoreBaseRow = false, string _Name = "")
	{
		if (!_Data)
		{
			yield break;
		}
		if (_Data.CardType == CardTypes.EnvDamage)
		{
			if (!this.CurrentExplorableCard)
			{
				yield break;
			}
			if (!this.CurrentExplorableCard.CardModel)
			{
				yield break;
			}
			if (!this.CurrentExplorableCard.CardModel.HasDamage(_Data))
			{
				yield break;
			}
		}
		if (_Data.IsAutoSolvableEvent && this.AutoSolveEvents)
		{
			if (!this.EncounteredEvents.Contains(_Data))
			{
				this.EncounteredEvents.Add(_Data);
			}
			yield return GameManager.PerformAction(_Data.DismantleActions[0], null, true);
			yield break;
		}
		if (_Data.CardType == CardTypes.Liquid)
		{
			if (!_Container)
			{
				Debug.LogError("Creating liquid without container! This is not supported");
				yield break;
			}
			if (!_Container.CanReceiveLiquid(_Data))
			{
				yield break;
			}
			if (_Container.ContainedLiquid)
			{
				if (_Container.ContainedLiquid.CardModel != _Data)
				{
					yield break;
				}
				_TransferedDurabilites.Liquid = Mathf.Min(_Container.ContainedLiquid.CurrentMaxLiquidQuantity - _Container.ContainedLiquid.CurrentLiquidQuantity, _TransferedDurabilites.Liquid);
				TransferedDurabilities transferedDurabilities = CardData.CalculateLiquidDurabilities(_Container.ContainedLiquid.GetDurabilities(), _TransferedDurabilites);
				CoroutineController controller;
				this.StartCoroutineEx(this.ChangeCardDurabilities(_Container.ContainedLiquid, transferedDurabilities.Spoilage, transferedDurabilities.Usage, transferedDurabilities.Fuel, transferedDurabilities.ConsumableCharges, transferedDurabilities.Liquid, transferedDurabilities.Special1, transferedDurabilities.Special2, transferedDurabilities.Special3, transferedDurabilities.Special4, true, true), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
				yield break;
			}
		}
		bool reallyPinned = _Data.CanPile && _Pinned;
		InGameCardBase spawned = null;
		switch (_Data.CardType)
		{
		case CardTypes.Item:
			if (this.PoolCards)
			{
				spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				spawned = UnityEngine.Object.Instantiate<InGameDraggableCard>(MBSingleton<GameManager>.Instance.ItemCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			if (!reallyPinned)
			{
				this.ItemCards.Add(spawned);
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString();
				}
			}
			if (this.GameGraphics.CharacterWindow.CardIsWound(_Data) && !this.IsInitializing)
			{
				this.GameGraphics.PlayWound(_Data);
			}
			break;
		case CardTypes.Base:
			if (this.PoolCards)
			{
				spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				spawned = UnityEngine.Object.Instantiate<InGameDraggableCard>(MBSingleton<GameManager>.Instance.BaseCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			if (!reallyPinned)
			{
				this.BaseCards.Add(spawned);
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString();
				}
			}
			break;
		case CardTypes.Location:
			if (this.PoolCards)
			{
				spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				spawned = UnityEngine.Object.Instantiate<InGameCardBase>(MBSingleton<GameManager>.Instance.LocationCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			if (!reallyPinned)
			{
				this.LocationCards.Add(spawned);
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString();
				}
			}
			break;
		case CardTypes.Event:
		{
			if (reallyPinned)
			{
				Debug.LogError("Trying to pin an event card, this is not supported");
				yield break;
			}
			bool flag = _Slot != null;
			if (flag)
			{
				flag = (_Slot.SlotType == SlotsTypes.Exploration);
			}
			if (!flag)
			{
				if (this.PoolCards)
				{
					this.CurrentEventCard = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
				}
				else
				{
					this.CurrentEventCard = UnityEngine.Object.Instantiate<InGameCardBase>(this.EventCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
				}
				if (!this.EncounteredEvents.Contains(_Data))
				{
					this.EncounteredEvents.Add(_Data);
				}
				spawned = this.CurrentEventCard;
			}
			else
			{
				if (this.PoolCards)
				{
					spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
				}
				else
				{
					spawned = UnityEngine.Object.Instantiate<InGameCardBase>(this.EventCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
				}
				this.ExplorationDroppedEvents.Add(_Data);
			}
			if (!GameManager.DontRenameGOs)
			{
				spawned.name = this.SpawnedCards.ToString() + "_Event";
			}
			break;
		}
		case CardTypes.Environment:
			if (reallyPinned)
			{
				Debug.LogError("Trying to pin an environment card, this is not supported");
				yield break;
			}
			this.NextEnvironment = _Data;
			if (this.CurrentEnvironmentCard)
			{
				yield return base.StartCoroutine(this.RemoveCard(this.CurrentEnvironmentCard, true, false, GameManager.RemoveOption.Standard, false));
			}
			else if (this.AllCards.Count > 0)
			{
				yield return base.StartCoroutine(this.ChangeEnvironment());
			}
			if (this.PoolCards)
			{
				this.CurrentEnvironmentCard = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				this.CurrentEnvironmentCard = UnityEngine.Object.Instantiate<InGameCardBase>(this.EnvironmentCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			spawned = this.CurrentEnvironmentCard;
			if (!GameManager.DontRenameGOs)
			{
				spawned.name = this.SpawnedCards.ToString() + "_Env";
			}
			break;
		case CardTypes.Weather:
			if (reallyPinned)
			{
				Debug.LogError("Trying to pin a weather card, this is not supported");
				yield break;
			}
			if (this.CurrentWeatherCard)
			{
				yield return base.StartCoroutine(this.RemoveCard(this.CurrentWeatherCard, false, false, GameManager.RemoveOption.Standard, false));
			}
			if (this.PoolCards)
			{
				this.CurrentWeatherCard = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				this.CurrentWeatherCard = UnityEngine.Object.Instantiate<InGameCardBase>(this.WeatherCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			spawned = this.CurrentWeatherCard;
			if (!GameManager.DontRenameGOs)
			{
				spawned.name = this.SpawnedCards.ToString() + "_Weather";
			}
			break;
		case CardTypes.Hand:
			if (reallyPinned)
			{
				Debug.LogError("Trying to pin a hand card, this is not supported");
				yield break;
			}
			if (this.CurrentHandCard)
			{
				base.StartCoroutine(this.CurrentHandCard.DestroyCard(false));
			}
			if (this.PoolCards)
			{
				this.CurrentHandCard = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				this.CurrentHandCard = UnityEngine.Object.Instantiate<InGameDraggableCard>(MBSingleton<GameManager>.Instance.HandCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			spawned = this.CurrentHandCard;
			if (!GameManager.DontRenameGOs)
			{
				spawned.name = this.SpawnedCards.ToString();
			}
			break;
		case CardTypes.Blueprint:
		{
			bool flag2 = _Slot == null;
			if (!flag2)
			{
				flag2 = (_Slot.SlotType == SlotsTypes.Blueprint);
			}
			if (flag2)
			{
				if (this.BlueprintModelCards.Contains(_Data))
				{
					yield break;
				}
				if (this.PoolCards)
				{
					spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
				}
				else
				{
					spawned = UnityEngine.Object.Instantiate<InGameCardBase>(MBSingleton<GameManager>.Instance.BlueprintModelCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
				}
				if (!this.BlueprintPurchasing || _Data.BlueprintUnlockSunsCost <= 0f || this.StartingBlueprints.Contains(_Data))
				{
					this.BlueprintModelStates[_Data] = BlueprintModelState.Available;
					if (this.BlueprintPurchasing && this.PurchasingWithTime && this.RootAction != null && _TransferedDurabilites == null && !this.StartingBlueprints.Contains(_Data))
					{
						this.FinishedBlueprintResearch = _Data;
					}
				}
				else if (this.PurchasableBlueprintCards.Contains(_Data))
				{
					this.BlueprintModelStates[_Data] = BlueprintModelState.Purchasable;
				}
				else if (_TransferedDurabilites == null)
				{
					this.BlueprintModelStates[_Data] = BlueprintModelState.Purchasable;
					this.PurchasableBlueprintCards.Add(_Data);
				}
				else
				{
					this.BlueprintModelStates[_Data] = BlueprintModelState.Available;
				}
				this.BlueprintModelCards.Add(_Data);
				if (this.AutoSolveEvents && !this.CheckedBlueprints.Contains(_Data))
				{
					this.CheckedBlueprints.Add(_Data);
				}
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString() + "_Model";
				}
				if (_UseDefaultInventory && !this.AutoSolveEvents)
				{
					this.GameGraphics.PlayBlueprintUnlocked(_Data);
				}
			}
			else
			{
				if (this.PoolCards)
				{
					spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, true);
				}
				else
				{
					spawned = UnityEngine.Object.Instantiate<InGameCardBase>(MBSingleton<GameManager>.Instance.BlueprintInstanceCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
				}
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString() + "_Instance";
				}
				if (this.GameGraphics.BlueprintInstanceGoToLocations)
				{
					this.LocationCards.Add(spawned);
				}
				else
				{
					this.BaseCards.Add(spawned);
				}
			}
			break;
		}
		case CardTypes.Explorable:
			if (this.PoolCards)
			{
				spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				spawned = UnityEngine.Object.Instantiate<InGameCardBase>(MBSingleton<GameManager>.Instance.ExplorableCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			if (!reallyPinned)
			{
				this.LocationCards.Add(spawned);
				this.CurrentExplorableCard = spawned;
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString();
				}
			}
			break;
		case CardTypes.Liquid:
			if (reallyPinned)
			{
				Debug.LogError("Trying to pin a liquid card, this is not supported");
				yield break;
			}
			if (this.PoolCards)
			{
				spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				spawned = UnityEngine.Object.Instantiate<InGameCardBase>(MBSingleton<GameManager>.Instance.LiquidCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			this.LiquidCards.Add(spawned);
			if (!GameManager.DontRenameGOs)
			{
				spawned.name = this.SpawnedCards.ToString();
			}
			break;
		case CardTypes.EnvImprovement:
			if (this.PoolCards)
			{
				spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				spawned = UnityEngine.Object.Instantiate<InGameCardBase>(MBSingleton<GameManager>.Instance.ImprovementCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			if (!reallyPinned)
			{
				this.ImprovementCards.Add(spawned);
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString();
				}
				if (!this.UnlockedImprovements.Contains(_Data))
				{
					if (!this.AutoSolveEvents && _UseDefaultInventory)
					{
						this.GameGraphics.PlayBlueprintUnlocked(_Data);
					}
					this.UnlockedImprovements.Add(_Data);
				}
			}
			break;
		case CardTypes.EnvDamage:
			if (this.PoolCards)
			{
				spawned = CardPooling.NextCard(this.GameGraphics.CardsMovingParent, _FromPosition, _Data, false);
			}
			else
			{
				spawned = UnityEngine.Object.Instantiate<InGameCardBase>(MBSingleton<GameManager>.Instance.EnvDamageCardPrefab, _FromPosition, Quaternion.identity, this.GameGraphics.CardsMovingParent);
			}
			if (!reallyPinned)
			{
				this.EnvDamageCards.Add(spawned);
				if (!GameManager.DontRenameGOs)
				{
					spawned.name = this.SpawnedCards.ToString();
				}
				if (_Blueprint == null)
				{
					this.GameGraphics.PlayBlueprintUnlocked(_Data);
				}
			}
			break;
		}
		if (_Data.IsWeapon && !reallyPinned)
		{
			this.WeaponCards.Add(spawned);
		}
		if (_Data.IsAmmo && !reallyPinned)
		{
			this.AmmoCards.Add(spawned);
		}
		if (_Data.IsArmor && !reallyPinned)
		{
			this.ArmorCards.Add(spawned);
		}
		if (_Data.IsCover && !reallyPinned)
		{
			this.CoverCards.Add(spawned);
		}
		if (reallyPinned && !GameManager.DontRenameGOs)
		{
			spawned.name = this.SpawnedCards.ToString() + "_Pinned";
		}
		this.SpawnedCards++;
		if (!GameManager.DontRenameGOs)
		{
			InGameCardBase inGameCardBase = spawned;
			inGameCardBase.name = inGameCardBase.name + "_" + _Data.name;
		}
		spawned.SetCustomName(_Name);
		spawned.SetModel(_Data);
		spawned.SetPinned(reallyPinned, _WithLiquid.LiquidCard);
		spawned.IgnoreBaseRow = _IgnoreBaseRow;
		if (!_WithLiquid.StayEmpty)
		{
			spawned.FutureLiquidContained = _WithLiquid.GetLiquid(_Data.DefaultLiquidContained.LiquidCard);
		}
		this.LatestCreatedCards.Add(spawned);
		while (this.LatestCreatedCards.Count > 20)
		{
			this.LatestCreatedCards.RemoveAt(0);
		}
		this.AllCards.Add(spawned);
		if (_Data.HasPassiveEffects)
		{
			this.CardsWithPassiveEffects.Add(spawned);
		}
		if (_Data.CanCook)
		{
			this.CardsWithCooking.Add(spawned);
		}
		if (_Data.ActiveCounters != null && _Data.ActiveCounters.Length != 0)
		{
			this.CardsWithCounters.Add(spawned);
		}
		if (_Data.IsTravellingCard)
		{
			CardData getTravelDestination = _Data.GetTravelDestination;
			if (getTravelDestination && getTravelDestination.InstancedEnvironment)
			{
				if (_TravelCardIndex != -1)
				{
					spawned.TravelCardIndex = _TravelCardIndex;
					if (!this.TravelCardCopies.ContainsKey(_Data))
					{
						this.TravelCardCopies.Add(_Data, _TravelCardIndex + 1);
					}
					else
					{
						this.TravelCardCopies[_Data] = Mathf.Max(_TravelCardIndex + 1, this.TravelCardCopies[_Data] + 1);
					}
				}
				else if (!this.TravelCardCopies.ContainsKey(_Data))
				{
					spawned.TravelCardIndex = 0;
					this.TravelCardCopies.Add(_Data, 1);
				}
				else
				{
					spawned.TravelCardIndex = this.TravelCardCopies[_Data];
					Dictionary<CardData, int> travelCardCopies = this.TravelCardCopies;
					int num = travelCardCopies[_Data];
					travelCardCopies[_Data] = num + 1;
				}
			}
		}
		this.CheckForPassiveEffects = true;
		bool flag3 = _InCurrentEnv;
		if (_FromEnvironment != null)
		{
			flag3 |= (_FromEnvironment == this.CurrentEnvironment || _Data.CardType == CardTypes.Hand || _Data.CardType == CardTypes.Environment || _Data.CardType == CardTypes.Weather || _Data.IsMandatoryEquipment);
		}
		SlotInfo finalSlotInfo = (_Slot == null) ? new SlotInfo(this.GameGraphics.CardToSlotType(_Data.CardType, this.GameGraphics.EncounterPopupWindow.OngoingEncounter), -2) : new SlotInfo(_Slot);
		if (finalSlotInfo.SlotType == SlotsTypes.Explorable && _Data.CardType != CardTypes.Explorable)
		{
			finalSlotInfo.SlotType = SlotsTypes.Base;
			finalSlotInfo.SlotIndex = this.GameGraphics.GetCenterScreenSlotIndex(_Data.CardType);
		}
		if (flag3 && _PrevEnv != null && _Data.CardType != CardTypes.Hand && _Data.CardType != CardTypes.Environment && _Data.CardType != CardTypes.Weather && finalSlotInfo.SlotType != SlotsTypes.Item)
		{
			flag3 &= (_PrevEnv == this.PrevEnvironment && _PrevEnvTravelIndex == this.CurrentTravelIndex);
		}
		if (finalSlotInfo.SlotType != SlotsTypes.Blueprint && _Data.CardType == CardTypes.Blueprint && _Blueprint == null)
		{
			finalSlotInfo.SlotIndex = this.GameGraphics.GetCenterScreenSlotIndex(_Data.CardType);
		}
		else if (_Data.IsTravellingCard && finalSlotInfo.SlotType != SlotsTypes.Location && finalSlotInfo.SlotType != SlotsTypes.Exploration)
		{
			int travellingCardIndex = this.GetTravellingCardIndex(spawned);
			if (travellingCardIndex != -2)
			{
				finalSlotInfo = new SlotInfo(this.GameGraphics.CardToSlotType(_Data.CardType, false), travellingCardIndex);
			}
		}
		if (_Data.IsMandatoryEquipment)
		{
			finalSlotInfo = new SlotInfo(SlotsTypes.Equipment, -2);
		}
		else if (_Data.CardType == CardTypes.EnvImprovement)
		{
			finalSlotInfo = new SlotInfo(SlotsTypes.Improvement, -2);
		}
		else if (_Data.CardType == CardTypes.EnvDamage)
		{
			finalSlotInfo = new SlotInfo(SlotsTypes.EnvDamage, -2);
		}
		if (_Container)
		{
			if (_Data.CardType == CardTypes.Liquid)
			{
				_Container.SetContainedLiquid(spawned, false, _MoveView);
				spawned.CurrentContainer = _Container;
				spawned.SetParent(_Container.CurrentParentObject, true);
				finalSlotInfo = _Container.CurrentSlotInfo;
			}
			else if (finalSlotInfo.SlotType != SlotsTypes.Exploration)
			{
				finalSlotInfo = new SlotInfo(SlotsTypes.Inventory, _Container.GetIndexForInventory((_Slot == null) ? 0 : _Slot.SlotIndex, _Data, _WithLiquid.GetLiquid(_Data.DefaultLiquidContained.LiquidCard), -1f));
				if (finalSlotInfo.SlotIndex != -1)
				{
					spawned.CurrentContainer = _Container;
					spawned.CurrentSlotInfo = finalSlotInfo;
					spawned.CurrentContainer.AddCardToInventory(spawned, finalSlotInfo.SlotIndex);
				}
				else
				{
					finalSlotInfo = new SlotInfo(SlotsTypes.Item, -2);
				}
			}
			else
			{
				spawned.CurrentContainer = _Container;
				if (finalSlotInfo.SlotIndex >= 0)
				{
					spawned.CurrentContainer.AddCardToInventory(spawned, finalSlotInfo.SlotIndex);
				}
			}
		}
		if (flag3 && _Data.CardType != CardTypes.EnvImprovement && _Data.CardType != CardTypes.EnvDamage)
		{
			bool flag4 = this.GameGraphics.CurrentInspectionPopup != null;
			if (flag4)
			{
				flag4 &= (this.GameGraphics.CurrentInspectionPopup.InventorySlotSettings != null);
			}
			if (!flag4)
			{
				flag4 = (finalSlotInfo.SlotType == SlotsTypes.Exploration);
			}
			if (!spawned.CurrentContainer || (this.GameGraphics.InspectedCard == spawned.CurrentContainer && flag4))
			{
				if (finalSlotInfo.SlotType == SlotsTypes.Exploration)
				{
					yield return base.StartCoroutine(MBSingleton<ExplorationPopup>.Instance.AddCard(spawned, _Data.CardType != CardTypes.Base && _Data.CardType > CardTypes.Item));
				}
				else
				{
					this.GameGraphics.GetSlotForCard(_Data, _WithLiquid.GetLiquid(_Data.DefaultLiquidContained.LiquidCard), finalSlotInfo, null, null, 0).AssignCard(spawned, false);
					spawned.PulseAfterReachingSlot = (_FromPosition != Vector3.zero && _Data.CardType != CardTypes.Environment && !reallyPinned && _Data.CardType != CardTypes.Blueprint);
					Canvas.ForceUpdateCanvases();
					if (_FromPosition == Vector3.zero)
					{
						spawned.transform.position = spawned.CurrentSlot.GetParent.position;
						spawned.transform.SetParent(spawned.CurrentSlot.GetParent);
					}
				}
			}
			else
			{
				spawned.CurrentSlotInfo = finalSlotInfo;
			}
		}
		else
		{
			spawned.CurrentSlotInfo = finalSlotInfo;
		}
		spawned.Environment = ((_FromEnvironment && !_InCurrentEnv) ? _FromEnvironment : this.CurrentEnvironment);
		spawned.PrevEnvironment = _PrevEnv;
		spawned.PrevEnvTravelIndex = _PrevEnvTravelIndex;
		yield return base.StartCoroutine(spawned.Init(_TransferedDurabilites, _Drops, _StatTriggeredActions, _Exploration, _Blueprint, _Tick));
		if (_Data.HasRemoteEffects)
		{
			this.AddRemotePassiveEffects(spawned);
		}
		if (spawned.CardVisuals)
		{
			spawned.CardVisuals.RefreshDurabilities();
		}
		else if (spawned.CurrentContainer && _Data.CardType == CardTypes.Liquid && spawned.CurrentContainer.CardVisuals)
		{
			spawned.CurrentContainer.CardVisuals.RefreshDurabilities();
		}
		this.GameGraphics.RefreshSlots(_MoveView || _Data.CardType == CardTypes.Event);
		if (spawned.CurrentSlot && _MoveView)
		{
			this.GameGraphics.MoveViewToSlot(spawned.CurrentSlot, false, false);
		}
		if (_Data.OnStatsChangeActions != null && !spawned.IsPinned && _Data.OnStatsChangeActions.Length != 0)
		{
			List<CoroutineController> waitFor = new List<CoroutineController>();
			for (int j = 0; j < _Data.OnStatsChangeActions.Length; j++)
			{
				for (int k = 0; k < _Data.OnStatsChangeActions[j].StatChangeTrigger.Length; k++)
				{
					if (_Data.OnStatsChangeActions[j].StatChangeTrigger[k].Stat == null)
					{
						Debug.LogError("Empty stat trigger condition on " + _Data.name, spawned);
					}
					else if (this.StatsDict.ContainsKey(_Data.OnStatsChangeActions[j].StatChangeTrigger[k].Stat))
					{
						this.StatsDict[_Data.OnStatsChangeActions[j].StatChangeTrigger[k].Stat].RegisterListener(spawned);
					}
				}
				try
				{
					if (spawned.StatTriggeredActions[j].ReadyToPlay)
					{
						CoroutineController item;
						this.StartCoroutineEx(this.ActionRoutine(spawned.CardModel.OnStatsChangeActions[j], spawned, false, false), out item);
						waitFor.Add(item);
					}
				}
				catch
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Mismatch in stat trigger action count between ",
						spawned.name,
						" (",
						spawned.StatTriggeredActions.Length,
						" actions) and ",
						_Data.name,
						" (",
						_Data.OnStatsChangeActions.Length,
						" actions)"
					}));
				}
				if (spawned.Destroyed)
				{
					break;
				}
			}
			int num;
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
			waitFor = null;
		}
		if (reallyPinned && !spawned.UpdatedInBackground)
		{
			InGamePinData pinData = this.GetPinData(_Data, _WithLiquid.LiquidCard);
			if (pinData == null)
			{
				this.PinnedCards.Add(new InGamePinData(spawned, _WithLiquid.LiquidCard));
			}
			else
			{
				if (pinData.CorrespondingCard && pinData.CorrespondingCard != spawned)
				{
					base.StartCoroutine(this.RemoveCard(pinData.CorrespondingCard, false, false, GameManager.RemoveOption.Standard, false));
				}
				pinData.CorrespondingCard = spawned;
				pinData.IsPinned = true;
			}
		}
		if (_StatTriggeredActions == null)
		{
			Action<InGameCardBase> onCardSpawned = GameManager.OnCardSpawned;
			if (onCardSpawned != null)
			{
				onCardSpawned(spawned);
			}
			if (_Data.CardType == CardTypes.Blueprint && finalSlotInfo.SlotType != SlotsTypes.Blueprint)
			{
				this.CardToInspect = spawned;
			}
		}
		else
		{
			Action<InGameCardBase> onCardLoaded = GameManager.OnCardLoaded;
			if (onCardLoaded != null)
			{
				onCardLoaded(spawned);
			}
		}
		if (_UseDefaultInventory && _Data.InventorySlots != null && _Data.InventorySlots.Length != 0)
		{
			List<CoroutineController> waitFor = new List<CoroutineController>();
			for (int l = 0; l < _Data.InventorySlots.Length; l++)
			{
				CoroutineController item2;
				this.StartCoroutineEx(this.AddCard(_Data.InventorySlots[l], null, spawned.Environment, spawned, _InCurrentEnv, null, null, null, null, null, spawned.transform.position, true, SpawningLiquid.DefaultLiquid, false, false, new Vector2Int(this.CurrentTickInfo.z, 0), spawned.PrevEnvironment, spawned.PrevEnvTravelIndex, -1, false, ""), out item2);
				waitFor.Add(item2);
			}
			int num;
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
			waitFor = null;
		}
		if (_WithLiquid.UseDefault && !_Data.DefaultLiquidContained.IsEmpty && _Data.CanContainLiquid)
		{
			TransferedDurabilities transferedDurabilities2 = new TransferedDurabilities();
			transferedDurabilities2.Spoilage.FloatValue = _Data.DefaultLiquidContained.LiquidCard.SpoilageTime;
			transferedDurabilities2.Usage.FloatValue = _Data.DefaultLiquidContained.LiquidCard.UsageDurability;
			transferedDurabilities2.Fuel.FloatValue = _Data.DefaultLiquidContained.LiquidCard.FuelCapacity;
			transferedDurabilities2.ConsumableCharges.FloatValue = _Data.DefaultLiquidContained.LiquidCard.Progress;
			transferedDurabilities2.Liquid = UnityEngine.Random.Range(_Data.DefaultLiquidContained.Quantity.x, _Data.DefaultLiquidContained.Quantity.y);
			transferedDurabilities2.Special1 = _Data.DefaultLiquidContained.LiquidCard.SpecialDurability1;
			transferedDurabilities2.Special2 = _Data.DefaultLiquidContained.LiquidCard.SpecialDurability2;
			transferedDurabilities2.Special3 = _Data.DefaultLiquidContained.LiquidCard.SpecialDurability3;
			transferedDurabilities2.Special4 = _Data.DefaultLiquidContained.LiquidCard.SpecialDurability4;
			CoroutineController controller;
			this.StartCoroutineEx(this.AddCard(_Data.DefaultLiquidContained.LiquidCard, spawned, false, transferedDurabilities2, false, SpawningLiquid.Empty, new Vector2Int(this.CurrentTickInfo.z, 0), _MoveView), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			controller = null;
		}
		if (spawned == this.CurrentEnvironmentCard)
		{
			this.CalculateEnvironmentWeight(true);
		}
		yield break;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000094F0 File Offset: 0x000076F0
	public int GetTravellingCardIndex(InGameCardBase _ForCard)
	{
		int result = -1;
		for (int i = 0; i < this.LocationCards.Count; i++)
		{
			if (this.LocationCards[i] && this.LocationCards[i].CardModel.IsTravellingCard && _ForCard != this.LocationCards[i])
			{
				result = this.LocationCards[i].CurrentSlotInfo.SlotIndex;
			}
		}
		return result;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000956C File Offset: 0x0000776C
	public void ClearCardInventory(InGameCardBase _Card, bool _RemoveAll, List<int> _ExceptionSlots)
	{
		if (!_Card)
		{
			return;
		}
		for (int i = _Card.CardsInInventory.Count - 1; i >= 0; i--)
		{
			if (_Card.CardsInInventory[i] != null && !_Card.CardsInInventory[i].IsFree && (_ExceptionSlots == null || !_ExceptionSlots.Contains(i)))
			{
				for (int j = _Card.CardsInInventory[i].AllCards.Count - 1; j >= 0; j--)
				{
					base.StartCoroutine(this.RemoveCard(_Card.CardsInInventory[i].AllCards[j], false, false, _RemoveAll ? GameManager.RemoveOption.RemoveAll : GameManager.RemoveOption.Standard, false));
				}
			}
		}
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00009620 File Offset: 0x00007820
	private InGameCardBase FindLatestCreatedCard(CardData _ForCard)
	{
		List<InGameCardBase> list;
		if (this.LatestCreatedCards == null)
		{
			list = this.AllCards;
		}
		else if (this.LatestCreatedCards.Count == 0)
		{
			list = this.AllCards;
		}
		else
		{
			list = this.LatestCreatedCards;
		}
		if (list.Count == 0)
		{
			return null;
		}
		for (int i = list.Count - 1; i >= 1; i--)
		{
			if (list[i].CardModel == _ForCard)
			{
				return list[i];
			}
		}
		return list[0];
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000969C File Offset: 0x0000789C
	public void AddRemotePassiveEffects(InGameCardBase _FromCard)
	{
		if (this.CardsWithRemoteEffects.Contains(_FromCard))
		{
			return;
		}
		if (!_FromCard.CardModel)
		{
			return;
		}
		if (_FromCard.CardModel.CardType == CardTypes.EnvImprovement && !_FromCard.BlueprintComplete)
		{
			return;
		}
		this.CardsWithRemoteEffects.Add(_FromCard);
		List<InGameCardBase> list = new List<InGameCardBase>();
		for (int i = 0; i < _FromCard.CardModel.RemotePassiveEffects.Length; i++)
		{
			list.Clear();
			for (int j = 0; j < _FromCard.CardModel.RemotePassiveEffects[i].AppliesTo.Length; j++)
			{
				if (_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].IsValid)
				{
					if (_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].TargetIsTag)
					{
						this.TagIsOnBoard(_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].Tag, true, true, false, true, list);
					}
					else
					{
						this.CardIsOnBoard(_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].Card, true, true, false, true, list, Array.Empty<InGameCardBase>());
					}
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				list[k].AddExternalPassiveEffect(_FromCard.CardModel.RemotePassiveEffects[i].Effect, _FromCard);
			}
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000981C File Offset: 0x00007A1C
	private void RemoveRemotePassiveEffects(InGameCardBase _FromCard)
	{
		if (!this.CardsWithRemoteEffects.Contains(_FromCard))
		{
			return;
		}
		this.CardsWithRemoteEffects.Remove(_FromCard);
		List<InGameCardBase> list = new List<InGameCardBase>();
		for (int i = 0; i < _FromCard.CardModel.RemotePassiveEffects.Length; i++)
		{
			list.Clear();
			for (int j = 0; j < _FromCard.CardModel.RemotePassiveEffects[i].AppliesTo.Length; j++)
			{
				if (_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].IsValid)
				{
					if (_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].TargetIsTag)
					{
						this.TagIsOnBoard(_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].Tag, true, true, false, true, list);
					}
					else
					{
						this.CardIsOnBoard(_FromCard.CardModel.RemotePassiveEffects[i].AppliesTo[j].Card, true, true, false, true, list, Array.Empty<InGameCardBase>());
					}
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				list[k].RemoveExternalPassiveEffect(_FromCard.CardModel.RemotePassiveEffects[i].Effect, _FromCard);
			}
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00009976 File Offset: 0x00007B76
	private IEnumerator RemoveCard(InGameCardBase _Card, bool _NoDelay, bool _DoDrops, GameManager.RemoveOption _RemoveOption = GameManager.RemoveOption.Standard, bool _DontSpillLiquid = false)
	{
		if (!_Card)
		{
			yield break;
		}
		if (_Card.Destroyed)
		{
			yield break;
		}
		if (this.GameGraphics.EncounterPopupWindow.gameObject.activeInHierarchy && _Card.CardModel.IsWeapon)
		{
			this.GameGraphics.EncounterPopupWindow.AddToLog(_Card.CardModel.BrokenDuringCombatLog);
		}
		CoroutineController controller = null;
		if (_Card.CardModel.OnStatsChangeActions != null && _Card.CardModel.OnStatsChangeActions.Length != 0)
		{
			for (int j = 0; j < _Card.CardModel.OnStatsChangeActions.Length; j++)
			{
				for (int k = 0; k < _Card.CardModel.OnStatsChangeActions[j].StatChangeTrigger.Length; k++)
				{
					if (_Card.CardModel.OnStatsChangeActions[j].StatChangeTrigger[k].Stat == null)
					{
						Debug.LogError("Empty stat trigger condition on " + _Card.name, _Card);
					}
					else if (this.StatsDict.ContainsKey(_Card.CardModel.OnStatsChangeActions[j].StatChangeTrigger[k].Stat))
					{
						this.StatsDict[_Card.CardModel.OnStatsChangeActions[j].StatChangeTrigger[k].Stat].RemoveListener(_Card);
					}
				}
			}
		}
		if (_Card.CardModel && (_Card.CardModel.HasOnDestroyDrops && _DoDrops))
		{
			CardAction action = new CardAction
			{
				ActionName = new LocalizedString
				{
					LocalizationKey = "IGNOREKEY",
					DefaultText = "Destroy Drops"
				},
				ProducedCards = _Card.CardModel.DroppedOnDestroy
			};
			this.StartCoroutineEx(GameManager.PerformActionAsEnumerator(action, _Card, false), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
		}
		this.AllCards.Remove(_Card);
		if (this.LatestCreatedCards.Contains(_Card))
		{
			this.LatestCreatedCards.Remove(_Card);
		}
		if (this.EnvDamageCards.Contains(_Card))
		{
			this.EnvDamageCards.Remove(_Card);
		}
		if (this.ImprovementCards.Contains(_Card))
		{
			this.ImprovementCards.Remove(_Card);
		}
		if (this.CardsWithPassiveEffects.Contains(_Card))
		{
			this.CardsWithPassiveEffects.Remove(_Card);
		}
		if (_Card.CardModel.HasRemoteEffects)
		{
			this.RemoveRemotePassiveEffects(_Card);
		}
		if (this.AllVisibleCards.Contains(_Card))
		{
			this.AllVisibleCards.Remove(_Card);
		}
		if (this.CardsWithCooking.Contains(_Card))
		{
			this.CardsWithCooking.Remove(_Card);
		}
		if (this.CardsWithCounters.Contains(_Card))
		{
			this.CardsWithCounters.Remove(_Card);
		}
		if (this.WeaponCards.Contains(_Card))
		{
			this.WeaponCards.Remove(_Card);
		}
		if (this.AmmoCards.Contains(_Card))
		{
			this.AmmoCards.Remove(_Card);
		}
		if (this.ArmorCards.Contains(_Card))
		{
			this.ArmorCards.Remove(_Card);
		}
		if (this.CoverCards.Contains(_Card))
		{
			this.CoverCards.Remove(_Card);
		}
		switch (_Card.CardModel.CardType)
		{
		case CardTypes.Item:
			this.ItemCards.Remove(_Card as InGameDraggableCard);
			break;
		case CardTypes.Base:
			if (this.BaseCards.Contains(_Card))
			{
				this.BaseCards.Remove(_Card);
			}
			break;
		case CardTypes.Location:
		case CardTypes.Explorable:
			this.LocationCards.Remove(_Card);
			if (this.CurrentExplorableCard == _Card)
			{
				this.CurrentExplorableCard = null;
			}
			break;
		case CardTypes.Event:
			this.CurrentEventCard = null;
			break;
		case CardTypes.Environment:
			yield return base.StartCoroutine(this.ChangeEnvironment());
			this.PrevEnvironment = _Card.CardModel;
			this.CurrentEnvironmentCard = null;
			this.CurrentTravelIndex = this.NextTravelIndex;
			break;
		case CardTypes.Weather:
			this.CurrentWeatherCard = null;
			break;
		case CardTypes.Blueprint:
			if (this.BaseCards.Contains(_Card))
			{
				this.BaseCards.Remove(_Card);
			}
			if (this.LocationCards.Contains(_Card))
			{
				this.LocationCards.Remove(_Card);
			}
			break;
		case CardTypes.Liquid:
			this.LiquidCards.Remove(_Card);
			break;
		}
		Action<InGameCardBase> onCardDestroyed = GameManager.OnCardDestroyed;
		if (onCardDestroyed != null)
		{
			onCardDestroyed(_Card);
		}
		if (_Card.CardsInInventory != null && _Card.CardsInInventory.Count != 0)
		{
			if (_RemoveOption == GameManager.RemoveOption.Standard)
			{
				for (int l = _Card.CardsInInventory.Count - 1; l >= 0; l--)
				{
					if (_Card.CardsInInventory[l] != null && !_Card.CardsInInventory[l].IsFree)
					{
						if (_Card.CardModel.SpillsInventoryOnDestroy || _Card.CardModel.CardType == CardTypes.Blueprint || _Card.CardModel.CardType == CardTypes.EnvImprovement || _Card.CardModel.CardType == CardTypes.EnvDamage)
						{
							for (int m = _Card.CardsInInventory[l].CardAmt - 1; m >= 0; m--)
							{
								if (_Card.CardsInInventory[l].AllCards != null && _Card.CardsInInventory[l].CardModel && _Card.CardsInInventory[l].AllCards[m])
								{
									if (_Card.Environment == this.CurrentEnvironment)
									{
										this.GameGraphics.GetSlotForCard(_Card.CardsInInventory[l].CardModel, _Card.CardsInInventory[l].AllCards[m].ContainedLiquidModel, _Card.CurrentSlotInfo, null, null, 0).AssignCard(_Card.CardsInInventory[l].AllCards[m], false);
									}
									else
									{
										_Card.CardsInInventory[l].AllCards[m].CurrentSlotInfo = _Card.CurrentSlotInfo;
										_Card.CardsInInventory[l].AllCards[m].CurrentContainer = null;
									}
								}
							}
						}
						else
						{
							for (int n = _Card.CardsInInventory[l].CardAmt - 1; n >= 0; n--)
							{
								base.StartCoroutine(this.RemoveCard(_Card.CardsInInventory[l].AllCards[n], _NoDelay, false, GameManager.RemoveOption.RemoveAll, false));
							}
						}
					}
				}
			}
			else if (_RemoveOption == GameManager.RemoveOption.RemoveAll)
			{
				List<CoroutineController> waitFor = new List<CoroutineController>();
				for (int num = _Card.CardsInInventory.Count - 1; num >= 0; num--)
				{
					for (int num2 = _Card.CardsInInventory[num].CardAmt - 1; num2 >= 0; num2--)
					{
						this.StartCoroutineEx(this.RemoveCard(_Card.CardsInInventory[num].AllCards[num2], _NoDelay, false, GameManager.RemoveOption.RemoveAll, false), out controller);
						waitFor.Add(controller);
					}
				}
				int num3;
				for (int i = 0; i < waitFor.Count; i = num3 + 1)
				{
					if (waitFor[i].state != CoroutineState.Finished)
					{
						i = -1;
						yield return null;
					}
					num3 = i;
				}
				waitFor = null;
			}
		}
		if (_Card.CookingCards != null && _Card.CookingCards.Count > 0)
		{
			for (int num4 = 0; num4 < _Card.CookingCards.Count; num4++)
			{
				if (_Card.CookingCards[num4].Card && !_Card.CookingCards[num4].Card.Destroyed)
				{
					_Card.CookingCards[num4].CancelCookingProgressVisuals();
				}
			}
		}
		if (_Card.ContainedLiquid != null && (_RemoveOption == GameManager.RemoveOption.RemoveAll || !_DontSpillLiquid))
		{
			this.StartCoroutineEx(this.RemoveCard(_Card.ContainedLiquid, _NoDelay, false, GameManager.RemoveOption.RemoveAll, false), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
		}
		this.StartCoroutineEx(_Card.DestroyCard(_NoDelay), out controller);
		while (controller.state != CoroutineState.Finished)
		{
			yield return null;
		}
		this.GameGraphics.RefreshSlots(false);
		yield break;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x000099AC File Offset: 0x00007BAC
	public void SortCards()
	{
		this.AllCards.Sort(new InGameCardComparer());
		this.AllVisibleCards.Sort(new InGameCardComparer());
		this.BaseCards.Sort(new InGameCardComparer());
		this.ItemCards.Sort(new InGameCardComparer());
		this.LocationCards.Sort(new InGameCardComparer());
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00009A09 File Offset: 0x00007C09
	private IEnumerator ChangeEnvironment()
	{
		this.LeavingEnvironment = true;
		this.EnvironmentTransition = true;
		this.GameSounds.StopAllOtherAmbiences();
		if (this.CurrentEnvironment == null)
		{
			for (int j = 0; j < this.AllCards.Count; j++)
			{
				this.AllCards[j].UpdateEnvironment(this.NextEnvironment, this.CurrentEnvironment, this.NextTravelIndex);
			}
			yield break;
		}
		float seconds = this.GameGraphics.SetLoading(true);
		yield return new WaitForSeconds(seconds);
		this.GameGraphics.ClearFilterTags();
		string envKey = this.CurrentEnvironment.EnvironmentDictionaryKey(this.PrevEnvironment, this.CurrentTravelIndex);
		if (!this.EnvironmentsData.ContainsKey(envKey))
		{
			this.EnvironmentsData.Add(envKey, new EnvironmentSaveData(this.CurrentEnvironment, this.CurrentTickInfo.z, UniqueIDScriptable.AddNamesToComplexID(envKey)));
			this.EnvironmentsData[envKey].FillCounters(this.AllCounters);
		}
		else
		{
			string[] bookmarkedCardsIDs = this.EnvironmentsData[envKey].BookmarkedCardsIDs;
			string[] bookmarkedLiquidsIDs = this.EnvironmentsData[envKey].BookmarkedLiquidsIDs;
			string[] collection = this.EnvironmentsData[envKey].CheckedImprovements.ToArray();
			List<InGameTickCounter> list = new List<InGameTickCounter>();
			list.AddRange(this.EnvironmentsData[envKey].LocalCounterValues);
			this.EnvironmentsData[envKey] = new EnvironmentSaveData(this.CurrentEnvironment, this.CurrentTickInfo.z, UniqueIDScriptable.AddNamesToComplexID(envKey));
			this.EnvironmentsData[envKey].BookmarkedCardsIDs = bookmarkedCardsIDs;
			this.EnvironmentsData[envKey].BookmarkedLiquidsIDs = bookmarkedLiquidsIDs;
			this.EnvironmentsData[envKey].CheckedImprovements.AddRange(collection);
			this.EnvironmentsData[envKey].FillCounters(list);
		}
		this.CalculateEnvironmentWeight(true);
		this.EnvironmentsData[envKey].CurrentWeight = this.CurrentEnvWeight;
		this.EnvironmentsData[envKey].CurrentMaxWeight = this.MaxEnvWeight;
		List<CoroutineController> waitFor = new List<CoroutineController>();
		InGameCardBase[] array = this.AllCards.ToArray();
		List<InGameRefCardSaveData> updatedBGCards = new List<InGameRefCardSaveData>();
		List<InGameCardBase> cardsRemainingInBG = new List<InGameCardBase>();
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k].CurrentContainer != null || array[k] == this.CurrentEnvironmentCard || array[k] == this.CurrentHandCard || array[k] == this.CurrentWeatherCard || array[k] == this.CurrentEventCard)
			{
				cardsRemainingInBG.Add(array[k]);
			}
			else if (array[k].IndependentFromEnv)
			{
				if (array[k].Environment == this.NextEnvironment)
				{
					updatedBGCards.Add(array[k].MakeRefData());
				}
				else
				{
					cardsRemainingInBG.Add(array[k]);
					array[k].UpdateEnvironment(this.NextEnvironment, this.CurrentEnvironment, this.NextTravelIndex);
				}
			}
			else if (array[k].IsInventoryCard || array[k].IsLiquidContainer)
			{
				this.EnvironmentsData[envKey].AllInventoryCards.Add(array[k].SaveInventory(this.EnvironmentsData[envKey].NestedInventoryCards, false));
			}
			else
			{
				this.EnvironmentsData[envKey].AllRegularCards.Add(array[k].Save());
			}
		}
		for (int l = 0; l < this.PinnedCards.Count; l++)
		{
			if (this.EnvironmentsData[envKey].AllPinnedCards == null)
			{
				this.EnvironmentsData[envKey].AllPinnedCards = new List<PinSaveData>();
			}
			if (!this.EnvironmentsData[envKey].HasPinData(this.PinnedCards[l].PinnedCard))
			{
				this.EnvironmentsData[envKey].AllPinnedCards.Add(new PinSaveData());
				this.EnvironmentsData[envKey].AllPinnedCards[this.EnvironmentsData[envKey].AllPinnedCards.Count - 1].SavePin(this.PinnedCards[l]);
			}
		}
		this.PinnedCards.Clear();
		for (int m = 0; m < array.Length; m++)
		{
			if (!(array[m].CurrentContainer != null) && !(array[m] == this.CurrentEnvironmentCard) && !(array[m] == this.CurrentHandCard) && !(array[m] == this.CurrentWeatherCard) && !(array[m] == this.CurrentEventCard) && !array[m].IndependentFromEnv)
			{
				CoroutineController controller;
				this.StartCoroutineEx(this.RemoveCard(array[m], true, false, GameManager.RemoveOption.RemoveAll, false), out controller);
				waitFor.Add(controller);
			}
		}
		int num;
		for (int i = 0; i < waitFor.Count; i = num + 1)
		{
			if (waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num = i;
		}
		this.TravelCardCopies.Clear();
		this.AllCards.Clear();
		for (int n = 0; n < cardsRemainingInBG.Count; n++)
		{
			if (cardsRemainingInBG[n] && !cardsRemainingInBG[n].Destroyed)
			{
				this.AllCards.Add(cardsRemainingInBG[n]);
				if ((cardsRemainingInBG[n].CardModel.HasPassiveEffects || cardsRemainingInBG[n].HasExternalPassiveEffects) && !this.CardsWithPassiveEffects.Contains(cardsRemainingInBG[n]))
				{
					this.CardsWithPassiveEffects.Add(cardsRemainingInBG[n]);
				}
				if (cardsRemainingInBG[n].CardModel.HasRemoteEffects)
				{
					this.AddRemotePassiveEffects(cardsRemainingInBG[n]);
				}
			}
		}
		envKey = this.NextEnvironment.EnvironmentDictionaryKey(this.CurrentEnvironment, this.NextTravelIndex);
		CardData prevEnv = this.CurrentEnvironment;
		if (this.EnvironmentsData.ContainsKey(envKey))
		{
			if (this.EnvironmentsData[envKey].AllPinnedCards != null)
			{
				for (int num2 = 0; num2 < this.EnvironmentsData[envKey].AllPinnedCards.Count; num2++)
				{
					if (UniqueIDScriptable.GetFromID<CardData>(this.EnvironmentsData[envKey].AllPinnedCards[num2].CardID))
					{
						this.PinnedCards.Add(new InGamePinData(this.EnvironmentsData[envKey].AllPinnedCards[num2]));
					}
				}
			}
			yield return base.StartCoroutine(this.LoadCardSet(this.EnvironmentsData[envKey].AllRegularCards, this.EnvironmentsData[envKey].AllInventoryCards, this.EnvironmentsData[envKey].NestedInventoryCards, updatedBGCards, true));
			for (int num3 = 0; num3 < this.AllCards.Count; num3++)
			{
				this.AllCards[num3].UpdateEnvironment(this.NextEnvironment, this.CurrentEnvironment, this.NextTravelIndex);
			}
			this.CurrentEnvironmentCard = null;
			this.LeavingEnvironment = false;
			this.GameGraphics.RefreshSlots(false);
			yield return null;
			CoroutineController controller;
			this.StartCoroutineEx(this.UpdatePassiveEffects(), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			this.IsCatchingUp = true;
			this.CatchingUpEnvData = this.EnvironmentsData[envKey];
			for (int i = 0; i < this.CurrentTickInfo.z - this.EnvironmentsData[envKey].LastUpdatedTick; i = num + 1)
			{
				this.StartCoroutineEx(this.ApplyRates(1, this.EnvironmentsData[envKey].LastUpdatedTick + i), out controller);
				while (controller.state != CoroutineState.Finished)
				{
					yield return null;
				}
				num = i;
			}
			this.IsCatchingUp = false;
			this.CatchingUpEnvData = null;
			for (int num4 = 0; num4 < this.AllCards.Count; num4++)
			{
				this.AllCards[num4].UpdateEnvironment(this.NextEnvironment, prevEnv, this.NextTravelIndex);
			}
		}
		this.GameGraphics.LoadBookmarks(this.GetEnvSaveData(this.NextEnvironment, prevEnv, this.NextTravelIndex, false));
		this.GameGraphics.SetLoading(false);
		this.GameGraphics.ExplorationDeckPopup.SelectTab(0);
		this.EnvironmentTransition = false;
		yield break;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00009A18 File Offset: 0x00007C18
	private IEnumerator ApplyCardStateChange(CardStateChange _Change, InGameCardBase _Card, float _LiquidScale)
	{
		GameManager.<>c__DisplayClass392_0 CS$<>8__locals1;
		CS$<>8__locals1._Change = _Change;
		CS$<>8__locals1._Card = _Card;
		if (!CS$<>8__locals1._Card)
		{
			yield break;
		}
		if (!CS$<>8__locals1._Card.CardModel)
		{
			yield break;
		}
		if (CS$<>8__locals1._Card.Destroyed)
		{
			yield break;
		}
		if (CS$<>8__locals1._Change.LiquidQuantityChange != Vector2.zero && CS$<>8__locals1._Change.ModifyLiquid)
		{
			if (CS$<>8__locals1._Card.IsLiquid)
			{
				yield return base.StartCoroutine(this.ChangeCardDurabilities(CS$<>8__locals1._Card, 0f, 0f, 0f, 0f, UnityEngine.Random.Range(CS$<>8__locals1._Change.LiquidQuantityChange.x, CS$<>8__locals1._Change.LiquidQuantityChange.y), 0f, 0f, 0f, 0f, true, true));
			}
			else if (CS$<>8__locals1._Card.ContainedLiquid)
			{
				yield return base.StartCoroutine(this.ChangeCardDurabilities(CS$<>8__locals1._Card.ContainedLiquid, 0f, 0f, 0f, 0f, UnityEngine.Random.Range(CS$<>8__locals1._Change.LiquidQuantityChange.x, CS$<>8__locals1._Change.LiquidQuantityChange.y), 0f, 0f, 0f, 0f, true, true));
			}
			if (CS$<>8__locals1._Card.Destroyed)
			{
				yield break;
			}
		}
		InGameCardBase transformedCard;
		switch (CS$<>8__locals1._Change.ModType)
		{
		case CardModifications.None:
			yield break;
		case CardModifications.DurabilityChanges:
			yield return base.StartCoroutine(this.ChangeCardDurabilities(CS$<>8__locals1._Card, UnityEngine.Random.Range(CS$<>8__locals1._Change.SpoilageChange.x, CS$<>8__locals1._Change.SpoilageChange.y), UnityEngine.Random.Range(CS$<>8__locals1._Change.UsageChange.x, CS$<>8__locals1._Change.UsageChange.y), UnityEngine.Random.Range(CS$<>8__locals1._Change.FuelChange.x, CS$<>8__locals1._Change.FuelChange.y), UnityEngine.Random.Range(CS$<>8__locals1._Change.ChargesChange.x, CS$<>8__locals1._Change.ChargesChange.y), 0f, UnityEngine.Random.Range(CS$<>8__locals1._Change.Special1Change.x, CS$<>8__locals1._Change.Special1Change.y), UnityEngine.Random.Range(CS$<>8__locals1._Change.Special2Change.x, CS$<>8__locals1._Change.Special2Change.y), UnityEngine.Random.Range(CS$<>8__locals1._Change.Special3Change.x, CS$<>8__locals1._Change.Special3Change.y), UnityEngine.Random.Range(CS$<>8__locals1._Change.Special4Change.x, CS$<>8__locals1._Change.Special4Change.y), true, true));
			break;
		case CardModifications.Transform:
		{
			DynamicLayoutSlot currentSlot = CS$<>8__locals1._Card.CurrentSlot;
			List<InventorySlot> list = new List<InventorySlot>();
			List<CookingCardStatus> list2 = new List<CookingCardStatus>();
			List<CardData> results = null;
			InGameCardBase inGameCardBase = null;
			if (GameManager.<ApplyCardStateChange>g__canTransferInventory|392_0(ref CS$<>8__locals1))
			{
				for (int i = 0; i < CS$<>8__locals1._Card.CardsInInventory.Count; i++)
				{
					if (CS$<>8__locals1._Card.CardsInInventory[i] != null)
					{
						if (!CS$<>8__locals1._Card.CardsInInventory[i].IsFree)
						{
							list.Add(CS$<>8__locals1._Card.CardsInInventory[i]);
							SlotsTypes type;
							if (list[list.Count - 1].MainCard.CurrentSlotInfo != null)
							{
								type = list[list.Count - 1].MainCard.CurrentSlotInfo.SlotType;
							}
							else
							{
								type = SlotsTypes.Inventory;
							}
							for (int j = 0; j < list[list.Count - 1].CardAmt; j++)
							{
								if (list[list.Count - 1].AllCards[j])
								{
									list[list.Count - 1].AllCards[j].CurrentSlotInfo = new SlotInfo(type, list.Count - 1);
								}
							}
						}
						CS$<>8__locals1._Card.CardsInInventory[i] = null;
					}
				}
				for (int k = 0; k < CS$<>8__locals1._Card.CookingCards.Count; k++)
				{
					CookingRecipe recipeForCard;
					if (CS$<>8__locals1._Card.CookingCards[k].Card)
					{
						recipeForCard = CS$<>8__locals1._Change.TransformInto.GetRecipeForCard(CS$<>8__locals1._Card.CookingCards[k].Card.CardModel, CS$<>8__locals1._Card.CookingCards[k].Card, CS$<>8__locals1._Card);
					}
					else
					{
						recipeForCard = CS$<>8__locals1._Change.TransformInto.GetRecipeForCard(null, null, CS$<>8__locals1._Card);
					}
					if (recipeForCard != null)
					{
						list2.Add(CS$<>8__locals1._Card.CookingCards[k]);
						list2[list2.Count - 1].TargetDuration = recipeForCard.RdmDuration;
						if (list2[list2.Count - 1].Card)
						{
							for (int l = 0; l < list.Count; l++)
							{
								if (list2[list2.Count - 1].Card.IsLiquid && list2[list2.Count - 1].Card.CurrentContainer)
								{
									if (list[l].HasCard(list2[list2.Count - 1].Card.CurrentContainer))
									{
										list2[list2.Count - 1].CardIndex = l;
										break;
									}
								}
								else if (list[l].HasCard(list2[list2.Count - 1].Card))
								{
									list2[list2.Count - 1].CardIndex = l;
									break;
								}
							}
						}
					}
				}
			}
			if (CS$<>8__locals1._Card.CardModel.TransferCookingResultsOnTransform)
			{
				results = CS$<>8__locals1._Card.GetCookingResults;
			}
			if (GameManager.<ApplyCardStateChange>g__canTransferLiquid|392_1(ref CS$<>8__locals1))
			{
				inGameCardBase = CS$<>8__locals1._Card.ContainedLiquid;
			}
			if (CS$<>8__locals1._Card.IsLiquid && CS$<>8__locals1._Card.CurrentContainer && CS$<>8__locals1._Change.TransformInto)
			{
				CS$<>8__locals1._Card.CurrentContainer.StayInSlotWhenLiquidChanges = (CS$<>8__locals1._Change.TransformInto.CardType == CardTypes.Liquid);
			}
			base.StartCoroutine(this.RemoveCard(CS$<>8__locals1._Card, false, false, GameManager.<ApplyCardStateChange>g__canTransferInventory|392_0(ref CS$<>8__locals1) ? GameManager.RemoveOption.TransferInventory : GameManager.RemoveOption.Standard, GameManager.<ApplyCardStateChange>g__canTransferLiquid|392_1(ref CS$<>8__locals1)));
			if (CS$<>8__locals1._Change.TransformInto == null)
			{
				yield break;
			}
			if (!CS$<>8__locals1._Change.TransformInto.CanSpawnOnBoard())
			{
				yield break;
			}
			if (CS$<>8__locals1._Change.TransformInto.CardType == CardTypes.Event)
			{
				this.EventCardQueue.Insert(0, CS$<>8__locals1._Change.TransformInto);
				yield break;
			}
			TransferedDurabilities transferedDurabilities = new TransferedDurabilities();
			transferedDurabilities.Spoilage = new OptionalFloatValue(CS$<>8__locals1._Change.TransferSpoilage, CS$<>8__locals1._Card.CurrentSpoilage);
			transferedDurabilities.Usage = new OptionalFloatValue(CS$<>8__locals1._Change.TransferUsage, CS$<>8__locals1._Card.CurrentUsageDurability);
			transferedDurabilities.Fuel = new OptionalFloatValue(CS$<>8__locals1._Change.TransferFuel, CS$<>8__locals1._Card.CurrentFuel);
			transferedDurabilities.ConsumableCharges = new OptionalFloatValue(CS$<>8__locals1._Change.TransferCharges, CS$<>8__locals1._Card.CurrentProgress);
			transferedDurabilities.Liquid = CS$<>8__locals1._Card.CurrentLiquidQuantity;
			transferedDurabilities.Special1 = new OptionalFloatValue(CS$<>8__locals1._Change.TransferSpecial1, CS$<>8__locals1._Card.CurrentSpecial1);
			transferedDurabilities.Special2 = new OptionalFloatValue(CS$<>8__locals1._Change.TransferSpecial2, CS$<>8__locals1._Card.CurrentSpecial2);
			transferedDurabilities.Special3 = new OptionalFloatValue(CS$<>8__locals1._Change.TransferSpecial3, CS$<>8__locals1._Card.CurrentSpecial3);
			transferedDurabilities.Special4 = new OptionalFloatValue(CS$<>8__locals1._Change.TransferSpecial4, CS$<>8__locals1._Card.CurrentSpecial4);
			if (CS$<>8__locals1._Card.UpdatedInBackground && !CS$<>8__locals1._Change.TransformInto.IndependentFromEnv)
			{
				this.CreateCardAsSaveData(CS$<>8__locals1._Card, CS$<>8__locals1._Change.TransformInto, transferedDurabilities);
			}
			else
			{
				Coroutine coroutine = base.StartCoroutine(this.AddCard(CS$<>8__locals1._Change.TransformInto, CS$<>8__locals1._Card, false, transferedDurabilities, !GameManager.<ApplyCardStateChange>g__canTransferInventory|392_0(ref CS$<>8__locals1), (!GameManager.<ApplyCardStateChange>g__canTransferLiquid|392_1(ref CS$<>8__locals1)) ? SpawningLiquid.DefaultLiquid : SpawningLiquid.Empty, new Vector2Int(this.CurrentTickInfo.z, 0), true));
				transformedCard = this.FindLatestCreatedCard(CS$<>8__locals1._Change.TransformInto);
				transformedCard.SetCustomName(CS$<>8__locals1._Card.CustomName);
				if (list2.Count > 0)
				{
					transformedCard.CookingCards.AddRange(list2);
				}
				for (int m = 0; m < list.Count; m++)
				{
					if (list[m].MainCard)
					{
						for (int n = list[m].CardAmt - 1; n >= 0; n--)
						{
							list[m].AllCards[n].CurrentContainer = transformedCard;
							transformedCard.AddCardToInventory(list[m].AllCards[n], m);
						}
					}
				}
				transformedCard.TransferCookingResults(results);
				if (inGameCardBase)
				{
					if (transformedCard.CanReceiveLiquid(inGameCardBase))
					{
						transformedCard.SetContainedLiquid(inGameCardBase, false, false);
						inGameCardBase.CurrentContainer = transformedCard;
						inGameCardBase.SetParent(transformedCard.transform, true);
					}
					else
					{
						transformedCard.SetContainedLiquid(null, false, false);
					}
				}
				else
				{
					transformedCard.SetContainedLiquid(null, false, false);
				}
				yield return coroutine;
				if (CS$<>8__locals1._Change.ModifyDurability)
				{
					CardStateChange change = CS$<>8__locals1._Change;
					change.ModType = CardModifications.DurabilityChanges;
					yield return this.ApplyCardStateChange(change, transformedCard, _LiquidScale);
				}
			}
			break;
		}
		case CardModifications.Destroy:
			base.StartCoroutine(this.RemoveCard(CS$<>8__locals1._Card, false, CS$<>8__locals1._Change.DropOnDestroyList, GameManager.RemoveOption.Standard, false));
			break;
		}
		transformedCard = null;
		yield break;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00009A3C File Offset: 0x00007C3C
	private IEnumerator ApplyExtraDurabilitiesChanges(ExtraDurabilityChange[] _Changes, float _Multiplier, InGameCardBase _GivenCard, InGameCardBase _FromCard, string _PopupTitle, string _NothingAffectedMessage, string _CustomLostItemsMessage, bool _DontShowDestroyMessage, bool _FastMode)
	{
		GameManager.<>c__DisplayClass393_0 CS$<>8__locals1;
		CS$<>8__locals1._Changes = _Changes;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1._Multiplier = _Multiplier;
		CS$<>8__locals1._FastMode = _FastMode;
		if (CS$<>8__locals1._Changes == null)
		{
			yield break;
		}
		CS$<>8__locals1.applyTo = new List<InGameCardBase>();
		CS$<>8__locals1.destroyedCards = new Dictionary<string, int>();
		CS$<>8__locals1.waitFor = new List<CoroutineController>();
		GameManager.<>c__DisplayClass393_1 CS$<>8__locals2;
		CS$<>8__locals2.i = 0;
		int num13;
		while (CS$<>8__locals2.i < CS$<>8__locals1._Changes.Length)
		{
			if (CS$<>8__locals1._Changes[CS$<>8__locals2.i] != null)
			{
				CS$<>8__locals1.applyTo.Clear();
				if (!_GivenCard || CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.GivenCard)
				{
					if (_GivenCard)
					{
						yield return null;
						if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesToAllCards(false))
						{
							CS$<>8__locals1.applyTo.Add(_GivenCard);
						}
						else if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectsCard(_GivenCard))
						{
							CS$<>8__locals1.applyTo.Add(_GivenCard);
						}
					}
					else
					{
						if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.AllCardsOnBoard)
						{
							if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesToAllCards(false))
							{
								CS$<>8__locals1.applyTo.AddRange(this.AllVisibleCards);
							}
							else
							{
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards != null)
								{
									for (int j = 0; j < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards.Count; j++)
									{
										this.CardIsOnBoard(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards[j], true, CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true, false, CS$<>8__locals1.applyTo, Array.Empty<InGameCardBase>());
									}
								}
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags != null)
								{
									for (int k = 0; k < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags.Length; k++)
									{
										this.TagIsOnBoard(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags[k], true, CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true, false, CS$<>8__locals1.applyTo);
									}
								}
							}
						}
						if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.EquippedCards)
						{
							if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesToAllCards(false))
							{
								CS$<>8__locals1.applyTo.AddRange(this.GameGraphics.GetEquippedCards(true));
							}
							else
							{
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards != null)
								{
									for (int l = 0; l < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards.Count; l++)
									{
										this.GameGraphics.CharacterWindow.HasCardEquipped(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards[l], CS$<>8__locals1.applyTo);
									}
								}
								this.GameGraphics.CharacterWindow.HasTagsEquipped(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags, CS$<>8__locals1.applyTo);
							}
						}
						if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.AllCardsInHand || CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.OneCardInHand)
						{
							if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesToAllCards(false))
							{
								CS$<>8__locals1.applyTo.AddRange(this.GameGraphics.GetHandCards(CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories));
							}
							else
							{
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards != null)
								{
									for (int m = 0; m < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards.Count; m++)
									{
										this.CardIsInHand(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards[m], CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, CS$<>8__locals1.applyTo, Array.Empty<InGameCardBase>());
									}
								}
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags != null)
								{
									for (int n = 0; n < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags.Length; n++)
									{
										this.TagIsInHand(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags[n], CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, CS$<>8__locals1.applyTo, Array.Empty<InGameCardBase>());
									}
								}
							}
							if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.OneCardInHand)
							{
								InGameCardBase inGameCardBase = null;
								float num = 0f;
								float spoilageMod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].SpoilageChange.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].SpoilageChange.y, 0.5f);
								float usageMod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].UsageChange.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].UsageChange.y, 0.5f);
								float fuelMod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].FuelChange.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].FuelChange.y, 0.5f);
								float progressMod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].ChargesChange.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].ChargesChange.y, 0.5f);
								float special1Mod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special1Change.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special1Change.y, 0.5f);
								float special2Mod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special2Change.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special2Change.y, 0.5f);
								float special3Mod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special3Change.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special3Change.y, 0.5f);
								float special4Mod = Mathf.Lerp(CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special4Change.x, CS$<>8__locals1._Changes[CS$<>8__locals2.i].Special4Change.y, 0.5f);
								for (int num2 = 0; num2 < CS$<>8__locals1.applyTo.Count; num2++)
								{
									float num3 = CS$<>8__locals1.applyTo[num2].DurabilityScore(spoilageMod, usageMod, fuelMod, progressMod, special1Mod, special2Mod, special3Mod, special4Mod);
									if (num3 < num || inGameCardBase == null)
									{
										num = num3;
										inGameCardBase = CS$<>8__locals1.applyTo[num2];
									}
								}
								CS$<>8__locals1.applyTo.Clear();
								if (inGameCardBase)
								{
									CS$<>8__locals1.applyTo.Add(inGameCardBase);
								}
							}
						}
						if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.AllCardsInBase)
						{
							if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesToAllCards(false))
							{
								CS$<>8__locals1.applyTo.AddRange(this.GameGraphics.GetBaseCards(CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true));
							}
							else
							{
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards != null)
								{
									for (int num4 = 0; num4 < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards.Count; num4++)
									{
										this.CardIsInBase(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards[num4], CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true, CS$<>8__locals1.applyTo, Array.Empty<InGameCardBase>());
									}
								}
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags != null)
								{
									for (int num5 = 0; num5 < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags.Length; num5++)
									{
										this.TagIsInBase(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags[num5], CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true, CS$<>8__locals1.applyTo, Array.Empty<InGameCardBase>());
									}
								}
							}
						}
						if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.AllCardsInLocation)
						{
							if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesToAllCards(false))
							{
								CS$<>8__locals1.applyTo.AddRange(this.GameGraphics.GetLocationCards(CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true));
							}
							else
							{
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards != null)
								{
									for (int num6 = 0; num6 < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards.Count; num6++)
									{
										this.CardIsInLocation(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedCards[num6], CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true, CS$<>8__locals1.applyTo, Array.Empty<InGameCardBase>());
									}
								}
								if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags != null)
								{
									for (int num7 = 0; num7 < CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags.Length; num7++)
									{
										this.TagIsInLocation(CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectedTags[num7], CS$<>8__locals1._Changes[CS$<>8__locals2.i].LookInInventories, true, CS$<>8__locals1.applyTo, Array.Empty<InGameCardBase>());
									}
								}
							}
						}
					}
					if (CS$<>8__locals1.applyTo.Count > 0)
					{
						for (int num8 = CS$<>8__locals1.applyTo.Count - 1; num8 >= 0; num8--)
						{
							if (!CS$<>8__locals1._Changes[CS$<>8__locals2.i].AffectsCard(CS$<>8__locals1.applyTo[num8]))
							{
								CS$<>8__locals1.applyTo.RemoveAt(num8);
							}
						}
					}
					if (CS$<>8__locals1.applyTo.Count != 0)
					{
						GameManager.<>c__DisplayClass393_2 CS$<>8__locals3;
						CS$<>8__locals3.multipliedChanges = null;
						if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.OneCardInHand || CS$<>8__locals1._Changes[CS$<>8__locals2.i].RandomlyAffectedCards <= 0f)
						{
							for (int num9 = 0; num9 < CS$<>8__locals1.applyTo.Count; num9++)
							{
								if (this.<ApplyExtraDurabilitiesChanges>g__ApplyEffects|393_1(num9, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3) && CS$<>8__locals1._Changes[CS$<>8__locals2.i].AppliesTo == RemoteDurabilityChanges.OneCardInHand)
								{
									break;
								}
							}
						}
						else if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].RandomlyAffectedCards > 0f)
						{
							int num10 = 0;
							int num12;
							if (CS$<>8__locals1._Changes[CS$<>8__locals2.i].RandomAmountType == RandomAffectCardsTypes.Percentage)
							{
								for (int num11 = CS$<>8__locals1.applyTo.Count - 1; num11 >= 0; num11--)
								{
									if (!this.<ApplyExtraDurabilitiesChanges>g__CanApplyEffects|393_0(num11, ref CS$<>8__locals1, ref CS$<>8__locals2))
									{
										CS$<>8__locals1.applyTo.RemoveAt(num11);
									}
								}
								num12 = Mathf.FloorToInt((float)CS$<>8__locals1.applyTo.Count * CS$<>8__locals1._Changes[CS$<>8__locals2.i].RandomlyAffectedCards);
							}
							else
							{
								num12 = (int)CS$<>8__locals1._Changes[CS$<>8__locals2.i].RandomlyAffectedCards;
							}
							while (num10 < num12 && CS$<>8__locals1.applyTo.Count > 0)
							{
								int index = UnityEngine.Random.Range(0, CS$<>8__locals1.applyTo.Count);
								if (this.<ApplyExtraDurabilitiesChanges>g__ApplyEffects|393_1(index, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3))
								{
									num10++;
								}
								CS$<>8__locals1.applyTo.RemoveAt(index);
							}
						}
					}
				}
			}
			num13 = CS$<>8__locals2.i;
			CS$<>8__locals2.i = num13 + 1;
		}
		for (int i = 0; i < CS$<>8__locals1.waitFor.Count; i = num13 + 1)
		{
			if (CS$<>8__locals1.waitFor[i].state != CoroutineState.Finished)
			{
				i = -1;
				yield return null;
			}
			num13 = i;
		}
		if (!_DontShowDestroyMessage)
		{
			if (CS$<>8__locals1.destroyedCards.Count > 0)
			{
				if (_FromCard)
				{
					this.GameGraphics.CardsDestroyed.Setup(CS$<>8__locals1.destroyedCards, _FromCard.CardName(true), _CustomLostItemsMessage);
				}
				else
				{
					this.GameGraphics.CardsDestroyed.Setup(CS$<>8__locals1.destroyedCards, string.IsNullOrEmpty(_PopupTitle) ? LocalizedString.EventTitle : _PopupTitle, _CustomLostItemsMessage);
				}
				while (this.GameGraphics.CardsDestroyed.gameObject.activeInHierarchy)
				{
					yield return null;
				}
			}
			else if (!string.IsNullOrEmpty(_NothingAffectedMessage))
			{
				if (_FromCard)
				{
					this.GameGraphics.CardsDestroyed.Setup(_NothingAffectedMessage, _FromCard.CardName(true));
				}
				else
				{
					this.GameGraphics.CardsDestroyed.Setup(_NothingAffectedMessage, string.IsNullOrEmpty(_PopupTitle) ? LocalizedString.EventTitle : _PopupTitle);
				}
				while (this.GameGraphics.CardsDestroyed.gameObject.activeInHierarchy)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00009A9C File Offset: 0x00007C9C
	private IEnumerator ChangeCardDurabilities(InGameCardBase _Card, float _Spoilage, float _Usage, float _Fuel, float _Consumables, float _Liquid, float _Special1, float _Special2, float _Special3, float _Special4, bool _Feedback, bool _SortSlot)
	{
		if (!_Card)
		{
			yield break;
		}
		if (_Card.Destroyed)
		{
			yield break;
		}
		if (_Spoilage == 0f && _Usage == 0f && _Fuel == 0f && _Consumables == 0f && _Liquid == 0f && _Special1 == 0f && _Special2 == 0f && _Special3 == 0f && _Special4 == 0f)
		{
			yield break;
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		bool wasCooking = _Card.IsCooking();
		if (_Spoilage != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Spoilage, _Spoilage, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Usage != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Usage, _Usage, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Fuel != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Fuel, _Fuel, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Consumables != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Progress, _Consumables, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Liquid != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Liquid, _Liquid, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Special1 != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Special1, _Special1, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Special2 != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Special2, _Special2, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Special3 != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Special3, _Special3, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (_Special4 != 0f)
		{
			CoroutineController controller;
			this.StartCoroutineEx(_Card.ModifyDurability(DurabilitiesTypes.Special4, _Special4, _Feedback), out controller);
			waitFor.Add(controller);
		}
		if (waitFor.Count != 0)
		{
			int num;
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
			CoroutineController controller;
			this.StartCoroutineEx(_Card.UpdatePassiveEffects(), out controller);
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
			if (wasCooking && !_Card.IsCooking())
			{
				if (_Card.CardModel.CookingConditions.CookingPausedSound)
				{
					MBSingleton<SoundManager>.Instance.PerformSingleSound(_Card.CardModel.CookingConditions.CookingPausedSound, true, true);
				}
				if (!string.IsNullOrEmpty(_Card.CardModel.CookingConditions.CookingPausedNotification))
				{
					this.GameGraphics.PlayCardNotification(_Card, _Card.CardModel.CookingConditions.CookingPausedNotification);
				}
			}
			if (_Card.LiquidEmpty)
			{
				if (!_Card.InBackground)
				{
					this.GameGraphics.PlayCardNotification(_Card, LocalizedString.LiquidContainerEmpty(_Card.CurrentContainer));
				}
				this.StartCoroutineEx(GameManager.PerformActionAsEnumerator(CardData.OnEvaporatedAction, _Card, !_Feedback), out controller);
			}
			else
			{
				this.StartCoroutineEx(_Card.PerformDurabilitiesActions(_Feedback), out controller);
			}
			while (controller.state != CoroutineState.Finished)
			{
				yield return null;
			}
		}
		if (_SortSlot && !_Card.Destroyed)
		{
			if (_Card.CurrentSlot)
			{
				_Card.CurrentSlot.SortCardPile();
			}
			else if (_Card.IsLiquid && _Card.CurrentContainer && _Card.CurrentContainer.CurrentSlot && !_Card.CurrentContainer.Destroyed)
			{
				_Card.CurrentContainer.CurrentSlot.SortCardPile();
			}
		}
		yield break;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00009B13 File Offset: 0x00007D13
	private IEnumerator UpdateCardCooking(InGameCardBase _Card, int _TimePoints)
	{
		if (_Card.Destroyed)
		{
			yield break;
		}
		_Card.UpdateEmptyCookingRecipe();
		if (_Card.CookingCards == null)
		{
			if (_Card.CardVisuals)
			{
				_Card.CardVisuals.RefreshCookingStatus();
			}
			yield break;
		}
		if (_Card.CookingCards.Count == 0)
		{
			if (_Card.CardVisuals)
			{
				_Card.CardVisuals.RefreshCookingStatus();
			}
			yield break;
		}
		if (!_Card.CardModel.CookingConditions.CanCook(_Card.CurrentSpoilage, _Card.CurrentUsageDurability, _Card.CurrentFuel, _Card.CurrentProgress, _Card.CurrentSpecial1, _Card.CurrentSpecial2, _Card.CurrentSpecial3, _Card.CurrentSpecial4, _Card.CardModel))
		{
			if (_Card.CardVisuals)
			{
				_Card.CardVisuals.RefreshCookingStatus();
			}
			yield break;
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		InGameCardBase tmpCard = new GameObject("tmp", new Type[]
		{
			typeof(RectTransform)
		}).AddComponent<InGameCardBase>();
		if (_Card.CurrentParentObject)
		{
			tmpCard.SetParent(_Card.CurrentParentObject, true);
		}
		else
		{
			tmpCard.SetParent(_Card.transform.parent, true);
		}
		tmpCard.CurrentContainer = _Card;
		tmpCard.Environment = _Card.Environment;
		tmpCard.CurrentSpoilage = _Card.CurrentSpoilage;
		tmpCard.CurrentUsageDurability = _Card.CurrentUsageDurability;
		tmpCard.CurrentFuel = _Card.CurrentFuel;
		tmpCard.CurrentProgress = _Card.CurrentProgress;
		tmpCard.CurrentSpecial1 = _Card.CurrentSpecial1;
		tmpCard.CurrentSpecial2 = _Card.CurrentSpecial2;
		tmpCard.CurrentSpecial3 = _Card.CurrentSpecial3;
		tmpCard.CurrentSpecial4 = _Card.CurrentSpecial4;
		if (!GameManager.DontRenameGOs)
		{
			tmpCard.name = "Empty Recipe on " + _Card.name;
		}
		int num;
		for (int t = 0; t < _TimePoints; t = num + 1)
		{
			waitFor.Clear();
			_Card.UpdateEmptyCookingRecipe();
			for (int j = _Card.CookingCards.Count - 1; j >= 0; j--)
			{
				CookingRecipe recipeForCard;
				if (_Card.CookingCards[j].Card)
				{
					recipeForCard = _Card.CardModel.GetRecipeForCard(_Card.CookingCards[j].Card.CardModel, _Card.CookingCards[j].Card, _Card);
				}
				else
				{
					recipeForCard = _Card.CardModel.GetRecipeForCard(null, null, _Card);
				}
				if (recipeForCard != null)
				{
					_Card.CookingCards[j].SelfPaused = !recipeForCard.Conditions.ConditionsValid(false, (recipeForCard.ConditionsCard == CookingConditionsCard.Cooker) ? _Card : _Card.CookingCards[j].Card);
					if (_Card.CookingCards[j].SelfPaused)
					{
						if (_Card.CookingCards[j].Card)
						{
							_Card.CookingCards[j].UpdateCookingProgressVisuals((float)_Card.CookingCards[j].CookedDuration / (float)_Card.CookingCards[j].TargetDuration, _Card.CookingCards[j].TargetDuration - _Card.CookingCards[j].CookedDuration, true, _Card.CookingCards[j].SelfPausedText);
						}
						if (_Card.CardVisuals)
						{
							_Card.CardVisuals.RefreshCookingStatus();
						}
					}
					else
					{
						_Card.CookingCards[j].CookedDuration++;
						if (_Card.CookingCards[j].Card)
						{
							_Card.CookingCards[j].UpdateCookingProgressVisuals((float)_Card.CookingCards[j].CookedDuration / (float)_Card.CookingCards[j].TargetDuration, _Card.CookingCards[j].TargetDuration - _Card.CookingCards[j].CookedDuration, _Card.CookingIsPaused(), _Card.CookingCards[j].GetCookingText(_Card.CookingIsPaused(), _Card.CookingCards[j].TargetDuration - _Card.CookingCards[j].CookedDuration));
						}
						if (_Card.CookingCards[j].CookedDuration >= _Card.CookingCards[j].TargetDuration)
						{
							if (recipeForCard.Notification != CardNotifications.DontNotify && recipeForCard.IngredientChanges.ModType != CardModifications.DurabilityChanges)
							{
								if (recipeForCard.CustomCompleteSound)
								{
									MBSingleton<SoundManager>.Instance.PerformSingleSound(recipeForCard.CustomCompleteSound, false, false);
								}
								else
								{
									MBSingleton<SoundManager>.Instance.PerformSingleSound(MBSingleton<SoundManager>.Instance.DefaultCookingComplete, false, false);
								}
							}
							CoroutineController item;
							if (_Card.CookingCards[j].Card)
							{
								if (recipeForCard.IngredientChanges.ModType != CardModifications.DurabilityChanges)
								{
									if (recipeForCard.Notification == CardNotifications.UseDefault)
									{
										this.GameGraphics.PlayCardNotification(_Card, LocalizedString.FinishedCooking(_Card, _Card.CookingCards[j].Card));
									}
									else if (recipeForCard.Notification == CardNotifications.UseActionDesc)
									{
										this.GameGraphics.PlayCardNotification(_Card, string.Format(recipeForCard.CustomNotification, _Card.CookingCards[j].Card.CardName(true)));
									}
								}
								_Card.CookingCards[j].ResetDuration(recipeForCard.RdmDuration);
								this.StartCoroutineEx(this.CardOnCardActionRoutine(recipeForCard.GetResult(_Card.CookingCards[j].Card), _Card, _Card.CookingCards[j].Card, false), out item);
							}
							else
							{
								if (recipeForCard.IngredientChanges.ModType != CardModifications.DurabilityChanges)
								{
									if (recipeForCard.Notification == CardNotifications.UseDefault)
									{
										this.GameGraphics.PlayCardNotification(_Card, LocalizedString.FinishedProducing(_Card));
									}
									else if (recipeForCard.Notification == CardNotifications.UseActionDesc)
									{
										this.GameGraphics.PlayCardNotification(_Card, recipeForCard.CustomNotification);
									}
								}
								tmpCard.CurrentSlotInfo = new SlotInfo(SlotsTypes.Inventory, _Card.CookingCards[j].CardIndex);
								_Card.CookingCards.RemoveAt(j);
								this.StartCoroutineEx(this.CardOnCardActionRoutine(recipeForCard.GetResult(null), _Card, tmpCard, false), out item);
							}
							waitFor.Add(item);
						}
					}
				}
			}
			for (int i = 0; i < waitFor.Count; i = num + 1)
			{
				if (waitFor[i].state != CoroutineState.Finished)
				{
					i = -1;
					yield return null;
				}
				num = i;
			}
			num = t;
		}
		if (_Card && !_Card.Destroyed)
		{
			_Card.UpdateEmptyCookingRecipe();
			if (_Card.CardVisuals)
			{
				_Card.CardVisuals.RefreshCookingStatus();
			}
		}
		UnityEngine.Object.Destroy(tmpCard.gameObject);
		yield break;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00009B30 File Offset: 0x00007D30
	public InGamePinData GetPinData(CardData _Card, CardData _WithLiquid)
	{
		if (this.PinnedCards == null)
		{
			return null;
		}
		if (this.PinnedCards.Count == 0)
		{
			return null;
		}
		for (int i = 0; i < this.PinnedCards.Count; i++)
		{
			if (this.PinnedCards[i].PinnedCard == _Card && this.PinnedCards[i].PinnedLiquid == _WithLiquid)
			{
				return this.PinnedCards[i];
			}
		}
		return null;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00009BAC File Offset: 0x00007DAC
	public bool CheckExclusiveGroups(CardData _ForCard)
	{
		if (this.ExclusiveGroups == null)
		{
			return true;
		}
		for (int i = 0; i < this.ExclusiveGroups.Count; i++)
		{
			if (this.ExclusiveGroups[i] && !this.ExclusiveGroups[i].CardCanSpawn(_ForCard))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00009C04 File Offset: 0x00007E04
	public bool CardIsOnBoard(CardData _Card, bool _ActiveImprovements, bool _CountInInventories = true, bool _BlueprintSpecialCase = false, bool _CountInBackground = false, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		GameManager.<>c__DisplayClass398_0 CS$<>8__locals1;
		CS$<>8__locals1._Exceptions = _Exceptions;
		if (_Card == null)
		{
			return false;
		}
		if (_Card.CardType == CardTypes.Liquid && !_CountInInventories)
		{
			return false;
		}
		bool result = false;
		if (_Card.CardType == CardTypes.Base || (_Card.CardType == CardTypes.Blueprint && !this.GameGraphics.BlueprintInstanceGoToLocations))
		{
			for (int i = 0; i < this.BaseCards.Count; i++)
			{
				if ((!this.BaseCards[i].InBackground || _CountInBackground) && this.BaseCards[i].CurrentSlotInfo.SlotType != SlotsTypes.Exploration)
				{
					if (_BlueprintSpecialCase && this.BaseCards[i].CardModel.CardType == CardTypes.Blueprint)
					{
						if (this.BaseCards[i].CardModel.BlueprintResult[0].DroppedCard != _Card)
						{
							goto IL_184;
						}
						if (this.BaseCards[i].BlueprintData.CurrentStage == 0)
						{
							goto IL_184;
						}
					}
					else if (this.BaseCards[i].CardModel != _Card)
					{
						goto IL_184;
					}
					if ((_CountInInventories || !this.BaseCards[i].HiddenInInventory || (this.BaseCards[i].CurrentContainer.CardModel.CardType == CardTypes.Blueprint && _BlueprintSpecialCase)) && !GameManager.<CardIsOnBoard>g__isException|398_0(this.BaseCards[i], ref CS$<>8__locals1))
					{
						if (_Results == null)
						{
							return true;
						}
						if (!_Results.Contains(this.BaseCards[i]))
						{
							_Results.Add(this.BaseCards[i]);
						}
						result = true;
					}
				}
				IL_184:;
			}
			if (this.GameGraphics.BlueprintInstanceGoToLocations && _BlueprintSpecialCase)
			{
				for (int j = 0; j < this.LocationCards.Count; j++)
				{
					if ((!this.LocationCards[j].InBackground || _CountInBackground) && this.LocationCards[j].CardModel.CardType == CardTypes.Blueprint && !(this.LocationCards[j].CardModel.BlueprintResult[0].DroppedCard != _Card) && this.LocationCards[j].BlueprintData.CurrentStage != 0 && !GameManager.<CardIsOnBoard>g__isException|398_0(this.LocationCards[j], ref CS$<>8__locals1))
					{
						if (_Results == null)
						{
							return true;
						}
						if (!_Results.Contains(this.LocationCards[j]))
						{
							_Results.Add(this.LocationCards[j]);
						}
						result = true;
					}
				}
			}
			return result;
		}
		if (_Card.CardType == CardTypes.Explorable || _Card.CardType == CardTypes.Location || (_Card.CardType == CardTypes.Blueprint && this.GameGraphics.BlueprintInstanceGoToLocations))
		{
			for (int k = 0; k < this.LocationCards.Count; k++)
			{
				if (!this.LocationCards[k].InBackground || _CountInBackground)
				{
					if (_BlueprintSpecialCase && this.LocationCards[k].CardModel.CardType == CardTypes.Blueprint)
					{
						if (this.LocationCards[k].CardModel.BlueprintResult[0].DroppedCard != _Card)
						{
							goto IL_3DF;
						}
						if (this.LocationCards[k].BlueprintData.CurrentStage == 0)
						{
							goto IL_3DF;
						}
					}
					else if (this.LocationCards[k].CardModel != _Card)
					{
						goto IL_3DF;
					}
					if ((_CountInInventories || !this.LocationCards[k].HiddenInInventory || (this.LocationCards[k].CurrentContainer.CardModel.CardType == CardTypes.Blueprint && _BlueprintSpecialCase)) && !GameManager.<CardIsOnBoard>g__isException|398_0(this.LocationCards[k], ref CS$<>8__locals1))
					{
						if (_Results == null)
						{
							return true;
						}
						if (!_Results.Contains(this.LocationCards[k]))
						{
							_Results.Add(this.LocationCards[k]);
						}
						result = true;
					}
				}
				IL_3DF:;
			}
			if (!this.GameGraphics.BlueprintInstanceGoToLocations && _BlueprintSpecialCase)
			{
				for (int l = 0; l < this.BaseCards.Count; l++)
				{
					if ((!this.BaseCards[l].InBackground || _CountInBackground) && this.BaseCards[l].CardModel.CardType == CardTypes.Blueprint && !(this.BaseCards[l].CardModel.BlueprintResult[0].DroppedCard != _Card) && this.BaseCards[l].BlueprintData.CurrentStage != 0 && !GameManager.<CardIsOnBoard>g__isException|398_0(this.BaseCards[l], ref CS$<>8__locals1))
					{
						if (_Results == null)
						{
							return true;
						}
						if (!_Results.Contains(this.BaseCards[l]))
						{
							_Results.Add(this.BaseCards[l]);
						}
						result = true;
					}
				}
			}
			return result;
		}
		switch (_Card.CardType)
		{
		case CardTypes.Item:
			for (int m = 0; m < this.ItemCards.Count; m++)
			{
				if (this.ItemCards[m].CardModel == _Card && (!this.ItemCards[m].InBackground || _CountInBackground) && this.ItemCards[m].CurrentSlotInfo.SlotType != SlotsTypes.Exploration && (_CountInInventories || !this.ItemCards[m].HiddenInInventory) && !GameManager.<CardIsOnBoard>g__isException|398_0(this.ItemCards[m], ref CS$<>8__locals1))
				{
					if (_Results == null)
					{
						return true;
					}
					if (!_Results.Contains(this.ItemCards[m]))
					{
						_Results.Add(this.ItemCards[m]);
					}
					result = true;
				}
			}
			return result;
		case CardTypes.Event:
			if (this.CurrentEventCard && this.CurrentEventCard.CardModel == _Card)
			{
				if (_Results != null && !_Results.Contains(this.CurrentEventCard))
				{
					_Results.Add(this.CurrentEventCard);
				}
				return true;
			}
			return this.EncounteredEvents.Contains(_Card);
		case CardTypes.Environment:
			if (this.CurrentEnvironmentCard)
			{
				if (this.CurrentEnvironmentCard.CardModel == _Card)
				{
					if (_Results != null && !_Results.Contains(this.CurrentEnvironmentCard))
					{
						_Results.Add(this.CurrentEnvironmentCard);
					}
					return true;
				}
				if (this.CurrentEnvironmentCard.CardModel.InstancedEnvironment && this.PrevEnvironment && this.PrevEnvironment == _Card)
				{
					return true;
				}
			}
			else if (this.NextEnvironment && this.NextEnvironment == _Card)
			{
				return true;
			}
			return false;
		case CardTypes.Weather:
			if (this.CurrentWeatherCard && this.CurrentWeatherCard.CardModel == _Card)
			{
				if (_Results != null && !_Results.Contains(this.CurrentWeatherCard))
				{
					_Results.Add(this.CurrentWeatherCard);
				}
				return true;
			}
			return false;
		case CardTypes.Hand:
			if (this.CurrentHandCard && this.CurrentHandCard.CardModel == _Card)
			{
				if (_Results != null && !_Results.Contains(this.CurrentHandCard))
				{
					_Results.Add(this.CurrentHandCard);
				}
				return true;
			}
			return false;
		case CardTypes.Liquid:
			for (int n = 0; n < this.LiquidCards.Count; n++)
			{
				if (this.LiquidCards[n].CardModel == _Card && (!this.LiquidCards[n].InBackground || _CountInBackground) && !GameManager.<CardIsOnBoard>g__isException|398_0(this.LiquidCards[n], ref CS$<>8__locals1))
				{
					if (_Results == null)
					{
						return true;
					}
					if (!_Results.Contains(this.LiquidCards[n]))
					{
						_Results.Add(this.LiquidCards[n]);
					}
					result = true;
				}
			}
			return result;
		case CardTypes.EnvImprovement:
			for (int num = 0; num < this.ImprovementCards.Count; num++)
			{
				if (this.ImprovementCards[num].CardModel == _Card && (!this.ImprovementCards[num].InBackground || _CountInBackground) && !GameManager.<CardIsOnBoard>g__isException|398_0(this.ImprovementCards[num], ref CS$<>8__locals1) && (!_ActiveImprovements || this.ImprovementCards[num].BlueprintComplete))
				{
					if (_Results == null)
					{
						return true;
					}
					if (!_Results.Contains(this.ImprovementCards[num]))
					{
						_Results.Add(this.ImprovementCards[num]);
					}
					result = true;
				}
			}
			return result;
		case CardTypes.EnvDamage:
			for (int num2 = 0; num2 < this.EnvDamageCards.Count; num2++)
			{
				if (this.EnvDamageCards[num2].CardModel == _Card && (!this.EnvDamageCards[num2].InBackground || _CountInBackground) && !GameManager.<CardIsOnBoard>g__isException|398_0(this.EnvDamageCards[num2], ref CS$<>8__locals1))
				{
					if (_Results == null)
					{
						return true;
					}
					if (!_Results.Contains(this.EnvDamageCards[num2]))
					{
						_Results.Add(this.EnvDamageCards[num2]);
					}
					result = true;
				}
			}
			return result;
		}
		return false;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000A574 File Offset: 0x00008774
	public bool TagIsOnBoard(CardTag _Tag, bool _ActiveImprovements, bool _CountInInventory = true, bool _BlueprintSpecialCase = false, bool _CountInBackground = false, List<InGameCardBase> _Results = null)
	{
		bool result = false;
		if (this.CurrentEnvironmentCard && this.CurrentEnvironmentCard.CardModel && this.CurrentEnvironmentCard.CardModel.InstancedEnvironment && this.PrevEnvironment && this.PrevEnvironment.HasTag(_Tag))
		{
			return true;
		}
		List<InGameCardBase> list = _CountInBackground ? this.AllCards : this.AllVisibleCards;
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].IsPinned && list[i].CardModel && (_CountInInventory || !list[i].HiddenInInventory || (_BlueprintSpecialCase && list[i].CurrentContainer.CardModel.CardType == CardTypes.Blueprint)) && list[i].CurrentSlotInfo.SlotType != SlotsTypes.Blueprint && (list[i].CurrentSlotInfo.SlotType != SlotsTypes.Exploration || (list[i].CardModel.CardType != CardTypes.Base && list[i].CardModel.CardType != CardTypes.Item)))
			{
				if (!_BlueprintSpecialCase || list[i].CardModel.CardType != CardTypes.Blueprint)
				{
					if ((list[i].CardModel.CardType != CardTypes.EnvImprovement || list[i].BlueprintComplete || !_ActiveImprovements) && list[i].CardModel.HasTag(_Tag))
					{
						if (_Results == null)
						{
							return true;
						}
						result = true;
						if (!_Results.Contains(list[i]))
						{
							_Results.Add(list[i]);
						}
					}
				}
				else if (list[i].CardModel.BlueprintResult[0].DroppedCard.HasTag(_Tag))
				{
					if (_Results == null)
					{
						return true;
					}
					result = true;
					if (!_Results.Contains(list[i]))
					{
						_Results.Add(list[i]);
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0000A77C File Offset: 0x0000897C
	public bool InGameCardIsInHand(InGameCardBase _InGameCardBase, bool _CountInInventories = true, params InGameCardBase[] _Exceptions)
	{
		List<InGameCardBase> list = this.GameGraphics.GetHandCards(_CountInInventories);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == _InGameCardBase)
			{
				return true;
			}
		}
		list.Clear();
		list = this.GameGraphics.GetEquippedCards(false);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].CardModel.InHandWhenEquipped)
			{
				if (list[j] == _InGameCardBase)
				{
					return true;
				}
				if (list[j].HasInventoryContent)
				{
					for (int k = 0; k < list[j].CardsInInventory.Count; k++)
					{
						if (list[j].CardsInInventory[k].HasCard(_InGameCardBase))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000A848 File Offset: 0x00008A48
	public bool CardIsInHand(CardData _Card, bool _CountInInventories = true, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		List<InGameCardBase> list = this.GameGraphics.GetHandCards(_CountInInventories);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].CardModel == _Card)
			{
				if (_Results == null)
				{
					return true;
				}
				_Results.Add(list[i]);
			}
		}
		list.Clear();
		list = this.GameGraphics.GetEquippedCards(false);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].CardModel.InHandWhenEquipped)
			{
				if (list[j].CardModel == _Card)
				{
					if (_Results == null)
					{
						return true;
					}
					_Results.Add(list[j]);
				}
				if (list[j].HasInventoryContent)
				{
					for (int k = 0; k < list[j].CardsInInventory.Count; k++)
					{
						if (list[j].CardsInInventory[k].CardModel == _Card)
						{
							if (_Results == null)
							{
								return true;
							}
							_Results.AddRange(list[j].CardsInInventory[k].AllCards);
						}
					}
				}
			}
		}
		return _Results != null && _Results.Count > 0;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000A980 File Offset: 0x00008B80
	public bool TagIsInHand(CardTag _Tag, bool _CountInInventories = true, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		List<InGameCardBase> list = this.GameGraphics.GetHandCards(_CountInInventories);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].CardModel.HasTag(_Tag))
			{
				if (_Results == null)
				{
					return true;
				}
				_Results.Add(list[i]);
			}
		}
		list.Clear();
		list = this.GameGraphics.GetEquippedCards(false);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].CardModel.InHandWhenEquipped)
			{
				if (list[j].CardModel.HasTag(_Tag))
				{
					if (_Results == null)
					{
						return true;
					}
					_Results.Add(list[j]);
				}
				if (list[j].HasInventoryContent)
				{
					for (int k = 0; k < list[j].CardsInInventory.Count; k++)
					{
						if (!list[j].CardsInInventory[k].IsFree && list[j].CardsInInventory[k].CardModel.HasTag(_Tag))
						{
							if (_Results == null)
							{
								return true;
							}
							_Results.AddRange(list[j].CardsInInventory[k].AllCards);
						}
					}
				}
			}
		}
		return _Results != null && _Results.Count > 0;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x0000AAD0 File Offset: 0x00008CD0
	public bool CardIsInBase(CardData _Card, bool _CountInInventories, bool _BlueprintSpecialCase, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		List<InGameCardBase> baseCards = this.GameGraphics.GetBaseCards(_CountInInventories, _BlueprintSpecialCase);
		return this.CardIsInList(_Card, baseCards, _BlueprintSpecialCase, _Results, _Exceptions);
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0000AAF8 File Offset: 0x00008CF8
	public bool TagIsInBase(CardTag _Tag, bool _CountInInventories, bool _BlueprintSpecialCase, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		List<InGameCardBase> baseCards = this.GameGraphics.GetBaseCards(_CountInInventories, _BlueprintSpecialCase);
		return this.TagIsInList(_Tag, baseCards, _BlueprintSpecialCase, _Results, _Exceptions);
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0000AB20 File Offset: 0x00008D20
	public bool CardIsInLocation(CardData _Card, bool _CountInInventories, bool _BlueprintSpecialCase, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		List<InGameCardBase> locationCards = this.GameGraphics.GetLocationCards(_CountInInventories, _BlueprintSpecialCase);
		return this.CardIsInList(_Card, locationCards, _BlueprintSpecialCase, _Results, _Exceptions);
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0000AB48 File Offset: 0x00008D48
	public bool TagIsInLocation(CardTag _Tag, bool _CountInInventories, bool _BlueprintSpecialCase, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		List<InGameCardBase> locationCards = this.GameGraphics.GetLocationCards(_CountInInventories, _BlueprintSpecialCase);
		return this.TagIsInList(_Tag, locationCards, _BlueprintSpecialCase, _Results, _Exceptions);
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000AB70 File Offset: 0x00008D70
	private bool CardIsInList(CardData _Card, List<InGameCardBase> _InList, bool _BlueprintSpecialCase, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		GameManager.<>c__DisplayClass407_0 CS$<>8__locals1;
		CS$<>8__locals1._Exceptions = _Exceptions;
		if (_InList == null)
		{
			return false;
		}
		if (_InList.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < _InList.Count; i++)
		{
			if (!GameManager.<CardIsInList>g__isException|407_0(_InList[i], ref CS$<>8__locals1))
			{
				if (_BlueprintSpecialCase && _InList[i].CardModel.CardType == CardTypes.Blueprint)
				{
					if (_InList[i].CardModel.BlueprintResult[0].DroppedCard == _Card)
					{
						if (_Results == null)
						{
							return true;
						}
						_Results.Add(_InList[i]);
					}
				}
				else if (_InList[i].CardModel == _Card)
				{
					if (_Results == null)
					{
						return true;
					}
					_Results.Add(_InList[i]);
				}
			}
		}
		return _Results != null && _Results.Count > 0;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000AC48 File Offset: 0x00008E48
	private bool TagIsInList(CardTag _Tag, List<InGameCardBase> _InList, bool _BlueprintSpecialCase, List<InGameCardBase> _Results = null, params InGameCardBase[] _Exceptions)
	{
		GameManager.<>c__DisplayClass408_0 CS$<>8__locals1;
		CS$<>8__locals1._Exceptions = _Exceptions;
		if (_InList == null)
		{
			return false;
		}
		if (_InList.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < _InList.Count; i++)
		{
			if (!GameManager.<TagIsInList>g__isException|408_0(_InList[i], ref CS$<>8__locals1))
			{
				if (_BlueprintSpecialCase && _InList[i].CardModel.CardType == CardTypes.Blueprint)
				{
					if (_InList[i].CardModel.BlueprintResult[0].DroppedCard.HasTag(_Tag))
					{
						if (_Results == null)
						{
							return true;
						}
						_Results.Add(_InList[i]);
					}
				}
				else if (_InList[i].CardModel.HasTag(_Tag))
				{
					if (_Results == null)
					{
						return true;
					}
					_Results.Add(_InList[i]);
				}
			}
		}
		return _Results != null && _Results.Count > 0;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0000AFB8 File Offset: 0x000091B8
	[CompilerGenerated]
	internal static bool <LoadCardSet>g__cardWillBeInEnv|326_0(CardSaveData _Card, ref GameManager.<>c__DisplayClass326_0 A_1)
	{
		return !(_Card.EnvironmentID != UniqueIDScriptable.SaveID(A_1.envToCheck)) && (string.IsNullOrEmpty(_Card.PrevEnvironmentID) || !A_1.envToCheck.InstancedEnvironment || (!(_Card.PrevEnvironmentID != UniqueIDScriptable.SaveID(A_1.prevEnvToCheck)) && _Card.PrevEnvTravelIndex == A_1.indexToCheck));
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000B028 File Offset: 0x00009228
	[CompilerGenerated]
	internal static bool <ApplyCardStateChange>g__canTransferInventory|392_0(ref GameManager.<>c__DisplayClass392_0 A_0)
	{
		if (!A_0._Change.TransferInventory || !A_0._Change.TransformInto)
		{
			return false;
		}
		if (A_0._Card.CardModel.CardType == CardTypes.Event)
		{
			return false;
		}
		if (!A_0._Change.TransformInto.HasInventory)
		{
			return false;
		}
		if (!A_0._Change.TransformInto.LegacyInventory)
		{
			return A_0._Change.TransformInto.GetWeightCapacity(0f) < 0f || A_0._Change.TransformInto.GetWeightCapacity(0f) >= A_0._Card.InventoryWeight(true);
		}
		if (A_0._Card.CardModel.InventorySlots.Length <= A_0._Change.TransformInto.InventorySlots.Length)
		{
			return true;
		}
		if (Application.isEditor)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Cannot transfer inventory from ",
				A_0._Card.CardModel.name,
				" to ",
				A_0._Change.TransformInto.name,
				" because the inventory size is not the same or larger."
			}));
		}
		return false;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000B154 File Offset: 0x00009354
	[CompilerGenerated]
	internal static bool <ApplyCardStateChange>g__canTransferLiquid|392_1(ref GameManager.<>c__DisplayClass392_0 A_0)
	{
		return A_0._Change.TransformInto && (A_0._Card.IsLiquidContainer && A_0._Change.TransformInto.CanContainLiquid && A_0._Change.TransferLiquid);
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000B1A4 File Offset: 0x000093A4
	[CompilerGenerated]
	private bool <ApplyExtraDurabilitiesChanges>g__CanApplyEffects|393_0(int _Index, ref GameManager.<>c__DisplayClass393_0 A_2, ref GameManager.<>c__DisplayClass393_1 A_3)
	{
		return !A_2.applyTo[_Index].InBackground && A_2.applyTo[_Index].CardModel && (A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.Item || A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.Base || A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.Location || A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.Inventory || A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.Equipment || A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.EnvDamage || A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.Improvement || A_2.applyTo[_Index].CurrentSlotInfo.SlotType == SlotsTypes.Explorable) && A_2._Changes[A_3.i].RequiredDurabilities.ValidConditions(A_2.applyTo[_Index]);
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0000B2DC File Offset: 0x000094DC
	[CompilerGenerated]
	private bool <ApplyExtraDurabilitiesChanges>g__ApplyEffects|393_1(int _Index, ref GameManager.<>c__DisplayClass393_0 A_2, ref GameManager.<>c__DisplayClass393_1 A_3, ref GameManager.<>c__DisplayClass393_2 A_4)
	{
		if (!this.<ApplyExtraDurabilitiesChanges>g__CanApplyEffects|393_0(_Index, ref A_2, ref A_3))
		{
			return false;
		}
		if (A_2._Changes[A_3.i].CardChanges == RemoteCardStateChanges.Destroy)
		{
			if (!A_2.destroyedCards.ContainsKey(A_2.applyTo[_Index].CardName(true)))
			{
				A_2.destroyedCards.Add(A_2.applyTo[_Index].CardName(true), 1);
			}
			else
			{
				Dictionary<string, int> destroyedCards = A_2.destroyedCards;
				string key = A_2.applyTo[_Index].CardName(true);
				int num = destroyedCards[key];
				destroyedCards[key] = num + 1;
			}
			if (A_2._Changes[A_3.i].CanSendToEnvironment)
			{
				int num2 = UnityEngine.Random.Range(0, A_2._Changes[A_3.i].SendToEnvironment.Length);
				if (A_2._Changes[A_3.i].SendToEnvironment[num2] != null)
				{
					EnvironmentSaveData envSaveData = this.GetEnvSaveData(A_2._Changes[A_3.i].SendToEnvironment[num2].Card, A_2._Changes[A_3.i].SendToEnvironment[num2].PrevEnv, A_2._Changes[A_3.i].SendToEnvironment[num2].TravelIndex, true);
					if (envSaveData != null)
					{
						A_2.applyTo[_Index].Environment = A_2._Changes[A_3.i].SendToEnvironment[num2].Card;
						if (A_2.applyTo[_Index].IndependentFromEnv)
						{
							A_2.applyTo[_Index].PrevEnvironment = A_2._Changes[A_3.i].SendToEnvironment[num2].PrevEnv;
							A_2.applyTo[_Index].PrevEnvTravelIndex = A_2._Changes[A_3.i].SendToEnvironment[num2].TravelIndex;
						}
						else
						{
							A_2.applyTo[_Index].PrevEnvironment = null;
						}
						SlotInfo slotInformation = new SlotInfo(MBSingleton<GraphicsManager>.Instance.CardToSlotType(A_2.applyTo[_Index].CardModel.CardType, false), envSaveData.AllRegularCards.Count + _Index);
						A_2.applyTo[_Index].CreatedInSaveDataTick = this.CurrentTickInfo.z;
						A_2.applyTo[_Index].IgnoreBaseRow = true;
						if (A_2.applyTo[_Index].IsInventoryCard || A_2.applyTo[_Index].IsLiquidContainer)
						{
							envSaveData.AllInventoryCards.Add(A_2.applyTo[_Index].SaveInventory(envSaveData.NestedInventoryCards, true));
							envSaveData.AllInventoryCards[envSaveData.AllInventoryCards.Count - 1].SlotInformation = slotInformation;
						}
						else
						{
							envSaveData.AllRegularCards.Add(A_2.applyTo[_Index].Save());
							envSaveData.AllRegularCards[envSaveData.AllRegularCards.Count - 1].SlotInformation = slotInformation;
						}
						if (envSaveData.CurrentMaxWeight > 0f)
						{
							envSaveData.CurrentWeight += A_2.applyTo[_Index].CurrentWeight;
						}
					}
				}
			}
			this.StartCoroutineEx(this.RemoveCard(A_2.applyTo[_Index], false, A_2._Changes[A_3.i].DropOnDestroyList, GameManager.RemoveOption.RemoveAll, false), out A_2.controller);
			A_2.waitFor.Add(A_2.controller);
			return true;
		}
		A_4.multipliedChanges = A_2._Changes[A_3.i] * A_2._Multiplier;
		this.StartCoroutineEx(this.ChangeCardDurabilities(A_2.applyTo[_Index], UnityEngine.Random.Range(A_4.multipliedChanges.SpoilageChange.x, A_4.multipliedChanges.SpoilageChange.y), UnityEngine.Random.Range(A_4.multipliedChanges.UsageChange.x, A_4.multipliedChanges.UsageChange.y), UnityEngine.Random.Range(A_4.multipliedChanges.FuelChange.x, A_4.multipliedChanges.FuelChange.y), UnityEngine.Random.Range(A_4.multipliedChanges.ChargesChange.x, A_4.multipliedChanges.ChargesChange.y), UnityEngine.Random.Range(A_4.multipliedChanges.LiquidChange.x, A_4.multipliedChanges.LiquidChange.y), UnityEngine.Random.Range(A_4.multipliedChanges.Special1Change.x, A_4.multipliedChanges.Special1Change.y), UnityEngine.Random.Range(A_4.multipliedChanges.Special2Change.x, A_4.multipliedChanges.Special2Change.y), UnityEngine.Random.Range(A_4.multipliedChanges.Special3Change.x, A_4.multipliedChanges.Special3Change.y), UnityEngine.Random.Range(A_4.multipliedChanges.Special4Change.x, A_4.multipliedChanges.Special4Change.y), !A_2._FastMode, true), out A_2.controller);
		A_2.waitFor.Add(A_2.controller);
		return true;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000B7F8 File Offset: 0x000099F8
	[CompilerGenerated]
	internal static bool <CardIsOnBoard>g__isException|398_0(InGameCardBase _InGameCard, ref GameManager.<>c__DisplayClass398_0 A_1)
	{
		if (_InGameCard.IsPinned)
		{
			return true;
		}
		if (A_1._Exceptions == null || _InGameCard == null)
		{
			return false;
		}
		if (A_1._Exceptions.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < A_1._Exceptions.Length; i++)
		{
			if (_InGameCard == A_1._Exceptions[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0000B854 File Offset: 0x00009A54
	[CompilerGenerated]
	internal static bool <CardIsInList>g__isException|407_0(InGameCardBase _InGameCard, ref GameManager.<>c__DisplayClass407_0 A_1)
	{
		if (_InGameCard.IsPinned)
		{
			return true;
		}
		if (A_1._Exceptions == null || _InGameCard == null)
		{
			return false;
		}
		if (A_1._Exceptions.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < A_1._Exceptions.Length; i++)
		{
			if (_InGameCard == A_1._Exceptions[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x0000B8B0 File Offset: 0x00009AB0
	[CompilerGenerated]
	internal static bool <TagIsInList>g__isException|408_0(InGameCardBase _InGameCard, ref GameManager.<>c__DisplayClass408_0 A_1)
	{
		if (_InGameCard.IsPinned)
		{
			return true;
		}
		if (A_1._Exceptions == null || _InGameCard == null)
		{
			return false;
		}
		if (A_1._Exceptions.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < A_1._Exceptions.Length; i++)
		{
			if (_InGameCard == A_1._Exceptions[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000055 RID: 85
	public static InGameCardBase HoveredCard;

	// Token: 0x04000056 RID: 86
	private static List<InGameDraggableCard> DraggedCardStack;

	// Token: 0x04000057 RID: 87
	public static Action<InGameDraggableCard> OnBeginDragItem;

	// Token: 0x04000058 RID: 88
	public static Action<InGameDraggableCard> OnEndDragItem;

	// Token: 0x04000059 RID: 89
	public static Action<InGameCardBase> OnCardSpawned;

	// Token: 0x0400005A RID: 90
	public static Action<InGameCardBase> OnCardLoaded;

	// Token: 0x0400005B RID: 91
	public static Action<InGameCardBase> OnCardDestroyed;

	// Token: 0x0400005C RID: 92
	public static Action<CollectionDropReport> OnCollectionDropsSelected;

	// Token: 0x0400005D RID: 93
	public static Action<StatModifierReport> OnStatModified;

	// Token: 0x0400005E RID: 94
	public static Action<ActionReport> OnActionPerformed;

	// Token: 0x0400005F RID: 95
	public static Action<ActionReport> OnActionStarted;

	// Token: 0x04000060 RID: 96
	public static Action OnDismantleActionHovered;

	// Token: 0x04000061 RID: 97
	public static Action OnStatsListReady;

	// Token: 0x04000062 RID: 98
	public static Gamemode CurrentGamemode;

	// Token: 0x04000063 RID: 99
	public static PlayerCharacter CurrentPlayerCharacter;

	// Token: 0x04000064 RID: 100
	public static List<GameModifierPackage> CurrentModifierPackages;

	// Token: 0x04000065 RID: 101
	public GlobalCharacterInfo Portraits;

	// Token: 0x04000066 RID: 102
	public DayTimeSettings DaySettings;

	// Token: 0x04000067 RID: 103
	public int AutoSavesPerDay = 1;

	// Token: 0x04000068 RID: 104
	public int EditorAutoSavesPerDay = 1;

	// Token: 0x04000069 RID: 105
	public int DaysPerMoon = 30;

	// Token: 0x0400006A RID: 106
	public float GameOverSunsPerDay;

	// Token: 0x0400006B RID: 107
	public float GameOverMoonsPerDay;

	// Token: 0x0400006C RID: 108
	public float VictorySunsPerDay;

	// Token: 0x0400006D RID: 109
	public float VictoryMoonsPerDay;

	// Token: 0x0400006E RID: 110
	public SpecialActionSet TimeOptions;

	// Token: 0x0400006F RID: 111
	public LiquidTransferRules LiquidTransfers;

	// Token: 0x04000070 RID: 112
	public SuccessChanceLabels SuccessChances;

	// Token: 0x04000071 RID: 113
	public GameStat PlayerWeightStat;

	// Token: 0x04000072 RID: 114
	public bool CanTakeCardsEvenOnFullWeight;

	// Token: 0x04000073 RID: 115
	public bool CanUseDragStacks;

	// Token: 0x04000074 RID: 116
	public bool BlueprintPurchasing;

	// Token: 0x04000075 RID: 117
	public bool PurchasingWithTime;

	// Token: 0x0400007D RID: 125
	[Header("Controls")]
	public RectTransform DraggingTr;

	// Token: 0x0400007E RID: 126
	public Vector2 DraggedCardOffset;

	// Token: 0x0400007F RID: 127
	public bool CardsStartPinned;

	// Token: 0x04000080 RID: 128
	public float HoldActionDuration = 0.5f;

	// Token: 0x04000081 RID: 129
	public int MinTicksForHoldAction = 3;

	// Token: 0x04000082 RID: 130
	[Header("In-game debug")]
	[SerializeField]
	private bool DontPoolCards;

	// Token: 0x04000083 RID: 131
	[SerializeField]
	private bool DontRenameGameObjects;

	// Token: 0x04000084 RID: 132
	public List<InGameCardBase> LocationCards = new List<InGameCardBase>();

	// Token: 0x04000085 RID: 133
	public List<InGameCardBase> BaseCards = new List<InGameCardBase>();

	// Token: 0x04000086 RID: 134
	public List<InGameCardBase> ItemCards = new List<InGameCardBase>();

	// Token: 0x04000087 RID: 135
	public List<InGameCardBase> LiquidCards = new List<InGameCardBase>();

	// Token: 0x04000088 RID: 136
	public List<CardData> BlueprintModelCards = new List<CardData>();

	// Token: 0x04000089 RID: 137
	public List<CardData> PurchasableBlueprintCards = new List<CardData>();

	// Token: 0x0400008A RID: 138
	public List<CardData> AllBlueprintModels = new List<CardData>();

	// Token: 0x0400008B RID: 139
	public Dictionary<CardData, CardData> AllBlueprintResults = new Dictionary<CardData, CardData>();

	// Token: 0x0400008C RID: 140
	public Dictionary<CardData, BlueprintModelState> BlueprintModelStates = new Dictionary<CardData, BlueprintModelState>();

	// Token: 0x0400008D RID: 141
	private List<CardData> StartingBlueprints = new List<CardData>();

	// Token: 0x0400008E RID: 142
	public CardData FinishedBlueprintResearch;

	// Token: 0x0400008F RID: 143
	public List<CardData> UnlockedImprovements = new List<CardData>();

	// Token: 0x04000090 RID: 144
	public List<InGameCardBase> ImprovementCards = new List<InGameCardBase>();

	// Token: 0x04000091 RID: 145
	public List<InGameCardBase> EnvDamageCards = new List<InGameCardBase>();

	// Token: 0x04000092 RID: 146
	public List<InGameCardBase> WeaponCards = new List<InGameCardBase>();

	// Token: 0x04000093 RID: 147
	public List<InGameCardBase> AmmoCards = new List<InGameCardBase>();

	// Token: 0x04000094 RID: 148
	public List<InGameCardBase> ArmorCards = new List<InGameCardBase>();

	// Token: 0x04000095 RID: 149
	public List<InGameCardBase> CoverCards = new List<InGameCardBase>();

	// Token: 0x04000096 RID: 150
	public InGameCardBase CurrentHandCard;

	// Token: 0x04000097 RID: 151
	public InGameCardBase CurrentEnvironmentCard;

	// Token: 0x04000098 RID: 152
	public InGameCardBase CurrentWeatherCard;

	// Token: 0x04000099 RID: 153
	public InGameCardBase CurrentEventCard;

	// Token: 0x0400009A RID: 154
	public InGameCardBase CurrentExplorableCard;

	// Token: 0x0400009B RID: 155
	public List<InGameCardBase> AllVisibleCards = new List<InGameCardBase>();

	// Token: 0x0400009C RID: 156
	public List<InGameCardBase> AllCards = new List<InGameCardBase>();

	// Token: 0x0400009D RID: 157
	public List<InGameCardBase> LatestCreatedCards = new List<InGameCardBase>();

	// Token: 0x0400009E RID: 158
	public List<InGameCardBase> CardsWithPassiveEffects = new List<InGameCardBase>();

	// Token: 0x0400009F RID: 159
	public List<InGameCardBase> CardsWithRemoteEffects = new List<InGameCardBase>();

	// Token: 0x040000A0 RID: 160
	public List<InGamePinData> PinnedCards = new List<InGamePinData>();

	// Token: 0x040000A1 RID: 161
	public List<InGameStat> AllStats = new List<InGameStat>();

	// Token: 0x040000A2 RID: 162
	[NonSerialized]
	public List<InGameTickCounter> AllCounters = new List<InGameTickCounter>();

	// Token: 0x040000A3 RID: 163
	public InGameStat InGamePlayerWeight;

	// Token: 0x040000A4 RID: 164
	public float CurrentEnvWeight;

	// Token: 0x040000A5 RID: 165
	public Dictionary<GameStat, InGameStat> StatsDict = new Dictionary<GameStat, InGameStat>();

	// Token: 0x040000A6 RID: 166
	public Dictionary<LocalTickCounter, InGameTickCounter> CountersDict = new Dictionary<LocalTickCounter, InGameTickCounter>();

	// Token: 0x040000A7 RID: 167
	public List<CardData> EncounteredEvents = new List<CardData>();

	// Token: 0x040000A8 RID: 168
	public List<CardData> CheckedBlueprints = new List<CardData>();

	// Token: 0x040000A9 RID: 169
	public List<SelfTriggeredAction> AllSelfActions = new List<SelfTriggeredAction>();

	// Token: 0x040000AA RID: 170
	public List<Objective> AllObjectives = new List<Objective>();

	// Token: 0x040000AB RID: 171
	private List<Objective> HiddenObjectives = new List<Objective>();

	// Token: 0x040000AC RID: 172
	public List<ExclusiveCardOnBoardGroup> ExclusiveGroups = new List<ExclusiveCardOnBoardGroup>();

	// Token: 0x040000AD RID: 173
	public List<CharacterPerk> AllPerks = new List<CharacterPerk>();

	// Token: 0x040000AE RID: 174
	public List<CardUnlockConditions> UnlockableCards = new List<CardUnlockConditions>();

	// Token: 0x040000AF RID: 175
	public List<CardData> CardsAboutToBeUnlocked = new List<CardData>();

	// Token: 0x040000B0 RID: 176
	public List<CollectionDropReport> CurrentDismantleActions = new List<CollectionDropReport>();

	// Token: 0x040000B1 RID: 177
	[NonSerialized]
	public List<ActionModifier> CurrentActionModifiers = new List<ActionModifier>();

	// Token: 0x040000B2 RID: 178
	private List<StatusActionBlocker> CurrentActionBlockers = new List<StatusActionBlocker>();

	// Token: 0x040000B3 RID: 179
	private CardData NextEnvironment;

	// Token: 0x040000BA RID: 186
	private bool DontCheckObjectivesYet;

	// Token: 0x040000BD RID: 189
	private EnvironmentSaveData CatchingUpEnvData;

	// Token: 0x040000BE RID: 190
	private bool AutoSolveEvents;

	// Token: 0x040000BF RID: 191
	private bool ObjectivesUpdateRequested;

	// Token: 0x040000C0 RID: 192
	private const int MaxObjectiveChain = 2;

	// Token: 0x040000C1 RID: 193
	private const int MaxCreatedCardsBuffer = 20;

	// Token: 0x040000C2 RID: 194
	public Dictionary<string, EnvironmentSaveData> EnvironmentsData = new Dictionary<string, EnvironmentSaveData>();

	// Token: 0x040000C3 RID: 195
	public Dictionary<CardData, int> TravelCardCopies = new Dictionary<CardData, int>();

	// Token: 0x040000C4 RID: 196
	public Dictionary<string, int> PassiveEffectsStacks = new Dictionary<string, int>();

	// Token: 0x040000C5 RID: 197
	public Dictionary<CardData, int> BlueprintResearchTimes = new Dictionary<CardData, int>();

	// Token: 0x040000C6 RID: 198
	[Header("Initial Tests")]
	public ContentDisplayer DefaultJournal;

	// Token: 0x040000C7 RID: 199
	public ContentDisplayer DefaultGuide;

	// Token: 0x040000C8 RID: 200
	public CardData HandCard;

	// Token: 0x040000C9 RID: 201
	public List<CardData> StartingItems = new List<CardData>();

	// Token: 0x040000CA RID: 202
	public List<CardData> StartingLocations = new List<CardData>();

	// Token: 0x040000CB RID: 203
	public List<CardData> StartingBaseStructures = new List<CardData>();

	// Token: 0x040000CC RID: 204
	public CardData StartingEnvironment;

	// Token: 0x040000CD RID: 205
	public CardData StartingWeather;

	// Token: 0x040000CE RID: 206
	[Header("Prefabs")]
	public InGameDraggableCard HandCardPrefab;

	// Token: 0x040000CF RID: 207
	public InGameCardBase EnvironmentCardPrefab;

	// Token: 0x040000D0 RID: 208
	public InGameCardBase LocationCardPrefab;

	// Token: 0x040000D1 RID: 209
	public InGameDraggableCard ItemCardPrefab;

	// Token: 0x040000D2 RID: 210
	public InGameDraggableCard BaseCardPrefab;

	// Token: 0x040000D3 RID: 211
	public InGameCardBase WeatherCardPrefab;

	// Token: 0x040000D4 RID: 212
	public InGameCardBase EventCardPrefab;

	// Token: 0x040000D5 RID: 213
	public InGameCardBase ExplorableCardPrefab;

	// Token: 0x040000D6 RID: 214
	public InGameCardBase ImprovementCardPrefab;

	// Token: 0x040000D7 RID: 215
	public InGameCardBase EnvDamageCardPrefab;

	// Token: 0x040000D8 RID: 216
	public InGameCardBase BlueprintModelCardPrefab;

	// Token: 0x040000D9 RID: 217
	public InGameCardBase BlueprintInstanceCardPrefab;

	// Token: 0x040000DA RID: 218
	public InGameCardBase LiquidCardPrefab;

	// Token: 0x040000DB RID: 219
	public CardData[] LiquidCheatContainers;

	// Token: 0x040000DC RID: 220
	private GraphicsManager GameGraphics;

	// Token: 0x040000DD RID: 221
	private SoundManager GameSounds;

	// Token: 0x040000DE RID: 222
	private GameStates CurrentGameState;

	// Token: 0x040000DF RID: 223
	private Transform GameStatsParent;

	// Token: 0x040000E0 RID: 224
	private GameSaveData CurrentGameData;

	// Token: 0x040000E1 RID: 225
	private GameOptions CurrentGameOptions;

	// Token: 0x040000E2 RID: 226
	private InGameCardBase LastSpawnedCard;

	// Token: 0x040000E3 RID: 227
	private List<InGameCardBase> CardsWithCooking = new List<InGameCardBase>();

	// Token: 0x040000E4 RID: 228
	private List<InGameCardBase> CardsWithCounters = new List<InGameCardBase>();

	// Token: 0x040000E5 RID: 229
	private List<InGameCardBase> IgnoredBaseRowCards = new List<InGameCardBase>();

	// Token: 0x040000E6 RID: 230
	private List<InGameStat> StatsWithTimeOfDayMods = new List<InGameStat>();

	// Token: 0x040000E7 RID: 231
	private List<InGameStat> StatsWithTimeOfDayModsAndRequirements = new List<InGameStat>();

	// Token: 0x040000E8 RID: 232
	public List<CardData> EventCardQueue = new List<CardData>();

	// Token: 0x040000E9 RID: 233
	public List<CardData> ExplorationDroppedEvents = new List<CardData>();

	// Token: 0x040000EE RID: 238
	private InGameCardBase CardToInspect;

	// Token: 0x040000EF RID: 239
	private bool SaveAfterAction;

	// Token: 0x040000F0 RID: 240
	private bool ActionCancelled;

	// Token: 0x040000F1 RID: 241
	private int CancelledAtTick;

	// Token: 0x040000F2 RID: 242
	public List<InGameActionRef> QueuedCardActions = new List<InGameActionRef>();

	// Token: 0x040000F3 RID: 243
	private int SpawnedCards;

	// Token: 0x040000F4 RID: 244
	private GameObject OpenWindow;

	// Token: 0x040000F5 RID: 245
	private ContentDisplayer OpenContent;

	// Token: 0x040000F6 RID: 246
	private ContentDisplayer Journal;

	// Token: 0x040000F7 RID: 247
	private ContentDisplayer Guide;

	// Token: 0x040000F8 RID: 248
	[NonSerialized]
	public List<CardAction> ObjectiveActionsQueue = new List<CardAction>();

	// Token: 0x040000F9 RID: 249
	private bool PerformingObjectiveActions;

	// Token: 0x040000FC RID: 252
	private bool WillCalculateCarryWeight;

	// Token: 0x040000FD RID: 253
	private bool WillCalculateEnvironmentWeight;

	// Token: 0x040000FE RID: 254
	private float DragStackTimer;

	// Token: 0x040000FF RID: 255
	private float CancelDragStackTimer;

	// Token: 0x04000100 RID: 256
	private bool ConfirmedAction;

	// Token: 0x020001FF RID: 511
	private enum RemoveOption
	{
		// Token: 0x04001209 RID: 4617
		Standard,
		// Token: 0x0400120A RID: 4618
		TransferInventory,
		// Token: 0x0400120B RID: 4619
		RemoveAll
	}
}
