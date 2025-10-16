using System;

namespace LitJson
{
	// Token: 0x02000011 RID: 17
	public enum JsonToken
	{
		// Token: 0x0400002F RID: 47
		None,
		// Token: 0x04000030 RID: 48
		ObjectStart,
		// Token: 0x04000031 RID: 49
		PropertyName,
		// Token: 0x04000032 RID: 50
		ObjectEnd,
		// Token: 0x04000033 RID: 51
		ArrayStart,
		// Token: 0x04000034 RID: 52
		ArrayEnd,
		// Token: 0x04000035 RID: 53
		Int,
		// Token: 0x04000036 RID: 54
		Long,
		// Token: 0x04000037 RID: 55
		Double,
		// Token: 0x04000038 RID: 56
		String,
		// Token: 0x04000039 RID: 57
		Boolean,
		// Token: 0x0400003A RID: 58
		Null
	}
}
