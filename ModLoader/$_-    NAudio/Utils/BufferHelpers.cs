using System;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NAudio.Utils
{
	// Token: 0x020000CC RID: 204
	internal static class BufferHelpers
	{
		// Token: 0x060004BF RID: 1215 RVA: 0x0001645D File Offset: 0x0001465D
		public static byte[] Ensure(byte[] buffer, int bytesRequired)
		{
			if (buffer == null || buffer.Length < bytesRequired)
			{
				buffer = new byte[bytesRequired];
			}
			return buffer;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00016471 File Offset: 0x00014671
		public static float[] Ensure(float[] buffer, int samplesRequired)
		{
			if (buffer == null || buffer.Length < samplesRequired)
			{
				buffer = new float[samplesRequired];
			}
			return buffer;
		}
	}
}
