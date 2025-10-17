using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E5 RID: 229
[Serializable]
public class ExtraDurabilityChange
{
	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060007DA RID: 2010 RVA: 0x0004DBD8 File Offset: 0x0004BDD8
	public bool CanSendToEnvironment
	{
		get
		{
			if (this.SendToEnvironment == null)
			{
				return false;
			}
			if (this.SendToEnvironment.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.SendToEnvironment.Length; i++)
			{
				if (this.SendToEnvironment[i] != null && this.SendToEnvironment[i].Card)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0004DC30 File Offset: 0x0004BE30
	public bool AppliesToAllCards(bool _IncludeNOTAffectedCards)
	{
		if (this.AffectedCards == null && this.AffectedTags == null && this.NOTAffectedThings == null)
		{
			return true;
		}
		if (this.AffectedCards == null && this.AffectedTags == null && !_IncludeNOTAffectedCards)
		{
			return true;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (this.AffectedCards != null)
		{
			flag = (this.AffectedCards.Count > 0);
		}
		if (this.AffectedTags != null)
		{
			flag2 = (this.AffectedTags.Length != 0);
		}
		if (this.NOTAffectedThings != null)
		{
			flag3 = (this.NOTAffectedThings.Length != 0);
		}
		return !flag && !flag2 && (!flag3 || !_IncludeNOTAffectedCards);
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0004DCC0 File Offset: 0x0004BEC0
	public bool AffectsCard(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.AppliesToAllCards(true))
		{
			return true;
		}
		if (this.NOTAffectedThings != null)
		{
			for (int i = 0; i < this.NOTAffectedThings.Length; i++)
			{
				if (this.NOTAffectedThings[i].CheckCard(_Card))
				{
					return false;
				}
			}
		}
		return this.AppliesToAllCards(false) || (this.AffectedCards != null && this.AffectedCards.Contains(_Card.CardModel)) || (this.AffectedTags != null && _Card.CardModel.HasAnyTag(this.AffectedTags));
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0004DD5C File Offset: 0x0004BF5C
	public static ExtraDurabilityChange operator *(ExtraDurabilityChange _Changes, float _Mult)
	{
		return new ExtraDurabilityChange
		{
			AffectedCards = _Changes.AffectedCards,
			AffectedTags = _Changes.AffectedTags,
			NOTAffectedThings = _Changes.NOTAffectedThings,
			AppliesTo = _Changes.AppliesTo,
			CardChanges = _Changes.CardChanges,
			RandomlyAffectedCards = _Changes.RandomlyAffectedCards,
			SpoilageChange = new Vector2(_Changes.SpoilageChange.x * _Mult, _Changes.SpoilageChange.y * _Mult),
			UsageChange = new Vector2(_Changes.UsageChange.x * _Mult, _Changes.UsageChange.y * _Mult),
			FuelChange = new Vector2(_Changes.FuelChange.x * _Mult, _Changes.FuelChange.y * _Mult),
			ChargesChange = new Vector2(_Changes.ChargesChange.x * _Mult, _Changes.ChargesChange.y * _Mult),
			Special1Change = new Vector2(_Changes.Special1Change.x * _Mult, _Changes.Special1Change.y * _Mult),
			Special2Change = new Vector2(_Changes.Special2Change.x * _Mult, _Changes.Special2Change.y * _Mult),
			Special3Change = new Vector2(_Changes.Special3Change.x * _Mult, _Changes.Special3Change.y * _Mult),
			Special4Change = new Vector2(_Changes.Special4Change.x * _Mult, _Changes.Special4Change.y * _Mult)
		};
	}

	// Token: 0x04000BC4 RID: 3012
	public List<CardData> AffectedCards;

	// Token: 0x04000BC5 RID: 3013
	public CardTag[] AffectedTags;

	// Token: 0x04000BC6 RID: 3014
	public CardOrTagRef[] NOTAffectedThings;

	// Token: 0x04000BC7 RID: 3015
	public RemoteDurabilityChanges AppliesTo;

	// Token: 0x04000BC8 RID: 3016
	public bool LookInInventories;

	// Token: 0x04000BC9 RID: 3017
	public DurabilitiesConditions RequiredDurabilities;

	// Token: 0x04000BCA RID: 3018
	[MinMax]
	public Vector2 SpoilageChange;

	// Token: 0x04000BCB RID: 3019
	[MinMax]
	public Vector2 UsageChange;

	// Token: 0x04000BCC RID: 3020
	[MinMax]
	public Vector2 FuelChange;

	// Token: 0x04000BCD RID: 3021
	[MinMax]
	public Vector2 ChargesChange;

	// Token: 0x04000BCE RID: 3022
	[MinMax]
	public Vector2 LiquidChange;

	// Token: 0x04000BCF RID: 3023
	[MinMax]
	public Vector2 Special1Change;

	// Token: 0x04000BD0 RID: 3024
	[MinMax]
	public Vector2 Special2Change;

	// Token: 0x04000BD1 RID: 3025
	[MinMax]
	public Vector2 Special3Change;

	// Token: 0x04000BD2 RID: 3026
	[MinMax]
	public Vector2 Special4Change;

	// Token: 0x04000BD3 RID: 3027
	public RemoteCardStateChanges CardChanges;

	// Token: 0x04000BD4 RID: 3028
	public bool DropOnDestroyList;

	// Token: 0x04000BD5 RID: 3029
	public float RandomlyAffectedCards;

	// Token: 0x04000BD6 RID: 3030
	public RandomAffectCardsTypes RandomAmountType;

	// Token: 0x04000BD7 RID: 3031
	public EnvironmentCardDataRef[] SendToEnvironment;
}
