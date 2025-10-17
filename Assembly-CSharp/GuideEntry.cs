using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000144 RID: 324
[CreateAssetMenu(menuName = "Survival/Guide Entry")]
public class GuideEntry : ScriptableObject
{
	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000949 RID: 2377 RVA: 0x000574CC File Offset: 0x000556CC
	public CardData MainAssociatedCard
	{
		get
		{
			if (this.AssociatedCards == null)
			{
				return null;
			}
			if (this.AssociatedCards.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < this.AssociatedCards.Count; i++)
			{
				if (this.AssociatedCards[i])
				{
					return this.AssociatedCards[i];
				}
			}
			return null;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x0600094A RID: 2378 RVA: 0x0005752C File Offset: 0x0005572C
	public GameStat MainAssociatedStat
	{
		get
		{
			if (this.AssociatedStats == null)
			{
				return null;
			}
			if (this.AssociatedStats.Count == 0)
			{
				return null;
			}
			for (int i = 0; i < this.AssociatedStats.Count; i++)
			{
				if (this.AssociatedStats[i])
				{
					return this.AssociatedStats[i];
				}
			}
			return null;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x0600094B RID: 2379 RVA: 0x00057589 File Offset: 0x00055789
	public bool HasDesc
	{
		get
		{
			return !string.IsNullOrEmpty(this.OverrideDescription);
		}
	}

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x0600094C RID: 2380 RVA: 0x0005759E File Offset: 0x0005579E
	public bool HasLinks
	{
		get
		{
			return this.RelatedEntries != null && this.RelatedEntries.Count > 0;
		}
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x000575B8 File Offset: 0x000557B8
	public ContentPage ExportToPage(float _ImageScale)
	{
		if (this.ExplicitPage)
		{
			return this.ExplicitPage;
		}
		if (string.IsNullOrEmpty(this.GetTitle))
		{
			return null;
		}
		ContentPage contentPage = ScriptableObject.CreateInstance<ContentPage>();
		contentPage.PageTitle = this.GetLocalizedTitle;
		contentPage.PageIllustration = this.GetIllustration;
		contentPage.PageDisplay = ContentDisplayOptions.Vertical;
		contentPage.TitleSeparator = true;
		contentPage.Sections = new ContentSection[1];
		contentPage.Sections[0].Entries = new PieceOfInfo[1];
		PieceOfInfo pieceOfInfo = default(PieceOfInfo);
		pieceOfInfo.InfoDisplay = ContentDisplayOptions.Vertical;
		pieceOfInfo.Content = this.GetLocalizedDescription;
		pieceOfInfo.ImageSizeMultiplier = _ImageScale;
		pieceOfInfo.OrderOfElements = new int[]
		{
			0,
			1,
			2
		};
		contentPage.Sections[0].Entries[0] = pieceOfInfo;
		return contentPage;
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x0600094E RID: 2382 RVA: 0x0005768C File Offset: 0x0005588C
	public LocalizedString GetLocalizedTitle
	{
		get
		{
			if (this.ExplicitPage)
			{
				return this.ExplicitPage.PageTitle;
			}
			if (!string.IsNullOrEmpty(this.OverrideTitle))
			{
				return this.OverrideTitle;
			}
			if (this.MainAssociatedCard)
			{
				return this.MainAssociatedCard.CardName;
			}
			if (this.MainAssociatedStat)
			{
				return this.MainAssociatedStat.GameName;
			}
			return default(LocalizedString);
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x0600094F RID: 2383 RVA: 0x00057708 File Offset: 0x00055908
	private LocalizedString GetLocalizedDescription
	{
		get
		{
			if (this.ExplicitPage)
			{
				return this.ExplicitPage.PageTitle;
			}
			if (!string.IsNullOrEmpty(this.OverrideDescription))
			{
				return this.OverrideDescription;
			}
			if (this.MainAssociatedCard)
			{
				return LocalizedString.GuidePlaceHolderText(this.MainAssociatedCard.CardDescription);
			}
			if (this.MainAssociatedStat)
			{
				return LocalizedString.GuidePlaceHolderText(this.MainAssociatedStat.Description);
			}
			return default(LocalizedString);
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x00057798 File Offset: 0x00055998
	public Sprite GetIllustration
	{
		get
		{
			if (this.ExplicitPage)
			{
				return null;
			}
			if (this.OverrideIllustration)
			{
				return this.OverrideIllustration;
			}
			if (this.MainAssociatedCard)
			{
				if (this.MainAssociatedCard.CardType != CardTypes.Liquid)
				{
					return this.MainAssociatedCard.CardImage;
				}
				CardData containerModelForLiquid = GuideManager.GetContainerModelForLiquid(this.MainAssociatedCard);
				if (!containerModelForLiquid)
				{
					return this.MainAssociatedCard.CardImage;
				}
				return containerModelForLiquid.GetImageForLiquid(this.MainAssociatedCard);
			}
			else
			{
				if (this.MainAssociatedStat)
				{
					return this.MainAssociatedStat.GetIcon;
				}
				return null;
			}
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000951 RID: 2385 RVA: 0x00057837 File Offset: 0x00055A37
	public string GetDescription
	{
		get
		{
			return this.GetLocalizedDescription;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x00057844 File Offset: 0x00055A44
	public string GetTitle
	{
		get
		{
			return this.GetLocalizedTitle;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000953 RID: 2387 RVA: 0x00057851 File Offset: 0x00055A51
	// (set) Token: 0x06000954 RID: 2388 RVA: 0x00057859 File Offset: 0x00055A59
	public int SearchMatchScore { get; private set; }

	// Token: 0x06000955 RID: 2389 RVA: 0x00057864 File Offset: 0x00055A64
	public bool SearchMatch(Regex _Pattern)
	{
		if (!string.IsNullOrEmpty(this.GetTitle) && _Pattern.IsMatch(this.GetTitle))
		{
			this.SearchMatchScore = 0;
			return true;
		}
		if (!string.IsNullOrEmpty(this.OverrideDescription) && _Pattern.IsMatch(this.OverrideDescription))
		{
			this.SearchMatchScore = 3;
			return true;
		}
		if (this.AssociatedCards != null)
		{
			for (int i = 0; i < this.AssociatedCards.Count; i++)
			{
				if (this.AssociatedCards[i])
				{
					if (_Pattern.IsMatch(this.AssociatedCards[i].CardName))
					{
						this.SearchMatchScore = 1;
						return true;
					}
					if (_Pattern.IsMatch(this.AssociatedCards[i].CardDescription))
					{
						this.SearchMatchScore = 3;
						return true;
					}
				}
			}
		}
		if (this.AssociatedStats != null)
		{
			for (int j = 0; j < this.AssociatedStats.Count; j++)
			{
				if (this.AssociatedStats[j])
				{
					if (_Pattern.IsMatch(this.AssociatedStats[j].GameName))
					{
						this.SearchMatchScore = 1;
						return true;
					}
					if (_Pattern.IsMatch(this.AssociatedStats[j].Description))
					{
						this.SearchMatchScore = 3;
						return true;
					}
				}
			}
		}
		if (this.RelatedEntries != null)
		{
			for (int k = 0; k < this.RelatedEntries.Count; k++)
			{
				if (this.RelatedEntries[k] && !string.IsNullOrEmpty(this.RelatedEntries[k].GetTitle) && _Pattern.IsMatch(this.RelatedEntries[k].GetTitle))
				{
					this.SearchMatchScore = 2;
					return true;
				}
			}
		}
		this.SearchMatchScore = -1;
		return false;
	}

	// Token: 0x04000EC2 RID: 3778
	public List<CardData> AssociatedCards;

	// Token: 0x04000EC3 RID: 3779
	public List<GameStat> AssociatedStats;

	// Token: 0x04000EC4 RID: 3780
	public Sprite OverrideIllustration;

	// Token: 0x04000EC5 RID: 3781
	public LocalizedString OverrideTitle;

	// Token: 0x04000EC6 RID: 3782
	public LocalizedString OverrideDescription;

	// Token: 0x04000EC7 RID: 3783
	public List<GuideEntry> RelatedEntries;

	// Token: 0x04000EC8 RID: 3784
	public ContentPage ExplicitPage;
}
