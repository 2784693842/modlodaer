using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class GuideDisplayer : ContentDisplayer
{
	// Token: 0x06000594 RID: 1428 RVA: 0x0003B25A File Offset: 0x0003945A
	private void Awake()
	{
		if (!this.ListContentPage)
		{
			this.ListContentPage = ScriptableObject.CreateInstance<ContentPage>();
		}
		if (!this.DefaultPage)
		{
			this.DefaultPage = this.ListContentPage;
		}
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x0003B28D File Offset: 0x0003948D
	private void OnEnable()
	{
		this.ClearSearch();
		base.Init();
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x0003B29C File Offset: 0x0003949C
	public override void Init()
	{
		this.ListPage.Init(this, ref MBSingleton<GuideManager>.Instance.AllEntries);
		this.SearchPage.Init(this, ref this.SearchedEntries);
		for (int i = 0; i < MBSingleton<GuideManager>.Instance.AllEntries.Count; i++)
		{
			this.ListPage.AddElement(-1);
		}
		base.Init();
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0003B300 File Offset: 0x00039500
	public override void OpenPage(ContentPage _Page)
	{
		this.CurrentPage = _Page;
		if (_Page == this.ListContentPage)
		{
			this.PageDisplayObject.gameObject.SetActive(false);
			this.NoSelectionPage.SetActive(true);
			return;
		}
		this.NoSelectionPage.SetActive(false);
		base.OpenPage(_Page);
		this.PageDisplayObject.gameObject.SetActive(true);
		int index = GuideManager.GuidePages.IndexOf(_Page);
		if (!this.FirstOpening)
		{
			if (this.NormalListObject.activeInHierarchy)
			{
				this.ListPage.SelectEntry(index);
			}
			if (this.SearchListObject.activeInHierarchy)
			{
				this.SearchPage.SelectEntry(this.SearchedEntries.IndexOf(MBSingleton<GuideManager>.Instance.AllEntries[index]));
			}
		}
		else
		{
			base.StartCoroutine(this.SelectWithDelay(index));
		}
		this.FirstOpening = false;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0003B3DB File Offset: 0x000395DB
	private IEnumerator SelectWithDelay(int _Index)
	{
		yield return null;
		if (this.NormalListObject.activeInHierarchy)
		{
			this.ListPage.SelectEntry(_Index);
		}
		if (this.SearchListObject.activeInHierarchy)
		{
			this.SearchPage.SelectEntry(this.SearchedEntries.IndexOf(MBSingleton<GuideManager>.Instance.AllEntries[_Index]));
		}
		yield break;
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0003B3F1 File Offset: 0x000395F1
	public void ClearSearch()
	{
		this.SearchInputField.text = "";
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0003B404 File Offset: 0x00039604
	public void OnSearchStringValueChanged(string _Value)
	{
		if (_Value == this.SearchString)
		{
			return;
		}
		this.SearchedEntries.Clear();
		this.SearchString = _Value;
		this.NormalListObject.SetActive(string.IsNullOrEmpty(_Value));
		this.SearchedEntries.Clear();
		if (!string.IsNullOrEmpty(this.SearchString))
		{
			this.SearchPage.HideAllElements();
			this.SearchRegex = new Regex(this.SearchString, RegexOptions.IgnoreCase);
			for (int i = 0; i < MBSingleton<GuideManager>.Instance.AllEntries.Count; i++)
			{
				if (string.IsNullOrEmpty(this.SearchString))
				{
					this.ListPage.SetElementActive(i, true);
				}
				else if (MBSingleton<GuideManager>.Instance.AllEntries[i].SearchMatch(this.SearchRegex))
				{
					this.SearchedEntries.Add(MBSingleton<GuideManager>.Instance.AllEntries[i]);
				}
			}
			this.SearchedEntries.Sort(new GuideDisplayer.SearchedEntriesComparer());
			int num = 0;
			while (num < this.SearchedEntries.Count || num < this.SearchPage.Count)
			{
				if (num >= this.SearchedEntries.Count)
				{
					this.SearchPage.SetElementActive(num, false);
				}
				else
				{
					if (num >= this.SearchPage.Count)
					{
						this.SearchPage.AddElement(-1);
					}
					this.SearchPage.SetElementActive(num, true);
				}
				num++;
			}
			if (this.CurrentPage)
			{
				this.SearchPage.SelectEntry(this.SearchedEntries.IndexOf(MBSingleton<GuideManager>.Instance.AllEntries[GuideManager.GuidePages.IndexOf(this.CurrentPage)]));
			}
			else
			{
				this.SearchPage.SelectEntry(-1);
			}
			this.SearchListObject.SetActive(true);
			return;
		}
		this.SearchListObject.SetActive(false);
		if (this.CurrentPage)
		{
			this.ListPage.SelectEntry(GuideManager.GuidePages.IndexOf(this.CurrentPage));
			return;
		}
		this.ListPage.SelectEntry(-1);
	}

	// Token: 0x04000790 RID: 1936
	private string SearchString;

	// Token: 0x04000791 RID: 1937
	[SerializeField]
	private GameObject NormalListObject;

	// Token: 0x04000792 RID: 1938
	[SerializeField]
	private GuideListPageDisplay ListPage;

	// Token: 0x04000793 RID: 1939
	[SerializeField]
	private GameObject SearchListObject;

	// Token: 0x04000794 RID: 1940
	[SerializeField]
	private GuideListPageDisplay SearchPage;

	// Token: 0x04000795 RID: 1941
	[SerializeField]
	private ContentPage ListContentPage;

	// Token: 0x04000796 RID: 1942
	[SerializeField]
	private GameObject NoSelectionPage;

	// Token: 0x04000797 RID: 1943
	[SerializeField]
	private TMP_InputField SearchInputField;

	// Token: 0x04000798 RID: 1944
	private Regex SearchRegex;

	// Token: 0x04000799 RID: 1945
	private List<GuideEntry> SearchedEntries = new List<GuideEntry>();

	// Token: 0x0400079A RID: 1946
	private ContentPage CurrentPage;

	// Token: 0x0400079B RID: 1947
	private bool FirstOpening = true;

	// Token: 0x0200026B RID: 619
	public class SearchedEntriesComparer : IComparer<GuideEntry>
	{
		// Token: 0x06000FBC RID: 4028 RVA: 0x0007FF10 File Offset: 0x0007E110
		public int Compare(GuideEntry _A, GuideEntry _B)
		{
			if (_A.SearchMatchScore == _B.SearchMatchScore)
			{
				return _A.GetTitle.CompareTo(_B.GetTitle);
			}
			return _A.SearchMatchScore.CompareTo(_B.SearchMatchScore);
		}
	}
}
