using System;

// Token: 0x0200001D RID: 29
public static class CardComparer
{
	// Token: 0x06000204 RID: 516 RVA: 0x00014F5F File Offset: 0x0001315F
	public static int Compare(InGameCardBase _A, InGameCardBase _B)
	{
		return CardComparer.Compare(new CardComparer.RelevantCardData(_A), new CardComparer.RelevantCardData(_B));
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00014F72 File Offset: 0x00013172
	public static int Compare(CardSaveData _A, CardSaveData _B)
	{
		return CardComparer.Compare(new CardComparer.RelevantCardData(_A), new CardComparer.RelevantCardData(_B));
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00014F88 File Offset: 0x00013188
	private static int Compare(CardComparer.RelevantCardData _A, CardComparer.RelevantCardData _B)
	{
		if (_A.EnvironmentHashCode != _B.EnvironmentHashCode)
		{
			return _A.EnvironmentHashCode.CompareTo(_B.EnvironmentHashCode);
		}
		if (_A.SlotType != _B.SlotType)
		{
			return _A.SlotType.CompareTo(_B.SlotType);
		}
		if (_A.SlotIndex != _B.SlotIndex)
		{
			return _A.SlotIndex.CompareTo(_B.SlotIndex);
		}
		return _A.Tick.CompareTo(_B.Tick);
	}

	// Token: 0x02000242 RID: 578
	private struct RelevantCardData
	{
		// Token: 0x06000F03 RID: 3843 RVA: 0x0007DF40 File Offset: 0x0007C140
		public RelevantCardData(InGameCardBase _FromCard)
		{
			this.Valid = (_FromCard != null);
			if (!this.Valid)
			{
				this.SlotType = -1;
				this.SlotIndex = -1;
				this.Tick = -1;
				this.EnvironmentHashCode = -1;
				return;
			}
			if (_FromCard.CurrentSlotInfo == null)
			{
				_FromCard.CurrentSlotInfo = new SlotInfo(SlotsTypes.Base, int.MaxValue);
			}
			this.SlotType = (int)_FromCard.CurrentSlotInfo.SlotType;
			this.SlotIndex = _FromCard.CurrentSlotInfo.SlotIndex;
			this.Tick = _FromCard.CreatedOnTick;
			if (_FromCard.Environment)
			{
				this.EnvironmentHashCode = _FromCard.Environment.GetHashCode();
				return;
			}
			this.EnvironmentHashCode = -1;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0007DFEC File Offset: 0x0007C1EC
		public RelevantCardData(CardSaveData _FromSaveData)
		{
			this.Valid = (_FromSaveData != null);
			if (!this.Valid)
			{
				this.SlotType = -1;
				this.SlotIndex = -1;
				this.Tick = -1;
				this.EnvironmentHashCode = -1;
				return;
			}
			UniqueIDScriptable fromID = UniqueIDScriptable.GetFromID(_FromSaveData.EnvironmentID);
			this.SlotType = (int)_FromSaveData.SlotInformation.SlotType;
			this.SlotIndex = _FromSaveData.SlotInformation.SlotIndex;
			this.Tick = _FromSaveData.CreatedOnTick;
			if (fromID)
			{
				this.EnvironmentHashCode = fromID.GetHashCode();
				return;
			}
			this.EnvironmentHashCode = -1;
		}

		// Token: 0x040013DC RID: 5084
		public bool Valid;

		// Token: 0x040013DD RID: 5085
		public int SlotType;

		// Token: 0x040013DE RID: 5086
		public int SlotIndex;

		// Token: 0x040013DF RID: 5087
		public int Tick;

		// Token: 0x040013E0 RID: 5088
		public int EnvironmentHashCode;
	}
}
