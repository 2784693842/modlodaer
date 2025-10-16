using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace gfoidl.Base64
{
	// Token: 0x020000A7 RID: 167
	internal interface IBase64
	{
		// Token: 0x0600053B RID: 1339
		int GetEncodedLength(int sourceLength);

		// Token: 0x0600053C RID: 1340
		int GetMaxDecodedLength(int encodedLength);

		// Token: 0x0600053D RID: 1341
		int GetDecodedLength(System.ReadOnlySpan<byte> encoded);

		// Token: 0x0600053E RID: 1342
		int GetDecodedLength(System.ReadOnlySpan<char> encoded);

		// Token: 0x0600053F RID: 1343
		OperationStatus Encode(System.ReadOnlySpan<byte> data, System.Span<byte> encoded, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x06000540 RID: 1344
		OperationStatus Encode(System.ReadOnlySpan<byte> data, System.Span<char> encoded, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x06000541 RID: 1345
		OperationStatus Decode(System.ReadOnlySpan<byte> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x06000542 RID: 1346
		OperationStatus Decode(System.ReadOnlySpan<char> encoded, System.Span<byte> data, out int consumed, out int written, bool isFinalBlock = true);

		// Token: 0x06000543 RID: 1347
		[return: Nullable(1)]
		string Encode(System.ReadOnlySpan<byte> data);

		// Token: 0x06000544 RID: 1348
		[return: Nullable(1)]
		byte[] Decode(System.ReadOnlySpan<char> encoded);
	}
}
