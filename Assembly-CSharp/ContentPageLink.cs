using System;
using UnityEngine;

// Token: 0x02000116 RID: 278
[Serializable]
public struct ContentPageLink
{
	// Token: 0x060008C3 RID: 2243 RVA: 0x00054818 File Offset: 0x00052A18
	public ContentPageLink(LocalizedString _Text, ContentPage _Page)
	{
		this.LocalizedLinkText = _Text;
		this.LinkText = null;
		this.TargetPage = _Page;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0005482F File Offset: 0x00052A2F
	public static implicit operator ContentPage(ContentPageLink _Page)
	{
		return _Page.TargetPage;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00054837 File Offset: 0x00052A37
	public static implicit operator string(ContentPageLink _Page)
	{
		return _Page.Text;
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x060008C6 RID: 2246 RVA: 0x00054840 File Offset: 0x00052A40
	public string Text
	{
		get
		{
			if (string.IsNullOrEmpty(this.LocalizedLinkText))
			{
				return this.LinkText;
			}
			return this.LocalizedLinkText;
		}
	}

	// Token: 0x04000D47 RID: 3399
	[SerializeField]
	private string LinkText;

	// Token: 0x04000D48 RID: 3400
	[SerializeField]
	private LocalizedString LocalizedLinkText;

	// Token: 0x04000D49 RID: 3401
	public ContentPage TargetPage;
}
