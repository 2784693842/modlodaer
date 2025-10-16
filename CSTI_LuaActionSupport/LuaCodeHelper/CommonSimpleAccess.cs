using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.Helper;
using HarmonyLib;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200003D RID: 61
	[NullableContext(2)]
	[Nullable(0)]
	public abstract class CommonSimpleAccess
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600012A RID: 298
		public abstract object AccessObj { get; }

		// Token: 0x1700003C RID: 60
		public virtual object this[string key]
		{
			[NullableContext(1)]
			[return: Nullable(2)]
			get
			{
				if (this.AccessObj == null)
				{
					return null;
				}
				FieldInfo fieldInfo = AccessTools.Field(this.AccessObj.GetType(), key);
				if (fieldInfo == null)
				{
					return null;
				}
				object value = fieldInfo.GetValue(this.AccessObj);
				if (value == null)
				{
					return null;
				}
				UniqueIDScriptable uniqueIDScriptable = value as UniqueIDScriptable;
				if (uniqueIDScriptable != null)
				{
					return new SimpleUniqueAccess(uniqueIDScriptable);
				}
				return new SimpleObjAccess(value);
			}
			[NullableContext(1)]
			[param: Nullable(2)]
			set
			{
				if (this.AccessObj == null)
				{
					return;
				}
				FieldInfo fieldInfo = AccessTools.Field(this.AccessObj.GetType(), key);
				if (fieldInfo == null)
				{
					return;
				}
				bool flag = value is double || value is long;
				if (flag)
				{
					if (fieldInfo.FieldType == typeof(float))
					{
						fieldInfo.SetValue(this.AccessObj, value.TryNum<float>());
						return;
					}
					if (fieldInfo.FieldType == typeof(int))
					{
						fieldInfo.SetValue(this.AccessObj, value.TryNum<int>());
						return;
					}
					if (fieldInfo.FieldType == typeof(long))
					{
						fieldInfo.SetValue(this.AccessObj, value.TryNum<long>());
					}
					return;
				}
				else
				{
					if (((value != null) ? value.GetType() : null) != fieldInfo.FieldType)
					{
						return;
					}
					fieldInfo.SetValue(this.AccessObj, value);
					return;
				}
			}
		}
	}
}
