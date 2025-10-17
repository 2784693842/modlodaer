using System;
using System.Windows.Forms;

namespace SFB
{
	// Token: 0x020001EE RID: 494
	public class WindowWrapper : IWin32Window
	{
		// Token: 0x06000CF9 RID: 3321 RVA: 0x00068F17 File Offset: 0x00067117
		public WindowWrapper(IntPtr handle)
		{
			this._hwnd = handle;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x00068F26 File Offset: 0x00067126
		public IntPtr Handle
		{
			get
			{
				return this._hwnd;
			}
		}

		// Token: 0x040011C0 RID: 4544
		private IntPtr _hwnd;
	}
}
