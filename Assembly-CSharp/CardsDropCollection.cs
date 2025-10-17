using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000E9 RID: 233
[Serializable]
public class CardsDropCollection
{
	// Token: 0x1700017C RID: 380
	// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0004E879 File Offset: 0x0004CA79
	// (set) Token: 0x060007EA RID: 2026 RVA: 0x0004E881 File Offset: 0x0004CA81
	public List<CardData> CurrentDrop { get; private set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x0004E88A File Offset: 0x0004CA8A
	// (set) Token: 0x060007EC RID: 2028 RVA: 0x0004E892 File Offset: 0x0004CA92
	public LiquidDrop CurrentLiquidDrop { get; private set; }

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x060007ED RID: 2029 RVA: 0x0004E89B File Offset: 0x0004CA9B
	// (set) Token: 0x060007EE RID: 2030 RVA: 0x0004E8A3 File Offset: 0x0004CAA3
	public string SaveDataKey { get; private set; }

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x060007EF RID: 2031 RVA: 0x0004E8AC File Offset: 0x0004CAAC
	// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0004E8B4 File Offset: 0x0004CAB4
	public List<CardData> CurrentSaveDataDrop { get; private set; }

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0004E8BD File Offset: 0x0004CABD
	// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0004E8C5 File Offset: 0x0004CAC5
	public List<StatModifier> CurrentStatModifiers { get; private set; }

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0004E8CE File Offset: 0x0004CACE
	// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0004E8D6 File Offset: 0x0004CAD6
	public string CurrentMessage { get; private set; }

	// Token: 0x060007F5 RID: 2037 RVA: 0x0004E8DF File Offset: 0x0004CADF
	public CollectionDropsSaveData ToSaveData()
	{
		return new CollectionDropsSaveData(this.CollectionName, Vector2Int.up * UnityEngine.Random.Range(this.CollectionUses.x, this.CollectionUses.y + 1));
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0004E913 File Offset: 0x0004CB13
	public bool IsIdentical(CardsDropCollection _To)
	{
		return _To.CollectionName == this.CollectionName;
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x0004E928 File Offset: 0x0004CB28
	public void GetAllPossibleCards(List<CardData> _Cards)
	{
		if (_Cards == null || this.DroppedCards == null)
		{
			return;
		}
		if (this.DroppedCards.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.DroppedCards.Length; i++)
		{
			if (!_Cards.Contains(this.DroppedCards[i].DroppedCard) && this.DroppedCards[i].DroppedCard != null)
			{
				_Cards.Add(this.DroppedCards[i].DroppedCard);
			}
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0004E9A7 File Offset: 0x0004CBA7
	public bool IsTravelDrop
	{
		get
		{
			return this.GetTravelDestination != null;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0004E9B8 File Offset: 0x0004CBB8
	public CardData GetTravelDestination
	{
		get
		{
			if (this.DroppedCards == null)
			{
				return null;
			}
			if (this.DroppedCards.Length == 0)
			{
				return null;
			}
			for (int i = 0; i < this.DroppedCards.Length; i++)
			{
				if (this.DroppedCards[i].Quantity.sqrMagnitude != 0 && this.DroppedCards[i].DroppedCard && this.DroppedCards[i].DroppedCard.CardType == CardTypes.Environment)
				{
					return this.DroppedCards[i].DroppedCard;
				}
			}
			return null;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060007FA RID: 2042 RVA: 0x0004EA49 File Offset: 0x0004CC49
	public bool DropsLiquid
	{
		get
		{
			return !this.CreatedLiquid.IsEmpty;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060007FB RID: 2043 RVA: 0x0004EA5C File Offset: 0x0004CC5C
	public bool HasUnspawnedUniqueCards
	{
		get
		{
			if (this.DroppedCards == null)
			{
				return false;
			}
			if (this.DroppedCards.Length == 0)
			{
				return false;
			}
			if (!MBSingleton<GameManager>.Instance)
			{
				return false;
			}
			for (int i = 0; i < this.DroppedCards.Length; i++)
			{
				if (this.DroppedCards[i].Quantity.sqrMagnitude != 0 && this.DroppedCards[i].DroppedCard && this.DroppedCards[i].DroppedCard.UniqueOnBoard && !MBSingleton<GameManager>.Instance.CardIsOnBoard(this.DroppedCards[i].DroppedCard, false, true, false, false, null, Array.Empty<InGameCardBase>()))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060007FC RID: 2044 RVA: 0x0004EB14 File Offset: 0x0004CD14
	public bool HasUnspawnedEvents
	{
		get
		{
			if (this.DroppedCards == null)
			{
				return false;
			}
			if (this.DroppedCards.Length == 0)
			{
				return false;
			}
			if (!MBSingleton<GameManager>.Instance)
			{
				return false;
			}
			for (int i = 0; i < this.DroppedCards.Length; i++)
			{
				if (this.DroppedCards[i].Quantity.sqrMagnitude != 0 && this.DroppedCards[i].DroppedCard && this.DroppedCards[i].DroppedCard.CardType == CardTypes.Event && !MBSingleton<GameManager>.Instance.EncounteredEvents.Contains(this.DroppedCards[i].DroppedCard))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0004EBC8 File Offset: 0x0004CDC8
	public bool ContainsCard(CardData _Card, bool _CountEmptyDrops)
	{
		if (!_CountEmptyDrops && !_Card)
		{
			return false;
		}
		if (this.DroppedCards == null)
		{
			return false;
		}
		if (this.DroppedCards.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.DroppedCards.Length; i++)
		{
			if ((this.DroppedCards[i].DroppedCard || _CountEmptyDrops) && _Card == this.DroppedCards[i].DroppedCard)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0004EC44 File Offset: 0x0004CE44
	public void SetDroppedCards(List<CardData> _Drops)
	{
		if (_Drops == null)
		{
			return;
		}
		if (_Drops.Count == 0)
		{
			return;
		}
		CardDrop[] array = new CardDrop[_Drops.Count];
		for (int i = 0; i < _Drops.Count; i++)
		{
			array[i] = new CardDrop(_Drops[i]);
		}
		this.SetDroppedCards(array, false);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0004EC96 File Offset: 0x0004CE96
	public void SetDroppedCards(CardDrop[] _Cards, bool _Random)
	{
		if (!_Random)
		{
			this.DroppedCards = _Cards;
		}
		else
		{
			this.DroppedCards = new CardDrop[]
			{
				_Cards[UnityEngine.Random.Range(0, _Cards.Length)]
			};
		}
		this.FillDropList(false, 0);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0004ECCE File Offset: 0x0004CECE
	public void SetLiquidDrop(LiquidDrop _Drop)
	{
		this.CreatedLiquid = _Drop;
		this.FillDropList(false, 0);
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0004ECDF File Offset: 0x0004CEDF
	public void SetStatModifiers(ConditionalStatModifier[] _Mods)
	{
		this.StatModifications = _Mods;
		this.FillStatModsList();
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0004ECF0 File Offset: 0x0004CEF0
	public void FillDropList(bool _CheckEnvironment, int _Multiplier)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.CollectionMessages != null && this.CollectionMessages.Length != 0)
		{
			this.CurrentMessage = this.CollectionMessages[UnityEngine.Random.Range(0, this.CollectionMessages.Length)];
		}
		if (this.CurrentDrop == null)
		{
			this.CurrentDrop = new List<CardData>();
		}
		this.CurrentDrop.Clear();
		if (this.CurrentSaveDataDrop == null)
		{
			this.CurrentSaveDataDrop = new List<CardData>();
		}
		this.CurrentSaveDataDrop.Clear();
		this.CurrentLiquidDrop = default(LiquidDrop);
		if (this.DroppedCards != null)
		{
			for (int i = 0; i < this.DroppedCards.Length; i++)
			{
				if (this.DroppedCards[i].CanDrop)
				{
					if (this.DroppedCards[i].DroppedCard != null)
					{
						if (this.DroppedCards[i].DroppedCard.CardType == CardTypes.Environment && _CheckEnvironment)
						{
							string text = this.DroppedCards[i].DroppedCard.EnvironmentDictionaryKey(MBSingleton<GameManager>.Instance.CurrentEnvironment, MBSingleton<GameManager>.Instance.NextTravelIndex);
							if (MBSingleton<GameManager>.Instance.EnvironmentsData.ContainsKey(text))
							{
								this.CurrentDrop.Clear();
								this.CurrentDrop.Add(this.DroppedCards[i].DroppedCard);
								this.DropInSaveData(this.DroppedCards[i].DroppedCard, text, false);
								break;
							}
							this.CurrentDrop.Clear();
							this.CurrentDrop.Add(this.DroppedCards[i].DroppedCard);
							this.DropInSaveData(this.DroppedCards[i].DroppedCard, text, true);
							break;
						}
						else
						{
							int num = UnityEngine.Random.Range(this.DroppedCards[i].Quantity.x, this.DroppedCards[i].Quantity.y + 1);
							if (_Multiplier > 0)
							{
								num *= _Multiplier;
							}
							if (this.DroppedCards[i].DroppedCard.CardType == CardTypes.Liquid)
							{
								Debug.LogError(this.DroppedCards[i].DroppedCard.name + " is a liquid, it needs to go in the 'Created Liquid' parameter of " + this.CollectionName);
							}
							else
							{
								for (int j = 0; j < num; j++)
								{
									this.CurrentDrop.Add(this.DroppedCards[i].DroppedCard);
								}
							}
						}
					}
					else
					{
						this.CurrentDrop.Add(null);
					}
				}
			}
		}
		if (!this.CreatedLiquid.IsEmpty)
		{
			float num2 = UnityEngine.Random.Range(this.CreatedLiquid.Quantity.x, this.CreatedLiquid.Quantity.y);
			if (num2 > 0f)
			{
				this.CurrentLiquidDrop = new LiquidDrop(this.CreatedLiquid.LiquidCard, Vector2.one * num2, this.CreatedLiquid.LiquidDurabilities);
			}
		}
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0004EFEC File Offset: 0x0004D1EC
	public void FillStatModsList()
	{
		if (this.CurrentStatModifiers == null)
		{
			this.CurrentStatModifiers = new List<StatModifier>();
		}
		else
		{
			this.CurrentStatModifiers.Clear();
		}
		if (this.StatModifications == null)
		{
			return;
		}
		if (this.StatModifications.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.StatModifications.Length; i++)
		{
			if (this.StatModifications[i].Stat && this.StatModifications[i].CheckConditions())
			{
				this.CurrentStatModifiers.Add(this.StatModifications[i].ToStatModifier(true));
			}
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0004F088 File Offset: 0x0004D288
	private void DropInSaveData(CardData _Env, string _ToKey, bool _IncludeDefaultCards)
	{
		this.SaveDataKey = _ToKey;
		if (_IncludeDefaultCards && _Env.DefaultEnvCards != null)
		{
			for (int i = 0; i < _Env.DefaultEnvCards.Length; i++)
			{
				if (!(_Env.DefaultEnvCards[i] == null))
				{
					this.CurrentSaveDataDrop.Add(_Env.DefaultEnvCards[i]);
				}
			}
		}
		for (int j = 0; j < this.DroppedCards.Length; j++)
		{
			if (!(_Env == this.DroppedCards[j].DroppedCard))
			{
				if (this.DroppedCards[j].CanDrop)
				{
					if (!(this.DroppedCards[j].DroppedCard == null))
					{
						int num = UnityEngine.Random.Range(this.DroppedCards[j].Quantity.x, this.DroppedCards[j].Quantity.y + 1);
						for (int k = 0; k < num; k++)
						{
							this.CurrentSaveDataDrop.Add(this.DroppedCards[j].DroppedCard);
						}
					}
				}
				else
				{
					Debug.LogWarning("Cannot create " + this.DroppedCards[j] + " in save data because it cannot spawn on board");
				}
			}
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06000805 RID: 2053 RVA: 0x0004F1C4 File Offset: 0x0004D3C4
	public int TotalPossibleDrops
	{
		get
		{
			int num = 0;
			if (this.DroppedCards != null)
			{
				for (int i = 0; i < this.DroppedCards.Length; i++)
				{
					if (this.DroppedCards[i].CanDrop)
					{
						num += Mathf.Max(this.DroppedCards[i].Quantity.x, this.DroppedCards[i].Quantity.y);
					}
				}
			}
			if (!this.CreatedLiquid.IsEmpty)
			{
				num++;
			}
			if (this.DroppedEncounter)
			{
				num++;
			}
			return Mathf.Max(num, 0);
		}
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0004F260 File Offset: 0x0004D460
	public void GetStatWeightMods(CardData _CardObject, List<StatDropWeightModReport> _Reports, bool _ClearList = true)
	{
		if (_Reports == null)
		{
			return;
		}
		GameManager instance = MBSingleton<GameManager>.Instance;
		if (_ClearList)
		{
			_Reports.Clear();
		}
		if (this.StatsDropChanceModifiers != null && this.StatsDropChanceModifiers.Length != 0)
		{
			for (int i = 0; i < this.StatsDropChanceModifiers.Length; i++)
			{
				if (!this.StatsDropChanceModifiers[i].Stat)
				{
					if (!_CardObject)
					{
						Debug.LogError("Empty drop modifier in collection " + this.CollectionName);
					}
					else
					{
						Debug.LogError("Empty drop modifier in collection " + this.CollectionName + " in " + _CardObject.name, _CardObject);
					}
				}
				else if (instance.StatsDict.ContainsKey(this.StatsDropChanceModifiers[i].Stat) && this.StatsDropChanceModifiers[i].WillHaveEffect(instance.StatsDict[this.StatsDropChanceModifiers[i].Stat].CurrentValue(instance.NotInBase)))
				{
					_Reports.Add(this.StatsDropChanceModifiers[i].ToReport(instance.StatsDict[this.StatsDropChanceModifiers[i].Stat].CurrentValue(instance.NotInBase)));
				}
			}
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0004F3A8 File Offset: 0x0004D5A8
	public void GetCardWeightMods(List<CardDropWeightModReport> _Reports, bool _ClearList = true)
	{
		if (_Reports == null)
		{
			return;
		}
		if (_ClearList)
		{
			_Reports.Clear();
		}
		if (this.CardDropChanceModifiers != null && this.CardDropChanceModifiers.Length != 0)
		{
			for (int i = 0; i < this.CardDropChanceModifiers.Length; i++)
			{
				this.CardDropChanceModifiers[i].GetExtraWeight(_Reports);
			}
		}
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x0004F3F9 File Offset: 0x0004D5F9
	public DurabilityDropWeightModReport GetDurabilitiesWeightMods(InGameCardBase _ForCard)
	{
		return new DurabilityDropWeightModReport(this.DurabilitiesDropChanceModifier, _ForCard);
	}

	// Token: 0x04000BF3 RID: 3059
	public string CollectionName;

	// Token: 0x04000BF4 RID: 3060
	[SerializeField]
	private LocalizedString[] CollectionMessages;

	// Token: 0x04000BF5 RID: 3061
	public bool CountsAsSuccess;

	// Token: 0x04000BF6 RID: 3062
	public bool RevealInventory;

	// Token: 0x04000BF7 RID: 3063
	[MinMax]
	public Vector2Int CollectionUses;

	// Token: 0x04000BF8 RID: 3064
	[Weight]
	public int CollectionWeight;

	// Token: 0x04000BF9 RID: 3065
	[FormerlySerializedAs("DropChanceModifiers")]
	public StatBasedDropChanceModifier[] StatsDropChanceModifiers;

	// Token: 0x04000BFA RID: 3066
	public CardBasedDropChanceModifier[] CardDropChanceModifiers;

	// Token: 0x04000BFB RID: 3067
	public DurabilityBasedDropChanceModifier DurabilitiesDropChanceModifier;

	// Token: 0x04000BFC RID: 3068
	[SerializeField]
	private LiquidDrop CreatedLiquid;

	// Token: 0x04000BFD RID: 3069
	[SerializeField]
	private CardDrop[] DroppedCards;

	// Token: 0x04000BFE RID: 3070
	public Encounter DroppedEncounter;

	// Token: 0x04000BFF RID: 3071
	public ConditionalStatModifier[] StatModifications;

	// Token: 0x04000C00 RID: 3072
	public AddedDurabilityModifier DurabilityModifications;
}
