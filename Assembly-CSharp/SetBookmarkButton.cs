using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A3 RID: 163
public class SetBookmarkButton : IndexButton
{
	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x00044DFB File Offset: 0x00042FFB
	// (set) Token: 0x06000695 RID: 1685 RVA: 0x00044E03 File Offset: 0x00043003
	public BookmarkGraphics LinkedBookmark { get; private set; }

	// Token: 0x06000696 RID: 1686 RVA: 0x00044E0C File Offset: 0x0004300C
	private void Awake()
	{
		this.GetManager();
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00044E14 File Offset: 0x00043014
	private void GetManager()
	{
		if (!this.GrM)
		{
			this.GrM = MBSingleton<GraphicsManager>.Instance;
		}
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00044E30 File Offset: 0x00043030
	private void GetLinkedBookmark()
	{
		if (this.LinkedBookmark)
		{
			return;
		}
		this.GetManager();
		if (!this.GrM)
		{
			return;
		}
		if (base.Index < 0 || base.Index >= this.GrM.Bookmarks.Length)
		{
			return;
		}
		this.LinkedBookmark = this.GrM.Bookmarks[base.Index];
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000699 RID: 1689 RVA: 0x00044E96 File Offset: 0x00043096
	// (set) Token: 0x0600069A RID: 1690 RVA: 0x00044EB6 File Offset: 0x000430B6
	public Color TextColor
	{
		get
		{
			if (!this.ButtonText)
			{
				return Color.clear;
			}
			return this.ButtonText.color;
		}
		set
		{
			if (!this.ButtonText)
			{
				return;
			}
			this.ButtonText.color = value;
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00044ED4 File Offset: 0x000430D4
	public void UpdateVisuals()
	{
		this.GetLinkedBookmark();
		if (!this.LinkedBookmark)
		{
			return;
		}
		if (this.ColoredImages != null)
		{
			for (int i = 0; i < this.ColoredImages.Length; i++)
			{
				if (this.ColoredImages[i])
				{
					this.ColoredImages[i].color = (this.LinkedBookmark.MarkedCard ? this.LinkedBookmark.BookmarkColor : this.LinkedBookmark.DisabledColor);
				}
			}
		}
		if (this.BookmarkShadow)
		{
			this.BookmarkShadow.enabled = this.LinkedBookmark.MarkedCard;
		}
		if (this.LinkedBookmark.MarkedCard)
		{
			base.Sprite = this.LinkedBookmark.MarkedCard.CardImage;
			base.SetTooltip(this.LinkedBookmark.MarkedGroup ? this.LinkedBookmark.MarkedGroup.GroupName : this.LinkedBookmark.MarkedCard.CardName, "", "", 0);
			if (this.Icon)
			{
				this.Icon.gameObject.SetActive(true);
				return;
			}
		}
		else
		{
			base.Sprite = null;
			base.CancelTooltip();
			if (this.Icon)
			{
				this.Icon.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0400092E RID: 2350
	[SerializeField]
	private Image[] ColoredImages;

	// Token: 0x0400092F RID: 2351
	[SerializeField]
	private Shadow BookmarkShadow;

	// Token: 0x04000931 RID: 2353
	private GraphicsManager GrM;
}
