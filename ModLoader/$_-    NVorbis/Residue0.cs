using System;
using System.IO;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000072 RID: 114
	internal class Residue0 : IResidue
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000DE84 File Offset: 0x0000C084
		private static int icount(int v)
		{
			int num = 0;
			while (v != 0)
			{
				num += (v & 1);
				v >>= 1;
			}
			return num;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000DEA4 File Offset: 0x0000C0A4
		public virtual void Init(IPacket packet, int channels, ICodebook[] codebooks)
		{
			this._begin = (int)packet.ReadBits(24);
			this._end = (int)packet.ReadBits(24);
			this._partitionSize = (int)packet.ReadBits(24) + 1;
			this._classifications = (int)packet.ReadBits(6) + 1;
			this._classBook = codebooks[(int)packet.ReadBits(8)];
			this._cascade = new int[this._classifications];
			int num = 0;
			for (int i = 0; i < this._classifications; i++)
			{
				int num2 = (int)packet.ReadBits(3);
				if (packet.ReadBit())
				{
					this._cascade[i] = ((int)packet.ReadBits(5) << 3 | num2);
				}
				else
				{
					this._cascade[i] = num2;
				}
				num += Residue0.icount(this._cascade[i]);
			}
			int[] array = new int[num];
			for (int j = 0; j < num; j++)
			{
				array[j] = (int)packet.ReadBits(8);
				if (codebooks[array[j]].MapType == 0)
				{
					throw new InvalidDataException();
				}
			}
			int entries = this._classBook.Entries;
			int k = this._classBook.Dimensions;
			int num3 = 1;
			while (k > 0)
			{
				num3 *= this._classifications;
				if (num3 > entries)
				{
					throw new InvalidDataException();
				}
				k--;
			}
			this._books = new ICodebook[this._classifications][];
			num = 0;
			int num4 = 0;
			for (int l = 0; l < this._classifications; l++)
			{
				int num5 = Utils.ilog(this._cascade[l]);
				this._books[l] = new ICodebook[num5];
				if (num5 > 0)
				{
					num4 = Math.Max(num4, num5);
					for (int m = 0; m < num5; m++)
					{
						if ((this._cascade[l] & 1 << m) > 0)
						{
							this._books[l][m] = codebooks[array[num++]];
						}
					}
				}
			}
			this._maxStages = num4;
			this._decodeMap = new int[num3][];
			for (int n = 0; n < num3; n++)
			{
				int num6 = n;
				int num7 = num3 / this._classifications;
				this._decodeMap[n] = new int[this._classBook.Dimensions];
				for (int num8 = 0; num8 < this._classBook.Dimensions; num8++)
				{
					int num9 = num6 / num7;
					num6 -= num9 * num7;
					num7 /= this._classifications;
					this._decodeMap[n][num8] = num9;
				}
			}
			this._channels = channels;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000E110 File Offset: 0x0000C310
		public virtual void Decode(IPacket packet, bool[] doNotDecodeChannel, int blockSize, float[][] buffer)
		{
			int num = ((this._end < blockSize / 2) ? this._end : (blockSize / 2)) - this._begin;
			if (num > 0 && Array.IndexOf<bool>(doNotDecodeChannel, false) != -1)
			{
				int num2 = num / this._partitionSize;
				int num3 = (num2 + this._classBook.Dimensions - 1) / this._classBook.Dimensions;
				int[,][] array = new int[this._channels, num3][];
				for (int i = 0; i < this._maxStages; i++)
				{
					int j = 0;
					int num4 = 0;
					while (j < num2)
					{
						if (i == 0)
						{
							for (int k = 0; k < this._channels; k++)
							{
								int num5 = this._classBook.DecodeScalar(packet);
								if (num5 < 0 || num5 >= this._decodeMap.Length)
								{
									j = num2;
									i = this._maxStages;
									break;
								}
								array[k, num4] = this._decodeMap[num5];
							}
						}
						int num6 = 0;
						while (j < num2 && num6 < this._classBook.Dimensions)
						{
							int offset = this._begin + j * this._partitionSize;
							for (int l = 0; l < this._channels; l++)
							{
								int num7 = array[l, num4][num6];
								if ((this._cascade[num7] & 1 << i) != 0)
								{
									ICodebook codebook = this._books[num7][i];
									if (codebook != null && this.WriteVectors(codebook, packet, buffer, l, offset, this._partitionSize))
									{
										j = num2;
										i = this._maxStages;
										break;
									}
								}
							}
							num6++;
							j++;
						}
						num4++;
					}
				}
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000E2BC File Offset: 0x0000C4BC
		protected virtual bool WriteVectors(ICodebook codebook, IPacket packet, float[][] residue, int channel, int offset, int partitionSize)
		{
			float[] array = residue[channel];
			int num = partitionSize / codebook.Dimensions;
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				if ((array2[i] = codebook.DecodeScalar(packet)) == -1)
				{
					return true;
				}
			}
			for (int j = 0; j < codebook.Dimensions; j++)
			{
				int k = 0;
				while (k < num)
				{
					array[offset] += codebook[array2[k], j];
					k++;
					offset++;
				}
			}
			return false;
		}

		// Token: 0x040002DF RID: 735
		private int _channels;

		// Token: 0x040002E0 RID: 736
		private int _begin;

		// Token: 0x040002E1 RID: 737
		private int _end;

		// Token: 0x040002E2 RID: 738
		private int _partitionSize;

		// Token: 0x040002E3 RID: 739
		private int _classifications;

		// Token: 0x040002E4 RID: 740
		private int _maxStages;

		// Token: 0x040002E5 RID: 741
		private ICodebook[][] _books;

		// Token: 0x040002E6 RID: 742
		private ICodebook _classBook;

		// Token: 0x040002E7 RID: 743
		private int[] _cascade;

		// Token: 0x040002E8 RID: 744
		private int[][] _decodeMap;
	}
}
