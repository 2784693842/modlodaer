using System;
using System.Collections.Generic;

// Token: 0x02000184 RID: 388
[Serializable]
public class EnvironmentSaveData
{
	// Token: 0x06000A4E RID: 2638 RVA: 0x0005BA30 File Offset: 0x00059C30
	public EnvironmentSaveData(CardData _Env, int _Tick, string _Key)
	{
		this.DictionaryKey = _Key;
		this.EnvironmentID = UniqueIDScriptable.SaveID(_Env);
		this.LastUpdatedTick = _Tick;
		this.CurrentMaxWeight = _Env.GetWeightCapacity(0f);
		this.BookmarkedCardsIDs = new string[0];
		this.BookmarkedLiquidsIDs = new string[0];
		this.CheckedImprovements = new List<string>();
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x0005BAC8 File Offset: 0x00059CC8
	public void FillCounters(List<InGameTickCounter> _From)
	{
		this.LocalCounterValues = new List<InGameTickCounter>();
		this.CountersDict = new Dictionary<LocalTickCounter, InGameTickCounter>();
		if (_From == null)
		{
			return;
		}
		for (int i = 0; i < _From.Count; i++)
		{
			this.LocalCounterValues.Add(new InGameTickCounter(_From[i]));
			this.CountersDict.Add(_From[i].Model, this.LocalCounterValues[i]);
		}
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x0005BB3C File Offset: 0x00059D3C
	public void FillCounterDictionnary(List<InGameTickCounter> _AllCounters)
	{
		if (this.LocalCounterValues == null)
		{
			this.FillCounters(_AllCounters);
			return;
		}
		this.CountersDict = new Dictionary<LocalTickCounter, InGameTickCounter>();
		List<InGameTickCounter> list = new List<InGameTickCounter>();
		for (int i = 0; i < this.LocalCounterValues.Count; i++)
		{
			list.Add(new InGameTickCounter(this.LocalCounterValues[i]));
		}
		this.LocalCounterValues.Clear();
		for (int j = 0; j < _AllCounters.Count; j++)
		{
			if (!_AllCounters[j].Model)
			{
				this.LocalCounterValues.Add(new InGameTickCounter(_AllCounters[j]));
			}
			else
			{
				bool flag = false;
				for (int k = 0; k < list.Count; k++)
				{
					if (!string.IsNullOrEmpty(list[j].ModelID) && list[k].ModelID == _AllCounters[j].ModelID)
					{
						list[k].Model = _AllCounters[j].Model;
						this.LocalCounterValues.Add(new InGameTickCounter(list[k]));
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.LocalCounterValues.Add(new InGameTickCounter(_AllCounters[j]));
				}
				this.CountersDict.Add(this.LocalCounterValues[j].Model, this.LocalCounterValues[j]);
			}
		}
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x0005BCA9 File Offset: 0x00059EA9
	public void SortCards()
	{
		this.AllRegularCards.Sort(new CardSaveDataComparer());
		this.AllInventoryCards.Sort(new CardSaveDataComparer());
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x0005BCCB File Offset: 0x00059ECB
	public void CheckImprovement(CardData _Card)
	{
		if (this.CheckedImprovements == null)
		{
			this.CheckedImprovements = new List<string>();
		}
		if (this.CheckedImprovements.Contains(_Card.UniqueID))
		{
			return;
		}
		this.CheckedImprovements.Add(_Card.UniqueID);
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x0005BD05 File Offset: 0x00059F05
	public bool ImprovementWasChecked(CardData _Card)
	{
		return !_Card || (this.CheckedImprovements != null && this.CheckedImprovements.Contains(_Card.UniqueID));
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x0005BD2C File Offset: 0x00059F2C
	public bool HasPinData(CardData _Card)
	{
		if (this.AllPinnedCards == null)
		{
			return false;
		}
		if (this.AllPinnedCards.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < this.AllPinnedCards.Count; i++)
		{
			if (UniqueIDScriptable.LoadID(this.AllPinnedCards[i].CardID) == _Card.UniqueID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x0005BD90 File Offset: 0x00059F90
	public bool ContainsCard(CardData _Card)
	{
		if (!_Card)
		{
			return true;
		}
		for (int i = 0; i < this.AllRegularCards.Count; i++)
		{
			if (UniqueIDScriptable.LoadID(this.AllRegularCards[i].CardID) == _Card.UniqueID)
			{
				return true;
			}
		}
		for (int j = 0; j < this.AllInventoryCards.Count; j++)
		{
			if (UniqueIDScriptable.LoadID(this.AllInventoryCards[j].CardID) == _Card.UniqueID)
			{
				return true;
			}
		}
		for (int k = 0; k < this.NestedInventoryCards.Count; k++)
		{
			if (UniqueIDScriptable.LoadID(this.NestedInventoryCards[k].CardID) == _Card.UniqueID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000FF2 RID: 4082
	public string DictionaryKey;

	// Token: 0x04000FF3 RID: 4083
	public string EnvironmentID;

	// Token: 0x04000FF4 RID: 4084
	public int LastUpdatedTick;

	// Token: 0x04000FF5 RID: 4085
	public List<CardSaveData> AllRegularCards = new List<CardSaveData>();

	// Token: 0x04000FF6 RID: 4086
	public List<PinSaveData> AllPinnedCards = new List<PinSaveData>();

	// Token: 0x04000FF7 RID: 4087
	public List<InventoryCardSaveData> AllInventoryCards = new List<InventoryCardSaveData>();

	// Token: 0x04000FF8 RID: 4088
	public List<InventoryCardSaveData> NestedInventoryCards = new List<InventoryCardSaveData>();

	// Token: 0x04000FF9 RID: 4089
	public string[] BookmarkedCardsIDs;

	// Token: 0x04000FFA RID: 4090
	public string[] BookmarkedLiquidsIDs;

	// Token: 0x04000FFB RID: 4091
	public List<string> CheckedImprovements = new List<string>();

	// Token: 0x04000FFC RID: 4092
	public float CurrentWeight;

	// Token: 0x04000FFD RID: 4093
	public float CurrentMaxWeight;

	// Token: 0x04000FFE RID: 4094
	public List<InGameTickCounter> LocalCounterValues;

	// Token: 0x04000FFF RID: 4095
	public Dictionary<LocalTickCounter, InGameTickCounter> CountersDict;
}
