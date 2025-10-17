using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
[Serializable]
public class CollectionDropsSaveData
{
	// Token: 0x060001F3 RID: 499 RVA: 0x00014A69 File Offset: 0x00012C69
	public CollectionDropsSaveData(string _Name, Vector2Int _Drops)
	{
		this.CollectionName = _Name;
		this.CollectionDrops = _Drops;
	}

	// Token: 0x040001EC RID: 492
	public string CollectionName;

	// Token: 0x040001ED RID: 493
	public Vector2Int CollectionDrops;
}
