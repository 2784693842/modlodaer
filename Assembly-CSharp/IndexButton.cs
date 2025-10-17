using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200018D RID: 397
public class IndexButton : TooltipButton
{
	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0005DF34 File Offset: 0x0005C134
	// (set) Token: 0x06000A8C RID: 2700 RVA: 0x0005DF3C File Offset: 0x0005C13C
	public int Index { get; private set; }

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0005DF45 File Offset: 0x0005C145
	// (set) Token: 0x06000A8E RID: 2702 RVA: 0x0005DF64 File Offset: 0x0005C164
	public bool Selected
	{
		get
		{
			return this.OnSelected && this.OnSelected.activeSelf;
		}
		set
		{
			if (this.OnSelected)
			{
				this.OnSelected.SetActive(value);
			}
			if (this.InteractionImage)
			{
				this.InteractionImage.color = new Color(this.InteractionImage.color.r, this.InteractionImage.color.g, this.InteractionImage.color.b, value ? 0f : 1f);
			}
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0005DFE6 File Offset: 0x0005C1E6
	// (set) Token: 0x06000A90 RID: 2704 RVA: 0x0005E002 File Offset: 0x0005C202
	public Sprite Sprite
	{
		get
		{
			if (!this.Icon)
			{
				return null;
			}
			return this.Icon.overrideSprite;
		}
		set
		{
			if (this.Icon)
			{
				this.Icon.overrideSprite = value;
			}
		}
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0005E01D File Offset: 0x0005C21D
	public void Setup(int _Index, string _Text, string _Tooltip, bool _Highlighted)
	{
		this.Index = _Index;
		base.Setup(_Text, _Tooltip, _Highlighted);
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0005E030 File Offset: 0x0005C230
	public void Click()
	{
		Action<int> onClicked = this.OnClicked;
		if (onClicked == null)
		{
			return;
		}
		onClicked(this.Index);
	}

	// Token: 0x04001040 RID: 4160
	[SerializeField]
	protected Image Icon;

	// Token: 0x04001041 RID: 4161
	[SerializeField]
	private GameObject OnSelected;

	// Token: 0x04001042 RID: 4162
	[SerializeField]
	private Image InteractionImage;

	// Token: 0x04001043 RID: 4163
	public Action<int> OnClicked;
}
