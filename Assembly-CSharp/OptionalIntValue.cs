using System;

// Token: 0x020001C3 RID: 451
[Serializable]
public class OptionalIntValue : OptionalValue
{
	// Token: 0x06000C53 RID: 3155 RVA: 0x000659BF File Offset: 0x00063BBF
	public OptionalIntValue(bool _Active, int _Value) : base(_Active)
	{
		this.IntValue = _Value;
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x000659CF File Offset: 0x00063BCF
	public static implicit operator int(OptionalIntValue _Value)
	{
		if (!_Value.Active)
		{
			return 0;
		}
		return _Value.IntValue;
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x000659E1 File Offset: 0x00063BE1
	public void Add(OptionalIntValue _From)
	{
		this.Active |= _From.Active;
		this.IntValue += _From.IntValue;
	}

	// Token: 0x0400113A RID: 4410
	public int IntValue;
}
