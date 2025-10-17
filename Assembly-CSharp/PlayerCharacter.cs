using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000150 RID: 336
[CreateAssetMenu(menuName = "Survival/PlayerCharacter")]
public class PlayerCharacter : UniqueIDScriptable
{
	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000972 RID: 2418 RVA: 0x00058200 File Offset: 0x00056400
	public bool StartUnlocked
	{
		get
		{
			return this.SunsCost <= 0 && this.MoonsCost <= 0;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000973 RID: 2419 RVA: 0x00058219 File Offset: 0x00056419
	public bool IsUnlocked
	{
		get
		{
			return this.StartUnlocked || UniqueIDScriptable.ListContains(GameLoad.Instance.SaveData.UnlockedCharacters, this);
		}
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0005823C File Offset: 0x0005643C
	public void Unlock()
	{
		if (!GameLoad.Instance.SaveData.UnlockedCharacters.Contains(UniqueIDScriptable.SaveID(this)))
		{
			GameLoad.Instance.SaveData.UnlockedCharacters.Add(UniqueIDScriptable.SaveID(this));
			GameLoad.Instance.SaveMainDataToFile();
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000975 RID: 2421 RVA: 0x0005828C File Offset: 0x0005648C
	public int AddedHours
	{
		get
		{
			if (this.CharacterPerks == null)
			{
				return this.AddedStartingHour;
			}
			if (this.CharacterPerks.Count == 0)
			{
				return this.AddedStartingHour;
			}
			for (int i = 0; i < this.CharacterPerks.Count; i++)
			{
				if (this.CharacterPerks[i] && this.CharacterPerks[i].AddedStartingHours > 0)
				{
					return this.CharacterPerks[i].AddedStartingHours;
				}
			}
			return this.AddedStartingHour;
		}
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00058314 File Offset: 0x00056514
	public string NameAndDifficulty(GlobalCharacterInfo _Info, bool _Brackets)
	{
		StringBuilder stringBuilder = new StringBuilder(this.CharacterName);
		if (!_Brackets)
		{
			stringBuilder.Append("\n<size=75%>");
			stringBuilder.Append(_Info.GetRating(this.CharacterScore));
		}
		else
		{
			stringBuilder.Append(" (");
			stringBuilder.Append(_Info.GetRating(this.CharacterScore));
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00058388 File Offset: 0x00056588
	public void CopyCards(PlayerCharacter _Model)
	{
		this.AddedStartingHour = _Model.AddedStartingHour;
		this.Environment = _Model.Environment;
		this.Weather = _Model.Weather;
		if (_Model.Locations != null)
		{
			this.Locations = new CardData[_Model.Locations.Length];
			_Model.Locations.CopyTo(this.Locations, 0);
		}
		else
		{
			this.Locations = new CardData[0];
		}
		if (_Model.BaseAndItemCards != null)
		{
			this.BaseAndItemCards = new CardData[_Model.BaseAndItemCards.Length];
			_Model.BaseAndItemCards.CopyTo(this.BaseAndItemCards, 0);
		}
		else
		{
			this.BaseAndItemCards = new CardData[0];
		}
		if (_Model.StartingClothes != null)
		{
			this.StartingClothes = new CardData[_Model.StartingClothes.Length];
			_Model.StartingClothes.CopyTo(this.StartingClothes, 0);
		}
		else
		{
			this.StartingClothes = new CardData[0];
		}
		if (_Model.InitialStatModifiers != null)
		{
			this.InitialStatModifiers = new StatModifier[_Model.InitialStatModifiers.Length];
			_Model.InitialStatModifiers.CopyTo(this.InitialStatModifiers, 0);
		}
		else
		{
			this.InitialStatModifiers = new StatModifier[0];
		}
		this.Journal = _Model.Journal;
		this.Guide = _Model.Guide;
		this.EasyPackage = _Model.EasyPackage;
		this.SunsCost = _Model.SunsCost;
		this.MoonsCost = _Model.MoonsCost;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x000584E4 File Offset: 0x000566E4
	public void LoadCustomCharacter(PlayerCharacterSaveData _FromData, Sprite _Portrait)
	{
		this.CustomCharacter = true;
		this.CharacterName = new LocalizedString
		{
			DefaultText = _FromData.CharacterName
		};
		this.CharacterDescription = new LocalizedString
		{
			DefaultText = _FromData.CharacterBio
		};
		this.CharacterPortrait = _Portrait;
		this.PortraitID = _FromData.PortraitID;
		if (_FromData.CharacterPerks == null)
		{
			return;
		}
		if (_FromData.CharacterPerks.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _FromData.CharacterPerks.Count; i++)
		{
			CharacterPerk fromID = UniqueIDScriptable.GetFromID<CharacterPerk>(UniqueIDScriptable.LoadID(_FromData.CharacterPerks[i]));
			if (fromID)
			{
				this.CharacterPerks.Add(fromID);
			}
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000979 RID: 2425 RVA: 0x0005859C File Offset: 0x0005679C
	public bool OverridesClothes
	{
		get
		{
			if (this.CharacterPerks == null)
			{
				return false;
			}
			if (this.CharacterPerks.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.CharacterPerks.Count; i++)
			{
				if (this.CharacterPerks[i].OverrideEquipment)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x000585F0 File Offset: 0x000567F0
	public bool OverrideLocations
	{
		get
		{
			if (this.CharacterPerks == null)
			{
				return false;
			}
			if (this.CharacterPerks.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.CharacterPerks.Count; i++)
			{
				if (this.CharacterPerks[i].OverrideEnvironment)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600097B RID: 2427 RVA: 0x00058648 File Offset: 0x00056848
	public int CharacterScore
	{
		get
		{
			if (this.CharacterPerks == null)
			{
				return 0;
			}
			if (this.CharacterPerks.Count == 0)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.CharacterPerks.Count; i++)
			{
				if (this.CharacterPerks[i])
				{
					num += this.CharacterPerks[i].DifficultyRating;
				}
			}
			return num;
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x000586B0 File Offset: 0x000568B0
	public List<CardData> GetStartingCards(bool _Clothes, bool _Items, bool _Packages)
	{
		List<CardData> result = new List<CardData>();
		if (_Clothes)
		{
			this.AddCardsToList(this.StartingClothes, ref result);
		}
		if (_Items)
		{
			this.AddCardsToList(this.BaseAndItemCards, ref result);
		}
		if (_Packages && this.EasyPackage)
		{
			this.AddCardsToList(this.EasyPackage.AddedCards, ref result);
		}
		return result;
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0005870C File Offset: 0x0005690C
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

	// Token: 0x04000EF6 RID: 3830
	public LocalizedString CharacterName;

	// Token: 0x04000EF7 RID: 3831
	public LocalizedString CharacterDescription;

	// Token: 0x04000EF8 RID: 3832
	public Sprite CharacterPortrait;

	// Token: 0x04000EF9 RID: 3833
	public bool EditorOnly;

	// Token: 0x04000EFA RID: 3834
	public int SunsCost;

	// Token: 0x04000EFB RID: 3835
	public int MoonsCost;

	// Token: 0x04000EFC RID: 3836
	[SpecialHeader("Setup", HeaderSizes.Normal, HeaderStyles.Underlined, 0f)]
	[SerializeField]
	private int AddedStartingHour;

	// Token: 0x04000EFD RID: 3837
	public CardData Environment;

	// Token: 0x04000EFE RID: 3838
	public CardData Weather;

	// Token: 0x04000EFF RID: 3839
	public CardData[] Locations;

	// Token: 0x04000F00 RID: 3840
	public CardData[] BaseAndItemCards;

	// Token: 0x04000F01 RID: 3841
	public CardData[] StartingClothes;

	// Token: 0x04000F02 RID: 3842
	[StatModifierOptions(false, false)]
	public StatModifier[] InitialStatModifiers;

	// Token: 0x04000F03 RID: 3843
	public ContentDisplayer Journal;

	// Token: 0x04000F04 RID: 3844
	public ContentDisplayer Guide;

	// Token: 0x04000F05 RID: 3845
	public GameModifierPackage EasyPackage;

	// Token: 0x04000F06 RID: 3846
	[Space]
	public List<CharacterPerk> CharacterPerks = new List<CharacterPerk>();

	// Token: 0x04000F07 RID: 3847
	[NonSerialized]
	public bool CustomCharacter;

	// Token: 0x04000F08 RID: 3848
	[NonSerialized]
	public string PortraitID;
}
