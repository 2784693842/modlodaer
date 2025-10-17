using System;

// Token: 0x02000018 RID: 24
public class InGameRefCardSaveData : CardSaveData
{
	// Token: 0x060001F1 RID: 497 RVA: 0x000149A8 File Offset: 0x00012BA8
	public override void SaveCard(InGameCardBase _Card)
	{
		this.CardID = UniqueIDScriptable.SaveID(_Card.CardModel);
		this.EnvironmentID = UniqueIDScriptable.SaveID(_Card.Environment);
		this.PrevEnvironmentID = UniqueIDScriptable.SaveID(_Card.PrevEnvironment);
		this.PrevEnvTravelIndex = _Card.PrevEnvTravelIndex;
		if (_Card.gameObject.activeInHierarchy)
		{
			this.SlotInformation = ((_Card.CurrentSlot != null) ? _Card.CurrentSlot.ToInfo() : new SlotInfo(_Card.CurrentSlotInfo.SlotType, _Card.CurrentSlotInfo.SlotIndex));
		}
		else
		{
			this.SlotInformation = new SlotInfo(_Card.CurrentSlotInfo.SlotType, _Card.CurrentSlotInfo.SlotIndex);
		}
		this.ReferencedCard = _Card;
	}

	// Token: 0x040001EB RID: 491
	public InGameCardBase ReferencedCard;
}
