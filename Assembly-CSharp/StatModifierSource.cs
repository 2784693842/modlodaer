using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
[Serializable]
public struct StatModifierSource
{
	// Token: 0x06000249 RID: 585 RVA: 0x00017318 File Offset: 0x00015518
	public StatModifierSource(float _Value, float _Rate, object _Source)
	{
		this.Value = _Value;
		this.Rate = _Rate;
		this.Stat = null;
		this.Card = null;
		this.Character = null;
		this.Perk = null;
		this.TimeOfDay = null;
		if (_Source != null && _Source != null)
		{
			InGameStat inGameStat;
			if ((inGameStat = (_Source as InGameStat)) != null)
			{
				InGameStat stat = inGameStat;
				this.Stat = stat;
				return;
			}
			InGameCardBase inGameCardBase;
			if ((inGameCardBase = (_Source as InGameCardBase)) != null)
			{
				InGameCardBase card = inGameCardBase;
				this.Card = card;
				return;
			}
			PlayerCharacter playerCharacter;
			if ((playerCharacter = (_Source as PlayerCharacter)) != null)
			{
				PlayerCharacter character = playerCharacter;
				this.Character = character;
				return;
			}
			CharacterPerk characterPerk;
			if ((characterPerk = (_Source as CharacterPerk)) != null)
			{
				CharacterPerk perk = characterPerk;
				this.Perk = perk;
				return;
			}
			TimeOfDayStatModSource timeOfDayStatModSource;
			if ((timeOfDayStatModSource = (_Source as TimeOfDayStatModSource)) != null)
			{
				TimeOfDayStatModSource timeOfDay = timeOfDayStatModSource;
				this.TimeOfDay = timeOfDay;
			}
		}
	}

	// Token: 0x0600024A RID: 586 RVA: 0x000173D4 File Offset: 0x000155D4
	public StatModifierSource(object _Source)
	{
		this.Value = 0f;
		this.Rate = 0f;
		this.Stat = null;
		this.Card = null;
		this.Character = null;
		this.Perk = null;
		this.TimeOfDay = null;
		if (_Source != null && _Source != null)
		{
			InGameStat inGameStat;
			if ((inGameStat = (_Source as InGameStat)) != null)
			{
				InGameStat stat = inGameStat;
				this.Stat = stat;
				return;
			}
			InGameCardBase inGameCardBase;
			if ((inGameCardBase = (_Source as InGameCardBase)) != null)
			{
				InGameCardBase card = inGameCardBase;
				this.Card = card;
				return;
			}
			PlayerCharacter playerCharacter;
			if ((playerCharacter = (_Source as PlayerCharacter)) != null)
			{
				PlayerCharacter character = playerCharacter;
				this.Character = character;
				return;
			}
			CharacterPerk characterPerk;
			if ((characterPerk = (_Source as CharacterPerk)) != null)
			{
				CharacterPerk perk = characterPerk;
				this.Perk = perk;
				return;
			}
			TimeOfDayStatModSource timeOfDayStatModSource;
			if ((timeOfDayStatModSource = (_Source as TimeOfDayStatModSource)) != null)
			{
				TimeOfDayStatModSource timeOfDay = timeOfDayStatModSource;
				this.TimeOfDay = timeOfDay;
			}
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600024B RID: 587 RVA: 0x00017498 File Offset: 0x00015698
	public object GetSource
	{
		get
		{
			if (this.Stat != null)
			{
				return this.Stat;
			}
			if (this.Card != null)
			{
				return this.Card;
			}
			if (this.Character != null)
			{
				return this.Character;
			}
			if (this.Perk != null)
			{
				return this.Perk;
			}
			if (this.TimeOfDay != null)
			{
				return this.TimeOfDay;
			}
			return null;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600024C RID: 588 RVA: 0x00017509 File Offset: 0x00015709
	public static StatModifierSource None
	{
		get
		{
			return new StatModifierSource(0f, 0f, null);
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600024D RID: 589 RVA: 0x0001751C File Offset: 0x0001571C
	public bool ValidSource
	{
		get
		{
			return this.Stat != null || this.Card != null || this.Character != null || this.Perk != null || this.TimeOfDay != null;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600024E RID: 590 RVA: 0x0001756C File Offset: 0x0001576C
	public bool ValidNumbers
	{
		get
		{
			return !Mathf.Approximately(this.Value, 0f) || !Mathf.Approximately(this.Rate, 0f);
		}
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00017598 File Offset: 0x00015798
	public bool IsIdentical(StatModifierSource _To, bool _Inverse)
	{
		float value = this.Value;
		float rate = this.Rate;
		float b = _Inverse ? (-_To.Value) : _To.Value;
		float b2 = _Inverse ? (-_To.Rate) : _To.Rate;
		return _To.Stat == this.Stat && _To.Card == this.Card && _To.Character == this.Character && _To.Perk == this.Perk && Mathf.Approximately(value, b) && Mathf.Approximately(rate, b2) && TimeOfDayStatModSource.IdenticalTimes(_To.TimeOfDay, this.TimeOfDay);
	}

	// Token: 0x0400027E RID: 638
	public float Value;

	// Token: 0x0400027F RID: 639
	public float Rate;

	// Token: 0x04000280 RID: 640
	public InGameStat Stat;

	// Token: 0x04000281 RID: 641
	public InGameCardBase Card;

	// Token: 0x04000282 RID: 642
	public PlayerCharacter Character;

	// Token: 0x04000283 RID: 643
	public CharacterPerk Perk;

	// Token: 0x04000284 RID: 644
	public TimeOfDayStatModSource TimeOfDay;
}
