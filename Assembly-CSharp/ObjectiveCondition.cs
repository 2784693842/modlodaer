using System;

// Token: 0x0200010E RID: 270
[Serializable]
public struct ObjectiveCondition
{
	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00053E1B File Offset: 0x0005201B
	public bool Complete
	{
		get
		{
			if (this.Inverted)
			{
				return !this.TargetObjective.Complete;
			}
			return this.TargetObjective.Complete;
		}
	}

	// Token: 0x04000D0C RID: 3340
	public Objective TargetObjective;

	// Token: 0x04000D0D RID: 3341
	public bool Inverted;
}
