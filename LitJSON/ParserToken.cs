using System;

namespace LitJson
{
	// Token: 0x02000018 RID: 24
	internal enum ParserToken
	{
		// Token: 0x04000074 RID: 116
		None = 65536,
		// Token: 0x04000075 RID: 117
		Number,
		// Token: 0x04000076 RID: 118
		True,
		// Token: 0x04000077 RID: 119
		False,
		// Token: 0x04000078 RID: 120
		Null,
		// Token: 0x04000079 RID: 121
		CharSeq,
		// Token: 0x0400007A RID: 122
		Char,
		// Token: 0x0400007B RID: 123
		Text,
		// Token: 0x0400007C RID: 124
		Object,
		// Token: 0x0400007D RID: 125
		ObjectPrime,
		// Token: 0x0400007E RID: 126
		Pair,
		// Token: 0x0400007F RID: 127
		PairRest,
		// Token: 0x04000080 RID: 128
		Array,
		// Token: 0x04000081 RID: 129
		ArrayPrime,
		// Token: 0x04000082 RID: 130
		Value,
		// Token: 0x04000083 RID: 131
		ValueRest,
		// Token: 0x04000084 RID: 132
		String,
		// Token: 0x04000085 RID: 133
		End,
		// Token: 0x04000086 RID: 134
		Epsilon
	}
}
