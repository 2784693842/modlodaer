using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000083 RID: 131
public class GraphicsManager : MBSingleton<GraphicsManager>
{
	// Token: 0x0600052C RID: 1324 RVA: 0x00034D73 File Offset: 0x00032F73
	public float GetActionTime(LineActionTypes _ForType)
	{
		if (_ForType == LineActionTypes.Insert)
		{
			return this.InsertingActionTime;
		}
		if (_ForType != LineActionTypes.Scroll)
		{
			return 0f;
		}
		return this.ScrollingActionTime;
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x0600052D RID: 1325 RVA: 0x00034D92 File Offset: 0x00032F92
	public static bool PlayerIsTyping
	{
		get
		{
			return GraphicsManager.CurrentTypingInput != null;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x0600052E RID: 1326 RVA: 0x00034D9F File Offset: 0x00032F9F
	private bool SomeFeedbackIsPlaying
	{
		get
		{
			return this.ObjectiveCompletedFeedbackIsPlaying || this.WoundReceivedFeedbackIsPlaying || this.BlueprintFeedbackIsPlaying;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x0600052F RID: 1327 RVA: 0x00034DB9 File Offset: 0x00032FB9
	private bool LimitedItemSlots
	{
		get
		{
			return this.MaxItemSlots > 0;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000530 RID: 1328 RVA: 0x00034DC4 File Offset: 0x00032FC4
	// (set) Token: 0x06000531 RID: 1329 RVA: 0x00034DCC File Offset: 0x00032FCC
	public InGameCardBase InspectedCard { get; private set; }

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000532 RID: 1330 RVA: 0x00034DD5 File Offset: 0x00032FD5
	public static float WeightPerCircle
	{
		get
		{
			if (!MBSingleton<GraphicsManager>.Instance)
			{
				return 1f;
			}
			return MBSingleton<GraphicsManager>.Instance.CardsWeightPerCircle;
		}
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00034DF3 File Offset: 0x00032FF3
	public static StatStatusGraphics CreateStatusGraphics(StatStatus _Status, bool _Ascending)
	{
		return MBSingleton<GraphicsManager>.Instance.GetStatusGraphics(_Status, _Ascending);
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00034E04 File Offset: 0x00033004
	public static Vector2 GetNotificationDelay(NotificationFrequency _Freq)
	{
		switch (_Freq)
		{
		case NotificationFrequency.Rarely:
			return MBSingleton<GraphicsManager>.Instance.RarelyNotifyDelay;
		case NotificationFrequency.Sometimes:
			return MBSingleton<GraphicsManager>.Instance.SometimesNotifyDelay;
		case NotificationFrequency.Often:
			return MBSingleton<GraphicsManager>.Instance.OftenNotifyDelay;
		case NotificationFrequency.Constantly:
			return MBSingleton<GraphicsManager>.Instance.ConstantlyModifyDelay;
		default:
			return Vector2.zero;
		}
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00034E5C File Offset: 0x0003305C
	public static bool IsStatusCritical(AlertLevels _AlertLevel)
	{
		switch (_AlertLevel)
		{
		case AlertLevels.Low:
			return MBSingleton<GraphicsManager>.Instance.LowAlertSettings.CriticalMode;
		case AlertLevels.Moderate:
			return MBSingleton<GraphicsManager>.Instance.ModerateAlertSettings.CriticalMode;
		case AlertLevels.High:
			return MBSingleton<GraphicsManager>.Instance.HighAlertSettings.CriticalMode;
		case AlertLevels.Critical:
			return MBSingleton<GraphicsManager>.Instance.CriticalAlertSettings.CriticalMode;
		case AlertLevels.Terminal:
			return MBSingleton<GraphicsManager>.Instance.TerminalAlertSettings.CriticalMode;
		default:
			return false;
		}
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00034ED8 File Offset: 0x000330D8
	public static StatusAlertSettings GetStatusAlert(AlertLevels _Level)
	{
		switch (_Level)
		{
		case AlertLevels.Low:
			return MBSingleton<GraphicsManager>.Instance.LowAlertSettings;
		case AlertLevels.Moderate:
			return MBSingleton<GraphicsManager>.Instance.ModerateAlertSettings;
		case AlertLevels.High:
			return MBSingleton<GraphicsManager>.Instance.HighAlertSettings;
		case AlertLevels.Critical:
			return MBSingleton<GraphicsManager>.Instance.CriticalAlertSettings;
		case AlertLevels.Terminal:
			return MBSingleton<GraphicsManager>.Instance.TerminalAlertSettings;
		default:
			return StatusAlertSettings.NoAlert;
		}
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00034F40 File Offset: 0x00033140
	public static float GetPulseFreq(AlertLevels _Level, bool _Outline)
	{
		switch (_Level)
		{
		case AlertLevels.Low:
			if (!_Outline)
			{
				return MBSingleton<GraphicsManager>.Instance.LowAlertPulseFreq;
			}
			return MBSingleton<GraphicsManager>.Instance.LowAlertOutlinePulseFreq;
		case AlertLevels.Moderate:
			if (!_Outline)
			{
				return MBSingleton<GraphicsManager>.Instance.ModerateAlertPulseFreq;
			}
			return MBSingleton<GraphicsManager>.Instance.ModerateAlertOutlinePulseFreq;
		case AlertLevels.High:
			if (!_Outline)
			{
				return MBSingleton<GraphicsManager>.Instance.HighAlertPulseFreq;
			}
			return MBSingleton<GraphicsManager>.Instance.HighAlertOutlinePulseFreq;
		case AlertLevels.Critical:
			if (!_Outline)
			{
				return MBSingleton<GraphicsManager>.Instance.CriticalAlertPulseFreq;
			}
			return MBSingleton<GraphicsManager>.Instance.CriticalAlertOutlinePulseFreq;
		case AlertLevels.Terminal:
			if (!_Outline)
			{
				return MBSingleton<GraphicsManager>.Instance.TerminalAlertPulseFreq;
			}
			return MBSingleton<GraphicsManager>.Instance.TerminalAlertOutlinePulseFreq;
		default:
			if (!_Outline)
			{
				return 0f;
			}
			return 1f;
		}
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00034FF8 File Offset: 0x000331F8
	public static float GetBlinkFreq(AlertLevels _Level)
	{
		switch (_Level)
		{
		case AlertLevels.Low:
			return MBSingleton<GraphicsManager>.Instance.LowAlertOutlineBlinkFreq;
		case AlertLevels.Moderate:
			return MBSingleton<GraphicsManager>.Instance.ModerateAlertOutlineBlinkFreq;
		case AlertLevels.High:
			return MBSingleton<GraphicsManager>.Instance.HighAlertOutlineBlinkFreq;
		case AlertLevels.Critical:
			return MBSingleton<GraphicsManager>.Instance.CriticalAlertOutlineBlinkFreq;
		case AlertLevels.Terminal:
			return MBSingleton<GraphicsManager>.Instance.TerminalAlertOutlineBlinkFreq;
		default:
			return 0f;
		}
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0003505F File Offset: 0x0003325F
	public static float GetTrendAnimFreq(int _TrendLevel)
	{
		switch (_TrendLevel)
		{
		case 1:
			return MBSingleton<GraphicsManager>.Instance.LowTrendAnimFreq;
		case 2:
			return MBSingleton<GraphicsManager>.Instance.MediumTrendAnimFreq;
		case 3:
			return MBSingleton<GraphicsManager>.Instance.HighTrendAnimFreq;
		default:
			return 0f;
		}
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x000350A0 File Offset: 0x000332A0
	public static float GetTrendAnimScale(int _TrendLevel)
	{
		switch (_TrendLevel)
		{
		case 1:
			return MBSingleton<GraphicsManager>.Instance.LowTrendAnim.AnimScale;
		case 2:
			return MBSingleton<GraphicsManager>.Instance.MediumTrendAnim.AnimScale;
		case 3:
			return MBSingleton<GraphicsManager>.Instance.HighTrendAnim.AnimScale;
		default:
			return 1f;
		}
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x000350F8 File Offset: 0x000332F8
	public static void FreeStatusGraphics(StatStatusGraphics _Graphics, bool _NoBar)
	{
		if (!MBSingleton<GraphicsManager>.Instance.AvailableStatusGraphics.Contains(_Graphics) && !_NoBar)
		{
			MBSingleton<GraphicsManager>.Instance.AvailableStatusGraphics.Add(_Graphics);
		}
		if (!MBSingleton<GraphicsManager>.Instance.AvailableNoBarStatusGraphics.Contains(_Graphics) && _NoBar)
		{
			MBSingleton<GraphicsManager>.Instance.AvailableNoBarStatusGraphics.Add(_Graphics);
		}
		GraphicsManager.RemoveStatusGraphics(_Graphics, _NoBar);
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00035158 File Offset: 0x00033358
	public static void RemoveStatusGraphics(StatStatusGraphics _Graphics, bool _NoBar)
	{
		if (MBSingleton<GraphicsManager>.Instance.CurrentStatusGraphics.Contains(_Graphics) && !_NoBar)
		{
			MBSingleton<GraphicsManager>.Instance.CurrentStatusGraphics.Remove(_Graphics);
		}
		if (MBSingleton<GraphicsManager>.Instance.CurrentNoBarStatusGraphics.Contains(_Graphics) && _NoBar)
		{
			MBSingleton<GraphicsManager>.Instance.CurrentNoBarStatusGraphics.Remove(_Graphics);
		}
		MBSingleton<GraphicsManager>.Instance.SortStatuses();
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x000351BC File Offset: 0x000333BC
	public float GetTimeSpentAnimDuration(int _Ticks, bool _Fade)
	{
		float num = (_Ticks <= 2) ? (this.TickRealTimeDuration * 2f) : (this.TickRealTimeDuration * (float)_Ticks);
		if (_Fade)
		{
			num += this.FadeToBlack.FadeDuration;
		}
		return num;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x000351F8 File Offset: 0x000333F8
	public void Init()
	{
		if (GameLoad.Instance)
		{
			GameLoad instance = GameLoad.Instance;
			instance.OnSaveFail = (Action)Delegate.Combine(instance.OnSaveFail, new Action(this.ShowSaveErrorPopup));
		}
		this.GM = MBSingleton<GameManager>.Instance;
		this.ExplorationDeckPopup.SetInstance();
		this.ExplorationDeckPopup.Init();
		this.CanOpenMenu = true;
		this.SlotPoolsParent = new GameObject("UnusedSlots", new Type[]
		{
			typeof(RectTransform)
		}).transform;
		this.SlotPoolsParent.transform.SetParent(this.ItemsParent.root);
		this.InventoryInspectionPopup.Init();
		this.BlueprintPopup.Init();
		this.EnvironmentSlot = new DynamicLayoutSlot(this.EnvSlotSettings, this.EnvSlotObject);
		this.WeatherSlot = new DynamicLayoutSlot(this.WeatherSlotSettings, this.WeatherSlotObject);
		if (this.ItemSlotsLine)
		{
			this.ItemSlotsLine.Init(this.ItemsParent, this.ItemSlotSettings, this.ItemCardsScrollView);
		}
		this.ItemSlotModel = new DynamicLayoutSlot(this.ItemSlotSettings, null);
		for (int i = 0; i < (this.LimitedItemSlots ? this.MaxItemSlots : this.MinItemSlots); i++)
		{
			this.AddSlot(SlotsTypes.Item, null, null, -2);
		}
		this.BaseSlotModel = new DynamicLayoutSlot(this.BaseSlotSettings, null);
		if (this.BaseSlotsLine)
		{
			this.BaseSlotsLine.Init(this.BaseParent, this.BaseSlotSettings, this.BaseCardsScrollView);
		}
		for (int j = 0; j < this.MinBaseSlots; j++)
		{
			this.AddSlot(SlotsTypes.Base, null, null, -2);
		}
		this.LocationSlotModel = new DynamicLayoutSlot(this.LocationSlotSettings, null);
		if (this.LocationSlotsLine)
		{
			this.LocationSlotsLine.Init(this.LocationParent, this.LocationSlotSettings, this.LocationCardsScrollView);
		}
		for (int k = 0; k < this.MinLocationSlots; k++)
		{
			this.AddSlot(SlotsTypes.Location, null, null, -2);
		}
		this.ExplorableSlotModel = new DynamicLayoutSlot(this.ExplorableSlotSettings, null);
		if (this.ExplorableSlotsLine)
		{
			this.ExplorableSlotsLine.Init(this.ExplorableParent, this.ExplorableSlotSettings, this.ExplorableCardsScrollView);
		}
		for (int l = 0; l < this.MinExplorableSlots; l++)
		{
			this.AddSlot(SlotsTypes.Explorable, null, null, -2);
		}
		this.BlueprintSlotModel = new DynamicLayoutSlot(this.BlueprintSlotSettings, null);
		if (this.BlueprintSlotsLine)
		{
			this.BlueprintSlotsLine.Init(this.BlueprintParent, this.BlueprintSlotSettings, this.BlueprintCardsScrollView);
		}
		for (int m = 0; m < this.MinBlueprintSlots; m++)
		{
			this.AddSlot(SlotsTypes.Blueprint, null, null, -2);
		}
		base.StartCoroutine(this.SetScrollView(Vector2.zero));
		this.RefreshSlots(true);
		this.CharacterWindow.Init();
		this.CurrentDaytimePoints = this.GM.CurrentTickInfo.z;
		this.CurrentMiniTicks = this.GM.CurrentMiniTicks;
		this.GameOver.Init();
		this.Victory.Init();
		this.EndGame.Init();
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00035528 File Offset: 0x00033728
	public void CollectVisibleStats()
	{
		this.VisibleStats = new List<InGameStat>();
		for (int i = 0; i < this.GM.AllStats.Count; i++)
		{
			if (this.GM.AllStats[i].StatModel && this.GM.AllStats[i].StatModel.Visibility != StatVisibilityOptions.NeverVisible && this.GM.AllStats[i].StatModel.CanBeVisible)
			{
				this.VisibleStats.Add(this.GM.AllStats[i]);
			}
		}
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x000355D4 File Offset: 0x000337D4
	public void LoadBookmarks(EnvironmentSaveData _ForEnv)
	{
		if (this.Bookmarks == null)
		{
			return;
		}
		if (_ForEnv != null)
		{
			if (_ForEnv.BookmarkedLiquidsIDs == null)
			{
				_ForEnv.BookmarkedLiquidsIDs = new string[this.Bookmarks.Length];
			}
			else if (_ForEnv.BookmarkedLiquidsIDs.Length != this.Bookmarks.Length)
			{
				_ForEnv.BookmarkedLiquidsIDs = new string[this.Bookmarks.Length];
			}
		}
		for (int i = 0; i < this.Bookmarks.Length; i++)
		{
			bool flag = false;
			CardData cardData = null;
			CardData withLiquid = null;
			if (_ForEnv != null)
			{
				flag = true;
			}
			if (flag)
			{
				flag &= (_ForEnv.BookmarkedCardsIDs != null);
			}
			if (flag)
			{
				flag &= (_ForEnv.BookmarkedCardsIDs.Length > i);
			}
			if (flag)
			{
				cardData = UniqueIDScriptable.GetFromID<CardData>(UniqueIDScriptable.LoadID(_ForEnv.BookmarkedCardsIDs[i]));
				withLiquid = UniqueIDScriptable.GetFromID<CardData>(UniqueIDScriptable.LoadID(_ForEnv.BookmarkedLiquidsIDs[i]));
				flag &= cardData;
			}
			if (flag)
			{
				this.Bookmarks[i].SetCard(cardData, withLiquid, false);
			}
			else
			{
				this.Bookmarks[i].SetCard(null, null, false);
			}
		}
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x000356CC File Offset: 0x000338CC
	public void SaveBookmarks()
	{
		if (this.Bookmarks == null)
		{
			return;
		}
		EnvironmentSaveData environmentSaveData = this.GM.CurrentEnvData(true);
		string[] bookmarkedCards = this.BookmarkedCards;
		string[] bookmarkedLiquidCards = this.BookmarkedLiquidCards;
		if (this.BookmarkedCards != null)
		{
			environmentSaveData.BookmarkedCardsIDs = new string[bookmarkedCards.Length];
			bookmarkedCards.CopyTo(environmentSaveData.BookmarkedCardsIDs, 0);
			environmentSaveData.BookmarkedLiquidsIDs = new string[bookmarkedLiquidCards.Length];
			bookmarkedLiquidCards.CopyTo(environmentSaveData.BookmarkedLiquidsIDs, 0);
			return;
		}
		environmentSaveData.BookmarkedCardsIDs = new string[0];
		environmentSaveData.BookmarkedLiquidsIDs = new string[0];
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000542 RID: 1346 RVA: 0x00035754 File Offset: 0x00033954
	public string[] BookmarkedCards
	{
		get
		{
			if (this.Bookmarks == null)
			{
				return null;
			}
			string[] array = new string[this.Bookmarks.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (this.Bookmarks[i])
				{
					array[i] = UniqueIDScriptable.SaveID(this.Bookmarks[i].MarkedCard);
				}
			}
			return array;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000543 RID: 1347 RVA: 0x000357AC File Offset: 0x000339AC
	public string[] BookmarkedLiquidCards
	{
		get
		{
			if (this.Bookmarks == null)
			{
				return null;
			}
			string[] array = new string[this.Bookmarks.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (this.Bookmarks[i])
				{
					array[i] = UniqueIDScriptable.SaveID(this.Bookmarks[i].MarkedLiquid);
				}
			}
			return array;
		}
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x00035804 File Offset: 0x00033A04
	private IEnumerator SetScrollView(Vector2 _Pos)
	{
		if (!this.ItemCardsScrollView)
		{
			yield break;
		}
		yield return null;
		this.ItemCardsScrollView.normalizedPosition = _Pos;
		yield break;
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x0003581C File Offset: 0x00033A1C
	public int GetBookmarkIndex(CardData _Card, CardData _WithLiquid)
	{
		if (this.Bookmarks == null || !_Card)
		{
			return -1;
		}
		if (this.Bookmarks.Length == 0)
		{
			return -1;
		}
		for (int i = 0; i < this.Bookmarks.Length; i++)
		{
			if (this.Bookmarks[i] && this.Bookmarks[i].HasCardMarked(_Card, _WithLiquid))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0003587C File Offset: 0x00033A7C
	public BookmarkGraphics GetBookmark(CardData _Card, CardData _WithLiquid)
	{
		int bookmarkIndex = this.GetBookmarkIndex(_Card, _WithLiquid);
		if (bookmarkIndex == -1)
		{
			return null;
		}
		return this.Bookmarks[bookmarkIndex];
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x000358A0 File Offset: 0x00033AA0
	private void Update()
	{
		if (this.ResearchingBlueprintIcon && this.GM)
		{
			if (!this.GM.BlueprintPurchasing)
			{
				this.ResearchingBlueprintIcon.SetActive(false);
				return;
			}
			if (!this.GM.PurchasingWithTime)
			{
				this.ResearchingBlueprintIcon.SetActive(false);
				return;
			}
			this.ResearchingBlueprintIcon.SetActive(this.BlueprintModelsPopup.CurrentResearch);
		}
		if (this.VisibleStats != null)
		{
			for (int i = 0; i < this.VisibleStats.Count; i++)
			{
				this.VisibleStats[i].UpdateAnimatedValue();
			}
		}
		bool terminalEffect = false;
		if (this.CurrentStatusGraphics != null && this.CurrentStatusGraphics.Count > 0)
		{
			for (int j = 0; j < this.CurrentStatusGraphics.Count; j++)
			{
				if (GraphicsManager.IsStatusCritical(this.CurrentStatusGraphics[j].ModelStatus.AlertLevel))
				{
					terminalEffect = true;
					break;
				}
			}
		}
		MBSingleton<SpecialEffectsManager>.Instance.TerminalEffect = terminalEffect;
		if (this.Bookmarks != null)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1) && this.Bookmarks.Length != 0)
			{
				BookmarkGraphics bookmarkGraphics = this.Bookmarks[0];
				if (bookmarkGraphics != null)
				{
					bookmarkGraphics.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha2) && this.Bookmarks.Length > 1)
			{
				BookmarkGraphics bookmarkGraphics2 = this.Bookmarks[1];
				if (bookmarkGraphics2 != null)
				{
					bookmarkGraphics2.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha3) && this.Bookmarks.Length > 2)
			{
				BookmarkGraphics bookmarkGraphics3 = this.Bookmarks[2];
				if (bookmarkGraphics3 != null)
				{
					bookmarkGraphics3.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha4) && this.Bookmarks.Length > 3)
			{
				BookmarkGraphics bookmarkGraphics4 = this.Bookmarks[3];
				if (bookmarkGraphics4 != null)
				{
					bookmarkGraphics4.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha5) && this.Bookmarks.Length > 4)
			{
				BookmarkGraphics bookmarkGraphics5 = this.Bookmarks[4];
				if (bookmarkGraphics5 != null)
				{
					bookmarkGraphics5.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha6) && this.Bookmarks.Length > 5)
			{
				BookmarkGraphics bookmarkGraphics6 = this.Bookmarks[5];
				if (bookmarkGraphics6 != null)
				{
					bookmarkGraphics6.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha7) && this.Bookmarks.Length > 6)
			{
				BookmarkGraphics bookmarkGraphics7 = this.Bookmarks[6];
				if (bookmarkGraphics7 != null)
				{
					bookmarkGraphics7.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha8) && this.Bookmarks.Length > 7)
			{
				BookmarkGraphics bookmarkGraphics8 = this.Bookmarks[7];
				if (bookmarkGraphics8 != null)
				{
					bookmarkGraphics8.FindBookmark();
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha9) && this.Bookmarks.Length > 8)
			{
				BookmarkGraphics bookmarkGraphics9 = this.Bookmarks[8];
				if (bookmarkGraphics9 != null)
				{
					bookmarkGraphics9.FindBookmark();
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.EscKeyPressed();
		}
		this.Hotkeys();
		if (this.DayTimePoints && this.GM && (this.PrevDaytimePoints != this.CurrentDaytimePoints || this.PrevMiniTicks != this.CurrentMiniTicks))
		{
			this.DayTimePoints.text = GameManager.TotalTicksToHourOfTheDayString(GameManager.HoursToTick((float)this.GM.DaySettings.DayStartingHour) + this.CurrentDaytimePoints, this.CurrentMiniTicks);
			this.PrevDaytimePoints = this.CurrentDaytimePoints;
		}
		if (this.GM && this.EventResolved && this.GM.CurrentEventCard == null)
		{
			this.EventResolved = false;
		}
		this.FreqTime += Time.deltaTime;
		if (this.FreqTime >= 1000f)
		{
			this.FreqTime -= 1000f;
		}
		this.TerminalAlertPulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.CriticalAlertSettings.PulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.TerminalAlertOutlinePulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.CriticalAlertSettings.OutlinePulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.TerminalAlertOutlineBlinkFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.CriticalAlertSettings.OutlineBlinkingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.CriticalAlertPulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.CriticalAlertSettings.PulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.CriticalAlertOutlinePulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.CriticalAlertSettings.OutlinePulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.CriticalAlertOutlineBlinkFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.CriticalAlertSettings.OutlineBlinkingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.HighAlertPulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.HighAlertSettings.PulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.HighAlertOutlinePulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.HighAlertSettings.OutlinePulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.HighAlertOutlineBlinkFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.HighAlertSettings.OutlineBlinkingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.ModerateAlertPulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.ModerateAlertSettings.PulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.ModerateAlertOutlinePulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.ModerateAlertSettings.OutlinePulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.ModerateAlertOutlineBlinkFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.ModerateAlertSettings.OutlineBlinkingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.LowAlertPulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.LowAlertSettings.PulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.LowAlertOutlinePulseFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.LowAlertSettings.OutlinePulsingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.LowAlertOutlineBlinkFreq = this.PulseCurve.Evaluate(Mathf.Sin(6.2831855f * this.LowAlertSettings.OutlineBlinkingFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.LowTrendAnimFreq = this.TrendAnimCurve.Evaluate(Mathf.Sin(6.2831855f * this.LowTrendAnim.AnimFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.MediumTrendAnimFreq = this.TrendAnimCurve.Evaluate(Mathf.Sin(6.2831855f * this.MediumTrendAnim.AnimFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
		this.HighTrendAnimFreq = this.TrendAnimCurve.Evaluate(Mathf.Sin(6.2831855f * this.HighTrendAnim.AnimFrequency * this.FreqTime - 1.5707964f) * 0.5f + 0.5f);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00036074 File Offset: 0x00034274
	private void EscKeyPressed()
	{
		if (this.StatInspection.gameObject.activeInHierarchy)
		{
			this.StatInspection.gameObject.SetActive(false);
			return;
		}
		if (this.AllStatsList.gameObject.activeInHierarchy)
		{
			this.AllStatsList.gameObject.SetActive(false);
			return;
		}
		if (this.GM.GuideIsOpen)
		{
			this.GM.CloseGuide();
			return;
		}
		if (this.GM.JournalIsOpen)
		{
			this.GM.CloseJournal();
			return;
		}
		if (this.BlueprintModelsPopup.gameObject.activeInHierarchy)
		{
			this.BlueprintModelsPopup.Hide();
			return;
		}
		if (this.CharacterWindow.gameObject.activeInHierarchy)
		{
			this.CharacterWindow.Close();
			return;
		}
		if (this.ExplorationDeckPopup.gameObject.activeInHierarchy && !this.CannotCloseCurrentPopup)
		{
			this.ExplorationDeckPopup.Hide(true);
			return;
		}
		if (this.CurrentInspectionPopup && !this.CannotCloseCurrentPopup)
		{
			this.CloseAllPopups();
			return;
		}
		if (this.CanOpenMenu && this.MenuObject)
		{
			this.MenuObject.SetActive(!this.MenuObject.activeInHierarchy);
		}
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x000361A8 File Offset: 0x000343A8
	private void Hotkeys()
	{
		if (GraphicsManager.PlayerIsTyping)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this.LocationSlotsLine.MoveToPrevPos();
			return;
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			this.LocationSlotsLine.MoveToNextPos();
			return;
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (this.CurrentInspectionPopup)
			{
				this.InventoryInspectionPopup.InventorySlotsLine.MoveToPrevPos();
				return;
			}
			this.BaseSlotsLine.MoveToPrevPos();
			return;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			if (this.CurrentInspectionPopup)
			{
				this.InventoryInspectionPopup.InventorySlotsLine.MoveToNextPos();
				return;
			}
			this.BaseSlotsLine.MoveToNextPos();
			return;
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.B) && !this.CannotCloseCurrentPopup)
			{
				this.BlueprintModelsPopup.Toggle();
				return;
			}
			if (Input.GetKeyDown(KeyCode.J) && !this.CannotCloseCurrentPopup)
			{
				this.GM.OpenJournal();
				return;
			}
			if (Input.GetKeyDown(KeyCode.E) && !this.CannotCloseCurrentPopup)
			{
				if (this.CharacterWindow.CurrentTab == 0)
				{
					this.CharacterWindow.Toggle();
				}
				else
				{
					this.CharacterWindow.Open();
				}
				this.CharacterWindow.ChangeTab(0);
				return;
			}
			if (Input.GetKeyDown(KeyCode.H) && !this.CannotCloseCurrentPopup)
			{
				if (this.CharacterWindow.CurrentTab == 1)
				{
					this.CharacterWindow.Toggle();
				}
				else
				{
					this.CharacterWindow.Open();
				}
				this.CharacterWindow.ChangeTab(1);
				return;
			}
			if (Input.GetKeyDown(KeyCode.C) && !this.CannotCloseCurrentPopup)
			{
				if (this.CharacterWindow.CurrentTab == 2)
				{
					this.CharacterWindow.Toggle();
				}
				else
				{
					this.CharacterWindow.Open();
				}
				this.CharacterWindow.ChangeTab(2);
				return;
			}
			if (Input.GetKeyDown(KeyCode.T) && !this.CannotCloseCurrentPopup)
			{
				this.CloseAllPopups();
				this.CurrentInspectionPopup = this.CardInspectionPopup;
				this.CardInspectionPopup.Setup(this.GM.TimeOptions);
				return;
			}
			if (!Input.GetKeyDown(KeyCode.D))
			{
				return;
			}
			if (this.AllStatsList.gameObject.activeInHierarchy)
			{
				this.AllStatsList.gameObject.SetActive(false);
				return;
			}
			this.AllStatsList.Show();
			return;
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x000363C8 File Offset: 0x000345C8
	private void LateUpdate()
	{
		if (this.DoSlotsRefresh)
		{
			this.RefreshSlots();
		}
		if (this.UnlockedPerksQueue.Count > 0 && this.PerkUnlocks != null)
		{
			if (!this.PerkUnlocks.gameObject.activeInHierarchy)
			{
				this.PerkUnlocks.Show(this.UnlockedPerksQueue[0]);
				this.UnlockedPerksQueue.RemoveAt(0);
			}
			return;
		}
		if (this.ObjectivesCompleteFeedbacks.Count > 0)
		{
			if (this.ObjectivesCompleteFeedbacks[0].IsFinished)
			{
				this.ObjectivesCompleteFeedbacks.RemoveAt(0);
				this.ObjectiveCompletedFeedbackIsPlaying = false;
			}
			if (!this.SomeFeedbackIsPlaying && this.ObjectivesCompleteFeedbacks.Count > 0)
			{
				this.ObjectivesCompleteFeedbacks[0].Play(this.MajorObjectiveCompletePrefab, this.MajorObjectiveNotificationParent, this.JournalButtonTr);
				this.ObjectiveCompletedFeedbackIsPlaying = true;
			}
		}
		if (this.WoundsReceivedFeedbacks.Count > 0)
		{
			if (this.WoundsReceivedFeedbacks[0].IsFinished)
			{
				this.WoundsReceivedFeedbacks.RemoveAt(0);
				this.WoundReceivedFeedbackIsPlaying = false;
			}
			if (!this.SomeFeedbackIsPlaying && this.WoundsReceivedFeedbacks.Count > 0)
			{
				this.WoundsReceivedFeedbacks[0].Play(this.WoundReceivedPrefab, this.BlueprintNotificationParent, this.CharacterButtonTr);
				this.WoundReceivedFeedbackIsPlaying = true;
			}
		}
		if (this.BlueprintsUnlockedFeedbacks.Count > 0)
		{
			if (this.BlueprintsUnlockedFeedbacks[0].IsFinished)
			{
				this.BlueprintsUnlockedFeedbacks.RemoveAt(0);
				this.BlueprintFeedbackIsPlaying = false;
			}
			if (!this.SomeFeedbackIsPlaying && this.BlueprintsUnlockedFeedbacks.Count > 0)
			{
				if (this.BlueprintsUnlockedFeedbacks[0].AssociatedBlueprint.CardType == CardTypes.EnvImprovement && this.GM.CurrentExplorableCard)
				{
					this.BlueprintsUnlockedFeedbacks[0].Play(this.BlueprintUnlockedPrefab, this.BlueprintNotificationParent, this.GM.CurrentExplorableCard.transform);
				}
				else if (this.BlueprintsUnlockedFeedbacks[0].AssociatedBlueprint.CardType == CardTypes.EnvDamage && this.GM.CurrentExplorableCard)
				{
					this.BlueprintsUnlockedFeedbacks[0].Play(this.DamageSpawnedPrefab, this.BlueprintNotificationParent, this.GM.CurrentExplorableCard.transform);
				}
				else if (this.BlueprintsUnlockedFeedbacks[0].AssociatedBlueprint.CardType == CardTypes.Blueprint)
				{
					this.BlueprintsUnlockedFeedbacks[0].Play(this.BlueprintUnlockedPrefab, this.BlueprintNotificationParent, this.BlueprintsButtonTr);
				}
				this.BlueprintFeedbackIsPlaying = true;
			}
		}
		if (this.ObjectivesCompleteFeedbacks.Count == 0)
		{
			this.ObjectiveCompletedFeedbackIsPlaying = false;
		}
		if (this.WoundsReceivedFeedbacks.Count == 0)
		{
			this.WoundReceivedFeedbackIsPlaying = false;
		}
		if (this.BlueprintsUnlockedFeedbacks.Count == 0)
		{
			this.BlueprintFeedbackIsPlaying = false;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600054B RID: 1355 RVA: 0x000366A8 File Offset: 0x000348A8
	public bool HasPopup
	{
		get
		{
			return this.GM.HasContentOpen || this.InventoryInspectionPopup.gameObject.activeInHierarchy || this.CardInspectionPopup.gameObject.activeInHierarchy || this.ExplorationDeckPopup.gameObject.activeInHierarchy || this.BlueprintPopup.gameObject.activeInHierarchy || this.CharacterWindow.gameObject.activeInHierarchy || this.BlueprintModelsPopup.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x0600054C RID: 1356 RVA: 0x00036730 File Offset: 0x00034930
	public bool CannotCloseCurrentPopup
	{
		get
		{
			return (this.CurrentInspectionPopup != null && this.CurrentInspectionPopup.CannotCloseWindow) || (this.InspectedCard != null && (this.InspectedCard.CardModel.CardType == CardTypes.Event || (this.InspectedCard.CardModel.CardType == CardTypes.Explorable && !MBSingleton<ExplorationPopup>.Instance.CanHide)));
		}
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x000367A0 File Offset: 0x000349A0
	public void OpenEnvImprovements(InGameCardBase _OnImprovement)
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (!this.GM.CurrentExplorableCard)
		{
			return;
		}
		if (!this.GM.CurrentExplorableCard.CardModel)
		{
			return;
		}
		if (!this.GM.CurrentExplorableCard.CardModel.HasImprovements)
		{
			return;
		}
		base.StartCoroutine(this.ShowImprovementsDelay(_OnImprovement));
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00036816 File Offset: 0x00034A16
	private IEnumerator ShowImprovementsDelay(InGameCardBase _OnImprovement)
	{
		while (GameManager.PerformingAction)
		{
			yield return null;
		}
		this.InspectCard(this.GM.CurrentExplorableCard, false);
		this.ExplorationDeckPopup.SelectTab(2);
		if (_OnImprovement)
		{
			_OnImprovement.Pulse(0f);
		}
		yield break;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0003682C File Offset: 0x00034A2C
	public void InspectStat(InGameStat _Stat)
	{
		this.StatInspection.Setup(_Stat);
		this.StatInspection.gameObject.SetActive(true);
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0003684C File Offset: 0x00034A4C
	public void InspectCard(InGameCardBase _Card, bool _ForceInventory = false)
	{
		if (this.CannotCloseCurrentPopup)
		{
			return;
		}
		if (GameManager.PerformingAction && _Card.CardModel.CardType != CardTypes.Event)
		{
			Debug.Log(LocalizedString.ActionHappening.ToString());
			this.ShowImpossibleToInspect(_Card, LocalizedString.ActionHappening);
			return;
		}
		if (_Card.CurrentContainer != null)
		{
			if (this.CannotInspectTextPrefab)
			{
				base.StartCoroutine(UnityEngine.Object.Instantiate<UIFeedbackText>(this.CannotInspectTextPrefab, this.TextsParent).PlayFeedback(_Card.transform.position, LocalizedString.CannotInspect));
			}
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Blueprint && _Card.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint)
		{
			if (this.GM.BlueprintPurchasing && this.GM.BlueprintModelStates[_Card.CardModel] == BlueprintModelState.Purchasable)
			{
				if (!this.BlueprintModelsPopup.CanAffordBlueprint(_Card.CardModel))
				{
					this.ShowImpossibleToInspect(_Card, LocalizedString.CannotAfford(new LocalizedString
					{
						LocalizationKey = "SUNS",
						DefaultText = "Suns"
					}));
				}
				return;
			}
			bool isNotCancelledByDemo = _Card.DismantleActions[0].IsNotCancelledByDemo;
			bool flag = _Card.DismantleActions[0].StatsAreCorrect(null, true);
			bool flag2 = _Card.DismantleActions[0].CardsAndTagsAreCorrect(_Card, null, null);
			bool flag3 = this.GM.MaxEnvWeight <= 0f || this.GM.CurrentEnvWeight + _Card.CurrentWeight <= this.GM.MaxEnvWeight;
			bool flag4 = _Card.CardModel.CanSpawnOnBoard();
			if (!isNotCancelledByDemo)
			{
				this.ShowImpossibleToInspect(_Card, LocalizedString.UnavailableInDemo);
			}
			if (!flag || !flag2 || !flag4)
			{
				this.ShowImpossibleToInspect(_Card, LocalizedString.CannotMakeBlueprint(_Card.CardModel));
				return;
			}
			if (!flag3)
			{
				this.ShowImpossibleToInspect(_Card, LocalizedString.InventoryCannotCarry(this.GM.CurrentEnvironmentCard));
				return;
			}
		}
		if (_Card == this.InspectedCard && !_ForceInventory)
		{
			this.CloseAllPopups();
			return;
		}
		this.CloseAllPopups();
		this.InspectedCard = _Card;
		if (_Card.CardModel.CardType == CardTypes.Explorable)
		{
			this.ExplorationDeckPopup.Setup(_Card);
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Blueprint || _Card.CardModel.CardType == CardTypes.EnvDamage || (_Card.CardModel.CardType == CardTypes.EnvImprovement && !_Card.BlueprintComplete))
		{
			if (!_Card.CurrentSlot)
			{
				this.CurrentInspectionPopup = this.BlueprintPopup;
				this.BlueprintPopup.Setup(_Card);
				return;
			}
			if (_Card.CurrentSlot.SlotType == SlotsTypes.Blueprint)
			{
				GameManager.PerformAction(_Card.DismantleActions[0], _Card, false);
				return;
			}
			this.CurrentInspectionPopup = this.BlueprintPopup;
			this.BlueprintPopup.Setup(_Card);
			return;
		}
		else
		{
			if (_Card.CardsInInventory.Count > 0 && (_ForceInventory || !_Card.CardModel.InventoryIsHidden) && _Card.CardModel.CardType != CardTypes.Explorable)
			{
				this.CurrentInspectionPopup = this.InventoryInspectionPopup;
				this.InventoryInspectionPopup.Setup(_Card);
				return;
			}
			this.CurrentInspectionPopup = this.CardInspectionPopup;
			this.CardInspectionPopup.Setup(_Card);
			return;
		}
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00036B6C File Offset: 0x00034D6C
	public void CloseAllPopups()
	{
		this.GM.CloseGuide();
		this.GM.CloseJournal();
		this.BlueprintModelsPopup.Hide();
		this.AllStatsList.gameObject.SetActive(false);
		this.StatInspection.gameObject.SetActive(false);
		if (this.CannotCloseCurrentPopup)
		{
			return;
		}
		this.InventoryInspectionPopup.Hide(true);
		this.CardInspectionPopup.Hide(true);
		this.ExplorationDeckPopup.Hide(true);
		this.BlueprintPopup.Hide(true);
		this.CharacterWindow.Close();
		this.CurrentInspectionPopup = null;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00036C07 File Offset: 0x00034E07
	public void ShowSaveErrorPopup()
	{
		if (this.SaveErrorPopup)
		{
			this.SaveErrorPopup.SetActive(true);
		}
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x00036C22 File Offset: 0x00034E22
	public void ShowImpossibleToInspect(InGameCardBase _Card, string _Content)
	{
		if (this.CannotInspectTextPrefab)
		{
			base.StartCoroutine(UnityEngine.Object.Instantiate<UIFeedbackText>(this.CannotInspectTextPrefab, this.TextsParent).PlayFeedback(_Card.transform.position, _Content));
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x00036C5A File Offset: 0x00034E5A
	public void ClearInspectedCard()
	{
		if (this.InspectedCard == this.GM.CurrentEventCard)
		{
			this.EventResolved = true;
		}
		this.InspectedCard = null;
		this.CurrentInspectionPopup = null;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x00036C8C File Offset: 0x00034E8C
	public void SwapStack(DynamicLayoutSlot _Slot)
	{
		if (!_Slot)
		{
			return;
		}
		List<InGameCardBase> cardPile = _Slot.GetCardPile(false, false, false);
		if (cardPile == null)
		{
			return;
		}
		if (cardPile.Count == 0)
		{
			return;
		}
		for (int i = 0; i < cardPile.Count; i++)
		{
			cardPile[i].SwapCard();
		}
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00036CD6 File Offset: 0x00034ED6
	private IEnumerator SwapStackRoutine(DynamicLayoutSlot _Slot)
	{
		List<InGameCardBase> allCards = _Slot.GetCardPile(false, false, false);
		if (allCards == null)
		{
			yield break;
		}
		if (allCards.Count == 0)
		{
			yield break;
		}
		int num;
		for (int i = 0; i < allCards.Count; i = num + 1)
		{
			allCards[i].SwapCard();
			yield return new WaitForSeconds(0.1f);
			num = i;
		}
		yield break;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00036CE8 File Offset: 0x00034EE8
	public void MoveCardToSlot(InGameCardBase _Card, SlotInfo _ToSlot, bool _Dragged, bool _InterPlacement)
	{
		bool flag = false;
		bool flag2 = false;
		if (_ToSlot.SlotType == SlotsTypes.Equipment)
		{
			string text = this.CharacterWindow.ReasonForNotEquipping(_Card.CardModel, _Card);
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowImpossibleToInspect(_Card, text);
				return;
			}
			this.CharacterWindow.ChangeTab(0);
		}
		if (_Dragged && _Card.CurrentSlot)
		{
			if (_ToSlot.SlotType == SlotsTypes.Item)
			{
				if (_Card.CurrentSlotInfo.SlotType == SlotsTypes.Item)
				{
					if (_Card.CurrentSlotInfo.SlotIndex < _ToSlot.SlotIndex)
					{
						this.ItemSlotsLine.MoveSlot(_Card.CurrentSlotInfo.SlotIndex, _ToSlot.SlotIndex);
					}
					else
					{
						this.ItemSlotsLine.MoveSlot(_Card.CurrentSlotInfo.SlotIndex, _ToSlot.SlotIndex + 1);
					}
					flag2 = true;
				}
				else if (this.GetItemsListFreeIndex(_ToSlot.SlotIndex, _Card.CardModel, _Card.ContainedLiquidModel, _Card.CurrentWeight) == -1)
				{
					if (this.DontSwapCards)
					{
						this.ShowImpossibleToInspect(_Card, LocalizedString.PlayerCannotCarry);
						return;
					}
					if (_ToSlot.SlotIndex >= this.ItemSlotsLine.Slots.Count)
					{
						_ToSlot.SlotIndex = 0;
					}
					if (_Card.CurrentSlotInfo.SlotType != SlotsTypes.Item)
					{
						int num = Mathf.Clamp(_ToSlot.SlotIndex, 0, this.ItemSlotsLine.Slots.Count - 1);
						InGameCardBase assignedCard = this.ItemSlotsLine.Slots[num].AssignedCard;
						SlotsTypes slotType = _Card.CurrentSlotInfo.SlotType;
						if (slotType != SlotsTypes.Base && slotType - SlotsTypes.Equipment > 1)
						{
							return;
						}
						while (!this.CanFindSlot(assignedCard, new SlotInfo(_Card.CurrentSlotInfo.SlotType, 0), _Card))
						{
							num++;
							if (num >= this.ItemSlotsLine.Slots.Count)
							{
								num = 0;
							}
							assignedCard = this.ItemSlotsLine.Slots[num].AssignedCard;
							if (num == _ToSlot.SlotIndex)
							{
								return;
							}
						}
						_ToSlot.SlotIndex = num;
						this.ItemSlotsLine.Slots[num].AssignCard(null, false);
						if (_Card.CurrentSlotInfo.SlotType == SlotsTypes.Inventory)
						{
							_Card.CurrentSlot.AssignCard(null, false);
							_Card.CurrentContainer.RemoveCardFromInventory(_Card);
						}
						if (_Card.CurrentSlotInfo.SlotType == SlotsTypes.Base)
						{
							this.GetSlotForCard(assignedCard.CardModel, assignedCard.ContainedLiquidModel, _Card.CurrentSlotInfo, null, null, 0).AssignCard(assignedCard, false);
						}
						else
						{
							this.AddSlot(_Card.CurrentSlotInfo, assignedCard.CardModel, assignedCard.ContainedLiquidModel).AssignCard(assignedCard, false);
						}
					}
					this.RefreshSlots(true);
				}
			}
			else if (_ToSlot.SlotType == SlotsTypes.Inventory)
			{
				if (!this.CanFindSlot(_Card, _ToSlot, null))
				{
					if (this.InspectedCard)
					{
						float num2 = this.InspectedCard.InventoryWeight(true);
						if (_Card.CurrentWeight + num2 > this.InspectedCard.MaxWeightCapacity && this.InspectedCard.CardModel.CardType != CardTypes.Blueprint && this.InspectedCard.CardModel.CardType != CardTypes.EnvImprovement && this.InspectedCard.CardModel.CardType != CardTypes.EnvDamage)
						{
							this.ShowImpossibleToInspect(_Card, LocalizedString.InventoryCannotCarry(this.InspectedCard));
							return;
						}
						if (this.GM.MaxEnvWeight > 0f && this.InspectedCard.CardModel.CardType == CardTypes.Blueprint)
						{
							float num3 = this.GM.CurrentEnvWeight - this.InspectedCard.CurrentWeight;
							float num4 = this.InspectedCard.CardModel.ObjectWeight + Mathf.Max(0f, num2 + _Card.CurrentWeight + this.InspectedCard.CardModel.ContentWeightReduction);
							if (num3 + num4 > this.GM.MaxEnvWeight)
							{
								this.ShowImpossibleToInspect(_Card, LocalizedString.InventoryCannotCarry(this.GM.CurrentEnvironmentCard));
							}
						}
					}
					return;
				}
				if (_Card.CurrentSlotInfo.SlotType == SlotsTypes.Inventory)
				{
					int num5 = _ToSlot.SlotIndex;
					if (this.InspectedCard)
					{
						num5 = Mathf.Clamp(num5, 0, this.InspectedCard.CardsInInventory.Count - 1);
					}
					if (_Card.CurrentSlotInfo.SlotIndex < num5)
					{
						this.CurrentInspectionPopup.MoveSlot(_Card.CurrentSlotInfo.SlotIndex, num5);
					}
					else if (num5 != _Card.CurrentSlotInfo.SlotIndex)
					{
						this.CurrentInspectionPopup.MoveSlot(_Card.CurrentSlotInfo.SlotIndex, num5 + 1);
					}
					flag = true;
				}
				else if (this.GetInventoryListFreeIndex(_ToSlot.SlotIndex, _Card.CardModel, _Card.ContainedLiquidModel, _Card.CurrentWeight) == -1)
				{
					if (_ToSlot.SlotIndex >= this.ItemSlotsLine.Slots.Count)
					{
						_ToSlot.SlotIndex = 0;
					}
					int num6 = Mathf.Clamp(_ToSlot.SlotIndex, 0, this.CurrentInspectionPopup.SlotCount - 1);
					InGameCardBase assignedCard2 = this.CurrentInspectionPopup.GetSlot(num6).AssignedCard;
					while (!this.CanFindSlot(assignedCard2, new SlotInfo(_Card.CurrentSlotInfo.SlotType, 0), _Card))
					{
						num6++;
						if (num6 >= this.CurrentInspectionPopup.SlotCount)
						{
							num6 = 0;
						}
						assignedCard2 = this.CurrentInspectionPopup.GetSlot(num6).AssignedCard;
						if (num6 == _ToSlot.SlotIndex)
						{
							return;
						}
					}
					_ToSlot.SlotIndex = num6;
					this.CurrentInspectionPopup.GetSlot(num6).AssignCard(null, false);
					assignedCard2.CurrentContainer.RemoveCardFromInventory(assignedCard2);
					_Card.CurrentSlot.AssignCard(null, false);
					this.AddSlot(_Card.CurrentSlotInfo, assignedCard2.CardModel, assignedCard2.ContainedLiquidModel).AssignCard(assignedCard2, false);
					this.RefreshSlots(true);
				}
			}
			else if (_ToSlot.SlotType == SlotsTypes.Equipment)
			{
				if (_Card.CurrentSlot && _Card.CurrentSlot.SlotType == SlotsTypes.Equipment)
				{
					_Card.CurrentSlot.RemoveSpecificCard(_Card, false, true);
				}
			}
			else if (_ToSlot.SlotType == SlotsTypes.Base && this.GM.MaxEnvWeight > 0f && this.GM.CurrentEnvWeight + _Card.CurrentWeight > this.GM.MaxEnvWeight && _Card.CurrentSlotInfo.SlotType != SlotsTypes.Base)
			{
				this.ShowImpossibleToInspect(_Card, LocalizedString.InventoryCannotCarry(this.GM.CurrentEnvironmentCard));
				return;
			}
		}
		if (!flag && !flag2)
		{
			this.AddSlot(_ToSlot, _Card.CardModel, _Card.ContainedLiquidModel).AssignCard(_Card, false);
		}
		this.RefreshSlots(true);
		this.MoveViewToSlot(_Card.CurrentSlot, false, false);
		MBSingleton<SoundManager>.Instance.PerformCardAppearanceSound(_Card.CardModel.WhenCreatedSounds);
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0003735C File Offset: 0x0003555C
	public bool CanFindSlot(InGameCardBase _Card, SlotInfo _Slot, InGameCardBase _SwappedCard)
	{
		if (!_Card)
		{
			return false;
		}
		if (_Slot.SlotType != SlotsTypes.Inventory)
		{
			switch (_Slot.SlotType)
			{
			case SlotsTypes.Item:
				for (int i = 0; i < this.ItemSlotsLine.Slots.Count; i++)
				{
					if (this.ItemSlotsLine.Slots[i].CanReceiveCard(_Card, _SwappedCard != null && this.ItemSlotsLine.Slots[i].AssignedCard))
					{
						return true;
					}
					if (this.ItemSlotsLine.Slots[i].AssignedCard == _Card)
					{
						return true;
					}
				}
				if (_Card.CurrentSlot && _SwappedCard == null && _Card.CurrentSlot.SlotType != SlotsTypes.Item)
				{
					for (int j = 0; j < this.ItemSlotsLine.Slots.Count; j++)
					{
						if (this.ItemSlotsLine.Slots[j].CanReceiveCard(_Card, true) && this.CanFindSlot(this.ItemSlotsLine.Slots[j].AssignedCard, new SlotInfo(_Card.CurrentSlot.SlotType, 0), _Card))
						{
							return true;
						}
					}
				}
				return false;
			case SlotsTypes.Base:
				return this.BaseSlotModel.CanReceiveCard(_Card, false);
			case SlotsTypes.Location:
				return this.LocationSlotModel.CanReceiveCard(_Card, false);
			case SlotsTypes.Event:
				return _Card.CardModel.CardType == CardTypes.Event;
			case SlotsTypes.Environment:
				return this.EnvironmentSlot.CanReceiveCard(_Card, false);
			case SlotsTypes.Hand:
				return false;
			case SlotsTypes.Blueprint:
				return this.BlueprintSlotModel.CanReceiveCard(_Card, false);
			case SlotsTypes.Equipment:
				return string.IsNullOrEmpty(this.CharacterWindow.ReasonForNotEquipping(_Card.CardModel, _Card));
			case SlotsTypes.Exploration:
				return this.ExplorationDeckPopup.SlotModel.CanReceiveCard(_Card, false);
			case SlotsTypes.Explorable:
				return this.ExplorableSlotModel.CanReceiveCard(_Card, false);
			}
			return true;
		}
		if (this.InspectedCard == _Card)
		{
			return false;
		}
		if (!this.InspectedCard.CanReceiveInInventoryInstance(_Card))
		{
			return false;
		}
		if (!this.CurrentInspectionPopup)
		{
			return false;
		}
		for (int k = 0; k < this.CurrentInspectionPopup.SlotCount; k++)
		{
			if (this.CurrentInspectionPopup.GetSlot(k).ContainsCard(_Card))
			{
				return true;
			}
			if (this.CurrentInspectionPopup.GetSlot(k).CanReceiveCard(_Card, _SwappedCard != null && _SwappedCard == this.CurrentInspectionPopup.GetSlot(k).AssignedCard))
			{
				if (!this.CurrentInspectionPopup.GetSlot(k).CanHostPile)
				{
					return true;
				}
				if (this.CurrentInspectionPopup.GetSlot(k).AssignedCard == null)
				{
					return true;
				}
				if (this.CurrentInspectionPopup.GetSlot(k).AssignedCard.CardModel == _Card.CardModel && this.CurrentInspectionPopup.GetSlot(k).AssignedCard.ContainedLiquidModel == _Card.ContainedLiquidModel)
				{
					return true;
				}
			}
		}
		if (_Card.CurrentSlot && _SwappedCard == null && !this.DontSwapCards && _Card.CurrentSlot.SlotType == SlotsTypes.Item)
		{
			for (int l = 0; l < this.CurrentInspectionPopup.SlotCount; l++)
			{
				if (this.CanFindSlot(this.CurrentInspectionPopup.GetSlot(l).AssignedCard, new SlotInfo(SlotsTypes.Item, 0), _Card))
				{
					return true;
				}
			}
		}
		Debug.LogWarning("Cannot swap anything for " + _Card);
		return false;
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x000376E0 File Offset: 0x000358E0
	public SlotsTypes CardToSlotType(CardTypes _CardType, bool _FailedToMatch = false)
	{
		switch (_CardType)
		{
		case CardTypes.Item:
			if (!this.DefaultSlotIsHand || !_FailedToMatch)
			{
				return SlotsTypes.Base;
			}
			return SlotsTypes.Item;
		case CardTypes.Base:
			return SlotsTypes.Base;
		case CardTypes.Location:
			return SlotsTypes.Location;
		case CardTypes.Event:
			return SlotsTypes.Event;
		case CardTypes.Environment:
			return SlotsTypes.Environment;
		case CardTypes.Weather:
			return SlotsTypes.Weather;
		case CardTypes.Hand:
			return SlotsTypes.Hand;
		case CardTypes.Blueprint:
			return SlotsTypes.Blueprint;
		case CardTypes.Explorable:
			return SlotsTypes.Explorable;
		case CardTypes.EnvImprovement:
			return SlotsTypes.Improvement;
		case CardTypes.EnvDamage:
			return SlotsTypes.EnvDamage;
		}
		return SlotsTypes.Item;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0003774C File Offset: 0x0003594C
	public DynamicLayoutSlot GetSlotModelFromInfo(SlotInfo _Info, CardData _ForCard, CardData _WithLiquid)
	{
		if (_Info == null)
		{
			return null;
		}
		switch (_Info.SlotType)
		{
		case SlotsTypes.Item:
			if (this.GetItemsListFreeIndex(_Info.SlotIndex, _ForCard, _WithLiquid, 0f) == -1)
			{
				return this.BaseSlotModel;
			}
			return this.ItemSlotModel;
		case SlotsTypes.Base:
			return this.BaseSlotModel;
		case SlotsTypes.Location:
			return this.LocationSlotModel;
		case SlotsTypes.Event:
			if (!this.CardInspectionPopup.Initialized)
			{
				this.CardInspectionPopup.Init();
			}
			return this.CardInspectionPopup.InspectionSlot;
		case SlotsTypes.Environment:
			return this.EnvironmentSlot;
		case SlotsTypes.Weather:
			return this.WeatherSlot;
		case SlotsTypes.Hand:
			return this.ItemSlotModel;
		case SlotsTypes.Blueprint:
			return this.BlueprintSlotModel;
		case SlotsTypes.Equipment:
			return this.CharacterWindow.EquipmentSlotModel;
		case SlotsTypes.Inventory:
			return this.CurrentInspectionPopup.InventorySlotModel;
		case SlotsTypes.Explorable:
			return this.ExplorableSlotModel;
		}
		return null;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00037834 File Offset: 0x00035A34
	public DynamicLayoutSlot GetSlotForCard(CardData _Card, CardData _WithLiquid, SlotInfo _Info, CardData _NextEnv = null, CardData _PrevEnv = null, int _TravelIndex = 0)
	{
		DynamicLayoutSlot slotModelFromInfo = this.GetSlotModelFromInfo(_Info, _Card, _WithLiquid);
		if (slotModelFromInfo)
		{
			int index = _Info.SlotIndex;
			if (_Info.SlotType == SlotsTypes.Item && slotModelFromInfo.SlotType == SlotsTypes.Base && _Card.CardType == CardTypes.Item)
			{
				index = this.GetCenterScreenSlotIndex(CardTypes.Item);
			}
			if (slotModelFromInfo.CanReceiveCard(_Card, _WithLiquid, false, false))
			{
				if (!slotModelFromInfo.PileCompatible(_Card))
				{
					return this.AddSlot(_Info, _Card, _WithLiquid);
				}
				return this.FindPileForCard(_Card, _WithLiquid, slotModelFromInfo.SlotType, false, index, _NextEnv, _PrevEnv, _TravelIndex);
			}
		}
		if (_Info != null && _Info.SlotType == SlotsTypes.Inventory)
		{
			return this.GetSlotForCard(_Card, _WithLiquid, new SlotInfo(SlotsTypes.Item, -2), _NextEnv, _PrevEnv, _TravelIndex);
		}
		slotModelFromInfo = this.GetSlotModelFromInfo(new SlotInfo(this.CardToSlotType(_Card.CardType, true), -2), _Card, _WithLiquid);
		if (!slotModelFromInfo.PileCompatible(_Card))
		{
			return this.AddSlot(slotModelFromInfo.SlotType, _Card, _WithLiquid, -2);
		}
		return this.FindPileForCard(_Card, _WithLiquid, slotModelFromInfo.SlotType, false, -2, _NextEnv, _PrevEnv, _TravelIndex);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00037923 File Offset: 0x00035B23
	public void RefreshSlots(bool _Now)
	{
		if (_Now)
		{
			this.RefreshSlots();
			return;
		}
		this.DoSlotsRefresh = true;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x00037938 File Offset: 0x00035B38
	private void RefreshSlots()
	{
		if (this.EnvironmentSlot.AssignedCard != this.GM.CurrentEnvironmentCard)
		{
			this.EnvironmentSlot.AssignCard(this.GM.CurrentEnvironmentCard, false);
		}
		if (this.WeatherSlot.AssignedCard != this.GM.CurrentWeatherCard)
		{
			this.WeatherSlot.AssignCard(this.GM.CurrentWeatherCard, false);
		}
		if (this.GM.CurrentEventCard != null && !this.GM.CurrentEventCard.Destroyed && this.InspectedCard != this.GM.CurrentEventCard && !this.EventResolved)
		{
			this.InspectCard(this.GM.CurrentEventCard, false);
		}
		if (!this.LimitedItemSlots)
		{
			for (int i = this.ItemSlotsLine.Slots.Count - 1; i >= 0; i--)
			{
				if (this.ItemSlotsLine.Slots[i].AssignedCard == null)
				{
					this.ItemSlotsLine.RemoveSlot(i);
				}
				else if (this.ItemSlotsLine.Slots[i].AssignedCard.Destroyed)
				{
					this.ItemSlotsLine.RemoveSlot(i);
				}
			}
		}
		for (int j = this.BaseSlotsLine.Slots.Count - 1; j >= 0; j--)
		{
			if (this.BaseSlotsLine.Slots[j].AssignedCard == null)
			{
				this.BaseSlotsLine.RemoveSlot(j);
			}
			else if (this.BaseSlotsLine.Slots[j].AssignedCard.Destroyed)
			{
				this.BaseSlotsLine.RemoveSlot(j);
			}
			else
			{
				this.BaseSlotsLine.Slots[j].IsActive = this.IncludedInAnyFilterGroup(this.BaseSlotsLine.Slots[j]);
			}
		}
		for (int k = this.LocationSlotsLine.Slots.Count - 1; k >= 0; k--)
		{
			if (this.LocationSlotsLine.Slots[k].AssignedCard == null)
			{
				this.LocationSlotsLine.RemoveSlot(k);
			}
			else if (this.LocationSlotsLine.Slots[k].AssignedCard.Destroyed)
			{
				this.LocationSlotsLine.RemoveSlot(k);
			}
			else if (this.FiltersAffectLocationRow)
			{
				this.LocationSlotsLine.Slots[k].IsActive = this.IncludedInAnyFilterGroup(this.LocationSlotsLine.Slots[k]);
			}
		}
		for (int l = this.ExplorableSlotsLine.Slots.Count - 1; l >= 0; l--)
		{
			if (this.ExplorableSlotsLine.Slots[l].AssignedCard == null)
			{
				this.ExplorableSlotsLine.RemoveSlot(l);
			}
			else if (this.ExplorableSlotsLine.Slots[l].AssignedCard.Destroyed)
			{
				this.ExplorableSlotsLine.RemoveSlot(l);
			}
		}
		this.SortBlueprints();
		this.UpdateSlotsVisibility();
		this.GM.SortCards();
		this.DoSlotsRefresh = false;
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x00037C64 File Offset: 0x00035E64
	public void SortBlueprints()
	{
		if (!this.BlueprintModelsPopup)
		{
			return;
		}
		List<DynamicLayoutSlot> list = new List<DynamicLayoutSlot>();
		List<DynamicLayoutSlot> list2 = new List<DynamicLayoutSlot>();
		list2.AddRange(this.BlueprintSlotsLine.Slots);
		for (int i = 0; i < this.BlueprintModelsPopup.BlueprintTabs[this.BlueprintModelsPopup.CurrentTab].IncludedCards.Count; i++)
		{
			for (int j = 0; j < this.BlueprintSlotsLine.Slots.Count; j++)
			{
				if (this.BlueprintSlotsLine.Slots[j].AssignedCard && this.BlueprintSlotsLine.Slots[j].AssignedCard.CardModel == this.BlueprintModelsPopup.BlueprintTabs[this.BlueprintModelsPopup.CurrentTab].IncludedCards[i])
				{
					if (!list.Contains(this.BlueprintSlotsLine.Slots[j]))
					{
						list.Add(this.BlueprintSlotsLine.Slots[j]);
					}
					if (list2.Contains(this.BlueprintSlotsLine.Slots[j]))
					{
						list2.Remove(this.BlueprintSlotsLine.Slots[j]);
					}
				}
			}
		}
		this.BlueprintSlotsLine.Slots.Clear();
		this.BlueprintSlotsLine.Slots.AddRange(list);
		this.BlueprintSlotsLine.Slots.AddRange(list2);
		for (int k = this.BlueprintSlotsLine.Slots.Count - 1; k >= 0; k--)
		{
			if (this.BlueprintSlotsLine.Slots[k].AssignedCard == null)
			{
				this.BlueprintSlotsLine.RemoveSlot(k);
			}
			else if (this.BlueprintSlotsLine.Slots[k].AssignedCard.Destroyed)
			{
				this.BlueprintSlotsLine.RemoveSlot(k);
			}
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x00037E60 File Offset: 0x00036060
	public BookmarkGroup GetGroupForCard(CardData _Card)
	{
		if (!_Card)
		{
			return null;
		}
		if (this.AllBookmarkGroups.Count == 0)
		{
			return null;
		}
		for (int i = 0; i < this.AllBookmarkGroups.Count; i++)
		{
			if (this.AllBookmarkGroups[i].HasCard(_Card))
			{
				return this.AllBookmarkGroups[i];
			}
		}
		return null;
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x00037EC0 File Offset: 0x000360C0
	public void FindAndPulseCard(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return;
		}
		if (_Card.InBackground)
		{
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Event || _Card.CardModel.CardType == CardTypes.Hand)
		{
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.EnvImprovement)
		{
			this.CloseAllPopups();
			this.ExplorationDeckPopup.Setup(this.GM.CurrentExplorableCard);
			this.ExplorationDeckPopup.SelectTab(2);
			_Card.Pulse(0f);
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.EnvDamage)
		{
			this.CloseAllPopups();
			this.ExplorationDeckPopup.Setup(this.GM.CurrentExplorableCard);
			this.ExplorationDeckPopup.SelectTab(3);
			_Card.Pulse(0f);
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Explorable)
		{
			this.CloseAllPopups();
			this.ExplorableSlotsLine.MoveViewTo(_Card.CurrentSlot, true, true);
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Environment)
		{
			this.FindAndPulseCard(this.GM.CurrentExplorableCard);
			return;
		}
		if (_Card.CurrentContainer)
		{
			this.FindAndPulseCard(_Card.CurrentContainer);
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Weather)
		{
			this.CloseAllPopups();
			this.GM.CurrentWeatherCard.Pulse(0f);
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Item && _Card.CurrentSlotInfo.SlotType == SlotsTypes.Equipment)
		{
			this.CloseAllPopups();
			this.CharacterWindow.Open();
			if (_Card.CurrentSlot)
			{
				this.CharacterWindow.EquipmentSlotsLine.MoveViewTo(_Card.CurrentSlot, true, true);
			}
			return;
		}
		if (_Card.CardModel.CardType == CardTypes.Base || _Card.CardModel.CardType == CardTypes.Location || _Card.CardModel.CardType == CardTypes.Item)
		{
			this.CloseAllPopups();
			this.MoveViewToSlot(_Card.CurrentSlot, true, true);
		}
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x000380A0 File Offset: 0x000362A0
	public DynamicLayoutSlot FindBookmarkedSlot(BookmarkGroup _Group, CardData _WithLiquid, int _FromIndex)
	{
		SlotsTypes slotsTypes = this.CardToSlotType(_Group.GetCardType, false);
		CardLine cardLine;
		if (slotsTypes > SlotsTypes.Base)
		{
			if (slotsTypes != SlotsTypes.Location)
			{
				if (slotsTypes != SlotsTypes.Blueprint)
				{
					return null;
				}
				cardLine = (this.BlueprintInstanceGoToLocations ? this.LocationSlotsLine : this.BaseSlotsLine);
			}
			else
			{
				cardLine = this.LocationSlotsLine;
			}
		}
		else
		{
			cardLine = this.BaseSlotsLine;
		}
		if (_FromIndex >= cardLine.Count)
		{
			_FromIndex = 0;
		}
		for (int i = _FromIndex; i < cardLine.Slots.Count; i++)
		{
			if (cardLine.Slots[i].AssignedCard && !cardLine.Slots[i].AssignedCard.InBackground && _Group.HasCard(cardLine.Slots[i].AssignedCard.CardModel) && cardLine.Slots[i].AssignedCard.ContainedLiquidModel == _WithLiquid)
			{
				return cardLine.Slots[i];
			}
		}
		if (_FromIndex > 0)
		{
			for (int j = 0; j < _FromIndex; j++)
			{
				if (cardLine.Slots[j].AssignedCard && !cardLine.Slots[j].AssignedCard.InBackground && _Group.HasCard(cardLine.Slots[j].AssignedCard.CardModel) && cardLine.Slots[j].AssignedCard.ContainedLiquidModel == _WithLiquid)
				{
					return cardLine.Slots[j];
				}
			}
		}
		return null;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x00038224 File Offset: 0x00036424
	public DynamicLayoutSlot FindBookmarkedSlot(CardData _Card, CardData _WithLiquid, int _FromIndex)
	{
		SlotsTypes slotsTypes = this.CardToSlotType(_Card.CardType, false);
		CardLine cardLine;
		switch (slotsTypes)
		{
		case SlotsTypes.Item:
			cardLine = this.ItemSlotsLine;
			break;
		case SlotsTypes.Base:
			cardLine = this.BaseSlotsLine;
			break;
		case SlotsTypes.Location:
			cardLine = this.LocationSlotsLine;
			break;
		default:
			if (slotsTypes != SlotsTypes.Blueprint)
			{
				return null;
			}
			cardLine = (this.BlueprintInstanceGoToLocations ? this.LocationSlotsLine : this.BaseSlotsLine);
			break;
		}
		if (_FromIndex >= cardLine.Count)
		{
			_FromIndex = 0;
		}
		for (int i = _FromIndex; i < cardLine.Slots.Count; i++)
		{
			if (cardLine.Slots[i].AssignedCard && !cardLine.Slots[i].AssignedCard.InBackground && cardLine.Slots[i].AssignedCard.CardModel == _Card && cardLine.Slots[i].AssignedCard.ContainedLiquidModel == _WithLiquid)
			{
				return cardLine.Slots[i];
			}
		}
		if (_FromIndex > 0)
		{
			for (int j = 0; j < _FromIndex; j++)
			{
				if (cardLine.Slots[j].AssignedCard && !cardLine.Slots[j].AssignedCard.InBackground && cardLine.Slots[j].AssignedCard.CardModel == _Card && cardLine.Slots[j].AssignedCard.ContainedLiquidModel == _WithLiquid)
				{
					return cardLine.Slots[j];
				}
			}
		}
		return null;
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x000383BC File Offset: 0x000365BC
	public DynamicLayoutSlot FindPileForCard(CardData _Card, CardData _WithLiquid, SlotsTypes _Type, bool _NullIfNone, int _Index, CardData _NextEnv, CardData _PrevEnv, int _TravelIndex)
	{
		CardLine cardLine;
		switch (_Type)
		{
		case SlotsTypes.Item:
			cardLine = this.ItemSlotsLine;
			break;
		case SlotsTypes.Base:
			cardLine = this.BaseSlotsLine;
			break;
		case SlotsTypes.Location:
			cardLine = this.LocationSlotsLine;
			break;
		default:
			if (_Type != SlotsTypes.Inventory)
			{
				if (!_NullIfNone)
				{
					return this.AddSlot(_Type, _Card, _WithLiquid, _Index);
				}
				return null;
			}
			else
			{
				cardLine = this.CurrentInspectionPopup.InventorySlotsLine;
			}
			break;
		}
		for (int i = 0; i < cardLine.Count; i++)
		{
			if (cardLine.Slots[i].AssignedCard && (!_NextEnv || !cardLine.Slots[i].AssignedCard.CheckForBackground(_NextEnv, _PrevEnv, _TravelIndex)))
			{
				if (!_Card.CanContainLiquid)
				{
					if (cardLine.Slots[i].AssignedCard.CardModel == _Card && cardLine.Slots[i].CanReceiveCard(_Card, _WithLiquid, false, false))
					{
						return cardLine.Slots[i];
					}
				}
				else if (_WithLiquid)
				{
					if (cardLine.Slots[i].AssignedCard.ContainedLiquidModel != null && cardLine.Slots[i].AssignedCard.CardModel == _Card && cardLine.Slots[i].AssignedCard.ContainedLiquidModel == _WithLiquid && cardLine.Slots[i].CanReceiveCard(_Card, _WithLiquid, false, false))
					{
						return cardLine.Slots[i];
					}
				}
				else if (cardLine.Slots[i].AssignedCard.CardModel == _Card && cardLine.Slots[i].AssignedCard.ContainedLiquidModel == null && cardLine.Slots[i].CanReceiveCard(_Card, _WithLiquid, false, false))
				{
					return cardLine.Slots[i];
				}
			}
		}
		if (!_NullIfNone)
		{
			return this.AddSlot(_Type, _Card, _WithLiquid, _Index);
		}
		return null;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x000385D0 File Offset: 0x000367D0
	private int GetItemsListFreeIndex(int _Index, CardData _ForCard, CardData _WithLiquid, float _ObjectWeight)
	{
		if (this.GM.InGamePlayerWeight && !this.GM.CanTakeCardsEvenOnFullWeight && this.GM.InGamePlayerWeight.CurrentValue(false) + _ObjectWeight > this.GM.PlayerWeightStat.MinMaxValue.y)
		{
			return -1;
		}
		int num = -1;
		if (_Index >= 0 && _Index < this.ItemSlotsLine.Slots.Count)
		{
			for (int i = _Index; i < this.ItemSlotsLine.Slots.Count; i++)
			{
				if (!this.ItemSlotsLine.Slots[i].AssignedCard)
				{
					num = i;
					break;
				}
				if (this.ItemSlotsLine.Slots[i].CanHostPile && this.ItemSlotsLine.Slots[i].CanReceiveCard(_ForCard, _WithLiquid, false, false) && _ForCard && _ForCard == this.ItemSlotsLine.Slots[i].AssignedCard.CardModel && _WithLiquid == this.ItemSlotsLine.Slots[i].AssignedCard.ContainedLiquidModel)
				{
					num = i;
					break;
				}
			}
		}
		if (num == -1)
		{
			for (int j = 0; j < this.ItemSlotsLine.Slots.Count; j++)
			{
				if (!this.ItemSlotsLine.Slots[j].AssignedCard)
				{
					num = j;
					break;
				}
				if (this.ItemSlotsLine.Slots[j].CanHostPile && this.ItemSlotsLine.Slots[j].CanReceiveCard(_ForCard, _WithLiquid, false, false) && _ForCard == this.ItemSlotsLine.Slots[j].AssignedCard.CardModel && _WithLiquid == this.ItemSlotsLine.Slots[j].AssignedCard.ContainedLiquidModel)
				{
					num = j;
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x000387DE File Offset: 0x000369DE
	private int GetInventoryListFreeIndex(int _Index, CardData _ForCard, CardData _WithLiquid, float _Weight)
	{
		return this.InspectedCard.GetIndexForInventory(_Index, _ForCard, _WithLiquid, _Weight);
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x000387F0 File Offset: 0x000369F0
	public List<InGameCardBase> GetHandCards(bool _CountInInventories)
	{
		List<InGameCardBase> list = new List<InGameCardBase>();
		for (int i = 0; i < this.ItemSlotsLine.Slots.Count; i++)
		{
			if (this.ItemSlotsLine.Slots[i].AssignedCard)
			{
				list.AddRange(this.ItemSlotsLine.Slots[i].GetCardPile(_CountInInventories || this.ItemSlotsLine.Slots[i].AssignedCard.CardModel.InHandWhenEquipped, true, false));
				if (this.ItemSlotsLine.Slots[i].AssignedCard.InventoryCount(null) > 0 && (_CountInInventories || this.ItemSlotsLine.Slots[i].AssignedCard.CardModel.InHandWhenEquipped))
				{
					for (int j = 0; j < this.ItemSlotsLine.Slots[i].AssignedCard.CardsInInventory.Count; j++)
					{
						if (this.ItemSlotsLine.Slots[i].AssignedCard.CardsInInventory[j] != null && !this.ItemSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].IsFree)
						{
							list.AddRange(this.ItemSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].AllCards);
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00038978 File Offset: 0x00036B78
	public List<InGameCardBase> GetBaseCards(bool _CountInInventories, bool _BlueprintSpecialMode)
	{
		List<InGameCardBase> list = new List<InGameCardBase>();
		if (!_BlueprintSpecialMode)
		{
			for (int i = 0; i < this.BaseSlotsLine.Slots.Count; i++)
			{
				if (this.BaseSlotsLine.Slots[i].AssignedCard)
				{
					list.AddRange(this.BaseSlotsLine.Slots[i].GetCardPile(_CountInInventories, true, false));
					if (this.BaseSlotsLine.Slots[i].AssignedCard.InventoryCount(null) > 0 && _CountInInventories)
					{
						for (int j = 0; j < this.BaseSlotsLine.Slots[i].AssignedCard.CardsInInventory.Count; j++)
						{
							if (this.BaseSlotsLine.Slots[i].AssignedCard.CardsInInventory[j] != null && !this.BaseSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].IsFree)
							{
								list.AddRange(this.BaseSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].AllCards);
							}
						}
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < this.BaseSlotsLine.Slots.Count; k++)
			{
				if (this.BaseSlotsLine.Slots[k].AssignedCard)
				{
					if (this.BaseSlotsLine.Slots[k].AssignedCard.CardModel.CardType != CardTypes.Blueprint)
					{
						list.AddRange(this.BaseSlotsLine.Slots[k].GetCardPile(_CountInInventories, true, false));
						if (this.BaseSlotsLine.Slots[k].AssignedCard.InventoryCount(null) > 0 && _CountInInventories)
						{
							for (int l = 0; l < this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory.Count; l++)
							{
								if (this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory[l] != null && !this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory[l].IsFree)
								{
									list.AddRange(this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory[l].AllCards);
								}
							}
						}
					}
					else
					{
						if (this.BaseSlotsLine.Slots[k].AssignedCard.CardModel.BlueprintResult != null && this.BaseSlotsLine.Slots[k].AssignedCard.BlueprintData.CurrentStage > 0 && this.BaseSlotsLine.Slots[k].AssignedCard.CardModel.BlueprintResult.Length != 0 && (this.BaseSlotsLine.Slots[k].AssignedCard.CardModel.BlueprintResult[0].DroppedCard.CardType == CardTypes.Base || this.BaseSlotsLine.Slots[k].AssignedCard.CardModel.BlueprintResult[0].DroppedCard.CardType == CardTypes.Item))
						{
							list.Add(this.BaseSlotsLine.Slots[k].AssignedCard);
						}
						if (this.BaseSlotsLine.Slots[k].AssignedCard.InventoryCount(null) > 0)
						{
							for (int m = 0; m < this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory.Count; m++)
							{
								if (this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory[m] != null && !this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory[m].IsFree)
								{
									list.AddRange(this.BaseSlotsLine.Slots[k].AssignedCard.CardsInInventory[m].AllCards);
								}
							}
						}
					}
				}
			}
			for (int n = 0; n < this.LocationSlotsLine.Slots.Count; n++)
			{
				if (this.LocationSlotsLine.Slots[n].AssignedCard && this.LocationSlotsLine.Slots[n].AssignedCard.CardModel.CardType == CardTypes.Blueprint)
				{
					if (this.LocationSlotsLine.Slots[n].AssignedCard.CardModel.BlueprintResult != null && this.LocationSlotsLine.Slots[n].AssignedCard.BlueprintData.CurrentStage > 0 && this.LocationSlotsLine.Slots[n].AssignedCard.CardModel.BlueprintResult.Length != 0 && (this.LocationSlotsLine.Slots[n].AssignedCard.CardModel.BlueprintResult[0].DroppedCard.CardType == CardTypes.Base || this.LocationSlotsLine.Slots[n].AssignedCard.CardModel.BlueprintResult[0].DroppedCard.CardType == CardTypes.Item))
					{
						list.Add(this.LocationSlotsLine.Slots[n].AssignedCard);
					}
					if (this.LocationSlotsLine.Slots[n].AssignedCard.InventoryCount(null) > 0)
					{
						for (int num = 0; num < this.LocationSlotsLine.Slots[n].AssignedCard.CardsInInventory.Count; num++)
						{
							if (this.LocationSlotsLine.Slots[n].AssignedCard.CardsInInventory[num] != null && !this.LocationSlotsLine.Slots[n].AssignedCard.CardsInInventory[num].IsFree)
							{
								list.AddRange(this.LocationSlotsLine.Slots[n].AssignedCard.CardsInInventory[num].AllCards);
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00039020 File Offset: 0x00037220
	public List<InGameCardBase> GetLocationCards(bool _CountInInventories, bool _BlueprintSpecialMode)
	{
		List<InGameCardBase> list = new List<InGameCardBase>();
		if (_BlueprintSpecialMode)
		{
			for (int i = 0; i < this.LocationSlotsLine.Slots.Count; i++)
			{
				if (this.LocationSlotsLine.Slots[i].AssignedCard)
				{
					if (this.LocationSlotsLine.Slots[i].AssignedCard.CardModel.CardType != CardTypes.Blueprint)
					{
						list.AddRange(this.LocationSlotsLine.Slots[i].GetCardPile(_CountInInventories, true, false));
						if (this.LocationSlotsLine.Slots[i].AssignedCard.InventoryCount(null) > 0 && _CountInInventories)
						{
							for (int j = 0; j < this.LocationSlotsLine.Slots[i].AssignedCard.CardsInInventory.Count; j++)
							{
								if (this.LocationSlotsLine.Slots[i].AssignedCard.CardsInInventory[j] != null && !this.LocationSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].IsFree)
								{
									list.AddRange(this.LocationSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].AllCards);
								}
							}
						}
					}
					else if (this.LocationSlotsLine.Slots[i].AssignedCard.CardModel.BlueprintResult != null && this.LocationSlotsLine.Slots[i].AssignedCard.BlueprintData.CurrentStage != 0 && this.LocationSlotsLine.Slots[i].AssignedCard.CardModel.BlueprintResult.Length != 0 && this.LocationSlotsLine.Slots[i].AssignedCard.CardModel.BlueprintResult[0].DroppedCard.CardType == CardTypes.Location)
					{
						list.Add(this.LocationSlotsLine.Slots[i].AssignedCard);
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < this.LocationSlotsLine.Slots.Count; k++)
			{
				if (this.LocationSlotsLine.Slots[k].AssignedCard)
				{
					list.AddRange(this.LocationSlotsLine.Slots[k].GetCardPile(_CountInInventories, true, false));
					if (this.LocationSlotsLine.Slots[k].AssignedCard.InventoryCount(null) > 0 && _CountInInventories)
					{
						for (int l = 0; l < this.LocationSlotsLine.Slots[k].AssignedCard.CardsInInventory.Count; l++)
						{
							if (this.LocationSlotsLine.Slots[k].AssignedCard.CardsInInventory[l] != null && !this.LocationSlotsLine.Slots[k].AssignedCard.CardsInInventory[l].IsFree)
							{
								list.AddRange(this.LocationSlotsLine.Slots[k].AssignedCard.CardsInInventory[l].AllCards);
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00039384 File Offset: 0x00037584
	public List<InGameCardBase> GetEquippedCards(bool _CountInInventories)
	{
		List<InGameCardBase> list = new List<InGameCardBase>();
		for (int i = 0; i < this.CharacterWindow.EquipmentSlotsLine.Slots.Count; i++)
		{
			if (this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard)
			{
				list.Add(this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard);
				if (this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard.InventoryCount(null) > 0 && _CountInInventories)
				{
					for (int j = 0; j < this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard.CardsInInventory.Count; j++)
					{
						if (this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard.CardsInInventory[j] != null && !this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].IsFree)
						{
							list.AddRange(this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard.CardsInInventory[j].AllCards);
						}
					}
				}
				if (this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard.ContainedLiquid && _CountInInventories)
				{
					list.Add(this.CharacterWindow.EquipmentSlotsLine.Slots[i].AssignedCard.ContainedLiquid);
				}
			}
		}
		return list;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0003953A File Offset: 0x0003773A
	private DynamicLayoutSlot AddSlot(SlotInfo _Info, CardData _ForCard, CardData _WithLiquid)
	{
		return this.AddSlot(_Info.SlotType, _ForCard, _WithLiquid, _Info.SlotIndex);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00039550 File Offset: 0x00037750
	private DynamicLayoutSlot AddSlot(SlotsTypes _Type, CardData _ForCard, CardData _WithLiquid, int _Index = -2)
	{
		switch (_Type)
		{
		case SlotsTypes.Item:
			if (!this.LimitedItemSlots || this.ItemSlotsLine.Slots.Count < this.MaxItemSlots)
			{
				if (_Index < 0)
				{
					return this.ItemSlotsLine.AddSlot(this.ItemSlotsLine.Slots.Count);
				}
				return this.ItemSlotsLine.AddSlot(_Index);
			}
			else
			{
				int itemsListFreeIndex = this.GetItemsListFreeIndex(_Index, _ForCard, _WithLiquid, _ForCard ? _ForCard.ObjectWeight : 0f);
				if (itemsListFreeIndex == -1)
				{
					return this.GetSlotForCard(_ForCard, _WithLiquid, new SlotInfo(SlotsTypes.Base, 0), null, null, 0);
				}
				if (itemsListFreeIndex == _Index || _Index < 0 || _Index >= this.ItemSlotsLine.Slots.Count)
				{
					return this.ItemSlotsLine.Slots[itemsListFreeIndex];
				}
				if (itemsListFreeIndex < _Index)
				{
					this.ItemSlotsLine.MoveSlot(itemsListFreeIndex, _Index);
					return this.ItemSlotsLine.Slots[_Index];
				}
				this.ItemSlotsLine.MoveSlot(itemsListFreeIndex, _Index + 1);
				return this.ItemSlotsLine.Slots[_Index + 1];
			}
			break;
		case SlotsTypes.Base:
			if (_Index < -1 || _Index >= this.BaseSlotsLine.Slots.Count)
			{
				return this.BaseSlotsLine.AddSlot(this.BaseSlotsLine.Slots.Count);
			}
			if (this.BaseSlotsLine.Slots[Mathf.Max(0, _Index)].AssignedCard == null)
			{
				return this.BaseSlotsLine.Slots[Mathf.Max(0, _Index)];
			}
			return this.BaseSlotsLine.AddSlot(_Index + 1);
		case SlotsTypes.Location:
			if (_Index < -1 || _Index >= this.LocationSlotsLine.Slots.Count)
			{
				return this.LocationSlotsLine.AddSlot(this.LocationSlotsLine.Slots.Count);
			}
			if (this.LocationSlotsLine.Slots[Mathf.Max(0, _Index)].AssignedCard == null)
			{
				return this.LocationSlotsLine.Slots[Mathf.Max(0, _Index)];
			}
			return this.LocationSlotsLine.AddSlot(_Index + 1);
		case SlotsTypes.Event:
			return this.CardInspectionPopup.InspectionSlot;
		case SlotsTypes.Environment:
			return this.EnvironmentSlot;
		case SlotsTypes.Weather:
			return this.WeatherSlot;
		case SlotsTypes.Blueprint:
			if (_Index < -1 || _Index >= this.BlueprintSlotsLine.Slots.Count)
			{
				return this.BlueprintSlotsLine.AddSlot(this.BlueprintSlotsLine.Slots.Count);
			}
			if (this.BlueprintSlotsLine.Slots[Mathf.Max(0, _Index)].AssignedCard == null)
			{
				return this.BlueprintSlotsLine.Slots[Mathf.Max(0, _Index)];
			}
			return this.BlueprintSlotsLine.AddSlot(_Index + 1);
		case SlotsTypes.Equipment:
			return this.CharacterWindow.FindSlotFor(_ForCard, false, _Index);
		case SlotsTypes.Inventory:
		{
			int inventoryListFreeIndex = this.GetInventoryListFreeIndex(_Index, _ForCard, _WithLiquid, -1f);
			if (inventoryListFreeIndex == -1)
			{
				return this.GetSlotForCard(_ForCard, _WithLiquid, new SlotInfo(SlotsTypes.Base, 0), null, null, 0);
			}
			if (inventoryListFreeIndex == _Index || _Index < 0 || _Index >= this.CurrentInspectionPopup.SlotCount)
			{
				return this.CurrentInspectionPopup.GetSlot(inventoryListFreeIndex);
			}
			if (this.InspectedCard.CardsInInventory.Count <= inventoryListFreeIndex)
			{
				this.InspectedCard.CardsInInventory.Add(new InventorySlot());
			}
			if (!this.InspectedCard.IsLegacyInventory)
			{
				return this.CurrentInspectionPopup.GetSlot(inventoryListFreeIndex);
			}
			if (inventoryListFreeIndex < _Index)
			{
				this.CurrentInspectionPopup.MoveSlot(inventoryListFreeIndex, _Index);
				return this.CurrentInspectionPopup.GetSlot(_Index);
			}
			this.CurrentInspectionPopup.MoveSlot(inventoryListFreeIndex, _Index + 1);
			return this.CurrentInspectionPopup.GetSlot(_Index + 1);
		}
		case SlotsTypes.Explorable:
			if (_Index < -1 || _Index >= this.ExplorableSlotsLine.Slots.Count)
			{
				return this.ExplorableSlotsLine.AddSlot(this.ExplorableSlotsLine.Slots.Count);
			}
			if (this.ExplorableSlotsLine.Slots[Mathf.Max(0, _Index)].AssignedCard == null)
			{
				return this.ExplorableSlotsLine.Slots[Mathf.Max(0, _Index)];
			}
			return this.ExplorableSlotsLine.AddSlot(_Index + 1);
		}
		return null;
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x000399A0 File Offset: 0x00037BA0
	public void ToggleFilterTag(CardFilterGroup _FilterTag)
	{
		if (!_FilterTag)
		{
			return;
		}
		if (this.OnlyOneFilterAtATime && !this.CurrentFilterTags.Contains(_FilterTag))
		{
			this.ClearFilterTags();
		}
		if (!this.CurrentFilterTags.Contains(_FilterTag))
		{
			this.CurrentFilterTags.Add(_FilterTag);
			if (this.FilterBackgroundImages != null)
			{
				for (int i = 0; i < this.FilterBackgroundImages.Length; i++)
				{
					if (this.FilterBackgroundImages[i])
					{
						this.FilterBackgroundImages[i].color = _FilterTag.GetColor(this.FilterBackgroundImages[i].color);
						this.FilterBackgroundImages[i].enabled = true;
					}
				}
			}
		}
		else
		{
			this.CurrentFilterTags.Remove(_FilterTag);
			if (this.FilterBackgroundImages != null)
			{
				for (int j = 0; j < this.FilterBackgroundImages.Length; j++)
				{
					if (this.FilterBackgroundImages[j])
					{
						this.FilterBackgroundImages[j].enabled = false;
					}
				}
			}
		}
		this.RefreshSlots();
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00039A94 File Offset: 0x00037C94
	public void ClearFilterTags()
	{
		this.CurrentFilterTags.Clear();
		if (this.FilterBackgroundImages != null)
		{
			for (int i = 0; i < this.FilterBackgroundImages.Length; i++)
			{
				if (this.FilterBackgroundImages[i])
				{
					this.FilterBackgroundImages[i].enabled = false;
				}
			}
		}
		this.RefreshSlots();
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00039AEC File Offset: 0x00037CEC
	private bool IncludedInAnyFilterGroup(DynamicLayoutSlot _Slot)
	{
		if (this.CurrentFilterTags == null)
		{
			return true;
		}
		if (this.CurrentFilterTags.Count == 0)
		{
			return true;
		}
		if (!_Slot.AssignedCard)
		{
			return true;
		}
		if (!_Slot.AssignedCard.CardModel)
		{
			return true;
		}
		for (int i = 0; i < this.CurrentFilterTags.Count; i++)
		{
			if (this.CurrentFilterTags[i] && this.CurrentFilterTags[i].Contains(_Slot.AssignedCard.CardModel))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00039B80 File Offset: 0x00037D80
	private bool IncludedInAllFilterGroups(DynamicLayoutSlot _Slot)
	{
		if (this.CurrentFilterTags == null)
		{
			return true;
		}
		if (this.CurrentFilterTags.Count == 0)
		{
			return true;
		}
		if (!_Slot.AssignedCard)
		{
			return true;
		}
		if (!_Slot.AssignedCard.CardModel)
		{
			return true;
		}
		for (int i = 0; i < this.CurrentFilterTags.Count; i++)
		{
			if (this.CurrentFilterTags[i] && !this.CurrentFilterTags[i].Contains(_Slot.AssignedCard.CardModel))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x00039C14 File Offset: 0x00037E14
	private void UpdateSlotsVisibility()
	{
		bool flag = true;
		if (!this.LimitedItemSlots)
		{
			for (int i = this.ItemSlotsLine.Slots.Count - 1; i >= 0; i--)
			{
				if (!this.ItemSlotsLine.Slots[i].AssignedCard)
				{
					flag = false;
				}
				if (i > 0 && this.ItemSlotsLine.Slots[i - 1].AssignedCard)
				{
					this.ItemSlotsLine.Slots[i].IsActive = true;
					break;
				}
				this.ItemSlotsLine.Slots[i].IsActive = (i < this.MinItemSlots);
			}
			if (flag)
			{
				this.AddSlot(SlotsTypes.Item, null, null, -2);
			}
			flag = true;
		}
		int j = this.BaseSlotsLine.Slots.Count - 1;
		while (j >= 0)
		{
			if (!this.BaseSlotsLine.Slots[j].AssignedCard)
			{
				flag = false;
			}
			if (j > 0 && this.BaseSlotsLine.Slots[j - 1].AssignedCard)
			{
				if (this.BaseSlotsLine.Slots[j].AssignedCard)
				{
					this.BaseSlotsLine.Slots[j].IsActive = this.IncludedInAnyFilterGroup(this.BaseSlotsLine.Slots[j]);
					break;
				}
				this.BaseSlotsLine.Slots[j].IsActive = this.IncludedInAnyFilterGroup(this.BaseSlotsLine.Slots[j - 1]);
				break;
			}
			else
			{
				this.BaseSlotsLine.Slots[j].IsActive = (j < this.MinBaseSlots);
				j--;
			}
		}
		if (flag)
		{
			this.AddSlot(SlotsTypes.Base, null, null, -2);
		}
		flag = true;
		int k = this.LocationSlotsLine.Slots.Count - 1;
		while (k >= 0)
		{
			if (!this.LocationSlotsLine.Slots[k].AssignedCard)
			{
				flag = false;
			}
			if (k > 0 && this.LocationSlotsLine.Slots[k - 1].AssignedCard)
			{
				if (this.LocationSlotsLine.Slots[k].AssignedCard)
				{
					this.LocationSlotsLine.Slots[k].IsActive = this.IncludedInAnyFilterGroup(this.LocationSlotsLine.Slots[k]);
					break;
				}
				this.LocationSlotsLine.Slots[k].IsActive = this.IncludedInAnyFilterGroup(this.LocationSlotsLine.Slots[k - 1]);
				break;
			}
			else
			{
				this.LocationSlotsLine.Slots[k].IsActive = (k < this.MinLocationSlots);
				k--;
			}
		}
		if (flag)
		{
			this.AddSlot(SlotsTypes.Location, null, null, -2);
		}
		for (int l = this.ExplorableSlotsLine.Slots.Count - 1; l >= 0; l--)
		{
			this.ExplorableSlotsLine.Slots[l].IsActive = (l < this.MinExplorableSlots || this.ExplorableSlotsLine.Slots[l].AssignedCard);
		}
		for (int m = this.BlueprintSlotsLine.Slots.Count - 1; m >= 0; m--)
		{
			this.BlueprintSlotsLine.Slots[m].IsActive = (m < this.MinBlueprintSlots || (this.BlueprintSlotsLine.Slots[m].AssignedCard && this.BlueprintModelsPopup.BlueprintIsVisible(this.BlueprintSlotsLine.Slots[m].AssignedCard)));
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00039FEC File Offset: 0x000381EC
	public void MoveViewToSlot(DynamicLayoutSlot _Slot, bool _ForceCenter = false, bool _Pulse = false)
	{
		if (_Slot == null)
		{
			return;
		}
		if (this.DoSlotsRefresh)
		{
			base.StartCoroutine(this.WaitBeforeMovingView(_Slot, _ForceCenter, _Pulse));
			return;
		}
		if (this.ItemSlotsLine.Slots.Contains(_Slot))
		{
			this.ItemSlotsLine.MoveViewTo(_Slot, _ForceCenter, _Pulse);
			return;
		}
		if (this.BaseSlotsLine.Slots.Contains(_Slot))
		{
			this.BaseSlotsLine.MoveViewTo(_Slot, _ForceCenter, _Pulse);
			return;
		}
		if (this.LocationSlotsLine.Slots.Contains(_Slot))
		{
			this.LocationSlotsLine.MoveViewTo(_Slot, _ForceCenter, _Pulse);
			return;
		}
		if (this.ExplorableSlotsLine.Slots.Contains(_Slot))
		{
			this.ExplorableSlotsLine.MoveViewTo(_Slot, _ForceCenter, _Pulse);
			return;
		}
		if (this.BlueprintSlotsLine.Slots.Contains(_Slot))
		{
			this.BlueprintSlotsLine.MoveViewTo(_Slot, _ForceCenter, _Pulse);
			return;
		}
		if (this.CharacterWindow.EquipmentSlotsLine.Slots.Contains(_Slot))
		{
			this.CharacterWindow.EquipmentSlotsLine.MoveViewTo(_Slot, _ForceCenter, _Pulse);
			return;
		}
		if (this.CurrentInspectionPopup && this.CurrentInspectionPopup.InventorySlotsLine && this.CurrentInspectionPopup.GetIndex(_Slot) != -1)
		{
			this.CurrentInspectionPopup.InventorySlotsLine.MoveViewTo(_Slot, _ForceCenter, _Pulse);
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0003A12D File Offset: 0x0003832D
	private IEnumerator WaitBeforeMovingView(DynamicLayoutSlot _Slot, bool _ForceCenter, bool _Pulse)
	{
		while (this.DoSlotsRefresh)
		{
			yield return null;
		}
		this.MoveViewToSlot(_Slot, _ForceCenter, _Pulse);
		yield break;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0003A154 File Offset: 0x00038354
	public int GetCenterScreenSlotIndex(CardTypes _ForType)
	{
		if (_ForType == CardTypes.Blueprint)
		{
			if (!this.BlueprintInstanceGoToLocations)
			{
				return this.BaseSlotsLine.GetCenterSlotIndex();
			}
			return this.LocationSlotsLine.GetCenterSlotIndex();
		}
		else
		{
			if (_ForType == CardTypes.Location)
			{
				return this.LocationSlotsLine.GetCenterSlotIndex();
			}
			return this.BaseSlotsLine.GetCenterSlotIndex();
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0003A1A0 File Offset: 0x000383A0
	public IEnumerator DayOverPopup(int _Day)
	{
		if (!this.DayChangePopup)
		{
			yield break;
		}
		base.StartCoroutine(this.DayChangePopup.PlayFeedback(LocalizedString.DayCounter(_Day)));
		if (this.SunEarned)
		{
			base.StartCoroutine(this.SunEarned.PlayFeedback());
		}
		if (this.MoonEarned && _Day % this.GM.DaysPerMoon == 0)
		{
			base.StartCoroutine(this.MoonEarned.PlayFeedback());
		}
		while (this.DayChangePopup.IsPlaying)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0003A1B8 File Offset: 0x000383B8
	public void UpdateTimeInfo(bool _Init)
	{
		if (this.CurrentDaytimePoints == this.GM.CurrentTickInfo.z && this.CurrentMiniTicks == this.GM.CurrentMiniTicks && !_Init)
		{
			return;
		}
		if (!_Init)
		{
			if (this.DayTimePointsUpdate != null)
			{
				this.DayTimePointsUpdate.Kill(true);
			}
			this.DayTimePointsUpdate = DOTween.Sequence().OnKill(delegate
			{
				this.DayTimePointsUpdate = null;
			});
			this.DayTimePointsUpdate.Append(DOTween.To(() => this.CurrentDaytimePoints, delegate(int x)
			{
				this.CurrentDaytimePoints = x;
			}, this.GM.CurrentTickInfo.z, this.TickRealTimeDuration - this.TickTimePulseDuration).SetEase(Ease.OutQuad));
			this.DayTimePointsUpdate.Insert(0f, DOTween.To(() => this.CurrentMiniTicks, delegate(int x)
			{
				this.CurrentMiniTicks = x;
			}, this.GM.CurrentMiniTicks, this.TickRealTimeDuration - this.TickTimePulseDuration).SetEase(Ease.OutQuad));
			this.DayTimePointsUpdate.Append(this.DayTimePoints.transform.DOPunchScale(Vector3.one * this.TickTimePulseScale, this.TickTimePulseDuration, 5, 0.5f));
			if (this.DaylightSpendingTextPrefab)
			{
				float num = Mathf.Abs(GameManager.TickToHours(this.GM.CurrentTickInfo.z - this.CurrentDaytimePoints, this.GM.CurrentMiniTicks - this.CurrentMiniTicks));
				string text = string.Format("+{0}", HoursDisplay.HoursToCompleteString(num));
				if (!this.CurrentDaylightSpendingText)
				{
					this.LastDaytimeChange = num;
					text = string.Format("+{0}", HoursDisplay.HoursToCompleteString(this.LastDaytimeChange));
					this.CurrentDaylightSpendingText = UnityEngine.Object.Instantiate<UIFeedbackText>(this.DaylightSpendingTextPrefab, this.TextsParent);
					base.StartCoroutine(this.CurrentDaylightSpendingText.PlayFeedback(this.TimeSpentWheel.transform.position + this.TimeSpentTextOffset, text, this.NegativeTextColor));
				}
				else
				{
					this.LastDaytimeChange += num;
					text = string.Format("+{0}", HoursDisplay.HoursToCompleteString(this.LastDaytimeChange));
					this.CurrentDaylightSpendingText.UpdateText(text, this.NegativeTextColor, true);
				}
			}
		}
		else
		{
			this.CurrentDaytimePoints = this.GM.CurrentTickInfo.z;
			this.CurrentMiniTicks = this.GM.CurrentMiniTicks;
		}
		if (this.CurrentDay)
		{
			this.CurrentDay.text = LocalizedString.DayCounter(this.GM.CurrentDay);
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0003A460 File Offset: 0x00038660
	public IEnumerator UpdateSpendingTime(int _SpentPoint, int _TotalPoints, InGameCardBase _Card, FadeToBlackTypes _FadeToBlack, string _Text, bool _Cancellable, bool _Tips)
	{
		float timer = 0f;
		float time = (_TotalPoints <= 1) ? (this.TickRealTimeDuration * 2f) : this.TickRealTimeDuration;
		if (_SpentPoint == 1)
		{
			this.FadeToBlack.SetFade(_FadeToBlack, _Text, _Tips);
			if (_FadeToBlack != FadeToBlackTypes.None)
			{
				yield return new WaitForSeconds(this.FadeToBlack.FadeDuration);
			}
			if (_FadeToBlack != FadeToBlackTypes.None || !_Card)
			{
				this.TimeSpentWheel.transform.position = this.FadeToBlack.TimeSpentPos;
			}
			else
			{
				this.TimeSpentWheel.transform.position = _Card.transform.position;
			}
			this.TimeSpentWheel.SetProgress(0f, true);
			if (this.CancelButton)
			{
				this.CancelButton.SetActive(_Cancellable);
			}
		}
		while (timer < time * 0.5f)
		{
			this.TimeSpentWheel.SetProgress(Mathf.Lerp((float)(_SpentPoint - 1) / (float)_TotalPoints, (float)_SpentPoint / (float)_TotalPoints, timer / (time * 0.5f)), false);
			timer += Time.deltaTime;
			yield return null;
		}
		if (_SpentPoint == _TotalPoints)
		{
			this.TimeSpentWheel.SetProgress(1f, true);
		}
		yield return new WaitForSeconds(time * 0.5f);
		if (_SpentPoint == _TotalPoints && _FadeToBlack != FadeToBlackTypes.None)
		{
			base.StartCoroutine(this.FadeOut(time));
		}
		yield break;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0003A4AF File Offset: 0x000386AF
	public void CancelWheel()
	{
		this.TimeSpentWheel.PlayStop();
		if (this.FadeToBlack.CurrentFadeType != FadeToBlackTypes.None && !this.LoadingScreen)
		{
			this.FadeToBlack.SetFade(FadeToBlackTypes.None, "", false);
		}
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0003A4E3 File Offset: 0x000386E3
	private IEnumerator FadeOut(float _ElapsedTime)
	{
		yield return new WaitForSeconds(this.FadeToBlack.FadeStay - _ElapsedTime);
		if (!this.LoadingScreen)
		{
			this.FadeToBlack.SetFade(FadeToBlackTypes.None, "", false);
		}
		yield break;
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0003A4F9 File Offset: 0x000386F9
	public IEnumerator UpdateStatusAlerts()
	{
		if (this.StatusAlerts.Count == 0 || !this.StatusAlertTextPrefab || !this.StatusAlertsParent)
		{
			yield break;
		}
		int num = this.StatusAlerts.Count - 1;
		while (num >= 0 && this.StatusAlerts.Count != 0)
		{
			if (!(this.StatusAlerts[num].StatusGraphics == null))
			{
				this.PlayStatusAlertText(this.StatusAlerts[num]);
				this.StatusAlerts.RemoveAt(num);
			}
			num--;
		}
		yield return null;
		this.StatusAlerts.Clear();
		yield break;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x0003A508 File Offset: 0x00038708
	public float SetLoading(bool _Loading)
	{
		this.LoadingScreen = _Loading;
		if (_Loading)
		{
			this.FadeToBlack.SetFade(FadeToBlackTypes.Partial, LocalizedString.Loading, true);
		}
		else
		{
			this.FadeToBlack.SetFade(FadeToBlackTypes.None, "", false);
		}
		return this.FadeToBlack.FadeDuration;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0003A558 File Offset: 0x00038758
	public void PlayStatusAlertText(StatStatus _Status)
	{
		if (this.StatusAlertMoves)
		{
			base.StartCoroutine(UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.StatusAlertTextPrefab, _Status.StatusGraphics.StatusAlertTextParent).PlayFeedback(_Status.StatusGraphics.transform.position, _Status.Description, _Status.Icon));
		}
		else
		{
			base.StartCoroutine(UnityEngine.Object.Instantiate<UIFeedbackTextAndIcon>(this.StatusAlertTextPrefab, _Status.StatusGraphics.StatusAlertTextParent).PlayFeedback(_Status.StatusGraphics.StatusAlertTextParent.position, _Status.Description, _Status.Icon));
		}
		base.StartCoroutine(GraphicsManager.DeactivateAfterDelay(_Status.StatusGraphics.StatusAlertTextParent.gameObject, this.StatusAlertTextPrefab.Duration));
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0003A61C File Offset: 0x0003881C
	public void PlayBlueprintUnlocked(CardData _Blueprint)
	{
		if (!this.BlueprintUnlockedPrefab || !this.BlueprintNotificationParent || !_Blueprint)
		{
			return;
		}
		this.BlueprintsUnlockedFeedbacks.Add(new GraphicsManager.PlayingBlueprintUnlockedPopup(_Blueprint));
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0003A652 File Offset: 0x00038852
	public void PlayWound(CardData _Wound)
	{
		if (!this.WoundReceivedPrefab || !this.BlueprintNotificationParent || !_Wound)
		{
			return;
		}
		this.WoundsReceivedFeedbacks.Add(new GraphicsManager.PlayingBlueprintUnlockedPopup(_Wound));
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0003A688 File Offset: 0x00038888
	public void PlayObjectiveComplete(Objective _Objective, float _Progress)
	{
		if (!this.ObjectiveCompletePrefab || !this.ObjectivesNotificationParent || !_Objective)
		{
			return;
		}
		if (_Progress >= 1f && _Objective.NotificationSettings.Frequency == ObjectiveNotificationFrequencies.OnCompleteOnly)
		{
			this.PlayMajorObjectiveComplete(_Objective);
			return;
		}
		if (this.CurrentProgressNotifications.ContainsKey(_Objective))
		{
			if (this.CurrentProgressNotifications[_Objective] != null)
			{
				this.CurrentProgressNotifications[_Objective].Close(true);
			}
			this.CurrentProgressNotifications[_Objective] = UnityEngine.Object.Instantiate<ObjectiveNotification>(this.ObjectiveCompletePrefab, this.ObjectivesNotificationParent);
		}
		else
		{
			this.CurrentProgressNotifications.Add(_Objective, UnityEngine.Object.Instantiate<ObjectiveNotification>(this.ObjectiveCompletePrefab, this.ObjectivesNotificationParent));
		}
		this.CurrentProgressNotifications[_Objective].Play(_Objective, _Progress, new Action<Objective>(this.PlayMajorObjectiveComplete));
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0003A764 File Offset: 0x00038964
	public void ShowPerkUnlocked(CharacterPerk _Perk)
	{
		if (!this.UnlockedPerksQueue.Contains(_Perk))
		{
			this.UnlockedPerksQueue.Add(_Perk);
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0003A780 File Offset: 0x00038980
	private void PlayMajorObjectiveComplete(Objective _Objective)
	{
		if (!this.MajorObjectiveCompletePrefab || !this.MajorObjectiveNotificationParent || !_Objective)
		{
			return;
		}
		this.ObjectivesCompleteFeedbacks.Add(new GraphicsManager.PlayingObjectiveCompletePopup(_Objective));
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x0003A7B8 File Offset: 0x000389B8
	public void PlayCardNotification(InGameCardBase _Card, string _Message)
	{
		if (!this.CardNotificationPrefab || !_Card || string.IsNullOrEmpty(_Message))
		{
			return;
		}
		if (!_Card.IsLiquid && _Card.CardVisuals)
		{
			this.PlayMessage(_Card.CardVisuals.CardNotificationsPos, _Message, _Card.CardVisuals.CardNotificationsTr);
			return;
		}
		if (_Card.CurrentContainer && _Card.CurrentContainer.CardVisuals)
		{
			this.PlayMessage(_Card.CurrentContainer.CardVisuals.CardNotificationsPos, _Message, _Card.CurrentContainer.CardVisuals.CardNotificationsTr);
		}
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x0003A85C File Offset: 0x00038A5C
	public void PlayMessage(Vector3 _Pos, string _Message, Transform _Follow)
	{
		UnityEngine.Object.Instantiate<FollowWithinFrameTextFeedback>(this.CardNotificationPrefab, this.CardNotificationFrame).Play(_Message, _Pos, _Follow, this.CardNotificationFrame);
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x0003A87D File Offset: 0x00038A7D
	public void ScrollToTopOfStats()
	{
		if (this.StatusesScrollRect)
		{
			this.StatusesScrollRect.DOKill(false);
			this.StatusesScrollRect.DOVerticalNormalizedPos(1f, 0.3f, false).SetEase(Ease.InOutSine);
		}
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x0003A8B6 File Offset: 0x00038AB6
	public IEnumerator ActionStatModFeedback(InGameStat _Stat, float _Value, Transform _Parent)
	{
		if (!_Stat.StatModel)
		{
			yield break;
		}
		if (!_Stat.StatModel.ShowActionEffects)
		{
			yield break;
		}
		int amtFeedback = _Stat.StatModel.FeedbackInfo.GetAmtFeedback(_Value);
		if (amtFeedback == 0)
		{
			yield break;
		}
		if (amtFeedback > 0 && !_Stat.StatModel.FeedbackInfo.Icon)
		{
			yield break;
		}
		if (amtFeedback < 0 && !_Stat.StatModel.FeedbackInfo.NegativeIcon)
		{
			yield break;
		}
		UIFeedbackStepsBase uifeedbackStepsBase = UnityEngine.Object.Instantiate<UIFeedbackStepsBase>(_Stat.StatModel.OverrideFeedbackPrefab ? _Stat.StatModel.OverrideFeedbackPrefab : this.DefaultStatModFeedback, _Parent ? _Parent : this.StatModsParent);
		if (!GameManager.DontRenameGOs)
		{
			uifeedbackStepsBase.name = _Stat.name;
		}
		StatStatusGraphics statStatusGraphics = null;
		if (this.TemporaryBarsOnChange && !_Stat.StatModel.StatusesHaveNoBar)
		{
			Sprite fallbackIcon;
			if (_Stat.StatModel.FeedbackInfo.GetAmtFeedback(_Value) >= 0)
			{
				fallbackIcon = _Stat.StatModel.FeedbackInfo.Icon;
			}
			else
			{
				fallbackIcon = _Stat.StatModel.FeedbackInfo.NegativeIcon;
			}
			statStatusGraphics = this.GetTemporaryStatusGraphics(_Stat, fallbackIcon);
			if (statStatusGraphics)
			{
				statStatusGraphics.ActivateTemporaryTimer();
			}
		}
		if (uifeedbackStepsBase is UIFeedbackStepsParticles)
		{
			ParticleSystemForceField[] forceFields = this.DefaultForceFields;
			UIFeedbackStepsParticles uifeedbackStepsParticles = uifeedbackStepsBase as UIFeedbackStepsParticles;
			if (statStatusGraphics)
			{
				forceFields = statStatusGraphics.ForceFields;
			}
			else if (_Stat.CurrentStatuses.Count > 0)
			{
				for (int i = 0; i < _Stat.CurrentStatuses.Count; i++)
				{
					if (_Stat.CurrentStatuses[i].StatusGraphics)
					{
						forceFields = _Stat.CurrentStatuses[i].StatusGraphics.ForceFields;
						if (this.ScrollToStats)
						{
							this.ScrollToStatus(_Stat.CurrentStatuses[i].StatusGraphics);
						}
						if (_Stat.CurrentStatuses[i].StatusGraphics.IsInAlertState)
						{
							break;
						}
					}
				}
			}
			uifeedbackStepsParticles.SetForceFields(forceFields);
			base.StartCoroutine(uifeedbackStepsBase.PlayFeedback(_Stat.StatModel.FeedbackInfo, _Value));
			yield return new WaitForSeconds(uifeedbackStepsBase.YieldWaitDuration);
		}
		yield break;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x0003A8DC File Offset: 0x00038ADC
	private StatStatusGraphics GetTemporaryStatusGraphics(InGameStat _ForStat, Sprite _FallbackIcon)
	{
		if (_ForStat.StatModel.Visibility == StatVisibilityOptions.NeverVisible)
		{
			return null;
		}
		if (!this.TemporaryStatuses.ContainsKey(_ForStat))
		{
			this.TemporaryStatuses.Add(_ForStat, null);
		}
		if (this.TemporaryStatuses[_ForStat])
		{
			return this.TemporaryStatuses[_ForStat];
		}
		this.TemporaryStatuses[_ForStat] = UnityEngine.Object.Instantiate<StatStatusGraphics>(this.StatusGraphicsPrefab, this.TemporaryGraphicsParent);
		StatStatus getDefaultStatus = _ForStat.StatModel.GetDefaultStatus;
		getDefaultStatus.ParentStat = _ForStat;
		if (string.IsNullOrEmpty(getDefaultStatus.GameName))
		{
			getDefaultStatus.GameName = _ForStat.StatModel.GameName;
		}
		StatStatus statStatus = _ForStat.AnyCurrentStatus(true);
		if (statStatus != null)
		{
			getDefaultStatus.Icon = statStatus.Icon;
		}
		else if (!getDefaultStatus.Icon)
		{
			getDefaultStatus.Icon = _FallbackIcon;
		}
		getDefaultStatus.Description = default(LocalizedString);
		this.TemporaryStatuses[_ForStat].Setup(getDefaultStatus, null);
		return this.TemporaryStatuses[_ForStat];
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x0003A9E0 File Offset: 0x00038BE0
	public void PinStat(InGameStat _Stat, bool _Value)
	{
		if (!_Stat)
		{
			return;
		}
		if (_Stat.IsPinned == _Value)
		{
			return;
		}
		_Stat.IsPinned = _Value;
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
		else if (_Stat.DefaultStatus != null)
		{
			if (_Stat.DefaultStatus.StatusGraphics)
			{
				_Stat.DefaultStatus.StatusGraphics.Hide();
			}
			_Stat.DefaultStatus = null;
		}
		this.SortStatuses();
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x0003AABC File Offset: 0x00038CBC
	private StatStatusGraphics GetStatusGraphics(StatStatus _Status, bool _Ascending)
	{
		bool flag = _Status.ParentStat;
		if (flag)
		{
			flag = _Status.ParentStat.StatModel;
		}
		if (flag)
		{
			flag = _Status.ParentStat.StatModel.StatusesHaveNoBar;
		}
		StatStatusGraphics original = flag ? this.NoBarStatusGraphicsPrefab : this.StatusGraphicsPrefab;
		List<StatStatusGraphics> list = flag ? this.AvailableNoBarStatusGraphics : this.AvailableStatusGraphics;
		List<StatStatusGraphics> list2 = flag ? this.CurrentNoBarStatusGraphics : this.CurrentStatusGraphics;
		Transform parent = flag ? this.NoBarStatusGraphicsParent : this.StatusGraphicsParent;
		if (flag && !this.NoBarStatusGraphicsParent.gameObject.activeInHierarchy)
		{
			this.NoBarStatusGraphicsParent.gameObject.SetActive(true);
		}
		if (_Status.AlertLevel != AlertLevels.None && !flag)
		{
			parent = this.ImportantStatusGraphicsParent;
			if (!this.ImportantStatusGraphicsParent.gameObject.activeSelf)
			{
				this.ImportantStatusGraphicsParent.gameObject.SetActive(true);
			}
		}
		StatStatusGraphics statStatusGraphics;
		if (list.Count != 0)
		{
			statStatusGraphics = list[0];
			list.RemoveAt(0);
			statStatusGraphics.transform.SetParent(parent);
		}
		else
		{
			statStatusGraphics = UnityEngine.Object.Instantiate<StatStatusGraphics>(original, parent);
		}
		StatStatusGraphics statStatusGraphics2 = null;
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i].ModelStatus.ParentStat == _Status.ParentStat)
			{
				statStatusGraphics2 = list2[i];
				break;
			}
		}
		bool flag2 = _Status.NotifyPlayer == AlertNotificationTypes.AlwaysNotify || (_Status.NotifyPlayer == AlertNotificationTypes.NotifyWhenGoingUp && _Ascending) || (_Status.NotifyPlayer == AlertNotificationTypes.NotifyWhenGoingDown && !_Ascending);
		if (flag2 || _Status.RepeatTextNotification != NotificationFrequency.Never)
		{
			GameObject gameObject = new GameObject(_Status.GameName + "_TextParent", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(this.StatusAlertsParent);
			gameObject.transform.localScale = Vector3.one;
			statStatusGraphics.Setup(_Status, gameObject.GetComponent<RectTransform>());
		}
		else
		{
			statStatusGraphics.Setup(_Status, null);
		}
		if (flag2)
		{
			this.StatusAlerts.Add(_Status);
			MBSingleton<SoundManager>.Instance.PerformStatusAlertSound(_Status.AlertSounds);
			MBSingleton<SoundManager>.Instance.PerformSingleSound(GraphicsManager.GetStatusAlert(_Status.AlertLevel).SoundEffect, false, false);
		}
		if (!list2.Contains(statStatusGraphics))
		{
			list2.Add(statStatusGraphics);
		}
		if (statStatusGraphics2)
		{
			statStatusGraphics.transform.position = statStatusGraphics2.transform.position;
		}
		this.SortStatuses();
		if (!statStatusGraphics2 && !flag)
		{
			statStatusGraphics.transform.localPosition = statStatusGraphics.GetComponent<UIReorderableListElement>().LocalPosTarget;
		}
		return statStatusGraphics;
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x0003AD58 File Offset: 0x00038F58
	private void SortStatuses()
	{
		this.CurrentStatusGraphics.Sort((StatStatusGraphics a, StatStatusGraphics b) => b.ModelStatus.PriorityScore.CompareTo(a.ModelStatus.PriorityScore));
		this.CurrentNoBarStatusGraphics.Sort((StatStatusGraphics a, StatStatusGraphics b) => b.ModelStatus.PriorityScore.CompareTo(a.ModelStatus.PriorityScore));
		bool flag = false;
		for (int i = this.CurrentStatusGraphics.Count - 1; i >= 0; i--)
		{
			if (!this.CurrentStatusGraphics[i])
			{
				this.CurrentStatusGraphics.RemoveAt(i);
			}
			else
			{
				if (this.CurrentStatusGraphics[i].IsInAlertState)
				{
					this.ImportantStatusGraphicsParent.gameObject.SetActive(true);
					this.CurrentStatusGraphics[i].transform.SetParent(this.ImportantStatusGraphicsParent);
					flag = true;
				}
				else
				{
					this.CurrentStatusGraphics[i].transform.SetParent(this.StatusGraphicsParent);
				}
				this.CurrentStatusGraphics[i].transform.SetAsFirstSibling();
			}
		}
		for (int j = this.CurrentNoBarStatusGraphics.Count - 1; j >= 0; j--)
		{
			if (!this.CurrentNoBarStatusGraphics[j])
			{
				this.CurrentNoBarStatusGraphics.RemoveAt(j);
			}
			else
			{
				this.CurrentNoBarStatusGraphics[j].transform.SetAsFirstSibling();
			}
		}
		this.NoBarStatusGraphicsParent.gameObject.SetActive(this.CurrentNoBarStatusGraphics.Count > 0);
		if (!flag)
		{
			this.ImportantStatusGraphicsParent.gameObject.SetActive(false);
		}
		if (!this.TemporaryBarsOnChange && this.TemporaryGraphicsParent)
		{
			this.TemporaryGraphicsParent.gameObject.SetActive(false);
		}
		this.StatusGraphicsParent.GetComponent<UIReorderableList>().Update();
		this.ImportantStatusGraphicsParent.GetComponent<UIReorderableList>().Update();
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x0003AF38 File Offset: 0x00039138
	public void ScrollToStatus(StatStatusGraphics _Status)
	{
		if (this.CurrentMoveTargetGraphics != null)
		{
			return;
		}
		if (!this.CurrentStatusGraphics.Contains(_Status))
		{
			return;
		}
		Canvas.ForceUpdateCanvases();
		RectTransform viewport = this.StatusesScrollRect.viewport;
		Rect rect = new Rect(viewport.transform.TransformPoint(viewport.rect.position), viewport.transform.TransformVector(viewport.rect.size));
		if (rect.Contains(_Status.transform.position))
		{
			this.StatusesScrollRect.DOKill(false);
			return;
		}
		float num = viewport.rect.height / 2f;
		float endValue = Mathf.InverseLerp(this.StatusesScrollRect.content.rect.height - num, num, Mathf.Abs(this.StatusesScrollRect.content.InverseTransformPoint(_Status.transform.position).y));
		this.CurrentMoveTargetGraphics = _Status;
		this.StatusesScrollRect.DOKill(false);
		this.StatusesScrollRect.DOVerticalNormalizedPos(endValue, 0.3f, false).SetEase(Ease.InOutSine).OnComplete(delegate
		{
			this.CurrentMoveTargetGraphics = null;
		});
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x0003B080 File Offset: 0x00039280
	public static void SetActiveGroup(GameObject[] _Group, bool _Active)
	{
		if (_Group == null)
		{
			return;
		}
		if (_Group.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _Group.Length; i++)
		{
			if (_Group[i])
			{
				_Group[i].SetActive(_Active);
			}
		}
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x0003B0B7 File Offset: 0x000392B7
	public static IEnumerator DeactivateAfterDelay(GameObject _Object, float _Delay)
	{
		if (!_Object)
		{
			yield break;
		}
		yield return new WaitForSeconds(_Delay);
		if (_Object)
		{
			_Object.SetActive(false);
		}
		yield break;
	}

	// Token: 0x040006B3 RID: 1715
	public TextMeshProUGUI DayTimePoints;

	// Token: 0x040006B4 RID: 1716
	public TextMeshProUGUI CurrentDay;

	// Token: 0x040006B5 RID: 1717
	[Header("Time Spent Feedback")]
	public TimeSpentFeedback TimeSpentWheel;

	// Token: 0x040006B6 RID: 1718
	[SerializeField]
	private float TickTimePulseScale = 0.5f;

	// Token: 0x040006B7 RID: 1719
	[SerializeField]
	private float TickTimePulseDuration = 0.3f;

	// Token: 0x040006B8 RID: 1720
	public FadeToBlackScreen FadeToBlack;

	// Token: 0x040006B9 RID: 1721
	public Vector2 TimeSpentTextOffset;

	// Token: 0x040006BA RID: 1722
	public float TickRealTimeDuration;

	// Token: 0x040006BB RID: 1723
	public float CardsWeightPerCircle;

	// Token: 0x040006BC RID: 1724
	public bool DontSwapCards;

	// Token: 0x040006BD RID: 1725
	public GameObject CancelButton;

	// Token: 0x040006BE RID: 1726
	public GameOverMenu GameOver;

	// Token: 0x040006BF RID: 1727
	public VictoryMenu Victory;

	// Token: 0x040006C0 RID: 1728
	public EndgameMenu EndGame;

	// Token: 0x040006C1 RID: 1729
	public CardsDestroyedPopup CardsDestroyed;

	// Token: 0x040006C2 RID: 1730
	public EncounterPopup EncounterPopupWindow;

	// Token: 0x040006C3 RID: 1731
	public Button MenuButton;

	// Token: 0x040006C4 RID: 1732
	public GameObject MenuObject;

	// Token: 0x040006C5 RID: 1733
	[InspectorReadOnly]
	public bool CanOpenMenu = true;

	// Token: 0x040006C6 RID: 1734
	[Space]
	public float SlotsLerpSpeed = 3f;

	// Token: 0x040006C7 RID: 1735
	public RectTransform CardsMovingParent;

	// Token: 0x040006C8 RID: 1736
	public Image LocationsBackground;

	// Token: 0x040006C9 RID: 1737
	public Image BaseBackground;

	// Token: 0x040006CA RID: 1738
	[Header("Lines and scrolling")]
	public float DragScrollMargins;

	// Token: 0x040006CB RID: 1739
	[MinMax]
	public Vector2 MinMaxDragScrollingSpeed;

	// Token: 0x040006CC RID: 1740
	public float ScrollingActionTime = 0.1f;

	// Token: 0x040006CD RID: 1741
	public float InsertingActionTime = 0.2f;

	// Token: 0x040006CE RID: 1742
	public float InsertingActionMargin = 10f;

	// Token: 0x040006CF RID: 1743
	[Header("Slots")]
	public bool DefaultSlotIsHand;

	// Token: 0x040006D0 RID: 1744
	[Space]
	public int MinItemSlots;

	// Token: 0x040006D1 RID: 1745
	public int MaxItemSlots;

	// Token: 0x040006D2 RID: 1746
	public RectTransform ItemsParent;

	// Token: 0x040006D3 RID: 1747
	public SlotSettings ItemSlotSettings;

	// Token: 0x040006D4 RID: 1748
	public CardLine ItemSlotsLine;

	// Token: 0x040006D5 RID: 1749
	public RectTransform ItemCardsParent;

	// Token: 0x040006D6 RID: 1750
	public ScrollRect ItemCardsScrollView;

	// Token: 0x040006D7 RID: 1751
	public DynamicLayoutSlot ItemSlotModel;

	// Token: 0x040006D8 RID: 1752
	[Space]
	public int MinBaseSlots;

	// Token: 0x040006D9 RID: 1753
	public RectTransform BaseParent;

	// Token: 0x040006DA RID: 1754
	public SlotSettings BaseSlotSettings;

	// Token: 0x040006DB RID: 1755
	public CardLine BaseSlotsLine;

	// Token: 0x040006DC RID: 1756
	public RectTransform BaseCardsParent;

	// Token: 0x040006DD RID: 1757
	public ScrollRect BaseCardsScrollView;

	// Token: 0x040006DE RID: 1758
	public DynamicLayoutSlot BaseSlotModel;

	// Token: 0x040006DF RID: 1759
	[Space]
	public int MinLocationSlots;

	// Token: 0x040006E0 RID: 1760
	public RectTransform LocationParent;

	// Token: 0x040006E1 RID: 1761
	public SlotSettings LocationSlotSettings;

	// Token: 0x040006E2 RID: 1762
	public CardLine LocationSlotsLine;

	// Token: 0x040006E3 RID: 1763
	public ScrollRect LocationCardsScrollView;

	// Token: 0x040006E4 RID: 1764
	public DynamicLayoutSlot LocationSlotModel;

	// Token: 0x040006E5 RID: 1765
	[Space]
	public int MinExplorableSlots;

	// Token: 0x040006E6 RID: 1766
	public RectTransform ExplorableParent;

	// Token: 0x040006E7 RID: 1767
	public SlotSettings ExplorableSlotSettings;

	// Token: 0x040006E8 RID: 1768
	public CardLine ExplorableSlotsLine;

	// Token: 0x040006E9 RID: 1769
	public ScrollRect ExplorableCardsScrollView;

	// Token: 0x040006EA RID: 1770
	public DynamicLayoutSlot ExplorableSlotModel;

	// Token: 0x040006EB RID: 1771
	[Space]
	public int MinBlueprintSlots;

	// Token: 0x040006EC RID: 1772
	public RectTransform BlueprintParent;

	// Token: 0x040006ED RID: 1773
	public SlotSettings BlueprintSlotSettings;

	// Token: 0x040006EE RID: 1774
	public CardLine BlueprintSlotsLine;

	// Token: 0x040006EF RID: 1775
	public ScrollRect BlueprintCardsScrollView;

	// Token: 0x040006F0 RID: 1776
	public BlueprintModelsScreen BlueprintModelsPopup;

	// Token: 0x040006F1 RID: 1777
	public bool BlueprintInstanceGoToLocations;

	// Token: 0x040006F2 RID: 1778
	public DynamicLayoutSlot BlueprintSlotModel;

	// Token: 0x040006F3 RID: 1779
	[Space]
	public SlotSettings EnvSlotSettings;

	// Token: 0x040006F4 RID: 1780
	public CardSlot EnvSlotObject;

	// Token: 0x040006F5 RID: 1781
	public DynamicLayoutSlot EnvironmentSlot;

	// Token: 0x040006F6 RID: 1782
	public SlotSettings WeatherSlotSettings;

	// Token: 0x040006F7 RID: 1783
	public CardSlot WeatherSlotObject;

	// Token: 0x040006F8 RID: 1784
	public DynamicLayoutSlot WeatherSlot;

	// Token: 0x040006F9 RID: 1785
	[Header("Inspection")]
	public InspectionPopup CardInspectionPopup;

	// Token: 0x040006FA RID: 1786
	public InspectionPopup InventoryInspectionPopup;

	// Token: 0x040006FB RID: 1787
	public ExplorationPopup ExplorationDeckPopup;

	// Token: 0x040006FC RID: 1788
	public BlueprintConstructionPopup BlueprintPopup;

	// Token: 0x040006FD RID: 1789
	public RectTransform SpecialInspectionPopupParent;

	// Token: 0x040006FE RID: 1790
	public InspectionPopup ActionHighlightPopup;

	// Token: 0x040006FF RID: 1791
	public StatDetailsPopup StatInspection;

	// Token: 0x04000700 RID: 1792
	public DetailedStatList AllStatsList;

	// Token: 0x04000701 RID: 1793
	public GameObject ConfirmActionPopup;

	// Token: 0x04000702 RID: 1794
	public TextMeshProUGUI ConfirmActionName;

	// Token: 0x04000703 RID: 1795
	public BlueprintResearchedPopup BlueprintResearched;

	// Token: 0x04000704 RID: 1796
	public GameObject ResearchingBlueprintIcon;

	// Token: 0x04000705 RID: 1797
	public InspectionPopup CurrentInspectionPopup;

	// Token: 0x04000706 RID: 1798
	[Space]
	public CharacterScreen CharacterWindow;

	// Token: 0x04000707 RID: 1799
	public UIFeedbackText DayChangePopup;

	// Token: 0x04000708 RID: 1800
	public UIFeedback SunEarned;

	// Token: 0x04000709 RID: 1801
	public UIFeedback MoonEarned;

	// Token: 0x0400070A RID: 1802
	[Header("Bookmarks")]
	public BookmarkGraphics[] Bookmarks;

	// Token: 0x0400070B RID: 1803
	public bool FiltersAffectLocationRow;

	// Token: 0x0400070C RID: 1804
	public bool OnlyOneFilterAtATime;

	// Token: 0x0400070D RID: 1805
	public Image[] FilterBackgroundImages;

	// Token: 0x0400070E RID: 1806
	[Header("Statuses")]
	public StatStatusGraphics StatusGraphicsPrefab;

	// Token: 0x0400070F RID: 1807
	public StatStatusGraphics NoBarStatusGraphicsPrefab;

	// Token: 0x04000710 RID: 1808
	public RectTransform NoBarStatusGraphicsParent;

	// Token: 0x04000711 RID: 1809
	public RectTransform ImportantStatusGraphicsParent;

	// Token: 0x04000712 RID: 1810
	public RectTransform StatusGraphicsParent;

	// Token: 0x04000713 RID: 1811
	public bool TemporaryBarsOnChange;

	// Token: 0x04000714 RID: 1812
	public RectTransform TemporaryGraphicsParent;

	// Token: 0x04000715 RID: 1813
	public RectTransform TemporaryGraphicsMovingParent;

	// Token: 0x04000716 RID: 1814
	public UIFeedbackTextAndIcon StatusAlertTextPrefab;

	// Token: 0x04000717 RID: 1815
	public bool StatusAlertMoves;

	// Token: 0x04000718 RID: 1816
	public RectTransform StatusAlertsParent;

	// Token: 0x04000719 RID: 1817
	public bool ScrollToStats;

	// Token: 0x0400071A RID: 1818
	public ScrollRect StatusesScrollRect;

	// Token: 0x0400071B RID: 1819
	[Space]
	[SerializeField]
	private Vector2 RarelyNotifyDelay;

	// Token: 0x0400071C RID: 1820
	[SerializeField]
	private Vector2 SometimesNotifyDelay;

	// Token: 0x0400071D RID: 1821
	[SerializeField]
	private Vector2 OftenNotifyDelay;

	// Token: 0x0400071E RID: 1822
	[SerializeField]
	private Vector2 ConstantlyModifyDelay;

	// Token: 0x0400071F RID: 1823
	[Space]
	[SerializeField]
	private StatusAlertSettings LowAlertSettings;

	// Token: 0x04000720 RID: 1824
	[SerializeField]
	private StatusAlertSettings ModerateAlertSettings;

	// Token: 0x04000721 RID: 1825
	[SerializeField]
	private StatusAlertSettings HighAlertSettings;

	// Token: 0x04000722 RID: 1826
	[SerializeField]
	private StatusAlertSettings CriticalAlertSettings;

	// Token: 0x04000723 RID: 1827
	[SerializeField]
	private StatusAlertSettings TerminalAlertSettings;

	// Token: 0x04000724 RID: 1828
	[SerializeField]
	private AnimationCurve PulseCurve;

	// Token: 0x04000725 RID: 1829
	private List<StatStatus> StatusAlerts = new List<StatStatus>();

	// Token: 0x04000726 RID: 1830
	public Dictionary<InGameStat, StatStatusGraphics> TemporaryStatuses = new Dictionary<InGameStat, StatStatusGraphics>();

	// Token: 0x04000727 RID: 1831
	[Space]
	public float StatusBarChangeAnimationDelay;

	// Token: 0x04000728 RID: 1832
	public float StatusBarChangeAnimationSpeed;

	// Token: 0x04000729 RID: 1833
	public Color NegativeChangeStatusBarColor;

	// Token: 0x0400072A RID: 1834
	[SerializeField]
	private TrendIndicatorAnimSettings LowTrendAnim;

	// Token: 0x0400072B RID: 1835
	[SerializeField]
	private TrendIndicatorAnimSettings MediumTrendAnim;

	// Token: 0x0400072C RID: 1836
	[SerializeField]
	private TrendIndicatorAnimSettings HighTrendAnim;

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private AnimationCurve TrendAnimCurve;

	// Token: 0x0400072E RID: 1838
	private float FreqTime;

	// Token: 0x0400072F RID: 1839
	private float LowAlertPulseFreq;

	// Token: 0x04000730 RID: 1840
	private float LowAlertOutlinePulseFreq;

	// Token: 0x04000731 RID: 1841
	private float LowAlertOutlineBlinkFreq;

	// Token: 0x04000732 RID: 1842
	private float ModerateAlertPulseFreq;

	// Token: 0x04000733 RID: 1843
	private float ModerateAlertOutlinePulseFreq;

	// Token: 0x04000734 RID: 1844
	private float ModerateAlertOutlineBlinkFreq;

	// Token: 0x04000735 RID: 1845
	private float HighAlertPulseFreq;

	// Token: 0x04000736 RID: 1846
	private float HighAlertOutlinePulseFreq;

	// Token: 0x04000737 RID: 1847
	private float HighAlertOutlineBlinkFreq;

	// Token: 0x04000738 RID: 1848
	private float CriticalAlertPulseFreq;

	// Token: 0x04000739 RID: 1849
	private float CriticalAlertOutlinePulseFreq;

	// Token: 0x0400073A RID: 1850
	private float CriticalAlertOutlineBlinkFreq;

	// Token: 0x0400073B RID: 1851
	private float TerminalAlertPulseFreq;

	// Token: 0x0400073C RID: 1852
	private float TerminalAlertOutlinePulseFreq;

	// Token: 0x0400073D RID: 1853
	private float TerminalAlertOutlineBlinkFreq;

	// Token: 0x0400073E RID: 1854
	private float LowTrendAnimFreq;

	// Token: 0x0400073F RID: 1855
	private float MediumTrendAnimFreq;

	// Token: 0x04000740 RID: 1856
	private float HighTrendAnimFreq;

	// Token: 0x04000741 RID: 1857
	private StatStatusGraphics CurrentMoveTargetGraphics;

	// Token: 0x04000742 RID: 1858
	[Header("Action Stat Modifiers Feedbacks")]
	public UIFeedbackStepsBase DefaultStatModFeedback;

	// Token: 0x04000743 RID: 1859
	public RectTransform StatModsParent;

	// Token: 0x04000744 RID: 1860
	public ParticleSystemForceField[] DefaultForceFields;

	// Token: 0x04000745 RID: 1861
	[Header("Text Feedbacks")]
	public RectTransform TextsParent;

	// Token: 0x04000746 RID: 1862
	public UIFeedbackText DaylightSpendingTextPrefab;

	// Token: 0x04000747 RID: 1863
	public Color PositiveTextColor;

	// Token: 0x04000748 RID: 1864
	public Color NegativeTextColor;

	// Token: 0x04000749 RID: 1865
	public UIFeedbackText CannotInspectTextPrefab;

	// Token: 0x0400074A RID: 1866
	public RectTransform ObjectivesNotificationParent;

	// Token: 0x0400074B RID: 1867
	public ObjectiveNotification ObjectiveCompletePrefab;

	// Token: 0x0400074C RID: 1868
	public RectTransform MajorObjectiveNotificationParent;

	// Token: 0x0400074D RID: 1869
	public ObjectiveCompletePopup MajorObjectiveCompletePrefab;

	// Token: 0x0400074E RID: 1870
	public FollowWithinFrameTextFeedback CardNotificationPrefab;

	// Token: 0x0400074F RID: 1871
	public RectTransform CardNotificationFrame;

	// Token: 0x04000750 RID: 1872
	public RectTransform JournalButtonTr;

	// Token: 0x04000751 RID: 1873
	public RectTransform BlueprintNotificationParent;

	// Token: 0x04000752 RID: 1874
	public BlueprintUnlockedPopup BlueprintUnlockedPrefab;

	// Token: 0x04000753 RID: 1875
	public BlueprintUnlockedPopup DamageSpawnedPrefab;

	// Token: 0x04000754 RID: 1876
	public BlueprintUnlockedPopup WoundReceivedPrefab;

	// Token: 0x04000755 RID: 1877
	public RectTransform BlueprintsButtonTr;

	// Token: 0x04000756 RID: 1878
	public RectTransform CharacterButtonTr;

	// Token: 0x04000757 RID: 1879
	public RectTransform UnlockPopupsParent;

	// Token: 0x04000758 RID: 1880
	public PerkUnlockPopup PerkUnlocks;

	// Token: 0x04000759 RID: 1881
	public GameObject SaveErrorPopup;

	// Token: 0x0400075A RID: 1882
	private Dictionary<Objective, ObjectiveNotification> CurrentProgressNotifications = new Dictionary<Objective, ObjectiveNotification>();

	// Token: 0x0400075B RID: 1883
	private bool DoSlotsRefresh;

	// Token: 0x0400075C RID: 1884
	[NonSerialized]
	public List<BookmarkGroup> AllBookmarkGroups = new List<BookmarkGroup>();

	// Token: 0x0400075D RID: 1885
	[NonSerialized]
	public List<CardFilterGroup> CurrentFilterTags = new List<CardFilterGroup>();

	// Token: 0x0400075E RID: 1886
	private List<InGameStat> VisibleStats = new List<InGameStat>();

	// Token: 0x0400075F RID: 1887
	public static TMP_InputField CurrentTypingInput;

	// Token: 0x04000760 RID: 1888
	private List<GraphicsManager.PlayingObjectiveCompletePopup> ObjectivesCompleteFeedbacks = new List<GraphicsManager.PlayingObjectiveCompletePopup>();

	// Token: 0x04000761 RID: 1889
	private List<GraphicsManager.PlayingBlueprintUnlockedPopup> BlueprintsUnlockedFeedbacks = new List<GraphicsManager.PlayingBlueprintUnlockedPopup>();

	// Token: 0x04000762 RID: 1890
	private List<GraphicsManager.PlayingBlueprintUnlockedPopup> WoundsReceivedFeedbacks = new List<GraphicsManager.PlayingBlueprintUnlockedPopup>();

	// Token: 0x04000763 RID: 1891
	private bool ObjectiveCompletedFeedbackIsPlaying;

	// Token: 0x04000764 RID: 1892
	private bool WoundReceivedFeedbackIsPlaying;

	// Token: 0x04000765 RID: 1893
	private bool BlueprintFeedbackIsPlaying;

	// Token: 0x04000766 RID: 1894
	private List<CharacterPerk> UnlockedPerksQueue = new List<CharacterPerk>();

	// Token: 0x04000767 RID: 1895
	private List<StatStatusGraphics> AvailableStatusGraphics = new List<StatStatusGraphics>();

	// Token: 0x04000768 RID: 1896
	private List<StatStatusGraphics> CurrentStatusGraphics = new List<StatStatusGraphics>();

	// Token: 0x04000769 RID: 1897
	private List<StatStatusGraphics> AvailableNoBarStatusGraphics = new List<StatStatusGraphics>();

	// Token: 0x0400076A RID: 1898
	private List<StatStatusGraphics> CurrentNoBarStatusGraphics = new List<StatStatusGraphics>();

	// Token: 0x0400076B RID: 1899
	private UIFeedbackText CurrentDaylightSpendingText;

	// Token: 0x0400076C RID: 1900
	private float LastDaytimeChange;

	// Token: 0x0400076D RID: 1901
	private bool LoadingScreen;

	// Token: 0x0400076E RID: 1902
	private Transform SlotPoolsParent;

	// Token: 0x0400076F RID: 1903
	private GameManager GM;

	// Token: 0x04000770 RID: 1904
	private int CurrentDaytimePoints;

	// Token: 0x04000771 RID: 1905
	private int CurrentMiniTicks;

	// Token: 0x04000772 RID: 1906
	private int PrevDaytimePoints = -1;

	// Token: 0x04000773 RID: 1907
	private int PrevMiniTicks = -1;

	// Token: 0x04000774 RID: 1908
	private Sequence DayTimePointsUpdate;

	// Token: 0x04000775 RID: 1909
	private const string ItemSlotName = "ItemSlot_";

	// Token: 0x04000776 RID: 1910
	private const string BaseSlotName = "BaseSlot_";

	// Token: 0x04000777 RID: 1911
	private const string LocationSlotName = "LocationSlot_";

	// Token: 0x04000778 RID: 1912
	private const string ExplorableSlotName = "ExplorableSlot_";

	// Token: 0x04000779 RID: 1913
	private const string BlueprintSlotName = "BlueprintSlot_";

	// Token: 0x0400077A RID: 1914
	private const string ItemPoolName = "ItemPool_";

	// Token: 0x0400077B RID: 1915
	private const string BasePoolName = "BasePool_";

	// Token: 0x0400077C RID: 1916
	private const string LocationPoolName = "LocationPool_";

	// Token: 0x0400077D RID: 1917
	private const string ExplorablePoolName = "ExplorablePool_";

	// Token: 0x0400077E RID: 1918
	private const string BlueprintPoolName = "BlueprintPool_";

	// Token: 0x04000780 RID: 1920
	private bool EventResolved;

	// Token: 0x0200025E RID: 606
	private class PlayingObjectiveCompletePopup
	{
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0007F323 File Offset: 0x0007D523
		// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0007F32B File Offset: 0x0007D52B
		public bool IsPlaying { get; private set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0007F334 File Offset: 0x0007D534
		public bool IsFinished
		{
			get
			{
				return this.IsPlaying && !this.Popup;
			}
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0007F34E File Offset: 0x0007D54E
		public PlayingObjectiveCompletePopup(Objective _Obj)
		{
			this.AssociatedObjective = _Obj;
			this.Popup = null;
			this.IsPlaying = false;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0007F36B File Offset: 0x0007D56B
		public void Play(ObjectiveCompletePopup _Prefab, Transform _Parent, Transform _JournalButton)
		{
			this.Popup = UnityEngine.Object.Instantiate<ObjectiveCompletePopup>(_Prefab, _Parent);
			this.Popup.Play(this.AssociatedObjective, _JournalButton);
			this.IsPlaying = true;
		}

		// Token: 0x04001441 RID: 5185
		public Objective AssociatedObjective;

		// Token: 0x04001442 RID: 5186
		public ObjectiveCompletePopup Popup;
	}

	// Token: 0x0200025F RID: 607
	private class PlayingBlueprintUnlockedPopup
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0007F393 File Offset: 0x0007D593
		// (set) Token: 0x06000F78 RID: 3960 RVA: 0x0007F39B File Offset: 0x0007D59B
		public bool IsPlaying { get; private set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0007F3A4 File Offset: 0x0007D5A4
		public bool IsFinished
		{
			get
			{
				return this.IsPlaying && this.Popup == null;
			}
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0007F3BC File Offset: 0x0007D5BC
		public PlayingBlueprintUnlockedPopup(CardData _Blueprint)
		{
			this.AssociatedBlueprint = _Blueprint;
			this.Popup = null;
			this.IsPlaying = false;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0007F3D9 File Offset: 0x0007D5D9
		public void Play(BlueprintUnlockedPopup _Prefab, Transform _Parent, Transform _BlueprintButton)
		{
			this.Popup = UnityEngine.Object.Instantiate<BlueprintUnlockedPopup>(_Prefab, _Parent);
			this.Popup.Play(this.AssociatedBlueprint, _BlueprintButton);
			this.IsPlaying = true;
		}

		// Token: 0x04001444 RID: 5188
		public CardData AssociatedBlueprint;

		// Token: 0x04001445 RID: 5189
		public BlueprintUnlockedPopup Popup;
	}
}
