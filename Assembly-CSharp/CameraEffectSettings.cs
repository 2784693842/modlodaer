using System;
using UnityEngine;

// Token: 0x020000C8 RID: 200
[CreateAssetMenu(menuName = "Survival/Camera Effects")]
public class CameraEffectSettings : ScriptableObject
{
	// Token: 0x04000A54 RID: 2644
	public AmbienceEffectSettings DerealizationEffect;

	// Token: 0x04000A55 RID: 2645
	public AmbienceEffectSettings GoodAlteredStateEffect;

	// Token: 0x04000A56 RID: 2646
	public AmbienceEffectSettings BadAlteredStateEffect;

	// Token: 0x04000A57 RID: 2647
	public AmbienceEffectSettings SpecialEndingEffect;
}
