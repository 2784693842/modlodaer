using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KeraLua;
using NLua.Exceptions;
using NLua.Extensions;
using NLua.Method;

namespace NLua
{
	// Token: 0x0200007D RID: 125
	internal class MetaFunctions
	{
		// Token: 0x06000393 RID: 915 RVA: 0x0000E222 File Offset: 0x0000C422
		public MetaFunctions(ObjectTranslator translator)
		{
			this._memberCache = new Dictionary<object, Dictionary<object, object>>();
			base..ctor();
			this._translator = translator;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000E23C File Offset: 0x0000C43C
		private static int RunFunctionDelegate(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			LuaFunction luaFunction = (LuaFunction)objectTranslator.GetRawNetObject(lua, 1);
			if (luaFunction == null)
			{
				return lua.Error();
			}
			lua.Remove(1);
			int result = luaFunction(luaState);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000E29C File Offset: 0x0000C49C
		private static int CollectObject(IntPtr state)
		{
			Lua luaState = Lua.FromIntPtr(state);
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(luaState);
			return MetaFunctions.CollectObject(luaState, translator);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
		private static int CollectObject(Lua luaState, ObjectTranslator translator)
		{
			int num = luaState.RawNetObj(1);
			if (num != -1)
			{
				translator.CollectObject(num);
			}
			return 0;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
		private static int ToStringLua(IntPtr state)
		{
			Lua luaState = Lua.FromIntPtr(state);
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(luaState);
			return MetaFunctions.ToStringLua(luaState, translator);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000E310 File Offset: 0x0000C510
		private static int ToStringLua(Lua luaState, ObjectTranslator translator)
		{
			object rawNetObject = translator.GetRawNetObject(luaState, 1);
			if (rawNetObject != null)
			{
				translator.Push(luaState, ((rawNetObject != null) ? rawNetObject.ToString() : null) + ": " + rawNetObject.GetHashCode().ToString());
			}
			else
			{
				luaState.PushNil();
			}
			return 1;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000E360 File Offset: 0x0000C560
		private static int AddLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_Addition", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		private static int SubtractLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_Subtraction", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		private static int MultiplyLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_Multiply", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000E42C File Offset: 0x0000C62C
		private static int DivideLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_Division", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000E470 File Offset: 0x0000C670
		private static int ModLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_Modulus", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		private static int UnaryNegationLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.UnaryNegationLua(lua, objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		private static int UnaryNegationLua(Lua luaState, ObjectTranslator translator)
		{
			object obj = translator.GetRawNetObject(luaState, 1);
			if (obj == null)
			{
				translator.ThrowError(luaState, "Cannot negate a nil object");
				return 1;
			}
			Type type = obj.GetType();
			MethodInfo method = type.GetMethod("op_UnaryNegation");
			if (method == null)
			{
				translator.ThrowError(luaState, "Cannot negate object (" + type.Name + " does not overload the operator -)");
				return 1;
			}
			obj = method.Invoke(obj, new object[]
			{
				obj
			});
			translator.Push(luaState, obj);
			return 1;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000E570 File Offset: 0x0000C770
		private static int EqualLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_Equality", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		private static int LessThanLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_LessThan", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000E5F8 File Offset: 0x0000C7F8
		private static int LessThanOrEqualLua(IntPtr luaState)
		{
			Lua lua = Lua.FromIntPtr(luaState);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = MetaFunctions.MatchOperator(lua, "op_LessThanOrEqual", objectTranslator);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000E63C File Offset: 0x0000C83C
		public static void DumpStack(ObjectTranslator translator, Lua luaState)
		{
			int top = luaState.GetTop();
			for (int i = 1; i <= top; i++)
			{
				LuaType luaType = luaState.Type(i);
				if (luaType != LuaType.Table)
				{
					luaState.TypeName(luaType);
				}
				luaState.ToString(i, false);
				if (luaType == LuaType.UserData)
				{
					object rawNetObject = translator.GetRawNetObject(luaState, i);
					if (rawNetObject != null)
					{
						rawNetObject.ToString();
					}
				}
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000E690 File Offset: 0x0000C890
		private static int GetMethod(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int methodInternal = objectTranslator.MetaFunctionsInstance.GetMethodInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return methodInternal;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000E6D4 File Offset: 0x0000C8D4
		private int GetMethodInternal(Lua luaState)
		{
			object rawNetObject = this._translator.GetRawNetObject(luaState, 1);
			if (rawNetObject == null)
			{
				this._translator.ThrowError(luaState, "Trying to index an invalid object reference");
				return 1;
			}
			object @object = this._translator.GetObject(luaState, 2);
			string text = @object as string;
			Type type = rawNetObject.GetType();
			ProxyType objType = new ProxyType(type);
			if (!string.IsNullOrEmpty(text) && this.IsMemberPresent(objType, text))
			{
				return this.GetMember(luaState, objType, rawNetObject, text, BindingFlags.Instance);
			}
			if (this.TryAccessByArray(luaState, type, rawNetObject, @object))
			{
				return 1;
			}
			int methodFallback = this.GetMethodFallback(luaState, type, rawNetObject, text);
			if (methodFallback != 0)
			{
				return methodFallback;
			}
			if (!string.IsNullOrEmpty(text) || @object != null)
			{
				if (string.IsNullOrEmpty(text))
				{
					text = @object.ToString();
				}
				return this.PushInvalidMethodCall(luaState, type, text);
			}
			luaState.PushBoolean(false);
			return 2;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000E796 File Offset: 0x0000C996
		private int PushInvalidMethodCall(Lua luaState, Type type, string name)
		{
			this.SetMemberCache(type, name, null);
			this._translator.Push(luaState, null);
			this._translator.Push(luaState, false);
			return 2;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		private bool TryAccessByArray(Lua luaState, Type objType, object obj, object index)
		{
			if (!objType.IsArray)
			{
				return false;
			}
			int num = -1;
			if (index is long)
			{
				long num2 = (long)index;
				num = (int)num2;
			}
			else if (index is double)
			{
				double num3 = (double)index;
				num = (int)num3;
			}
			if (num == -1)
			{
				return false;
			}
			Type underlyingSystemType = objType.UnderlyingSystemType;
			if (underlyingSystemType == typeof(long[]))
			{
				long[] array = (long[])obj;
				this._translator.Push(luaState, array[num]);
				return true;
			}
			if (underlyingSystemType == typeof(float[]))
			{
				float[] array2 = (float[])obj;
				this._translator.Push(luaState, array2[num]);
				return true;
			}
			if (underlyingSystemType == typeof(double[]))
			{
				double[] array3 = (double[])obj;
				this._translator.Push(luaState, array3[num]);
				return true;
			}
			if (underlyingSystemType == typeof(int[]))
			{
				int[] array4 = (int[])obj;
				this._translator.Push(luaState, array4[num]);
				return true;
			}
			if (underlyingSystemType == typeof(byte[]))
			{
				byte[] array5 = (byte[])obj;
				this._translator.Push(luaState, array5[num]);
				return true;
			}
			if (underlyingSystemType == typeof(short[]))
			{
				short[] array6 = (short[])obj;
				this._translator.Push(luaState, array6[num]);
				return true;
			}
			if (underlyingSystemType == typeof(ushort[]))
			{
				ushort[] array7 = (ushort[])obj;
				this._translator.Push(luaState, array7[num]);
				return true;
			}
			if (underlyingSystemType == typeof(ulong[]))
			{
				ulong[] array8 = (ulong[])obj;
				this._translator.Push(luaState, array8[num]);
				return true;
			}
			if (underlyingSystemType == typeof(uint[]))
			{
				uint[] array9 = (uint[])obj;
				this._translator.Push(luaState, array9[num]);
				return true;
			}
			if (underlyingSystemType == typeof(sbyte[]))
			{
				sbyte[] array10 = (sbyte[])obj;
				this._translator.Push(luaState, array10[num]);
				return true;
			}
			object value = ((Array)obj).GetValue(num);
			this._translator.Push(luaState, value);
			return true;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000EA1C File Offset: 0x0000CC1C
		private int GetMethodFallback(Lua luaState, Type objType, object obj, string methodName)
		{
			object method;
			if (!string.IsNullOrEmpty(methodName) && this.TryGetExtensionMethod(objType, methodName, out method))
			{
				return this.PushExtensionMethod(luaState, objType, obj, methodName, method);
			}
			MethodInfo[] methods = objType.GetMethods();
			int num = this.TryIndexMethods(luaState, methods, obj);
			if (num != 0)
			{
				return num;
			}
			methods = objType.GetRuntimeMethods().ToArray<MethodInfo>();
			num = this.TryIndexMethods(luaState, methods, obj);
			if (num != 0)
			{
				return num;
			}
			num = this.TryGetValueForKeyMethods(luaState, methods, obj);
			if (num != 0)
			{
				return num;
			}
			MethodInfo methodInfo = objType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault((MethodInfo m) => m.Name == methodName && m.IsPrivate && m.IsVirtual && m.IsFinal);
			if (methodInfo != null)
			{
				ProxyType proxyType = new ProxyType(objType);
				LuaFunction luaFunction = new LuaFunction(new LuaMethodWrapper(this._translator, obj, proxyType, methodInfo).InvokeFunction.Invoke);
				this.SetMemberCache(proxyType, methodName, luaFunction);
				this._translator.PushFunction(luaState, luaFunction);
				this._translator.Push(luaState, true);
				return 2;
			}
			return 0;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000EB28 File Offset: 0x0000CD28
		private int TryGetValueForKeyMethods(Lua luaState, MethodInfo[] methods, object obj)
		{
			foreach (MethodInfo methodInfo in methods)
			{
				if (!(methodInfo.Name != "TryGetValueForKey") && methodInfo.GetParameters().Length == 2)
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					object asType = this._translator.GetAsType(luaState, 2, parameters[0].ParameterType);
					if (asType == null)
					{
						break;
					}
					object[] array = new object[2];
					array[0] = asType;
					try
					{
						if (!(bool)methodInfo.Invoke(obj, array))
						{
							this._translator.ThrowError(luaState, "key not found: " + ((asType != null) ? asType.ToString() : null));
							return 1;
						}
						this._translator.Push(luaState, array[1]);
						return 1;
					}
					catch (TargetInvocationException ex)
					{
						if (ex.InnerException is KeyNotFoundException)
						{
							this._translator.ThrowError(luaState, "key '" + ((asType != null) ? asType.ToString() : null) + "' not found ");
						}
						else
						{
							this._translator.ThrowError(luaState, "exception indexing '" + ((asType != null) ? asType.ToString() : null) + "' " + ex.Message);
						}
						return 1;
					}
				}
			}
			return 0;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		private int TryIndexMethods(Lua luaState, MethodInfo[] methods, object obj)
		{
			foreach (MethodInfo methodInfo in methods)
			{
				if (!(methodInfo.Name != "get_Item") && methodInfo.GetParameters().Length == 1)
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					object asType = this._translator.GetAsType(luaState, 2, parameters[0].ParameterType);
					if (asType != null)
					{
						object[] parameters2 = new object[]
						{
							asType
						};
						try
						{
							object o = methodInfo.Invoke(obj, parameters2);
							this._translator.Push(luaState, o);
							return 1;
						}
						catch (TargetInvocationException ex)
						{
							if (ex.InnerException is KeyNotFoundException)
							{
								this._translator.ThrowError(luaState, "key '" + ((asType != null) ? asType.ToString() : null) + "' not found ");
							}
							else
							{
								this._translator.ThrowError(luaState, "exception indexing '" + ((asType != null) ? asType.ToString() : null) + "' " + ex.Message);
							}
							return 1;
						}
					}
				}
			}
			return 0;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000ED9C File Offset: 0x0000CF9C
		private static int GetBaseMethod(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int baseMethodInternal = objectTranslator.MetaFunctionsInstance.GetBaseMethodInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return baseMethodInternal;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
		private int GetBaseMethodInternal(Lua luaState)
		{
			object rawNetObject = this._translator.GetRawNetObject(luaState, 1);
			if (rawNetObject == null)
			{
				this._translator.ThrowError(luaState, "Trying to index an invalid object reference");
				return 1;
			}
			string text = luaState.ToString(2, false);
			if (string.IsNullOrEmpty(text))
			{
				luaState.PushNil();
				luaState.PushBoolean(false);
				return 2;
			}
			this.GetMember(luaState, new ProxyType(rawNetObject.GetType()), rawNetObject, "__luaInterface_base_" + text, BindingFlags.Instance);
			luaState.SetTop(-2);
			if (luaState.Type(-1) == LuaType.Nil)
			{
				luaState.SetTop(-2);
				return this.GetMember(luaState, new ProxyType(rawNetObject.GetType()), rawNetObject, text, BindingFlags.Instance);
			}
			luaState.PushBoolean(false);
			return 2;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000EE88 File Offset: 0x0000D088
		private bool IsMemberPresent(ProxyType objType, string methodName)
		{
			return this.CheckMemberCache(objType, methodName) != null || objType.GetMember(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public).Length != 0;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000EEA4 File Offset: 0x0000D0A4
		private bool TryGetExtensionMethod(Type type, string name, out object method)
		{
			object obj = this.CheckMemberCache(type, name);
			if (obj != null)
			{
				method = obj;
				return true;
			}
			MethodInfo methodInfo;
			bool result = this._translator.TryGetExtensionMethod(type, name, out methodInfo);
			method = methodInfo;
			return result;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000EED4 File Offset: 0x0000D0D4
		private int PushExtensionMethod(Lua luaState, Type type, object obj, string name, object method)
		{
			LuaFunction luaFunction = method as LuaFunction;
			if (luaFunction != null)
			{
				this._translator.PushFunction(luaState, luaFunction);
				this._translator.Push(luaState, true);
				return 2;
			}
			MethodInfo method2 = (MethodInfo)method;
			LuaFunction luaFunction2 = new LuaFunction(new LuaMethodWrapper(this._translator, obj, new ProxyType(type), method2).InvokeFunction.Invoke);
			this.SetMemberCache(type, name, luaFunction2);
			this._translator.PushFunction(luaState, luaFunction2);
			this._translator.Push(luaState, true);
			return 2;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000EF64 File Offset: 0x0000D164
		private int GetMember(Lua luaState, ProxyType objType, object obj, string methodName, BindingFlags bindingType)
		{
			bool flag = false;
			MemberInfo memberInfo = null;
			object obj2 = this.CheckMemberCache(objType, methodName);
			if (obj2 is LuaFunction)
			{
				this._translator.PushFunction(luaState, (LuaFunction)obj2);
				this._translator.Push(luaState, true);
				return 2;
			}
			if (obj2 != null)
			{
				memberInfo = (MemberInfo)obj2;
			}
			else
			{
				MemberInfo[] member = objType.GetMember(methodName, bindingType | BindingFlags.Public);
				if (member.Length != 0)
				{
					memberInfo = member[0];
				}
				else
				{
					member = objType.GetMember(methodName, bindingType | BindingFlags.Static | BindingFlags.Public);
					if (member.Length != 0)
					{
						memberInfo = member[0];
						flag = true;
					}
				}
			}
			if (memberInfo != null)
			{
				if (memberInfo.MemberType == MemberTypes.Field)
				{
					FieldInfo fieldInfo = (FieldInfo)memberInfo;
					if (obj2 == null)
					{
						this.SetMemberCache(objType, methodName, memberInfo);
					}
					try
					{
						object value = fieldInfo.GetValue(obj);
						this._translator.Push(luaState, value);
						goto IL_2CD;
					}
					catch
					{
						luaState.PushNil();
						goto IL_2CD;
					}
				}
				if (memberInfo.MemberType == MemberTypes.Property)
				{
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					if (obj2 == null)
					{
						this.SetMemberCache(objType, methodName, memberInfo);
					}
					try
					{
						object value2 = propertyInfo.GetValue(obj, null);
						this._translator.Push(luaState, value2);
						goto IL_2CD;
					}
					catch (ArgumentException)
					{
						if (objType.UnderlyingSystemType != typeof(object))
						{
							return this.GetMember(luaState, new ProxyType(objType.UnderlyingSystemType.BaseType), obj, methodName, bindingType);
						}
						luaState.PushNil();
						goto IL_2CD;
					}
					catch (TargetInvocationException e)
					{
						this.ThrowError(luaState, e);
						luaState.PushNil();
						goto IL_2CD;
					}
				}
				if (memberInfo.MemberType == MemberTypes.Event)
				{
					EventInfo eventInfo = (EventInfo)memberInfo;
					if (obj2 == null)
					{
						this.SetMemberCache(objType, methodName, memberInfo);
					}
					this._translator.Push(luaState, new RegisterEventHandler(this._translator.PendingEvents, obj, eventInfo));
				}
				else
				{
					if (flag)
					{
						this._translator.ThrowError(luaState, "Can't pass instance to static method " + methodName);
						return 1;
					}
					if (memberInfo.MemberType != MemberTypes.NestedType || !(memberInfo.DeclaringType != null))
					{
						LuaFunction invokeFunction = new LuaMethodWrapper(this._translator, objType, methodName, bindingType).InvokeFunction;
						if (obj2 == null)
						{
							this.SetMemberCache(objType, methodName, invokeFunction);
						}
						this._translator.PushFunction(luaState, invokeFunction);
						this._translator.Push(luaState, true);
						return 2;
					}
					if (obj2 == null)
					{
						this.SetMemberCache(objType, methodName, memberInfo);
					}
					string name = memberInfo.Name;
					string className = memberInfo.DeclaringType.FullName + "+" + name;
					Type t = this._translator.FindType(className);
					this._translator.PushType(luaState, t);
				}
				IL_2CD:
				this._translator.Push(luaState, false);
				return 2;
			}
			if (objType.UnderlyingSystemType != typeof(object))
			{
				return this.GetMember(luaState, new ProxyType(objType.UnderlyingSystemType.BaseType), obj, methodName, bindingType);
			}
			this._translator.ThrowError(luaState, "Unknown member name " + methodName);
			return 1;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000F27C File Offset: 0x0000D47C
		private object CheckMemberCache(Type objType, string memberName)
		{
			return this.CheckMemberCache(new ProxyType(objType), memberName);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000F28C File Offset: 0x0000D48C
		private object CheckMemberCache(ProxyType objType, string memberName)
		{
			Dictionary<object, object> dictionary;
			if (!this._memberCache.TryGetValue(objType, out dictionary))
			{
				return null;
			}
			object result;
			if (dictionary == null || !dictionary.TryGetValue(memberName, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000F2BC File Offset: 0x0000D4BC
		private void SetMemberCache(Type objType, string memberName, object member)
		{
			this.SetMemberCache(new ProxyType(objType), memberName, member);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		private void SetMemberCache(ProxyType objType, string memberName, object member)
		{
			Dictionary<object, object> dictionary;
			Dictionary<object, object> dictionary2;
			if (this._memberCache.TryGetValue(objType, out dictionary))
			{
				dictionary2 = dictionary;
			}
			else
			{
				dictionary2 = new Dictionary<object, object>();
				this._memberCache[objType] = dictionary2;
			}
			dictionary2[memberName] = member;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000F308 File Offset: 0x0000D508
		private static int SetFieldOrProperty(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = objectTranslator.MetaFunctionsInstance.SetFieldOrPropertyInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000F34C File Offset: 0x0000D54C
		private int SetFieldOrPropertyInternal(Lua luaState)
		{
			object rawNetObject = this._translator.GetRawNetObject(luaState, 1);
			if (rawNetObject == null)
			{
				this._translator.ThrowError(luaState, "trying to index and invalid object reference");
				return 1;
			}
			Type type = rawNetObject.GetType();
			string e;
			if (this.TrySetMember(luaState, new ProxyType(type), rawNetObject, BindingFlags.Instance, out e))
			{
				return 0;
			}
			try
			{
				if (type.IsArray && luaState.IsNumber(2))
				{
					int index = (int)luaState.ToNumber(2);
					Array array = (Array)rawNetObject;
					object asType = this._translator.GetAsType(luaState, 3, array.GetType().GetElementType());
					array.SetValue(asType, index);
				}
				else
				{
					MethodInfo method = type.GetMethod("set_Item");
					if (!(method != null))
					{
						this._translator.ThrowError(luaState, e);
						return 1;
					}
					ParameterInfo[] parameters = method.GetParameters();
					Type parameterType = parameters[1].ParameterType;
					object asType2 = this._translator.GetAsType(luaState, 3, parameterType);
					Type parameterType2 = parameters[0].ParameterType;
					object asType3 = this._translator.GetAsType(luaState, 2, parameterType2);
					method.Invoke(rawNetObject, new object[]
					{
						asType3,
						asType2
					});
				}
			}
			catch (Exception e2)
			{
				this.ThrowError(luaState, e2);
				return 1;
			}
			return 0;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000F494 File Offset: 0x0000D694
		private bool TrySetMember(Lua luaState, ProxyType targetType, object target, BindingFlags bindingType, out string detailMessage)
		{
			detailMessage = null;
			if (luaState.Type(2) != LuaType.String)
			{
				detailMessage = "property names must be strings";
				return false;
			}
			string text = luaState.ToString(2, false);
			if (string.IsNullOrEmpty(text) || (!char.IsLetter(text[0]) && text[0] != '_'))
			{
				detailMessage = "Invalid property name";
				return false;
			}
			MemberInfo memberInfo = (MemberInfo)this.CheckMemberCache(targetType, text);
			if (memberInfo == null)
			{
				MemberInfo[] member = targetType.GetMember(text, bindingType | BindingFlags.Public);
				if (member.Length == 0)
				{
					detailMessage = "field or property '" + text + "' does not exist";
					return false;
				}
				memberInfo = member[0];
				this.SetMemberCache(targetType, text, memberInfo);
			}
			if (memberInfo.MemberType == MemberTypes.Field)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				object asType = this._translator.GetAsType(luaState, 3, fieldInfo.FieldType);
				try
				{
					fieldInfo.SetValue(target, asType);
				}
				catch (Exception ex)
				{
					detailMessage = "Error setting field: " + ex.Message;
					return false;
				}
				return true;
			}
			if (memberInfo.MemberType == MemberTypes.Property)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				object asType2 = this._translator.GetAsType(luaState, 3, propertyInfo.PropertyType);
				try
				{
					propertyInfo.SetValue(target, asType2, null);
				}
				catch (Exception ex2)
				{
					detailMessage = "Error setting property: " + ex2.Message;
					return false;
				}
				return true;
			}
			detailMessage = "'" + text + "' is not a .net field or property";
			return false;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000F60C File Offset: 0x0000D80C
		private int SetMember(Lua luaState, ProxyType targetType, object target, BindingFlags bindingType)
		{
			string e;
			if (!this.TrySetMember(luaState, targetType, target, bindingType, out e))
			{
				this._translator.ThrowError(luaState, e);
				return 1;
			}
			return 0;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000F638 File Offset: 0x0000D838
		private void ThrowError(Lua luaState, Exception e)
		{
			TargetInvocationException ex = e as TargetInvocationException;
			if (ex != null)
			{
				e = ex.InnerException;
			}
			this._translator.ThrowError(luaState, e);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000F664 File Offset: 0x0000D864
		private static int GetClassMethod(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int classMethodInternal = objectTranslator.MetaFunctionsInstance.GetClassMethodInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return classMethodInternal;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000F6A8 File Offset: 0x0000D8A8
		private int GetClassMethodInternal(Lua luaState)
		{
			ProxyType proxyType = this._translator.GetRawNetObject(luaState, 1) as ProxyType;
			if (proxyType == null)
			{
				this._translator.ThrowError(luaState, "Trying to index an invalid type reference");
				return 1;
			}
			if (luaState.IsNumber(2))
			{
				int length = (int)luaState.ToNumber(2);
				this._translator.Push(luaState, Array.CreateInstance(proxyType.UnderlyingSystemType, length));
				return 1;
			}
			string text = luaState.ToString(2, false);
			if (string.IsNullOrEmpty(text))
			{
				luaState.PushNil();
				return 1;
			}
			return this.GetMember(luaState, proxyType, null, text, BindingFlags.Static);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000F730 File Offset: 0x0000D930
		private static int SetClassFieldOrProperty(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = objectTranslator.MetaFunctionsInstance.SetClassFieldOrPropertyInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000F774 File Offset: 0x0000D974
		private int SetClassFieldOrPropertyInternal(Lua luaState)
		{
			ProxyType proxyType = this._translator.GetRawNetObject(luaState, 1) as ProxyType;
			if (proxyType == null)
			{
				this._translator.ThrowError(luaState, "trying to index an invalid type reference");
				return 1;
			}
			return this.SetMember(luaState, proxyType, null, BindingFlags.Static);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
		private static int CallDelegate(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = objectTranslator.MetaFunctionsInstance.CallDelegateInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
		private int CallDelegateInternal(Lua luaState)
		{
			Delegate @delegate = this._translator.GetRawNetObject(luaState, 1) as Delegate;
			if (@delegate == null)
			{
				this._translator.ThrowError(luaState, "Trying to invoke a not delegate or callable value");
				return 1;
			}
			luaState.Remove(1);
			MethodCache methodCache = new MethodCache();
			MethodBase method = @delegate.Method;
			if (this.MatchParameters(luaState, method, methodCache, 0))
			{
				try
				{
					object o;
					if (method.IsStatic)
					{
						o = method.Invoke(null, methodCache.args);
					}
					else
					{
						o = method.Invoke(@delegate.Target, methodCache.args);
					}
					this._translator.Push(luaState, o);
					return 1;
				}
				catch (TargetInvocationException ex)
				{
					if (this._translator.interpreter.UseTraceback)
					{
						ex.GetBaseException().Data["Traceback"] = this._translator.interpreter.GetDebugTraceback();
					}
					return this._translator.Interpreter.SetPendingException(ex.GetBaseException());
				}
				catch (Exception pendingException)
				{
					return this._translator.Interpreter.SetPendingException(pendingException);
				}
			}
			this._translator.ThrowError(luaState, "Cannot invoke delegate (invalid arguments for  " + method.Name + ")");
			return 1;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000F940 File Offset: 0x0000DB40
		private static int CallConstructor(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(lua);
			int result = objectTranslator.MetaFunctionsInstance.CallConstructorInternal(lua);
			if (objectTranslator.GetObject(lua, -1) is LuaScriptException)
			{
				return lua.Error();
			}
			return result;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000F984 File Offset: 0x0000DB84
		private static ConstructorInfo[] ReorderConstructors(ConstructorInfo[] constructors)
		{
			if (constructors.Length < 2)
			{
				return constructors;
			}
			return (from c in constructors
			group c by c.GetParameters().Length).SelectMany((IGrouping<int, ConstructorInfo> g) => from ci in g
			orderby ci.ToString() descending
			select ci).ToArray<ConstructorInfo>();
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000F9E8 File Offset: 0x0000DBE8
		private int CallConstructorInternal(Lua luaState)
		{
			ProxyType proxyType = this._translator.GetRawNetObject(luaState, 1) as ProxyType;
			if (proxyType == null)
			{
				this._translator.ThrowError(luaState, "Trying to call constructor on an invalid type reference");
				return 1;
			}
			MethodCache methodCache = new MethodCache();
			luaState.Remove(1);
			ConstructorInfo[] array = proxyType.UnderlyingSystemType.GetConstructors();
			array = MetaFunctions.ReorderConstructors(array);
			foreach (ConstructorInfo constructorInfo in array)
			{
				if (this.MatchParameters(luaState, constructorInfo, methodCache, 0))
				{
					try
					{
						this._translator.Push(luaState, constructorInfo.Invoke(methodCache.args));
					}
					catch (TargetInvocationException e)
					{
						this.ThrowError(luaState, e);
						return 1;
					}
					catch
					{
						luaState.PushNil();
					}
					return 1;
				}
			}
			if (proxyType.UnderlyingSystemType.IsValueType && luaState.GetTop() == 0)
			{
				this._translator.Push(luaState, Activator.CreateInstance(proxyType.UnderlyingSystemType));
				return 1;
			}
			string arg = (array.Length == 0) ? "unknown" : array[0].Name;
			this._translator.ThrowError(luaState, string.Format("{0} does not contain constructor({1}) argument match", proxyType.UnderlyingSystemType, arg));
			return 1;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000FB20 File Offset: 0x0000DD20
		private static bool IsInteger(double x)
		{
			return Math.Ceiling(x) == x;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000FB2C File Offset: 0x0000DD2C
		private static object GetTargetObject(Lua luaState, string operation, ObjectTranslator translator)
		{
			object rawNetObject = translator.GetRawNetObject(luaState, 1);
			if (rawNetObject != null && rawNetObject.GetType().HasMethod(operation))
			{
				return rawNetObject;
			}
			rawNetObject = translator.GetRawNetObject(luaState, 2);
			if (rawNetObject != null && rawNetObject.GetType().HasMethod(operation))
			{
				return rawNetObject;
			}
			return null;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000FB74 File Offset: 0x0000DD74
		private static int MatchOperator(Lua luaState, string operation, ObjectTranslator translator)
		{
			MethodCache methodCache = new MethodCache();
			object targetObject = MetaFunctions.GetTargetObject(luaState, operation, translator);
			if (targetObject == null)
			{
				translator.ThrowError(luaState, "Cannot call " + operation + " on a nil object");
				return 1;
			}
			Type type = targetObject.GetType();
			foreach (MethodInfo methodInfo in type.GetMethods(operation, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
			{
				if (translator.MatchParameters(luaState, methodInfo, methodCache, 0))
				{
					object o;
					if (methodInfo.IsStatic)
					{
						o = methodInfo.Invoke(null, methodCache.args);
					}
					else
					{
						o = methodInfo.Invoke(targetObject, methodCache.args);
					}
					translator.Push(luaState, o);
					return 1;
				}
			}
			translator.ThrowError(luaState, "Cannot call (" + operation + ") on object type " + type.Name);
			return 1;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000FC38 File Offset: 0x0000DE38
		internal static Array CreateParamsArray(Lua luaState, ExtractValue extractValue, Type paramArrayType, int startIndex, int count)
		{
			Array array = Array.CreateInstance(paramArrayType, count);
			for (int i = 0; i < count; i++)
			{
				object value = extractValue(luaState, startIndex);
				array.SetValue(value, i);
				startIndex++;
			}
			return array;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000FC74 File Offset: 0x0000DE74
		internal bool MatchParameters(Lua luaState, MethodBase method, MethodCache methodCache, int skipParam)
		{
			ParameterInfo[] parameters = method.GetParameters();
			int num = 1;
			int num2 = luaState.GetTop() - skipParam;
			List<object> list = new List<object>();
			List<int> list2 = new List<int>();
			List<MethodArgs> list3 = new List<MethodArgs>();
			foreach (ParameterInfo parameterInfo in parameters)
			{
				ExtractValue extractValue;
				if (!parameterInfo.IsIn && parameterInfo.IsOut)
				{
					list.Add(null);
					list2.Add(list.Count - 1);
				}
				else if (this.IsParamsArray(luaState, num2, num, parameterInfo, out extractValue))
				{
					int num3 = num2 - num + 1;
					Type elementType = parameterInfo.ParameterType.GetElementType();
					Array item = MetaFunctions.CreateParamsArray(luaState, extractValue, elementType, num, num3);
					num += num3;
					list.Add(item);
					int index = list.LastIndexOf(item);
					list3.Add(new MethodArgs
					{
						Index = index,
						ExtractValue = extractValue,
						IsParamsArray = true,
						ParameterType = elementType
					});
				}
				else if (num > num2)
				{
					if (!parameterInfo.IsOptional)
					{
						return false;
					}
					list.Add(parameterInfo.DefaultValue);
				}
				else if (this.IsTypeCorrect(luaState, num, parameterInfo, out extractValue))
				{
					object item2 = extractValue(luaState, num);
					list.Add(item2);
					int num4 = list.Count - 1;
					list3.Add(new MethodArgs
					{
						Index = num4,
						ExtractValue = extractValue,
						ParameterType = parameterInfo.ParameterType
					});
					if (parameterInfo.ParameterType.IsByRef)
					{
						list2.Add(num4);
					}
					num++;
				}
				else
				{
					if (!parameterInfo.IsOptional)
					{
						return false;
					}
					list.Add(parameterInfo.DefaultValue);
				}
			}
			if (num != num2 + 1)
			{
				return false;
			}
			methodCache.args = list.ToArray();
			methodCache.cachedMethod = method;
			methodCache.outList = list2.ToArray();
			methodCache.argTypes = list3.ToArray();
			return true;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000FE5D File Offset: 0x0000E05D
		private bool IsTypeCorrect(Lua luaState, int currentLuaParam, ParameterInfo currentNetParam, out ExtractValue extractValue)
		{
			extractValue = this._translator.typeChecker.CheckLuaType(luaState, currentLuaParam, currentNetParam.ParameterType);
			return extractValue != null;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000FE80 File Offset: 0x0000E080
		private bool IsParamsArray(Lua luaState, int nLuaParams, int currentLuaParam, ParameterInfo currentNetParam, out ExtractValue extractValue)
		{
			extractValue = null;
			if (!currentNetParam.GetCustomAttributes(typeof(ParamArrayAttribute), false).Any<object>())
			{
				return false;
			}
			bool result = nLuaParams < currentLuaParam;
			if (luaState.Type(currentLuaParam) == LuaType.Table)
			{
				extractValue = this._translator.typeChecker.GetExtractor(typeof(LuaTable));
				if (extractValue != null)
				{
					return true;
				}
			}
			else
			{
				Type elementType = currentNetParam.ParameterType.GetElementType();
				extractValue = this._translator.typeChecker.CheckLuaType(luaState, currentLuaParam, elementType);
				if (extractValue != null)
				{
					return true;
				}
			}
			return result;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000FF0C File Offset: 0x0000E10C
		// Note: this type is marked as 'beforefieldinit'.
		static MetaFunctions()
		{
			MetaFunctions.GcFunction = new LuaFunction(MetaFunctions.CollectObject);
			MetaFunctions.IndexFunction = new LuaFunction(MetaFunctions.GetMethod);
			MetaFunctions.NewIndexFunction = new LuaFunction(MetaFunctions.SetFieldOrProperty);
			MetaFunctions.BaseIndexFunction = new LuaFunction(MetaFunctions.GetBaseMethod);
			MetaFunctions.ClassIndexFunction = new LuaFunction(MetaFunctions.GetClassMethod);
			MetaFunctions.ClassNewIndexFunction = new LuaFunction(MetaFunctions.SetClassFieldOrProperty);
			MetaFunctions.ExecuteDelegateFunction = new LuaFunction(MetaFunctions.RunFunctionDelegate);
			MetaFunctions.CallConstructorFunction = new LuaFunction(MetaFunctions.CallConstructor);
			MetaFunctions.ToStringFunction = new LuaFunction(MetaFunctions.ToStringLua);
			MetaFunctions.CallDelegateFunction = new LuaFunction(MetaFunctions.CallDelegate);
			MetaFunctions.AddFunction = new LuaFunction(MetaFunctions.AddLua);
			MetaFunctions.SubtractFunction = new LuaFunction(MetaFunctions.SubtractLua);
			MetaFunctions.MultiplyFunction = new LuaFunction(MetaFunctions.MultiplyLua);
			MetaFunctions.DivisionFunction = new LuaFunction(MetaFunctions.DivideLua);
			MetaFunctions.ModulosFunction = new LuaFunction(MetaFunctions.ModLua);
			MetaFunctions.UnaryNegationFunction = new LuaFunction(MetaFunctions.UnaryNegationLua);
			MetaFunctions.EqualFunction = new LuaFunction(MetaFunctions.EqualLua);
			MetaFunctions.LessThanFunction = new LuaFunction(MetaFunctions.LessThanLua);
			MetaFunctions.LessThanOrEqualFunction = new LuaFunction(MetaFunctions.LessThanOrEqualLua);
		}

		// Token: 0x04000163 RID: 355
		public static readonly LuaFunction GcFunction;

		// Token: 0x04000164 RID: 356
		public static readonly LuaFunction IndexFunction;

		// Token: 0x04000165 RID: 357
		public static readonly LuaFunction NewIndexFunction;

		// Token: 0x04000166 RID: 358
		public static readonly LuaFunction BaseIndexFunction;

		// Token: 0x04000167 RID: 359
		public static readonly LuaFunction ClassIndexFunction;

		// Token: 0x04000168 RID: 360
		public static readonly LuaFunction ClassNewIndexFunction;

		// Token: 0x04000169 RID: 361
		public static readonly LuaFunction ExecuteDelegateFunction;

		// Token: 0x0400016A RID: 362
		public static readonly LuaFunction CallConstructorFunction;

		// Token: 0x0400016B RID: 363
		public static readonly LuaFunction ToStringFunction;

		// Token: 0x0400016C RID: 364
		public static readonly LuaFunction CallDelegateFunction;

		// Token: 0x0400016D RID: 365
		public static readonly LuaFunction AddFunction;

		// Token: 0x0400016E RID: 366
		public static readonly LuaFunction SubtractFunction;

		// Token: 0x0400016F RID: 367
		public static readonly LuaFunction MultiplyFunction;

		// Token: 0x04000170 RID: 368
		public static readonly LuaFunction DivisionFunction;

		// Token: 0x04000171 RID: 369
		public static readonly LuaFunction ModulosFunction;

		// Token: 0x04000172 RID: 370
		public static readonly LuaFunction UnaryNegationFunction;

		// Token: 0x04000173 RID: 371
		public static readonly LuaFunction EqualFunction;

		// Token: 0x04000174 RID: 372
		public static readonly LuaFunction LessThanFunction;

		// Token: 0x04000175 RID: 373
		public static readonly LuaFunction LessThanOrEqualFunction;

		// Token: 0x04000176 RID: 374
		private readonly Dictionary<object, Dictionary<object, object>> _memberCache;

		// Token: 0x04000177 RID: 375
		private readonly ObjectTranslator _translator;

		// Token: 0x04000178 RID: 376
		public const string LuaIndexFunction = "local a={}local function b(c,d)local e=getmetatable(c)local f=e.cache[d]if f~=nil then if f==a then return nil end;return f else local g,h=get_object_member(c,d)if h then if g==nil then e.cache[d]=a else e.cache[d]=g end end;return g end end;return b";
	}
}
