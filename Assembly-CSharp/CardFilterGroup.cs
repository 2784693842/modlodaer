using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FF RID: 255
[CreateAssetMenu(menuName = "Survival/Card Filter Group")]
public class CardFilterGroup : ScriptableObject
{
	// Token: 0x06000856 RID: 2134 RVA: 0x00052650 File Offset: 0x00050850
	public Color GetColor(Color _WithAlpha)
	{
		return new Color(this.FilterColor.r, this.FilterColor.g, this.FilterColor.b, _WithAlpha.a);
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0005267E File Offset: 0x0005087E
	[ContextMenu("Toggle Filter")]
	public void ToggleFilter()
	{
		if (MBSingleton<GraphicsManager>.Instance && Application.isPlaying)
		{
			MBSingleton<GraphicsManager>.Instance.ToggleFilterTag(this);
		}
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0005269E File Offset: 0x0005089E
	public bool Contains(CardData _Card)
	{
		return this.IncludedCards != null && this.IncludedCards.Count != 0 && this.IncludedCards.Contains(_Card);
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x000526C5 File Offset: 0x000508C5
	[ContextMenu("Sort alphabetically")]
	public void SortList()
	{
		this.IncludedCards.Sort((CardData a, CardData b) => a.name.CompareTo(b.name));
	}

	// Token: 0x04000CA9 RID: 3241
	public LocalizedString GameName;

	// Token: 0x04000CAA RID: 3242
	public Sprite FilterIcon;

	// Token: 0x04000CAB RID: 3243
	[SerializeField]
	private Color FilterColor;

	// Token: 0x04000CAC RID: 3244
	public List<CardData> IncludedCards = new List<CardData>();
}
