using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000005 RID: 5
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002FE6 File Offset: 0x000011E6
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002FF4 File Offset: 0x000011F4
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> curr = this.list_enumerator.Current;
				return new DictionaryEntry(curr.Key, curr.Value);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003020 File Offset: 0x00001220
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003040 File Offset: 0x00001240
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003060 File Offset: 0x00001260
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000306F File Offset: 0x0000126F
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000307C File Offset: 0x0000127C
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04000014 RID: 20
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
