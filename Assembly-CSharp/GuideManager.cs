using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class GuideManager : MBSingleton<GuideManager>
{
	// Token: 0x060000F5 RID: 245 RVA: 0x0000B946 File Offset: 0x00009B46
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000B953 File Offset: 0x00009B53
	private void Start()
	{
		this.GenerateAllPages();
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000B95C File Offset: 0x00009B5C
	public static CardData GetContainerModelForLiquid(CardData _Liquid)
	{
		if (!MBSingleton<GuideManager>.Instance)
		{
			return null;
		}
		for (int i = 0; i < MBSingleton<GuideManager>.Instance.LiquidDefaultContainers.Length; i++)
		{
			if (MBSingleton<GuideManager>.Instance.LiquidDefaultContainers[i].CanContainThisLiquid(_Liquid))
			{
				return MBSingleton<GuideManager>.Instance.LiquidDefaultContainers[i];
			}
		}
		return null;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000B9B0 File Offset: 0x00009BB0
	public static ContentPage GetPageFor(InGameCardBase _Card)
	{
		if (!_Card)
		{
			return null;
		}
		return GuideManager.GetPageFor(_Card.CardModel);
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000B9C8 File Offset: 0x00009BC8
	public static ContentPage GetPageFor(CardData _Card)
	{
		if (!_Card)
		{
			return null;
		}
		if (GuideManager.CardsToEntriesDict == null)
		{
			return null;
		}
		if (GuideManager.CardsToEntriesDict.Count == 0)
		{
			return null;
		}
		if (GuideManager.CardsToEntriesDict.ContainsKey(_Card))
		{
			return GuideManager.CardsToEntriesDict[_Card];
		}
		if (_Card.CardType != CardTypes.Blueprint)
		{
			return null;
		}
		if (_Card.BlueprintResult == null)
		{
			return null;
		}
		if (_Card.BlueprintResult.Length == 0)
		{
			return null;
		}
		ContentPage contentPage = null;
		for (int i = 0; i < _Card.BlueprintResult.Length; i++)
		{
			if (_Card == _Card.BlueprintResult[i].DroppedCard)
			{
				Debug.LogError(_Card + " is a blueprint that is dropping itself as a result :(");
			}
			else
			{
				contentPage = GuideManager.GetPageFor(_Card.BlueprintResult[i].DroppedCard);
				if (contentPage)
				{
					return contentPage;
				}
			}
		}
		return contentPage;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000BA94 File Offset: 0x00009C94
	public static ContentPage GetPageFor(InGameStat _Stat)
	{
		if (!_Stat)
		{
			return null;
		}
		if (!_Stat.StatModel)
		{
			return null;
		}
		if (GuideManager.StatsToEntriesDict == null)
		{
			return null;
		}
		if (GuideManager.StatsToEntriesDict.Count == 0)
		{
			return null;
		}
		if (GuideManager.StatsToEntriesDict.ContainsKey(_Stat.StatModel))
		{
			return GuideManager.StatsToEntriesDict[_Stat.StatModel];
		}
		return null;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x0000BAF8 File Offset: 0x00009CF8
	private void GenerateAllPages()
	{
		if (!this.FillGuide)
		{
			return;
		}
		for (int i = this.AllEntries.Count - 1; i >= 0; i--)
		{
			if (!this.AllEntries[i].HasDesc && !this.AllEntries[i].ExplicitPage)
			{
				this.AllEntries.RemoveAt(i);
			}
		}
		this.AllEntries.Sort((GuideEntry a, GuideEntry b) => a.GetTitle.CompareTo(b.GetTitle));
		if (GuideManager.GuidePages == null)
		{
			GuideManager.GuidePages = new List<ContentPage>();
		}
		else
		{
			GuideManager.GuidePages.Clear();
		}
		if (GuideManager.PagesDict == null)
		{
			GuideManager.PagesDict = new Dictionary<GuideEntry, ContentPage>();
		}
		else
		{
			GuideManager.PagesDict.Clear();
		}
		if (GuideManager.CardsToEntriesDict == null)
		{
			GuideManager.CardsToEntriesDict = new Dictionary<CardData, ContentPage>();
		}
		else
		{
			GuideManager.CardsToEntriesDict.Clear();
		}
		if (GuideManager.StatsToEntriesDict == null)
		{
			GuideManager.StatsToEntriesDict = new Dictionary<GameStat, ContentPage>();
		}
		else
		{
			GuideManager.StatsToEntriesDict.Clear();
		}
		for (int j = 0; j < this.AllEntries.Count; j++)
		{
			ContentPage contentPage = this.AllEntries[j].ExportToPage(this.EntriesImageScale);
			if (contentPage)
			{
				GuideManager.GuidePages.Add(contentPage);
				GuideManager.PagesDict.Add(this.AllEntries[j], contentPage);
				for (int k = 0; k < this.AllEntries[j].AssociatedCards.Count; k++)
				{
					if (this.AllEntries[j].AssociatedCards[k] && !GuideManager.CardsToEntriesDict.ContainsKey(this.AllEntries[j].AssociatedCards[k]))
					{
						GuideManager.CardsToEntriesDict.Add(this.AllEntries[j].AssociatedCards[k], contentPage);
					}
				}
				for (int l = 0; l < this.AllEntries[j].AssociatedStats.Count; l++)
				{
					if (this.AllEntries[j].AssociatedStats[l] && !GuideManager.StatsToEntriesDict.ContainsKey(this.AllEntries[j].AssociatedStats[l]))
					{
						GuideManager.StatsToEntriesDict.Add(this.AllEntries[j].AssociatedStats[l], contentPage);
					}
				}
				if (this.AllEntries[j].RelatedEntries == null)
				{
					this.AllEntries[j].RelatedEntries = new List<GuideEntry>();
				}
				else
				{
					for (int m = this.AllEntries[j].RelatedEntries.Count - 1; m >= 0; m--)
					{
						if (!this.AllEntries[j].RelatedEntries[m])
						{
							this.AllEntries[j].RelatedEntries.RemoveAt(m);
						}
					}
				}
			}
		}
		for (int n = 0; n < GuideManager.GuidePages.Count; n++)
		{
			if (this.AllEntries[n].RelatedEntries != null && this.AllEntries[n].ExplicitPage == null)
			{
				GuideManager.GuidePages[n].GlobalPageLinks = new ContentPageLink[this.AllEntries[n].RelatedEntries.Count];
				for (int num = 0; num < this.AllEntries[n].RelatedEntries.Count; num++)
				{
					if (GuideManager.PagesDict.ContainsKey(this.AllEntries[n].RelatedEntries[num]))
					{
						GuideManager.GuidePages[n].GlobalPageLinks[num] = new ContentPageLink(this.AllEntries[n].RelatedEntries[num].GetLocalizedTitle, GuideManager.PagesDict[this.AllEntries[n].RelatedEntries[num]]);
					}
				}
			}
			if (this.LinkEntryPagesTogether)
			{
				if (n > 0)
				{
					GuideManager.GuidePages[n].LeftButton = GuideManager.GuidePages[n - 1];
				}
				if (n < GuideManager.GuidePages.Count - 1)
				{
					GuideManager.GuidePages[n].RightButton = GuideManager.GuidePages[n + 1];
				}
			}
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060000FC RID: 252 RVA: 0x0000BF83 File Offset: 0x0000A183
	public static int TipsCount
	{
		get
		{
			if (!MBSingleton<GuideManager>.Instance)
			{
				return 0;
			}
			if (MBSingleton<GuideManager>.Instance.AllTips == null)
			{
				return 0;
			}
			return MBSingleton<GuideManager>.Instance.AllTips.Length;
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x0000BFB0 File Offset: 0x0000A1B0
	public static string GetTip(int _TipTitleSize)
	{
		if (!MBSingleton<GuideManager>.Instance)
		{
			return "";
		}
		if (MBSingleton<GuideManager>.Instance.AllTips == null)
		{
			return "";
		}
		if (MBSingleton<GuideManager>.Instance.AllTips.Length == 0)
		{
			return "";
		}
		if ((float)_TipTitleSize <= 0f)
		{
			return MBSingleton<GuideManager>.Instance.AllTips[UnityEngine.Random.Range(0, MBSingleton<GuideManager>.Instance.AllTips.Length)];
		}
		if (LocalizationManager.LanguageDoesNotSupportUnderlined)
		{
			return string.Format("<size={0}><b>{1}:</b></size>\n{2}", _TipTitleSize.ToString(), LocalizedString.Tip, MBSingleton<GuideManager>.Instance.AllTips[UnityEngine.Random.Range(0, MBSingleton<GuideManager>.Instance.AllTips.Length)]);
		}
		return string.Format("<size={0}><u>{1}:</u></size>\n{2}", _TipTitleSize.ToString(), LocalizedString.Tip, MBSingleton<GuideManager>.Instance.AllTips[UnityEngine.Random.Range(0, MBSingleton<GuideManager>.Instance.AllTips.Length)]);
	}

	// Token: 0x04000121 RID: 289
	public static List<ContentPage> GuidePages = new List<ContentPage>();

	// Token: 0x04000122 RID: 290
	public static Dictionary<GuideEntry, ContentPage> PagesDict;

	// Token: 0x04000123 RID: 291
	public static Dictionary<CardData, ContentPage> CardsToEntriesDict;

	// Token: 0x04000124 RID: 292
	public static Dictionary<GameStat, ContentPage> StatsToEntriesDict;

	// Token: 0x04000125 RID: 293
	public LocalizedString[] AllTips;

	// Token: 0x04000126 RID: 294
	[SerializeField]
	private bool FillGuide;

	// Token: 0x04000127 RID: 295
	[SerializeField]
	private float EntriesImageScale;

	// Token: 0x04000128 RID: 296
	[SerializeField]
	private bool LinkEntryPagesTogether;

	// Token: 0x04000129 RID: 297
	[SerializeField]
	private CardData[] LiquidDefaultContainers;

	// Token: 0x0400012A RID: 298
	public List<GuideEntry> AllEntries = new List<GuideEntry>();
}
