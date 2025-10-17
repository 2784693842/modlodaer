using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class DetailedStatList : MonoBehaviour
{
	// Token: 0x060003C6 RID: 966 RVA: 0x000274A4 File Offset: 0x000256A4
	private void Awake()
	{
		if (this.GM)
		{
			return;
		}
		this.GM = MBSingleton<GameManager>.Instance;
		for (int i = 0; i < this.GM.AllStats.Count; i++)
		{
			if (this.GM.AllStats[i].StatModel && this.GM.AllStats[i].StatModel.ShowInList)
			{
				this.AllElements.Add(UnityEngine.Object.Instantiate<DetailedStatListElement>(this.ElementPrefab, this.ListParent));
				this.AllElements[this.AllElements.Count - 1].Setup(this.GM.AllStats[i]);
			}
		}
		if (this.SortUsingStatPriority)
		{
			this.AllElements.Sort((DetailedStatListElement a, DetailedStatListElement b) => b.StatModel.StatPriority.CompareTo(a.StatModel.StatPriority));
		}
		for (int j = 0; j < this.Tabs.Length; j++)
		{
			this.TabButtons.Add(UnityEngine.Object.Instantiate<IndexButton>(this.TabButtonPrefab, this.TabsParent));
			this.TabButtons[j].Setup(j, "", this.Tabs[j].TabName, false);
			this.TabButtons[j].Sprite = this.Tabs[j].Icon;
			IndexButton indexButton = this.TabButtons[j];
			indexButton.OnClicked = (Action<int>)Delegate.Combine(indexButton.OnClicked, new Action<int>(this.OpenTab));
		}
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0002764B File Offset: 0x0002584B
	public void Show()
	{
		this.Show(this.CurrentTab);
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0002765C File Offset: 0x0002585C
	public void Show(int _AtIndex)
	{
		if (!this.GM)
		{
			this.Awake();
		}
		else
		{
			for (int i = 0; i < this.Tabs.Length; i++)
			{
				this.TabButtons[i].Setup(i, "", this.Tabs[i].TabName, false);
			}
		}
		if (_AtIndex == -1)
		{
			this.OpenTab(0);
		}
		else
		{
			this.OpenTab(_AtIndex);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x000276DC File Offset: 0x000258DC
	public void OpenTab(int _Index)
	{
		bool flag = this.CurrentTab != _Index;
		this.CurrentTab = _Index;
		if (flag)
		{
			this.SortList();
			if (this.Tabs.Length != 0)
			{
				for (int i = 0; i < this.TabButtons.Count; i++)
				{
					this.TabButtons[i].Selected = (i == this.CurrentTab);
				}
			}
		}
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00027740 File Offset: 0x00025940
	private void SortList()
	{
		this.ActiveElements.Clear();
		for (int i = 0; i < this.AllElements.Count; i++)
		{
			if (this.Tabs.Length != 0 && !this.Tabs[this.CurrentTab].ContainedStats.Contains(this.AllElements[i].StatModel))
			{
				this.AllElements[i].gameObject.SetActive(false);
			}
			else if (!this.SortUsingStatPriority)
			{
				this.ActiveElements.Add(this.AllElements[i]);
			}
			else
			{
				this.AllElements[i].gameObject.SetActive(true);
			}
		}
		if (!this.SortUsingStatPriority && this.ActiveElements.Count > 0)
		{
			for (int j = 0; j < this.Tabs[this.CurrentTab].ContainedStats.Count; j++)
			{
				for (int k = 0; k < this.ActiveElements.Count; k++)
				{
					if (this.Tabs[this.CurrentTab].ContainedStats[j] == this.ActiveElements[k].StatModel)
					{
						this.ActiveElements[k].transform.SetAsLastSibling();
						break;
					}
				}
			}
			for (int l = 0; l < this.ActiveElements.Count; l++)
			{
				this.ActiveElements[l].gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x040004D6 RID: 1238
	[SerializeField]
	private StatListTab[] Tabs;

	// Token: 0x040004D7 RID: 1239
	[SerializeField]
	private bool SortUsingStatPriority;

	// Token: 0x040004D8 RID: 1240
	[Space]
	[SerializeField]
	private DetailedStatListElement ElementPrefab;

	// Token: 0x040004D9 RID: 1241
	[SerializeField]
	private RectTransform ListParent;

	// Token: 0x040004DA RID: 1242
	[SerializeField]
	private IndexButton TabButtonPrefab;

	// Token: 0x040004DB RID: 1243
	[SerializeField]
	private RectTransform TabsParent;

	// Token: 0x040004DC RID: 1244
	private List<IndexButton> TabButtons = new List<IndexButton>();

	// Token: 0x040004DD RID: 1245
	private int CurrentTab = -1;

	// Token: 0x040004DE RID: 1246
	private List<DetailedStatListElement> AllElements = new List<DetailedStatListElement>();

	// Token: 0x040004DF RID: 1247
	private GameManager GM;

	// Token: 0x040004E0 RID: 1248
	private List<DetailedStatListElement> ActiveElements = new List<DetailedStatListElement>();
}
