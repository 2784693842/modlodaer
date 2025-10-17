using System;

// Token: 0x020001D8 RID: 472
public class MissingStatInfo
{
	// Token: 0x06000CA5 RID: 3237 RVA: 0x0006781F File Offset: 0x00065A1F
	public MissingStatInfo(GameStat _Stat, bool _TooMuch)
	{
		this.Stat = _Stat;
		this.TooMuch = _TooMuch;
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00067835 File Offset: 0x00065A35
	public string GetNotification
	{
		get
		{
			if (!this.Stat)
			{
				return "";
			}
			return this.Stat.StatMissingText(this.TooMuch);
		}
	}

	// Token: 0x04001197 RID: 4503
	public GameStat Stat;

	// Token: 0x04001198 RID: 4504
	public bool TooMuch;
}
