using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
[Serializable]
public struct PieceOfInfo
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x060008C1 RID: 2241 RVA: 0x000547A8 File Offset: 0x000529A8
	public bool CanShow
	{
		get
		{
			if (this.Conditions == null)
			{
				return true;
			}
			if (this.Conditions.Length == 0)
			{
				return true;
			}
			for (int i = 0; i < this.Conditions.Length; i++)
			{
				if (!this.Conditions[i].Complete)
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x04000D35 RID: 3381
	public float SpaceBtwnElements;

	// Token: 0x04000D36 RID: 3382
	public LocalizedString Title;

	// Token: 0x04000D37 RID: 3383
	public ContentDisplayOptions InfoDisplay;

	// Token: 0x04000D38 RID: 3384
	public bool OverrideAlignment;

	// Token: 0x04000D39 RID: 3385
	public TextAnchor ContentAlignment;

	// Token: 0x04000D3A RID: 3386
	public Sprite Image;

	// Token: 0x04000D3B RID: 3387
	public LocalizedString Content;

	// Token: 0x04000D3C RID: 3388
	public float ImageSizeMultiplier;

	// Token: 0x04000D3D RID: 3389
	public Objective RelatedObjective;

	// Token: 0x04000D3E RID: 3390
	public ObjectiveAnchor AnchorObjectiveTo;

	// Token: 0x04000D3F RID: 3391
	public ObjectiveDisplay ObjectiveDisplayOptions;

	// Token: 0x04000D40 RID: 3392
	public ContentPageLink[] PageLinks;

	// Token: 0x04000D41 RID: 3393
	public WebLink[] WebLinks;

	// Token: 0x04000D42 RID: 3394
	public bool StretchButtons;

	// Token: 0x04000D43 RID: 3395
	public float SpaceBtwnButtons;

	// Token: 0x04000D44 RID: 3396
	public int[] OrderOfElements;

	// Token: 0x04000D45 RID: 3397
	[Space]
	public ObjectiveCondition[] Conditions;

	// Token: 0x04000D46 RID: 3398
	public static string[] Elements = new string[]
	{
		"Image",
		"Content",
		"Buttons"
	};
}
