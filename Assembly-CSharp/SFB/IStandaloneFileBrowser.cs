using System;

namespace SFB
{
	// Token: 0x020001EB RID: 491
	public interface IStandaloneFileBrowser
	{
		// Token: 0x06000CE6 RID: 3302
		string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect);

		// Token: 0x06000CE7 RID: 3303
		string[] OpenFolderPanel(string title, string directory, bool multiselect);

		// Token: 0x06000CE8 RID: 3304
		string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions);

		// Token: 0x06000CE9 RID: 3305
		void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb);

		// Token: 0x06000CEA RID: 3306
		void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb);

		// Token: 0x06000CEB RID: 3307
		void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb);
	}
}
