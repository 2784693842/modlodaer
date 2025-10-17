using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class MenuingHelper : MonoBehaviour
{
	// Token: 0x06000B6D RID: 2925 RVA: 0x00061144 File Offset: 0x0005F344
	private void Awake()
	{
		this.SetGroupActive(0);
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00061150 File Offset: 0x0005F350
	public void SetGroupActive(int _Index)
	{
		if (this.Groups == null)
		{
			return;
		}
		if (this.Groups.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.Groups.Length; i++)
		{
			this.Groups[i].SetActive(false);
		}
		this.Groups[Mathf.Clamp(_Index, 0, this.Groups.Length - 1)].SetActive(true);
		Action<int> onScreenChanged = this.OnScreenChanged;
		if (onScreenChanged == null)
		{
			return;
		}
		onScreenChanged(_Index);
	}

	// Token: 0x04001066 RID: 4198
	public MenuGroup[] Groups;

	// Token: 0x04001067 RID: 4199
	public Action<int> OnScreenChanged;
}
