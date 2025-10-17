using System;

// Token: 0x020000F2 RID: 242
[Serializable]
public struct RemotePassiveEffect
{
	// Token: 0x06000828 RID: 2088 RVA: 0x000502BC File Offset: 0x0004E4BC
	public bool AppliesToCard(InGameCardBase _Card)
	{
		if (this.AppliesTo == null)
		{
			return false;
		}
		if (this.AppliesTo.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < this.AppliesTo.Length; i++)
		{
			if (this.AppliesTo[i].CheckCard(_Card))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000C48 RID: 3144
	public CardOrTagRef[] AppliesTo;

	// Token: 0x04000C49 RID: 3145
	public PassiveEffect Effect;
}
