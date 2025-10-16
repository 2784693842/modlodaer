using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.LuaCodeHelper;
using HarmonyLib;
using NLua;
using UnityEngine;

namespace CSTI_LuaActionSupport
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	[BepInPlugin("zender.LuaActionSupport.LuaSupportRuntime", "LuaActionSupport", "1.0.2.3")]
	public class LuaSupportRuntime : BaseUnityPlugin
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021B8 File Offset: 0x000003B8
		static LuaSupportRuntime()
		{
			LuaSupportRuntime.HarmonyInstance.PatchAll(typeof(CardActionPatcher));
			LuaSupportRuntime.HarmonyInstance.PatchAll(typeof(OnGameLoad));
			LuaSupportRuntime.HarmonyInstance.PatchAll(typeof(SavePatcher));
			LuaSupportRuntime.HarmonyInstance.PatchAll(typeof(ObjModifyPatcher));
			LuaSupportRuntime.HarmonyInstance.PatchAll(typeof(LuaRegister));
			LuaSupportRuntime.HarmonyInstance.PatchAll(typeof(LuaTimer));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002290 File Offset: 0x00000490
		private static void LoadLuaSave()
		{
			if (!File.Exists(SavePatcher.SavePath))
			{
				return;
			}
			using (BufferedStream bufferedStream = new BufferedStream(File.OpenRead(SavePatcher.SavePath)))
			{
				using (BinaryReader binaryReader = new BinaryReader(bufferedStream))
				{
					int num;
					using (SavePatcher.LoadEnv.BeginLoadEnv(binaryReader, out num, new int?(0)))
					{
						int num2 = binaryReader.ReadInt32();
						for (int i = 0; i < num2; i++)
						{
							string key = binaryReader.ReadString();
							CardActionPatcher.GSaveData[key] = DataNode.Load(binaryReader);
						}
					}
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000234C File Offset: 0x0000054C
		private void Awake()
		{
			LuaSupportRuntime.Runtime = this;
			LuaSupportRuntime.LoadLuaSave();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000235C File Offset: 0x0000055C
		private void Update()
		{
			List<LuaFunction> list = (from <>h__TransparentIdentifier0 in (from function in LuaTimer.FrameFunctions
			select new
			{
				function = function,
				objects = function.Call(Array.Empty<object>())
			}).Where(delegate(<>h__TransparentIdentifier0)
			{
				if (<>h__TransparentIdentifier0.objects.Length != 0)
				{
					object obj2 = <>h__TransparentIdentifier0.objects[0];
					return obj2 is bool && !(bool)obj2;
				}
				return false;
			})
			select <>h__TransparentIdentifier0.function).ToList<LuaFunction>();
			foreach (LuaFunction item in list)
			{
				LuaTimer.FrameFunctions.Remove(item);
			}
			list.Clear();
			foreach (KeyValuePair<LuaFunction, LuaTimer.SimpleTimer> pair in LuaTimer.EveryTimeFunctions)
			{
				LuaFunction luaFunction;
				LuaTimer.SimpleTimer simpleTimer;
				pair.Deconstruct(out luaFunction, out simpleTimer);
				LuaFunction luaFunction2 = luaFunction;
				LuaTimer.SimpleTimer simpleTimer2 = simpleTimer;
				simpleTimer2.CurTime += Time.deltaTime;
				if (simpleTimer2.CurTime >= simpleTimer2.Time)
				{
					object[] array = luaFunction2.Call(Array.Empty<object>());
					if (array.Length != 0)
					{
						object obj = array[0];
						if (obj is bool && !(bool)obj)
						{
							list.Add(luaFunction2);
						}
					}
					simpleTimer2.CurTime -= simpleTimer2.Time;
				}
			}
			foreach (LuaFunction key in list)
			{
				LuaTimer.EveryTimeFunctions.Remove(key);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000252C File Offset: 0x0000072C
		private void FixedUpdate()
		{
			foreach (LuaFunction item in (from <>h__TransparentIdentifier0 in (from function in LuaTimer.FixFrameFunctions
			select new
			{
				function = function,
				objects = function.Call(Array.Empty<object>())
			}).Where(delegate(<>h__TransparentIdentifier0)
			{
				if (<>h__TransparentIdentifier0.objects.Length != 0)
				{
					object obj = <>h__TransparentIdentifier0.objects[0];
					return obj is bool && !(bool)obj;
				}
				return false;
			})
			select <>h__TransparentIdentifier0.function).ToList<LuaFunction>())
			{
				LuaTimer.FixFrameFunctions.Remove(item);
			}
		}

		// Token: 0x04000006 RID: 6
		public static readonly Harmony HarmonyInstance = new Harmony("zender.LuaActionSupport.LuaSupportRuntime");

		// Token: 0x04000007 RID: 7
		public static readonly string ModInfo = "ModInfo.json";

		// Token: 0x04000008 RID: 8
		public static readonly string LuaInit = "LuaInit";

		// Token: 0x04000009 RID: 9
		public static readonly string LuaOnGameLoad = "LuaOnGameLoad";

		// Token: 0x0400000A RID: 10
		public static readonly string LuaOnGameSave = "LuaOnGameSave";

		// Token: 0x0400000B RID: 11
		public static readonly List<string> LuaFilesOnGameLoad = new List<string>();

		// Token: 0x0400000C RID: 12
		public static readonly List<string> LuaFilesOnGameSave = new List<string>();

		// Token: 0x0400000D RID: 13
		public static LuaSupportRuntime Runtime = null;
	}
}
