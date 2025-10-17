using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
[Serializable]
public class StatSubObjective : SubObjective
{
	// Token: 0x060008A9 RID: 2217 RVA: 0x00053BB0 File Offset: 0x00051DB0
	public override void Init()
	{
		base.Init();
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00053BB8 File Offset: 0x00051DB8
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
		if (this.StatCondition.Stat)
		{
			base.Complete = this.StatCondition.IsInRange(MBSingleton<GameManager>.Instance.StatsDict[this.StatCondition.Stat].CurrentValue(MBSingleton<GameManager>.Instance.NotInBase));
		}
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00053C30 File Offset: 0x00051E30
	public override float GetCompletion()
	{
		if (base.Complete)
		{
			return base.GetCompletion();
		}
		if (!this.StatCondition.Stat)
		{
			return base.GetCompletion();
		}
		if (!MBSingleton<GameManager>.Instance)
		{
			return base.GetCompletion();
		}
		float num = MBSingleton<GameManager>.Instance.StatsDict[this.StatCondition.Stat].CurrentValue(MBSingleton<GameManager>.Instance.NotInBase);
		if (num > this.StatCondition.TriggerRange.y)
		{
			return Mathf.InverseLerp(this.StatCondition.Stat.MinMaxValue.y, this.StatCondition.TriggerRange.y, num) * (float)this.CompletionWeight;
		}
		if (num < this.StatCondition.TriggerRange.x)
		{
			return Mathf.InverseLerp(this.StatCondition.Stat.MinMaxValue.x, this.StatCondition.TriggerRange.x, num) * (float)this.CompletionWeight;
		}
		return base.GetCompletion();
	}

	// Token: 0x04000CFB RID: 3323
	public StatValueTrigger StatCondition;

	// Token: 0x04000CFC RID: 3324
	public bool ConstantlyChecking;
}
