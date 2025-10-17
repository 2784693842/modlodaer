using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AF RID: 175
public abstract class UIFeedbackStepsBase : UIFeedback
{
	// Token: 0x0600070D RID: 1805 RVA: 0x00047E0B File Offset: 0x0004600B
	public IEnumerator PlayFeedback(Vector3 _Pos, Sprite _Icon, int _Steps)
	{
		base.transform.position = _Pos;
		yield return this.PlayFeedback(_Icon, _Steps);
		yield break;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00047E2F File Offset: 0x0004602F
	public IEnumerator PlayFeedback(AmtFeedbackInfo _Info, float _Value)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			yield break;
		}
		int amtFeedback = _Info.GetAmtFeedback(_Value);
		if (amtFeedback == 0)
		{
			base.StopPlaying();
			yield break;
		}
		yield return base.StartCoroutine(this.PlayFeedback((amtFeedback < 0) ? _Info.NegativeIcon : _Info.Icon, amtFeedback));
		yield break;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00047E4C File Offset: 0x0004604C
	public IEnumerator PlayFeedback(Sprite _Icon, int _Steps)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			yield break;
		}
		if (_Steps == 0)
		{
			base.StopPlaying();
			yield break;
		}
		if (this.Icon)
		{
			this.Icon.sprite = _Icon;
		}
		yield return base.StartCoroutine(this.CustomStepsProcessing(_Icon, Mathf.Abs(_Steps), _Steps < 0));
		if (!base.gameObject.activeInHierarchy)
		{
			yield break;
		}
		yield return base.PlayFeedback();
		yield break;
	}

	// Token: 0x06000710 RID: 1808
	protected abstract IEnumerator CustomStepsProcessing(Sprite _Icon, int _Steps, bool _Negative);

	// Token: 0x06000711 RID: 1809
	protected abstract void OnStop();

	// Token: 0x06000712 RID: 1810 RVA: 0x00047E69 File Offset: 0x00046069
	public override void StopPlaying()
	{
		base.StopPlaying();
		this.OnStop();
	}

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	private Image Icon;
}
