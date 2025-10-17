using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class CheatsManager : MBSingleton<CheatsManager>
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000021 RID: 33 RVA: 0x00003C08 File Offset: 0x00001E08
	public bool CheatsActive
	{
		get
		{
			return ((Application.isEditor || Debug.isDebugBuild) && !this.CheatsOffInEditor) || this.CheatsAreOn;
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00003C28 File Offset: 0x00001E28
	public void SetCheatsActive(bool _Active)
	{
		this.CheatsAreOn = _Active;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00003C34 File Offset: 0x00001E34
	private void FillCards()
	{
		if (!GameLoad.Instance)
		{
			return;
		}
		if (!GameLoad.Instance.DataBase)
		{
			return;
		}
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < GameLoad.Instance.DataBase.AllData.Count; i++)
		{
			if (GameLoad.Instance.DataBase.AllData[i] is CardData)
			{
				list.Add(GameLoad.Instance.DataBase.AllData[i] as CardData);
			}
		}
		this.AllCards = list.ToArray();
		this.MaxPages = list.Count / 150;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00003CDF File Offset: 0x00001EDF
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		if (this.CheatsActive)
		{
			this.FillCards();
			CheatsManager.ShowFPS = true;
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003CFC File Offset: 0x00001EFC
	private void Update()
	{
		if (this.CheatsActive)
		{
			if (!this.GM)
			{
				this.GM = MBSingleton<GameManager>.Instance;
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				CheatsManager.ShowFPS = !CheatsManager.ShowFPS;
			}
			if (this.GM)
			{
				if (Input.GetKeyDown(KeyCode.KeypadDivide))
				{
					MBSingleton<GameManager>.Instance.OpenEndgameJournal(true);
				}
				else if (Input.GetKeyDown(KeyCode.KeypadMultiply))
				{
					MBSingleton<GameManager>.Instance.OpenEndgameJournal(false);
				}
			}
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				this.ShowGUI = !this.ShowGUI;
			}
			this.CheatsMenuBGObject.SetActive(this.ShowGUI);
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003DA8 File Offset: 0x00001FA8
	private void OnGUI()
	{
		if (!this.ShowGUI)
		{
			return;
		}
		GUILayout.BeginArea(new Rect((float)Screen.width * 0.75f, 0f, (float)Screen.width * 0.25f, (float)Screen.height));
		this.GeneralOptionsGUI();
		this.TimeGUI();
		this.CardsGUI();
		GUILayout.EndArea();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003E04 File Offset: 0x00002004
	private void GeneralOptionsGUI()
	{
		GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
		CheatsManager.ShowFPS = GUILayout.Toggle(CheatsManager.ShowFPS, new GUIContent("FPS Counter"), Array.Empty<GUILayoutOption>());
		CheatsManager.CanDeleteAllCards = GUILayout.Toggle(CheatsManager.CanDeleteAllCards, new GUIContent("All cards can be trashed"), Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(string.Format("Suns ({0})", GameLoad.Instance.SaveData.Suns.ToString()), Array.Empty<GUILayoutOption>());
		if (GUILayout.Button("+1", Array.Empty<GUILayoutOption>()))
		{
			GameLoad.Instance.SaveData.Suns++;
		}
		if (GUILayout.Button("+10", Array.Empty<GUILayoutOption>()))
		{
			GameLoad.Instance.SaveData.Suns += 10;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(string.Format("Moons ({0})", GameLoad.Instance.SaveData.Moons.ToString()), Array.Empty<GUILayoutOption>());
		if (GUILayout.Button("+1", Array.Empty<GUILayoutOption>()))
		{
			GameLoad.Instance.SaveData.Moons++;
		}
		if (GUILayout.Button("+10", Array.Empty<GUILayoutOption>()))
		{
			GameLoad.Instance.SaveData.Moons += 10;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003F78 File Offset: 0x00002178
	private void TimeGUI()
	{
		if (!this.GM)
		{
			return;
		}
		GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
		GUILayout.Label("Set time to:", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Day:", Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		if (GUILayout.RepeatButton("-", new GUILayoutOption[]
		{
			GUILayout.Width(25f)
		}) && Time.frameCount % 4 == 0)
		{
			this.SetTimeDay--;
		}
		GUILayout.Label(this.SetTimeDay.ToString(), new GUILayoutOption[]
		{
			GUILayout.Width(37.5f)
		});
		if (GUILayout.RepeatButton("+", new GUILayoutOption[]
		{
			GUILayout.Width(25f)
		}) && Time.frameCount % 4 == 0)
		{
			this.SetTimeDay++;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label(string.Format("Tick ({0}):", GameManager.TotalTicksToHourOfTheDayString(GameManager.HoursToTick((float)this.GM.DaySettings.DayStartingHour) + this.SetTimeTick, 0)), Array.Empty<GUILayoutOption>());
		GUILayout.FlexibleSpace();
		if (GUILayout.RepeatButton("-", new GUILayoutOption[]
		{
			GUILayout.Width(25f)
		}) && Time.frameCount % 4 == 0)
		{
			this.SetTimeTick--;
		}
		GUILayout.Label(this.SetTimeTick.ToString(), new GUILayoutOption[]
		{
			GUILayout.Width(37.5f)
		});
		if (GUILayout.RepeatButton("+", new GUILayoutOption[]
		{
			GUILayout.Width(25f)
		}) && Time.frameCount % 4 == 0)
		{
			this.SetTimeTick++;
		}
		GUILayout.EndHorizontal();
		if (this.GM)
		{
			this.SetTimeTick = Mathf.Clamp(this.SetTimeTick, 0, this.GM.DaySettings.DailyPoints);
			this.SetTimeDay = Mathf.Max(0, this.SetTimeDay);
		}
		else
		{
			GUILayout.Label("No GameManager found", Array.Empty<GUILayoutOption>());
		}
		if (GUILayout.Button("Set time!", Array.Empty<GUILayoutOption>()) && this.GM)
		{
			this.GM.SetTimeTo(this.SetTimeDay, this.SetTimeTick);
		}
		GUILayout.EndVertical();
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000041D0 File Offset: 0x000023D0
	private void CardsGUI()
	{
		if (!this.GM)
		{
			return;
		}
		if (this.AllCards == null)
		{
			this.FillCards();
		}
		if (this.AllCards == null)
		{
			return;
		}
		if (this.AllCards.Length == 0)
		{
			return;
		}
		GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
		GUILayout.Label("Cards", Array.Empty<GUILayoutOption>());
		GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
		GUILayout.Label("Search", Array.Empty<GUILayoutOption>());
		this.SearchedCardString = GUILayout.TextField(this.SearchedCardString, Array.Empty<GUILayoutOption>());
		GUILayout.EndHorizontal();
		this.CardsListScrollView = GUILayout.BeginScrollView(this.CardsListScrollView, new GUILayoutOption[]
		{
			GUILayout.ExpandHeight(true)
		});
		if (this.SearchedCardString == null)
		{
			this.SearchedCardString = "";
		}
		for (int i = 0; i < this.AllCards.Length; i++)
		{
			if (this.AllCards[i].name.ToLower().Contains(this.SearchedCardString.ToLower()))
			{
				if (i / 150 != this.CurrentPage && string.IsNullOrEmpty(this.SearchedCardString))
				{
					if (i >= 150 * this.CurrentPage)
					{
						break;
					}
				}
				else
				{
					GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
					GUILayout.Label(this.AllCards[i].name, Array.Empty<GUILayoutOption>());
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Give", Array.Empty<GUILayoutOption>()))
					{
						GameManager.GiveCard(this.AllCards[i], false);
					}
					if (this.AllCards[i].CardType != CardTypes.EnvImprovement)
					{
						if (GUILayout.Button("Give 5", Array.Empty<GUILayoutOption>()))
						{
							for (int j = 0; j < 5; j++)
							{
								GameManager.GiveCard(this.AllCards[i], false);
							}
						}
					}
					else if (GUILayout.Button("Give and complete", Array.Empty<GUILayoutOption>()))
					{
						GameManager.GiveCard(this.AllCards[i], true);
					}
					GUILayout.EndHorizontal();
				}
			}
		}
		GUILayout.EndScrollView();
		if (string.IsNullOrEmpty(this.SearchedCardString))
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (this.CurrentPage == 0)
			{
				GUILayout.Box("<", new GUILayoutOption[]
				{
					GUILayout.Width(25f)
				});
			}
			else if (GUILayout.Button("<", new GUILayoutOption[]
			{
				GUILayout.Width(25f)
			}))
			{
				this.CurrentPage--;
			}
			GUILayout.FlexibleSpace();
			GUILayout.Label(string.Format("{0}/{1}", (this.CurrentPage + 1).ToString(), this.MaxPages.ToString()), Array.Empty<GUILayoutOption>());
			GUILayout.FlexibleSpace();
			if (this.CurrentPage == this.MaxPages - 1)
			{
				GUILayout.Box(">", new GUILayoutOption[]
				{
					GUILayout.Width(25f)
				});
			}
			else if (GUILayout.Button(">", new GUILayoutOption[]
			{
				GUILayout.Width(25f)
			}))
			{
				this.CurrentPage++;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	// Token: 0x04000041 RID: 65
	[SerializeField]
	private bool CheatsOffInEditor;

	// Token: 0x04000042 RID: 66
	[SerializeField]
	private GameObject CheatsMenuBGObject;

	// Token: 0x04000043 RID: 67
	private bool CheatsAreOn;

	// Token: 0x04000044 RID: 68
	private CardData[] AllCards;

	// Token: 0x04000045 RID: 69
	private string SearchedCardString;

	// Token: 0x04000046 RID: 70
	private bool ShowGUI;

	// Token: 0x04000047 RID: 71
	private Vector2 CardsListScrollView;

	// Token: 0x04000048 RID: 72
	private const int CardsPerPage = 150;

	// Token: 0x04000049 RID: 73
	private int CurrentPage;

	// Token: 0x0400004A RID: 74
	private int MaxPages;

	// Token: 0x0400004B RID: 75
	private GameManager GM;

	// Token: 0x0400004C RID: 76
	private int SetTimeDay;

	// Token: 0x0400004D RID: 77
	private int SetTimeTick;

	// Token: 0x0400004E RID: 78
	private const float SmallButtonWidth = 25f;

	// Token: 0x0400004F RID: 79
	public static bool CanDeleteAllCards;

	// Token: 0x04000050 RID: 80
	public static bool ShowFPS;
}
