using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000092 RID: 146
public class MenuPerkPreview : TooltipProvider
{
	// Token: 0x06000639 RID: 1593 RVA: 0x00041930 File Offset: 0x0003FB30
	public void Setup(CharacterPerk _Data)
	{
		if (this.Icon)
		{
			this.Icon.overrideSprite = _Data.PerkIcon;
		}
		if (this.Title)
		{
			this.Title.text = _Data.PerkName;
		}
		if (!this.NoTooltip)
		{
			base.SetTooltip(_Data.PerkName, _Data.PerkDescription, "", 0);
			return;
		}
		base.CancelTooltip();
	}

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	private bool NoTooltip;

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	private Image Icon;

	// Token: 0x0400087D RID: 2173
	[SerializeField]
	private new TextMeshProUGUI Title;
}
