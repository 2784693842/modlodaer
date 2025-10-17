using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A2 RID: 418
public class PerfDebug : MonoBehaviour
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x000618D4 File Offset: 0x0005FAD4
	private void Update()
	{
		if (!Debug.isDebugBuild)
		{
			if (this.FeedbackImage)
			{
				this.FeedbackImage.gameObject.SetActive(false);
			}
			return;
		}
		if (!MBSingleton<GameManager>.Instance)
		{
			return;
		}
		if (this.FeedbackImage)
		{
			this.FeedbackImage.color = (MBSingleton<GameManager>.Instance.PoolCards ? Color.green : Color.red);
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			MBSingleton<GameManager>.Instance.PoolCards = !MBSingleton<GameManager>.Instance.PoolCards;
		}
	}

	// Token: 0x04001077 RID: 4215
	[SerializeField]
	private Image FeedbackImage;
}
