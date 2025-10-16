using System;
using System.ComponentModel;

namespace Ionic
{
	// Token: 0x02000004 RID: 4
	internal enum ComparisonOperator
	{
		// Token: 0x0400000B RID: 11
		[Description(">")]
		GreaterThan,
		// Token: 0x0400000C RID: 12
		[Description(">=")]
		GreaterThanOrEqualTo,
		// Token: 0x0400000D RID: 13
		[Description("<")]
		LesserThan,
		// Token: 0x0400000E RID: 14
		[Description("<=")]
		LesserThanOrEqualTo,
		// Token: 0x0400000F RID: 15
		[Description("=")]
		EqualTo,
		// Token: 0x04000010 RID: 16
		[Description("!=")]
		NotEqualTo
	}
}
