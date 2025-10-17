using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000015 RID: 21
[Serializable]
public class CardSaveData
{
	// Token: 0x060001E8 RID: 488 RVA: 0x00013FFC File Offset: 0x000121FC
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

	// Token: 0x060001E9 RID: 489 RVA: 0x000140AC File Offset: 0x000122AC
	public virtual void SaveCard(InGameCardBase _Card)
	{
		this.CardID = UniqueIDScriptable.SaveID(_Card.CardModel);
		this.CustomName = _Card.CustomName;
		this.EnvironmentID = UniqueIDScriptable.SaveID(_Card.Environment);
		this.PrevEnvironmentID = UniqueIDScriptable.SaveID(_Card.PrevEnvironment);
		this.PrevEnvTravelIndex = _Card.PrevEnvTravelIndex;
		this.CreatedOnTick = _Card.CreatedOnTick;
		this.CreatedInSaveDataTick = _Card.CreatedInSaveDataTick;
		this.IsTravelCard = _Card.CardModel.IsTravellingCard;
		this.TravelCardIndex = _Card.TravelCardIndex;
		this.IgnoreBaseRow = _Card.IgnoreBaseRow;
		if (_Card.gameObject.activeInHierarchy)
		{
			this.SlotInformation = (_Card.CurrentSlot ? _Card.CurrentSlot.ToInfo() : new SlotInfo(_Card.CurrentSlotInfo.SlotType, _Card.CurrentSlotInfo.SlotIndex));
		}
		else
		{
			this.SlotInformation = new SlotInfo(_Card.CurrentSlotInfo.SlotType, _Card.CurrentSlotInfo.SlotIndex);
		}
		this.IsPinned = _Card.IsPinned;
		this.Spoilage = _Card.CurrentSpoilage;
		this.Usage = _Card.CurrentUsageDurability;
		this.Fuel = _Card.CurrentFuel;
		this.ConsumableCharges = _Card.CurrentProgress;
		this.LiquidQuantity = _Card.CurrentLiquidQuantity;
		this.Special1 = _Card.CurrentSpecial1;
		this.Special2 = _Card.CurrentSpecial2;
		this.Special3 = _Card.CurrentSpecial3;
		this.Special4 = _Card.CurrentSpecial4;
		foreach (KeyValuePair<string, Vector2Int> keyValuePair in _Card.DroppedCollections)
		{
			this.CollectionUses.Add(new CollectionDropsSaveData(keyValuePair.Key, keyValuePair.Value));
		}
		if (_Card.StatTriggeredActions != null)
		{
			this.StatTriggeredActions.AddRange(_Card.StatTriggeredActions);
		}
		if (_Card.ExplorationData != null && _Card.CardModel.CardType == CardTypes.Explorable)
		{
			this.ExplorationData = new ExplorationSaveData(_Card.ExplorationData);
		}
		if (_Card.BlueprintData != null)
		{
			this.BlueprintData = new BlueprintSaveData(_Card.BlueprintData, _Card.CardModel.BlueprintStages, _Card.Environment, _Card.CardModel.CardType != CardTypes.EnvImprovement);
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00014304 File Offset: 0x00012504
	public virtual void SaveCardModel(CardData _Card, CardData _Environment, SlotInfo _Slot)
	{
		if (!_Card)
		{
			return;
		}
		this.CardID = UniqueIDScriptable.SaveID(_Card);
		this.EnvironmentID = UniqueIDScriptable.SaveID(_Environment);
		this.CreatedOnTick = (MBSingleton<GameManager>.Instance ? MBSingleton<GameManager>.Instance.CurrentTickInfo.z : 0);
		this.NotYetCreated = true;
		if (_Slot != null)
		{
			this.SlotInformation = new SlotInfo(_Slot.SlotType, _Slot.SlotIndex);
		}
		else
		{
			this.SlotInformation = (MBSingleton<GraphicsManager>.Instance ? new SlotInfo(MBSingleton<GraphicsManager>.Instance.CardToSlotType(_Card.CardType, false), -2) : new SlotInfo(SlotsTypes.Base, -2));
		}
		this.IsPinned = false;
		this.IsTravelCard = _Card.IsTravellingCard;
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

	// Token: 0x040001C9 RID: 457
	public string CardID;

	// Token: 0x040001CA RID: 458
	public string CustomName;

	// Token: 0x040001CB RID: 459
	public string EnvironmentID;

	// Token: 0x040001CC RID: 460
	public string PrevEnvironmentID;

	// Token: 0x040001CD RID: 461
	public int PrevEnvTravelIndex;

	// Token: 0x040001CE RID: 462
	public bool IgnoreBaseRow;

	// Token: 0x040001CF RID: 463
	public SlotInfo SlotInformation;

	// Token: 0x040001D0 RID: 464
	public int CreatedOnTick;

	// Token: 0x040001D1 RID: 465
	public int CreatedInSaveDataTick;

	// Token: 0x040001D2 RID: 466
	public bool IsPinned;

	// Token: 0x040001D3 RID: 467
	public bool NotYetCreated;

	// Token: 0x040001D4 RID: 468
	public bool IsTravelCard;

	// Token: 0x040001D5 RID: 469
	public int TravelCardIndex;

	// Token: 0x040001D6 RID: 470
	public float Spoilage;

	// Token: 0x040001D7 RID: 471
	public float Usage;

	// Token: 0x040001D8 RID: 472
	public float Fuel;

	// Token: 0x040001D9 RID: 473
	public float ConsumableCharges;

	// Token: 0x040001DA RID: 474
	public float LiquidQuantity;

	// Token: 0x040001DB RID: 475
	public float Special1;

	// Token: 0x040001DC RID: 476
	public float Special2;

	// Token: 0x040001DD RID: 477
	public float Special3;

	// Token: 0x040001DE RID: 478
	public float Special4;

	// Token: 0x040001DF RID: 479
	public ExplorationSaveData ExplorationData;

	// Token: 0x040001E0 RID: 480
	public BlueprintSaveData BlueprintData;

	// Token: 0x040001E1 RID: 481
	public List<CollectionDropsSaveData> CollectionUses = new List<CollectionDropsSaveData>();

	// Token: 0x040001E2 RID: 482
	public List<StatTriggeredActionStatus> StatTriggeredActions = new List<StatTriggeredActionStatus>();
}
