using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000195 RID: 405
public class MaskFrameImageResizer : MonoBehaviour
{
	// Token: 0x06000B60 RID: 2912 RVA: 0x00060D97 File Offset: 0x0005EF97
	private void OnEnable()
	{
		base.StartCoroutine(this.UpdateAfterOneFrame());
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00060DA6 File Offset: 0x0005EFA6
	private IEnumerator UpdateAfterOneFrame()
	{
		yield return null;
		this.UpdateImageSprite();
		this.UpdateImageSize();
		yield break;
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00060DB5 File Offset: 0x0005EFB5
	// (set) Token: 0x06000B63 RID: 2915 RVA: 0x00060DD1 File Offset: 0x0005EFD1
	public Sprite sprite
	{
		get
		{
			if (!this.ImageObject)
			{
				return null;
			}
			return this.ImageObject.sprite;
		}
		set
		{
			this.ImageObject.sprite = value;
			this.UpdateImageSprite();
			this.UpdateImageSize();
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00060DEB File Offset: 0x0005EFEB
	// (set) Token: 0x06000B65 RID: 2917 RVA: 0x00060E07 File Offset: 0x0005F007
	public Sprite overrideSprite
	{
		get
		{
			if (!this.ImageObject)
			{
				return null;
			}
			return this.ImageObject.overrideSprite;
		}
		set
		{
			this.ImageObject.overrideSprite = value;
			this.UpdateImageSprite();
			this.UpdateImageSize();
		}
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00060E24 File Offset: 0x0005F024
	private void UpdateImageSprite()
	{
		if (!this.ImageObject)
		{
			this.CurrentImageSprite = null;
			return;
		}
		this.CurrentImageSprite = (this.ImageObject.overrideSprite ? this.ImageObject.overrideSprite : this.ImageObject.sprite);
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00060E78 File Offset: 0x0005F078
	private void UpdateImageSize()
	{
		if (!this.ImageObject || !this.FrameRect)
		{
			return;
		}
		if (!this.ImageRect)
		{
			this.ImageRect = this.ImageObject.GetComponent<RectTransform>();
		}
		float num;
		RectTransform.Axis axis;
		RectTransform.Axis axis2;
		float num2;
		if (this.CurrentImageSprite.rect.width >= this.CurrentImageSprite.rect.height)
		{
			num = this.CurrentImageSprite.rect.width / this.CurrentImageSprite.rect.height;
			axis = RectTransform.Axis.Vertical;
			axis2 = RectTransform.Axis.Horizontal;
			num2 = this.FrameRect.rect.height;
		}
		else
		{
			num = this.CurrentImageSprite.rect.height / this.CurrentImageSprite.rect.width;
			axis = RectTransform.Axis.Horizontal;
			axis2 = RectTransform.Axis.Vertical;
			num2 = this.FrameRect.rect.width;
		}
		this.ImageRect.SetSizeWithCurrentAnchors(axis, num2);
		this.ImageRect.SetSizeWithCurrentAnchors(axis2, num2 * num);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00060F9C File Offset: 0x0005F19C
	private void LateUpdate()
	{
		if (!this.AutoResizeOnUpdate || !this.ImageObject)
		{
			return;
		}
		if (this.ImageObject.overrideSprite)
		{
			if (this.ImageObject.overrideSprite != this.CurrentImageSprite)
			{
				this.UpdateImageSprite();
				this.UpdateImageSize();
				return;
			}
		}
		else if (this.ImageObject.sprite != this.CurrentImageSprite)
		{
			this.UpdateImageSprite();
			this.UpdateImageSize();
		}
	}

	// Token: 0x0400105F RID: 4191
	[SerializeField]
	private Image ImageObject;

	// Token: 0x04001060 RID: 4192
	[SerializeField]
	private RectTransform FrameRect;

	// Token: 0x04001061 RID: 4193
	[SerializeField]
	private bool AutoResizeOnUpdate;

	// Token: 0x04001062 RID: 4194
	private RectTransform ImageRect;

	// Token: 0x04001063 RID: 4195
	private Sprite CurrentImageSprite;
}
