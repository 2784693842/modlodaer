using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class WebLinkButton : MonoBehaviour
{
	// Token: 0x0600073D RID: 1853 RVA: 0x000484AC File Offset: 0x000466AC
	public void Click()
	{
		if (Application.platform == RuntimePlatform.Android && !string.IsNullOrEmpty(this.AndroidOverride))
		{
			Application.OpenURL(this.AndroidOverride);
			return;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer && !string.IsNullOrEmpty(this.IOSOverride))
		{
			Application.OpenURL(this.IOSOverride);
			return;
		}
		if (!string.IsNullOrEmpty(this.MainLink))
		{
			Application.OpenURL(this.MainLink);
		}
	}

	// Token: 0x040009EA RID: 2538
	public string MainLink;

	// Token: 0x040009EB RID: 2539
	public string AndroidOverride;

	// Token: 0x040009EC RID: 2540
	public string IOSOverride;
}
