using System;

// Token: 0x02000031 RID: 49
public class TimeOfDayStatModSource
{
	// Token: 0x06000246 RID: 582 RVA: 0x000172B6 File Offset: 0x000154B6
	public TimeOfDayStatModSource(string _Start, string _End)
	{
		this.EffectStartingTime = _Start;
		this.EffectEndTime = _End;
	}

	// Token: 0x06000247 RID: 583 RVA: 0x000172CC File Offset: 0x000154CC
	public static bool IdenticalTimes(TimeOfDayStatModSource _A, TimeOfDayStatModSource _B)
	{
		if (_A == null && _B == null)
		{
			return true;
		}
		if (_A != null)
		{
			return _A.IsIdentical(_B);
		}
		return _B.IsIdentical(_A);
	}

	// Token: 0x06000248 RID: 584 RVA: 0x000172E8 File Offset: 0x000154E8
	public bool IsIdentical(TimeOfDayStatModSource _To)
	{
		return _To != null && _To.EffectStartingTime == this.EffectStartingTime && _To.EffectEndTime == this.EffectEndTime;
	}

	// Token: 0x0400027C RID: 636
	public string EffectStartingTime;

	// Token: 0x0400027D RID: 637
	public string EffectEndTime;
}
