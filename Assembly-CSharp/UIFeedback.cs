using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class UIFeedback : MonoBehaviour
{
	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000700 RID: 1792 RVA: 0x00047C81 File Offset: 0x00045E81
	// (set) Token: 0x06000701 RID: 1793 RVA: 0x00047C89 File Offset: 0x00045E89
	public bool IsPlaying { get; protected set; }

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000702 RID: 1794 RVA: 0x00047C92 File Offset: 0x00045E92
	// (set) Token: 0x06000703 RID: 1795 RVA: 0x00047C9A File Offset: 0x00045E9A
	public float Progress { get; protected set; }

	// Token: 0x06000704 RID: 1796 RVA: 0x00047CA4 File Offset: 0x00045EA4
	protected virtual void Awake()
	{
		if (this.PlayObject)
		{
			this.PlayObject.SetActive(false);
			if (this.ResetOnPlay)
			{
				this.PlayObjectBackup = UnityEngine.Object.Instantiate<GameObject>(this.PlayObject, this.PlayObject.transform.parent);
			}
		}
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00047CF3 File Offset: 0x00045EF3
	private void OnDisable()
	{
		this.StopPlaying();
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00047CFB File Offset: 0x00045EFB
	public IEnumerator PlayFeedback(Vector3 _AtPosition)
	{
		base.transform.position = _AtPosition;
		yield return base.StartCoroutine(this.PlayFeedback());
		yield break;
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00047D11 File Offset: 0x00045F11
	public IEnumerator PlayFeedback()
	{
		if (this.IsPlaying)
		{
			if (!this.ResetOnPlay)
			{
				yield break;
			}
			if (this.PlayObjectBackup)
			{
				UnityEngine.Object.Destroy(this.PlayObject.gameObject);
				yield return null;
				this.PlayObject = this.PlayObjectBackup;
			}
			if (this.PlayObject)
			{
				this.PlayObjectBackup = UnityEngine.Object.Instantiate<GameObject>(this.PlayObject, this.PlayObject.transform.parent);
			}
		}
		if (this.Delay > 0f)
		{
			yield return new WaitForSeconds(this.Delay);
		}
		this.StartPlaying();
		this.PlayTimer = 0f;
		while (this.PlayTimer < this.Duration || this.Duration <= 0f)
		{
			this.PlayTimer += Time.deltaTime;
			this.UpdatePlaying(this.PlayTimer);
			yield return null;
		}
		this.IsPlaying = false;
		this.StopPlaying();
		yield break;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x00047D20 File Offset: 0x00045F20
	protected virtual void StartPlaying()
	{
		this.IsPlaying = true;
		if (this.PlayObject)
		{
			this.PlayObject.SetActive(true);
		}
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00047D42 File Offset: 0x00045F42
	protected virtual void UpdatePlaying(float _Time)
	{
		this.Progress = Mathf.Clamp01(_Time / this.Duration);
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00047D58 File Offset: 0x00045F58
	public virtual void StopPlaying()
	{
		if (this.IsPlaying)
		{
			this.IsPlaying = false;
			base.StopAllCoroutines();
		}
		if (this.DestroyAfterPlaying)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.PlayObject)
		{
			this.PlayObject.SetActive(false);
		}
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00047DA8 File Offset: 0x00045FA8
	public virtual void ResetPlaying()
	{
		if (!this.IsPlaying)
		{
			return;
		}
		this.PlayTimer = 0f;
		if (this.AnimationsToReset != null)
		{
			for (int i = 0; i < this.AnimationsToReset.Length; i++)
			{
				if (this.AnimationsToReset[i])
				{
					this.AnimationsToReset[i].tween.Restart(true, -1f);
				}
			}
		}
	}

	// Token: 0x040009C9 RID: 2505
	public float Delay;

	// Token: 0x040009CA RID: 2506
	public float Duration;

	// Token: 0x040009CB RID: 2507
	public float YieldWaitDuration;

	// Token: 0x040009CC RID: 2508
	public bool DestroyAfterPlaying;

	// Token: 0x040009CD RID: 2509
	public GameObject PlayObject;

	// Token: 0x040009CE RID: 2510
	public bool ResetOnPlay;

	// Token: 0x040009CF RID: 2511
	public DOTweenAnimation[] AnimationsToReset;

	// Token: 0x040009D2 RID: 2514
	private GameObject PlayObjectBackup;

	// Token: 0x040009D3 RID: 2515
	protected float PlayTimer;
}
