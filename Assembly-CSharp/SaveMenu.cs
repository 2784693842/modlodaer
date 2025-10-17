using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000035 RID: 53
public class SaveMenu : MonoBehaviour
{
	// Token: 0x06000276 RID: 630 RVA: 0x000184D4 File Offset: 0x000166D4
	private void OnEnable()
	{
		if (this.OtherScreens != null)
		{
			for (int i = 0; i < this.OtherScreens.Length; i++)
			{
				this.OtherScreens[i].SetActive(false);
			}
		}
		GraphicsManager.SetActiveGroup(this.MainScreen, true);
		if (GameLoad.Instance)
		{
			if (this.SunsCount)
			{
				this.SunsCount.text = GameLoad.Instance.SaveData.Suns.ToString();
			}
			if (this.MoonsCount)
			{
				this.MoonsCount.text = GameLoad.Instance.SaveData.Moons.ToString();
			}
		}
		if (this.LoadCheckpointButton)
		{
			this.LoadCheckpointButton.SetActive(MBSingleton<GameManager>.Instance.IsSafeMode);
		}
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0001859C File Offset: 0x0001679C
	public void WaitAndRefresh()
	{
		base.StartCoroutine(this.WaitAndRefreshRoutine());
	}

	// Token: 0x06000278 RID: 632 RVA: 0x000185AC File Offset: 0x000167AC
	public void Refresh()
	{
		this.GL = GameLoad.Instance;
		for (int i = 0; i < this.CurrentGameText.Length; i++)
		{
			if (this.CurrentGameText[i])
			{
				if (GameLoad.Instance.CurrentGameDataIndex >= 0)
				{
					this.CurrentGameText[i].text = "Current slot: " + (GameLoad.Instance.CurrentGameDataIndex + 1).ToString();
				}
				else
				{
					this.CurrentGameText[i].text = "Current slot: -1 (autosaves in slot " + 5.ToString() + ")";
				}
			}
		}
		for (int j = 0; j <= this.GL.Games.Count; j++)
		{
			if (this.LoadingButtons.Count <= j && j < this.GL.Games.Count)
			{
				this.LoadingButtons.Add(UnityEngine.Object.Instantiate<SaveButton>(this.ButtonPrefab, this.LoadButtonsParent));
			}
			if (this.SavingButtons.Count <= j)
			{
				this.SavingButtons.Add(UnityEngine.Object.Instantiate<SaveButton>(this.ButtonPrefab, this.SaveButtonsParent));
			}
			if (j < this.GL.Games.Count)
			{
				if (this.GL.Games[j].MainData.HasCardsData)
				{
					this.LoadingButtons[j].Setup(this.GL.Games[j].MainData, j, this, false);
					this.SavingButtons[j].Setup(this.GL.Games[j].MainData, j, this, true);
				}
				else
				{
					this.LoadingButtons[j].gameObject.SetActive(false);
					this.SavingButtons[j].gameObject.SetActive(false);
				}
			}
			else
			{
				this.SavingButtons[j].Setup(null, j, this, true);
			}
		}
		for (int k = this.GL.Games.Count; k < this.SavingButtons.Count; k++)
		{
			if (k > this.GL.Games.Count)
			{
				this.SavingButtons[k].gameObject.SetActive(false);
			}
			if (k < this.LoadingButtons.Count)
			{
				this.LoadingButtons[k].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0001880F File Offset: 0x00016A0F
	private IEnumerator WaitAndRefreshRoutine()
	{
		yield return null;
		this.Refresh();
		yield break;
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00018820 File Offset: 0x00016A20
	public void QuitGame(bool _Save)
	{
		if (MBSingleton<GameManager>.Instance)
		{
			MBSingleton<GameManager>.Instance.QuitGame();
		}
		if (_Save)
		{
			this.GL.AutoSaveGame(false);
			SceneManager.LoadScene(this.GL.MenuSceneIndex);
			return;
		}
		SceneManager.LoadScene(this.GL.MenuSceneIndex);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00018874 File Offset: 0x00016A74
	public void LoadCheckpoint()
	{
		GameLoad.Instance.LoadCheckpoint();
	}

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	private SaveButton ButtonPrefab;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	private RectTransform SaveButtonsParent;

	// Token: 0x040002A3 RID: 675
	[SerializeField]
	private RectTransform LoadButtonsParent;

	// Token: 0x040002A4 RID: 676
	[SerializeField]
	private TextMeshProUGUI[] CurrentGameText;

	// Token: 0x040002A5 RID: 677
	[SerializeField]
	private GameObject[] MainScreen;

	// Token: 0x040002A6 RID: 678
	[SerializeField]
	private GameObject[] OtherScreens;

	// Token: 0x040002A7 RID: 679
	[SerializeField]
	private TextMeshProUGUI SunsCount;

	// Token: 0x040002A8 RID: 680
	[SerializeField]
	private TextMeshProUGUI MoonsCount;

	// Token: 0x040002A9 RID: 681
	[SerializeField]
	private GameObject LoadCheckpointButton;

	// Token: 0x040002AA RID: 682
	private GameLoad GL;

	// Token: 0x040002AB RID: 683
	private List<SaveButton> SavingButtons = new List<SaveButton>();

	// Token: 0x040002AC RID: 684
	private List<SaveButton> LoadingButtons = new List<SaveButton>();
}
