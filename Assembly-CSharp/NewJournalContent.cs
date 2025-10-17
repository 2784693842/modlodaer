using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class NewJournalContent : MonoBehaviour
{
	// Token: 0x0600064A RID: 1610 RVA: 0x00041F12 File Offset: 0x00040112
	private void Start()
	{
		this.GM = MBSingleton<GameManager>.Instance;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00041F20 File Offset: 0x00040120
	private void Update()
	{
		if (!this.GM.GetJournal)
		{
			GraphicsManager.SetActiveGroup(this.MainNewGroup, false);
			GraphicsManager.SetActiveGroup(this.SecondaryNewGroup, false);
			GraphicsManager.SetActiveGroup(this.UpdatedGroup, false);
			return;
		}
		if (this.MyTransform && this.OtherButtonTransform && !this.WasHighlighted && (this.GM.GetJournal.HasUpdatedContent || this.GM.GetJournal.HasNewContent))
		{
			this.MyTransform.SetSiblingIndex(this.OtherButtonTransform.GetSiblingIndex());
		}
		GraphicsManager.SetActiveGroup(this.MainNewGroup, this.GM.GetJournal.HasNewContent);
		GraphicsManager.SetActiveGroup(this.SecondaryNewGroup, this.GM.GetJournal.HasNewContent && !this.GM.GetJournal.HasUpdatedContent);
		GraphicsManager.SetActiveGroup(this.UpdatedGroup, this.GM.GetJournal.HasUpdatedContent);
		this.WasHighlighted = (this.GM.GetJournal.HasUpdatedContent || this.GM.GetJournal.HasNewContent);
	}

	// Token: 0x0400089B RID: 2203
	[SerializeField]
	private Transform MyTransform;

	// Token: 0x0400089C RID: 2204
	[SerializeField]
	private Transform OtherButtonTransform;

	// Token: 0x0400089D RID: 2205
	[SerializeField]
	private GameObject[] MainNewGroup;

	// Token: 0x0400089E RID: 2206
	[SerializeField]
	private GameObject[] SecondaryNewGroup;

	// Token: 0x0400089F RID: 2207
	[SerializeField]
	private GameObject[] UpdatedGroup;

	// Token: 0x040008A0 RID: 2208
	private GameManager GM;

	// Token: 0x040008A1 RID: 2209
	private bool WasHighlighted;
}
