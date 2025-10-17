using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009E RID: 158
public class PieceOfInfoDisplay : MonoBehaviour
{
	// Token: 0x06000674 RID: 1652 RVA: 0x000437E0 File Offset: 0x000419E0
	public void Init(PieceOfInfo _Info, ContentDisplayer _ParentDisplayer)
	{
		this.ParentDisplayer = _ParentDisplayer;
		switch (_Info.InfoDisplay)
		{
		case ContentDisplayOptions.Vertical:
			this.ParentObject = this.VerticalDisplay.transform;
			this.ButtonsParentObject = this.VerticalButtonsDisplay.transform;
			if (this.CustomContentText)
			{
				this.CustomContentText.gameObject.SetActive(false);
			}
			this.CurrentContentText = this.ContentText;
			this.CurrentContentText.gameObject.SetActive(true);
			break;
		case ContentDisplayOptions.Horizontal:
			this.ParentObject = this.HorizontalDisplay.transform;
			this.ButtonsParentObject = this.HorizontalButtonsDisplay.transform;
			if (this.CustomContentText)
			{
				this.CustomContentText.gameObject.SetActive(false);
			}
			this.CurrentContentText = this.ContentText;
			this.CurrentContentText.gameObject.SetActive(true);
			break;
		case ContentDisplayOptions.Custom:
			this.ParentObject = this.CustomDisplay.transform;
			this.ButtonsParentObject = this.CustomButtonsDisplay.transform;
			if (this.CustomContentText)
			{
				this.ContentText.gameObject.SetActive(false);
			}
			this.CurrentContentText = (this.CustomContentText ? this.CustomContentText : this.ContentText);
			this.CurrentContentText.gameObject.SetActive(true);
			break;
		}
		this.HorizontalDisplay.childAlignment = (_Info.OverrideAlignment ? _Info.ContentAlignment : this.DefaultHorizontalAlignment);
		this.VerticalDisplay.childAlignment = (_Info.OverrideAlignment ? _Info.ContentAlignment : this.DefaultVerticalAlignment);
		this.HorizontalDisplay.gameObject.SetActive(this.ParentObject == this.HorizontalDisplay.transform);
		this.VerticalDisplay.gameObject.SetActive(this.ParentObject == this.VerticalDisplay.transform);
		this.CustomDisplay.gameObject.SetActive(this.ParentObject == this.CustomDisplay.transform);
		this.HorizontalDisplay.spacing = this.DefaultButtonsSpacing + _Info.SpaceBtwnElements;
		this.VerticalDisplay.spacing = this.DefaultButtonsSpacing + _Info.SpaceBtwnElements;
		this.HorizontalButtonsDisplay.gameObject.SetActive(_Info.InfoDisplay == ContentDisplayOptions.Horizontal);
		this.VerticalButtonsDisplay.gameObject.SetActive(_Info.InfoDisplay == ContentDisplayOptions.Vertical);
		this.CustomButtonsDisplay.gameObject.SetActive(_Info.InfoDisplay == ContentDisplayOptions.Custom);
		this.HorizontalButtonsDisplay.spacing = this.DefaultButtonsSpacing + _Info.SpaceBtwnButtons;
		this.VerticalButtonsDisplay.spacing = this.DefaultButtonsSpacing + _Info.SpaceBtwnButtons;
		this.ButtonsParentObject.SetParent(this.ParentObject);
		bool flag = _Info.PageLinks != null;
		if (flag)
		{
			flag &= (_Info.PageLinks.Length != 0);
		}
		bool flag2 = _Info.WebLinks != null;
		if (flag2)
		{
			flag2 &= (_Info.WebLinks.Length != 0);
		}
		this.ButtonsParentObject.gameObject.SetActive(flag || flag2);
		if (this.ImageDisplay && !this.ImageElement)
		{
			this.ImageElement = (this.ImageElement = this.ImageDisplay.gameObject.GetComponent<LayoutElement>());
			this.ImageDefaultMinSize = new Vector2(this.ImageElement.minWidth, this.ImageElement.minHeight);
			this.ImageDefaultPreferredSize = new Vector2(this.ImageElement.preferredWidth, this.ImageElement.preferredHeight);
		}
		if (this.TitleText)
		{
			this.TitleText.text = _Info.Title;
			this.TitleText.gameObject.SetActive(!string.IsNullOrEmpty(_Info.Title));
			if (this.TitleAnchorParent)
			{
				this.TitleAnchorParent.gameObject.SetActive(!string.IsNullOrEmpty(_Info.Title));
			}
		}
		if (this.ImageDisplay)
		{
			this.ImageDisplay.sprite = _Info.Image;
			this.ImageDisplay.gameObject.SetActive(_Info.Image);
			this.ImageDisplay.transform.SetParent(this.ParentObject);
			if (!this.UseScaleForImageSize)
			{
				if (this.ImageElement)
				{
					Vector2 vector = (!Mathf.Approximately(_Info.ImageSizeMultiplier, 0f)) ? (this.ImageDefaultMinSize * _Info.ImageSizeMultiplier) : this.ImageDefaultMinSize;
					Vector2 vector2 = (!Mathf.Approximately(_Info.ImageSizeMultiplier, 0f)) ? (this.ImageDefaultPreferredSize * _Info.ImageSizeMultiplier) : this.ImageDefaultPreferredSize;
					this.ImageElement.minWidth = vector.x;
					this.ImageElement.minHeight = vector.y;
					this.ImageElement.preferredWidth = vector2.x;
					this.ImageElement.preferredHeight = vector2.y;
				}
			}
			else if (!Mathf.Approximately(_Info.ImageSizeMultiplier, 0f))
			{
				this.ImageDisplay.transform.localScale = Vector3.one * _Info.ImageSizeMultiplier;
			}
		}
		if (this.CurrentContentText)
		{
			this.CurrentContentText.text = _Info.Content;
			this.CurrentContentText.gameObject.SetActive(!string.IsNullOrEmpty(_Info.Content));
			if (this.ContentParent)
			{
				this.ContentParent.SetParent(this.ParentObject);
				this.ContentParent.gameObject.SetActive(!string.IsNullOrEmpty(_Info.Content));
			}
			else
			{
				this.CurrentContentText.transform.SetParent(this.ParentObject);
			}
			if (this.ContentAnchorParent)
			{
				this.ContentAnchorParent.gameObject.SetActive(!string.IsNullOrEmpty(_Info.Content));
			}
		}
		Transform transform = null;
		Transform transform2 = null;
		if (this.ObjectiveToggleObject)
		{
			this.ObjectiveToggleObject.AssociatedObjective = _Info.RelatedObjective;
			this.ObjectiveToggleObject.Refresh();
			this.ObjectiveToggleObject.gameObject.SetActive(_Info.RelatedObjective);
			if (_Info.AnchorObjectiveTo == ObjectiveAnchor.Content)
			{
				if (!this.ContentAnchorParent)
				{
					this.ObjectiveToggleObject.gameObject.SetActive(false);
				}
				else
				{
					this.ObjectiveToggleObject.transform.SetParent(this.ContentAnchorParent);
					this.ObjectiveToggleObject.transform.SetSiblingIndex((int)(this.CurrentContentText.transform.GetSiblingIndex() + this.ObjectiveToggleSide));
					this.ObjectiveToggleObject.SetTextObject(this.CurrentContentText);
				}
			}
			else if (_Info.AnchorObjectiveTo == ObjectiveAnchor.Title)
			{
				if (!this.TitleAnchorParent)
				{
					this.ObjectiveToggleObject.gameObject.SetActive(false);
				}
				else
				{
					this.ObjectiveToggleObject.transform.SetParent(this.TitleAnchorParent);
					this.ObjectiveToggleObject.transform.SetSiblingIndex((int)(this.TitleText.transform.GetSiblingIndex() + this.ObjectiveToggleSide));
					this.ObjectiveToggleObject.SetTextObject(this.TitleText);
				}
			}
			else if (_Info.AnchorObjectiveTo == ObjectiveAnchor.Buttons)
			{
				ContentDisplayOptions infoDisplay = _Info.InfoDisplay;
				if (infoDisplay != ContentDisplayOptions.Horizontal)
				{
					if (infoDisplay != ContentDisplayOptions.Custom)
					{
						transform = this.ButtonsParentObject;
					}
					else if (this.CustomButtonsAnchor)
					{
						transform = this.CustomButtonsAnchor;
					}
					else
					{
						transform = this.ButtonsParentObject;
					}
				}
				else
				{
					transform = this.ButtonsParentObject.parent;
					transform2 = this.ButtonsParentObject;
				}
				if (!transform)
				{
					this.ObjectiveToggleObject.gameObject.SetActive(false);
				}
				else
				{
					this.ObjectiveToggleObject.transform.SetParent(transform);
					this.ObjectiveToggleObject.SetTextObject(this.TitleText);
				}
			}
		}
		if (this.ObjectiveCheckboxFrame)
		{
			this.ObjectiveCheckboxFrame.enabled = (_Info.ObjectiveDisplayOptions == ObjectiveDisplay.Checkbox || _Info.ObjectiveDisplayOptions == ObjectiveDisplay.BarAndCheckbox);
		}
		if (!_Info.RelatedObjective)
		{
			this.PercentCompletionObjects.SetActive(false);
			GraphicsManager.SetActiveGroup(this.NewInfoObjects, false);
			GraphicsManager.SetActiveGroup(this.UpdatedInfoObjects, false);
		}
		else
		{
			this.PercentCompletionObjects.SetActive(_Info.RelatedObjective.NotificationSettings.Frequency == ObjectiveNotificationFrequencies.OnPercentThreshold && this.ParentDisplayer.ShowDetailedObjectiveInfo && (_Info.ObjectiveDisplayOptions == ObjectiveDisplay.Bar || _Info.ObjectiveDisplayOptions == ObjectiveDisplay.BarAndCheckbox));
			if (this.PercentBar)
			{
				this.PercentBar.fillAmount = _Info.RelatedObjective.CompletionPercent;
			}
			if (this.PercentText)
			{
				this.PercentText.text = (_Info.RelatedObjective.CompletionPercent * 100f).ToString("0") + "%";
			}
			GraphicsManager.SetActiveGroup(this.UpdatedInfoObjects, !_Info.RelatedObjective.HasBeenCheckedComplete);
			if (this.ParentDisplayer.ShowDetailedObjectiveInfo)
			{
				GraphicsManager.SetActiveGroup(this.NewInfoObjects, !_Info.RelatedObjective.HasBeenCheckedNew);
				_Info.RelatedObjective.CheckRelatedContent();
			}
			else
			{
				GraphicsManager.SetActiveGroup(this.NewInfoObjects, false);
			}
		}
		if (this.LinkButtonPrefab && this.ButtonsParentObject.gameObject.activeSelf)
		{
			if (_Info.PageLinks != null)
			{
				int num = 0;
				while (num < _Info.PageLinks.Length || num < this.PageLinkButtons.Count)
				{
					if (num >= _Info.PageLinks.Length && num < this.PageLinkButtons.Count)
					{
						this.PageLinkButtons[num].gameObject.SetActive(false);
						this.PageLinkButtons[num].transform.SetParent(base.transform);
						this.LinkedPages[num] = null;
					}
					else
					{
						if (num >= this.PageLinkButtons.Count)
						{
							this.PageLinkButtons.Add(UnityEngine.Object.Instantiate<IndexButton>(this.LinkButtonPrefab, this.ButtonsParentObject));
							if (_ParentDisplayer.PageLinkNewIconPrefab)
							{
								this.LinkButtonsNewIcons.Add(UnityEngine.Object.Instantiate<GameObject>(_ParentDisplayer.PageLinkNewIconPrefab, this.PageLinkButtons[this.PageLinkButtons.Count - 1].transform));
							}
							IndexButton indexButton = this.PageLinkButtons[num];
							indexButton.OnClicked = (Action<int>)Delegate.Combine(indexButton.OnClicked, new Action<int>(this.OnPageButtonClicked));
							this.LinkedPages.Add(_Info.PageLinks[num]);
						}
						else
						{
							this.PageLinkButtons[num].transform.SetParent(this.ButtonsParentObject);
							this.PageLinkButtons[num].gameObject.SetActive(true);
							this.LinkedPages[num] = _Info.PageLinks[num];
						}
						LayoutElement component = this.PageLinkButtons[num].GetComponent<LayoutElement>();
						if (component)
						{
							component.flexibleHeight = ((_Info.StretchButtons && _Info.InfoDisplay == ContentDisplayOptions.Horizontal) ? 1f : 0f);
							component.flexibleWidth = ((_Info.StretchButtons && _Info.InfoDisplay == ContentDisplayOptions.Vertical) ? 1f : 0f);
						}
						this.PageLinkButtons[num].Setup(num, _Info.PageLinks[num], "", false);
						if (_ParentDisplayer.PageLinkNewIconPrefab)
						{
							if (_Info.PageLinks[num].TargetPage)
							{
								this.LinkButtonsNewIcons[num].SetActive(_Info.PageLinks[num].TargetPage.HasNewContent);
							}
							else
							{
								this.LinkButtonsNewIcons[num].SetActive(false);
							}
						}
					}
					num++;
				}
			}
			if (_Info.WebLinks != null)
			{
				int num2 = 0;
				while (num2 < _Info.WebLinks.Length || num2 < this.WebLinkButtons.Count)
				{
					if (num2 >= _Info.WebLinks.Length && num2 < this.WebLinkButtons.Count)
					{
						this.WebLinkButtons[num2].gameObject.SetActive(false);
						this.WebLinkButtons[num2].transform.SetParent(base.transform);
						this.WebLinks[num2] = null;
					}
					else
					{
						if (num2 >= this.WebLinkButtons.Count)
						{
							this.WebLinkButtons.Add(UnityEngine.Object.Instantiate<IndexButton>(this.LinkButtonPrefab, this.ButtonsParentObject));
							IndexButton indexButton2 = this.WebLinkButtons[num2];
							indexButton2.OnClicked = (Action<int>)Delegate.Combine(indexButton2.OnClicked, new Action<int>(this.OnWebButtonClicked));
							this.WebLinks.Add(_Info.WebLinks[num2].Link);
						}
						else
						{
							this.WebLinkButtons[num2].transform.SetParent(this.ButtonsParentObject);
							this.WebLinkButtons[num2].gameObject.SetActive(true);
							this.WebLinks[num2] = _Info.WebLinks[num2].Link;
						}
						LayoutElement component = this.WebLinkButtons[num2].GetComponent<LayoutElement>();
						if (component)
						{
							component.flexibleHeight = ((_Info.StretchButtons && _Info.InfoDisplay == ContentDisplayOptions.Horizontal) ? 1f : 0f);
							component.flexibleWidth = ((_Info.StretchButtons && _Info.InfoDisplay == ContentDisplayOptions.Vertical) ? 1f : 0f);
						}
						this.WebLinkButtons[num2].Setup(num2, _Info.WebLinks[num2].Text, "", false);
					}
					num2++;
				}
			}
		}
		if (_Info.InfoDisplay != ContentDisplayOptions.Custom)
		{
			for (int i = 0; i < PieceOfInfo.Elements.Length; i++)
			{
				int num3 = (_Info.OrderOfElements.Length == PieceOfInfo.Elements.Length) ? _Info.OrderOfElements[i] : i;
				string a = PieceOfInfo.Elements[num3];
				if (!(a == "Content"))
				{
					if (!(a == "Image"))
					{
						if (a == "Buttons")
						{
							this.ButtonsParentObject.transform.SetAsLastSibling();
						}
					}
					else if (this.ImageDisplay)
					{
						this.ImageDisplay.transform.SetAsLastSibling();
					}
				}
				else if (this.ContentParent)
				{
					this.ContentParent.transform.SetAsLastSibling();
				}
				else if (this.CurrentContentText)
				{
					this.CurrentContentText.transform.SetAsLastSibling();
				}
			}
		}
		else
		{
			if (this.ImageDisplay && this.CustomImageParent)
			{
				this.ImageDisplay.transform.SetParent(this.CustomImageParent);
			}
			if ((this.ContentParent || this.CurrentContentText) && this.CustomContentParent)
			{
				if (this.ContentParent)
				{
					this.ContentParent.SetParent(this.CustomContentParent);
				}
				else
				{
					this.CurrentContentText.transform.SetParent(this.CustomContentParent);
				}
			}
		}
		if (transform && this.ObjectiveToggleObject)
		{
			if (!transform2)
			{
				if (this.ObjectiveToggleSide == PieceOfInfoDisplay.ObjectiveAnchorSide.Left)
				{
					this.ObjectiveToggleObject.transform.SetAsFirstSibling();
					return;
				}
				this.ObjectiveToggleObject.transform.SetAsLastSibling();
				return;
			}
			else
			{
				this.ObjectiveToggleObject.transform.SetSiblingIndex((int)(transform2.GetSiblingIndex() + this.ObjectiveToggleSide));
			}
		}
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x000447EA File Offset: 0x000429EA
	private void OnPageButtonClicked(int _Index)
	{
		if (this.ParentDisplayer)
		{
			this.ParentDisplayer.OpenPage(this.LinkedPages[_Index]);
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x00044810 File Offset: 0x00042A10
	private void OnWebButtonClicked(int _Index)
	{
		Application.OpenURL(this.WebLinks[_Index]);
	}

	// Token: 0x040008E6 RID: 2278
	[SerializeField]
	private HorizontalLayoutGroup HorizontalDisplay;

	// Token: 0x040008E7 RID: 2279
	[SerializeField]
	private TextAnchor DefaultHorizontalAlignment;

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	private VerticalLayoutGroup VerticalDisplay;

	// Token: 0x040008E9 RID: 2281
	[SerializeField]
	private TextAnchor DefaultVerticalAlignment;

	// Token: 0x040008EA RID: 2282
	[SerializeField]
	private float DefaultSpacing;

	// Token: 0x040008EB RID: 2283
	[SerializeField]
	private HorizontalLayoutGroup VerticalButtonsDisplay;

	// Token: 0x040008EC RID: 2284
	[SerializeField]
	private VerticalLayoutGroup HorizontalButtonsDisplay;

	// Token: 0x040008ED RID: 2285
	[SerializeField]
	private float DefaultButtonsSpacing;

	// Token: 0x040008EE RID: 2286
	[SerializeField]
	private RectTransform CustomDisplay;

	// Token: 0x040008EF RID: 2287
	[SerializeField]
	private RectTransform CustomContentParent;

	// Token: 0x040008F0 RID: 2288
	[SerializeField]
	private RectTransform CustomImageParent;

	// Token: 0x040008F1 RID: 2289
	[SerializeField]
	private RectTransform CustomButtonsDisplay;

	// Token: 0x040008F2 RID: 2290
	[SerializeField]
	private RectTransform CustomButtonsAnchor;

	// Token: 0x040008F3 RID: 2291
	[Space]
	[SerializeField]
	private RectTransform ContentParent;

	// Token: 0x040008F4 RID: 2292
	[SerializeField]
	private RectTransform TitleAnchorParent;

	// Token: 0x040008F5 RID: 2293
	[SerializeField]
	private RectTransform ContentAnchorParent;

	// Token: 0x040008F6 RID: 2294
	[Space]
	[SerializeField]
	private TextMeshProUGUI TitleText;

	// Token: 0x040008F7 RID: 2295
	[SerializeField]
	private Image ImageDisplay;

	// Token: 0x040008F8 RID: 2296
	[SerializeField]
	private bool UseScaleForImageSize;

	// Token: 0x040008F9 RID: 2297
	[SerializeField]
	private TextMeshProUGUI ContentText;

	// Token: 0x040008FA RID: 2298
	[SerializeField]
	private TextMeshProUGUI CustomContentText;

	// Token: 0x040008FB RID: 2299
	[SerializeField]
	private ObjectiveToggle ObjectiveToggleObject;

	// Token: 0x040008FC RID: 2300
	[SerializeField]
	private Image ObjectiveCheckboxFrame;

	// Token: 0x040008FD RID: 2301
	[SerializeField]
	private PieceOfInfoDisplay.ObjectiveAnchorSide ObjectiveToggleSide;

	// Token: 0x040008FE RID: 2302
	[SerializeField]
	private IndexButton LinkButtonPrefab;

	// Token: 0x040008FF RID: 2303
	[SerializeField]
	private GameObject PercentCompletionObjects;

	// Token: 0x04000900 RID: 2304
	[SerializeField]
	private Image PercentBar;

	// Token: 0x04000901 RID: 2305
	[SerializeField]
	private TextMeshProUGUI PercentText;

	// Token: 0x04000902 RID: 2306
	[SerializeField]
	private GameObject[] NewInfoObjects;

	// Token: 0x04000903 RID: 2307
	[SerializeField]
	private GameObject[] UpdatedInfoObjects;

	// Token: 0x04000904 RID: 2308
	private Transform ParentObject;

	// Token: 0x04000905 RID: 2309
	private Transform ButtonsParentObject;

	// Token: 0x04000906 RID: 2310
	private TextMeshProUGUI CurrentContentText;

	// Token: 0x04000907 RID: 2311
	private LayoutElement ImageElement;

	// Token: 0x04000908 RID: 2312
	private Vector2 ImageDefaultPreferredSize;

	// Token: 0x04000909 RID: 2313
	private Vector2 ImageDefaultMinSize;

	// Token: 0x0400090A RID: 2314
	private List<IndexButton> PageLinkButtons = new List<IndexButton>();

	// Token: 0x0400090B RID: 2315
	private List<GameObject> LinkButtonsNewIcons = new List<GameObject>();

	// Token: 0x0400090C RID: 2316
	private List<ContentPage> LinkedPages = new List<ContentPage>();

	// Token: 0x0400090D RID: 2317
	private List<IndexButton> WebLinkButtons = new List<IndexButton>();

	// Token: 0x0400090E RID: 2318
	private List<string> WebLinks = new List<string>();

	// Token: 0x0400090F RID: 2319
	private ContentDisplayer ParentDisplayer;

	// Token: 0x02000271 RID: 625
	public enum ObjectiveAnchorSide
	{
		// Token: 0x0400148E RID: 5262
		Left = -1,
		// Token: 0x0400148F RID: 5263
		Right = 1
	}
}
