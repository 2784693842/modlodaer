using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
[Serializable]
public struct ActionHighlight
{
	// Token: 0x0600029E RID: 670 RVA: 0x00019578 File Offset: 0x00017778
	public bool IsActionHighlighted(ScriptableObject _FromObject, CardAction _Action)
	{
		if (this.TargetObjects == null)
		{
			return _Action.ActionName == this.ActionName;
		}
		if (this.TargetObjects.Length == 0)
		{
			return _Action.ActionName == this.ActionName;
		}
		for (int i = 0; i < this.TargetObjects.Length; i++)
		{
			if (_FromObject == this.TargetObjects[i])
			{
				return _Action.ActionName == this.ActionName;
			}
		}
		return false;
	}

	// Token: 0x040002F0 RID: 752
	public ScriptableObject[] TargetObjects;

	// Token: 0x040002F1 RID: 753
	[SerializeField]
	private string ActionName;
}
