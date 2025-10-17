using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000F1 RID: 241
[Serializable]
public struct PassiveEffect
{
	// Token: 0x1700018C RID: 396
	// (get) Token: 0x0600081C RID: 2076 RVA: 0x0004FE16 File Offset: 0x0004E016
	// (set) Token: 0x0600081D RID: 2077 RVA: 0x0004FE1E File Offset: 0x0004E01E
	public int CurrentStack { get; private set; }

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x0600081E RID: 2078 RVA: 0x0004FE27 File Offset: 0x0004E027
	// (set) Token: 0x0600081F RID: 2079 RVA: 0x0004FE2F File Offset: 0x0004E02F
	public int PrevStack { get; private set; }

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06000820 RID: 2080 RVA: 0x0004FE38 File Offset: 0x0004E038
	// (set) Token: 0x06000821 RID: 2081 RVA: 0x0004FE40 File Offset: 0x0004E040
	public InGameCardBase ConditionsCard { get; private set; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06000822 RID: 2082 RVA: 0x0004FE49 File Offset: 0x0004E049
	// (set) Token: 0x06000823 RID: 2083 RVA: 0x0004FE51 File Offset: 0x0004E051
	public DynamicLayoutSlot ConditionsSlot { get; private set; }

	// Token: 0x06000824 RID: 2084 RVA: 0x0004FE5A File Offset: 0x0004E05A
	public static string GetGlobalEffectName(CardData _WithCard, string _EffectName)
	{
		return string.Format("{0}_{1}", _WithCard.name, _EffectName);
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0004FE70 File Offset: 0x0004E070
	public bool ConditionsValid(InGameCardBase _Card, DynamicLayoutSlot _Slot, bool _CheckStack)
	{
		InGameCardBase inGameCardBase = this.ConditionsCard ? this.ConditionsCard : _Card;
		DynamicLayoutSlot dynamicLayoutSlot = (this.ConditionsSlot != null) ? this.ConditionsSlot : _Slot;
		if (this.OnBoardEffectCap > 0 && !string.IsNullOrEmpty(this.EffectName) && MBSingleton<GameManager>.Instance && inGameCardBase && _CheckStack)
		{
			string globalEffectName = PassiveEffect.GetGlobalEffectName(inGameCardBase.CardModel, this.EffectName);
			if (MBSingleton<GameManager>.Instance.PassiveEffectsStacks.ContainsKey(globalEffectName) && MBSingleton<GameManager>.Instance.PassiveEffectsStacks[globalEffectName] >= this.OnBoardEffectCap)
			{
				return false;
			}
		}
		if (this.OnlyWhenNotInBG && _Card.InBackground)
		{
			return false;
		}
		if (!this.OnlyWhenEquipped)
		{
			return this.Conditions.ConditionsValid(MBSingleton<GameManager>.Instance.NotInBase, inGameCardBase);
		}
		return dynamicLayoutSlot && dynamicLayoutSlot.SlotType == SlotsTypes.Equipment && this.Conditions.ConditionsValid(MBSingleton<GameManager>.Instance.NotInBase, inGameCardBase);
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0004FF70 File Offset: 0x0004E170
	public PassiveEffect Instantiate(InGameCardBase _ConditionsCard, DynamicLayoutSlot _ConditionsSlot)
	{
		if (this.StatModifiers == null)
		{
			this.StatModifiers = new StatModifier[0];
		}
		PassiveEffect passiveEffect = new PassiveEffect
		{
			EffectName = this.EffectName,
			OnlyWhenEquipped = this.OnlyWhenEquipped,
			OnlyOnBaseActions = this.OnlyOnBaseActions,
			OnlyWhenNotInBG = this.OnlyWhenNotInBG,
			Conditions = this.Conditions,
			EffectStacksWithRequiredCards = this.EffectStacksWithRequiredCards,
			OnBoardEffectCap = this.OnBoardEffectCap,
			StatModifiers = new StatModifier[this.StatModifiers.Length],
			SpoilageRateModifier = this.SpoilageRateModifier,
			UsageRateModifier = this.UsageRateModifier,
			FuelRateModifier = this.FuelRateModifier,
			ConsumableChargesModifier = this.ConsumableChargesModifier,
			LiquidRateModifier = this.LiquidRateModifier,
			Special1RateModifier = this.Special1RateModifier,
			Special2RateModifier = this.Special2RateModifier,
			Special3RateModifier = this.Special3RateModifier,
			Special4RateModifier = this.Special4RateModifier,
			GeneratedLiquid = new LiquidDrop(this.GeneratedLiquid.LiquidCard, Vector2.one * UnityEngine.Random.Range(this.GeneratedLiquid.Quantity.x, this.GeneratedLiquid.Quantity.y), null),
			WeightCapacityModifier = this.WeightCapacityModifier,
			ConditionsCard = _ConditionsCard,
			ConditionsSlot = _ConditionsSlot
		};
		for (int i = 0; i < this.StatModifiers.Length; i++)
		{
			passiveEffect.StatModifiers[i] = new StatModifier
			{
				Stat = this.StatModifiers[i].Stat,
				RateModifier = (Mathf.Approximately(this.StatModifiers[i].RateModifier.x, this.StatModifiers[i].RateModifier.y) ? (Vector2.one * this.StatModifiers[i].RateModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.StatModifiers[i].RateModifier.x, this.StatModifiers[i].RateModifier.y))),
				ValueModifier = (Mathf.Approximately(this.StatModifiers[i].ValueModifier.x, this.StatModifiers[i].ValueModifier.y) ? (Vector2.one * this.StatModifiers[i].ValueModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.StatModifiers[i].ValueModifier.x, this.StatModifiers[i].ValueModifier.y)))
			};
		}
		if (!this.EffectStacksWithRequiredCards)
		{
			passiveEffect.CurrentStack = 1;
			passiveEffect.PrevStack = 1;
		}
		else
		{
			passiveEffect.PrevStack = 0;
			passiveEffect.CurrentStack = 0;
		}
		return passiveEffect;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x00050282 File Offset: 0x0004E482
	public void UpdateStack(InGameCardBase _ForCard)
	{
		if (!this.EffectStacksWithRequiredCards)
		{
			this.CurrentStack = 1;
			this.PrevStack = 1;
			return;
		}
		this.PrevStack = this.CurrentStack;
		this.CurrentStack = this.Conditions.RequiredCardsCount(_ForCard);
	}

	// Token: 0x04000C31 RID: 3121
	public string EffectName;

	// Token: 0x04000C32 RID: 3122
	public bool OnlyWhenEquipped;

	// Token: 0x04000C33 RID: 3123
	public bool OnlyOnBaseActions;

	// Token: 0x04000C34 RID: 3124
	public bool OnlyWhenNotInBG;

	// Token: 0x04000C35 RID: 3125
	public GeneralCondition Conditions;

	// Token: 0x04000C36 RID: 3126
	[StatModifierOptions(true, false)]
	public StatModifier[] StatModifiers;

	// Token: 0x04000C37 RID: 3127
	public bool EffectStacksWithRequiredCards;

	// Token: 0x04000C38 RID: 3128
	public int OnBoardEffectCap;

	// Token: 0x04000C39 RID: 3129
	public OptionalFloatValue SpoilageRateModifier;

	// Token: 0x04000C3A RID: 3130
	public OptionalFloatValue UsageRateModifier;

	// Token: 0x04000C3B RID: 3131
	public OptionalFloatValue FuelRateModifier;

	// Token: 0x04000C3C RID: 3132
	public OptionalFloatValue ConsumableChargesModifier;

	// Token: 0x04000C3D RID: 3133
	public OptionalFloatValue Special1RateModifier;

	// Token: 0x04000C3E RID: 3134
	public OptionalFloatValue Special2RateModifier;

	// Token: 0x04000C3F RID: 3135
	public OptionalFloatValue Special3RateModifier;

	// Token: 0x04000C40 RID: 3136
	public OptionalFloatValue Special4RateModifier;

	// Token: 0x04000C41 RID: 3137
	[FormerlySerializedAs("EvaporationModifier")]
	public float LiquidRateModifier;

	// Token: 0x04000C42 RID: 3138
	public LiquidDrop GeneratedLiquid;

	// Token: 0x04000C43 RID: 3139
	public float WeightCapacityModifier;
}
