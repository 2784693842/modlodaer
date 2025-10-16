using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x0200005E RID: 94
	internal interface IPacket
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001E8 RID: 488
		bool IsResync { get; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E9 RID: 489
		bool IsShort { get; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001EA RID: 490
		long? GranulePosition { get; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001EB RID: 491
		bool IsEndOfStream { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001EC RID: 492
		int BitsRead { get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001ED RID: 493
		int BitsRemaining { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001EE RID: 494
		int ContainerOverheadBits { get; }

		// Token: 0x060001EF RID: 495
		ulong TryPeekBits(int count, out int bitsRead);

		// Token: 0x060001F0 RID: 496
		void SkipBits(int count);

		// Token: 0x060001F1 RID: 497
		ulong ReadBits(int count);

		// Token: 0x060001F2 RID: 498
		void Done();

		// Token: 0x060001F3 RID: 499
		void Reset();
	}
}
