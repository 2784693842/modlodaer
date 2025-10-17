using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C7 RID: 199
[CreateAssetMenu(menuName = "Survival/Bookmark Group")]
public class BookmarkGroup : UniqueIDScriptable
{
	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000755 RID: 1877 RVA: 0x00048DD8 File Offset: 0x00046FD8
	public CardTypes GetCardType
	{
		get
		{
			if (this.IncludedCards == null)
			{
				return CardTypes.Base;
			}
			if (this.IncludedCards.Count < 0)
			{
				return CardTypes.Base;
			}
			for (int i = 0; i < this.IncludedCards.Count; i++)
			{
				if (this.IncludedCards[i])
				{
					return this.IncludedCards[i].CardType;
				}
			}
			return CardTypes.Base;
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00048E3C File Offset: 0x0004703C
	public bool HasCard(CardData _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.IncludedCards == null)
		{
			return false;
		}
		if (this.IncludedCards.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < this.IncludedCards.Count; i++)
		{
			if (this.IncludedCards[i] == _Card)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00048E99 File Offset: 0x00047099
	[ContextMenu("Sort alphabetically")]
	public void SortList()
	{
		this.IncludedCards.Sort((CardData a, CardData b) => a.name.CompareTo(b.name));
	}

	// Token: 0x04000A52 RID: 2642
	public LocalizedString GroupName;

	// Token: 0x04000A53 RID: 2643
	[SerializeField]
	private List<CardData> IncludedCards = new List<CardData>();
}
