using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000162 RID: 354
public class AsyncLoading : MBSingleton<AsyncLoading>
{
	// Token: 0x060009C8 RID: 2504 RVA: 0x000596C8 File Offset: 0x000578C8
	public static void LoadScene(int _Index)
	{
		if (!MBSingleton<AsyncLoading>.Instance)
		{
			SceneManager.LoadScene(_Index);
			return;
		}
		if (MBSingleton<AsyncLoading>.Instance.LoadingSceneIndex > -1)
		{
			SceneManager.LoadScene(MBSingleton<AsyncLoading>.Instance.LoadingSceneIndex);
		}
		if (MBSingleton<AsyncLoading>.Instance.TipsText)
		{
			MBSingleton<AsyncLoading>.Instance.TipsText.text = GuideManager.GetTip(0);
		}
		MBSingleton<AsyncLoading>.Instance.StartCoroutine(MBSingleton<AsyncLoading>.Instance.LoadSceneRoutine(_Index));
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0000B946 File Offset: 0x00009B46
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00059740 File Offset: 0x00057940
	private IEnumerator LoadSceneRoutine(int _Index)
	{
		this.LoadingOperation = SceneManager.LoadSceneAsync(_Index);
		this.TipsVersion.SetActive(GuideManager.TipsCount > 0);
		this.NoTipsVersion.SetActive(GuideManager.TipsCount <= 0);
		if (this.LoadingVisuals)
		{
			this.LoadingVisuals.gameObject.SetActive(true);
		}
		while (!this.LoadingOperation.isDone)
		{
			if (this.LoadingBar)
			{
				this.LoadingBar.fillAmount = this.LoadingOperation.progress;
			}
			if (this.PercentText)
			{
				this.PercentText.text = string.Format("{0}%", Mathf.FloorToInt(this.LoadingOperation.progress * 100f));
			}
			yield return null;
		}
		if (this.LoadingVisuals)
		{
			this.LoadingVisuals.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x04000F4A RID: 3914
	[SerializeField]
	private GameObject LoadingVisuals;

	// Token: 0x04000F4B RID: 3915
	[SerializeField]
	private Image LoadingBar;

	// Token: 0x04000F4C RID: 3916
	[SerializeField]
	private TextMeshProUGUI PercentText;

	// Token: 0x04000F4D RID: 3917
	[SerializeField]
	private int LoadingSceneIndex = -1;

	// Token: 0x04000F4E RID: 3918
	[SerializeField]
	private GameObject TipsVersion;

	// Token: 0x04000F4F RID: 3919
	[SerializeField]
	private GameObject NoTipsVersion;

	// Token: 0x04000F50 RID: 3920
	[SerializeField]
	private TextMeshProUGUI TipsText;

	// Token: 0x04000F51 RID: 3921
	private AsyncOperation LoadingOperation;
}
