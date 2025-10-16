using System;
using System.Diagnostics.Tracing;

namespace System.Buffers
{
	// Token: 0x020000E3 RID: 227
	[EventSource(Name = "System.Buffers.ArrayPoolEventSource")]
	internal sealed class ArrayPoolEventSource : EventSource
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x0003079C File Offset: 0x0002E99C
		[Event(1, Level = EventLevel.Verbose)]
		internal unsafe void BufferRented(int bufferId, int bufferSize, int poolId, int bucketId)
		{
			EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)4) * (UIntPtr)sizeof(EventSource.EventData))];
			*ptr = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&bufferId))
			};
			ptr[1] = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&bufferSize))
			};
			ptr[2] = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&poolId))
			};
			ptr[3] = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&bucketId))
			};
			base.WriteEventCore(1, 4, ptr);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00030874 File Offset: 0x0002EA74
		[Event(2, Level = EventLevel.Informational)]
		internal unsafe void BufferAllocated(int bufferId, int bufferSize, int poolId, int bucketId, ArrayPoolEventSource.BufferAllocatedReason reason)
		{
			EventSource.EventData* ptr = stackalloc EventSource.EventData[checked(unchecked((UIntPtr)5) * (UIntPtr)sizeof(EventSource.EventData))];
			*ptr = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&bufferId))
			};
			ptr[1] = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&bufferSize))
			};
			ptr[2] = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&poolId))
			};
			ptr[3] = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&bucketId))
			};
			ptr[4] = new EventSource.EventData
			{
				Size = 4,
				DataPointer = (IntPtr)((void*)(&reason))
			};
			base.WriteEventCore(2, 5, ptr);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00030979 File Offset: 0x0002EB79
		[Event(3, Level = EventLevel.Verbose)]
		internal void BufferReturned(int bufferId, int bufferSize, int poolId)
		{
			base.WriteEvent(3, bufferId, bufferSize, poolId);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0003098D File Offset: 0x0002EB8D
		// Note: this type is marked as 'beforefieldinit'.
		static ArrayPoolEventSource()
		{
			ArrayPoolEventSource.Log = new ArrayPoolEventSource();
		}

		// Token: 0x0400027F RID: 639
		internal static readonly ArrayPoolEventSource Log;

		// Token: 0x020000E4 RID: 228
		internal enum BufferAllocatedReason
		{
			// Token: 0x04000281 RID: 641
			Pooled,
			// Token: 0x04000282 RID: 642
			OverMaximumSize,
			// Token: 0x04000283 RID: 643
			PoolExhausted
		}
	}
}
