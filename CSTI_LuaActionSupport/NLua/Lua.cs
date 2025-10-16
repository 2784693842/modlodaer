using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using KeraLua;
using NLua.Event;
using NLua.Exceptions;
using NLua.Extensions;
using NLua.Method;

namespace NLua
{
	// Token: 0x02000061 RID: 97
	internal class Lua : IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001F5 RID: 501 RVA: 0x00009AA8 File Offset: 0x00007CA8
		// (remove) Token: 0x060001F6 RID: 502 RVA: 0x00009AE0 File Offset: 0x00007CE0
		public event EventHandler<HookExceptionEventArgs> HookException;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001F7 RID: 503 RVA: 0x00009B18 File Offset: 0x00007D18
		// (remove) Token: 0x060001F8 RID: 504 RVA: 0x00009B50 File Offset: 0x00007D50
		public event EventHandler<DebugHookEventArgs> DebugHook;

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00009B85 File Offset: 0x00007D85
		public bool IsExecuting
		{
			get
			{
				return this._executing;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00009B8D File Offset: 0x00007D8D
		public Lua State
		{
			get
			{
				return this._luaState;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001FB RID: 507 RVA: 0x00009B95 File Offset: 0x00007D95
		internal ObjectTranslator Translator
		{
			get
			{
				return this._translator;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00009B9D File Offset: 0x00007D9D
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00009BA5 File Offset: 0x00007DA5
		public bool UseTraceback { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00009BAE File Offset: 0x00007DAE
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00009BBB File Offset: 0x00007DBB
		public int MaximumRecursion
		{
			get
			{
				return this._globals.MaximumRecursion;
			}
			set
			{
				this._globals.MaximumRecursion = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00009BC9 File Offset: 0x00007DC9
		public IEnumerable<string> Globals
		{
			get
			{
				return this._globals.Globals;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00009BD8 File Offset: 0x00007DD8
		public LuaThread Thread
		{
			get
			{
				int top = this._luaState.GetTop();
				this._luaState.PushThread();
				object @object = this._translator.GetObject(this._luaState, -1);
				this._luaState.SetTop(top);
				return (LuaThread)@object;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00009C20 File Offset: 0x00007E20
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

		// Token: 0x06000203 RID: 515 RVA: 0x00009C60 File Offset: 0x00007E60
		public Lua(bool openLibs = true)
		{
			this._globals = new LuaGlobals();
			base..ctor();
			this._luaState = new Lua(openLibs);
			this.Init();
			this._luaState.AtPanic(new LuaFunction(Lua.PanicCallback));
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00009CA0 File Offset: 0x00007EA0
		public Lua(Lua luaState)
		{
			this._globals = new LuaGlobals();
			base..ctor();
			luaState.PushString("NLua_Loaded");
			luaState.GetTable(-1001000);
			if (luaState.ToBoolean(-1))
			{
				luaState.SetTop(-2);
				throw new LuaException("There is already a NLua.Lua instance associated with this Lua state");
			}
			this._luaState = luaState;
			this._StatePassed = true;
			luaState.SetTop(-2);
			this.Init();
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00009D10 File Offset: 0x00007F10
		private void Init()
		{
			this._luaState.PushString("NLua_Loaded");
			this._luaState.PushBoolean(true);
			this._luaState.SetTable(-1001000);
			if (!this._StatePassed)
			{
				this._luaState.NewTable();
				this._luaState.SetGlobal("luanet");
			}
			this._luaState.PushGlobalTable();
			this._luaState.GetGlobal("luanet");
			this._luaState.PushString("getmetatable");
			this._luaState.GetGlobal("getmetatable");
			this._luaState.SetTable(-3);
			this._luaState.PopGlobalTable();
			this._translator = new ObjectTranslator(this, this._luaState);
			ObjectTranslatorPool.Instance.Add(this._luaState, this._translator);
			this._luaState.PopGlobalTable();
			this._luaState.DoString("local a={}local rawget=rawget;local b=luanet.import_type;local c=luanet.load_assembly;luanet.error,luanet.type=error,type;function a:__index(d)local e=rawget(self,'.fqn')e=(e and e..'.'or'')..d;local f=rawget(luanet,d)or b(e)if f==nil then pcall(c,e)f={['.fqn']=e}setmetatable(f,a)end;rawset(self,d,f)return f end;function a:__call(...)error('No such type: '..rawget(self,'.fqn'),2)end;luanet['.fqn']=false;setmetatable(luanet,a)luanet.load_assembly('mscorlib')");
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00009E05 File Offset: 0x00008005
		public void Close()
		{
			if (this._StatePassed || this._luaState == null)
			{
				return;
			}
			this._luaState.Close();
			ObjectTranslatorPool.Instance.Remove(this._luaState);
			this._luaState = null;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009E3C File Offset: 0x0000803C
		private static int PanicCallback(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			throw new LuaException(string.Format("Unprotected error in call to Lua API ({0})", lua.ToString(-1, false)));
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009E68 File Offset: 0x00008068
		private void ThrowExceptionFromError(int oldTop)
		{
			object obj = this._translator.GetObject(this._luaState, -1);
			this._luaState.SetTop(oldTop);
			LuaScriptException ex = obj as LuaScriptException;
			if (ex != null)
			{
				throw ex;
			}
			if (obj == null)
			{
				obj = "Unknown Lua Error";
			}
			throw new LuaScriptException(obj.ToString(), string.Empty);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009EBC File Offset: 0x000080BC
		private static int PushDebugTraceback(Lua luaState, int argCount)
		{
			luaState.GetGlobal("debug");
			luaState.GetField(-1, "traceback");
			luaState.Remove(-2);
			int num = -argCount - 2;
			luaState.Insert(num);
			return num;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009EF8 File Offset: 0x000080F8
		public string GetDebugTraceback()
		{
			int top = this._luaState.GetTop();
			this._luaState.GetGlobal("debug");
			this._luaState.GetField(-1, "traceback");
			this._luaState.Remove(-2);
			this._luaState.PCall(0, -1, 0);
			return this._translator.PopValues(this._luaState, top)[0] as string;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009F6C File Offset: 0x0000816C
		internal int SetPendingException(Exception e)
		{
			if (e == null)
			{
				return 0;
			}
			this._translator.ThrowError(this._luaState, e);
			return 1;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009F94 File Offset: 0x00008194
		public LuaFunction LoadString(string chunk, string name)
		{
			int top = this._luaState.GetTop();
			this._executing = true;
			try
			{
				if (this._luaState.LoadString(chunk, name) != LuaStatus.OK)
				{
					this.ThrowExceptionFromError(top);
				}
			}
			finally
			{
				this._executing = false;
			}
			LuaFunction function = this._translator.GetFunction(this._luaState, -1);
			this._translator.PopValues(this._luaState, top);
			return function;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A00C File Offset: 0x0000820C
		public LuaFunction LoadString(byte[] chunk, string name)
		{
			int top = this._luaState.GetTop();
			this._executing = true;
			try
			{
				if (this._luaState.LoadBuffer(chunk, name) != LuaStatus.OK)
				{
					this.ThrowExceptionFromError(top);
				}
			}
			finally
			{
				this._executing = false;
			}
			LuaFunction function = this._translator.GetFunction(this._luaState, -1);
			this._translator.PopValues(this._luaState, top);
			return function;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A084 File Offset: 0x00008284
		public LuaFunction LoadFile(string fileName)
		{
			int top = this._luaState.GetTop();
			if (this._luaState.LoadFile(fileName) != LuaStatus.OK)
			{
				this.ThrowExceptionFromError(top);
			}
			LuaFunction function = this._translator.GetFunction(this._luaState, -1);
			this._translator.PopValues(this._luaState, top);
			return function;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000A0D8 File Offset: 0x000082D8
		public object[] DoString(byte[] chunk, string chunkName = "chunk")
		{
			int num = this._luaState.GetTop();
			this._executing = true;
			if (this._luaState.LoadBuffer(chunk, chunkName) != LuaStatus.OK)
			{
				this.ThrowExceptionFromError(num);
			}
			int errorFunctionIndex = 0;
			if (this.UseTraceback)
			{
				errorFunctionIndex = Lua.PushDebugTraceback(this._luaState, 0);
				num++;
			}
			object[] result;
			try
			{
				if (this._luaState.PCall(0, -1, errorFunctionIndex) != LuaStatus.OK)
				{
					this.ThrowExceptionFromError(num);
				}
				result = this._translator.PopValues(this._luaState, num);
			}
			finally
			{
				this._executing = false;
			}
			return result;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000A170 File Offset: 0x00008370
		public object[] DoString(string chunk, string chunkName = "chunk")
		{
			int num = this._luaState.GetTop();
			this._executing = true;
			if (this._luaState.LoadString(chunk, chunkName) != LuaStatus.OK)
			{
				this.ThrowExceptionFromError(num);
			}
			int errorFunctionIndex = 0;
			if (this.UseTraceback)
			{
				errorFunctionIndex = Lua.PushDebugTraceback(this._luaState, 0);
				num++;
			}
			object[] result;
			try
			{
				if (this._luaState.PCall(0, -1, errorFunctionIndex) != LuaStatus.OK)
				{
					this.ThrowExceptionFromError(num);
				}
				result = this._translator.PopValues(this._luaState, num);
			}
			finally
			{
				this._executing = false;
			}
			return result;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000A208 File Offset: 0x00008408
		public object[] DoFile(string fileName)
		{
			int num = this._luaState.GetTop();
			if (this._luaState.LoadFile(fileName) != LuaStatus.OK)
			{
				this.ThrowExceptionFromError(num);
			}
			this._executing = true;
			int errorFunctionIndex = 0;
			if (this.UseTraceback)
			{
				errorFunctionIndex = Lua.PushDebugTraceback(this._luaState, 0);
				num++;
			}
			object[] result;
			try
			{
				if (this._luaState.PCall(0, -1, errorFunctionIndex) != LuaStatus.OK)
				{
					this.ThrowExceptionFromError(num);
				}
				result = this._translator.PopValues(this._luaState, num);
			}
			finally
			{
				this._executing = false;
			}
			return result;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000A2A0 File Offset: 0x000084A0
		public object GetObjectFromPath(string fullPath)
		{
			int top = this._luaState.GetTop();
			string[] array = this.FullPathToArray(fullPath);
			this._luaState.GetGlobal(array[0]);
			object @object = this._translator.GetObject(this._luaState, -1);
			if (array.Length > 1)
			{
				LuaBase luaBase = @object as LuaBase;
				string[] array2 = new string[array.Length - 1];
				Array.Copy(array, 1, array2, 0, array.Length - 1);
				@object = this.GetObject(array2);
				if (luaBase != null)
				{
					luaBase.Dispose();
				}
			}
			this._luaState.SetTop(top);
			return @object;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000A328 File Offset: 0x00008528
		public void SetObjectToPath(string fullPath, object value)
		{
			int top = this._luaState.GetTop();
			string[] array = this.FullPathToArray(fullPath);
			if (array.Length == 1)
			{
				this._translator.Push(this._luaState, value);
				this._luaState.SetGlobal(fullPath);
			}
			else
			{
				this._luaState.GetGlobal(array[0]);
				string[] array2 = new string[array.Length - 1];
				Array.Copy(array, 1, array2, 0, array.Length - 1);
				this.SetObject(array2, value);
			}
			this._luaState.SetTop(top);
			if (value == null)
			{
				this._globals.RemoveGlobal(fullPath);
				return;
			}
			if (!this._globals.Contains(fullPath))
			{
				this._globals.RegisterGlobal(fullPath, value.GetType(), 0);
			}
		}

		// Token: 0x1700006C RID: 108
		public object this[string fullPath]
		{
			get
			{
				object objectFromPath = this.GetObjectFromPath(fullPath);
				if (objectFromPath is long)
				{
					long num = (long)objectFromPath;
					return (double)num;
				}
				return objectFromPath;
			}
			set
			{
				if (value != null && value.GetType().IsSubclassOf(typeof(MethodBase)))
				{
					this.RegisterFunction(fullPath, (MethodBase)value);
					return;
				}
				this.SetObjectToPath(fullPath, value);
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000A43C File Offset: 0x0000863C
		private object GetObject(string[] remainingPath)
		{
			object obj = null;
			for (int i = 0; i < remainingPath.Length; i++)
			{
				this._luaState.PushString(remainingPath[i]);
				this._luaState.GetTable(-2);
				obj = this._translator.GetObject(this._luaState, -1);
				if (obj == null)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000A48C File Offset: 0x0000868C
		public double GetNumber(string fullPath)
		{
			object objectFromPath = this.GetObjectFromPath(fullPath);
			if (objectFromPath is long)
			{
				long num = (long)objectFromPath;
				return (double)num;
			}
			return (double)objectFromPath;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000A4BC File Offset: 0x000086BC
		public int GetInteger(string fullPath)
		{
			object objectFromPath = this.GetObjectFromPath(fullPath);
			if (objectFromPath == null)
			{
				return 0;
			}
			return (int)((long)objectFromPath);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000A4E0 File Offset: 0x000086E0
		public long GetLong(string fullPath)
		{
			object objectFromPath = this.GetObjectFromPath(fullPath);
			if (objectFromPath == null)
			{
				return 0L;
			}
			return (long)objectFromPath;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000A504 File Offset: 0x00008704
		public string GetString(string fullPath)
		{
			object objectFromPath = this.GetObjectFromPath(fullPath);
			if (objectFromPath == null)
			{
				return null;
			}
			return objectFromPath.ToString();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000A524 File Offset: 0x00008724
		public LuaTable GetTable(string fullPath)
		{
			return (LuaTable)this.GetObjectFromPath(fullPath);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000A532 File Offset: 0x00008732
		public object GetTable(Type interfaceType, string fullPath)
		{
			return CodeGeneration.Instance.GetClassInstance(interfaceType, this.GetTable(fullPath));
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000A546 File Offset: 0x00008746
		public LuaThread GetThread(string fullPath)
		{
			return (LuaThread)this.GetObjectFromPath(fullPath);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A554 File Offset: 0x00008754
		public LuaFunction GetFunction(string fullPath)
		{
			object objectFromPath = this.GetObjectFromPath(fullPath);
			LuaFunction luaFunction = objectFromPath as LuaFunction;
			if (luaFunction != null)
			{
				return luaFunction;
			}
			return new LuaFunction((LuaFunction)objectFromPath, this);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000A583 File Offset: 0x00008783
		public void RegisterLuaDelegateType(Type delegateType, Type luaDelegateType)
		{
			CodeGeneration.Instance.RegisterLuaDelegateType(delegateType, luaDelegateType);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A591 File Offset: 0x00008791
		public void RegisterLuaClassType(Type klass, Type luaClass)
		{
			CodeGeneration.Instance.RegisterLuaClassType(klass, luaClass);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000A59F File Offset: 0x0000879F
		public void LoadCLRPackage()
		{
			this._luaState.DoString("if not luanet then require'luanet'end;local a,b=luanet.import_type,luanet.load_assembly;local c={__index=function(d,e)local f=rawget(d,e)if f==nil then f=a(d.packageName..\".\"..e)if f==nil then f=a(e)end;d[e]=f end;return f end}function luanet.namespace(g)if type(g)=='table'then local h={}for i=1,#g do h[i]=luanet.namespace(g[i])end;return unpack(h)end;local j={packageName=g}setmetatable(j,c)return j end;local k,l;local function m()l={}k={__index=function(n,e)for i,d in ipairs(l)do local f=d[e]if f then _G[e]=f;return f end end end}setmetatable(_G,k)end;function CLRPackage(o,p)p=p or o;local q=pcall(b,o)return luanet.namespace(p)end;function import(o,p)if not k then m()end;if not p then local i=o:find('%.dll$')if i then p=o:sub(1,i-1)else p=o end end;local j=CLRPackage(o,p)table.insert(l,j)return j end;function luanet.make_array(r,s)local t=r[#s]for i,u in ipairs(s)do t:SetValue(u,i-1)end;return t end;function luanet.each(v)local w=v:GetEnumerator()return function()if w:MoveNext()then return w.Current end end end");
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000A5B2 File Offset: 0x000087B2
		public Delegate GetFunction(Type delegateType, string fullPath)
		{
			return CodeGeneration.Instance.GetDelegate(delegateType, this.GetFunction(fullPath));
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000A5C6 File Offset: 0x000087C6
		internal object[] CallFunction(object function, object[] args)
		{
			return this.CallFunction(function, args, null);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000A5D4 File Offset: 0x000087D4
		internal object[] CallFunction(object function, object[] args, Type[] returnTypes)
		{
			int num = 0;
			int num2 = this._luaState.GetTop();
			if (!this._luaState.CheckStack(args.Length + 6))
			{
				throw new LuaException("Lua stack overflow");
			}
			this._translator.Push(this._luaState, function);
			if (args.Length != 0)
			{
				num = args.Length;
				for (int i = 0; i < args.Length; i++)
				{
					this._translator.Push(this._luaState, args[i]);
				}
			}
			this._executing = true;
			try
			{
				int errorFunctionIndex = 0;
				if (this.UseTraceback)
				{
					errorFunctionIndex = Lua.PushDebugTraceback(this._luaState, num);
					num2++;
				}
				if (this._luaState.PCall(num, -1, errorFunctionIndex) != LuaStatus.OK)
				{
					this.ThrowExceptionFromError(num2);
				}
			}
			finally
			{
				this._executing = false;
			}
			if (returnTypes != null)
			{
				return this._translator.PopValues(this._luaState, num2, returnTypes);
			}
			return this._translator.PopValues(this._luaState, num2);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000A6C4 File Offset: 0x000088C4
		private void SetObject(string[] remainingPath, object val)
		{
			for (int i = 0; i < remainingPath.Length - 1; i++)
			{
				this._luaState.PushString(remainingPath[i]);
				this._luaState.GetTable(-2);
			}
			this._luaState.PushString(remainingPath[remainingPath.Length - 1]);
			this._translator.Push(this._luaState, val);
			this._luaState.SetTable(-3);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A72E File Offset: 0x0000892E
		private string[] FullPathToArray(string fullPath)
		{
			return fullPath.SplitWithEscape('.', '\\').ToArray<string>();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A740 File Offset: 0x00008940
		public void NewTable(string fullPath)
		{
			string[] array = this.FullPathToArray(fullPath);
			int top = this._luaState.GetTop();
			if (array.Length == 1)
			{
				this._luaState.NewTable();
				this._luaState.SetGlobal(fullPath);
			}
			else
			{
				this._luaState.GetGlobal(array[0]);
				for (int i = 1; i < array.Length - 1; i++)
				{
					this._luaState.PushString(array[i]);
					this._luaState.GetTable(-2);
				}
				this._luaState.PushString(array[array.Length - 1]);
				this._luaState.NewTable();
				this._luaState.SetTable(-3);
			}
			this._luaState.SetTop(top);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A7F4 File Offset: 0x000089F4
		public Dictionary<object, object> GetTableDict(LuaTable table)
		{
			if (table == null)
			{
				throw new ArgumentNullException("table");
			}
			Dictionary<object, object> dictionary = new Dictionary<object, object>();
			int top = this._luaState.GetTop();
			this._translator.Push(this._luaState, table);
			this._luaState.PushNil();
			while (this._luaState.Next(-2))
			{
				dictionary[this._translator.GetObject(this._luaState, -2)] = this._translator.GetObject(this._luaState, -1);
				this._luaState.SetTop(-2);
			}
			this._luaState.SetTop(top);
			return dictionary;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000A894 File Offset: 0x00008A94
		public int SetDebugHook(LuaHookMask mask, int count)
		{
			if (this._hookCallback == null)
			{
				this._hookCallback = new LuaHookFunction(Lua.DebugHookCallback);
				this._luaState.SetHook(this._hookCallback, mask, count);
			}
			return -1;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A8C4 File Offset: 0x00008AC4
		public void RemoveDebugHook()
		{
			this._hookCallback = null;
			this._luaState.SetHook(null, LuaHookMask.Disabled, 0);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000A8DB File Offset: 0x00008ADB
		public LuaHookMask GetHookMask()
		{
			return this._luaState.HookMask;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000A8E8 File Offset: 0x00008AE8
		public int GetHookCount()
		{
			return this._luaState.HookCount;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000A8F5 File Offset: 0x00008AF5
		public string GetLocal(LuaDebug luaDebug, int n)
		{
			return this._luaState.GetLocal(luaDebug, n);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000A904 File Offset: 0x00008B04
		public string SetLocal(LuaDebug luaDebug, int n)
		{
			return this._luaState.SetLocal(luaDebug, n);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000A913 File Offset: 0x00008B13
		public int GetStack(int level, ref LuaDebug ar)
		{
			return this._luaState.GetStack(level, ref ar);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000A922 File Offset: 0x00008B22
		public bool GetInfo(string what, ref LuaDebug ar)
		{
			return this._luaState.GetInfo(what, ref ar);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000A931 File Offset: 0x00008B31
		public string GetUpValue(int funcindex, int n)
		{
			return this._luaState.GetUpValue(funcindex, n);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000A940 File Offset: 0x00008B40
		public string SetUpValue(int funcindex, int n)
		{
			return this._luaState.SetUpValue(funcindex, n);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000A950 File Offset: 0x00008B50
		private static void DebugHookCallback(IntPtr luaState, IntPtr luaDebug)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			lua.GetStack(0, luaDebug);
			if (!lua.GetInfo("Snlu", luaDebug))
			{
				return;
			}
			LuaDebug luaDebug2 = LuaDebug.FromIntPtr(luaDebug);
			ObjectTranslatorPool.Instance.Find(lua).Interpreter.DebugHookCallbackInternal(luaDebug2);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000A99C File Offset: 0x00008B9C
		private void DebugHookCallbackInternal(LuaDebug luaDebug)
		{
			try
			{
				EventHandler<DebugHookEventArgs> debugHook = this.DebugHook;
				if (debugHook != null)
				{
					debugHook(this, new DebugHookEventArgs(luaDebug));
				}
			}
			catch (Exception ex)
			{
				this.OnHookException(new HookExceptionEventArgs(ex));
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		private void OnHookException(HookExceptionEventArgs e)
		{
			EventHandler<HookExceptionEventArgs> hookException = this.HookException;
			if (hookException != null)
			{
				hookException(this, e);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000AA04 File Offset: 0x00008C04
		public object Pop()
		{
			int top = this._luaState.GetTop();
			return this._translator.PopValues(this._luaState, top - 1)[0];
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000AA33 File Offset: 0x00008C33
		public void Push(object value)
		{
			this._translator.Push(this._luaState, value);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000AA47 File Offset: 0x00008C47
		internal void DisposeInternal(int reference, bool finalized)
		{
			if (finalized && this._translator != null)
			{
				this._translator.AddFinalizedReference(reference);
				return;
			}
			if (this._luaState != null && !finalized)
			{
				this._luaState.Unref(reference);
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000AA78 File Offset: 0x00008C78
		internal object RawGetObject(int reference, string field)
		{
			int top = this._luaState.GetTop();
			this._luaState.GetRef(reference);
			this._luaState.PushString(field);
			this._luaState.RawGet(-2);
			object @object = this._translator.GetObject(this._luaState, -1);
			this._luaState.SetTop(top);
			return @object;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000AAD8 File Offset: 0x00008CD8
		internal object GetObject(int reference, string field)
		{
			int top = this._luaState.GetTop();
			this._luaState.GetRef(reference);
			object @object = this.GetObject(this.FullPathToArray(field));
			this._luaState.SetTop(top);
			return @object;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000AB18 File Offset: 0x00008D18
		internal object GetObject(int reference, object field)
		{
			int top = this._luaState.GetTop();
			this._luaState.GetRef(reference);
			this._translator.Push(this._luaState, field);
			this._luaState.GetTable(-2);
			object @object = this._translator.GetObject(this._luaState, -1);
			this._luaState.SetTop(top);
			return @object;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000AB7C File Offset: 0x00008D7C
		internal void SetObject(int reference, string field, object val)
		{
			int top = this._luaState.GetTop();
			this._luaState.GetRef(reference);
			this.SetObject(this.FullPathToArray(field), val);
			this._luaState.SetTop(top);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000ABBC File Offset: 0x00008DBC
		internal void SetObject(int reference, object field, object val)
		{
			int top = this._luaState.GetTop();
			this._luaState.GetRef(reference);
			this._translator.Push(this._luaState, field);
			this._translator.Push(this._luaState, val);
			this._luaState.SetTable(-3);
			this._luaState.SetTop(top);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000AC20 File Offset: 0x00008E20
		internal Lua GetThreadState(int reference)
		{
			int top = this._luaState.GetTop();
			this._luaState.GetRef(reference);
			Lua result = this._luaState.ToThread(-1);
			this._luaState.SetTop(top);
			return result;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000AC60 File Offset: 0x00008E60
		public void XMove(Lua to, object val, int index = 1)
		{
			int top = this._luaState.GetTop();
			this._translator.Push(this._luaState, val);
			this._luaState.XMove(to, index);
			this._luaState.SetTop(top);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		public void XMove(Lua to, object val, int index = 1)
		{
			int top = this._luaState.GetTop();
			this._translator.Push(this._luaState, val);
			this._luaState.XMove(to._luaState, index);
			this._luaState.SetTop(top);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000ACF0 File Offset: 0x00008EF0
		public void XMove(LuaThread thread, object val, int index = 1)
		{
			int top = this._luaState.GetTop();
			this._translator.Push(this._luaState, val);
			this._luaState.XMove(thread.State, index);
			this._luaState.SetTop(top);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000AD3C File Offset: 0x00008F3C
		public Lua NewThread(out LuaThread thread)
		{
			int top = this._luaState.GetTop();
			Lua result = this._luaState.NewThread();
			thread = (LuaThread)this._translator.GetObject(this._luaState, -1);
			this._luaState.SetTop(top);
			return result;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000AD88 File Offset: 0x00008F88
		public Lua NewThread(string fullPath)
		{
			string[] array = this.FullPathToArray(fullPath);
			int top = this._luaState.GetTop();
			Lua result;
			if (array.Length == 1)
			{
				result = this._luaState.NewThread();
				this._luaState.SetGlobal(fullPath);
			}
			else
			{
				this._luaState.GetGlobal(array[0]);
				for (int i = 1; i < array.Length - 1; i++)
				{
					this._luaState.PushString(array[i]);
					this._luaState.GetTable(-2);
				}
				this._luaState.PushString(array[array.Length - 1]);
				result = this._luaState.NewThread();
				this._luaState.SetTable(-3);
			}
			this._luaState.SetTop(top);
			return result;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000AE3C File Offset: 0x0000903C
		public Lua NewThread(LuaFunction function, out LuaThread thread)
		{
			int top = this._luaState.GetTop();
			Lua lua = this._luaState.NewThread();
			thread = (LuaThread)this._translator.GetObject(this._luaState, -1);
			this._translator.Push(this._luaState, function);
			this._luaState.XMove(lua, 1);
			this._luaState.SetTop(top);
			return lua;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000AEA8 File Offset: 0x000090A8
		public void NewThread(string fullPath, LuaFunction function)
		{
			string[] array = this.FullPathToArray(fullPath);
			int top = this._luaState.GetTop();
			Lua to;
			if (array.Length == 1)
			{
				to = this._luaState.NewThread();
				this._luaState.SetGlobal(fullPath);
			}
			else
			{
				this._luaState.GetGlobal(array[0]);
				for (int i = 1; i < array.Length - 1; i++)
				{
					this._luaState.PushString(array[i]);
					this._luaState.GetTable(-2);
				}
				this._luaState.PushString(array[array.Length - 1]);
				to = this._luaState.NewThread();
				this._luaState.SetTable(-3);
			}
			this._translator.Push(this._luaState, function);
			this._luaState.XMove(to, 1);
			this._luaState.SetTop(top);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000AF7A File Offset: 0x0000917A
		public LuaFunction RegisterFunction(string path, MethodBase function)
		{
			return this.RegisterFunction(path, null, function);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000AF88 File Offset: 0x00009188
		public LuaFunction RegisterFunction(string path, object target, MethodBase function)
		{
			int top = this._luaState.GetTop();
			LuaMethodWrapper luaMethodWrapper = new LuaMethodWrapper(this._translator, target, new ProxyType(function.DeclaringType), function);
			this._translator.Push(this._luaState, new LuaFunction(luaMethodWrapper.InvokeFunction.Invoke));
			object @object = this._translator.GetObject(this._luaState, -1);
			this.SetObjectToPath(path, @object);
			LuaFunction function2 = this.GetFunction(path);
			this._luaState.SetTop(top);
			return function2;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B00C File Offset: 0x0000920C
		internal bool CompareRef(int ref1, int ref2)
		{
			int top = this._luaState.GetTop();
			this._luaState.GetRef(ref1);
			this._luaState.GetRef(ref2);
			bool result = this._luaState.AreEqual(-1, -2);
			this._luaState.SetTop(top);
			return result;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000B057 File Offset: 0x00009257
		internal void PushCSFunction(LuaFunction function)
		{
			this._translator.PushFunction(this._luaState, function);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000B06C File Offset: 0x0000926C
		~Lua()
		{
			this.Dispose();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B098 File Offset: 0x00009298
		public virtual void Dispose()
		{
			if (this._translator != null)
			{
				this._translator.PendingEvents.Dispose();
				if (this._translator.Tag != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this._translator.Tag);
				}
				this._translator = null;
			}
			this.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x040000E5 RID: 229
		private LuaHookFunction _hookCallback;

		// Token: 0x040000E6 RID: 230
		private readonly LuaGlobals _globals;

		// Token: 0x040000E7 RID: 231
		private Lua _luaState;

		// Token: 0x040000E8 RID: 232
		private ObjectTranslator _translator;

		// Token: 0x040000E9 RID: 233
		private bool _StatePassed;

		// Token: 0x040000EA RID: 234
		private bool _executing;

		// Token: 0x040000EB RID: 235
		private const string InitLuanet = "local a={}local rawget=rawget;local b=luanet.import_type;local c=luanet.load_assembly;luanet.error,luanet.type=error,type;function a:__index(d)local e=rawget(self,'.fqn')e=(e and e..'.'or'')..d;local f=rawget(luanet,d)or b(e)if f==nil then pcall(c,e)f={['.fqn']=e}setmetatable(f,a)end;rawset(self,d,f)return f end;function a:__call(...)error('No such type: '..rawget(self,'.fqn'),2)end;luanet['.fqn']=false;setmetatable(luanet,a)luanet.load_assembly('mscorlib')";

		// Token: 0x040000EC RID: 236
		private const string ClrPackage = "if not luanet then require'luanet'end;local a,b=luanet.import_type,luanet.load_assembly;local c={__index=function(d,e)local f=rawget(d,e)if f==nil then f=a(d.packageName..\".\"..e)if f==nil then f=a(e)end;d[e]=f end;return f end}function luanet.namespace(g)if type(g)=='table'then local h={}for i=1,#g do h[i]=luanet.namespace(g[i])end;return unpack(h)end;local j={packageName=g}setmetatable(j,c)return j end;local k,l;local function m()l={}k={__index=function(n,e)for i,d in ipairs(l)do local f=d[e]if f then _G[e]=f;return f end end end}setmetatable(_G,k)end;function CLRPackage(o,p)p=p or o;local q=pcall(b,o)return luanet.namespace(p)end;function import(o,p)if not k then m()end;if not p then local i=o:find('%.dll$')if i then p=o:sub(1,i-1)else p=o end end;local j=CLRPackage(o,p)table.insert(l,j)return j end;function luanet.make_array(r,s)local t=r[#s]for i,u in ipairs(s)do t:SetValue(u,i-1)end;return t end;function luanet.each(v)local w=v:GetEnumerator()return function()if w:MoveNext()then return w.Current end end end";
	}
}
