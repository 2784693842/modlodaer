using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
[Serializable]
public struct StatTimeOfDayModifier
{
	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x0600092D RID: 2349 RVA: 0x00056BE6 File Offset: 0x00054DE6
	public Vector2Int TimeRange
	{
		get
		{
			return new Vector2Int(this.StartingHour, this.EndingHour);
		}
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00056BF9 File Offset: 0x00054DF9
	public bool EffectIsActive(float _DaylightPoints)
	{
		return this.IsInRange(_DaylightPoints) && this.StatsValid() && this.CardsAndTagsValid();
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00056C16 File Offset: 0x00054E16
	private bool IsInRange(float _DaylightPointsToHour)
	{
		if (this.StartingHour > this.EndingHour)
		{
			return _DaylightPointsToHour >= (float)this.StartingHour || _DaylightPointsToHour < (float)this.EndingHour;
		}
		return _DaylightPointsToHour >= (float)this.StartingHour && _DaylightPointsToHour < (float)this.EndingHour;
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06000930 RID: 2352 RVA: 0x00056C54 File Offset: 0x00054E54
	public bool HasStatRequirements
	{
		get
		{
			if (this.RequiredStatValues == null)
			{
				return false;
			}
			if (this.RequiredStatValues.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.RequiredStatValues.Length; i++)
			{
				if (this.RequiredStatValues[i].Stat)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x00056CA4 File Offset: 0x00054EA4
	private bool StatsValid()
	{
		if (this.RequiredStatValues == null)
		{
			return true;
		}
		if (this.RequiredStatValues.Length == 0)
		{
			return true;
		}
		GameManager instance = MBSingleton<GameManager>.Instance;
		for (int i = 0; i < this.RequiredStatValues.Length; i++)
		{
			if (this.RequiredStatValues[i].Stat && !this.RequiredStatValues[i].IsInRange(instance.StatsDict[this.RequiredStatValues[i].Stat].CurrentValue(instance.NotInBase)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000932 RID: 2354 RVA: 0x00056D34 File Offset: 0x00054F34
	public bool HasCardsOrTagRequirements
	{
		get
		{
			if (this.RequiredCardsOrTagsOnBoard == null)
			{
				return false;
			}
			if (this.RequiredCardsOrTagsOnBoard.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.RequiredCardsOrTagsOnBoard.Length; i++)
			{
				if (this.RequiredCardsOrTagsOnBoard[i].IsValid)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00056D80 File Offset: 0x00054F80
	private bool CardsAndTagsValid()
	{
		if (this.RequiredCardsOrTagsOnBoard == null)
		{
			return true;
		}
		if (this.RequiredCardsOrTagsOnBoard.Length == 0)
		{
			return true;
		}
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (!instance)
		{
			return false;
		}
		for (int i = 0; i < this.RequiredCardsOrTagsOnBoard.Length; i++)
		{
			if (this.RequiredCardsOrTagsOnBoard[i].IsValid)
			{
				if (this.RequiredCardsOrTagsOnBoard[i].TargetIsCard && !instance.CardIsOnBoard(this.RequiredCardsOrTagsOnBoard[i].Card, true, true, false, false, null, Array.Empty<InGameCardBase>()))
				{
					return false;
				}
				if (this.RequiredCardsOrTagsOnBoard[i].TargetIsTag && !instance.TagIsOnBoard(this.RequiredCardsOrTagsOnBoard[i].Tag, true, true, false, false, null))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00056E4C File Offset: 0x0005504C
	public StatTimeOfDayModifier Instantiate()
	{
		return new StatTimeOfDayModifier
		{
			StartingHour = this.StartingHour,
			EndingHour = this.EndingHour,
			RequiredStatValues = this.RequiredStatValues,
			RequiredCardsOrTagsOnBoard = this.RequiredCardsOrTagsOnBoard,
			ValueModifier = (Mathf.Approximately(this.ValueModifier.x, this.ValueModifier.y) ? (Vector2.one * this.ValueModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.ValueModifier.x, this.ValueModifier.y))),
			RateModifier = (Mathf.Approximately(this.RateModifier.x, this.RateModifier.y) ? (Vector2.one * this.RateModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.RateModifier.x, this.RateModifier.y)))
		};
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00056F58 File Offset: 0x00055158
	public StatModifier ToStatMod(GameStat _Stat)
	{
		return new StatModifier
		{
			Stat = _Stat,
			ValueModifier = this.ValueModifier,
			RateModifier = this.RateModifier
		};
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x00056F90 File Offset: 0x00055190
	public TimeOfDayStatModSource ToSource
	{
		get
		{
			return new TimeOfDayStatModSource(string.Format("{0}:00", this.StartingHour.ToString("00")), string.Format("{0}:00", this.EndingHour.ToString("00")));
		}
	}

	// Token: 0x04000EA5 RID: 3749
	public int StartingHour;

	// Token: 0x04000EA6 RID: 3750
	public int EndingHour;

	// Token: 0x04000EA7 RID: 3751
	public CardOrTagRef[] RequiredCardsOrTagsOnBoard;

	// Token: 0x04000EA8 RID: 3752
	public StatValueTrigger[] RequiredStatValues;

	// Token: 0x04000EA9 RID: 3753
	[MinMax]
	public Vector2 ValueModifier;

	// Token: 0x04000EAA RID: 3754
	[MinMax]
	public Vector2 RateModifier;
}
