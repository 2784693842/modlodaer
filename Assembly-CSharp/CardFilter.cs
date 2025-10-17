using System;
using System.Collections.Generic;

// Token: 0x02000007 RID: 7
[Serializable]
public struct CardFilter
{
	// Token: 0x06000019 RID: 25 RVA: 0x00002EBC File Offset: 0x000010BC
	public bool TypesContains(CardTypes _Type)
	{
		if (this.AcceptedTypes == null)
		{
			return false;
		}
		if (this.AcceptedTypes.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.AcceptedTypes.Length; i++)
		{
			if (this.AcceptedTypes[i] == _Type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002F00 File Offset: 0x00001100
	public bool TagsContains(CardTag _Tag)
	{
		if (this.AcceptedTags == null)
		{
			return false;
		}
		if (this.AcceptedTags.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.AcceptedTags.Length; i++)
		{
			if (this.AcceptedTags[i] == _Tag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002F48 File Offset: 0x00001148
	public bool CardsContains(CardData _Card)
	{
		if (this.AcceptedCards == null)
		{
			return false;
		}
		if (this.AcceptedCards.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.AcceptedCards.Length; i++)
		{
			if (this.AcceptedCards[i] == _Card)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600001C RID: 28 RVA: 0x00002F90 File Offset: 0x00001190
	public bool IsEmpty
	{
		get
		{
			bool flag = this.AcceptedCards == null;
			if (!flag)
			{
				flag = (this.AcceptedCards.Length == 0);
			}
			bool flag2 = this.AcceptedTypes == null;
			if (!flag2)
			{
				flag2 = (this.AcceptedTypes.Length == 0);
			}
			bool flag3 = this.AcceptedTags == null;
			if (!flag3)
			{
				flag3 = (this.AcceptedTags.Length == 0);
			}
			bool flag4 = this.NOTAcceptedCards == null;
			if (!flag4)
			{
				flag4 = (this.NOTAcceptedCards.Length == 0);
			}
			bool flag5 = this.NOTAcceptedTypes == null;
			if (!flag5)
			{
				flag5 = (this.NOTAcceptedTypes.Length == 0);
			}
			bool flag6 = this.NOTAcceptedTags == null;
			if (!flag6)
			{
				flag6 = (this.NOTAcceptedTags.Length == 0);
			}
			bool flag7 = this.CardFilters == null;
			if (!flag7)
			{
				flag7 = (this.CardFilters.Length == 0);
			}
			bool flag8 = this.TagFilters == null;
			if (!flag8)
			{
				flag8 = (this.TagFilters.Length == 0);
			}
			return flag && flag2 && flag3 && flag4 && flag5 && flag6 && flag7 && flag8;
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0000307C File Offset: 0x0000127C
	public void Clear()
	{
		this.AcceptedCards = new CardData[0];
		this.AcceptedTypes = new CardTypes[0];
		this.AcceptedTags = new CardTag[0];
		this.NOTAcceptedCards = new CardData[0];
		this.NOTAcceptedTypes = new CardTypes[0];
		this.NOTAcceptedTags = new CardTag[0];
		this.CardFilters = new CardFilterRef[0];
		this.TagFilters = new TagFilterRef[0];
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000030EC File Offset: 0x000012EC
	public void AddFilters(CardFilter _Filter)
	{
		List<CardData> list = new List<CardData>();
		List<CardTypes> list2 = new List<CardTypes>();
		List<CardTag> list3 = new List<CardTag>();
		List<CardData> list4 = new List<CardData>();
		List<CardTypes> list5 = new List<CardTypes>();
		List<CardTag> list6 = new List<CardTag>();
		List<CardFilterRef> list7 = new List<CardFilterRef>();
		List<TagFilterRef> list8 = new List<TagFilterRef>();
		if (this.AcceptedCards != null)
		{
			list.AddRange(this.AcceptedCards);
		}
		if (this.AcceptedTypes != null)
		{
			list2.AddRange(this.AcceptedTypes);
		}
		if (this.AcceptedTags != null)
		{
			list3.AddRange(this.AcceptedTags);
		}
		if (this.CardFilters != null)
		{
			list7.AddRange(this.CardFilters);
		}
		if (this.TagFilters != null)
		{
			list8.AddRange(this.TagFilters);
		}
		if (this.NOTAcceptedCards != null)
		{
			list4.AddRange(this.NOTAcceptedCards);
		}
		if (this.NOTAcceptedTypes != null)
		{
			list5.AddRange(this.NOTAcceptedTypes);
		}
		if (this.NOTAcceptedTags != null)
		{
			list6.AddRange(this.NOTAcceptedTags);
		}
		if (_Filter.NOTAcceptedCards != null)
		{
			for (int i = 0; i < _Filter.NOTAcceptedCards.Length; i++)
			{
				if (!list4.Contains(_Filter.NOTAcceptedCards[i]))
				{
					list4.Add(_Filter.NOTAcceptedCards[i]);
				}
			}
		}
		if (_Filter.NOTAcceptedTypes != null)
		{
			for (int j = 0; j < _Filter.NOTAcceptedTypes.Length; j++)
			{
				if (!list5.Contains(_Filter.NOTAcceptedTypes[j]))
				{
					list5.Add(_Filter.NOTAcceptedTypes[j]);
				}
			}
		}
		if (_Filter.NOTAcceptedTags != null)
		{
			for (int k = 0; k < _Filter.NOTAcceptedTags.Length; k++)
			{
				if (!list6.Contains(_Filter.NOTAcceptedTags[k]))
				{
					list6.Add(_Filter.NOTAcceptedTags[k]);
				}
			}
		}
		if (_Filter.AcceptedCards != null)
		{
			for (int l = 0; l < _Filter.AcceptedCards.Length; l++)
			{
				if (!list.Contains(_Filter.AcceptedCards[l]) && !list4.Contains(_Filter.AcceptedCards[l]))
				{
					list.Add(_Filter.AcceptedCards[l]);
				}
			}
		}
		if (_Filter.AcceptedTypes != null)
		{
			for (int m = 0; m < _Filter.AcceptedTypes.Length; m++)
			{
				if (!list2.Contains(_Filter.AcceptedTypes[m]) && !list5.Contains(_Filter.AcceptedTypes[m]))
				{
					list2.Add(_Filter.AcceptedTypes[m]);
				}
			}
		}
		if (_Filter.AcceptedTags != null)
		{
			for (int n = 0; n < _Filter.AcceptedTags.Length; n++)
			{
				if (!list3.Contains(_Filter.AcceptedTags[n]) && !list6.Contains(_Filter.AcceptedTags[n]))
				{
					list3.Add(_Filter.AcceptedTags[n]);
				}
			}
		}
		if (_Filter.CardFilters != null)
		{
			for (int num = 0; num < _Filter.CardFilters.Length; num++)
			{
				if (!list7.Contains(_Filter.CardFilters[num]))
				{
					list7.Add(_Filter.CardFilters[num]);
				}
			}
		}
		if (_Filter.TagFilters != null)
		{
			for (int num2 = 0; num2 < _Filter.TagFilters.Length; num2++)
			{
				if (!list8.Contains(_Filter.TagFilters[num2]))
				{
					list8.Add(_Filter.TagFilters[num2]);
				}
			}
		}
		this.AcceptedCards = list.ToArray();
		this.AcceptedTypes = list2.ToArray();
		this.AcceptedTags = list3.ToArray();
		this.NOTAcceptedCards = list4.ToArray();
		this.NOTAcceptedTypes = list5.ToArray();
		this.NOTAcceptedTags = list6.ToArray();
		this.CardFilters = list7.ToArray();
		this.TagFilters = list8.ToArray();
	}

	// Token: 0x0600001F RID: 31 RVA: 0x0000347C File Offset: 0x0000167C
	public void RemoveFilters(CardFilter _Filter)
	{
		List<CardData> list = new List<CardData>();
		List<CardTypes> list2 = new List<CardTypes>();
		List<CardTag> list3 = new List<CardTag>();
		List<CardData> list4 = new List<CardData>();
		List<CardTypes> list5 = new List<CardTypes>();
		List<CardTag> list6 = new List<CardTag>();
		List<CardFilterRef> list7 = new List<CardFilterRef>();
		List<TagFilterRef> list8 = new List<TagFilterRef>();
		if (this.AcceptedCards != null)
		{
			list.AddRange(this.AcceptedCards);
		}
		if (this.AcceptedTypes != null)
		{
			list2.AddRange(this.AcceptedTypes);
		}
		if (this.AcceptedTags != null)
		{
			list3.AddRange(this.AcceptedTags);
		}
		if (this.CardFilters != null)
		{
			list7.AddRange(this.CardFilters);
		}
		if (this.TagFilters != null)
		{
			list8.AddRange(this.TagFilters);
		}
		if (_Filter.AcceptedCards != null)
		{
			for (int i = 0; i < _Filter.AcceptedCards.Length; i++)
			{
				if (list.Contains(_Filter.AcceptedCards[i]))
				{
					list.Remove(_Filter.AcceptedCards[i]);
				}
			}
		}
		if (_Filter.AcceptedTypes != null)
		{
			for (int j = 0; j < _Filter.AcceptedTypes.Length; j++)
			{
				if (list2.Contains(_Filter.AcceptedTypes[j]))
				{
					list2.Remove(_Filter.AcceptedTypes[j]);
				}
			}
		}
		if (_Filter.AcceptedTags != null)
		{
			for (int k = 0; k < _Filter.AcceptedTags.Length; k++)
			{
				if (list3.Contains(_Filter.AcceptedTags[k]))
				{
					list3.Remove(_Filter.AcceptedTags[k]);
				}
			}
		}
		if (_Filter.CardFilters != null)
		{
			for (int l = 0; l < _Filter.CardFilters.Length; l++)
			{
				if (list7.Contains(_Filter.CardFilters[l]))
				{
					list7.Remove(_Filter.CardFilters[l]);
				}
			}
		}
		if (_Filter.TagFilters != null)
		{
			for (int m = 0; m < _Filter.TagFilters.Length; m++)
			{
				if (list8.Contains(_Filter.TagFilters[m]))
				{
					list8.Remove(_Filter.TagFilters[m]);
				}
			}
		}
		if (this.NOTAcceptedCards != null)
		{
			list4.AddRange(this.NOTAcceptedCards);
		}
		if (this.NOTAcceptedTypes != null)
		{
			list5.AddRange(this.NOTAcceptedTypes);
		}
		if (this.NOTAcceptedTags != null)
		{
			list6.AddRange(this.NOTAcceptedTags);
		}
		if (_Filter.NOTAcceptedCards != null)
		{
			for (int n = 0; n < _Filter.NOTAcceptedCards.Length; n++)
			{
				if (list4.Contains(_Filter.NOTAcceptedCards[n]))
				{
					list4.Remove(_Filter.NOTAcceptedCards[n]);
				}
			}
		}
		if (_Filter.NOTAcceptedTypes != null)
		{
			for (int num = 0; num < _Filter.NOTAcceptedTypes.Length; num++)
			{
				if (list5.Contains(_Filter.NOTAcceptedTypes[num]))
				{
					list5.Remove(_Filter.NOTAcceptedTypes[num]);
				}
			}
		}
		if (_Filter.NOTAcceptedTags != null)
		{
			for (int num2 = 0; num2 < _Filter.NOTAcceptedTags.Length; num2++)
			{
				if (list6.Contains(_Filter.NOTAcceptedTags[num2]))
				{
					list6.Remove(_Filter.NOTAcceptedTags[num2]);
				}
			}
		}
		this.AcceptedCards = list.ToArray();
		this.AcceptedTypes = list2.ToArray();
		this.AcceptedTags = list3.ToArray();
		this.NOTAcceptedCards = list4.ToArray();
		this.NOTAcceptedTypes = list5.ToArray();
		this.NOTAcceptedTags = list6.ToArray();
		this.CardFilters = list7.ToArray();
		this.TagFilters = list8.ToArray();
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000037E0 File Offset: 0x000019E0
	public bool SupportsCard(CardData _Card, CardData _WithLiquid)
	{
		if (!_Card)
		{
			return false;
		}
		List<CardData> list = new List<CardData>();
		List<CardData> list2 = new List<CardData>();
		List<CardTag> list3 = new List<CardTag>();
		List<CardTag> list4 = new List<CardTag>();
		if (this.CardFilters != null)
		{
			for (int i = 0; i < this.CardFilters.Length; i++)
			{
				if ((!this.CardFilters[i].OnlyWithLiquid || _WithLiquid) && this.CardFilters[i].Card)
				{
					if (this.CardFilters[i].NOT)
					{
						list.Add(this.CardFilters[i].Card);
					}
					else
					{
						list2.Add(this.CardFilters[i].Card);
					}
				}
			}
		}
		if (this.TagFilters != null)
		{
			for (int j = 0; j < this.TagFilters.Length; j++)
			{
				if ((!this.TagFilters[j].OnlyWithLiquid || _WithLiquid) && this.TagFilters[j].Tag)
				{
					if (this.TagFilters[j].NOT)
					{
						list3.Add(this.TagFilters[j].Tag);
					}
					else
					{
						list4.Add(this.TagFilters[j].Tag);
					}
				}
			}
		}
		if (this.NOTAcceptedTypes != null && this.NOTAcceptedTypes.Length != 0)
		{
			for (int k = 0; k < this.NOTAcceptedTypes.Length; k++)
			{
				if (_Card.CardType == this.NOTAcceptedTypes[k])
				{
					return false;
				}
			}
		}
		if (this.NOTAcceptedTags != null && this.NOTAcceptedTags.Length != 0)
		{
			for (int l = 0; l < this.NOTAcceptedTags.Length; l++)
			{
				if (_Card.HasTag(this.NOTAcceptedTags[l]))
				{
					return false;
				}
			}
		}
		if (list3.Count > 0)
		{
			for (int m = 0; m < list3.Count; m++)
			{
				if (_Card.HasTag(list3[m]))
				{
					return false;
				}
			}
		}
		if (this.NOTAcceptedCards != null && this.NOTAcceptedCards.Length != 0)
		{
			for (int n = 0; n < this.NOTAcceptedCards.Length; n++)
			{
				if (_Card == this.NOTAcceptedCards[n])
				{
					return false;
				}
			}
		}
		if (list.Count > 0)
		{
			for (int num = 0; num < list.Count; num++)
			{
				if (_Card == list[num])
				{
					return false;
				}
			}
		}
		if (_WithLiquid && this.SupportsCard(_WithLiquid, null))
		{
			return true;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		if (this.AcceptedTypes != null)
		{
			if (this.AcceptedTypes.Length == 0)
			{
				flag4 = true;
			}
			else
			{
				for (int num2 = 0; num2 < this.AcceptedTypes.Length; num2++)
				{
					if (_Card.CardType == this.AcceptedTypes[num2])
					{
						flag = true;
						break;
					}
				}
			}
		}
		else
		{
			flag4 = true;
		}
		if (this.AcceptedTags != null)
		{
			if (this.AcceptedTags.Length == 0)
			{
				flag5 = true;
			}
			else
			{
				for (int num3 = 0; num3 < this.AcceptedTags.Length; num3++)
				{
					if (_Card.HasTag(this.AcceptedTags[num3]))
					{
						flag2 = true;
						break;
					}
				}
			}
		}
		else
		{
			flag5 = true;
		}
		if (list4.Count > 0)
		{
			flag5 = false;
			for (int num4 = 0; num4 < list4.Count; num4++)
			{
				if (_Card.HasTag(list4[num4]))
				{
					flag2 = true;
					break;
				}
			}
		}
		if (this.AcceptedCards != null)
		{
			if (this.AcceptedCards.Length == 0)
			{
				flag6 = true;
			}
			else
			{
				for (int num5 = 0; num5 < this.AcceptedCards.Length; num5++)
				{
					if (_Card == this.AcceptedCards[num5])
					{
						flag3 = true;
						break;
					}
				}
			}
		}
		else
		{
			flag6 = true;
		}
		if (list2.Count > 0)
		{
			flag6 = false;
			for (int num6 = 0; num6 < list2.Count; num6++)
			{
				if (_Card == list2[num6])
				{
					flag3 = true;
					break;
				}
			}
		}
		if (flag6 && flag5 && flag4)
		{
			return true;
		}
		if (flag3)
		{
			return true;
		}
		if (flag5 && flag4)
		{
			return false;
		}
		if (!flag4 && flag5)
		{
			return flag;
		}
		if (flag4 && !flag5)
		{
			return flag2;
		}
		return flag2 && flag;
	}

	// Token: 0x04000039 RID: 57
	public CardData[] AcceptedCards;

	// Token: 0x0400003A RID: 58
	public CardTypes[] AcceptedTypes;

	// Token: 0x0400003B RID: 59
	public CardTag[] AcceptedTags;

	// Token: 0x0400003C RID: 60
	public CardData[] NOTAcceptedCards;

	// Token: 0x0400003D RID: 61
	public CardTypes[] NOTAcceptedTypes;

	// Token: 0x0400003E RID: 62
	public CardTag[] NOTAcceptedTags;

	// Token: 0x0400003F RID: 63
	public CardFilterRef[] CardFilters;

	// Token: 0x04000040 RID: 64
	public TagFilterRef[] TagFilters;
}
