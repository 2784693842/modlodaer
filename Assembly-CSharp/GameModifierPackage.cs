using System;
using UnityEngine;

// Token: 0x02000131 RID: 305
[CreateAssetMenu(menuName = "Survival/Modifier Package")]
public class GameModifierPackage : UniqueIDScriptable
{
	// Token: 0x04000E29 RID: 3625
	public LocalizedString PackageName;

	// Token: 0x04000E2A RID: 3626
	public LocalizedString PackageDescription;

	// Token: 0x04000E2B RID: 3627
	public CardData[] AddedCards;

	// Token: 0x04000E2C RID: 3628
	[StatModifierOptions(false, false)]
	public StatModifier[] StartingStatModifiers;
}
