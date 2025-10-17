using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000196 RID: 406
public class MenuSoundManager : MonoBehaviour
{
	// Token: 0x06000B6A RID: 2922 RVA: 0x0006101A File Offset: 0x0005F21A
	private void Awake()
	{
		this.GameData = GameLoad.Instance;
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00061028 File Offset: 0x0005F228
	private void LateUpdate()
	{
		if (!this.GameData || !this.GameMixer)
		{
			return;
		}
		if (this.GameData.CurrentGameOptions.NormalizedMusicVolume > 0f)
		{
			this.GameMixer.SetFloat("MusicVol", this.GameData.CurrentGameOptions.MusicVolume);
		}
		else
		{
			this.GameMixer.SetFloat("MusicVol", -80f);
		}
		if (this.GameData.CurrentGameOptions.NormalizedAmbienceVolume > 0f)
		{
			this.GameMixer.SetFloat("AmbienceVol", this.GameData.CurrentGameOptions.AmbienceVolume(1f));
		}
		else
		{
			this.GameMixer.SetFloat("AmbienceVol", -80f);
		}
		if (this.GameData.CurrentGameOptions.NormalizedSFXVolume > 0f)
		{
			this.GameMixer.SetFloat("SFXVol", this.GameData.CurrentGameOptions.SFXVolume);
			return;
		}
		this.GameMixer.SetFloat("SFXVol", -80f);
	}

	// Token: 0x04001064 RID: 4196
	[SerializeField]
	private AudioMixer GameMixer;

	// Token: 0x04001065 RID: 4197
	private GameLoad GameData;
}
