using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
[Serializable]
public struct PlayerEncounterVariable
{
	// Token: 0x06000486 RID: 1158 RVA: 0x0002F2D0 File Offset: 0x0002D4D0
	public float GenerateValue(bool _WithRandomness)
	{
		if (!this.Stat)
		{
			return 0f;
		}
		if (!MBSingleton<GameManager>.Instance.StatsDict.ContainsKey(this.Stat))
		{
			return 0f;
		}
		return this.AddedValue.GenerateValue(MBSingleton<GameManager>.Instance.StatsDict[this.Stat].SimpleCurrentValue, _WithRandomness);
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0002F334 File Offset: 0x0002D534
	public Vector2 GenerateRandomRange()
	{
		if (!this.Stat)
		{
			return Vector2.zero;
		}
		if (!MBSingleton<GameManager>.Instance.StatsDict.ContainsKey(this.Stat))
		{
			return Vector2.zero;
		}
		return this.AddedValue.GenerateRandomRange(MBSingleton<GameManager>.Instance.StatsDict[this.Stat].SimpleCurrentValue);
	}

	// Token: 0x040005A4 RID: 1444
	public GameStat Stat;

	// Token: 0x040005A5 RID: 1445
	[SerializeField]
	private EncounterVariable AddedValue;
}
