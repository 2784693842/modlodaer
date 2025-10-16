using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000015 RID: 21
	public class JsonWriter
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00005204 File Offset: 0x00003404
		// (set) Token: 0x0600010E RID: 270 RVA: 0x0000520C File Offset: 0x0000340C
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000522A File Offset: 0x0000342A
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00005232 File Offset: 0x00003432
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000523B File Offset: 0x0000343B
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005243 File Offset: 0x00003443
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000524B File Offset: 0x0000344B
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005254 File Offset: 0x00003454
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000525C File Offset: 0x0000345C
		public bool LowerCaseProperties
		{
			get
			{
				return this.lower_case_properties;
			}
			set
			{
				this.lower_case_properties = value;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005271 File Offset: 0x00003471
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000529B File Offset: 0x0000349B
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000052A9 File Offset: 0x000034A9
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000052CC File Offset: 0x000034CC
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000053F0 File Offset: 0x000035F0
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.lower_case_properties = false;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000545C File Offset: 0x0000365C
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000549D File Offset: 0x0000369D
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000054BC File Offset: 0x000036BC
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005508 File Offset: 0x00003708
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005514 File Offset: 0x00003714
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write(Environment.NewLine);
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005574 File Offset: 0x00003774
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int i = str.Length;
			int j = 0;
			while (j < i)
			{
				char c = str[j];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				case '\v':
					goto IL_E4;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						goto IL_E4;
					}
					this.writer.Write('\\');
					this.writer.Write(str[j]);
					break;
				}
				IL_141:
				j++;
				continue;
				IL_E4:
				if (str[j] >= ' ' && str[j] <= '~')
				{
					this.writer.Write(str[j]);
					goto IL_141;
				}
				JsonWriter.IntToHex((int)str[j], this.hex_seq);
				this.writer.Write("\\u");
				this.writer.Write(this.hex_seq);
				goto IL_141;
			}
			this.writer.Write('"');
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000056DA File Offset: 0x000038DA
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000056F7 File Offset: 0x000038F7
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005714 File Offset: 0x00003914
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000576F File Offset: 0x0000396F
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(boolean ? "true" : "false");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000579F File Offset: 0x0000399F
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000057CC File Offset: 0x000039CC
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string str = Convert.ToString(number, JsonWriter.number_format);
			this.Put(str);
			if (str.IndexOf('.') == -1 && str.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000582C File Offset: 0x00003A2C
		public void Write(float number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string str = Convert.ToString(number, JsonWriter.number_format);
			this.Put(str);
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005865 File Offset: 0x00003A65
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005891 File Offset: 0x00003A91
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000058BD File Offset: 0x00003ABD
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000058EF File Offset: 0x00003AEF
		[CLSCompliant(false)]
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000591C File Offset: 0x00003B1C
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005988 File Offset: 0x00003B88
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000059DC File Offset: 0x00003BDC
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005A48 File Offset: 0x00003C48
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005A9C File Offset: 0x00003C9C
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			string propertyName = (property_name == null || !this.lower_case_properties) ? property_name : property_name.ToLowerInvariant();
			this.PutString(propertyName);
			if (this.pretty_print)
			{
				if (propertyName.Length > this.context.Padding)
				{
					this.context.Padding = propertyName.Length;
				}
				for (int i = this.context.Padding - propertyName.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04000055 RID: 85
		private static readonly NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x04000056 RID: 86
		private WriterContext context;

		// Token: 0x04000057 RID: 87
		private Stack<WriterContext> ctx_stack;

		// Token: 0x04000058 RID: 88
		private bool has_reached_end;

		// Token: 0x04000059 RID: 89
		private char[] hex_seq;

		// Token: 0x0400005A RID: 90
		private int indentation;

		// Token: 0x0400005B RID: 91
		private int indent_value;

		// Token: 0x0400005C RID: 92
		private StringBuilder inst_string_builder;

		// Token: 0x0400005D RID: 93
		private bool pretty_print;

		// Token: 0x0400005E RID: 94
		private bool validate;

		// Token: 0x0400005F RID: 95
		private bool lower_case_properties;

		// Token: 0x04000060 RID: 96
		private TextWriter writer;
	}
}
