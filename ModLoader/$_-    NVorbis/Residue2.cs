using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000074 RID: 116
	internal class Residue2 : Residue0
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000E3A1 File Offset: 0x0000C5A1
		public override void Init(IPacket packet, int channels, ICodebook[] codebooks)
		{
			this._channels = channels;
			base.Init(packet, 1, codebooks);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000E3B3 File Offset: 0x0000C5B3
		public override void Decode(IPacket packet, bool[] doNotDecodeChannel, int blockSize, float[][] buffer)
		{
			base.Decode(packet, doNotDecodeChannel, blockSize * this._channels, buffer);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000E3C8 File Offset: 0x0000C5C8
		protected override bool WriteVectors(ICodebook codebook, IPacket packet, float[][] residue, int channel, int offset, int partitionSize)
		{
			int num = 0;
			offset /= this._channels;
			int i = 0;
			while (i < partitionSize)
			{
				int num2 = codebook.DecodeScalar(packet);
				if (num2 == -1)
				{
					return true;
				}
				int j = 0;
				while (j < codebook.Dimensions)
				{
					residue[num][offset] += codebook[num2, j];
					if (++num == this._channels)
					{
						num = 0;
						offset++;
					}
					j++;
					i++;
				}
			}
			return false;
		}

		// Token: 0x040002E9 RID: 745
		private int _channels;
	}
}
