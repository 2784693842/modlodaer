using System;
using System.Collections.Generic;

namespace NLua.Extensions
{
	// Token: 0x0200009A RID: 154
	internal static class StringExtensions
	{
		// Token: 0x0600048A RID: 1162 RVA: 0x000124D9 File Offset: 0x000106D9
		public static IEnumerable<string> SplitWithEscape(this string input, char separator, char escapeCharacter)
		{
			int num = 0;
			int index = 0;
			while (index < input.Length)
			{
				index = input.IndexOf(separator, index);
				if (index == -1)
				{
					break;
				}
				if (input[index - 1] == escapeCharacter)
				{
					input = input.Remove(index - 1, 1);
				}
				else
				{
					yield return input.Substring(num, index - num);
					int num2 = index;
					index = num2 + 1;
					num = index;
				}
			}
			yield return input.Substring(num);
			yield break;
		}
	}
}
