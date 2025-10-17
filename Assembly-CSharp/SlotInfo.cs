using System;

// Token: 0x02000002 RID: 2
[Serializable]
public class SlotInfo
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public SlotInfo(SlotsTypes _Type, int _Index = 0)
	{
		this.SlotType = _Type;
		this.SlotIndex = _Index;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002066 File Offset: 0x00000266
	public SlotInfo(SlotInfo _From)
	{
		if (_From == null)
		{
			this.SlotType = SlotsTypes.Base;
			this.SlotIndex = -2;
			return;
		}
		this.SlotType = _From.SlotType;
		this.SlotIndex = _From.SlotIndex;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002099 File Offset: 0x00000299
	public override string ToString()
	{
		return string.Format("SlotInfo({0}, {1})", this.SlotType.ToString(), this.SlotIndex.ToString());
	}

	// Token: 0x04000001 RID: 1
	public SlotsTypes SlotType;

	// Token: 0x04000002 RID: 2
	public int SlotIndex;
}
