using System;
using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.System.Memory;

namespace System
{
	// Token: 0x020000C6 RID: 198
	internal static class SR
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00017119 File Offset: 0x00015319
		private static ResourceManager ResourceManager
		{
			get
			{
				ResourceManager result;
				if ((result = System.SR.s_resourceManager) == null)
				{
					result = (System.SR.s_resourceManager = new ResourceManager(System.SR.ResourceType));
				}
				return result;
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00017134 File Offset: 0x00015334
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool UsingResourceKeys()
		{
			return false;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00017138 File Offset: 0x00015338
		internal static string GetResourceString(string resourceKey, string defaultString)
		{
			string text = null;
			try
			{
				text = System.SR.ResourceManager.GetString(resourceKey);
			}
			catch (MissingManifestResourceException)
			{
			}
			if (defaultString != null && resourceKey.Equals(text, StringComparison.Ordinal))
			{
				return defaultString;
			}
			return text;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00017178 File Offset: 0x00015378
		internal static string Format(string resourceFormat, params object[] args)
		{
			if (args == null)
			{
				return resourceFormat;
			}
			if (System.SR.UsingResourceKeys())
			{
				return resourceFormat + string.Join(", ", args);
			}
			return string.Format(resourceFormat, args);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001719F File Offset: 0x0001539F
		internal static string Format(string resourceFormat, object p1)
		{
			if (System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[]
				{
					resourceFormat,
					p1
				});
			}
			return string.Format(resourceFormat, p1);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x000171C8 File Offset: 0x000153C8
		internal static string Format(string resourceFormat, object p1, object p2)
		{
			if (System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[]
				{
					resourceFormat,
					p1,
					p2
				});
			}
			return string.Format(resourceFormat, p1, p2);
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000171F6 File Offset: 0x000153F6
		internal static string Format(string resourceFormat, object p1, object p2, object p3)
		{
			if (System.SR.UsingResourceKeys())
			{
				return string.Join(", ", new object[]
				{
					resourceFormat,
					p1,
					p2,
					p3
				});
			}
			return string.Format(resourceFormat, p1, p2, p3);
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00017229 File Offset: 0x00015429
		internal static Type ResourceType { get; } = typeof(FxResources.System.Memory.SR);

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00017230 File Offset: 0x00015430
		internal static string NotSupported_CannotCallEqualsOnSpan
		{
			get
			{
				return System.SR.GetResourceString("NotSupported_CannotCallEqualsOnSpan", null);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0001723D File Offset: 0x0001543D
		internal static string NotSupported_CannotCallGetHashCodeOnSpan
		{
			get
			{
				return System.SR.GetResourceString("NotSupported_CannotCallGetHashCodeOnSpan", null);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001724A File Offset: 0x0001544A
		internal static string Argument_InvalidTypeWithPointersNotSupported
		{
			get
			{
				return System.SR.GetResourceString("Argument_InvalidTypeWithPointersNotSupported", null);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00017257 File Offset: 0x00015457
		internal static string Argument_DestinationTooShort
		{
			get
			{
				return System.SR.GetResourceString("Argument_DestinationTooShort", null);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x00017264 File Offset: 0x00015464
		internal static string MemoryDisposed
		{
			get
			{
				return System.SR.GetResourceString("MemoryDisposed", null);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x00017271 File Offset: 0x00015471
		internal static string OutstandingReferences
		{
			get
			{
				return System.SR.GetResourceString("OutstandingReferences", null);
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001727E File Offset: 0x0001547E
		internal static string Argument_BadFormatSpecifier
		{
			get
			{
				return System.SR.GetResourceString("Argument_BadFormatSpecifier", null);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001728B File Offset: 0x0001548B
		internal static string Argument_GWithPrecisionNotSupported
		{
			get
			{
				return System.SR.GetResourceString("Argument_GWithPrecisionNotSupported", null);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x00017298 File Offset: 0x00015498
		internal static string Argument_CannotParsePrecision
		{
			get
			{
				return System.SR.GetResourceString("Argument_CannotParsePrecision", null);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x000172A5 File Offset: 0x000154A5
		internal static string Argument_PrecisionTooLarge
		{
			get
			{
				return System.SR.GetResourceString("Argument_PrecisionTooLarge", null);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x000172B2 File Offset: 0x000154B2
		internal static string Argument_OverlapAlignmentMismatch
		{
			get
			{
				return System.SR.GetResourceString("Argument_OverlapAlignmentMismatch", null);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x000172BF File Offset: 0x000154BF
		internal static string EndPositionNotReached
		{
			get
			{
				return System.SR.GetResourceString("EndPositionNotReached", null);
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x000172CC File Offset: 0x000154CC
		internal static string UnexpectedSegmentType
		{
			get
			{
				return System.SR.GetResourceString("UnexpectedSegmentType", null);
			}
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000172EA File Offset: 0x000154EA
		internal static string get_Arg_ArgumentOutOfRangeException()
		{
			return System.SR.GetResourceString("Arg_ArgumentOutOfRangeException", null);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x000172F7 File Offset: 0x000154F7
		internal static string get_Arg_ElementsInSourceIsGreaterThanDestination()
		{
			return System.SR.GetResourceString("Arg_ElementsInSourceIsGreaterThanDestination", null);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00017304 File Offset: 0x00015504
		internal static string get_Arg_NullArgumentNullRef()
		{
			return System.SR.GetResourceString("Arg_NullArgumentNullRef", null);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00017311 File Offset: 0x00015511
		internal static string get_Arg_TypeNotSupported()
		{
			return System.SR.GetResourceString("Arg_TypeNotSupported", null);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001731E File Offset: 0x0001551E
		internal static string get_Arg_InsufficientNumberOfElements()
		{
			return System.SR.GetResourceString("Arg_InsufficientNumberOfElements", null);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001732B File Offset: 0x0001552B
		internal static string get_ArgumentException_BufferNotFromPool()
		{
			return System.SR.GetResourceString("ArgumentException_BufferNotFromPool", null);
		}

		// Token: 0x04000215 RID: 533
		private static ResourceManager s_resourceManager;
	}
}
