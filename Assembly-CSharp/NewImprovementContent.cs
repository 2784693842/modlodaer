using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class NewImprovementContent : MonoBehaviour
{
	// Token: 0x06000644 RID: 1604 RVA: 0x00041E33 File Offset: 0x00040033
	private void Start()
	{
		this.GM = MBSingleton<GameManager>.Instance;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00041E40 File Offset: 0x00040040
	private void Update()
	{
		if (!this.AssociatedCard)
		{
			GraphicsManager.SetActiveGroup(this.NewGroup, this.GM.NewImprovements);
			return;
		}
		bool flag = this.AssociatedCard.CardLogic;
		if (flag)
		{
			flag = !this.GM.CurrentEnvData(true).ImprovementWasChecked(this.AssociatedCard.CardLogic.CardModel);
		}
		GraphicsManager.SetActiveGroup(this.NewGroup, flag);
	}

	// Token: 0x04000896 RID: 2198
	[SerializeField]
	private GameObject[] NewGroup;

	// Token: 0x04000897 RID: 2199
	[SerializeField]
	private CardGraphics AssociatedCard;

	// Token: 0x04000898 RID: 2200
	private GameManager GM;
}
