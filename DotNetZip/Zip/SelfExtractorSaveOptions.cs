using System;

namespace Ionic.Zip
{
	// Token: 0x02000052 RID: 82
	public class SelfExtractorSaveOptions
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00017EE0 File Offset: 0x000160E0
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00017EE8 File Offset: 0x000160E8
		public SelfExtractorFlavor Flavor { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00017EF1 File Offset: 0x000160F1
		// (set) Token: 0x060003A4 RID: 932 RVA: 0x00017EF9 File Offset: 0x000160F9
		public string PostExtractCommandLine { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x00017F02 File Offset: 0x00016102
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x00017F0A File Offset: 0x0001610A
		public string DefaultExtractDirectory { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00017F13 File Offset: 0x00016113
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00017F1B File Offset: 0x0001611B
		public string IconFile { get; set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00017F24 File Offset: 0x00016124
		// (set) Token: 0x060003AA RID: 938 RVA: 0x00017F2C File Offset: 0x0001612C
		public bool Quiet { get; set; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00017F35 File Offset: 0x00016135
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00017F3D File Offset: 0x0001613D
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00017F46 File Offset: 0x00016146
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00017F4E File Offset: 0x0001614E
		public bool RemoveUnpackedFilesAfterExecute { get; set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060003AF RID: 943 RVA: 0x00017F57 File Offset: 0x00016157
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x00017F5F File Offset: 0x0001615F
		public Version FileVersion { get; set; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00017F68 File Offset: 0x00016168
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x00017F70 File Offset: 0x00016170
		public string ProductVersion { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x00017F79 File Offset: 0x00016179
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x00017F81 File Offset: 0x00016181
		public string Copyright { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00017F8A File Offset: 0x0001618A
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00017F92 File Offset: 0x00016192
		public string Description { get; set; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00017F9B File Offset: 0x0001619B
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00017FA3 File Offset: 0x000161A3
		public string ProductName { get; set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00017FAC File Offset: 0x000161AC
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00017FB4 File Offset: 0x000161B4
		public string SfxExeWindowTitle { get; set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00017FBD File Offset: 0x000161BD
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00017FC5 File Offset: 0x000161C5
		public string AdditionalCompilerSwitches { get; set; }
	}
}
