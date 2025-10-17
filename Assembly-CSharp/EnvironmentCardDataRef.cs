using System;

// Token: 0x0200016A RID: 362
[Serializable]
public class EnvironmentCardDataRef : CardDataRef
{
	// Token: 0x04000F6F RID: 3951
	[NonSerialized]
	public CardData PrevEnv;

	// Token: 0x04000F70 RID: 3952
	[NonSerialized]
	public int TravelIndex;
}
