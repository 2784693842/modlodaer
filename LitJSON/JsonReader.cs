using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LitJson
{
	// Token: 0x02000012 RID: 18
	public class JsonReader
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x000048D4 File Offset: 0x00002AD4
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x000048E1 File Offset: 0x00002AE1
		public bool AllowComments
		{
			get
			{
				return this.lexer.AllowComments;
			}
			set
			{
				this.lexer.AllowComments = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000048EF File Offset: 0x00002AEF
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000048FC File Offset: 0x00002AFC
		public bool AllowSingleQuotedStrings
		{
			get
			{
				return this.lexer.AllowSingleQuotedStrings;
			}
			set
			{
				this.lexer.AllowSingleQuotedStrings = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000490A File Offset: 0x00002B0A
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004912 File Offset: 0x00002B12
		public bool SkipNonMembers
		{
			get
			{
				return this.skip_non_members;
			}
			set
			{
				this.skip_non_members = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000491B File Offset: 0x00002B1B
		public bool EndOfInput
		{
			get
			{
				return this.end_of_input;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00004923 File Offset: 0x00002B23
		public bool EndOfJson
		{
			get
			{
				return this.end_of_json;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000492B File Offset: 0x00002B2B
		public JsonToken Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00004933 File Offset: 0x00002B33
		public object Value
		{
			get
			{
				return this.token_value;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00004947 File Offset: 0x00002B47
		public JsonReader(string json_text) : this(new StringReader(json_text), true)
		{
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00004956 File Offset: 0x00002B56
		public JsonReader(TextReader reader) : this(reader, false)
		{
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004960 File Offset: 0x00002B60
		private JsonReader(TextReader reader, bool owned)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.read_started = false;
			this.automaton_stack = new Stack<int>();
			this.automaton_stack.Push(65553);
			this.automaton_stack.Push(65543);
			this.lexer = new Lexer(reader);
			this.end_of_input = false;
			this.end_of_json = false;
			this.skip_non_members = true;
			this.reader = reader;
			this.reader_is_owned = owned;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x000049F0 File Offset: 0x00002BF0
		private static IDictionary<int, IDictionary<int, int[]>> PopulateParseTable()
		{
			IDictionary<int, IDictionary<int, int[]>> parse_table = new Dictionary<int, IDictionary<int, int[]>>();
			JsonReader.TableAddRow(parse_table, ParserToken.Array);
			JsonReader.TableAddCol(parse_table, ParserToken.Array, 91, new int[]
			{
				91,
				65549
			});
			JsonReader.TableAddRow(parse_table, ParserToken.ArrayPrime);
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 34, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 91, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 93, new int[]
			{
				93
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 123, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65537, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65538, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65539, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ArrayPrime, 65540, new int[]
			{
				65550,
				65551,
				93
			});
			JsonReader.TableAddRow(parse_table, ParserToken.Object);
			JsonReader.TableAddCol(parse_table, ParserToken.Object, 123, new int[]
			{
				123,
				65545
			});
			JsonReader.TableAddRow(parse_table, ParserToken.ObjectPrime);
			JsonReader.TableAddCol(parse_table, ParserToken.ObjectPrime, 34, new int[]
			{
				65546,
				65547,
				125
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ObjectPrime, 125, new int[]
			{
				125
			});
			JsonReader.TableAddRow(parse_table, ParserToken.Pair);
			JsonReader.TableAddCol(parse_table, ParserToken.Pair, 34, new int[]
			{
				65552,
				58,
				65550
			});
			JsonReader.TableAddRow(parse_table, ParserToken.PairRest);
			JsonReader.TableAddCol(parse_table, ParserToken.PairRest, 44, new int[]
			{
				44,
				65546,
				65547
			});
			JsonReader.TableAddCol(parse_table, ParserToken.PairRest, 125, new int[]
			{
				65554
			});
			JsonReader.TableAddRow(parse_table, ParserToken.String);
			JsonReader.TableAddCol(parse_table, ParserToken.String, 34, new int[]
			{
				34,
				65541,
				34
			});
			JsonReader.TableAddRow(parse_table, ParserToken.Text);
			JsonReader.TableAddCol(parse_table, ParserToken.Text, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(parse_table, ParserToken.Text, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddRow(parse_table, ParserToken.Value);
			JsonReader.TableAddCol(parse_table, ParserToken.Value, 34, new int[]
			{
				65552
			});
			JsonReader.TableAddCol(parse_table, ParserToken.Value, 91, new int[]
			{
				65548
			});
			JsonReader.TableAddCol(parse_table, ParserToken.Value, 123, new int[]
			{
				65544
			});
			JsonReader.TableAddCol(parse_table, ParserToken.Value, 65537, new int[]
			{
				65537
			});
			JsonReader.TableAddCol(parse_table, ParserToken.Value, 65538, new int[]
			{
				65538
			});
			JsonReader.TableAddCol(parse_table, ParserToken.Value, 65539, new int[]
			{
				65539
			});
			JsonReader.TableAddCol(parse_table, ParserToken.Value, 65540, new int[]
			{
				65540
			});
			JsonReader.TableAddRow(parse_table, ParserToken.ValueRest);
			JsonReader.TableAddCol(parse_table, ParserToken.ValueRest, 44, new int[]
			{
				44,
				65550,
				65551
			});
			JsonReader.TableAddCol(parse_table, ParserToken.ValueRest, 93, new int[]
			{
				65554
			});
			return parse_table;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004D8B File Offset: 0x00002F8B
		private static void TableAddCol(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken row, int col, params int[] symbols)
		{
			parse_table[(int)row].Add(col, symbols);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004D9B File Offset: 0x00002F9B
		private static void TableAddRow(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken rule)
		{
			parse_table.Add((int)rule, new Dictionary<int, int[]>());
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004DAC File Offset: 0x00002FAC
		private void ProcessNumber(string number)
		{
			double n_double;
			if ((number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) && double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out n_double))
			{
				this.token = JsonToken.Double;
				this.token_value = n_double;
				return;
			}
			int n_int32;
			if (int.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out n_int32))
			{
				this.token = JsonToken.Int;
				this.token_value = n_int32;
				return;
			}
			long n_int33;
			if (long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out n_int33))
			{
				this.token = JsonToken.Long;
				this.token_value = n_int33;
				return;
			}
			ulong n_uint64;
			if (ulong.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out n_uint64))
			{
				this.token = JsonToken.Long;
				this.token_value = n_uint64;
				return;
			}
			this.token = JsonToken.Int;
			this.token_value = 0;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00004E84 File Offset: 0x00003084
		private void ProcessSymbol()
		{
			if (this.current_symbol == 91)
			{
				this.token = JsonToken.ArrayStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 93)
			{
				this.token = JsonToken.ArrayEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 123)
			{
				this.token = JsonToken.ObjectStart;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 125)
			{
				this.token = JsonToken.ObjectEnd;
				this.parser_return = true;
				return;
			}
			if (this.current_symbol == 34)
			{
				if (this.parser_in_string)
				{
					this.parser_in_string = false;
					this.parser_return = true;
					return;
				}
				if (this.token == JsonToken.None)
				{
					this.token = JsonToken.String;
				}
				this.parser_in_string = true;
				return;
			}
			else
			{
				if (this.current_symbol == 65541)
				{
					this.token_value = this.lexer.StringValue;
					return;
				}
				if (this.current_symbol == 65539)
				{
					this.token = JsonToken.Boolean;
					this.token_value = false;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65540)
				{
					this.token = JsonToken.Null;
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65537)
				{
					this.ProcessNumber(this.lexer.StringValue);
					this.parser_return = true;
					return;
				}
				if (this.current_symbol == 65546)
				{
					this.token = JsonToken.PropertyName;
					return;
				}
				if (this.current_symbol == 65538)
				{
					this.token = JsonToken.Boolean;
					this.token_value = true;
					this.parser_return = true;
				}
				return;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004FF6 File Offset: 0x000031F6
		private bool ReadToken()
		{
			if (this.end_of_input)
			{
				return false;
			}
			this.lexer.NextToken();
			if (this.lexer.EndOfInput)
			{
				this.Close();
				return false;
			}
			this.current_input = this.lexer.Token;
			return true;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005038 File Offset: 0x00003238
		public void Close()
		{
			if (this.end_of_input)
			{
				return;
			}
			this.end_of_input = true;
			this.end_of_json = true;
			if (this.reader_is_owned)
			{
				using (this.reader)
				{
				}
			}
			this.reader = null;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005090 File Offset: 0x00003290
		public bool Read()
		{
			if (this.end_of_input)
			{
				return false;
			}
			if (this.end_of_json)
			{
				this.end_of_json = false;
				this.automaton_stack.Clear();
				this.automaton_stack.Push(65553);
				this.automaton_stack.Push(65543);
			}
			this.parser_in_string = false;
			this.parser_return = false;
			this.token = JsonToken.None;
			this.token_value = null;
			if (!this.read_started)
			{
				this.read_started = true;
				if (!this.ReadToken())
				{
					return false;
				}
			}
			while (!this.parser_return)
			{
				this.current_symbol = this.automaton_stack.Pop();
				this.ProcessSymbol();
				if (this.current_symbol == this.current_input)
				{
					if (!this.ReadToken())
					{
						if (this.automaton_stack.Peek() != 65553)
						{
							throw new JsonException("Input doesn't evaluate to proper JSON text");
						}
						return this.parser_return;
					}
				}
				else
				{
					int[] entry_symbols;
					try
					{
						entry_symbols = JsonReader.parse_table[this.current_symbol][this.current_input];
					}
					catch (KeyNotFoundException e)
					{
						throw new JsonException((ParserToken)this.current_input, e);
					}
					if (entry_symbols[0] != 65554)
					{
						for (int i = entry_symbols.Length - 1; i >= 0; i--)
						{
							this.automaton_stack.Push(entry_symbols[i]);
						}
					}
				}
			}
			if (this.automaton_stack.Peek() == 65553)
			{
				this.end_of_json = true;
			}
			return true;
		}

		// Token: 0x0400003B RID: 59
		private static readonly IDictionary<int, IDictionary<int, int[]>> parse_table = JsonReader.PopulateParseTable();

		// Token: 0x0400003C RID: 60
		private Stack<int> automaton_stack;

		// Token: 0x0400003D RID: 61
		private int current_input;

		// Token: 0x0400003E RID: 62
		private int current_symbol;

		// Token: 0x0400003F RID: 63
		private bool end_of_json;

		// Token: 0x04000040 RID: 64
		private bool end_of_input;

		// Token: 0x04000041 RID: 65
		private Lexer lexer;

		// Token: 0x04000042 RID: 66
		private bool parser_in_string;

		// Token: 0x04000043 RID: 67
		private bool parser_return;

		// Token: 0x04000044 RID: 68
		private bool read_started;

		// Token: 0x04000045 RID: 69
		private TextReader reader;

		// Token: 0x04000046 RID: 70
		private bool reader_is_owned;

		// Token: 0x04000047 RID: 71
		private bool skip_non_members;

		// Token: 0x04000048 RID: 72
		private object token_value;

		// Token: 0x04000049 RID: 73
		private JsonToken token;
	}
}
