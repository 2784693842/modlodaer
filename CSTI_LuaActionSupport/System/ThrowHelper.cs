using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000BC RID: 188
	internal static class ThrowHelper
	{
		// Token: 0x060005ED RID: 1517 RVA: 0x0001627C File Offset: 0x0001447C
		internal static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw ThrowHelper.CreateArgumentNullException(argument);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00016284 File Offset: 0x00014484
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentNullException(ExceptionArgument argument)
		{
			return new ArgumentNullException(argument.ToString());
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00016298 File Offset: 0x00014498
		internal static void ThrowArrayTypeMismatchException()
		{
			throw ThrowHelper.CreateArrayTypeMismatchException();
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001629F File Offset: 0x0001449F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArrayTypeMismatchException()
		{
			return new ArrayTypeMismatchException();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000162A6 File Offset: 0x000144A6
		internal static void ThrowArgumentException_InvalidTypeWithPointersNotSupported(Type type)
		{
			throw ThrowHelper.CreateArgumentException_InvalidTypeWithPointersNotSupported(type);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000162AE File Offset: 0x000144AE
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentException_InvalidTypeWithPointersNotSupported(Type type)
		{
			return new ArgumentException(SR.Format(SR.Argument_InvalidTypeWithPointersNotSupported, type));
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000162C0 File Offset: 0x000144C0
		internal static void ThrowArgumentException_DestinationTooShort()
		{
			throw ThrowHelper.CreateArgumentException_DestinationTooShort();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000162C7 File Offset: 0x000144C7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentException_DestinationTooShort()
		{
			return new ArgumentException(SR.Argument_DestinationTooShort);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000162D3 File Offset: 0x000144D3
		internal static void ThrowIndexOutOfRangeException()
		{
			throw ThrowHelper.CreateIndexOutOfRangeException();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000162DA File Offset: 0x000144DA
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateIndexOutOfRangeException()
		{
			return new IndexOutOfRangeException();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000162E1 File Offset: 0x000144E1
		internal static void ThrowArgumentOutOfRangeException()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException();
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000162E8 File Offset: 0x000144E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException()
		{
			return new ArgumentOutOfRangeException();
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000162EF File Offset: 0x000144EF
		internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException(argument);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000162F7 File Offset: 0x000144F7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException(ExceptionArgument argument)
		{
			return new ArgumentOutOfRangeException(argument.ToString());
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0001630B File Offset: 0x0001450B
		internal static void ThrowArgumentOutOfRangeException_PrecisionTooLarge()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_PrecisionTooLarge();
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00016312 File Offset: 0x00014512
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_PrecisionTooLarge()
		{
			return new ArgumentOutOfRangeException("precision", SR.Format(SR.Argument_PrecisionTooLarge, 99));
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001632F File Offset: 0x0001452F
		internal static void ThrowArgumentOutOfRangeException_SymbolDoesNotFit()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_SymbolDoesNotFit();
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00016336 File Offset: 0x00014536
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_SymbolDoesNotFit()
		{
			return new ArgumentOutOfRangeException("symbol", SR.Argument_BadFormatSpecifier);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00016347 File Offset: 0x00014547
		internal static void ThrowInvalidOperationException()
		{
			throw ThrowHelper.CreateInvalidOperationException();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001634E File Offset: 0x0001454E
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException()
		{
			return new InvalidOperationException();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00016355 File Offset: 0x00014555
		internal static void ThrowInvalidOperationException_OutstandingReferences()
		{
			throw ThrowHelper.CreateInvalidOperationException_OutstandingReferences();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001635C File Offset: 0x0001455C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException_OutstandingReferences()
		{
			return new InvalidOperationException(SR.OutstandingReferences);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00016368 File Offset: 0x00014568
		internal static void ThrowInvalidOperationException_UnexpectedSegmentType()
		{
			throw ThrowHelper.CreateInvalidOperationException_UnexpectedSegmentType();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001636F File Offset: 0x0001456F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException_UnexpectedSegmentType()
		{
			return new InvalidOperationException(SR.UnexpectedSegmentType);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001637B File Offset: 0x0001457B
		internal static void ThrowInvalidOperationException_EndPositionNotReached()
		{
			throw ThrowHelper.CreateInvalidOperationException_EndPositionNotReached();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00016382 File Offset: 0x00014582
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateInvalidOperationException_EndPositionNotReached()
		{
			return new InvalidOperationException(SR.EndPositionNotReached);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001638E File Offset: 0x0001458E
		internal static void ThrowArgumentOutOfRangeException_PositionOutOfRange()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_PositionOutOfRange();
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00016395 File Offset: 0x00014595
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_PositionOutOfRange()
		{
			return new ArgumentOutOfRangeException("position");
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000163A1 File Offset: 0x000145A1
		internal static void ThrowArgumentOutOfRangeException_OffsetOutOfRange()
		{
			throw ThrowHelper.CreateArgumentOutOfRangeException_OffsetOutOfRange();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000163A8 File Offset: 0x000145A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentOutOfRangeException_OffsetOutOfRange()
		{
			return new ArgumentOutOfRangeException("offset");
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x000163B4 File Offset: 0x000145B4
		internal static void ThrowObjectDisposedException_ArrayMemoryPoolBuffer()
		{
			throw ThrowHelper.CreateObjectDisposedException_ArrayMemoryPoolBuffer();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000163BB File Offset: 0x000145BB
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateObjectDisposedException_ArrayMemoryPoolBuffer()
		{
			return new ObjectDisposedException("ArrayMemoryPoolBuffer");
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x000163C7 File Offset: 0x000145C7
		internal static void ThrowFormatException_BadFormatSpecifier()
		{
			throw ThrowHelper.CreateFormatException_BadFormatSpecifier();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x000163CE File Offset: 0x000145CE
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateFormatException_BadFormatSpecifier()
		{
			return new FormatException(SR.Argument_BadFormatSpecifier);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000163DA File Offset: 0x000145DA
		internal static void ThrowArgumentException_OverlapAlignmentMismatch()
		{
			throw ThrowHelper.CreateArgumentException_OverlapAlignmentMismatch();
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000163E1 File Offset: 0x000145E1
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateArgumentException_OverlapAlignmentMismatch()
		{
			return new ArgumentException(SR.Argument_OverlapAlignmentMismatch);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000163ED File Offset: 0x000145ED
		internal static void ThrowNotSupportedException()
		{
			throw ThrowHelper.CreateThrowNotSupportedException();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000163F4 File Offset: 0x000145F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Exception CreateThrowNotSupportedException()
		{
			return new NotSupportedException();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000163FB File Offset: 0x000145FB
		public static bool TryFormatThrowFormatException(out int bytesWritten)
		{
			bytesWritten = 0;
			ThrowHelper.ThrowFormatException_BadFormatSpecifier();
			return false;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00016406 File Offset: 0x00014606
		public static bool TryParseThrowFormatException<T>(out T value, out int bytesConsumed)
		{
			value = default(T);
			bytesConsumed = 0;
			ThrowHelper.ThrowFormatException_BadFormatSpecifier();
			return false;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00016418 File Offset: 0x00014618
		public static void ThrowArgumentValidationException<T>(ReadOnlySequenceSegment<T> startSegment, int startIndex, ReadOnlySequenceSegment<T> endSegment)
		{
			throw ThrowHelper.CreateArgumentValidationException<T>(startSegment, startIndex, endSegment);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00016424 File Offset: 0x00014624
		private static Exception CreateArgumentValidationException<T>(ReadOnlySequenceSegment<T> startSegment, int startIndex, ReadOnlySequenceSegment<T> endSegment)
		{
			if (startSegment == null)
			{
				return ThrowHelper.CreateArgumentNullException(ExceptionArgument.startSegment);
			}
			if (endSegment == null)
			{
				return ThrowHelper.CreateArgumentNullException(ExceptionArgument.endSegment);
			}
			if (startSegment != endSegment && startSegment.RunningIndex > endSegment.RunningIndex)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.endSegment);
			}
			if (startSegment.Memory.Length < startIndex)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.startIndex);
			}
			return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.endIndex);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00016480 File Offset: 0x00014680
		public static void ThrowArgumentValidationException(Array array, int start)
		{
			throw ThrowHelper.CreateArgumentValidationException(array, start);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00016489 File Offset: 0x00014689
		private static Exception CreateArgumentValidationException(Array array, int start)
		{
			if (array == null)
			{
				return ThrowHelper.CreateArgumentNullException(ExceptionArgument.array);
			}
			if (start > array.Length)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.length);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000164AC File Offset: 0x000146AC
		public static void ThrowStartOrEndArgumentValidationException(long start)
		{
			throw ThrowHelper.CreateStartOrEndArgumentValidationException(start);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000164B4 File Offset: 0x000146B4
		private static Exception CreateStartOrEndArgumentValidationException(long start)
		{
			if (start < 0L)
			{
				return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.start);
			}
			return ThrowHelper.CreateArgumentOutOfRangeException(ExceptionArgument.length);
		}
	}
}
