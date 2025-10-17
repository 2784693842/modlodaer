using System;
using UnityEngine.Serialization;

// Token: 0x020001C4 RID: 452
[Serializable]
public class OptionalFloatValue : OptionalValue
{
	// Token: 0x06000C56 RID: 3158 RVA: 0x00065A09 File Offset: 0x00063C09
	public OptionalFloatValue(bool _Active, float _Value) : base(_Active)
	{
		this.FloatValue = _Value;
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00065A19 File Offset: 0x00063C19
	public static implicit operator float(OptionalFloatValue _Value)
	{
		if (!_Value.Active)
		{
			return 0f;
		}
		return _Value.FloatValue;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00065A2F File Offset: 0x00063C2F
	public void Add(OptionalFloatValue _From)
	{
		this.Active |= _From.Active;
		this.FloatValue += _From.FloatValue;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00065A57 File Offset: 0x00063C57
	public void Multiply(float _With)
	{
		if (!this.Active)
		{
			return;
		}
		this.FloatValue *= _With;
	}

	// Token: 0x0400113B RID: 4411
	[FormerlySerializedAs("IntValue")]
	public float FloatValue;
}
