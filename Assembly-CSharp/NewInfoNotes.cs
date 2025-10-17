using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class NewInfoNotes : MonoBehaviour
{
	// Token: 0x06000647 RID: 1607 RVA: 0x00041EB6 File Offset: 0x000400B6
	private void OnEnable()
	{
		if (GameLoad.Instance)
		{
			GraphicsManager.SetActiveGroup(this.NewIconGroup, GameLoad.Instance.LastCheckedNotesVersion != 17 && !this.EditorFlag);
		}
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x00041EE9 File Offset: 0x000400E9
	public void CheckNotes()
	{
		if (GameLoad.Instance)
		{
			if (Application.isEditor)
			{
				this.EditorFlag = true;
				return;
			}
			GameLoad.Instance.LastCheckedNotesVersion = 17;
		}
	}

	// Token: 0x04000899 RID: 2201
	[SerializeField]
	private GameObject[] NewIconGroup;

	// Token: 0x0400089A RID: 2202
	private bool EditorFlag;
}
