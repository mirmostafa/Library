#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 4:00 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Library35.Windows.Actions
{
	internal sealed class ActionCollectionEditor : CollectionEditor
	{
		private Type[] returnedTypes;

		public ActionCollectionEditor()
			: base(typeof (ActionCollection))
		{
		}

		protected override Type[] CreateNewItemTypes()
		{
			return this.returnedTypes;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (this.returnedTypes == null)
				this.returnedTypes = GetReturnedTypes(provider);
			return base.EditValue(context, provider, value);
		}

		private static Type[] GetReturnedTypes(IServiceProvider provider)
		{
			var res = new List<Type>();

			var tds = (ITypeDiscoveryService)provider.GetService(typeof (ITypeDiscoveryService));

			if (tds != null)
				foreach (Type actionType in tds.GetTypes(typeof (Action), false))
					if (actionType.GetCustomAttributes(typeof (StandardActionAttribute), false).Length > 0 && !res.Contains(actionType))
						res.Add(actionType);

			return res.ToArray();
		}
	}
}