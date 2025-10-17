using System;
using UnityEngine;

// Token: 0x0200014D RID: 333
[Serializable]
public struct ObjectiveNotificationSettings
{
	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x0600096A RID: 2410 RVA: 0x0005813D File Offset: 0x0005633D
	// (set) Token: 0x0600096B RID: 2411 RVA: 0x00058145 File Offset: 0x00056345
	public float LastUpdatedValue { get; private set; }

	// Token: 0x0600096C RID: 2412 RVA: 0x0005814E File Offset: 0x0005634E
	public void Init()
	{
		this.LastUpdatedValue = 0f;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0005815B File Offset: 0x0005635B
	public bool UpdateProgression(float _NextPercent)
	{
		if (_NextPercent - this.LastUpdatedValue >= this.PercentThreshold)
		{
			this.LastUpdatedValue = this.PercentThreshold * (float)Mathf.FloorToInt(_NextPercent / this.PercentThreshold);
			return true;
		}
		return false;
	}

	// Token: 0x04000EED RID: 3821
	public ObjectiveNotificationFrequencies Frequency;

	// Token: 0x04000EEE RID: 3822
	[Percent(true)]
	public float PercentThreshold;
}
