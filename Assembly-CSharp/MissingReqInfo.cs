using System;

// Token: 0x0200004C RID: 76
public class MissingReqInfo
{
	// Token: 0x06000318 RID: 792 RVA: 0x0001F4FD File Offset: 0x0001D6FD
	public MissingReqInfo(MissingStatInfo _Stat, string _Durabilities, string _BlockingStatus, bool _ActionPlaying, bool _UnavailableInDemo)
	{
		this.Stat = _Stat;
		this.MissingDurabilities = _Durabilities;
		this.BlockingStatus = _BlockingStatus;
		this.ActionPlaying = _ActionPlaying;
		this.UnavailableInDemo = _UnavailableInDemo;
	}

	// Token: 0x040003B7 RID: 951
	public MissingStatInfo Stat;

	// Token: 0x040003B8 RID: 952
	public string MissingDurabilities;

	// Token: 0x040003B9 RID: 953
	public string BlockingStatus;

	// Token: 0x040003BA RID: 954
	public bool ActionPlaying;

	// Token: 0x040003BB RID: 955
	public bool UnavailableInDemo;
}
