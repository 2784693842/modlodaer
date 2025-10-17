using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class NewBlueprintContent : MonoBehaviour
{
	// Token: 0x0600063F RID: 1599 RVA: 0x00041CDD File Offset: 0x0003FEDD
	private void Start()
	{
		this.GM = MBSingleton<GameManager>.Instance;
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00041CEC File Offset: 0x0003FEEC
	private void Update()
	{
		if (!this.AssociatedCard)
		{
			if (this.MyTransform && this.OtherButtonTransform && !this.WasHighlighted && this.GM.NewBlueprints)
			{
				this.MyTransform.SetSiblingIndex(this.OtherButtonTransform.GetSiblingIndex());
			}
			GraphicsManager.SetActiveGroup(this.NewGroup, this.GM.NewBlueprints);
			this.WasHighlighted = this.GM.NewBlueprints;
			return;
		}
		bool flag = this.AssociatedCard.CardLogic;
		if (flag)
		{
			flag = !this.GM.CheckedBlueprints.Contains(this.AssociatedCard.CardLogic.CardModel);
		}
		if (this.MyTransform && this.OtherButtonTransform && (!this.WasHighlighted && flag))
		{
			this.MyTransform.SetSiblingIndex(this.OtherButtonTransform.GetSiblingIndex());
		}
		GraphicsManager.SetActiveGroup(this.NewGroup, flag);
		this.WasHighlighted = flag;
	}

	// Token: 0x0400088E RID: 2190
	[SerializeField]
	private Transform MyTransform;

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private Transform OtherButtonTransform;

	// Token: 0x04000890 RID: 2192
	[SerializeField]
	private GameObject[] NewGroup;

	// Token: 0x04000891 RID: 2193
	[SerializeField]
	private CardGraphics AssociatedCard;

	// Token: 0x04000892 RID: 2194
	private GameManager GM;

	// Token: 0x04000893 RID: 2195
	private bool WasHighlighted;
}
