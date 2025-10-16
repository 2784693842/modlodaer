using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000009 RID: 9
	internal class TypeCriterion : SelectionCriterion
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000024E2 File Offset: 0x000006E2
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000024EF File Offset: 0x000006EF
		internal string AttributeString
		{
			get
			{
				return this.ObjectType.ToString();
			}
			set
			{
				if (value.Length != 1 || (value[0] != 'D' && value[0] != 'F'))
				{
					throw new ArgumentException("Specify a single character: either D or F");
				}
				this.ObjectType = value[0];
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002528 File Offset: 0x00000728
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("type ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.AttributeString);
			return stringBuilder.ToString();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002578 File Offset: 0x00000778
		internal override bool Evaluate(string filename)
		{
			bool flag = (this.ObjectType == 'D') ? Directory.Exists(filename) : File.Exists(filename);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025B0 File Offset: 0x000007B0
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = (this.ObjectType == 'D') ? entry.IsDirectory : (!entry.IsDirectory);
			if (this.Operator != ComparisonOperator.EqualTo)
			{
				flag = !flag;
			}
			return flag;
		}

		// Token: 0x0400001B RID: 27
		private char ObjectType;

		// Token: 0x0400001C RID: 28
		internal ComparisonOperator Operator;
	}
}
