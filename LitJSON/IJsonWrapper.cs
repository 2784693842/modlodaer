using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000003 RID: 3
	public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		bool IsArray { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		bool IsBoolean { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3
		bool IsDouble { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4
		bool IsInt { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000005 RID: 5
		bool IsLong { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000006 RID: 6
		bool IsObject { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000007 RID: 7
		bool IsString { get; }

		// Token: 0x06000008 RID: 8
		bool GetBoolean();

		// Token: 0x06000009 RID: 9
		double GetDouble();

		// Token: 0x0600000A RID: 10
		int GetInt();

		// Token: 0x0600000B RID: 11
		JsonType GetJsonType();

		// Token: 0x0600000C RID: 12
		long GetLong();

		// Token: 0x0600000D RID: 13
		string GetString();

		// Token: 0x0600000E RID: 14
		void SetBoolean(bool val);

		// Token: 0x0600000F RID: 15
		void SetDouble(double val);

		// Token: 0x06000010 RID: 16
		void SetInt(int val);

		// Token: 0x06000011 RID: 17
		void SetJsonType(JsonType type);

		// Token: 0x06000012 RID: 18
		void SetLong(long val);

		// Token: 0x06000013 RID: 19
		void SetString(string val);

		// Token: 0x06000014 RID: 20
		string ToJson();

		// Token: 0x06000015 RID: 21
		void ToJson(JsonWriter writer);
	}
}
