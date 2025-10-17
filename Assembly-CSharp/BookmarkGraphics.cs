using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000049 RID: 73
public class BookmarkGraphics : TooltipProvider
{
	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x060002FF RID: 767 RVA: 0x0001ED31 File Offset: 0x0001CF31
	// (set) Token: 0x06000300 RID: 768 RVA: 0x0001ED39 File Offset: 0x0001CF39
	public CardData MarkedCard { get; private set; }

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06000301 RID: 769 RVA: 0x0001ED42 File Offset: 0x0001CF42
	// (set) Token: 0x06000302 RID: 770 RVA: 0x0001ED4A File Offset: 0x0001CF4A
	public CardData MarkedLiquid { get; private set; }

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06000303 RID: 771 RVA: 0x0001ED53 File Offset: 0x0001CF53
	// (set) Token: 0x06000304 RID: 772 RVA: 0x0001ED5B File Offset: 0x0001CF5B
	public BookmarkGroup MarkedGroup { get; private set; }

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000305 RID: 773 RVA: 0x0001ED64 File Offset: 0x0001CF64
	public int GetIndex
	{
		get
		{
			if (!this.GrM)
			{
				this.GetManagers();
			}
			if (!this.GrM)
			{
				return -1;
			}
			for (int i = 0; i < this.GrM.Bookmarks.Length; i++)
			{
				if (this.GrM.Bookmarks[i] == this)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0001EDC4 File Offset: 0x0001CFC4
	public bool HasCardMarked(CardData _Card, CardData _WithLiquid)
	{
		if (!this.MarkedCard || !_Card)
		{
			return false;
		}
		if (this.MarkedGroup)
		{
			return this.MarkedGroup.HasCard(_Card) && _WithLiquid == this.MarkedLiquid;
		}
		return this.MarkedCard == _Card && _WithLiquid == this.MarkedLiquid;
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0001EE2E File Offset: 0x0001D02E
	private void Start()
	{
		this.GetManagers();
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0001EE38 File Offset: 0x0001D038
	private void GetManagers()
	{
		if (!this.GrM)
		{
			this.GrM = MBSingleton<GraphicsManager>.Instance;
		}
		if (!this.GM)
		{
			this.GM = MBSingleton<GameManager>.Instance;
			GameManager.OnBeginDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnBeginDragItem, new Action<InGameDraggableCard>(this.OnDragBegin));
			GameManager.OnEndDragItem = (Action<InGameDraggableCard>)Delegate.Combine(GameManager.OnEndDragItem, new Action<InGameDraggableCard>(this.OnDragEnd));
		}
		if (this.IndexText)
		{
			this.IndexText.text = (this.GetIndex + 1).ToString();
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0001EEDD File Offset: 0x0001D0DD
	private void OnDragBegin(InGameDraggableCard _Card)
	{
		if (this.Group)
		{
			this.Group.blocksRaycasts = false;
		}
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0001EEF8 File Offset: 0x0001D0F8
	private void OnDragEnd(InGameDraggableCard _Card)
	{
		if (this.Group)
		{
			this.Group.blocksRaycasts = true;
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0001EF14 File Offset: 0x0001D114
	public void SetCard(CardData _Card, CardData _WithLiquid, bool _Save)
	{
		if (!this.GrM)
		{
			this.GetManagers();
		}
		if (_Card)
		{
			if (this.IconImage)
			{
				this.IconImage.gameObject.SetActive(_Card.CardImage);
				this.IconImage.overrideSprite = _Card.CardImage;
			}
			if (this.ColorImage)
			{
				this.ColorImage.color = this.BookmarkColor;
			}
			if (this.ShadowEffect)
			{
				this.ShadowEffect.enabled = true;
			}
			if (this.Group)
			{
				this.Group.interactable = true;
			}
			this.MarkedGroup = this.GrM.GetGroupForCard(_Card);
			this.MarkedLiquid = _WithLiquid;
			base.SetTooltip(this.TooltipTitle(_Card.CardName), "", "", 0);
			this.LatestFoundSlot = null;
		}
		else
		{
			if (this.Group)
			{
				this.Group.interactable = false;
			}
			if (this.ColorImage)
			{
				this.ColorImage.color = this.DisabledColor;
			}
			if (this.ShadowEffect)
			{
				this.ShadowEffect.enabled = false;
			}
			if (this.IconImage)
			{
				this.IconImage.gameObject.SetActive(false);
			}
			base.CancelTooltip();
			this.MarkedGroup = null;
			this.MarkedLiquid = null;
		}
		if (this.Group && this.Sound)
		{
			this.Sound.MuteHoverSounds = !this.Group.interactable;
			this.Sound.MuteClickSounds = !this.Group.interactable;
		}
		this.MarkedCard = _Card;
		if (_Save)
		{
			this.GrM.SaveBookmarks();
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0001F0F4 File Offset: 0x0001D2F4
	private string TooltipTitle(string _CardName)
	{
		if (this.MarkedLiquid)
		{
			return LocalizedString.BookmarkWithLiquid(this.MarkedLiquid, this.MarkedGroup ? this.MarkedGroup.GroupName : _CardName);
		}
		if (!this.MarkedGroup)
		{
			return _CardName;
		}
		return this.MarkedGroup.GroupName;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0001F15C File Offset: 0x0001D35C
	public void FindBookmark()
	{
		if (!this.MarkedCard)
		{
			return;
		}
		if (!this.GrM)
		{
			this.GetManagers();
		}
		if (!this.GrM)
		{
			return;
		}
		if (!this.MarkedGroup)
		{
			this.LatestFoundSlot = this.GrM.FindBookmarkedSlot(this.MarkedCard, this.MarkedLiquid, this.LatestFoundSlot ? (this.LatestFoundSlot.Index + 1) : 0);
		}
		else
		{
			this.LatestFoundSlot = this.GrM.FindBookmarkedSlot(this.MarkedGroup, this.MarkedLiquid, this.LatestFoundSlot ? (this.LatestFoundSlot.Index + 1) : 0);
		}
		if (!this.LatestFoundSlot)
		{
			this.GrM.PlayMessage(this.NotificationPos ? this.NotificationPos.position : base.transform.position, LocalizedString.BookmarkNotFound(this.TooltipTitle(this.MarkedCard.CardName)), this.NotificationPos ? this.NotificationPos : base.transform);
		}
		this.GrM.MoveViewToSlot(this.LatestFoundSlot, true, true);
	}

	// Token: 0x0400039F RID: 927
	[SerializeField]
	private Image ColorImage;

	// Token: 0x040003A0 RID: 928
	[SerializeField]
	private Shadow ShadowEffect;

	// Token: 0x040003A1 RID: 929
	[SerializeField]
	private Image IconImage;

	// Token: 0x040003A2 RID: 930
	[SerializeField]
	private TextMeshProUGUI IndexText;

	// Token: 0x040003A3 RID: 931
	[SerializeField]
	private CanvasGroup Group;

	// Token: 0x040003A4 RID: 932
	[SerializeField]
	private Transform NotificationPos;

	// Token: 0x040003A5 RID: 933
	[SerializeField]
	private ButtonSounds Sound;

	// Token: 0x040003A6 RID: 934
	public Color BookmarkColor;

	// Token: 0x040003A7 RID: 935
	public Color DisabledColor;

	// Token: 0x040003A8 RID: 936
	private GraphicsManager GrM;

	// Token: 0x040003A9 RID: 937
	private GameManager GM;

	// Token: 0x040003AD RID: 941
	private DynamicLayoutSlot LatestFoundSlot;
}
