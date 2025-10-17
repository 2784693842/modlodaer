using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x0200007A RID: 122
public class EndgameMenu : MBSingleton<EndgameMenu>
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060004D7 RID: 1239 RVA: 0x000319E4 File Offset: 0x0002FBE4
	public static bool IsGameOver
	{
		get
		{
			return MBSingleton<EndgameMenu>.PrivateInstance && MBSingleton<EndgameMenu>.PrivateInstance.gameObject.activeInHierarchy && MBSingleton<EndgameMenu>.PrivateInstance.GameOver;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00031A14 File Offset: 0x0002FC14
	public static bool IsVictory
	{
		get
		{
			return MBSingleton<EndgameMenu>.PrivateInstance && ((MBSingleton<EndgameMenu>.PrivateInstance.SpecialEndingScript && MBSingleton<EndgameMenu>.PrivateInstance.SpecialEndingScript.Playing) || (MBSingleton<EndgameMenu>.PrivateInstance.gameObject.activeInHierarchy && !MBSingleton<EndgameMenu>.PrivateInstance.GameOver));
		}
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00031A73 File Offset: 0x0002FC73
	public void Init()
	{
		if (MBSingleton<EndgameMenu>.PrivateInstance)
		{
			return;
		}
		MBSingleton<EndgameMenu>.PrivateInstance = this;
		this.GL = GameLoad.Instance;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00031A9F File Offset: 0x0002FC9F
	private void OnDisable()
	{
		if (MBSingleton<AmbienceImageEffect>.Instance)
		{
			MBSingleton<AmbienceImageEffect>.Instance.SetConfetti(false);
		}
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00031AB8 File Offset: 0x0002FCB8
	public void Clear()
	{
		if (this.LastPageMenuButton)
		{
			this.LastPageMenuButton.SetParent(this.PagesObject.transform);
		}
		this.LastLogText.transform.SetParent(this.PagesObject.transform);
		if (this.AllPages.Count > 0)
		{
			for (int i = this.AllPages.Count - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.AllPages[i].gameObject);
			}
			this.AllPages.Clear();
		}
		if (this.LogLines.Count > 0)
		{
			for (int j = this.LogLines.Count - 1; j >= 0; j--)
			{
				UnityEngine.Object.Destroy(this.LogLines[j].gameObject);
			}
			this.LogLines.Clear();
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00031B94 File Offset: 0x0002FD94
	public void SetupWhilePlaying()
	{
		this.Setup("", 0, 0, MBSingleton<GameManager>.Instance.CurrentSaveData, false, false, false, false, false);
		this.CheckingWhilePlaying = true;
		this.BackgroundButton.enabled = true;
		this.CloseButton.SetActive(true);
		this.LastPageMenuButton.gameObject.SetActive(false);
		this.SetPage(this.LastCheckedPage);
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00031BFC File Offset: 0x0002FDFC
	public void DemoEndSetup(GameSaveData _SaveData)
	{
		this.Setup(LocalizedString.BuyToContinue, -1, -1, _SaveData, false, false, false, false, false);
		this.TitleText.text = LocalizedString.DemoOver;
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00031C34 File Offset: 0x0002FE34
	public void Setup(string _Cause, int _Suns, int _Moons, GameSaveData _SaveData, bool _GameOver, bool _KeepPlaying, bool _Confetti, bool _SpecialEnding, bool _CheckObjectives)
	{
		this.Clear();
		if (!MBSingleton<GameManager>.Instance)
		{
			return;
		}
		if (_SpecialEnding && this.SpecialEndingScript)
		{
			this.SpecialEndingScript.StartSpecialEnding();
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = -1;
		bool flag = false;
		this.GameData = _SaveData;
		this.CheckingWhilePlaying = false;
		this.BackgroundButton.enabled = false;
		this.CloseButton.SetActive(false);
		if (_SaveData != null)
		{
			LogSaveData[] array = null;
			if (_SaveData.AllEndgameLogs != null)
			{
				array = _SaveData.AllEndgameLogs.ToArray();
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					num2 = array[i].LoggedOnTick / MBSingleton<GameManager>.Instance.DaySettings.DailyPoints;
					if (num3 < num2)
					{
						this.AllPages.Add(UnityEngine.Object.Instantiate<RectTransform>(this.PagePrefab, this.PageParent));
						UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.DayTitlePrefab, this.AllPages[this.AllPages.Count - 1]).text = LocalizedString.DayCounter(num2 + 1);
						num = 0;
						flag = true;
						num3 = num2;
					}
					this.LogLines.Add(UnityEngine.Object.Instantiate<EndGameLogLine>(this.LogLinePrefab, this.AllPages[this.AllPages.Count - 1]));
					this.LogLines[this.LogLines.Count - 1].SetLogText(array[i]);
					num++;
					if (num >= (flag ? this.MaxLinesPerDayPage : this.MaxLinesPerPage))
					{
						this.AllPages.Add(UnityEngine.Object.Instantiate<RectTransform>(this.PagePrefab, this.PageParent));
						if (this.AlwaysShowDay)
						{
							UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.DayTitlePrefab, this.AllPages[this.AllPages.Count - 1]).text = LocalizedString.DayCounter(num2 + 1);
						}
						num = 0;
						flag = this.AlwaysShowDay;
					}
				}
			}
		}
		if (this.LogLines.Count == 0)
		{
			this.AllPages.Add(UnityEngine.Object.Instantiate<RectTransform>(this.PagePrefab, this.PageParent));
			UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.DayTitlePrefab, this.AllPages[this.AllPages.Count - 1]).text = LocalizedString.DayCounter(num2 + 1);
		}
		else if (num >= (flag ? this.MaxLinesPerDayPage : this.MaxLinesPerPage) - this.NeededLinesForLastPage)
		{
			this.AllPages.Add(UnityEngine.Object.Instantiate<RectTransform>(this.PagePrefab, this.PageParent));
			if (this.AlwaysShowDay)
			{
				UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.DayTitlePrefab, this.AllPages[this.AllPages.Count - 1]).text = LocalizedString.DayCounter(num2);
			}
		}
		this.LastLogText.transform.SetParent(this.AllPages[this.AllPages.Count - 1]);
		LogSaveData logText = default(LogSaveData);
		logText.LogText = _Cause;
		if (this.LastLogCategory)
		{
			logText.CategoryID = this.LastLogCategory.UniqueID;
		}
		logText.LoggedOnTick = MBSingleton<GameManager>.Instance.CurrentTickInfo.z;
		this.LastLogText.SetLogText(logText);
		this.LastPageMenuButton.SetParent(this.AllPages[this.AllPages.Count - 1]);
		this.LastPageMenuButton.gameObject.SetActive(true);
		this.GameOver = _GameOver;
		if (_Confetti)
		{
			MBSingleton<AmbienceImageEffect>.Instance.SetConfetti(true);
		}
		this.SetupCover(_Cause, _Suns, _Moons, _KeepPlaying && MBSingleton<CheatsManager>.Instance.CheatsActive, _GameOver && MBSingleton<GameManager>.Instance.IsSafeMode);
		this.SetPage(-1);
		base.gameObject.SetActive(true);
		if (MBSingleton<GameManager>.Instance && _CheckObjectives)
		{
			MBSingleton<GameManager>.Instance.UpdateObjectivesCompletion(true);
		}
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00032008 File Offset: 0x00030208
	private void SetupCover(string _Cause, int _Suns, int _Moons, bool _KeepPlaying, bool _Checkpoint)
	{
		this.KeepPlayingButton.SetActive(_KeepPlaying);
		this.LoadCheckpointButton.SetActive(_Checkpoint);
		StringBuilder stringBuilder = new StringBuilder();
		bool gameOver = this.GameOver;
		stringBuilder.Append(LocalizedString.DaysSurvived(MBSingleton<GameManager>.Instance.CurrentDay));
		if (!string.IsNullOrEmpty(_Cause))
		{
			stringBuilder.Append("\n");
			stringBuilder.Append(_Cause);
		}
		if (_Suns < 0 && _Moons < 0)
		{
			this.SunsAndMoons.SetActive(false);
		}
		else
		{
			this.SunsAndMoons.SetActive(true);
			this.EarningsTitle.text = (this.GameOver ? LocalizedString.SurvivalBonus : LocalizedString.VictoryBonus);
			this.SunsEarned.transform.parent.gameObject.SetActive(_Suns >= 0);
			this.MoonsEarned.transform.parent.gameObject.SetActive(_Moons >= 0);
			this.SunsEarned.text = _Suns.ToString();
			this.MoonsEarned.text = _Moons.ToString();
		}
		this.TitleText.text = (this.GameOver ? LocalizedString.GameOver : LocalizedString.Victory);
		this.CauseText.text = stringBuilder.ToString();
		if (this.GameData != null)
		{
			string characterName = this.GetCharacterName();
			int score = this.CharacterReference.GetScore(GameManager.CurrentPlayerCharacter.CharacterPerks.ToArray());
			Sprite sprite = null;
			this.CharacterName.text = LocalizedString.AuthorName(characterName);
			this.SubtitleText.text = LocalizedString.JournalTaglineWithName(this.CharacterReference.GetRating(score), score, characterName, MBSingleton<GameManager>.Instance.IsSafeMode);
			if (!string.IsNullOrEmpty(this.GameData.CurrentCharacter) && this.GameData.CurrentCharacter != "Custom")
			{
				PlayerCharacter fromID = UniqueIDScriptable.GetFromID<PlayerCharacter>(this.GameData.CurrentCharacter);
				if (fromID)
				{
					sprite = fromID.CharacterPortrait;
				}
			}
			if (this.GameData.CharacterData != null && !sprite)
			{
				sprite = this.CharacterReference.GetCharacterPortrait(this.GameData.CharacterData.PortraitID);
			}
			this.CharacterPortrait.overrideSprite = sprite;
		}
		this.DemoStuffObject.SetActive(false);
		this.CharacterPortraitObject.SetActive(true);
		this.CharacterPortrait.material.SetFloat("_Saturation", this.GameOver ? 0f : 1f);
		this.CharacterPortraitBG.material.SetFloat("_Saturation", this.GameOver ? 0f : 1f);
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x000322B0 File Offset: 0x000304B0
	private string GetCharacterName()
	{
		if (this.GameData == null)
		{
			return LocalizedString.UnknownAuthor;
		}
		if (this.GameData.CharacterData == null && string.IsNullOrEmpty(this.GameData.CurrentCharacter))
		{
			return LocalizedString.UnknownAuthor;
		}
		if (!string.IsNullOrEmpty(this.GameData.CurrentCharacter))
		{
			PlayerCharacter fromID = UniqueIDScriptable.GetFromID<PlayerCharacter>(this.GameData.CurrentCharacter);
			if (fromID && !string.IsNullOrEmpty(fromID.CharacterName))
			{
				return fromID.CharacterName;
			}
		}
		if (this.GameData.CharacterData == null)
		{
			return LocalizedString.UnknownAuthor;
		}
		if (string.IsNullOrEmpty(this.GameData.CharacterData.CharacterName))
		{
			return LocalizedString.UnknownAuthor;
		}
		return this.GameData.CharacterData.CharacterName;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00032390 File Offset: 0x00030590
	private void SetPage(int _Page)
	{
		this.CurrentPage = Mathf.Min(_Page, this.AllPages.Count - 1);
		if (this.CheckingWhilePlaying)
		{
			this.LastCheckedPage = this.CurrentPage;
		}
		this.CoverMenu.SetActive(_Page < 0);
		this.PagesObject.SetActive(_Page >= 0);
		if (_Page < 0)
		{
			return;
		}
		for (int i = 0; i < this.AllPages.Count; i++)
		{
			if (this.AllPages[i])
			{
				this.AllPages[i].gameObject.SetActive(i == this.CurrentPage);
			}
		}
		if (this.PageNumber)
		{
			this.PageNumber.text = (this.CurrentPage + 1).ToString() + "/" + this.AllPages.Count.ToString();
		}
		if (this.PrevPageButton)
		{
			this.PrevPageButton.interactable = ((this.CurrentPage >= 0 && !this.CheckingWhilePlaying) || this.CurrentPage > 0);
		}
		if (this.FirstPageButton)
		{
			this.FirstPageButton.interactable = ((this.CurrentPage >= 0 && !this.CheckingWhilePlaying) || this.CurrentPage > 0);
		}
		if (this.NextPageButton)
		{
			this.NextPageButton.interactable = (this.CurrentPage < this.AllPages.Count - 1);
		}
		if (this.LastPageButton)
		{
			this.LastPageButton.interactable = (this.CurrentPage < this.AllPages.Count - 1);
		}
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00032541 File Offset: 0x00030741
	public void NextPage()
	{
		this.SetPage(this.CurrentPage + 1);
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00032551 File Offset: 0x00030751
	public void PrevPage()
	{
		this.SetPage(this.CurrentPage - 1);
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00032561 File Offset: 0x00030761
	public void FirstPage()
	{
		if (!this.CheckingWhilePlaying)
		{
			this.SetPage(-1);
			return;
		}
		this.SetPage(0);
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x0003257A File Offset: 0x0003077A
	public void LastPage()
	{
		this.SetPage(this.AllPages.Count - 1);
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00032590 File Offset: 0x00030790
	public void QuitGame()
	{
		bool isGameOver = EndgameMenu.IsGameOver;
		this.GL.DeleteGameData(this.GL.CurrentGameDataIndex);
		if (this.SpecialEndingScript.Playing && !Application.isEditor)
		{
			Application.Quit();
			return;
		}
		SceneManager.LoadScene(this.GL.MenuSceneIndex);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x000325E3 File Offset: 0x000307E3
	public void LoadCheckpoint()
	{
		this.GL.LoadCheckpoint();
	}

	// Token: 0x0400061E RID: 1566
	[SerializeField]
	private GameObject CloseButton;

	// Token: 0x0400061F RID: 1567
	[SerializeField]
	private Button BackgroundButton;

	// Token: 0x04000620 RID: 1568
	[SerializeField]
	private SpecialEnding SpecialEndingScript;

	// Token: 0x04000621 RID: 1569
	[Header("Prefabs")]
	[SerializeField]
	private EndGameLogLine LogLinePrefab;

	// Token: 0x04000622 RID: 1570
	[SerializeField]
	private TextMeshProUGUI DayTitlePrefab;

	// Token: 0x04000623 RID: 1571
	[SerializeField]
	private RectTransform PagePrefab;

	// Token: 0x04000624 RID: 1572
	[SerializeField]
	private EndgameLogCategory LastLogCategory;

	// Token: 0x04000625 RID: 1573
	[SerializeField]
	private GlobalCharacterInfo CharacterReference;

	// Token: 0x04000626 RID: 1574
	[Header("Pages")]
	[SerializeField]
	private GameObject PagesObject;

	// Token: 0x04000627 RID: 1575
	[SerializeField]
	private int MaxLinesPerPage;

	// Token: 0x04000628 RID: 1576
	[SerializeField]
	private int MaxLinesPerDayPage;

	// Token: 0x04000629 RID: 1577
	[SerializeField]
	private int NeededLinesForLastPage;

	// Token: 0x0400062A RID: 1578
	[SerializeField]
	private bool AlwaysShowDay;

	// Token: 0x0400062B RID: 1579
	[SerializeField]
	private RectTransform PageParent;

	// Token: 0x0400062C RID: 1580
	[SerializeField]
	private TextMeshProUGUI PageNumber;

	// Token: 0x0400062D RID: 1581
	[SerializeField]
	private Button NextPageButton;

	// Token: 0x0400062E RID: 1582
	[SerializeField]
	private Button PrevPageButton;

	// Token: 0x0400062F RID: 1583
	[SerializeField]
	private Button LastPageButton;

	// Token: 0x04000630 RID: 1584
	[SerializeField]
	private Button FirstPageButton;

	// Token: 0x04000631 RID: 1585
	[SerializeField]
	private EndGameLogLine LastLogText;

	// Token: 0x04000632 RID: 1586
	[SerializeField]
	[FormerlySerializedAs("GoToMenuButton")]
	private RectTransform LastPageMenuButton;

	// Token: 0x04000633 RID: 1587
	[SerializeField]
	private GameObject DemoStuffObject;

	// Token: 0x04000634 RID: 1588
	[Header("Cover menu")]
	[SerializeField]
	private GameObject CoverMenu;

	// Token: 0x04000635 RID: 1589
	[SerializeField]
	private GameObject KeepPlayingButton;

	// Token: 0x04000636 RID: 1590
	[SerializeField]
	private TextMeshProUGUI TitleText;

	// Token: 0x04000637 RID: 1591
	[SerializeField]
	private TextMeshProUGUI CauseText;

	// Token: 0x04000638 RID: 1592
	[SerializeField]
	private GameObject SunsAndMoons;

	// Token: 0x04000639 RID: 1593
	[SerializeField]
	private GameObject NoSunsAndMoonsSpace;

	// Token: 0x0400063A RID: 1594
	[SerializeField]
	private TextMeshProUGUI SunsEarned;

	// Token: 0x0400063B RID: 1595
	[SerializeField]
	private TextMeshProUGUI MoonsEarned;

	// Token: 0x0400063C RID: 1596
	[SerializeField]
	private TextMeshProUGUI EarningsTitle;

	// Token: 0x0400063D RID: 1597
	[SerializeField]
	private TextMeshProUGUI SubtitleText;

	// Token: 0x0400063E RID: 1598
	[SerializeField]
	private TextMeshProUGUI CharacterName;

	// Token: 0x0400063F RID: 1599
	[SerializeField]
	private Image CharacterPortrait;

	// Token: 0x04000640 RID: 1600
	[SerializeField]
	private Image CharacterPortraitBG;

	// Token: 0x04000641 RID: 1601
	[SerializeField]
	private GameObject CharacterPortraitObject;

	// Token: 0x04000642 RID: 1602
	[SerializeField]
	private GameObject LoadCheckpointButton;

	// Token: 0x04000643 RID: 1603
	private GameLoad GL;

	// Token: 0x04000644 RID: 1604
	private GameSaveData GameData;

	// Token: 0x04000645 RID: 1605
	private List<RectTransform> AllPages = new List<RectTransform>();

	// Token: 0x04000646 RID: 1606
	private List<EndGameLogLine> LogLines = new List<EndGameLogLine>();

	// Token: 0x04000647 RID: 1607
	private int CurrentPage;

	// Token: 0x04000648 RID: 1608
	private bool GameOver;

	// Token: 0x04000649 RID: 1609
	private bool CheckingWhilePlaying;

	// Token: 0x0400064A RID: 1610
	private int LastCheckedPage;
}
