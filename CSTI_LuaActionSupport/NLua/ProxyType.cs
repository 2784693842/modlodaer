using System;
using System.Linq;
using System.Reflection;

namespace NLua
{
	// Token: 0x02000080 RID: 128
	internal class ProxyType
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x000100CE File Offset: 0x0000E2CE
		public ProxyType(Type proxy)
		{
			this._proxy = proxy;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000100DD File Offset: 0x0000E2DD
		public override string ToString()
		{
			string str = "ProxyType(";
			Type underlyingSystemType = this.UnderlyingSystemType;
			return str + ((underlyingSystemType != null) ? underlyingSystemType.ToString() : null) + ")";
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00010100 File Offset: 0x0000E300
		public Type UnderlyingSystemType
		{
			get
			{
				return this._proxy;
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00010108 File Offset: 0x0000E308
		public override bool Equals(object obj)
		{
			if (obj is Type)
			{
				return this._proxy == (Type)obj;
			}
			if (obj is ProxyType)
			{
				return this._proxy == ((ProxyType)obj).UnderlyingSystemType;
			}
			return this._proxy.Equals(obj);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001015A File Offset: 0x0000E35A
		public override int GetHashCode()
		{
			return this._proxy.GetHashCode();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00010167 File Offset: 0x0000E367
		public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return LuaMemberAttribute.GetMembersForType(this._proxy, name, bindingAttr);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00010176 File Offset: 0x0000E376
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Type[] signature)
		{
			return LuaMemberAttribute.GetMethodsForType(this._proxy, name, bindingAttr, signature).FirstOrDefault<MethodInfo>();
		}

		// Token: 0x0400017E RID: 382
		private readonly Type _proxy;
	}
}
