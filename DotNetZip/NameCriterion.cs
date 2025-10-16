using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000008 RID: 8
	internal class NameCriterion : SelectionCriterion
	{
		// Token: 0x17000002 RID: 2
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002368 File Offset: 0x00000568
		internal virtual string MatchingFileSpec
		{
			set
			{
				if (Directory.Exists(value))
				{
					this._MatchingFileSpec = ".\\" + value + "\\*.*";
				}
				else
				{
					this._MatchingFileSpec = value;
				}
				this._regexString = "^" + Regex.Escape(this._MatchingFileSpec).Replace("\\\\\\*\\.\\*", "\\\\([^\\.]+|.*\\.[^\\\\\\.]*)").Replace("\\.\\*", "\\.[^\\\\\\.]*").Replace("\\*", ".*").Replace("\\?", "[^\\\\\\.]") + "$";
				this._re = new Regex(this._regexString, RegexOptions.IgnoreCase);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000240C File Offset: 0x0000060C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("name ").Append(EnumUtil.GetDescription(this.Operator)).Append(" '").Append(this._MatchingFileSpec).Append("'");
			return stringBuilder.ToString();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002463 File Offset: 0x00000663
		internal override bool Evaluate(string filename)
		{
			return this._Evaluate(filename);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000246C File Offset: 0x0000066C
		private bool _Evaluate(string fullpath)
		{
			string input = (this._MatchingFileSpec.IndexOf('\\') == -1) ? Path.GetFileName(fullpath) : fullpath;
			bool flag = this._re.IsMatch(input);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024B0 File Offset: 0x000006B0
		internal override bool Evaluate(ZipEntry entry)
		{
			string fullpath = entry.FileName.Replace("/", "\\");
			return this._Evaluate(fullpath);
		}

		// Token: 0x04000017 RID: 23
		private Regex _re;

		// Token: 0x04000018 RID: 24
		private string _regexString;

		// Token: 0x04000019 RID: 25
		internal ComparisonOperator Operator;

		// Token: 0x0400001A RID: 26
		private string _MatchingFileSpec;
	}
}
