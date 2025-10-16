using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000010 RID: 16
	public class JsonMockWrapper : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004830 File Offset: 0x00002A30
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004833 File Offset: 0x00002A33
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004836 File Offset: 0x00002A36
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00004839 File Offset: 0x00002A39
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000483C File Offset: 0x00002A3C
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000483F File Offset: 0x00002A3F
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004842 File Offset: 0x00002A42
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004845 File Offset: 0x00002A45
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004848 File Offset: 0x00002A48
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004853 File Offset: 0x00002A53
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004856 File Offset: 0x00002A56
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004859 File Offset: 0x00002A59
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000485D File Offset: 0x00002A5D
		public string GetString()
		{
			return "";
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004864 File Offset: 0x00002A64
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004866 File Offset: 0x00002A66
		public void SetDouble(double val)
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004868 File Offset: 0x00002A68
		public void SetInt(int val)
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000486A File Offset: 0x00002A6A
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000486C File Offset: 0x00002A6C
		public void SetLong(long val)
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000486E File Offset: 0x00002A6E
		public void SetString(string val)
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004870 File Offset: 0x00002A70
		public string ToJson()
		{
			return "";
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004877 File Offset: 0x00002A77
		public void ToJson(JsonWriter writer)
		{
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004879 File Offset: 0x00002A79
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000487C File Offset: 0x00002A7C
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000039 RID: 57
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004884 File Offset: 0x00002A84
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004887 File Offset: 0x00002A87
		void IList.Clear()
		{
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004889 File Offset: 0x00002A89
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000488C File Offset: 0x00002A8C
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000488F File Offset: 0x00002A8F
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004891 File Offset: 0x00002A91
		void IList.Remove(object value)
		{
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004893 File Offset: 0x00002A93
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004895 File Offset: 0x00002A95
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00004898 File Offset: 0x00002A98
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000489B File Offset: 0x00002A9B
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000489E File Offset: 0x00002A9E
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000048A0 File Offset: 0x00002AA0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000048A3 File Offset: 0x00002AA3
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000048A6 File Offset: 0x00002AA6
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000048A9 File Offset: 0x00002AA9
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000048AC File Offset: 0x00002AAC
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000041 RID: 65
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000048B4 File Offset: 0x00002AB4
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000048B6 File Offset: 0x00002AB6
		void IDictionary.Clear()
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000048B8 File Offset: 0x00002AB8
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000048BB File Offset: 0x00002ABB
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000048BD File Offset: 0x00002ABD
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x17000042 RID: 66
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000048C5 File Offset: 0x00002AC5
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000048C8 File Offset: 0x00002AC8
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000048CA File Offset: 0x00002ACA
		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
