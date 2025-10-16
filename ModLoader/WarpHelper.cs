using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HarmonyLib;
using Unity.Collections.LowLevel.Unsafe;

namespace ModLoader
{
	// Token: 0x02000011 RID: 17
	public static class WarpHelper
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00002D1C File Offset: 0x00000F1C
		public static Func<object> ConstructorFromCache(this Type type)
		{
			Func<object> result;
			if (WarpHelper.ClassConstructorCache.TryGetValue(type, out result))
			{
				return result;
			}
			if (type.IsValueType)
			{
				WarpHelper.ClassConstructorCache[type] = WarpHelper.CreateConstructor(type, null);
				return WarpHelper.ClassConstructorCache[type];
			}
			WarpHelper.ClassConstructorCache[type] = WarpHelper.CreateConstructor(type, AccessTools.FirstConstructor(type, (ConstructorInfo info) => info.GetParameters().Length == 0));
			return WarpHelper.ClassConstructorCache[type];
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public static Func<object> CreateConstructor(Type type, ConstructorInfo constructorInfo)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("createInstance", typeof(object), Type.EmptyTypes);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (!type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Newobj, constructorInfo);
				ilgenerator.Emit(OpCodes.Ret);
			}
			else
			{
				LocalBuilder local = ilgenerator.DeclareLocal(type);
				ilgenerator.Emit(OpCodes.Ldloc_S, local);
				ilgenerator.Emit(OpCodes.Box, type);
				ilgenerator.Emit(OpCodes.Ret);
			}
			return dynamicMethod.CreateDelegate(typeof(Func<object>)) as Func<object>;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002E34 File Offset: 0x00001034
		[return: TupleElementNames(new string[]
		{
			"field",
			"getter",
			"setter"
		})]
		public static ValueTuple<FieldInfo, Func<object, object>, Action<object, object>> FieldFromCache(this Type type, string field_name, bool getter_use = true, bool setter_use = true)
		{
			Func<object, object> func = null;
			Action<object, object> action = null;
			Dictionary<string, FieldInfo> dictionary;
			FieldInfo fieldInfo2;
			if (WarpHelper.FieldInfoCache.TryGetValue(type, out dictionary))
			{
				FieldInfo fieldInfo;
				if (dictionary.TryGetValue(field_name, out fieldInfo))
				{
					fieldInfo2 = fieldInfo;
				}
				else
				{
					fieldInfo2 = AccessTools.Field(type, field_name);
					dictionary[field_name] = fieldInfo2;
				}
			}
			else
			{
				fieldInfo2 = AccessTools.Field(type, field_name);
				WarpHelper.FieldInfoCache[type] = new Dictionary<string, FieldInfo>
				{
					{
						field_name,
						fieldInfo2
					}
				};
			}
			if (fieldInfo2 != null && getter_use)
			{
				Dictionary<string, Func<object, object>> dictionary2;
				if (WarpHelper.FieldGetterDynamicMethodCache.TryGetValue(type, out dictionary2))
				{
					Func<object, object> func2;
					if (dictionary2.TryGetValue(field_name, out func2))
					{
						func = func2;
					}
					else
					{
						func = WarpHelper.GenGetter(fieldInfo2, type);
						dictionary2[field_name] = func;
					}
				}
				else
				{
					func = WarpHelper.GenGetter(fieldInfo2, type);
					WarpHelper.FieldGetterDynamicMethodCache[type] = new Dictionary<string, Func<object, object>>
					{
						{
							field_name,
							func
						}
					};
				}
			}
			if (fieldInfo2 != null && setter_use)
			{
				Dictionary<string, Action<object, object>> dictionary3;
				if (WarpHelper.FieldSetterDynamicMethodCache.TryGetValue(type, out dictionary3))
				{
					Action<object, object> action2;
					if (dictionary3.TryGetValue(field_name, out action2))
					{
						action = action2;
					}
					else
					{
						action = WarpHelper.GenSetter(fieldInfo2, type);
						dictionary3[field_name] = action;
					}
				}
				else
				{
					action = WarpHelper.GenSetter(fieldInfo2, type);
					WarpHelper.FieldSetterDynamicMethodCache[type] = new Dictionary<string, Action<object, object>>
					{
						{
							field_name,
							action
						}
					};
				}
			}
			return new ValueTuple<FieldInfo, Func<object, object>, Action<object, object>>(fieldInfo2, func, action);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002F5C File Offset: 0x0000115C
		public static Func<object, object> GenGetter(FieldInfo fieldInfo, Type type)
		{
			Type fieldType = fieldInfo.FieldType;
			bool isValueType = type.IsValueType;
			bool isValueType2 = fieldType.IsValueType;
			if (!isValueType && !isValueType2)
			{
				return new Func<object, object>(WarpHelper.AccessHelper.ByOffset(UnsafeUtility.GetFieldOffset(fieldInfo)).Get);
			}
			DynamicMethod dynamicMethod = new DynamicMethod("simple_getter", WarpHelper.ObjType, new Type[]
			{
				WarpHelper.ObjType
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(isValueType ? OpCodes.Unbox : OpCodes.Castclass, type);
			ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			if (isValueType2)
			{
				ilgenerator.Emit(OpCodes.Box, fieldType);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return dynamicMethod.CreateDelegate(typeof(Func<object, object>)) as Func<object, object>;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000301C File Offset: 0x0000121C
		public static Action<object, object> GenSetter(FieldInfo fieldInfo, Type type)
		{
			Type fieldType = fieldInfo.FieldType;
			bool isValueType = fieldType.IsValueType;
			bool isValueType2 = type.IsValueType;
			if (!isValueType2 && !isValueType)
			{
				return new Action<object, object>(WarpHelper.AccessHelper.ByOffset(UnsafeUtility.GetFieldOffset(fieldInfo)).Set);
			}
			DynamicMethod dynamicMethod = new DynamicMethod("simple_setter", typeof(void), new Type[]
			{
				WarpHelper.ObjType,
				WarpHelper.ObjType
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(isValueType2 ? OpCodes.Unbox : OpCodes.Castclass, type);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(isValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, fieldType);
			ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return dynamicMethod.CreateDelegate(typeof(Action<object, object>)) as Action<object, object>;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000030F8 File Offset: 0x000012F8
		public static void Deconstruct<TKey, TVal>(this KeyValuePair<TKey, TVal> keyValuePair, out TKey key, out TVal val)
		{
			key = keyValuePair.Key;
			val = keyValuePair.Value;
		}

		// Token: 0x0400002F RID: 47
		public static readonly Dictionary<Type, Dictionary<string, FieldInfo>> FieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x04000030 RID: 48
		public static readonly Dictionary<Type, Dictionary<string, Func<object, object>>> FieldGetterDynamicMethodCache = new Dictionary<Type, Dictionary<string, Func<object, object>>>();

		// Token: 0x04000031 RID: 49
		public static readonly Dictionary<Type, Dictionary<string, Action<object, object>>> FieldSetterDynamicMethodCache = new Dictionary<Type, Dictionary<string, Action<object, object>>>();

		// Token: 0x04000032 RID: 50
		public static readonly Dictionary<Type, Func<object>> ClassConstructorCache = new Dictionary<Type, Func<object>>();

		// Token: 0x04000033 RID: 51
		private static readonly Type ObjType = typeof(object);

		// Token: 0x02000012 RID: 18
		public class AccessHelper
		{
			// Token: 0x06000032 RID: 50 RVA: 0x00003150 File Offset: 0x00001350
			public static WarpHelper.AccessHelper ByOffset(int offset)
			{
				WeakReference<WarpHelper.AccessHelper> weakReference;
				if (!WarpHelper.AccessHelper.UsedAccessHelper.TryGetValue(offset, out weakReference))
				{
					WarpHelper.AccessHelper accessHelper = new WarpHelper.AccessHelper(offset);
					WarpHelper.AccessHelper.UsedAccessHelper[offset] = new WeakReference<WarpHelper.AccessHelper>(accessHelper);
					return accessHelper;
				}
				WarpHelper.AccessHelper accessHelper2;
				if (weakReference.TryGetTarget(out accessHelper2))
				{
					return accessHelper2;
				}
				accessHelper2 = new WarpHelper.AccessHelper(offset);
				weakReference.SetTarget(accessHelper2);
				return accessHelper2;
			}

			// Token: 0x06000033 RID: 51 RVA: 0x000031A1 File Offset: 0x000013A1
			private AccessHelper(int offset)
			{
				this.Offset = offset;
				WarpHelper.AccessHelper.UsedAccessHelper[offset] = new WeakReference<WarpHelper.AccessHelper>(this);
			}

			// Token: 0x06000034 RID: 52 RVA: 0x000031C4 File Offset: 0x000013C4
			public unsafe object Get(object o)
			{
				ulong gcHandle;
				void* value = UnsafeUtility.PinGCObjectAndGetAddress(o, out gcHandle);
				object result = WarpHelper.AccessHelper.Unsafe.ToObj(*(IntPtr*)((void*)((IntPtr)value + this.Offset)));
				UnsafeUtility.ReleaseGCObject(gcHandle);
				return result;
			}

			// Token: 0x06000035 RID: 53 RVA: 0x00003208 File Offset: 0x00001408
			public unsafe void Set(object o, object val)
			{
				ulong gcHandle;
				void* value = UnsafeUtility.PinGCObjectAndGetAddress(o, out gcHandle);
				ulong gcHandle2;
				void* ptr = UnsafeUtility.PinGCObjectAndGetAddress(val, out gcHandle2);
				*(IntPtr*)((void*)((IntPtr)value + this.Offset)) = ptr;
				UnsafeUtility.ReleaseGCObject(gcHandle);
				UnsafeUtility.ReleaseGCObject(gcHandle2);
			}

			// Token: 0x04000034 RID: 52
			private static readonly Dictionary<int, WeakReference<WarpHelper.AccessHelper>> UsedAccessHelper = new Dictionary<int, WeakReference<WarpHelper.AccessHelper>>();

			// Token: 0x04000035 RID: 53
			private readonly int Offset;

			// Token: 0x04000036 RID: 54
			private static readonly WarpHelper.AccessHelper.UnsafeTool Unsafe = new WarpHelper.AccessHelper.UnsafeTool();

			// Token: 0x02000013 RID: 19
			[StructLayout(LayoutKind.Explicit)]
			private class UnsafeTool
			{
				// Token: 0x06000037 RID: 55 RVA: 0x0000325F File Offset: 0x0000145F
				public UnsafeTool()
				{
					this.__accessBackend = new Func<object, object>(this.__transform);
				}

				// Token: 0x06000038 RID: 56 RVA: 0x00003279 File Offset: 0x00001479
				private object __transform(object o)
				{
					return o;
				}

				// Token: 0x04000037 RID: 55
				[FieldOffset(0)]
				private Func<object, object> __accessBackend;

				// Token: 0x04000038 RID: 56
				[FieldOffset(0)]
				public WarpHelper.AccessHelper.UnsafeTool.Obj2PtrFunc ToPtr;

				// Token: 0x04000039 RID: 57
				[FieldOffset(0)]
				public WarpHelper.AccessHelper.UnsafeTool.Ptr2ObjFunc ToObj;

				// Token: 0x02000014 RID: 20
				// (Invoke) Token: 0x0600003A RID: 58
				public unsafe delegate void* Obj2PtrFunc(object o);

				// Token: 0x02000015 RID: 21
				// (Invoke) Token: 0x0600003E RID: 62
				public unsafe delegate object Ptr2ObjFunc(void* o);
			}
		}
	}
}
