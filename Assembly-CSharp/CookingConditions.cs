using System;
using UnityEngine;

// Token: 0x020000F6 RID: 246
[Serializable]
public struct CookingConditions
{
	// Token: 0x0600082E RID: 2094 RVA: 0x000511B4 File Offset: 0x0004F3B4
	public bool CanCook(float _Spoilage, float _Usage, float _Fuel, float _Progress, float _Special1, float _Special2, float _Special3, float _Special4, CardData _Model)
	{
		if (!_Model)
		{
			return (!this.NeedsSpoilage || _Spoilage > 0f) && (!this.NeedsUsage || _Usage > 0f) && (!this.NeedsFuel || _Fuel > 0f) && (!this.NeedsProgress || _Progress > 0f) && (!this.NeedsSpecial1 || _Special1 > 0f) && (!this.NeedsSpecial2 || _Special2 > 0f) && (!this.NeedsSpecial3 || _Special3 > 0f) && (!this.NeedsSpecial4 || _Special4 > 0f);
		}
		return (!this.NeedsSpoilage || _Spoilage > 0f || !_Model.SpoilageTime) && (!this.NeedsUsage || _Usage > 0f || !_Model.UsageDurability) && (!this.NeedsFuel || _Fuel > 0f || !_Model.FuelCapacity) && (!this.NeedsProgress || _Progress > 0f || !_Model.Progress) && (!this.NeedsSpecial1 || _Special1 > 0f || !_Model.SpecialDurability1) && (!this.NeedsSpecial2 || _Special2 > 0f || !_Model.SpecialDurability2) && (!this.NeedsSpecial3 || _Special3 > 0f || !_Model.SpecialDurability3) && (!this.NeedsSpecial4 || _Special4 > 0f || !_Model.SpecialDurability4);
	}

	// Token: 0x04000C65 RID: 3173
	public bool NeedsSpoilage;

	// Token: 0x04000C66 RID: 3174
	public bool NeedsUsage;

	// Token: 0x04000C67 RID: 3175
	public bool NeedsFuel;

	// Token: 0x04000C68 RID: 3176
	public bool NeedsProgress;

	// Token: 0x04000C69 RID: 3177
	public bool NeedsSpecial1;

	// Token: 0x04000C6A RID: 3178
	public bool NeedsSpecial2;

	// Token: 0x04000C6B RID: 3179
	public bool NeedsSpecial3;

	// Token: 0x04000C6C RID: 3180
	public bool NeedsSpecial4;

	// Token: 0x04000C6D RID: 3181
	public float ExtraSpoilageRate;

	// Token: 0x04000C6E RID: 3182
	public float ExtraUsageRate;

	// Token: 0x04000C6F RID: 3183
	public float ExtraFuelRate;

	// Token: 0x04000C70 RID: 3184
	public float ExtraProgressRate;

	// Token: 0x04000C71 RID: 3185
	public float ExtraSpecial1Rate;

	// Token: 0x04000C72 RID: 3186
	public float ExtraSpecial2Rate;

	// Token: 0x04000C73 RID: 3187
	public float ExtraSpecial3Rate;

	// Token: 0x04000C74 RID: 3188
	public float ExtraSpecial4Rate;

	// Token: 0x04000C75 RID: 3189
	public AudioClip CookingPausedSound;

	// Token: 0x04000C76 RID: 3190
	public LocalizedString CookingPausedNotification;
}
