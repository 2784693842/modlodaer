using System;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x02000073 RID: 115
	internal class Residue1 : Residue0
	{
		// Token: 0x06000290 RID: 656 RVA: 0x0000E344 File Offset: 0x0000C544
		protected override bool WriteVectors(ICodebook codebook, IPacket packet, float[][] residue, int channel, int offset, int partitionSize)
		{
			float[] array = residue[channel];
			int i = 0;
			while (i < partitionSize)
			{
				int num = codebook.DecodeScalar(packet);
				if (num == -1)
				{
					return true;
				}
				for (int j = 0; j < codebook.Dimensions; j++)
				{
					array[offset + i] += codebook[num, j];
					i++;
				}
			}
			return false;
		}
	}
}
