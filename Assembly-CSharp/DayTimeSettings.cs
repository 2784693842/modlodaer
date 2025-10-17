using System;

// Token: 0x0200000B RID: 11
[Serializable]
public struct DayTimeSettings
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000B90B File Offset: 0x00009B0B
	public float PointToHours
	{
		get
		{
			return 24f / (float)this.DailyPoints;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000B91A File Offset: 0x00009B1A
	public float MiniTicksToHours
	{
		get
		{
			if (this.MiniTicksPerTick <= 0)
			{
				return 0f;
			}
			return 24f / ((float)this.DailyPoints / (1f / (float)this.MiniTicksPerTick));
		}
	}

	// Token: 0x04000101 RID: 257
	public int DailyPoints;

	// Token: 0x04000102 RID: 258
	public int DayStartingHour;

	// Token: 0x04000103 RID: 259
	public bool UseMiniTicks;

	// Token: 0x04000104 RID: 260
	public int MiniTicksPerTick;

	// Token: 0x04000105 RID: 261
	public MiniTicksBehavior DefaultMiniTicksBehavior;
}
