using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
[CreateAssetMenu(fileName = "New Equipment Tag", menuName = "Survival/Equipment Tag")]
public class EquipmentTag : ScriptableObject
{
	// Token: 0x04000E1C RID: 3612
	public LocalizedString InGameName;

	// Token: 0x04000E1D RID: 3613
	public int MaxEquipped;

	// Token: 0x04000E1E RID: 3614
	public bool MandatoryEquip;

	// Token: 0x04000E1F RID: 3615
	public bool NotifyWhenNew;
}
