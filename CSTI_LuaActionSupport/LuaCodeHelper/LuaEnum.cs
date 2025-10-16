using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CSTI_LuaActionSupport.AllPatcher;
using CSTI_LuaActionSupport.Helper;
using NLua;

namespace CSTI_LuaActionSupport.LuaCodeHelper
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	[Nullable(0)]
	public class LuaEnum
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004F23 File Offset: 0x00003123
		private static LuaFunction iter_L
		{
			get
			{
				LuaFunction result;
				if ((result = LuaEnum._iter_L) == null)
				{
					result = (LuaEnum._iter_L = CardActionPatcher.LuaRuntime.RegisterFunction("__temp", new LuaEnum.LuaIter<LuaEnum.MyListEnumerator, object, object>(LuaEnum.<get_iter_L>g___iter|5_0).Method));
				}
				return result;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00004F54 File Offset: 0x00003154
		private static LuaFunction iter_D
		{
			get
			{
				LuaFunction result;
				if ((result = LuaEnum._iter_D) == null)
				{
					result = (LuaEnum._iter_D = CardActionPatcher.LuaRuntime.RegisterFunction("__temp", new LuaEnum.LuaIter<IDictionaryEnumerator, object, object>(LuaEnum.<get_iter_D>g___iter|8_0).Method));
				}
				return result;
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004F85 File Offset: 0x00003185
		[TestCode("for i,v in Enum:Pairs(GetGameCards(\"6b87970979841684bb6d6a7471430798\")) do\r\n  debug.debug = v.Id\r\nend")]
		public void Pairs(IList list, out LuaFunction func, out LuaEnum.MyListEnumerator pack, [Nullable(2)] out object stat)
		{
			func = LuaEnum.iter_L;
			pack = new LuaEnum.MyListEnumerator(list);
			stat = null;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004F9A File Offset: 0x0000319A
		public void Pairs(IDictionary dictionary, out LuaFunction iter, out IDictionaryEnumerator dict, [Nullable(2)] out object stat)
		{
			iter = LuaEnum.iter_D;
			dict = dictionary.GetEnumerator();
			stat = null;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004FB0 File Offset: 0x000031B0
		public void Foreach(IEnumerable enumerable, LuaFunction func)
		{
			foreach (object obj in enumerable)
			{
				func.Call(new object[]
				{
					obj
				});
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000500C File Offset: 0x0000320C
		public double Sum(IEnumerable enumerable, double init, LuaFunction func)
		{
			foreach (object obj in enumerable)
			{
				object[] array = func.Call(new object[]
				{
					init,
					obj
				});
				if (array.Length != 0)
				{
					double? num = array[0].TryNum<double>();
					if (num != null)
					{
						double valueOrDefault = num.GetValueOrDefault();
						init = valueOrDefault;
					}
				}
			}
			return init;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005094 File Offset: 0x00003294
		public IList Map(IEnumerable enumerable, LuaFunction func)
		{
			List<object> list = new List<object>();
			foreach (object obj in enumerable)
			{
				object[] array = func.Call(new object[]
				{
					obj
				});
				if (array.Length != 0)
				{
					list.Add(array[0]);
				}
			}
			return list;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00002608 File Offset: 0x00000808
		private LuaEnum()
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005110 File Offset: 0x00003310
		[NullableContext(2)]
		[CompilerGenerated]
		internal static object <get_iter_L>g___iter|5_0([Nullable(1)] LuaEnum.MyListEnumerator l, object index, out object item)
		{
			if (l.MoveNext())
			{
				item = l.Current;
				return l.Index;
			}
			item = null;
			return null;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005132 File Offset: 0x00003332
		[NullableContext(2)]
		[CompilerGenerated]
		internal static object <get_iter_D>g___iter|8_0([Nullable(1)] IDictionaryEnumerator dictionaryEnumerator, object key, out object value)
		{
			if (dictionaryEnumerator.MoveNext())
			{
				value = dictionaryEnumerator.Value;
				return dictionaryEnumerator.Key;
			}
			value = null;
			return null;
		}

		// Token: 0x04000052 RID: 82
		public static readonly LuaEnum Enum = new LuaEnum();

		// Token: 0x04000053 RID: 83
		[Nullable(2)]
		private static LuaFunction _iter_L;

		// Token: 0x04000054 RID: 84
		[Nullable(2)]
		private static LuaFunction _iter_D;

		// Token: 0x02000029 RID: 41
		[NullableContext(0)]
		public class MyListEnumerator : IEnumerator
		{
			// Token: 0x060000D9 RID: 217 RVA: 0x0000514F File Offset: 0x0000334F
			[NullableContext(1)]
			public MyListEnumerator(IList list)
			{
				this._list = list;
				this.Index = -1;
			}

			// Token: 0x060000DA RID: 218 RVA: 0x00005165 File Offset: 0x00003365
			public bool MoveNext()
			{
				this.Index++;
				return this.Index >= 0 && this.Index < this._list.Count;
			}

			// Token: 0x060000DB RID: 219 RVA: 0x00005193 File Offset: 0x00003393
			public void Reset()
			{
				this.Index = -1;
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x060000DC RID: 220 RVA: 0x0000519C File Offset: 0x0000339C
			// (set) Token: 0x060000DD RID: 221 RVA: 0x000051A4 File Offset: 0x000033A4
			public int Index { get; private set; }

			// Token: 0x1700002E RID: 46
			// (get) Token: 0x060000DE RID: 222 RVA: 0x000051AD File Offset: 0x000033AD
			[Nullable(2)]
			public object Current
			{
				[NullableContext(2)]
				get
				{
					if (this.Index >= 0 && this.Index < this._list.Count)
					{
						return this._list[this.Index];
					}
					return null;
				}
			}

			// Token: 0x04000055 RID: 85
			[Nullable(1)]
			private readonly IList _list;
		}

		// Token: 0x0200002A RID: 42
		// (Invoke) Token: 0x060000E0 RID: 224
		[NullableContext(0)]
		public delegate TStat LuaIter<[Nullable(2)] in TConst, [Nullable(2)] TStat, [Nullable(2)] TItem>(TConst constStat, TStat stat, out TItem item);
	}
}
