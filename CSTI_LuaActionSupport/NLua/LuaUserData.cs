using System;
using KeraLua;
using NLua.Extensions;

namespace NLua
{
	// Token: 0x02000087 RID: 135
	internal class LuaUserData : LuaBase
	{
		// Token: 0x06000420 RID: 1056 RVA: 0x00010D23 File Offset: 0x0000EF23
		public LuaUserData(int reference, Lua interpreter) : base(reference, interpreter)
		{
		}

		// Token: 0x1700008D RID: 141
		public object this[string field]
		{
			get
			{
				Lua lua;
				if (!base.TryGet(out lua))
				{
					return null;
				}
				return lua.GetObject(this._Reference, field);
			}
			set
			{
				Lua lua;
				if (!base.TryGet(out lua))
				{
					return;
				}
				lua.SetObject(this._Reference, field, value);
			}
		}

		// Token: 0x1700008E RID: 142
		public object this[object field]
		{
			get
			{
				Lua lua;
				if (!base.TryGet(out lua))
				{
					return null;
				}
				return lua.GetObject(this._Reference, field);
			}
			set
			{
				Lua lua;
				if (!base.TryGet(out lua))
				{
					return;
				}
				lua.SetObject(this._Reference, field, value);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000111E0 File Offset: 0x0000F3E0
		public object[] Call(params object[] args)
		{
			Lua lua;
			if (!base.TryGet(out lua))
			{
				return null;
			}
			return lua.CallFunction(this, args);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00010E76 File Offset: 0x0000F076
		internal void Push(Lua luaState)
		{
			luaState.GetRef(this._Reference);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00011201 File Offset: 0x0000F401
		public override string ToString()
		{
			return "userdata";
		}
	}
}
