using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// Token: 0x02000177 RID: 375
public static class CSVParser
{
	// Token: 0x06000A19 RID: 2585 RVA: 0x0005AA77 File Offset: 0x00058C77
	public static Dictionary<string, List<string>> LoadFromPath(string path, Delimiter delimiter = Delimiter.Auto, Encoding encoding = null)
	{
		encoding = (encoding ?? Encoding.UTF8);
		if (delimiter == Delimiter.Auto)
		{
			delimiter = CSVParser.EstimateDelimiter(path);
		}
		return CSVParser.Parse(File.ReadAllText(path, encoding), delimiter);
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0005AAA0 File Offset: 0x00058CA0
	public static async Task<Dictionary<string, List<string>>> LoadFromPathAsync(string path, Delimiter delimiter = Delimiter.Auto, Encoding encoding = null)
	{
		encoding = (encoding ?? Encoding.UTF8);
		if (delimiter == Delimiter.Auto)
		{
			delimiter = CSVParser.EstimateDelimiter(path);
		}
		Dictionary<string, List<string>> result;
		using (StreamReader reader = new StreamReader(path, encoding))
		{
			result = CSVParser.Parse(await reader.ReadToEndAsync(), delimiter);
		}
		return result;
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0005AAF8 File Offset: 0x00058CF8
	private static Delimiter EstimateDelimiter(string path)
	{
		string extension = Path.GetExtension(path);
		if (extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
		{
			return Delimiter.Comma;
		}
		if (extension.Equals(".tsv", StringComparison.OrdinalIgnoreCase))
		{
			return Delimiter.Tab;
		}
		throw new Exception("Delimiter estimation failed. Unknown Extension: " + extension);
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0005AB3C File Offset: 0x00058D3C
	public static Dictionary<string, List<string>> LoadFromString(string data, Delimiter delimiter = Delimiter.Comma)
	{
		if (delimiter == Delimiter.Auto)
		{
			throw new InvalidEnumArgumentException("Delimiter estimation from string is not supported.");
		}
		return CSVParser.Parse(data, delimiter);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0005AB54 File Offset: 0x00058D54
	private static Dictionary<string, List<string>> Parse(string data, Delimiter delimiter)
	{
		CSVParser.ConvertToCrlf(ref data);
		data = data.Trim(new char[]
		{
			'﻿',
			'​'
		});
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		int i = 0;
		string value = delimiter.ToChar().ToString();
		string value2 = "\r\n";
		string value3 = "\"";
		string value4 = "\"\"";
		while (i < data.Length)
		{
			int length = (i <= data.Length - 2) ? 2 : 1;
			string text = data.Substring(i, length);
			if (text.StartsWith(value))
			{
				if (flag)
				{
					stringBuilder.Append(delimiter.ToChar());
				}
				else
				{
					CSVParser.AddCell(list, stringBuilder);
				}
				i++;
			}
			else if (text.StartsWith(value2))
			{
				if (flag)
				{
					stringBuilder.Append("\r\n");
				}
				else
				{
					CSVParser.AddCell(list, stringBuilder);
					CSVParser.AddRow(dictionary, ref list);
				}
				i += 2;
			}
			else if (text.StartsWith(value4))
			{
				stringBuilder.Append("\"");
				i += 2;
			}
			else if (text.StartsWith(value3))
			{
				flag = !flag;
				i++;
			}
			else
			{
				stringBuilder.Append(text[0]);
				i++;
			}
		}
		if (list.Count > 0)
		{
			CSVParser.AddCell(list, stringBuilder);
			CSVParser.AddRow(dictionary, ref list);
		}
		return dictionary;
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0005ACB7 File Offset: 0x00058EB7
	private static void AddCell(List<string> row, StringBuilder cell)
	{
		row.Add(cell.ToString());
		cell.Length = 0;
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0005ACCC File Offset: 0x00058ECC
	private static void AddRow(Dictionary<string, List<string>> sheet, ref List<string> row)
	{
		if (!sheet.ContainsKey(row[0]))
		{
			sheet.Add(row[0], row);
			row.RemoveAt(0);
		}
		row = new List<string>();
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0005ACFD File Offset: 0x00058EFD
	private static void ConvertToCrlf(ref string data)
	{
		data = Regex.Replace(data, "\\r\\n|\\r|\\n", "\r\n");
	}
}
