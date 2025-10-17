using System;
using UnityEngine;

// Token: 0x0200003A RID: 58
[Serializable]
public class TutorialStep
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000299 RID: 665 RVA: 0x000194A1 File Offset: 0x000176A1
	// (set) Token: 0x0600029A RID: 666 RVA: 0x000194A9 File Offset: 0x000176A9
	public bool Active { get; private set; }

	// Token: 0x0600029B RID: 667 RVA: 0x000194B4 File Offset: 0x000176B4
	public void UpdateState()
	{
		if (this.DisappearOnJournalOpened && MBSingleton<GameManager>.Instance.OpenedJournal)
		{
			this.Active = false;
			return;
		}
		bool flag = this.DisappearConditions != null;
		flag &= (this.DisappearConditions.Length != 0);
		if (flag)
		{
			for (int i = 0; i < this.DisappearConditions.Length; i++)
			{
				flag &= this.DisappearConditions[i].Complete;
				if (!flag)
				{
					break;
				}
			}
		}
		if (flag)
		{
			this.Active = false;
			return;
		}
		if (this.AppearConditions == null)
		{
			this.Active = true;
			return;
		}
		if (this.AppearConditions.Length == 0)
		{
			this.Active = true;
			return;
		}
		bool flag2 = true;
		for (int j = 0; j < this.AppearConditions.Length; j++)
		{
			flag2 &= this.AppearConditions[j].Complete;
			if (!flag2)
			{
				break;
			}
		}
		this.Active = flag2;
	}

	// Token: 0x040002E4 RID: 740
	[SerializeField]
	private string StepName;

	// Token: 0x040002E5 RID: 741
	[SerializeField]
	private Objective[] AppearConditions;

	// Token: 0x040002E6 RID: 742
	[SerializeField]
	private Objective[] DisappearConditions;

	// Token: 0x040002E7 RID: 743
	[SerializeField]
	private bool DisappearOnJournalOpened;

	// Token: 0x040002E8 RID: 744
	public ActionHighlight[] ActionsHighlighted;

	// Token: 0x040002E9 RID: 745
	public GameObject[] ActiveObjects;
}
