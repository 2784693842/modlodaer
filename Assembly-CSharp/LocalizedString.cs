using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000193 RID: 403
[Serializable]
public struct LocalizedString
{
	// Token: 0x06000AAC RID: 2732 RVA: 0x0005E656 File Offset: 0x0005C856
	public static implicit operator string(LocalizedString s)
	{
		return s.ToString();
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0005E668 File Offset: 0x0005C868
	public override string ToString()
	{
		if (!string.IsNullOrEmpty(this.LocalizedText))
		{
			return this.LocalizedText;
		}
		if (string.IsNullOrEmpty(this.LocalizationKey))
		{
			if (!Application.isEditor)
			{
				this.LocalizedText = this.DefaultText;
			}
			return this.DefaultText;
		}
		if (this.LocalizationKey == "IGNOREKEY")
		{
			if (!Application.isEditor)
			{
				this.LocalizedText = this.DefaultText;
			}
			return this.DefaultText;
		}
		if (!LocalizationManager.GetText(this.LocalizationKey, out this.LocalizedText))
		{
			if (!Application.isEditor)
			{
				this.LocalizedText = this.DefaultText;
			}
			return this.DefaultText;
		}
		if (!string.IsNullOrEmpty(this.LocalizedText))
		{
			return this.LocalizedText;
		}
		if (!Application.isEditor)
		{
			this.LocalizedText = this.DefaultText;
		}
		return this.DefaultText;
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x0005E738 File Offset: 0x0005C938
	public static List<LocalizedString> GatherAllStaticStrings()
	{
		return new List<LocalizedString>
		{
			LocalizedString.ActionHappening,
			LocalizedString.AddToBlueprintFormat,
			LocalizedString.AuthorNameFormat,
			LocalizedString.Available,
			LocalizedString.BlueprintFullFormat,
			LocalizedString.BlueprintResearchTab,
			LocalizedString.BlueprintShop,
			LocalizedString.BlueprintUnlocked,
			LocalizedString.BookmarkNotFoundFormat,
			LocalizedString.BookmarkWithLiquidFormat,
			LocalizedString.Build,
			LocalizedString.Built,
			LocalizedString.BuyToContinueFormat,
			LocalizedString.Cancel,
			LocalizedString.CannotAffordFormat,
			LocalizedString.CannotEquipMoreFormat,
			LocalizedString.CannotEquipPins,
			LocalizedString.CannotInspect,
			LocalizedString.CannotMakeBlueprintFormat,
			LocalizedString.Character,
			LocalizedString.CharacterCreator,
			LocalizedString.ClearCardFilters,
			LocalizedString.Confirm,
			LocalizedString.ConfirmBlueprintResearchFormat,
			LocalizedString.CookingTimeTextFormat,
			LocalizedString.CurrentStatus,
			LocalizedString.CustomCharacters,
			LocalizedString.CustomPortraitCountFormat,
			LocalizedString.Damages,
			LocalizedString.DayCounterFormat,
			LocalizedString.DayFormat,
			LocalizedString.DaysSurvivedFormat,
			LocalizedString.Default,
			LocalizedString.DefaultCharacter,
			LocalizedString.DefaultCookingSlot,
			LocalizedString.DefaultEmptyRecipeSlot,
			LocalizedString.DefaultEnemyGettingCloseLogFormat,
			LocalizedString.DefaultEnemyName,
			LocalizedString.DefaultFuelFullFormat,
			LocalizedString.DefaultFuelOnZeroFormat,
			LocalizedString.DefaultInventorySlot,
			LocalizedString.DefaultProgressFullFormat,
			LocalizedString.DefaultProgressOnZeroFormat,
			LocalizedString.DefaultSpoilageFullFormat,
			LocalizedString.DefaultSpoilageOnZeroFormat,
			LocalizedString.DefaultUsageFullFormat,
			LocalizedString.DefaultUsageOnZeroFormat,
			LocalizedString.DemoOver,
			LocalizedString.DifficultyFormat,
			LocalizedString.DiscardDesc,
			LocalizedString.DiscardTitle,
			LocalizedString.DismantleDesc,
			LocalizedString.DismantleFirstStepDesc,
			LocalizedString.DoStackActionFormat,
			LocalizedString.Duration,
			LocalizedString.Empty,
			LocalizedString.EmptyCharacterBio,
			LocalizedString.EquipFormat,
			LocalizedString.Equipment,
			LocalizedString.EventTitle,
			LocalizedString.Exploration,
			LocalizedString.Explored,
			LocalizedString.Exploring,
			LocalizedString.FillWithLiquidFormat,
			LocalizedString.FinishedCookingFormat,
			LocalizedString.FinishedProducingFormat,
			LocalizedString.GameOver,
			LocalizedString.GuidePlaceHolderTextFormat,
			LocalizedString.HoldForAction,
			LocalizedString.HoldFormat,
			LocalizedString.HopeYouHadAGoodTime,
			LocalizedString.HourFormat,
			LocalizedString.HoverFormat,
			LocalizedString.ImportPortrait,
			LocalizedString.ImpossibleAction,
			LocalizedString.ImprovementDescFormat,
			LocalizedString.Improvements,
			LocalizedString.InspectionTitle,
			LocalizedString.Interaction,
			LocalizedString.InventoryCannotCarryFormat,
			LocalizedString.InventoryFull,
			LocalizedString.IsNotEquipmentFormat,
			LocalizedString.JournalTaglineFormat,
			LocalizedString.LiquidAutoFillingFormat,
			LocalizedString.LiquidContainerEmptyFormat,
			LocalizedString.LiquidTransfer,
			LocalizedString.Load,
			LocalizedString.Loading,
			LocalizedString.MinuteFormat,
			LocalizedString.MissingCardInHand,
			LocalizedString.MissingCardOnBoard,
			LocalizedString.MoveItemToLocationFormat,
			LocalizedString.NewBlueprint,
			LocalizedString.NewDamage,
			LocalizedString.NewImprovement,
			LocalizedString.NoDamagesPresent,
			LocalizedString.NoImprovementsUnlocked,
			LocalizedString.None,
			LocalizedString.OfficialCharacters,
			LocalizedString.Paused,
			LocalizedString.PlayerAttackAndFormat,
			LocalizedString.PlayerAttackButFormat,
			LocalizedString.PlayerCannotCarry,
			LocalizedString.Research,
			LocalizedString.Researching,
			LocalizedString.Resume,
			LocalizedString.SafetyMode,
			LocalizedString.Save,
			LocalizedString.SaveNewGame,
			LocalizedString.SelectCharacter,
			LocalizedString.SelectedCards,
			LocalizedString.Slot,
			LocalizedString.StartBuilding,
			LocalizedString.StatInfluences,
			LocalizedString.StoreInInventory,
			LocalizedString.SuccessChance,
			LocalizedString.SurvivalBonus,
			LocalizedString.TimeOfDayEffectFormat,
			LocalizedString.Tip,
			LocalizedString.TransferToLocation,
			LocalizedString.TrashConfirmFormat,
			LocalizedString.UnavailableInDemo,
			LocalizedString.UnknownAuthor,
			LocalizedString.UnlockForeword,
			LocalizedString.Victory,
			LocalizedString.VictoryBonus,
			LocalizedString.WeaponAndProjectileActionFormat,
			LocalizedString.Wounded,
			LocalizedString.Wounds
		};
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0005ECD8 File Offset: 0x0005CED8
	public static LocalizedString Tip
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "TIP",
				DefaultText = "Tip"
			};
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0005ED08 File Offset: 0x0005CF08
	public static LocalizedString DiscardTitle
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DISCARD_ACTION_TITLE",
				DefaultText = "Discard"
			};
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0005ED38 File Offset: 0x0005CF38
	public static LocalizedString DiscardDesc
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DISCARD_ACTION_DESC",
				DefaultText = "Permanently get rid of this object."
			};
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0005ED68 File Offset: 0x0005CF68
	public static LocalizedString Duration
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DURATION",
				DefaultText = "Duration"
			};
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0005ED98 File Offset: 0x0005CF98
	public static LocalizedString StoreInInventory
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "INVENTORY_STORE",
				DefaultText = "Store inside"
			};
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0005EDC8 File Offset: 0x0005CFC8
	public static LocalizedString InventoryFull
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "INVENTORY_FULL",
				DefaultText = "inventory full"
			};
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0005EDF8 File Offset: 0x0005CFF8
	private static LocalizedString DayCounterFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DAY_COUNTER",
				DefaultText = "Day {0}"
			};
		}
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x0005EE26 File Offset: 0x0005D026
	public static string DayCounter(int _Days)
	{
		return string.Format(LocalizedString.DayCounterFormat, _Days);
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0005EE40 File Offset: 0x0005D040
	public static LocalizedString InspectionTitle
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "INSPECTION_TITLE",
				DefaultText = "DETAILS"
			};
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0005EE70 File Offset: 0x0005D070
	public static LocalizedString EventTitle
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EVENT_TITLE",
				DefaultText = "EVENT"
			};
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x0005EEA0 File Offset: 0x0005D0A0
	public static LocalizedString DayFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FORMAT_DAYS",
				DefaultText = "0D"
			};
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000ABA RID: 2746 RVA: 0x0005EED0 File Offset: 0x0005D0D0
	public static LocalizedString HourFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FORMAT_HOURS",
				DefaultText = "0h"
			};
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0005EF00 File Offset: 0x0005D100
	public static LocalizedString MinuteFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FORMAT_MINUTES",
				DefaultText = "0min"
			};
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0005EF30 File Offset: 0x0005D130
	public static LocalizedString SaveNewGame
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SAVE_NEW_GAME",
				DefaultText = "Save New Game"
			};
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0005EF60 File Offset: 0x0005D160
	public static LocalizedString Save
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SAVE",
				DefaultText = "Save"
			};
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0005EF90 File Offset: 0x0005D190
	public static LocalizedString Load
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "LOAD",
				DefaultText = "Load"
			};
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0005EFC0 File Offset: 0x0005D1C0
	public static LocalizedString Slot
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SLOT",
				DefaultText = "Slot"
			};
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x0005EFF0 File Offset: 0x0005D1F0
	public static LocalizedString Default
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DEFAULT",
				DefaultText = "Default"
			};
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x0005F020 File Offset: 0x0005D220
	public static LocalizedString DefaultCharacter
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DEFAULT_CHARACTER",
				DefaultText = "Default Character"
			};
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x0005F050 File Offset: 0x0005D250
	public static LocalizedString SelectCharacter
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SELECT_CHARACTER",
				DefaultText = "SELECT CHARACTER"
			};
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0005F080 File Offset: 0x0005D280
	public static LocalizedString OfficialCharacters
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "OFFICIAL_CHARACTERS",
				DefaultText = "Premade Characters"
			};
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0005F0B0 File Offset: 0x0005D2B0
	public static LocalizedString CustomCharacters
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CUSTOM_CHARACTERS",
				DefaultText = "Custom Characters"
			};
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0005F0E0 File Offset: 0x0005D2E0
	public static LocalizedString CannotInspect
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CANNOT_INSPECT",
				DefaultText = "Take item out of container to inspect."
			};
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0005F110 File Offset: 0x0005D310
	public static LocalizedString SuccessChance
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SUCCESS_CHANCE",
				DefaultText = "Success"
			};
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0005F140 File Offset: 0x0005D340
	public static LocalizedString Loading
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "LOADING",
				DefaultText = "LOADING"
			};
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0005F170 File Offset: 0x0005D370
	public static LocalizedString Explored
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EXPLORED",
				DefaultText = "Explored"
			};
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0005F1A0 File Offset: 0x0005D3A0
	public static LocalizedString Exploring
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EXPLORING",
				DefaultText = "Exploring"
			};
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0005F1D0 File Offset: 0x0005D3D0
	public static LocalizedString Interaction
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "INTERACTION",
				DefaultText = "Interaction"
			};
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0005F200 File Offset: 0x0005D400
	public static LocalizedString Exploration
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EXPLORATION",
				DefaultText = "Exploration"
			};
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0005F230 File Offset: 0x0005D430
	public static LocalizedString Improvements
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "IMPROVEMENTS",
				DefaultText = "Improvements"
			};
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0005F260 File Offset: 0x0005D460
	public static LocalizedString Damages
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DAMAGES",
				DefaultText = "Damages"
			};
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0005F290 File Offset: 0x0005D490
	public static LocalizedString Confirm
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CONFIRM",
				DefaultText = "Confirm"
			};
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0005F2C0 File Offset: 0x0005D4C0
	public static LocalizedString SelectedCards
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SELECTED_CARDS",
				DefaultText = "Selected Cards"
			};
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0005F2F0 File Offset: 0x0005D4F0
	public static LocalizedString Built
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BUILT",
				DefaultText = "Built"
			};
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0005F320 File Offset: 0x0005D520
	public static LocalizedString Build
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BUILD",
				DefaultText = "Build"
			};
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0005F350 File Offset: 0x0005D550
	public static LocalizedString Cancel
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CANCEL",
				DefaultText = "Cancel"
			};
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x0005F380 File Offset: 0x0005D580
	public static LocalizedString DismantleDesc
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DISMANTLE_DESC",
				DefaultText = "Collect all items and revert to previous building stage."
			};
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0005F3B0 File Offset: 0x0005D5B0
	public static LocalizedString DismantleFirstStepDesc
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DISMANTLE_DESC_FIRST_STEP",
				DefaultText = "Collect all items and destroy building."
			};
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0005F3E0 File Offset: 0x0005D5E0
	public static LocalizedString StartBuilding
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "START_BUILDING",
				DefaultText = "Start the construction of "
			};
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0005F410 File Offset: 0x0005D610
	private static LocalizedString AddToBlueprintFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "ADD_TO_BLUEPRINT",
				DefaultText = "Add {0} to {1}"
			};
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0005F43E File Offset: 0x0005D63E
	public static string AddToBlueprint(InGameCardBase _Addition, CardData _Blueprint)
	{
		return string.Format(LocalizedString.AddToBlueprintFormat, _Addition.CardName(true), _Blueprint.CardName);
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0005F464 File Offset: 0x0005D664
	private static LocalizedString BlueprintFullFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BLUEPRINT_FULL",
				DefaultText = "{1} already has enough of {0}"
			};
		}
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0005F492 File Offset: 0x0005D692
	public static string BlueprintFull(CardData _Addition, CardData _Blueprint)
	{
		return string.Format(LocalizedString.BlueprintFullFormat, _Addition.CardName, _Blueprint.CardName);
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0005F4BC File Offset: 0x0005D6BC
	private static LocalizedString IsNotEquipmentFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "IS_NOT_EQUIPMENT",
				DefaultText = "{0} is not equippable"
			};
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0005F4EA File Offset: 0x0005D6EA
	public static string IsNotEquipment(CardData _Card)
	{
		return string.Format(LocalizedString.IsNotEquipmentFormat, _Card.CardName);
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0005F508 File Offset: 0x0005D708
	public static LocalizedString CannotEquipPins
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CANNOT_EQUIP_PINS",
				DefaultText = "Pins cannot be equipped!"
			};
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0005F538 File Offset: 0x0005D738
	private static LocalizedString CannotEquipMoreFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CANNOT_EQUIP_MORE",
				DefaultText = "Cannot equip more than {0} {1}"
			};
		}
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0005F566 File Offset: 0x0005D766
	public static string CannotEquipMore(EquipmentTag _Tag)
	{
		return string.Format(LocalizedString.CannotEquipMoreFormat, _Tag.MaxEquipped, _Tag.InGameName);
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0005F590 File Offset: 0x0005D790
	private static LocalizedString DefaultSpoilageOnZeroFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SPOILAGE_ZERO",
				DefaultText = "{0} spoiled"
			};
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0005F5BE File Offset: 0x0005D7BE
	public static string DefaultSpoilageOnZero(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultSpoilageOnZeroFormat, _Card.CardName(true));
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x0005F5EC File Offset: 0x0005D7EC
	private static LocalizedString DefaultSpoilageFullFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SPOILAGE_FULL",
				DefaultText = "{0} is good as new"
			};
		}
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0005F61A File Offset: 0x0005D81A
	public static string DefaultSpoilageFull(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultSpoilageFullFormat, _Card.CardName(true));
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0005F648 File Offset: 0x0005D848
	private static LocalizedString DefaultFuelOnZeroFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FUEL_ZERO",
				DefaultText = "{0} ran out of fuel"
			};
		}
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x0005F676 File Offset: 0x0005D876
	public static string DefaultFuelOnZero(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultFuelOnZeroFormat, _Card.CardName(true));
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0005F6A4 File Offset: 0x0005D8A4
	private static LocalizedString DefaultFuelFullFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FUEL_FULL",
				DefaultText = "{0} is full"
			};
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0005F6D2 File Offset: 0x0005D8D2
	public static string DefaultFuelFull(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultFuelFullFormat, _Card.CardName(true));
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0005F700 File Offset: 0x0005D900
	private static LocalizedString DefaultUsageOnZeroFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "USAGE_ZERO",
				DefaultText = "{0} broke"
			};
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0005F72E File Offset: 0x0005D92E
	public static string DefaultUsageOnZero(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultUsageOnZeroFormat, _Card.CardName(true));
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x0005F75C File Offset: 0x0005D95C
	private static LocalizedString DefaultUsageFullFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "USAGE_FULL",
				DefaultText = "{0} is good as new"
			};
		}
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0005F78A File Offset: 0x0005D98A
	public static string DefaultUsageFull(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultUsageFullFormat, _Card.CardName(true));
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000AEB RID: 2795 RVA: 0x0005F7B8 File Offset: 0x0005D9B8
	private static LocalizedString DefaultProgressOnZeroFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "PROGRESS_ZERO",
				DefaultText = "{0} ran out of uses"
			};
		}
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0005F7E6 File Offset: 0x0005D9E6
	public static string DefaultProgressOnZero(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultProgressOnZeroFormat, _Card.CardName(true));
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000AED RID: 2797 RVA: 0x0005F814 File Offset: 0x0005DA14
	private static LocalizedString DefaultProgressFullFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "PROGRESS_FULL",
				DefaultText = "{0} is complete"
			};
		}
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0005F842 File Offset: 0x0005DA42
	public static string DefaultProgressFull(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.DefaultProgressFullFormat, _Card.CardName(true));
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0005F870 File Offset: 0x0005DA70
	private static LocalizedString LiquidContainerEmptyFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "LIQUID_CONTAINER_EMPTY",
				DefaultText = "{0} is empty"
			};
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0005F89E File Offset: 0x0005DA9E
	public static string LiquidContainerEmpty(InGameCardBase _Card)
	{
		if (!_Card.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.LiquidContainerEmptyFormat, _Card.CardName(true));
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0005F8CC File Offset: 0x0005DACC
	public static LocalizedString LiquidTransfer
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "LIQUID_TRANSFER",
				DefaultText = "Transfer Liquid"
			};
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0005F8FC File Offset: 0x0005DAFC
	private static LocalizedString FillWithLiquidFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FILL_WITH_LIQUID",
				DefaultText = "Fill {0} with {1}"
			};
		}
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0005F92A File Offset: 0x0005DB2A
	public static string FillWithLiquid(InGameCardBase _To, CardData _Liquid)
	{
		if (!_To.CardModel || !_Liquid)
		{
			return "";
		}
		return string.Format(LocalizedString.FillWithLiquidFormat, _To.CardName(true), _Liquid.CardName);
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0005F968 File Offset: 0x0005DB68
	public static LocalizedString Empty
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EMPTY",
				DefaultText = "Empty"
			};
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0005F998 File Offset: 0x0005DB98
	private static LocalizedString FinishedCookingFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FINISHED_COOKING",
				DefaultText = "{0} finished cooking {1}"
			};
		}
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0005F9C8 File Offset: 0x0005DBC8
	public static string FinishedCooking(InGameCardBase _CookerCard, InGameCardBase _CookedCard)
	{
		if (!_CookerCard || !_CookedCard)
		{
			return "";
		}
		if (!_CookerCard.CardModel || !_CookedCard.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.FinishedCookingFormat, _CookerCard.CardName(true), _CookedCard.CardName(true));
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0005FA28 File Offset: 0x0005DC28
	private static LocalizedString FinishedProducingFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "FINISHED_COOKING_EMPTY",
				DefaultText = "{0} is ready"
			};
		}
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x0005FA56 File Offset: 0x0005DC56
	public static string FinishedProducing(InGameCardBase _Producer)
	{
		if (!_Producer)
		{
			return "";
		}
		if (!_Producer.CardModel)
		{
			return "";
		}
		return string.Format(LocalizedString.FinishedProducingFormat, _Producer.CardName(true));
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x0005FA90 File Offset: 0x0005DC90
	private static LocalizedString CookingTimeTextFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "COOKING_TIME_TEXT",
				DefaultText = "Ready in {0}"
			};
		}
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0005FABE File Offset: 0x0005DCBE
	public static string CookingTimeText(int _Ticks, int _MiniTicks)
	{
		return string.Format(LocalizedString.CookingTimeTextFormat, HoursDisplay.HoursToShortString(GameManager.TickToHours(_Ticks, _MiniTicks)));
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0005FADC File Offset: 0x0005DCDC
	public static LocalizedString Research
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "RESEARCH",
				DefaultText = "Research"
			};
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000AFC RID: 2812 RVA: 0x0005FB0C File Offset: 0x0005DD0C
	public static LocalizedString Researching
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "RESEARCHING",
				DefaultText = "Researching..."
			};
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000AFD RID: 2813 RVA: 0x0005FB3C File Offset: 0x0005DD3C
	public static LocalizedString Resume
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "RESUME",
				DefaultText = "Resume"
			};
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000AFE RID: 2814 RVA: 0x0005FB6C File Offset: 0x0005DD6C
	private static LocalizedString ConfirmBlueprintResearchFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CONFIRM_RESEARCH_SWITCH",
				DefaultText = "Currently researching {0}.\nSwitch research to {1}?\n(You will keep research progress on {0})"
			};
		}
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0005FB9A File Offset: 0x0005DD9A
	public static string ConfirmBlueprintResearch(CardData _CurrentResearch, CardData _NewResearch)
	{
		return string.Format(LocalizedString.ConfirmBlueprintResearchFormat, _CurrentResearch.CardName, _NewResearch.CardName);
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0005FBC4 File Offset: 0x0005DDC4
	public static string BlueprintResearchText(int _Ticks, bool _Paused)
	{
		GameManager instance = MBSingleton<GameManager>.Instance;
		int num = _Ticks;
		int num2 = 0;
		if (instance && instance.CurrentMiniTicks != 0)
		{
			num--;
			num2 = instance.DaySettings.MiniTicksPerTick - instance.CurrentMiniTicks;
		}
		if (_Paused)
		{
			return string.Format("{0} ({1})", LocalizedString.Paused, HoursDisplay.HoursToShortString(GameManager.TickToHours(num, num2)));
		}
		return LocalizedString.CookingTimeText(num, num2);
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0005FC30 File Offset: 0x0005DE30
	public static LocalizedString Paused
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "PAUSED",
				DefaultText = "Paused"
			};
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0005FC60 File Offset: 0x0005DE60
	private static LocalizedString DaysSurvivedFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DAYS_SURVIVED",
				DefaultText = "I survived for {0} days."
			};
		}
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0005FC8E File Offset: 0x0005DE8E
	public static string DaysSurvived(int _Days)
	{
		return string.Format(LocalizedString.DaysSurvivedFormat, _Days.ToString());
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000B04 RID: 2820 RVA: 0x0005FCA8 File Offset: 0x0005DEA8
	private static LocalizedString MissingCardOnBoard
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "MISSING_CARD_ONBOARD",
				DefaultText = "Missing {0}"
			};
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0005FCD8 File Offset: 0x0005DED8
	private static LocalizedString MissingCardInHand
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "MISSING_CARD_INHAND",
				DefaultText = "Missing {0} in hand"
			};
		}
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0005FD08 File Offset: 0x0005DF08
	public static string MissingCard(string _CardName, bool _InHand)
	{
		LocalizedString s;
		if (!_InHand)
		{
			s = LocalizedString.MissingCardOnBoard;
		}
		else
		{
			s = LocalizedString.MissingCardInHand;
		}
		return string.Format(s, _CardName);
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000B07 RID: 2823 RVA: 0x0005FD34 File Offset: 0x0005DF34
	public static LocalizedString ActionHappening
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "ACTION_HAPPENING",
				DefaultText = "I can't do two things at once..."
			};
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000B08 RID: 2824 RVA: 0x0005FD64 File Offset: 0x0005DF64
	private static LocalizedString DoStackActionFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DO_STACK_ACTION",
				DefaultText = "Do x{0}"
			};
		}
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0005FD92 File Offset: 0x0005DF92
	public static string DoStackAction(int _Stack)
	{
		return string.Format(LocalizedString.DoStackActionFormat, _Stack.ToString());
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000B0A RID: 2826 RVA: 0x0005FDAC File Offset: 0x0005DFAC
	public static LocalizedString DefaultCookingSlot
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "COOKING_SLOT",
				DefaultText = "DRAG ITEMS HERE TO COOK"
			};
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0005FDDC File Offset: 0x0005DFDC
	public static LocalizedString DefaultEmptyRecipeSlot
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EMPTY_RECIPE_SLOT",
				DefaultText = "COME BACK LATER"
			};
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0005FE0C File Offset: 0x0005E00C
	public static LocalizedString DefaultInventorySlot
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "INVENTORY_SLOT",
				DefaultText = "DRAG ITEMS HERE"
			};
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000B0D RID: 2829 RVA: 0x0005FE3C File Offset: 0x0005E03C
	private static LocalizedString TrashConfirmFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "TRASH_CONFIRM",
				DefaultText = "Throw {0} away?"
			};
		}
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x0005FE6A File Offset: 0x0005E06A
	public static string TrashConfirmText(CardData _Card)
	{
		if (!_Card)
		{
			return "";
		}
		return string.Format(LocalizedString.TrashConfirmFormat, _Card.CardName);
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0005FE94 File Offset: 0x0005E094
	private static LocalizedString BookmarkNotFoundFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BOOKMARK_NOT_FOUND",
				DefaultText = "There is no {0} here"
			};
		}
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0005FEC2 File Offset: 0x0005E0C2
	public static string BookmarkNotFound(string _TargetName)
	{
		if (string.IsNullOrEmpty(_TargetName))
		{
			return "";
		}
		return string.Format(LocalizedString.BookmarkNotFoundFormat, _TargetName);
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0005FEE4 File Offset: 0x0005E0E4
	private static LocalizedString DifficultyFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DIFFICULTY_SCORE",
				DefaultText = "Difficulty: {0}pts"
			};
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x0005FF12 File Offset: 0x0005E112
	public static string Difficulty(int _Value)
	{
		return string.Format(LocalizedString.DifficultyFormat, _Value.ToString());
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000B13 RID: 2835 RVA: 0x0005FF2C File Offset: 0x0005E12C
	private static LocalizedString UnlockForeword
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "TO_UNLOCK",
				DefaultText = "To unlock: "
			};
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000B14 RID: 2836 RVA: 0x0005FF5C File Offset: 0x0005E15C
	private static LocalizedString Available
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "AVAILABLE",
				DefaultText = "Available"
			};
		}
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0005FF8C File Offset: 0x0005E18C
	public static string UnlockConditions(string _Conditions, bool _Unlocked)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!_Unlocked)
		{
			stringBuilder.Append("<b>");
			stringBuilder.Append(LocalizedString.UnlockForeword);
			stringBuilder.Append("</b>");
			stringBuilder.Append(_Conditions);
		}
		else
		{
			stringBuilder.Append("<color=#358F3F>");
			if (!string.IsNullOrEmpty(_Conditions))
			{
				stringBuilder.Append(_Conditions);
			}
			else
			{
				stringBuilder.Append(LocalizedString.Available);
			}
			stringBuilder.Append("</color>");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00060018 File Offset: 0x0005E218
	public static LocalizedString CharacterCreator
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CHARACTER_CREATOR",
				DefaultText = "Create New Character"
			};
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00060048 File Offset: 0x0005E248
	public static LocalizedString Victory
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "VICTORY",
				DefaultText = "I SURVIVED!"
			};
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00060078 File Offset: 0x0005E278
	public static LocalizedString GameOver
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "GAME_OVER",
				DefaultText = "GAME OVER"
			};
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000B19 RID: 2841 RVA: 0x000600A8 File Offset: 0x0005E2A8
	public static LocalizedString SurvivalBonus
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SURVIVAL_BONUS",
				DefaultText = "Survival bonus"
			};
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000B1A RID: 2842 RVA: 0x000600D8 File Offset: 0x0005E2D8
	public static LocalizedString VictoryBonus
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "VICTORY_BONUS",
				DefaultText = "Victory Bonus"
			};
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00060108 File Offset: 0x0005E308
	private static LocalizedString AuthorNameFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BY_AUTHOR",
				DefaultText = "By {0}"
			};
		}
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00060136 File Offset: 0x0005E336
	public static string AuthorName(string _CharacterName)
	{
		return string.Format(LocalizedString.AuthorNameFormat, _CharacterName.ToString());
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00060150 File Offset: 0x0005E350
	private static LocalizedString JournalTaglineFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "JOURNAL_SUBTITLE_NAME",
				DefaultText = "The {0} tale of {2}\n({1})"
			};
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00060180 File Offset: 0x0005E380
	public static string JournalTaglineWithName(string _Rating, int _RatingScore, string _CharacterName, bool _SafetyMode)
	{
		LocalizedString journalTaglineFormat = LocalizedString.JournalTaglineFormat;
		if (_SafetyMode)
		{
			journalTaglineFormat.DefaultText += " - {3}";
			return string.Format(journalTaglineFormat, new object[]
			{
				_Rating,
				_RatingScore.ToString(),
				_CharacterName,
				LocalizedString.SafetyMode
			});
		}
		return string.Format(journalTaglineFormat, _Rating, _RatingScore.ToString(), _CharacterName);
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000B1F RID: 2847 RVA: 0x000601F0 File Offset: 0x0005E3F0
	public static LocalizedString SafetyMode
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "SAFETY_MODE",
				DefaultText = "Safety Mode"
			};
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00060220 File Offset: 0x0005E420
	public static LocalizedString UnknownAuthor
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "UNKNOWN_AUTHOR",
				DefaultText = "Unknown Author"
			};
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00060250 File Offset: 0x0005E450
	public static LocalizedString ClearCardFilters
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CLEAR_CARD_FILTERS",
				DefaultText = "Clear Filters"
			};
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00060280 File Offset: 0x0005E480
	private static LocalizedString CannotMakeBlueprintFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CANNOT_MAKE_BLUEPRINT",
				DefaultText = "{0} cannot be made here..."
			};
		}
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x000602AE File Offset: 0x0005E4AE
	public static string CannotMakeBlueprint(CardData _Blueprint)
	{
		return string.Format(LocalizedString.CannotMakeBlueprintFormat, _Blueprint.CardName);
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000B24 RID: 2852 RVA: 0x000602CC File Offset: 0x0005E4CC
	private static LocalizedString CannotAffordFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CANNOT_AFFORD",
				DefaultText = "Not enough {0} to buy this..."
			};
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x000602FA File Offset: 0x0005E4FA
	public static string CannotAfford(string _Currency)
	{
		return string.Format(LocalizedString.CannotAffordFormat, _Currency);
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0006030C File Offset: 0x0005E50C
	public static LocalizedString ImportPortrait
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "IMPORT_PORTRAIT",
				DefaultText = "Import Custom Portrait"
			};
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x0006033C File Offset: 0x0005E53C
	private static LocalizedString CustomPortraitCountFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CUSTOM_PORTRAIT_COUNT",
				DefaultText = "Default portrait resolution is 344x446\n{0}/{1} portraits saved"
			};
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0006036A File Offset: 0x0005E56A
	public static string CustomPortraitCount(int _Count, int _MaxCount)
	{
		return string.Format(LocalizedString.CustomPortraitCountFormat, _Count, _MaxCount);
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00060388 File Offset: 0x0005E588
	public static LocalizedString BlueprintUnlocked
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BLUEPRINT_UNLOCKED",
				DefaultText = "New Blueprint Unlocked!"
			};
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000B2A RID: 2858 RVA: 0x000603B8 File Offset: 0x0005E5B8
	public static LocalizedString ImpossibleAction
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "IMPOSSIBLE_ACTION",
				DefaultText = "I can't do this now..."
			};
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000B2B RID: 2859 RVA: 0x000603E8 File Offset: 0x0005E5E8
	private static LocalizedString BookmarkWithLiquidFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BOOKMARK_TITLE_WITH_LIQUID",
				DefaultText = "{0} ({1})"
			};
		}
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00060416 File Offset: 0x0005E616
	public static string BookmarkWithLiquid(CardData _Liquid, string _Container)
	{
		return string.Format(LocalizedString.BookmarkWithLiquidFormat, _Container, _Liquid.CardName);
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000B2D RID: 2861 RVA: 0x00060434 File Offset: 0x0005E634
	public static LocalizedString PlayerCannotCarry
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "PLAYER_CANNOT_CARRY",
				DefaultText = "This is more than I can carry..."
			};
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00060464 File Offset: 0x0005E664
	private static LocalizedString InventoryCannotCarryFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "INVENTORY_CANNOT_CARRY",
				DefaultText = "{0} doesn't have room for this..."
			};
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00060492 File Offset: 0x0005E692
	public static string InventoryCannotCarry(InGameCardBase _Inventory)
	{
		return string.Format(LocalizedString.InventoryCannotCarryFormat, _Inventory.CardName(true));
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000B30 RID: 2864 RVA: 0x000604AC File Offset: 0x0005E6AC
	public static LocalizedString HoldForAction
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "HOLD_FOR_ACTION",
				DefaultText = "Hold for action..."
			};
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000B31 RID: 2865 RVA: 0x000604DC File Offset: 0x0005E6DC
	public static LocalizedString Equipment
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EQUIPMENT",
				DefaultText = "Equipment"
			};
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0006050C File Offset: 0x0005E70C
	public static LocalizedString Wounds
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "WOUNDS",
				DefaultText = "Wounds"
			};
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0006053C File Offset: 0x0005E73C
	public static LocalizedString Character
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CHARACTER",
				DefaultText = "Character"
			};
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000B34 RID: 2868 RVA: 0x0006056C File Offset: 0x0005E76C
	private static LocalizedString EquipFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "EQUIP_CARD",
				DefaultText = "Equip {0}"
			};
		}
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0006059A File Offset: 0x0005E79A
	public static string Equip(InGameCardBase _Card)
	{
		return string.Format(LocalizedString.EquipFormat, _Card.CardName(true));
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000B36 RID: 2870 RVA: 0x000605B4 File Offset: 0x0005E7B4
	public static LocalizedString NewBlueprint
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "NEW_BLUEPRINT",
				DefaultText = "New Blueprint"
			};
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000B37 RID: 2871 RVA: 0x000605E4 File Offset: 0x0005E7E4
	public static LocalizedString NewImprovement
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "NEW_IMPROVEMENT",
				DefaultText = "New Improvement"
			};
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00060614 File Offset: 0x0005E814
	public static LocalizedString NewDamage
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "NEW_DAMAGE",
				DefaultText = "Something got damaged!"
			};
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00060644 File Offset: 0x0005E844
	private static LocalizedString ImprovementDescFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "IMPROVEMENT_DESC",
				DefaultText = "{0} on a card for info"
			};
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000B3A RID: 2874 RVA: 0x00060674 File Offset: 0x0005E874
	private static LocalizedString HoldFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "HOLD",
				DefaultText = "Hold"
			};
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000B3B RID: 2875 RVA: 0x000606A4 File Offset: 0x0005E8A4
	private static LocalizedString HoverFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "HOVER",
				DefaultText = "Hover"
			};
		}
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x000606D4 File Offset: 0x0005E8D4
	public static string ImprovementDescription(bool _Hold)
	{
		LocalizedString improvementDescFormat = LocalizedString.ImprovementDescFormat;
		LocalizedString holdFormat = LocalizedString.HoldFormat;
		LocalizedString hoverFormat = LocalizedString.HoverFormat;
		return string.Format(improvementDescFormat, _Hold ? holdFormat : hoverFormat);
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00060708 File Offset: 0x0005E908
	public static LocalizedString NoImprovementsUnlocked
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "NO_IMPROVEMENTS",
				DefaultText = "No Improvements Unlocked"
			};
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00060738 File Offset: 0x0005E938
	public static LocalizedString NoDamagesPresent
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "NO_DAMAGES",
				DefaultText = "No Damages To Repair"
			};
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00060768 File Offset: 0x0005E968
	public static LocalizedString None
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "NONE",
				DefaultText = "None"
			};
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00060798 File Offset: 0x0005E998
	private static LocalizedString TimeOfDayEffectFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "TIME_IS_BETWEEN",
				DefaultText = "It's between {0} and {1}"
			};
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x000607C6 File Offset: 0x0005E9C6
	public static string TimeOfDayEffect(string _StartTime, string _EndTime)
	{
		return string.Format(LocalizedString.TimeOfDayEffectFormat, _StartTime, _EndTime);
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x000607DC File Offset: 0x0005E9DC
	public static LocalizedString CurrentStatus
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "CURRENT_STATUS",
				DefaultText = "Current Status"
			};
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0006080C File Offset: 0x0005EA0C
	public static LocalizedString StatInfluences
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "STAT_INFLUENCES",
				DefaultText = "Influenced by"
			};
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0006083C File Offset: 0x0005EA3C
	public static LocalizedString TransferToLocation
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "TRANSFER_TO_LOCATION",
				DefaultText = "Move Item"
			};
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0006086C File Offset: 0x0005EA6C
	private static LocalizedString MoveItemToLocationFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "MOVE_ITEM_TO_LOCATION",
				DefaultText = "Move {0} to {1}"
			};
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x0006089A File Offset: 0x0005EA9A
	public static string MoveItemToLocation(InGameCardBase _Item, CardData _ToLocation)
	{
		if (!_Item.CardModel || !_ToLocation)
		{
			return "";
		}
		return string.Format(LocalizedString.MoveItemToLocationFormat, _Item.CardName(true), _ToLocation.CardName);
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x000608D8 File Offset: 0x0005EAD8
	public static LocalizedString EmptyCharacterBio
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "NO_CHARACTER_BIO",
				DefaultText = "You don't remember anything before the island..."
			};
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x00060908 File Offset: 0x0005EB08
	public static LocalizedString BlueprintShop
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BLUEPRINT_SHOP",
				DefaultText = "Shop"
			};
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00060938 File Offset: 0x0005EB38
	public static LocalizedString BlueprintResearchTab
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BLUEPRINT_RESEARCH_TAB",
				DefaultText = "Research"
			};
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00060968 File Offset: 0x0005EB68
	public static LocalizedString UnavailableInDemo
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DEMO_UNAVAILABLE",
				DefaultText = "Full Game Only"
			};
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00060998 File Offset: 0x0005EB98
	private static LocalizedString BuyToContinueFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "BUY_TO_CONTINUE",
				DefaultText = "Congratulations on surviving for {0} days!"
			};
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x000609C8 File Offset: 0x0005EBC8
	public static string BuyToContinue
	{
		get
		{
			return string.Format(LocalizedString.BuyToContinueFormat, 7.ToString());
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x000609F0 File Offset: 0x0005EBF0
	public static LocalizedString DemoOver
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DEMO_OVER",
				DefaultText = "END OF FIRST DAYS"
			};
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00060A20 File Offset: 0x0005EC20
	public static LocalizedString HopeYouHadAGoodTime
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "HOPE_GOOD_TIME",
				DefaultText = "We hope you had a good time!"
			};
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00060A50 File Offset: 0x0005EC50
	private static LocalizedString GuidePlaceHolderTextFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "PLACEHOLDER_DISCLAIMER",
				DefaultText = "{0}\n\n<b>-The text above is a placeholder-</b>"
			};
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00060A80 File Offset: 0x0005EC80
	public static LocalizedString GuidePlaceHolderText(string _Text)
	{
		LocalizedString guidePlaceHolderTextFormat = LocalizedString.GuidePlaceHolderTextFormat;
		guidePlaceHolderTextFormat.DefaultText = string.Format(guidePlaceHolderTextFormat, _Text);
		guidePlaceHolderTextFormat.LocalizationKey = "IGNOREKEY";
		return guidePlaceHolderTextFormat;
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00060AB4 File Offset: 0x0005ECB4
	private static LocalizedString LiquidAutoFillingFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "LIQUID_AUTO_FILLING",
				DefaultText = "Filling with {0}"
			};
		}
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00060AE2 File Offset: 0x0005ECE2
	public static string LiquidAutoFilling(string _Liquid)
	{
		return string.Format(LocalizedString.LiquidAutoFillingFormat, _Liquid);
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00060AF4 File Offset: 0x0005ECF4
	public static LocalizedString DefaultEnemyName
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "DEFAULT_ENCOUNTER_ENEMY_NAME",
				DefaultText = "Enemy"
			};
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00060B24 File Offset: 0x0005ED24
	private static LocalizedString PlayerAttackButFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "ENCOUNTER_BUT",
				DefaultText = "... but {0}"
			};
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00060B54 File Offset: 0x0005ED54
	private static LocalizedString PlayerAttackAndFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "ENCOUNTER_AND",
				DefaultText = "... and {0}"
			};
		}
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00060B84 File Offset: 0x0005ED84
	public static EncounterLogMessage PlayerAttackSuccessLog(bool _EnemySuccess, EncounterLogMessage _SuccessLog)
	{
		if (string.IsNullOrEmpty(_SuccessLog))
		{
			return default(EncounterLogMessage);
		}
		if (_EnemySuccess)
		{
			return EncounterLogMessage.DuplicateWithText(string.Format(LocalizedString.PlayerAttackButFormat, _SuccessLog.ToString()), _SuccessLog);
		}
		return EncounterLogMessage.DuplicateWithText(string.Format(LocalizedString.PlayerAttackAndFormat, _SuccessLog.ToString()), _SuccessLog);
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00060BF0 File Offset: 0x0005EDF0
	public static EncounterLogMessage PlayerAttackFailureLog(bool _EnemySuccess, EncounterLogMessage _FailureLog)
	{
		if (string.IsNullOrEmpty(_FailureLog))
		{
			return default(EncounterLogMessage);
		}
		if (_EnemySuccess)
		{
			return EncounterLogMessage.DuplicateWithText(string.Format(LocalizedString.PlayerAttackAndFormat, _FailureLog.ToString()), _FailureLog);
		}
		return EncounterLogMessage.DuplicateWithText(string.Format(LocalizedString.PlayerAttackButFormat, _FailureLog.ToString()), _FailureLog);
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00060C5C File Offset: 0x0005EE5C
	private static LocalizedString DefaultEnemyGettingCloseLogFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "ENCOUNTER_ENEMY_GETTING_CLOSE",
				DefaultText = "{0} is approaching you."
			};
		}
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00060C8A File Offset: 0x0005EE8A
	public static EncounterLogMessage DefaultEnemyGettingCloseLog(string _EnemyName)
	{
		return new EncounterLogMessage(string.Format(LocalizedString.DefaultEnemyGettingCloseLogFormat, _EnemyName));
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000B5A RID: 2906 RVA: 0x00060CA4 File Offset: 0x0005EEA4
	private static LocalizedString WeaponAndProjectileActionFormat
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "ENCOUNTER_WEAPON_AND_PROJECTILE",
				DefaultText = "{0} and {1}"
			};
		}
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00060CD2 File Offset: 0x0005EED2
	public static string WeaponAndProjectileName(string _WeaponName, string _ProjName)
	{
		return string.Format(LocalizedString.WeaponAndProjectileActionFormat, _WeaponName, _ProjName);
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00060CE8 File Offset: 0x0005EEE8
	public static LocalizedString Wounded
	{
		get
		{
			return new LocalizedString
			{
				LocalizationKey = "WOUNDED",
				DefaultText = "YOU GOT WOUNDED!"
			};
		}
	}

	// Token: 0x04001058 RID: 4184
	public string ParentObjectID;

	// Token: 0x04001059 RID: 4185
	public string LocalizationKey;

	// Token: 0x0400105A RID: 4186
	[TextArea(0, 10)]
	public string DefaultText;

	// Token: 0x0400105B RID: 4187
	[NonSerialized]
	private string LocalizedText;

	// Token: 0x0400105C RID: 4188
	public const string IgnoreKey = "IGNOREKEY";

	// Token: 0x0400105D RID: 4189
	public const string DefaultEnemyGettingCloseKey = "ENCOUNTER_ENEMY_GETTING_CLOSE";
}
