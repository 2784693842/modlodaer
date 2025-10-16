using System;
using System.Diagnostics;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000005 RID: 5
	internal abstract class SelectionCriterion
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		internal virtual bool Verbose { get; set; }

		// Token: 0x06000003 RID: 3
		internal abstract bool Evaluate(string filename);

		// Token: 0x06000004 RID: 4 RVA: 0x00002061 File Offset: 0x00000261
		[Conditional("SelectorTrace")]
		protected static void CriterionTrace(string format, params object[] args)
		{
		}

		// Token: 0x06000005 RID: 5
		internal abstract bool Evaluate(ZipEntry entry);
	}
}
