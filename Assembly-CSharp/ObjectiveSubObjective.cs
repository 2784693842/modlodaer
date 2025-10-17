using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
[Serializable]
public class ObjectiveSubObjective : SubObjective
{
	// Token: 0x060008AF RID: 2223 RVA: 0x00053DE5 File Offset: 0x00051FE5
	public void CheckForCompletion(bool _Init)
	{
		if (base.Complete)
		{
			return;
		}
		if (!this.ObjectiveCondition || _Init)
		{
			base.Complete = false;
			return;
		}
		base.Complete = this.ObjectiveCondition.Complete;
	}

	// Token: 0x04000D01 RID: 3329
	[SerializeField]
	private Objective ObjectiveCondition;
}
