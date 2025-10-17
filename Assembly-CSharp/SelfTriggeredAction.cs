using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000152 RID: 338
[CreateAssetMenu(menuName = "Survival/Self Triggered Action")]
public class SelfTriggeredAction : UniqueIDScriptable
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000982 RID: 2434 RVA: 0x0005881B File Offset: 0x00056A1B
	// (set) Token: 0x06000983 RID: 2435 RVA: 0x00058823 File Offset: 0x00056A23
	public StatTriggeredActionStatus[] StatTriggeredActions { get; private set; }

	// Token: 0x06000984 RID: 2436 RVA: 0x0005882C File Offset: 0x00056A2C
	public void Init(List<StatTriggeredActionStatus> _StatTriggeredActions)
	{
		if (this.Actions != null)
		{
			this.StatTriggeredActions = new StatTriggeredActionStatus[this.Actions.Length];
			for (int i = 0; i < this.Actions.Length; i++)
			{
				this.StatTriggeredActions[i] = new StatTriggeredActionStatus(this.Actions[i].StatChangeTrigger.Length, this.Actions[i].ActionName.DefaultText);
				if (_StatTriggeredActions != null && _StatTriggeredActions.Count != 0)
				{
					for (int j = 0; j < _StatTriggeredActions.Count; j++)
					{
						if (_StatTriggeredActions[j].ID == this.StatTriggeredActions[i].ID && !string.IsNullOrEmpty(_StatTriggeredActions[j].ID))
						{
							this.StatTriggeredActions[i].Load(_StatTriggeredActions[j]);
							break;
						}
					}
				}
			}
			return;
		}
		this.StatTriggeredActions = new StatTriggeredActionStatus[0];
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0005891C File Offset: 0x00056B1C
	public SelfTriggeredActionSaveData Save()
	{
		SelfTriggeredActionSaveData selfTriggeredActionSaveData = new SelfTriggeredActionSaveData();
		selfTriggeredActionSaveData.SaveAction(this);
		return selfTriggeredActionSaveData;
	}

	// Token: 0x04000F0D RID: 3853
	public FromStatChangeAction[] Actions;
}
