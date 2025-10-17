using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
[CreateAssetMenu(menuName = "Survival/Liquid Transfer Rules")]
public class LiquidTransferRules : ScriptableObject
{
	// Token: 0x06000957 RID: 2391 RVA: 0x00057A34 File Offset: 0x00055C34
	public LiquidTransferInteraction FindTransferResult(CardData _Given, CardData _Receiving)
	{
		if ((_Given && !_Receiving) || (!_Given && _Receiving))
		{
			return LiquidTransferInteraction.UseDefaultTransferRules;
		}
		if (_Given && _Receiving && _Given == _Receiving)
		{
			return LiquidTransferInteraction.UseDefaultTransferRules;
		}
		for (int i = 0; i < this.Rules.Length; i++)
		{
			if (this.Rules[i].AppliesTo(_Given, _Receiving))
			{
				return this.Rules[i].Interaction;
			}
		}
		return LiquidTransferInteraction.DontTransfer;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x00057AB4 File Offset: 0x00055CB4
	public static LiquidTransferInteraction TransferResult(CardData _Given, CardData _Receiving)
	{
		if ((_Given && !_Receiving) || (!_Given && _Receiving))
		{
			return LiquidTransferInteraction.UseDefaultTransferRules;
		}
		if (_Given && _Receiving && _Given == _Receiving)
		{
			return LiquidTransferInteraction.UseDefaultTransferRules;
		}
		if (!MBSingleton<GameManager>.Instance)
		{
			return LiquidTransferInteraction.DontTransfer;
		}
		if (!MBSingleton<GameManager>.Instance.LiquidTransfers)
		{
			return LiquidTransferInteraction.DontTransfer;
		}
		return MBSingleton<GameManager>.Instance.LiquidTransfers.FindTransferResult(_Given, _Receiving);
	}

	// Token: 0x04000ECA RID: 3786
	public LiquidTransfers[] Rules;
}
