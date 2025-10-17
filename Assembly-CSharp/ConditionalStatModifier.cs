using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
[Serializable]
public struct ConditionalStatModifier
{
	// Token: 0x0600091C RID: 2332 RVA: 0x0005643A File Offset: 0x0005463A
	public bool CheckConditions()
	{
		return this.CardsAreOk();
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00056444 File Offset: 0x00054644
	private bool CardsAreOk()
	{
		if (this.RequiredCardsOnBoard == null)
		{
			return true;
		}
		if (this.RequiredCardsOnBoard.Length == 0)
		{
			return true;
		}
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (!instance)
		{
			return false;
		}
		for (int i = 0; i < this.RequiredCardsOnBoard.Length; i++)
		{
			if (this.RequiredCardsOnBoard[i] && !instance.CardIsOnBoard(this.RequiredCardsOnBoard[i], true, true, false, false, null, Array.Empty<InGameCardBase>()))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x000564B8 File Offset: 0x000546B8
	public StatModifier ToStatModifier(bool _CollapseRange)
	{
		StatModifier result;
		if (!Mathf.Approximately(this.ValueModifier.x, this.ValueModifier.y))
		{
			result = new StatModifier
			{
				Stat = this.Stat,
				ValueModifier = (_CollapseRange ? (Vector2.one * UnityEngine.Random.Range(this.ValueModifier.x, this.ValueModifier.y)) : this.ValueModifier),
				RateModifier = Vector2.zero
			};
			return result;
		}
		result = new StatModifier
		{
			Stat = this.Stat,
			ValueModifier = Vector2.one * this.ValueModifier.x,
			RateModifier = Vector2.zero
		};
		return result;
	}

	// Token: 0x04000E6D RID: 3693
	public GameStat Stat;

	// Token: 0x04000E6E RID: 3694
	public CardData[] RequiredCardsOnBoard;

	// Token: 0x04000E6F RID: 3695
	[MinMax]
	public Vector2 ValueModifier;

	// Token: 0x04000E70 RID: 3696
	[MinMax]
	public Vector2 RateModifier;
}
