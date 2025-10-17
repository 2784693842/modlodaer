using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
[Serializable]
public class DismantleCardAction : CardAction
{
	// Token: 0x060007BA RID: 1978 RVA: 0x0004CC30 File Offset: 0x0004AE30
	public bool CanAppear(InGameCardBase _Card, bool _CountTimeAsEffect, bool _Inventory)
	{
		if (this.AlwaysShow)
		{
			return this.ShowWithInventory(_Inventory);
		}
		return base.CardsAndTagsAreCorrect(_Card, null, null) && this.WillHaveAnEffect(_Card, false, false, false, _CountTimeAsEffect, Array.Empty<CardModifications>()) && this.ShowWithInventory(_Inventory) && this.RequiredReceivingLiquidContent.IsValid(_Card, false);
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0004CC82 File Offset: 0x0004AE82
	private bool ShowWithInventory(bool _Inventory)
	{
		return this.VisibilityWithInventory == DismantleActionInventoryVisibility.IgnoreInventory || (_Inventory && this.VisibilityWithInventory == DismantleActionInventoryVisibility.ShowIfVisibleInventory) || (!_Inventory && this.VisibilityWithInventory == DismantleActionInventoryVisibility.ShowIfNOTVisibleInventory);
	}

	// Token: 0x04000B60 RID: 2912
	public Vector2Int MinMaxExplorationDrops;

	// Token: 0x04000B61 RID: 2913
	public float ExplorationValue;

	// Token: 0x04000B62 RID: 2914
	public bool PerformUponInspection;

	// Token: 0x04000B63 RID: 2915
	public bool DontCloseInspectionWindow;

	// Token: 0x04000B64 RID: 2916
	public bool AlwaysShow;

	// Token: 0x04000B65 RID: 2917
	public DismantleActionInventoryVisibility VisibilityWithInventory;
}
