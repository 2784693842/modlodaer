using System;
using System.Collections.Generic;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000080 RID: 128
	internal class Huffman : IHuffman, IComparer<HuffmanListNode>
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0001141F File Offset: 0x0000F61F
		// (set) Token: 0x060002DA RID: 730 RVA: 0x00011427 File Offset: 0x0000F627
		public int TableBits { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060002DB RID: 731 RVA: 0x00011430 File Offset: 0x0000F630
		// (set) Token: 0x060002DC RID: 732 RVA: 0x00011438 File Offset: 0x0000F638
		public IReadOnlyList<HuffmanListNode> PrefixTree { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00011441 File Offset: 0x0000F641
		// (set) Token: 0x060002DE RID: 734 RVA: 0x00011449 File Offset: 0x0000F649
		public IReadOnlyList<HuffmanListNode> OverflowList { get; private set; }

		// Token: 0x060002DF RID: 735 RVA: 0x00011454 File Offset: 0x0000F654
		public void GenerateTable(IReadOnlyList<int> values, int[] lengthList, int[] codeList)
		{
			HuffmanListNode[] array = new HuffmanListNode[lengthList.Length];
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new HuffmanListNode
				{
					Value = values[i],
					Length = ((lengthList[i] <= 0) ? 99999 : lengthList[i]),
					Bits = codeList[i],
					Mask = (1 << lengthList[i]) - 1
				};
				if (lengthList[i] > 0 && num < lengthList[i])
				{
					num = lengthList[i];
				}
			}
			Array.Sort<HuffmanListNode>(array, 0, array.Length, this);
			int num2 = (num > 10) ? 10 : num;
			List<HuffmanListNode> list = new List<HuffmanListNode>(1 << num2);
			List<HuffmanListNode> list2 = null;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].Length >= 99999)
				{
					break;
				}
				int length = array[j].Length;
				if (length > num2)
				{
					list2 = new List<HuffmanListNode>(array.Length - j);
					while (j < array.Length)
					{
						if (array[j].Length >= 99999)
						{
							break;
						}
						list2.Add(array[j]);
						j++;
					}
				}
				else
				{
					int num3 = 1 << num2 - length;
					HuffmanListNode huffmanListNode = array[j];
					for (int k = 0; k < num3; k++)
					{
						int num4 = k << length | huffmanListNode.Bits;
						while (list.Count <= num4)
						{
							list.Add(null);
						}
						list[num4] = huffmanListNode;
					}
				}
			}
			while (list.Count < 1 << num2)
			{
				list.Add(null);
			}
			this.TableBits = num2;
			this.PrefixTree = list;
			this.OverflowList = list2;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000115F0 File Offset: 0x0000F7F0
		int IComparer<HuffmanListNode>.Compare(HuffmanListNode x, HuffmanListNode y)
		{
			int num = x.Length - y.Length;
			if (num == 0)
			{
				return x.Bits - y.Bits;
			}
			return num;
		}

		// Token: 0x04000335 RID: 821
		private const int MAX_TABLE_BITS = 10;
	}
}
