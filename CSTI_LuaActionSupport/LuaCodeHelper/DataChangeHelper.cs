using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.AllPatcher;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	public static class DataChangeHelper
	{
		// Token: 0x0600009E RID: 158 RVA: 0x000047E4 File Offset: 0x000029E4
		public static void ChangeStatValueTo(this GameManager gameManager, InGameStat inGameStat, float _Value)
		{
			if (inGameStat == null)
			{
				return;
			}
			float simpleCurrentValue = inGameStat.SimpleCurrentValue;
			List<LuaFunction> list;
			if (LuaRegister.Register.TryGet("GameManager", "ChangeStatValue", inGameStat.StatModel.UniqueID, out list))
			{
				float num = _Value - simpleCurrentValue;
				foreach (LuaFunction luaFunction in list)
				{
					try
					{
						luaFunction.Call(new object[]
						{
							gameManager,
							inGameStat,
							num,
							StatModification.Permanent
						});
					}
					catch (Exception message)
					{
						Debug.LogWarning(message);
					}
				}
			}
			inGameStat.CurrentBaseValue = _Value - inGameStat.GlobalModifiedValue - (gameManager.NotInBase ? 0f : inGameStat.AtBaseModifiedValue);
			if (inGameStat.StatModel.MinMaxValue != Vector2.zero)
			{
				inGameStat.CurrentBaseValue = Mathf.Clamp(inGameStat.CurrentBaseValue, inGameStat.StatModel.MinMaxValue.x, inGameStat.StatModel.MinMaxValue.y);
			}
			CardActionPatcher.Enumerators.Add(gameManager.UpdateStatStatuses(inGameStat, simpleCurrentValue, null));
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004920 File Offset: 0x00002B20
		public static void ChangeStatRateTo(this GameManager gameManager, InGameStat inGameStat, float _Rate)
		{
			if (inGameStat == null)
			{
				return;
			}
			List<LuaFunction> list;
			if (LuaRegister.Register.TryGet("GameManager", "ChangeStatRate", inGameStat.StatModel.UniqueID, out list))
			{
				float num = _Rate - inGameStat.SimpleRatePerTick;
				foreach (LuaFunction luaFunction in list)
				{
					try
					{
						luaFunction.Call(new object[]
						{
							gameManager,
							inGameStat,
							num,
							StatModification.Permanent
						});
					}
					catch (Exception message)
					{
						Debug.LogWarning(message);
					}
				}
			}
			inGameStat.CurrentBaseRate = _Rate - inGameStat.GlobalModifiedRate - (gameManager.NotInBase ? 0f : inGameStat.AtBaseModifiedRate);
			if (inGameStat.StatModel.MinMaxRate != Vector2.zero)
			{
				inGameStat.CurrentBaseRate = Mathf.Clamp(inGameStat.CurrentBaseRate, inGameStat.StatModel.MinMaxRate.x, inGameStat.StatModel.MinMaxRate.y);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004A48 File Offset: 0x00002C48
		public static OptionalFloatValue Copy(this OptionalFloatValue optionalFloatValue)
		{
			return new OptionalFloatValue(optionalFloatValue.Active, optionalFloatValue.FloatValue);
		}
	}
}
