using System;

// Token: 0x02000181 RID: 385
[Serializable]
public class GameSaveFile
{
	// Token: 0x06000A44 RID: 2628 RVA: 0x0005B5CB File Offset: 0x000597CB
	public GameSaveFile(string _FileName, int _Index)
	{
		this.FileName = _FileName;
		this.SlotIndex = _Index;
		this.MainData = new GameSaveData();
		this.CheckpointData = new GameSaveData();
	}

	// Token: 0x04000FB8 RID: 4024
	public string FileName;

	// Token: 0x04000FB9 RID: 4025
	public int SlotIndex;

	// Token: 0x04000FBA RID: 4026
	public GameSaveData MainData;

	// Token: 0x04000FBB RID: 4027
	public GameSaveData CheckpointData;
}
