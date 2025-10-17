using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A4 RID: 164
public class ShowCharacterPortrait : MonoBehaviour
{
	// Token: 0x0600069D RID: 1693 RVA: 0x0004503A File Offset: 0x0004323A
	private void Start()
	{
		this.UpdatePortrait();
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x00045044 File Offset: 0x00043244
	private void UpdatePortrait()
	{
		if (!GameManager.CurrentPlayerCharacter || !this.PortraitImage)
		{
			return;
		}
		if (this.PortraitImage.overrideSprite != GameManager.CurrentPlayerCharacter.CharacterPortrait)
		{
			this.PortraitImage.overrideSprite = GameManager.CurrentPlayerCharacter.CharacterPortrait;
		}
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x0004503A File Offset: 0x0004323A
	private void Update()
	{
		this.UpdatePortrait();
	}

	// Token: 0x04000932 RID: 2354
	[SerializeField]
	private Image PortraitImage;
}
