using System;
using System.Runtime.InteropServices;
using KeraLua;

namespace NLua.Extensions
{
	// Token: 0x0200008D RID: 141
	internal static class LuaExtensions
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x00011B49 File Offset: 0x0000FD49
		public static bool CheckMetaTable(this Lua state, int index, IntPtr tag)
		{
			if (!state.GetMetaTable(index))
			{
				return false;
			}
			state.PushLightUserData(tag);
			state.RawGet(-2);
			bool result = !state.IsNil(-1);
			state.SetTop(-3);
			return result;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00011B78 File Offset: 0x0000FD78
		public static void PopGlobalTable(this Lua luaState)
		{
			luaState.RawSetInteger(LuaRegistry.Index, 2L);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00011B87 File Offset: 0x0000FD87
		public static void GetRef(this Lua luaState, int reference)
		{
			luaState.RawGetInteger(LuaRegistry.Index, (long)reference);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00011B97 File Offset: 0x0000FD97
		public static void Unref(this Lua luaState, int reference)
		{
			luaState.Unref(LuaRegistry.Index, reference);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00011BA5 File Offset: 0x0000FDA5
		public static bool AreEqual(this Lua luaState, int ref1, int ref2)
		{
			return luaState.Compare(ref1, ref2, LuaCompare.Equal);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00011BB0 File Offset: 0x0000FDB0
		public static IntPtr CheckUData(this Lua state, int ud, string name)
		{
			IntPtr intPtr = state.ToUserData(ud);
			if (intPtr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			if (!state.GetMetaTable(ud))
			{
				return IntPtr.Zero;
			}
			state.GetField(LuaRegistry.Index, name);
			bool flag = state.RawEqual(-1, -2);
			state.Pop(2);
			if (flag)
			{
				return intPtr;
			}
			return IntPtr.Zero;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00011C10 File Offset: 0x0000FE10
		public static int ToNetObject(this Lua state, int index, IntPtr tag)
		{
			if (state.Type(index) != LuaType.UserData)
			{
				return -1;
			}
			IntPtr intPtr;
			if (state.CheckMetaTable(index, tag))
			{
				intPtr = state.ToUserData(index);
				if (intPtr != IntPtr.Zero)
				{
					return Marshal.ReadInt32(intPtr);
				}
			}
			intPtr = state.CheckUData(index, "luaNet_class");
			if (intPtr != IntPtr.Zero)
			{
				return Marshal.ReadInt32(intPtr);
			}
			intPtr = state.CheckUData(index, "luaNet_searchbase");
			if (intPtr != IntPtr.Zero)
			{
				return Marshal.ReadInt32(intPtr);
			}
			intPtr = state.CheckUData(index, "luaNet_function");
			if (intPtr != IntPtr.Zero)
			{
				return Marshal.ReadInt32(intPtr);
			}
			return -1;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00011CB3 File Offset: 0x0000FEB3
		public static void NewUData(this Lua state, int val)
		{
			Marshal.WriteInt32(state.NewUserData(Marshal.SizeOf(typeof(int))), val);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00011CD0 File Offset: 0x0000FED0
		public static int RawNetObj(this Lua state, int index)
		{
			IntPtr intPtr = state.ToUserData(index);
			if (intPtr == IntPtr.Zero)
			{
				return -1;
			}
			return Marshal.ReadInt32(intPtr);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00011CFC File Offset: 0x0000FEFC
		public static int CheckUObject(this Lua state, int index, string name)
		{
			IntPtr intPtr = state.CheckUData(index, name);
			if (intPtr == IntPtr.Zero)
			{
				return -1;
			}
			return Marshal.ReadInt32(intPtr);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00011D27 File Offset: 0x0000FF27
		public static bool IsNumericType(this Lua state, int index)
		{
			return state.Type(index) == LuaType.Number;
		}
	}
}
