using System;
using System.Collections.Generic;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts.Ogg;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Ogg
{
	// Token: 0x02000097 RID: 151
	internal class Packet : DataPacket
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x00013B09 File Offset: 0x00011D09
		internal Packet(IReadOnlyList<int> dataParts, IPacketReader packetReader, Memory<byte> initialData)
		{
			this._dataParts = dataParts;
			this._packetReader = packetReader;
			this._data = initialData;
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003BA RID: 954 RVA: 0x00013B26 File Offset: 0x00011D26
		protected override int TotalBits
		{
			get
			{
				return (this._dataCount + this._data.Length) * 8;
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00013B3C File Offset: 0x00011D3C
		protected unsafe override int ReadNextByte()
		{
			if (this._dataIndex == this._dataParts.Count)
			{
				return -1;
			}
			byte result = *this._data.Span[this._dataOfs];
			int num = this._dataOfs + 1;
			this._dataOfs = num;
			if (num == this._data.Length)
			{
				this._dataOfs = 0;
				this._dataCount += this._data.Length;
				num = this._dataIndex + 1;
				this._dataIndex = num;
				if (num < this._dataParts.Count)
				{
					this._data = this._packetReader.GetPacketData(this._dataParts[this._dataIndex]);
					return (int)result;
				}
				this._data = Memory<byte>.Empty;
			}
			return (int)result;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00013C04 File Offset: 0x00011E04
		public override void Reset()
		{
			this._dataIndex = 0;
			this._dataOfs = 0;
			if (this._dataParts.Count > 0)
			{
				this._data = this._packetReader.GetPacketData(this._dataParts[0]);
			}
			base.Reset();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00013C50 File Offset: 0x00011E50
		public override void Done()
		{
			IPacketReader packetReader = this._packetReader;
			if (packetReader != null)
			{
				packetReader.InvalidatePacketCache(this);
			}
			base.Done();
		}

		// Token: 0x0400039E RID: 926
		private IReadOnlyList<int> _dataParts;

		// Token: 0x0400039F RID: 927
		private IPacketReader _packetReader;

		// Token: 0x040003A0 RID: 928
		private int _dataCount;

		// Token: 0x040003A1 RID: 929
		private Memory<byte> _data;

		// Token: 0x040003A2 RID: 930
		private int _dataIndex;

		// Token: 0x040003A3 RID: 931
		private int _dataOfs;
	}
}
