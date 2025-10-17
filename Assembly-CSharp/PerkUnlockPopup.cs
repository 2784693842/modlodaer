using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009D RID: 157
public class PerkUnlockPopup : MonoBehaviour
{
	// Token: 0x06000672 RID: 1650 RVA: 0x00043734 File Offset: 0x00041934
	public void Show(CharacterPerk _Perk)
	{
		if (!_Perk)
		{
			return;
		}
		if (this.PerkName)
		{
			this.PerkName.text = _Perk.PerkName;
		}
		if (this.PerkDesc)
		{
			this.PerkDesc.text = _Perk.PerkDescription;
		}
		if (this.PerkUnlock)
		{
			this.PerkUnlock.text = _Perk.PerkUnlockConditions;
		}
		if (this.PerkIcon)
		{
			this.PerkIcon.overrideSprite = _Perk.PerkIcon;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x040008E2 RID: 2274
	[SerializeField]
	private TextMeshProUGUI PerkName;

	// Token: 0x040008E3 RID: 2275
	[SerializeField]
	private TextMeshProUGUI PerkDesc;

	// Token: 0x040008E4 RID: 2276
	[SerializeField]
	private TextMeshProUGUI PerkUnlock;

	// Token: 0x040008E5 RID: 2277
	[SerializeField]
	private Image PerkIcon;
}
