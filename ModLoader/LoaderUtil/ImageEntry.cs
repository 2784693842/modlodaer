using System;
using System.IO;

namespace ModLoader.LoaderUtil
{
	// Token: 0x02000025 RID: 37
	public class ImageEntry
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00009528 File Offset: 0x00007728
		public ImageEntry(string imgPath)
		{
			this.ImgPath = imgPath;
			this.Name = Path.GetFileNameWithoutExtension(imgPath);
			string text = Path.ChangeExtension(imgPath, "dds");
			if (File.Exists(text))
			{
				this.DdsPath = text;
				return;
			}
			string path = imgPath.Substring(imgPath.LastIndexOf(ResourceLoadHelper.PicturePat, StringComparison.Ordinal)).Substring(8);
			text = Path.Combine(imgPath.Substring(0, imgPath.LastIndexOf(ResourceLoadHelper.ResourcePat, StringComparison.Ordinal) - 1), ResourceLoadHelper.ResourcePat, "Dxt", Path.ChangeExtension(path, "dds"));
			if (File.Exists(text))
			{
				this.DdsPath = text;
			}
		}

		// Token: 0x0400009B RID: 155
		public readonly string ImgPath;

		// Token: 0x0400009C RID: 156
		public readonly string DdsPath;

		// Token: 0x0400009D RID: 157
		public readonly string Name;
	}
}
