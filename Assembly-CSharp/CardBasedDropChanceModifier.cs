using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EC RID: 236
[Serializable]
public struct CardBasedDropChanceModifier
{
	// Token: 0x0600080E RID: 2062 RVA: 0x0004F54C File Offset: 0x0004D74C
	public int GetExtraWeight(List<CardDropWeightModReport> _Reports)
	{
		bool flag = false;
		GameManager instance = MBSingleton<GameManager>.Instance;
		List<InGameCardBase> list = new List<InGameCardBase>();
		if (_Reports == null)
		{
			_Reports = new List<CardDropWeightModReport>();
		}
		if (this.CardOnBoard)
		{
			if (this.OnlyInHand)
			{
				flag |= instance.CardIsInHand(this.CardOnBoard, false, list, Array.Empty<InGameCardBase>());
			}
			if (this.NotInHand)
			{
				flag |= instance.CardIsInBase(this.CardOnBoard, !this.ExcludeInventory, false, list, Array.Empty<InGameCardBase>());
				flag |= instance.CardIsInLocation(this.CardOnBoard, !this.ExcludeInventory, false, list, Array.Empty<InGameCardBase>());
			}
			if (!this.OnlyInHand && !this.NotInHand)
			{
				flag |= instance.CardIsOnBoard(this.CardOnBoard, true, !this.ExcludeInventory, false, false, list, Array.Empty<InGameCardBase>());
			}
		}
		if (this.TagOnBoard)
		{
			if (this.OnlyInHand)
			{
				flag |= instance.TagIsInHand(this.TagOnBoard, false, list, Array.Empty<InGameCardBase>());
			}
			if (this.NotInHand)
			{
				flag |= instance.TagIsInBase(this.TagOnBoard, !this.ExcludeInventory, false, list, Array.Empty<InGameCardBase>());
				flag |= instance.TagIsInLocation(this.TagOnBoard, !this.ExcludeInventory, false, list, Array.Empty<InGameCardBase>());
			}
			if (!this.OnlyInHand && !this.NotInHand)
			{
				flag |= instance.TagIsOnBoard(this.TagOnBoard, true, !this.ExcludeInventory, false, false, list);
			}
			flag |= (this.OnlyInHand ? instance.TagIsInHand(this.TagOnBoard, false, list, Array.Empty<InGameCardBase>()) : instance.TagIsOnBoard(this.TagOnBoard, true, !this.ExcludeInventory, false, false, list));
		}
		if (!flag)
		{
			return 0;
		}
		float num = 0f;
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				float num2 = 1f;
				if (this.SpoilageTime)
				{
					num2 *= list[i].CurrentSpoilagePercent;
				}
				if (this.UsageDurability)
				{
					num2 *= list[i].CurrentUsagePercent;
				}
				if (this.FuelCapacity)
				{
					num2 *= list[i].CurrentFuelPercent;
				}
				if (this.Progress)
				{
					num2 *= list[i].CurrentProgressPercent;
				}
				if (this.Special1)
				{
					num2 *= list[i].CurrentSpecial1Percent;
				}
				if (this.Special2)
				{
					num2 *= list[i].CurrentSpecial2Percent;
				}
				if (this.Special3)
				{
					num2 *= list[i].CurrentSpecial3Percent;
				}
				if (this.Special4)
				{
					num2 *= list[i].CurrentSpecial4Percent;
				}
				_Reports.Add(new CardDropWeightModReport
				{
					Card = list[i],
					BonusWeight = num2 * (float)this.MaxWeightPerCard
				});
			}
			if (this.MaxStack > 0)
			{
				_Reports.Sort((CardDropWeightModReport a, CardDropWeightModReport b) => b.BonusWeight.CompareTo(a.BonusWeight));
				if (_Reports.Count > this.MaxStack)
				{
					for (int j = _Reports.Count - 1; j >= this.MaxStack; j--)
					{
						_Reports.RemoveAt(j);
					}
				}
			}
		}
		else
		{
			if (this.CardOnBoard && this.CardOnBoard.CardType == CardTypes.Environment)
			{
				_Reports.Add(new CardDropWeightModReport
				{
					Card = null,
					BonusWeight = (float)this.MaxWeightPerCard
				});
			}
			if (this.TagOnBoard && instance.PrevEnvironment.HasTag(this.TagOnBoard))
			{
				_Reports.Add(new CardDropWeightModReport
				{
					Card = null,
					BonusWeight = (float)this.MaxWeightPerCard
				});
			}
		}
		for (int k = 0; k < _Reports.Count; k++)
		{
			num += _Reports[k].BonusWeight;
		}
		if (this.AddedStatModifiers != null)
		{
			float num3 = 0f;
			for (int l = 0; l < this.AddedStatModifiers.Length; l++)
			{
				if (!this.AddedStatModifiers[l].Stat)
				{
					Debug.LogError("Empty stat drop modifier in card drop modifier");
				}
				else if (instance.StatsDict.ContainsKey(this.AddedStatModifiers[l].Stat) && this.AddedStatModifiers[l].WillHaveEffect(instance.StatsDict[this.AddedStatModifiers[l].Stat].CurrentValue(instance.NotInBase)))
				{
					num3 += (float)((this.MultiplyByStacks ? _Reports.Count : 1) * this.AddedStatModifiers[l].GetExtraWeight(instance.StatsDict[this.AddedStatModifiers[l].Stat].CurrentValue(instance.NotInBase)));
				}
			}
			if (num3 != 0f)
			{
				CardDropWeightModReport value = default(CardDropWeightModReport);
				for (int m = 0; m < _Reports.Count; m++)
				{
					value = _Reports[m];
					value.BonusWeight += num3 / (float)_Reports.Count;
					_Reports[m] = value;
				}
				num += num3;
			}
		}
		return Mathf.RoundToInt(num);
	}

	// Token: 0x04000C0F RID: 3087
	public CardData CardOnBoard;

	// Token: 0x04000C10 RID: 3088
	public CardTag TagOnBoard;

	// Token: 0x04000C11 RID: 3089
	public bool OnlyInHand;

	// Token: 0x04000C12 RID: 3090
	public bool NotInHand;

	// Token: 0x04000C13 RID: 3091
	public bool ExcludeInventory;

	// Token: 0x04000C14 RID: 3092
	public int MaxWeightPerCard;

	// Token: 0x04000C15 RID: 3093
	public int MaxStack;

	// Token: 0x04000C16 RID: 3094
	public bool SpoilageTime;

	// Token: 0x04000C17 RID: 3095
	public bool UsageDurability;

	// Token: 0x04000C18 RID: 3096
	public bool FuelCapacity;

	// Token: 0x04000C19 RID: 3097
	public bool Progress;

	// Token: 0x04000C1A RID: 3098
	public bool Special1;

	// Token: 0x04000C1B RID: 3099
	public bool Special2;

	// Token: 0x04000C1C RID: 3100
	public bool Special3;

	// Token: 0x04000C1D RID: 3101
	public bool Special4;

	// Token: 0x04000C1E RID: 3102
	public StatBasedDropChanceModifier[] AddedStatModifiers;

	// Token: 0x04000C1F RID: 3103
	public bool MultiplyByStacks;
}
