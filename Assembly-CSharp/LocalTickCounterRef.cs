using System;

// Token: 0x0200016C RID: 364
[Serializable]
public class LocalTickCounterRef
{
	// Token: 0x06000A01 RID: 2561 RVA: 0x0005A141 File Offset: 0x00058341
	public static implicit operator LocalTickCounter(LocalTickCounterRef _Ref)
	{
		if (_Ref == null)
		{
			return null;
		}
		return _Ref.Counter;
	}

	// Token: 0x04000F72 RID: 3954
	public LocalTickCounter Counter;
}
