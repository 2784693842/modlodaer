using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using KeraLua;
using NLua.Exceptions;
using NLua.Extensions;
using NLua.Method;

namespace NLua
{
	// Token: 0x02000079 RID: 121
	internal class ObjectTranslator
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000C952 File Offset: 0x0000AB52
		public MetaFunctions MetaFunctionsInstance
		{
			get
			{
				return this.metaFunctions;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000C95A File Offset: 0x0000AB5A
		public Lua Interpreter
		{
			get
			{
				return this.interpreter;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000C962 File Offset: 0x0000AB62
		public IntPtr Tag
		{
			get
			{
				return this._tagPtr;
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C96C File Offset: 0x0000AB6C
		public ObjectTranslator(Lua interpreter, Lua luaState)
		{
			this._objectsBackMap = new Dictionary<object, int>(new ObjectTranslator.ReferenceComparer());
			this._objects = new Dictionary<int, object>();
			this.finalizedReferences = new ConcurrentQueue<int>();
			this.PendingEvents = new EventHandlerContainer();
			base..ctor();
			this._tagPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
			this.interpreter = interpreter;
			this.typeChecker = new CheckType(this);
			this.metaFunctions = new MetaFunctions(this);
			this.assemblies = new List<Assembly>();
			this.CreateLuaObjectList(luaState);
			this.CreateIndexingMetaFunction(luaState);
			this.CreateBaseClassMetatable(luaState);
			this.CreateClassMetatable(luaState);
			this.CreateFunctionMetatable(luaState);
			this.SetGlobalFunctions(luaState);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000CA20 File Offset: 0x0000AC20
		private void CreateLuaObjectList(Lua luaState)
		{
			luaState.PushString("luaNet_objects");
			luaState.NewTable();
			luaState.NewTable();
			luaState.PushString("__mode");
			luaState.PushString("v");
			luaState.SetTable(-3);
			luaState.SetMetaTable(-2);
			luaState.SetTable(-1001000);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000CA75 File Offset: 0x0000AC75
		private void CreateIndexingMetaFunction(Lua luaState)
		{
			luaState.PushString("luaNet_indexfunction");
			luaState.DoString("local a={}local function b(c,d)local e=getmetatable(c)local f=e.cache[d]if f~=nil then if f==a then return nil end;return f else local g,h=get_object_member(c,d)if h then if g==nil then e.cache[d]=a else e.cache[d]=g end end;return g end end;return b");
			luaState.RawSet(LuaRegistry.Index);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000CA9C File Offset: 0x0000AC9C
		private void CreateBaseClassMetatable(Lua luaState)
		{
			luaState.NewMetaTable("luaNet_searchbase");
			luaState.PushString("__gc");
			luaState.PushCFunction(MetaFunctions.GcFunction);
			luaState.SetTable(-3);
			luaState.PushString("__tostring");
			luaState.PushCFunction(MetaFunctions.ToStringFunction);
			luaState.SetTable(-3);
			luaState.PushString("__index");
			luaState.PushCFunction(MetaFunctions.BaseIndexFunction);
			luaState.SetTable(-3);
			luaState.PushString("__newindex");
			luaState.PushCFunction(MetaFunctions.NewIndexFunction);
			luaState.SetTable(-3);
			luaState.SetTop(-2);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000CB38 File Offset: 0x0000AD38
		private void CreateClassMetatable(Lua luaState)
		{
			luaState.NewMetaTable("luaNet_class");
			luaState.PushString("__gc");
			luaState.PushCFunction(MetaFunctions.GcFunction);
			luaState.SetTable(-3);
			luaState.PushString("__tostring");
			luaState.PushCFunction(MetaFunctions.ToStringFunction);
			luaState.SetTable(-3);
			luaState.PushString("__index");
			luaState.PushCFunction(MetaFunctions.ClassIndexFunction);
			luaState.SetTable(-3);
			luaState.PushString("__newindex");
			luaState.PushCFunction(MetaFunctions.ClassNewIndexFunction);
			luaState.SetTable(-3);
			luaState.PushString("__call");
			luaState.PushCFunction(MetaFunctions.CallConstructorFunction);
			luaState.SetTable(-3);
			luaState.SetTop(-2);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000CBF0 File Offset: 0x0000ADF0
		private void SetGlobalFunctions(Lua luaState)
		{
			luaState.PushCFunction(MetaFunctions.IndexFunction);
			luaState.SetGlobal("get_object_member");
			luaState.PushCFunction(ObjectTranslator._importTypeFunction);
			luaState.SetGlobal("import_type");
			luaState.PushCFunction(ObjectTranslator._loadAssemblyFunction);
			luaState.SetGlobal("load_assembly");
			luaState.PushCFunction(ObjectTranslator._registerTableFunction);
			luaState.SetGlobal("make_object");
			luaState.PushCFunction(ObjectTranslator._unregisterTableFunction);
			luaState.SetGlobal("free_object");
			luaState.PushCFunction(ObjectTranslator._getMethodSigFunction);
			luaState.SetGlobal("get_method_bysig");
			luaState.PushCFunction(ObjectTranslator._getConstructorSigFunction);
			luaState.SetGlobal("get_constructor_bysig");
			luaState.PushCFunction(ObjectTranslator._ctypeFunction);
			luaState.SetGlobal("ctype");
			luaState.PushCFunction(ObjectTranslator._enumFromIntFunction);
			luaState.SetGlobal("enum");
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000CCC4 File Offset: 0x0000AEC4
		private void CreateFunctionMetatable(Lua luaState)
		{
			luaState.NewMetaTable("luaNet_function");
			luaState.PushString("__gc");
			luaState.PushCFunction(MetaFunctions.GcFunction);
			luaState.SetTable(-3);
			luaState.PushString("__call");
			luaState.PushCFunction(MetaFunctions.ExecuteDelegateFunction);
			luaState.SetTable(-3);
			luaState.SetTop(-2);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000CD24 File Offset: 0x0000AF24
		internal void ThrowError(Lua luaState, object e)
		{
			int top = luaState.GetTop();
			luaState.Where(1);
			object[] array = this.PopValues(luaState, top);
			string source = string.Empty;
			if (array.Length != 0)
			{
				source = array[0].ToString();
			}
			string text = e as string;
			if (text != null)
			{
				if (this.interpreter.UseTraceback)
				{
					text = text + Environment.NewLine + this.interpreter.GetDebugTraceback();
				}
				e = new LuaScriptException(text, source);
			}
			else
			{
				Exception ex = e as Exception;
				if (ex != null)
				{
					if (this.interpreter.UseTraceback)
					{
						ex.Data["Traceback"] = this.interpreter.GetDebugTraceback();
					}
					e = new LuaScriptException(ex, source);
				}
			}
			this.Push(luaState, e);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000CDDC File Offset: 0x0000AFDC
		private static int LoadAssembly(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = objectTranslator.LoadAssemblyInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000CE1C File Offset: 0x0000B01C
		private int LoadAssemblyInternal(Lua luaState)
		{
			try
			{
				string text = luaState.ToString(1, false);
				Assembly assembly = null;
				Exception ex = null;
				try
				{
					assembly = Assembly.Load(text);
				}
				catch (BadImageFormatException)
				{
				}
				catch (FileNotFoundException ex)
				{
				}
				if (assembly == null)
				{
					try
					{
						assembly = Assembly.Load(AssemblyName.GetAssemblyName(text));
					}
					catch (FileNotFoundException ex)
					{
					}
					if (assembly == null)
					{
						AssemblyName name = this.assemblies[0].GetName();
						AssemblyName assemblyName = new AssemblyName();
						assemblyName.Name = text;
						assemblyName.CultureInfo = name.CultureInfo;
						assemblyName.Version = name.Version;
						assemblyName.SetPublicKeyToken(name.GetPublicKeyToken());
						assemblyName.SetPublicKey(name.GetPublicKey());
						assembly = Assembly.Load(assemblyName);
						if (assembly != null)
						{
							ex = null;
						}
					}
					if (ex != null)
					{
						this.ThrowError(luaState, ex);
						return 1;
					}
				}
				if (assembly != null && !this.assemblies.Contains(assembly))
				{
					this.assemblies.Add(assembly);
				}
			}
			catch (Exception e)
			{
				this.ThrowError(luaState, e);
				return 1;
			}
			return 0;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000CF48 File Offset: 0x0000B148
		internal Type FindType(string className)
		{
			foreach (Assembly assembly in this.assemblies)
			{
				Type type = assembly.GetType(className);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000CFAC File Offset: 0x0000B1AC
		public bool TryGetExtensionMethod(Type type, string name, out MethodInfo method)
		{
			method = this.GetExtensionMethod(type, name);
			return method != null;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		public MethodInfo GetExtensionMethod(Type type, string name)
		{
			return type.GetExtensionMethod(name, this.assemblies);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000CFD0 File Offset: 0x0000B1D0
		private static int ImportType(IntPtr luaState)
		{
			Lua luaState2 = Lua.FromIntPtr(luaState);
			return ObjectTranslatorPool.Instance.Find(luaState2).ImportTypeInternal(luaState2);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000CFF8 File Offset: 0x0000B1F8
		private int ImportTypeInternal(Lua luaState)
		{
			string className = luaState.ToString(1, false);
			Type type = this.FindType(className);
			if (type != null)
			{
				this.PushType(luaState, type);
			}
			else
			{
				luaState.PushNil();
			}
			return 1;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000D030 File Offset: 0x0000B230
		private static int RegisterTable(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = objectTranslator.RegisterTableInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000D070 File Offset: 0x0000B270
		private int RegisterTableInternal(Lua luaState)
		{
			if (luaState.Type(1) != LuaType.Table)
			{
				this.ThrowError(luaState, "register_table: first arg is not a table");
				return 1;
			}
			LuaTable table = this.GetTable(luaState, 1);
			string text = luaState.ToString(2, false);
			if (string.IsNullOrEmpty(text))
			{
				this.ThrowError(luaState, "register_table: superclass name can not be null");
				return 1;
			}
			Type type = this.FindType(text);
			if (type == null)
			{
				this.ThrowError(luaState, "register_table: can not find superclass '" + text + "'");
				return 1;
			}
			object classInstance = CodeGeneration.Instance.GetClassInstance(type, table);
			this.PushObject(luaState, classInstance, "luaNet_metatable");
			luaState.NewTable();
			luaState.PushString("__index");
			luaState.PushCopy(-3);
			luaState.SetTable(-3);
			luaState.PushString("__newindex");
			luaState.PushCopy(-3);
			luaState.SetTable(-3);
			luaState.SetMetaTable(1);
			luaState.PushString("base");
			int index = this.AddObject(classInstance);
			this.PushNewObject(luaState, classInstance, index, "luaNet_searchbase");
			luaState.RawSet(1);
			return 0;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000D170 File Offset: 0x0000B370
		private static int UnregisterTable(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = objectTranslator.UnregisterTableInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		private int UnregisterTableInternal(Lua luaState)
		{
			if (!luaState.GetMetaTable(1))
			{
				this.ThrowError(luaState, "unregister_table: arg is not valid table");
				return 1;
			}
			luaState.PushString("__index");
			luaState.GetTable(-2);
			object rawNetObject = this.GetRawNetObject(luaState, -1);
			if (rawNetObject == null)
			{
				this.ThrowError(luaState, "unregister_table: arg is not valid table");
				return 1;
			}
			FieldInfo field = rawNetObject.GetType().GetField("__luaInterface_luaTable");
			if (field == null)
			{
				this.ThrowError(luaState, "unregister_table: arg is not valid table");
				return 1;
			}
			field.SetValue(rawNetObject, null);
			luaState.PushNil();
			luaState.SetMetaTable(1);
			luaState.PushString("base");
			luaState.PushNil();
			luaState.SetTable(1);
			return 0;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000D258 File Offset: 0x0000B458
		private static int GetMethodSignature(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int methodSignatureInternal = objectTranslator.GetMethodSignatureInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return methodSignatureInternal;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D298 File Offset: 0x0000B498
		private int GetMethodSignatureInternal(Lua luaState)
		{
			int num = luaState.CheckUObject(1, "luaNet_class");
			ProxyType proxyType;
			object obj;
			if (num != -1)
			{
				proxyType = (ProxyType)this._objects[num];
				obj = null;
			}
			else
			{
				obj = this.GetRawNetObject(luaState, 1);
				if (obj == null)
				{
					this.ThrowError(luaState, "get_method_bysig: first arg is not type or object reference");
					return 1;
				}
				proxyType = new ProxyType(obj.GetType());
			}
			string name = luaState.ToString(2, false);
			Type[] array = new Type[luaState.GetTop() - 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FindType(luaState.ToString(i + 3, false));
			}
			try
			{
				MethodInfo method = proxyType.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, array);
				LuaFunction invokeFunction = new LuaMethodWrapper(this, obj, proxyType, method).InvokeFunction;
				this.PushFunction(luaState, invokeFunction);
			}
			catch (Exception e)
			{
				this.ThrowError(luaState, e);
			}
			return 1;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000D37C File Offset: 0x0000B57C
		private static int GetConstructorSignature(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int constructorSignatureInternal = objectTranslator.GetConstructorSignatureInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return constructorSignatureInternal;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		private int GetConstructorSignatureInternal(Lua luaState)
		{
			ProxyType proxyType = null;
			int num = luaState.CheckUObject(1, "luaNet_class");
			if (num != -1)
			{
				proxyType = (ProxyType)this._objects[num];
			}
			if (proxyType == null)
			{
				this.ThrowError(luaState, "get_constructor_bysig: first arg is invalid type reference");
				return 1;
			}
			Type[] array = new Type[luaState.GetTop() - 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.FindType(luaState.ToString(i + 2, false));
			}
			try
			{
				ConstructorInfo constructor = proxyType.UnderlyingSystemType.GetConstructor(array);
				LuaFunction invokeFunction = new LuaMethodWrapper(this, null, proxyType, constructor).InvokeFunction;
				this.PushFunction(luaState, invokeFunction);
			}
			catch (Exception e)
			{
				this.ThrowError(luaState, e);
			}
			return 1;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000D478 File Offset: 0x0000B678
		internal void PushType(Lua luaState, Type t)
		{
			this.PushObject(luaState, new ProxyType(t), "luaNet_class");
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000D48C File Offset: 0x0000B68C
		internal void PushFunction(Lua luaState, LuaFunction func)
		{
			this.PushObject(luaState, func, "luaNet_function");
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000D49C File Offset: 0x0000B69C
		internal void PushObject(Lua luaState, object o, string metatable)
		{
			int num = -1;
			if (o == null)
			{
				luaState.PushNil();
				return;
			}
			if ((!o.GetType().IsValueType || o.GetType().IsEnum) && this._objectsBackMap.TryGetValue(o, out num))
			{
				luaState.GetMetaTable("luaNet_objects");
				luaState.RawGetInteger(-1, (long)num);
				if (luaState.Type(-1) != LuaType.Nil)
				{
					luaState.Remove(-2);
					return;
				}
				luaState.Remove(-1);
				luaState.Remove(-1);
				this.CollectObject(o, num);
			}
			num = this.AddObject(o);
			this.PushNewObject(luaState, o, num, metatable);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000D534 File Offset: 0x0000B734
		private void PushNewObject(Lua luaState, object o, int index, string metatable)
		{
			if (metatable == "luaNet_metatable")
			{
				luaState.GetMetaTable(o.GetType().AssemblyQualifiedName);
				if (luaState.IsNil(-1))
				{
					luaState.SetTop(-2);
					luaState.NewMetaTable(o.GetType().AssemblyQualifiedName);
					luaState.PushString("cache");
					luaState.NewTable();
					luaState.RawSet(-3);
					luaState.PushLightUserData(this._tagPtr);
					luaState.PushNumber(1.0);
					luaState.RawSet(-3);
					luaState.PushString("__index");
					luaState.PushString("luaNet_indexfunction");
					luaState.RawGet(LuaRegistry.Index);
					luaState.RawSet(-3);
					luaState.PushString("__gc");
					luaState.PushCFunction(MetaFunctions.GcFunction);
					luaState.RawSet(-3);
					luaState.PushString("__tostring");
					luaState.PushCFunction(MetaFunctions.ToStringFunction);
					luaState.RawSet(-3);
					luaState.PushString("__newindex");
					luaState.PushCFunction(MetaFunctions.NewIndexFunction);
					luaState.RawSet(-3);
					this.RegisterOperatorsFunctions(luaState, o.GetType());
					this.RegisterCallMethodForDelegate(luaState, o);
				}
			}
			else
			{
				luaState.GetMetaTable(metatable);
			}
			luaState.GetMetaTable("luaNet_objects");
			luaState.NewUData(index);
			luaState.PushCopy(-3);
			luaState.Remove(-4);
			luaState.SetMetaTable(-2);
			luaState.PushCopy(-1);
			luaState.RawSetInteger(-3, (long)index);
			luaState.Remove(-2);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000D6AE File Offset: 0x0000B8AE
		private void RegisterCallMethodForDelegate(Lua luaState, object o)
		{
			if (!(o is Delegate))
			{
				return;
			}
			luaState.PushString("__call");
			luaState.PushCFunction(MetaFunctions.CallDelegateFunction);
			luaState.RawSet(-3);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000D6D8 File Offset: 0x0000B8D8
		private void RegisterOperatorsFunctions(Lua luaState, Type type)
		{
			if (type.HasAdditionOperator())
			{
				luaState.PushString("__add");
				luaState.PushCFunction(MetaFunctions.AddFunction);
				luaState.RawSet(-3);
			}
			if (type.HasSubtractionOperator())
			{
				luaState.PushString("__sub");
				luaState.PushCFunction(MetaFunctions.SubtractFunction);
				luaState.RawSet(-3);
			}
			if (type.HasMultiplyOperator())
			{
				luaState.PushString("__mul");
				luaState.PushCFunction(MetaFunctions.MultiplyFunction);
				luaState.RawSet(-3);
			}
			if (type.HasDivisionOperator())
			{
				luaState.PushString("__div");
				luaState.PushCFunction(MetaFunctions.DivisionFunction);
				luaState.RawSet(-3);
			}
			if (type.HasModulusOperator())
			{
				luaState.PushString("__mod");
				luaState.PushCFunction(MetaFunctions.ModulosFunction);
				luaState.RawSet(-3);
			}
			if (type.HasUnaryNegationOperator())
			{
				luaState.PushString("__unm");
				luaState.PushCFunction(MetaFunctions.UnaryNegationFunction);
				luaState.RawSet(-3);
			}
			if (type.HasEqualityOperator())
			{
				luaState.PushString("__eq");
				luaState.PushCFunction(MetaFunctions.EqualFunction);
				luaState.RawSet(-3);
			}
			if (type.HasLessThanOperator())
			{
				luaState.PushString("__lt");
				luaState.PushCFunction(MetaFunctions.LessThanFunction);
				luaState.RawSet(-3);
			}
			if (type.HasLessThanOrEqualOperator())
			{
				luaState.PushString("__le");
				luaState.PushCFunction(MetaFunctions.LessThanOrEqualFunction);
				luaState.RawSet(-3);
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000D83C File Offset: 0x0000BA3C
		internal object GetAsType(Lua luaState, int stackPos, Type paramType)
		{
			ExtractValue extractValue = this.typeChecker.CheckLuaType(luaState, stackPos, paramType);
			if (extractValue == null)
			{
				return null;
			}
			return extractValue(luaState, stackPos);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000D868 File Offset: 0x0000BA68
		internal void CollectObject(int udata)
		{
			object o;
			if (this._objects.TryGetValue(udata, out o))
			{
				this.CollectObject(o, udata);
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000D88D File Offset: 0x0000BA8D
		private void CollectObject(object o, int udata)
		{
			this._objects.Remove(udata);
			if (!o.GetType().IsValueType || o.GetType().IsEnum)
			{
				this._objectsBackMap.Remove(o);
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000D8C4 File Offset: 0x0000BAC4
		private int AddObject(object obj)
		{
			int nextObj = this._nextObj;
			this._nextObj = nextObj + 1;
			int num = nextObj;
			this._objects[num] = obj;
			if (!obj.GetType().IsValueType || obj.GetType().IsEnum)
			{
				this._objectsBackMap[obj] = num;
			}
			return num;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000D918 File Offset: 0x0000BB18
		internal object GetObject(Lua luaState, int index)
		{
			switch (luaState.Type(index))
			{
			case LuaType.Boolean:
				return luaState.ToBoolean(index);
			case LuaType.Number:
				if (luaState.IsInteger(index))
				{
					return luaState.ToInteger(index);
				}
				return luaState.ToNumber(index);
			case LuaType.String:
				return luaState.ToString(index, false);
			case LuaType.Table:
				return this.GetTable(luaState, index);
			case LuaType.Function:
				return this.GetFunction(luaState, index);
			case LuaType.UserData:
			{
				int num = luaState.ToNetObject(index, this.Tag);
				if (num == -1)
				{
					return this.GetUserData(luaState, index);
				}
				return this._objects[num];
			}
			case LuaType.Thread:
				return this.GetThread(luaState, index);
			}
			return null;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		internal LuaTable GetTable(Lua luaState, int index)
		{
			this.CleanFinalizedReferences(luaState);
			luaState.PushCopy(index);
			int num = luaState.Ref(LuaRegistry.Index);
			if (num == -1)
			{
				return null;
			}
			return new LuaTable(num, this.interpreter);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000DA10 File Offset: 0x0000BC10
		internal LuaThread GetThread(Lua luaState, int index)
		{
			this.CleanFinalizedReferences(luaState);
			luaState.PushCopy(index);
			int num = luaState.Ref(LuaRegistry.Index);
			if (num == -1)
			{
				return null;
			}
			return new LuaThread(num, this.interpreter);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000DA4C File Offset: 0x0000BC4C
		internal LuaUserData GetUserData(Lua luaState, int index)
		{
			this.CleanFinalizedReferences(luaState);
			luaState.PushCopy(index);
			int num = luaState.Ref(LuaRegistry.Index);
			if (num == -1)
			{
				return null;
			}
			return new LuaUserData(num, this.interpreter);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000DA88 File Offset: 0x0000BC88
		internal LuaFunction GetFunction(Lua luaState, int index)
		{
			this.CleanFinalizedReferences(luaState);
			luaState.PushCopy(index);
			int num = luaState.Ref(LuaRegistry.Index);
			if (num == -1)
			{
				return null;
			}
			return new LuaFunction(num, this.interpreter);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
		internal object GetNetObject(Lua luaState, int index)
		{
			int num = luaState.ToNetObject(index, this.Tag);
			if (num == -1)
			{
				return null;
			}
			return this._objects[num];
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		internal object GetRawNetObject(Lua luaState, int index)
		{
			int num = luaState.RawNetObj(index);
			if (num == -1)
			{
				return null;
			}
			return this._objects[num];
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		internal object[] PopValues(Lua luaState, int oldTop)
		{
			int top = luaState.GetTop();
			if (oldTop == top)
			{
				return new object[0];
			}
			List<object> list = new List<object>();
			for (int i = oldTop + 1; i <= top; i++)
			{
				list.Add(this.GetObject(luaState, i));
			}
			luaState.SetTop(oldTop);
			return list.ToArray();
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000DB6C File Offset: 0x0000BD6C
		internal object[] PopValues(Lua luaState, int oldTop, Type[] popTypes)
		{
			int top = luaState.GetTop();
			if (oldTop == top)
			{
				return new object[0];
			}
			List<object> list = new List<object>();
			int num;
			if (popTypes[0] == typeof(void))
			{
				num = 1;
			}
			else
			{
				num = 0;
			}
			for (int i = oldTop + 1; i <= top; i++)
			{
				list.Add(this.GetAsType(luaState, i, popTypes[num]));
				num++;
			}
			luaState.SetTop(oldTop);
			return list.ToArray();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000DBDB File Offset: 0x0000BDDB
		private static bool IsILua(object o)
		{
			return o is ILuaGeneratedType && o.GetType().GetInterface("ILuaGeneratedType", true) != null;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000DC00 File Offset: 0x0000BE00
		internal void Push(Lua luaState, object o)
		{
			if (o == null)
			{
				luaState.PushNil();
				return;
			}
			if (o is sbyte)
			{
				sbyte b = (sbyte)o;
				luaState.PushInteger((long)b);
				return;
			}
			if (o is byte)
			{
				byte b2 = (byte)o;
				luaState.PushInteger((long)((ulong)b2));
				return;
			}
			if (o is short)
			{
				short num = (short)o;
				luaState.PushInteger((long)num);
				return;
			}
			if (o is ushort)
			{
				ushort num2 = (ushort)o;
				luaState.PushInteger((long)((ulong)num2));
				return;
			}
			if (o is int)
			{
				int num3 = (int)o;
				luaState.PushInteger((long)num3);
				return;
			}
			if (o is uint)
			{
				uint num4 = (uint)o;
				luaState.PushInteger((long)((ulong)num4));
				return;
			}
			if (o is long)
			{
				long n = (long)o;
				luaState.PushInteger(n);
				return;
			}
			if (o is ulong)
			{
				ulong n2 = (ulong)o;
				luaState.PushInteger((long)n2);
				return;
			}
			if (o is char)
			{
				char c = (char)o;
				luaState.PushInteger((long)((ulong)c));
				return;
			}
			if (o is float)
			{
				float num5 = (float)o;
				luaState.PushNumber((double)num5);
				return;
			}
			if (o is decimal)
			{
				decimal value = (decimal)o;
				luaState.PushNumber((double)value);
				return;
			}
			if (o is double)
			{
				double number = (double)o;
				luaState.PushNumber(number);
				return;
			}
			string text = o as string;
			if (text != null)
			{
				luaState.PushString(text);
				return;
			}
			if (o is bool)
			{
				bool b3 = (bool)o;
				luaState.PushBoolean(b3);
				return;
			}
			if (ObjectTranslator.IsILua(o))
			{
				((ILuaGeneratedType)o).LuaInterfaceGetLuaTable().Push(luaState);
				return;
			}
			LuaTable luaTable = o as LuaTable;
			if (luaTable != null)
			{
				luaTable.Push(luaState);
				return;
			}
			LuaThread luaThread = o as LuaThread;
			if (luaThread != null)
			{
				luaThread.Push(luaState);
				return;
			}
			LuaFunction luaFunction = o as LuaFunction;
			if (luaFunction != null)
			{
				this.PushFunction(luaState, luaFunction);
				return;
			}
			LuaFunction luaFunction2 = o as LuaFunction;
			if (luaFunction2 != null)
			{
				luaFunction2.Push(luaState);
				return;
			}
			LuaUserData luaUserData = o as LuaUserData;
			if (luaUserData != null)
			{
				luaUserData.Push(luaState);
				return;
			}
			this.PushObject(luaState, o, "luaNet_metatable");
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000DE08 File Offset: 0x0000C008
		public int PushMultiple(Lua luaState, object o)
		{
			this.Push(luaState, o);
			return 1;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000DE13 File Offset: 0x0000C013
		internal bool MatchParameters(Lua luaState, MethodBase method, MethodCache methodCache, int skipParam)
		{
			return this.metaFunctions.MatchParameters(luaState, method, methodCache, skipParam);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000DE25 File Offset: 0x0000C025
		internal Array CreateParamsArray(Lua luaState, ExtractValue extractValue, Type paramArrayType, int startIndex, int count)
		{
			return MetaFunctions.CreateParamsArray(luaState, extractValue, paramArrayType, startIndex, count);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000DE34 File Offset: 0x0000C034
		private Type TypeOf(Lua luaState, int idx)
		{
			int num = luaState.CheckUObject(idx, "luaNet_class");
			if (num == -1)
			{
				return null;
			}
			return ((ProxyType)this._objects[num]).UnderlyingSystemType;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000DE6A File Offset: 0x0000C06A
		private static int PushError(Lua luaState, string msg)
		{
			luaState.PushNil();
			luaState.PushString(msg);
			return 2;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000DE7C File Offset: 0x0000C07C
		private static int CType(IntPtr luaState)
		{
			Lua luaState2 = Lua.FromIntPtr(luaState);
			return ObjectTranslatorPool.Instance.Find(luaState2).CTypeInternal(luaState2);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000DEA4 File Offset: 0x0000C0A4
		private int CTypeInternal(Lua luaState)
		{
			Type type = this.TypeOf(luaState, 1);
			if (type == null)
			{
				return ObjectTranslator.PushError(luaState, "Not a CLR Class");
			}
			this.PushObject(luaState, type, "luaNet_metatable");
			return 1;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000DEE0 File Offset: 0x0000C0E0
		private static int EnumFromInt(IntPtr luaState)
		{
			Lua luaState2 = Lua.FromIntPtr(luaState);
			return ObjectTranslatorPool.Instance.Find(luaState2).EnumFromIntInternal(luaState2);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000DF08 File Offset: 0x0000C108
		private int EnumFromIntInternal(Lua luaState)
		{
			Type type = this.TypeOf(luaState, 1);
			if (type == null || !type.IsEnum)
			{
				return ObjectTranslator.PushError(luaState, "Not an Enum.");
			}
			object o = null;
			LuaType luaType = luaState.Type(2);
			if (luaType == LuaType.Number)
			{
				int value = (int)luaState.ToNumber(2);
				o = Enum.ToObject(type, value);
			}
			else
			{
				if (luaType != LuaType.String)
				{
					return ObjectTranslator.PushError(luaState, "Second argument must be a integer or a string.");
				}
				string value2 = luaState.ToString(2, false);
				string text = null;
				try
				{
					o = Enum.Parse(type, value2, true);
				}
				catch (ArgumentException ex)
				{
					text = ex.Message;
				}
				if (text != null)
				{
					return ObjectTranslator.PushError(luaState, text);
				}
			}
			this.PushObject(luaState, o, "luaNet_metatable");
			return 1;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000DFBC File Offset: 0x0000C1BC
		internal void AddFinalizedReference(int reference)
		{
			this.finalizedReferences.Enqueue(reference);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		private void CleanFinalizedReferences(Lua state)
		{
			if (this.finalizedReferences.Count == 0)
			{
				return;
			}
			int reference;
			while (this.finalizedReferences.TryDequeue(out reference))
			{
				state.Unref(LuaRegistry.Index, reference);
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000E004 File Offset: 0x0000C204
		// Note: this type is marked as 'beforefieldinit'.
		static ObjectTranslator()
		{
			ObjectTranslator._registerTableFunction = new LuaFunction(ObjectTranslator.RegisterTable);
			ObjectTranslator._unregisterTableFunction = new LuaFunction(ObjectTranslator.UnregisterTable);
			ObjectTranslator._getMethodSigFunction = new LuaFunction(ObjectTranslator.GetMethodSignature);
			ObjectTranslator._getConstructorSigFunction = new LuaFunction(ObjectTranslator.GetConstructorSignature);
			ObjectTranslator._importTypeFunction = new LuaFunction(ObjectTranslator.ImportType);
			ObjectTranslator._loadAssemblyFunction = new LuaFunction(ObjectTranslator.LoadAssembly);
			ObjectTranslator._ctypeFunction = new LuaFunction(ObjectTranslator.CType);
			ObjectTranslator._enumFromIntFunction = new LuaFunction(ObjectTranslator.EnumFromInt);
		}

		// Token: 0x0400014D RID: 333
		private static readonly LuaFunction _registerTableFunction;

		// Token: 0x0400014E RID: 334
		private static readonly LuaFunction _unregisterTableFunction;

		// Token: 0x0400014F RID: 335
		private static readonly LuaFunction _getMethodSigFunction;

		// Token: 0x04000150 RID: 336
		private static readonly LuaFunction _getConstructorSigFunction;

		// Token: 0x04000151 RID: 337
		private static readonly LuaFunction _importTypeFunction;

		// Token: 0x04000152 RID: 338
		private static readonly LuaFunction _loadAssemblyFunction;

		// Token: 0x04000153 RID: 339
		private static readonly LuaFunction _ctypeFunction;

		// Token: 0x04000154 RID: 340
		private static readonly LuaFunction _enumFromIntFunction;

		// Token: 0x04000155 RID: 341
		private readonly Dictionary<object, int> _objectsBackMap;

		// Token: 0x04000156 RID: 342
		private readonly Dictionary<int, object> _objects;

		// Token: 0x04000157 RID: 343
		private readonly ConcurrentQueue<int> finalizedReferences;

		// Token: 0x04000158 RID: 344
		internal EventHandlerContainer PendingEvents;

		// Token: 0x04000159 RID: 345
		private MetaFunctions metaFunctions;

		// Token: 0x0400015A RID: 346
		private List<Assembly> assemblies;

		// Token: 0x0400015B RID: 347
		internal CheckType typeChecker;

		// Token: 0x0400015C RID: 348
		internal Lua interpreter;

		// Token: 0x0400015D RID: 349
		private int _nextObj;

		// Token: 0x0400015E RID: 350
		private readonly IntPtr _tagPtr;

		// Token: 0x0200007A RID: 122
		private class ReferenceComparer : IEqualityComparer<object>
		{
			// Token: 0x06000387 RID: 903 RVA: 0x0000E09C File Offset: 0x0000C29C
			public bool Equals(object x, object y)
			{
				if (x != null && y != null && x.GetType() == y.GetType() && x.GetType().IsValueType && y.GetType().IsValueType)
				{
					return x.Equals(y);
				}
				return x == y;
			}

			// Token: 0x06000388 RID: 904 RVA: 0x0000E0E8 File Offset: 0x0000C2E8
			public int GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
