using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class JournalHelpButton : MonoBehaviour
{
	// Token: 0x06000A95 RID: 2709 RVA: 0x0005E050 File Offset: 0x0005C250
	public void OnClick()
	{
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.GM)
		{
			this.GM.OpenGuide();
		}
	}

	// Token: 0x04001044 RID: 4164
	private GameManager GM;
}
