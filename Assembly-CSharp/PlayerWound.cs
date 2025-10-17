using System;

// Token: 0x02000124 RID: 292
[Serializable]
public class PlayerWound
{
	// Token: 0x04000DD1 RID: 3537
	public EncounterLogMessage WoundLog;

	// Token: 0x04000DD2 RID: 3538
	public BodyLocationFilter LocationFilter;

	// Token: 0x04000DD3 RID: 3539
	public CardData[] DroppedCards;

	// Token: 0x04000DD4 RID: 3540
	[StatModifierOptions(true, false)]
	public StatModifier[] StatChanges;

	// Token: 0x04000DD5 RID: 3541
	public float WoundSelectionWeight;

	// Token: 0x04000DD6 RID: 3542
	public EncounterResult EncounterResult;
}
