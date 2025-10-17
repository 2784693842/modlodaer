using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000E RID: 14
public struct CardCreationSettings
{
	// Token: 0x0400010D RID: 269
	public CardData Data;

	// Token: 0x0400010E RID: 270
	public SlotInfo Slot;

	// Token: 0x0400010F RID: 271
	public CardData FromEnv;

	// Token: 0x04000110 RID: 272
	public InGameCardBase Container;

	// Token: 0x04000111 RID: 273
	public bool InCurrentEnv;

	// Token: 0x04000112 RID: 274
	public TransferedDurabilities Durabilities;

	// Token: 0x04000113 RID: 275
	public List<CollectionDropsSaveData> Drops;

	// Token: 0x04000114 RID: 276
	public List<StatTriggeredActionStatus> StatTriggeredActions;

	// Token: 0x04000115 RID: 277
	public ExplorationSaveData Exploration;

	// Token: 0x04000116 RID: 278
	public BlueprintSaveData Blueprint;

	// Token: 0x04000117 RID: 279
	public Vector3 FromPosition;

	// Token: 0x04000118 RID: 280
	public bool UseDefaultInventory;

	// Token: 0x04000119 RID: 281
	public SpawningLiquid WithLiquid;

	// Token: 0x0400011A RID: 282
	public bool MoveView;

	// Token: 0x0400011B RID: 283
	public bool Pinned;

	// Token: 0x0400011C RID: 284
	public int Tick;

	// Token: 0x0400011D RID: 285
	public CardData PrevEnv;

	// Token: 0x0400011E RID: 286
	public int TravelCardIndex;

	// Token: 0x0400011F RID: 287
	public int FromTravelIndex;

	// Token: 0x04000120 RID: 288
	public string Name;
}
