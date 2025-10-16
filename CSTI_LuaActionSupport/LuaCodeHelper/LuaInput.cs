using System;
using System.Runtime.CompilerServices;
using BepInEx;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000032 RID: 50
	[NullableContext(1)]
	[Nullable(0)]
	public static class LuaInput
	{
		// Token: 0x06000104 RID: 260 RVA: 0x000055CF File Offset: 0x000037CF
		[LuaFunc]
		public static float GetScroll()
		{
			return UnityInput.Current.mouseScrollDelta.y;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000055E0 File Offset: 0x000037E0
		[LuaFunc]
		public static bool GetKey(string key)
		{
			return UnityInput.Current.GetKey(key);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000055ED File Offset: 0x000037ED
		[LuaFunc]
		public static bool GetKeyDown(string key)
		{
			return UnityInput.Current.GetKeyDown(key);
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000055FA File Offset: 0x000037FA
		[LuaFunc]
		public static bool GetKeyUp(string key)
		{
			return UnityInput.Current.GetKeyUp(key);
		}
	}
}
