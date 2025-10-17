using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F8 RID: 248
[Serializable]
public class CookingRecipe
{
	// Token: 0x17000191 RID: 401
	// (get) Token: 0x0600082F RID: 2095 RVA: 0x0005135D File Offset: 0x0004F55D
	public bool FillsCookerLiquid
	{
		get
		{
			return Mathf.Max(this.CookerChanges.LiquidQuantityChange.x, this.CookerChanges.LiquidQuantityChange.y) > 0f;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000830 RID: 2096 RVA: 0x0005138C File Offset: 0x0004F58C
	public bool FillsIngredientLiquid
	{
		get
		{
			if (Mathf.Max(this.IngredientChanges.LiquidQuantityChange.x, this.IngredientChanges.LiquidQuantityChange.y) > 0f)
			{
				return true;
			}
			if (this.DropsAsCollection != null && this.DropsAsCollection.Length != 0)
			{
				for (int i = 0; i < this.DropsAsCollection.Length; i++)
				{
					if (this.DropsAsCollection[i].DropsLiquid)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000831 RID: 2097 RVA: 0x00051400 File Offset: 0x0004F600
	public List<DurabilityChangeFeedbackInfo> IngredientDurabilityChangesList
	{
		get
		{
			if (this.IngredientChanges.ModType == CardModifications.Destroy || this.IngredientChanges.ModType == CardModifications.None)
			{
				return null;
			}
			if (!this.IngredientChanges.ModifyDurability && this.IngredientChanges.ModType != CardModifications.DurabilityChanges)
			{
				return null;
			}
			List<DurabilityChangeFeedbackInfo> list = new List<DurabilityChangeFeedbackInfo>();
			int num = this.IngredientDurabilityChanges(DurabilitiesTypes.Spoilage);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Spoilage, num));
			}
			num = this.IngredientDurabilityChanges(DurabilitiesTypes.Usage);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Usage, num));
			}
			num = this.IngredientDurabilityChanges(DurabilitiesTypes.Fuel);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Fuel, num));
			}
			num = this.IngredientDurabilityChanges(DurabilitiesTypes.Progress);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Progress, num));
			}
			num = this.IngredientDurabilityChanges(DurabilitiesTypes.Special1);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Special1, num));
			}
			num = this.IngredientDurabilityChanges(DurabilitiesTypes.Special2);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Special2, num));
			}
			num = this.IngredientDurabilityChanges(DurabilitiesTypes.Special3);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Special3, num));
			}
			num = this.IngredientDurabilityChanges(DurabilitiesTypes.Special4);
			if (num != 0)
			{
				list.Add(new DurabilityChangeFeedbackInfo(DurabilitiesTypes.Special4, num));
			}
			return list;
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00051510 File Offset: 0x0004F710
	private int IngredientDurabilityChanges(DurabilitiesTypes _Type)
	{
		if (this.IngredientChanges.ModType == CardModifications.Destroy || this.IngredientChanges.ModType == CardModifications.None)
		{
			return 0;
		}
		if (!this.IngredientChanges.ModifyDurability && this.IngredientChanges.ModType != CardModifications.DurabilityChanges)
		{
			return 0;
		}
		Vector2 vector;
		switch (_Type)
		{
		case DurabilitiesTypes.Spoilage:
			vector = this.IngredientChanges.SpoilageChange;
			goto IL_D8;
		case DurabilitiesTypes.Usage:
			vector = this.IngredientChanges.UsageChange;
			goto IL_D8;
		case DurabilitiesTypes.Fuel:
			vector = this.IngredientChanges.FuelChange;
			goto IL_D8;
		case DurabilitiesTypes.Progress:
			vector = this.IngredientChanges.ChargesChange;
			goto IL_D8;
		case DurabilitiesTypes.Special1:
			vector = this.IngredientChanges.Special1Change;
			goto IL_D8;
		case DurabilitiesTypes.Special2:
			vector = this.IngredientChanges.Special2Change;
			goto IL_D8;
		case DurabilitiesTypes.Special3:
			vector = this.IngredientChanges.Special3Change;
			goto IL_D8;
		case DurabilitiesTypes.Special4:
			vector = this.IngredientChanges.Special4Change;
			goto IL_D8;
		}
		return 0;
		IL_D8:
		if (vector == Vector2.zero)
		{
			return 0;
		}
		if (vector.x >= 0f && vector.y >= 0f)
		{
			return 1;
		}
		if (vector.x <= 0f && vector.y <= 0f)
		{
			return -1;
		}
		float num = Mathf.Min(vector.x, vector.y);
		float num2 = Mathf.Max(vector.x, vector.y);
		if (num <= 0f && num2 >= 0f)
		{
			num = Mathf.Abs(num);
			num2 = Mathf.Abs(num2);
			if (num == num2)
			{
				return 0;
			}
			if (num > num2)
			{
				return -1;
			}
			if (num < num2)
			{
				return 1;
			}
		}
		return 0;
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000833 RID: 2099 RVA: 0x00051691 File Offset: 0x0004F891
	public int RdmDuration
	{
		get
		{
			return Mathf.Max(UnityEngine.Random.Range(this.MinDuration, this.MaxDuration + 1), 1);
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000834 RID: 2100 RVA: 0x000516AC File Offset: 0x0004F8AC
	public int MinDuration
	{
		get
		{
			return Mathf.Max(Mathf.Min(this.Duration, this.Duration + this.DurationRdmVariation), 1);
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000835 RID: 2101 RVA: 0x000516CC File Offset: 0x0004F8CC
	public int MaxDuration
	{
		get
		{
			return Mathf.Max(new int[]
			{
				this.Duration,
				this.Duration + this.DurationRdmVariation,
				1
			});
		}
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x000516F8 File Offset: 0x0004F8F8
	public bool ValidForCard(CardData _Card)
	{
		if (!_Card)
		{
			return false;
		}
		if (this.CompatibleTags != null && this.CompatibleTags.Length != 0)
		{
			for (int i = 0; i < this.CompatibleTags.Length; i++)
			{
				if (_Card.HasTag(this.CompatibleTags[i]))
				{
					return true;
				}
			}
		}
		if (this.CompatibleCards == null)
		{
			return false;
		}
		if (this.CompatibleCards.Length == 0)
		{
			return false;
		}
		for (int j = 0; j < this.CompatibleCards.Length; j++)
		{
			if (this.CompatibleCards[j] == _Card)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000837 RID: 2103 RVA: 0x00051780 File Offset: 0x0004F980
	private bool CreatesLiquidInIngredient
	{
		get
		{
			if (this.DropsAsCollection == null)
			{
				return false;
			}
			if (this.DropsAsCollection.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.DropsAsCollection.Length; i++)
			{
				if (this.DropsAsCollection[i] != null && this.DropsAsCollection[i].DropsLiquid)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x000517D4 File Offset: 0x0004F9D4
	public CardOnCardAction GetResult(InGameCardBase _Ingredient)
	{
		CardOnCardAction cardOnCardAction = new CardOnCardAction();
		bool flag = false;
		cardOnCardAction.UseMiniTicks = MiniTicksBehavior.KeepsZeroCost;
		if (this.ScaleDropsPerLiquidQuantity && this.ScaleDropsPerLiquidQuantity.FloatValue > 0f && _Ingredient != null)
		{
			float num = 0f;
			if (_Ingredient.IsLiquid)
			{
				num = _Ingredient.CurrentLiquidQuantity;
			}
			else if (_Ingredient.ContainedLiquid)
			{
				num = _Ingredient.ContainedLiquid.CurrentLiquidQuantity;
			}
			cardOnCardAction.DropsMultiplier = ExtraMath.RoundOrCeil(num / this.ScaleDropsPerLiquidQuantity);
		}
		cardOnCardAction.ActionName = this.ActionName;
		if ((this.IngredientChanges.ModType == CardModifications.None && !this.IngredientChanges.ModifyLiquid && !this.CreatesLiquidInIngredient) || this.IngredientChanges.ModType == CardModifications.Destroy || !_Ingredient)
		{
			cardOnCardAction.ReceivingCardChanges = new CardStateChange
			{
				ModType = CardModifications.Destroy
			};
		}
		else
		{
			cardOnCardAction.ReceivingCardChanges = this.IngredientChanges;
		}
		cardOnCardAction.GivenCardChanges = this.CookerChanges;
		if (this.DropsAsCollection != null && this.DropsAsCollection.Length != 0)
		{
			cardOnCardAction.ProducedCards = this.DropsAsCollection;
			flag = true;
		}
		if (!flag)
		{
			if (this.Drops == null)
			{
				if (cardOnCardAction.ProducedCards == null)
				{
					cardOnCardAction.ProducedCards = new CardsDropCollection[0];
				}
			}
			else if (this.Drops.Length == 0)
			{
				if (cardOnCardAction.ProducedCards == null)
				{
					cardOnCardAction.ProducedCards = new CardsDropCollection[0];
				}
			}
			else
			{
				if (cardOnCardAction.ProducedCards == null)
				{
					cardOnCardAction.ProducedCards = new CardsDropCollection[1];
				}
				else if (cardOnCardAction.ProducedCards.Length != 1)
				{
					cardOnCardAction.ProducedCards = new CardsDropCollection[1];
				}
				cardOnCardAction.ProducedCards[0] = new CardsDropCollection
				{
					CollectionName = this.ActionName + "_collection",
					CollectionWeight = 1
				};
				cardOnCardAction.ProducedCards[0].SetDroppedCards(this.Drops, this.RandomSingleDrop);
			}
		}
		cardOnCardAction.StatModifications = this.StatModifications;
		return cardOnCardAction;
	}

	// Token: 0x04000C7A RID: 3194
	public LocalizedString ActionName;

	// Token: 0x04000C7B RID: 3195
	public LocalizedString CustomCookingText;

	// Token: 0x04000C7C RID: 3196
	public LocalizedString CannotCookText;

	// Token: 0x04000C7D RID: 3197
	public CardData[] CompatibleCards;

	// Token: 0x04000C7E RID: 3198
	public CardTag[] CompatibleTags;

	// Token: 0x04000C7F RID: 3199
	public CookingConditionsCard ConditionsCard;

	// Token: 0x04000C80 RID: 3200
	public GeneralCondition Conditions;

	// Token: 0x04000C81 RID: 3201
	[SerializeField]
	private int Duration;

	// Token: 0x04000C82 RID: 3202
	[SerializeField]
	private int DurationRdmVariation;

	// Token: 0x04000C83 RID: 3203
	public CardStateChange CookerChanges;

	// Token: 0x04000C84 RID: 3204
	public CardStateChange IngredientChanges;

	// Token: 0x04000C85 RID: 3205
	public bool HideCookingProgress;

	// Token: 0x04000C86 RID: 3206
	public bool HideResultNotification;

	// Token: 0x04000C87 RID: 3207
	public bool RandomSingleDrop;

	// Token: 0x04000C88 RID: 3208
	public AudioClip CustomCompleteSound;

	// Token: 0x04000C89 RID: 3209
	public CardNotifications Notification;

	// Token: 0x04000C8A RID: 3210
	public LocalizedString CustomNotification;

	// Token: 0x04000C8B RID: 3211
	[Space]
	public OptionalFloatValue ScaleDropsPerLiquidQuantity;

	// Token: 0x04000C8C RID: 3212
	public CardDrop[] Drops;

	// Token: 0x04000C8D RID: 3213
	[StatModifierOptions(false, false)]
	public StatModifier[] StatModifications;

	// Token: 0x04000C8E RID: 3214
	public CardsDropCollection[] DropsAsCollection;
}
