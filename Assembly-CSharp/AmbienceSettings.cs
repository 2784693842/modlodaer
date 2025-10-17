using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
[Serializable]
public class AmbienceSettings
{
	// Token: 0x17000198 RID: 408
	// (get) Token: 0x0600083A RID: 2106 RVA: 0x000519C4 File Offset: 0x0004FBC4
	public bool IsEmpty
	{
		get
		{
			if (this.BackgroundSound)
			{
				return false;
			}
			if (this.RandomNoises != null && this.RandomNoises.Length != 0)
			{
				for (int i = 0; i < this.RandomNoises.Length; i++)
				{
					if (this.RandomNoises[i])
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	// Token: 0x04000C8F RID: 3215
	public AudioClip BackgroundSound;

	// Token: 0x04000C90 RID: 3216
	public AudioClip[] RandomNoises;

	// Token: 0x04000C91 RID: 3217
	public DurabilitiesConditions AmbienceConditions;
}
