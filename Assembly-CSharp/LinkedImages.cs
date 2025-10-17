using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200008C RID: 140
[RequireComponent(typeof(Image))]
public class LinkedImages : MonoBehaviour
{
	// Token: 0x060005E8 RID: 1512 RVA: 0x0003E128 File Offset: 0x0003C328
	private void Awake()
	{
		this.MyImage = base.GetComponent<Image>();
		this.UpdateImages();
		this.OnDirtyVertices = (UnityAction)Delegate.Combine(this.OnDirtyVertices, new UnityAction(this.UpdateImages));
		this.MyImage.RegisterDirtyVerticesCallback(this.OnDirtyVertices);
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0003E17A File Offset: 0x0003C37A
	private void OnEnable()
	{
		this.UpdateImages();
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0003E184 File Offset: 0x0003C384
	private void UpdateImages()
	{
		if (this.MyImage.sprite != this.MySprite)
		{
			this.MySprite = this.MyImage.sprite;
			for (int i = 0; i < this.LinkedWith.Length; i++)
			{
				if (this.LinkedWith[i].Target)
				{
					this.LinkedWith[i].Target.sprite = this.MySprite;
				}
			}
		}
		if (this.MyImage.overrideSprite != this.MyOverrideSprite)
		{
			this.MyOverrideSprite = this.MyImage.overrideSprite;
			for (int j = 0; j < this.LinkedWith.Length; j++)
			{
				if (this.LinkedWith[j].Target)
				{
					this.LinkedWith[j].Target.overrideSprite = this.MyOverrideSprite;
				}
			}
		}
		if (this.MyImage.color != this.MyColor)
		{
			this.MyColor = this.MyImage.color;
			for (int k = 0; k < this.LinkedWith.Length; k++)
			{
				if (this.LinkedWith[k].Target && this.LinkedWith[k].LinkColor)
				{
					this.LinkedWith[k].Target.color = this.MyColor;
				}
			}
		}
	}

	// Token: 0x040007E2 RID: 2018
	[SerializeField]
	private LinkedImages.LinkedImage[] LinkedWith;

	// Token: 0x040007E3 RID: 2019
	private Image MyImage;

	// Token: 0x040007E4 RID: 2020
	private Sprite MySprite;

	// Token: 0x040007E5 RID: 2021
	private Sprite MyOverrideSprite;

	// Token: 0x040007E6 RID: 2022
	private Color MyColor;

	// Token: 0x040007E7 RID: 2023
	private UnityAction OnDirtyVertices;

	// Token: 0x0200026E RID: 622
	[Serializable]
	private class LinkedImage
	{
		// Token: 0x04001485 RID: 5253
		public Image Target;

		// Token: 0x04001486 RID: 5254
		public bool LinkColor;
	}
}
