using System;
using System.Diagnostics;

namespace System
{
	// Token: 0x020000AF RID: 175
	internal sealed class SpanDebugView<T>
	{
		// Token: 0x06000584 RID: 1412 RVA: 0x0001497D File Offset: 0x00012B7D
		public SpanDebugView(System.Span<T> span)
		{
			this._array = span.ToArray();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00014992 File Offset: 0x00012B92
		public SpanDebugView(System.ReadOnlySpan<T> span)
		{
			this._array = span.ToArray();
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x000149A7 File Offset: 0x00012BA7
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this._array;
			}
		}

		// Token: 0x040001DC RID: 476
		private readonly T[] _array;
	}
}
