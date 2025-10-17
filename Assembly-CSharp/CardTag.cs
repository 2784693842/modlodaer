using System;
using UnityEngine;

// Token: 0x02000101 RID: 257
[CreateAssetMenu(fileName = "New Card Tag", menuName = "Survival/Card Tag")]
public class CardTag : ScriptableObject
{
	// Token: 0x04000CB3 RID: 3251
	public LocalizedString InGameName;

	// Token: 0x04000CB4 RID: 3252
	public bool UniqueOnBoard;

	// Token: 0x04000CB5 RID: 3253
	public ActionModifier[] ActionModifiers;
}
