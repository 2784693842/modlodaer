using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014F RID: 335
[CreateAssetMenu(menuName = "Survival/Perk Tab Group")]
public class PerkTabGroup : UniqueIDScriptable
{
	// Token: 0x06000970 RID: 2416 RVA: 0x000581DE File Offset: 0x000563DE
	public bool ContainsPerk(CharacterPerk _Perk)
	{
		return this.IncludesAllPerks || (this.ContainedPerks != null && this.ContainedPerks.Contains(_Perk));
	}

	// Token: 0x04000EF2 RID: 3826
	public LocalizedString TabName;

	// Token: 0x04000EF3 RID: 3827
	public Sprite Icon;

	// Token: 0x04000EF4 RID: 3828
	public bool IncludesAllPerks;

	// Token: 0x04000EF5 RID: 3829
	public List<CharacterPerk> ContainedPerks;
}
