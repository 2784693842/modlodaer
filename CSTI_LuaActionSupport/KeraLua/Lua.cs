using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace KeraLua
{
	// Token: 0x0200006A RID: 106
	internal class Lua : IDisposable
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000B70B File Offset: 0x0000990B
		public IntPtr Handle
		{
			get
			{
				return this._luaState;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000B713 File Offset: 0x00009913
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000B71B File Offset: 0x0000991B
		public Encoding Encoding { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000B724 File Offset: 0x00009924
		public IntPtr ExtraSpace
		{
			get
			{
				return this._luaState - IntPtr.Size;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000B736 File Offset: 0x00009936
		public Lua MainThread
		{
			get
			{
				return this._mainState ?? this;
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000B743 File Offset: 0x00009943
		public Lua(bool openLibs = true)
		{
			this.Encoding = Encoding.ASCII;
			this._luaState = NativeMethods.luaL_newstate();
			if (openLibs)
			{
				this.OpenLibs();
			}
			this.SetExtraObject<Lua>(this, true);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000B772 File Offset: 0x00009972
		public Lua(LuaAlloc allocator, IntPtr ud)
		{
			this.Encoding = Encoding.ASCII;
			this._luaState = NativeMethods.lua_newstate(allocator.ToFunctionPointer(), ud);
			this.SetExtraObject<Lua>(this, true);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000B79F File Offset: 0x0000999F
		private Lua(IntPtr luaThread, Lua mainState)
		{
			this._mainState = mainState;
			this._luaState = luaThread;
			this.Encoding = mainState.Encoding;
			this.SetExtraObject<Lua>(this, false);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public static Lua FromIntPtr(IntPtr luaState)
		{
			if (luaState == IntPtr.Zero)
			{
				return null;
			}
			Lua extraObject = Lua.GetExtraObject<Lua>(luaState);
			if (extraObject != null && extraObject._luaState == luaState)
			{
				return extraObject;
			}
			return new Lua(luaState, extraObject.MainThread);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000B814 File Offset: 0x00009A14
		~Lua()
		{
			this.Dispose(false);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000B844 File Offset: 0x00009A44
		protected virtual void Dispose(bool disposing)
		{
			this.Close();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000B84C File Offset: 0x00009A4C
		public void Close()
		{
			if (this._luaState == IntPtr.Zero || this._mainState != null)
			{
				return;
			}
			NativeMethods.lua_close(this._luaState);
			this._luaState = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000B885 File Offset: 0x00009A85
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000B890 File Offset: 0x00009A90
		private void SetExtraObject<T>(T obj, bool weak) where T : class
		{
			GCHandle value = GCHandle.Alloc(obj, weak ? GCHandleType.Weak : GCHandleType.Normal);
			Marshal.WriteIntPtr(this._luaState - IntPtr.Size, GCHandle.ToIntPtr(value));
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000B8CC File Offset: 0x00009ACC
		private static T GetExtraObject<T>(IntPtr luaState) where T : class
		{
			GCHandle gchandle = GCHandle.FromIntPtr(Marshal.ReadIntPtr(luaState - IntPtr.Size));
			if (!gchandle.IsAllocated)
			{
				return default(T);
			}
			return (T)((object)gchandle.Target);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000B90E File Offset: 0x00009B0E
		public int AbsIndex(int index)
		{
			return NativeMethods.lua_absindex(this._luaState, index);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000B91C File Offset: 0x00009B1C
		public void Arith(LuaOperation operation)
		{
			NativeMethods.lua_arith(this._luaState, (int)operation);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000B92C File Offset: 0x00009B2C
		public LuaFunction AtPanic(LuaFunction panicFunction)
		{
			IntPtr panicf = panicFunction.ToFunctionPointer();
			return NativeMethods.lua_atpanic(this._luaState, panicf).ToLuaFunction();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000B951 File Offset: 0x00009B51
		public void Call(int arguments, int results)
		{
			NativeMethods.lua_callk(this._luaState, arguments, results, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000B96C File Offset: 0x00009B6C
		public void CallK(int arguments, int results, int context, LuaKFunction continuation)
		{
			IntPtr k = continuation.ToFunctionPointer();
			NativeMethods.lua_callk(this._luaState, arguments, results, (IntPtr)context, k);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000B995 File Offset: 0x00009B95
		public bool CheckStack(int nExtraSlots)
		{
			return NativeMethods.lua_checkstack(this._luaState, nExtraSlots) != 0;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000B9A6 File Offset: 0x00009BA6
		public bool Compare(int index1, int index2, LuaCompare comparison)
		{
			return NativeMethods.lua_compare(this._luaState, index1, index2, (int)comparison) != 0;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000B9B9 File Offset: 0x00009BB9
		public void Concat(int n)
		{
			NativeMethods.lua_concat(this._luaState, n);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000B9C7 File Offset: 0x00009BC7
		public void Copy(int fromIndex, int toIndex)
		{
			NativeMethods.lua_copy(this._luaState, fromIndex, toIndex);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000B9D6 File Offset: 0x00009BD6
		public void CreateTable(int elements, int records)
		{
			NativeMethods.lua_createtable(this._luaState, elements, records);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000B9E5 File Offset: 0x00009BE5
		public int Dump(LuaWriter writer, IntPtr data, bool stripDebug)
		{
			return NativeMethods.lua_dump(this._luaState, writer.ToFunctionPointer(), data, stripDebug ? 1 : 0);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000BA00 File Offset: 0x00009C00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Error()
		{
			return NativeMethods.lua_error(this._luaState);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000BA0D File Offset: 0x00009C0D
		public int GarbageCollector(LuaGC what, int data)
		{
			return NativeMethods.lua_gc(this._luaState, (int)what, data);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000BA1C File Offset: 0x00009C1C
		public int GarbageCollector(LuaGC what, int data, int data2)
		{
			return NativeMethods.lua_gc(this._luaState, (int)what, data, data2);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000BA2C File Offset: 0x00009C2C
		public LuaAlloc GetAllocFunction(ref IntPtr ud)
		{
			return NativeMethods.lua_getallocf(this._luaState, ref ud).ToLuaAlloc();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000BA3F File Offset: 0x00009C3F
		public LuaType GetField(int index, string key)
		{
			return (LuaType)NativeMethods.lua_getfield(this._luaState, index, key);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000BA3F File Offset: 0x00009C3F
		public LuaType GetField(LuaRegistry index, string key)
		{
			return (LuaType)NativeMethods.lua_getfield(this._luaState, (int)index, key);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000BA4E File Offset: 0x00009C4E
		public LuaType GetGlobal(string name)
		{
			return (LuaType)NativeMethods.lua_getglobal(this._luaState, name);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000BA5C File Offset: 0x00009C5C
		public LuaType GetInteger(int index, long i)
		{
			return (LuaType)NativeMethods.lua_geti(this._luaState, index, i);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000BA6B File Offset: 0x00009C6B
		public bool GetInfo(string what, IntPtr ar)
		{
			return NativeMethods.lua_getinfo(this._luaState, what, ar) != 0;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000BA80 File Offset: 0x00009C80
		public bool GetInfo(string what, ref LuaDebug ar)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LuaDebug>(ar));
			bool result = false;
			try
			{
				Marshal.StructureToPtr<LuaDebug>(ar, intPtr, false);
				result = this.GetInfo(what, intPtr);
				ar = LuaDebug.FromIntPtr(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000BADC File Offset: 0x00009CDC
		public string GetLocal(IntPtr ar, int n)
		{
			return Marshal.PtrToStringAnsi(NativeMethods.lua_getlocal(this._luaState, ar, n));
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000BAF0 File Offset: 0x00009CF0
		public string GetLocal(LuaDebug ar, int n)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LuaDebug>(ar));
			string result = string.Empty;
			try
			{
				Marshal.StructureToPtr<LuaDebug>(ar, intPtr, false);
				result = this.GetLocal(intPtr, n);
				ar = LuaDebug.FromIntPtr(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000BB44 File Offset: 0x00009D44
		public bool GetMetaTable(int index)
		{
			return NativeMethods.lua_getmetatable(this._luaState, index) != 0;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000BB55 File Offset: 0x00009D55
		public int GetStack(int level, IntPtr ar)
		{
			return NativeMethods.lua_getstack(this._luaState, level, ar);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000BB64 File Offset: 0x00009D64
		public int GetStack(int level, ref LuaDebug ar)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LuaDebug>(ar));
			int result = 0;
			try
			{
				Marshal.StructureToPtr<LuaDebug>(ar, intPtr, false);
				result = this.GetStack(level, intPtr);
				ar = LuaDebug.FromIntPtr(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public LuaType GetTable(int index)
		{
			return (LuaType)NativeMethods.lua_gettable(this._luaState, index);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public LuaType GetTable(LuaRegistry index)
		{
			return (LuaType)NativeMethods.lua_gettable(this._luaState, (int)index);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000BBCE File Offset: 0x00009DCE
		public int GetTop()
		{
			return NativeMethods.lua_gettop(this._luaState);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000BBDB File Offset: 0x00009DDB
		public int GetIndexedUserValue(int index, int nth)
		{
			return NativeMethods.lua_getiuservalue(this._luaState, index, nth);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000BBEA File Offset: 0x00009DEA
		public int GetUserValue(int index)
		{
			return this.GetIndexedUserValue(index, 1);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		public string GetUpValue(int functionIndex, int n)
		{
			return Marshal.PtrToStringAnsi(NativeMethods.lua_getupvalue(this._luaState, functionIndex, n));
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000BC08 File Offset: 0x00009E08
		public LuaHookFunction Hook
		{
			get
			{
				return NativeMethods.lua_gethook(this._luaState).ToLuaHookFunction();
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000BC1A File Offset: 0x00009E1A
		public int HookCount
		{
			get
			{
				return NativeMethods.lua_gethookcount(this._luaState);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000BC27 File Offset: 0x00009E27
		public LuaHookMask HookMask
		{
			get
			{
				return (LuaHookMask)NativeMethods.lua_gethookmask(this._luaState);
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000BC34 File Offset: 0x00009E34
		public void Insert(int index)
		{
			NativeMethods.lua_rotate(this._luaState, index, 1);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000BC43 File Offset: 0x00009E43
		public bool IsBoolean(int index)
		{
			return this.Type(index) == LuaType.Boolean;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000BC4F File Offset: 0x00009E4F
		public bool IsCFunction(int index)
		{
			return NativeMethods.lua_iscfunction(this._luaState, index) != 0;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000BC60 File Offset: 0x00009E60
		public bool IsFunction(int index)
		{
			return this.Type(index) == LuaType.Function;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000BC6C File Offset: 0x00009E6C
		public bool IsInteger(int index)
		{
			return NativeMethods.lua_isinteger(this._luaState, index) != 0;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000BC7D File Offset: 0x00009E7D
		public bool IsLightUserData(int index)
		{
			return this.Type(index) == LuaType.LightUserData;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000BC89 File Offset: 0x00009E89
		public bool IsNil(int index)
		{
			return this.Type(index) == LuaType.Nil;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000BC95 File Offset: 0x00009E95
		public bool IsNone(int index)
		{
			return this.Type(index) == LuaType.None;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000BCA1 File Offset: 0x00009EA1
		public bool IsNoneOrNil(int index)
		{
			return this.IsNone(index) || this.IsNil(index);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000BCB5 File Offset: 0x00009EB5
		public bool IsNumber(int index)
		{
			return NativeMethods.lua_isnumber(this._luaState, index) != 0;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000BCC6 File Offset: 0x00009EC6
		public bool IsStringOrNumber(int index)
		{
			return NativeMethods.lua_isstring(this._luaState, index) != 0;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000BCD7 File Offset: 0x00009ED7
		public bool IsString(int index)
		{
			return this.Type(index) == LuaType.String;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000BCE3 File Offset: 0x00009EE3
		public bool IsTable(int index)
		{
			return this.Type(index) == LuaType.Table;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000BCEF File Offset: 0x00009EEF
		public bool IsThread(int index)
		{
			return this.Type(index) == LuaType.Thread;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000BCFB File Offset: 0x00009EFB
		public bool IsUserData(int index)
		{
			return NativeMethods.lua_isuserdata(this._luaState, index) != 0;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public bool IsYieldable
		{
			get
			{
				return NativeMethods.lua_isyieldable(this._luaState) != 0;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000BD1C File Offset: 0x00009F1C
		public void PushLength(int index)
		{
			NativeMethods.lua_len(this._luaState, index);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000BD2A File Offset: 0x00009F2A
		public LuaStatus Load(LuaReader reader, IntPtr data, string chunkName, string mode)
		{
			return (LuaStatus)NativeMethods.lua_load(this._luaState, reader.ToFunctionPointer(), data, chunkName, mode);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000BD41 File Offset: 0x00009F41
		public void NewTable()
		{
			NativeMethods.lua_createtable(this._luaState, 0, 0);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000BD50 File Offset: 0x00009F50
		public Lua NewThread()
		{
			return new Lua(NativeMethods.lua_newthread(this._luaState), this);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000BD63 File Offset: 0x00009F63
		public IntPtr NewIndexedUserData(int size, int uv)
		{
			return NativeMethods.lua_newuserdatauv(this._luaState, (UIntPtr)((ulong)((long)size)), uv);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000BD78 File Offset: 0x00009F78
		public IntPtr NewUserData(int size)
		{
			return this.NewIndexedUserData(size, 1);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000BD82 File Offset: 0x00009F82
		public bool Next(int index)
		{
			return NativeMethods.lua_next(this._luaState, index) != 0;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000BD93 File Offset: 0x00009F93
		public LuaStatus PCall(int arguments, int results, int errorFunctionIndex)
		{
			return (LuaStatus)NativeMethods.lua_pcallk(this._luaState, arguments, results, errorFunctionIndex, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000BDAD File Offset: 0x00009FAD
		public LuaStatus PCallK(int arguments, int results, int errorFunctionIndex, int context, LuaKFunction k)
		{
			return (LuaStatus)NativeMethods.lua_pcallk(this._luaState, arguments, results, errorFunctionIndex, (IntPtr)context, k.ToFunctionPointer());
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000BDCB File Offset: 0x00009FCB
		public void Pop(int n)
		{
			NativeMethods.lua_settop(this._luaState, -n - 1);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000BDDC File Offset: 0x00009FDC
		public void PushBoolean(bool b)
		{
			NativeMethods.lua_pushboolean(this._luaState, b ? 1 : 0);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		public void PushCClosure(LuaFunction function, int n)
		{
			NativeMethods.lua_pushcclosure(this._luaState, function.ToFunctionPointer(), n);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000BE04 File Offset: 0x0000A004
		public void PushCFunction(LuaFunction function)
		{
			this.PushCClosure(function, 0);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000BE0E File Offset: 0x0000A00E
		public void PushGlobalTable()
		{
			NativeMethods.lua_rawgeti(this._luaState, -1001000, 2L);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000BE23 File Offset: 0x0000A023
		public void PushInteger(long n)
		{
			NativeMethods.lua_pushinteger(this._luaState, n);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000BE31 File Offset: 0x0000A031
		public void PushLightUserData(IntPtr data)
		{
			NativeMethods.lua_pushlightuserdata(this._luaState, data);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000BE40 File Offset: 0x0000A040
		public void PushObject<T>(T obj)
		{
			if (obj == null)
			{
				this.PushNil();
				return;
			}
			GCHandle value = GCHandle.Alloc(obj);
			this.PushLightUserData(GCHandle.ToIntPtr(value));
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000BE74 File Offset: 0x0000A074
		public void PushBuffer(byte[] buffer)
		{
			if (buffer == null)
			{
				this.PushNil();
				return;
			}
			NativeMethods.lua_pushlstring(this._luaState, buffer, (UIntPtr)((ulong)((long)buffer.Length)));
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000BE98 File Offset: 0x0000A098
		public void PushString(string value)
		{
			if (value == null)
			{
				this.PushNil();
				return;
			}
			byte[] bytes = this.Encoding.GetBytes(value);
			this.PushBuffer(bytes);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000BEC3 File Offset: 0x0000A0C3
		public void PushString(string value, params object[] args)
		{
			this.PushString(string.Format(value, args));
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000BED2 File Offset: 0x0000A0D2
		public void PushNil()
		{
			NativeMethods.lua_pushnil(this._luaState);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000BEDF File Offset: 0x0000A0DF
		public void PushNumber(double number)
		{
			NativeMethods.lua_pushnumber(this._luaState, number);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000BEED File Offset: 0x0000A0ED
		public bool PushThread()
		{
			return NativeMethods.lua_pushthread(this._luaState) == 1;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000BEFD File Offset: 0x0000A0FD
		public void PushCopy(int index)
		{
			NativeMethods.lua_pushvalue(this._luaState, index);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000BF0B File Offset: 0x0000A10B
		public bool RawEqual(int index1, int index2)
		{
			return NativeMethods.lua_rawequal(this._luaState, index1, index2) != 0;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000BF1D File Offset: 0x0000A11D
		public LuaType RawGet(int index)
		{
			return (LuaType)NativeMethods.lua_rawget(this._luaState, index);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000BF1D File Offset: 0x0000A11D
		public LuaType RawGet(LuaRegistry index)
		{
			return (LuaType)NativeMethods.lua_rawget(this._luaState, (int)index);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000BF2B File Offset: 0x0000A12B
		public LuaType RawGetInteger(int index, long n)
		{
			return (LuaType)NativeMethods.lua_rawgeti(this._luaState, index, n);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000BF2B File Offset: 0x0000A12B
		public LuaType RawGetInteger(LuaRegistry index, long n)
		{
			return (LuaType)NativeMethods.lua_rawgeti(this._luaState, (int)index, n);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000BF3A File Offset: 0x0000A13A
		public LuaType RawGetByHashCode(int index, object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "obj shouldn't be null");
			}
			return (LuaType)NativeMethods.lua_rawgetp(this._luaState, index, (IntPtr)obj.GetHashCode());
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000BF66 File Offset: 0x0000A166
		public int RawLen(int index)
		{
			return (int)((uint)NativeMethods.lua_rawlen(this._luaState, index));
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000BF79 File Offset: 0x0000A179
		public void RawSet(int index)
		{
			NativeMethods.lua_rawset(this._luaState, index);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000BF79 File Offset: 0x0000A179
		public void RawSet(LuaRegistry index)
		{
			NativeMethods.lua_rawset(this._luaState, (int)index);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000BF87 File Offset: 0x0000A187
		public void RawSetInteger(int index, long i)
		{
			NativeMethods.lua_rawseti(this._luaState, index, i);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000BF87 File Offset: 0x0000A187
		public void RawSetInteger(LuaRegistry index, long i)
		{
			NativeMethods.lua_rawseti(this._luaState, (int)index, i);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000BF96 File Offset: 0x0000A196
		public void RawSetByHashCode(int index, object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "obj shouldn't be null");
			}
			NativeMethods.lua_rawsetp(this._luaState, index, (IntPtr)obj.GetHashCode());
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000BFC2 File Offset: 0x0000A1C2
		public void Register(string name, LuaFunction function)
		{
			this.PushCFunction(function);
			this.SetGlobal(name);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000BFD2 File Offset: 0x0000A1D2
		public void Remove(int index)
		{
			this.Rotate(index, -1);
			this.Pop(1);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000BFE3 File Offset: 0x0000A1E3
		public void Replace(int index)
		{
			this.Copy(-1, index);
			this.Pop(1);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000BFF4 File Offset: 0x0000A1F4
		public LuaStatus Resume(Lua from, int arguments, out int results)
		{
			return (LuaStatus)NativeMethods.lua_resume(this._luaState, (from != null) ? from._luaState : IntPtr.Zero, arguments, out results);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000C014 File Offset: 0x0000A214
		public LuaStatus Resume(Lua from, int arguments)
		{
			int num;
			return (LuaStatus)NativeMethods.lua_resume(this._luaState, (from != null) ? from._luaState : IntPtr.Zero, arguments, out num);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000C03F File Offset: 0x0000A23F
		public int ResetThread()
		{
			return NativeMethods.lua_resetthread(this._luaState);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000C04C File Offset: 0x0000A24C
		public void Rotate(int index, int n)
		{
			NativeMethods.lua_rotate(this._luaState, index, n);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000C05B File Offset: 0x0000A25B
		public void SetAllocFunction(LuaAlloc alloc, ref IntPtr ud)
		{
			NativeMethods.lua_setallocf(this._luaState, alloc.ToFunctionPointer(), ud);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000C070 File Offset: 0x0000A270
		public void SetField(int index, string key)
		{
			NativeMethods.lua_setfield(this._luaState, index, key);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000C07F File Offset: 0x0000A27F
		public void SetHook(LuaHookFunction hookFunction, LuaHookMask mask, int count)
		{
			NativeMethods.lua_sethook(this._luaState, hookFunction.ToFunctionPointer(), (int)mask, count);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000C094 File Offset: 0x0000A294
		public void SetGlobal(string name)
		{
			NativeMethods.lua_setglobal(this._luaState, name);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000C0A2 File Offset: 0x0000A2A2
		public void SetInteger(int index, long n)
		{
			NativeMethods.lua_seti(this._luaState, index, n);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000C0B1 File Offset: 0x0000A2B1
		public string SetLocal(IntPtr ar, int n)
		{
			return Marshal.PtrToStringAnsi(NativeMethods.lua_setlocal(this._luaState, ar, n));
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
		public string SetLocal(LuaDebug ar, int n)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<LuaDebug>(ar));
			string result = string.Empty;
			try
			{
				Marshal.StructureToPtr<LuaDebug>(ar, intPtr, false);
				result = this.SetLocal(intPtr, n);
				ar = LuaDebug.FromIntPtr(intPtr);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000C11C File Offset: 0x0000A31C
		public void SetMetaTable(int index)
		{
			NativeMethods.lua_setmetatable(this._luaState, index);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000C12A File Offset: 0x0000A32A
		public void SetTable(int index)
		{
			NativeMethods.lua_settable(this._luaState, index);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000C138 File Offset: 0x0000A338
		public void SetTop(int newTop)
		{
			NativeMethods.lua_settop(this._luaState, newTop);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000C146 File Offset: 0x0000A346
		public string SetUpValue(int functionIndex, int n)
		{
			return Marshal.PtrToStringAnsi(NativeMethods.lua_setupvalue(this._luaState, functionIndex, n));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000C15A File Offset: 0x0000A35A
		public void SetWarningFunction(LuaWarnFunction function, IntPtr userData)
		{
			NativeMethods.lua_setwarnf(this._luaState, function.ToFunctionPointer(), userData);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000C16E File Offset: 0x0000A36E
		public void SetIndexedUserValue(int index, int nth)
		{
			NativeMethods.lua_setiuservalue(this._luaState, index, nth);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000C17D File Offset: 0x0000A37D
		public void SetUserValue(int index)
		{
			this.SetIndexedUserValue(index, 1);
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000C187 File Offset: 0x0000A387
		public LuaStatus Status
		{
			get
			{
				return (LuaStatus)NativeMethods.lua_status(this._luaState);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000C194 File Offset: 0x0000A394
		public bool StringToNumber(string s)
		{
			return NativeMethods.lua_stringtonumber(this._luaState, s) != UIntPtr.Zero;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		public bool ToBoolean(int index)
		{
			return NativeMethods.lua_toboolean(this._luaState, index) != 0;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000C1BD File Offset: 0x0000A3BD
		public LuaFunction ToCFunction(int index)
		{
			return NativeMethods.lua_tocfunction(this._luaState, index).ToLuaFunction();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000C1D0 File Offset: 0x0000A3D0
		public void ToClose(int index)
		{
			NativeMethods.lua_toclose(this._luaState, index);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		public long ToInteger(int index)
		{
			int num;
			return NativeMethods.lua_tointegerx(this._luaState, index, out num);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		public long? ToIntegerX(int index)
		{
			int num;
			long value = NativeMethods.lua_tointegerx(this._luaState, index, out num);
			if (num != 0)
			{
				return new long?(value);
			}
			return null;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000C22B File Offset: 0x0000A42B
		public byte[] ToBuffer(int index)
		{
			return this.ToBuffer(index, true);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000C238 File Offset: 0x0000A438
		public byte[] ToBuffer(int index, bool callMetamethod)
		{
			UIntPtr value;
			IntPtr intPtr;
			if (callMetamethod)
			{
				intPtr = NativeMethods.luaL_tolstring(this._luaState, index, out value);
				this.Pop(1);
			}
			else
			{
				intPtr = NativeMethods.lua_tolstring(this._luaState, index, out value);
			}
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			int num = (int)((uint)value);
			if (num == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[num];
			Marshal.Copy(intPtr, array, 0, num);
			return array;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
		public string ToString(int index)
		{
			return this.ToString(index, true);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		public string ToString(int index, bool callMetamethod)
		{
			byte[] array = this.ToBuffer(index, callMetamethod);
			if (array == null)
			{
				return null;
			}
			return this.Encoding.GetString(array);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000C2D4 File Offset: 0x0000A4D4
		public double ToNumber(int index)
		{
			int num;
			return NativeMethods.lua_tonumberx(this._luaState, index, out num);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000C2F0 File Offset: 0x0000A4F0
		public double? ToNumberX(int index)
		{
			int num;
			double value = NativeMethods.lua_tonumberx(this._luaState, index, out num);
			if (num != 0)
			{
				return new double?(value);
			}
			return null;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000C31F File Offset: 0x0000A51F
		public IntPtr ToPointer(int index)
		{
			return NativeMethods.lua_topointer(this._luaState, index);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000C330 File Offset: 0x0000A530
		public Lua ToThread(int index)
		{
			IntPtr intPtr = NativeMethods.lua_tothread(this._luaState, index);
			if (intPtr == this._luaState)
			{
				return this;
			}
			return Lua.FromIntPtr(intPtr);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000C360 File Offset: 0x0000A560
		public T ToObject<T>(int index, bool freeGCHandle = true)
		{
			if (this.IsNil(index) || !this.IsLightUserData(index))
			{
				return default(T);
			}
			IntPtr intPtr = this.ToUserData(index);
			if (intPtr == IntPtr.Zero)
			{
				return default(T);
			}
			GCHandle gchandle = GCHandle.FromIntPtr(intPtr);
			if (!gchandle.IsAllocated)
			{
				return default(T);
			}
			T result = (T)((object)gchandle.Target);
			if (freeGCHandle)
			{
				gchandle.Free();
			}
			return result;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000C3D8 File Offset: 0x0000A5D8
		public IntPtr ToUserData(int index)
		{
			return NativeMethods.lua_touserdata(this._luaState, index);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000C3E6 File Offset: 0x0000A5E6
		public LuaType Type(int index)
		{
			return (LuaType)NativeMethods.lua_type(this._luaState, index);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		public string TypeName(LuaType type)
		{
			return Marshal.PtrToStringAnsi(NativeMethods.lua_typename(this._luaState, (int)type));
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000C407 File Offset: 0x0000A607
		public long UpValueId(int functionIndex, int n)
		{
			return (long)NativeMethods.lua_upvalueid(this._luaState, functionIndex, n);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000C41B File Offset: 0x0000A61B
		public static int UpValueIndex(int i)
		{
			return -1001000 - i;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000C424 File Offset: 0x0000A624
		public void UpValueJoin(int functionIndex1, int n1, int functionIndex2, int n2)
		{
			NativeMethods.lua_upvaluejoin(this._luaState, functionIndex1, n1, functionIndex2, n2);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000C436 File Offset: 0x0000A636
		public double Version()
		{
			return NativeMethods.lua_version(this._luaState);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000C443 File Offset: 0x0000A643
		public void XMove(Lua to, int n)
		{
			if (to == null)
			{
				throw new ArgumentNullException("to", "to shouldn't be null");
			}
			NativeMethods.lua_xmove(this._luaState, to._luaState, n);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000C46A File Offset: 0x0000A66A
		public int Yield(int results)
		{
			return NativeMethods.lua_yieldk(this._luaState, results, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000C484 File Offset: 0x0000A684
		public int YieldK(int results, int context, LuaKFunction continuation)
		{
			IntPtr k = continuation.ToFunctionPointer();
			return NativeMethods.lua_yieldk(this._luaState, results, (IntPtr)context, k);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000C4AB File Offset: 0x0000A6AB
		public void ArgumentCheck(bool condition, int argument, string message)
		{
			if (condition)
			{
				return;
			}
			this.ArgumentError(argument, message);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000C4BA File Offset: 0x0000A6BA
		public int ArgumentError(int argument, string message)
		{
			return NativeMethods.luaL_argerror(this._luaState, argument, message);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000C4C9 File Offset: 0x0000A6C9
		public bool CallMetaMethod(int obj, string field)
		{
			return NativeMethods.luaL_callmeta(this._luaState, obj, field) != 0;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000C4DB File Offset: 0x0000A6DB
		public void CheckAny(int argument)
		{
			NativeMethods.luaL_checkany(this._luaState, argument);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000C4E9 File Offset: 0x0000A6E9
		public long CheckInteger(int argument)
		{
			return NativeMethods.luaL_checkinteger(this._luaState, argument);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
		public byte[] CheckBuffer(int argument)
		{
			UIntPtr value;
			IntPtr intPtr = NativeMethods.luaL_checklstring(this._luaState, argument, out value);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			int num = (int)((uint)value);
			if (num == 0)
			{
				return new byte[0];
			}
			byte[] array = new byte[num];
			Marshal.Copy(intPtr, array, 0, num);
			return array;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000C548 File Offset: 0x0000A748
		public string CheckString(int argument)
		{
			byte[] array = this.CheckBuffer(argument);
			if (array == null)
			{
				return null;
			}
			return this.Encoding.GetString(array);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000C56E File Offset: 0x0000A76E
		public double CheckNumber(int argument)
		{
			return NativeMethods.luaL_checknumber(this._luaState, argument);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000C57C File Offset: 0x0000A77C
		public int CheckOption(int argument, string def, string[] list)
		{
			return NativeMethods.luaL_checkoption(this._luaState, argument, def, list);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000C58C File Offset: 0x0000A78C
		public void CheckStack(int newSize, string message)
		{
			NativeMethods.luaL_checkstack(this._luaState, newSize, message);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000C59B File Offset: 0x0000A79B
		public void CheckType(int argument, LuaType type)
		{
			NativeMethods.luaL_checktype(this._luaState, argument, (int)type);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000C5AC File Offset: 0x0000A7AC
		public T CheckObject<T>(int argument, string typeName, bool freeGCHandle = true)
		{
			if (this.IsNil(argument) || !this.IsLightUserData(argument))
			{
				return default(T);
			}
			IntPtr intPtr = this.CheckUserData(argument, typeName);
			if (intPtr == IntPtr.Zero)
			{
				return default(T);
			}
			GCHandle gchandle = GCHandle.FromIntPtr(intPtr);
			if (!gchandle.IsAllocated)
			{
				return default(T);
			}
			T result = (T)((object)gchandle.Target);
			if (freeGCHandle)
			{
				gchandle.Free();
			}
			return result;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000C625 File Offset: 0x0000A825
		public IntPtr CheckUserData(int argument, string typeName)
		{
			return NativeMethods.luaL_checkudata(this._luaState, argument, typeName);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000C634 File Offset: 0x0000A834
		public bool DoFile(string file)
		{
			return this.LoadFile(file) != LuaStatus.OK || this.PCall(0, -1, 0) > LuaStatus.OK;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000C64D File Offset: 0x0000A84D
		public bool DoString(string file)
		{
			return this.LoadString(file) != LuaStatus.OK || this.PCall(0, -1, 0) > LuaStatus.OK;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000C668 File Offset: 0x0000A868
		public int Error(string value, params object[] v)
		{
			string message = string.Format(value, v);
			return NativeMethods.luaL_error(this._luaState, message);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000C689 File Offset: 0x0000A889
		public int ExecResult(int stat)
		{
			return NativeMethods.luaL_execresult(this._luaState, stat);
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000C697 File Offset: 0x0000A897
		public int FileResult(int stat, string fileName)
		{
			return NativeMethods.luaL_fileresult(this._luaState, stat, fileName);
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000C6A6 File Offset: 0x0000A8A6
		public LuaType GetMetaField(int obj, string field)
		{
			return (LuaType)NativeMethods.luaL_getmetafield(this._luaState, obj, field);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000C6B5 File Offset: 0x0000A8B5
		public LuaType GetMetaTable(string tableName)
		{
			return this.GetField(LuaRegistry.Index, tableName);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000C6C3 File Offset: 0x0000A8C3
		public bool GetSubTable(int index, string name)
		{
			return NativeMethods.luaL_getsubtable(this._luaState, index, name) != 0;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000C6D5 File Offset: 0x0000A8D5
		public long Length(int index)
		{
			return NativeMethods.luaL_len(this._luaState, index);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000C6E3 File Offset: 0x0000A8E3
		public LuaStatus LoadBuffer(byte[] buffer, string name, string mode)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "buffer shouldn't be null");
			}
			return (LuaStatus)NativeMethods.luaL_loadbufferx(this._luaState, buffer, (UIntPtr)((ulong)((long)buffer.Length)), name, mode);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000C70F File Offset: 0x0000A90F
		public LuaStatus LoadBuffer(byte[] buffer, string name)
		{
			return this.LoadBuffer(buffer, name, null);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000C71A File Offset: 0x0000A91A
		public LuaStatus LoadBuffer(byte[] buffer)
		{
			return this.LoadBuffer(buffer, null, null);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000C728 File Offset: 0x0000A928
		public LuaStatus LoadString(string chunk, string name)
		{
			byte[] bytes = this.Encoding.GetBytes(chunk);
			return this.LoadBuffer(bytes, name);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000C74A File Offset: 0x0000A94A
		public LuaStatus LoadString(string chunk)
		{
			return this.LoadString(chunk, null);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000C754 File Offset: 0x0000A954
		public LuaStatus LoadFile(string file, string mode)
		{
			return (LuaStatus)NativeMethods.luaL_loadfilex(this._luaState, file, mode);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000C763 File Offset: 0x0000A963
		public LuaStatus LoadFile(string file)
		{
			return this.LoadFile(file, null);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000C76D File Offset: 0x0000A96D
		public void NewLib(LuaRegister[] library)
		{
			this.NewLibTable(library);
			this.SetFuncs(library, 0);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000C77E File Offset: 0x0000A97E
		public void NewLibTable(LuaRegister[] library)
		{
			if (library == null)
			{
				throw new ArgumentNullException("library", "library shouldn't be null");
			}
			this.CreateTable(0, library.Length);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000C79D File Offset: 0x0000A99D
		public bool NewMetaTable(string name)
		{
			return NativeMethods.luaL_newmetatable(this._luaState, name) != 0;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000C7AE File Offset: 0x0000A9AE
		public void OpenLibs()
		{
			NativeMethods.luaL_openlibs(this._luaState);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000C7BB File Offset: 0x0000A9BB
		public long OptInteger(int argument, long d)
		{
			return NativeMethods.luaL_optinteger(this._luaState, argument, d);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000C7CA File Offset: 0x0000A9CA
		public byte[] OptBuffer(int index, byte[] def)
		{
			if (this.IsNoneOrNil(index))
			{
				return def;
			}
			return this.CheckBuffer(index);
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000C7DE File Offset: 0x0000A9DE
		public string OptString(int index, string def)
		{
			if (this.IsNoneOrNil(index))
			{
				return def;
			}
			return this.CheckString(index);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000C7F2 File Offset: 0x0000A9F2
		public double OptNumber(int index, double def)
		{
			return NativeMethods.luaL_optnumber(this._luaState, index, def);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000C801 File Offset: 0x0000AA01
		public int Ref(LuaRegistry tableIndex)
		{
			return NativeMethods.luaL_ref(this._luaState, (int)tableIndex);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000C80F File Offset: 0x0000AA0F
		public void RequireF(string moduleName, LuaFunction openFunction, bool global)
		{
			NativeMethods.luaL_requiref(this._luaState, moduleName, openFunction.ToFunctionPointer(), global ? 1 : 0);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000C82A File Offset: 0x0000AA2A
		public void SetFuncs(LuaRegister[] library, int numberUpValues)
		{
			NativeMethods.luaL_setfuncs(this._luaState, library, numberUpValues);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000C839 File Offset: 0x0000AA39
		public void SetMetaTable(string name)
		{
			NativeMethods.luaL_setmetatable(this._luaState, name);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000C848 File Offset: 0x0000AA48
		public T TestObject<T>(int argument, string typeName, bool freeGCHandle = true)
		{
			if (this.IsNil(argument) || !this.IsLightUserData(argument))
			{
				return default(T);
			}
			IntPtr intPtr = this.TestUserData(argument, typeName);
			if (intPtr == IntPtr.Zero)
			{
				return default(T);
			}
			GCHandle gchandle = GCHandle.FromIntPtr(intPtr);
			if (!gchandle.IsAllocated)
			{
				return default(T);
			}
			T result = (T)((object)gchandle.Target);
			if (freeGCHandle)
			{
				gchandle.Free();
			}
			return result;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000C8C1 File Offset: 0x0000AAC1
		public IntPtr TestUserData(int argument, string typeName)
		{
			return NativeMethods.luaL_testudata(this._luaState, argument, typeName);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		public void Traceback(Lua state, int level = 0)
		{
			this.Traceback(state, null, level);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000C8DB File Offset: 0x0000AADB
		public void Traceback(Lua state, string message, int level)
		{
			if (state == null)
			{
				throw new ArgumentNullException("state", "state shouldn't be null");
			}
			NativeMethods.luaL_traceback(this._luaState, state._luaState, message, level);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000C904 File Offset: 0x0000AB04
		public string TypeName(int index)
		{
			LuaType type = this.Type(index);
			return this.TypeName(type);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000C920 File Offset: 0x0000AB20
		public void Unref(LuaRegistry tableIndex, int reference)
		{
			NativeMethods.luaL_unref(this._luaState, (int)tableIndex, reference);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000C92F File Offset: 0x0000AB2F
		public void Warning(string message, bool toContinue)
		{
			NativeMethods.lua_warning(this._luaState, message, toContinue ? 1 : 0);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000C944 File Offset: 0x0000AB44
		public void Where(int level)
		{
			NativeMethods.luaL_where(this._luaState, level);
		}

		// Token: 0x0400010F RID: 271
		private IntPtr _luaState;

		// Token: 0x04000110 RID: 272
		private readonly Lua _mainState;
	}
}
