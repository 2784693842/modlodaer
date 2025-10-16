using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Resources;
using System.Runtime.CompilerServices;

namespace gfoidl.Base64
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ThrowHelper
	{
		// Token: 0x060005DD RID: 1501 RVA: 0x00016150 File Offset: 0x00014350
		static ThrowHelper()
		{
			string ns = typeof(ThrowHelper).Namespace;
			ThrowHelper.s_resources = new Lazy<ResourceManager>(() => new ResourceManager(ns + ".Strings", typeof(ThrowHelper).Assembly));
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00016181 File Offset: 0x00014381
		[DoesNotReturn]
		public static void ThrowArgumentNullException(ExceptionArgument argument)
		{
			throw ThrowHelper.GetArgumentNullException(argument);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00016189 File Offset: 0x00014389
		[DoesNotReturn]
		public static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
		{
			throw ThrowHelper.GetArgumentOutOfRangeException(argument);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00016191 File Offset: 0x00014391
		[DoesNotReturn]
		public static void ThrowMalformedInputException(int urlEncodedLen)
		{
			throw ThrowHelper.GetMalformdedInputException(urlEncodedLen);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00016199 File Offset: 0x00014399
		[DoesNotReturn]
		public static void ThrowForOperationNotDone(OperationStatus status)
		{
			throw ThrowHelper.GetExceptionForOperationNotDone(status);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000161A1 File Offset: 0x000143A1
		[DoesNotReturn]
		public static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionRessource ressource)
		{
			throw ThrowHelper.GetArgumentOutOfRangeException(argument, ressource);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000161AA File Offset: 0x000143AA
		private static Exception GetArgumentNullException(ExceptionArgument argument)
		{
			return new ArgumentNullException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000161B7 File Offset: 0x000143B7
		private static Exception GetArgumentOutOfRangeException(ExceptionArgument argument)
		{
			return new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument));
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000161C4 File Offset: 0x000143C4
		private static Exception GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionRessource ressource)
		{
			return new ArgumentOutOfRangeException(ThrowHelper.GetArgumentName(argument), ThrowHelper.GetResource(ressource));
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000161D7 File Offset: 0x000143D7
		private static FormatException GetMalformdedInputException(int urlEncodedLen)
		{
			return new FormatException(string.Format(ThrowHelper.GetResource(ExceptionRessource.MalformedInput), urlEncodedLen));
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000161F0 File Offset: 0x000143F0
		private static Exception GetExceptionForOperationNotDone(OperationStatus status)
		{
			Exception result;
			if (status != OperationStatus.DestinationTooSmall)
			{
				if (status != OperationStatus.InvalidData)
				{
					throw new NotSupportedException();
				}
				result = new FormatException(ThrowHelper.GetResource(ExceptionRessource.InvalidInput));
			}
			else
			{
				result = new InvalidOperationException("should not be here");
			}
			return result;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00016229 File Offset: 0x00014429
		private static string GetArgumentName(ExceptionArgument argument)
		{
			return argument.ToString();
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00016238 File Offset: 0x00014438
		private static string GetResource(ExceptionRessource ressource)
		{
			return ThrowHelper.s_resources.Value.GetString(ressource.ToString());
		}

		// Token: 0x040001EB RID: 491
		private static readonly Lazy<ResourceManager> s_resources;
	}
}
