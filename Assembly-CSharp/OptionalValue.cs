using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
[Serializable]
public class OptionalValue
{
	// Token: 0x06000C51 RID: 3153 RVA: 0x000659A3 File Offset: 0x00063BA3
	public OptionalValue(bool _Active)
	{
		this.Active = _Active;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x000659B2 File Offset: 0x00063BB2
	public static implicit operator bool(OptionalValue _Value)
	{
		return _Value != null && _Value.Active;
	}

	// Token: 0x04001139 RID: 4409
	[SerializeField]
	protected bool Active;
}
