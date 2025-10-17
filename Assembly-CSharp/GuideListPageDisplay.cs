using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000088 RID: 136
public class GuideListPageDisplay : DynamicViewLayoutGroup
{
	// Token: 0x0600059C RID: 1436 RVA: 0x0003B617 File Offset: 0x00039817
	public void Init(GuideDisplayer _Parent, ref List<GuideEntry> _PagesList)
	{
		this.ParentDisplayer = _Parent;
		this.EntriesList = _PagesList;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0003B628 File Offset: 0x00039828
	protected override void OnElementVisible(int _Index)
	{
		base.OnElementVisible(_Index);
		if (this.AllElements[_Index].ElementObject.GetType() != typeof(DynamicViewIndexButton))
		{
			return;
		}
		DynamicViewIndexButton dynamicViewIndexButton = this.AllElements[_Index].ElementObject as DynamicViewIndexButton;
		dynamicViewIndexButton.Setup(_Index, this.EntriesList[_Index].GetTitle, "");
		dynamicViewIndexButton.RegisterClickAction(new Action<int>(this.ClickButton));
		dynamicViewIndexButton.Selected = (_Index == this.SelectedEntry);
		if (_Index == this.SelectedEntry)
		{
			this.SelectedButton = dynamicViewIndexButton.Button;
		}
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0003B6D0 File Offset: 0x000398D0
	protected override void OnElementNotVisible(int _Index)
	{
		DynamicViewIndexButton dynamicViewIndexButton = this.AllElements[_Index].ElementObject as DynamicViewIndexButton;
		dynamicViewIndexButton.UnregisterClickAction(new Action<int>(this.ClickButton));
		if (this.SelectedButton == dynamicViewIndexButton.Button)
		{
			this.SelectedButton = null;
		}
		base.OnElementNotVisible(_Index);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0003B727 File Offset: 0x00039927
	private void ClickButton(int _Index)
	{
		if (this.ParentDisplayer)
		{
			this.ParentDisplayer.OpenPage(GuideManager.PagesDict[this.EntriesList[_Index]]);
		}
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0003B758 File Offset: 0x00039958
	public void SelectEntry(int _Index)
	{
		if (this.SelectedButton)
		{
			this.SelectedButton.Selected = (_Index == this.SelectedButton.Index);
		}
		this.SelectedEntry = _Index;
		if (_Index < 0 || _Index >= this.AllElements.Count)
		{
			this.SelectedButton = null;
			return;
		}
		if (this.AllElements[_Index].ElementObject)
		{
			DynamicViewIndexButton dynamicViewIndexButton = this.AllElements[_Index].ElementObject as DynamicViewIndexButton;
			dynamicViewIndexButton.Button.Selected = true;
			this.SelectedButton = dynamicViewIndexButton.Button;
		}
		this.MoveViewTo(_Index, true, null);
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0003B7FC File Offset: 0x000399FC
	public void MoveViewTo(int _Entry, bool _ForceCenter, TweenCallback _OnComplete)
	{
		if (_Entry < 0 || _Entry >= base.Count)
		{
			return;
		}
		if (!this.AllElements[_Entry].IsActive)
		{
			return;
		}
		if (this.WorldMaskRect.Contains(base.GetElementWorldRect(_Entry).center) && !_ForceCenter)
		{
			this.ScrollView.DOKill(false);
			return;
		}
		this.MoveToPos(base.ElementScrollPosition(_Entry), 0f, Ease.InOutSine, _OnComplete);
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0003B86C File Offset: 0x00039A6C
	public void MoveToPos(float _Pos, float _Time, Ease _Ease, TweenCallback _OnComplete)
	{
		if (!this.ScrollView)
		{
			return;
		}
		this.ScrollView.DOKill(false);
		if (this.LayoutOrientation == RectTransform.Axis.Horizontal)
		{
			if (_OnComplete == null)
			{
				this.ScrollView.DOHorizontalNormalizedPos(Mathf.Clamp01(_Pos), _Time, false).SetEase(_Ease);
				return;
			}
			this.ScrollView.DOHorizontalNormalizedPos(Mathf.Clamp01(_Pos), _Time, false).SetEase(_Ease).OnComplete(_OnComplete);
			return;
		}
		else
		{
			if (_OnComplete == null)
			{
				this.ScrollView.DOVerticalNormalizedPos(Mathf.Clamp01(_Pos), _Time, false).SetEase(_Ease);
				return;
			}
			this.ScrollView.DOVerticalNormalizedPos(Mathf.Clamp01(_Pos), _Time, false).SetEase(_Ease).OnComplete(_OnComplete);
			return;
		}
	}

	// Token: 0x0400079C RID: 1948
	[SerializeField]
	private ScrollRect ScrollView;

	// Token: 0x0400079D RID: 1949
	private GuideDisplayer ParentDisplayer;

	// Token: 0x0400079E RID: 1950
	[NonSerialized]
	public int SelectedEntry = -1;

	// Token: 0x0400079F RID: 1951
	private IndexButton SelectedButton;

	// Token: 0x040007A0 RID: 1952
	private List<GuideEntry> EntriesList;
}
