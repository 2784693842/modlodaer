using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000009 RID: 9
	internal struct ObjectMetadata
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003154 File Offset: 0x00001354
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00003175 File Offset: 0x00001375
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000317E File Offset: 0x0000137E
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00003186 File Offset: 0x00001386
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008E RID: 142 RVA: 0x0000318F File Offset: 0x0000138F
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00003197 File Offset: 0x00001397
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x0400001B RID: 27
		private Type element_type;

		// Token: 0x0400001C RID: 28
		private bool is_dictionary;

		// Token: 0x0400001D RID: 29
		private IDictionary<string, PropertyMetadata> properties;
	}
}
