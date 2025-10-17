using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
public struct EncounterStealthReport
{
	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060004C6 RID: 1222 RVA: 0x000315C2 File Offset: 0x0002F7C2
	public float PlayerStealth
	{
		get
		{
			return this.PlayerStatsStealth + this.PlayerCoverStealth + this.PlayerEquipmentStealth;
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060004C7 RID: 1223 RVA: 0x000315D8 File Offset: 0x0002F7D8
	public float PlayerAlertness
	{
		get
		{
			return Mathf.Max(0f, this.PlayerStatsAlertness);
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x060004C8 RID: 1224 RVA: 0x000315EA File Offset: 0x0002F7EA
	public float EnemyStealth
	{
		get
		{
			return this.EnemyBaseStealth + this.EnemyCoverStealth;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060004C9 RID: 1225 RVA: 0x000315F9 File Offset: 0x0002F7F9
	public float EnemyAlertness
	{
		get
		{
			return Mathf.Max(0f, this.EnemyBaseAlertness);
		}
	}

	// Token: 0x0400060D RID: 1549
	public float PlayerStatsStealth;

	// Token: 0x0400060E RID: 1550
	public float PlayerStatsAlertness;

	// Token: 0x0400060F RID: 1551
	public float PlayerCoverStealth;

	// Token: 0x04000610 RID: 1552
	public float PlayerEquipmentStealth;

	// Token: 0x04000611 RID: 1553
	public float EnemyBaseStealth;

	// Token: 0x04000612 RID: 1554
	public float EnemyBaseAlertness;

	// Token: 0x04000613 RID: 1555
	public float EnemyCoverStealth;
}
