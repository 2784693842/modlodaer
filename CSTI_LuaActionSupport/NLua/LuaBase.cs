using System;

namespace NLua
{
	// Token: 0x02000060 RID: 96
	internal abstract class LuaBase : IDisposable
	{
		// Token: 0x060001ED RID: 493 RVA: 0x0000998D File Offset: 0x00007B8D
		protected bool TryGet(out Lua lua)
		{
			if (this._lua.State == null)
			{
				lua = null;
				return false;
			}
			lua = this._lua;
			return true;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000099AA File Offset: 0x00007BAA
		protected LuaBase(int reference, Lua lua)
		{
			this._lua = lua;
			this._Reference = reference;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000099C0 File Offset: 0x00007BC0
		~LuaBase()
		{
			this.Dispose(false);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000099F0 File Offset: 0x00007BF0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009A00 File Offset: 0x00007C00
		private void DisposeLuaReference(bool finalized)
		{
			if (this._lua == null)
			{
				return;
			}
			Lua lua;
			if (!this.TryGet(out lua))
			{
				return;
			}
			lua.DisposeInternal(this._Reference, finalized);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009A30 File Offset: 0x00007C30
		public virtual void Dispose(bool disposeManagedResources)
		{
			if (this._disposed)
			{
				return;
			}
			bool finalized = !disposeManagedResources;
			if (this._Reference != 0)
			{
				this.DisposeLuaReference(finalized);
			}
			this._lua = null;
			this._disposed = true;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009A68 File Offset: 0x00007C68
		public override bool Equals(object o)
		{
			LuaBase luaBase = o as LuaBase;
			Lua lua;
			return luaBase != null && this.TryGet(out lua) && lua.CompareRef(luaBase._Reference, this._Reference);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009A9F File Offset: 0x00007C9F
		public override int GetHashCode()
		{
			return this._Reference;
		}

		// Token: 0x040000E0 RID: 224
		private bool _disposed;

		// Token: 0x040000E1 RID: 225
		protected readonly int _Reference;

		// Token: 0x040000E2 RID: 226
		private Lua _lua;
	}
}
