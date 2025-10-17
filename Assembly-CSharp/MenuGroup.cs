using System;
using UnityEngine;

// Token: 0x02000198 RID: 408
[Serializable]
public struct MenuGroup
{
	// Token: 0x06000B70 RID: 2928 RVA: 0x000611C8 File Offset: 0x0005F3C8
	public void SetActive(bool _Active)
	{
		if (this.TargetObjects == null)
		{
			return;
		}
		if (this.TargetObjects.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.TargetObjects.Length; i++)
		{
			if (this.TargetObjects[i])
			{
				this.TargetObjects[i].SetActive(_Active);
			}
		}
	}

	// Token: 0x04001068 RID: 4200
	public string GroupName;

	// Token: 0x04001069 RID: 4201
	public GameObject[] TargetObjects;
}
