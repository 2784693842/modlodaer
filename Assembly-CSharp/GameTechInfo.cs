using System;
using UnityEngine;

// Token: 0x0200013F RID: 319
public static class GameTechInfo
{
	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x00056FCB File Offset: 0x000551CB
	public static string CurrentVersionNumber
	{
		get
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return "1.05.19";
			}
			return "v1.05s";
		}
	}

	// Token: 0x04000EAB RID: 3755
	public const string VersionNumber = "v1.05s";

	// Token: 0x04000EAC RID: 3756
	private const string IOSVersionNumber = "1.05.19";
}
