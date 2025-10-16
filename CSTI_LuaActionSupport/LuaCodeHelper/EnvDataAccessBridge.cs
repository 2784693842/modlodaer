using System;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	public class EnvDataAccessBridge : CommonSimpleAccess
	{
		// Token: 0x0600014E RID: 334 RVA: 0x00006864 File Offset: 0x00004A64
		public EnvDataAccessBridge(EnvironmentSaveData environmentSaveData)
		{
			this.EnvironmentSaveData = environmentSaveData;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00006873 File Offset: 0x00004A73
		public override object AccessObj
		{
			get
			{
				return this.EnvironmentSaveData;
			}
		}

		// Token: 0x04000091 RID: 145
		public readonly EnvironmentSaveData EnvironmentSaveData;
	}
}
