using System;
using System.IO;
using Ionic.Crc;

namespace Ionic.Zip
{
	// Token: 0x02000043 RID: 67
	internal class ZipCrypto
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000DAD5 File Offset: 0x0000BCD5
		private ZipCrypto()
		{
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000DAFF File Offset: 0x0000BCFF
		public static ZipCrypto ForWrite(string password)
		{
			ZipCrypto zipCrypto = new ZipCrypto();
			if (password == null)
			{
				throw new BadPasswordException("This entry requires a password.");
			}
			zipCrypto.InitCipher(password);
			return zipCrypto;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		public static ZipCrypto ForRead(string password, ZipEntry e)
		{
			Stream archiveStream = e._archiveStream;
			e._WeakEncryptionHeader = new byte[12];
			byte[] weakEncryptionHeader = e._WeakEncryptionHeader;
			ZipCrypto zipCrypto = new ZipCrypto();
			if (password == null)
			{
				throw new BadPasswordException("This entry requires a password.");
			}
			zipCrypto.InitCipher(password);
			ZipEntry.ReadWeakEncryptionHeader(archiveStream, weakEncryptionHeader);
			byte[] array = zipCrypto.DecryptMessage(weakEncryptionHeader, weakEncryptionHeader.Length);
			if (array[11] != (byte)(e._Crc32 >> 24 & 255))
			{
				if ((e._BitField & 8) != 8)
				{
					throw new BadPasswordException("The password did not match.");
				}
				if (array[11] != (byte)(e._TimeBlob >> 8 & 255))
				{
					throw new BadPasswordException("The password did not match.");
				}
			}
			return zipCrypto;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000DBC0 File Offset: 0x0000BDC0
		private byte MagicByte
		{
			get
			{
				ushort num = (ushort)(this._Keys[2] & 65535U) | 2;
				return (byte)(num * (num ^ 1) >> 8);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		public byte[] DecryptMessage(byte[] cipherText, int length)
		{
			if (cipherText == null)
			{
				throw new ArgumentNullException("cipherText");
			}
			if (length > cipherText.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Bad length during Decryption: the length parameter must be smaller than or equal to the size of the destination array.");
			}
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				byte b = cipherText[i] ^ this.MagicByte;
				this.UpdateKeys(b);
				array[i] = b;
			}
			return array;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000DC44 File Offset: 0x0000BE44
		public byte[] EncryptMessage(byte[] plainText, int length)
		{
			if (plainText == null)
			{
				throw new ArgumentNullException("plaintext");
			}
			if (length > plainText.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Bad length during Encryption: The length parameter must be smaller than or equal to the size of the destination array.");
			}
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				byte byteValue = plainText[i];
				array[i] = (plainText[i] ^ this.MagicByte);
				this.UpdateKeys(byteValue);
			}
			return array;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000DCA4 File Offset: 0x0000BEA4
		public void InitCipher(string passphrase)
		{
			byte[] array = SharedUtilities.StringToByteArray(passphrase);
			for (int i = 0; i < passphrase.Length; i++)
			{
				this.UpdateKeys(array[i]);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000DCD4 File Offset: 0x0000BED4
		private void UpdateKeys(byte byteValue)
		{
			this._Keys[0] = (uint)this.crc32.ComputeCrc32((int)this._Keys[0], byteValue);
			this._Keys[1] = this._Keys[1] + (uint)((byte)this._Keys[0]);
			this._Keys[1] = this._Keys[1] * 134775813U + 1U;
			this._Keys[2] = (uint)this.crc32.ComputeCrc32((int)this._Keys[2], (byte)(this._Keys[1] >> 24));
		}

		// Token: 0x040001B9 RID: 441
		private uint[] _Keys = new uint[]
		{
			305419896U,
			591751049U,
			878082192U
		};

		// Token: 0x040001BA RID: 442
		private CRC32 crc32 = new CRC32();
	}
}
