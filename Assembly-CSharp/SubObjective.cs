using System;

// Token: 0x02000105 RID: 261
public abstract class SubObjective
{
	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000889 RID: 2185 RVA: 0x000535B9 File Offset: 0x000517B9
	// (set) Token: 0x0600088A RID: 2186 RVA: 0x000535C1 File Offset: 0x000517C1
	public bool Complete { get; protected set; }

	// Token: 0x0600088B RID: 2187 RVA: 0x000535CA File Offset: 0x000517CA
	public virtual float GetCompletion()
	{
		if (this.Complete)
		{
			return (float)this.CompletionWeight;
		}
		return 0f;
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x000535E1 File Offset: 0x000517E1
	public virtual void ForceComplete()
	{
		this.Complete = true;
		this.ForceCompleted = true;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x000535F1 File Offset: 0x000517F1
	public virtual void CompleteThroughName()
	{
		this.Complete = true;
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x000535FC File Offset: 0x000517FC
	public SubObjectiveSaveData Save()
	{
		return new SubObjectiveSaveData
		{
			ObjectiveID = this.ObjectiveName,
			Complete = this.Complete,
			ForceCompleted = this.ForceCompleted,
			Counter = this.GetSaveCounter()
		};
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00053646 File Offset: 0x00051846
	public void Load(SubObjectiveSaveData _From)
	{
		this.Complete = _From.Complete;
		this.ForceCompleted = _From.ForceCompleted;
		this.LoadSaveCounter(_From.Counter);
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0005366C File Offset: 0x0005186C
	public virtual void Init()
	{
		this.Complete = false;
		this.ForceCompleted = false;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00052B1A File Offset: 0x00050D1A
	protected virtual int GetSaveCounter()
	{
		return 0;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x00018E36 File Offset: 0x00017036
	protected virtual void LoadSaveCounter(int _Counter)
	{
	}

	// Token: 0x04000CE4 RID: 3300
	public string ObjectiveName;

	// Token: 0x04000CE5 RID: 3301
	public int CompletionWeight;

	// Token: 0x04000CE7 RID: 3303
	protected bool ForceCompleted;
}
