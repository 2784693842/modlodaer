using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200008A RID: 138
public class IconAndTextPair : MonoBehaviour
{
	// Token: 0x17000122 RID: 290
	// (get) Token: 0x060005AE RID: 1454 RVA: 0x0003BA79 File Offset: 0x00039C79
	// (set) Token: 0x060005AF RID: 1455 RVA: 0x0003BA95 File Offset: 0x00039C95
	public Sprite Sprite
	{
		get
		{
			if (!this.IconObject)
			{
				return null;
			}
			return this.IconObject.overrideSprite;
		}
		set
		{
			if (this.IconObject)
			{
				this.IconObject.overrideSprite = value;
			}
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0003BAB0 File Offset: 0x00039CB0
	// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0003BAD0 File Offset: 0x00039CD0
	public string Text
	{
		get
		{
			if (!this.TextObject)
			{
				return "";
			}
			return this.TextObject.text;
		}
		set
		{
			if (this.TextObject)
			{
				this.TextObject.text = value;
			}
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0003BAEB File Offset: 0x00039CEB
	// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0003BB0B File Offset: 0x00039D0B
	public Color TextColor
	{
		get
		{
			if (!this.TextObject)
			{
				return Color.white;
			}
			return this.TextObject.color;
		}
		set
		{
			if (this.TextObject)
			{
				this.TextObject.color = value;
			}
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x060005B4 RID: 1460 RVA: 0x0003BB26 File Offset: 0x00039D26
	// (set) Token: 0x060005B5 RID: 1461 RVA: 0x0003BB46 File Offset: 0x00039D46
	public Color IconColor
	{
		get
		{
			if (!this.IconObject)
			{
				return Color.white;
			}
			return this.IconObject.color;
		}
		set
		{
			if (this.IconObject)
			{
				this.IconObject.color = value;
			}
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x060005B6 RID: 1462 RVA: 0x0003BB61 File Offset: 0x00039D61
	public Transform TextTr
	{
		get
		{
			if (!this.TextObject)
			{
				return base.transform;
			}
			return this.TextObject.transform;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x060005B7 RID: 1463 RVA: 0x0003BB82 File Offset: 0x00039D82
	public Transform IconTr
	{
		get
		{
			if (!this.IconObject)
			{
				return base.transform;
			}
			return this.IconObject.transform;
		}
	}

	// Token: 0x040007AB RID: 1963
	[SerializeField]
	private Image IconObject;

	// Token: 0x040007AC RID: 1964
	[SerializeField]
	private TextMeshProUGUI TextObject;
}
