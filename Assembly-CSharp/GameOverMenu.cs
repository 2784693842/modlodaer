using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000080 RID: 128
public class GameOverMenu : MBSingleton<GameOverMenu>
{
	// Token: 0x06000527 RID: 1319 RVA: 0x00034BF6 File Offset: 0x00032DF6
	public void Init()
	{
		if (MBSingleton<GameOverMenu>.PrivateInstance)
		{
			return;
		}
		MBSingleton<GameOverMenu>.PrivateInstance = this;
		this.GL = GameLoad.Instance;
		base.gameObject.SetActive(false);
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000528 RID: 1320 RVA: 0x00034C22 File Offset: 0x00032E22
	public static bool IsGameOver
	{
		get
		{
			return MBSingleton<GameOverMenu>.PrivateInstance && MBSingleton<GameOverMenu>.PrivateInstance.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00034C44 File Offset: 0x00032E44
	public void Setup(string _Cause, int _Suns, int _Moons)
	{
		this.KeepPlayingButton.SetActive(MBSingleton<CheatsManager>.Instance.CheatsActive);
		StringBuilder stringBuilder = new StringBuilder();
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
			this.SunsEarned.transform.parent.gameObject.SetActive(_Suns >= 0);
			this.MoonsEarned.transform.parent.gameObject.SetActive(_Moons >= 0);
			this.SunsEarned.text = _Suns.ToString();
			this.MoonsEarned.text = _Moons.ToString();
		}
		this.CauseText.text = stringBuilder.ToString();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00034D43 File Offset: 0x00032F43
	public void QuitGame()
	{
		this.GL.DeleteGameData(this.GL.CurrentGameDataIndex);
		SceneManager.LoadScene(this.GL.MenuSceneIndex);
	}

	// Token: 0x040006A5 RID: 1701
	[SerializeField]
	private GameObject KeepPlayingButton;

	// Token: 0x040006A6 RID: 1702
	[SerializeField]
	private TextMeshProUGUI CauseText;

	// Token: 0x040006A7 RID: 1703
	[SerializeField]
	private GameObject SunsAndMoons;

	// Token: 0x040006A8 RID: 1704
	[SerializeField]
	private TextMeshProUGUI SunsEarned;

	// Token: 0x040006A9 RID: 1705
	[SerializeField]
	private TextMeshProUGUI MoonsEarned;

	// Token: 0x040006AA RID: 1706
	private GameLoad GL;
}
