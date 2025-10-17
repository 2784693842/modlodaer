using System;

// Token: 0x02000023 RID: 35
[Serializable]
public struct ExplorationResultSaveData
{
	// Token: 0x06000212 RID: 530 RVA: 0x0001519B File Offset: 0x0001339B
	public ExplorationResultSaveData(ExplorationResult _From)
	{
		if (_From.Action != null)
		{
			this.ActionName = _From.ActionName;
		}
		else
		{
			this.ActionName = "";
		}
		this.Triggered = false;
		this.TriggeredWithoutResults = false;
	}

	// Token: 0x0400020A RID: 522
	public string ActionName;

	// Token: 0x0400020B RID: 523
	public bool Triggered;

	// Token: 0x0400020C RID: 524
	public bool TriggeredWithoutResults;
}
