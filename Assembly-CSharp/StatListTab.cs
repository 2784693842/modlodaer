using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000156 RID: 342
[CreateAssetMenu(menuName = "Survival/Stats Tab")]
public class StatListTab : ScriptableObject
{
	// Token: 0x04000F1A RID: 3866
	public LocalizedString TabName;

	// Token: 0x04000F1B RID: 3867
	public Sprite Icon;

	// Token: 0x04000F1C RID: 3868
	public List<GameStat> ContainedStats;
}
