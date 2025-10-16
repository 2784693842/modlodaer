using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KeraLua;

namespace NLua
{
	// Token: 0x02000067 RID: 103
	internal class LuaGlobals
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000B1B4 File Offset: 0x000093B4
		// (set) Token: 0x0600025C RID: 604 RVA: 0x0000B1BC File Offset: 0x000093BC
		public int MaximumRecursion { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000B1C5 File Offset: 0x000093C5
		public IEnumerable<string> Globals
		{
			get
			{
				if (!this._globalsSorted)
				{
					this._globals.Sort();
					this._globalsSorted = true;
				}
				return this._globals;
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000B1E7 File Offset: 0x000093E7
		public bool Contains(string fullPath)
		{
			return this._globals.Contains(fullPath);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000B1F8 File Offset: 0x000093F8
		public void RemoveGlobal(string path)
		{
			LuaGlobalEntry knownType = this.GetKnownType(path);
			if (knownType != null)
			{
				foreach (string item in knownType.linkedGlobals)
				{
					this._globals.Remove(item);
				}
				this._knownTypes.Remove(knownType);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000B26C File Offset: 0x0000946C
		private LuaGlobalEntry GetKnownType(string path)
		{
			return this._knownTypes.Find((LuaGlobalEntry x) => x.Path.Equals(path));
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000B2A0 File Offset: 0x000094A0
		public void RegisterGlobal(string path, Type type, int recursionCounter)
		{
			LuaGlobalEntry knownType = this.GetKnownType(path);
			if (knownType != null)
			{
				if (type.Equals(knownType.Type))
				{
					return;
				}
				this.RemoveGlobal(path);
			}
			this.RegisterPath(path, type, recursionCounter, null);
			this._globalsSorted = false;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B2E0 File Offset: 0x000094E0
		private void RegisterPath(string path, Type type, int recursionCounter, LuaGlobalEntry entry = null)
		{
			if (type == typeof(LuaFunction))
			{
				this.RegisterLuaFunction(path, entry);
				return;
			}
			if ((type.IsClass || type.IsInterface) && type != typeof(string) && recursionCounter < this.MaximumRecursion)
			{
				this.RegisterClassOrInterface(path, type, recursionCounter, entry);
				return;
			}
			this.RegisterPrimitive(path, entry);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B349 File Offset: 0x00009549
		private void RegisterLuaFunction(string path, LuaGlobalEntry entry = null)
		{
			this._globals.Add(path + "(");
			if (entry != null)
			{
				entry.linkedGlobals.Add(path);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B370 File Offset: 0x00009570
		private void RegisterPrimitive(string path, LuaGlobalEntry entry = null)
		{
			this._globals.Add(path);
			if (entry != null)
			{
				entry.linkedGlobals.Add(path);
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B390 File Offset: 0x00009590
		private void RegisterClassOrInterface(string path, Type type, int recursionCounter, LuaGlobalEntry entry = null)
		{
			if (entry == null)
			{
				entry = new LuaGlobalEntry(type, path);
				this._knownTypes.Add(entry);
			}
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
			{
				string name = methodInfo.Name;
				if (!methodInfo.GetCustomAttributes(typeof(LuaHideAttribute), false).Any<object>() && name != "GetType" && name != "GetHashCode" && name != "Equals" && name != "ToString" && name != "Clone" && name != "Dispose" && name != "GetEnumerator" && name != "CopyTo" && !name.StartsWith("get_", StringComparison.Ordinal) && !name.StartsWith("set_", StringComparison.Ordinal) && !name.StartsWith("add_", StringComparison.Ordinal) && !name.StartsWith("remove_", StringComparison.Ordinal))
				{
					if (methodInfo.GetCustomAttributes(typeof(LuaMemberAttribute), false).Any<object>())
					{
						name = ((LuaMemberAttribute)methodInfo.GetCustomAttributes(typeof(LuaMemberAttribute), false).First<object>()).Name;
					}
					string text = path + ":" + name + "(";
					if (methodInfo.GetParameters().Length == 0)
					{
						text += ")";
					}
					this._globals.Add(text);
					entry.linkedGlobals.Add(text);
				}
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				if (!fieldInfo.GetCustomAttributes(typeof(LuaHideAttribute), false).Any<object>())
				{
					string name2 = fieldInfo.Name;
					if (fieldInfo.GetCustomAttributes(typeof(LuaMemberAttribute), false).Any<object>())
					{
						name2 = ((LuaMemberAttribute)fieldInfo.GetCustomAttributes(typeof(LuaMemberAttribute), false).First<object>()).Name;
					}
					this.RegisterPath(path + "." + name2, fieldInfo.FieldType, recursionCounter + 1, entry);
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (!propertyInfo.GetCustomAttributes(typeof(LuaHideAttribute), false).Any<object>() && propertyInfo.Name != "Item")
				{
					string name3 = propertyInfo.Name;
					if (propertyInfo.GetCustomAttributes(typeof(LuaMemberAttribute), false).Any<object>())
					{
						name3 = ((LuaMemberAttribute)propertyInfo.GetCustomAttributes(typeof(LuaMemberAttribute), false).First<object>()).Name;
					}
					this.RegisterPath(path + "." + name3, propertyInfo.PropertyType, recursionCounter + 1, entry);
				}
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B690 File Offset: 0x00009890
		public LuaGlobals()
		{
			this._globals = new List<string>();
			this._knownTypes = new List<LuaGlobalEntry>();
			this.MaximumRecursion = 2;
			base..ctor();
		}

		// Token: 0x04000107 RID: 263
		private List<string> _globals;

		// Token: 0x04000108 RID: 264
		private List<LuaGlobalEntry> _knownTypes;

		// Token: 0x04000109 RID: 265
		public bool _globalsSorted;
	}
}
