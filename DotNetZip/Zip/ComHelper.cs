using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
	// Token: 0x0200002B RID: 43
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000F")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ComHelper
	{
		// Token: 0x06000173 RID: 371 RVA: 0x0000CCDA File Offset: 0x0000AEDA
		public bool IsZipFile(string filename)
		{
			return ZipFile.IsZipFile(filename);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000CCE2 File Offset: 0x0000AEE2
		public bool IsZipFileWithExtract(string filename)
		{
			return ZipFile.IsZipFile(filename, true);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000CCEB File Offset: 0x0000AEEB
		public bool CheckZip(string filename)
		{
			return ZipFile.CheckZip(filename);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000CCF3 File Offset: 0x0000AEF3
		public bool CheckZipPassword(string filename, string password)
		{
			return ZipFile.CheckZipPassword(filename, password);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000CCFC File Offset: 0x0000AEFC
		public void FixZipDirectory(string filename)
		{
			ZipFile.FixZipDirectory(filename);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000CD04 File Offset: 0x0000AF04
		public string GetZipLibraryVersion()
		{
			return ZipFile.LibraryVersion.ToString();
		}
	}
}
