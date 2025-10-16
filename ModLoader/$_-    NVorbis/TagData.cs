using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis
{
	// Token: 0x0200006F RID: 111
	internal class TagData : ITagData
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000DA14 File Offset: 0x0000BC14
		public TagData(string vendor, string[] comments)
		{
			this.EncoderVendor = vendor;
			Dictionary<string, IReadOnlyList<string>> dictionary = new Dictionary<string, IReadOnlyList<string>>();
			for (int i = 0; i < comments.Length; i++)
			{
				string[] array = comments[i].Split(new char[]
				{
					'='
				});
				if (array.Length == 1)
				{
					array = new string[]
					{
						array[0],
						string.Empty
					};
				}
				int num = array[0].IndexOf('[');
				if (num > -1)
				{
					array[1] = array[0].Substring(num + 1, array[0].Length - num - 2).ToUpper(CultureInfo.CurrentCulture) + ": " + array[1];
					array[0] = array[0].Substring(0, num);
				}
				IReadOnlyList<string> readOnlyList;
				if (dictionary.TryGetValue(array[0].ToUpperInvariant(), out readOnlyList))
				{
					((List<string>)readOnlyList).Add(array[1]);
				}
				else
				{
					dictionary.Add(array[0].ToUpperInvariant(), new List<string>
					{
						array[1]
					});
				}
			}
			this._tags = dictionary;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000DB0C File Offset: 0x0000BD0C
		public string GetTagSingle(string key, bool concatenate = false)
		{
			IReadOnlyList<string> tagMulti = this.GetTagMulti(key);
			if (tagMulti.Count <= 0)
			{
				return string.Empty;
			}
			if (concatenate)
			{
				return string.Join(Environment.NewLine, tagMulti.ToArray<string>());
			}
			return tagMulti[tagMulti.Count - 1];
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000DB54 File Offset: 0x0000BD54
		public IReadOnlyList<string> GetTagMulti(string key)
		{
			IReadOnlyList<string> result;
			if (this._tags.TryGetValue(key.ToUpperInvariant(), out result))
			{
				return result;
			}
			return TagData.s_emptyList;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000DB7D File Offset: 0x0000BD7D
		public IReadOnlyDictionary<string, IReadOnlyList<string>> All
		{
			get
			{
				return this._tags;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000DB85 File Offset: 0x0000BD85
		public string EncoderVendor { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000DB8D File Offset: 0x0000BD8D
		public string Title
		{
			get
			{
				return this.GetTagSingle("TITLE", false);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000DB9B File Offset: 0x0000BD9B
		public string Version
		{
			get
			{
				return this.GetTagSingle("VERSION", false);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000DBA9 File Offset: 0x0000BDA9
		public string Album
		{
			get
			{
				return this.GetTagSingle("ALBUM", false);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000DBB7 File Offset: 0x0000BDB7
		public string TrackNumber
		{
			get
			{
				return this.GetTagSingle("TRACKNUMBER", false);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000DBC5 File Offset: 0x0000BDC5
		public string Artist
		{
			get
			{
				return this.GetTagSingle("ARTIST", false);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000DBD3 File Offset: 0x0000BDD3
		public IReadOnlyList<string> Performers
		{
			get
			{
				return this.GetTagMulti("PERFORMER");
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000DBE0 File Offset: 0x0000BDE0
		public string Copyright
		{
			get
			{
				return this.GetTagSingle("COPYRIGHT", false);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000DBEE File Offset: 0x0000BDEE
		public string License
		{
			get
			{
				return this.GetTagSingle("LICENSE", false);
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000DBFC File Offset: 0x0000BDFC
		public string Organization
		{
			get
			{
				return this.GetTagSingle("ORGANIZATION", false);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000DC0A File Offset: 0x0000BE0A
		public string Description
		{
			get
			{
				return this.GetTagSingle("DESCRIPTION", false);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000DC18 File Offset: 0x0000BE18
		public IReadOnlyList<string> Genres
		{
			get
			{
				return this.GetTagMulti("GENRE");
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000DC25 File Offset: 0x0000BE25
		public IReadOnlyList<string> Dates
		{
			get
			{
				return this.GetTagMulti("DATE");
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000DC32 File Offset: 0x0000BE32
		public IReadOnlyList<string> Locations
		{
			get
			{
				return this.GetTagMulti("LOCATION");
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000DC3F File Offset: 0x0000BE3F
		public string Contact
		{
			get
			{
				return this.GetTagSingle("CONTACT", false);
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000DC4D File Offset: 0x0000BE4D
		public string Isrc
		{
			get
			{
				return this.GetTagSingle("ISRC", false);
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000DC5B File Offset: 0x0000BE5B
		// Note: this type is marked as 'beforefieldinit'.
		static TagData()
		{
			TagData.s_emptyList = new List<string>();
		}

		// Token: 0x040002DC RID: 732
		private static IReadOnlyList<string> s_emptyList;

		// Token: 0x040002DD RID: 733
		private Dictionary<string, IReadOnlyList<string>> _tags;
	}
}
