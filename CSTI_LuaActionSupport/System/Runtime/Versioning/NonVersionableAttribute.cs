using System;

namespace System.Runtime.Versioning
{
	// Token: 0x020000DB RID: 219
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	internal sealed class NonVersionableAttribute : Attribute
	{
	}
}
