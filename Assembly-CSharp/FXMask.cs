using System;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class FXMask : MonoBehaviour
{
	// Token: 0x06000A2F RID: 2607 RVA: 0x0005B250 File Offset: 0x00059450
	private void Awake()
	{
		this.MyRectTr = base.GetComponent<RectTransform>();
		this.AmbienceEffects = MBSingleton<AmbienceImageEffect>.Instance;
		this.MaskObject = UnityEngine.Object.Instantiate<SpriteMask>(this.AmbienceEffects.MaskPrefab, this.AmbienceEffects.WeatherEffectsParent);
		if (!GameManager.DontRenameGOs)
		{
			this.MaskObject.name = base.name + "_Mask";
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0005B2B7 File Offset: 0x000594B7
	private void OnEnable()
	{
		if (this.MaskObject)
		{
			this.MaskObject.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0005B2D7 File Offset: 0x000594D7
	private void OnDisable()
	{
		if (this.MaskObject)
		{
			this.MaskObject.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0005B2F7 File Offset: 0x000594F7
	private void OnDestroy()
	{
		if (this.MaskObject)
		{
			UnityEngine.Object.Destroy(this.MaskObject.gameObject);
		}
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0005B318 File Offset: 0x00059518
	private void LateUpdate()
	{
		if (!this.MaskObject)
		{
			return;
		}
		this.WorldRect = new Rect(base.transform.TransformPoint(this.MyRectTr.rect.position), base.transform.TransformVector(this.MyRectTr.rect.size));
		this.MaskObject.transform.localPosition = this.WorldRect.center;
		this.MaskObject.transform.localScale = this.AmbienceEffects.ScaleForMask(this.WorldRect.size);
		if (this.AlphaGroup)
		{
			this.MaskObject.gameObject.SetActive(this.AlphaGroup.alpha >= this.TransparencyActiveValue);
		}
	}

	// Token: 0x04000FA7 RID: 4007
	[SerializeField]
	private CanvasGroup AlphaGroup;

	// Token: 0x04000FA8 RID: 4008
	[SerializeField]
	private float TransparencyActiveValue;

	// Token: 0x04000FA9 RID: 4009
	private RectTransform MyRectTr;

	// Token: 0x04000FAA RID: 4010
	private AmbienceImageEffect AmbienceEffects;

	// Token: 0x04000FAB RID: 4011
	private SpriteMask MaskObject;

	// Token: 0x04000FAC RID: 4012
	private Rect WorldRect;
}
