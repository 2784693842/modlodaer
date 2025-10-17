using System;
using UnityEngine;

// Token: 0x02000155 RID: 341
[CreateAssetMenu(menuName = "Survival/SpecialActionSet")]
public class SpecialActionSet : ScriptableObject
{
	// Token: 0x04000F17 RID: 3863
	public LocalizedString SetName;

	// Token: 0x04000F18 RID: 3864
	public LocalizedString Description;

	// Token: 0x04000F19 RID: 3865
	public DismantleCardAction[] Actions;
}
