using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008F RID: 143
public class MenuCardPreview : TooltipProvider
{
	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000628 RID: 1576 RVA: 0x00041440 File Offset: 0x0003F640
	// (set) Token: 0x06000629 RID: 1577 RVA: 0x00041448 File Offset: 0x0003F648
	public CardData AssociatedCard { get; private set; }

	// Token: 0x0600062A RID: 1578 RVA: 0x00041454 File Offset: 0x0003F654
	public virtual void Setup(CardData _Data, bool _DescIsUnlockConditions)
	{
		this.AssociatedCard = _Data;
		if (this.Layout)
		{
			Canvas.ForceUpdateCanvases();
			if (this.SetHeight)
			{
				float num = base.GetComponent<RectTransform>().rect.width * 1.5f;
				this.Layout.minHeight = num;
				this.Layout.preferredHeight = num;
			}
			else
			{
				float num = base.GetComponent<RectTransform>().rect.height * 0.6666667f;
				this.Layout.minWidth = num;
				this.Layout.preferredWidth = num;
			}
		}
		if (this.Background)
		{
			this.Background.overrideSprite = _Data.BaseBackground;
		}
		if (this.Icon)
		{
			this.Icon.overrideSprite = _Data.CardImage;
		}
		if (this.CardTitle)
		{
			this.CardTitle.text = _Data.CardName;
		}
		if (this.CardDesc)
		{
			this.CardDesc.text = (_DescIsUnlockConditions ? _Data.UnlockConditionsDesc : _Data.CardDescription);
		}
		if (!this.NoTooltip)
		{
			base.SetTooltip(_Data.CardName, _DescIsUnlockConditions ? _Data.UnlockConditionsDesc : "", "", 0);
		}
		else
		{
			base.CancelTooltip();
		}
		if (this.HelpButton)
		{
			this.HelpButton.SetActive(GuideManager.GetPageFor(this.AssociatedCard));
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x000415E0 File Offset: 0x0003F7E0
	public void ClickHelp()
	{
		if (!MBSingleton<GameManager>.Instance)
		{
			return;
		}
		ContentPage pageFor = GuideManager.GetPageFor(this.AssociatedCard);
		if (pageFor)
		{
			MBSingleton<GameManager>.Instance.OpenGuide(pageFor);
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0004161C File Offset: 0x0003F81C
	public void ClickBlueprint()
	{
		if (!this.AssociatedCard)
		{
			return;
		}
		if (!MBSingleton<GameManager>.Instance || !MBSingleton<GraphicsManager>.Instance)
		{
			return;
		}
		if (this.AssociatedCard.ExplicitBlueprintNeeded)
		{
			MBSingleton<GraphicsManager>.Instance.BlueprintModelsPopup.ShowBlueprint(this.AssociatedCard.ExplicitBlueprintNeeded, true);
			return;
		}
		if (this.AssociatedCard.CardsOnBoard == null)
		{
			return;
		}
		if (this.AssociatedCard.CardsOnBoard.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.AssociatedCard.CardsOnBoard.Count; i++)
		{
			if (this.AssociatedCard.CardsOnBoard[i] != null && this.AssociatedCard.CardsOnBoard[i].Card)
			{
				if (MBSingleton<GameManager>.Instance.AllBlueprintResults.ContainsKey(this.AssociatedCard.CardsOnBoard[i].Card))
				{
					MBSingleton<GraphicsManager>.Instance.BlueprintModelsPopup.ShowBlueprint(MBSingleton<GameManager>.Instance.AllBlueprintResults[this.AssociatedCard.CardsOnBoard[i].Card], true);
					return;
				}
				if (MBSingleton<GameManager>.Instance.AllBlueprintModels.Contains(this.AssociatedCard.CardsOnBoard[i].Card))
				{
					MBSingleton<GraphicsManager>.Instance.BlueprintModelsPopup.ShowBlueprint(this.AssociatedCard.CardsOnBoard[i].Card, true);
					return;
				}
			}
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x000417A4 File Offset: 0x0003F9A4
	public void Pulse()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.ActionPulse != null)
		{
			this.ActionPulse.Complete(true);
			this.ActionPulse = null;
		}
		base.transform.localScale = Vector3.one;
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.ActionPulse = DOTween.Sequence();
		this.ActionPulse.Append(base.transform.DOPunchScale(Vector3.one * 0.25f, 0.33f, 5, 0.5f));
		this.ActionPulse.AppendCallback(delegate
		{
			this.ActionPulse = null;
		});
	}

	// Token: 0x0400086A RID: 2154
	[SerializeField]
	private LayoutElement Layout;

	// Token: 0x0400086B RID: 2155
	[SerializeField]
	private bool SetHeight;

	// Token: 0x0400086C RID: 2156
	[SerializeField]
	private bool NoTooltip;

	// Token: 0x0400086D RID: 2157
	[SerializeField]
	private Image Background;

	// Token: 0x0400086E RID: 2158
	[SerializeField]
	private Image Icon;

	// Token: 0x0400086F RID: 2159
	[SerializeField]
	private TextMeshProUGUI CardTitle;

	// Token: 0x04000870 RID: 2160
	[SerializeField]
	private TextMeshProUGUI CardDesc;

	// Token: 0x04000871 RID: 2161
	[SerializeField]
	private GameObject HelpButton;

	// Token: 0x04000873 RID: 2163
	private Sequence ActionPulse;
}
