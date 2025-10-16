using System;
using System.Collections;
using KeraLua;
using NLua.Extensions;

namespace NLua
{
	// Token: 0x02000085 RID: 133
	internal class LuaTable : LuaBase
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x00010D23 File Offset: 0x0000EF23
		public LuaTable(int reference, Lua interpreter) : base(reference, interpreter)
		{
		}

		// Token: 0x17000087 RID: 135
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

		// Token: 0x17000088 RID: 136
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

		// Token: 0x06000400 RID: 1024 RVA: 0x00010DD0 File Offset: 0x0000EFD0
		public IDictionaryEnumerator GetEnumerator()
		{
			Lua lua;
			if (!base.TryGet(out lua))
			{
				return null;
			}
			return lua.GetTableDict(this).GetEnumerator();
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x00010DFC File Offset: 0x0000EFFC
		public ICollection Keys
		{
			get
			{
				Lua lua;
				if (!base.TryGet(out lua))
				{
					return null;
				}
				return lua.GetTableDict(this).Keys;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00010E24 File Offset: 0x0000F024
		public ICollection Values
		{
			get
			{
				Lua lua;
				if (!base.TryGet(out lua))
				{
					return new object[0];
				}
				return lua.GetTableDict(this).Values;
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00010E50 File Offset: 0x0000F050
		internal object RawGet(string field)
		{
			Lua lua;
			if (!base.TryGet(out lua))
			{
				return null;
			}
			return lua.RawGetObject(this._Reference, field);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00010E76 File Offset: 0x0000F076
		internal void Push(Lua luaState)
		{
			luaState.GetRef(this._Reference);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00010E84 File Offset: 0x0000F084
		public override string ToString()
		{
			return "table";
		}
	}
}
