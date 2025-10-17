using System;
using TMPro;
using UnityEngine;

// Token: 0x0200003D RID: 61
public class ActionConfirmPopup : MonoBehaviour
{
	// Token: 0x0600029F RID: 671 RVA: 0x00019600 File Offset: 0x00017800
	public void Setup(string _ActionName, string _ActionDuration, int _ActionIndex, bool _StackVersion, InspectionPopup _InspectionWindow)
	{
		this.Index = _ActionIndex;
		this.InspectionWindow = _InspectionWindow;
		if (!string.IsNullOrEmpty(_ActionDuration))
		{
			this.ActionName.text = string.Format("{0} ({1})", _ActionName, _ActionDuration);
		}
		else
		{
			this.ActionName.text = _ActionName;
		}
		this.Stack = _StackVersion;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00019654 File Offset: 0x00017854
	public void Confirm()
	{
		if (this.InspectionWindow)
		{
			this.InspectionWindow.OnButtonClicked(this.Index, this.Stack);
		}
		else if (MBSingleton<ExplorationPopup>.Instance && this.IsExplorationConfirm)
		{
			MBSingleton<ExplorationPopup>.Instance.OnActionButtonClicked(this.Index);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x040002F2 RID: 754
	[SerializeField]
	private TextMeshProUGUI ActionName;

	// Token: 0x040002F3 RID: 755
	[SerializeField]
	private bool IsExplorationConfirm;

	// Token: 0x040002F4 RID: 756
	private int Index;

	// Token: 0x040002F5 RID: 757
	private InspectionPopup InspectionWindow;

	// Token: 0x040002F6 RID: 758
	private bool Stack;
}
