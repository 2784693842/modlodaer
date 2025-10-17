using System;

// Token: 0x0200001A RID: 26
[Serializable]
public class PassiveEffectsSaveData
{
	// Token: 0x060001F4 RID: 500 RVA: 0x00014A7F File Offset: 0x00012C7F
	public PassiveEffectsSaveData(string _Name, StatModifier[] _Mods)
	{
		this.EffectName = _Name;
		this.Mods = new StatModifier[_Mods.Length];
		_Mods.CopyTo(this.Mods, 0);
	}

	// Token: 0x040001EE RID: 494
	public string EffectName;

	// Token: 0x040001EF RID: 495
	public StatModifier[] Mods;
}
