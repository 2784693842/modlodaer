using System;
using TMPro;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class MenuPerkButton : IndexButton
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x000418B1 File Offset: 0x0003FAB1
	// (set) Token: 0x06000636 RID: 1590 RVA: 0x000418B9 File Offset: 0x0003FAB9
	public CharacterPerk AssociatedPerk { get; private set; }

	// Token: 0x06000637 RID: 1591 RVA: 0x000418C4 File Offset: 0x0003FAC4
	public void Setup(int _Index, CharacterPerk _Perk, bool _Unlocked)
	{
		this.AssociatedPerk = _Perk;
		base.Sprite = _Perk.PerkIcon;
		this.RatingText.text = _Perk.DifficultyRating.ToString();
		GraphicsManager.SetActiveGroup(this.LockedObjects, !_Unlocked);
		GraphicsManager.SetActiveGroup(this.UnlockedObjects, _Unlocked);
		base.Setup(_Index, this.AssociatedPerk.PerkName, null, false);
	}

	// Token: 0x04000877 RID: 2167
	public TextMeshProUGUI RatingText;

	// Token: 0x04000878 RID: 2168
	public GameObject[] LockedObjects;

	// Token: 0x04000879 RID: 2169
	public GameObject[] UnlockedObjects;
}
