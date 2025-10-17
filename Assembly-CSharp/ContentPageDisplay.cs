using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000056 RID: 86
public class ContentPageDisplay : MonoBehaviour
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060003AA RID: 938 RVA: 0x000268CC File Offset: 0x00024ACC
	// (set) Token: 0x060003AB RID: 939 RVA: 0x000268D4 File Offset: 0x00024AD4
	public int PageIndex { get; private set; }

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060003AC RID: 940 RVA: 0x000268DD File Offset: 0x00024ADD
	// (set) Token: 0x060003AD RID: 941 RVA: 0x000268E5 File Offset: 0x00024AE5
	public ContentDisplayer ParentDisplay { get; private set; }

	// Token: 0x060003AE RID: 942 RVA: 0x000268F0 File Offset: 0x00024AF0
	public void Init(ContentDisplayer _ParentDisplay, int _Index, ContentPage _PageContent)
	{
		this.PageIndex = _Index;
		this.ParentDisplay = _ParentDisplay;
		this.DisplayedContent = _PageContent;
		if (this.TitleSeparator)
		{
			this.TitleSeparator.SetActive(_PageContent.TitleSeparator);
		}
		if (this.NextAndPrevPageSection)
		{
			if (!_PageContent.LeftButton && !_PageContent.RightButton && _Index == -1)
			{
				this.NextAndPrevPageSection.SetActive(false);
			}
			else
			{
				this.NextAndPrevPageSection.SetActive(true);
			}
		}
		if (this.PageLinksParent)
		{
			this.PageLinksParent.gameObject.SetActive(_PageContent.HasLinks);
			if (this.PageLinksParent.gameObject.activeInHierarchy)
			{
				int num = 0;
				while (num < this.PageLinkButtons.Count || num < _PageContent.GlobalPageLinks.Length)
				{
					if (num >= _PageContent.GlobalPageLinks.Length)
					{
						this.PageLinkButtons[num].gameObject.SetActive(false);
						this.LinkedPages[num] = null;
					}
					else
					{
						if (num >= this.PageLinkButtons.Count)
						{
							this.PageLinkButtons.Add(UnityEngine.Object.Instantiate<IndexButton>(this.LinkButtonPrefab, this.PageLinksParent));
							this.LinkedPages.Add(_PageContent.GlobalPageLinks[num]);
							IndexButton indexButton = this.PageLinkButtons[num];
							indexButton.OnClicked = (Action<int>)Delegate.Combine(indexButton.OnClicked, new Action<int>(this.OnPageButtonClicked));
							if (string.IsNullOrEmpty(_PageContent.GlobalPageLinks[num]))
							{
								this.PageLinkButtons[num].gameObject.SetActive(false);
								this.LinkedPages[num] = null;
								goto IL_243;
							}
						}
						else
						{
							if (string.IsNullOrEmpty(_PageContent.GlobalPageLinks[num]))
							{
								this.PageLinkButtons[num].gameObject.SetActive(false);
								this.LinkedPages[num] = null;
								goto IL_243;
							}
							this.LinkedPages[num] = _PageContent.GlobalPageLinks[num];
							this.PageLinkButtons[num].gameObject.SetActive(true);
						}
						this.PageLinkButtons[num].Setup(num, _PageContent.GlobalPageLinks[num], "", false);
					}
					IL_243:
					num++;
				}
			}
		}
		if (this.NextButton)
		{
			this.NextButton.gameObject.SetActive(_PageContent.RightButton);
			if (!this.NextButtonNewIcon && _ParentDisplay.NextButtonNewIconPrefab)
			{
				this.NextButtonNewIcon = UnityEngine.Object.Instantiate<GameObject>(_ParentDisplay.NextButtonNewIconPrefab, this.NextButton.transform);
			}
			if (this.NextButtonNewIcon && _PageContent.RightButton)
			{
				this.NextButtonNewIcon.SetActive(_PageContent.RightButton.HasNewContent && this.ParentDisplay.ShowDetailedObjectiveInfo);
			}
		}
		if (_PageContent.RightButton)
		{
			GraphicsManager.SetActiveGroup(this.NextNewContentObjects, _PageContent.RightButton.HasNewContent && this.ParentDisplay.ShowDetailedObjectiveInfo);
		}
		if (this.PreviousButton)
		{
			this.PreviousButton.gameObject.SetActive(_PageContent.LeftButton);
			if (!this.PrevButtonNewIcon && _ParentDisplay.NextButtonNewIconPrefab)
			{
				this.PrevButtonNewIcon = UnityEngine.Object.Instantiate<GameObject>(_ParentDisplay.NextButtonNewIconPrefab, this.PreviousButton.transform);
			}
			if (this.PrevButtonNewIcon && _PageContent.LeftButton)
			{
				this.PrevButtonNewIcon.SetActive(_PageContent.LeftButton.HasNewContent && this.ParentDisplay.ShowDetailedObjectiveInfo);
			}
		}
		if (_PageContent.LeftButton)
		{
			GraphicsManager.SetActiveGroup(this.PrevNewContentObjects, _PageContent.LeftButton.HasNewContent && this.ParentDisplay.ShowDetailedObjectiveInfo);
		}
		if (this.BackButton)
		{
			this.BackButton.gameObject.SetActive(_PageContent.BackButton);
		}
		if (this.PageText)
		{
			if (_Index != -1)
			{
				this.PageText.text = (_Index + 1).ToString() + "/" + _ParentDisplay.PageCount.ToString();
				if (!this.UseGameObjectToHideText)
				{
					this.PageText.enabled = true;
				}
				else
				{
					this.PageText.gameObject.SetActive(true);
				}
			}
			else if (!this.UseGameObjectToHideText)
			{
				this.PageText.enabled = false;
			}
			else
			{
				this.PageText.gameObject.SetActive(false);
			}
		}
		if (this.SectionTitle)
		{
			this.SectionTitle.text = _PageContent.PageTitle;
		}
		if (this.SectionIllustration)
		{
			if (_PageContent.PageIllustration)
			{
				this.SectionIllustration.gameObject.SetActive(true);
				this.SectionIllustration.sprite = _PageContent.PageIllustration;
			}
			else
			{
				this.SectionIllustration.gameObject.SetActive(false);
			}
		}
		this.ParentTr = ((_PageContent.PageDisplay == ContentDisplayOptions.Horizontal) ? this.HorizontalDisplay.transform : this.VerticalDisplay.transform);
		this.HorizontalDisplay.gameObject.SetActive(this.ParentTr == this.HorizontalDisplay.transform);
		this.VerticalDisplay.gameObject.SetActive(this.ParentTr == this.VerticalDisplay.transform);
		this.HorizontalDisplay.spacing = this.DefaultSpacing + _PageContent.SpaceBtwnSections;
		this.VerticalDisplay.spacing = this.DefaultSpacing + _PageContent.SpaceBtwnSections;
		int num2 = 0;
		int num3 = 0;
		while (num3 < _PageContent.Sections.Length || num3 < this.AllSections.Count)
		{
			if (num3 >= _PageContent.Sections.Length)
			{
				this.AllSections[num3].gameObject.SetActive(false);
			}
			else
			{
				if (num3 >= this.AllSections.Count)
				{
					this.AllSections.Add(UnityEngine.Object.Instantiate<ContentSectionDisplay>(this.SectionPrefab, this.ParentTr));
				}
				this.AllSections[num3].transform.SetParent(this.ParentTr);
				this.AllSections[num3].transform.SetAsLastSibling();
				this.AllSections[num3].Init(_PageContent.Sections[num3]);
				int num4 = 0;
				while (num4 < _PageContent.Sections[num3].Entries.Length || num2 < this.AllInfo.Count)
				{
					if (num4 >= _PageContent.Sections[num3].Entries.Length)
					{
						if (num3 != _PageContent.Sections.Length - 1)
						{
							break;
						}
						this.AllInfo[num2].gameObject.SetActive(false);
						this.AllInfo[num2].transform.SetParent(base.transform);
						num2++;
					}
					else if (_PageContent.Sections[num3].Entries[num4].CanShow)
					{
						if (num2 >= this.AllInfo.Count)
						{
							this.AllInfo.Add(UnityEngine.Object.Instantiate<PieceOfInfoDisplay>(this.PieceOfInfoPrefab, this.ParentTr));
						}
						this.AllSections[num3].AddInfo(this.AllInfo[num2]);
						this.AllInfo[num2].Init(_PageContent.Sections[num3].Entries[num4], _ParentDisplay);
						this.AllInfo[num2].gameObject.SetActive(true);
						num2++;
					}
					num4++;
				}
				this.AllSections[num3].gameObject.SetActive(true);
			}
			num3++;
		}
	}

	// Token: 0x060003AF RID: 943 RVA: 0x00027107 File Offset: 0x00025307
	public void NextButtonPress()
	{
		if (!this.ParentDisplay)
		{
			return;
		}
		this.ParentDisplay.OpenPage(this.DisplayedContent.RightButton);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0002712D File Offset: 0x0002532D
	public void PrevButtonPress()
	{
		if (!this.ParentDisplay)
		{
			return;
		}
		this.ParentDisplay.OpenPage(this.DisplayedContent.LeftButton);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00027153 File Offset: 0x00025353
	public void BackButtonPress()
	{
		if (!this.ParentDisplay)
		{
			return;
		}
		this.ParentDisplay.OpenPage(this.DisplayedContent.BackButton);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00027179 File Offset: 0x00025379
	private void OnPageButtonClicked(int _Index)
	{
		if (!this.ParentDisplay)
		{
			return;
		}
		this.ParentDisplay.OpenPage(this.LinkedPages[_Index]);
	}

	// Token: 0x040004AA RID: 1194
	[SerializeField]
	private TextMeshProUGUI SectionTitle;

	// Token: 0x040004AB RID: 1195
	[SerializeField]
	private MaskFrameImageResizer SectionIllustration;

	// Token: 0x040004AC RID: 1196
	[SerializeField]
	private GameObject TitleSeparator;

	// Token: 0x040004AD RID: 1197
	[SerializeField]
	private Button NextButton;

	// Token: 0x040004AE RID: 1198
	[SerializeField]
	private GameObject[] NextNewContentObjects;

	// Token: 0x040004AF RID: 1199
	[SerializeField]
	private Button PreviousButton;

	// Token: 0x040004B0 RID: 1200
	[SerializeField]
	private GameObject[] PrevNewContentObjects;

	// Token: 0x040004B1 RID: 1201
	[SerializeField]
	private Button BackButton;

	// Token: 0x040004B2 RID: 1202
	[SerializeField]
	private TextMeshProUGUI PageText;

	// Token: 0x040004B3 RID: 1203
	[SerializeField]
	private bool UseGameObjectToHideText;

	// Token: 0x040004B4 RID: 1204
	[SerializeField]
	private GameObject NextAndPrevPageSection;

	// Token: 0x040004B5 RID: 1205
	[SerializeField]
	private Transform PageLinksParent;

	// Token: 0x040004B6 RID: 1206
	[SerializeField]
	private IndexButton LinkButtonPrefab;

	// Token: 0x040004B9 RID: 1209
	[SerializeField]
	private float DefaultSpacing;

	// Token: 0x040004BA RID: 1210
	[SerializeField]
	private HorizontalLayoutGroup HorizontalDisplay;

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	private VerticalLayoutGroup VerticalDisplay;

	// Token: 0x040004BC RID: 1212
	[SerializeField]
	private ContentSectionDisplay SectionPrefab;

	// Token: 0x040004BD RID: 1213
	[SerializeField]
	private PieceOfInfoDisplay PieceOfInfoPrefab;

	// Token: 0x040004BE RID: 1214
	private Transform ParentTr;

	// Token: 0x040004BF RID: 1215
	private List<PieceOfInfoDisplay> AllInfo = new List<PieceOfInfoDisplay>();

	// Token: 0x040004C0 RID: 1216
	private List<ContentSectionDisplay> AllSections = new List<ContentSectionDisplay>();

	// Token: 0x040004C1 RID: 1217
	private ContentPage DisplayedContent;

	// Token: 0x040004C2 RID: 1218
	private GameObject NextButtonNewIcon;

	// Token: 0x040004C3 RID: 1219
	private GameObject PrevButtonNewIcon;

	// Token: 0x040004C4 RID: 1220
	private List<IndexButton> PageLinkButtons = new List<IndexButton>();

	// Token: 0x040004C5 RID: 1221
	private List<ContentPage> LinkedPages = new List<ContentPage>();
}
