using System;
using System.Collections.Generic;

// Token: 0x02000153 RID: 339
[Serializable]
public class SelfTriggeredActionSaveData
{
	// Token: 0x06000987 RID: 2439 RVA: 0x0005892A File Offset: 0x00056B2A
	public void SaveAction(SelfTriggeredAction _From)
	{
		this.ActionID = UniqueIDScriptable.SaveID(_From);
		this.StatTriggeredActions.AddRange(_From.StatTriggeredActions);
	}

	// Token: 0x04000F0F RID: 3855
	public string ActionID;

	// Token: 0x04000F10 RID: 3856
	public List<StatTriggeredActionStatus> StatTriggeredActions = new List<StatTriggeredActionStatus>();
}
