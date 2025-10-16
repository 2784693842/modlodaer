using System;
using System.Text;
using Ionic.Zip;

namespace Ionic
{
	// Token: 0x0200000B RID: 11
	internal class CompoundCriterion : SelectionCriterion
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000029AB File Offset: 0x00000BAB
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000029B3 File Offset: 0x00000BB3
		internal SelectionCriterion Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
				if (value == null)
				{
					this.Conjunction = LogicalConjunction.NONE;
					return;
				}
				if (this.Conjunction == LogicalConjunction.NONE)
				{
					this.Conjunction = LogicalConjunction.AND;
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029D8 File Offset: 0x00000BD8
		internal override bool Evaluate(string filename)
		{
			bool flag = this.Left.Evaluate(filename);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(filename);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(filename);
				break;
			default:
				throw new ArgumentException("Conjunction");
			}
			return flag;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A50 File Offset: 0x00000C50
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(").Append((this.Left != null) ? this.Left.ToString() : "null").Append(" ").Append(this.Conjunction.ToString()).Append(" ").Append((this.Right != null) ? this.Right.ToString() : "null").Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002AE8 File Offset: 0x00000CE8
		internal override bool Evaluate(ZipEntry entry)
		{
			bool flag = this.Left.Evaluate(entry);
			switch (this.Conjunction)
			{
			case LogicalConjunction.AND:
				if (flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.OR:
				if (!flag)
				{
					flag = this.Right.Evaluate(entry);
				}
				break;
			case LogicalConjunction.XOR:
				flag ^= this.Right.Evaluate(entry);
				break;
			}
			return flag;
		}

		// Token: 0x0400001F RID: 31
		internal LogicalConjunction Conjunction;

		// Token: 0x04000020 RID: 32
		internal SelectionCriterion Left;

		// Token: 0x04000021 RID: 33
		private SelectionCriterion _Right;
	}
}
