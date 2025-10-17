using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200009F RID: 159
public class PulsingButton : MonoBehaviour
{
	// Token: 0x06000678 RID: 1656 RVA: 0x00044864 File Offset: 0x00042A64
	private void Awake()
	{
		Button component = base.GetComponent<Button>();
		if (component)
		{
			component.onClick.AddListener(new UnityAction(this.DoPulse));
		}
		if (!this.PulsingTr)
		{
			this.PulsingTr = base.transform;
		}
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000448B0 File Offset: 0x00042AB0
	public void DoPulse()
	{
		this.PulsingTr.DOKill(true);
		this.PulsingTr.DOPunchScale(this.PulseScale, this.PulseDuration, this.PulseVibrato, this.PulseElasticity).SetEase(this.PulseEase);
	}

	// Token: 0x04000910 RID: 2320
	[SerializeField]
	private Transform PulsingTr;

	// Token: 0x04000911 RID: 2321
	[SerializeField]
	private float PulseDuration = 0.3f;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private Vector3 PulseScale = Vector3.one * 0.25f;

	// Token: 0x04000913 RID: 2323
	[SerializeField]
	private Ease PulseEase = Ease.Linear;

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private int PulseVibrato = 5;

	// Token: 0x04000915 RID: 2325
	[SerializeField]
	private float PulseElasticity = 0.5f;
}
