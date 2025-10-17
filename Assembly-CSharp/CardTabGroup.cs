using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000100 RID: 256
[CreateAssetMenu(menuName = "Survival/Card Tab Group")]
public class CardTabGroup : ScriptableObject
{
	// Token: 0x0600085B RID: 2139 RVA: 0x00052704 File Offset: 0x00050904
	[ContextMenu("Fill Sorting List")]
	public void FillSortingList()
	{
		if (this.ShopSortingList == null)
		{
			this.ShopSortingList = new List<CardData>();
		}
		this.AddRangeToSortingList(this.IncludedCards);
		if (this.HasSubGroups)
		{
			for (int i = 0; i < this.SubGroups.Count; i++)
			{
				if (this.SubGroups[i])
				{
					this.AddRangeToSortingList(this.SubGroups[i].IncludedCards);
				}
			}
		}
		for (int j = this.ShopSortingList.Count - 1; j >= 0; j--)
		{
			if (!this.ShopSortingList[j])
			{
				this.ShopSortingList.RemoveAt(j);
			}
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x000527B4 File Offset: 0x000509B4
	private bool AddRangeToSortingList(List<CardData> _CardList)
	{
		if (_CardList == null)
		{
			return false;
		}
		if (_CardList.Count == 0)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < _CardList.Count; i++)
		{
			if (this.AddToSortingList(_CardList[i]))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x000527F5 File Offset: 0x000509F5
	private bool AddToSortingList(CardData _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.ShopSortingList.Contains(_Card))
		{
			return false;
		}
		this.ShopSortingList.Add(_Card);
		return true;
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x0600085E RID: 2142 RVA: 0x00052820 File Offset: 0x00050A20
	public ContentPage GetHelpPage
	{
		get
		{
			if (this.AssociatedEntry && GuideManager.PagesDict != null && GuideManager.PagesDict.ContainsKey(this.AssociatedEntry))
			{
				return GuideManager.PagesDict[this.AssociatedEntry];
			}
			if (this.IncludedCards != null)
			{
				for (int i = 0; i < this.IncludedCards.Count; i++)
				{
					ContentPage contentPage = GuideManager.GetPageFor(this.IncludedCards[i]);
					if (contentPage)
					{
						return contentPage;
					}
				}
			}
			if (this.SubGroups != null)
			{
				for (int j = 0; j < this.SubGroups.Count; j++)
				{
					ContentPage contentPage = this.SubGroups[j].GetHelpPage;
					if (contentPage)
					{
						return contentPage;
					}
				}
			}
			return null;
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x0600085F RID: 2143 RVA: 0x000528DC File Offset: 0x00050ADC
	public bool IsMixedLiquidGroup
	{
		get
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < this.IncludedCards.Count; i++)
			{
				if (this.IncludedCards[i].CardType == CardTypes.Liquid)
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
				if (flag && flag2)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00052926 File Offset: 0x00050B26
	public int BlueprintSortingIndex(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return -1;
		}
		return this.BlueprintSortingIndex(_Card.CardModel, 0);
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0005293F File Offset: 0x00050B3F
	public int BlueprintSortingIndex(CardData _Card, int _DepthLevel)
	{
		if (!_Card)
		{
			return -1;
		}
		if (this.ShopSortingList != null && this.ShopSortingList.Count > 0)
		{
			return this.ShopSortingList.IndexOf(_Card);
		}
		return this.IncludedCards.IndexOf(_Card);
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0005297C File Offset: 0x00050B7C
	public bool HasCard(CardData _Card, bool _CheckSubGroups)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.IncludedCards != null)
		{
			for (int i = 0; i < this.IncludedCards.Count; i++)
			{
				if (this.IncludedCards[i] == _Card)
				{
					return true;
				}
			}
		}
		if (this.SubGroups != null && _CheckSubGroups)
		{
			for (int j = 0; j < this.SubGroups.Count; j++)
			{
				if (this.SubGroups[j].HasCard(_Card, true))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00052A02 File Offset: 0x00050C02
	[ContextMenu("Sort alphabetically")]
	public void SortList()
	{
		this.IncludedCards.Sort((CardData a, CardData b) => a.name.CompareTo(b.name));
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000864 RID: 2148 RVA: 0x00052A30 File Offset: 0x00050C30
	public bool HasSubGroups
	{
		get
		{
			if (this.SubGroups == null)
			{
				return false;
			}
			for (int i = 0; i < this.SubGroups.Count; i++)
			{
				if (this.SubGroups[i])
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x04000CAD RID: 3245
	public LocalizedString TabName;

	// Token: 0x04000CAE RID: 3246
	public GuideEntry AssociatedEntry;

	// Token: 0x04000CAF RID: 3247
	public Sprite TabIcon;

	// Token: 0x04000CB0 RID: 3248
	public List<CardData> IncludedCards = new List<CardData>();

	// Token: 0x04000CB1 RID: 3249
	public List<CardTabGroup> SubGroups = new List<CardTabGroup>();

	// Token: 0x04000CB2 RID: 3250
	public List<CardData> ShopSortingList = new List<CardData>();
}
