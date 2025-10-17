using System;

namespace SFB
{
	// Token: 0x020001ED RID: 493
	public class StandaloneFileBrowser
	{
		// Token: 0x06000CEE RID: 3310 RVA: 0x00068DA0 File Offset: 0x00066FA0
		public static string[] OpenFilePanel(string title, string directory, string extension, bool multiselect)
		{
			ExtensionFilter[] array;
			if (!string.IsNullOrEmpty(extension))
			{
				(array = new ExtensionFilter[1])[0] = new ExtensionFilter("", new string[]
				{
					extension
				});
			}
			else
			{
				array = null;
			}
			ExtensionFilter[] extensions = array;
			return StandaloneFileBrowser.OpenFilePanel(title, directory, extensions, multiselect);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00068DE3 File Offset: 0x00066FE3
		public static string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
		{
			return StandaloneFileBrowser._platformWrapper.OpenFilePanel(title, directory, extensions, multiselect);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00068DF4 File Offset: 0x00066FF4
		public static void OpenFilePanelAsync(string title, string directory, string extension, bool multiselect, Action<string[]> cb)
		{
			ExtensionFilter[] array;
			if (!string.IsNullOrEmpty(extension))
			{
				(array = new ExtensionFilter[1])[0] = new ExtensionFilter("", new string[]
				{
					extension
				});
			}
			else
			{
				array = null;
			}
			ExtensionFilter[] extensions = array;
			StandaloneFileBrowser.OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00068E39 File Offset: 0x00067039
		public static void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb)
		{
			StandaloneFileBrowser._platformWrapper.OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00068E4B File Offset: 0x0006704B
		public static string[] OpenFolderPanel(string title, string directory, bool multiselect)
		{
			return StandaloneFileBrowser._platformWrapper.OpenFolderPanel(title, directory, multiselect);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00068E5A File Offset: 0x0006705A
		public static void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb)
		{
			StandaloneFileBrowser._platformWrapper.OpenFolderPanelAsync(title, directory, multiselect, cb);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00068E6C File Offset: 0x0006706C
		public static string SaveFilePanel(string title, string directory, string defaultName, string extension)
		{
			ExtensionFilter[] array;
			if (!string.IsNullOrEmpty(extension))
			{
				(array = new ExtensionFilter[1])[0] = new ExtensionFilter("", new string[]
				{
					extension
				});
			}
			else
			{
				array = null;
			}
			ExtensionFilter[] extensions = array;
			return StandaloneFileBrowser.SaveFilePanel(title, directory, defaultName, extensions);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00068EAF File Offset: 0x000670AF
		public static string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
		{
			return StandaloneFileBrowser._platformWrapper.SaveFilePanel(title, directory, defaultName, extensions);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00068EC0 File Offset: 0x000670C0
		public static void SaveFilePanelAsync(string title, string directory, string defaultName, string extension, Action<string> cb)
		{
			ExtensionFilter[] array;
			if (!string.IsNullOrEmpty(extension))
			{
				(array = new ExtensionFilter[1])[0] = new ExtensionFilter("", new string[]
				{
					extension
				});
			}
			else
			{
				array = null;
			}
			ExtensionFilter[] extensions = array;
			StandaloneFileBrowser.SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00068F05 File Offset: 0x00067105
		public static void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb)
		{
			StandaloneFileBrowser._platformWrapper.SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
		}

		// Token: 0x040011BF RID: 4543
		private static IStandaloneFileBrowser _platformWrapper = new StandaloneFileBrowserWindows();
	}
}
