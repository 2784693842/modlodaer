using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000110 RID: 272
[CreateAssetMenu(menuName = "Survival/ContentPage")]
public class ContentPage : ScriptableObject
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x060008BB RID: 2235 RVA: 0x000542D0 File Offset: 0x000524D0
	public bool HasLinks
	{
		get
		{
			if (this.GlobalPageLinks == null)
			{
				return false;
			}
			if (this.GlobalPageLinks.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.GlobalPageLinks.Length; i++)
			{
				if (this.GlobalPageLinks[i].TargetPage)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x060008BC RID: 2236 RVA: 0x00054320 File Offset: 0x00052520
	public bool HasNewContent
	{
		get
		{
			if (this.Sections == null)
			{
				return false;
			}
			if (this.Sections.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.Sections.Length; i++)
			{
				if (this.Sections[i].Entries != null && this.Sections[i].Entries.Length != 0)
				{
					for (int j = 0; j < this.Sections[i].Entries.Length; j++)
					{
						if (this.Sections[i].Entries[j].RelatedObjective && this.Sections[i].Entries[j].CanShow && !this.Sections[i].Entries[j].RelatedObjective.HasBeenCheckedNew)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x060008BD RID: 2237 RVA: 0x00054410 File Offset: 0x00052610
	public bool HasUpdatedContent
	{
		get
		{
			if (this.Sections == null)
			{
				return false;
			}
			if (this.Sections.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.Sections.Length; i++)
			{
				if (this.Sections[i].Entries != null && this.Sections[i].Entries.Length != 0)
				{
					for (int j = 0; j < this.Sections[i].Entries.Length; j++)
					{
						if (this.Sections[i].Entries[j].RelatedObjective && this.Sections[i].Entries[j].CanShow && !this.Sections[i].Entries[j].RelatedObjective.HasBeenCheckedComplete)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00054500 File Offset: 0x00052700
	public void FillList(List<ContentPage> _PrevPages)
	{
		if (!_PrevPages.Contains(this))
		{
			_PrevPages.Add(this);
		}
		if (this.RightButton && !_PrevPages.Contains(this.RightButton))
		{
			this.RightButton.FillList(_PrevPages);
		}
		if (this.LeftButton && !_PrevPages.Contains(this.LeftButton))
		{
			this.LeftButton.FillList(_PrevPages);
		}
		if (this.Sections == null)
		{
			return;
		}
		if (this.Sections.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.Sections.Length; i++)
		{
			if (this.Sections[i].Entries != null && this.Sections[i].Entries.Length != 0)
			{
				for (int j = 0; j < this.Sections[i].Entries.Length; j++)
				{
					if (this.Sections[i].Entries[j].PageLinks != null && this.Sections[i].Entries[j].PageLinks.Length != 0)
					{
						for (int k = 0; k < this.Sections[i].Entries[j].PageLinks.Length; k++)
						{
							if (this.Sections[i].Entries[j].PageLinks[k].TargetPage && !_PrevPages.Contains(this.Sections[i].Entries[j].PageLinks[k].TargetPage))
							{
								this.Sections[i].Entries[j].PageLinks[k].TargetPage.FillList(_PrevPages);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x000546E4 File Offset: 0x000528E4
	public void UpdateObjectives()
	{
		if (this.Sections == null)
		{
			return;
		}
		if (this.Sections.Length == 0)
		{
			return;
		}
		for (int i = 0; i < this.Sections.Length; i++)
		{
			if (this.Sections[i].Entries != null && this.Sections[i].Entries.Length != 0)
			{
				for (int j = 0; j < this.Sections[i].Entries.Length; j++)
				{
					if (this.Sections[i].Entries[j].RelatedObjective)
					{
						this.Sections[i].Entries[j].RelatedObjective.AddRelatedPage(this);
					}
				}
			}
		}
	}

	// Token: 0x04000D1C RID: 3356
	public LocalizedString PageTitle;

	// Token: 0x04000D1D RID: 3357
	public ContentPage RightButton;

	// Token: 0x04000D1E RID: 3358
	public ContentPage LeftButton;

	// Token: 0x04000D1F RID: 3359
	public ContentPage BackButton;

	// Token: 0x04000D20 RID: 3360
	public Sprite PageIllustration;

	// Token: 0x04000D21 RID: 3361
	[Space]
	public ContentDisplayOptions PageDisplay;

	// Token: 0x04000D22 RID: 3362
	public float SpaceBtwnSections;

	// Token: 0x04000D23 RID: 3363
	public bool TitleSeparator;

	// Token: 0x04000D24 RID: 3364
	public ContentSection[] Sections;

	// Token: 0x04000D25 RID: 3365
	public ContentPageLink[] GlobalPageLinks;
}
