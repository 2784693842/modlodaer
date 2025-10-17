using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200012E RID: 302
[CreateAssetMenu(menuName = "Survival/Exclusive Group")]
public class ExclusiveCardOnBoardGroup : UniqueIDScriptable
{
	// Token: 0x060008F3 RID: 2291 RVA: 0x000558F0 File Offset: 0x00053AF0
	public bool CardCanSpawn(CardData _Card)
	{
		if (!_Card)
		{
			return true;
		}
		if (this.ExclusiveCards == null)
		{
			return true;
		}
		if (!this.ExclusiveCards.Contains(_Card))
		{
			return true;
		}
		if (!ExclusiveCardOnBoardGroup.GM)
		{
			ExclusiveCardOnBoardGroup.GM = MBSingleton<GameManager>.Instance;
		}
		if (!ExclusiveCardOnBoardGroup.GM)
		{
			return true;
		}
		for (int i = 0; i < this.ExclusiveCards.Count; i++)
		{
			if (this.ExclusiveCards[i])
			{
				if (this.ExclusiveCards[i].CardType == CardTypes.Event)
				{
					if (ExclusiveCardOnBoardGroup.GM.CurrentEventCard && ExclusiveCardOnBoardGroup.GM.CurrentEventCard.CardModel == this.ExclusiveCards[i])
					{
						return false;
					}
					if (!this.ExclusiveCards[i].UniqueOnBoard)
					{
						goto IL_104;
					}
					if (ExclusiveCardOnBoardGroup.GM.EncounteredEvents.Contains(this.ExclusiveCards[i]))
					{
						return false;
					}
				}
				if (ExclusiveCardOnBoardGroup.GM.CardIsOnBoard(this.ExclusiveCards[i], false, true, false, false, null, Array.Empty<InGameCardBase>()))
				{
					return false;
				}
			}
			IL_104:;
		}
		return true;
	}

	// Token: 0x04000E20 RID: 3616
	public List<CardData> ExclusiveCards;

	// Token: 0x04000E21 RID: 3617
	private static GameManager GM;
}
