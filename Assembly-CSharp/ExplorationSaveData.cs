using System;
using System.Collections.Generic;

// Token: 0x02000022 RID: 34
[Serializable]
public class ExplorationSaveData
{
	// Token: 0x0600020F RID: 527 RVA: 0x000150E7 File Offset: 0x000132E7
	public ExplorationSaveData(ExplorationSaveData _From)
	{
		this.CurrentExploration = _From.CurrentExploration;
		this.ExplorationResults = new List<ExplorationResultSaveData>();
		this.ExplorationResults.AddRange(_From.ExplorationResults);
	}

	// Token: 0x06000210 RID: 528 RVA: 0x00015122 File Offset: 0x00013322
	public ExplorationSaveData()
	{
		this.CurrentExploration = 0f;
		this.ExplorationResults = new List<ExplorationResultSaveData>();
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0001514C File Offset: 0x0001334C
	public bool HasData(string _ActionName, out int _Result)
	{
		_Result = -1;
		if (this.ExplorationResults == null)
		{
			return false;
		}
		for (int i = 0; i < this.ExplorationResults.Count; i++)
		{
			if (this.ExplorationResults[i].ActionName == _ActionName)
			{
				_Result = i;
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000208 RID: 520
	public float CurrentExploration;

	// Token: 0x04000209 RID: 521
	public List<ExplorationResultSaveData> ExplorationResults = new List<ExplorationResultSaveData>();
}
