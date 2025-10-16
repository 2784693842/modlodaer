using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KeraLua;
using NLua.Exceptions;
using NLua.Extensions;

namespace NLua.Method
{
	// Token: 0x0200008A RID: 138
	internal class LuaMethodWrapper
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x0001127C File Offset: 0x0000F47C
		public LuaMethodWrapper(ObjectTranslator translator, object target, ProxyType targetType, MethodBase method)
		{
			this.InvokeFunction = new LuaFunction(this.Call);
			this._translator = translator;
			this._target = target;
			this._extractTarget = translator.typeChecker.GetExtractor(targetType);
			this._lastCalledMethod = new MethodCache();
			this._method = method;
			this._methodName = method.Name;
			this._isStatic = method.IsStatic;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000112F0 File Offset: 0x0000F4F0
		public LuaMethodWrapper(ObjectTranslator translator, ProxyType targetType, string methodName, BindingFlags bindingType)
		{
			this.InvokeFunction = new LuaFunction(this.Call);
			this._translator = translator;
			this._methodName = methodName;
			this._extractTarget = translator.typeChecker.GetExtractor(targetType);
			this._lastCalledMethod = new MethodCache();
			this._isStatic = ((bindingType & BindingFlags.Static) == BindingFlags.Static);
			MethodInfo[] methodsRecursively = this.GetMethodsRecursively(targetType.UnderlyingSystemType, methodName, bindingType | BindingFlags.Public);
			this._members = LuaMethodWrapper.ReorderMethods(methodsRecursively);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001136C File Offset: 0x0000F56C
		private static MethodInfo[] ReorderMethods(MethodInfo[] m)
		{
			if (m.Length < 2)
			{
				return m;
			}
			return (from c in m
			group c by c.GetParameters().Length).SelectMany((IGrouping<int, MethodInfo> g) => from ci in g
			orderby ci.ToString() descending
			select ci).ToArray<MethodInfo>();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000113D0 File Offset: 0x0000F5D0
		private MethodInfo[] GetMethodsRecursively(Type type, string methodName, BindingFlags bindingType)
		{
			if (type == typeof(object))
			{
				return type.GetMethods(methodName, bindingType);
			}
			IEnumerable<MethodInfo> methodsForType = LuaMemberAttribute.GetMethodsForType(type, methodName, bindingType);
			MethodInfo[] methodsRecursively = this.GetMethodsRecursively(type.BaseType, methodName, bindingType);
			return methodsForType.Concat(methodsRecursively).ToArray<MethodInfo>();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001141A File Offset: 0x0000F61A
		private int SetPendingException(Exception e)
		{
			return this._translator.interpreter.SetPendingException(e);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00011430 File Offset: 0x0000F630
		private void FillMethodArguments(Lua luaState, int numStackToSkip)
		{
			object[] args = this._lastCalledMethod.args;
			for (int i = 0; i < this._lastCalledMethod.argTypes.Length; i++)
			{
				MethodArgs methodArgs = this._lastCalledMethod.argTypes[i];
				int num = i + 1 + numStackToSkip;
				if (this._lastCalledMethod.argTypes[i].IsParamsArray)
				{
					int count = this._lastCalledMethod.argTypes.Length - i;
					Array array = this._translator.CreateParamsArray(luaState, methodArgs.ExtractValue, methodArgs.ParameterType, num, count);
					args[this._lastCalledMethod.argTypes[i].Index] = array;
				}
				else
				{
					args[methodArgs.Index] = methodArgs.ExtractValue(luaState, num);
				}
				if (this._lastCalledMethod.args[this._lastCalledMethod.argTypes[i].Index] == null && !luaState.IsNil(i + 1 + numStackToSkip))
				{
					throw new LuaException(string.Format("Argument number {0} is invalid", i + 1));
				}
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00011530 File Offset: 0x0000F730
		private int PushReturnValue(Lua luaState, object returnValue)
		{
			int num = 0;
			if (!this._lastCalledMethod.IsReturnVoid)
			{
				num = this._translator.PushMultiple(luaState, returnValue);
			}
			for (int i = 0; i < this._lastCalledMethod.outList.Length; i++)
			{
				num++;
				this._translator.Push(luaState, this._lastCalledMethod.args[this._lastCalledMethod.outList[i]]);
			}
			return num;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001159C File Offset: 0x0000F79C
		private int CallInvoke(Lua luaState, MethodBase method, object targetObject)
		{
			if (!luaState.CheckStack(this._lastCalledMethod.outList.Length + 6))
			{
				throw new LuaException("Lua stack overflow");
			}
			int result;
			try
			{
				object returnValue;
				if (method.IsConstructor)
				{
					returnValue = ((ConstructorInfo)method).Invoke(this._lastCalledMethod.args);
				}
				else
				{
					returnValue = method.Invoke(targetObject, this._lastCalledMethod.args);
				}
				result = this.PushReturnValue(luaState, returnValue);
			}
			catch (TargetInvocationException ex)
			{
				if (this._translator.interpreter.UseTraceback)
				{
					ex.GetBaseException().Data["Traceback"] = this._translator.interpreter.GetDebugTraceback();
				}
				result = this.SetPendingException(ex.GetBaseException());
			}
			catch (Exception pendingException)
			{
				result = this.SetPendingException(pendingException);
			}
			return result;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001167C File Offset: 0x0000F87C
		private bool IsMethodCached(Lua luaState, int numArgsPassed, int skipParams)
		{
			return !(this._lastCalledMethod.cachedMethod == null) && numArgsPassed == this._lastCalledMethod.argTypes.Length && (this._members.Length == 1 || this._translator.MatchParameters(luaState, this._lastCalledMethod.cachedMethod, this._lastCalledMethod, skipParams));
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000116DC File Offset: 0x0000F8DC
		private int CallMethodFromName(Lua luaState)
		{
			object obj = null;
			if (!this._isStatic)
			{
				obj = this._extractTarget(luaState, 1);
			}
			int num = this._isStatic ? 0 : 1;
			int numArgsPassed = luaState.GetTop() - num;
			if (this.IsMethodCached(luaState, numArgsPassed, num))
			{
				MethodBase cachedMethod = this._lastCalledMethod.cachedMethod;
				if (!luaState.CheckStack(this._lastCalledMethod.outList.Length + 6))
				{
					throw new LuaException("Lua stack overflow");
				}
				this.FillMethodArguments(luaState, num);
				return this.CallInvoke(luaState, cachedMethod, obj);
			}
			else
			{
				if (!this._isStatic)
				{
					if (obj == null)
					{
						this._translator.ThrowError(luaState, string.Format("instance method '{0}' requires a non null target object", this._methodName));
						return 1;
					}
					luaState.Remove(1);
				}
				bool flag = false;
				string text = null;
				foreach (MethodInfo methodInfo in this._members)
				{
					if (!(methodInfo.ReflectedType == null))
					{
						text = methodInfo.ReflectedType.Name + "." + methodInfo.Name;
						if (this._translator.MatchParameters(luaState, methodInfo, this._lastCalledMethod, 0))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					string e = (text == null) ? "Invalid arguments to method call" : ("Invalid arguments to method: " + text);
					this._translator.ThrowError(luaState, e);
					return 1;
				}
				if (this._lastCalledMethod.cachedMethod.ContainsGenericParameters)
				{
					return this.CallInvokeOnGenericMethod(luaState, (MethodInfo)this._lastCalledMethod.cachedMethod, obj);
				}
				return this.CallInvoke(luaState, this._lastCalledMethod.cachedMethod, obj);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00011870 File Offset: 0x0000FA70
		private int CallInvokeOnGenericMethod(Lua luaState, MethodInfo methodToCall, object targetObject)
		{
			List<Type> list = new List<Type>();
			ParameterInfo[] parameters = methodToCall.GetParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].ParameterType.IsGenericParameter)
				{
					list.Add(this._lastCalledMethod.args[i].GetType());
				}
			}
			object returnValue = methodToCall.MakeGenericMethod(list.ToArray()).Invoke(targetObject, this._lastCalledMethod.args);
			return this.PushReturnValue(luaState, returnValue);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x000118E8 File Offset: 0x0000FAE8
		private int Call(IntPtr state)
		{
			Lua lua = Lua.FromIntPtr(state);
			MethodBase method = this._method;
			object obj = this._target;
			if (!lua.CheckStack(5))
			{
				throw new LuaException("Lua stack overflow");
			}
			this.SetPendingException(null);
			if (method == null)
			{
				return this.CallMethodFromName(lua);
			}
			if (!method.ContainsGenericParameters)
			{
				if (!method.IsStatic && !method.IsConstructor && obj == null)
				{
					obj = this._extractTarget(lua, 1);
					lua.Remove(1);
				}
				if (!this._translator.MatchParameters(lua, method, this._lastCalledMethod, 0))
				{
					this._translator.ThrowError(lua, "Invalid arguments to method call");
					return 1;
				}
				if (this._isStatic)
				{
					obj = null;
				}
				return this.CallInvoke(lua, this._lastCalledMethod.cachedMethod, obj);
			}
			else
			{
				if (!method.IsGenericMethodDefinition)
				{
					this._translator.ThrowError(lua, "Unable to invoke method on generic class as the current method is an open generic method");
					return 1;
				}
				this._translator.MatchParameters(lua, method, this._lastCalledMethod, 0);
				return this.CallInvokeOnGenericMethod(lua, (MethodInfo)method, obj);
			}
		}

		// Token: 0x0400018F RID: 399
		internal LuaFunction InvokeFunction;

		// Token: 0x04000190 RID: 400
		private readonly ObjectTranslator _translator;

		// Token: 0x04000191 RID: 401
		private readonly MethodBase _method;

		// Token: 0x04000192 RID: 402
		private readonly ExtractValue _extractTarget;

		// Token: 0x04000193 RID: 403
		private readonly object _target;

		// Token: 0x04000194 RID: 404
		private readonly bool _isStatic;

		// Token: 0x04000195 RID: 405
		private readonly string _methodName;

		// Token: 0x04000196 RID: 406
		private readonly MethodInfo[] _members;

		// Token: 0x04000197 RID: 407
		private MethodCache _lastCalledMethod;
	}
}
