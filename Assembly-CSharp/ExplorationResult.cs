using System;

// Token: 0x020000FA RID: 250
[Serializable]
public class ExplorationResult
{
	// Token: 0x17000199 RID: 409
	// (get) Token: 0x0600083C RID: 2108 RVA: 0x00051A16 File Offset: 0x0004FC16
	public string ActionName
	{
		get
		{
			if (this.Action == null)
			{
				return "";
			}
			return this.Action.ActionName.DefaultText;
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00051A36 File Offset: 0x0004FC36
	public bool IsIdentical(ExplorationResult _Exploration)
	{
		return this.IsIdentical(_Exploration.ActionName);
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00051A44 File Offset: 0x0004FC44
	public bool IsIdentical(string _ActionName)
	{
		return !string.IsNullOrEmpty(this.ActionName) && !string.IsNullOrEmpty(_ActionName) && this.ActionName == _ActionName;
	}

	// Token: 0x04000C92 RID: 3218
	public float TriggerValue;

	// Token: 0x04000C93 RID: 3219
	public CardAction Action;
}
