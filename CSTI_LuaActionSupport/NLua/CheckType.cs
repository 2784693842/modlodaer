using System;
using System.Collections.Generic;
using KeraLua;
using NLua.Extensions;
using NLua.Method;

namespace NLua
{
	// Token: 0x02000084 RID: 132
	internal sealed class CheckType
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x00010200 File Offset: 0x0000E400
		public CheckType(ObjectTranslator translator)
		{
			this._extractValues = new Dictionary<Type, ExtractValue>();
			base..ctor();
			this._translator = translator;
			this._extractValues.Add(typeof(object), new ExtractValue(this.GetAsObject));
			this._extractValues.Add(typeof(sbyte), new ExtractValue(this.GetAsSbyte));
			this._extractValues.Add(typeof(byte), new ExtractValue(this.GetAsByte));
			this._extractValues.Add(typeof(short), new ExtractValue(this.GetAsShort));
			this._extractValues.Add(typeof(ushort), new ExtractValue(this.GetAsUshort));
			this._extractValues.Add(typeof(int), new ExtractValue(this.GetAsInt));
			this._extractValues.Add(typeof(uint), new ExtractValue(this.GetAsUint));
			this._extractValues.Add(typeof(long), new ExtractValue(this.GetAsLong));
			this._extractValues.Add(typeof(ulong), new ExtractValue(this.GetAsUlong));
			this._extractValues.Add(typeof(double), new ExtractValue(this.GetAsDouble));
			this._extractValues.Add(typeof(char), new ExtractValue(this.GetAsChar));
			this._extractValues.Add(typeof(float), new ExtractValue(this.GetAsFloat));
			this._extractValues.Add(typeof(decimal), new ExtractValue(this.GetAsDecimal));
			this._extractValues.Add(typeof(bool), new ExtractValue(this.GetAsBoolean));
			this._extractValues.Add(typeof(string), new ExtractValue(this.GetAsString));
			this._extractValues.Add(typeof(char[]), new ExtractValue(this.GetAsCharArray));
			this._extractValues.Add(typeof(byte[]), new ExtractValue(this.GetAsByteArray));
			this._extractValues.Add(typeof(LuaFunction), new ExtractValue(this.GetAsFunction));
			this._extractValues.Add(typeof(LuaTable), new ExtractValue(this.GetAsTable));
			this._extractValues.Add(typeof(LuaThread), new ExtractValue(this.GetAsThread));
			this._extractValues.Add(typeof(LuaUserData), new ExtractValue(this.GetAsUserdata));
			this._extractNetObject = new ExtractValue(this.GetAsNetObject);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000104EC File Offset: 0x0000E6EC
		internal ExtractValue GetExtractor(ProxyType paramType)
		{
			return this.GetExtractor(paramType.UnderlyingSystemType);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000104FA File Offset: 0x0000E6FA
		internal ExtractValue GetExtractor(Type paramType)
		{
			if (paramType.IsByRef)
			{
				paramType = paramType.GetElementType();
			}
			if (!this._extractValues.ContainsKey(paramType))
			{
				return this._extractNetObject;
			}
			return this._extractValues[paramType];
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00010530 File Offset: 0x0000E730
		internal ExtractValue CheckLuaType(Lua luaState, int stackPos, Type paramType)
		{
			LuaType luaType = luaState.Type(stackPos);
			if (paramType.IsByRef)
			{
				paramType = paramType.GetElementType();
			}
			Type underlyingType = Nullable.GetUnderlyingType(paramType);
			if (underlyingType != null)
			{
				paramType = underlyingType;
			}
			bool flag = paramType == typeof(int) || paramType == typeof(uint) || paramType == typeof(long) || paramType == typeof(ulong) || paramType == typeof(short) || paramType == typeof(ushort) || paramType == typeof(float) || paramType == typeof(double) || paramType == typeof(decimal) || paramType == typeof(byte);
			if (underlyingType != null && luaType == LuaType.Nil)
			{
				if (flag || paramType == typeof(bool))
				{
					return this._extractValues[paramType];
				}
				return this._extractNetObject;
			}
			else
			{
				if (paramType == typeof(object))
				{
					return this._extractValues[paramType];
				}
				if (paramType.IsGenericParameter)
				{
					if (luaType == LuaType.Boolean)
					{
						return this._extractValues[typeof(bool)];
					}
					if (luaType == LuaType.String)
					{
						return this._extractValues[typeof(string)];
					}
					if (luaType == LuaType.Table)
					{
						return this._extractValues[typeof(LuaTable)];
					}
					if (luaType == LuaType.Thread)
					{
						return this._extractValues[typeof(LuaThread)];
					}
					if (luaType == LuaType.UserData)
					{
						return this._extractValues[typeof(object)];
					}
					if (luaType == LuaType.Function)
					{
						return this._extractValues[typeof(LuaFunction)];
					}
					if (luaType == LuaType.Number)
					{
						return this._extractValues[typeof(double)];
					}
				}
				bool flag2 = paramType == typeof(string) || paramType == typeof(char[]) || paramType == typeof(byte[]);
				if (flag)
				{
					if (luaState.IsNumericType(stackPos) && !flag2)
					{
						return this._extractValues[paramType];
					}
				}
				else if (paramType == typeof(bool))
				{
					if (luaState.IsBoolean(stackPos))
					{
						return this._extractValues[paramType];
					}
				}
				else if (flag2)
				{
					if (luaState.IsString(stackPos) || luaType == LuaType.Nil)
					{
						return this._extractValues[paramType];
					}
				}
				else if (paramType == typeof(LuaTable))
				{
					if (luaType == LuaType.Table || luaType == LuaType.Nil)
					{
						return this._extractValues[paramType];
					}
				}
				else if (paramType == typeof(LuaThread))
				{
					if (luaType == LuaType.Thread || luaType == LuaType.Nil)
					{
						return this._extractValues[paramType];
					}
				}
				else if (paramType == typeof(LuaUserData))
				{
					if (luaType == LuaType.UserData || luaType == LuaType.Nil)
					{
						return this._extractValues[paramType];
					}
				}
				else if (paramType == typeof(LuaFunction))
				{
					if (luaType == LuaType.Function || luaType == LuaType.Nil)
					{
						return this._extractValues[paramType];
					}
				}
				else
				{
					if (typeof(Delegate).IsAssignableFrom(paramType) && luaType == LuaType.Function && paramType.GetMethod("Invoke") != null)
					{
						return new ExtractValue(new DelegateGenerator(this._translator, paramType).ExtractGenerated);
					}
					if (paramType.IsInterface && luaType == LuaType.Table)
					{
						return new ExtractValue(new ClassGenerator(this._translator, paramType).ExtractGenerated);
					}
					if ((paramType.IsInterface || paramType.IsClass) && luaType == LuaType.Nil)
					{
						return this._extractNetObject;
					}
					if (luaState.Type(stackPos) == LuaType.Table)
					{
						if (luaState.GetMetaField(stackPos, "__index") == LuaType.Nil)
						{
							return null;
						}
						object netObject = this._translator.GetNetObject(luaState, -1);
						luaState.SetTop(-2);
						if (netObject != null && paramType.IsInstanceOfType(netObject))
						{
							return this._extractNetObject;
						}
					}
				}
				object netObject2 = this._translator.GetNetObject(luaState, stackPos);
				if (netObject2 != null && paramType.IsInstanceOfType(netObject2))
				{
					return this._extractNetObject;
				}
				return null;
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001096B File Offset: 0x0000EB6B
		private object GetAsSbyte(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (sbyte)luaState.ToInteger(stackPos);
			}
			return (sbyte)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001099C File Offset: 0x0000EB9C
		private object GetAsByte(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (byte)luaState.ToInteger(stackPos);
			}
			return (byte)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000109CD File Offset: 0x0000EBCD
		private object GetAsShort(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (short)luaState.ToInteger(stackPos);
			}
			return (short)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000109FE File Offset: 0x0000EBFE
		private object GetAsUshort(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (ushort)luaState.ToInteger(stackPos);
			}
			return (ushort)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00010A2F File Offset: 0x0000EC2F
		private object GetAsInt(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (int)luaState.ToInteger(stackPos);
			}
			return (int)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00010A60 File Offset: 0x0000EC60
		private object GetAsUint(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (uint)luaState.ToInteger(stackPos);
			}
			return (uint)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00010A91 File Offset: 0x0000EC91
		private object GetAsLong(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return luaState.ToInteger(stackPos);
			}
			return (long)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00010AC1 File Offset: 0x0000ECC1
		private object GetAsUlong(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (ulong)luaState.ToInteger(stackPos);
			}
			return (ulong)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00010AF1 File Offset: 0x0000ECF1
		private object GetAsDouble(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (double)luaState.ToInteger(stackPos);
			}
			return luaState.ToNumber(stackPos);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00010B21 File Offset: 0x0000ED21
		private object GetAsChar(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (char)luaState.ToInteger(stackPos);
			}
			return (char)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00010B52 File Offset: 0x0000ED52
		private object GetAsFloat(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return (float)luaState.ToInteger(stackPos);
			}
			return (float)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00010B83 File Offset: 0x0000ED83
		private object GetAsDecimal(Lua luaState, int stackPos)
		{
			if (!luaState.IsNumericType(stackPos))
			{
				return null;
			}
			if (luaState.IsInteger(stackPos))
			{
				return luaState.ToInteger(stackPos);
			}
			return (decimal)luaState.ToNumber(stackPos);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00010BBC File Offset: 0x0000EDBC
		private object GetAsBoolean(Lua luaState, int stackPos)
		{
			return luaState.ToBoolean(stackPos);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00010BCA File Offset: 0x0000EDCA
		private object GetAsCharArray(Lua luaState, int stackPos)
		{
			if (!luaState.IsString(stackPos))
			{
				return null;
			}
			return luaState.ToString(stackPos, false).ToCharArray();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00010BE4 File Offset: 0x0000EDE4
		private object GetAsByteArray(Lua luaState, int stackPos)
		{
			if (!luaState.IsString(stackPos))
			{
				return null;
			}
			return luaState.ToBuffer(stackPos, false);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00010BF9 File Offset: 0x0000EDF9
		private object GetAsString(Lua luaState, int stackPos)
		{
			if (!luaState.IsString(stackPos))
			{
				return null;
			}
			return luaState.ToString(stackPos, false);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00010C0E File Offset: 0x0000EE0E
		private object GetAsTable(Lua luaState, int stackPos)
		{
			return this._translator.GetTable(luaState, stackPos);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00010C1D File Offset: 0x0000EE1D
		private object GetAsThread(Lua luaState, int stackPos)
		{
			return this._translator.GetThread(luaState, stackPos);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00010C2C File Offset: 0x0000EE2C
		private object GetAsFunction(Lua luaState, int stackPos)
		{
			return this._translator.GetFunction(luaState, stackPos);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00010C3B File Offset: 0x0000EE3B
		private object GetAsUserdata(Lua luaState, int stackPos)
		{
			return this._translator.GetUserData(luaState, stackPos);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00010C4C File Offset: 0x0000EE4C
		public object GetAsObject(Lua luaState, int stackPos)
		{
			if (luaState.Type(stackPos) == LuaType.Table && luaState.GetMetaField(stackPos, "__index") != LuaType.Nil)
			{
				if (luaState.CheckMetaTable(-1, this._translator.Tag))
				{
					luaState.Insert(stackPos);
					luaState.Remove(stackPos + 1);
				}
				else
				{
					luaState.SetTop(-2);
				}
			}
			return this._translator.GetObject(luaState, stackPos);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00010CAC File Offset: 0x0000EEAC
		public object GetAsNetObject(Lua luaState, int stackPos)
		{
			object netObject = this._translator.GetNetObject(luaState, stackPos);
			if (netObject != null || luaState.Type(stackPos) != LuaType.Table)
			{
				return netObject;
			}
			if (luaState.GetMetaField(stackPos, "__index") == LuaType.Nil)
			{
				return null;
			}
			if (luaState.CheckMetaTable(-1, this._translator.Tag))
			{
				luaState.Insert(stackPos);
				luaState.Remove(stackPos + 1);
				netObject = this._translator.GetNetObject(luaState, stackPos);
			}
			else
			{
				luaState.SetTop(-2);
			}
			return netObject;
		}

		// Token: 0x04000188 RID: 392
		private readonly Dictionary<Type, ExtractValue> _extractValues;

		// Token: 0x04000189 RID: 393
		private readonly ExtractValue _extractNetObject;

		// Token: 0x0400018A RID: 394
		private readonly ObjectTranslator _translator;
	}
}
