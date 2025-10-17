using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ookii.Dialogs;

namespace SFB
{
	// Token: 0x020001EF RID: 495
	public class StandaloneFileBrowserWindows : IStandaloneFileBrowser
	{
		// Token: 0x06000CFB RID: 3323
		[DllImport("user32.dll")]
		private static extern IntPtr GetActiveWindow();

		// Token: 0x06000CFC RID: 3324 RVA: 0x00068F30 File Offset: 0x00067130
		public string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
		{
			VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog();
			vistaOpenFileDialog.Title = title;
			if (extensions != null)
			{
				vistaOpenFileDialog.Filter = StandaloneFileBrowserWindows.GetFilterFromFileExtensionList(extensions);
				vistaOpenFileDialog.FilterIndex = 1;
			}
			else
			{
				vistaOpenFileDialog.Filter = string.Empty;
			}
			vistaOpenFileDialog.Multiselect = multiselect;
			if (!string.IsNullOrEmpty(directory))
			{
				vistaOpenFileDialog.FileName = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
			}
			string[] result = (vistaOpenFileDialog.ShowDialog(new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) == DialogResult.OK) ? vistaOpenFileDialog.FileNames : new string[0];
			vistaOpenFileDialog.Dispose();
			return result;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00068FB0 File Offset: 0x000671B0
		public void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb)
		{
			cb(this.OpenFilePanel(title, directory, extensions, multiselect));
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00068FC4 File Offset: 0x000671C4
		public string[] OpenFolderPanel(string title, string directory, bool multiselect)
		{
			VistaFolderBrowserDialog vistaFolderBrowserDialog = new VistaFolderBrowserDialog();
			vistaFolderBrowserDialog.Description = title;
			if (!string.IsNullOrEmpty(directory))
			{
				vistaFolderBrowserDialog.SelectedPath = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
			}
			string[] result;
			if (vistaFolderBrowserDialog.ShowDialog(new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) != DialogResult.OK)
			{
				result = new string[0];
			}
			else
			{
				(result = new string[1])[0] = vistaFolderBrowserDialog.SelectedPath;
			}
			vistaFolderBrowserDialog.Dispose();
			return result;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00069022 File Offset: 0x00067222
		public void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb)
		{
			cb(this.OpenFolderPanel(title, directory, multiselect));
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00069034 File Offset: 0x00067234
		public string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
		{
			VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog();
			vistaSaveFileDialog.Title = title;
			string text = "";
			if (!string.IsNullOrEmpty(directory))
			{
				text = StandaloneFileBrowserWindows.GetDirectoryPath(directory);
			}
			if (!string.IsNullOrEmpty(defaultName))
			{
				text += defaultName;
			}
			vistaSaveFileDialog.FileName = text;
			if (extensions != null)
			{
				vistaSaveFileDialog.Filter = StandaloneFileBrowserWindows.GetFilterFromFileExtensionList(extensions);
				vistaSaveFileDialog.FilterIndex = 1;
				vistaSaveFileDialog.DefaultExt = extensions[0].Extensions[0];
				vistaSaveFileDialog.AddExtension = true;
			}
			else
			{
				vistaSaveFileDialog.DefaultExt = string.Empty;
				vistaSaveFileDialog.Filter = string.Empty;
				vistaSaveFileDialog.AddExtension = false;
			}
			string result = (vistaSaveFileDialog.ShowDialog(new WindowWrapper(StandaloneFileBrowserWindows.GetActiveWindow())) == DialogResult.OK) ? vistaSaveFileDialog.FileName : "";
			vistaSaveFileDialog.Dispose();
			return result;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000690F5 File Offset: 0x000672F5
		public void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb)
		{
			cb(this.SaveFilePanel(title, directory, defaultName, extensions));
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0006910C File Offset: 0x0006730C
		private static string GetFilterFromFileExtensionList(ExtensionFilter[] extensions)
		{
			string text = "";
			foreach (ExtensionFilter extensionFilter in extensions)
			{
				text = text + extensionFilter.Name + "(";
				foreach (string str in extensionFilter.Extensions)
				{
					text = text + "*." + str + ",";
				}
				text = text.Remove(text.Length - 1);
				text += ") |";
				foreach (string str2 in extensionFilter.Extensions)
				{
					text = text + "*." + str2 + "; ";
				}
				text += "|";
			}
			return text.Remove(text.Length - 1);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000691F0 File Offset: 0x000673F0
		private static string GetDirectoryPath(string directory)
		{
			string text = Path.GetFullPath(directory);
			if (!text.EndsWith("\\"))
			{
				text += "\\";
			}
			if (Path.GetPathRoot(text) == text)
			{
				return directory;
			}
			return Path.GetDirectoryName(text) + Path.DirectorySeparatorChar.ToString();
		}
	}
}
