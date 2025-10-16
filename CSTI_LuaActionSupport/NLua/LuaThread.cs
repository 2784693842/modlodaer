using System;
using KeraLua;
using NLua.Extensions;

namespace NLua
{
	// Token: 0x02000086 RID: 134
	internal class LuaThread : LuaBase, IEquatable<LuaThread>, IEquatable<Lua>, IEquatable<Lua>
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00010E8B File Offset: 0x0000F08B
		public Lua State
		{
			get
			{
				return this._luaState;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00010E94 File Offset: 0x0000F094
		public LuaThread MainThread
		{
			get
			{
				Lua mainThread = this._luaState.MainThread;
				int top = mainThread.GetTop();
				mainThread.PushThread();
				object @object = this._translator.GetObject(mainThread, -1);
				mainThread.SetTop(top);
				return (LuaThread)@object;
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00010ED4 File Offset: 0x0000F0D4
		public LuaThread(int reference, Lua interpreter) : base(reference, interpreter)
		{
			this._luaState = interpreter.GetThreadState(reference);
			this._translator = interpreter.Translator;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00010EF8 File Offset: 0x0000F0F8
		public int Reset()
		{
			int top = this._luaState.GetTop();
			int result = this._luaState.ResetThread();
			this._luaState.SetTop(top);
			return result;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00010F28 File Offset: 0x0000F128
		public void XMove(Lua to, object val, int index = 1)
		{
			int top = this._luaState.GetTop();
			this._translator.Push(this._luaState, val);
			this._luaState.XMove(to, index);
			this._luaState.SetTop(top);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00010F6C File Offset: 0x0000F16C
		public void XMove(Lua to, object val, int index = 1)
		{
			int top = this._luaState.GetTop();
			this._translator.Push(this._luaState, val);
			this._luaState.XMove(to.State, index);
			this._luaState.SetTop(top);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00010FB8 File Offset: 0x0000F1B8
		public void XMove(LuaThread thread, object val, int index = 1)
		{
			int top = this._luaState.GetTop();
			this._translator.Push(this._luaState, val);
			this._luaState.XMove(thread.State, index);
			this._luaState.SetTop(top);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00010E76 File Offset: 0x0000F076
		internal void Push(Lua luaState)
		{
			luaState.GetRef(this._Reference);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00011001 File Offset: 0x0000F201
		public override string ToString()
		{
			return "thread";
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00011008 File Offset: 0x0000F208
		public override bool Equals(object obj)
		{
			LuaThread luaThread = obj as LuaThread;
			if (luaThread != null)
			{
				return this.State == luaThread.State;
			}
			Lua lua = obj as Lua;
			if (lua != null)
			{
				return this.State == lua.State;
			}
			Lua lua2 = obj as Lua;
			if (lua2 != null)
			{
				return this.State == lua2;
			}
			return base.Equals(obj);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00011062 File Offset: 0x0000F262
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001106A File Offset: 0x0000F26A
		public bool Equals(LuaThread other)
		{
			return this.State == other.State;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001107A File Offset: 0x0000F27A
		public bool Equals(Lua other)
		{
			return this.State == other;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00011085 File Offset: 0x0000F285
		public bool Equals(Lua other)
		{
			return this.State == other.State;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00011095 File Offset: 0x0000F295
		public static explicit operator Lua(LuaThread thread)
		{
			return thread.State;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001109D File Offset: 0x0000F29D
		public static explicit operator LuaThread(Lua interpreter)
		{
			return interpreter.Thread;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000110A5 File Offset: 0x0000F2A5
		public static bool operator ==(LuaThread threadA, LuaThread threadB)
		{
			return threadA.State == threadB.State;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000110B5 File Offset: 0x0000F2B5
		public static bool operator !=(LuaThread threadA, LuaThread threadB)
		{
			return threadA.State != threadB.State;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000110C8 File Offset: 0x0000F2C8
		public static bool operator ==(LuaThread thread, Lua state)
		{
			return thread.State == state;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000110D3 File Offset: 0x0000F2D3
		public static bool operator !=(LuaThread thread, Lua state)
		{
			return thread.State != state;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000110E1 File Offset: 0x0000F2E1
		public static bool operator ==(Lua state, LuaThread thread)
		{
			return state == thread.State;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000110EC File Offset: 0x0000F2EC
		public static bool operator !=(Lua state, LuaThread thread)
		{
			return state != thread.State;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000110FA File Offset: 0x0000F2FA
		public static bool operator ==(LuaThread thread, Lua interpreter)
		{
			return thread.State == interpreter.State;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001110A File Offset: 0x0000F30A
		public static bool operator !=(LuaThread thread, Lua interpreter)
		{
			return thread.State != interpreter.State;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001111D File Offset: 0x0000F31D
		public static bool operator ==(Lua interpreter, LuaThread thread)
		{
			return interpreter.State == thread.State;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001112D File Offset: 0x0000F32D
		public static bool operator !=(Lua interpreter, LuaThread thread)
		{
			return interpreter.State != thread.State;
		}

		// Token: 0x0400018B RID: 395
		private Lua _luaState;

		// Token: 0x0400018C RID: 396
		private ObjectTranslator _translator;
	}
}
