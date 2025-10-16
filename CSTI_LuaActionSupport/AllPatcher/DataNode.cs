using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CSTI_LuaActionSupport.LuaCodeHelper;
using UnityEngine;

namespace CSTI_LuaActionSupport.AllPatcher
{
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	[Nullable(0)]
	public struct DataNode
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00007958 File Offset: 0x00005B58
		public double number
		{
			get
			{
				return this.NodeData.number;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00007965 File Offset: 0x00005B65
		[Nullable(2)]
		public string str
		{
			[NullableContext(2)]
			get
			{
				return this.NodeData.str;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007972 File Offset: 0x00005B72
		public bool _bool
		{
			get
			{
				return this.NodeData._bool;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000797F File Offset: 0x00005B7F
		public Vector2 vector2
		{
			get
			{
				return this.NodeData.vector2;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000798C File Offset: 0x00005B8C
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public Dictionary<string, DataNode> table
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return this.NodeData.table;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00007999 File Offset: 0x00005B99
		public static DataNode EmptyTable
		{
			get
			{
				return new DataNode(new Dictionary<string, DataNode>());
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000188 RID: 392 RVA: 0x000079A8 File Offset: 0x00005BA8
		public static DataNode Nil
		{
			get
			{
				return new DataNode
				{
					NodeType = DataNode.DataNodeType.Nil
				};
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000079C6 File Offset: 0x00005BC6
		public DataNode(double number)
		{
			this.NodeType = DataNode.DataNodeType.Number;
			this.NodeData = new DataNode.DataNodeDataUnion(number);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000079DB File Offset: 0x00005BDB
		public DataNode(string str)
		{
			this.NodeType = DataNode.DataNodeType.Str;
			this.NodeData = new DataNode.DataNodeDataUnion(str);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000079F0 File Offset: 0x00005BF0
		public DataNode(bool b)
		{
			this.NodeType = DataNode.DataNodeType.Bool;
			this.NodeData = new DataNode.DataNodeDataUnion(b);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007A05 File Offset: 0x00005C05
		public DataNode(Vector2 vector2)
		{
			this.NodeType = DataNode.DataNodeType.Vector2;
			this.NodeData = new DataNode.DataNodeDataUnion(vector2);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007A1A File Offset: 0x00005C1A
		public DataNode(Dictionary<string, DataNode> dataNodes)
		{
			this.NodeType = DataNode.DataNodeType.Table;
			this.NodeData = new DataNode.DataNodeDataUnion(dataNodes);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007A30 File Offset: 0x00005C30
		public void Save(BinaryWriter binaryWriter)
		{
			binaryWriter.Write((int)this.NodeType);
			switch (this.NodeType)
			{
			case DataNode.DataNodeType.Number:
				binaryWriter.Write(this.number);
				return;
			case DataNode.DataNodeType.Str:
				binaryWriter.Write(this.str ?? "");
				return;
			case DataNode.DataNodeType.Bool:
				binaryWriter.Write(this._bool);
				return;
			case DataNode.DataNodeType.Table:
				if (this.table == null)
				{
					binaryWriter.Write(0);
					return;
				}
				binaryWriter.Write(this.table.Count((KeyValuePair<string, DataNode> pair) => pair.Value.NodeType != DataNode.DataNodeType.Nil));
				using (Dictionary<string, DataNode>.Enumerator enumerator = this.table.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, DataNode> pair2 = enumerator.Current;
						string text;
						DataNode dataNode;
						pair2.Deconstruct(out text, out dataNode);
						string value = text;
						DataNode dataNode2 = dataNode;
						if (dataNode2.NodeType != DataNode.DataNodeType.Nil)
						{
							binaryWriter.Write(value);
							dataNode2.Save(binaryWriter);
						}
					}
					return;
				}
				break;
			case DataNode.DataNodeType.Nil:
				return;
			case DataNode.DataNodeType.Vector2:
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			binaryWriter.Write(this.vector2.x);
			binaryWriter.Write(this.vector2.y);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007B74 File Offset: 0x00005D74
		public static DataNode Load(BinaryReader binaryReader)
		{
			DataNode dataNode = new DataNode
			{
				NodeType = (DataNode.DataNodeType)binaryReader.ReadInt32()
			};
			switch (dataNode.NodeType)
			{
			case DataNode.DataNodeType.Number:
				dataNode.NodeData = new DataNode.DataNodeDataUnion(binaryReader.ReadDouble());
				break;
			case DataNode.DataNodeType.Str:
				dataNode.NodeData = new DataNode.DataNodeDataUnion(binaryReader.ReadString());
				break;
			case DataNode.DataNodeType.Bool:
				dataNode.NodeData = new DataNode.DataNodeDataUnion(binaryReader.ReadBoolean());
				break;
			case DataNode.DataNodeType.Table:
			{
				int num = binaryReader.ReadInt32();
				Dictionary<string, DataNode> dictionary = new Dictionary<string, DataNode>();
				for (int i = 0; i < num; i++)
				{
					string key = binaryReader.ReadString();
					DataNode value = DataNode.Load(binaryReader);
					dictionary[key] = value;
				}
				dataNode.NodeData = new DataNode.DataNodeDataUnion(dictionary);
				break;
			}
			case DataNode.DataNodeType.Nil:
				break;
			case DataNode.DataNodeType.Vector2:
			{
				float x = binaryReader.ReadSingle();
				float y = binaryReader.ReadSingle();
				dataNode.NodeData = new DataNode.DataNodeDataUnion(new Vector2(x, y));
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			return dataNode;
		}

		// Token: 0x040000AB RID: 171
		public DataNode.DataNodeType NodeType;

		// Token: 0x040000AC RID: 172
		public DataNode.DataNodeDataUnion NodeData;

		// Token: 0x0200004C RID: 76
		[Nullable(0)]
		[StructLayout(LayoutKind.Explicit)]
		public struct DataNodeDataUnion
		{
			// Token: 0x06000190 RID: 400 RVA: 0x00007C76 File Offset: 0x00005E76
			public DataNodeDataUnion(double num)
			{
				this._bool = false;
				this.vector2 = default(Vector2);
				this.str = null;
				this.table = null;
				this.number = num;
			}

			// Token: 0x06000191 RID: 401 RVA: 0x00007CA0 File Offset: 0x00005EA0
			public DataNodeDataUnion(string str)
			{
				this.number = 0.0;
				this._bool = false;
				this.vector2 = default(Vector2);
				this.table = null;
				this.str = str;
			}

			// Token: 0x06000192 RID: 402 RVA: 0x00007CD2 File Offset: 0x00005ED2
			public DataNodeDataUnion(bool b)
			{
				this.number = 0.0;
				this.vector2 = default(Vector2);
				this.str = null;
				this.table = null;
				this._bool = b;
			}

			// Token: 0x06000193 RID: 403 RVA: 0x00007D04 File Offset: 0x00005F04
			public DataNodeDataUnion(Vector2 vector2)
			{
				this.number = 0.0;
				this._bool = false;
				this.str = null;
				this.table = null;
				this.vector2 = vector2;
			}

			// Token: 0x06000194 RID: 404 RVA: 0x00007D31 File Offset: 0x00005F31
			public DataNodeDataUnion(Dictionary<string, DataNode> table)
			{
				this.number = 0.0;
				this._bool = false;
				this.vector2 = default(Vector2);
				this.str = null;
				this.table = table;
			}

			// Token: 0x040000AD RID: 173
			[FieldOffset(8)]
			public double number;

			// Token: 0x040000AE RID: 174
			[FieldOffset(8)]
			public bool _bool;

			// Token: 0x040000AF RID: 175
			[FieldOffset(8)]
			public Vector2 vector2;

			// Token: 0x040000B0 RID: 176
			[Nullable(2)]
			[FieldOffset(0)]
			public string str;

			// Token: 0x040000B1 RID: 177
			[Nullable(new byte[]
			{
				2,
				1
			})]
			[FieldOffset(0)]
			public Dictionary<string, DataNode> table;
		}

		// Token: 0x0200004D RID: 77
		[NullableContext(0)]
		public enum DataNodeType
		{
			// Token: 0x040000B3 RID: 179
			Number,
			// Token: 0x040000B4 RID: 180
			Str,
			// Token: 0x040000B5 RID: 181
			Bool,
			// Token: 0x040000B6 RID: 182
			Table,
			// Token: 0x040000B7 RID: 183
			Nil,
			// Token: 0x040000B8 RID: 184
			Vector2
		}
	}
}
