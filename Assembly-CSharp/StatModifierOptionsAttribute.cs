using System;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class StatModifierOptionsAttribute : PropertyAttribute
{
	// Token: 0x06000BCB RID: 3019 RVA: 0x000630FB File Offset: 0x000612FB
	public StatModifierOptionsAttribute(bool _ShowRate, bool _ShowEachTick)
	{
		this.ShowRate = _ShowRate;
		this.ShowEachTick = _ShowEachTick;
	}

	// Token: 0x040010D9 RID: 4313
	public bool ShowRate;

	// Token: 0x040010DA RID: 4314
	public bool ShowEachTick;
}
