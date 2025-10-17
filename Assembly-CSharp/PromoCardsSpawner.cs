using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
public class PromoCardsSpawner : MonoBehaviour
{
	// Token: 0x04001078 RID: 4216
	public RectTransform Parent;

	// Token: 0x04001079 RID: 4217
	public InGameCardBase PromoArtCardPrefab;

	// Token: 0x0400107A RID: 4218
	public bool Shuffle;

	// Token: 0x0400107B RID: 4219
	public CardData[] SpawnedCards;
}
