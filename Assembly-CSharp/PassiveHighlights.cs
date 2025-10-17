using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[Serializable]
public class PassiveHighlights
{
	// Token: 0x040002EB RID: 747
	public string HighlightName;

	// Token: 0x040002EC RID: 748
	public GeneralCondition Conditions;

	// Token: 0x040002ED RID: 749
	public ActionHighlight[] HighlightedActions;

	// Token: 0x040002EE RID: 750
	public GameObject[] HighlightedObjects;

	// Token: 0x040002EF RID: 751
	public bool Inactive;
}
