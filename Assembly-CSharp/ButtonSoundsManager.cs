using System;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class ButtonSoundsManager : MBSingleton<ButtonSoundsManager>
{
	// Token: 0x060009E2 RID: 2530 RVA: 0x0000B946 File Offset: 0x00009B46
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x00059BD0 File Offset: 0x00057DD0
	public static void PerformButtonSound(AudioClip[] _Sounds)
	{
		if (_Sounds == null || !MBSingleton<ButtonSoundsManager>.Instance)
		{
			return;
		}
		if (!MBSingleton<ButtonSoundsManager>.Instance.ButtonsSoundsObject)
		{
			return;
		}
		if (_Sounds.Length == 0)
		{
			return;
		}
		AudioClip audioClip = _Sounds[UnityEngine.Random.Range(0, _Sounds.Length)];
		if (audioClip)
		{
			MBSingleton<ButtonSoundsManager>.Instance.ButtonsSoundsObject.PlaySound(audioClip, MBSingleton<ButtonSoundsManager>.Instance.PitchVariation, MBSingleton<ButtonSoundsManager>.Instance.VolumeVariation);
		}
	}

	// Token: 0x04000F5F RID: 3935
	[SerializeField]
	private RandomSoundPlay ButtonsSoundsObject;

	// Token: 0x04000F60 RID: 3936
	[SerializeField]
	private bool PitchVariation;

	// Token: 0x04000F61 RID: 3937
	[SerializeField]
	private bool VolumeVariation;
}
