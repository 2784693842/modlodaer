using System;
using System.IO;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x02000007 RID: 7
	internal class TimeCriterion : SelectionCriterion
	{
		// Token: 0x0600000C RID: 12 RVA: 0x0000218C File Offset: 0x0000038C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Which.ToString()).Append(" ").Append(EnumUtil.GetDescription(this.Operator)).Append(" ").Append(this.Time.ToString("yyyy-MM-dd-HH:mm:ss"));
			return stringBuilder.ToString();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021FC File Offset: 0x000003FC
		internal override bool Evaluate(string filename)
		{
			DateTime x;
			switch (this.Which)
			{
			case WhichTime.atime:
				x = File.GetLastAccessTime(filename).ToUniversalTime();
				break;
			case WhichTime.mtime:
				x = File.GetLastWriteTime(filename).ToUniversalTime();
				break;
			case WhichTime.ctime:
				x = File.GetCreationTime(filename).ToUniversalTime();
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return this._Evaluate(x);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000226C File Offset: 0x0000046C
		private bool _Evaluate(DateTime x)
		{
			bool result;
			switch (this.Operator)
			{
			case ComparisonOperator.GreaterThan:
				result = (x > this.Time);
				break;
			case ComparisonOperator.GreaterThanOrEqualTo:
				result = (x >= this.Time);
				break;
			case ComparisonOperator.LesserThan:
				result = (x < this.Time);
				break;
			case ComparisonOperator.LesserThanOrEqualTo:
				result = (x <= this.Time);
				break;
			case ComparisonOperator.EqualTo:
				result = (x == this.Time);
				break;
			case ComparisonOperator.NotEqualTo:
				result = (x != this.Time);
				break;
			default:
				throw new ArgumentException("Operator");
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002308 File Offset: 0x00000508
		internal override bool Evaluate(ZipEntry entry)
		{
			DateTime x;
			switch (this.Which)
			{
			case WhichTime.atime:
				x = entry.AccessedTime;
				break;
			case WhichTime.mtime:
				x = entry.ModifiedTime;
				break;
			case WhichTime.ctime:
				x = entry.CreationTime;
				break;
			default:
				throw new ArgumentException("??time");
			}
			return this._Evaluate(x);
		}

		// Token: 0x04000014 RID: 20
		internal ComparisonOperator Operator;

		// Token: 0x04000015 RID: 21
		internal WhichTime Which;

		// Token: 0x04000016 RID: 22
		internal DateTime Time;
	}
}
