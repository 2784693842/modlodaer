using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
[Serializable]
public class TimeObjective : SubObjective
{
	// Token: 0x060008AD RID: 2221 RVA: 0x00053D38 File Offset: 0x00051F38
	public void CheckForCompletion()
	{
		if (base.Complete && !this.ConstantlyChecking)
		{
			return;
		}
		if (!MBSingleton<GameManager>.Instance)
		{
			return;
		}
		int a = 0;
		float num = GameManager.TickToHours(MBSingleton<GameManager>.Instance.CurrentTickInfo.y, MBSingleton<GameManager>.Instance.CurrentMiniTicks);
		switch (this.TimeType)
		{
		case TimeValueTypes.Day:
			a = MBSingleton<GameManager>.Instance.CurrentDay;
			break;
		case TimeValueTypes.Hour:
			a = Mathf.FloorToInt(num);
			break;
		case TimeValueTypes.Minute:
			a = Mathf.FloorToInt(60f * (num - (float)Mathf.FloorToInt(num)));
			break;
		}
		base.Complete = SimpleComparison.ValidComparison(this.CompareType, a, this.Value);
	}

	// Token: 0x04000CFD RID: 3325
	[SerializeField]
	private TimeValueTypes TimeType;

	// Token: 0x04000CFE RID: 3326
	[SerializeField]
	private SimpleComparison.Comparisons CompareType;

	// Token: 0x04000CFF RID: 3327
	[SerializeField]
	private int Value;

	// Token: 0x04000D00 RID: 3328
	[SerializeField]
	private bool ConstantlyChecking;
}
