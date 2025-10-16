using System;
using System.ComponentModel;

namespace Ionic
{
	// Token: 0x0200000D RID: 13
	internal sealed class EnumUtil
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00003990 File Offset: 0x00001B90
		private EnumUtil()
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003998 File Offset: 0x00001B98
		internal static string GetDescription(Enum value)
		{
			DescriptionAttribute[] array = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (array.Length != 0)
			{
				return array[0].Description;
			}
			return value.ToString();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000039DF File Offset: 0x00001BDF
		internal static object Parse(Type enumType, string stringRepresentation)
		{
			return EnumUtil.Parse(enumType, stringRepresentation, false);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000039EC File Offset: 0x00001BEC
		internal static object Parse(Type enumType, string stringRepresentation, bool ignoreCase)
		{
			if (ignoreCase)
			{
				stringRepresentation = stringRepresentation.ToLower();
			}
			foreach (object obj in Enum.GetValues(enumType))
			{
				Enum @enum = (Enum)obj;
				string text = EnumUtil.GetDescription(@enum);
				if (ignoreCase)
				{
					text = text.ToLower();
				}
				if (text == stringRepresentation)
				{
					return @enum;
				}
			}
			return Enum.Parse(enumType, stringRepresentation, ignoreCase);
		}
	}
}
