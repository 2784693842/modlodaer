using System;
using UnityEngine;

// Token: 0x020001BD RID: 445
public class LayoutTest : MonoBehaviour
{
	// Token: 0x06000C2E RID: 3118 RVA: 0x00064CF8 File Offset: 0x00062EF8
	private void Start()
	{
		for (int i = 0; i < this.Quantity; i++)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.Prefab, base.transform);
		}
	}

	// Token: 0x0400111D RID: 4381
	public GameObject Prefab;

	// Token: 0x0400111E RID: 4382
	public int Quantity;
}
