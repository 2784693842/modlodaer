using System;

// Token: 0x020000D9 RID: 217
[Serializable]
public struct CardInteractionTrigger
{
	// Token: 0x060007C5 RID: 1989 RVA: 0x0004CF6C File Offset: 0x0004B16C
	public bool IsValidTrigger(CardData _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.TriggerCards != null && this.TriggerCards.Length != 0)
		{
			for (int i = 0; i < this.TriggerCards.Length; i++)
			{
				if (this.TriggerCards[i] == _Card)
				{
					return true;
				}
			}
		}
		if (this.TriggerTags != null && this.TriggerTags.Length != 0)
		{
			for (int j = 0; j < this.TriggerTags.Length; j++)
			{
				if (_Card.HasTag(this.TriggerTags[j]))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0004CFF0 File Offset: 0x0004B1F0
	public bool IsEmpty
	{
		get
		{
			if (this.TriggerCards != null && this.TriggerCards.Length != 0)
			{
				for (int i = 0; i < this.TriggerCards.Length; i++)
				{
					if (this.TriggerCards[i])
					{
						return false;
					}
				}
			}
			if (this.TriggerTags == null)
			{
				return true;
			}
			if (this.TriggerTags.Length == 0)
			{
				return true;
			}
			for (int j = 0; j < this.TriggerTags.Length; j++)
			{
				if (this.TriggerTags[j])
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0004D06C File Offset: 0x0004B26C
	public bool IsHandAction
	{
		get
		{
			if (this.TriggerTags != null && this.TriggerTags.Length != 0)
			{
				for (int i = 0; i < this.TriggerTags.Length; i++)
				{
					if (this.TriggerTags[i])
					{
						return false;
					}
				}
			}
			if (this.TriggerCards == null)
			{
				return false;
			}
			if (this.TriggerCards.Length == 0)
			{
				return false;
			}
			for (int j = 0; j < this.TriggerCards.Length; j++)
			{
				if (this.TriggerCards[j] && this.TriggerCards[j].CardType != CardTypes.Hand)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x04000B76 RID: 2934
	public CardData[] TriggerCards;

	// Token: 0x04000B77 RID: 2935
	public CardTag[] TriggerTags;
}
