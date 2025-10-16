using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000093 RID: 147
[CompilerGenerated]
internal sealed class dll!<>f__AnonymousType0<<extensionType>j__TPar, <method>j__TPar>
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600046E RID: 1134 RVA: 0x00012119 File Offset: 0x00010319
	public <extensionType>j__TPar extensionType
	{
		get
		{
			return this.<extensionType>i__Field;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600046F RID: 1135 RVA: 0x00012121 File Offset: 0x00010321
	public <method>j__TPar method
	{
		get
		{
			return this.<method>i__Field;
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00012129 File Offset: 0x00010329
	[DebuggerHidden]
	public dll!<>f__AnonymousType0(<extensionType>j__TPar extensionType, <method>j__TPar method)
	{
		this.<extensionType>i__Field = extensionType;
		this.<method>i__Field = method;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00012140 File Offset: 0x00010340
	[DebuggerHidden]
	public override bool Equals(object value)
	{
		NLua.dll!<>f__AnonymousType0<<extensionType>j__TPar, <method>j__TPar> nlua.dll!<>f__AnonymousType = value as NLua.dll!<>f__AnonymousType0<<extensionType>j__TPar, <method>j__TPar>;
		return nlua.dll!<>f__AnonymousType != null && EqualityComparer<<extensionType>j__TPar>.Default.Equals(this.<extensionType>i__Field, nlua.dll!<>f__AnonymousType.<extensionType>i__Field) && EqualityComparer<<method>j__TPar>.Default.Equals(this.<method>i__Field, nlua.dll!<>f__AnonymousType.<method>i__Field);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x00012187 File Offset: 0x00010387
	[DebuggerHidden]
	public override int GetHashCode()
	{
		return (1466930676 * -1521134295 + EqualityComparer<<extensionType>j__TPar>.Default.GetHashCode(this.<extensionType>i__Field)) * -1521134295 + EqualityComparer<<method>j__TPar>.Default.GetHashCode(this.<method>i__Field);
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x000121BC File Offset: 0x000103BC
	[DebuggerHidden]
	public override string ToString()
	{
		IFormatProvider provider = null;
		string format = "{{ extensionType = {0}, method = {1} }}";
		object[] array = new object[2];
		int num = 0;
		<extensionType>j__TPar <extensionType>j__TPar = this.<extensionType>i__Field;
		array[num] = ((<extensionType>j__TPar != null) ? <extensionType>j__TPar.ToString() : null);
		int num2 = 1;
		<method>j__TPar <method>j__TPar = this.<method>i__Field;
		array[num2] = ((<method>j__TPar != null) ? <method>j__TPar.ToString() : null);
		return string.Format(provider, format, array);
	}

	// Token: 0x040001A7 RID: 423
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly <extensionType>j__TPar <extensionType>i__Field;

	// Token: 0x040001A8 RID: 424
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly <method>j__TPar <method>i__Field;
}
