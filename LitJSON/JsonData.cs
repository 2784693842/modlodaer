using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
	// Token: 0x02000004 RID: 4
	public class JsonData : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary, IEquatable<JsonData>
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002050 File Offset: 0x00000250
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000205D File Offset: 0x0000025D
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002068 File Offset: 0x00000268
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002073 File Offset: 0x00000273
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000207E File Offset: 0x0000027E
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002089 File Offset: 0x00000289
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002094 File Offset: 0x00000294
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000209F File Offset: 0x0000029F
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000020AA File Offset: 0x000002AA
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000020BE File Offset: 0x000002BE
		public bool ContainsKey(string key)
		{
			this.EnsureDictionary();
			return this.inst_object.Keys.Contains(key);
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000020D8 File Offset: 0x000002D8
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000020E0 File Offset: 0x000002E0
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000020ED File Offset: 0x000002ED
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000020FA File Offset: 0x000002FA
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002107 File Offset: 0x00000307
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002114 File Offset: 0x00000314
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> keys = new List<string>();
				foreach (KeyValuePair<string, JsonData> entry in this.object_list)
				{
					keys.Add(entry.Key);
				}
				return (ICollection)keys;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000217C File Offset: 0x0000037C
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> values = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> entry in this.object_list)
				{
					values.Add(entry.Value);
				}
				return (ICollection)values;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000021E4 File Offset: 0x000003E4
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000021EC File Offset: 0x000003EC
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000021F4 File Offset: 0x000003F4
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000021FC File Offset: 0x000003FC
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002204 File Offset: 0x00000404
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000220C File Offset: 0x0000040C
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002214 File Offset: 0x00000414
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000221C File Offset: 0x0000041C
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002229 File Offset: 0x00000429
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x17000021 RID: 33
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData data = this.ToJsonData(value);
				this[(string)key] = data;
			}
		}

		// Token: 0x17000022 RID: 34
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData data = this.ToJsonData(value);
				KeyValuePair<string, JsonData> old_entry = this.object_list[idx];
				this.inst_object[old_entry.Key] = data;
				KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>(old_entry.Key, data);
				this.object_list[idx] = entry;
			}
		}

		// Token: 0x17000023 RID: 35
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData data = this.ToJsonData(value);
				this[index] = data;
			}
		}

		// Token: 0x17000024 RID: 36
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = entry;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(entry);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x17000025 RID: 37
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> entry = this.object_list[index];
					KeyValuePair<string, JsonData> new_entry = new KeyValuePair<string, JsonData>(entry.Key, value);
					this.object_list[index] = new_entry;
					this.inst_object[entry.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002487 File Offset: 0x00000687
		public JsonData()
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000248F File Offset: 0x0000068F
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000024A5 File Offset: 0x000006A5
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000024BB File Offset: 0x000006BB
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000024D1 File Offset: 0x000006D1
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000024E8 File Offset: 0x000006E8
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002591 File Offset: 0x00000791
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000025A7 File Offset: 0x000007A7
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000025AF File Offset: 0x000007AF
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000025B7 File Offset: 0x000007B7
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000025BF File Offset: 0x000007BF
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000025C7 File Offset: 0x000007C7
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000025CF File Offset: 0x000007CF
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000025EB File Offset: 0x000007EB
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002607 File Offset: 0x00000807
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int && data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			if (data.type != JsonType.Int)
			{
				return (int)data.inst_long;
			}
			return data.inst_int;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000263D File Offset: 0x0000083D
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long && data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a long");
			}
			if (data.type != JsonType.Long)
			{
				return (long)data.inst_int;
			}
			return data.inst_long;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002673 File Offset: 0x00000873
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000268F File Offset: 0x0000088F
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000026A0 File Offset: 0x000008A0
		void IDictionary.Add(object key, object value)
		{
			JsonData data = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, data);
			KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>((string)key, data);
			this.object_list.Add(entry);
			this.json = null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000026E3 File Offset: 0x000008E3
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002702 File Offset: 0x00000902
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002710 File Offset: 0x00000910
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002718 File Offset: 0x00000918
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000277D File Offset: 0x0000097D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000278A File Offset: 0x0000098A
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000027A6 File Offset: 0x000009A6
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000027C2 File Offset: 0x000009C2
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000027DE File Offset: 0x000009DE
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000027FA File Offset: 0x000009FA
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002816 File Offset: 0x00000A16
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000282D File Offset: 0x00000A2D
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002844 File Offset: 0x00000A44
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000285B File Offset: 0x00000A5B
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002872 File Offset: 0x00000A72
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002889 File Offset: 0x00000A89
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002891 File Offset: 0x00000A91
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000289A File Offset: 0x00000A9A
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000028A3 File Offset: 0x00000AA3
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000028B7 File Offset: 0x00000AB7
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000028C5 File Offset: 0x00000AC5
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000028D3 File Offset: 0x00000AD3
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000028E9 File Offset: 0x00000AE9
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000028FE File Offset: 0x00000AFE
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002913 File Offset: 0x00000B13
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000292C File Offset: 0x00000B2C
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string property = (string)key;
			JsonData data = this.ToJsonData(value);
			this[property] = data;
			KeyValuePair<string, JsonData> entry = new KeyValuePair<string, JsonData>(property, data);
			this.object_list.Insert(idx, entry);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002968 File Offset: 0x00000B68
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000029A8 File Offset: 0x00000BA8
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000029E0 File Offset: 0x00000BE0
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002A40 File Offset: 0x00000C40
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002A92 File Offset: 0x00000C92
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002AB0 File Offset: 0x00000CB0
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				foreach (object obj2 in obj)
				{
					JsonData.WriteJson((JsonData)obj2, writer);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				foreach (object obj3 in obj)
				{
					DictionaryEntry entry = (DictionaryEntry)obj3;
					writer.WritePropertyName((string)entry.Key);
					JsonData.WriteJson((JsonData)entry.Value, writer);
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002C00 File Offset: 0x00000E00
		public int Add(object value)
		{
			JsonData data = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(data);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002C28 File Offset: 0x00000E28
		public bool Remove(object obj)
		{
			this.json = null;
			if (this.IsObject)
			{
				JsonData value = null;
				if (this.inst_object.TryGetValue((string)obj, out value))
				{
					return this.inst_object.Remove((string)obj) && this.object_list.Remove(new KeyValuePair<string, JsonData>((string)obj, value));
				}
				throw new KeyNotFoundException("The specified key was not found in the JsonData object.");
			}
			else
			{
				if (this.IsArray)
				{
					return this.inst_array.Remove(this.ToJsonData(obj));
				}
				throw new InvalidOperationException("Instance of JsonData is not an object or a list.");
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002CD8 File Offset: 0x00000ED8
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type && ((x.type != JsonType.Int && x.type != JsonType.Long) || (this.type != JsonType.Int && this.type != JsonType.Long)))
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				if (x.IsLong)
				{
					return x.inst_long >= -2147483648L && x.inst_long <= 2147483647L && this.inst_int.Equals((int)x.inst_long);
				}
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				if (x.IsInt)
				{
					return this.inst_long >= -2147483648L && this.inst_long <= 2147483647L && x.inst_int.Equals((int)this.inst_long);
				}
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002E43 File Offset: 0x00001043
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002E4C File Offset: 0x0000104C
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002EEC File Offset: 0x000010EC
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter sw = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(sw)
			{
				Validate = false
			});
			this.json = sw.ToString();
			return this.json;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002F38 File Offset: 0x00001138
		public void ToJson(JsonWriter writer)
		{
			bool old_validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = old_validate;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002F64 File Offset: 0x00001164
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x0400000A RID: 10
		private IList<JsonData> inst_array;

		// Token: 0x0400000B RID: 11
		private bool inst_boolean;

		// Token: 0x0400000C RID: 12
		private double inst_double;

		// Token: 0x0400000D RID: 13
		private int inst_int;

		// Token: 0x0400000E RID: 14
		private long inst_long;

		// Token: 0x0400000F RID: 15
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x04000010 RID: 16
		private string inst_string;

		// Token: 0x04000011 RID: 17
		private string json;

		// Token: 0x04000012 RID: 18
		private JsonType type;

		// Token: 0x04000013 RID: 19
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
