using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
[Serializable]
public class BlueprintSaveData
{
	// Token: 0x06000213 RID: 531 RVA: 0x000151CC File Offset: 0x000133CC
	public BlueprintSaveData(BlueprintStage[] _Stages, CardData _Env)
	{
		this.CurrentStage = 0;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x000151DB File Offset: 0x000133DB
	public BlueprintSaveData(BlueprintSaveData _From, BlueprintStage[] _Stages, CardData _Env, bool _Clamp)
	{
		if (_Clamp)
		{
			this.CurrentStage = Mathf.Clamp(_From.CurrentStage, 0, _Stages.Length - 1);
			return;
		}
		this.CurrentStage = _From.CurrentStage;
	}

	// Token: 0x0400020D RID: 525
	public int CurrentStage;
}
