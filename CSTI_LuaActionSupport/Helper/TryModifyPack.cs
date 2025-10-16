using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace CSTI_LuaActionSupport.Helper
{
	// Token: 0x02000045 RID: 69
	public static class TryModifyPack
	{
		// Token: 0x06000154 RID: 340 RVA: 0x00006984 File Offset: 0x00004B84
		[NullableContext(1)]
		private static MulticastDelegate GenTryNum(Type self, Type tIn)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("TryModifyPack_TryNum_" + self.Name + "_" + tIn.Name, self, new Type[]
			{
				typeof(object)
			}, typeof(TryModifyPack));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Unbox_Any, tIn);
			ilgenerator.Emit(TryModifyPack.FloatLike[self]);
			ilgenerator.Emit(OpCodes.Ret);
			return (MulticastDelegate)dynamicMethod.CreateDelegate(typeof(TryModifyPack._TryNum<int>).GetGenericTypeDefinition().MakeGenericType(new Type[]
			{
				self
			}));
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006A30 File Offset: 0x00004C30
		public static T? TryNum<T>([Nullable(2)] this object o) where T : struct
		{
			if (o == null)
			{
				return null;
			}
			Type type = o.GetType();
			if (TryModifyPack.FloatLike.ContainsKey(typeof(T)) && TryModifyPack.FloatLike.ContainsKey(type))
			{
				if (!TryModifyPack.tryNumCache.ContainsKey(typeof(T)))
				{
					TryModifyPack.tryNumCache[typeof(T)] = new Dictionary<Type, MulticastDelegate>();
				}
				MulticastDelegate multicastDelegate;
				if (!TryModifyPack.tryNumCache[typeof(T)].TryGetValue(type, out multicastDelegate))
				{
					multicastDelegate = TryModifyPack.GenTryNum(typeof(T), type);
					TryModifyPack.tryNumCache[typeof(T)][type] = multicastDelegate;
				}
				return new T?(((TryModifyPack._TryNum<T>)multicastDelegate)(o));
			}
			return null;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006B10 File Offset: 0x00004D10
		[NullableContext(2)]
		public static void TryModBy(this float self, object o)
		{
			if (o == null)
			{
				return;
			}
			Type type = o.GetType();
			if (TryModifyPack.FloatLike.ContainsKey(type))
			{
				float? num = o.TryNum<float>();
				if (num != null)
				{
					float valueOrDefault = num.GetValueOrDefault();
					self = valueOrDefault;
				}
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006B50 File Offset: 0x00004D50
		[NullableContext(2)]
		public static void TryModBy(this int self, object o)
		{
			if (o == null)
			{
				return;
			}
			Type type = o.GetType();
			if (TryModifyPack.FloatLike.ContainsKey(type))
			{
				int? num = o.TryNum<int>();
				if (num != null)
				{
					int valueOrDefault = num.GetValueOrDefault();
					self = valueOrDefault;
				}
			}
		}

		// Token: 0x04000092 RID: 146
		[Nullable(1)]
		private static readonly Dictionary<Type, Dictionary<Type, MulticastDelegate>> tryNumCache = new Dictionary<Type, Dictionary<Type, MulticastDelegate>>();

		// Token: 0x04000093 RID: 147
		[Nullable(1)]
		private static readonly Dictionary<Type, OpCode> FloatLike = new Dictionary<Type, OpCode>
		{
			{
				typeof(double),
				OpCodes.Conv_R8
			},
			{
				typeof(float),
				OpCodes.Conv_R4
			},
			{
				typeof(long),
				OpCodes.Conv_I8
			},
			{
				typeof(ulong),
				OpCodes.Conv_U8
			},
			{
				typeof(int),
				OpCodes.Conv_I4
			},
			{
				typeof(uint),
				OpCodes.Conv_U4
			},
			{
				typeof(IntPtr),
				OpCodes.Conv_I
			},
			{
				typeof(UIntPtr),
				OpCodes.Conv_U
			}
		};

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x0600015A RID: 346
		private delegate T _TryNum<out T>([Nullable(2)] object o) where T : struct;
	}
}
