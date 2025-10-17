using System;
using UnityEngine;

// Token: 0x02000159 RID: 345
[Serializable]
public struct WeatherColors
{
	// Token: 0x06000996 RID: 2454 RVA: 0x00058C63 File Offset: 0x00056E63
	public bool IsSameAs(WeatherColors _Other)
	{
		return this.MainColor == _Other.MainColor && this.TimeRange == _Other.TimeRange && this.LightSourceAllowed == _Other.LightSourceAllowed;
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00058C9C File Offset: 0x00056E9C
	public bool IsInRange(float _Hour)
	{
		if (this.TimeRange.x > this.TimeRange.y)
		{
			return _Hour >= (float)this.TimeRange.x || _Hour < (float)this.TimeRange.y;
		}
		return _Hour >= (float)this.TimeRange.x && _Hour < (float)this.TimeRange.y;
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00058D04 File Offset: 0x00056F04
	public static WeatherColors Lerp(WeatherColors _A, WeatherColors _B, float _T)
	{
		return new WeatherColors
		{
			MainColor = Color.Lerp(_A.MainColor, _B.MainColor, _T)
		};
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000999 RID: 2457 RVA: 0x00058D34 File Offset: 0x00056F34
	public static WeatherColors Default
	{
		get
		{
			return new WeatherColors
			{
				ColorName = "Default",
				TimeRange = new Vector2Int(0, 23),
				MainColor = Color.white
			};
		}
	}

	// Token: 0x04000F23 RID: 3875
	public string ColorName;

	// Token: 0x04000F24 RID: 3876
	public Vector2Int TimeRange;

	// Token: 0x04000F25 RID: 3877
	public Color MainColor;

	// Token: 0x04000F26 RID: 3878
	public bool LightSourceAllowed;
}
