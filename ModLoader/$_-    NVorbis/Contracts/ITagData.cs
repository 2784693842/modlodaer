using System;
using System.Collections.Generic;

namespace \u000A\u0020\u0020\u0020\u0020\u0020\u0020$_\u000A\u0020\u0020\u0020\u0020NVorbis.Contracts
{
	// Token: 0x02000057 RID: 87
	internal interface ITagData
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000183 RID: 387
		IReadOnlyDictionary<string, IReadOnlyList<string>> All { get; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000184 RID: 388
		string EncoderVendor { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000185 RID: 389
		string Title { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000186 RID: 390
		string Version { get; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000187 RID: 391
		string Album { get; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000188 RID: 392
		string TrackNumber { get; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000189 RID: 393
		string Artist { get; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600018A RID: 394
		IReadOnlyList<string> Performers { get; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600018B RID: 395
		string Copyright { get; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600018C RID: 396
		string License { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600018D RID: 397
		string Organization { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600018E RID: 398
		string Description { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600018F RID: 399
		IReadOnlyList<string> Genres { get; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000190 RID: 400
		IReadOnlyList<string> Dates { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000191 RID: 401
		IReadOnlyList<string> Locations { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000192 RID: 402
		string Contact { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000193 RID: 403
		string Isrc { get; }

		// Token: 0x06000194 RID: 404
		string GetTagSingle(string key, bool concatenate = false);

		// Token: 0x06000195 RID: 405
		IReadOnlyList<string> GetTagMulti(string key);
	}
}
