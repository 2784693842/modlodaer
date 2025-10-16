using System;
using System.Reflection;

namespace NLua.Method
{
	// Token: 0x0200007C RID: 124
	internal class RegisterEventHandler
	{
		// Token: 0x0600038E RID: 910 RVA: 0x0000E18F File Offset: 0x0000C38F
		public RegisterEventHandler(EventHandlerContainer pendingEvents, object target, EventInfo eventInfo)
		{
			this._target = target;
			this._eventInfo = eventInfo;
			this._pendingEvents = pendingEvents;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000E1AC File Offset: 0x0000C3AC
		public Delegate Add(LuaFunction function)
		{
			Delegate @delegate = CodeGeneration.Instance.GetDelegate(this._eventInfo.EventHandlerType, function);
			return this.Add(@delegate);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000E1D7 File Offset: 0x0000C3D7
		public Delegate Add(Delegate handlerDelegate)
		{
			this._eventInfo.AddEventHandler(this._target, handlerDelegate);
			this._pendingEvents.Add(handlerDelegate, this);
			return handlerDelegate;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000E1F9 File Offset: 0x0000C3F9
		public void Remove(Delegate handlerDelegate)
		{
			this.RemovePending(handlerDelegate);
			this._pendingEvents.Remove(handlerDelegate);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000E20E File Offset: 0x0000C40E
		internal void RemovePending(Delegate handlerDelegate)
		{
			this._eventInfo.RemoveEventHandler(this._target, handlerDelegate);
		}

		// Token: 0x04000160 RID: 352
		private readonly EventHandlerContainer _pendingEvents;

		// Token: 0x04000161 RID: 353
		private readonly EventInfo _eventInfo;

		// Token: 0x04000162 RID: 354
		private readonly object _target;
	}
}
