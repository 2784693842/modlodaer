using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
[RequireComponent(typeof(IndexButton))]
public class DynamicViewIndexButton : DynamicViewLayoutElement
{
	// Token: 0x0600043D RID: 1085 RVA: 0x00029898 File Offset: 0x00027A98
	public void Setup(int _Index, string _Text, string _Tooltip)
	{
		if (!this.Button)
		{
			this.Button = base.GetComponent<IndexButton>();
		}
		this.Button.Setup(_Index, _Text, _Tooltip, false);
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x000298C2 File Offset: 0x00027AC2
	public void RegisterClickAction(Action<int> _Action)
	{
		if (!this.Button)
		{
			this.Button = base.GetComponent<IndexButton>();
		}
		IndexButton button = this.Button;
		button.OnClicked = (Action<int>)Delegate.Combine(button.OnClicked, _Action);
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x000298F9 File Offset: 0x00027AF9
	public void UnregisterClickAction(Action<int> _Action)
	{
		if (!this.Button)
		{
			this.Button = base.GetComponent<IndexButton>();
		}
		IndexButton button = this.Button;
		button.OnClicked = (Action<int>)Delegate.Remove(button.OnClicked, _Action);
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x06000440 RID: 1088 RVA: 0x00029930 File Offset: 0x00027B30
	// (set) Token: 0x06000441 RID: 1089 RVA: 0x00029956 File Offset: 0x00027B56
	public bool Selected
	{
		get
		{
			if (!this.Button)
			{
				this.Button = base.GetComponent<IndexButton>();
			}
			return this.Button.Selected;
		}
		set
		{
			if (!this.Button)
			{
				this.Button = base.GetComponent<IndexButton>();
			}
			this.Button.Selected = value;
		}
	}

	// Token: 0x04000522 RID: 1314
	[NonSerialized]
	public IndexButton Button;
}
