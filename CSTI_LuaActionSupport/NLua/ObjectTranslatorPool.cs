using System;
using System.Collections.Concurrent;
using KeraLua;

namespace NLua
{
	// Token: 0x02000099 RID: 153
	internal class ObjectTranslatorPool
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00012431 File Offset: 0x00010631
		public static ObjectTranslatorPool Instance
		{
			get
			{
				return ObjectTranslatorPool._instance;
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001243A File Offset: 0x0001063A
		public void Add(Lua luaState, ObjectTranslator translator)
		{
			if (!this.translators.TryAdd(luaState, translator))
			{
				throw new ArgumentException("An item with the same key has already been added. ", "luaState");
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0001245C File Offset: 0x0001065C
		public ObjectTranslator Find(Lua luaState)
		{
			ObjectTranslator result;
			if (!this.translators.TryGetValue(luaState, out result))
			{
				Lua mainThread = luaState.MainThread;
				if (!this.translators.TryGetValue(mainThread, out result))
				{
					throw new Exception("Invalid luaState, couldn't find ObjectTranslator");
				}
			}
			return result;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001249C File Offset: 0x0001069C
		public void Remove(Lua luaState)
		{
			ObjectTranslator objectTranslator;
			this.translators.TryRemove(luaState, out objectTranslator);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000124B8 File Offset: 0x000106B8
		public ObjectTranslatorPool()
		{
			this.translators = new ConcurrentDictionary<Lua, ObjectTranslator>();
			base..ctor();
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000124CB File Offset: 0x000106CB
		// Note: this type is marked as 'beforefieldinit'.
		static ObjectTranslatorPool()
		{
			ObjectTranslatorPool._instance = new ObjectTranslatorPool();
		}

		// Token: 0x040001B1 RID: 433
		private static volatile ObjectTranslatorPool _instance;

		// Token: 0x040001B2 RID: 434
		private ConcurrentDictionary<Lua, ObjectTranslator> translators;
	}
}
