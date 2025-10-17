using System;
using System.Collections.Generic;

// Token: 0x02000182 RID: 386
[Serializable]
public class GlobalSaveData
{
	// Token: 0x06000A45 RID: 2629 RVA: 0x0005B5F8 File Offset: 0x000597F8
	public GlobalSaveData()
	{
		this.Games = new List<GameSaveData>();
		this.Checkpoints = new List<GameSaveData>();
	}

	// Token: 0x04000FBC RID: 4028
	public List<GameSaveData> Games = new List<GameSaveData>();

	// Token: 0x04000FBD RID: 4029
	public List<GameSaveData> Checkpoints = new List<GameSaveData>();

	// Token: 0x04000FBE RID: 4030
	public bool IsValid;

	// Token: 0x04000FBF RID: 4031
	public bool DontShowEasyPopup;

	// Token: 0x04000FC0 RID: 4032
	public List<string> UnlockedPerks = new List<string>();

	// Token: 0x04000FC1 RID: 4033
	public List<string> UnlockedCharacters = new List<string>();

	// Token: 0x04000FC2 RID: 4034
	public List<PlayerCharacterSaveData> CreatedCharacters = new List<PlayerCharacterSaveData>();

	// Token: 0x04000FC3 RID: 4035
	public List<string> GlobalObjectives = new List<string>();

	// Token: 0x04000FC4 RID: 4036
	public int Suns;

	// Token: 0x04000FC5 RID: 4037
	public int Moons;

	// Token: 0x04000FC6 RID: 4038
	public int LastCheckedInfoNotes;

	// Token: 0x04000FC7 RID: 4039
	public bool SafeModeState;

	// Token: 0x04000FC8 RID: 4040
	public bool SavedFromFullGame;

	// Token: 0x04000FC9 RID: 4041
	public int PerkUnlockFixVersion;
}
