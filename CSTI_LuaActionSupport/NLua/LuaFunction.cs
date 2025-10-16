using System;
using KeraLua;

namespace NLua
{
	// Token: 0x0200008C RID: 140
	internal class LuaFunction : LuaBase
	{
		// Token: 0x06000440 RID: 1088 RVA: 0x00011A1F File Offset: 0x0000FC1F
		public LuaFunction(int reference, Lua interpreter) : base(reference, interpreter)
		{
			this.function = null;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00011A30 File Offset: 0x0000FC30
		public LuaFunction(LuaFunction nativeFunction, Lua interpreter) : base(0, interpreter)
		{
			this.function = nativeFunction;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00011A44 File Offset: 0x0000FC44
		internal object[] Call(object[] args, Type[] returnTypes)
		{
			Lua lua;
			if (!base.TryGet(out lua))
			{
				return null;
			}
			return lua.CallFunction(this, args, returnTypes);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00011A68 File Offset: 0x0000FC68
		public object[] Call(params object[] args)
		{
			Lua lua;
			if (!base.TryGet(out lua))
			{
				return null;
			}
			return lua.CallFunction(this, args);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00011A8C File Offset: 0x0000FC8C
		internal void Push(Lua luaState)
		{
			Lua lua;
			if (!base.TryGet(out lua))
			{
				return;
			}
			if (this._Reference != 0)
			{
				luaState.RawGetInteger(LuaRegistry.Index, (long)this._Reference);
				return;
			}
			lua.PushCSFunction(this.function);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00011ACC File Offset: 0x0000FCCC
		public override string ToString()
		{
			return "function";
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00011AD4 File Offset: 0x0000FCD4
		public override bool Equals(object o)
		{
			LuaFunction luaFunction = o as LuaFunction;
			if (luaFunction == null)
			{
				return false;
			}
			Lua lua;
			if (!base.TryGet(out lua))
			{
				return false;
			}
			if (this._Reference != 0 && luaFunction._Reference != 0)
			{
				return lua.CompareRef(luaFunction._Reference, this._Reference);
			}
			return this.function == luaFunction.function;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00011B2D File Offset: 0x0000FD2D
		public override int GetHashCode()
		{
			if (this._Reference == 0)
			{
				return this.function.GetHashCode();
			}
			return this._Reference;
		}

		// Token: 0x0400019C RID: 412
		internal readonly LuaFunction function;
	}
}
