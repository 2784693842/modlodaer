using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
[CreateAssetMenu(menuName = "Survival/Slot Settings")]
public class SlotSettings : ScriptableObject
{
	// Token: 0x04000F11 RID: 3857
	public CardSlot Visuals;

	// Token: 0x04000F12 RID: 3858
	public SlotsTypes SlotType;

	// Token: 0x04000F13 RID: 3859
	public CardFilter CompatibleCards;

	// Token: 0x04000F14 RID: 3860
	public bool CanHostPile;

	// Token: 0x04000F15 RID: 3861
	public int MaxPileCount;

	// Token: 0x04000F16 RID: 3862
	public bool CanPin;
}
