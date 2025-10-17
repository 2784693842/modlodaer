using System;
using System.Text;
using UnityEngine;

// Token: 0x020001DD RID: 477
public static class HoursDisplay
{
	// Token: 0x06000CBA RID: 3258 RVA: 0x000681D8 File Offset: 0x000663D8
	public static string HoursToTimeOfDay(float _Hours)
	{
		int num = Mathf.FloorToInt(_Hours / 24f);
		int num2 = Mathf.FloorToInt(_Hours - (float)(num * 24));
		int num3 = Mathf.FloorToInt((_Hours - (float)(num * 24) - (float)num2) * 60f + 0.0001f);
		return string.Format("{0:D2}:{1:D2}", num2, num3);
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00068234 File Offset: 0x00066434
	public static string HoursToShortString(float _Hours)
	{
		if (Mathf.Abs(_Hours) / 24f > 0.99f)
		{
			return ((float)Mathf.CeilToInt(Mathf.Abs(_Hours) / 24f) * Mathf.Sign(_Hours)).ToString(LocalizedString.DayFormat);
		}
		if (Mathf.Abs(_Hours) > 0.99f)
		{
			return ((float)Mathf.CeilToInt(Mathf.Abs(_Hours)) * Mathf.Sign(_Hours)).ToString(LocalizedString.HourFormat);
		}
		return ((float)Mathf.FloorToInt(Mathf.Abs(_Hours * 60f)) * Mathf.Sign(_Hours)).ToString(LocalizedString.MinuteFormat);
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x000682E0 File Offset: 0x000664E0
	public static string HoursToCompleteString(float _Hours)
	{
		int num = Mathf.FloorToInt(Mathf.Abs(_Hours) / 24f);
		int num2 = Mathf.FloorToInt(Mathf.Abs(_Hours));
		int num3 = Mathf.FloorToInt((_Hours - (float)num2) * 60f + 0.0001f);
		StringBuilder stringBuilder = new StringBuilder();
		if (Mathf.Sign(_Hours) < 0f)
		{
			stringBuilder.Append("-");
		}
		if (num >= 1)
		{
			stringBuilder.Append(num.ToString(LocalizedString.DayFormat));
		}
		if (num2 >= 1 || (num3 > 0 && num >= 1))
		{
			stringBuilder.Append(num2.ToString(LocalizedString.HourFormat));
		}
		if (num3 > 0)
		{
			stringBuilder.Append(num3.ToString(LocalizedString.MinuteFormat));
		}
		return stringBuilder.ToString();
	}
}
