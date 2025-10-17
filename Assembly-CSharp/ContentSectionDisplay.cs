using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000057 RID: 87
public class ContentSectionDisplay : MonoBehaviour
{
	// Token: 0x060003B4 RID: 948 RVA: 0x000271D4 File Offset: 0x000253D4
	public void Init(ContentSection _Section)
	{
		this.SectionParent = ((_Section.SectionDisplay == ContentDisplayOptions.Horizontal) ? this.HorizontalDisplay.transform : this.VerticalDisplay.transform);
		this.HorizontalDisplay.gameObject.SetActive(_Section.SectionDisplay == ContentDisplayOptions.Horizontal);
		this.VerticalDisplay.gameObject.SetActive(_Section.SectionDisplay == ContentDisplayOptions.Vertical);
		this.HorizontalDisplay.spacing = _Section.SpaceBtwnEntries + this.DefaultSpace;
		this.VerticalDisplay.spacing = _Section.SpaceBtwnEntries + this.DefaultSpace;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0002726A File Offset: 0x0002546A
	public void AddInfo(PieceOfInfoDisplay _Info)
	{
		_Info.transform.SetParent(this.SectionParent);
		_Info.transform.SetAsLastSibling();
	}

	// Token: 0x040004C6 RID: 1222
	[SerializeField]
	private HorizontalLayoutGroup HorizontalDisplay;

	// Token: 0x040004C7 RID: 1223
	[SerializeField]
	private VerticalLayoutGroup VerticalDisplay;

	// Token: 0x040004C8 RID: 1224
	[SerializeField]
	private float DefaultSpace;

	// Token: 0x040004C9 RID: 1225
	private Transform SectionParent;
}
