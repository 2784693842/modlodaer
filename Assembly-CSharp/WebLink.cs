using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
[Serializable]
public struct WebLink
{
	// Token: 0x170001BD RID: 445
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00054866 File Offset: 0x00052A66
	public string Text
	{
		get
		{
			if (string.IsNullOrEmpty(this.LocalizedLinkText))
			{
				return this.ButtonText;
			}
			return this.LocalizedLinkText;
		}
	}

	// Token: 0x04000D4A RID: 3402
	[SerializeField]
	private string ButtonText;

	// Token: 0x04000D4B RID: 3403
	[SerializeField]
	private LocalizedString LocalizedLinkText;

	// Token: 0x04000D4C RID: 3404
	public string Link;
}
