using System;
using UnityEngine;

// Token: 0x02000158 RID: 344
[CreateAssetMenu(menuName = "Survival/WeatherSet")]
public class WeatherSet : ScriptableObject
{
	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000994 RID: 2452 RVA: 0x00058BEC File Offset: 0x00056DEC
	public WeatherColors CurrentColors
	{
		get
		{
			GameManager instance = MBSingleton<GameManager>.Instance;
			float hour = instance ? Mathf.Repeat(GameManager.HourOfTheDayValue(instance.DayTimePoints, instance.CurrentMiniTicks), 24f) : 0f;
			for (int i = 0; i < this.ColorsOfTheDay.Length; i++)
			{
				if (this.ColorsOfTheDay[i].IsInRange(hour))
				{
					return this.ColorsOfTheDay[i];
				}
			}
			return WeatherColors.Default;
		}
	}

	// Token: 0x04000F21 RID: 3873
	public WeatherColors[] ColorsOfTheDay;

	// Token: 0x04000F22 RID: 3874
	public WeatherSpecialEffect[] EffectsToSpawn;
}
