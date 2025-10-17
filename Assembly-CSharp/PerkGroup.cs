using System;
using UnityEngine;

// Token: 0x0200014E RID: 334
[CreateAssetMenu(menuName = "Survival/Perk Group")]
public class PerkGroup : UniqueIDScriptable
{
	// Token: 0x0600096E RID: 2414 RVA: 0x0005818C File Offset: 0x0005638C
	public bool HasPerk(CharacterPerk _Perk)
	{
		if (!_Perk)
		{
			return false;
		}
		if (this.PerksList == null)
		{
			return false;
		}
		if (this.PerksList.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.PerksList.Length; i++)
		{
			if (this.PerksList[i] == _Perk)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000EF0 RID: 3824
	public LocalizedString GroupName;

	// Token: 0x04000EF1 RID: 3825
	public CharacterPerk[] PerksList;
}
