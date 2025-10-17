using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
[Serializable]
public struct StatusAlertSettings
{
	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000593 RID: 1427 RVA: 0x0003B208 File Offset: 0x00039408
	public static StatusAlertSettings NoAlert
	{
		get
		{
			return new StatusAlertSettings
			{
				AlertIcon = null,
				PulsingFrequency = 0f,
				OutlineColor = Color.clear,
				OutlinePulsingFrequency = 0f,
				SoundEffect = null,
				CriticalMode = false
			};
		}
	}

	// Token: 0x04000786 RID: 1926
	public Sprite AlertIcon;

	// Token: 0x04000787 RID: 1927
	public float PulsingFrequency;

	// Token: 0x04000788 RID: 1928
	public Color OutlineColor;

	// Token: 0x04000789 RID: 1929
	public float OutlinePulsingFrequency;

	// Token: 0x0400078A RID: 1930
	public Color OutlineBlinkingColor;

	// Token: 0x0400078B RID: 1931
	public float OutlineBlinkingFrequency;

	// Token: 0x0400078C RID: 1932
	public AudioClip SoundEffect;

	// Token: 0x0400078D RID: 1933
	public bool CriticalMode;
}
