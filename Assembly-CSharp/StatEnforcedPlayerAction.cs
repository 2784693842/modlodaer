using System;

// Token: 0x020001E2 RID: 482
[Serializable]
public struct StatEnforcedPlayerAction
{
	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0006851C File Offset: 0x0006671C
	public bool ConditionsAreMet
	{
		get
		{
			if (this.Conditions == null)
			{
				return false;
			}
			if (this.Conditions.Length == 0)
			{
				return false;
			}
			GameManager instance = MBSingleton<GameManager>.Instance;
			if (!instance)
			{
				return false;
			}
			for (int i = 0; i < this.Conditions.Length; i++)
			{
				if (instance.StatsDict.ContainsKey(this.Conditions[i].Stat) && !this.Conditions[i].IsInRange(instance.StatsDict[this.Conditions[i].Stat].SimpleCurrentValue))
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x040011B1 RID: 4529
	public StatValueTrigger[] Conditions;

	// Token: 0x040011B2 RID: 4530
	public EncounterLogMessage LogMessage;

	// Token: 0x040011B3 RID: 4531
	public GenericEncounterPlayerAction Action;
}
