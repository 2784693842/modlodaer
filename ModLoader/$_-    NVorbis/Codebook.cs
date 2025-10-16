using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x0200007C RID: 124
	internal class Codebook : ICodebook
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000FA44 File Offset: 0x0000DC44
		public void Init(IPacket packet, IHuffman huffman)
		{
			if (packet.ReadBits(24) != 5653314UL)
			{
				throw new InvalidDataException("Book header had invalid signature!");
			}
			this.Dimensions = (int)packet.ReadBits(16);
			this.Entries = (int)packet.ReadBits(24);
			this._lengths = new int[this.Entries];
			this.InitTree(packet, huffman);
			this.InitLookupTable(packet);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		private void InitTree(IPacket packet, IHuffman huffman)
		{
			int num = 0;
			bool flag;
			int num4;
			if (packet.ReadBit())
			{
				int num2 = (int)packet.ReadBits(5) + 1;
				int i = 0;
				while (i < this.Entries)
				{
					int num3 = (int)packet.ReadBits(Utils.ilog(this.Entries - i));
					while (--num3 >= 0)
					{
						this._lengths[i++] = num2;
					}
					num2++;
				}
				num = 0;
				flag = false;
				num4 = num2;
			}
			else
			{
				num4 = -1;
				flag = packet.ReadBit();
				for (int j = 0; j < this.Entries; j++)
				{
					if (!flag || packet.ReadBit())
					{
						this._lengths[j] = (int)packet.ReadBits(5) + 1;
						num++;
					}
					else
					{
						this._lengths[j] = -1;
					}
					if (this._lengths[j] > num4)
					{
						num4 = this._lengths[j];
					}
				}
			}
			if ((this._maxBits = num4) > -1)
			{
				int[] array = null;
				if (flag && num >= this.Entries >> 2)
				{
					array = new int[this.Entries];
					Array.Copy(this._lengths, array, this.Entries);
					flag = false;
				}
				int num5;
				if (flag)
				{
					num5 = num;
				}
				else
				{
					num5 = 0;
				}
				int[] array2 = null;
				int[] array3 = null;
				if (!flag)
				{
					array3 = new int[this.Entries];
				}
				else if (num5 != 0)
				{
					array = new int[num5];
					array3 = new int[num5];
					array2 = new int[num5];
				}
				if (!this.ComputeCodewords(flag, array3, array, this._lengths, this.Entries, array2))
				{
					throw new InvalidDataException();
				}
				IReadOnlyList<int> readOnlyList = array2;
				IReadOnlyList<int> value = readOnlyList ?? Codebook.FastRange.Get(0, array3.Length);
				huffman.GenerateTable(value, array ?? this._lengths, array3);
				this._prefixList = huffman.PrefixTree;
				this._prefixBitLength = huffman.TableBits;
				this._overflowList = huffman.OverflowList;
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000FC78 File Offset: 0x0000DE78
		private bool ComputeCodewords(bool sparse, int[] codewords, int[] codewordLengths, int[] len, int n, int[] values)
		{
			int num = 0;
			uint[] array = new uint[32];
			int num2 = 0;
			while (num2 < n && len[num2] <= 0)
			{
				num2++;
			}
			if (num2 == n)
			{
				return true;
			}
			this.AddEntry(sparse, codewords, codewordLengths, 0U, num2, num++, len[num2], values);
			for (int i = 1; i <= len[num2]; i++)
			{
				array[i] = 1U << 32 - i;
			}
			for (int i = num2 + 1; i < n; i++)
			{
				int num3 = len[i];
				if (num3 > 0)
				{
					while (num3 > 0 && array[num3] == 0U)
					{
						num3--;
					}
					if (num3 == 0)
					{
						return false;
					}
					uint num4 = array[num3];
					array[num3] = 0U;
					this.AddEntry(sparse, codewords, codewordLengths, Utils.BitReverse(num4), i, num++, len[i], values);
					if (num3 != len[i])
					{
						for (int j = len[i]; j > num3; j--)
						{
							array[j] = num4 + (1U << 32 - j);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000FD66 File Offset: 0x0000DF66
		private void AddEntry(bool sparse, int[] codewords, int[] codewordLengths, uint huffCode, int symbol, int count, int len, int[] values)
		{
			if (sparse)
			{
				codewords[count] = (int)huffCode;
				codewordLengths[count] = len;
				values[count] = symbol;
				return;
			}
			codewords[symbol] = (int)huffCode;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000FD88 File Offset: 0x0000DF88
		private void InitLookupTable(IPacket packet)
		{
			this.MapType = (int)packet.ReadBits(4);
			if (this.MapType == 0)
			{
				return;
			}
			float num = Utils.ConvertFromVorbisFloat32((uint)packet.ReadBits(32));
			float num2 = Utils.ConvertFromVorbisFloat32((uint)packet.ReadBits(32));
			int count = (int)packet.ReadBits(4) + 1;
			bool flag = packet.ReadBit();
			int num3 = this.Entries * this.Dimensions;
			float[] array = new float[num3];
			if (this.MapType == 1)
			{
				num3 = this.lookup1_values();
			}
			uint[] array2 = new uint[num3];
			for (int i = 0; i < num3; i++)
			{
				array2[i] = (uint)packet.ReadBits(count);
			}
			if (this.MapType == 1)
			{
				for (int j = 0; j < this.Entries; j++)
				{
					double num4 = 0.0;
					int num5 = 1;
					for (int k = 0; k < this.Dimensions; k++)
					{
						int num6 = j / num5 % num3;
						double num7 = (double)(array2[num6] * num2 + num) + num4;
						array[j * this.Dimensions + k] = (float)num7;
						if (flag)
						{
							num4 = num7;
						}
						num5 *= num3;
					}
				}
			}
			else
			{
				for (int l = 0; l < this.Entries; l++)
				{
					double num8 = 0.0;
					int num9 = l * this.Dimensions;
					for (int m = 0; m < this.Dimensions; m++)
					{
						double num10 = (double)(array2[num9] * num2 + num) + num8;
						array[l * this.Dimensions + m] = (float)num10;
						if (flag)
						{
							num8 = num10;
						}
						num9++;
					}
				}
			}
			this._lookupTable = array;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000FF24 File Offset: 0x0000E124
		private int lookup1_values()
		{
			int num = (int)Math.Floor(Math.Exp(Math.Log((double)this.Entries) / (double)this.Dimensions));
			if (Math.Floor(Math.Pow((double)(num + 1), (double)this.Dimensions)) <= (double)this.Entries)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000FF74 File Offset: 0x0000E174
		public int DecodeScalar(IPacket packet)
		{
			int num2;
			int num = (int)packet.TryPeekBits(this._prefixBitLength, out num2);
			if (num2 == 0)
			{
				return -1;
			}
			HuffmanListNode huffmanListNode = this._prefixList[num];
			if (huffmanListNode != null)
			{
				packet.SkipBits(huffmanListNode.Length);
				return huffmanListNode.Value;
			}
			int num3;
			num = (int)packet.TryPeekBits(this._maxBits, out num3);
			for (int i = 0; i < this._overflowList.Count; i++)
			{
				huffmanListNode = this._overflowList[i];
				if (huffmanListNode.Bits == (num & huffmanListNode.Mask))
				{
					packet.SkipBits(huffmanListNode.Length);
					return huffmanListNode.Value;
				}
			}
			return -1;
		}

		// Token: 0x170000E5 RID: 229
		public float this[int entry, int dim]
		{
			get
			{
				return this._lookupTable[entry * this.Dimensions + dim];
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00010028 File Offset: 0x0000E228
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x00010030 File Offset: 0x0000E230
		public int Dimensions { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x00010039 File Offset: 0x0000E239
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x00010041 File Offset: 0x0000E241
		public int Entries { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0001004A File Offset: 0x0000E24A
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x00010052 File Offset: 0x0000E252
		public int MapType { get; private set; }

		// Token: 0x0400031E RID: 798
		private int[] _lengths;

		// Token: 0x0400031F RID: 799
		private float[] _lookupTable;

		// Token: 0x04000320 RID: 800
		private IReadOnlyList<HuffmanListNode> _overflowList;

		// Token: 0x04000321 RID: 801
		private IReadOnlyList<HuffmanListNode> _prefixList;

		// Token: 0x04000322 RID: 802
		private int _prefixBitLength;

		// Token: 0x04000323 RID: 803
		private int _maxBits;

		// Token: 0x0200007D RID: 125
		private class FastRange : IReadOnlyList<int>, IReadOnlyCollection<int>, IEnumerable<int>, IEnumerable
		{
			// Token: 0x060002CA RID: 714 RVA: 0x0001005B File Offset: 0x0000E25B
			internal static Codebook.FastRange Get(int start, int count)
			{
				Codebook.FastRange fastRange;
				if ((fastRange = Codebook.FastRange._cachedRange) == null)
				{
					fastRange = (Codebook.FastRange._cachedRange = new Codebook.FastRange());
				}
				Codebook.FastRange fastRange2 = fastRange;
				fastRange2._start = start;
				fastRange2._count = count;
				return fastRange2;
			}

			// Token: 0x060002CB RID: 715 RVA: 0x0000240A File Offset: 0x0000060A
			private FastRange()
			{
			}

			// Token: 0x170000E9 RID: 233
			public int this[int index]
			{
				get
				{
					if (index > this._count)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this._start + index;
				}
			}

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x060002CD RID: 717 RVA: 0x00010098 File Offset: 0x0000E298
			public int Count
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x060002CE RID: 718 RVA: 0x00007CE8 File Offset: 0x00005EE8
			public IEnumerator<int> GetEnumerator()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060002CF RID: 719 RVA: 0x000100A0 File Offset: 0x0000E2A0
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x04000327 RID: 807
			[ThreadStatic]
			private static Codebook.FastRange _cachedRange;

			// Token: 0x04000328 RID: 808
			private int _start;

			// Token: 0x04000329 RID: 809
			private int _count;
		}
	}
}
