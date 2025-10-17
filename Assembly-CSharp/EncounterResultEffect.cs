using System;
using System.Collections.Generic;

// Token: 0x02000066 RID: 102
[Serializable]
public class EncounterResultEffect
{
	// Token: 0x06000444 RID: 1092 RVA: 0x0002998D File Offset: 0x00027B8D
	public EncounterLogMessage GetLog(EncounterLogMessage _DefaultMessage)
	{
		if (!string.IsNullOrEmpty(this.ResultLog))
		{
			return this.ResultLog;
		}
		return _DefaultMessage;
	}

	// Token: 0x04000532 RID: 1330
	public EncounterLogMessage ResultLog;

	// Token: 0x04000533 RID: 1331
	public List<CardData> DroppedCards;

	// Token: 0x04000534 RID: 1332
	[StatModifierOptions(true, false)]
	public StatModifier[] StatChanges;

	// Token: 0x04000535 RID: 1333
	public EnemyValueToStatModifier[] TransferValuesToStats;

	// Token: 0x04000536 RID: 1334
	public AmmoRecoveryOptions AmmoRecovery;
}
