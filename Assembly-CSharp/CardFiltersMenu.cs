using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class CardFiltersMenu : MonoBehaviour
{
	// Token: 0x06000316 RID: 790 RVA: 0x0001F4B0 File Offset: 0x0001D6B0
	private void Awake()
	{
		for (int i = 0; i < this.AllFilters.Length; i++)
		{
			if (this.AllFilters[i])
			{
				UnityEngine.Object.Instantiate<CardFilterGroupButton>(this.ButtonPrefab, this.ButtonParent).Setup(this.AllFilters[i]);
			}
		}
	}

	// Token: 0x040003B4 RID: 948
	public CardFilterGroup[] AllFilters;

	// Token: 0x040003B5 RID: 949
	public CardFilterGroupButton ButtonPrefab;

	// Token: 0x040003B6 RID: 950
	public RectTransform ButtonParent;
}
