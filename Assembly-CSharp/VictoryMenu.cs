using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B4 RID: 180
public class VictoryMenu : MBSingleton<VictoryMenu>
{
	// Token: 0x06000730 RID: 1840 RVA: 0x00048255 File Offset: 0x00046455
	public void Init()
	{
		if (MBSingleton<VictoryMenu>.PrivateInstance)
		{
			return;
		}
		MBSingleton<VictoryMenu>.PrivateInstance = this;
		this.GL = GameLoad.Instance;
		base.gameObject.SetActive(false);
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x00048281 File Offset: 0x00046481
	public static bool IsVictory
	{
		get
		{
			return MBSingleton<VictoryMenu>.PrivateInstance && MBSingleton<VictoryMenu>.PrivateInstance.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x000482A0 File Offset: 0x000464A0
	public void Setup(string _Cause, int _Suns, int _Moons)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(LocalizedString.DaysSurvived(MBSingleton<GameManager>.Instance.CurrentDay));
		if (!string.IsNullOrEmpty(_Cause))
		{
			stringBuilder.Append("\n");
			stringBuilder.Append(_Cause);
		}
		this.CauseText.text = stringBuilder.ToString();
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
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0004838A File Offset: 0x0004658A
	public void QuitGame()
	{
		this.GL.DeleteGameData(this.GL.CurrentGameDataIndex);
		SceneManager.LoadScene(this.GL.MenuSceneIndex);
	}

	// Token: 0x040009DF RID: 2527
	[SerializeField]
	private TextMeshProUGUI CauseText;

	// Token: 0x040009E0 RID: 2528
	[SerializeField]
	private GameObject SunsAndMoons;

	// Token: 0x040009E1 RID: 2529
	[SerializeField]
	private TextMeshProUGUI SunsEarned;

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	private TextMeshProUGUI MoonsEarned;

	// Token: 0x040009E3 RID: 2531
	private GameLoad GL;
}
