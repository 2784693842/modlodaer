using System;
using UnityEngine;

// Token: 0x020001C6 RID: 454
[Serializable]
public class OptionalObjectValue<T> : OptionalValue where T : UnityEngine.Object
{
	// Token: 0x06000C5D RID: 3165 RVA: 0x00065AFE File Offset: 0x00063CFE
	public OptionalObjectValue(bool _Active, T _Value) : base(_Active)
	{
		this.ObjectValue = _Value;
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x00065B10 File Offset: 0x00063D10
	public static implicit operator T(OptionalObjectValue<T> _Value)
	{
		if (!_Value.Active)
		{
			return default(T);
		}
		return _Value.ObjectValue;
	}

	// Token: 0x0400113D RID: 4413
	public T ObjectValue;
}
