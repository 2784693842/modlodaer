using System;
using System.Collections.Generic;

namespace NLua.Method
{
	// Token: 0x0200007B RID: 123
	internal class EventHandlerContainer : IDisposable
	{
		// Token: 0x0600038A RID: 906 RVA: 0x0000E0F0 File Offset: 0x0000C2F0
		public void Add(Delegate handler, RegisterEventHandler eventInfo)
		{
			this._dict.Add(handler, eventInfo);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000E0FF File Offset: 0x0000C2FF
		public void Remove(Delegate handler)
		{
			this._dict.Remove(handler);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000E110 File Offset: 0x0000C310
		public void Dispose()
		{
			foreach (KeyValuePair<Delegate, RegisterEventHandler> keyValuePair in this._dict)
			{
				keyValuePair.Value.RemovePending(keyValuePair.Key);
			}
			this._dict.Clear();
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000E17C File Offset: 0x0000C37C
		public EventHandlerContainer()
		{
			this._dict = new Dictionary<Delegate, RegisterEventHandler>();
			base..ctor();
		}

		// Token: 0x0400015F RID: 351
		private readonly Dictionary<Delegate, RegisterEventHandler> _dict;
	}
}
