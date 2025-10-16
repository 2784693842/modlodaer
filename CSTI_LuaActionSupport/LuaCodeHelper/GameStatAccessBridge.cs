using System;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	public class GameStatAccessBridge
	{
		// Token: 0x06000097 RID: 151 RVA: 0x000046DC File Offset: 0x000028DC
		public GameStatAccessBridge(InGameStat gameStat)
		{
			this.GameStat = gameStat;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000046EB File Offset: 0x000028EB
		// (set) Token: 0x06000099 RID: 153 RVA: 0x000046F8 File Offset: 0x000028F8
		public float Value
		{
			get
			{
				return this.GameStat.SimpleCurrentValue;
			}
			set
			{
				MBSingleton<GameManager>.Instance.ChangeStatValueTo(this.GameStat, value);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009A RID: 154 RVA: 0x0000470B File Offset: 0x0000290B
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004718 File Offset: 0x00002918
		public float Rate
		{
			get
			{
				return this.GameStat.SimpleRatePerTick;
			}
			set
			{
				MBSingleton<GameManager>.Instance.ChangeStatRateTo(this.GameStat, value);
			}
		}

		// Token: 0x0400003E RID: 62
		public readonly InGameStat GameStat;
	}
}
