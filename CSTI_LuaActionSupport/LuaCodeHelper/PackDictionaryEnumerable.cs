using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NLua;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x0200001D RID: 29
	[NullableContext(1)]
	[Nullable(0)]
	public class PackDictionaryEnumerable : IEnumerable<KeyValuePair<object, object>>, IEnumerable
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00004A87 File Offset: 0x00002C87
		public PackDictionaryEnumerable(LuaTable table)
		{
			this.Table = table;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004A96 File Offset: 0x00002C96
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
		{
			return new PackDictionaryEnumerable.PackDictionaryEnumerator(this.Table.GetEnumerator());
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004AA8 File Offset: 0x00002CA8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400003F RID: 63
		public readonly LuaTable Table;

		// Token: 0x0200001E RID: 30
		[Nullable(0)]
		public class PackDictionaryEnumerator : IEnumerator<KeyValuePair<object, object>>, IDisposable, IEnumerator
		{
			// Token: 0x060000A7 RID: 167 RVA: 0x00004AB0 File Offset: 0x00002CB0
			public PackDictionaryEnumerator(IDictionaryEnumerator dictionaryEnumerator)
			{
				this.DictionaryEnumerator = dictionaryEnumerator;
			}

			// Token: 0x060000A8 RID: 168 RVA: 0x00004ABF File Offset: 0x00002CBF
			public bool MoveNext()
			{
				return this.DictionaryEnumerator.MoveNext();
			}

			// Token: 0x060000A9 RID: 169 RVA: 0x00004ACC File Offset: 0x00002CCC
			public void Reset()
			{
				this.DictionaryEnumerator.Reset();
			}

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000AA RID: 170 RVA: 0x00004AD9 File Offset: 0x00002CD9
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000AB RID: 171 RVA: 0x00004AE6 File Offset: 0x00002CE6
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public KeyValuePair<object, object> Current
			{
				[return: Nullable(new byte[]
				{
					0,
					1,
					1
				})]
				get
				{
					return new KeyValuePair<object, object>(this.DictionaryEnumerator.Key, this.DictionaryEnumerator.Value);
				}
			}

			// Token: 0x060000AC RID: 172 RVA: 0x0000420E File Offset: 0x0000240E
			public void Dispose()
			{
			}

			// Token: 0x04000040 RID: 64
			public readonly IDictionaryEnumerator DictionaryEnumerator;
		}
	}
}
