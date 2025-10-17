using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000140 RID: 320
[CreateAssetMenu(menuName = "Survival/Gamemode")]
public class Gamemode : UniqueIDScriptable
{
	// Token: 0x06000938 RID: 2360 RVA: 0x00056FE0 File Offset: 0x000551E0
	public List<CardData> GetStartingCards()
	{
		List<CardData> result = new List<CardData>();
		this.AddCardsToList(this.BaseAndItemCards, ref result);
		return result;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x00057004 File Offset: 0x00055204
	private void AddCardsToList(CardData[] _Cards, ref List<CardData> _List)
	{
		if (_Cards == null || _List == null)
		{
			return;
		}
		for (int i = 0; i < _Cards.Length; i++)
		{
			if (_Cards[i] && _Cards[i].CardType != CardTypes.Event)
			{
				_List.Add(_Cards[i]);
			}
		}
	}

	// Token: 0x04000EAD RID: 3757
	public LocalizedString ModeName;

	// Token: 0x04000EAE RID: 3758
	public LocalizedString ModeDescription;

	// Token: 0x04000EAF RID: 3759
	public Sprite ModeIllustration;

	// Token: 0x04000EB0 RID: 3760
	[SpecialHeader("Setup", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	public CardData[] BaseAndItemCards;

	// Token: 0x04000EB1 RID: 3761
	public PlayerCharacter[] PlayableCharacters;

	// Token: 0x04000EB2 RID: 3762
	public PlayerCharacter[] PremadeCustomCharacters;

	// Token: 0x04000EB3 RID: 3763
	[StatModifierOptions(false, false)]
	public StatModifier[] InitialStatModifiers;

	// Token: 0x04000EB4 RID: 3764
	[Header("OLD")]
	public CardData Environment;

	// Token: 0x04000EB5 RID: 3765
	public CardData Weather;

	// Token: 0x04000EB6 RID: 3766
	public CardData[] Locations;
}
