using System;
using System.ComponentModel;

// Token: 0x02000176 RID: 374
public static class DelimiterExtensions
{
	// Token: 0x06000A18 RID: 2584 RVA: 0x0005AA38 File Offset: 0x00058C38
	public static char ToChar(this Delimiter delimiter)
	{
		switch (delimiter)
		{
		case Delimiter.Auto:
			throw new InvalidEnumArgumentException("Could not return char of Delimiter.Auto.");
		case Delimiter.Comma:
			return ',';
		case Delimiter.Tab:
			return '\t';
		case Delimiter.Semicolon:
			return ';';
		default:
			throw new ArgumentOutOfRangeException("delimiter", delimiter, null);
		}
	}
}
