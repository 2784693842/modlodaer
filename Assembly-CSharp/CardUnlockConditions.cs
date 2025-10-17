using System;
using System.Collections.Generic;

// Token: 0x0200010F RID: 271
public class CardUnlockConditions
{
	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00053E3F File Offset: 0x0005203F
	// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00053E47 File Offset: 0x00052047
	public CardData UnlockedCard { get; private set; }

	// Token: 0x060008B5 RID: 2229 RVA: 0x00053E50 File Offset: 0x00052050
	public CardUnlockConditions(CardData _FromCard, bool _StartUnlocked)
	{
		this.CardsOnBoard = _FromCard.CardsOnBoard;
		this.TagsOnBoard = _FromCard.TagsOnBoard;
		this.StatValues = _FromCard.StatValues;
		this.TimeValues = _FromCard.TimeValues;
		this.CompletedObjectives = _FromCard.CompletedObjectives;
		this.UnlockedCard = _FromCard;
		this.StartUnlocked = _StartUnlocked;
		this.CollectAllConditions();
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00053EB4 File Offset: 0x000520B4
	public CardAction UnlockAction
	{
		get
		{
			if (!this.UnlockedCard)
			{
				return null;
			}
			return new CardAction((this.UnlockedCard.CardType == CardTypes.Blueprint || this.UnlockedCard.CardType == CardTypes.EnvImprovement) ? LocalizedString.BlueprintUnlocked : this.UnlockedCard.CardName, default(LocalizedString), 0, new List<CardData>
			{
				this.UnlockedCard
			}, null, null)
			{
				ActionLog = this.UnlockedCard.OnUnlockedLog
			};
		}
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00053F34 File Offset: 0x00052134
	public bool IsUnlocked()
	{
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (!this.UnlockedCard)
		{
			return true;
		}
		if (!instance)
		{
			return true;
		}
		if (instance.CardsAboutToBeUnlocked.Contains(this.UnlockedCard))
		{
			return true;
		}
		CardTypes cardType = this.UnlockedCard.CardType;
		if (cardType == CardTypes.Event)
		{
			return !this.UnlockedCard.UniqueOnBoard || instance.EncounteredEvents.Contains(this.UnlockedCard);
		}
		if (cardType != CardTypes.Blueprint)
		{
			return cardType != CardTypes.EnvImprovement || instance.UnlockedImprovements.Contains(this.UnlockedCard);
		}
		return instance.BlueprintModelCards.Contains(this.UnlockedCard);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00053FD4 File Offset: 0x000521D4
	public bool CheckForCompletion()
	{
		if (this.StartUnlocked)
		{
			return true;
		}
		if (this.AllConditions == null)
		{
			this.CollectAllConditions();
		}
		if (CardUnlockConditions.CompletedNames == null)
		{
			CardUnlockConditions.CompletedNames = new List<string>();
		}
		else
		{
			CardUnlockConditions.CompletedNames.Clear();
		}
		for (int i = 0; i < this.AllConditions.Count; i++)
		{
			if (string.IsNullOrEmpty(this.AllConditions[i].ObjectiveName) || !CardUnlockConditions.CompletedNames.Contains(this.AllConditions[i].ObjectiveName))
			{
				this.AllConditions[i].Init();
				if (this.AllConditions[i] is CardOnBoardSubObjective)
				{
					(this.AllConditions[i] as CardOnBoardSubObjective).CheckForCompletion();
				}
				else if (this.AllConditions[i] is TagOnBoardSubObjective)
				{
					(this.AllConditions[i] as TagOnBoardSubObjective).CheckForCompletion();
				}
				else if (this.AllConditions[i] is StatSubObjective)
				{
					(this.AllConditions[i] as StatSubObjective).CheckForCompletion();
				}
				else if (this.AllConditions[i] is TimeObjective)
				{
					(this.AllConditions[i] as TimeObjective).CheckForCompletion();
				}
				else if (this.AllConditions[i] is ObjectiveSubObjective)
				{
					(this.AllConditions[i] as ObjectiveSubObjective).CheckForCompletion(false);
				}
				if (!string.IsNullOrEmpty(this.AllConditions[i].ObjectiveName) && this.AllConditions[i].Complete)
				{
					CardUnlockConditions.CompletedNames.Add(this.AllConditions[i].ObjectiveName);
				}
			}
		}
		for (int j = 0; j < this.AllConditions.Count; j++)
		{
			if (!this.AllConditions[j].Complete && !CardUnlockConditions.CompletedNames.Contains(this.AllConditions[j].ObjectiveName))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x000541E8 File Offset: 0x000523E8
	private void CollectAllConditions()
	{
		this.AllConditions = new List<SubObjective>();
		if (this.CardsOnBoard != null && this.CardsOnBoard.Count > 0)
		{
			this.AllConditions.AddRange(this.CardsOnBoard);
		}
		if (this.TagsOnBoard != null && this.TagsOnBoard.Count > 0)
		{
			this.AllConditions.AddRange(this.TagsOnBoard);
		}
		if (this.StatValues != null && this.StatValues.Count > 0)
		{
			this.AllConditions.AddRange(this.StatValues);
		}
		if (this.TimeValues != null && this.TimeValues.Count > 0)
		{
			this.AllConditions.AddRange(this.TimeValues);
		}
		if (this.CompletedObjectives != null && this.CompletedObjectives.Count > 0)
		{
			this.AllConditions.AddRange(this.CompletedObjectives);
		}
	}

	// Token: 0x04000D0E RID: 3342
	private List<CardOnBoardSubObjective> CardsOnBoard;

	// Token: 0x04000D0F RID: 3343
	private List<TagOnBoardSubObjective> TagsOnBoard;

	// Token: 0x04000D10 RID: 3344
	private List<StatSubObjective> StatValues;

	// Token: 0x04000D11 RID: 3345
	private List<TimeObjective> TimeValues;

	// Token: 0x04000D12 RID: 3346
	private List<ObjectiveSubObjective> CompletedObjectives;

	// Token: 0x04000D13 RID: 3347
	private List<SubObjective> AllConditions;

	// Token: 0x04000D15 RID: 3349
	private bool StartUnlocked;

	// Token: 0x04000D16 RID: 3350
	private static List<string> CompletedNames = new List<string>();

	// Token: 0x04000D17 RID: 3351
	private static CardOnBoardSubObjective CardOnBoard;

	// Token: 0x04000D18 RID: 3352
	private static TagOnBoardSubObjective TagOnBoard;

	// Token: 0x04000D19 RID: 3353
	private static StatSubObjective StatValue;

	// Token: 0x04000D1A RID: 3354
	private static TimeObjective TimeValue;

	// Token: 0x04000D1B RID: 3355
	private static ObjectiveSubObjective CompletedObjective;
}
