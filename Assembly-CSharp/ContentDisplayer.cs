using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class ContentDisplayer : MonoBehaviour
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600039A RID: 922 RVA: 0x00026365 File Offset: 0x00024565
	// (set) Token: 0x0600039B RID: 923 RVA: 0x0002636D File Offset: 0x0002456D
	public List<ContentPage> AllPages { get; private set; }

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600039C RID: 924 RVA: 0x00026378 File Offset: 0x00024578
	public bool HasNewContent
	{
		get
		{
			if (this.AllPages == null)
			{
				return false;
			}
			if (this.AllPages.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.AllPages.Count; i++)
			{
				if (this.AllPages[i] && this.AllPages[i].HasNewContent)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600039D RID: 925 RVA: 0x000263E0 File Offset: 0x000245E0
	public bool HasUpdatedContent
	{
		get
		{
			if (this.AllPages == null)
			{
				return false;
			}
			if (this.AllPages.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.AllPages.Count; i++)
			{
				if (this.AllPages[i] && this.AllPages[i].HasUpdatedContent)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600039E RID: 926 RVA: 0x00026445 File Offset: 0x00024645
	public int PageCount
	{
		get
		{
			return this.ExplicitPageContent.Count;
		}
	}

	// Token: 0x0600039F RID: 927 RVA: 0x00026452 File Offset: 0x00024652
	public int PageIndex(ContentPage _Page)
	{
		if (this.ExplicitPageContent == null)
		{
			return -1;
		}
		if (this.ExplicitPageContent.Count == 0)
		{
			return -1;
		}
		if (!this.ExplicitPageContent.Contains(_Page))
		{
			return -1;
		}
		return this.ExplicitPageContent.IndexOf(_Page);
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0002648C File Offset: 0x0002468C
	public bool ContainsPage(ContentPage _Page)
	{
		if (this.AllPages == null)
		{
			this.FillAllPagesList();
		}
		else if (this.AllPages.Count == 0)
		{
			this.FillAllPagesList();
		}
		return this.AllPages != null && this.AllPages.Count != 0 && this.AllPages.Contains(_Page);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x000264E1 File Offset: 0x000246E1
	private void Start()
	{
		this.Init();
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x000264EC File Offset: 0x000246EC
	public virtual void Init()
	{
		this.FillAllPagesList();
		if (this.OpenOnNewestContent)
		{
			for (int i = 0; i < this.AllPages.Count; i++)
			{
				if (this.AllPages[i].HasNewContent || this.AllPages[i].HasUpdatedContent)
				{
					this.OpenPage(this.AllPages[i]);
					return;
				}
			}
		}
		if (ContentDisplayer.OpenDisplayers.ContainsKey(base.name))
		{
			this.OpenPage(ContentDisplayer.OpenDisplayers[base.name]);
			return;
		}
		ContentDisplayer.OpenDisplayers.Add(base.name, this.DefaultPage);
		this.OpenPage(this.DefaultPage);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x000265A4 File Offset: 0x000247A4
	public void FillAllPagesList()
	{
		this.AllPages = new List<ContentPage>();
		if (this.DefaultPage && !this.AllPages.Contains(this.DefaultPage))
		{
			this.AllPages.Add(this.DefaultPage);
			this.DefaultPage.FillList(this.AllPages);
		}
		if (this.ExplicitPageContent != null)
		{
			for (int i = 0; i < this.ExplicitPageContent.Count; i++)
			{
				if (!this.AllPages.Contains(this.ExplicitPageContent[i]) && this.ExplicitPageContent[i])
				{
					this.AllPages.Add(this.ExplicitPageContent[i]);
					this.ExplicitPageContent[i].FillList(this.AllPages);
				}
			}
		}
		for (int j = 0; j < this.AllPages.Count; j++)
		{
			this.AllPages[j].UpdateObjectives();
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0002669D File Offset: 0x0002489D
	public void OpenDefaultPage()
	{
		this.OpenPage(this.DefaultPage);
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x000266AB File Offset: 0x000248AB
	public virtual void OpenPage(ContentPage _Page)
	{
		if (_Page == null)
		{
			Debug.LogWarning("Trying to open a null page");
			return;
		}
		this.PageDisplayObject.Init(this, this.PageIndex(_Page), _Page);
		ContentDisplayer.OpenDisplayers[base.name] = _Page;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x000266E6 File Offset: 0x000248E6
	[ContextMenu("Auto Fill")]
	public void AutoFillContent()
	{
		if (!this.DefaultPage)
		{
			return;
		}
		this.ExplicitPageContent.Clear();
		this.FillContent(this.DefaultPage, 0, true);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00026710 File Offset: 0x00024910
	private void FillContent(ContentPage _Page, int _Index, bool _Left)
	{
		if (this.ExplicitPageContent.Contains(_Page) || !_Page)
		{
			return;
		}
		if (_Left)
		{
			this.ExplicitPageContent.Insert(Mathf.Max(0, _Index), _Page);
		}
		else
		{
			this.ExplicitPageContent.Insert(Mathf.Min(this.ExplicitPageContent.Count, _Index), _Page);
		}
		if (_Page.LeftButton)
		{
			this.FillContent(_Page.LeftButton, _Index, true);
		}
		if (_Page.RightButton)
		{
			this.FillContent(_Page.RightButton, _Index + 1, false);
		}
		if (_Page.Sections == null)
		{
			return;
		}
		if (_Page.Sections.Length == 0)
		{
			return;
		}
		for (int i = 0; i < _Page.Sections.Length; i++)
		{
			if (_Page.Sections[i].Entries != null && _Page.Sections[i].Entries.Length != 0)
			{
				for (int j = 0; j < _Page.Sections[i].Entries.Length; j++)
				{
					if (_Page.Sections[i].Entries[j].PageLinks != null && _Page.Sections[i].Entries[j].PageLinks.Length != 0)
					{
						for (int k = 0; k < _Page.Sections[i].Entries[j].PageLinks.Length; k++)
						{
							this.FillContent(_Page.Sections[i].Entries[j].PageLinks[k], this.ExplicitPageContent.Count, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x0400049F RID: 1183
	[SerializeField]
	protected ContentPage DefaultPage;

	// Token: 0x040004A0 RID: 1184
	public bool ShowDetailedObjectiveInfo;

	// Token: 0x040004A1 RID: 1185
	[SerializeField]
	private bool OpenOnNewestContent;

	// Token: 0x040004A2 RID: 1186
	[SerializeField]
	protected ContentPageDisplay PageDisplayObject;

	// Token: 0x040004A3 RID: 1187
	[SerializeField]
	private List<ContentPage> ExplicitPageContent;

	// Token: 0x040004A4 RID: 1188
	public AudioClip OpeningSound;

	// Token: 0x040004A5 RID: 1189
	public AudioClip ClosingSound;

	// Token: 0x040004A6 RID: 1190
	public GameObject NextButtonNewIconPrefab;

	// Token: 0x040004A7 RID: 1191
	public GameObject PageLinkNewIconPrefab;

	// Token: 0x040004A8 RID: 1192
	private static Dictionary<string, ContentPage> OpenDisplayers = new Dictionary<string, ContentPage>();
}
