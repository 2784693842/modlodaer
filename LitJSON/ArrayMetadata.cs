using System;

namespace LitJson
{
	// Token: 0x02000008 RID: 8
	internal struct ArrayMetadata
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003108 File Offset: 0x00001308
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003129 File Offset: 0x00001329
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003132 File Offset: 0x00001332
		// (set) Token: 0x06000087 RID: 135 RVA: 0x0000313A File Offset: 0x0000133A
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003143 File Offset: 0x00001343
		// (set) Token: 0x06000089 RID: 137 RVA: 0x0000314B File Offset: 0x0000134B
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x04000018 RID: 24
		private Type element_type;

		// Token: 0x04000019 RID: 25
		private bool is_array;

		// Token: 0x0400001A RID: 26
		private bool is_list;
	}
}
