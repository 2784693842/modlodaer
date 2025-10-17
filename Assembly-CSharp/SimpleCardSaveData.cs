using System;

// Token: 0x02000014 RID: 20
[Serializable]
public class SimpleCardSaveData
{
	// Token: 0x060001E3 RID: 483 RVA: 0x00013C24 File Offset: 0x00011E24
	public virtual void SaveCard(InGameCardBase _Card)
	{
		this.CardID = UniqueIDScriptable.SaveID(_Card.CardModel);
		this.EnvironmentID = UniqueIDScriptable.SaveID(_Card.Environment);
		this.PrevEnvironmentID = UniqueIDScriptable.SaveID(_Card.PrevEnvironment);
		this.PrevEnvTravelIndex = _Card.PrevEnvTravelIndex;
		this.CreatedOnTick = _Card.CreatedOnTick;
		if (_Card.gameObject.activeInHierarchy)
		{
			this.SlotInformation = ((_Card.CurrentSlot != null) ? _Card.CurrentSlot.ToInfo() : new SlotInfo(_Card.CurrentSlotInfo.SlotType, _Card.CurrentSlotInfo.SlotIndex));
		}
		else
		{
			this.SlotInformation = new SlotInfo(_Card.CurrentSlotInfo.SlotType, _Card.CurrentSlotInfo.SlotIndex);
		}
		this.Spoilage = _Card.CurrentSpoilage;
		this.Usage = _Card.CurrentUsageDurability;
		this.Fuel = _Card.CurrentFuel;
		this.ConsumableCharges = _Card.CurrentProgress;
		this.LiquidQuantity = _Card.CurrentLiquidQuantity;
		this.Special1 = _Card.CurrentSpecial1;
		this.Special2 = _Card.CurrentSpecial2;
		this.Special3 = _Card.CurrentSpecial3;
		this.Special4 = _Card.CurrentSpecial4;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00013D50 File Offset: 0x00011F50
	public virtual void SaveCardModel(CardData _Card, CardData _Environment, SlotInfo _Slot)
	{
		if (!_Card)
		{
			return;
		}
		this.CardID = UniqueIDScriptable.SaveID(_Card);
		this.EnvironmentID = UniqueIDScriptable.SaveID(_Environment);
		this.CreatedOnTick = (MBSingleton<GameManager>.Instance ? MBSingleton<GameManager>.Instance.CurrentTickInfo.z : 0);
		if (_Slot != null)
		{
			this.SlotInformation = new SlotInfo(_Slot.SlotType, _Slot.SlotIndex);
		}
		else
		{
			this.SlotInformation = (MBSingleton<GraphicsManager>.Instance ? new SlotInfo(MBSingleton<GraphicsManager>.Instance.CardToSlotType(_Card.CardType, false), -2) : new SlotInfo(SlotsTypes.Base, -2));
		}
		this.Spoilage = _Card.SpoilageTime;
		this.Usage = _Card.UsageDurability;
		this.Fuel = _Card.FuelCapacity;
		this.ConsumableCharges = _Card.Progress;
		this.LiquidQuantity = 0f;
		this.Special1 = _Card.SpecialDurability1;
		this.Special2 = _Card.SpecialDurability2;
		this.Special3 = _Card.SpecialDurability3;
		this.Special4 = _Card.SpecialDurability4;
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x00013E88 File Offset: 0x00012088
	public CardSaveData ToFullSaveData
	{
		get
		{
			return new CardSaveData
			{
				CardID = this.CardID,
				EnvironmentID = this.EnvironmentID,
				SlotInformation = this.SlotInformation,
				CreatedOnTick = this.CreatedOnTick,
				CreatedInSaveDataTick = this.CreatedInSaveDataTick,
				Spoilage = this.Spoilage,
				Usage = this.Usage,
				Fuel = this.Fuel,
				ConsumableCharges = this.ConsumableCharges,
				LiquidQuantity = this.LiquidQuantity,
				Special1 = this.Special1,
				Special2 = this.Special2,
				Special3 = this.Special3,
				Special4 = this.Special4
			};
		}
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00013F44 File Offset: 0x00012144
	public TransferedDurabilities GetDurabilities()
	{
		return new TransferedDurabilities
		{
			Spoilage = new OptionalFloatValue(true, this.Spoilage),
			Usage = new OptionalFloatValue(true, this.Usage),
			Fuel = new OptionalFloatValue(true, this.Fuel),
			ConsumableCharges = new OptionalFloatValue(true, this.ConsumableCharges),
			Liquid = this.LiquidQuantity,
			Special1 = new OptionalFloatValue(true, this.Special1),
			Special2 = new OptionalFloatValue(true, this.Special2),
			Special3 = new OptionalFloatValue(true, this.Special3),
			Special4 = new OptionalFloatValue(true, this.Special4)
		};
	}

	// Token: 0x040001B9 RID: 441
	public string CardID;

	// Token: 0x040001BA RID: 442
	public string EnvironmentID;

	// Token: 0x040001BB RID: 443
	public string PrevEnvironmentID;

	// Token: 0x040001BC RID: 444
	public int PrevEnvTravelIndex;

	// Token: 0x040001BD RID: 445
	public SlotInfo SlotInformation;

	// Token: 0x040001BE RID: 446
	public int CreatedOnTick;

	// Token: 0x040001BF RID: 447
	public int CreatedInSaveDataTick;

	// Token: 0x040001C0 RID: 448
	public float Spoilage;

	// Token: 0x040001C1 RID: 449
	public float Usage;

	// Token: 0x040001C2 RID: 450
	public float Fuel;

	// Token: 0x040001C3 RID: 451
	public float ConsumableCharges;

	// Token: 0x040001C4 RID: 452
	public float LiquidQuantity;

	// Token: 0x040001C5 RID: 453
	public float Special1;

	// Token: 0x040001C6 RID: 454
	public float Special2;

	// Token: 0x040001C7 RID: 455
	public float Special3;

	// Token: 0x040001C8 RID: 456
	public float Special4;
}
