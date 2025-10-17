using System;

// Token: 0x02000149 RID: 329
[Serializable]
public class InGameTickCounter
{
	// Token: 0x0600095D RID: 2397 RVA: 0x00057B89 File Offset: 0x00055D89
	public InGameTickCounter(LocalTickCounter _Model)
	{
		this.Model = _Model;
		this.ModelID = UniqueIDScriptable.SaveID(_Model);
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x00057BA4 File Offset: 0x00055DA4
	public InGameTickCounter(InGameTickCounter _From)
	{
		this.ModelID = _From.ModelID;
		if (_From.Model)
		{
			this.Model = _From.Model;
		}
		else
		{
			this.Model = UniqueIDScriptable.GetFromID<LocalTickCounter>(this.ModelID);
		}
		this.Value = _From.Value;
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x00057BFB File Offset: 0x00055DFB
	public static implicit operator int(InGameTickCounter _Counter)
	{
		if (_Counter == null)
		{
			return 0;
		}
		return _Counter.Value;
	}

	// Token: 0x04000ED5 RID: 3797
	[NonSerialized]
	public LocalTickCounter Model;

	// Token: 0x04000ED6 RID: 3798
	public string ModelID;

	// Token: 0x04000ED7 RID: 3799
	public int Value;

	// Token: 0x04000ED8 RID: 3800
	[NonSerialized]
	public bool Updated;
}
