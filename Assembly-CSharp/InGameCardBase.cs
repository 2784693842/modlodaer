using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000012 RID: 18
public class InGameCardBase : TooltipProvider
{
	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000100 RID: 256 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
	public Rect CardRect
	{
		get
		{
			return new Rect(base.transform.position - new Vector2(800f, 1200f) * 0.5f, new Vector2(800f, 1200f));
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x06000101 RID: 257 RVA: 0x0000C11F File Offset: 0x0000A31F
	// (set) Token: 0x06000102 RID: 258 RVA: 0x0000C127 File Offset: 0x0000A327
	public CardData CardModel { get; private set; }

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000103 RID: 259 RVA: 0x0000C130 File Offset: 0x0000A330
	// (set) Token: 0x06000104 RID: 260 RVA: 0x0000C138 File Offset: 0x0000A338
	public int CardCreationIndex { get; private set; }

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000105 RID: 261 RVA: 0x0000C141 File Offset: 0x0000A341
	// (set) Token: 0x06000106 RID: 262 RVA: 0x0000C149 File Offset: 0x0000A349
	public string CustomName { get; private set; }

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000107 RID: 263 RVA: 0x0000C152 File Offset: 0x0000A352
	// (set) Token: 0x06000108 RID: 264 RVA: 0x0000C15A File Offset: 0x0000A35A
	public bool DragStackCompatible { get; private set; }

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000109 RID: 265 RVA: 0x0000C163 File Offset: 0x0000A363
	// (set) Token: 0x0600010A RID: 266 RVA: 0x0000C16B File Offset: 0x0000A36B
	public bool LiquidEmpty { get; private set; }

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600010B RID: 267 RVA: 0x0000C174 File Offset: 0x0000A374
	// (set) Token: 0x0600010C RID: 268 RVA: 0x0000C17C File Offset: 0x0000A37C
	public EnvironmentSaveData TravelToData { get; private set; }

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x0600010D RID: 269 RVA: 0x0000C185 File Offset: 0x0000A385
	// (set) Token: 0x0600010E RID: 270 RVA: 0x0000C18D File Offset: 0x0000A38D
	public bool VisibleOnScreen { get; private set; }

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x0600010F RID: 271 RVA: 0x0000C196 File Offset: 0x0000A396
	// (set) Token: 0x06000110 RID: 272 RVA: 0x0000C19E File Offset: 0x0000A39E
	public TutorialHighlightState TutorialHighlight { get; private set; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000111 RID: 273 RVA: 0x0000C1A7 File Offset: 0x0000A3A7
	// (set) Token: 0x06000112 RID: 274 RVA: 0x0000C1AF File Offset: 0x0000A3AF
	public CardGraphics.CardGraphicsStates CurrentGraphicState { get; private set; }

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000113 RID: 275 RVA: 0x0000C1B8 File Offset: 0x0000A3B8
	// (set) Token: 0x06000114 RID: 276 RVA: 0x0000C1C0 File Offset: 0x0000A3C0
	public InGameCardBase GraphicStateInventoryCard { get; private set; }

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000115 RID: 277 RVA: 0x0000C1C9 File Offset: 0x0000A3C9
	// (set) Token: 0x06000116 RID: 278 RVA: 0x0000C1D1 File Offset: 0x0000A3D1
	public MissingReqInfo CurrentMissingRequirements { get; private set; }

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000117 RID: 279 RVA: 0x0000C1DA File Offset: 0x0000A3DA
	// (set) Token: 0x06000118 RID: 280 RVA: 0x0000C1E2 File Offset: 0x0000A3E2
	public CookingBarInfo CurrentCookingBarInfo { get; private set; }

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000119 RID: 281 RVA: 0x0000C1EB File Offset: 0x0000A3EB
	// (set) Token: 0x0600011A RID: 282 RVA: 0x0000C1F3 File Offset: 0x0000A3F3
	public bool SmallCollider { get; private set; }

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x0600011B RID: 283 RVA: 0x0000C1FC File Offset: 0x0000A3FC
	// (set) Token: 0x0600011C RID: 284 RVA: 0x0000C204 File Offset: 0x0000A404
	public bool ActionPerformedObjects { get; private set; }

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x0600011D RID: 285 RVA: 0x0000C20D File Offset: 0x0000A40D
	public CardData ContainedLiquidModel
	{
		get
		{
			if (this.IsPinned)
			{
				return this.PinnedLiquidModel;
			}
			if (!this.ContainedLiquid)
			{
				return this.FutureLiquidContained;
			}
			return this.ContainedLiquid.CardModel;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600011E RID: 286 RVA: 0x0000C23D File Offset: 0x0000A43D
	public bool IsLiquidContainer
	{
		get
		{
			return this.CardModel && this.CardModel.CanContainLiquid;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600011F RID: 287 RVA: 0x0000C259 File Offset: 0x0000A459
	public bool IsLiquid
	{
		get
		{
			return this.CardModel && this.CardModel.CardType == CardTypes.Liquid;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000120 RID: 288 RVA: 0x0000C27C File Offset: 0x0000A47C
	public LiquidContainerStates LiquidContainerState
	{
		get
		{
			if (!this.IsLiquidContainer)
			{
				return LiquidContainerStates.NotContainer;
			}
			if (!this.ContainedLiquid)
			{
				return LiquidContainerStates.Empty;
			}
			if (this.ContainedLiquid.CurrentLiquidQuantity <= 0f)
			{
				return LiquidContainerStates.Empty;
			}
			if (this.ContainedLiquid.CurrentLiquidQuantity >= this.ContainedLiquid.CurrentMaxLiquidQuantity)
			{
				return LiquidContainerStates.Full;
			}
			return LiquidContainerStates.PartiallyFull;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06000121 RID: 289 RVA: 0x0000C2D1 File Offset: 0x0000A4D1
	public bool HasExternalPassiveEffects
	{
		get
		{
			return this.ExternalPassiveEffects != null && this.ExternalPassiveEffects.Count > 0;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000122 RID: 290 RVA: 0x0000C2EC File Offset: 0x0000A4EC
	public float CurrentSpoilageRate
	{
		get
		{
			float num = this.BaseSpoilageRate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraSpoilageRate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].SpoilageRateModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.SpoilageTime.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06000123 RID: 291 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
	public float CurrentUsageRate
	{
		get
		{
			float num = this.BaseUsageRate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraUsageRate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].UsageRateModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.UsageDurability.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06000124 RID: 292 RVA: 0x0000C474 File Offset: 0x0000A674
	public float CurrentFuelRate
	{
		get
		{
			float num = this.BaseFuelRate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraFuelRate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].FuelRateModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.FuelCapacity.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000125 RID: 293 RVA: 0x0000C538 File Offset: 0x0000A738
	public float CurrentConsumableRate
	{
		get
		{
			float num = this.BaseConsumableRate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraProgressRate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].ConsumableChargesModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.Progress.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000126 RID: 294 RVA: 0x0000C5FC File Offset: 0x0000A7FC
	public float CurrentEvaporationRate
	{
		get
		{
			float num = this.BaseEvaporationRate;
			if (this.CurrentContainer && this.CurrentContainer.CurrentProducedLiquids != null)
			{
				for (int i = 0; i < this.CurrentContainer.CurrentProducedLiquids.Count; i++)
				{
					if (!this.CurrentContainer.CurrentProducedLiquids[i].IsEmpty && !(this.CurrentContainer.CurrentProducedLiquids[i].LiquidCard != this.CardModel))
					{
						num += this.CurrentContainer.CurrentProducedLiquids[i].Quantity.x;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000127 RID: 295 RVA: 0x0000C6A8 File Offset: 0x0000A8A8
	public bool IsFillingWithLiquid
	{
		get
		{
			if (this.CurrentEvaporationRate > 0f)
			{
				return true;
			}
			if (this.CookingCards == null)
			{
				return false;
			}
			if (this.CookingCards.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.CookingCards.Count; i++)
			{
				if (this.CookingCards[i].FillsCookerLiquid)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000128 RID: 296 RVA: 0x0000C70C File Offset: 0x0000A90C
	public float CurrentSpecial1Rate
	{
		get
		{
			float num = this.BaseSpecial1Rate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraSpecial1Rate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].Special1RateModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.SpecialDurability1.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000129 RID: 297 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
	public float CurrentSpecial2Rate
	{
		get
		{
			float num = this.BaseSpecial2Rate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraSpecial2Rate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].Special2RateModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.SpecialDurability2.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600012A RID: 298 RVA: 0x0000C894 File Offset: 0x0000AA94
	public float CurrentSpecial3Rate
	{
		get
		{
			float num = this.BaseSpecial3Rate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraSpecial3Rate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].Special3RateModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.SpecialDurability3.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x0600012B RID: 299 RVA: 0x0000C958 File Offset: 0x0000AB58
	public float CurrentSpecial4Rate
	{
		get
		{
			float num = this.BaseSpecial4Rate;
			if (this.IsCooking())
			{
				num += this.CardModel.CookingConditions.ExtraSpecial4Rate;
			}
			if (this.CardModel.LocalCounterEffects != null)
			{
				for (int i = 0; i < this.CardModel.LocalCounterEffects.Length; i++)
				{
					if (this.CardModel.LocalCounterEffects[i].IsActive(this))
					{
						num += this.CardModel.LocalCounterEffects[i].Special4RateModifier;
					}
				}
			}
			if (!this.GraphicsM)
			{
				return num;
			}
			if (!this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				return num;
			}
			return num + this.CardModel.SpecialDurability4.ExtraRateWhenEquipped;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x0600012C RID: 300 RVA: 0x0000CA1C File Offset: 0x0000AC1C
	public float CurrentMaxLiquidQuantity
	{
		get
		{
			if (!this.CardModel)
			{
				return -1f;
			}
			if (this.CardModel.CardType == CardTypes.Liquid)
			{
				if (!this.CurrentContainer)
				{
					return -1f;
				}
				if (!this.CurrentContainer.CardModel)
				{
					return -1f;
				}
				return this.CurrentContainer.CardModel.MaxLiquidCapacity;
			}
			else
			{
				if (this.CardModel.CanContainLiquid)
				{
					return this.CardModel.MaxLiquidCapacity;
				}
				return -1f;
			}
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600012D RID: 301 RVA: 0x0000CAA8 File Offset: 0x0000ACA8
	public float CurrentSpoilagePercent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.SpoilageTime)
			{
				return 1f;
			}
			if (this.CardModel.SpoilageTime.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentSpoilage / this.CardModel.SpoilageTime.Max;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x0600012E RID: 302 RVA: 0x0000CB14 File Offset: 0x0000AD14
	public float CurrentUsagePercent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.UsageDurability)
			{
				return 1f;
			}
			if (this.CardModel.UsageDurability.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentUsageDurability / this.CardModel.UsageDurability.Max;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600012F RID: 303 RVA: 0x0000CB80 File Offset: 0x0000AD80
	public float CurrentFuelPercent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.FuelCapacity)
			{
				return 1f;
			}
			if (this.CardModel.FuelCapacity.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentFuel / this.CardModel.FuelCapacity.Max;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000130 RID: 304 RVA: 0x0000CBEC File Offset: 0x0000ADEC
	public float CurrentProgressPercent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.Progress)
			{
				return 1f;
			}
			if (this.CardModel.Progress.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentProgress / this.CardModel.Progress.Max;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000131 RID: 305 RVA: 0x0000CC58 File Offset: 0x0000AE58
	public float CurrentSpecial1Percent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.SpecialDurability1)
			{
				return 1f;
			}
			if (this.CardModel.SpecialDurability1.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentSpecial1 / this.CardModel.SpecialDurability1.Max;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000132 RID: 306 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
	public float CurrentSpecial2Percent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.SpecialDurability2)
			{
				return 1f;
			}
			if (this.CardModel.SpecialDurability2.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentSpecial2 / this.CardModel.SpecialDurability2.Max;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000133 RID: 307 RVA: 0x0000CD30 File Offset: 0x0000AF30
	public float CurrentSpecial3Percent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.SpecialDurability3)
			{
				return 1f;
			}
			if (this.CardModel.SpecialDurability3.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentSpecial3 / this.CardModel.SpecialDurability3.Max;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000134 RID: 308 RVA: 0x0000CD9C File Offset: 0x0000AF9C
	public float CurrentSpecial4Percent
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (!this.CardModel.SpecialDurability4)
			{
				return 1f;
			}
			if (this.CardModel.SpecialDurability4.Max <= 0f)
			{
				return 1f;
			}
			return this.CurrentSpecial4 / this.CardModel.SpecialDurability4.Max;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000135 RID: 309 RVA: 0x0000CE08 File Offset: 0x0000B008
	public float CurrentWeight
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (this.IsLiquid)
			{
				return this.CardModel.ObjectWeight * this.CurrentLiquidQuantity;
			}
			if (this.CardModel.CardType == CardTypes.Blueprint)
			{
				if (this.BlueprintWeight <= 0f)
				{
					this.BlueprintWeight = this.CardModel.BlueprintResultWeight;
				}
				return this.BlueprintWeight + this.InventoryWeight(false);
			}
			float num = this.CardModel.ObjectWeight;
			if (this.GraphicsM && this.GraphicsM.CharacterWindow.HasCardEquipped(this))
			{
				num += this.CardModel.WeightReductionWhenEquipped;
			}
			if (this.CardsInInventory == null && !this.ContainedLiquid)
			{
				return Mathf.Max(0f, num);
			}
			if (this.CardsInInventory.Count == 0 && !this.ContainedLiquid)
			{
				return Mathf.Max(0f, num);
			}
			return Mathf.Max(0f, num + this.InventoryWeight(false));
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000CF14 File Offset: 0x0000B114
	public float InventoryWeight(bool _IgnoreBonus = false)
	{
		if (!this.CardModel)
		{
			return 0f;
		}
		if (this.DontCountInventoryWeight && !this.CardModel.CanContainLiquid)
		{
			return 0f;
		}
		float num = 0f;
		if (this.ContainedLiquid)
		{
			num += this.ContainedLiquid.CurrentWeight;
		}
		if (this.CardsInInventory != null)
		{
			for (int i = 0; i < this.CardsInInventory.Count; i++)
			{
				if (this.CardsInInventory[i] != null)
				{
					num += this.CardsInInventory[i].CurrentWeight;
				}
			}
		}
		if (!_IgnoreBonus)
		{
			float num2 = (this.CardModel.CardType == CardTypes.Blueprint) ? (-this.BlueprintWeight) : this.CardModel.ContentWeightReduction;
			return Mathf.Max(0f, num + num2);
		}
		return num;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000CFE8 File Offset: 0x0000B1E8
	public TransferedDurabilities GetDurabilities()
	{
		return new TransferedDurabilities
		{
			Spoilage = new OptionalFloatValue(true, this.CurrentSpoilage),
			Usage = new OptionalFloatValue(true, this.CurrentUsageDurability),
			Fuel = new OptionalFloatValue(true, this.CurrentFuel),
			ConsumableCharges = new OptionalFloatValue(true, this.CurrentProgress),
			Liquid = this.CurrentLiquidQuantity,
			Special1 = new OptionalFloatValue(true, this.CurrentSpecial1),
			Special2 = new OptionalFloatValue(true, this.CurrentSpecial2),
			Special3 = new OptionalFloatValue(true, this.CurrentSpecial3),
			Special4 = new OptionalFloatValue(true, this.CurrentSpecial4)
		};
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000D096 File Offset: 0x0000B296
	public void SetCustomName(string _Name)
	{
		if (this.CustomName == _Name)
		{
			return;
		}
		this.CustomName = _Name;
		if (this.CardVisuals)
		{
			this.CardVisuals.CardTitle.text = this.CardName(false);
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000139 RID: 313 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
	public Sprite CurrentImage
	{
		get
		{
			if (!this.CardModel)
			{
				return null;
			}
			if (this.ContainedLiquid)
			{
				Sprite imageForLiquid = this.CardModel.GetImageForLiquid(this.ContainedLiquidModel);
				if (imageForLiquid)
				{
					return imageForLiquid;
				}
			}
			return this.CardModel.CardImage;
		}
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000D124 File Offset: 0x0000B324
	public string CardName(bool _IgnoreLiquid = false)
	{
		if (this.IsPinned && this.PinnedLiquidModel && !string.IsNullOrEmpty(this.PinnedLiquidModel.CardName))
		{
			return this.PinnedLiquidModel.CardName;
		}
		if (this.ContainedLiquid && !_IgnoreLiquid && !this.CardModel.KeepName && !string.IsNullOrEmpty(this.ContainedLiquid.CardName(false)))
		{
			return this.ContainedLiquid.CardName(false);
		}
		if (!this.CardModel)
		{
			return "";
		}
		if (!string.IsNullOrEmpty(this.CustomName) && this.CardModel.CanBeRenamed)
		{
			return this.CustomName;
		}
		return this.CardModel.CardName;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
	public string CardDescription(bool _IgnoreLiquid = false)
	{
		if (this.ContainedLiquid && !_IgnoreLiquid && !string.IsNullOrEmpty(this.ContainedLiquid.CardDescription(false)))
		{
			return this.ContainedLiquid.CardDescription(false);
		}
		if (!this.CardModel)
		{
			return "";
		}
		return this.CardModel.CardDescription;
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x0600013C RID: 316 RVA: 0x0000D250 File Offset: 0x0000B450
	// (set) Token: 0x0600013D RID: 317 RVA: 0x0000D258 File Offset: 0x0000B458
	public bool Destroyed { get; private set; }

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600013E RID: 318 RVA: 0x0000D261 File Offset: 0x0000B461
	// (set) Token: 0x0600013F RID: 319 RVA: 0x0000D269 File Offset: 0x0000B469
	public bool InBackground { get; private set; }

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000140 RID: 320 RVA: 0x0000D272 File Offset: 0x0000B472
	// (set) Token: 0x06000141 RID: 321 RVA: 0x0000D27A File Offset: 0x0000B47A
	public bool BlocksRaycasts
	{
		get
		{
			return this.PrivateBlocksRaycasts;
		}
		set
		{
			this.PrivateBlocksRaycasts = value;
			if (this.CardVisuals && this.CardVisuals.CurrentCollision)
			{
				this.CardVisuals.CurrentCollision.raycastTarget = value;
			}
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000142 RID: 322 RVA: 0x0000D2B3 File Offset: 0x0000B4B3
	// (set) Token: 0x06000143 RID: 323 RVA: 0x0000D2BB File Offset: 0x0000B4BB
	public DynamicLayoutSlot CurrentSlot { get; private set; }

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000144 RID: 324 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
	// (set) Token: 0x06000145 RID: 325 RVA: 0x0000D2CC File Offset: 0x0000B4CC
	public Transform CurrentParentObject { get; private set; }

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000146 RID: 326 RVA: 0x0000D2D5 File Offset: 0x0000B4D5
	// (set) Token: 0x06000147 RID: 327 RVA: 0x0000D2DD File Offset: 0x0000B4DD
	public StatTriggeredActionStatus[] StatTriggeredActions { get; private set; }

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000148 RID: 328 RVA: 0x0000D2E6 File Offset: 0x0000B4E6
	// (set) Token: 0x06000149 RID: 329 RVA: 0x0000D2EE File Offset: 0x0000B4EE
	public bool HasAction { get; private set; }

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600014A RID: 330 RVA: 0x0000D2F7 File Offset: 0x0000B4F7
	// (set) Token: 0x0600014B RID: 331 RVA: 0x0000D2FF File Offset: 0x0000B4FF
	public bool IsPinned { get; private set; }

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600014C RID: 332 RVA: 0x0000D308 File Offset: 0x0000B508
	// (set) Token: 0x0600014D RID: 333 RVA: 0x0000D310 File Offset: 0x0000B510
	public bool IsBlueprintInstance { get; private set; }

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600014E RID: 334 RVA: 0x0000D31C File Offset: 0x0000B51C
	public bool CanBePinned
	{
		get
		{
			return this.CardModel && this.CardModel.CanPile && (this.CardModel.CardType == CardTypes.Item || this.CardModel.CardType == CardTypes.Base) && !this.CardModel.HasInventory && this.CurrentSlot != null && this.CurrentSlot.CanPin;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600014F RID: 335 RVA: 0x0000D388 File Offset: 0x0000B588
	private bool IsPulsing
	{
		get
		{
			return this.ActionPulse != null && this.ActionPulse.IsPlaying();
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000150 RID: 336 RVA: 0x0000D39F File Offset: 0x0000B59F
	private bool IsTimeAnimated
	{
		get
		{
			return this.TimeAnimation != null && this.TimeAnimation.IsPlaying();
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000151 RID: 337 RVA: 0x0000D3B6 File Offset: 0x0000B5B6
	public bool HiddenInInventory
	{
		get
		{
			return this.CurrentContainer != null && this.GraphicsM.InspectedCard != this.CurrentContainer;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000152 RID: 338 RVA: 0x0000D3DE File Offset: 0x0000B5DE
	public bool IsInventoryCard
	{
		get
		{
			return this.CardModel && this.CardModel.HasInventory;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000153 RID: 339 RVA: 0x0000D3FA File Offset: 0x0000B5FA
	public float MaxWeightCapacity
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			return this.CardModel.GetWeightCapacity(this.WeightCapacityBonus);
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000154 RID: 340 RVA: 0x0000D420 File Offset: 0x0000B620
	public Vector3 ValidPosition
	{
		get
		{
			if (this.CurrentContainer && this.GraphicsM.InspectedCard != this.CurrentContainer)
			{
				return this.CurrentContainer.ValidPosition;
			}
			if (this.Destroyed)
			{
				return this.LastAlivePosition;
			}
			return base.transform.position;
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000D478 File Offset: 0x0000B678
	public float DurabilityScore()
	{
		return this.DurabilityScore(1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
	public float DurabilityScore(float _SpoilageMod, float _UsageMod, float _FuelMod, float _ProgressMod, float _Special1Mod, float _Special2Mod, float _Special3Mod, float _Special4Mod)
	{
		if (!this.CardModel.SpoilageTime && !this.CardModel.UsageDurability && !this.CardModel.FuelCapacity && !this.CardModel.Progress)
		{
			return 100000f;
		}
		float num = 0f;
		if (this.CardModel.SpoilageTime && _SpoilageMod != 0f)
		{
			num += Mathf.Max(this.CurrentSpoilage - _SpoilageMod, 0f);
		}
		if (this.CardModel.UsageDurability && _UsageMod != 0f)
		{
			num += Mathf.Max(this.CurrentUsageDurability + _UsageMod, 0f);
		}
		if (this.CardModel.FuelCapacity && _FuelMod != 0f)
		{
			num += Mathf.Max(this.CurrentFuel + _FuelMod, 0f);
		}
		if (this.CardModel.Progress && _ProgressMod != 0f)
		{
			num += Mathf.Max(this.CurrentProgress + _ProgressMod, 0f);
		}
		if (this.CardModel.SpecialDurability1 && _Special1Mod != 0f)
		{
			num += Mathf.Max(this.CurrentSpecial1 + _Special1Mod, 0f);
		}
		if (this.CardModel.SpecialDurability2 && _Special2Mod != 0f)
		{
			num += Mathf.Max(this.CurrentSpecial2 + _Special2Mod, 0f);
		}
		if (this.CardModel.SpecialDurability3 && _Special3Mod != 0f)
		{
			num += Mathf.Max(this.CurrentSpecial3 + _Special3Mod, 0f);
		}
		if (this.CardModel.SpecialDurability4 && _Special4Mod != 0f)
		{
			num += Mathf.Max(this.CurrentSpecial4 + _Special4Mod, 0f);
		}
		return num;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000D698 File Offset: 0x0000B898
	public bool IsCooking()
	{
		return !this.IsPinned && this.CookingCards != null && this.CookingCards.Count != 0 && this.CardModel.CookingConditions.CanCook(this.CurrentSpoilage, this.CurrentUsageDurability, this.CurrentFuel, this.CurrentProgress, this.CurrentSpecial1, this.CurrentSpecial2, this.CurrentSpecial3, this.CurrentSpecial4, this.CardModel);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000D710 File Offset: 0x0000B910
	public bool CookingIsPaused()
	{
		return !this.IsPinned && this.CookingCards != null && this.CookingCards.Count != 0 && !this.CardModel.CookingConditions.CanCook(this.CurrentSpoilage, this.CurrentUsageDurability, this.CurrentFuel, this.CurrentProgress, this.CurrentSpecial1, this.CurrentSpecial2, this.CurrentSpecial3, this.CurrentSpecial4, this.CardModel);
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000D78C File Offset: 0x0000B98C
	public bool OneOrMoreRecipesArePaused()
	{
		if (this.CookingIsPaused())
		{
			return true;
		}
		for (int i = 0; i < this.CookingCards.Count; i++)
		{
			if (this.CookingCards[i].SelfPaused)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000D7D0 File Offset: 0x0000B9D0
	public bool ContainsCookedContent()
	{
		if (!this.CardModel)
		{
			return false;
		}
		if (this.IsPinned)
		{
			return false;
		}
		if (this.CardsInInventory == null)
		{
			return false;
		}
		if (this.CardsInInventory.Count == 0)
		{
			return false;
		}
		if (this.CookingResultsList == null)
		{
			return false;
		}
		if (this.CookingResultsList.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < this.CookingResultsList.Count; i++)
		{
			for (int j = 0; j < this.CardsInInventory.Count; j++)
			{
				if (this.CardsInInventory[j] != null && !this.CardsInInventory[j].IsFree && this.CardsInInventory[j].CardModel == this.CookingResultsList[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000D89C File Offset: 0x0000BA9C
	public void AddCookingResult(CardData _Result)
	{
		if (!_Result)
		{
			return;
		}
		if (this.CookingResultsList == null)
		{
			this.CookingResultsList = new List<CardData>();
		}
		if (!this.CookingResultsList.Contains(_Result))
		{
			this.CookingResultsList.Add(_Result);
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000D8D4 File Offset: 0x0000BAD4
	public void TransferCookingResults(List<CardData> _Results)
	{
		if (_Results == null)
		{
			return;
		}
		if (_Results.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _Results.Count; i++)
		{
			this.AddCookingResult(_Results[i]);
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600015D RID: 349 RVA: 0x0000D90C File Offset: 0x0000BB0C
	public List<CardData> GetCookingResults
	{
		get
		{
			return this.CookingResultsList;
		}
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000D914 File Offset: 0x0000BB14
	public void SaveCookingResults(ref List<string> _Results)
	{
		if (this.CookingResultsList == null)
		{
			return;
		}
		if (this.CookingResultsList.Count == 0)
		{
			return;
		}
		if (_Results == null)
		{
			return;
		}
		for (int i = 0; i < this.CookingResultsList.Count; i++)
		{
			_Results.Add(UniqueIDScriptable.SaveID(this.CookingResultsList[i]));
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x0600015F RID: 351 RVA: 0x0000D96C File Offset: 0x0000BB6C
	public bool IndependentFromEnv
	{
		get
		{
			if (!this.CardModel)
			{
				return false;
			}
			if (this.IsPinned)
			{
				return false;
			}
			if (this.WasIndependentFromEnv)
			{
				return true;
			}
			if (this.ContainedLiquidModel && this.ContainedLiquidModel.IndependentFromEnv)
			{
				return true;
			}
			if (this.CurrentContainer)
			{
				return this.CurrentContainer.IndependentFromEnv;
			}
			return (this.CurrentSlot != null && (this.CurrentSlot.SlotType == SlotsTypes.Equipment || this.CurrentSlot.SlotType == SlotsTypes.Item || this.CurrentSlot.SlotType == SlotsTypes.Blueprint)) || this.CardModel.IndependentFromEnv;
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000DA10 File Offset: 0x0000BC10
	public CookingCardStatus GetCookingStatusForCard(InGameCardBase _Card)
	{
		if (this.IsPinned)
		{
			return null;
		}
		if (this.CookingCards == null)
		{
			return null;
		}
		if (this.CookingCards.Count == 0)
		{
			return null;
		}
		for (int i = 0; i < this.CookingCards.Count; i++)
		{
			if (this.CookingCards[i].Card == _Card)
			{
				return this.CookingCards[i];
			}
		}
		return null;
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000161 RID: 353 RVA: 0x0000DA7D File Offset: 0x0000BC7D
	public bool IsLegacyInventory
	{
		get
		{
			return this.CardModel && this.CardModel.LegacyInventory;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000162 RID: 354 RVA: 0x0000DA99 File Offset: 0x0000BC99
	public bool DontCountInventoryWeight
	{
		get
		{
			return this.CardModel && this.CardModel.CardType == CardTypes.Explorable;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000163 RID: 355 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
	public bool InventoryFull
	{
		get
		{
			return this.GetIndexForInventory(0, null, null, -1f) == -1;
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000DACB File Offset: 0x0000BCCB
	public bool HasInGameCardInInventory(InGameCardBase _Card)
	{
		return _Card.CurrentContainer == this;
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000165 RID: 357 RVA: 0x0000DADC File Offset: 0x0000BCDC
	public bool HasInventoryContent
	{
		get
		{
			if (this.CardsInInventory == null)
			{
				return false;
			}
			if (this.CardsInInventory.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.CardsInInventory.Count; i++)
			{
				if (this.CardsInInventory[i] != null && this.CardsInInventory[i].MainCard != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000DB44 File Offset: 0x0000BD44
	public bool HasCardInInventory(CardData _Card)
	{
		if (this.CardsInInventory == null)
		{
			return false;
		}
		if ((this.CardsInInventory.Count == 0 && !this.ContainedLiquidModel) || !this.CardModel || _Card == null)
		{
			return false;
		}
		if (this.CardsInInventory.Count > 0)
		{
			for (int i = 0; i < this.CardsInInventory.Count; i++)
			{
				if (this.CardsInInventory[i] != null && !this.CardsInInventory[i].IsFree && _Card == this.CardsInInventory[i].CardModel)
				{
					return true;
				}
			}
		}
		return this.ContainedLiquidModel == _Card && this.ContainedLiquidModel != null && this.ContainedLiquid.CurrentLiquidQuantity > 0f;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000DC20 File Offset: 0x0000BE20
	public bool HasTagInInventory(CardTag _Tag)
	{
		if (this.CardsInInventory == null)
		{
			return false;
		}
		if ((this.CardsInInventory.Count == 0 && !this.ContainedLiquidModel) || !this.CardModel || _Tag == null)
		{
			return false;
		}
		if (this.CardsInInventory.Count > 0)
		{
			for (int i = 0; i < this.CardsInInventory.Count; i++)
			{
				if (this.CardsInInventory[i] != null && !this.CardsInInventory[i].IsFree && this.CardsInInventory[i].CardModel.HasTag(_Tag))
				{
					return true;
				}
			}
		}
		return this.ContainedLiquidModel != null && this.ContainedLiquidModel.HasTag(_Tag) && this.ContainedLiquid.CurrentLiquidQuantity > 0f;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000DCFC File Offset: 0x0000BEFC
	public int InventoryCount(CardData _OfCard = null)
	{
		if (this.CardsInInventory == null)
		{
			return 0;
		}
		if (this.CardsInInventory.Count == 0 || !this.CardModel)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < this.CardsInInventory.Count; i++)
		{
			if (this.CardsInInventory[i] != null && !this.CardsInInventory[i].IsFree && (_OfCard == this.CardsInInventory[i].CardModel || (this.CardModel.CardType != CardTypes.Blueprint && this.CardModel.CardType != CardTypes.EnvImprovement && this.CardModel.CardType != CardTypes.EnvDamage) || _OfCard == null))
			{
				num += this.CardsInInventory[i].CardAmt;
			}
		}
		return num;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
	public int MaxInventoryContent(InGameCardBase _OfCard, bool _IgnoreLiquid = false)
	{
		if (this.CardsInInventory == null)
		{
			return 0;
		}
		if (!this.CardModel || !_OfCard || !this.IsInventoryCard)
		{
			return 0;
		}
		int num = 0;
		if (this.CardModel.CardType == CardTypes.Blueprint || this.CardModel.CardType == CardTypes.EnvImprovement || this.CardModel.CardType == CardTypes.EnvDamage)
		{
			if (this.BlueprintData.CurrentStage < 0 || this.BlueprintData.CurrentStage >= this.CardModel.BlueprintStages.Length || this.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint)
			{
				return 0;
			}
			for (int i = 0; i < this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements.Length; i++)
			{
				if (this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].CompatibleInGameCard(_OfCard))
				{
					if (this.CardsInInventory[i].MainCard)
					{
						if (this.CardsInInventory[i].MainCard.CardModel == _OfCard.CardModel)
						{
							if (!_IgnoreLiquid)
							{
								num += this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].GetCorrectMaxQuantity(_OfCard);
							}
							else
							{
								num += this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].GetQuantity;
							}
						}
					}
					else if (!_IgnoreLiquid)
					{
						num += this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].GetCorrectMaxQuantity(_OfCard);
					}
					else
					{
						num += this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].GetQuantity;
					}
				}
			}
			return num;
		}
		else
		{
			if (!this.CardModel.InventoryFilter.SupportsCard(_OfCard.CardModel, _IgnoreLiquid ? null : _OfCard.ContainedLiquidModel))
			{
				return 0;
			}
			if (!this.IsLegacyInventory)
			{
				return -1;
			}
			return this.CardsInInventory.Count;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600016A RID: 362 RVA: 0x0000E006 File Offset: 0x0000C206
	public int BlueprintSteps
	{
		get
		{
			if (this.CardModel.BlueprintStages == null)
			{
				return 0;
			}
			return this.CardModel.BlueprintStages.Length;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600016B RID: 363 RVA: 0x0000E024 File Offset: 0x0000C224
	public float BlueprintBuildPercentage
	{
		get
		{
			if (!this.CardModel)
			{
				return 0f;
			}
			if (this.CardModel.CardType != CardTypes.Blueprint && this.CardModel.CardType != CardTypes.EnvImprovement && this.CardModel.CardType != CardTypes.EnvDamage)
			{
				return 0f;
			}
			if (this.BlueprintSteps <= 0)
			{
				return 0f;
			}
			if (this.BlueprintData == null)
			{
				return 0f;
			}
			return (float)this.BlueprintData.CurrentStage / (float)this.BlueprintSteps;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600016C RID: 364 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
	public BlueprintStage CurrentBlueprintStage
	{
		get
		{
			if (!this.CardModel)
			{
				return null;
			}
			if (this.CardModel.CardType != CardTypes.Blueprint && this.CardModel.CardType != CardTypes.EnvImprovement && this.CardModel.CardType != CardTypes.EnvDamage)
			{
				return null;
			}
			if (this.CardModel.BlueprintStages == null)
			{
				return null;
			}
			if (this.CardModel.BlueprintStages.Length == 0)
			{
				return null;
			}
			if (this.BlueprintData == null)
			{
				return null;
			}
			return this.CardModel.BlueprintStages[Mathf.Clamp(this.BlueprintData.CurrentStage, 0, this.CardModel.BlueprintStages.Length - 1)];
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600016D RID: 365 RVA: 0x0000E148 File Offset: 0x0000C348
	public bool ReadyToBuildBlueprint
	{
		get
		{
			BlueprintStage currentBlueprintStage = this.CurrentBlueprintStage;
			if (currentBlueprintStage == null)
			{
				return false;
			}
			if (this.BlueprintData.CurrentStage >= this.CardModel.BlueprintStages.Length)
			{
				return false;
			}
			if (currentBlueprintStage.RequiredElements == null)
			{
				return true;
			}
			if (currentBlueprintStage.RequiredElements.Length == 0)
			{
				return true;
			}
			if (this.CardsInInventory == null)
			{
				Debug.LogError("Inventory does not match blueprint requirements");
				return false;
			}
			if (this.CardsInInventory.Count != currentBlueprintStage.RequiredElements.Length)
			{
				Debug.LogError("Inventory does not match blueprint requirements");
				return false;
			}
			for (int i = 0; i < currentBlueprintStage.RequiredElements.Length; i++)
			{
				if (!currentBlueprintStage.RequiredElements[i].CompatibleInGameCard(this.CardsInInventory[i].MainCard))
				{
					return false;
				}
				if (this.CardsInInventory[i].CardAmt != currentBlueprintStage.RequiredElements[i].GetQuantity)
				{
					return false;
				}
				if (this.CardsInInventory[i].MainCard.IsLiquidContainer && currentBlueprintStage.RequiredElements[i].GetLiquidQuantity > 0 && !currentBlueprintStage.RequiredElements[i].CorrectLiquidQuantity(this.CardsInInventory[i].MainCard))
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600016E RID: 366 RVA: 0x0000E27F File Offset: 0x0000C47F
	public bool ReadyToDeconstruct
	{
		get
		{
			return this.CurrentBlueprintStage != null && this.BlueprintData.CurrentStage >= 0 && this.BlueprintData.CurrentStage < this.CardModel.BlueprintStages.Length;
		}
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
	public int GetIndexForInventory(int _From, CardData _ForCard, CardData _WithLiquid, float _CardWeight)
	{
		if (_ForCard != null && !this.CanReceiveInInventory(_ForCard, _WithLiquid))
		{
			return -1;
		}
		if (!this.IsInventoryCard)
		{
			return -1;
		}
		if (this.CardModel.CardType != CardTypes.Blueprint && this.CardModel.CardType != CardTypes.EnvImprovement && this.CardModel.CardType != CardTypes.EnvDamage)
		{
			if (!this.IsLegacyInventory)
			{
				if (_ForCard)
				{
					if (_CardWeight < 0f)
					{
						_CardWeight = _ForCard.ObjectWeight;
					}
					if (_CardWeight > this.MaxWeightCapacity - this.InventoryWeight(true))
					{
						return -1;
					}
				}
				else if (this.InventoryWeight(true) >= this.MaxWeightCapacity)
				{
					return -1;
				}
				for (int i = 0; i < this.CardsInInventory.Count; i++)
				{
					if (this.CardsInInventory[i] != null)
					{
						if (this.CardsInInventory[i].IsFree)
						{
							return i;
						}
						if (this.CardsInInventory[i].CardModel == _ForCard && this.CardsInInventory[i].LiquidModel == _WithLiquid)
						{
							return i;
						}
					}
				}
				return this.CardsInInventory.Count;
			}
			if (_From < 0 || _From >= this.CardsInInventory.Count)
			{
				_From = 0;
			}
			for (int j = _From; j < this.CardsInInventory.Count; j++)
			{
				if (this.CardsInInventory[j] != null && this.CardsInInventory[j].IsFree)
				{
					return j;
				}
			}
			for (int k = 0; k < _From; k++)
			{
				if (this.CardsInInventory[k] != null && this.CardsInInventory[k].IsFree)
				{
					return k;
				}
			}
		}
		else
		{
			for (int l = 0; l < this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements.Length; l++)
			{
				if (this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[l].GetQuantity > this.CardsInInventory[l].CardAmt)
				{
					if (_ForCard == null)
					{
						return l;
					}
					if (this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[l].CompatibleCard(_ForCard, _WithLiquid) && (this.CardsInInventory[l].CardModel == null || (this.CardsInInventory[l].CardModel == _ForCard && this.CardsInInventory[l].LiquidModel == _WithLiquid)))
					{
						return l;
					}
				}
			}
		}
		if (_WithLiquid)
		{
			return this.GetIndexForInventory(_From, _WithLiquid, null, -1f);
		}
		return -1;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000E564 File Offset: 0x0000C764
	public bool CanReceiveInInventoryInstance(InGameCardBase _Card)
	{
		if (!this.CardModel || !_Card)
		{
			return false;
		}
		if (this.CardModel.CardType == CardTypes.Blueprint || this.CardModel.CardType == CardTypes.EnvImprovement || this.CardModel.CardType == CardTypes.EnvDamage)
		{
			if (this.BlueprintData.CurrentStage < 0 || this.BlueprintData.CurrentStage >= this.CardModel.BlueprintStages.Length || this.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint)
			{
				return false;
			}
			for (int i = 0; i < this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements.Length; i++)
			{
				if (this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].CompatibleInGameCard(_Card))
				{
					if (!this.CardsInInventory[i].CardModel)
					{
						return this.GM.MaxEnvWeight <= 0f || _Card.CurrentSlotInfo.SlotType == SlotsTypes.Base || _Card.CurrentSlotInfo.SlotType == SlotsTypes.Location || this.GM.CurrentEnvWeight + _Card.CurrentWeight <= this.GM.MaxEnvWeight || (this.CurrentSlotInfo.SlotType != SlotsTypes.Base && this.CurrentSlotInfo.SlotType != SlotsTypes.Location);
					}
					if (!(this.CardsInInventory[i].CardModel != _Card.CardModel) && this.CardsInInventory[i].CardAmt < this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].GetCorrectMaxQuantity(_Card))
					{
						return this.GM.MaxEnvWeight <= 0f || _Card.CurrentSlotInfo.SlotType == SlotsTypes.Base || _Card.CurrentSlotInfo.SlotType == SlotsTypes.Location || this.GM.CurrentEnvWeight + _Card.CurrentWeight <= this.GM.MaxEnvWeight || (this.CurrentSlotInfo.SlotType != SlotsTypes.Base && this.CurrentSlotInfo.SlotType != SlotsTypes.Location);
					}
				}
			}
			return false;
		}
		else
		{
			if (this.IsLegacyInventory)
			{
				return this.CanReceiveInInventory(_Card.CardModel, _Card.ContainedLiquidModel);
			}
			float num = this.InventoryWeight(true);
			return (this.MaxWeightCapacity >= num + _Card.CurrentWeight || !(_Card.CurrentContainer != this)) && this.CanReceiveInInventory(_Card.CardModel, _Card.ContainedLiquidModel);
		}
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000E804 File Offset: 0x0000CA04
	public bool CanTransferToTravelPlace(InGameCardBase _Card, out string _Reason)
	{
		_Reason = "";
		if (!_Card)
		{
			return false;
		}
		if (!_Card.CardModel)
		{
			return false;
		}
		if (_Card.CardModel.CardType != CardTypes.Item)
		{
			return false;
		}
		if (!this.CardModel)
		{
			return false;
		}
		CardData getTravelDestination = this.CardModel.GetTravelDestination;
		if (!getTravelDestination || !this.CardModel.CanDragItemsToTravel)
		{
			return false;
		}
		if (this.GM == null)
		{
			this.GM = MBSingleton<GameManager>.Instance;
		}
		if (this.TravelToData == null)
		{
			CardData prevEnv = null;
			if (getTravelDestination.InstancedEnvironment)
			{
				prevEnv = this.GM.CurrentEnvironment;
			}
			this.TravelToData = this.GM.GetEnvSaveData(getTravelDestination, prevEnv, this.TravelCardIndex, true);
		}
		_Reason = LocalizedString.InventoryCannotCarry(this);
		return this.TravelToData.CurrentWeight + _Card.CurrentWeight <= this.TravelToData.CurrentMaxWeight || this.TravelToData.CurrentMaxWeight <= 0f;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000E904 File Offset: 0x0000CB04
	public bool CanReceiveInInventory(CardData _Card, CardData _WithLiquid)
	{
		if (!this.CardModel || !_Card)
		{
			return false;
		}
		if (_Card.IsMandatoryEquipment)
		{
			return false;
		}
		if (this.CardModel.CardType != CardTypes.Blueprint && this.CardModel.CardType != CardTypes.EnvImprovement && this.CardModel.CardType != CardTypes.EnvDamage)
		{
			return this.CardModel.CompleteInventoryFilter.SupportsCard(_Card, _WithLiquid);
		}
		if (_Card.CardType != CardTypes.Item && _Card.CardType != CardTypes.Base && _Card.CardType != CardTypes.Liquid)
		{
			return false;
		}
		if (this.BlueprintData.CurrentStage < 0 || this.BlueprintData.CurrentStage >= this.CardModel.BlueprintStages.Length || this.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint)
		{
			return false;
		}
		for (int i = 0; i < this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements.Length; i++)
		{
			if (this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].CompatibleCard(_Card, _WithLiquid))
			{
				if (!this.CardsInInventory[i].CardModel)
				{
					return true;
				}
				if (!(this.CardsInInventory[i].CardModel != _Card))
				{
					return true;
				}
			}
			else if (this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements[i].CompatibleCard(_WithLiquid, null) && !this.CardsInInventory[i].CardModel)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000EAA1 File Offset: 0x0000CCA1
	public bool CanReceiveLiquid(InGameCardBase _Liquid)
	{
		return _Liquid && _Liquid.CardModel && this.CanReceiveLiquid(_Liquid.CardModel);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000EAC8 File Offset: 0x0000CCC8
	public bool CanReceiveLiquid(CardData _Liquid)
	{
		return this.CardModel && this.CardModel.CanContainThisLiquid(_Liquid);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000EAE5 File Offset: 0x0000CCE5
	public CardSaveData Save()
	{
		if (!this.CardModel)
		{
			return null;
		}
		CardSaveData cardSaveData = new CardSaveData();
		cardSaveData.SaveCard(this);
		return cardSaveData;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000EB02 File Offset: 0x0000CD02
	public InGameRefCardSaveData MakeRefData()
	{
		if (!this.CardModel)
		{
			return null;
		}
		InGameRefCardSaveData inGameRefCardSaveData = new InGameRefCardSaveData();
		inGameRefCardSaveData.SaveCard(this);
		return inGameRefCardSaveData;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000EB1F File Offset: 0x0000CD1F
	public InventoryCardSaveData SaveInventory(List<InventoryCardSaveData> _ExtraInventories, bool _OverrideEnv = false)
	{
		if (!this.CardModel)
		{
			return null;
		}
		InventoryCardSaveData inventoryCardSaveData = new InventoryCardSaveData();
		inventoryCardSaveData.SaveCard(this, _ExtraInventories, _OverrideEnv);
		return inventoryCardSaveData;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000EB40 File Offset: 0x0000CD40
	public void Awake()
	{
		this.CardCreationIndex = InGameCardBase.CreatedCardsCount;
		InGameCardBase.CreatedCardsCount++;
		this.GetManagers();
		if (!this.CardVisuals)
		{
			this.CardVisuals = base.GetComponent<CardGraphics>();
		}
		this.InitialScale = base.transform.localScale;
		if (this.CardVisuals)
		{
			this.PrivateBlocksRaycasts = !this.CardVisuals.DontBlockRaycasts;
		}
		else
		{
			this.PrivateBlocksRaycasts = false;
		}
		this.SetCollider(false);
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000EBC5 File Offset: 0x0000CDC5
	private void GetManagers()
	{
		if (this.GM && this.GraphicsM)
		{
			return;
		}
		this.GM = MBSingleton<GameManager>.Instance;
		this.GraphicsM = MBSingleton<GraphicsManager>.Instance;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
	public void UpdateEmptyCookingRecipe()
	{
		if (this.IsPinned)
		{
			return;
		}
		CookingRecipe recipeForCard = this.CardModel.GetRecipeForCard(null, null, this);
		if (recipeForCard == null)
		{
			if (this.GraphicsM.InspectedCard == this && this.GraphicsM.CurrentInspectionPopup)
			{
				this.GraphicsM.CurrentInspectionPopup.UpdateCookingProgress();
			}
			return;
		}
		int num = -1;
		for (int i = 0; i < this.CardsInInventory.Count; i++)
		{
			if (this.CardsInInventory[i] != null && this.CardsInInventory[i].IsFree)
			{
				num = i;
				break;
			}
		}
		CookingCardStatus cookingStatusForCard = this.GetCookingStatusForCard(null);
		if (cookingStatusForCard == null && num == -1)
		{
			if (this.GraphicsM.InspectedCard == this && this.GraphicsM.CurrentInspectionPopup)
			{
				this.GraphicsM.CurrentInspectionPopup.UpdateCookingProgress();
			}
			return;
		}
		if (num == -1)
		{
			this.CookingCards.Remove(cookingStatusForCard);
			if (this.CardVisuals)
			{
				this.CardVisuals.RefreshCookingStatus();
			}
		}
		else if (cookingStatusForCard != null)
		{
			cookingStatusForCard.CardIndex = num;
		}
		else
		{
			this.CookingCards.Add(new CookingCardStatus(null, recipeForCard.RdmDuration, recipeForCard.CustomCookingText, this.CardModel.CookingConditions.CookingPausedNotification, recipeForCard.CannotCookText, recipeForCard.HideCookingProgress, recipeForCard.FillsCookerLiquid, recipeForCard.FillsIngredientLiquid)
			{
				CardIndex = num
			});
			if (this.CardVisuals)
			{
				this.CardVisuals.RefreshCookingStatus();
			}
		}
		if (this.GraphicsM.InspectedCard == this && this.GraphicsM.CurrentInspectionPopup)
		{
			this.GraphicsM.CurrentInspectionPopup.UpdateCookingProgress();
		}
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
	public void UpdateInventory(List<DynamicLayoutSlot> _Slots)
	{
		for (int i = 0; i < this.CardsInInventory.Count; i++)
		{
			this.CardsInInventory[i].Clear();
			if (i < _Slots.Count)
			{
				this.CardsInInventory[i].AddCards(_Slots[i].GetCardPile(false, false, false));
			}
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000EE08 File Offset: 0x0000D008
	private void WeightHasChanged()
	{
		if (this.CardModel && this.CardModel.CardType == CardTypes.Explorable)
		{
			return;
		}
		if (this.CurrentContainer)
		{
			this.CurrentContainer.WeightHasChanged();
			return;
		}
		if (this.CurrentSlot != null)
		{
			if (this.GraphicsM.CharacterWindow.HasCardEquipped(this) || this.CurrentSlot.SlotType == SlotsTypes.Item)
			{
				this.GM.CalculateCarryWeight();
				return;
			}
			if (this.CurrentSlot.SlotType == SlotsTypes.Base || this.CurrentSlot.SlotType == SlotsTypes.Location)
			{
				this.GM.CalculateEnvironmentWeight(false);
				return;
			}
		}
		else if (this.GraphicsM.CharacterWindow.HasCardEquipped(this))
		{
			this.GM.CalculateCarryWeight();
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000EEC8 File Offset: 0x0000D0C8
	public void AddCardToInventory(InGameCardBase _Card, int _Index)
	{
		int i = 0;
		while (i < this.CardsInInventory.Count)
		{
			if (this.CardsInInventory[i].HasCard(_Card))
			{
				if (i == _Index)
				{
					return;
				}
				this.CardsInInventory[i].RemoveCard(_Card);
				break;
			}
			else
			{
				i++;
			}
		}
		if (_Index >= this.CardsInInventory.Count)
		{
			_Index = this.CardsInInventory.Count;
			this.CardsInInventory.Add(new InventorySlot());
		}
		else if (this.CardsInInventory[_Index].CardModel != _Card.CardModel && !this.CardsInInventory[_Index].IsFree)
		{
			this.CardsInInventory.Insert(_Index, new InventorySlot());
		}
		this.CardsInInventory[_Index].AddCard(_Card);
		if (this.CardModel)
		{
			if (this.CardModel.HasPassiveEffects)
			{
				this.GM.StartCoroutine(this.UpdatePassiveEffects());
				this.UpdatePassiveEffectStacks();
			}
			if (this.CardModel.EffectsToInventoryContent != null)
			{
				for (int j = 0; j < this.CardModel.EffectsToInventoryContent.Length; j++)
				{
					if (string.IsNullOrEmpty(this.CardModel.EffectsToInventoryContent[j].EffectName))
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Effect to Inventory Content ",
							j.ToString(),
							" on ",
							this.CardModel
						}));
					}
					else
					{
						_Card.AddExternalPassiveEffect(this.CardModel.EffectsToInventoryContent[j], null);
					}
				}
			}
		}
		this.UpdateEmptyCookingRecipe();
		this.AddToCookingRecipes(_Card);
		if (this.CardVisuals)
		{
			this.CardVisuals.RefreshCookingStatus();
		}
		this.WeightHasChanged();
		if (this.GraphicsM.CurrentInspectionPopup && this.GraphicsM.InspectedCard == this && !this.IsLegacyInventory && this.GraphicsM.CurrentInspectionPopup.SlotCount != this.CardsInInventory.Count + 1)
		{
			this.GraphicsM.CurrentInspectionPopup.RefreshInventory();
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
	public void AddToCookingRecipes(InGameCardBase _Card)
	{
		if (_Card.ContainedLiquid)
		{
			this.AddToCookingRecipes(_Card.ContainedLiquid);
		}
		CookingRecipe recipeForCard = this.CardModel.GetRecipeForCard(_Card.CardModel, _Card, this);
		if (recipeForCard == null)
		{
			if (this.CardVisuals)
			{
				this.CardVisuals.RefreshCookingStatus();
			}
			return;
		}
		for (int i = 0; i < this.CookingCards.Count; i++)
		{
			if (this.CookingCards[i].Card == _Card)
			{
				if (_Card != null)
				{
					this.CookingCards[i].UpdateCookingProgressVisuals((float)this.CookingCards[i].CookedDuration / (float)this.CookingCards[i].TargetDuration, this.CookingCards[i].TargetDuration - this.CookingCards[i].CookedDuration, _Card.CookingIsPaused(), this.CookingCards[i].GetCookingText(this.CookingIsPaused(), this.CookingCards[i].TargetDuration - this.CookingCards[i].CookedDuration));
				}
				return;
			}
		}
		this.CookingCards.Add(new CookingCardStatus(_Card, recipeForCard.RdmDuration, recipeForCard.CustomCookingText, this.CardModel.CookingConditions.CookingPausedNotification, recipeForCard.CannotCookText, recipeForCard.HideCookingProgress, recipeForCard.FillsCookerLiquid, recipeForCard.FillsIngredientLiquid));
		this.CookingCards[this.CookingCards.Count - 1].SelfPaused = !recipeForCard.Conditions.ConditionsValid(false, (recipeForCard.ConditionsCard == CookingConditionsCard.Cooker) ? this : _Card);
		if (this.CookingCards[this.CookingCards.Count - 1].SelfPaused)
		{
			if (_Card)
			{
				this.CookingCards[this.CookingCards.Count - 1].UpdateCookingProgressVisuals((float)this.CookingCards[this.CookingCards.Count - 1].CookedDuration / (float)this.CookingCards[this.CookingCards.Count - 1].TargetDuration, this.CookingCards[this.CookingCards.Count - 1].TargetDuration - this.CookingCards[this.CookingCards.Count - 1].CookedDuration, true, this.CookingCards[this.CookingCards.Count - 1].SelfPausedText);
			}
			if (this.CardVisuals)
			{
				this.CardVisuals.RefreshCookingStatus();
			}
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000F398 File Offset: 0x0000D598
	public void UpdateCookingRecipes()
	{
		bool flag = this.CookingIsPaused();
		for (int i = 0; i < this.CookingCards.Count; i++)
		{
			if (this.CookingCards[i].Card)
			{
				CookingRecipe recipeForCard = this.CardModel.GetRecipeForCard(this.CookingCards[i].Card.CardModel, this.CookingCards[i].Card, this);
				if (recipeForCard != null)
				{
					int remainingTicks = this.CookingCards[i].TargetDuration - this.CookingCards[i].CookedDuration;
					this.CookingCards[i].SelfPaused = !recipeForCard.Conditions.ConditionsValid(false, (recipeForCard.ConditionsCard == CookingConditionsCard.Cooker) ? this : this.CookingCards[i].Card);
					if (this.CookingCards[i].Card)
					{
						this.CookingCards[i].UpdateCookingProgressVisuals((float)this.CookingCards[i].CookedDuration / (float)this.CookingCards[i].TargetDuration, remainingTicks, this.CookingCards[i].SelfPaused || flag, this.CookingCards[i].SelfPaused ? this.CookingCards[i].SelfPausedText : this.CookingCards[i].GetCookingText(flag, remainingTicks));
					}
				}
			}
		}
		if (this.CardVisuals)
		{
			this.CardVisuals.RefreshCookingStatus();
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000F538 File Offset: 0x0000D738
	private void CheckForRemoteEffects()
	{
		if (this.GM.CardsWithRemoteEffects != null)
		{
			for (int i = 0; i < this.GM.CardsWithRemoteEffects.Count; i++)
			{
				if (this.GM.CardsWithRemoteEffects[i] && !this.GM.CardsWithRemoteEffects[i].Destroyed)
				{
					for (int j = 0; j < this.GM.CardsWithRemoteEffects[i].CardModel.RemotePassiveEffects.Length; j++)
					{
						if (this.GM.CardsWithRemoteEffects[i].CardModel.RemotePassiveEffects[j].AppliesToCard(this))
						{
							this.AddExternalPassiveEffect(this.GM.CardsWithRemoteEffects[i].CardModel.RemotePassiveEffects[j].Effect, this.GM.CardsWithRemoteEffects[i]);
						}
					}
				}
			}
		}
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000F640 File Offset: 0x0000D840
	public bool IsInSameEnv(InGameCardBase _AsCard)
	{
		if (!_AsCard)
		{
			return false;
		}
		if (!this.IndependentFromEnv && !_AsCard.IndependentFromEnv)
		{
			return this.Environment == _AsCard.Environment;
		}
		if (!this.IndependentFromEnv && _AsCard.IndependentFromEnv)
		{
			return !_AsCard.InBackground && _AsCard.Environment == this.Environment;
		}
		if (this.IndependentFromEnv && !_AsCard.IndependentFromEnv)
		{
			return !this.InBackground && _AsCard.Environment == this.Environment;
		}
		return this.Environment == _AsCard.Environment && this.PrevEnvironment == _AsCard.PrevEnvironment && this.PrevEnvTravelIndex == _AsCard.PrevEnvTravelIndex;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000F708 File Offset: 0x0000D908
	public void AddExternalPassiveEffect(PassiveEffect _Effect, InGameCardBase _ConditionsCard)
	{
		if (_ConditionsCard && !this.IsInSameEnv(_ConditionsCard))
		{
			return;
		}
		if (this.ExternalPassiveEffects == null)
		{
			this.ExternalPassiveEffects = new List<PassiveEffect>();
		}
		if (!_ConditionsCard)
		{
			for (int i = 0; i < this.ExternalPassiveEffects.Count; i++)
			{
				if (this.ExternalPassiveEffects[i].EffectName == _Effect.EffectName)
				{
					return;
				}
			}
		}
		else
		{
			for (int j = 0; j < this.ExternalPassiveEffects.Count; j++)
			{
				if (this.ExternalPassiveEffects[j].ConditionsCard == _ConditionsCard && this.ExternalPassiveEffects[j].EffectName.Contains(_Effect.EffectName))
				{
					return;
				}
			}
		}
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder(_Effect.EffectName);
		stringBuilder.Append(num);
		while (this.ExternalEffectsIDs.Contains(stringBuilder.ToString()))
		{
			num++;
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append(num);
		}
		this.ExternalEffectsIDs.Add(stringBuilder.ToString());
		_Effect.EffectName = stringBuilder.ToString();
		if (_ConditionsCard)
		{
			this.ExternalPassiveEffects.Add(_Effect.Instantiate(_ConditionsCard, _ConditionsCard.CurrentSlot));
		}
		else
		{
			this.ExternalPassiveEffects.Add(_Effect.Instantiate(this, this.CurrentSlot));
		}
		this.GM.StartCoroutine(this.UpdatePassiveEffects());
		if (!this.GM.CardsWithPassiveEffects.Contains(this))
		{
			this.GM.CardsWithPassiveEffects.Add(this);
		}
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000F8A4 File Offset: 0x0000DAA4
	public void RemoveCardFromInventory(InGameCardBase _Card)
	{
		if (!_Card || this.CardsInInventory.Count == 0)
		{
			return;
		}
		this.RemoveFromCookingRecipes(_Card);
		int i = 0;
		while (i < this.CardsInInventory.Count)
		{
			if (this.CardsInInventory[i].HasCard(_Card))
			{
				this.CardsInInventory[i].RemoveCard(_Card);
				if (this.CardsInInventory[i].IsFree && !this.IsLegacyInventory && this.CardsInInventory.Count > 1)
				{
					this.CardsInInventory.RemoveAt(i);
					this.SortInventory();
				}
				if (this.CardModel && this.CardModel.EffectsToInventoryContent != null)
				{
					for (int j = 0; j < this.CardModel.EffectsToInventoryContent.Length; j++)
					{
						if (string.IsNullOrEmpty(this.CardModel.EffectsToInventoryContent[j].EffectName))
						{
							Debug.LogError(string.Concat(new object[]
							{
								"Effect to Inventory Content ",
								i.ToString(),
								" on ",
								this.CardModel
							}));
						}
						else
						{
							_Card.RemoveExternalPassiveEffect(this.CardModel.EffectsToInventoryContent[j], null);
						}
					}
					break;
				}
				break;
			}
			else
			{
				i++;
			}
		}
		this.UpdateEmptyCookingRecipe();
		if (this.CardVisuals)
		{
			this.CardVisuals.RefreshCookingStatus();
		}
		if (this.CardModel && this.CardModel.HasPassiveEffects)
		{
			this.GM.StartCoroutine(this.UpdatePassiveEffects());
			this.UpdatePassiveEffectStacks();
		}
		this.WeightHasChanged();
		if (this.GraphicsM.CurrentInspectionPopup && this.GraphicsM.InspectedCard == this && !this.IsLegacyInventory && this.GraphicsM.CurrentInspectionPopup.SlotCount != this.CardsInInventory.Count + 1)
		{
			this.GraphicsM.CurrentInspectionPopup.RefreshInventory();
		}
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
	public void RemoveFromCookingRecipes(InGameCardBase _Card)
	{
		if (this.CookingCards.Count > 0)
		{
			for (int i = this.CookingCards.Count - 1; i >= 0; i--)
			{
				if (this.CookingCards[i].Card == _Card)
				{
					this.CookingCards[i].CancelCookingProgressVisuals();
					this.CookingCards.RemoveAt(i);
					break;
				}
			}
		}
		if (_Card.ContainedLiquid)
		{
			this.RemoveFromCookingRecipes(_Card.ContainedLiquid);
		}
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000FB28 File Offset: 0x0000DD28
	public void RemoveExternalPassiveEffect(PassiveEffect _Effect, InGameCardBase _ConditionsCard)
	{
		if (this.ExternalPassiveEffects == null)
		{
			this.ExternalPassiveEffects = new List<PassiveEffect>();
		}
		for (int i = 0; i < this.ExternalPassiveEffects.Count; i++)
		{
			if (this.ExternalPassiveEffects[i].EffectName.Contains(_Effect.EffectName) && (_ConditionsCard == this.ExternalPassiveEffects[i].ConditionsCard || !_ConditionsCard))
			{
				if (this.ExternalEffectsIDs.Contains(this.ExternalPassiveEffects[i].EffectName))
				{
					this.ExternalEffectsIDs.Remove(this.ExternalPassiveEffects[i].EffectName);
				}
				this.GM.StartCoroutine(this.CancelPassiveEffect(this.ExternalPassiveEffects[i]));
				this.ExternalPassiveEffects.RemoveAt(i);
				if (this.ExternalPassiveEffects.Count == 0 && this.GM.CardsWithPassiveEffects.Contains(this) && !this.CardModel.HasPassiveEffects)
				{
					this.GM.CardsWithPassiveEffects.Remove(this);
				}
				return;
			}
		}
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000FC54 File Offset: 0x0000DE54
	public void SetContainedLiquid(InGameCardBase _Liquid, bool _FromSelfLiquid, bool _MoveView)
	{
		if (this.ContainedLiquid == _Liquid)
		{
			this.FutureLiquidContained = null;
			this.WeightHasChanged();
			return;
		}
		CardData x = null;
		bool flag = this.CurrentContainer && this.GraphicsM.InspectedCard != this.CurrentContainer;
		if (this.CurrentSlot != null && !flag)
		{
			x = this.CurrentSlot.AssignedCard.ContainedLiquidModel;
		}
		if (this.ContainedLiquid)
		{
			if (this.CurrentContainer)
			{
				this.CurrentContainer.RemoveFromCookingRecipes(this.ContainedLiquid);
			}
			if (!this.ContainedLiquid.Destroyed && !_FromSelfLiquid)
			{
				GameManager.PerformAction(CardData.OnEvaporatedAction, this.ContainedLiquid, true);
			}
		}
		this.ContainedLiquid = _Liquid;
		this.FutureLiquidContained = null;
		if (this.ContainedLiquid && this.CurrentContainer)
		{
			this.CurrentContainer.AddToCookingRecipes(this.ContainedLiquid);
		}
		if (this.CardVisuals)
		{
			this.CardVisuals.RefreshDurabilities();
			this.CardVisuals.RefreshCookingStatus();
			this.CardVisuals.CardTitle.text = this.CardName(false);
		}
		if (x != this.ContainedLiquidModel && this.CurrentSlot != null && !flag && !this.StayInSlotWhenLiquidChanges)
		{
			this.CurrentSlot.RemoveSpecificCard(this, true, true);
			this.SetSlot(null, true);
			this.GraphicsM.GetSlotForCard(this.CardModel, this.ContainedLiquidModel, this.CurrentSlotInfo, null, null, 0).AssignCard(this, false);
			if (_MoveView)
			{
				this.GraphicsM.RefreshSlots(false);
				this.GraphicsM.MoveViewToSlot(this.CurrentSlot, false, false);
			}
		}
		else
		{
			this.StayInSlotWhenLiquidChanges = false;
		}
		this.WeightHasChanged();
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000FE14 File Offset: 0x0000E014
	public void SetSlot(DynamicLayoutSlot _Slot, bool _InstantMove)
	{
		this.CurrentSlot = _Slot;
		if (_Slot)
		{
			if (_Slot.SlotType == SlotsTypes.Inventory)
			{
				this.CurrentContainer = MBSingleton<GraphicsManager>.Instance.InspectedCard;
			}
			else if (_Slot.SlotType == SlotsTypes.Exploration)
			{
				this.CurrentContainer = MBSingleton<ExplorationPopup>.Instance.ExplorationCard;
			}
			else
			{
				if (this.CurrentContainer)
				{
					this.CurrentContainer.RemoveCardFromInventory(this);
				}
				this.CurrentContainer = null;
			}
			this.CurrentSlotInfo = _Slot.ToInfo();
		}
		this.UpdateVisibility();
		if (_Slot)
		{
			this.SetParent(_Slot.GetParent, _Slot.SlotType == SlotsTypes.Environment || _Slot.SlotType == SlotsTypes.Event || _Slot.SlotType == SlotsTypes.Improvement || _Slot.SlotType == SlotsTypes.EnvDamage || _InstantMove);
		}
		if (!this.CardModel)
		{
			return;
		}
		if (this.CardModel && this.CardModel.HasPassiveEffects)
		{
			this.GM.StartCoroutine(this.UpdatePassiveEffects());
		}
		this.GM.UpdateObjectivesCompletion(false);
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000FF1F File Offset: 0x0000E11F
	public void SetModel(CardData _Model)
	{
		this.CardModel = _Model;
		if (this.Destroyed)
		{
			this.Destroyed = false;
			this.RegisterToEvents();
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000FF3D File Offset: 0x0000E13D
	public void SetPinned(bool _Pinned, CardData _WithLiquid)
	{
		this.IsPinned = _Pinned;
		if (_Pinned)
		{
			this.PinnedLiquidModel = _WithLiquid;
		}
		else
		{
			this.PinnedLiquidModel = null;
		}
		this.SetGraphicState(CardGraphics.CardGraphicsStates.Pinned, null);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000FF61 File Offset: 0x0000E161
	public IEnumerator Init(TransferedDurabilities _TransferedDurabilities, List<CollectionDropsSaveData> _CollectionDrops, List<StatTriggeredActionStatus> _StatTriggeredActions, ExplorationSaveData _Exploration, BlueprintSaveData _Blueprint, Vector2Int _TickInfo)
	{
		this.GetManagers();
		this.CreatedOnTick = _TickInfo.x;
		this.CreatedInSaveDataTick = ((_TickInfo.y == -1) ? this.GM.CurrentTickInfo.z : _TickInfo.y);
		if (this.PulseAfterReachingSlot)
		{
			base.transform.localScale = this.InitialScale * 1.5f;
		}
		if (this.CardModel.CardType != CardTypes.Blueprint || this.CurrentSlotInfo == null)
		{
			if (this.CardModel.CardType == CardTypes.Liquid && !this.CardModel.CannotBeTrashed)
			{
				this.DismantleActions = new DismantleCardAction[this.CardModel.DismantleActions.Count + 1];
				for (int i = 0; i < this.DismantleActions.Length - 1; i++)
				{
					this.DismantleActions[i] = this.CardModel.DismantleActions[i];
				}
				this.DismantleActions[this.DismantleActions.Length - 1] = CardData.EmptyLiquidAction(this.CardModel);
			}
			else
			{
				this.DismantleActions = this.CardModel.DismantleActions.ToArray();
			}
		}
		else
		{
			if (this.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint)
			{
				this.DismantleActions = new DismantleCardAction[]
				{
					this.CardModel.BlueprintCreationAction
				};
			}
			else
			{
				this.DismantleActions = this.CardModel.DismantleActions.ToArray();
			}
			this.BlueprintWeight = this.CardModel.BlueprintResultWeight;
		}
		if (this.CardModel.OnStatsChangeActions != null && !this.IsPinned)
		{
			this.StatTriggeredActions = new StatTriggeredActionStatus[this.CardModel.OnStatsChangeActions.Length];
			for (int j = 0; j < this.CardModel.OnStatsChangeActions.Length; j++)
			{
				this.StatTriggeredActions[j] = new StatTriggeredActionStatus(this.CardModel.OnStatsChangeActions[j].StatChangeTrigger.Length, this.CardModel.OnStatsChangeActions[j].ActionName.DefaultText);
				if (_StatTriggeredActions != null && _StatTriggeredActions.Count != 0)
				{
					for (int k = 0; k < _StatTriggeredActions.Count; k++)
					{
						if (_StatTriggeredActions[k].ID == this.StatTriggeredActions[j].ID && !string.IsNullOrEmpty(_StatTriggeredActions[k].ID))
						{
							this.StatTriggeredActions[j].Load(_StatTriggeredActions[k]);
							break;
						}
					}
				}
			}
		}
		else
		{
			this.StatTriggeredActions = new StatTriggeredActionStatus[0];
		}
		if (this.CardModel.SpoilageTime)
		{
			this.CurrentSpoilage = this.CardModel.SpoilageTime;
			this.BaseSpoilageRate = this.CardModel.SpoilageTime.RatePerDaytimePoint;
		}
		if (this.CardModel.UsageDurability)
		{
			this.CurrentUsageDurability = this.CardModel.UsageDurability;
			this.BaseUsageRate = this.CardModel.UsageDurability.RatePerDaytimePoint;
		}
		if (this.CardModel.FuelCapacity)
		{
			this.CurrentFuel = this.CardModel.FuelCapacity;
			this.BaseFuelRate = this.CardModel.FuelCapacity.RatePerDaytimePoint;
		}
		if (this.CardModel.Progress)
		{
			this.CurrentProgress = this.CardModel.Progress;
			this.BaseConsumableRate = this.CardModel.Progress.RatePerDaytimePoint;
		}
		if (this.CardModel.SpecialDurability1)
		{
			this.CurrentSpecial1 = this.CardModel.SpecialDurability1;
			this.BaseSpecial1Rate = this.CardModel.SpecialDurability1.RatePerDaytimePoint;
		}
		if (this.CardModel.SpecialDurability2)
		{
			this.CurrentSpecial2 = this.CardModel.SpecialDurability2;
			this.BaseSpecial2Rate = this.CardModel.SpecialDurability2.RatePerDaytimePoint;
		}
		if (this.CardModel.SpecialDurability3)
		{
			this.CurrentSpecial3 = this.CardModel.SpecialDurability3;
			this.BaseSpecial3Rate = this.CardModel.SpecialDurability3.RatePerDaytimePoint;
		}
		if (this.CardModel.SpecialDurability4)
		{
			this.CurrentSpecial4 = this.CardModel.SpecialDurability4;
			this.BaseSpecial4Rate = this.CardModel.SpecialDurability4.RatePerDaytimePoint;
		}
		if (this.IsLiquid)
		{
			this.BaseEvaporationRate = this.CardModel.LiquidEvaporationRate;
		}
		if (_TransferedDurabilities != null)
		{
			if (_TransferedDurabilities.Spoilage && this.CardModel.SpoilageTime)
			{
				this.CurrentSpoilage = Mathf.Clamp(_TransferedDurabilities.Spoilage, 0f, this.CardModel.SpoilageTime.Max);
			}
			if (_TransferedDurabilities.Usage && this.CardModel.UsageDurability)
			{
				this.CurrentUsageDurability = Mathf.Clamp(_TransferedDurabilities.Usage, 0f, this.CardModel.UsageDurability.Max);
			}
			if (_TransferedDurabilities.Fuel && this.CardModel.FuelCapacity)
			{
				this.CurrentFuel = Mathf.Clamp(_TransferedDurabilities.Fuel, 0f, this.CardModel.FuelCapacity.Max);
			}
			if (_TransferedDurabilities.ConsumableCharges && this.CardModel.Progress)
			{
				this.CurrentProgress = Mathf.Clamp(_TransferedDurabilities.ConsumableCharges, 0f, this.CardModel.Progress.Max);
			}
			if (_TransferedDurabilities.Special1 && this.CardModel.SpecialDurability1)
			{
				this.CurrentSpecial1 = Mathf.Clamp(_TransferedDurabilities.Special1, 0f, this.CardModel.SpecialDurability1.Max);
			}
			if (_TransferedDurabilities.Special2 && this.CardModel.SpecialDurability2)
			{
				this.CurrentSpecial2 = Mathf.Clamp(_TransferedDurabilities.Special2, 0f, this.CardModel.SpecialDurability2.Max);
			}
			if (_TransferedDurabilities.Special3 && this.CardModel.SpecialDurability3)
			{
				this.CurrentSpecial3 = Mathf.Clamp(_TransferedDurabilities.Special3, 0f, this.CardModel.SpecialDurability3.Max);
			}
			if (_TransferedDurabilities.Special4 && this.CardModel.SpecialDurability4)
			{
				this.CurrentSpecial4 = Mathf.Clamp(_TransferedDurabilities.Special4, 0f, this.CardModel.SpecialDurability4.Max);
			}
			if (this.IsLiquid)
			{
				this.CurrentLiquidQuantity = Mathf.Clamp(_TransferedDurabilities.Liquid, 0f, this.CurrentMaxLiquidQuantity);
			}
		}
		if (this.IsPinned)
		{
			this.SetGraphicState(CardGraphics.CardGraphicsStates.Pinned, null);
		}
		else
		{
			this.SetGraphicState(CardGraphics.CardGraphicsStates.Normal, null);
		}
		this.CurrentMissingRequirements = null;
		if (this.CardVisuals)
		{
			this.CardVisuals.Setup(this);
		}
		if (!this.GM)
		{
			yield break;
		}
		this.CheckForRemoteEffects();
		CardsDropCollection cardsDropCollection = new CardsDropCollection();
		if (this.CardModel.AllDrops != null)
		{
			for (int l = 0; l < this.CardModel.AllDrops.Count; l++)
			{
				cardsDropCollection = this.CardModel.AllDrops[l];
				if (cardsDropCollection.CollectionUses.sqrMagnitude > 0 && !this.DroppedCollections.ContainsKey(cardsDropCollection.CollectionName))
				{
					this.DroppedCollections.Add(cardsDropCollection.CollectionName, Vector2Int.up * UnityEngine.Random.Range(cardsDropCollection.CollectionUses.x, cardsDropCollection.CollectionUses.y + 1));
				}
			}
		}
		if (_CollectionDrops != null)
		{
			for (int m = 0; m < _CollectionDrops.Count; m++)
			{
				if (!string.IsNullOrEmpty(_CollectionDrops[m].CollectionName))
				{
					if (this.DroppedCollections.ContainsKey(_CollectionDrops[m].CollectionName))
					{
						this.DroppedCollections[_CollectionDrops[m].CollectionName] = _CollectionDrops[m].CollectionDrops;
					}
					else
					{
						this.DroppedCollections.Add(_CollectionDrops[m].CollectionName, _CollectionDrops[m].CollectionDrops);
					}
				}
			}
		}
		if (this.CardModel.CardType == CardTypes.Blueprint || this.CardModel.CardType == CardTypes.EnvImprovement || this.CardModel.CardType == CardTypes.EnvDamage)
		{
			if (_Blueprint != null)
			{
				this.BlueprintData = new BlueprintSaveData(_Blueprint, this.CardModel.BlueprintStages, this.Environment, this.CardModel.CardType != CardTypes.EnvImprovement);
			}
			else
			{
				this.BlueprintData = new BlueprintSaveData(this.CardModel.BlueprintStages, this.Environment);
			}
		}
		else
		{
			this.BlueprintData = null;
		}
		int num = 1;
		if (this.CardsInInventory != null)
		{
			this.CardsInInventory.Clear();
		}
		else
		{
			this.CardsInInventory = new List<InventorySlot>();
		}
		if (this.CardModel.CardType == CardTypes.Explorable)
		{
			num = Mathf.Max(0, MBSingleton<ExplorationPopup>.Instance.ExplorationSlotCount);
		}
		else if (this.CardModel.CardType == CardTypes.Blueprint || this.CardModel.CardType == CardTypes.EnvImprovement || this.CardModel.CardType == CardTypes.EnvDamage)
		{
			if (this.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint)
			{
				num = 0;
			}
			else
			{
				if (this.BlueprintData.CurrentStage < this.CardModel.BlueprintStages.Length)
				{
					num = this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements.Length;
				}
				else
				{
					num = 0;
				}
				this.IsBlueprintInstance = (this.CardModel.CardType == CardTypes.Blueprint);
			}
		}
		else if (this.IsLegacyInventory)
		{
			num = this.CardModel.InventorySlots.Length;
		}
		if (num > 0)
		{
			for (int n = 0; n < num; n++)
			{
				this.CardsInInventory.Add(new InventorySlot());
			}
		}
		if (this.CardVisuals)
		{
			this.CardVisuals.UpdateInventoryInfo();
		}
		this.UpdateEnvironment(this.GM.CurrentEnvironment, this.GM.PrevEnvironment, this.GM.CurrentTravelIndex);
		this.UpdateVisibility();
		if (this.CurrentContainer)
		{
			if (!this.IsLiquid)
			{
				this.CurrentContainer.UpdateCookingRecipes();
			}
			else if (this.CurrentContainer.CurrentContainer)
			{
				this.CurrentContainer.CurrentContainer.UpdateCookingRecipes();
			}
		}
		if (!this.InBackground && !this.GM.AllVisibleCards.Contains(this) && !this.IsPinned && this.CardModel.CardType != CardTypes.Blueprint)
		{
			this.GM.AllVisibleCards.Add(this);
		}
		if (this.CardModel.CardType == CardTypes.Explorable)
		{
			if (_Exploration != null)
			{
				this.ExplorationData = new ExplorationSaveData(_Exploration);
			}
			else
			{
				this.ExplorationData = new ExplorationSaveData();
			}
			int num2 = -1;
			for (int num3 = 0; num3 < this.CardModel.ExplorationResults.Length; num3++)
			{
				if (!this.ExplorationData.HasData(this.CardModel.ExplorationResults[num3].ActionName, out num2))
				{
					this.ExplorationData.ExplorationResults.Add(new ExplorationResultSaveData(this.CardModel.ExplorationResults[num3]));
				}
			}
		}
		else
		{
			this.ExplorationData = null;
		}
		if (!this.IsPinned)
		{
			if (this.CardModel)
			{
				if (this.CardModel.HasPassiveEffects)
				{
					this.GM.StartCoroutine(this.UpdatePassiveEffects());
				}
				if (this.CardModel.VisualEffects != null)
				{
					for (int num4 = 0; num4 < this.CardModel.VisualEffects.Length; num4++)
					{
						AmbienceImageEffect.SpawnCardVisualEffect(this.CardModel.VisualEffects[num4], this);
					}
				}
			}
			this.UpdateEmptyCookingRecipe();
			if (this.CardModel.LocationsBackground)
			{
				this.GraphicsM.LocationsBackground.sprite = this.CardModel.LocationsBackground;
			}
			if (this.CardModel.BaseBackground)
			{
				this.GraphicsM.BaseBackground.sprite = this.CardModel.BaseBackground;
			}
			if (this.CardModel.CardType == CardTypes.Weather)
			{
				AmbienceImageEffect.SetWeather(this.CardModel.WeatherEffects);
			}
		}
		for (int num5 = 0; num5 < this.CardModel.CookingRecipes.Length; num5++)
		{
			if (!this.CardModel.CookingRecipes[num5].HideResultNotification)
			{
				if (this.CardModel.CookingRecipes[num5].IngredientChanges.ModType == CardModifications.Transform && this.CardModel.CookingRecipes[num5].IngredientChanges.TransformInto != null)
				{
					this.AddCookingResult(this.CardModel.CookingRecipes[num5].IngredientChanges.TransformInto);
				}
				if (this.CardModel.CookingRecipes[num5].Drops != null && this.CardModel.CookingRecipes[num5].Drops.Length != 0)
				{
					for (int num6 = 0; num6 < this.CardModel.CookingRecipes[num5].Drops.Length; num6++)
					{
						if ((float)this.CardModel.CookingRecipes[num5].Drops[num6].Quantity.sqrMagnitude > 0f && this.CardModel.CookingRecipes[num5].Drops[num6].DroppedCard)
						{
							this.AddCookingResult(this.CardModel.CookingRecipes[num5].Drops[num6].DroppedCard);
						}
					}
				}
				if (this.CardModel.CookingRecipes[num5].DropsAsCollection != null && this.CardModel.CookingRecipes[num5].DropsAsCollection.Length != 0)
				{
					if (this.CookingResultsList == null)
					{
						this.CookingResultsList = new List<CardData>();
					}
					for (int num7 = 0; num7 < this.CardModel.CookingRecipes[num5].DropsAsCollection.Length; num7++)
					{
						this.CardModel.CookingRecipes[num5].DropsAsCollection[num7].GetAllPossibleCards(this.CookingResultsList);
					}
				}
			}
		}
		if (this.CurrentSlot)
		{
			this.CurrentSlot.SortCardPile();
		}
		this.Initialized = true;
		yield return null;
		yield break;
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600018B RID: 395 RVA: 0x0000FFA0 File Offset: 0x0000E1A0
	public bool BlueprintComplete
	{
		get
		{
			return this.CardModel && (this.CardModel.CardType == CardTypes.Blueprint || this.CardModel.CardType == CardTypes.EnvImprovement || this.CardModel.CardType == CardTypes.EnvDamage) && (this.BlueprintData == null || this.BlueprintData.CurrentStage >= this.BlueprintSteps);
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0001000C File Offset: 0x0000E20C
	private void SetBlueprintStage(int _Stage)
	{
		this.BlueprintData.CurrentStage = _Stage;
		if (this.CardModel.CardType == CardTypes.EnvImprovement && _Stage >= this.CardModel.BlueprintStages.Length)
		{
			this.GM.AddRemotePassiveEffects(this);
		}
		for (int i = 0; i < this.CardsInInventory.Count; i++)
		{
			if (this.CardsInInventory[i] != null && !this.CardsInInventory[i].IsFree)
			{
				for (int j = this.CardsInInventory[i].AllCards.Count - 1; j >= 0; j--)
				{
					this.GraphicsM.MoveCardToSlot(this.CardsInInventory[i].AllCards[j], new SlotInfo(SlotsTypes.Item, 0), false, false);
					if (this.CardsInInventory[i].AllCards.Count == 0)
					{
						break;
					}
				}
			}
		}
		this.CardsInInventory.Clear();
		if (this.BlueprintData.CurrentStage < this.CardModel.BlueprintStages.Length)
		{
			for (int k = 0; k < this.CardModel.BlueprintStages[this.BlueprintData.CurrentStage].RequiredElements.Length; k++)
			{
				this.CardsInInventory.Add(new InventorySlot());
			}
		}
		if (this.CardVisuals)
		{
			this.CardVisuals.UpdateInventoryInfo();
		}
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00010169 File Offset: 0x0000E369
	public void IncreaseBlueprintStage()
	{
		this.SetBlueprintStage(this.BlueprintData.CurrentStage + 1);
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0001017E File Offset: 0x0000E37E
	public void DecreaseBlueprintStage()
	{
		this.SetBlueprintStage(this.BlueprintData.CurrentStage - 1);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00010194 File Offset: 0x0000E394
	public void SetBlueprintStage(int _Stage, bool _Progressively)
	{
		if (!this.CardModel)
		{
			return;
		}
		if (this.CardModel.BlueprintStages == null)
		{
			return;
		}
		if (this.CardModel.BlueprintStages.Length == 0)
		{
			return;
		}
		if (this.BlueprintData.CurrentStage == _Stage)
		{
			return;
		}
		if (!_Progressively)
		{
			this.SetBlueprintStage(_Stage);
			return;
		}
		int currentStage = this.BlueprintData.CurrentStage;
		if (currentStage > _Stage)
		{
			for (int i = currentStage; i > _Stage; i--)
			{
				this.DecreaseBlueprintStage();
			}
			return;
		}
		for (int j = currentStage; j < _Stage; j++)
		{
			this.IncreaseBlueprintStage();
		}
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0001021B File Offset: 0x0000E41B
	public IEnumerator UpdatePassiveEffects()
	{
		InGameCardBase.<>c__DisplayClass340_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		if (!this.CardModel || this.IsPinned || !this.Initialized)
		{
			yield break;
		}
		if (this.CardModel.CardType == CardTypes.EnvImprovement && !this.BlueprintComplete)
		{
			yield break;
		}
		CS$<>8__locals1.waitFor = new List<CoroutineController>();
		if (this.CardModel.PassiveEffects != null && this.CardModel.PassiveEffects.Length != 0)
		{
			for (int i = 0; i < this.CardModel.PassiveEffects.Length; i++)
			{
				if (string.IsNullOrEmpty(this.CardModel.PassiveEffects[i].EffectName))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Passive Effect with index ",
						i.ToString(),
						" on ",
						this.CardModel.name,
						" does not have a name, it will be disabled."
					}));
				}
				else
				{
					this.<UpdatePassiveEffects>g__UpdatePassiveEffect|340_0(this.CardModel.PassiveEffects[i], ref CS$<>8__locals1);
				}
			}
		}
		if (this.ExternalPassiveEffects != null)
		{
			for (int j = 0; j < this.ExternalPassiveEffects.Count; j++)
			{
				if (string.IsNullOrEmpty(this.ExternalPassiveEffects[j].EffectName))
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Empty Remote Effect on ",
						base.name,
						" from ",
						this.ExternalPassiveEffects[j].ConditionsCard
					}));
				}
				else
				{
					this.<UpdatePassiveEffects>g__UpdatePassiveEffect|340_0(this.ExternalPassiveEffects[j], ref CS$<>8__locals1);
				}
			}
		}
		if ((this.InBackground && !this.CardModel.AlwaysUpdate) || this.Destroyed)
		{
			this.GM.StartCoroutineEx(this.CancelStatModifiers(), out CS$<>8__locals1.controller);
			CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
		}
		else if (this.CardModel.PassiveStatEffects != null)
		{
			if (this.CardModel.PassiveStatEffects.Length != 0)
			{
				if (this.InBackground && this.CardModel.AffectStatsOnlyWhenNotInBackground)
				{
					this.GM.StartCoroutineEx(this.CancelStatModifiers(), out CS$<>8__locals1.controller);
					CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
				}
				else if (!this.CardModel.AffectStatsOnlyWhenEquipped)
				{
					this.GM.StartCoroutineEx(this.ApplyStatModifiers(), out CS$<>8__locals1.controller);
					CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
				}
				else if (this.CurrentSlot)
				{
					if (this.CurrentSlot.SlotType == SlotsTypes.Equipment)
					{
						this.GM.StartCoroutineEx(this.ApplyStatModifiers(), out CS$<>8__locals1.controller);
						CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
					}
					else
					{
						this.GM.StartCoroutineEx(this.CancelStatModifiers(), out CS$<>8__locals1.controller);
						CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
					}
				}
				else
				{
					this.GM.StartCoroutineEx(this.CancelStatModifiers(), out CS$<>8__locals1.controller);
					CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
				}
			}
			else
			{
				this.GM.StartCoroutineEx(this.CancelStatModifiers(), out CS$<>8__locals1.controller);
				CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
			}
		}
		else
		{
			this.GM.StartCoroutineEx(this.CancelStatModifiers(), out CS$<>8__locals1.controller);
			CS$<>8__locals1.waitFor.Add(CS$<>8__locals1.controller);
		}
		while (CoroutineController.WaitForControllerList(CS$<>8__locals1.waitFor))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0001022C File Offset: 0x0000E42C
	public void UpdatePassiveEffectStacks()
	{
		for (int i = 0; i < this.CardModel.PassiveEffects.Length; i++)
		{
			if (this.CardModel.PassiveEffects[i].EffectStacksWithRequiredCards && this.PassiveEffects.ContainsKey(this.CardModel.PassiveEffects[i].EffectName))
			{
				PassiveEffect passiveEffect = this.PassiveEffects[this.CardModel.PassiveEffects[i].EffectName];
				passiveEffect.UpdateStack(this);
				if (passiveEffect.CurrentStack != passiveEffect.PrevStack)
				{
					if (passiveEffect.CurrentStack > passiveEffect.PrevStack)
					{
						for (int j = passiveEffect.PrevStack; j < passiveEffect.CurrentStack; j++)
						{
							this.ApplyPassiveEffectDurabilities(passiveEffect);
							this.GM.StartCoroutine(this.ApplyPassiveEffectStatModifiers(passiveEffect));
						}
					}
					else
					{
						for (int k = passiveEffect.PrevStack; k > passiveEffect.CurrentStack; k--)
						{
							this.CancelPassiveEffectDurabilities(passiveEffect);
							this.GM.StartCoroutine(this.CancelPassiveEffectStatModifiers(passiveEffect));
						}
					}
					this.PassiveEffects[this.CardModel.PassiveEffects[i].EffectName] = passiveEffect;
				}
			}
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00010370 File Offset: 0x0000E570
	public void UpdateProducedLiquids()
	{
		if (!this.CardModel)
		{
			return;
		}
		if (this.CardModel.CardType == CardTypes.EnvImprovement && !this.BlueprintComplete)
		{
			return;
		}
		if (!this.ContainedLiquid && this.CurrentProducedLiquids.Count > 0)
		{
			LiquidDrop liquidDrop = this.CurrentProducedLiquids[0];
			for (int i = 1; i < this.CurrentProducedLiquids.Count; i++)
			{
				if (this.CurrentProducedLiquids[i].LiquidCard == liquidDrop.LiquidCard)
				{
					liquidDrop.Quantity += this.CurrentProducedLiquids[i].Quantity;
				}
			}
			CardAction cardAction = new CardAction();
			cardAction.ProducedCards = new CardsDropCollection[1];
			cardAction.ProducedCards[0] = new CardsDropCollection
			{
				CollectionName = "Creating liquid " + liquidDrop.LiquidCard.name,
				CollectionWeight = 1
			};
			cardAction.ProducedCards[0].SetLiquidDrop(liquidDrop);
			GameManager.PerformAction(cardAction, this, false);
		}
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00010483 File Offset: 0x0000E683
	protected IEnumerator ApplyPassiveEffect(PassiveEffect _Effect)
	{
		string effectName = _Effect.EffectName;
		if (this.PassiveEffects.ContainsKey(effectName))
		{
			yield break;
		}
		InGameCardBase inGameCardBase = _Effect.ConditionsCard ? _Effect.ConditionsCard : this;
		DynamicLayoutSlot conditionsSlot = _Effect.ConditionsSlot ? _Effect.ConditionsSlot : null;
		this.PassiveEffects.Add(effectName, _Effect.Instantiate(inGameCardBase, conditionsSlot));
		if (_Effect.OnBoardEffectCap > 0 && !string.IsNullOrEmpty(effectName) && inGameCardBase)
		{
			string globalEffectName = PassiveEffect.GetGlobalEffectName(inGameCardBase.CardModel, effectName);
			if (this.GM.PassiveEffectsStacks.ContainsKey(globalEffectName))
			{
				Dictionary<string, int> passiveEffectsStacks = this.GM.PassiveEffectsStacks;
				string key = globalEffectName;
				int num = passiveEffectsStacks[key];
				passiveEffectsStacks[key] = num + 1;
			}
			else
			{
				this.GM.PassiveEffectsStacks.Add(globalEffectName, 1);
			}
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		if (_Effect.EffectStacksWithRequiredCards)
		{
			PassiveEffect passiveEffect = this.PassiveEffects[effectName];
			passiveEffect.UpdateStack(this);
			for (int i = 0; i < passiveEffect.CurrentStack; i++)
			{
				this.ApplyPassiveEffectDurabilities(passiveEffect);
				CoroutineController item;
				this.GM.StartCoroutineEx(this.ApplyPassiveEffectStatModifiers(this.PassiveEffects[effectName]), out item);
				waitFor.Add(item);
			}
			passiveEffect.UpdateStack(this);
			this.PassiveEffects[effectName] = passiveEffect;
		}
		else
		{
			this.ApplyPassiveEffectDurabilities(this.PassiveEffects[effectName]);
			CoroutineController item;
			this.GM.StartCoroutineEx(this.ApplyPassiveEffectStatModifiers(this.PassiveEffects[effectName]), out item);
			waitFor.Add(item);
		}
		this.UpdateProducedLiquids();
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0001049C File Offset: 0x0000E69C
	protected void ApplyPassiveEffectDurabilities(PassiveEffect _Effect)
	{
		if (this.CardModel.SpoilageTime && _Effect.SpoilageRateModifier)
		{
			this.BaseSpoilageRate += _Effect.SpoilageRateModifier;
		}
		if (this.CardModel.UsageDurability && _Effect.UsageRateModifier)
		{
			this.BaseUsageRate += _Effect.UsageRateModifier;
		}
		if (this.CardModel.FuelCapacity && _Effect.FuelRateModifier)
		{
			this.BaseFuelRate += _Effect.FuelRateModifier;
		}
		if (this.CardModel.Progress && _Effect.ConsumableChargesModifier)
		{
			this.BaseConsumableRate += _Effect.ConsumableChargesModifier;
		}
		if (this.CardModel.SpecialDurability1 && _Effect.Special1RateModifier)
		{
			this.BaseSpecial1Rate += _Effect.Special1RateModifier;
		}
		if (this.CardModel.SpecialDurability2 && _Effect.Special2RateModifier)
		{
			this.BaseSpecial2Rate += _Effect.Special2RateModifier;
		}
		if (this.CardModel.SpecialDurability3 && _Effect.Special3RateModifier)
		{
			this.BaseSpecial3Rate += _Effect.Special3RateModifier;
		}
		if (this.CardModel.SpecialDurability4 && _Effect.Special4RateModifier)
		{
			this.BaseSpecial4Rate += _Effect.Special4RateModifier;
		}
		if (this.IsLiquid)
		{
			this.BaseEvaporationRate += _Effect.LiquidRateModifier;
		}
		else if (this.ContainedLiquid)
		{
			this.ContainedLiquid.BaseEvaporationRate += _Effect.LiquidRateModifier;
		}
		if (this.IsLiquidContainer && !_Effect.GeneratedLiquid.IsEmpty)
		{
			this.CurrentProducedLiquids.Add(_Effect.GeneratedLiquid);
		}
		if (this.IsInventoryCard && !this.IsLegacyInventory)
		{
			this.WeightCapacityBonus += _Effect.WeightCapacityModifier;
		}
	}

	// Token: 0x06000195 RID: 405 RVA: 0x000106ED File Offset: 0x0000E8ED
	protected IEnumerator ApplyPassiveEffectStatModifiers(PassiveEffect _Effect)
	{
		List<CoroutineController> waitFor = new List<CoroutineController>();
		for (int i = 0; i < _Effect.StatModifiers.Length; i++)
		{
			waitFor.Add(this.GM.ApplyStatModifier(_Effect.StatModifiers[i], _Effect.OnlyOnBaseActions ? StatModification.AtBaseModifier : StatModification.GlobalModifier, StatModifierReport.SourceFromPassiveEffect(_Effect.EffectName, this), this));
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00010703 File Offset: 0x0000E903
	protected IEnumerator CancelPassiveEffect(PassiveEffect _Effect)
	{
		string effectName = _Effect.EffectName;
		if (!this.PassiveEffects.ContainsKey(effectName))
		{
			yield break;
		}
		PassiveEffect passiveEffect = this.PassiveEffects[effectName];
		this.PassiveEffects.Remove(effectName);
		if (_Effect.OnBoardEffectCap > 0 && !string.IsNullOrEmpty(effectName) && passiveEffect.ConditionsCard)
		{
			string globalEffectName = PassiveEffect.GetGlobalEffectName(passiveEffect.ConditionsCard.CardModel, effectName);
			if (this.GM.PassiveEffectsStacks.ContainsKey(globalEffectName))
			{
				Dictionary<string, int> passiveEffectsStacks = this.GM.PassiveEffectsStacks;
				string key = globalEffectName;
				int num = passiveEffectsStacks[key];
				passiveEffectsStacks[key] = num - 1;
			}
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		if (passiveEffect.EffectStacksWithRequiredCards)
		{
			for (int i = 0; i < passiveEffect.CurrentStack; i++)
			{
				this.CancelPassiveEffectDurabilities(passiveEffect);
				CoroutineController item;
				this.GM.StartCoroutineEx(this.CancelPassiveEffectStatModifiers(passiveEffect), out item);
				waitFor.Add(item);
			}
		}
		else
		{
			this.CancelPassiveEffectDurabilities(passiveEffect);
			CoroutineController item;
			this.GM.StartCoroutineEx(this.CancelPassiveEffectStatModifiers(passiveEffect), out item);
			waitFor.Add(item);
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0001071C File Offset: 0x0000E91C
	protected void CancelPassiveEffectDurabilities(PassiveEffect _Effect)
	{
		if (this.CardModel.SpoilageTime && _Effect.SpoilageRateModifier)
		{
			this.BaseSpoilageRate -= _Effect.SpoilageRateModifier;
		}
		if (this.CardModel.UsageDurability && _Effect.UsageRateModifier)
		{
			this.BaseUsageRate -= _Effect.UsageRateModifier;
		}
		if (this.CardModel.FuelCapacity && _Effect.FuelRateModifier)
		{
			this.BaseFuelRate -= _Effect.FuelRateModifier;
		}
		if (this.CardModel.Progress && _Effect.ConsumableChargesModifier)
		{
			this.BaseConsumableRate -= _Effect.ConsumableChargesModifier;
		}
		if (this.CardModel.SpecialDurability1 && _Effect.Special1RateModifier)
		{
			this.BaseSpecial1Rate -= _Effect.Special1RateModifier;
		}
		if (this.CardModel.SpecialDurability2 && _Effect.Special2RateModifier)
		{
			this.BaseSpecial2Rate -= _Effect.Special2RateModifier;
		}
		if (this.CardModel.SpecialDurability3 && _Effect.Special3RateModifier)
		{
			this.BaseSpecial3Rate -= _Effect.Special3RateModifier;
		}
		if (this.CardModel.SpecialDurability4 && _Effect.Special4RateModifier)
		{
			this.BaseSpecial4Rate -= _Effect.Special4RateModifier;
		}
		if (this.IsLiquid)
		{
			this.BaseEvaporationRate -= _Effect.LiquidRateModifier;
		}
		else if (this.ContainedLiquid)
		{
			this.ContainedLiquid.BaseEvaporationRate -= _Effect.LiquidRateModifier;
		}
		if (this.IsLiquidContainer && !_Effect.GeneratedLiquid.IsEmpty && this.CurrentProducedLiquids.Count > 0)
		{
			this.CurrentProducedLiquids.Remove(_Effect.GeneratedLiquid);
		}
		if (this.IsInventoryCard && !this.IsLegacyInventory)
		{
			this.WeightCapacityBonus -= _Effect.WeightCapacityModifier;
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0001097C File Offset: 0x0000EB7C
	protected IEnumerator CancelPassiveEffectStatModifiers(PassiveEffect _Effect)
	{
		List<CoroutineController> waitFor = new List<CoroutineController>();
		for (int i = 0; i < _Effect.StatModifiers.Length; i++)
		{
			waitFor.Add(this.GM.ApplyStatModifier(_Effect.StatModifiers[i].Inverse, _Effect.OnlyOnBaseActions ? StatModification.AtBaseModifier : StatModification.GlobalModifier, StatModifierReport.SourceFromPassiveEffect(_Effect.EffectName, this), this));
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00010992 File Offset: 0x0000EB92
	protected IEnumerator ApplyStatModifiers()
	{
		if (this.CurrentStatModifiers.Count > 0 || this.IsPinned)
		{
			yield break;
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		for (int i = 0; i < this.CardModel.PassiveStatEffects.Length; i++)
		{
			this.CurrentStatModifiers.Add(new StatModifier
			{
				Stat = this.CardModel.PassiveStatEffects[i].Stat,
				ValueModifier = (Mathf.Approximately(this.CardModel.PassiveStatEffects[i].ValueModifier.x, this.CardModel.PassiveStatEffects[i].ValueModifier.y) ? (Vector2.one * this.CardModel.PassiveStatEffects[i].ValueModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.CardModel.PassiveStatEffects[i].ValueModifier.x, this.CardModel.PassiveStatEffects[i].ValueModifier.y))),
				RateModifier = (Mathf.Approximately(this.CardModel.PassiveStatEffects[i].RateModifier.x, this.CardModel.PassiveStatEffects[i].RateModifier.y) ? (Vector2.one * this.CardModel.PassiveStatEffects[i].RateModifier.x) : (Vector2.one * UnityEngine.Random.Range(this.CardModel.PassiveStatEffects[i].RateModifier.x, this.CardModel.PassiveStatEffects[i].RateModifier.y)))
			});
			waitFor.Add(this.GM.ApplyStatModifier(this.CurrentStatModifiers[i], this.CardModel.AffectStatsOnlyOnBase ? StatModification.AtBaseModifier : StatModification.GlobalModifier, StatModifierReport.SourceFromPassiveEffect("Passive Stat Effect " + i.ToString(), this), this));
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x000109A1 File Offset: 0x0000EBA1
	protected IEnumerator CancelStatModifiers()
	{
		if (this.CurrentStatModifiers.Count == 0)
		{
			yield break;
		}
		StatModifier[] array = this.CurrentStatModifiers.ToArray();
		this.CurrentStatModifiers.Clear();
		List<CoroutineController> waitFor = new List<CoroutineController>();
		for (int i = 0; i < array.Length; i++)
		{
			waitFor.Add(this.GM.ApplyStatModifier(array[i].Inverse, this.CardModel.AffectStatsOnlyOnBase ? StatModification.AtBaseModifier : StatModification.GlobalModifier, StatModifierReport.SourceFromPassiveEffect("Passive Stat Effect " + i.ToString(), this), this));
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x000109B0 File Offset: 0x0000EBB0
	protected virtual void Start()
	{
		this.GM = MBSingleton<GameManager>.Instance;
		if (!this.CardVisuals)
		{
			this.CardVisuals = base.GetComponent<CardGraphics>();
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x000109D6 File Offset: 0x0000EBD6
	protected virtual void OnEnable()
	{
		this.RegisterToEvents();
		if (MobilePlatformDetection.IsMobilePlatform)
		{
			this.SetCollider(!GameManager.DraggedCard);
			return;
		}
		this.SetCollider(false);
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00010A00 File Offset: 0x0000EC00
	protected override void OnDisable()
	{
		this.UnregisterToEvents();
		base.OnDisable();
		if (this.Destroyed && base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		if (this.ActionPulse != null)
		{
			this.ActionPulse.Complete(true);
			this.ActionPulse = null;
		}
		base.transform.localScale = this.InitialScale;
		if (MBSingleton<ExplorationPopup>.Instance && MBSingleton<ExplorationPopup>.Instance.HoveredCard == this)
		{
			MBSingleton<ExplorationPopup>.Instance.HoveredCard = null;
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00010A90 File Offset: 0x0000EC90
	public override void OnHoverEnter()
	{
		base.OnHoverEnter();
		if (this.CurrentSlot && (this.CurrentSlot.SlotType == SlotsTypes.Improvement || this.CurrentSlot.SlotType == SlotsTypes.EnvDamage))
		{
			MBSingleton<ExplorationPopup>.Instance.HoveredCard = this;
		}
		if (!this.DragStackCompatible)
		{
			GameManager.ClearDraggedStack();
		}
		GameManager.HoveredCard = this;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00010AEC File Offset: 0x0000ECEC
	public override void OnHoverExit()
	{
		base.OnHoverExit();
		if (MBSingleton<ExplorationPopup>.Instance.HoveredCard == this)
		{
			MBSingleton<ExplorationPopup>.Instance.HoveredCard = null;
		}
		if (GameManager.HoveredCard == this)
		{
			GameManager.HoveredCard = null;
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00010B24 File Offset: 0x0000ED24
	protected void RegisterToEvents()
	{
		if (this.IsRegisteredToDragEvents || this.Destroyed)
		{
			return;
		}
		if (this.CardModel && this.IsLiquid)
		{
			return;
		}
		GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.GetPossibleAction));
		GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.CancelPossibleAction));
		if (GameManager.DraggedCard)
		{
			this.GetPossibleAction(GameManager.DraggedCard);
		}
		this.IsRegisteredToDragEvents = true;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x00010BB8 File Offset: 0x0000EDB8
	protected void UnregisterToEvents()
	{
		this.CancelPossibleAction(null);
		GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Remove(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.GetPossibleAction));
		GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Remove(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.CancelPossibleAction));
		this.IsRegisteredToDragEvents = false;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00010C15 File Offset: 0x0000EE15
	public void SetTutorialHighlight(TutorialHighlightState _State)
	{
		this.TutorialHighlight = _State;
		if (this.CardVisuals)
		{
			this.CardVisuals.UpdateTutorialHighlight();
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00010C36 File Offset: 0x0000EE36
	public void SetGraphicState(CardGraphics.CardGraphicsStates _State, InGameCardBase _InventoryCard = null)
	{
		this.CurrentGraphicState = _State;
		this.GraphicStateInventoryCard = _InventoryCard;
		if (this.CardVisuals)
		{
			this.CardVisuals.SetGraphicState(this.CurrentGraphicState, this.GraphicStateInventoryCard);
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00010C6A File Offset: 0x0000EE6A
	private void UpdateMissingRequirements(MissingStatInfo _Stat, string _Durabilities, string _BlockingStatus, bool _ActionPlaying, bool _UnavailableInDemo)
	{
		this.CurrentMissingRequirements = new MissingReqInfo(_Stat, _Durabilities, _BlockingStatus, _ActionPlaying, _UnavailableInDemo);
		if (this.CardVisuals)
		{
			this.CardVisuals.UpdateMissingRequirements(this.CurrentMissingRequirements);
		}
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00010C9C File Offset: 0x0000EE9C
	public void UpdateCookingProgress(float _Value, int _RemainingTicks, bool _Paused, string _CustomCookingText, bool _HideProgress, bool _FillsLiquid)
	{
		this.CurrentCookingBarInfo = new CookingBarInfo(_Value, _RemainingTicks, _Paused, _CustomCookingText, _HideProgress, _FillsLiquid);
		if (this.CardVisuals)
		{
			this.CardVisuals.UpdateCookingProgress(this.CurrentCookingBarInfo);
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00010CD0 File Offset: 0x0000EED0
	public void SetCollider(bool _Small)
	{
		this.SmallCollider = _Small;
		if (this.CardVisuals)
		{
			this.CardVisuals.SetCollider(this.SmallCollider);
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00010CF7 File Offset: 0x0000EEF7
	public void SetActionPerformedObjects(bool _Active)
	{
		this.ActionPerformedObjects = _Active;
		if (this.CardVisuals)
		{
			GraphicsManager.SetActiveGroup(this.CardVisuals.ActionPerformedObjects, this.ActionPerformedObjects);
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00010D24 File Offset: 0x0000EF24
	protected virtual bool CheckIfVisibleOnScreen()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return false;
		}
		if (!this.GM)
		{
			return false;
		}
		if (!this.CardModel)
		{
			return false;
		}
		if (this.CardModel.CardType == CardTypes.Environment || this.CardModel.CardType == CardTypes.Liquid)
		{
			return false;
		}
		if (!this.CurrentSlot)
		{
			return !this.GM.EnvironmentTransition && CameraModeSwitch.CanvasWorldRect.Overlaps(this.CardRect);
		}
		if ((this.CurrentSlot.SlotType == SlotsTypes.Base || this.CurrentSlot.SlotType == SlotsTypes.Location) && this.GM.EnvironmentTransition)
		{
			return false;
		}
		if (Vector2.SqrMagnitude(base.transform.position - this.CurrentSlot.WorldPosition) > 0.0001f && this.CurrentSlot.SlotType != SlotsTypes.Blueprint)
		{
			return CameraModeSwitch.CanvasWorldRect.Overlaps(this.CardRect);
		}
		return this.CurrentSlot.IsVisible && this.IndexInPileMinusDraggedIndex() <= 1;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00010E3C File Offset: 0x0000F03C
	protected virtual void LateUpdate()
	{
		if (!this.Initialized)
		{
			return;
		}
		this.VisibleOnScreen = this.CheckIfVisibleOnScreen();
		if (this.VisibleOnScreen && !this.CardVisuals)
		{
			this.CardVisuals = CardVisualsManager.NextCard(base.transform, base.transform.position, this.CardModel, this.IsBlueprintInstance);
			this.PrivateBlocksRaycasts = !this.CardVisuals.DontBlockRaycasts;
			this.CardVisuals.Setup(this);
			if (this.CurrentSlot)
			{
				this.CurrentSlot.SortCardPile();
			}
		}
		else if (!this.VisibleOnScreen && this.CardVisuals && !this.GM.EnvironmentTransition)
		{
			CardVisualsManager.FreeCard(this.CardVisuals);
			this.CardVisuals = null;
		}
		if (this.CardVisuals)
		{
			if (this.CardVisuals.FullCollision)
			{
				this.CardVisuals.FullCollision.enabled = (!this.Destroyed && !this.IsPerformingAction);
			}
			if (this.CardVisuals.SmallCollision)
			{
				this.CardVisuals.SmallCollision.enabled = (!this.Destroyed && !this.IsPerformingAction);
			}
		}
		if (this.Destroyed || !this.CurrentParentObject)
		{
			if (!this.IsPulsing && !this.IsTimeAnimated && !this.IsPerformingAction)
			{
				base.transform.position = Vector3.up * 1000f;
				this.TimeToDeactivation -= Time.deltaTime;
				if (this.TimeToDeactivation <= 0f)
				{
					base.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			if (!this.IsPulsing && !this.IsTimeAnimated && !this.IsPerformingAction)
			{
				this.MoveToParent(base.transform, this.CurrentParentObject);
				if (base.transform.parent == this.CurrentParentObject)
				{
					if (this.IsHovered && this.ImpossibleAction == null && this.PossibleAction == null && this.HasAction)
					{
						base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.InitialScale + Vector3.one * 0.25f, 10f * Time.deltaTime);
					}
					else
					{
						base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.InitialScale, 10f * Time.deltaTime);
					}
				}
			}
			if (this.InspectionDuplicate)
			{
				this.MoveToParent(this.InspectionDuplicate, this.DuplicateParentObject);
			}
			if (this.PossibleAction != null)
			{
				if (this.IsHovered && this.PossibleAction.TotalDaytimeCost >= this.GM.MinTicksForHoldAction)
				{
					this.ActionHoldTimer += Time.deltaTime;
				}
				else
				{
					this.ActionHoldTimer = 0f;
				}
			}
			else
			{
				this.ActionHoldTimer = 0f;
			}
		}
		if (!this.Destroyed)
		{
			this.UpdateActiveState();
		}
		this.TooltipTitle = base.Title;
		base.NormalizedHoldTime = Mathf.Clamp01(this.ActionHoldTimer / this.GM.HoldActionDuration);
		if (this.CardVisuals)
		{
			this.CardVisuals.transform.position = base.transform.position;
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x000111B4 File Offset: 0x0000F3B4
	private int IndexInPileMinusDraggedIndex()
	{
		if (!(this is InGameDraggableCard))
		{
			return this.CurrentSlot.CardIndexInPile(this);
		}
		int num = this.CurrentSlot.CardIndexInPile(this);
		int draggedCardIndex = this.CurrentSlot.DraggedCardIndex;
		if (draggedCardIndex == -1)
		{
			return num;
		}
		return num - draggedCardIndex;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x000111F8 File Offset: 0x0000F3F8
	public void UpdateActiveState()
	{
		if (this.ForceActive)
		{
			base.gameObject.SetActive(true);
			return;
		}
		if (this.CurrentParentObject && base.transform.parent != this.CurrentParentObject)
		{
			return;
		}
		if (!this.CurrentSlot || this.InBackground)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.IndexInPileMinusDraggedIndex() > 1)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00011284 File Offset: 0x0000F484
	private void MoveToParent(Transform _Target, Transform _ToParent)
	{
		InGameCardBase.<>c__DisplayClass368_0 CS$<>8__locals1;
		CS$<>8__locals1._Target = _Target;
		CS$<>8__locals1._ToParent = _ToParent;
		CS$<>8__locals1.<>4__this = this;
		if (CS$<>8__locals1._Target.parent != CS$<>8__locals1._ToParent)
		{
			if (CS$<>8__locals1._Target.position.z != CS$<>8__locals1._ToParent.position.z)
			{
				CS$<>8__locals1._Target.position = new Vector3(CS$<>8__locals1._Target.position.x, CS$<>8__locals1._Target.position.y, CS$<>8__locals1._ToParent.position.z);
			}
			if (!this.CardVisuals && CS$<>8__locals1._Target == base.transform)
			{
				this.PulseAfterReachingSlot = false;
				this.<MoveToParent>g__FinishMoving|368_0(ref CS$<>8__locals1);
				return;
			}
			if (Vector3.SqrMagnitude(CS$<>8__locals1._Target.position - CS$<>8__locals1._ToParent.position) > 0.01f && CS$<>8__locals1._ToParent.gameObject.activeInHierarchy)
			{
				CS$<>8__locals1._Target.position = Vector3.MoveTowards(CS$<>8__locals1._Target.position, CS$<>8__locals1._ToParent.position, Mathf.Clamp(Vector3.Distance(CS$<>8__locals1._Target.position, CS$<>8__locals1._ToParent.position) * 10f, 10f, 70f) * Time.deltaTime);
				return;
			}
			this.<MoveToParent>g__FinishMoving|368_0(ref CS$<>8__locals1);
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x000113F4 File Offset: 0x0000F5F4
	public virtual void Pulse(float _ExtraDelay)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.ActionPulse != null)
		{
			this.ActionPulse.Complete(true);
			this.ActionPulse = null;
		}
		base.transform.localScale = this.InitialScale;
		if (this.CardVisuals && this.CardVisuals.HiddenInPile)
		{
			return;
		}
		this.ActionPulse = DOTween.Sequence();
		this.ActionPulse.Append(base.transform.DOPunchScale(Vector3.one * 0.25f, 0.33f, 5, 0.5f));
		this.ActionPulse.AppendInterval(_ExtraDelay);
		this.ActionPulse.AppendCallback(delegate
		{
			this.ActionPulse = null;
		});
		if (this.CurrentSlot && (base.transform.parent != this.CurrentSlot.GetParent || this.CardModel.CardType == CardTypes.Event))
		{
			this.SetActionPerformedObjects(true);
			this.ActionPulse.AppendCallback(delegate
			{
				this.SetActionPerformedObjects(false);
			});
		}
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00011510 File Offset: 0x0000F710
	public virtual void TimeAnimate(float _Duration)
	{
		if (!this.CardVisuals)
		{
			return;
		}
		if (this.TimeAnimation != null)
		{
			this.TimeAnimation.Kill(true);
		}
		this.CardVisuals.TimeAnimationTr.localPosition = Vector3.zero;
		this.CardVisuals.TimeAnimationTr.localRotation = Quaternion.identity;
		this.TimeAnimation = DOTween.Sequence();
		this.TimeAnimation.Append(this.CardVisuals.TimeAnimationTr.DOShakePosition(_Duration, 5f, 15, 90f, false, false));
		this.TimeAnimation.Insert(0f, this.CardVisuals.TimeAnimationTr.DOShakeRotation(_Duration, Vector3.forward * 1f, 15, 90f, false));
		this.TimeAnimation.AppendCallback(delegate
		{
			this.TimeAnimation = null;
		});
		if (this.CurrentSlot && (base.transform.parent != this.CurrentSlot.GetParent || this.CardModel.CardType == CardTypes.Event))
		{
			this.SetActionPerformedObjects(true);
			this.TimeAnimation.AppendCallback(delegate
			{
				this.SetActionPerformedObjects(false);
			});
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00011647 File Offset: 0x0000F847
	public virtual void CancelTimeAnimation()
	{
		if (this.TimeAnimation != null)
		{
			this.TimeAnimation.Complete(true);
		}
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00011660 File Offset: 0x0000F860
	public virtual void SetParent(Transform _Tr, bool _Instant = false)
	{
		this.GetManagers();
		if (this.InspectionDuplicate)
		{
			this.DuplicateParentObject = _Tr;
			if (!_Instant && this.InspectionDuplicate.gameObject.activeSelf)
			{
				this.InspectionDuplicate.SetParent(this.GraphicsM.CardsMovingParent);
			}
			else
			{
				this.InspectionDuplicate.SetParent(_Tr);
				this.InspectionDuplicate.localPosition = Vector3.zero;
			}
			if (!this.Destroyed)
			{
				return;
			}
		}
		this.CurrentParentObject = _Tr;
		if (!_Instant && base.gameObject.activeSelf)
		{
			base.transform.SetParent(this.GraphicsM.CardsMovingParent);
		}
		else
		{
			base.transform.SetParent(_Tr);
			base.transform.localPosition = Vector3.zero;
		}
		if (this.ContainedLiquid)
		{
			this.ContainedLiquid.SetParent(this.CurrentParentObject, true);
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00011744 File Offset: 0x0000F944
	public virtual void SetInspectionParent(Transform _Tr, bool _PlaySound, bool _Instant = false)
	{
		if (!_Tr)
		{
			this.InspectionDuplicate = null;
			if (this.CurrentSlot)
			{
				this.SetParent(this.CurrentSlot.GetParent, _Instant);
			}
			if (_PlaySound)
			{
				MBSingleton<SoundManager>.Instance.PerformCardAppearanceSound(this.CardModel.WhenCreatedSounds);
				return;
			}
		}
		else
		{
			this.DuplicateParentObject = this.CurrentParentObject;
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			this.SetParent(_Tr, _Instant);
			if (this.CardVisuals)
			{
				this.InspectionDuplicate = this.CardVisuals.CreateDuplicate(this.DuplicateParentObject);
			}
			if (_PlaySound)
			{
				MBSingleton<SoundManager>.Instance.PerformCardAppearanceSound(this.CardModel.WhenCreatedSounds);
			}
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00011804 File Offset: 0x0000FA04
	public void SortInventory()
	{
		if (!this.CardModel)
		{
			return;
		}
		if (this.CardModel.CardType == CardTypes.Blueprint || this.CardModel.CardType == CardTypes.EnvImprovement || this.CardModel.CardType == CardTypes.EnvDamage)
		{
			return;
		}
		if (this.CardsInInventory == null)
		{
			return;
		}
		if (this.CardsInInventory.Count == 0)
		{
			return;
		}
		List<InventorySlot> list = new List<InventorySlot>();
		for (int i = 0; i < this.CardsInInventory.Count; i++)
		{
			if (!this.CardsInInventory[i].IsFree)
			{
				for (int j = 0; j < this.CardsInInventory[i].AllCards.Count; j++)
				{
					this.CardsInInventory[i].AllCards[j].CurrentSlotInfo.SlotIndex = list.Count;
				}
				list.Add(this.CardsInInventory[i]);
			}
		}
		for (int k = 0; k < this.CardsInInventory.Count; k++)
		{
			if (k >= list.Count)
			{
				this.CardsInInventory[k] = new InventorySlot();
			}
			else
			{
				this.CardsInInventory[k] = list[k];
			}
		}
		this.UpdateEmptyCookingRecipe();
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00011938 File Offset: 0x0000FB38
	public void OnDrop(PointerEventData _Pointer)
	{
		if (_Pointer.pointerDrag == base.gameObject || GameManager.DraggedCard == this || this.IsPinned)
		{
			return;
		}
		InGameCardBase.DroppedCard = GameManager.DraggedCard;
		if (!InGameCardBase.DroppedCard)
		{
			return;
		}
		if (!InGameCardBase.DroppedCard.CanBeDragged)
		{
			return;
		}
		if (this.PossibleAction == null)
		{
			if (this.ImpossibleAction == null && this.HasAction)
			{
				List<InGameCardBase> list = GameManager.DraggedStackResult();
				for (int i = 0; i < list.Count; i++)
				{
					if (!this.DropInInventory(list[i]))
					{
						return;
					}
				}
				return;
			}
			if (this.ImpossibleAction != null && GameManager.PerformingAction)
			{
				this.GraphicsM.ShowImpossibleToInspect(this, LocalizedString.ActionHappening);
			}
			return;
		}
		if (this.PossibleAction.TotalDaytimeCost >= this.GM.MinTicksForHoldAction && this.ActionHoldTimer < this.GM.HoldActionDuration)
		{
			if (InGameCardBase.DroppedCard.CardModel == this.CardModel && this.CurrentSlot && this.CurrentSlot.PileCompatible(this.CardModel))
			{
				this.GraphicsM.MoveCardToSlot(InGameCardBase.DroppedCard, this.CurrentSlot.ToInfo(), true, false);
			}
			return;
		}
		if (MBSingleton<GraphicsManager>.Instance.InspectedCard == this)
		{
			if (InGameCardBase.DroppedCard.CurrentContainer == this)
			{
				this.GraphicsM.GetSlotForCard(InGameCardBase.DroppedCard.CardModel, InGameCardBase.DroppedCard.ContainedLiquidModel, new SlotInfo(SlotsTypes.Item, 0), null, null, 0).AssignCard(InGameCardBase.DroppedCard, false);
			}
			this.GraphicsM.InspectCard(this, false);
		}
		switch (this.ActionType)
		{
		case InGameCardBase.ActionTypes.LiquidDouble:
			if (!this.ActionIsReversed)
			{
				GameManager.PerformCardOnCardAction(this.PossibleAction, InGameCardBase.DroppedCard.ContainedLiquid, this.ContainedLiquid);
				return;
			}
			GameManager.PerformCardOnCardAction(this.PossibleAction, this.ContainedLiquid, InGameCardBase.DroppedCard.ContainedLiquid);
			return;
		case InGameCardBase.ActionTypes.GivenLiquidStandard:
			if (!this.ActionIsReversed)
			{
				GameManager.PerformCardOnCardAction(this.PossibleAction, InGameCardBase.DroppedCard.ContainedLiquid, this);
				return;
			}
			GameManager.PerformCardOnCardAction(this.PossibleAction, this, InGameCardBase.DroppedCard.ContainedLiquid);
			return;
		case InGameCardBase.ActionTypes.ReceivingLiquidStandard:
			if (!this.ActionIsReversed)
			{
				GameManager.PerformCardOnCardAction(this.PossibleAction, InGameCardBase.DroppedCard, this.ContainedLiquid);
				return;
			}
			GameManager.PerformCardOnCardAction(this.PossibleAction, this.ContainedLiquid, InGameCardBase.DroppedCard);
			return;
		case InGameCardBase.ActionTypes.Standard:
			if (!this.ActionIsReversed)
			{
				GameManager.PerformCardOnCardActionStack(this.PossibleAction, GameManager.DraggedStackResult(), this);
				return;
			}
			GameManager.PerformCardOnCardAction(this.PossibleAction, this, InGameCardBase.DroppedCard);
			return;
		case InGameCardBase.ActionTypes.LiquidTransfer:
			if (!this.ActionIsReversed)
			{
				GameManager.PerformCardOnCardAction(this.PossibleAction, InGameCardBase.DroppedCard.ContainedLiquid ? InGameCardBase.DroppedCard.ContainedLiquid : InGameCardBase.DroppedCard, this.ContainedLiquid ? this.ContainedLiquid : this);
				return;
			}
			GameManager.PerformCardOnCardAction(this.PossibleAction, this.ContainedLiquid ? this.ContainedLiquid : this, InGameCardBase.DroppedCard.ContainedLiquid ? InGameCardBase.DroppedCard.ContainedLiquid : InGameCardBase.DroppedCard);
			return;
		case InGameCardBase.ActionTypes.TravelCardStoring:
			GameManager.PerformCardOnCardActionStack(this.PossibleAction, GameManager.DraggedStackResult(), this);
			return;
		default:
			return;
		}
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00011C8C File Offset: 0x0000FE8C
	private bool DropInInventory(InGameCardBase _Card)
	{
		int indexForInventory = this.GetIndexForInventory(0, _Card.CardModel, _Card.ContainedLiquidModel, _Card.CurrentWeight);
		if (indexForInventory != -1)
		{
			if (_Card.CurrentSlot)
			{
				_Card.CurrentSlot.RemoveSpecificCard(_Card, true, true);
			}
			if (_Card.CurrentContainer)
			{
				_Card.CurrentContainer.RemoveCardFromInventory(_Card);
			}
			_Card.CurrentContainer = this;
			_Card.SetSlot(null, true);
			_Card.CurrentSlotInfo = new SlotInfo(SlotsTypes.Inventory, indexForInventory);
			this.AddCardToInventory(_Card, indexForInventory);
			this.Pulse(0f);
			if (InGameCardBase.DroppedCard == _Card)
			{
				InGameCardBase.DroppedCard.OnEndDrag(null);
			}
			if (_Card.CardVisuals)
			{
				_Card.BlocksRaycasts = !_Card.CardVisuals.DontBlockRaycasts;
			}
			else
			{
				_Card.BlocksRaycasts = true;
			}
			MBSingleton<SoundManager>.Instance.PerformCardAppearanceSound(_Card.CardModel.WhenCreatedSounds);
			return true;
		}
		return false;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00011D78 File Offset: 0x0000FF78
	protected bool CanTransferLiquids(InGameCardBase _WithCard)
	{
		if (this.IsPinned || _WithCard.IsPinned)
		{
			return false;
		}
		LiquidContainerStates liquidContainerState = this.LiquidContainerState;
		LiquidContainerStates liquidContainerState2 = _WithCard.LiquidContainerState;
		if (liquidContainerState == LiquidContainerStates.NotContainer || liquidContainerState2 == LiquidContainerStates.NotContainer)
		{
			return false;
		}
		if (liquidContainerState == LiquidContainerStates.Full && liquidContainerState2 == LiquidContainerStates.Full)
		{
			return false;
		}
		if (liquidContainerState == LiquidContainerStates.Empty && liquidContainerState2 == LiquidContainerStates.Empty)
		{
			return false;
		}
		if (liquidContainerState == LiquidContainerStates.Empty && liquidContainerState2 != LiquidContainerStates.Empty)
		{
			return this.CanReceiveLiquid(_WithCard.ContainedLiquid);
		}
		if (liquidContainerState != LiquidContainerStates.Empty && liquidContainerState2 == LiquidContainerStates.Empty)
		{
			return _WithCard.CanReceiveLiquid(this.ContainedLiquid);
		}
		switch (LiquidTransferRules.TransferResult(_WithCard.ContainedLiquidModel, this.ContainedLiquidModel))
		{
		case LiquidTransferInteraction.TransferToReceiving:
			return liquidContainerState != LiquidContainerStates.Full;
		case LiquidTransferInteraction.TransferToGiven:
			return liquidContainerState2 != LiquidContainerStates.Full;
		case LiquidTransferInteraction.UseDefaultTransferRules:
			return true;
		case LiquidTransferInteraction.DontTransfer:
			return false;
		default:
			return false;
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00011E2C File Offset: 0x0001002C
	protected virtual void GetPossibleAction(InGameCardBase _WithCard)
	{
		InGameCardBase.<>c__DisplayClass378_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		if (!this)
		{
			this.UnregisterToEvents();
			return;
		}
		if (!base.gameObject)
		{
			this.UnregisterToEvents();
			return;
		}
		if (!this.Initialized || this.Destroyed)
		{
			return;
		}
		this.SetCollider(false);
		this.CurrentMissingRequirements = null;
		this.PossibleAction = null;
		this.ImpossibleAction = null;
		this.TravelToData = null;
		this.HasAction = false;
		this.BlocksRaycasts = false;
		this.DragStackCompatible = false;
		base.CancelTooltip();
		if (!this.IsPinned)
		{
			this.SetGraphicState(CardGraphics.CardGraphicsStates.GreyedOut, null);
		}
		if (this.CurrentSlot && this.CurrentSlot.SlotType == SlotsTypes.Exploration)
		{
			return;
		}
		if (this.CurrentContainer && (this.CurrentContainer.CardModel.CardType == CardTypes.Blueprint || this.CurrentContainer.CardModel.CardType == CardTypes.EnvImprovement || this.CurrentContainer.CardModel.CardType == CardTypes.EnvDamage))
		{
			return;
		}
		if (!_WithCard || !this.CardModel || _WithCard == this || !base.gameObject.activeInHierarchy || this.IsPinned || this.Destroyed)
		{
			return;
		}
		if (!_WithCard.CardModel || _WithCard.IsPinned)
		{
			return;
		}
		if (_WithCard.CardModel.CardInteractions == null && this.CardModel.CardInteractions == null && !this.IsInventoryCard && !this.ContainedLiquid && !_WithCard.ContainedLiquid && !this.CardModel.CanDragItemsToTravel)
		{
			return;
		}
		if (_WithCard.CardModel.CardInteractions == null)
		{
			_WithCard.CardModel.CardInteractions = new CardOnCardAction[0];
		}
		if (this.CardModel.CardInteractions == null)
		{
			this.CardModel.CardInteractions = new CardOnCardAction[0];
		}
		if (_WithCard.CardModel.CardInteractions.Length == 0 && this.CardModel.CardInteractions.Length == 0 && !this.IsInventoryCard && !this.ContainedLiquid && !_WithCard.ContainedLiquid && !this.CardModel.CanDragItemsToTravel)
		{
			return;
		}
		CS$<>8__locals1.missingStats = new List<MissingStatInfo>();
		CS$<>8__locals1.missingCards = new List<CardData>();
		CS$<>8__locals1.missingTags = new List<CardTag>();
		CS$<>8__locals1.missingDurabilities = "";
		CS$<>8__locals1.blockingStatus = "";
		CS$<>8__locals1.stats = false;
		CS$<>8__locals1.demoOK = true;
		CS$<>8__locals1.board = false;
		CS$<>8__locals1.durabilities = false;
		CS$<>8__locals1.noBlockingStatus = false;
		CS$<>8__locals1.effect = false;
		CS$<>8__locals1.time = false;
		CS$<>8__locals1.actionPlaying = GameManager.PerformingAction;
		this.ActionType = InGameCardBase.ActionTypes.LiquidDouble;
		while (this.PossibleAction == null)
		{
			switch (this.ActionType)
			{
			case InGameCardBase.ActionTypes.LiquidDouble:
				if (GameManager.DraggedCard.ContainedLiquid && this.ContainedLiquid)
				{
					this.<GetPossibleAction>g__CheckPossibleActions|378_0(GameManager.DraggedCard.ContainedLiquid, this.ContainedLiquid, false, ref CS$<>8__locals1);
					if (this.PossibleAction == null)
					{
						this.<GetPossibleAction>g__CheckPossibleActions|378_0(this.ContainedLiquid, GameManager.DraggedCard.ContainedLiquid, true, ref CS$<>8__locals1);
					}
				}
				break;
			case InGameCardBase.ActionTypes.GivenLiquidStandard:
				if (GameManager.DraggedCard.ContainedLiquid)
				{
					this.<GetPossibleAction>g__CheckPossibleActions|378_0(GameManager.DraggedCard.ContainedLiquid, this, false, ref CS$<>8__locals1);
					if (this.PossibleAction == null)
					{
						this.<GetPossibleAction>g__CheckPossibleActions|378_0(this, GameManager.DraggedCard.ContainedLiquid, true, ref CS$<>8__locals1);
					}
				}
				break;
			case InGameCardBase.ActionTypes.ReceivingLiquidStandard:
				if (this.ContainedLiquid)
				{
					this.<GetPossibleAction>g__CheckPossibleActions|378_0(GameManager.DraggedCard, this.ContainedLiquid, false, ref CS$<>8__locals1);
					if (this.PossibleAction == null)
					{
						this.<GetPossibleAction>g__CheckPossibleActions|378_0(this.ContainedLiquid, GameManager.DraggedCard, true, ref CS$<>8__locals1);
					}
				}
				break;
			case InGameCardBase.ActionTypes.Standard:
				this.<GetPossibleAction>g__CheckPossibleActions|378_0(GameManager.DraggedCard, this, false, ref CS$<>8__locals1);
				if (this.PossibleAction == null)
				{
					this.<GetPossibleAction>g__CheckPossibleActions|378_0(this, GameManager.DraggedCard, true, ref CS$<>8__locals1);
				}
				break;
			case InGameCardBase.ActionTypes.LiquidTransfer:
				if (this.CanTransferLiquids(GameManager.DraggedCard))
				{
					if (!CS$<>8__locals1.actionPlaying)
					{
						this.PossibleAction = CardData.GenerateLiquidTransferAction(this, _WithCard, out this.ActionIsReversed);
					}
					else
					{
						this.ImpossibleAction = CardData.GenerateLiquidTransferAction(this, _WithCard, out this.ActionIsReversed);
					}
				}
				break;
			case InGameCardBase.ActionTypes.TravelCardStoring:
			{
				string text;
				if (!this.CanTransferToTravelPlace(GameManager.DraggedCard, out text))
				{
					if (!string.IsNullOrEmpty(text))
					{
						this.ImpossibleAction = CardData.GenerateMoveToLocationAction(this, GameManager.DraggedCard, string.Format("\n{0}", text));
					}
				}
				else if (!CS$<>8__locals1.actionPlaying)
				{
					this.PossibleAction = CardData.GenerateMoveToLocationAction(this, GameManager.DraggedCard, "");
				}
				else
				{
					this.ImpossibleAction = CardData.GenerateMoveToLocationAction(this, GameManager.DraggedCard, "");
				}
				break;
			}
			}
			if (this.PossibleAction == null)
			{
				if (this.ActionType == InGameCardBase.ActionTypes.TravelCardStoring)
				{
					break;
				}
				this.ActionType++;
			}
		}
		if ((this.PossibleAction != null || this.ImpossibleAction != null) && this.ActionType == InGameCardBase.ActionTypes.TravelCardStoring && this.CardVisuals)
		{
			this.CardVisuals.UpdateInventoryInfo();
		}
		if (this.PossibleAction != null)
		{
			this.PossibleAction.CollectActionModifiers(null, null);
			base.SetTooltip(this.PossibleAction.ActionName, this.PossibleAction.TooltipDescription(true, this.PossibleAction.IsNotCancelledByDemo, -1f, CS$<>8__locals1.actionPlaying, "", "", null, null, null), LocalizedString.HoldForAction, 0);
			this.SetGraphicState(CardGraphics.CardGraphicsStates.Highlighted, null);
			this.DragStackCompatible = (this.PossibleAction.StackCompatible && !this.ActionIsReversed);
		}
		else if (this.ImpossibleAction != null)
		{
			this.ImpossibleAction.CollectActionModifiers(null, null);
			if (!CS$<>8__locals1.time)
			{
				base.SetTooltip(this.ImpossibleAction.ActionName, this.ImpossibleAction.TooltipDescription(true, this.ImpossibleAction.IsNotCancelledByDemo, -1f, CS$<>8__locals1.actionPlaying, "", "", null, null, null), LocalizedString.HoldForAction, 0);
				this.UpdateMissingRequirements(null, null, null, CS$<>8__locals1.actionPlaying, !CS$<>8__locals1.demoOK);
				this.SetGraphicState(CardGraphics.CardGraphicsStates.MissingRequirements, null);
			}
			else if ((!CS$<>8__locals1.stats && CS$<>8__locals1.missingStats.Count > 0) || (((!CS$<>8__locals1.durabilities && !string.IsNullOrEmpty(CS$<>8__locals1.missingDurabilities)) || (!CS$<>8__locals1.noBlockingStatus && !string.IsNullOrEmpty(CS$<>8__locals1.blockingStatus))) | CS$<>8__locals1.actionPlaying) || !CS$<>8__locals1.demoOK)
			{
				base.SetTooltip(this.ImpossibleAction.ActionName, this.ImpossibleAction.TooltipDescription(true, this.ImpossibleAction.IsNotCancelledByDemo, -1f, CS$<>8__locals1.actionPlaying, CS$<>8__locals1.missingDurabilities, CS$<>8__locals1.blockingStatus, CS$<>8__locals1.missingStats.ToArray(), null, null), LocalizedString.HoldForAction, 0);
				this.UpdateMissingRequirements((CS$<>8__locals1.missingStats.Count > 0) ? CS$<>8__locals1.missingStats[0] : null, CS$<>8__locals1.missingDurabilities, CS$<>8__locals1.blockingStatus, CS$<>8__locals1.actionPlaying, !CS$<>8__locals1.demoOK);
				this.SetGraphicState(CardGraphics.CardGraphicsStates.MissingRequirements, null);
			}
		}
		else if (this.IsInventoryCard && this.GraphicsM.InspectedCard != this && this.CardModel.CardType != CardTypes.Explorable && this.CanReceiveInInventoryInstance(GameManager.DraggedCard) && !this.CardModel.InventoryIsHidden)
		{
			this.DragStackCompatible = true;
			if (this.CardModel.CardType != CardTypes.Blueprint && this.CardModel.CardType != CardTypes.EnvImprovement && this.CardModel.CardType != CardTypes.EnvDamage)
			{
				if (!this.InventoryFull)
				{
					base.SetTooltip(LocalizedString.StoreInInventory + " " + this.CardName(true), "", LocalizedString.HoldForAction, 0);
				}
				else
				{
					base.SetTooltip(this.CardName(true) + " " + LocalizedString.InventoryFull, "", LocalizedString.HoldForAction, 0);
				}
			}
			else if (this.InventoryCount(GameManager.DraggedCard.CardModel) < this.MaxInventoryContent(GameManager.DraggedCard, false))
			{
				base.SetTooltip(LocalizedString.AddToBlueprint(GameManager.DraggedCard, this.CardModel), "", LocalizedString.HoldForAction, 0);
			}
			else
			{
				base.SetTooltip(LocalizedString.BlueprintFull(GameManager.DraggedCard.CardModel, this.CardModel), "", LocalizedString.HoldForAction, 0);
			}
			this.SetGraphicState(CardGraphics.CardGraphicsStates.InventoryDisplay, GameManager.DraggedCard);
		}
		this.HasAction = !string.IsNullOrEmpty(base.Title);
		if (this.CardVisuals)
		{
			this.BlocksRaycasts = (this.HasAction && !this.CardVisuals.DontBlockRaycasts);
			return;
		}
		this.BlocksRaycasts = this.HasAction;
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00012728 File Offset: 0x00010928
	protected virtual void CancelPossibleAction(InGameCardBase _Card)
	{
		if (!this)
		{
			return;
		}
		if (!base.gameObject)
		{
			return;
		}
		this.CurrentMissingRequirements = null;
		if (this.TravelToData != null)
		{
			this.TravelToData = null;
			if (this.CardVisuals)
			{
				this.CardVisuals.UpdateInventoryInfo();
			}
		}
		if (MobilePlatformDetection.IsMobilePlatform)
		{
			this.SetCollider(true);
		}
		if (!this.IsPinned)
		{
			this.SetGraphicState(CardGraphics.CardGraphicsStates.Normal, null);
		}
		base.CancelTooltip();
		this.HasAction = false;
		if (this.CardVisuals)
		{
			this.BlocksRaycasts = !this.CardVisuals.DontBlockRaycasts;
		}
		else
		{
			this.BlocksRaycasts = true;
		}
		if (this.PossibleAction == null)
		{
			return;
		}
		this.PossibleAction = null;
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x000127DF File Offset: 0x000109DF
	public virtual IEnumerator ModifyDurability(DurabilitiesTypes _Type, float _Amt, bool _Feedback)
	{
		if (this.IsPinned)
		{
			yield break;
		}
		float num;
		float num2;
		DurabilityStat durabilityStat;
		float amt;
		switch (_Type)
		{
		case DurabilitiesTypes.Spoilage:
			if (!this.CardModel.SpoilageTime)
			{
				yield break;
			}
			num = this.CurrentSpoilage;
			this.CurrentSpoilage += _Amt;
			num2 = this.CurrentSpoilage;
			durabilityStat = this.CardModel.SpoilageTime;
			this.CurrentSpoilage = Mathf.Clamp(this.CurrentSpoilage, 0f, this.CardModel.SpoilageTime.Max);
			amt = this.CurrentSpoilage - num;
			break;
		case DurabilitiesTypes.Usage:
			if (!this.CardModel.UsageDurability)
			{
				yield break;
			}
			num = this.CurrentUsageDurability;
			this.CurrentUsageDurability += _Amt;
			num2 = this.CurrentUsageDurability;
			durabilityStat = this.CardModel.UsageDurability;
			this.CurrentUsageDurability = Mathf.Clamp(this.CurrentUsageDurability, 0f, this.CardModel.UsageDurability.Max);
			amt = this.CurrentUsageDurability - num;
			break;
		case DurabilitiesTypes.Fuel:
			if (!this.CardModel.FuelCapacity)
			{
				yield break;
			}
			num = this.CurrentFuel;
			this.CurrentFuel += _Amt;
			num2 = this.CurrentFuel;
			durabilityStat = this.CardModel.FuelCapacity;
			this.CurrentFuel = Mathf.Clamp(this.CurrentFuel, 0f, this.CardModel.FuelCapacity.Max);
			amt = this.CurrentFuel - num;
			break;
		case DurabilitiesTypes.Progress:
			if (!this.CardModel.Progress)
			{
				yield break;
			}
			num = this.CurrentProgress;
			this.CurrentProgress += _Amt;
			num2 = this.CurrentProgress;
			durabilityStat = this.CardModel.Progress;
			this.CurrentProgress = Mathf.Clamp(this.CurrentProgress, 0f, this.CardModel.Progress.Max);
			amt = this.CurrentProgress - num;
			break;
		case DurabilitiesTypes.Liquid:
			if (!this.IsLiquid)
			{
				yield break;
			}
			num = this.CurrentLiquidQuantity;
			this.CurrentLiquidQuantity += _Amt;
			num2 = this.CurrentLiquidQuantity;
			durabilityStat = null;
			this.CurrentLiquidQuantity = ((this.CurrentMaxLiquidQuantity > 0f) ? Mathf.Clamp(this.CurrentLiquidQuantity, 0f, this.CurrentMaxLiquidQuantity) : Mathf.Min(this.CurrentLiquidQuantity, 0f));
			amt = this.CurrentLiquidQuantity - num;
			this.WeightHasChanged();
			break;
		case DurabilitiesTypes.Special1:
			if (!this.CardModel.SpecialDurability1)
			{
				yield break;
			}
			num = this.CurrentSpecial1;
			this.CurrentSpecial1 += _Amt;
			num2 = this.CurrentSpecial1;
			durabilityStat = this.CardModel.SpecialDurability1;
			this.CurrentSpecial1 = Mathf.Clamp(this.CurrentSpecial1, 0f, this.CardModel.SpecialDurability1.Max);
			amt = this.CurrentSpecial1 - num;
			break;
		case DurabilitiesTypes.Special2:
			if (!this.CardModel.SpecialDurability2)
			{
				yield break;
			}
			num = this.CurrentSpecial2;
			this.CurrentSpecial2 += _Amt;
			num2 = this.CurrentSpecial2;
			durabilityStat = this.CardModel.SpecialDurability2;
			this.CurrentSpecial2 = Mathf.Clamp(this.CurrentSpecial2, 0f, this.CardModel.SpecialDurability2.Max);
			amt = this.CurrentSpecial2 - num;
			break;
		case DurabilitiesTypes.Special3:
			if (!this.CardModel.SpecialDurability3)
			{
				yield break;
			}
			num = this.CurrentSpecial3;
			this.CurrentSpecial3 += _Amt;
			num2 = this.CurrentSpecial3;
			durabilityStat = this.CardModel.SpecialDurability3;
			this.CurrentSpecial3 = Mathf.Clamp(this.CurrentSpecial3, 0f, this.CardModel.SpecialDurability3.Max);
			amt = this.CurrentSpecial3 - num;
			break;
		case DurabilitiesTypes.Special4:
			if (!this.CardModel.SpecialDurability4)
			{
				yield break;
			}
			num = this.CurrentSpecial4;
			this.CurrentSpecial4 += _Amt;
			num2 = this.CurrentSpecial4;
			durabilityStat = this.CardModel.SpecialDurability4;
			this.CurrentSpecial4 = Mathf.Clamp(this.CurrentSpecial4, 0f, this.CardModel.SpecialDurability4.Max);
			amt = this.CurrentSpecial4 - num;
			break;
		default:
			yield break;
		}
		if (this.CardVisuals)
		{
			if (_Feedback && durabilityStat.Show(this.ContainedLiquid != null, num2))
			{
				this.CardVisuals.ModifyDurability(_Type, amt, false);
			}
			this.CardVisuals.RefreshDurabilities();
		}
		else if (this.IsLiquid && this.CurrentContainer && this.CurrentContainer.CardVisuals)
		{
			if (_Feedback)
			{
				if (durabilityStat == null)
				{
					this.CurrentContainer.CardVisuals.ModifyDurability(_Type, amt, true);
				}
				else if (durabilityStat.Show(false, num2))
				{
					this.CurrentContainer.CardVisuals.ModifyDurability(_Type, amt, true);
				}
			}
			this.CurrentContainer.CardVisuals.RefreshDurabilities();
		}
		if (durabilityStat != null)
		{
			if (durabilityStat.HasActionOnFull && num2 >= durabilityStat.Max - 0.0001f && (num < durabilityStat.Max || durabilityStat.OnFull.DestroysReceivingCard))
			{
				switch (_Type)
				{
				case DurabilitiesTypes.Spoilage:
					this.SpoilFull = true;
					break;
				case DurabilitiesTypes.Usage:
					this.UsageFull = true;
					break;
				case DurabilitiesTypes.Fuel:
					this.FuelFull = true;
					break;
				case DurabilitiesTypes.Progress:
					this.ProgressFull = true;
					break;
				case DurabilitiesTypes.Special1:
					this.Special1Full = true;
					break;
				case DurabilitiesTypes.Special2:
					this.Special2Full = true;
					break;
				case DurabilitiesTypes.Special3:
					this.Special3Full = true;
					break;
				case DurabilitiesTypes.Special4:
					this.Special4Full = true;
					break;
				}
			}
			if (durabilityStat.HasActionOnZero && num2 <= 0.0001f && (num > 0f || durabilityStat.OnZero.DestroysReceivingCard))
			{
				switch (_Type)
				{
				case DurabilitiesTypes.Spoilage:
					this.SpoilEmpty = true;
					break;
				case DurabilitiesTypes.Usage:
					this.UsageEmpty = true;
					break;
				case DurabilitiesTypes.Fuel:
					this.FuelEmpty = true;
					break;
				case DurabilitiesTypes.Progress:
					this.ProgressEmpty = true;
					break;
				case DurabilitiesTypes.Special1:
					this.Special1Empty = true;
					break;
				case DurabilitiesTypes.Special2:
					this.Special2Empty = true;
					break;
				case DurabilitiesTypes.Special3:
					this.Special3Empty = true;
					break;
				case DurabilitiesTypes.Special4:
					this.Special4Empty = true;
					break;
				}
			}
		}
		else if (num2 <= 0.0001f && _Type == DurabilitiesTypes.Liquid)
		{
			this.LiquidEmpty = true;
		}
		yield break;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00012803 File Offset: 0x00010A03
	public IEnumerator PerformDurabilitiesActions(bool _Feedback)
	{
		if (this.Destroyed)
		{
			this.SpoilFull = false;
			this.SpoilEmpty = false;
			this.UsageFull = false;
			this.UsageEmpty = false;
			this.FuelFull = false;
			this.FuelEmpty = false;
			this.ProgressFull = false;
			this.ProgressEmpty = false;
			this.Special1Full = false;
			this.Special1Empty = false;
			this.Special2Full = false;
			this.Special2Empty = false;
			this.Special3Full = false;
			this.Special3Empty = false;
			this.Special4Full = false;
			this.Special4Empty = false;
			yield break;
		}
		if (this.SpoilFull)
		{
			this.SpoilFull = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpoilageTime, DurabilitiesTypes.Spoilage, true, _Feedback));
		}
		else if (this.SpoilEmpty)
		{
			this.SpoilEmpty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpoilageTime, DurabilitiesTypes.Spoilage, false, _Feedback));
		}
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.UsageFull)
		{
			this.UsageFull = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.UsageDurability, DurabilitiesTypes.Usage, true, _Feedback));
		}
		else if (this.UsageEmpty)
		{
			this.UsageEmpty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.UsageDurability, DurabilitiesTypes.Usage, false, _Feedback));
		}
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.FuelFull)
		{
			this.FuelFull = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.FuelCapacity, DurabilitiesTypes.Fuel, true, _Feedback));
		}
		else if (this.FuelEmpty)
		{
			this.FuelEmpty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.FuelCapacity, DurabilitiesTypes.Fuel, false, _Feedback));
		}
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.ProgressFull)
		{
			this.ProgressFull = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.Progress, DurabilitiesTypes.Progress, true, _Feedback));
		}
		else if (this.ProgressEmpty)
		{
			this.ProgressEmpty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.Progress, DurabilitiesTypes.Progress, false, _Feedback));
		}
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.Special1Full)
		{
			this.Special1Full = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability1, DurabilitiesTypes.Special1, true, _Feedback));
		}
		else if (this.Special1Empty)
		{
			this.Special1Empty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability1, DurabilitiesTypes.Special1, false, _Feedback));
		}
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.Special2Full)
		{
			this.Special2Full = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability2, DurabilitiesTypes.Special2, true, _Feedback));
		}
		else if (this.Special2Empty)
		{
			this.Special2Empty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability2, DurabilitiesTypes.Special2, false, _Feedback));
		}
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.Special3Full)
		{
			this.Special3Full = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability3, DurabilitiesTypes.Special3, true, _Feedback));
		}
		else if (this.Special3Empty)
		{
			this.Special3Empty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability3, DurabilitiesTypes.Special3, false, _Feedback));
		}
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.Special4Full)
		{
			this.Special4Full = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability4, DurabilitiesTypes.Special4, true, _Feedback));
		}
		else if (this.Special4Empty)
		{
			this.Special4Empty = false;
			yield return this.GM.StartCoroutine(this.DoActionForDurability(this.CardModel.SpecialDurability4, DurabilitiesTypes.Special4, false, _Feedback));
		}
		yield break;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00012819 File Offset: 0x00010A19
	private IEnumerator DoActionForDurability(DurabilityStat _Stat, DurabilitiesTypes _Type, bool _Full, bool _Feedback)
	{
		string text;
		if (_Full && _Stat.HasActionOnFull && _Stat.OnFull.IsNotCancelledByDemo && _Stat.OnFull.StatsAreCorrect(null, true) && _Stat.OnFull.CardsAndTagsAreCorrect(this, null, null) && _Stat.OnFull.DurabilitiesAreCorrect(this, out text))
		{
			if (!_Stat.OnFull.HasActionSounds && _Stat.OnFullNotification != CardNotifications.DontNotify)
			{
				MBSingleton<SoundManager>.Instance.PerformSingleSound(this.GetDurabilityNotificationSound(_Type, true), true, true);
			}
			_Stat.OnFull.CollectActionModifiers(this, null);
			if (!this.InBackground)
			{
				string durabilityActionNotification = this.GetDurabilityActionNotification(_Type, true);
				if (!string.IsNullOrEmpty(durabilityActionNotification))
				{
					this.GraphicsM.PlayCardNotification(this, durabilityActionNotification);
				}
				if (_Stat.ShowPopupOnFull && _Stat.OnFull.TotalDaytimeCost <= 0)
				{
					this.GM.QueuedCardActions.Add(new InGameActionRef
					{
						OnCard = this,
						Action = _Stat.OnFull,
						Message = durabilityActionNotification
					});
				}
			}
			yield return GameManager.PerformAction(_Stat.OnFull, this, !_Feedback);
		}
		string text2;
		if (!_Full && _Stat.HasActionOnZero && _Stat.OnZero.IsNotCancelledByDemo && _Stat.OnZero.StatsAreCorrect(null, true) && _Stat.OnZero.CardsAndTagsAreCorrect(this, null, null) && _Stat.OnZero.DurabilitiesAreCorrect(this, out text2))
		{
			if (!_Stat.OnZero.HasActionSounds && _Stat.OnZeroNotification != CardNotifications.DontNotify)
			{
				MBSingleton<SoundManager>.Instance.PerformSingleSound(this.GetDurabilityNotificationSound(_Type, false), true, true);
			}
			_Stat.OnZero.CollectActionModifiers(this, null);
			if (!this.InBackground)
			{
				string durabilityActionNotification2 = this.GetDurabilityActionNotification(_Type, false);
				if (!string.IsNullOrEmpty(durabilityActionNotification2))
				{
					this.GraphicsM.PlayCardNotification(this, durabilityActionNotification2);
				}
				if (_Stat.ShowPopupOnZero && _Stat.OnZero.TotalDaytimeCost <= 0)
				{
					this.GM.QueuedCardActions.Add(new InGameActionRef
					{
						OnCard = this,
						Action = _Stat.OnZero,
						Message = durabilityActionNotification2
					});
				}
			}
			yield return GameManager.PerformAction(_Stat.OnZero, this, !_Feedback);
		}
		yield break;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00012848 File Offset: 0x00010A48
	private AudioClip GetDurabilityNotificationSound(DurabilitiesTypes _Type, bool _OnFull)
	{
		switch (_Type)
		{
		case DurabilitiesTypes.Spoilage:
			if (!_OnFull)
			{
				return MBSingleton<SoundManager>.Instance.DefaultSpoilageOnZero;
			}
			return MBSingleton<SoundManager>.Instance.DefaultSpoilageFull;
		case DurabilitiesTypes.Usage:
			if (!_OnFull)
			{
				return MBSingleton<SoundManager>.Instance.DefaultUsageOnZero;
			}
			return MBSingleton<SoundManager>.Instance.DefaultUsageFull;
		case DurabilitiesTypes.Fuel:
			if (!_OnFull)
			{
				return MBSingleton<SoundManager>.Instance.DefaultFuelOnZero;
			}
			return MBSingleton<SoundManager>.Instance.DefaultFuelFull;
		case DurabilitiesTypes.Progress:
			if (!_OnFull)
			{
				return MBSingleton<SoundManager>.Instance.DefaultProgressOnZero;
			}
			return MBSingleton<SoundManager>.Instance.DefaultProgressFull;
		case DurabilitiesTypes.Special1:
		case DurabilitiesTypes.Special2:
		case DurabilitiesTypes.Special3:
		case DurabilitiesTypes.Special4:
			if (!_OnFull)
			{
				return MBSingleton<SoundManager>.Instance.DefaultSpecialDurabilityOnZero;
			}
			return MBSingleton<SoundManager>.Instance.DefaultSpecialDurabilityFull;
		}
		return null;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00012900 File Offset: 0x00010B00
	private string GetDurabilityActionNotification(DurabilitiesTypes _Type, bool _OnFull)
	{
		DurabilityStat durabilityStat = null;
		string result = "";
		switch (_Type)
		{
		case DurabilitiesTypes.Spoilage:
			durabilityStat = this.CardModel.SpoilageTime;
			result = (_OnFull ? LocalizedString.DefaultSpoilageFull(this) : LocalizedString.DefaultSpoilageOnZero(this));
			break;
		case DurabilitiesTypes.Usage:
			durabilityStat = this.CardModel.UsageDurability;
			result = (_OnFull ? LocalizedString.DefaultUsageFull(this) : LocalizedString.DefaultUsageOnZero(this));
			break;
		case DurabilitiesTypes.Fuel:
			durabilityStat = this.CardModel.FuelCapacity;
			result = (_OnFull ? LocalizedString.DefaultFuelFull(this) : LocalizedString.DefaultFuelOnZero(this));
			break;
		case DurabilitiesTypes.Progress:
			durabilityStat = this.CardModel.Progress;
			result = (_OnFull ? LocalizedString.DefaultProgressFull(this) : LocalizedString.DefaultProgressOnZero(this));
			break;
		case DurabilitiesTypes.Special1:
			durabilityStat = this.CardModel.SpecialDurability1;
			result = "";
			break;
		case DurabilitiesTypes.Special2:
			durabilityStat = this.CardModel.SpecialDurability2;
			result = "";
			break;
		case DurabilitiesTypes.Special3:
			durabilityStat = this.CardModel.SpecialDurability3;
			result = "";
			break;
		case DurabilitiesTypes.Special4:
			durabilityStat = this.CardModel.SpecialDurability4;
			result = "";
			break;
		}
		CardNotifications cardNotifications = _OnFull ? durabilityStat.OnFullNotification : durabilityStat.OnZeroNotification;
		if (cardNotifications == CardNotifications.UseDefault)
		{
			return result;
		}
		if (cardNotifications != CardNotifications.UseActionDesc)
		{
			return "";
		}
		return _OnFull ? durabilityStat.OnFull.ActionDescription : durabilityStat.OnZero.ActionDescription;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00012A5B File Offset: 0x00010C5B
	public virtual IEnumerator DestroyCard(bool _NoDelay)
	{
		if (this.Destroyed)
		{
			yield break;
		}
		if (this.CardVisuals)
		{
			if (!this.GM.PoolCards)
			{
				this.CardVisuals.OnLogicDestroyed();
			}
			else
			{
				this.CardVisuals.OnLogicReset();
			}
		}
		if (this.CurrentSlot)
		{
			this.CurrentSlot.RemoveSpecificCard(this, !_NoDelay, true);
		}
		if (MBSingleton<ExplorationPopup>.Instance.HoveredCard == this)
		{
			MBSingleton<ExplorationPopup>.Instance.HoveredCard = null;
		}
		List<CoroutineController> waitFor = new List<CoroutineController>();
		if (this.CurrentStatModifiers.Count != 0)
		{
			CoroutineController item;
			this.GM.StartCoroutineEx(this.CancelStatModifiers(), out item);
			waitFor.Add(item);
		}
		for (int i = 0; i < this.CardModel.PassiveEffects.Length; i++)
		{
			CoroutineController item;
			this.GM.StartCoroutineEx(this.CancelPassiveEffect(this.CardModel.PassiveEffects[i]), out item);
			waitFor.Add(item);
		}
		if (this.ExternalPassiveEffects != null)
		{
			for (int j = 0; j < this.ExternalPassiveEffects.Count; j++)
			{
				CoroutineController item;
				this.GM.StartCoroutineEx(this.CancelPassiveEffect(this.ExternalPassiveEffects[j]), out item);
				waitFor.Add(item);
			}
		}
		while (CoroutineController.WaitForControllerList(waitFor))
		{
			yield return null;
		}
		if (this.CurrentContainer)
		{
			if (!this.IsLiquid)
			{
				this.CurrentContainer.RemoveCardFromInventory(this);
			}
			else if (this.CurrentContainer.ContainedLiquid == this)
			{
				this.CurrentContainer.SetContainedLiquid(null, true, false);
				if (this.CardModel.IndependentFromEnv && this.UpdatedInBackground && !this.CurrentContainer.CardModel.IndependentFromEnv)
				{
					this.CurrentContainer.WasIndependentFromEnv = true;
				}
			}
		}
		this.IsPerformingAction = false;
		this.LastAlivePosition = this.ValidPosition;
		this.SetDestroyedOn();
		this.TimeToDeactivation = UnityEngine.Random.Range(2.5f, 5f);
		this.UnregisterToEvents();
		yield return null;
		if (this.GM)
		{
			if (!this.GM.PoolCards)
			{
				UnityEngine.Object.Destroy(base.gameObject, 5f);
			}
			else if (_NoDelay)
			{
				yield return this.GM.StartCoroutine(this.ResetCard(0f));
			}
			else
			{
				this.GM.StartCoroutine(this.ResetCard(5f));
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject, 5f);
		}
		yield break;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00012A71 File Offset: 0x00010C71
	protected virtual void SetDestroyedOn()
	{
		this.Destroyed = true;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00012A7A File Offset: 0x00010C7A
	protected virtual IEnumerator ResetCard(float _Delay)
	{
		if (_Delay > 0f)
		{
			yield return new WaitForSeconds(_Delay);
		}
		else
		{
			yield return null;
		}
		if (this.ActionPulse != null)
		{
			this.ActionPulse.Complete(true);
		}
		this.CurrentSpoilage = 0f;
		this.CurrentUsageDurability = 0f;
		this.CurrentFuel = 0f;
		this.CurrentProgress = 0f;
		this.CurrentLiquidQuantity = 0f;
		this.CurrentSpecial1 = 0f;
		this.CurrentSpecial2 = 0f;
		this.CurrentSpecial3 = 0f;
		this.CurrentSpecial4 = 0f;
		this.PulseAfterReachingSlot = false;
		this.CreatedOnTick = 0;
		this.ExplorationData = null;
		this.BlueprintData = null;
		this.Initialized = false;
		this.UpdatedInBackground = false;
		this.MarkedAsBlueprintIngredient = false;
		this.TravelCardIndex = 0;
		this.PrevEnvTravelIndex = 0;
		this.SpoilFull = false;
		this.SpoilEmpty = false;
		this.UsageFull = false;
		this.UsageEmpty = false;
		this.FuelFull = false;
		this.FuelEmpty = false;
		this.ProgressFull = false;
		this.ProgressEmpty = false;
		this.LiquidEmpty = false;
		this.Special1Full = false;
		this.Special1Empty = false;
		this.Special2Full = false;
		this.Special2Empty = false;
		this.Special3Full = false;
		this.Special3Empty = false;
		this.Special4Full = false;
		this.Special4Empty = false;
		this.IgnoreTickDurabilityChanges = false;
		this.ContainedLiquid = null;
		this.FutureLiquidContained = null;
		this.StayInSlotWhenLiquidChanges = false;
		this.DismantleActions = null;
		this.BaseSpoilageRate = 0f;
		this.BaseUsageRate = 0f;
		this.BaseFuelRate = 0f;
		this.BaseConsumableRate = 0f;
		this.BaseEvaporationRate = 0f;
		this.BaseSpecial1Rate = 0f;
		this.BaseSpecial2Rate = 0f;
		this.BaseSpecial3Rate = 0f;
		this.BaseSpecial4Rate = 0f;
		this.Environment = null;
		this.PrevEnvironment = null;
		this.CanCarryToNewEnv = false;
		this.InBackground = false;
		this.LastAlivePosition = Vector3.zero;
		this.PossibleAction = null;
		this.ImpossibleAction = null;
		this.ActionIsReversed = false;
		this.ActionHoldTimer = 0f;
		this.CurrentSlot = null;
		this.CurrentSlotInfo = null;
		this.CurrentParentObject = null;
		this.TooltipTitle = null;
		this.CurrentContainer = null;
		this.CardsInInventory = null;
		this.CookingCards.Clear();
		if (this.ExternalPassiveEffects != null)
		{
			this.ExternalPassiveEffects.Clear();
		}
		this.CurrentStatModifiers.Clear();
		this.DroppedCollections.Clear();
		this.PassiveEffects.Clear();
		if (this.CookingResultsList != null)
		{
			this.CookingResultsList.Clear();
		}
		this.StatTriggeredActions = null;
		this.InspectionDuplicate = null;
		this.DuplicateParentObject = null;
		if (this.ActionPulse != null)
		{
			if (this.ActionPulse.IsActive())
			{
				this.ActionPulse.Kill(true);
			}
			this.ActionPulse = null;
		}
		this.Clicks = 0;
		if (this.WaitBeforeInspection != null)
		{
			base.StopCoroutine(this.WaitBeforeInspection);
			this.WaitBeforeInspection = null;
		}
		if (this.TimeAnimation != null)
		{
			if (this.TimeAnimation.IsActive())
			{
				this.TimeAnimation.Kill(true);
			}
			this.TimeAnimation = null;
		}
		this.TravelToData = null;
		this.HasAction = false;
		this.IsPinned = false;
		this.UnregisterToEvents();
		if (!base.gameObject.activeInHierarchy && base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
		this.TutorialHighlight = TutorialHighlightState.NotHighlighted;
		if (this.CardVisuals)
		{
			CardVisualsManager.FreeCard(this.CardVisuals);
			this.CardVisuals = null;
		}
		CardPooling.FreeCard(this);
		this.CardModel = null;
		yield break;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00012A90 File Offset: 0x00010C90
	public virtual void UpdateEnvironment(CardData _NewEnvironment, CardData _PrevEnv, int _TravellingIndex)
	{
		this.GetManagers();
		bool flag = this.WillBeInBG(_NewEnvironment, _PrevEnv, _TravellingIndex, false);
		if (!flag)
		{
			this.Environment = _NewEnvironment;
			if (this.IndependentFromEnv)
			{
				if (this.Environment.InstancedEnvironment)
				{
					this.PrevEnvironment = _PrevEnv;
					this.PrevEnvTravelIndex = _TravellingIndex;
				}
				else
				{
					this.PrevEnvironment = null;
					this.PrevEnvTravelIndex = 0;
				}
			}
			else
			{
				this.PrevEnvironment = null;
				this.PrevEnvTravelIndex = 0;
			}
		}
		this.UpdatedInBackground = (flag && this.IndependentFromEnv);
		if (flag == this.InBackground)
		{
			return;
		}
		this.InBackground = flag;
		if (this.WasIndependentFromEnv)
		{
			this.WasIndependentFromEnv = false;
		}
		if (this.UpdatedInBackground && this.Initialized)
		{
			if (this.Environment.InstancedEnvironment)
			{
				this.PrevEnvironment = this.GM.PrevEnvironment;
				this.PrevEnvTravelIndex = this.GM.CurrentTravelIndex;
			}
			else
			{
				this.PrevEnvironment = null;
				this.PrevEnvTravelIndex = 0;
			}
		}
		this.UpdateVisibility();
		if (this.CardModel && this.CardModel.HasPassiveEffects)
		{
			this.GM.StartCoroutine(this.UpdatePassiveEffects());
		}
		if (this.InBackground)
		{
			if (this.CurrentSlot)
			{
				this.CurrentSlot.RemoveSpecificCard(this, false, true);
				this.CurrentSlotInfo = this.CurrentSlot.ToInfo();
				this.CurrentSlot = null;
			}
			MBSingleton<SoundManager>.Instance.StopOtherAmbience(this);
			MBSingleton<AmbienceImageEffect>.Instance.StopLightSource(this);
			if (this.GM.AllVisibleCards.Contains(this))
			{
				this.GM.AllVisibleCards.Remove(this);
				return;
			}
		}
		else
		{
			if ((!this.CurrentContainer || MBSingleton<GraphicsManager>.Instance.InspectedCard == this.CurrentContainer) && this.CardModel.CardType != CardTypes.EnvImprovement && this.CardModel.CardType != CardTypes.EnvDamage)
			{
				if (this.CardModel.CardType == CardTypes.Blueprint)
				{
					if (this.CurrentSlotInfo.SlotType == SlotsTypes.Blueprint)
					{
						this.GraphicsM.GetSlotForCard(this.CardModel, this.ContainedLiquidModel, this.CurrentSlotInfo, null, null, 0).AssignCard(this, true);
					}
					else
					{
						this.GraphicsM.GetSlotForCard(this.CardModel, this.ContainedLiquidModel, new SlotInfo(this.CurrentSlotInfo.SlotType, -2), null, null, 0).AssignCard(this, true);
					}
				}
				else if (this.CurrentContainer)
				{
					this.GraphicsM.GetSlotForCard(this.CardModel, this.ContainedLiquidModel, this.CurrentSlotInfo, null, null, 0).AssignCard(this, true);
				}
				else
				{
					this.GraphicsM.GetSlotForCard(this.CardModel, this.ContainedLiquidModel, new SlotInfo(this.GraphicsM.CardToSlotType(this.CardModel.CardType, false), -2), _NewEnvironment, _PrevEnv, _TravellingIndex).AssignCard(this, true);
				}
			}
			base.transform.localPosition = Vector3.zero;
			MBSingleton<SoundManager>.Instance.PlayOtherAmbience(this);
			MBSingleton<AmbienceImageEffect>.Instance.ApplyLightSource(this);
			if (!this.GM.AllVisibleCards.Contains(this) && !this.IsPinned)
			{
				this.GM.AllVisibleCards.Add(this);
			}
			this.CheckForRemoteEffects();
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00012DBD File Offset: 0x00010FBD
	public bool CheckForBackground(CardData _NextEnv, CardData _PrevEnv, int _TravelIndex)
	{
		return this.WillBeInBG(_NextEnv, _PrevEnv, _TravelIndex, true);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00012DCC File Offset: 0x00010FCC
	private bool WillBeInBG(CardData _NextEnv, CardData _PrevEnv, int _TravelIndex, bool _DontCarry)
	{
		InGameCardBase.<>c__DisplayClass390_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1._NextEnv = _NextEnv;
		CS$<>8__locals1._PrevEnv = _PrevEnv;
		CS$<>8__locals1._TravelIndex = _TravelIndex;
		if (!this.CardModel)
		{
			return true;
		}
		if ((CS$<>8__locals1._NextEnv == this.Environment || !this.Environment) && this.<WillBeInBG>g__checkPrevEnv|390_0(ref CS$<>8__locals1))
		{
			return false;
		}
		if (this.CurrentContainer)
		{
			return this.CurrentContainer.WillBeInBG(CS$<>8__locals1._NextEnv, CS$<>8__locals1._PrevEnv, CS$<>8__locals1._TravelIndex, _DontCarry);
		}
		this.GetManagers();
		switch (this.CardModel.CardType)
		{
		case CardTypes.Item:
		case CardTypes.Base:
		case CardTypes.Location:
		case CardTypes.Explorable:
		case CardTypes.EnvImprovement:
		case CardTypes.EnvDamage:
		{
			bool flag = this.CardModel.CardType > CardTypes.Item;
			flag = (!this.CurrentSlot || (flag | this.CurrentSlot.SlotType == SlotsTypes.Base));
			if (this.CanCarryToNewEnv)
			{
				this.CanCarryToNewEnv = false;
				return false;
			}
			return flag && (!this.CardModel.CarriesOverTo.Contains(CS$<>8__locals1._NextEnv) || !(this.GM.CurrentEnvironment == this.Environment));
		}
		case CardTypes.Blueprint:
			return !this.CurrentSlot || this.CurrentSlot.SlotType != SlotsTypes.Blueprint;
		}
		return false;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00012F4C File Offset: 0x0001114C
	public void UpdateVisibility()
	{
		if (this.InBackground)
		{
			base.gameObject.SetActive(false);
			if (this.CardVisuals)
			{
				this.CardVisuals.OnLogicDestroyed();
			}
			return;
		}
		if (!this)
		{
			Debug.LogWarning(this.CardModel);
		}
		if (this.CurrentContainer)
		{
			base.gameObject.SetActive(MBSingleton<GraphicsManager>.Instance.InspectedCard == this.CurrentContainer);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00012FD4 File Offset: 0x000111D4
	public void UseCollection(CardsDropCollection _Collection)
	{
		if (string.IsNullOrEmpty(_Collection.CollectionName))
		{
			return;
		}
		if (!this.DroppedCollections.ContainsKey(_Collection.CollectionName))
		{
			return;
		}
		Vector2Int value = this.DroppedCollections[_Collection.CollectionName];
		int x = value.x;
		value.x = x + 1;
		this.DroppedCollections[_Collection.CollectionName] = value;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00013038 File Offset: 0x00011238
	public bool CanUseCollection(CardsDropCollection _Collection)
	{
		return !this.DroppedCollections.ContainsKey(_Collection.CollectionName) || this.DroppedCollections[_Collection.CollectionName].x < this.DroppedCollections[_Collection.CollectionName].y;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0001308E File Offset: 0x0001128E
	public void BuyBlueprint()
	{
		if (this.CardModel)
		{
			this.GraphicsM.BlueprintModelsPopup.BuyBlueprint(this);
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x000130B0 File Offset: 0x000112B0
	public void OpenBlueprintGuidePage()
	{
		ContentPage pageFor = GuideManager.GetPageFor(this);
		if (pageFor)
		{
			this.GM.OpenGuide(pageFor);
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x000130D8 File Offset: 0x000112D8
	public void OnPointerClick(PointerEventData _Pointer)
	{
		if (this.Destroyed)
		{
			return;
		}
		if (!this.CardModel)
		{
			return;
		}
		if (this.CardModel.CardType == CardTypes.Event)
		{
			return;
		}
		if (this.CurrentSlot && this.CurrentSlot.SlotType == SlotsTypes.Exploration)
		{
			if (_Pointer.button == PointerEventData.InputButton.Left)
			{
				MBSingleton<ExplorationPopup>.Instance.ClickCard(this);
			}
			return;
		}
		bool flag = this.CurrentSlot && this.CardModel && !this.IsPinned && this.GraphicsM.InspectedCard != this;
		if (flag)
		{
			flag &= (this.CardModel.CardType == CardTypes.Item);
		}
		if (this.Clicks == 0 && _Pointer.button == PointerEventData.InputButton.Left)
		{
			this.GraphicsM.InspectCard(this, false);
			return;
		}
		if ((this.Clicks >= 1 || _Pointer.button == PointerEventData.InputButton.Right) && flag)
		{
			if (this.WaitBeforeInspection != null)
			{
				base.StopCoroutine(this.WaitBeforeInspection);
			}
			this.SwapCard();
			this.Clicks = 0;
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x000131E0 File Offset: 0x000113E0
	public void SwapCard()
	{
		SlotInfo slotInfo = null;
		if (this.CurrentSlot.SlotType == SlotsTypes.Inventory || this.CurrentSlot.SlotType == SlotsTypes.Base || this.CurrentSlot.SlotType == SlotsTypes.Equipment)
		{
			if (this.CurrentSlot.SlotType != SlotsTypes.Equipment || !this.CardModel.IsMandatoryEquipment)
			{
				slotInfo = new SlotInfo(SlotsTypes.Item, 0);
			}
		}
		else if (this.CurrentSlot.SlotType == SlotsTypes.Item)
		{
			if (this.GraphicsM.InspectedCard && this.GraphicsM.CurrentInspectionPopup)
			{
				if (this.GraphicsM.InspectedCard.CardModel.CardType != CardTypes.Explorable)
				{
					slotInfo = new SlotInfo(SlotsTypes.Inventory, 0);
				}
			}
			else if (this.GraphicsM.CharacterWindow.gameObject.activeInHierarchy)
			{
				slotInfo = new SlotInfo(SlotsTypes.Equipment, 0);
			}
			else if (!this.GraphicsM.HasPopup)
			{
				slotInfo = new SlotInfo(SlotsTypes.Base, 0);
			}
		}
		if (slotInfo != null)
		{
			slotInfo.SlotIndex = 1000;
			this.GraphicsM.MoveCardToSlot(this, slotInfo, true, false);
		}
	}

	// Token: 0x060001CA RID: 458 RVA: 0x000132EC File Offset: 0x000114EC
	private IEnumerator DoubleClickWaitDelay()
	{
		yield return new WaitForSeconds(0.1f);
		this.GraphicsM.InspectCard(this, false);
		this.Clicks = 0;
		yield break;
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00013368 File Offset: 0x00011568
	[CompilerGenerated]
	private void <UpdatePassiveEffects>g__UpdatePassiveEffect|340_0(PassiveEffect _Effect, ref InGameCardBase.<>c__DisplayClass340_0 A_2)
	{
		if (this.PassiveEffects.ContainsKey(_Effect.EffectName))
		{
			if ((this.InBackground && !this.CardModel.AlwaysUpdate) || this.Destroyed)
			{
				this.GM.StartCoroutineEx(this.CancelPassiveEffect(_Effect), out A_2.controller);
				A_2.waitFor.Add(A_2.controller);
				return;
			}
			if (!this.PassiveEffects[_Effect.EffectName].ConditionsValid(this, this.CurrentSlot, false))
			{
				this.GM.StartCoroutineEx(this.CancelPassiveEffect(_Effect), out A_2.controller);
				A_2.waitFor.Add(A_2.controller);
				return;
			}
		}
		else if ((!this.InBackground || this.CardModel.AlwaysUpdate) && !this.Destroyed && _Effect.ConditionsValid(this, this.CurrentSlot, true))
		{
			this.GM.StartCoroutineEx(this.ApplyPassiveEffect(_Effect), out A_2.controller);
			A_2.waitFor.Add(A_2.controller);
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0001347C File Offset: 0x0001167C
	[CompilerGenerated]
	private void <MoveToParent>g__FinishMoving|368_0(ref InGameCardBase.<>c__DisplayClass368_0 A_1)
	{
		A_1._Target.SetParent(A_1._ToParent);
		A_1._Target.localPosition = Vector3.zero;
		if (A_1._Target == base.transform)
		{
			if (this.PulseAfterReachingSlot)
			{
				if (!this.CurrentSlot)
				{
					this.Pulse(0f);
				}
				else if (this.CurrentSlot.AssignedCard == this)
				{
					this.Pulse(0f);
					this.CurrentSlot.PlayCardLandParticles();
				}
				else
				{
					base.transform.localScale = this.InitialScale;
					if (this.CurrentSlot.AssignedCard.transform.parent == this.CurrentSlot.GetParent)
					{
						this.CurrentSlot.AssignedCard.Pulse(0f);
						this.CurrentSlot.PlayCardLandParticles();
					}
				}
				this.PulseAfterReachingSlot = false;
			}
			if (this.CurrentSlot && this.CurrentSlot.GetParent == A_1._ToParent)
			{
				this.CurrentSlot.SortCardPile();
				if (this.CardVisuals)
				{
					this.CardVisuals.DestroyDuplicate();
				}
			}
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x000135D8 File Offset: 0x000117D8
	[CompilerGenerated]
	private void <GetPossibleAction>g__CheckPossibleActions|378_0(InGameCardBase _Given, InGameCardBase _Receiving, bool _Reverse, ref InGameCardBase.<>c__DisplayClass378_0 A_4)
	{
		if (_Receiving.CardModel.CardInteractions == null)
		{
			return;
		}
		for (int i = 0; i < _Receiving.CardModel.CardInteractions.Length; i++)
		{
			if ((_Receiving.CardModel.CardInteractions[i].WorksBothWays || !_Reverse) && (_Receiving.CardModel.CardInteractions[i].CompatibleCards.IsValidTrigger(_Given.CardModel) || _Receiving.CardModel.CardInteractions[i].CanGiveLiquid(_Given)) && _Receiving.CardModel.CardInteractions[i].RequiredGivenLiquidContent.IsValid(_Given, false))
			{
				A_4.demoOK = _Receiving.CardModel.CardInteractions[i].IsNotCancelledByDemo;
				A_4.effect = _Receiving.CardModel.CardInteractions[i].WillHaveAnEffect(this, false, false, false, false, Array.Empty<CardModifications>());
				A_4.stats = _Receiving.CardModel.CardInteractions[i].StatsAreCorrect(A_4.missingStats, false);
				A_4.board = _Receiving.CardModel.CardInteractions[i].CardsAndTagsAreCorrect(_Receiving, _Given, A_4.missingCards, A_4.missingTags);
				A_4.durabilities = _Receiving.CardModel.CardInteractions[i].DurabilitiesAreCorrect(_Receiving, _Given, out A_4.missingDurabilities);
				if (this.GM.AnyActionBlockers)
				{
					_Receiving.CardModel.CardInteractions[i].CollectActionModifiers(null, null);
				}
				A_4.noBlockingStatus = this.GM.CheckActionBlockers(_Receiving.CardModel.CardInteractions[i], out A_4.blockingStatus);
				A_4.time = _Receiving.CardModel.CardInteractions[i].EnoughDaylightPoints();
				if (((A_4.effect & A_4.stats & A_4.board & A_4.durabilities & A_4.noBlockingStatus & A_4.time) && !A_4.actionPlaying) & A_4.demoOK)
				{
					this.PossibleAction = _Receiving.CardModel.CardInteractions[i];
					this.ActionIsReversed = _Reverse;
					return;
				}
				if (this.ImpossibleAction == null)
				{
					this.ImpossibleAction = _Receiving.CardModel.CardInteractions[i];
					this.ActionIsReversed = _Reverse;
				}
				if ((A_4.effect & A_4.stats & A_4.board & A_4.durabilities & A_4.noBlockingStatus & A_4.demoOK) && !A_4.time)
				{
					break;
				}
			}
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0001384C File Offset: 0x00011A4C
	[CompilerGenerated]
	private bool <WillBeInBG>g__checkPrevEnv|390_0(ref InGameCardBase.<>c__DisplayClass390_0 A_1)
	{
		bool flag = this.CardModel.AlwaysUpdate || this.WasIndependentFromEnv;
		if (!flag && this.ContainedLiquidModel)
		{
			flag = this.ContainedLiquidModel.AlwaysUpdate;
		}
		if (flag)
		{
			if (!A_1._NextEnv)
			{
				flag = false;
			}
			else if (!A_1._NextEnv.InstancedEnvironment)
			{
				flag = false;
			}
		}
		return !flag || (A_1._PrevEnv == this.PrevEnvironment && this.PrevEnvTravelIndex == A_1._TravelIndex) || !this.PrevEnvironment || !A_1._PrevEnv;
	}

	// Token: 0x0400013A RID: 314
	private static int CreatedCardsCount;

	// Token: 0x0400013B RID: 315
	protected const float SpeedPerDistance = 10f;

	// Token: 0x0400013C RID: 316
	protected const float CardMoveMinSpeed = 10f;

	// Token: 0x0400013D RID: 317
	protected const float CardMoveMaxSpeed = 70f;

	// Token: 0x0400013E RID: 318
	protected const float CardMinDistance = 0.01f;

	// Token: 0x0400013F RID: 319
	public const float CardPulseScale = 0.25f;

	// Token: 0x04000140 RID: 320
	public const float CardPulseDuration = 0.33f;

	// Token: 0x04000141 RID: 321
	protected const float CardMovingScale = 1.5f;

	// Token: 0x04000142 RID: 322
	protected const float DoubleClickTime = 0.1f;

	// Token: 0x04000143 RID: 323
	protected const float PosShakeScale = 5f;

	// Token: 0x04000144 RID: 324
	protected const float RotShakeScale = 1f;

	// Token: 0x04000145 RID: 325
	protected const int ShakeVibrato = 15;

	// Token: 0x04000146 RID: 326
	protected const float StandardDestroyDelay = 5f;

	// Token: 0x04000148 RID: 328
	protected static InGameDraggableCard DroppedCard;

	// Token: 0x0400014B RID: 331
	[Header("In Game Values")]
	public float CurrentSpoilage;

	// Token: 0x0400014C RID: 332
	public float CurrentUsageDurability;

	// Token: 0x0400014D RID: 333
	public float CurrentFuel;

	// Token: 0x0400014E RID: 334
	public float CurrentProgress;

	// Token: 0x0400014F RID: 335
	public float CurrentLiquidQuantity;

	// Token: 0x04000150 RID: 336
	public float CurrentSpecial1;

	// Token: 0x04000151 RID: 337
	public float CurrentSpecial2;

	// Token: 0x04000152 RID: 338
	public float CurrentSpecial3;

	// Token: 0x04000153 RID: 339
	public float CurrentSpecial4;

	// Token: 0x04000154 RID: 340
	public float WeightCapacityBonus;

	// Token: 0x04000155 RID: 341
	public bool PulseAfterReachingSlot;

	// Token: 0x04000156 RID: 342
	public int CreatedOnTick;

	// Token: 0x04000157 RID: 343
	public int CreatedInSaveDataTick;

	// Token: 0x04000158 RID: 344
	public ExplorationSaveData ExplorationData;

	// Token: 0x04000159 RID: 345
	public BlueprintSaveData BlueprintData;

	// Token: 0x0400015A RID: 346
	private bool Initialized;

	// Token: 0x0400015B RID: 347
	public bool UpdatedInBackground;

	// Token: 0x0400015C RID: 348
	public bool MarkedAsBlueprintIngredient;

	// Token: 0x0400015D RID: 349
	public int TravelCardIndex;

	// Token: 0x0400015E RID: 350
	protected bool ForceActive;

	// Token: 0x0400015F RID: 351
	private bool SpoilFull;

	// Token: 0x04000160 RID: 352
	private bool SpoilEmpty;

	// Token: 0x04000161 RID: 353
	private bool UsageFull;

	// Token: 0x04000162 RID: 354
	private bool UsageEmpty;

	// Token: 0x04000163 RID: 355
	private bool FuelFull;

	// Token: 0x04000164 RID: 356
	private bool FuelEmpty;

	// Token: 0x04000165 RID: 357
	private bool ProgressFull;

	// Token: 0x04000166 RID: 358
	private bool ProgressEmpty;

	// Token: 0x04000167 RID: 359
	private bool Special1Full;

	// Token: 0x04000168 RID: 360
	private bool Special1Empty;

	// Token: 0x04000169 RID: 361
	private bool Special2Full;

	// Token: 0x0400016A RID: 362
	private bool Special2Empty;

	// Token: 0x0400016B RID: 363
	private bool Special3Full;

	// Token: 0x0400016C RID: 364
	private bool Special3Empty;

	// Token: 0x0400016D RID: 365
	private bool Special4Full;

	// Token: 0x0400016E RID: 366
	private bool Special4Empty;

	// Token: 0x0400016F RID: 367
	public bool IgnoreTickDurabilityChanges;

	// Token: 0x04000170 RID: 368
	public bool IgnoreBaseRow;

	// Token: 0x04000173 RID: 371
	public InGameCardBase ContainedLiquid;

	// Token: 0x04000174 RID: 372
	public CardData FutureLiquidContained;

	// Token: 0x04000175 RID: 373
	[NonSerialized]
	public bool StayInSlotWhenLiquidChanges;

	// Token: 0x0400017F RID: 383
	private CardData PinnedLiquidModel;

	// Token: 0x04000180 RID: 384
	[HideInInspector]
	public DismantleCardAction[] DismantleActions;

	// Token: 0x04000181 RID: 385
	private float BaseSpoilageRate;

	// Token: 0x04000182 RID: 386
	private float BaseUsageRate;

	// Token: 0x04000183 RID: 387
	private float BaseFuelRate;

	// Token: 0x04000184 RID: 388
	private float BaseConsumableRate;

	// Token: 0x04000185 RID: 389
	private float BaseEvaporationRate;

	// Token: 0x04000186 RID: 390
	private float BaseSpecial1Rate;

	// Token: 0x04000187 RID: 391
	private float BaseSpecial2Rate;

	// Token: 0x04000188 RID: 392
	private float BaseSpecial3Rate;

	// Token: 0x04000189 RID: 393
	private float BaseSpecial4Rate;

	// Token: 0x0400018A RID: 394
	public List<LiquidDrop> CurrentProducedLiquids = new List<LiquidDrop>();

	// Token: 0x0400018B RID: 395
	private float BlueprintWeight;

	// Token: 0x0400018C RID: 396
	public CardData Environment;

	// Token: 0x0400018D RID: 397
	public CardData PrevEnvironment;

	// Token: 0x0400018E RID: 398
	public int PrevEnvTravelIndex;

	// Token: 0x0400018F RID: 399
	public bool CanCarryToNewEnv;

	// Token: 0x04000192 RID: 402
	private Vector3 LastAlivePosition;

	// Token: 0x04000193 RID: 403
	private Vector3 InitialScale;

	// Token: 0x04000194 RID: 404
	private bool PrivateBlocksRaycasts;

	// Token: 0x04000195 RID: 405
	protected GameManager GM;

	// Token: 0x04000196 RID: 406
	protected GraphicsManager GraphicsM;

	// Token: 0x04000197 RID: 407
	protected CardOnCardAction PossibleAction;

	// Token: 0x04000198 RID: 408
	protected CardOnCardAction ImpossibleAction;

	// Token: 0x04000199 RID: 409
	protected bool ActionIsReversed;

	// Token: 0x0400019A RID: 410
	protected InGameCardBase.ActionTypes ActionType;

	// Token: 0x0400019B RID: 411
	protected float ActionHoldTimer;

	// Token: 0x0400019C RID: 412
	public CardGraphics CardVisuals;

	// Token: 0x0400019E RID: 414
	public SlotInfo CurrentSlotInfo;

	// Token: 0x040001A0 RID: 416
	public string TooltipTitle;

	// Token: 0x040001A1 RID: 417
	public InGameCardBase CurrentContainer;

	// Token: 0x040001A2 RID: 418
	public List<InventorySlot> CardsInInventory = new List<InventorySlot>();

	// Token: 0x040001A3 RID: 419
	public List<CookingCardStatus> CookingCards = new List<CookingCardStatus>();

	// Token: 0x040001A4 RID: 420
	private List<CardData> CookingResultsList;

	// Token: 0x040001A5 RID: 421
	private List<PassiveEffect> ExternalPassiveEffects = new List<PassiveEffect>();

	// Token: 0x040001A6 RID: 422
	private List<string> ExternalEffectsIDs = new List<string>();

	// Token: 0x040001A7 RID: 423
	private List<StatModifier> CurrentStatModifiers = new List<StatModifier>();

	// Token: 0x040001A8 RID: 424
	public Dictionary<string, Vector2Int> DroppedCollections = new Dictionary<string, Vector2Int>();

	// Token: 0x040001A9 RID: 425
	public Dictionary<string, PassiveEffect> PassiveEffects = new Dictionary<string, PassiveEffect>();

	// Token: 0x040001AB RID: 427
	private Transform InspectionDuplicate;

	// Token: 0x040001AC RID: 428
	private Transform DuplicateParentObject;

	// Token: 0x040001AD RID: 429
	private Sequence ActionPulse;

	// Token: 0x040001AE RID: 430
	private int Clicks;

	// Token: 0x040001AF RID: 431
	private Coroutine WaitBeforeInspection;

	// Token: 0x040001B0 RID: 432
	private Sequence TimeAnimation;

	// Token: 0x040001B1 RID: 433
	private float TimeToDeactivation;

	// Token: 0x040001B5 RID: 437
	private bool IsRegisteredToDragEvents;

	// Token: 0x040001B6 RID: 438
	public bool IsPerformingAction;

	// Token: 0x040001B7 RID: 439
	private bool WasIndependentFromEnv;

	// Token: 0x0200022E RID: 558
	protected enum ActionTypes
	{
		// Token: 0x04001374 RID: 4980
		LiquidDouble,
		// Token: 0x04001375 RID: 4981
		GivenLiquidStandard,
		// Token: 0x04001376 RID: 4982
		ReceivingLiquidStandard,
		// Token: 0x04001377 RID: 4983
		Standard,
		// Token: 0x04001378 RID: 4984
		LiquidTransfer,
		// Token: 0x04001379 RID: 4985
		TravelCardStoring
	}
}
