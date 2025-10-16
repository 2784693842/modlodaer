using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x0200000A RID: 10
	internal class AttributesCriterion : SelectionCriterion
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000025F0 File Offset: 0x000007F0
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002694 File Offset: 0x00000894
		internal string AttributeString
		{
			get
			{
				string text = "";
				if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes)0)
				{
					text += "H";
				}
				if ((this._Attributes & FileAttributes.System) != (FileAttributes)0)
				{
					text += "S";
				}
				if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
				{
					text += "R";
				}
				if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
				{
					text += "A";
				}
				if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
				{
					text += "L";
				}
				if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes)0)
				{
					text += "I";
				}
				return text;
			}
			set
			{
				this._Attributes = FileAttributes.Normal;
				string text = value.ToUpper();
				int i = 0;
				while (i < text.Length)
				{
					char c = text[i];
					if (c <= 'L')
					{
						if (c != 'A')
						{
							switch (c)
							{
							case 'H':
								if ((this._Attributes & FileAttributes.Hidden) != (FileAttributes)0)
								{
									throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
								}
								this._Attributes |= FileAttributes.Hidden;
								break;
							case 'I':
								if ((this._Attributes & FileAttributes.NotContentIndexed) != (FileAttributes)0)
								{
									throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
								}
								this._Attributes |= FileAttributes.NotContentIndexed;
								break;
							case 'J':
							case 'K':
								goto IL_1BB;
							case 'L':
								if ((this._Attributes & FileAttributes.ReparsePoint) != (FileAttributes)0)
								{
									throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
								}
								this._Attributes |= FileAttributes.ReparsePoint;
								break;
							default:
								goto IL_1BB;
							}
						}
						else
						{
							if ((this._Attributes & FileAttributes.Archive) != (FileAttributes)0)
							{
								throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
							}
							this._Attributes |= FileAttributes.Archive;
						}
					}
					else if (c != 'R')
					{
						if (c != 'S')
						{
							goto IL_1BB;
						}
						if ((this._Attributes & FileAttributes.System) != (FileAttributes)0)
						{
							throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
						}
						this._Attributes |= FileAttributes.System;
					}
					else
					{
						if ((this._Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
						{
							throw new ArgumentException(string.Format("Repeated flag. ({0})", c), "value");
						}
						this._Attributes |= FileAttributes.ReadOnly;
					}
					i++;
					continue;
					IL_1BB:
					throw new ArgumentException(value);
				}
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002874 File Offset: 0x00000A74
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("attributes ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028C4 File Offset: 0x00000AC4
		private bool _EvaluateOne(FileAttributes fileAttrs, FileAttributes criterionAttrs)
		{
			return (this._Attributes & criterionAttrs) != criterionAttrs || (fileAttrs & criterionAttrs) == criterionAttrs;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028EC File Offset: 0x00000AEC
		internal override bool Evaluate(string filename)
		{
			if (Directory.Exists(filename))
			{
				return this.Operator != ComparisonOperator.EqualTo;
			}
			FileAttributes attributes = File.GetAttributes(filename);
			return this._Evaluate(attributes);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000291C File Offset: 0x00000B1C
		private bool _Evaluate(FileAttributes fileAttrs)
		{
			bool flag = this._EvaluateOne(fileAttrs, FileAttributes.Hidden);
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.System);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReadOnly);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.Archive);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.NotContentIndexed);
			}
			if (flag)
			{
				flag = this._EvaluateOne(fileAttrs, FileAttributes.ReparsePoint);
			}
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002988 File Offset: 0x00000B88
		internal override bool Evaluate(ZipEntry entry)
		{
			FileAttributes attributes = entry.Attributes;
			return this._Evaluate(attributes);
		}

		// Token: 0x0400001D RID: 29
		private FileAttributes _Attributes;

		// Token: 0x0400001E RID: 30
		internal ComparisonOperator Operator;
	}
}
