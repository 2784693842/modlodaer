using System;
using System.Collections.Generic;

// Token: 0x02000151 RID: 337
[Serializable]
public class PlayerCharacterSaveData
{
	// Token: 0x0600097F RID: 2431 RVA: 0x00058761 File Offset: 0x00056961
	public PlayerCharacterSaveData()
	{
		this.CharacterName = null;
		this.CharacterBio = null;
		this.PortraitID = null;
		this.CharacterPerks = new List<string>();
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00058794 File Offset: 0x00056994
	public PlayerCharacterSaveData(PlayerCharacterSaveData _From)
	{
		this.CharacterName = _From.CharacterName;
		this.CharacterBio = _From.CharacterBio;
		this.PortraitID = _From.PortraitID;
		this.CharacterPerks = new List<string>();
		this.CharacterPerks.AddRange(_From.CharacterPerks);
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000981 RID: 2433 RVA: 0x000587F2 File Offset: 0x000569F2
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.CharacterName) || (this.CharacterPerks != null && this.CharacterPerks.Count > 0);
		}
	}

	// Token: 0x04000F09 RID: 3849
	public string CharacterName;

	// Token: 0x04000F0A RID: 3850
	public string CharacterBio;

	// Token: 0x04000F0B RID: 3851
	public string PortraitID;

	// Token: 0x04000F0C RID: 3852
	public List<string> CharacterPerks = new List<string>();
}
