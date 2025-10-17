using System;

// Token: 0x0200012C RID: 300
[Serializable]
public struct LogSaveData
{
	// Token: 0x060008EF RID: 2287 RVA: 0x0005581B File Offset: 0x00053A1B
	public LogSaveData(EndgameLog _Log, int _Tick)
	{
		this.LogText = _Log.LogText;
		this.CategoryID = UniqueIDScriptable.SaveID(_Log.Category);
		this.LoggedOnTick = _Tick;
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00055846 File Offset: 0x00053A46
	public string GetText
	{
		get
		{
			return "• " + GameManager.TotalTicksToHourOfTheDayString(this.TicksWithinCurrentDay(this.LoggedOnTick), 0) + " - " + this.LogText;
		}
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00055870 File Offset: 0x00053A70
	private int TicksWithinCurrentDay(int _Ticks)
	{
		if (!MBSingleton<GameManager>.Instance)
		{
			return _Ticks;
		}
		if (_Ticks < MBSingleton<GameManager>.Instance.DaySettings.DailyPoints)
		{
			return GameManager.HoursToTick((float)MBSingleton<GameManager>.Instance.DaySettings.DayStartingHour) + _Ticks;
		}
		int num = _Ticks / MBSingleton<GameManager>.Instance.DaySettings.DailyPoints;
		return GameManager.HoursToTick((float)MBSingleton<GameManager>.Instance.DaySettings.DayStartingHour) + _Ticks - num * MBSingleton<GameManager>.Instance.DaySettings.DailyPoints;
	}

	// Token: 0x04000E19 RID: 3609
	public string LogText;

	// Token: 0x04000E1A RID: 3610
	public string CategoryID;

	// Token: 0x04000E1B RID: 3611
	public int LoggedOnTick;
}
