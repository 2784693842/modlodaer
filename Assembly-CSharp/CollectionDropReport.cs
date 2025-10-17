using System;
using System.Text;
using UnityEngine;

// Token: 0x020001CE RID: 462
public struct CollectionDropReport
{
	// Token: 0x06000C75 RID: 3189 RVA: 0x000660A4 File Offset: 0x000642A4
	public bool IsIdentical(CollectionDropReport _To)
	{
		bool flag = (_To.FromAction != null && this.FromAction != null) || (_To.FromAction == null && this.FromAction == null);
		if (!flag)
		{
			return false;
		}
		if (this.FromAction != null)
		{
			flag = (_To.FromAction.ActionName == this.FromAction.ActionName);
		}
		return _To.FromCard == this.FromCard && flag && _To.FromData == this.FromData;
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00066134 File Offset: 0x00064334
	public string CollectionReportName
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.FromAction != null)
			{
				if (string.IsNullOrEmpty(this.FromAction.ActionName))
				{
					stringBuilder.Append("Unnamed Action");
				}
				else
				{
					stringBuilder.Append(this.FromAction.ActionName);
				}
			}
			else
			{
				stringBuilder.Append("Null Action");
			}
			if (this.FromCard)
			{
				stringBuilder.Append(" on ");
				if (this.FromCard.CardModel == this.FromData || !this.FromData)
				{
					stringBuilder.Append(this.FromCard.name);
				}
				else
				{
					stringBuilder.Append(this.FromData.name);
					stringBuilder.Append(" (Recycled)");
				}
			}
			else if (this.FromData)
			{
				stringBuilder.Append(" on ");
				stringBuilder.Append(this.FromData.name);
				stringBuilder.Append(" (Destroyed)");
			}
			else
			{
				stringBuilder.Append(" (self triggered)");
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00066258 File Offset: 0x00064458
	public float GetDropPercent(int _Index, bool _WithStats, bool _WithCards, bool _WithDurabilities)
	{
		if (_Index < 0 || _Index >= this.DropsInfo.Length || (this.TotalValue <= 0 && _WithStats && _WithCards) || (this.BaseValue <= 0 && !_WithStats && !_WithCards))
		{
			return 0f;
		}
		if (_WithStats && _WithCards)
		{
			return Mathf.Max(0f, (float)this.DropsInfo[_Index].FinalWeight) / (float)this.TotalValue;
		}
		if (!_WithStats && !_WithCards)
		{
			return Mathf.Max(0f, (float)this.DropsInfo[_Index].BaseWeight) / (float)this.BaseValue;
		}
		return Mathf.Max(0f, (float)(this.DropsInfo[_Index].BaseWeight + this.DropsInfo[_Index].BonusWeight(_WithStats, _WithCards, _WithDurabilities))) / (float)(this.TotalValue - this.DropsInfo[_Index].BonusWeight(!_WithStats, !_WithCards, !_WithDurabilities));
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0006634C File Offset: 0x0006454C
	public float GetSuccessPercent(bool _WithStats, bool _WithCards, bool _WithDurabilities)
	{
		if (this.FromAction == null)
		{
			return -1f;
		}
		if (!this.FromAction.HasSuccessfulDrop)
		{
			return -1f;
		}
		float num = 0f;
		for (int i = 0; i < this.DropsInfo.Length; i++)
		{
			if (this.DropsInfo[i].IsSuccess)
			{
				num += this.GetDropPercent(i, _WithStats, _WithCards, _WithDurabilities);
			}
		}
		return Mathf.Max(0f, num);
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x000663C0 File Offset: 0x000645C0
	public int GetSuccessWeight(bool _WithStatModifiers, bool _WithCardsModifiers, bool _WithDurabilitiesModifiers)
	{
		if (this.FromAction == null)
		{
			return -1;
		}
		if (!this.FromAction.HasSuccessfulDrop)
		{
			return -1;
		}
		int num = 0;
		for (int i = 0; i < this.DropsInfo.Length; i++)
		{
			if (this.DropsInfo[i].IsSuccess)
			{
				if (_WithCardsModifiers && _WithStatModifiers && _WithDurabilitiesModifiers)
				{
					num += this.DropsInfo[i].FinalWeight;
				}
				else if (!_WithCardsModifiers && !_WithStatModifiers && !_WithDurabilitiesModifiers)
				{
					num += this.DropsInfo[i].BaseWeight;
				}
				else
				{
					num += this.DropsInfo[i].BaseWeight + this.DropsInfo[i].BonusWeight(_WithStatModifiers, _WithCardsModifiers, _WithDurabilitiesModifiers);
				}
			}
		}
		return num;
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00066478 File Offset: 0x00064678
	public float GetAddedSuccessFromCard(InGameCardBase _Card)
	{
		float num = (float)(this.GetSuccessWeight(true, true, true) - this.GetSuccessWeight(true, false, true));
		float num2 = this.GetSuccessPercent(true, true, true) - this.GetSuccessPercent(true, false, true);
		float num3 = 0f;
		for (int i = 0; i < this.DropsInfo.Length; i++)
		{
			if (this.DropsInfo[i].IsSuccess && this.DropsInfo[i].CardWeightMods != null && this.DropsInfo[i].CardWeightMods.Count != 0)
			{
				for (int j = 0; j < this.DropsInfo[i].CardWeightMods.Count; j++)
				{
					if (this.DropsInfo[i].CardWeightMods[j].Card == _Card)
					{
						num3 += this.DropsInfo[i].CardWeightMods[j].BonusWeight;
					}
				}
			}
		}
		if (Mathf.Approximately(0f, num3))
		{
			return 0f;
		}
		return num3 / num * num2;
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x00066594 File Offset: 0x00064794
	public float GetAddedSuccessForStat(InGameStat _Stat)
	{
		float num = (float)(this.GetSuccessWeight(true, true, true) - this.GetSuccessWeight(false, true, true));
		float num2 = this.GetSuccessPercent(true, true, true) - this.GetSuccessPercent(false, true, true);
		float num3 = 0f;
		for (int i = 0; i < this.DropsInfo.Length; i++)
		{
			if (this.DropsInfo[i].IsSuccess && this.DropsInfo[i].StatWeightMods != null && this.DropsInfo[i].StatWeightMods.Count != 0)
			{
				for (int j = 0; j < this.DropsInfo[i].StatWeightMods.Count; j++)
				{
					if (this.DropsInfo[i].StatWeightMods[j].Stat == _Stat.StatModel)
					{
						num3 += (float)this.DropsInfo[i].StatWeightMods[j].BonusWeight;
					}
				}
			}
		}
		if (Mathf.Approximately(0f, num3))
		{
			return 0f;
		}
		return num3 / num * num2;
	}

	// Token: 0x0400114C RID: 4428
	public Vector3Int TickInfo;

	// Token: 0x0400114D RID: 4429
	public InGameCardBase FromCard;

	// Token: 0x0400114E RID: 4430
	public CardData FromData;

	// Token: 0x0400114F RID: 4431
	public CardAction FromAction;

	// Token: 0x04001150 RID: 4432
	public CollectionDropInfo[] DropsInfo;

	// Token: 0x04001151 RID: 4433
	public int TotalValue;

	// Token: 0x04001152 RID: 4434
	public int BaseValue;

	// Token: 0x04001153 RID: 4435
	public float RandomValue;

	// Token: 0x04001154 RID: 4436
	public int SelectedDrop;
}
