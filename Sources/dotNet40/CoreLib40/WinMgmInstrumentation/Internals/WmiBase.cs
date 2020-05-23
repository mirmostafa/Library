#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:05 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Linq;
using System.Management;
using Library40.Helpers;

namespace Library40.WinMgmInstrumentation.Internals
{
	public class WmiBase
	{
		public WmiBase(bool initialize = true)
		{
			this.IsLocal = true;
			if (initialize)
				this.Initialize();
		}

		public bool Initiated { get; protected set; }

		public bool IsLocal { get; private set; }

		public virtual void Initialize()
		{
			if (this.Initiated)
				return;
			using (var searcher = this.GetSearcher())
			using (var queryObj = searcher.Get().Cast<ManagementObject>().FirstOrDefault())
				foreach (var property in this.GetType().GetProperties())
				{
					var wmiProp = property.GetCustomAttributes(true).Cast<Attribute>().FirstOrDefault(att => att is WmiPropAttribute) as WmiPropAttribute;
					if (wmiProp == null)
						continue;
					var wmiPropName = wmiProp.Name ?? property.Name;
					property.SetValue(this,
						queryObj[wmiPropName],
						new object[]
						{
						});
				}
			this.Initiated = true;
		}

		protected virtual ManagementObjectSearcher GetSearcher()
		{
			if (this.IsLocal)
				return new ManagementObjectSearcher(string.Format("root\\{0}", ObjectHelper.GetAttribute<WmiClassAttribute>(this).Namespace),
					string.Format("SELECT * FROM {0}", ObjectHelper.GetAttribute<WmiClassAttribute>(this).ClassName));
			return null;
		}
	}
}