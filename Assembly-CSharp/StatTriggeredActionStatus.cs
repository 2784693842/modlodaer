using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[Serializable]
public struct StatTriggeredActionStatus
{
	// Token: 0x060001F5 RID: 501 RVA: 0x00014AA9 File Offset: 0x00012CA9
	public StatTriggeredActionStatus(int _Conditions, string _ActionID)
	{
		this.Conditions = new bool[_Conditions];
		this.ReadyToPlay = false;
		this.PlayedCounter = 0;
		this.LastPlayedTick = 0;
		this.ID = _ActionID;
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00014AD3 File Offset: 0x00012CD3
	public void Load(StatTriggeredActionStatus _From)
	{
		this.PlayedCounter = _From.PlayedCounter;
		this.LastPlayedTick = _From.LastPlayedTick;
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060001F7 RID: 503 RVA: 0x00014AED File Offset: 0x00012CED
	// (set) Token: 0x060001F8 RID: 504 RVA: 0x00014AF5 File Offset: 0x00012CF5
	public bool[] Conditions { get; private set; }

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060001F9 RID: 505 RVA: 0x00014AFE File Offset: 0x00012CFE
	// (set) Token: 0x060001FA RID: 506 RVA: 0x00014B06 File Offset: 0x00012D06
	public bool ReadyToPlay { get; private set; }

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060001FB RID: 507 RVA: 0x00014B10 File Offset: 0x00012D10
	private bool AllConditionsTrue
	{
		get
		{
			if (this.Conditions == null)
			{
				return false;
			}
			if (this.Conditions.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.Conditions.Length; i++)
			{
				if (!this.Conditions[i])
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00014B54 File Offset: 0x00012D54
	public void UpdateReadyToPlay(bool _OtherConditions, StatTriggerTypes _Repeat, bool _OncePerTick)
	{
		this.ReadyToPlay = this.AllConditionsTrue;
		switch (_Repeat)
		{
		case StatTriggerTypes.Repeat:
			if (MBSingleton<GameManager>.Instance && _OncePerTick)
			{
				this.ReadyToPlay &= (this.LastPlayedTick != MBSingleton<GameManager>.Instance.CurrentTickInfo.z);
			}
			break;
		case StatTriggerTypes.OnlyOnce:
			this.ReadyToPlay &= (this.PlayedCounter <= 0);
			break;
		case StatTriggerTypes.ResetWhenOutOfRange:
			if (!this.ReadyToPlay)
			{
				this.PlayedCounter = 0;
			}
			else
			{
				this.ReadyToPlay &= (this.PlayedCounter <= 0);
			}
			break;
		}
		this.ReadyToPlay = (this.ReadyToPlay && _OtherConditions);
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00014C10 File Offset: 0x00012E10
	public void PlayAction()
	{
		this.PlayedCounter++;
		if (MBSingleton<GameManager>.Instance)
		{
			this.LastPlayedTick = MBSingleton<GameManager>.Instance.CurrentTickInfo.z;
		}
		this.ReadyToPlay = false;
	}

	// Token: 0x040001F2 RID: 498
	[SerializeField]
	private int PlayedCounter;

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	private int LastPlayedTick;

	// Token: 0x040001F4 RID: 500
	public string ID;
}
