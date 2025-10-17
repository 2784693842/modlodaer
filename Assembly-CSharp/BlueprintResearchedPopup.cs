using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class BlueprintResearchedPopup : MonoBehaviour
{
	// Token: 0x060002F5 RID: 757 RVA: 0x0001E8F4 File Offset: 0x0001CAF4
	public void Show(CardData _Card)
	{
		this.BlueprintCard = _Card;
		this.CardPreview.Setup(this.BlueprintCard, false);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0001E91C File Offset: 0x0001CB1C
	public void OpenBlueprintModels()
	{
		GraphicsManager instance = MBSingleton<GraphicsManager>.Instance;
		if (!instance)
		{
			return;
		}
		instance.BlueprintModelsPopup.Show();
		instance.BlueprintModelsPopup.ShowBlueprint(this.BlueprintCard, false);
		this.Hide();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0001E95B File Offset: 0x0001CB5B
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000386 RID: 902
	[SerializeField]
	private MenuCardPreview CardPreview;

	// Token: 0x04000387 RID: 903
	private CardData BlueprintCard;
}
