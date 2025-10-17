using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
[Serializable]
public class FromStatChangeAction : CardAction
{
	// Token: 0x04000B6F RID: 2927
	[Space]
	public StatTriggerTypes RepeatOptions;

	// Token: 0x04000B70 RID: 2928
	public bool OncePerTick;

	// Token: 0x04000B71 RID: 2929
	public StatValueTrigger[] StatChangeTrigger;
}
